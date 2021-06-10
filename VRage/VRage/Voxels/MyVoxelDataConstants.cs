// Decompiled with JetBrains decompiler
// Type: VRage.Voxels.MyVoxelDataConstants
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.Voxels
{
  public static class MyVoxelDataConstants
  {
    public const string StorageV2Extension = ".vx2";
    public const byte IsoLevel = 127;
    public const byte ContentEmpty = 0;
    public const byte ContentFull = 255;
    public const float HalfContent = 127.5f;
    public const float HalfContentReciprocal = 0.007843138f;
    public const float ContentReciprocal = 0.003921569f;
    public const byte NullMaterial = 255;
    private static readonly byte[] Defaults = new byte[2]
    {
      (byte) 0,
      byte.MaxValue
    };
    public const int LodCount = 16;

    public static byte DefaultValue(MyStorageDataTypeEnum type) => MyVoxelDataConstants.Defaults[(int) type];
  }
}
