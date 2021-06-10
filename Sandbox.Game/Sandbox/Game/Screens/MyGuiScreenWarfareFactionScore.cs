// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenWarfareFactionScore
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
  internal class MyGuiScreenWarfareFactionScore : MyGuiScreenBase
  {
    private MyGuiControlLabel m_warfare_timeRemainting_time;
    private MySessionComponentMatch m_match;
    private float m_yPos = -0.45f;
    private float m_warfareHeight = 0.09f;
    private float m_warfareWidth = 0.18f;
    private int m_warfareUpdate_frameCount = 30;
    private int m_warfareUpdate_frameCount_current = 30;

    public MyGuiScreenWarfareFactionScore()
      : base()
    {
      this.CloseButtonEnabled = false;
      this.CanHaveFocus = false;
      this.m_drawEvenWithoutFocus = true;
      this.CanHideOthers = false;
      this.m_match = MySession.Static.GetComponent<MySessionComponentMatch>();
      this.RecreateControls(false);
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenWarfareFactionScore);

    public override bool Update(bool hasFocus)
    {
      if (this.m_warfareUpdate_frameCount_current >= this.m_warfareUpdate_frameCount)
      {
        this.m_warfareUpdate_frameCount_current = 0;
        foreach (MyGuiControlBase control in this.Controls)
        {
          if (control.GetType() == typeof (MyGuiScreenPlayersWarfareTeamScoreTable))
          {
            IMyFaction factionById = MySession.Static.Factions.TryGetFactionById(((MyGuiScreenPlayersWarfareTeamScoreTable) control).FactionId);
            if (factionById != null && factionById.FactionId == ((MyGuiScreenPlayersWarfareTeamScoreTable) control).FactionId)
              MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (s => new Action<long>(MyFactionCollection.RequestFactionScoreAndPercentageUpdate)), factionById.FactionId, MyEventContext.Current.Sender);
          }
        }
        if (this.m_match != null && this.m_match.IsEnabled)
        {
          TimeSpan timeSpan = TimeSpan.FromMinutes((double) this.m_match.RemainingMinutes);
          string str = timeSpan.ToString(timeSpan.TotalHours >= 1.0 ? "hh\\:mm\\:ss" : "mm\\:ss");
          if (this.m_warfare_timeRemainting_time.Text != str)
            this.m_warfare_timeRemainting_time.Text = str;
        }
        foreach (MyGuiControlBase control in this.Controls)
        {
          if (control is MyGuiScreenPlayersWarfareTeamScoreTable)
          {
            MyGuiScreenPlayersWarfareTeamScoreTable warfareTeamScoreTable = control as MyGuiScreenPlayersWarfareTeamScoreTable;
            IMyFaction factionById = MySession.Static.Factions.TryGetFactionById(warfareTeamScoreTable.FactionId);
            if (factionById != null && factionById.FactionId == warfareTeamScoreTable.FactionId)
            {
              warfareTeamScoreTable.ScorePoints = factionById.Score;
              warfareTeamScoreTable.ObjectiveFinishedPercentage = factionById.ObjectivePercentageCompleted;
              warfareTeamScoreTable.SetThisAsLocalsPlayerFactionTable(factionById.IsMember(MySession.Static.LocalHumanPlayer.Identity.IdentityId));
            }
          }
        }
      }
      ++this.m_warfareUpdate_frameCount_current;
      if (!MyHud.IsVisible || MyHud.HudState == 0 || MyGuiScreenHudSpace.Static == null || (MyGuiScreenHudSpace.Static != null && MyGuiScreenHudSpace.Static.State == MyGuiScreenState.HIDDEN || MyGuiScreenHudSpace.Static.State == MyGuiScreenState.HIDING))
      {
        if (this.State != MyGuiScreenState.HIDDEN && this.State != MyGuiScreenState.HIDING)
          this.State = MyGuiScreenState.HIDING;
      }
      else if (this.State != MyGuiScreenState.OPENING && this.State != MyGuiScreenState.OPENED)
        this.State = MyGuiScreenState.OPENING;
      return base.Update(hasFocus);
    }

    public override void RecreateControls(bool constructor)
    {
      this.State = MyGuiScreenState.HIDDEN;
      base.RecreateControls(constructor);
      MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
      if (!component.IsEnabled)
        return;
      List<MyFaction> myFactionList = new List<MyFaction>();
      foreach (MyFaction allFaction in MySession.Static.Factions.GetAllFactions())
      {
        if ((allFaction.Name.StartsWith("Red") || allFaction.Name.StartsWith("Green") || allFaction.Name.StartsWith("Blue")) && allFaction.Name.EndsWith("Faction"))
          myFactionList.Add(allFaction);
      }
      int num = 0;
      foreach (MyFaction myFaction in myFactionList)
      {
        this.Controls.Add((MyGuiControlBase) new MyGuiScreenPlayersWarfareTeamScoreTable(new Vector2((float) (0.5 - (double) this.m_warfareWidth * 1.5 + (double) num * (double) this.m_warfareWidth), this.m_warfareHeight / 2f), this.m_warfareWidth, this.m_warfareHeight, myFaction.Name, myFaction.FactionIcon.Value.String, MyTexts.GetString(MySpaceTexts.WarfareCounter_EscapePod), myFaction.FactionId, true));
        ++num;
      }
      TimeSpan timeSpan = TimeSpan.FromMinutes((double) component.RemainingMinutes);
      this.m_warfare_timeRemainting_time = new MyGuiControlLabel();
      this.m_warfare_timeRemainting_time.DrawAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_warfare_timeRemainting_time.Text = timeSpan.ToString(timeSpan.TotalHours >= 1.0 ? "hh\\:mm\\:ss" : "mm\\:ss");
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(new Vector2(-this.m_warfare_timeRemainting_time.Size.Y, -0.47f)));
      myGuiControlLabel.DrawAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
      myGuiControlLabel.Text = MyTexts.GetString(MySpaceTexts.WarfareCounter_TimeRemaining).ToString() + ": ";
      this.m_warfare_timeRemainting_time.Position = new Vector2(myGuiControlLabel.PositionX + myGuiControlLabel.Size.X / 2f, myGuiControlLabel.PositionY);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      this.Controls.Add((MyGuiControlBase) this.m_warfare_timeRemainting_time);
    }
  }
}
