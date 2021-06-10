// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyRenderComponentCubeBlock
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using System;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Components
{
  public class MyRenderComponentCubeBlock : MyRenderComponent
  {
    protected MyCubeBlock m_cubeBlock;

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      this.m_cubeBlock = this.Container.Entity as MyCubeBlock;
      this.NeedsDraw = false;
      this.NeedsDrawFromParent = false;
      this.NeedForDrawFromParentChanged = this.NeedForDrawFromParentChanged + new Action(this.OnNeedForDrawFromParentChanged);
    }

    public override void InvalidateRenderObjects() => this.m_cubeBlock.InvalidateOnMove = false;

    public override void AddRenderObjects()
    {
      this.CalculateBlockDepthBias(this.m_cubeBlock);
      base.AddRenderObjects();
      this.UpdateGridParent();
    }

    protected void UpdateGridParent()
    {
      if (!MyFakes.MANUAL_CULL_OBJECTS)
        return;
      MyCubeGridRenderCell orAddCell = this.m_cubeBlock.CubeGrid.RenderData.GetOrAddCell(this.m_cubeBlock.Position * this.m_cubeBlock.CubeGrid.GridSize);
      if (orAddCell.ParentCullObject == uint.MaxValue)
        orAddCell.RebuildInstanceParts(this.GetRenderFlags());
      for (int index = 0; index < this.m_renderObjectIDs.Length; ++index)
        this.SetParent(index, orAddCell.ParentCullObject, new Matrix?(this.Entity.PositionComp.LocalMatrixRef));
    }

    private void OnNeedForDrawFromParentChanged()
    {
      if (this.m_cubeBlock.SlimBlock == null || this.m_cubeBlock.CubeGrid == null || this.m_cubeBlock.CubeGrid.BlocksForDraw.Contains(this.m_cubeBlock) == this.NeedsDrawFromParent)
        return;
      if (this.NeedsDrawFromParent)
        this.m_cubeBlock.CubeGrid.BlocksForDraw.Add(this.m_cubeBlock);
      else
        this.m_cubeBlock.CubeGrid.BlocksForDraw.Remove(this.m_cubeBlock);
      this.m_cubeBlock.Render.SetVisibilityUpdates(this.NeedsDrawFromParent);
      this.m_cubeBlock.CubeGrid.MarkForDraw();
    }

    private class Sandbox_Game_Components_MyRenderComponentCubeBlock\u003C\u003EActor : IActivator, IActivator<MyRenderComponentCubeBlock>
    {
      object IActivator.CreateInstance() => (object) new MyRenderComponentCubeBlock();

      MyRenderComponentCubeBlock IActivator<MyRenderComponentCubeBlock>.CreateInstance() => new MyRenderComponentCubeBlock();
    }
  }
}
