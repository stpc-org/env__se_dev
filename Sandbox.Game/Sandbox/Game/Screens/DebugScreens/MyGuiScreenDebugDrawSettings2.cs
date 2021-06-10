// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.DebugScreens.MyGuiScreenDebugDrawSettings2
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
  [MyDebugScreen("VRage", "Debug draw settings 2")]
  internal class MyGuiScreenDebugDrawSettings2 : MyGuiScreenDebugBase
  {
    public override string GetFriendlyName() => "MyGuiScreenDebugDrawSettings";

    public MyGuiScreenDebugDrawSettings2()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.m_currentPosition.Y += 0.01f;
      this.m_scale = 0.7f;
      this.AddCaption("Debug draw settings 2", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.AddCheckBox("Entity components", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_ENTITY_COMPONENTS)));
      this.AddCheckBox("Controlled entities", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_CONTROLLED_ENTITIES)));
      this.AddCheckBox("Copy paste", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_COPY_PASTE)));
      this.AddCheckBox("Voxel geometry cell", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_VOXEL_GEOMETRY_CELL)));
      this.AddCheckBox("Voxel map AABB", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_VOXEL_MAP_AABB)));
      this.AddCheckBox("Respawn ship counters", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_RESPAWN_SHIP_COUNTERS)));
      this.AddCheckBox("Explosion Havok raycasts", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_EXPLOSION_HAVOK_RAYCASTS)));
      this.AddCheckBox("Explosion DDA raycasts", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_EXPLOSION_DDA_RAYCASTS)));
      this.AddCheckBox("Physics clusters", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_CLUSTERS)));
      this.AddCheckBox("Environment items (trees, bushes, ...)", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_ENVIRONMENT_ITEMS)));
      this.AddCheckBox("Block groups - small to large", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_SMALL_TO_LARGE_BLOCK_GROUPS)));
      this.AddCheckBox("Ropes", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_ROPES)));
      this.AddCheckBox("Oxygen", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_OXYGEN)));
      this.AddCheckBox("Voxel physics prediction", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_VOXEL_PHYSICS_PREDICTION)));
      this.AddCheckBox("Update trigger", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_UPDATE_TRIGGER)));
    }
  }
}
