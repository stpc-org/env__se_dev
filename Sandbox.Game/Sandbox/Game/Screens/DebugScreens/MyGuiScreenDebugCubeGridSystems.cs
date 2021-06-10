// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.DebugScreens.MyGuiScreenDebugCubeGridSystems
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using System;
using System.Linq.Expressions;
using VRage;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
  [MyDebugScreen("VRage", "Grid Systems")]
  internal class MyGuiScreenDebugCubeGridSystems : MyGuiScreenDebugBase
  {
    public override string GetFriendlyName() => "MyGuiScreenDebugCubeGrid";

    public MyGuiScreenDebugCubeGridSystems()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.m_currentPosition.Y += 0.01f;
      this.m_scale = 0.7f;
      this.AddCaption("Cube Grid Systems", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.AddCheckBox("Conveyor line IDs", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_CONVEYORS_LINE_IDS)));
      this.AddCheckBox("Conveyor line capsules", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_CONVEYORS_LINE_CAPSULES)));
      this.AddCheckBox("Connectors and merge blocks", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_CONNECTORS_AND_MERGE_BLOCKS)));
      this.AddCheckBox("Terminal block names", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_BLOCK_NAMES)));
      this.AddCheckBox("Radio broadcasters", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_RADIO_BROADCASTERS)));
      this.AddCheckBox("Neutral ships", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_NEUTRAL_SHIPS)));
      this.AddCheckBox("Resource Distribution", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_RESOURCE_RECEIVERS)));
      this.AddCheckBox("Cockpit", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_COCKPIT)));
      this.AddCheckBox("Conveyors", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_CONVEYORS)));
      this.AddCheckBox("Thruster damage", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_THRUSTER_DAMAGE)));
      this.AddCheckBox("Block groups", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_BLOCK_GROUPS)));
      this.AddCheckBox("Rotors", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_ROTORS)));
      this.AddCheckBox("Gyros", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_GYROS)));
      this.AddCheckBox("Local coordinate system", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.ENABLE_DEBUG_DRAW_COORD_SYS)));
      this.AddCheckBox("Drill Clusters", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyShipMiningSystem.DebugDrawClusters)));
      this.AddCheckBox("Drill Cut-Outs", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyShipMiningSystem.DebugDrawCutOuts)));
      this.AddSlider("Drill Cut-Out Permanence", 1f, 60f, (object) null, MemberHelper.GetMember<float>((Expression<Func<float>>) (() => MyShipMiningSystem.DebugDrawCutOutPermanence)));
      this.AddSlider("Drill Cut-Out Slowdown (s)", 0.0f, 5f, (object) null, MemberHelper.GetMember<float>((Expression<Func<float>>) (() => MyShipMiningSystem.DebugOperationDelay)));
      this.AddCheckBox("Disable Mining System", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyShipMiningSystem.DebugDisable)));
    }
  }
}
