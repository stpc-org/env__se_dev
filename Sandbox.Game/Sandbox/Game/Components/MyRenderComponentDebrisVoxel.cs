// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyRenderComponentDebrisVoxel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Network;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
  internal class MyRenderComponentDebrisVoxel : MyRenderComponent
  {
    public float TexCoordOffset { get; set; }

    public float TexCoordScale { get; set; }

    public byte VoxelMaterialIndex { get; set; }

    public override void AddRenderObjects()
    {
      if (this.m_renderObjectIDs[0] != uint.MaxValue)
        return;
      string assetName = this.Model.AssetName;
      Matrix matrix = (Matrix) ref this.Container.Entity.PositionComp.WorldMatrixRef;
      MatrixD worldMatrix = (MatrixD) ref matrix;
      double texCoordOffset = (double) this.TexCoordOffset;
      double texCoordScale = (double) this.TexCoordScale;
      int voxelMaterialIndex = (int) this.VoxelMaterialIndex;
      int num = this.FadeIn ? 1 : 0;
      this.SetRenderObjectID(0, MyRenderProxy.CreateRenderVoxelDebris("Voxel debris", assetName, worldMatrix, (float) texCoordOffset, (float) texCoordScale, 1f, (byte) voxelMaterialIndex, num != 0));
    }

    private class Sandbox_Game_Components_MyRenderComponentDebrisVoxel\u003C\u003EActor : IActivator, IActivator<MyRenderComponentDebrisVoxel>
    {
      object IActivator.CreateInstance() => (object) new MyRenderComponentDebrisVoxel();

      MyRenderComponentDebrisVoxel IActivator<MyRenderComponentDebrisVoxel>.CreateInstance() => new MyRenderComponentDebrisVoxel();
    }
  }
}
