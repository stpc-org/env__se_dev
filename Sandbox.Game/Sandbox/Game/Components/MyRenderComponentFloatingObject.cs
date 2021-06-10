// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyRenderComponentFloatingObject
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using VRage.Network;
using VRage.Utils;
using VRageRender;

namespace Sandbox.Game.Components
{
  internal class MyRenderComponentFloatingObject : MyRenderComponent
  {
    private MyFloatingObject m_floatingObject;

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      this.m_floatingObject = this.Container.Entity as MyFloatingObject;
    }

    public override void AddRenderObjects()
    {
      if (this.m_floatingObject.VoxelMaterial == null)
      {
        base.AddRenderObjects();
      }
      else
      {
        if (this.m_renderObjectIDs[0] != uint.MaxValue)
          return;
        this.SetRenderObjectID(0, MyRenderProxy.CreateRenderVoxelDebris("Voxel debris", this.Model.AssetName, this.Container.Entity.PositionComp.WorldMatrixRef, 5f, 8f, MyUtils.GetRandomFloat(0.0f, 2f), this.m_floatingObject.VoxelMaterial.Index, this.FadeIn));
      }
    }

    private class Sandbox_Game_Components_MyRenderComponentFloatingObject\u003C\u003EActor : IActivator, IActivator<MyRenderComponentFloatingObject>
    {
      object IActivator.CreateInstance() => (object) new MyRenderComponentFloatingObject();

      MyRenderComponentFloatingObject IActivator<MyRenderComponentFloatingObject>.CreateInstance() => new MyRenderComponentFloatingObject();
    }
  }
}
