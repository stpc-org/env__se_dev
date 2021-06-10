// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.Renders.MyRenderComponentCockpit
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using VRage.Game.Entity;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.EntityComponents.Renders
{
  public class MyRenderComponentCockpit : MyRenderComponentScreenAreas
  {
    protected MyCockpit m_cockpit;

    public MyRenderComponentCockpit(MyEntity entity)
      : base(entity)
    {
    }

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      this.m_cockpit = (MyCockpit) this.Container.Entity;
    }

    public override void AddRenderObjects()
    {
      if (this.m_model == null || this.m_cockpit == null || this.m_renderObjectIDs[0] != uint.MaxValue)
        return;
      if (!string.IsNullOrEmpty(this.m_cockpit.BlockDefinition.InteriorModel))
        this.ResizeRenderObjectArray(2);
      string friendlyName1 = this.Container.Entity.GetFriendlyName();
      long entityId = this.Container.Entity.EntityId;
      string str1 = entityId.ToString();
      this.SetRenderObjectID(0, MyRenderProxy.CreateRenderEntity(friendlyName1 + " " + str1, this.m_model.AssetName, this.Container.Entity.PositionComp.WorldMatrixRef, MyMeshDrawTechnique.MESH, this.GetRenderFlags(), this.GetRenderCullingOptions(), this.m_diffuseColor, this.m_colorMaskHsv, this.Transparency, depthBias: this.DepthBias, rescale: this.m_model.ScaleFactor, fadeIn: ((double) this.Transparency == 0.0 && this.FadeIn)));
      if (this.m_textureChanges != null)
        MyRenderProxy.ChangeMaterialTexture(this.m_renderObjectIDs[0], this.m_textureChanges);
      if (!string.IsNullOrEmpty(this.m_cockpit.BlockDefinition.InteriorModel))
      {
        string friendlyName2 = this.Container.Entity.GetFriendlyName();
        entityId = this.Container.Entity.EntityId;
        string str2 = entityId.ToString();
        this.SetRenderObjectID(1, MyRenderProxy.CreateRenderEntity(friendlyName2 + " " + str2 + "_interior", this.m_cockpit.BlockDefinition.InteriorModel, this.Container.Entity.PositionComp.WorldMatrixRef, MyMeshDrawTechnique.MESH, this.GetRenderFlags(), this.GetRenderCullingOptions(), this.m_diffuseColor, this.m_colorMaskHsv, this.Transparency, depthBias: this.DepthBias, rescale: this.m_model.ScaleFactor, fadeIn: this.FadeIn));
        MyRenderProxy.UpdateRenderObjectVisibility(this.m_renderObjectIDs[1], false, this.NearFlag);
        if (this.m_textureChanges != null)
          MyRenderProxy.ChangeMaterialTexture(this.m_renderObjectIDs[1], this.m_textureChanges);
      }
      this.m_cockpit.UpdateCockpitModel();
      this.UpdateGridParent();
      this.UpdateRenderAreas();
    }

    public uint ExteriorRenderId => this.m_renderObjectIDs[0];

    public uint InteriorRenderId => this.m_renderObjectIDs[1];

    private class Sandbox_Game_EntityComponents_Renders_MyRenderComponentCockpit\u003C\u003EActor
    {
    }
  }
}
