// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyRenderComponentCompoundCubeBlock
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using VRage.Network;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
  internal class MyRenderComponentCompoundCubeBlock : MyRenderComponentCubeBlock
  {
    public override void InvalidateRenderObjects()
    {
      base.InvalidateRenderObjects();
      foreach (MySlimBlock block in (this.m_cubeBlock as MyCompoundCubeBlock).GetBlocks())
      {
        if (block.FatBlock != null && (block.FatBlock.Render.Visible || block.FatBlock.Render.CastShadows) && (block.FatBlock.InScene && block.FatBlock.InvalidateOnMove))
        {
          foreach (int renderObjectId in block.FatBlock.Render.RenderObjectIDs)
          {
            MatrixD worldMatrix = block.FatBlock.WorldMatrix;
            ref readonly MatrixD local1 = ref block.FatBlock.PositionComp.WorldMatrixRef;
            ref readonly BoundingBox local2 = ref BoundingBox.Invalid;
            MyRenderProxy.UpdateRenderObject((uint) renderObjectId, in local1, in local2, false);
          }
        }
      }
    }

    public override void AddRenderObjects() => this.InvalidateRenderObjects();

    private class Sandbox_Game_Components_MyRenderComponentCompoundCubeBlock\u003C\u003EActor : IActivator, IActivator<MyRenderComponentCompoundCubeBlock>
    {
      object IActivator.CreateInstance() => (object) new MyRenderComponentCompoundCubeBlock();

      MyRenderComponentCompoundCubeBlock IActivator<MyRenderComponentCompoundCubeBlock>.CreateInstance() => new MyRenderComponentCompoundCubeBlock();
    }
  }
}
