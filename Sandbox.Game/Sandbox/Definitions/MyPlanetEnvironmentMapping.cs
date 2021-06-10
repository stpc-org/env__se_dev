// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyPlanetEnvironmentMapping
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRage.Game;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
  public class MyPlanetEnvironmentMapping
  {
    public MyMaterialEnvironmentItem[] Items;
    public MyPlanetSurfaceRule Rule;
    private float[] CumulativeIntervals;
    public float TotalFrequency;

    public MyPlanetEnvironmentMapping(PlanetEnvironmentItemMapping map)
    {
      this.Rule = map.Rule;
      this.Items = new MyMaterialEnvironmentItem[map.Items.Length];
      if (this.Items.Length == 0)
      {
        this.CumulativeIntervals = (float[]) null;
        this.TotalFrequency = 0.0f;
      }
      else
      {
        this.TotalFrequency = 0.0f;
        for (int index = 0; index < map.Items.Length; ++index)
        {
          MyPlanetEnvironmentItemDef environmentItemDef = map.Items[index];
          MyObjectBuilderType result;
          if (environmentItemDef.TypeId != null && MyObjectBuilderType.TryParse(environmentItemDef.TypeId, out result))
          {
            if (!typeof (MyObjectBuilder_BotDefinition).IsAssignableFrom((Type) result) && !typeof (MyObjectBuilder_VoxelMapStorageDefinition).IsAssignableFrom((Type) result) && !typeof (MyObjectBuilder_EnvironmentItems).IsAssignableFrom((Type) result))
            {
              MyLog.Default.WriteLine(string.Format("Object builder type {0} is not supported for environment items.", (object) environmentItemDef.TypeId));
              this.Items[index].Frequency = 0.0f;
            }
            else
              this.Items[index] = new MyMaterialEnvironmentItem()
              {
                Definition = new MyDefinitionId(result, environmentItemDef.SubtypeId),
                Frequency = map.Items[index].Density,
                IsDetail = map.Items[index].IsDetail,
                IsBot = typeof (MyObjectBuilder_BotDefinition).IsAssignableFrom((Type) result),
                IsVoxel = typeof (MyObjectBuilder_VoxelMapStorageDefinition).IsAssignableFrom((Type) result),
                IsEnvironemntItem = typeof (MyObjectBuilder_EnvironmentItems).IsAssignableFrom((Type) result),
                BaseColor = map.Items[index].BaseColor,
                ColorSpread = map.Items[index].ColorSpread,
                MaxRoll = (float) Math.Cos((double) MathHelper.ToDegrees(map.Items[index].MaxRoll)),
                Offset = map.Items[index].Offset,
                GroupId = map.Items[index].GroupId,
                GroupIndex = map.Items[index].GroupIndex,
                ModifierId = map.Items[index].ModifierId,
                ModifierIndex = map.Items[index].ModifierIndex
              };
          }
          else
          {
            MyLog.Default.WriteLine(string.Format("Object builder type {0} does not exist.", (object) environmentItemDef.TypeId));
            this.Items[index].Frequency = 0.0f;
          }
        }
        this.ComputeDistribution();
      }
    }

    public void ComputeDistribution()
    {
      if (!this.Valid)
      {
        this.TotalFrequency = 0.0f;
        this.CumulativeIntervals = (float[]) null;
      }
      else
      {
        this.TotalFrequency = 0.0f;
        for (int index = 0; index < this.Items.Length; ++index)
          this.TotalFrequency += this.Items[index].Frequency;
        this.CumulativeIntervals = new float[this.Items.Length - 1];
        float num = 0.0f;
        for (int index = 0; index < this.CumulativeIntervals.Length; ++index)
        {
          this.CumulativeIntervals[index] = num + this.Items[index].Frequency / this.TotalFrequency;
          num = this.CumulativeIntervals[index];
        }
      }
    }

    public int GetItemRated(float rate) => this.CumulativeIntervals.BinaryIntervalSearch<float>(rate);

    public bool Valid => this.Items != null && (uint) this.Items.Length > 0U;
  }
}
