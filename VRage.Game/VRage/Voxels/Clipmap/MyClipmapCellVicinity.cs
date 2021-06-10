// Decompiled with JetBrains decompiler
// Type: VRage.Voxels.Clipmap.MyClipmapCellVicinity
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Voxels.Sewing;

namespace VRage.Voxels.Clipmap
{
  public struct MyClipmapCellVicinity
  {
    public readonly uint Lods;
    public unsafe fixed sbyte Versions[8];
    public static readonly MyClipmapCellVicinity Invalid = new MyClipmapCellVicinity(false);

    public unsafe MyClipmapCellVicinity(VrSewGuide[] guides, MyCellCoord[] coords)
    {
      this.Lods = 0U;
      fixed (MyClipmapCellVicinity* clipmapCellVicinityPtr = &this)
      {
        for (int index = 0; index < 8; ++index)
        {
          int lod = MyVoxelClipmap.MakeFulfilled(coords[index]).Lod;
          if (guides[index] == null)
          {
            clipmapCellVicinityPtr->Versions[index] = (sbyte) -1;
          }
          else
          {
            this.Lods |= (uint) (lod << index * 4);
            clipmapCellVicinityPtr->Versions[index] = (sbyte) (guides[index].Version & (int) sbyte.MaxValue);
          }
        }
      }
    }

    private unsafe MyClipmapCellVicinity(bool dummySelector)
    {
      this.Lods = 0U;
      fixed (MyClipmapCellVicinity* clipmapCellVicinityPtr = &this)
      {
        for (int index = 0; index < 8; ++index)
          clipmapCellVicinityPtr->Versions[index] = (sbyte) -1;
      }
    }

    public unsafe bool Equals(MyClipmapCellVicinity other)
    {
      fixed (MyClipmapCellVicinity* clipmapCellVicinityPtr = &this)
        return (int) this.Lods == (int) other.Lods && *(long*) clipmapCellVicinityPtr->Versions == *(long*) other.Versions;
    }

    public override bool Equals(object obj) => obj != null && obj is MyClipmapCellVicinity other && this.Equals(other);

    public override unsafe int GetHashCode()
    {
      fixed (MyClipmapCellVicinity* clipmapCellVicinityPtr = &this)
        return (int) this.Lods * 397 ^ (int) clipmapCellVicinityPtr->Versions;
    }

    public static bool operator ==(MyClipmapCellVicinity left, MyClipmapCellVicinity right) => left.Equals(right);

    public static bool operator !=(MyClipmapCellVicinity left, MyClipmapCellVicinity right) => !(left == right);
  }
}
