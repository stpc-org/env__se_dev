// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.Renders.MyRenderComponentCubeBlockWithParentedSubpart
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Components;
using VRage.Game.Components;
using VRage.Network;

namespace Sandbox.Game.EntityComponents.Renders
{
  public class MyRenderComponentCubeBlockWithParentedSubpart : MyRenderComponentCubeBlock
  {
    public override void AddRenderObjects()
    {
      base.AddRenderObjects();
      this.UpdateChildren();
    }

    protected void UpdateChildren()
    {
      foreach (MyEntityComponentBase child in this.m_cubeBlock.Hierarchy.Children)
      {
        if (child.Entity.Render is MyParentedSubpartRenderComponent render)
          render.UpdateParent();
      }
    }

    private class Sandbox_Game_EntityComponents_Renders_MyRenderComponentCubeBlockWithParentedSubpart\u003C\u003EActor : IActivator, IActivator<MyRenderComponentCubeBlockWithParentedSubpart>
    {
      object IActivator.CreateInstance() => (object) new MyRenderComponentCubeBlockWithParentedSubpart();

      MyRenderComponentCubeBlockWithParentedSubpart IActivator<MyRenderComponentCubeBlockWithParentedSubpart>.CreateInstance() => new MyRenderComponentCubeBlockWithParentedSubpart();
    }
  }
}
