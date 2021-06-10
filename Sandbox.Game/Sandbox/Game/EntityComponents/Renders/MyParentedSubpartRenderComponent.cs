// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.Renders.MyParentedSubpartRenderComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Components;
using VRage.Game.Entity;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.EntityComponents.Renders
{
  public class MyParentedSubpartRenderComponent : MyRenderComponent
  {
    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      MyEntity entity = (MyEntity) this.Entity;
      entity.InvalidateOnMove = false;
      entity.NeedsWorldMatrix = false;
    }

    public override void AddRenderObjects()
    {
      base.AddRenderObjects();
      this.UpdateParent();
    }

    public void UpdateParent()
    {
      if (this.GetRenderObjectID() == uint.MaxValue)
        return;
      uint parentId = this.Entity.Parent.Render.ParentIDs[0];
      if (parentId == uint.MaxValue)
        return;
      Matrix relativeMatrix;
      this.GetCullObjectRelativeMatrix(out relativeMatrix);
      this.SetParent(0, parentId, new Matrix?(relativeMatrix));
      this.OnParented();
    }

    public void GetCullObjectRelativeMatrix(out Matrix relativeMatrix) => relativeMatrix = this.Entity.PositionComp.LocalMatrixRef * this.Entity.Parent.PositionComp.LocalMatrixRef;

    public virtual void OnParented()
    {
    }

    private class Sandbox_Game_EntityComponents_Renders_MyParentedSubpartRenderComponent\u003C\u003EActor : IActivator, IActivator<MyParentedSubpartRenderComponent>
    {
      object IActivator.CreateInstance() => (object) new MyParentedSubpartRenderComponent();

      MyParentedSubpartRenderComponent IActivator<MyParentedSubpartRenderComponent>.CreateInstance() => new MyParentedSubpartRenderComponent();
    }
  }
}
