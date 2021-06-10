// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyCompositeShapeProvider
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VRage.Game;
using VRage.Noise;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.World.Generator
{
  [MyStorageDataProvider(10002)]
  internal sealed class MyCompositeShapeProvider : MyCompositeShapeProviderBase, IMyStorageDataProvider
  {
    private const uint CURRENT_VERSION = 3;
    private const uint VERSION_WITHOUT_PLANETS = 1;
    private const uint VERSION_WITHOUT_GENERATOR_SEED = 2;
    private MyCompositeShapeProvider.State m_state;

    private void InitFromState(MyCompositeShapeProvider.State state)
    {
      this.m_state = state;
      this.m_infoProvider = MyCompositeShapes.AsteroidGenerators[state.Generator](state.GeneratorSeed, state.Seed, state.Size);
    }

    public override int SerializedSize => sizeof (MyCompositeShapeProvider.State);

    public override void WriteTo(Stream stream)
    {
      stream.WriteNoAlloc(this.m_state.Version);
      stream.WriteNoAlloc(this.m_state.Generator);
      stream.WriteNoAlloc(this.m_state.Seed);
      stream.WriteNoAlloc(this.m_state.Size);
      stream.WriteNoAlloc(this.m_state.UnusedCompat);
      stream.WriteNoAlloc(this.m_state.GeneratorSeed);
    }

    public override void ReadFrom(
      int storageVersion,
      Stream stream,
      int size,
      ref bool isOldFormat)
    {
      MyCompositeShapeProvider.State state;
      state.Version = stream.ReadUInt32();
      if (state.Version != 3U)
        isOldFormat = true;
      state.Generator = stream.ReadInt32();
      state.Seed = stream.ReadInt32();
      state.Size = stream.ReadFloat();
      if (state.Version == 1U)
      {
        state.UnusedCompat = 0U;
        state.GeneratorSeed = 0;
      }
      else
      {
        state.UnusedCompat = stream.ReadUInt32();
        if (state.UnusedCompat == 1U)
          throw new InvalidBranchException();
        if (state.Version <= 2U)
        {
          isOldFormat = true;
          state.GeneratorSeed = 0;
        }
        else
          state.GeneratorSeed = stream.ReadInt32();
      }
      this.InitFromState(state);
      this.m_state.Version = 3U;
    }

    private new static void SetupReading(
      int lodIndex,
      ref Vector3I minInLod,
      ref Vector3I maxInLod,
      out int lodVoxelSize,
      out BoundingBox queryBox,
      out BoundingSphere querySphere)
    {
      float num = 0.5f * (float) (1 << lodIndex);
      lodVoxelSize = (int) ((double) num * 2.0);
      Vector3I voxelCoord1 = minInLod << lodIndex;
      Vector3I voxelCoord2 = maxInLod << lodIndex;
      Vector3 localPosition;
      MyVoxelCoordSystems.VoxelCoordToLocalPosition(ref voxelCoord1, out localPosition);
      Vector3 vector3_1 = localPosition;
      MyVoxelCoordSystems.VoxelCoordToLocalPosition(ref voxelCoord2, out localPosition);
      Vector3 vector3_2 = localPosition;
      Vector3 min = vector3_1 - num;
      Vector3 max = vector3_2 + num;
      queryBox = new BoundingBox(min, max);
      BoundingSphere.CreateFromBoundingBox(ref queryBox, out querySphere);
    }

    public override void DebugDraw(ref MatrixD worldMatrix)
    {
      base.DebugDraw(ref worldMatrix);
      if (!MyDebugDrawSettings.DEBUG_DRAW_ASTEROID_SEEDS)
        return;
      MyRenderProxy.DebugDrawText3D(worldMatrix.Translation, "Size: " + (object) this.m_state.Size + Environment.NewLine + "Seed: " + (object) this.m_state.Seed + Environment.NewLine + "GeneratorSeed: " + (object) this.m_state.GeneratorSeed, Color.Red, 0.7f, false);
    }

    public static MyCompositeShapeProvider CreateAsteroidShape(
      int seed,
      float size,
      int generatorSeed = 0,
      int? generator = null)
    {
      MyCompositeShapeProvider.State state;
      state.Version = 3U;
      state.Generator = generator.GetValueOrDefault(MySession.Static.Settings.VoxelGeneratorVersion);
      state.Seed = seed;
      state.Size = size;
      state.UnusedCompat = 0U;
      state.GeneratorSeed = generatorSeed;
      MyCompositeShapeProvider compositeShapeProvider = new MyCompositeShapeProvider();
      compositeShapeProvider.InitFromState(state);
      return compositeShapeProvider;
    }

    public class MyCombinedCompositeInfoProvider : MyCompositeShapeProvider.MyProceduralCompositeInfoProvider, IMyCompositionInfoProvider
    {
      private readonly IMyCompositeShape[] m_filledShapes;
      private readonly IMyCompositeShape[] m_removedShapes;

      IMyCompositeShape[] IMyCompositionInfoProvider.FilledShapes => this.m_filledShapes;

      IMyCompositeShape[] IMyCompositionInfoProvider.RemovedShapes => this.m_removedShapes;

      public MyCombinedCompositeInfoProvider(
        ref MyCompositeShapeProvider.MyProceduralCompositeInfoProvider.ConstructionData data,
        IMyCompositeShape[] filledShapes,
        IMyCompositeShape[] removedShapes)
        : base(ref data)
      {
        this.m_filledShapes = ((IEnumerable<IMyCompositeShape>) base.m_filledShapes).Concat<IMyCompositeShape>((IEnumerable<IMyCompositeShape>) filledShapes).ToArray<IMyCompositeShape>();
        this.m_removedShapes = ((IEnumerable<IMyCompositeShape>) base.m_removedShapes).Concat<IMyCompositeShape>((IEnumerable<IMyCompositeShape>) removedShapes).ToArray<IMyCompositeShape>();
      }

      public new void UpdateMaterials(
        MyVoxelMaterialDefinition defaultMaterial,
        MyCompositeShapeOreDeposit[] deposits)
      {
        base.UpdateMaterials(defaultMaterial, deposits);
      }
    }

    private struct State
    {
      public uint Version;
      public int Generator;
      public int Seed;
      public float Size;
      public uint UnusedCompat;
      public int GeneratorSeed;
    }

    public class MyProceduralCompositeInfoProvider : IMyCompositionInfoProvider
    {
      public readonly IMyModule MacroModule;
      public readonly IMyModule DetailModule;
      protected MyCompositeShapeProvider.MyProceduralCompositeInfoProvider.ProceduralCompositeOreDeposit[] m_deposits;
      protected MyVoxelMaterialDefinition m_defaultMaterial;
      protected readonly MyCompositeShapeProvider.MyProceduralCompositeInfoProvider.ProceduralCompositeShape[] m_filledShapes;
      protected readonly MyCompositeShapeProvider.MyProceduralCompositeInfoProvider.ProceduralCompositeShape[] m_removedShapes;

      public MyProceduralCompositeInfoProvider(
        ref MyCompositeShapeProvider.MyProceduralCompositeInfoProvider.ConstructionData data)
      {
        this.MacroModule = data.MacroModule;
        this.DetailModule = data.DetailModule;
        this.m_defaultMaterial = data.DefaultMaterial;
        this.m_deposits = ((IEnumerable<MyCompositeShapeOreDeposit>) data.Deposits).Select<MyCompositeShapeOreDeposit, MyCompositeShapeProvider.MyProceduralCompositeInfoProvider.ProceduralCompositeOreDeposit>((Func<MyCompositeShapeOreDeposit, MyCompositeShapeProvider.MyProceduralCompositeInfoProvider.ProceduralCompositeOreDeposit>) (x => new MyCompositeShapeProvider.MyProceduralCompositeInfoProvider.ProceduralCompositeOreDeposit(this, x))).ToArray<MyCompositeShapeProvider.MyProceduralCompositeInfoProvider.ProceduralCompositeOreDeposit>();
        this.m_filledShapes = ((IEnumerable<MyCsgShapeBase>) data.FilledShapes).Select<MyCsgShapeBase, MyCompositeShapeProvider.MyProceduralCompositeInfoProvider.ProceduralCompositeShape>((Func<MyCsgShapeBase, MyCompositeShapeProvider.MyProceduralCompositeInfoProvider.ProceduralCompositeShape>) (x => new MyCompositeShapeProvider.MyProceduralCompositeInfoProvider.ProceduralCompositeShape(this, x))).ToArray<MyCompositeShapeProvider.MyProceduralCompositeInfoProvider.ProceduralCompositeShape>();
        this.m_removedShapes = ((IEnumerable<MyCsgShapeBase>) data.RemovedShapes).Select<MyCsgShapeBase, MyCompositeShapeProvider.MyProceduralCompositeInfoProvider.ProceduralCompositeShape>((Func<MyCsgShapeBase, MyCompositeShapeProvider.MyProceduralCompositeInfoProvider.ProceduralCompositeShape>) (x => new MyCompositeShapeProvider.MyProceduralCompositeInfoProvider.ProceduralCompositeShape(this, x))).ToArray<MyCompositeShapeProvider.MyProceduralCompositeInfoProvider.ProceduralCompositeShape>();
      }

      IMyCompositeDeposit[] IMyCompositionInfoProvider.Deposits => (IMyCompositeDeposit[]) this.m_deposits;

      IMyCompositeShape[] IMyCompositionInfoProvider.FilledShapes => (IMyCompositeShape[]) this.m_filledShapes;

      IMyCompositeShape[] IMyCompositionInfoProvider.RemovedShapes => (IMyCompositeShape[]) this.m_removedShapes;

      MyVoxelMaterialDefinition IMyCompositionInfoProvider.DefaultMaterial => this.m_defaultMaterial;

      void IMyCompositionInfoProvider.Close()
      {
      }

      protected void UpdateMaterials(
        MyVoxelMaterialDefinition defaultMaterial,
        MyCompositeShapeOreDeposit[] deposits)
      {
        this.m_defaultMaterial = defaultMaterial;
        this.m_deposits = ((IEnumerable<MyCompositeShapeOreDeposit>) deposits).Select<MyCompositeShapeOreDeposit, MyCompositeShapeProvider.MyProceduralCompositeInfoProvider.ProceduralCompositeOreDeposit>((Func<MyCompositeShapeOreDeposit, MyCompositeShapeProvider.MyProceduralCompositeInfoProvider.ProceduralCompositeOreDeposit>) (x => new MyCompositeShapeProvider.MyProceduralCompositeInfoProvider.ProceduralCompositeOreDeposit(this, x))).ToArray<MyCompositeShapeProvider.MyProceduralCompositeInfoProvider.ProceduralCompositeOreDeposit>();
      }

      public struct ConstructionData
      {
        public IMyModule MacroModule;
        public IMyModule DetailModule;
        public MyCsgShapeBase[] FilledShapes;
        public MyCsgShapeBase[] RemovedShapes;
        public MyCompositeShapeOreDeposit[] Deposits;
        public MyVoxelMaterialDefinition DefaultMaterial;
      }

      protected class ProceduralCompositeShape : IMyCompositeShape
      {
        private MyCsgShapeBase m_shape;
        private MyCompositeShapeProvider.MyProceduralCompositeInfoProvider m_context;

        public ProceduralCompositeShape(
          MyCompositeShapeProvider.MyProceduralCompositeInfoProvider context,
          MyCsgShapeBase shape)
        {
          this.m_shape = shape;
          this.m_context = context;
        }

        public ContainmentType Contains(
          ref BoundingBox queryBox,
          ref BoundingSphere querySphere,
          int lodVoxelSize)
        {
          return this.m_shape.Contains(ref queryBox, ref querySphere, (float) lodVoxelSize);
        }

        public float SignedDistance(ref Vector3 localPos, int lodVoxelSize) => this.m_shape.SignedDistance(ref localPos, (float) lodVoxelSize, this.m_context.MacroModule, this.m_context.DetailModule);

        public unsafe void ComputeContent(
          MyStorageData target,
          int lodIndex,
          Vector3I minInLod,
          Vector3I maxInLod,
          int lodVoxelSize)
        {
          Vector3I vector3I1 = minInLod;
          Vector3I vector3I2 = vector3I1 * lodVoxelSize;
          Vector3I vector3I3 = vector3I2;
          fixed (byte* numPtr1 = target[MyStorageDataTypeEnum.Content])
          {
            byte* numPtr2 = numPtr1;
            int sizeLinear = target.SizeLinear;
            for (vector3I1.Z = minInLod.Z; vector3I1.Z <= maxInLod.Z; ++vector3I1.Z)
            {
              for (vector3I1.Y = minInLod.Y; vector3I1.Y <= maxInLod.Y; ++vector3I1.Y)
              {
                for (vector3I1.X = minInLod.X; vector3I1.X <= maxInLod.X; ++vector3I1.X)
                {
                  Vector3 localPos = new Vector3(vector3I2);
                  float signedDistance = this.SignedDistance(ref localPos, lodVoxelSize);
                  *numPtr2 = MyCompositeShapeProviderBase.SignedDistanceToContent(signedDistance);
                  numPtr2 += target.StepLinear;
                  vector3I2.X += lodVoxelSize;
                }
                vector3I2.Y += lodVoxelSize;
                vector3I2.X = vector3I3.X;
              }
              vector3I2.Z += lodVoxelSize;
              vector3I2.Y = vector3I3.Y;
            }
          }
        }

        public void DebugDraw(ref MatrixD worldMatrix, Color color) => this.m_shape.DebugDraw(ref worldMatrix, color);

        public void Close()
        {
        }
      }

      protected class ProceduralCompositeOreDeposit : MyCompositeShapeProvider.MyProceduralCompositeInfoProvider.ProceduralCompositeShape, IMyCompositeDeposit, IMyCompositeShape
      {
        private readonly MyCompositeShapeOreDeposit m_deposit;

        public ProceduralCompositeOreDeposit(
          MyCompositeShapeProvider.MyProceduralCompositeInfoProvider context,
          MyCompositeShapeOreDeposit deposit)
          : base(context, deposit.Shape)
        {
          this.m_deposit = deposit;
        }

        public MyVoxelMaterialDefinition GetMaterialForPosition(
          ref Vector3 localPos,
          float lodVoxelSize)
        {
          return this.m_deposit.GetMaterialForPosition(ref localPos, lodVoxelSize);
        }

        public new void DebugDraw(ref MatrixD worldMatrix, Color color) => this.m_deposit.DebugDraw(ref worldMatrix, color);
      }
    }
  }
}
