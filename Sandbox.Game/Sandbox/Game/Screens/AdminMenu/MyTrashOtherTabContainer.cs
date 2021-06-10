// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.AdminMenu.MyTrashOtherTabContainer
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Game.Entity;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.AdminMenu
{
  [StaticEventOwner]
  internal class MyTrashOtherTabContainer : MyTabContainer
  {
    private MyGuiControlTextbox m_textboxOptimalGridCount;
    private MyGuiControlTextbox m_textboxCharacterRemovalTrash;
    private MyGuiControlTextbox m_textboxAfkTimeout;
    private MyGuiControlTextbox m_tbStopGridsPeriod;
    private MyGuiControlTextbox m_tbRemoveInactiveIdent;

    public MyTrashOtherTabContainer(MyGuiScreenBase parentScreen)
      : base(parentScreen)
    {
      this.Control.Size = new Vector2(this.Control.Size.X, 0.265f);
      Vector2 currentPosition1 = -this.Control.Size * 0.5f;
      Vector2? size = parentScreen.GetSize();
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
      myGuiControlLabel1.Position = currentPosition1;
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel1.Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_OptimalGridCount);
      MyGuiControlLabel control1 = myGuiControlLabel1;
      control1.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_OptimalGridCount_Tooltip));
      this.Control.Controls.Add((MyGuiControlBase) control1);
      this.m_textboxOptimalGridCount = this.AddTextbox(ref currentPosition1, MySession.Static.Settings.OptimalGridCount.ToString(), (Action<MyGuiControlTextbox>) null, new Vector4?(MyTabContainer.LABEL_COLOR), 0.9f, MyGuiControlTextboxType.DigitsOnly, addToControls: false);
      this.Control.Controls.Add((MyGuiControlBase) this.m_textboxOptimalGridCount);
      this.m_textboxOptimalGridCount.Size = new Vector2(0.07f, this.m_textboxOptimalGridCount.Size.Y);
      this.m_textboxOptimalGridCount.PositionY = currentPosition1.Y - 0.01f;
      this.m_textboxOptimalGridCount.PositionX = (float) ((double) currentPosition1.X + (double) size.Value.X - (double) this.m_textboxOptimalGridCount.Size.X - 0.0450000017881393);
      this.m_textboxOptimalGridCount.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_OptimalGridCount_Tooltip));
      currentPosition1.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel();
      myGuiControlLabel2.Position = currentPosition1;
      myGuiControlLabel2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel2.Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_PlayerCharacterRemoval);
      myGuiControlLabel2.IsAutoEllipsisEnabled = true;
      myGuiControlLabel2.IsAutoScaleEnabled = true;
      MyGuiControlLabel control2 = myGuiControlLabel2;
      control2.SetMaxWidth(0.24f);
      control2.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_PlayerCharacterRemoval_Tooltip));
      this.Control.Controls.Add((MyGuiControlBase) control2);
      this.m_textboxCharacterRemovalTrash = this.AddTextbox(ref currentPosition1, MySession.Static.Settings.PlayerCharacterRemovalThreshold.ToString(), (Action<MyGuiControlTextbox>) null, new Vector4?(MyTabContainer.LABEL_COLOR), 0.9f, MyGuiControlTextboxType.DigitsOnly, addToControls: false);
      this.Control.Controls.Add((MyGuiControlBase) this.m_textboxCharacterRemovalTrash);
      this.m_textboxCharacterRemovalTrash.Size = new Vector2(0.07f, this.m_textboxCharacterRemovalTrash.Size.Y);
      this.m_textboxCharacterRemovalTrash.PositionX = (float) ((double) currentPosition1.X + (double) size.Value.X - (double) this.m_textboxCharacterRemovalTrash.Size.X - 0.0450000017881393);
      this.m_textboxCharacterRemovalTrash.PositionY = currentPosition1.Y - 0.01f;
      this.m_textboxCharacterRemovalTrash.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_PlayerCharacterRemoval_Tooltip));
      currentPosition1.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel();
      myGuiControlLabel3.Position = currentPosition1;
      myGuiControlLabel3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel3.Text = MyTexts.GetString(MyCommonTexts.ScreenAdmin_Trash_AFKTimeout);
      myGuiControlLabel3.IsAutoEllipsisEnabled = true;
      myGuiControlLabel3.IsAutoScaleEnabled = true;
      MyGuiControlLabel control3 = myGuiControlLabel3;
      control3.SetMaxWidth(0.24f);
      control3.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenAdmin_Trash_AFKTimeout_TTIP));
      this.Control.Controls.Add((MyGuiControlBase) control3);
      this.m_textboxAfkTimeout = this.AddTextbox(ref currentPosition1, MySession.Static.Settings.AFKTimeountMin.ToString(), (Action<MyGuiControlTextbox>) null, new Vector4?(MyTabContainer.LABEL_COLOR), 0.9f, MyGuiControlTextboxType.DigitsOnly, addToControls: false);
      this.Control.Controls.Add((MyGuiControlBase) this.m_textboxAfkTimeout);
      this.m_textboxAfkTimeout.Size = new Vector2(0.07f, this.m_textboxAfkTimeout.Size.Y);
      this.m_textboxAfkTimeout.PositionX = (float) ((double) currentPosition1.X + (double) size.Value.X - (double) this.m_textboxAfkTimeout.Size.X - 0.0450000017881393);
      this.m_textboxAfkTimeout.PositionY = currentPosition1.Y - 0.01f;
      this.m_textboxAfkTimeout.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenAdmin_Trash_AFKTimeout_TTIP));
      currentPosition1.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel4 = new MyGuiControlLabel();
      myGuiControlLabel4.Position = currentPosition1;
      myGuiControlLabel4.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel4.Text = MyTexts.GetString(MyCommonTexts.ScreenAdmin_Trash_StopGridsPeriod);
      myGuiControlLabel4.IsAutoEllipsisEnabled = true;
      myGuiControlLabel4.IsAutoScaleEnabled = true;
      MyGuiControlLabel control4 = myGuiControlLabel4;
      control4.SetMaxWidth(0.24f);
      control4.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenAdmin_Trash_StopGridsPeriod_TTIP));
      this.Control.Controls.Add((MyGuiControlBase) control4);
      this.m_tbStopGridsPeriod = this.AddTextbox(ref currentPosition1, MySession.Static.Settings.StopGridsPeriodMin.ToString(), (Action<MyGuiControlTextbox>) null, new Vector4?(MyTabContainer.LABEL_COLOR), 0.9f, MyGuiControlTextboxType.DigitsOnly, addToControls: false);
      this.Control.Controls.Add((MyGuiControlBase) this.m_tbStopGridsPeriod);
      this.m_tbStopGridsPeriod.Size = new Vector2(0.07f, this.m_tbStopGridsPeriod.Size.Y);
      this.m_tbStopGridsPeriod.PositionX = (float) ((double) currentPosition1.X + (double) size.Value.X - (double) this.m_tbStopGridsPeriod.Size.X - 0.0450000017881393);
      this.m_tbStopGridsPeriod.PositionY = currentPosition1.Y - 0.01f;
      this.m_tbStopGridsPeriod.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenAdmin_Trash_StopGridsPeriod_TTIP));
      currentPosition1.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel5 = new MyGuiControlLabel();
      myGuiControlLabel5.Position = currentPosition1;
      myGuiControlLabel5.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel5.Text = MyTexts.GetString(MyCommonTexts.ScreenAdmin_Trash_RemoveInactiveEnt);
      myGuiControlLabel5.IsAutoEllipsisEnabled = true;
      myGuiControlLabel5.IsAutoScaleEnabled = true;
      MyGuiControlLabel control5 = myGuiControlLabel5;
      control5.SetMaxWidth(0.24f);
      control5.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenAdmin_Trash_RemoveInactiveEnt_TTIP));
      this.Control.Controls.Add((MyGuiControlBase) control5);
      this.m_tbRemoveInactiveIdent = this.AddTextbox(ref currentPosition1, MySession.Static.Settings.RemoveOldIdentitiesH.ToString(), (Action<MyGuiControlTextbox>) null, new Vector4?(MyTabContainer.LABEL_COLOR), 0.9f, MyGuiControlTextboxType.DigitsOnly, addToControls: false);
      this.Control.Controls.Add((MyGuiControlBase) this.m_tbRemoveInactiveIdent);
      this.m_tbRemoveInactiveIdent.Size = new Vector2(0.07f, this.m_tbRemoveInactiveIdent.Size.Y);
      this.m_tbRemoveInactiveIdent.PositionX = (float) ((double) currentPosition1.X + (double) size.Value.X - (double) this.m_tbRemoveInactiveIdent.Size.X - 0.0450000017881393);
      this.m_tbRemoveInactiveIdent.PositionY = currentPosition1.Y - 0.01f;
      this.m_tbRemoveInactiveIdent.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenAdmin_Trash_RemoveInactiveEnt_TTIP));
      currentPosition1.Y += 0.045f;
      float usableWidth = 0.14f;
      Vector2 currentPosition2 = currentPosition1 + new Vector2(usableWidth * 0.5f, 0.0f);
      this.Control.Controls.Add((MyGuiControlBase) this.CreateDebugButton(ref currentPosition2, usableWidth, MySpaceTexts.ScreenDebugAdminMenu_RemoveFloating, new Action<MyGuiControlButton>(this.OnRemoveFloating), increaseSpacing: false, addToControls: false, isAutoScaleEnabled: true, isAutoEllipsisEnabled: true));
      float num = (float) (0.28600001335144 - 2.0 * (double) usableWidth);
      Vector2 currentPosition3 = currentPosition1 + new Vector2(usableWidth * 1.5f + num, 0.0f);
      this.Control.Controls.Add((MyGuiControlBase) this.CreateDebugButton(ref currentPosition3, usableWidth, MySpaceTexts.ScreenDebugAdminMenu_StopAll, new Action<MyGuiControlButton>(this.OnStopEntities), increaseSpacing: false, addToControls: false, isAutoScaleEnabled: true, isAutoEllipsisEnabled: true));
    }

    private void OnRemoveFloating(MyGuiControlButton obj) => MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (x => new Action(MyTrashOtherTabContainer.RemoveFloating_Implementation)));

    [Event(null, 167)]
    [Reliable]
    [Server]
    private static void RemoveFloating_Implementation()
    {
      if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
      {
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        foreach (MyEntity entity in MyEntities.GetEntities())
        {
          if (entity is MyFloatingObject || entity is MyInventoryBagEntity)
            entity.Close();
        }
      }
    }

    private void OnStopEntities(MyGuiControlButton myGuiControlButton) => MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (x => new Action(MyTrashOtherTabContainer.StopEntities_Implementation)));

    [Event(null, 187)]
    [Server]
    [Reliable]
    private static void StopEntities_Implementation()
    {
      if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
      {
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        foreach (MyEntity entity in MyEntities.GetEntities())
        {
          if (entity.Physics != null && !entity.Closed && (!(entity is MyCharacter) && MySession.Static.Players.GetEntityController(entity) == null))
            entity.Physics.ClearSpeed();
        }
      }
    }

    internal override bool GetSettings(ref MyGuiScreenAdminMenu.AdminSettings newSettings)
    {
      if (this.m_textboxCharacterRemovalTrash == null || this.m_textboxOptimalGridCount == null || (this.m_textboxAfkTimeout == null || this.m_tbStopGridsPeriod == null) || this.m_tbRemoveInactiveIdent == null)
        return false;
      int result1;
      int.TryParse(this.m_textboxCharacterRemovalTrash.Text, out result1);
      int result2;
      int.TryParse(this.m_textboxOptimalGridCount.Text, out result2);
      int result3;
      int.TryParse(this.m_textboxAfkTimeout.Text, out result3);
      int result4;
      int.TryParse(this.m_tbStopGridsPeriod.Text, out result4);
      int result5;
      int.TryParse(this.m_tbRemoveInactiveIdent.Text, out result5);
      int num = 0 | (MySession.Static.Settings.PlayerCharacterRemovalThreshold != result1 ? 1 : 0) | (MySession.Static.Settings.OptimalGridCount != result2 ? 1 : 0) | (MySession.Static.Settings.AFKTimeountMin != result3 ? 1 : 0) | (MySession.Static.Settings.StopGridsPeriodMin != result4 ? 1 : 0) | (MySession.Static.Settings.RemoveOldIdentitiesH != result5 ? 1 : 0);
      newSettings.CharacterRemovalThreshold = result1;
      newSettings.GridCount = result2;
      newSettings.AfkTimeout = result3;
      newSettings.StopGridsPeriod = result4;
      newSettings.RemoveOldIdentities = result5;
      return num != 0;
    }

    protected sealed class RemoveFloating_Implementation\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyTrashOtherTabContainer.RemoveFloating_Implementation();
      }
    }

    protected sealed class StopEntities_Implementation\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyTrashOtherTabContainer.StopEntities_Implementation();
      }
    }
  }
}
