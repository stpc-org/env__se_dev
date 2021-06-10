// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.DebugScreens.MyGuiScreenDebugDrawSettings3
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions.GUI;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.Weapons;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using System;
using System.Linq.Expressions;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.GUI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Screens.DebugScreens
{
  [MyDebugScreen("VRage", "Debug draw settings 3")]
  internal class MyGuiScreenDebugDrawSettings3 : MyGuiScreenDebugBase
  {
    public override string GetFriendlyName() => nameof (MyGuiScreenDebugDrawSettings3);

    public MyGuiScreenDebugDrawSettings3()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.m_currentPosition.Y += 0.01f;
      this.m_scale = 0.7f;
      this.AddCaption("Debug draw settings 3", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.AddCheckBox("Debug decals", MyRenderProxy.Settings.DebugDrawDecals, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DebugDrawDecals = x.IsChecked));
      this.AddCheckBox("Decals default material", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.ENABLE_USE_DEFAULT_DAMAGE_DECAL)));
      this.AddButton(new StringBuilder("Clear decals"), new Action<MyGuiControlButton>(MyGuiScreenDebugDrawSettings3.ClearDecals));
      this.AddCheckBox("Debug Particles", (Func<bool>) (() => MyDebugDrawSettings.DEBUG_DRAW_PARTICLES), (Action<bool>) (x => MyDebugDrawSettings.DEBUG_DRAW_PARTICLES = x));
      this.AddCheckBox("Entity update statistics", (Func<bool>) (() => MyDebugDrawSettings.DEBUG_DRAW_ENTITY_STATISTICS), (Action<bool>) (x => MyDebugDrawSettings.DEBUG_DRAW_ENTITY_STATISTICS = x));
      this.AddCheckBox("3rd person camera", (Func<bool>) (() => MyThirdPersonSpectator.Static != null && MyThirdPersonSpectator.Static.EnableDebugDraw), (Action<bool>) (x =>
      {
        if (MyThirdPersonSpectator.Static == null)
          return;
        MyThirdPersonSpectator.Static.EnableDebugDraw = x;
      }));
      this.AddCheckBox("Inverse kinematics", (Func<bool>) (() => MyDebugDrawSettings.DEBUG_DRAW_INVERSE_KINEMATICS), (Action<bool>) (x => MyDebugDrawSettings.DEBUG_DRAW_INVERSE_KINEMATICS = x));
      this.AddCheckBox("Character tools", (Func<bool>) (() => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_TOOLS), (Action<bool>) (x => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_TOOLS = x));
      this.AddCheckBox("Force tools 1st person", (Func<bool>) (() => MyFakes.FORCE_CHARTOOLS_1ST_PERSON), (Action<bool>) (x => MyFakes.FORCE_CHARTOOLS_1ST_PERSON = x));
      this.AddCheckBox("HUD", (Func<bool>) (() => MyDebugDrawSettings.DEBUG_DRAW_HUD), (Action<bool>) (x => MyDebugDrawSettings.DEBUG_DRAW_HUD = x));
      this.AddCheckBox("Server Messages (Performance Warnings)", (Func<bool>) (() => MyDebugDrawSettings.DEBUG_DRAW_SERVER_WARNINGS), (Action<bool>) (x => MyDebugDrawSettings.DEBUG_DRAW_SERVER_WARNINGS = x));
      this.AddCheckBox("Network Sync", (Func<bool>) (() => MyDebugDrawSettings.DEBUG_DRAW_NETWORK_SYNC), (Action<bool>) (x => MyDebugDrawSettings.DEBUG_DRAW_NETWORK_SYNC = x));
      this.AddCheckBox("Grid hierarchy", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_GRID_HIERARCHY)));
      this.AddButton("Reload HUD", (Action<MyGuiControlButton>) (x => MyGuiScreenDebugDrawSettings3.ReloadHud()));
      this.AddCheckBox("Turret Target Prediction", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyLargeTurretBase.DEBUG_DRAW_TARGET_PREDICTION)));
      this.AddCheckBox("Projectile Trajectory", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyProjectile.DEBUG_DRAW_PROJECTILE_TRAJECTORY)));
      this.AddCheckBox("Missile Trajectory", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyMissile.DEBUG_DRAW_MISSILE_TRAJECTORY)));
      this.AddCheckBox("Show Joystick Controls", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_JOYSTICK_CONTROL_HINTS)));
      this.AddCheckBox("Draw Gui Control Borders", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyGuiControlBase.DEBUG_CONTROL_BORDERS)));
      this.AddCheckBox("Voxel - full cells", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_VOXEL_FULLCELLS)));
      this.AddCheckBox("Voxel - content micro nodes", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_VOXEL_CONTENT_MICRONODES)));
      this.AddCheckBox("Voxel - content micro nodes scaled", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_VOXEL_CONTENT_MICRONODES_SCALED)));
      this.AddCheckBox("Voxel - content macro nodes", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_VOXEL_CONTENT_MACRONODES)));
      this.AddCheckBox("Voxel - content macro leaves", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_VOXEL_CONTENT_MACROLEAVES)));
      this.AddCheckBox("Voxel - content macro scaled", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_VOXEL_CONTENT_MACRO_SCALED)));
      this.AddCheckBox("Voxel - materials macro nodes", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_VOXEL_MATERIALS_MACRONODES)));
      this.AddCheckBox("Voxel - materials macro leaves", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_VOXEL_MATERIALS_MACROLEAVES)));
    }

    private static void ClearDecals(MyGuiControlButton button) => MyRenderProxy.ClearDecals();

    protected override void ValueChanged(MyGuiControlBase sender) => MyRenderProxy.SetSettingsDirty();

    private static bool ReloadHud()
    {
      MyHudDefinition hudDefinition = MyHud.HudDefinition;
      MyGuiTextureAtlasDefinition definition = MyDefinitionManagerBase.Static.GetDefinition<MyGuiTextureAtlasDefinition>(MyStringHash.GetOrCompute("Base"));
      MyObjectBuilder_Definitions objectBuilder;
      if (!MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_Definitions>(hudDefinition.Context.CurrentFile, out objectBuilder))
      {
        MyAPIGateway.Utilities.ShowNotification("Failed to load Hud.sbc!", 3000, "Red");
        return false;
      }
      hudDefinition.Init(objectBuilder.Definitions[0], hudDefinition.Context);
      if (!MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_Definitions>(definition.Context.CurrentFile, out objectBuilder))
      {
        MyAPIGateway.Utilities.ShowNotification("Failed to load GuiTextures.sbc!", 3000, "Red");
        return false;
      }
      definition.Init(objectBuilder.Definitions[0], definition.Context);
      MyGuiTextures.Static.Reload();
      MyScreenManager.CloseScreen(MyPerGameSettings.GUI.HUDScreen);
      MyScreenManager.AddScreen(Activator.CreateInstance(MyPerGameSettings.GUI.HUDScreen) as MyGuiScreenBase);
      return true;
    }
  }
}
