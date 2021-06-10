// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyRenderComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using VRage.Game.Components;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Network;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.Components
{
  public class MyRenderComponent : MyRenderComponentBase
  {
    protected MyModel m_model;

    public MyRenderComponent()
    {
      this.m_parentIDs = (uint[]) this.m_parentIDs.Clone();
      this.m_renderObjectIDs = (uint[]) this.m_renderObjectIDs.Clone();
    }

    public override void AddRenderObjects()
    {
      if (this.m_model == null || this.m_renderObjectIDs[0] != uint.MaxValue)
        return;
      this.SetRenderObjectID(0, MyRenderProxy.CreateRenderEntity(this.Container.Entity.GetFriendlyName() + " " + this.Container.Entity.EntityId.ToString(), this.m_model.AssetName, this.Container.Entity.PositionComp.WorldMatrixRef, MyMeshDrawTechnique.MESH, this.GetRenderFlags(), this.GetRenderCullingOptions(), this.m_diffuseColor, this.m_colorMaskHsv, this.Transparency, depthBias: this.DepthBias, rescale: this.m_model.ScaleFactor, fadeIn: ((double) this.Transparency == 0.0 && this.FadeIn)));
      if (this.m_textureChanges == null)
        return;
      MyRenderProxy.ChangeMaterialTexture(this.m_renderObjectIDs[0], this.m_textureChanges);
    }

    public MyModel Model
    {
      get => this.m_model;
      set => this.m_model = value;
    }

    public override object ModelStorage
    {
      get => (object) this.Model;
      set => this.Model = (MyModel) value;
    }

    public override void SetRenderObjectID(int index, uint ID)
    {
      this.m_renderObjectIDs[index] = ID;
      MyEntities.AddRenderObjectToMap(ID, this.Container.Entity);
      this.PropagateVisibilityUpdates();
    }

    public override void ReleaseRenderObjectID(int index)
    {
      if (this.m_renderObjectIDs[index] == uint.MaxValue)
        return;
      MyEntities.RemoveRenderObjectFromMap(this.m_renderObjectIDs[index]);
      MyRenderProxy.RemoveRenderObject(this.m_renderObjectIDs[index], MyRenderProxy.ObjectType.Invalid, this.FadeOut);
      this.m_renderObjectIDs[index] = uint.MaxValue;
      this.m_parentIDs[index] = uint.MaxValue;
    }

    public override void Draw()
    {
    }

    public override bool IsVisible() => MyEntities.IsVisible(this.Container.Entity) && this.Visible && this.Container.Entity.InScene;

    public override bool NeedsDraw
    {
      get => (uint) (this.Container.Entity.Flags & EntityFlags.NeedsDraw) > 0U;
      set
      {
        if (value == this.NeedsDraw)
          return;
        this.Container.Entity.Flags &= ~EntityFlags.NeedsDraw;
        if (value)
          this.Container.Entity.Flags |= EntityFlags.NeedsDraw;
        if (!this.Container.Entity.InScene)
          return;
        if (value)
          MyEntities.RegisterForDraw(this.Container.Entity);
        else
          MyEntities.UnregisterForDraw(this.Container.Entity);
      }
    }

    private class Sandbox_Game_Components_MyRenderComponent\u003C\u003EActor : IActivator, IActivator<MyRenderComponent>
    {
      object IActivator.CreateInstance() => (object) new MyRenderComponent();

      MyRenderComponent IActivator<MyRenderComponent>.CreateInstance() => new MyRenderComponent();
    }
  }
}
