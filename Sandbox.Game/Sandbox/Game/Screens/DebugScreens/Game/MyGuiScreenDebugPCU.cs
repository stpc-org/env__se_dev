// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.DebugScreens.Game.MyGuiScreenDebugPCU
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VRage;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Screens.DebugScreens.Game
{
  [MyDebugScreen("Game", "PCU")]
  internal class MyGuiScreenDebugPCU : MyGuiScreenDebugBase
  {
    private List<MyCubeGrid> m_selectedGrids = new List<MyCubeGrid>();

    public MyGuiScreenDebugPCU()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.AddCaption("PCU", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.AddVerticalSpacing(0.01f * this.m_scale);
      this.AddCheckBox("Use console PCU", (object) MySession.Static.Settings, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MySession.Static.Settings.UseConsolePCU)));
      this.AddVerticalSpacing(0.01f * this.m_scale);
      MyGuiControlCombobox identities = this.AddCombo();
      identities.AddItem(0L, "Nobody");
      MySession mySession = MySession.Static;
      foreach (MyIdentity myIdentity in (IEnumerable<MyIdentity>) ((mySession != null ? (object) mySession.Players.GetAllIdentities() : (object) null) ?? (object) Array.Empty<MyIdentity>()))
        identities.AddItem(myIdentity.IdentityId, myIdentity.DisplayName);
      this.AddButton("Set Authorship", (Action<MyGuiControlButton>) (_ => this.ForEachBlockOnSelectedGrids((Action<MySlimBlock>) (x => x.TransferAuthorship(identities.GetSelectedKey())))));
    }

    public override bool Update(bool hasFocus)
    {
      this.m_selectedGrids.Clear();
      MySession mySession = MySession.Static;
      MyCubeGrid targetGrid;
      if ((mySession != null ? (mySession.Ready ? 1 : 0) : 0) != 0 && (targetGrid = MyCubeGrid.GetTargetGrid()) != null)
      {
        MyCubeGridGroups.Static.Mechanical.GetGroupNodes(targetGrid, this.m_selectedGrids);
        if (this.m_selectedGrids.Count > 0)
        {
          BoundingBoxD invalid = BoundingBoxD.CreateInvalid();
          foreach (MyCubeGrid selectedGrid in this.m_selectedGrids)
            invalid.Include(selectedGrid.PositionComp.WorldAABB);
          MyRenderProxy.DebugDrawAABB(invalid, Color.Red);
        }
      }
      return base.Update(hasFocus);
    }

    private void ForEachBlockOnSelectedGrids(Action<MySlimBlock> func)
    {
      foreach (MyCubeGrid selectedGrid in this.m_selectedGrids)
      {
        foreach (MySlimBlock cubeBlock in selectedGrid.CubeBlocks)
          func(cubeBlock);
      }
    }
  }
}
