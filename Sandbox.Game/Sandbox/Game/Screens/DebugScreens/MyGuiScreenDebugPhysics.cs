// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.DebugScreens.MyGuiScreenDebugPhysics
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using System;
using System.Linq.Expressions;
using VRage;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
  [MyDebugScreen("VRage", "Physics")]
  public class MyGuiScreenDebugPhysics : MyGuiScreenDebugBase
  {
    public MyGuiScreenDebugPhysics()
      : base()
      => this.RecreateControls(true);

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugPhysics);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.BackgroundColor = new Vector4?(new Vector4(1f, 1f, 1f, 0.5f));
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.13f);
      this.AddCaption("Physics", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.AddCaption("Debug Draw");
      this.AddCheckBox("Shapes", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_SHAPES)));
      this.AddCheckBox("Inertia tensors", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_INERTIA_TENSORS)));
      this.AddCheckBox("Clusters", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_CLUSTERS)));
      this.AddCheckBox("Forces", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_FORCES)));
      this.AddCheckBox("Friction", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_FRICTION)));
      this.AddCheckBox("Constraints", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_CONSTRAINTS)));
      this.AddCheckBox("Simulation islands", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_SIMULATION_ISLANDS)));
      this.AddCheckBox("Motion types", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_MOTION_TYPES)));
      this.AddCheckBox("Velocities", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_VELOCITIES)));
      this.AddCheckBox("Velocities interpolated", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_INTERPOLATED_VELOCITIES)));
      this.AddCheckBox("TOI optimized grids", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_TOI_OPTIMIZED_GRIDS)));
      this.AddSubcaption("Hk scheduling");
      this.AddCheckBox("Havok multithreading", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.ENABLE_HAVOK_MULTITHREADING)));
      this.AddCheckBox("Parallel scheduling", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.ENABLE_HAVOK_PARALLEL_SCHEDULING)));
      this.AddButton("Set on server", (Action<MyGuiControlButton>) (x => MyPhysics.CommitSchedulingSettingToServer()));
      this.AddButton("Record VDB", (Action<MyGuiControlButton>) (x =>
      {
        MyPhysics.SyncVDBCamera = true;
        MyMultiplayer.RaiseStaticEvent<string>((Func<IMyEventOwner, Action<string>>) (_ => new Action<string>(MyPhysics.ControlVDBRecording)), DateTime.Now.ToString() + ".hkm");
      }));
      this.AddButton("Stop VDB recording", (Action<MyGuiControlButton>) (x => MyMultiplayer.RaiseStaticEvent<string>((Func<IMyEventOwner, Action<string>>) (_ => new Action<string>(MyPhysics.ControlVDBRecording)), (string) null)));
      this.AddSubcaption("Physics options");
      this.AddCheckBox("Enable Welding", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.WELD_LANDING_GEARS)));
      this.AddCheckBox("Weld pistons", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.WELD_PISTONS)));
      this.AddCheckBox("Wheel softness", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.WHEEL_SOFTNESS))).SetToolTip("Needs to be true at world load.");
      this.AddCheckBox("Suspension power ratio", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.SUSPENSION_POWER_RATIO)));
      this.AddCheckBox("Two step simulations", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.TWO_STEP_SIMULATIONS)));
      this.AddButton("Start VDB", (Action<MyGuiControlButton>) (x => HkVDB.Start()));
      this.AddButton("Force cluster reorder", (Action<MyGuiControlButton>) (x => MyFakes.FORCE_CLUSTER_REORDER = true));
    }
  }
}
