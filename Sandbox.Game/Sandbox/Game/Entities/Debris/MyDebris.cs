// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Debris.MyDebris
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using ParallelTasks;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Debris
{
  [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
  public class MyDebris : MySessionComponentBase
  {
    private static MyDebris m_static;
    private List<Vector3D> m_positionBuffer;
    private List<Vector3> m_voxelDebrisOffsets;
    private static string[] m_debrisModels;
    private static MyDebrisDefinition[] m_debrisVoxels;
    public static readonly float VoxelDebrisModelVolume = 0.15f;
    private MyConcurrentDictionary<MyDebris.MyModelShapeInfo, HkShape> m_shapes = new MyConcurrentDictionary<MyDebris.MyModelShapeInfo, HkShape>();
    private const int MaxDebrisCount = 33;
    private int m_debrisCount;
    private Queue<MyDebris.DebrisCreationInfo> m_creationBuffer = new Queue<MyDebris.DebrisCreationInfo>(33);
    private MyDebrisBaseDescription m_desc = new MyDebrisBaseDescription();
    private int m_debrisModelIndex;

    public static MyDebris Static
    {
      get => MyDebris.m_static;
      private set => MyDebris.m_static = value;
    }

    public MyDebris()
    {
      MyDebris.m_debrisModels = MyDefinitionManager.Static.GetDebrisDefinitions().Where<MyDebrisDefinition>((Func<MyDebrisDefinition, bool>) (x => x.Type == MyDebrisType.Model)).Select<MyDebrisDefinition, string>((Func<MyDebrisDefinition, string>) (x => x.Model)).ToArray<string>();
      MyDebris.m_debrisVoxels = MyDefinitionManager.Static.GetDebrisDefinitions().Where<MyDebrisDefinition>((Func<MyDebrisDefinition, bool>) (x => x.Type == MyDebrisType.Voxel)).OrderByDescending<MyDebrisDefinition, float>((Func<MyDebrisDefinition, float>) (x => x.MinAmount)).ToArray<MyDebrisDefinition>();
    }

    public override System.Type[] Dependencies => new System.Type[1]
    {
      typeof (MyPhysics)
    };

    private void EnqueueDebrisCreation(MyDebris.DebrisCreationInfo info)
    {
      while (this.m_creationBuffer.Count >= 33)
        this.m_creationBuffer.Dequeue();
      if (!MyFakes.ENABLE_DEBRIS)
        return;
      this.m_creationBuffer.Enqueue(info);
    }

    public HkShape GetDebrisShape(MyModel model, HkShapeType shapeType, float scale)
    {
      MyDebris.MyModelShapeInfo key = new MyDebris.MyModelShapeInfo();
      key.Model = model;
      key.ShapeType = shapeType;
      key.Scale = scale;
      HkShape hkShape;
      if (!this.m_shapes.TryGetValue(key, out hkShape))
      {
        hkShape = this.CreateShape(model, shapeType, scale);
        if (!this.m_shapes.TryAdd(key, hkShape))
        {
          hkShape.RemoveReference();
          hkShape = this.m_shapes.GetValueOrDefault(key, HkShape.Empty);
        }
      }
      return hkShape;
    }

    private HkShape CreateShape(MyModel model, HkShapeType shapeType, float scale)
    {
      if (model.HavokCollisionShapes != null && model.HavokCollisionShapes.Length != 0)
      {
        HkShape hkShape;
        if (model.HavokCollisionShapes.Length == 1)
        {
          hkShape = model.HavokCollisionShapes[0];
          hkShape.AddReference();
        }
        else
          hkShape = (HkShape) new HkListShape(model.HavokCollisionShapes, HkReferencePolicy.None);
        return hkShape;
      }
      switch (shapeType)
      {
        case HkShapeType.Sphere:
          return (HkShape) new HkSphereShape(scale * model.BoundingSphere.Radius);
        case HkShapeType.Box:
          return (HkShape) new HkBoxShape(Vector3.Max(scale * (model.BoundingBox.Max - model.BoundingBox.Min) / 2f - 0.05f, new Vector3(0.025f)), 0.02f);
        case HkShapeType.ConvexVertices:
          Vector3[] verts = new Vector3[model.GetVerticesCount()];
          for (int vertexIndex = 0; vertexIndex < model.GetVerticesCount(); ++vertexIndex)
            verts[vertexIndex] = scale * model.GetVertex(vertexIndex);
          return (HkShape) new HkConvexVerticesShape(verts, verts.Length, true, 0.1f);
        default:
          throw new InvalidOperationException("This shape is not supported");
      }
    }

    public bool TooManyDebris => this.m_debrisCount > 33;

    public override void LoadData()
    {
      this.m_positionBuffer = new List<Vector3D>(24);
      this.m_voxelDebrisOffsets = new List<Vector3>(8);
      this.m_desc.LifespanMinInMiliseconds = 10000;
      this.m_desc.LifespanMaxInMiliseconds = 20000;
      this.m_desc.OnCloseAction = new Action<MyDebrisBase>(this.OnDebrisClosed);
      this.GenerateVoxelDebrisPositionOffsets(this.m_voxelDebrisOffsets);
      MyDebris.Static = this;
    }

    private void OnDebrisClosed(MyDebrisBase obj) => Interlocked.Decrement(ref this.m_debrisCount);

    protected override void UnloadData()
    {
      if (MyDebris.Static == null)
        return;
      foreach (KeyValuePair<MyDebris.MyModelShapeInfo, HkShape> shape in this.m_shapes)
        shape.Value.RemoveReference();
      this.m_shapes.Clear();
      this.m_positionBuffer = (List<Vector3D>) null;
      MyDebris.Static = (MyDebris) null;
      this.m_creationBuffer.Clear();
    }

    public void CreateDirectedDebris(
      Vector3 sourceWorldPosition,
      Vector3 offsetDirection,
      float minSourceDistance,
      float maxSourceDistance,
      float minDeviationAngle,
      float maxDeviationAngle,
      int debrisPieces,
      float initialSpeed)
    {
      MyDebris.MyCreateDebrisWork createDebrisWork = MyDebris.MyCreateDebrisWork.Create();
      createDebrisWork.sourceWorldPosition = sourceWorldPosition;
      createDebrisWork.offsetDirection = offsetDirection;
      createDebrisWork.minSourceDistance = minSourceDistance;
      createDebrisWork.maxSourceDistance = maxSourceDistance;
      createDebrisWork.minDeviationAngle = minDeviationAngle;
      createDebrisWork.maxDeviationAngle = maxDeviationAngle;
      createDebrisWork.debrisPieces = debrisPieces;
      createDebrisWork.initialSpeed = initialSpeed;
      createDebrisWork.Context = this;
      Parallel.Start((IWork) createDebrisWork, createDebrisWork.CompletionCallback);
    }

    public void CreateDirectedDebris(
      Vector3 sourceWorldPosition,
      Vector3 offsetDirection,
      float minSourceDistance,
      float maxSourceDistance,
      float minDeviationAngle,
      float maxDeviationAngle,
      int debrisPieces,
      float initialSpeed,
      float maxAmount,
      MyVoxelMaterialDefinition material)
    {
      for (int index = 0; index < debrisPieces; ++index)
      {
        float randomFloat1 = MyUtils.GetRandomFloat(minSourceDistance, maxSourceDistance);
        double randomFloat2 = (double) MyUtils.GetRandomFloat(minDeviationAngle, maxDeviationAngle);
        float randomFloat3 = MyUtils.GetRandomFloat(minDeviationAngle, maxDeviationAngle);
        Matrix matrix = Matrix.CreateRotationX((float) randomFloat2) * Matrix.CreateRotationY(randomFloat3);
        Vector3 vector3_1 = Vector3.Transform(offsetDirection, matrix);
        Vector3 vector3_2 = sourceWorldPosition + vector3_1 * randomFloat1;
        Vector3 vector3_3 = vector3_1 * initialSpeed;
        this.EnqueueDebrisCreation(new MyDebris.DebrisCreationInfo()
        {
          Type = MyDebris.DebrisType.Voxel,
          Position = (Vector3D) vector3_2,
          Velocity = vector3_3,
          Material = material,
          Ammount = maxAmount
        });
      }
    }

    public void CreateExplosionDebris(ref BoundingSphereD explosionSphere, MyEntity entity)
    {
      BoundingBoxD worldAabb = entity.PositionComp.WorldAABB;
      this.CreateExplosionDebris(ref explosionSphere, entity, ref worldAabb);
    }

    public void CreateExplosionDebris(
      ref BoundingSphereD explosionSphere,
      MyEntity entity,
      ref BoundingBoxD bb,
      float scaleMultiplier = 1f,
      bool applyVelocity = true)
    {
      MyUtils.GetRandomVector3Normalized();
      double randomFloat = (double) MyUtils.GetRandomFloat(0.0f, (float) explosionSphere.Radius);
      this.GeneratePositions(bb, this.m_positionBuffer);
      Vector3 vector3_1 = entity.Physics != null ? entity.Physics.LinearVelocity : Vector3.Zero;
      foreach (Vector3D vector3D in this.m_positionBuffer)
      {
        Vector3 vector3_2 = applyVelocity ? MyUtils.GetRandomVector3Normalized() * MyUtils.GetRandomFloat(4f, 8f) + vector3_1 : Vector3.Zero;
        this.EnqueueDebrisCreation(new MyDebris.DebrisCreationInfo()
        {
          Type = MyDebris.DebrisType.Random,
          Position = vector3D,
          Velocity = vector3_2
        });
      }
    }

    public void CreateExplosionDebris(
      ref BoundingSphereD explosionSphere,
      float voxelsCountInPercent,
      MyVoxelMaterialDefinition voxelMaterial,
      MyVoxelBase voxelMap)
    {
      MatrixD matrix = MatrixD.CreateRotationX((double) MyUtils.GetRandomRadian()) * MatrixD.CreateRotationY((double) MyUtils.GetRandomRadian());
      int count1 = this.m_voxelDebrisOffsets.Count;
      int count2 = this.m_voxelDebrisOffsets.Count;
      for (int index = 0; index < count2; ++index)
      {
        MyDebrisVoxel voxelDebris = this.CreateVoxelDebris((float) explosionSphere.Radius * 100f, (float) explosionSphere.Radius * 1000f);
        if (voxelDebris == null)
          break;
        Vector3D result = (Vector3D) (this.m_voxelDebrisOffsets[index] * (float) explosionSphere.Radius * 0.5780347f);
        Vector3D.Transform(ref result, ref matrix, out result);
        Vector3D position = result + explosionSphere.Center;
        Vector3 vector3Normalized = MyUtils.GetRandomVector3Normalized();
        if (!(vector3Normalized == Vector3.Zero))
        {
          Vector3 vector3 = vector3Normalized * MyUtils.GetRandomFloat(4f, 8f);
          (voxelDebris.Debris as MyDebrisVoxel.MyDebrisVoxelLogic).Start(position, (Vector3D) vector3, voxelMaterial);
        }
      }
    }

    public override void UpdateBeforeSimulation()
    {
      base.UpdateBeforeSimulation();
      int count = this.m_creationBuffer.Count;
      if (count > 20)
        count /= 10;
      while (count-- > 0)
      {
        MyDebris.DebrisCreationInfo debrisCreationInfo = this.m_creationBuffer.Dequeue();
        switch (debrisCreationInfo.Type)
        {
          case MyDebris.DebrisType.Voxel:
            (this.CreateVoxelDebris(50f, debrisCreationInfo.Ammount).Debris as MyDebrisVoxel.MyDebrisVoxelLogic).Start(debrisCreationInfo.Position, (Vector3D) debrisCreationInfo.Velocity, debrisCreationInfo.Material);
            continue;
          case MyDebris.DebrisType.Random:
            MyDebrisBase randomDebris = this.CreateRandomDebris();
            if (randomDebris != null)
            {
              randomDebris.Debris.Start(debrisCreationInfo.Position, (Vector3D) debrisCreationInfo.Velocity);
              continue;
            }
            continue;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }

    private void GeneratePositions(BoundingBoxD boundingBox, List<Vector3D> positionBuffer)
    {
      positionBuffer.Clear();
      Vector3D vector3D1 = boundingBox.Max - boundingBox.Min;
      double num1 = vector3D1.X * vector3D1.Y * vector3D1.Z;
      int num2 = 24;
      if (num1 < 1.0)
        num2 = 1;
      else if (num1 < 10.0)
        num2 = 12;
      else if (num1 > 100.0)
        num2 = 48;
      double num3 = Math.Pow((double) num2 / num1, 0.333333343267441);
      Vector3D vector3D2 = vector3D1 * num3;
      int num4 = (int) Math.Ceiling(vector3D2.X);
      int num5 = (int) Math.Ceiling(vector3D2.Y);
      int num6 = (int) Math.Ceiling(vector3D2.Z);
      Vector3D vector3D3 = new Vector3D(vector3D1.X / (double) num4, vector3D1.Y / (double) num5, vector3D1.Z / (double) num6);
      Vector3D vector3D4 = boundingBox.Min + 0.5 * vector3D3;
      for (int index1 = 0; index1 < num4; ++index1)
      {
        for (int index2 = 0; index2 < num5; ++index2)
        {
          for (int index3 = 0; index3 < num6; ++index3)
          {
            Vector3D vector3D5 = vector3D4 + new Vector3D((double) index1 * vector3D3.X, (double) index2 * vector3D3.Y, (double) index3 * vector3D3.Z);
            positionBuffer.Add(vector3D5);
          }
        }
      }
    }

    private void GenerateVoxelDebrisPositionOffsets(List<Vector3> offsetBuffer)
    {
      offsetBuffer.Clear();
      Vector3 vector3_1 = new Vector3(-0.7f);
      for (int index1 = 0; index1 < 2; ++index1)
      {
        for (int index2 = 0; index2 < 2; ++index2)
        {
          for (int index3 = 0; index3 < 2; ++index3)
          {
            Vector3 vector3_2 = vector3_1 + new Vector3((float) index1 * 1.4f, (float) index2 * 1.4f, (float) index3 * 1.4f);
            offsetBuffer.Add(vector3_2);
          }
        }
      }
    }

    public static string GetRandomDebrisModel() => MyDebris.m_debrisModels.GetRandomItem<string>();

    public static string GetRandomDebrisVoxel() => MyDebris.m_debrisVoxels.GetRandomItem<MyDebrisDefinition>().Model;

    public static string GetAmountBasedDebrisVoxel(float amount)
    {
      foreach (MyDebrisDefinition debrisVoxel in MyDebris.m_debrisVoxels)
      {
        if ((double) debrisVoxel.MinAmount <= (double) amount)
          return debrisVoxel.Model;
      }
      return MyDebris.m_debrisVoxels[0].Model;
    }

    public static string GetAnyAmountLessDebrisVoxel(float minAmount, float maxAmount)
    {
      int num1 = 0;
      int num2 = 0;
      foreach (MyDebrisDefinition debrisVoxel in MyDebris.m_debrisVoxels)
      {
        if ((double) debrisVoxel.MinAmount > (double) maxAmount)
          ++num1;
        if ((double) debrisVoxel.MinAmount > (double) minAmount)
          ++num2;
      }
      int index = MyUtils.GetRandomInt(num2 - num1 + 1) + num1;
      return MyDebris.m_debrisVoxels[index].Model;
    }

    private MyDebrisVoxel CreateVoxelDebris(float minAmount, float maxAmount)
    {
      MyDebrisVoxel myDebrisVoxel = new MyDebrisVoxel();
      this.m_desc.Model = MyDebris.GetAnyAmountLessDebrisVoxel(minAmount, maxAmount);
      myDebrisVoxel.Debris.Init(this.m_desc);
      Interlocked.Increment(ref this.m_debrisCount);
      return myDebrisVoxel;
    }

    private MyDebrisBase CreateRandomDebris()
    {
      MyDebrisBase myDebrisBase = (MyDebrisBase) null;
      if (this.m_debrisModelIndex < MyDebris.m_debrisModels.Length)
      {
        int debrisModelIndex = this.m_debrisModelIndex;
        if (debrisModelIndex > MyDebris.m_debrisModels.Length)
          this.m_debrisModelIndex = (debrisModelIndex %= MyDebris.m_debrisModels.Length);
        myDebrisBase = (MyDebrisBase) this.CreateDebris(MyDebris.m_debrisModels[debrisModelIndex]);
        ++this.m_debrisModelIndex;
      }
      return myDebrisBase;
    }

    public MyEntity CreateDebris(string model)
    {
      if (!MyFakes.ENABLE_DEBRIS)
        return (MyEntity) null;
      MyDebrisBase myDebrisBase = new MyDebrisBase();
      this.m_desc.Model = model;
      myDebrisBase.Debris.Init(this.m_desc);
      Interlocked.Increment(ref this.m_debrisCount);
      this.m_desc.LifespanMinInMiliseconds = 4000;
      this.m_desc.LifespanMaxInMiliseconds = 7000;
      return (MyEntity) myDebrisBase;
    }

    public MyEntity CreateTreeDebris(string model)
    {
      MyDebrisTree myDebrisTree = new MyDebrisTree();
      this.m_desc.Model = model;
      myDebrisTree.Debris.Init(this.m_desc);
      Interlocked.Increment(ref this.m_debrisCount);
      this.m_desc.LifespanMinInMiliseconds = 4000;
      this.m_desc.LifespanMaxInMiliseconds = 7000;
      return (MyEntity) myDebrisTree;
    }

    private struct MyModelShapeInfo
    {
      public MyModel Model;
      public HkShapeType ShapeType;
      public float Scale;
    }

    private enum DebrisType
    {
      Voxel,
      Random,
    }

    private struct DebrisCreationInfo
    {
      public MyDebris.DebrisType Type;
      public float Ammount;
      public Vector3 Velocity;
      public Vector3D Position;
      public MyVoxelMaterialDefinition Material;
    }

    private class MyCreateDebrisWork : AbstractWork
    {
      private static Stack<MyDebris.MyCreateDebrisWork> m_pool = new Stack<MyDebris.MyCreateDebrisWork>();
      public readonly Action CompletionCallback;
      private readonly List<MyDebris.MyCreateDebrisWork.DebrisData> m_pieces = new List<MyDebris.MyCreateDebrisWork.DebrisData>();
      public MyDebris Context;
      public int debrisPieces;
      public float initialSpeed;
      public float minSourceDistance;
      public float maxSourceDistance;
      public float minDeviationAngle;
      public float maxDeviationAngle;
      public Vector3 offsetDirection;
      public Vector3 sourceWorldPosition;

      public static MyDebris.MyCreateDebrisWork Create()
      {
        if (MyDebris.MyCreateDebrisWork.m_pool.Count != 0)
          return MyDebris.MyCreateDebrisWork.m_pool.Pop();
        MyDebris.MyCreateDebrisWork createDebrisWork = new MyDebris.MyCreateDebrisWork();
        createDebrisWork.Options = Parallel.DefaultOptions;
        return createDebrisWork;
      }

      private void Release()
      {
        this.Context = (MyDebris) null;
        this.m_pieces.Clear();
        MyDebris.MyCreateDebrisWork.m_pool.Push(this);
      }

      private MyCreateDebrisWork() => this.CompletionCallback = new Action(this.OnWorkCompleted);

      public override void DoWork(WorkData unused)
      {
        if (!MySession.Static.Ready)
          return;
        MyEntityIdentifier.InEntityCreationBlock = true;
        MyEntityIdentifier.LazyInitPerThreadStorage(2048);
        for (int index = 0; index < this.debrisPieces; ++index)
        {
          MyDebrisBase randomDebris = this.Context.CreateRandomDebris();
          if (randomDebris != null)
          {
            float randomFloat1 = MyUtils.GetRandomFloat(this.minSourceDistance, this.maxSourceDistance);
            double randomFloat2 = (double) MyUtils.GetRandomFloat(this.minDeviationAngle, this.maxDeviationAngle);
            float randomFloat3 = MyUtils.GetRandomFloat(this.minDeviationAngle, this.maxDeviationAngle);
            Vector3 vector3_1 = Vector3.Transform(this.offsetDirection, Matrix.CreateRotationX((float) randomFloat2) * Matrix.CreateRotationY(randomFloat3));
            Vector3 vector3_2 = this.sourceWorldPosition + vector3_1 * randomFloat1;
            Vector3 vector3_3 = vector3_1 * this.initialSpeed;
            this.m_pieces.Add(new MyDebris.MyCreateDebrisWork.DebrisData()
            {
              Object = randomDebris,
              StartPos = vector3_2,
              InitialVelocity = vector3_3
            });
          }
          else
            break;
        }
        MyEntityIdentifier.ClearPerThreadEntities();
        MyEntityIdentifier.InEntityCreationBlock = false;
      }

      private void OnWorkCompleted()
      {
        if (!MySession.Static.Ready)
          return;
        MyEntityIdentifier.InEntityCreationBlock = true;
        foreach (MyDebris.MyCreateDebrisWork.DebrisData piece in this.m_pieces)
        {
          MyDebrisBase myDebrisBase = piece.Object;
          MyEntityIdentifier.AddEntityWithId((IMyEntity) myDebrisBase);
          myDebrisBase.Debris.Start((Vector3D) piece.StartPos, (Vector3D) piece.InitialVelocity);
        }
        MyEntityIdentifier.InEntityCreationBlock = false;
        this.Release();
      }

      private struct DebrisData
      {
        public MyDebrisBase Object;
        public Vector3 InitialVelocity;
        public Vector3 StartPos;
      }

      private class Sandbox_Game_Entities_Debris_MyDebris\u003C\u003EMyCreateDebrisWork\u003C\u003EActor : IActivator, IActivator<MyDebris.MyCreateDebrisWork>
      {
        object IActivator.CreateInstance() => (object) new MyDebris.MyCreateDebrisWork();

        MyDebris.MyCreateDebrisWork IActivator<MyDebris.MyCreateDebrisWork>.CreateInstance() => new MyDebris.MyCreateDebrisWork();
      }
    }
  }
}
