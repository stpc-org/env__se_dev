// Decompiled with JetBrains decompiler
// Type: VRage.Voxels.MyVoxelDataRequest
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRageMath;

namespace VRage.Voxels
{
  public struct MyVoxelDataRequest
  {
    public int Lod;
    public Vector3I MinInLod;
    public Vector3I MaxInLod;
    public Vector3I Offset;
    public MyStorageDataTypeFlags RequestedData;
    public MyVoxelRequestFlags RequestFlags;
    public MyVoxelRequestFlags Flags;
    public MyStorageData Target;

    public string ToStringShort() => string.Format("lod{0}: {1}voxels", (object) this.Lod, (object) this.SizeLinear);

    public int SizeLinear => (this.MaxInLod - this.MinInLod + Vector3I.One).Size;
  }
}
