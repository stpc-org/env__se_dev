// Decompiled with JetBrains decompiler
// Type: VRage.Voxels.DualContouring.MyMesherResult
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

namespace VRage.Voxels.DualContouring
{
  public struct MyMesherResult
  {
    public readonly MyVoxelContentConstitution Constitution;
    public readonly VrVoxelMesh Mesh;
    public static MyMesherResult Empty = new MyMesherResult(MyVoxelContentConstitution.Empty);

    public bool MeshProduced => this.Mesh != null;

    internal MyMesherResult(VrVoxelMesh mesh)
    {
      this.Constitution = MyVoxelContentConstitution.Mixed;
      this.Mesh = mesh;
    }

    internal MyMesherResult(MyVoxelContentConstitution constitution)
    {
      this.Constitution = constitution;
      this.Mesh = (VrVoxelMesh) null;
    }
  }
}
