// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyFracturedPiece
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
  [MyEntityType(typeof (MyObjectBuilder_FracturedPiece), true)]
  public class MyFracturedPiece : MyEntity, IMyDestroyableObject, IMyEventProxy, IMyEventOwner
  {
    public HkdBreakableShape Shape;
    public MyFracturedPiece.HitInfo InitialHit;
    private float m_hitPoints;
    public List<MyDefinitionId> OriginalBlocks = new List<MyDefinitionId>();
    private List<HkdShapeInstanceInfo> m_children = new List<HkdShapeInstanceInfo>();
    private List<MyObjectBuilder_FracturedPiece.Shape> m_shapes = new List<MyObjectBuilder_FracturedPiece.Shape>();
    private List<HkdShapeInstanceInfo> m_shapeInfos = new List<HkdShapeInstanceInfo>();
    private MyTimeSpan m_markedBreakImpulse = MyTimeSpan.Zero;
    private HkEasePenetrationAction m_easePenetrationAction;
    private MyEntity3DSoundEmitter m_soundEmitter;
    private DateTime m_soundStart;
    private bool m_obstacleContact;
    private bool m_groundContact;
    private VRage.Sync.Sync<bool, SyncDirection.FromServer> m_fallSoundShouldPlay;
    private MySoundPair m_fallSound;
    private VRage.Sync.Sync<string, SyncDirection.FromServer> m_fallSoundString;
    private bool m_contactSet;

    public event Action<MyEntity> OnRemove;

    public MyRenderComponentFracturedPiece Render => (MyRenderComponentFracturedPiece) base.Render;

    public MyPhysicsBody Physics
    {
      get => base.Physics as MyPhysicsBody;
      set => this.Physics = (MyPhysicsComponentBase) value;
    }

    public MyFracturedPiece()
    {
      this.SyncFlag = true;
      this.PositionComp = (MyPositionComponentBase) new MyFracturePiecePositionComponent();
      this.Render = (MyRenderComponentBase) new MyRenderComponentFracturedPiece();
      base.Render.NeedsDraw = true;
      base.Render.PersistentFlags = MyPersistentEntityFlags2.Enabled;
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyFracturedPiece.MyFracturedPieceDebugDraw(this));
      this.UseDamageSystem = false;
      this.NeedsUpdate = MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;
      this.m_fallSoundString.SetLocalValue("");
      this.m_fallSoundString.ValueChanged += (Action<SyncBase>) (x => this.SetFallSound());
    }

    public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
    {
      MyObjectBuilder_FracturedPiece objectBuilder = base.GetObjectBuilder(copy) as MyObjectBuilder_FracturedPiece;
      foreach (MyDefinitionId originalBlock in this.OriginalBlocks)
        objectBuilder.BlockDefinitions.Add((SerializableDefinitionId) originalBlock);
      if (this.Physics == null)
      {
        foreach (MyObjectBuilder_FracturedPiece.Shape shape in this.m_shapes)
          objectBuilder.Shapes.Add(new MyObjectBuilder_FracturedPiece.Shape()
          {
            Name = shape.Name,
            Orientation = shape.Orientation
          });
        return (MyObjectBuilder_EntityBase) objectBuilder;
      }
      if (this.Physics.BreakableBody.BreakableShape.IsCompound() || string.IsNullOrEmpty(this.Physics.BreakableBody.BreakableShape.Name))
      {
        this.Physics.BreakableBody.BreakableShape.GetChildren(this.m_children);
        if (this.m_children.Count == 0)
          return (MyObjectBuilder_EntityBase) objectBuilder;
        int count = this.m_children.Count;
        for (int index = 0; index < count; ++index)
        {
          HkdShapeInstanceInfo child = this.m_children[index];
          if (string.IsNullOrEmpty(child.ShapeName))
            child.GetChildren(this.m_children);
        }
        foreach (HkdShapeInstanceInfo child in this.m_children)
        {
          string shapeName = child.ShapeName;
          if (!string.IsNullOrEmpty(shapeName))
          {
            MyObjectBuilder_FracturedPiece.Shape shape = new MyObjectBuilder_FracturedPiece.Shape()
            {
              Name = shapeName,
              Orientation = (SerializableQuaternion) Quaternion.CreateFromRotationMatrix(child.GetTransform().GetOrientation()),
              Fixed = MyDestructionHelper.IsFixed(child.Shape)
            };
            objectBuilder.Shapes.Add(shape);
          }
        }
        if (this.Physics.IsInWorld)
        {
          Vector3D world = this.Physics.ClusterToWorld(Vector3.Transform(this.m_children[0].GetTransform().Translation, this.Physics.RigidBody.GetRigidBodyMatrix()));
          MyPositionAndOrientation positionAndOrientation = objectBuilder.PositionAndOrientation.Value;
          positionAndOrientation.Position = (SerializableVector3D) world;
          objectBuilder.PositionAndOrientation = new MyPositionAndOrientation?(positionAndOrientation);
        }
        this.m_children.Clear();
      }
      else
        objectBuilder.Shapes.Add(new MyObjectBuilder_FracturedPiece.Shape()
        {
          Name = this.Physics.BreakableBody.BreakableShape.Name
        });
      return (MyObjectBuilder_EntityBase) objectBuilder;
    }

    public override void Init(MyObjectBuilder_EntityBase objectBuilder)
    {
      base.Init(objectBuilder);
      MyObjectBuilder_FracturedPiece builderFracturedPiece = objectBuilder as MyObjectBuilder_FracturedPiece;
      if (builderFracturedPiece.Shapes.Count == 0)
        return;
      foreach (MyObjectBuilder_FracturedPiece.Shape shape in builderFracturedPiece.Shapes)
      {
        MyRenderComponentFracturedPiece render = this.Render;
        string name = shape.Name;
        Matrix fromQuaternion = Matrix.CreateFromQuaternion((Quaternion) shape.Orientation);
        MatrixD localTransform = (MatrixD) ref fromQuaternion;
        render.AddPiece(name, localTransform);
      }
      this.OriginalBlocks.Clear();
      foreach (SerializableDefinitionId blockDefinition in builderFracturedPiece.BlockDefinitions)
      {
        string str = (string) null;
        MyPhysicalModelDefinition definition1;
        if (MyDefinitionManager.Static.TryGetDefinition<MyPhysicalModelDefinition>((MyDefinitionId) blockDefinition, out definition1))
          str = definition1.Model;
        MyCubeBlockDefinition definition2 = (MyCubeBlockDefinition) null;
        MyDefinitionManager.Static.TryGetDefinition<MyCubeBlockDefinition>((MyDefinitionId) blockDefinition, out definition2);
        if (str != null)
        {
          string model = definition1.Model;
          if (MyModels.GetModelOnlyData(model).HavokBreakableShapes == null)
            MyDestructionData.Static.LoadModelDestruction(model, definition1, Vector3.One);
          HkdBreakableShape havokBreakableShape1 = MyModels.GetModelOnlyData(model).HavokBreakableShapes[0];
          HkdShapeInstanceInfo shapeInstanceInfo = new HkdShapeInstanceInfo(havokBreakableShape1, new Quaternion?(), new Vector3?());
          this.m_children.Add(shapeInstanceInfo);
          havokBreakableShape1.GetChildren(this.m_children);
          if (definition2 != null && definition2.BuildProgressModels != null)
          {
            foreach (MyCubeBlockDefinition.BuildProgressModel buildProgressModel in definition2.BuildProgressModels)
            {
              string file = buildProgressModel.File;
              if (MyModels.GetModelOnlyData(file).HavokBreakableShapes == null)
                MyDestructionData.Static.LoadModelDestruction(file, (MyPhysicalModelDefinition) definition2, Vector3.One);
              HkdBreakableShape havokBreakableShape2 = MyModels.GetModelOnlyData(file).HavokBreakableShapes[0];
              shapeInstanceInfo = new HkdShapeInstanceInfo(havokBreakableShape2, new Quaternion?(), new Vector3?());
              this.m_children.Add(shapeInstanceInfo);
              havokBreakableShape2.GetChildren(this.m_children);
            }
          }
          this.OriginalBlocks.Add((MyDefinitionId) blockDefinition);
        }
      }
      this.m_shapes.AddRange((IEnumerable<MyObjectBuilder_FracturedPiece.Shape>) builderFracturedPiece.Shapes);
      Vector3? nullable = new Vector3?();
      int index1 = 0;
      for (int index2 = 0; index2 < this.m_children.Count; ++index2)
      {
        HkdShapeInstanceInfo child = this.m_children[index2];
        IEnumerable<MyObjectBuilder_FracturedPiece.Shape> source = this.m_shapes.Where<MyObjectBuilder_FracturedPiece.Shape>((Func<MyObjectBuilder_FracturedPiece.Shape, bool>) (s => s.Name == child.ShapeName));
        if (source.Count<MyObjectBuilder_FracturedPiece.Shape>() > 0)
        {
          MyObjectBuilder_FracturedPiece.Shape shape = source.First<MyObjectBuilder_FracturedPiece.Shape>();
          Matrix fromQuaternion = Matrix.CreateFromQuaternion((Quaternion) shape.Orientation);
          Matrix transform;
          if (!nullable.HasValue && shape.Name == this.m_shapes[0].Name)
          {
            ref Vector3? local = ref nullable;
            transform = child.GetTransform();
            Vector3 translation = transform.Translation;
            local = new Vector3?(translation);
            index1 = this.m_shapeInfos.Count;
          }
          ref Matrix local1 = ref fromQuaternion;
          transform = child.GetTransform();
          Vector3 translation1 = transform.Translation;
          local1.Translation = translation1;
          HkdShapeInstanceInfo shapeInstanceInfo = new HkdShapeInstanceInfo(child.Shape.Clone(), fromQuaternion);
          if (shape.Fixed)
            shapeInstanceInfo.Shape.SetFlagRecursively(HkdBreakableShape.Flags.IS_FIXED);
          this.m_shapeInfos.Add(shapeInstanceInfo);
          this.m_shapes.Remove(shape);
        }
        else
          child.GetChildren(this.m_children);
      }
      if (this.m_shapeInfos.Count == 0)
      {
        List<string> source = new List<string>();
        foreach (MyObjectBuilder_FracturedPiece.Shape shape in builderFracturedPiece.Shapes)
          source.Add(shape.Name);
        string str3 = source.Aggregate<string>((Func<string, string, string>) ((str1, str2) => str1 + ", " + str2));
        this.OriginalBlocks.Aggregate<MyDefinitionId, string>("", (Func<string, MyDefinitionId, string>) ((str, defId) => str + ", " + defId.ToString()));
        throw new Exception("No relevant shape was found for fractured piece. It was probably reexported and names changed. Shapes: " + str3 + ". Original blocks: " + str3);
      }
      HkdShapeInstanceInfo shapeInfo;
      if (nullable.HasValue)
      {
        for (int index2 = 0; index2 < this.m_shapeInfos.Count; ++index2)
        {
          shapeInfo = this.m_shapeInfos[index2];
          Matrix transform = shapeInfo.GetTransform();
          transform.Translation -= nullable.Value;
          shapeInfo = this.m_shapeInfos[index2];
          shapeInfo.SetTransform(ref transform);
        }
        shapeInfo = this.m_shapeInfos[index1];
        Matrix transform1 = shapeInfo.GetTransform();
        transform1.Translation = Vector3.Zero;
        shapeInfo = this.m_shapeInfos[index1];
        shapeInfo.SetTransform(ref transform1);
      }
      if (this.m_shapeInfos.Count > 0)
      {
        if (this.m_shapeInfos.Count == 1)
        {
          shapeInfo = this.m_shapeInfos[0];
          this.Shape = shapeInfo.Shape;
        }
        else
        {
          this.Shape = (HkdBreakableShape) new HkdCompoundBreakableShape((HkdBreakableShape) null, this.m_shapeInfos);
          ((HkdCompoundBreakableShape) this.Shape).RecalcMassPropsFromChildren();
        }
        this.Shape.SetStrenght(MyDestructionConstants.STRENGTH);
        HkMassProperties massProperties = new HkMassProperties();
        this.Shape.BuildMassProperties(ref massProperties);
        this.Shape.SetChildrenParent(this.Shape);
        this.Physics = new MyPhysicsBody((IMyEntity) this, RigidBodyFlag.RBF_DEBRIS);
        this.Physics.CanUpdateAccelerations = true;
        this.Physics.InitialSolverDeactivation = HkSolverDeactivation.High;
        this.Physics.CreateFromCollisionObject(this.Shape.GetShape(), Vector3.Zero, this.PositionComp.WorldMatrixRef, new HkMassProperties?(massProperties));
        this.Physics.BreakableBody = new HkdBreakableBody(this.Shape, this.Physics.RigidBody, (HkdWorld) null, (Matrix) ref this.PositionComp.WorldMatrixRef);
        this.Physics.BreakableBody.AfterReplaceBody += new BreakableBodyReplaced(this.Physics.FracturedBody_AfterReplaceBody);
        MyPhysicalModelDefinition definition;
        if (this.OriginalBlocks.Count > 0 && MyDefinitionManager.Static.TryGetDefinition<MyPhysicalModelDefinition>(this.OriginalBlocks[0], out definition))
          this.Physics.MaterialType = definition.PhysicalMaterial.Id.SubtypeId;
        HkRigidBody rigidBody = this.Physics.RigidBody;
        if (MyDestructionHelper.IsFixed(this.Physics.BreakableBody.BreakableShape))
        {
          rigidBody.UpdateMotionType(HkMotionType.Fixed);
          rigidBody.LinearVelocity = Vector3.Zero;
          rigidBody.AngularVelocity = Vector3.Zero;
        }
        this.Physics.Enabled = true;
      }
      this.m_children.Clear();
      this.m_shapeInfos.Clear();
    }

    internal void InitFromBreakableBody(HkdBreakableBody b, MatrixD worldMatrix, MyCubeBlock block)
    {
      this.OriginalBlocks.Clear();
      switch (block)
      {
        case null:
          HkRigidBody rigidBody = b.GetRigidBody();
          bool flag = MyDestructionHelper.IsFixed(b.BreakableShape);
          if (flag)
          {
            rigidBody.UpdateMotionType(HkMotionType.Fixed);
            rigidBody.LinearVelocity = Vector3.Zero;
            rigidBody.AngularVelocity = Vector3.Zero;
          }
          if (this.SyncFlag)
            this.CreateSync();
          this.PositionComp.SetWorldMatrix(ref worldMatrix);
          this.Physics.Flags = flag || !Sandbox.Game.Multiplayer.Sync.IsServer ? RigidBodyFlag.RBF_STATIC : RigidBodyFlag.RBF_DEBRIS;
          this.Physics.BreakableBody = b;
          rigidBody.UserObject = (object) this.Physics;
          if (!flag)
          {
            rigidBody.Motion.SetDeactivationClass(HkSolverDeactivation.High);
            rigidBody.EnableDeactivation = true;
            rigidBody.Layer = !MyFakes.REDUCE_FRACTURES_COUNT ? 15 : ((double) b.BreakableShape.Volume >= 1.0 || MyRandom.Instance.Next(6) <= 1 ? 15 : 14);
          }
          else
            rigidBody.Layer = 13;
          this.Physics.BreakableBody.AfterReplaceBody += new BreakableBodyReplaced(this.Physics.FracturedBody_AfterReplaceBody);
          MyPhysicalModelDefinition definition;
          if (this.OriginalBlocks.Count > 0 && MyDefinitionManager.Static.TryGetDefinition<MyPhysicalModelDefinition>(this.OriginalBlocks[0], out definition))
            this.Physics.MaterialType = definition.PhysicalMaterial.Id.SubtypeId;
          this.Physics.Enabled = true;
          MyDestructionHelper.FixPosition(this);
          this.SetDataFromHavok(b.BreakableShape);
          Vector3 centerOfMassLocal = b.GetRigidBody().CenterOfMassLocal;
          Vector3 centerOfMassWorld = b.GetRigidBody().CenterOfMassWorld;
          Vector3 coM = b.BreakableShape.CoM;
          b.GetRigidBody().CenterOfMassLocal = coM;
          b.BreakableShape.RemoveReference();
          break;
        case MyCompoundCubeBlock _:
          using (List<MySlimBlock>.Enumerator enumerator = (block as MyCompoundCubeBlock).GetBlocks().GetEnumerator())
          {
            while (enumerator.MoveNext())
              this.OriginalBlocks.Add(enumerator.Current.BlockDefinition.Id);
            goto case null;
          }
        case MyFracturedBlock _:
          this.OriginalBlocks.AddRange((IEnumerable<MyDefinitionId>) (block as MyFracturedBlock).OriginalBlocks);
          goto case null;
        default:
          this.OriginalBlocks.Add(block.BlockDefinition.Id);
          goto case null;
      }
    }

    public void SetDataFromHavok(HkdBreakableShape shape)
    {
      this.Shape = shape;
      this.Shape.AddReference();
      if (this.Render != null)
      {
        if (shape.IsCompound() || string.IsNullOrEmpty(shape.Name))
        {
          shape.GetChildren(this.m_shapeInfos);
          foreach (HkdShapeInstanceInfo shapeInfo in this.m_shapeInfos)
          {
            if (shapeInfo.IsValid())
            {
              MyRenderComponentFracturedPiece render = this.Render;
              string shapeName = shapeInfo.ShapeName;
              Matrix transform = shapeInfo.GetTransform();
              MatrixD localTransform = (MatrixD) ref transform;
              render.AddPiece(shapeName, localTransform);
            }
          }
          this.m_shapeInfos.Clear();
        }
        else
          this.Render.AddPiece(shape.Name, (MatrixD) ref Matrix.Identity);
      }
      this.m_hitPoints = this.Shape.Volume * 100f;
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      this.Physics.Enabled = true;
      this.Physics.RigidBody.Activate();
      this.Physics.RigidBody.ContactPointCallbackDelay = 0;
      this.Physics.RigidBody.ContactSoundCallbackEnabled = true;
      if (this.InitialHit != null)
      {
        this.Physics.ApplyImpulse(this.InitialHit.Impulse, this.Physics.CenterOfMassWorld);
        MyPhysics.FractureImpactDetails details = new MyPhysics.FractureImpactDetails();
        details.Entity = (MyEntity) this;
        details.World = this.Physics.HavokWorld;
        details.ContactInWorld = this.InitialHit.Position;
        HkdFractureImpactDetails fractureImpactDetails = HkdFractureImpactDetails.Create();
        fractureImpactDetails.SetBreakingBody(this.Physics.RigidBody);
        fractureImpactDetails.SetContactPoint((Vector3) this.Physics.WorldToCluster(this.InitialHit.Position));
        fractureImpactDetails.SetDestructionRadius(0.05f);
        fractureImpactDetails.SetBreakingImpulse(30000f);
        fractureImpactDetails.SetParticleVelocity(this.InitialHit.Impulse);
        fractureImpactDetails.SetParticlePosition((Vector3) this.Physics.WorldToCluster(this.InitialHit.Position));
        fractureImpactDetails.SetParticleMass(500f);
        details.Details = fractureImpactDetails;
        MyPhysics.EnqueueDestruction(details);
      }
      this.Physics.RigidBody.Gravity = MyGravityProviderSystem.CalculateTotalGravityInPoint(this.PositionComp.GetPosition());
    }

    public void RegisterObstacleContact(ref HkContactPointEvent e)
    {
      if (this.m_obstacleContact || !this.m_fallSoundShouldPlay.Value || (DateTime.UtcNow - this.m_soundStart).TotalSeconds < 1.0)
        return;
      this.m_obstacleContact = true;
    }

    private void SetFallSound()
    {
      if (this.OriginalBlocks == null || !this.OriginalBlocks[0].TypeId.ToString().Equals("MyObjectBuilder_Tree"))
        return;
      this.m_fallSound = new MySoundPair(this.m_fallSoundString.Value);
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
    }

    public void StartFallSound(string sound)
    {
      this.m_groundContact = false;
      this.m_obstacleContact = false;
      this.m_fallSoundString.Value = sound;
      this.m_soundStart = DateTime.UtcNow;
      this.m_fallSoundShouldPlay.Value = true;
      if (!this.m_contactSet && (Sandbox.Engine.Platform.Game.IsDedicated || MyMultiplayer.Static == null || MyMultiplayer.Static.IsServer))
        this.Physics.RigidBody.ContactSoundCallback += new HkContactPointEventHandler(this.RegisterObstacleContact);
      this.m_contactSet = true;
    }

    public override void UpdateAfterSimulation10()
    {
      base.UpdateAfterSimulation10();
      if (this.m_markedBreakImpulse != MyTimeSpan.Zero)
        this.UnmarkEntityBreakable(true);
      if (this.m_fallSoundShouldPlay.Value || (double) this.Physics.LinearVelocity.LengthSquared() <= 25.0 || (DateTime.UtcNow - this.m_soundStart).TotalSeconds < 1.0)
        return;
      this.m_fallSoundShouldPlay.Value = true;
      this.m_obstacleContact = false;
      this.m_groundContact = false;
    }

    public override void UpdateBeforeSimulation()
    {
      base.UpdateBeforeSimulation();
      if (!Sandbox.Engine.Platform.Game.IsDedicated)
      {
        if (this.m_fallSoundShouldPlay.Value)
        {
          if (this.m_soundEmitter == null)
            this.m_soundEmitter = new MyEntity3DSoundEmitter((MyEntity) this);
          if (!this.m_soundEmitter.IsPlaying && this.m_fallSound != null && this.m_fallSound != MySoundPair.Empty)
            this.m_soundEmitter.PlaySound(this.m_fallSound, true, true);
        }
        else if (this.m_soundEmitter != null && this.m_soundEmitter.IsPlaying)
          this.m_soundEmitter.StopSound(false);
      }
      if (!this.m_obstacleContact || this.m_groundContact)
        return;
      if ((double) this.Physics.LinearVelocity.Y > 0.0 || (double) this.Physics.LinearVelocity.LengthSquared() < 9.0)
      {
        this.m_groundContact = true;
        this.m_fallSoundShouldPlay.Value = false;
        this.m_soundStart = DateTime.UtcNow;
      }
      else
        this.m_obstacleContact = false;
    }

    public override void UpdateBeforeSimulation100()
    {
      base.UpdateBeforeSimulation100();
      if (this.m_soundEmitter != null)
      {
        this.m_soundEmitter.Update();
        if (this.m_soundEmitter.IsPlaying && (DateTime.UtcNow - this.m_soundStart).TotalSeconds >= 15.0)
          this.m_fallSoundShouldPlay.Value = false;
      }
      this.Physics.RigidBody.Gravity = MyGravityProviderSystem.CalculateTotalGravityInPoint(this.PositionComp.GetPosition());
    }

    private void UnmarkEntityBreakable(bool checkTime)
    {
      if (!(this.m_markedBreakImpulse != MyTimeSpan.Zero) || checkTime && !(MySandboxGame.Static.TotalTime - this.m_markedBreakImpulse > MyTimeSpan.FromSeconds(1.5)))
        return;
      this.m_markedBreakImpulse = MyTimeSpan.Zero;
      if (this.Physics == null || this.Physics.HavokWorld == null)
        return;
      this.Physics.HavokWorld.BreakOffPartsUtil.UnmarkEntityBreakable((HkEntity) this.Physics.RigidBody);
      if (!checkTime)
        return;
      this.CreateEasyPenetrationAction(1f);
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      MyCubeBlockDefinition blockDefinition = (MyCubeBlockDefinition) null;
      if (this.Physics.HavokWorld == null || this.OriginalBlocks.Count == 0 || (!MyDefinitionManager.Static.TryGetCubeBlockDefinition(this.OriginalBlocks[0], out blockDefinition) || blockDefinition.CubeSize != MyCubeSize.Large))
        return;
      this.Physics.HavokWorld.BreakOffPartsUtil.MarkEntityBreakable((HkEntity) this.Physics.RigidBody, this.Physics.Mass * 0.4f);
      this.m_markedBreakImpulse = MySandboxGame.Static.TotalTime;
    }

    public override void OnRemovedFromScene(object source)
    {
      base.OnRemovedFromScene(source);
      this.UnmarkEntityBreakable(false);
      if (this.m_soundEmitter != null)
        this.m_soundEmitter.StopSound(true);
      if (this.OnRemove == null)
        return;
      this.OnRemove((MyEntity) this);
    }

    private void CreateEasyPenetrationAction(float duration)
    {
      if (this.Physics == null || !((HkReferenceObject) this.Physics.RigidBody != (HkReferenceObject) null))
        return;
      this.m_easePenetrationAction = new HkEasePenetrationAction(this.Physics.RigidBody, duration);
      this.m_easePenetrationAction.InitialAllowedPenetrationDepthMultiplier = 5f;
      this.m_easePenetrationAction.InitialAdditionalAllowedPenetrationDepth = 2f;
    }

    public void OnDestroy()
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      MyFracturedPiecesManager.Static.RemoveFracturePiece(this, 2f);
    }

    public bool DoDamage(
      float damage,
      MyStringHash damageType,
      bool sync,
      MyHitInfo? hitInfo,
      long attackerId,
      long realHitEntityId = 0)
    {
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        MyDamageInformation info = new MyDamageInformation(false, damage, damageType, attackerId);
        if (this.UseDamageSystem)
          MyDamageSystem.Static.RaiseBeforeDamageApplied((object) this, ref info);
        this.m_hitPoints -= info.Amount;
        if (this.UseDamageSystem)
          MyDamageSystem.Static.RaiseAfterDamageApplied((object) this, info);
        if ((double) this.m_hitPoints <= 0.0)
        {
          MyFracturedPiecesManager.Static.RemoveFracturePiece(this, 2f);
          if (this.UseDamageSystem)
            MyDamageSystem.Static.RaiseDestroyed((object) this, info);
        }
      }
      return true;
    }

    public float Integrity => this.m_hitPoints;

    public bool UseDamageSystem { get; private set; }

    public void DebugCheckValidShapes()
    {
      bool flag1 = false;
      HashSet<Tuple<string, float>> outNamesAndBuildProgress1 = new HashSet<Tuple<string, float>>();
      HashSet<Tuple<string, float>> outNamesAndBuildProgress2 = new HashSet<Tuple<string, float>>();
      foreach (MyDefinitionId originalBlock in this.OriginalBlocks)
      {
        MyCubeBlockDefinition blockDefinition;
        if (MyDefinitionManager.Static.TryGetCubeBlockDefinition(originalBlock, out blockDefinition))
        {
          flag1 = true;
          MyFracturedBlock.GetAllBlockBreakableShapeNames(blockDefinition, outNamesAndBuildProgress1);
        }
      }
      MyFracturedBlock.GetAllBlockBreakableShapeNames(this.Shape, outNamesAndBuildProgress2, 0.0f);
      foreach (Tuple<string, float> tuple1 in outNamesAndBuildProgress2)
      {
        bool flag2 = false;
        foreach (Tuple<string, float> tuple2 in outNamesAndBuildProgress1)
        {
          if (tuple1.Item1 == tuple2.Item1)
          {
            flag2 = true;
            break;
          }
        }
        if (!flag2 & flag1)
          tuple1.Item1.ToLower().Contains("compound");
      }
    }

    public class HitInfo
    {
      public Vector3D Position;
      public Vector3 Impulse;
    }

    private class MyFracturedPieceDebugDraw : MyDebugRenderComponentBase
    {
      private MyFracturedPiece m_piece;

      public MyFracturedPieceDebugDraw(MyFracturedPiece piece) => this.m_piece = piece;

      public override void DebugDraw()
      {
        if (!MyDebugDrawSettings.DEBUG_DRAW_FRACTURED_PIECES)
          return;
        MyRenderProxy.DebugDrawAxis(this.m_piece.WorldMatrix, 1f, false);
        if (this.m_piece.Physics == null || !((HkReferenceObject) this.m_piece.Physics.RigidBody != (HkReferenceObject) null))
          return;
        MyPhysicsBody physics = this.m_piece.Physics;
        HkRigidBody rigidBody = physics.RigidBody;
        Vector3 world = (Vector3) physics.ClusterToWorld(rigidBody.CenterOfMassWorld);
        BoundingBoxD boundingBoxD = new BoundingBoxD(world - Vector3D.One * 0.100000001490116, world + Vector3D.One * 0.100000001490116);
        string text = string.Format("{0}\n, {1}\n{2}", (object) rigidBody.GetMotionType(), (object) physics.Friction, (object) physics.Entity.EntityId.ToString().Substring(0, 5));
        MyRenderProxy.DebugDrawText3D((Vector3D) world, text, Color.White, 0.6f, false);
      }

      public override void DebugDrawInvalidTriangles()
      {
      }
    }

    protected class m_fallSoundShouldPlay\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.FromServer>(obj1, obj2));
        ((MyFracturedPiece) obj0).m_fallSoundShouldPlay = (VRage.Sync.Sync<bool, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_fallSoundString\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<string, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<string, SyncDirection.FromServer>(obj1, obj2));
        ((MyFracturedPiece) obj0).m_fallSoundString = (VRage.Sync.Sync<string, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_MyFracturedPiece\u003C\u003EActor : IActivator, IActivator<MyFracturedPiece>
    {
      object IActivator.CreateInstance() => (object) new MyFracturedPiece();

      MyFracturedPiece IActivator<MyFracturedPiece>.CreateInstance() => new MyFracturedPiece();
    }
  }
}
