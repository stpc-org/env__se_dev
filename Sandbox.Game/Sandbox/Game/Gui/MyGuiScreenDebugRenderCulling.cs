// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugRenderCulling
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics.GUI;
using System;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
  [MyDebugScreen("Render", "Culling")]
  internal class MyGuiScreenDebugRenderCulling : MyGuiScreenDebugBase
  {
    public MyGuiScreenDebugRenderCulling()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.AddCaption("Culling", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.AddSlider("CullGroupsThreshold", 0.0f, 1000f, (Func<float>) (() => (float) MyRenderProxy.Settings.CullGroupsThreshold), (Action<float>) (f => MyRenderProxy.Settings.CullGroupsThreshold = (int) f));
      this.AddSlider("CullTreeFallbackThreshold", 0.0f, 1f, (Func<float>) (() => MyRenderProxy.Settings.IncrementalCullingTreeFallbackThreshold), (Action<float>) (x => MyRenderProxy.Settings.IncrementalCullingTreeFallbackThreshold = x));
      this.AddCheckBox("UseIncrementalCulling", (Func<bool>) (() => MyRenderProxy.Settings.UseIncrementalCulling), (Action<bool>) (x => MyRenderProxy.Settings.UseIncrementalCulling = x));
      this.AddSlider("IncrementalCullFrames", 1f, 100f, (Func<float>) (() => (float) MyRenderProxy.Settings.IncrementalCullFrames), (Action<float>) (x => MyRenderProxy.Settings.IncrementalCullFrames = (int) x));
      this.m_currentPosition.Y += 0.01f;
      this.AddLabel("Occlusion", Color.Yellow.ToVector4(), 1.2f);
      this.AddCheckBox("Skip occlusion queries", MyRenderProxy.Settings.IgnoreOcclusionQueries, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.IgnoreOcclusionQueries = x.IsChecked));
      this.AddCheckBox("Disable occlusion queries", MyRenderProxy.Settings.DisableOcclusionQueries, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisableOcclusionQueries = x.IsChecked));
      this.AddCheckBox("Draw occlusion queries debug", MyRenderProxy.Settings.DrawOcclusionQueriesDebug, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DrawOcclusionQueriesDebug = x.IsChecked));
      this.AddCheckBox("Draw group occlusion queries debug", MyRenderProxy.Settings.DrawGroupOcclusionQueriesDebug, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DrawGroupOcclusionQueriesDebug = x.IsChecked));
    }

    protected override void ValueChanged(MyGuiControlBase sender) => MyRenderProxy.SetSettingsDirty();

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugRenderCulling);
  }
}
