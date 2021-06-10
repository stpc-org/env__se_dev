// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyCompositeOrePlanetDeposit
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Voxels;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Library.Utils;
using VRage.Noise;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
  internal class MyCompositeOrePlanetDeposit : MyCompositeShapeOreDeposit
  {
    private float m_minDepth;
    private const float DEPOSIT_MAX_SIZE = 1000f;
    private int m_numDeposits;
    private Dictionary<Vector3I, MyCompositeShapeOreDeposit> m_deposits = new Dictionary<Vector3I, MyCompositeShapeOreDeposit>();
    private Dictionary<string, List<MyVoxelMaterialDefinition>> m_materialsByOreType = new Dictionary<string, List<MyVoxelMaterialDefinition>>();

    public float MinDepth => this.m_minDepth;

    public MyCompositeOrePlanetDeposit(
      MyCsgShapeBase baseShape,
      int seed,
      float minDepth,
      float maxDepth,
      MyOreProbability[] oreProbabilties,
      MyVoxelMaterialDefinition material)
      : base(baseShape, material)
    {
      this.m_minDepth = minDepth;
      double num1 = 12.5663719177246 * Math.Pow((double) minDepth, 3.0) / 3.0;
      double num2 = 12.5663719177246 * Math.Pow((double) maxDepth, 3.0) / 3.0;
      double num3 = 12.5663719177246 * Math.Pow(1000.0, 3.0) / 3.0;
      double num4 = num2;
      double num5 = num1 - num4;
      this.m_numDeposits = oreProbabilties.Length != 0 ? (int) Math.Floor(num5 * 0.400000005960464 / num3) : 0;
      double num6 = (double) minDepth / 1000.0;
      MyRandom instance = MyRandom.Instance;
      this.FillMaterialCollections();
      Vector3D vector3D1 = -new Vector3D(500.0);
      using (instance.PushSeed(seed))
      {
        for (int index = 0; index < this.m_numDeposits; ++index)
        {
          Vector3D vector3D2 = MyProceduralWorldGenerator.GetRandomDirection(instance) * (double) instance.NextFloat(maxDepth, minDepth);
          Vector3I key = Vector3I.Ceiling((Vector3) ((this.Shape.Center() + vector3D2) / 1000.0));
          MyCompositeShapeOreDeposit compositeShapeOreDeposit;
          if (!this.m_deposits.TryGetValue(key, out compositeShapeOreDeposit))
          {
            MyOreProbability ore = this.GetOre(instance.NextFloat(0.0f, 1f), oreProbabilties);
            MyVoxelMaterialDefinition material1 = this.m_materialsByOreType[ore.OreName][instance.Next() % this.m_materialsByOreType[ore.OreName].Count];
            compositeShapeOreDeposit = new MyCompositeShapeOreDeposit((MyCsgShapeBase) new MyCsgSimpleSphere((Vector3) (key * 1000f + vector3D1), instance.NextFloat(64f, 500f)), material1);
            this.m_deposits[key] = compositeShapeOreDeposit;
          }
        }
      }
      this.m_materialsByOreType.Clear();
    }

    public override MyVoxelMaterialDefinition GetMaterialForPosition(
      ref Vector3 pos,
      float lodSize)
    {
      MyCompositeShapeOreDeposit compositeShapeOreDeposit;
      return this.m_deposits.TryGetValue(Vector3I.Ceiling(pos / 1000f), out compositeShapeOreDeposit) && (double) compositeShapeOreDeposit.Shape.SignedDistance(ref pos, lodSize, (IMyModule) null, (IMyModule) null) == -1.0 ? compositeShapeOreDeposit.GetMaterialForPosition(ref pos, lodSize) : (MyVoxelMaterialDefinition) null;
    }

    private MyOreProbability GetOre(
      float probability,
      MyOreProbability[] probalities)
    {
      foreach (MyOreProbability probality in probalities)
      {
        if ((double) probality.CummulativeProbability >= (double) probability)
          return probality;
      }
      return (MyOreProbability) null;
    }

    private void FillMaterialCollections()
    {
      foreach (MyVoxelMaterialDefinition materialDefinition in MyDefinitionManager.Static.GetVoxelMaterialDefinitions())
      {
        if (materialDefinition.MinedOre != "Organic")
        {
          List<MyVoxelMaterialDefinition> materialDefinitionList;
          if (!this.m_materialsByOreType.TryGetValue(materialDefinition.MinedOre, out materialDefinitionList))
            materialDefinitionList = new List<MyVoxelMaterialDefinition>();
          materialDefinitionList.Add(materialDefinition);
          this.m_materialsByOreType[materialDefinition.MinedOre] = materialDefinitionList;
        }
      }
    }

    public override void DebugDraw(ref MatrixD translation, Color materialColor)
    {
      foreach (KeyValuePair<Vector3I, MyCompositeShapeOreDeposit> deposit in this.m_deposits)
        deposit.Value.DebugDraw(ref translation, materialColor);
    }
  }
}
