// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyRenderComponentSafeZone
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Game.Models;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;
using VRageRender.Messages;

namespace Sandbox.Game.Components
{
  internal class MyRenderComponentSafeZone : MyRenderComponent
  {
    private MatrixD m_scaledRenderMatrix;
    private Vector3 m_hsvColor = new Color(0.1f, 0.63f, 0.95f).ColorToHSV();
    private uint m_tempRenderObjPtr = uint.MaxValue;

    private void UpdateRenderObjectMatrices(Matrix matrix)
    {
      for (int index = 0; index < this.m_renderObjectIDs.Length; ++index)
      {
        if (this.m_renderObjectIDs[index] != uint.MaxValue)
          MyRenderProxy.UpdateRenderObject(this.RenderObjectIDs[index], new MatrixD?((MatrixD) ref matrix), lastMomentUpdateIndex: this.LastMomentUpdateIndex);
      }
    }

    public void ChangeScale(Vector3 scale)
    {
      scale /= this.Model.BoundingBoxSizeHalf;
      MatrixD worldMatrix = this.Container.Entity.WorldMatrix;
      Matrix normalized = (Matrix) ref worldMatrix;
      MyUtils.Normalize(ref normalized, out normalized);
      Matrix matrix = Matrix.CreateScale(scale) * normalized;
      this.m_scaledRenderMatrix = (MatrixD) ref matrix;
    }

    public void SwitchModel(string modelName)
    {
      MyModel modelOnlyData = MyModels.GetModelOnlyData(modelName);
      if (modelOnlyData == null || this.ModelStorage == modelOnlyData)
        return;
      if (this.RenderObjectIDs[0] != uint.MaxValue)
        this.RemoveRenderObjects();
      this.ModelStorage = (object) modelOnlyData;
      if (this.RenderObjectIDs[0] != uint.MaxValue)
        return;
      this.AddRenderObjects();
    }

    public void ChangeColor(Color newColor) => this.m_hsvColor = newColor.ColorToHSV();

    public override void InvalidateRenderObjects()
    {
      if (!this.Container.Entity.Visible && !this.Container.Entity.CastShadows || (!this.Container.Entity.InScene || !this.Container.Entity.InvalidateOnMove))
        return;
      for (int index = 0; index < this.m_renderObjectIDs.Length; ++index)
      {
        if (this.RenderObjectIDs[index] != uint.MaxValue)
        {
          MyRenderProxy.UpdateRenderObject(this.RenderObjectIDs[index], new MatrixD?(this.m_scaledRenderMatrix), lastMomentUpdateIndex: this.LastMomentUpdateIndex);
          MyRenderProxy.UpdateRenderEntity(this.RenderObjectIDs[index], new Color?(), new Vector3?(this.m_hsvColor));
        }
      }
    }

    public void AddTransitionObject(Dictionary<string, MyTextureChange> texture)
    {
      if (this.m_tempRenderObjPtr == uint.MaxValue)
      {
        MatrixD scaledRenderMatrix = this.m_scaledRenderMatrix;
        this.m_tempRenderObjPtr = MyRenderProxy.CreateRenderEntity(this.Container.Entity.GetFriendlyName() + " " + this.Container.Entity.EntityId.ToString(), this.m_model.AssetName, scaledRenderMatrix, MyMeshDrawTechnique.MESH, this.GetRenderFlags(), this.GetRenderCullingOptions(), this.m_diffuseColor, this.m_colorMaskHsv, this.Transparency, depthBias: this.DepthBias, rescale: this.m_model.ScaleFactor);
      }
      MyRenderProxy.ChangeMaterialTexture(this.m_tempRenderObjPtr, texture);
    }

    public void RemoveTransitionObject()
    {
      MyRenderProxy.RemoveRenderObject(this.m_tempRenderObjPtr, MyRenderProxy.ObjectType.Invalid, this.FadeOut);
      this.m_tempRenderObjPtr = uint.MaxValue;
    }

    public void UpdateTransitionObjColor(Color color)
    {
      if (!this.Container.Entity.Visible && !this.Container.Entity.CastShadows || (!this.Container.Entity.InScene || !this.Container.Entity.InvalidateOnMove))
        return;
      MyRenderProxy.UpdateRenderEntity(this.m_tempRenderObjPtr, new Color?(), new Vector3?(color.ColorToHSV()));
    }

    public override void AddRenderObjects()
    {
      base.AddRenderObjects();
      this.m_scaledRenderMatrix = this.Container.Entity.WorldMatrix;
    }

    public override void RemoveRenderObjects()
    {
      base.RemoveRenderObjects();
      if (this.m_tempRenderObjPtr == uint.MaxValue)
        return;
      this.RemoveTransitionObject();
    }

    private class Sandbox_Game_Components_MyRenderComponentSafeZone\u003C\u003EActor : IActivator, IActivator<MyRenderComponentSafeZone>
    {
      object IActivator.CreateInstance() => (object) new MyRenderComponentSafeZone();

      MyRenderComponentSafeZone IActivator<MyRenderComponentSafeZone>.CreateInstance() => new MyRenderComponentSafeZone();
    }
  }
}
