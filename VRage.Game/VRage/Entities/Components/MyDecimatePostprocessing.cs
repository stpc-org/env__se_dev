// Decompiled with JetBrains decompiler
// Type: VRage.Entities.Components.MyDecimatePostprocessing
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using VRage.ObjectBuilders.Definitions.Components;
using VRage.ObjectBuilders.Voxels;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;

namespace VRage.Entities.Components
{
  [VoxelPostprocessing(typeof (MyObjectBuilder_VoxelPostprocessingDecimate), true)]
  public class MyDecimatePostprocessing : MyVoxelPostprocessing
  {
    [ThreadStatic]
    private static VrDecimatePostprocessing m_instance;
    private List<MyDecimatePostprocessing.Settings> m_perLodSettings = new List<MyDecimatePostprocessing.Settings>();

    protected internal override void Init(MyObjectBuilder_VoxelPostprocessing builder)
    {
      base.Init(builder);
      MyObjectBuilder_VoxelPostprocessingDecimate postprocessingDecimate = (MyObjectBuilder_VoxelPostprocessingDecimate) builder;
      int num = -1;
      foreach (MyObjectBuilder_VoxelPostprocessingDecimate.Settings lodSetting in postprocessingDecimate.LodSettings)
      {
        if (lodSetting.FromLod <= num)
          MyLog.Default.Error("Decimation lod sets must have strictly ascending lod indices.");
        else
          this.m_perLodSettings.Add(new MyDecimatePostprocessing.Settings(lodSetting));
      }
    }

    public override bool Get(int lod, out VrPostprocessing postprocess)
    {
      if (MyDecimatePostprocessing.m_instance == null)
        MyDecimatePostprocessing.m_instance = new VrDecimatePostprocessing();
      int index = this.m_perLodSettings.BinaryIntervalSearch<MyDecimatePostprocessing.Settings>((Func<MyDecimatePostprocessing.Settings, bool>) (x => x.FromLod <= lod)) - 1;
      if (index == -1)
      {
        postprocess = (VrPostprocessing) null;
        return false;
      }
      MyDecimatePostprocessing.Settings perLodSetting = this.m_perLodSettings[index];
      MyDecimatePostprocessing.m_instance.FeatureAngle = perLodSetting.FeatureAngle;
      MyDecimatePostprocessing.m_instance.EdgeThreshold = perLodSetting.EdgeThreshold;
      MyDecimatePostprocessing.m_instance.PlaneThreshold = perLodSetting.PlaneThreshold;
      MyDecimatePostprocessing.m_instance.IgnoreEdges = perLodSetting.IgnoreEdges;
      postprocess = (VrPostprocessing) MyDecimatePostprocessing.m_instance;
      return true;
    }

    public struct Settings
    {
      public int FromLod;
      public float FeatureAngle;
      public float EdgeThreshold;
      public float PlaneThreshold;
      public bool IgnoreEdges;

      public Settings(
        MyObjectBuilder_VoxelPostprocessingDecimate.Settings obSettings)
      {
        this.FromLod = obSettings.FromLod;
        this.FeatureAngle = MathHelper.ToRadians(obSettings.FeatureAngle);
        this.EdgeThreshold = obSettings.EdgeThreshold;
        this.PlaneThreshold = obSettings.PlaneThreshold;
        this.IgnoreEdges = obSettings.IgnoreEdges;
      }
    }
  }
}
