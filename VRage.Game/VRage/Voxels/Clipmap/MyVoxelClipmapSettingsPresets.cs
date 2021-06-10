// Decompiled with JetBrains decompiler
// Type: VRage.Voxels.Clipmap.MyVoxelClipmapSettingsPresets
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

namespace VRage.Voxels.Clipmap
{
  public class MyVoxelClipmapSettingsPresets
  {
    public static MyVoxelClipmapSettings[] NormalSettings = new MyVoxelClipmapSettings[4]
    {
      MyVoxelClipmapSettings.Create(4, 3, 2f, 4, 16384),
      MyVoxelClipmapSettings.Create(5, 3, 2f, 4, 16384),
      MyVoxelClipmapSettings.Create(5, 3, 3f),
      MyVoxelClipmapSettings.Create(5, 4000, 9f)
    };
    public static MyVoxelClipmapSettings[] PlanetSettings = new MyVoxelClipmapSettings[4]
    {
      MyVoxelClipmapSettings.Create(4, 2, 2f, minSize: 16),
      MyVoxelClipmapSettings.Create(5, 2, 2f, minSize: 16),
      MyVoxelClipmapSettings.Create(5, 3, 2f, minSize: 16),
      MyVoxelClipmapSettings.Create(5, 3, 3f, minSize: 16)
    };
  }
}
