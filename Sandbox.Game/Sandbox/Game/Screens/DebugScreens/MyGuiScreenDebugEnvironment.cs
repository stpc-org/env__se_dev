// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.DebugScreens.MyGuiScreenDebugEnvironment
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Planet;
using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using System;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
  [MyDebugScreen("Game", "Environment")]
  public class MyGuiScreenDebugEnvironment : MyGuiScreenDebugBase
  {
    public static Action DeleteEnvironmentItems;

    public MyGuiScreenDebugEnvironment()
      : base()
      => this.RecreateControls(true);

    public override string GetFriendlyName() => "MyGuiScreenDebugRenderEnvironment";

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.13f);
      this.AddShareFocusHint();
      this.Spacing = 0.01f;
      this.AddCaption("World Environment", new Vector4?(Color.Yellow.ToVector4()));
      this.AddCaption("Debug Tools:", new Vector4?(Color.Yellow.ToVector4()));
      this.AddCheckBox("Update Environment Sectors", (Func<bool>) (() => MyPlanetEnvironmentSessionComponent.EnableUpdate), (Action<bool>) (x => MyPlanetEnvironmentSessionComponent.EnableUpdate = x));
      this.AddButton("Refresh Sectors", (Action<MyGuiControlButton>) (x => this.RefreshSectors()));
      this.AddLabel("Debug Draw Options:", (Vector4) Color.White, 1f);
      this.AddCheckBox("Debug Draw Sectors", (Func<bool>) (() => MyPlanetEnvironmentSessionComponent.DebugDrawSectors), (Action<bool>) (x => MyPlanetEnvironmentSessionComponent.DebugDrawSectors = x));
      this.AddCheckBox("Debug Draw Clipmap Proxies", (Func<bool>) (() => MyPlanetEnvironmentSessionComponent.DebugDrawProxies), (Action<bool>) (x => MyPlanetEnvironmentSessionComponent.DebugDrawProxies = x));
      this.AddCheckBox("Debug Draw Dynamic Clusters", (Func<bool>) (() => MyPlanetEnvironmentSessionComponent.DebugDrawDynamicObjectClusters), (Action<bool>) (x => MyPlanetEnvironmentSessionComponent.DebugDrawDynamicObjectClusters = x));
      this.AddCheckBox("Debug Draw Collision Boxes", (Func<bool>) (() => MyPlanetEnvironmentSessionComponent.DebugDrawCollisionCheckers), (Action<bool>) (x => MyPlanetEnvironmentSessionComponent.DebugDrawCollisionCheckers = x));
      this.AddCheckBox("Debug Draw Providers", (Func<bool>) (() => MyPlanetEnvironmentSessionComponent.DebugDrawEnvironmentProviders), (Action<bool>) (x => MyPlanetEnvironmentSessionComponent.DebugDrawEnvironmentProviders = x));
      this.AddCheckBox("Debug Draw Active Sector Items", (Func<bool>) (() => MyPlanetEnvironmentSessionComponent.DebugDrawActiveSectorItems), (Action<bool>) (x => MyPlanetEnvironmentSessionComponent.DebugDrawActiveSectorItems = x));
      this.AddCheckBox("Debug Draw Active Sector Provider", (Func<bool>) (() => MyPlanetEnvironmentSessionComponent.DebugDrawActiveSectorProvider), (Action<bool>) (x => MyPlanetEnvironmentSessionComponent.DebugDrawActiveSectorProvider = x));
      this.AddSlider("Sector Name Draw Distance:", (MyGuiSliderProperties) new MyGuiSliderPropertiesExponential(1f, 1000f), (Func<float>) (() => MyPlanetEnvironmentSessionComponent.DebugDrawDistance), (Action<float>) (x => MyPlanetEnvironmentSessionComponent.DebugDrawDistance = x));
    }

    private void RefreshSectors()
    {
      foreach (MyEntity planet in MyPlanets.GetPlanets())
        planet.Components.Get<MyPlanetEnvironmentComponent>().CloseAll();
    }
  }
}
