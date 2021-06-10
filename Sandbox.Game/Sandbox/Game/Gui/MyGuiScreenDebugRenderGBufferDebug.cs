// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugRenderGBufferDebug
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
  [MyDebugScreen("Render", "GBuffer Debug")]
  internal class MyGuiScreenDebugRenderGBufferDebug : MyGuiScreenDebugBase
  {
    private List<MyGuiControlCheckbox> m_cbs = new List<MyGuiControlCheckbox>();
    private bool m_radioUpdate;

    public MyGuiScreenDebugRenderGBufferDebug()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.m_sliderDebugScale = 0.7f;
      this.AddCaption("GBuffer Debug", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.m_currentPosition.Y += 0.01f;
      this.AddLabel("Gbuffer", Color.Yellow.ToVector4(), 1.2f);
      this.m_cbs.Clear();
      this.m_cbs.Add(this.AddCheckBox("Base color", MyRenderProxy.Settings.DisplayGbufferColor, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayGbufferColor = x.IsChecked)));
      this.m_cbs.Add(this.AddCheckBox("Albedo", MyRenderProxy.Settings.DisplayGbufferAlbedo, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayGbufferAlbedo = x.IsChecked)));
      this.m_cbs.Add(this.AddCheckBox("Normals", MyRenderProxy.Settings.DisplayGbufferNormal, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayGbufferNormal = x.IsChecked)));
      this.m_cbs.Add(this.AddCheckBox("Normals view", MyRenderProxy.Settings.DisplayGbufferNormalView, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayGbufferNormalView = x.IsChecked)));
      this.m_cbs.Add(this.AddCheckBox("Glossiness", MyRenderProxy.Settings.DisplayGbufferGlossiness, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayGbufferGlossiness = x.IsChecked)));
      this.m_cbs.Add(this.AddCheckBox("Metalness", MyRenderProxy.Settings.DisplayGbufferMetalness, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayGbufferMetalness = x.IsChecked)));
      this.m_cbs.Add(this.AddCheckBox("NDotL", MyRenderProxy.Settings.DisplayNDotL, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayNDotL = x.IsChecked)));
      this.m_cbs.Add(this.AddCheckBox("LOD", MyRenderProxy.Settings.DisplayGbufferLOD, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayGbufferLOD = x.IsChecked)));
      this.m_cbs.Add(this.AddCheckBox("Mipmap", MyRenderProxy.Settings.DisplayMipmap, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayMipmap = x.IsChecked)));
      this.m_cbs.Add(this.AddCheckBox("Ambient occlusion", MyRenderProxy.Settings.DisplayGbufferAO, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayGbufferAO = x.IsChecked)));
      this.m_cbs.Add(this.AddCheckBox("Emissive", MyRenderProxy.Settings.DisplayEmissive, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayEmissive = x.IsChecked)));
      this.m_cbs.Add(this.AddCheckBox("Edge mask", MyRenderProxy.Settings.DisplayEdgeMask, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayEdgeMask = x.IsChecked)));
      this.m_cbs.Add(this.AddCheckBox("Depth", MyRenderProxy.Settings.DisplayDepth, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayDepth = x.IsChecked)));
      this.m_cbs.Add(this.AddCheckBox("Stencil", MyRenderProxy.Settings.DisplayStencil, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayStencil = x.IsChecked)));
      this.m_currentPosition.Y += 0.01f;
      this.m_cbs.Add(this.AddCheckBox("Reprojection test", MyRenderProxy.Settings.DisplayReprojectedDepth, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayReprojectedDepth = x.IsChecked)));
      this.m_currentPosition.Y += 0.01f;
      this.AddLabel("Environment light", Color.Yellow.ToVector4(), 1.2f);
      this.m_cbs.Add(this.AddCheckBox("Ambient diffuse", MyRenderProxy.Settings.DisplayAmbientDiffuse, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayAmbientDiffuse = x.IsChecked)));
      this.m_cbs.Add(this.AddCheckBox("Ambient specular", MyRenderProxy.Settings.DisplayAmbientSpecular, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayAmbientSpecular = x.IsChecked)));
      this.m_currentPosition.Y += 0.01f;
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugRenderGBufferDebug);

    protected override void ValueChanged(MyGuiControlBase sender)
    {
      MyRenderProxy.SetSettingsDirty();
      if (this.m_radioUpdate)
        return;
      this.m_radioUpdate = true;
      foreach (MyGuiControlCheckbox cb in this.m_cbs)
      {
        if (cb != sender)
          cb.IsChecked = false;
      }
      this.m_radioUpdate = false;
    }
  }
}
