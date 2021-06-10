// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.DebugScreens.MyGuiScreenDebugAi
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using VRage;
using VRage.Library.Utils;
using VRageMath;
using VRageRender.Utils;

namespace Sandbox.Game.Screens.DebugScreens
{
  [MyDebugScreen("Game", "AI")]
  internal class MyGuiScreenDebugAi : MyGuiScreenDebugBase
  {
    private int m_ctr;

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugAi);

    public MyGuiScreenDebugAi()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.m_currentPosition.Y += 0.01f;
      this.m_scale = 0.7f;
      this.AddCaption("Debug screen AI", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.AddButton("Test Chatbot", (Action<MyGuiControlButton>) (x => this.TestChatbot()));
      this.AddLabel("Options:", Color.OrangeRed.ToVector4(), 1f);
      this.AddCheckBox("Remove voxel navmesh cells", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.REMOVE_VOXEL_NAVMESH_CELLS)));
      this.m_currentPosition.Y -= 0.005f;
      this.AddCheckBox("Debug draw bots", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_BOTS)));
      this.m_currentPosition.Y -= 0.005f;
      this.AddCheckBox("    * Bot steering", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_BOT_STEERING)));
      this.m_currentPosition.Y -= 0.005f;
      this.AddCheckBox("    * Bot aiming", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_BOT_AIMING)));
      this.m_currentPosition.Y -= 0.005f;
      this.AddCheckBox("    * Bot navigation", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_BOT_NAVIGATION)));
      this.m_currentPosition.Y += 0.01f;
      this.AddLabel("Navmesh debug draw:", Color.OrangeRed.ToVector4(), 1f);
      this.AddCheckBox("Draw found path", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.DEBUG_DRAW_FOUND_PATH)));
      this.m_currentPosition.Y -= 0.005f;
      this.AddCheckBox("Draw funnel path refining", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.DEBUG_DRAW_FUNNEL)));
      this.m_currentPosition.Y -= 0.005f;
      this.AddCheckBox("Processed voxel navmesh cells", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.DEBUG_DRAW_NAVMESH_PROCESSED_VOXEL_CELLS)));
      this.m_currentPosition.Y -= 0.005f;
      this.AddCheckBox("Prepared voxel navmesh cells", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.DEBUG_DRAW_NAVMESH_PREPARED_VOXEL_CELLS)));
      this.m_currentPosition.Y -= 0.005f;
      this.AddCheckBox("Cells on paths", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.DEBUG_DRAW_NAVMESH_CELLS_ON_PATHS)));
      this.m_currentPosition.Y -= 0.005f;
      this.AddCheckBox("Voxel navmesh connection helper", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.DEBUG_DRAW_VOXEL_CONNECTION_HELPER)));
      this.m_currentPosition.Y -= 0.005f;
      this.AddCheckBox("Draw navmesh links", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.DEBUG_DRAW_NAVMESH_LINKS)));
      this.m_currentPosition.Y -= 0.005f;
      this.m_currentPosition.Y += 0.01f;
      this.AddLabel("Hierarchical pathfinding:", Color.OrangeRed.ToVector4(), 1f);
      this.AddCheckBox("Navmesh cell borders", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.DEBUG_DRAW_NAVMESH_CELL_BORDERS)));
      this.m_currentPosition.Y -= 0.005f;
      this.AddCheckBox("HPF (draw navmesh hierarchy)", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.DEBUG_DRAW_NAVMESH_HIERARCHY)));
      this.m_currentPosition.Y -= 0.005f;
      this.AddCheckBox("    * (Lite version)", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.DEBUG_DRAW_NAVMESH_HIERARCHY_LITE)));
      this.m_currentPosition.Y -= 0.005f;
      this.AddCheckBox("    + Explored HL cells", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.DEBUG_DRAW_NAVMESH_EXPLORED_HL_CELLS)));
      this.m_currentPosition.Y -= 0.005f;
      this.AddCheckBox("    + Fringe HL cells", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.DEBUG_DRAW_NAVMESH_FRINGE_HL_CELLS)));
      this.m_currentPosition.Y -= 0.005f;
      this.m_currentPosition.Y += 0.01f;
      this.AddLabel("Winged-edge mesh debug draw:", Color.OrangeRed.ToVector4(), 1f);
      Vector2 currentPosition1 = this.m_currentPosition;
      this.AddCheckBox("    Lines", (Func<bool>) (() => (uint) (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES & MyWEMDebugDrawMode.LINES) > 0U), (Action<bool>) (b => MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES = b ? MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES | MyWEMDebugDrawMode.LINES : MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES & ~MyWEMDebugDrawMode.LINES), checkBoxOffset: new Vector2?(new Vector2(-0.15f, 0.0f)));
      this.m_currentPosition = currentPosition1 + new Vector2(0.15f, 0.0f);
      this.AddCheckBox("    Lines Z-culled", (Func<bool>) (() => (uint) (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES & MyWEMDebugDrawMode.LINES_DEPTH) > 0U), (Action<bool>) (b => MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES = b ? MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES | MyWEMDebugDrawMode.LINES_DEPTH : MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES & ~MyWEMDebugDrawMode.LINES_DEPTH), checkBoxOffset: new Vector2?(new Vector2(-0.15f, 0.0f)));
      this.m_currentPosition.X = currentPosition1.X;
      Vector2 currentPosition2 = this.m_currentPosition;
      this.AddCheckBox("    Edges", (Func<bool>) (() => (uint) (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES & MyWEMDebugDrawMode.EDGES) > 0U), (Action<bool>) (b => MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES = b ? MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES | MyWEMDebugDrawMode.EDGES : MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES & ~MyWEMDebugDrawMode.EDGES), checkBoxOffset: new Vector2?(new Vector2(-0.15f, 0.0f)));
      this.m_currentPosition = currentPosition2 + new Vector2(0.15f, 0.0f);
      this.AddCheckBox("    Faces", (Func<bool>) (() => (uint) (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES & MyWEMDebugDrawMode.FACES) > 0U), (Action<bool>) (b => MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES = b ? MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES | MyWEMDebugDrawMode.FACES : MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES & ~MyWEMDebugDrawMode.FACES), checkBoxOffset: new Vector2?(new Vector2(-0.15f, 0.0f)));
      this.m_currentPosition.X = currentPosition2.X;
      Vector2 currentPosition3 = this.m_currentPosition;
      this.AddCheckBox("    Vertices", (Func<bool>) (() => (uint) (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES & MyWEMDebugDrawMode.VERTICES) > 0U), (Action<bool>) (b => MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES = b ? MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES | MyWEMDebugDrawMode.VERTICES : MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES & ~MyWEMDebugDrawMode.VERTICES), checkBoxOffset: new Vector2?(new Vector2(-0.15f, 0.0f)));
      this.m_currentPosition = currentPosition3 + new Vector2(0.15f, 0.0f);
      this.AddCheckBox("    Vertices detailed", (Func<bool>) (() => (uint) (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES & MyWEMDebugDrawMode.VERTICES_DETAILED) > 0U), (Action<bool>) (b => MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES = b ? MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES | MyWEMDebugDrawMode.VERTICES_DETAILED : MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES & ~MyWEMDebugDrawMode.VERTICES_DETAILED), checkBoxOffset: new Vector2?(new Vector2(-0.15f, 0.0f)));
      this.m_currentPosition.X = currentPosition3.X;
      Vector2 currentPosition4 = this.m_currentPosition;
      this.AddCheckBox("    Normals", (Func<bool>) (() => (uint) (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES & MyWEMDebugDrawMode.NORMALS) > 0U), (Action<bool>) (b => MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES = b ? MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES | MyWEMDebugDrawMode.NORMALS : MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES & ~MyWEMDebugDrawMode.NORMALS), checkBoxOffset: new Vector2?(new Vector2(-0.15f, 0.0f)));
      this.AddCheckBox("Animals", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_ANIMALS)));
      this.AddCheckBox("Spiders", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_FAUNA_COMPONENT)));
      this.AddCheckBox("Switch Survival/Creative", (Func<bool>) (() => MySession.Static.CreativeMode), (Action<bool>) (b => MySession.Static.Settings.GameMode = b ? MyGameModeEnum.Creative : MyGameModeEnum.Survival));
    }

    private void TestChatbot()
    {
      StreamReader streamReader = new StreamReader("c:\\x\\stats.log");
      StreamWriter outF = new StreamWriter("c:\\x\\stats_resp.csv", false);
      this.m_ctr = 0;
      int num = 0;
      MyTimeSpan myTimeSpan1 = new MyTimeSpan(Stopwatch.GetTimestamp());
      while (!streamReader.EndOfStream)
      {
        string line = streamReader.ReadLine();
        MySession.Static.ChatBot?.FilterMessage("? " + line, (Action<string>) (x =>
        {
          outF.WriteLine("\"" + line + "\", \"" + x + "\"");
          --this.m_ctr;
        }));
        MySandboxGame.Static.ProcessInvoke();
        ++this.m_ctr;
        ++num;
      }
      while (this.m_ctr != 0)
        MySandboxGame.Static.ProcessInvoke();
      streamReader.Close();
      outF.Close();
      MyTimeSpan myTimeSpan2 = new MyTimeSpan(Stopwatch.GetTimestamp());
    }
  }
}
