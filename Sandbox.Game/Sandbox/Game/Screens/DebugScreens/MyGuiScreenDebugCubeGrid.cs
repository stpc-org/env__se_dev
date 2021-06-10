// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.DebugScreens.MyGuiScreenDebugCubeGrid
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using System;
using System.Linq.Expressions;
using VRage;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
  [MyDebugScreen("VRage", "Cube Grid")]
  internal class MyGuiScreenDebugCubeGrid : MyGuiScreenDebugBase
  {
    public override string GetFriendlyName() => nameof (MyGuiScreenDebugCubeGrid);

    public MyGuiScreenDebugCubeGrid()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.m_currentPosition.Y += 0.01f;
      this.m_scale = 0.7f;
      this.AddCaption("Cube Grid", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.AddLabel("Cube Grid", Color.White.ToVector4(), 1f);
      this.m_currentPosition.Y += 0.01f;
      this.AddCheckBox("Grid names", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_GRID_NAMES)));
      this.AddCheckBox("Grid origins", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_GRID_ORIGINS)));
      this.AddCheckBox("Grid control", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_GRID_CONTROL)));
      this.AddCheckBox("Grid AABBs", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_GRID_AABB)));
      this.AddCheckBox("Removed cube coordinates", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_REMOVE_CUBE_COORDS)));
      this.AddCheckBox("Grid counter", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_GRID_COUNTER)));
      this.AddCheckBox("Grid terminal systems", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_GRID_TERMINAL_SYSTEMS)));
      this.AddCheckBox("Grid dirty blocks", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_GRID_DIRTY_BLOCKS)));
      this.AddCheckBox("Grid groups - physical", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_GRID_GROUPS_PHYSICAL)));
      this.AddCheckBox("Grid groups - physical dynamic", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_DYNAMIC_PHYSICAL_GROUPS)));
      this.AddCheckBox("Grid groups - logical", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_GRID_GROUPS_LOGICAL)));
      this.AddCheckBox("CubeBlock AABBs", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_CUBE_BLOCK_AABBS)));
      this.AddCheckBox("Grid statistics", (Func<bool>) (() => MyDebugDrawSettings.DEBUG_DRAW_GRID_STATISTICS), (Action<bool>) (x => MyDebugDrawSettings.DEBUG_DRAW_GRID_STATISTICS = x));
      this.AddCheckBox("Grid Update Schedule", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_GRID_UPDATES)));
      this.AddSlider("Grid Update History Length", 60f, 1200f, (Func<float>) (() => (float) MyCubeGrid.DebugUpdateHistoryDuration), (Action<float>) (x => MyCubeGrid.DebugUpdateHistoryDuration = (int) x));
    }
  }
}
