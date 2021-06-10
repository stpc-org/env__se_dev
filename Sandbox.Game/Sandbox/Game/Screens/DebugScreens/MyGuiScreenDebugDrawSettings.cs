// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.DebugScreens.MyGuiScreenDebugDrawSettings
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using System;
using System.Linq.Expressions;
using VRage;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
  [MyDebugScreen("VRage", "Debug draw settings")]
  internal class MyGuiScreenDebugDrawSettings : MyGuiScreenDebugBase
  {
    public override string GetFriendlyName() => nameof (MyGuiScreenDebugDrawSettings);

    public MyGuiScreenDebugDrawSettings()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.m_currentPosition.Y += 0.01f;
      this.m_scale = 0.7f;
      this.AddCaption("Debug draw settings 1", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.AddCheckBox("Debug draw", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.ENABLE_DEBUG_DRAW)));
      this.AddCheckBox("Entity IDs", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_ENTITY_IDS)));
      this.AddCheckBox("    Only root entities", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_ENTITY_IDS_ONLY_ROOT)));
      this.AddCheckBox("Model dummies", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_MODEL_DUMMIES)));
      this.AddCheckBox("Displaced bones", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_DISPLACED_BONES)));
      this.AddCheckBox("Skeleton cube bones", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_SKETELON_CUBE_BONES)));
      this.AddCheckBox("Vertices Cache", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_VERTICES_CACHE)));
      this.AddCheckBox("Projectiles", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_PROJECTILES)));
      this.AddCheckBox("Interpolation", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_INTERPOLATION)));
      this.AddCheckBox("Mount points", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS)));
      this.AddCheckBox("GUI screen borders", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.DRAW_GUI_SCREEN_BORDERS)));
      this.AddCheckBox("Draw physics", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_PHYSICS)));
      this.AddCheckBox("Triangle physics", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_TRIANGLE_PHYSICS)));
      this.AddCheckBox("Audio debug draw", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_AUDIO)));
      this.AddCheckBox("Show invalid triangles", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.SHOW_INVALID_TRIANGLES)));
      this.AddCheckBox("Show stockpile quantities", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_STOCKPILE_QUANTITIES)));
      this.AddCheckBox("Show suit battery capacity", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_SUIT_BATTERY_CAPACITY)));
      this.AddCheckBox("Show character bones", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_BONES)));
      this.AddCheckBox("Character miscellaneous", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_MISC)));
      this.AddCheckBox("Game prunning structure", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_GAME_PRUNNING)));
      this.AddCheckBox("Miscellaneous", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_MISCELLANEOUS)));
      this.AddCheckBox("Events", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_EVENTS)));
      this.AddCheckBox("Volumetric explosion coloring", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_VOLUMETRIC_EXPLOSION_COLORING)));
    }
  }
}
