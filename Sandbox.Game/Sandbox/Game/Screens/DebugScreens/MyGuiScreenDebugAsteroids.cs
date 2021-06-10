// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.DebugScreens.MyGuiScreenDebugAsteroids
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using System;
using System.Linq.Expressions;
using VRage;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
  [MyDebugScreen("VRage", "Asteroids")]
  public class MyGuiScreenDebugAsteroids : MyGuiScreenDebugBase
  {
    public MyGuiScreenDebugAsteroids()
      : base()
      => this.RecreateControls(true);

    public override string GetFriendlyName() => this.GetType().Name;

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.13f);
      this.AddCaption("Asteroids", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.AddLabel("Asteroid generator " + (object) MySession.Static?.Settings.VoxelGeneratorVersion, (Vector4) Color.Yellow, 1f);
      this.AddCheckBox("Draw voxelmap AABB", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_VOXEL_MAP_AABB)));
      this.AddCheckBox("Draw asteroid composition", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_ASTEROID_COMPOSITION)));
      this.AddCheckBox("Draw asteroid seeds", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_ASTEROID_SEEDS)));
      this.AddCheckBox("Draw asteroid content", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_ASTEROID_COMPOSITION_CONTENT)));
      this.AddCheckBox("Draw asteroid ores", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_ASTEROID_ORES)));
    }
  }
}
