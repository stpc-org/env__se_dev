// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugRenderOutdoor
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics.GUI;
using System;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
  [MyDebugScreen("Render", "Outdoor")]
  internal class MyGuiScreenDebugRenderOutdoor : MyGuiScreenDebugBase
  {
    public MyGuiScreenDebugRenderOutdoor()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.m_sliderDebugScale = 0.7f;
      this.AddCaption("Outdoor", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.AddCheckBox("Freeze terrain queries", MyRenderProxy.Settings.FreezeTerrainQueries, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.FreezeTerrainQueries = x.IsChecked));
      this.m_currentPosition.Y += 0.01f;
      this.AddLabel("Grass", Color.Yellow.ToVector4(), 1.2f);
      this.AddSlider("Grass maximum draw distance", MyRenderProxy.Settings.User.GrassDrawDistance, 0.0f, 1000f, (Action<MyGuiControlSlider>) (x => MyRenderProxy.Settings.User.GrassDrawDistance = x.Value));
      this.AddSlider("Scaling near distance", MyRenderProxy.Settings.GrassGeometryScalingNearDistance, 0.0f, 1000f, (Action<MyGuiControlSlider>) (x => MyRenderProxy.Settings.GrassGeometryScalingNearDistance = x.Value));
      this.AddSlider("Scaling far distance", MyRenderProxy.Settings.GrassGeometryScalingFarDistance, 0.0f, 1000f, (Action<MyGuiControlSlider>) (x => MyRenderProxy.Settings.GrassGeometryScalingFarDistance = x.Value));
      this.AddSlider("Scaling factor", MyRenderProxy.Settings.GrassGeometryDistanceScalingFactor, 0.0f, 10f, (Action<MyGuiControlSlider>) (x => MyRenderProxy.Settings.GrassGeometryDistanceScalingFactor = x.Value));
      this.m_currentPosition.Y += 0.01f;
      this.AddLabel("Wind", Color.Yellow.ToVector4(), 1.2f);
      this.AddSlider("Strength", MyRenderProxy.Settings.WindStrength, 0.0f, 10f, (Action<MyGuiControlSlider>) (x => MyRenderProxy.Settings.WindStrength = x.Value));
      this.AddSlider("Azimuth", MyRenderProxy.Settings.WindAzimuth, 0.0f, 360f, (Action<MyGuiControlSlider>) (x => MyRenderProxy.Settings.WindAzimuth = x.Value));
      this.m_currentPosition.Y += 0.01f;
      this.AddLabel("Lights", Color.Yellow.ToVector4(), 1.2f);
    }

    protected override void ValueChanged(MyGuiControlBase sender) => MyRenderProxy.SetSettingsDirty();

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugRenderOutdoor);
  }
}
