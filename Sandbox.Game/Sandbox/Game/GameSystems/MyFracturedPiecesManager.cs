// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyFracturedPiecesManager
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.GameSystems
{
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
  public class MyFracturedPiecesManager : MySessionComponentBase
  {
    public const int FakePieceLayer = 14;
    public static MyFracturedPiecesManager Static;
    private static float LIFE_OF_CUBIC_PIECE = 300f;
    private Queue<MyFracturedPiece> m_piecesPool = new Queue<MyFracturedPiece>();
    private const int MAX_ALLOC_PER_FRAME = 50;
    private int m_allocatedThisFrame;
    private HashSet<HkdBreakableBody> m_tmpToReturn = new HashSet<HkdBreakableBody>();
    private HashSet<long> m_dbgCreated = new HashSet<long>();
    private HashSet<long> m_dbgRemoved = new HashSet<long>();
    private List<HkBodyCollision> m_rigidList = new List<HkBodyCollision>();
    private int m_addedThisFrame;
    private Queue<MyFracturedPiecesManager.Bodies> m_bodyPool = new Queue<MyFracturedPiecesManager.Bodies>();
    private const int PREALLOCATE_PIECES = 400;
    private const int PREALLOCATE_BODIES = 400;
    public HashSet<HkRigidBody> m_givenRBs = new HashSet<HkRigidBody>((IEqualityComparer<HkRigidBody>) InstanceComparer<HkRigidBody>.Default);

    public override bool IsRequiredByGame => MyPerGameSettings.Destruction;

    public override void LoadData()
    {
      base.LoadData();
      this.InitPools();
      MyFracturedPiecesManager.Static = this;
    }

    private MyFracturedPiece AllocatePiece()
    {
      ++this.m_allocatedThisFrame;
      MyFracturedPiece entity = MyEntities.CreateEntity(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_FracturedPiece)), false) as MyFracturedPiece;
      entity.Physics = new MyPhysicsBody((IMyEntity) entity, RigidBodyFlag.RBF_DEBRIS);
      entity.Physics.CanUpdateAccelerations = true;
      return entity;
    }

    protected override void UnloadData()
    {
      foreach (MyFracturedPiecesManager.Bodies bodies in this.m_bodyPool)
        bodies.Breakable.ClearListener();
      this.m_bodyPool.Clear();
      this.m_piecesPool.Clear();
      base.UnloadData();
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      foreach (HkdBreakableBody body in this.m_tmpToReturn)
        this.ReturnToPoolInternal(body);
      this.m_tmpToReturn.Clear();
      while (this.m_bodyPool.Count < 400 && this.m_allocatedThisFrame < 50)
        this.m_bodyPool.Enqueue(this.AllocateBodies());
      while (this.m_piecesPool.Count < 400 && this.m_allocatedThisFrame < 50)
        this.m_piecesPool.Enqueue(this.AllocatePiece());
      this.m_allocatedThisFrame = 0;
    }

    private void RemoveInternal(MyFracturedPiece fp, bool fromServer = false)
    {
      if (fp.Physics != null && (HkReferenceObject) fp.Physics.RigidBody != (HkReferenceObject) null && fp.Physics.RigidBody.IsDisposed)
        fp.Physics.BreakableBody = fp.Physics.BreakableBody;
      if (fp.Physics == null || (HkReferenceObject) fp.Physics.RigidBody == (HkReferenceObject) null || fp.Physics.RigidBody.IsDisposed)
      {
        MyEntities.Remove((MyEntity) fp);
      }
      else
      {
        if (!fp.Physics.RigidBody.IsActive)
          fp.Physics.RigidBody.Activate();
        MyPhysics.RemoveDestructions(fp.Physics.RigidBody);
        HkdBreakableBody breakableBody = fp.Physics.BreakableBody;
        breakableBody.AfterReplaceBody -= new BreakableBodyReplaced(fp.Physics.FracturedBody_AfterReplaceBody);
        this.ReturnToPool(breakableBody);
        fp.Physics.Enabled = false;
        MyEntities.Remove((MyEntity) fp);
        fp.Physics.BreakableBody = (HkdBreakableBody) null;
        fp.Render.ClearModels();
        fp.OriginalBlocks.Clear();
        int num = Sync.IsServer ? 1 : 0;
        fp.EntityId = 0L;
        fp.Physics.BreakableBody = (HkdBreakableBody) null;
        this.m_piecesPool.Enqueue(fp);
      }
    }

    public MyFracturedPiece GetPieceFromPool(long entityId, bool fromServer = false)
    {
      int num = Sync.IsServer ? 1 : 0;
      MyFracturedPiece myFracturedPiece = this.m_piecesPool.Count != 0 ? this.m_piecesPool.Dequeue() : this.AllocatePiece();
      if (Sync.IsServer)
        myFracturedPiece.EntityId = MyEntityIdentifier.AllocateId();
      return myFracturedPiece;
    }

    public void GetFracturesInSphere(
      ref BoundingSphereD searchSphere,
      ref List<MyFracturedPiece> output)
    {
      HkShape shape = (HkShape) new HkSphereShape((float) searchSphere.Radius);
      try
      {
        MyPhysics.GetPenetrationsShape(shape, ref searchSphere.Center, ref Quaternion.Identity, this.m_rigidList, 12);
        foreach (HkBodyCollision rigid in this.m_rigidList)
        {
          if (rigid.GetCollisionEntity() is MyFracturedPiece collisionEntity)
            output.Add(collisionEntity);
        }
      }
      finally
      {
        this.m_rigidList.Clear();
        shape.RemoveReference();
      }
    }

    public void GetFracturesInBox(ref BoundingBoxD searchBox, List<MyFracturedPiece> output)
    {
      this.m_rigidList.Clear();
      HkShape shape = (HkShape) new HkBoxShape((Vector3) searchBox.HalfExtents);
      try
      {
        Vector3D center = searchBox.Center;
        MyPhysics.GetPenetrationsShape(shape, ref center, ref Quaternion.Identity, this.m_rigidList, 12);
        foreach (HkBodyCollision rigid in this.m_rigidList)
        {
          if (rigid.GetCollisionEntity() is MyFracturedPiece collisionEntity)
            output.Add(collisionEntity);
        }
      }
      finally
      {
        this.m_rigidList.Clear();
        shape.RemoveReference();
      }
    }

    private MyFracturedPiecesManager.Bodies AllocateBodies()
    {
      ++this.m_allocatedThisFrame;
      MyFracturedPiecesManager.Bodies bodies;
      bodies.Rigid = (HkRigidBody) null;
      bodies.Breakable = HkdBreakableBody.Allocate();
      return bodies;
    }

    public void InitPools()
    {
      for (int index = 0; index < 400; ++index)
        this.m_piecesPool.Enqueue(this.AllocatePiece());
      for (int index = 0; index < 400; ++index)
        this.m_bodyPool.Enqueue(this.AllocateBodies());
    }

    public HkdBreakableBody GetBreakableBody(HkdBreakableBodyInfo bodyInfo)
    {
      MyFracturedPiecesManager.Bodies bodies = this.m_bodyPool.Count != 0 ? this.m_bodyPool.Dequeue() : this.AllocateBodies();
      bodies.Breakable.Initialize(bodyInfo, bodies.Rigid);
      return bodies.Breakable;
    }

    public void RemoveFracturePiece(
      MyFracturedPiece piece,
      float blendTimeSeconds,
      bool fromServer = false,
      bool sync = true)
    {
      if ((double) blendTimeSeconds != 0.0)
        return;
      this.RemoveInternal(piece, fromServer);
    }

    public void RemoveFracturesInBox(ref BoundingBoxD box, float blendTimeSeconds)
    {
      if (!Sync.IsServer)
        return;
      List<MyFracturedPiece> output = new List<MyFracturedPiece>();
      this.GetFracturesInBox(ref box, output);
      foreach (MyFracturedPiece piece in output)
        this.RemoveFracturePiece(piece, blendTimeSeconds);
    }

    public void RemoveFracturesInSphere(Vector3D center, float radius)
    {
      float num = radius * radius;
      foreach (MyEntity entity in MyEntities.GetEntities())
      {
        if (entity is MyFracturedPiece && ((double) radius <= 0.0 || (center - entity.Physics.CenterOfMassWorld).LengthSquared() < (double) num))
          MyFracturedPiecesManager.Static.RemoveFracturePiece(entity as MyFracturedPiece, 2f);
      }
    }

    public void ReturnToPool(HkdBreakableBody body) => this.m_tmpToReturn.Add(body);

    private void ReturnToPoolInternal(HkdBreakableBody body)
    {
      HkRigidBody rigidBody = body.GetRigidBody();
      if ((HkReferenceObject) rigidBody == (HkReferenceObject) null)
        return;
      rigidBody.ContactPointCallbackEnabled = false;
      this.m_givenRBs.Remove(rigidBody);
      foreach (MyFracturedPiecesManager.Bodies bodies in this.m_bodyPool)
      {
        if (!((HkReferenceObject) body == (HkReferenceObject) bodies.Breakable))
        {
          int num = (HkReferenceObject) rigidBody == (HkReferenceObject) bodies.Rigid ? 1 : 0;
        }
      }
      body.BreakableShape.ClearConnections();
      body.Clear();
      MyFracturedPiecesManager.Bodies bodies1;
      bodies1.Rigid = rigidBody;
      bodies1.Breakable = body;
      body.InitListener();
      this.m_bodyPool.Enqueue(bodies1);
    }

    internal void DbgCheck(long createdId, long removedId)
    {
    }

    private struct Bodies
    {
      public HkRigidBody Rigid;
      public HkdBreakableBody Breakable;
    }
  }
}
