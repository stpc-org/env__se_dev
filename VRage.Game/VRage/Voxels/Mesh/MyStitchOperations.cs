// Decompiled with JetBrains decompiler
// Type: VRage.Voxels.Mesh.MyStitchOperations
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Voxels.Sewing;

namespace VRage.Voxels.Mesh
{
  public static class MyStitchOperations
  {
    public static VrSewOperation GetInstruction(
      bool x = false,
      bool y = false,
      bool z = false,
      bool xy = false,
      bool xz = false,
      bool yz = false,
      bool xyz = false)
    {
      return (VrSewOperation) (0 | (!x ? 0 : 2) | (!y ? 0 : 4) | (!z ? 0 : 6) | (!xy ? 0 : 8) | (!xz ? 0 : 10) | (!yz ? 0 : 12) | (!xyz ? 0 : 14));
    }

    public static bool Contains(this VrSewOperation self, VrSewOperation flags) => (self & flags) == flags;

    public static VrSewOperation Without(
      this VrSewOperation self,
      VrSewOperation flags)
    {
      return self & ~flags;
    }

    public static VrSewOperation With(this VrSewOperation self, VrSewOperation flags) => self | flags;
  }
}
