// Decompiled with JetBrains decompiler
// Type: VRage.Voxels.MyVoxelEnumExtensions
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.Voxels
{
  public static class MyVoxelEnumExtensions
  {
    public static bool Requests(this MyStorageDataTypeFlags self, MyStorageDataTypeEnum value) => (uint) (self & (MyStorageDataTypeFlags) (1 << (int) (value & (MyStorageDataTypeEnum) 31))) > 0U;

    public static MyStorageDataTypeFlags Without(
      this MyStorageDataTypeFlags self,
      MyStorageDataTypeEnum value)
    {
      return self & (MyStorageDataTypeFlags) ~(byte) (1U << (int) (value & (MyStorageDataTypeEnum) 31)) & MyStorageDataTypeFlags.ContentAndMaterial;
    }

    public static MyStorageDataTypeFlags ToFlags(
      this MyStorageDataTypeEnum self)
    {
      return (MyStorageDataTypeFlags) (1U << (int) (self & (MyStorageDataTypeEnum) 31));
    }
  }
}
