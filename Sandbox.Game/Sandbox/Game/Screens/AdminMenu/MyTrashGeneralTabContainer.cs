// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.AdminMenu.MyTrashGeneralTabContainer
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Game;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.AdminMenu
{
  [StaticEventOwner]
  internal class MyTrashGeneralTabContainer : MyTabContainer
  {
    private MyGuiControlTextbox m_textboxBlockCount;
    private MyGuiControlTextbox m_textboxDistanceTrash;
    private MyGuiControlTextbox m_textboxLogoutAgeTrash;
    private MyGuiControlCheckbox m_checkboxFixed;
    private MyGuiControlCheckbox m_checkboxStationary;
    private MyGuiControlCheckbox m_checkboxLinear;
    private MyGuiControlCheckbox m_chkeckboxAccelerating;
    private MyGuiControlCheckbox m_checkboxPowered;
    private MyGuiControlCheckbox m_checkboxControlled;
    private MyGuiControlCheckbox m_checkboxWithProduction;
    private MyGuiControlCheckbox m_checkboxMedbay;
    private bool m_showMedbayNotification = true;

    public MyTrashGeneralTabContainer(MyGuiScreenBase parentScreen)
      : base(parentScreen)
    {
      this.Control.Size = new Vector2(this.Control.Size.X, 0.557f);
      Vector2 currentPosition1 = -this.Control.Size * 0.5f;
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      this.CreateTrashCheckBoxes(ref currentPosition1);
      Vector2? size = parentScreen.GetSize();
      currentPosition1.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
      myGuiControlLabel1.Position = currentPosition1;
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel1.Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_WithBlockCount);
      MyGuiControlLabel control1 = myGuiControlLabel1;
      control1.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_WithBlockCount_Tooltip));
      this.Control.Controls.Add((MyGuiControlBase) control1);
      this.m_textboxBlockCount = this.AddTextbox(ref currentPosition1, MySession.Static.Settings.BlockCountThreshold.ToString(), (Action<MyGuiControlTextbox>) null, new Vector4?(MyTabContainer.LABEL_COLOR), 0.9f, MyGuiControlTextboxType.DigitsOnly, addToControls: false);
      this.Control.Controls.Add((MyGuiControlBase) this.m_textboxBlockCount);
      this.m_textboxBlockCount.Size = new Vector2(0.07f, this.m_textboxBlockCount.Size.Y);
      this.m_textboxBlockCount.PositionX = (float) ((double) currentPosition1.X + (double) size.Value.X - (double) this.m_textboxBlockCount.Size.X - 0.0450000017881393);
      this.m_textboxBlockCount.PositionY = currentPosition1.Y - 0.01f;
      this.m_textboxBlockCount.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_WithBlockCount_Tooltip));
      currentPosition1.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel();
      myGuiControlLabel2.Position = currentPosition1;
      myGuiControlLabel2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel2.Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_DistanceFromPlayer);
      MyGuiControlLabel control2 = myGuiControlLabel2;
      control2.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_DistanceFromPlayer_Tooltip));
      this.Control.Controls.Add((MyGuiControlBase) control2);
      this.m_textboxDistanceTrash = this.AddTextbox(ref currentPosition1, MySession.Static.Settings.PlayerDistanceThreshold.ToString(), (Action<MyGuiControlTextbox>) null, new Vector4?(MyTabContainer.LABEL_COLOR), 0.9f, MyGuiControlTextboxType.DigitsOnly, addToControls: false);
      this.Control.Controls.Add((MyGuiControlBase) this.m_textboxDistanceTrash);
      this.m_textboxDistanceTrash.Size = new Vector2(0.07f, this.m_textboxDistanceTrash.Size.Y);
      this.m_textboxDistanceTrash.PositionX = (float) ((double) currentPosition1.X + (double) size.Value.X - (double) this.m_textboxDistanceTrash.Size.X - 0.0450000017881393);
      this.m_textboxDistanceTrash.PositionY = currentPosition1.Y - 0.01f;
      this.m_textboxDistanceTrash.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_DistanceFromPlayer_Tooltip));
      currentPosition1.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel();
      myGuiControlLabel3.Position = currentPosition1;
      myGuiControlLabel3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel3.Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_PlayerLogoutAge);
      myGuiControlLabel3.IsAutoEllipsisEnabled = true;
      myGuiControlLabel3.IsAutoScaleEnabled = true;
      MyGuiControlLabel control3 = myGuiControlLabel3;
      control3.SetMaxWidth(0.21f);
      control3.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_PlayerLogoutAge_Tooltip));
      this.Control.Controls.Add((MyGuiControlBase) control3);
      this.m_textboxLogoutAgeTrash = this.AddTextbox(ref currentPosition1, MySession.Static.Settings.PlayerInactivityThreshold.ToString(), (Action<MyGuiControlTextbox>) null, new Vector4?(MyTabContainer.LABEL_COLOR), 0.9f, MyGuiControlTextboxType.DigitsOnly, addToControls: false);
      this.Control.Controls.Add((MyGuiControlBase) this.m_textboxLogoutAgeTrash);
      this.m_textboxLogoutAgeTrash.Size = new Vector2(0.07f, this.m_textboxLogoutAgeTrash.Size.Y);
      this.m_textboxLogoutAgeTrash.PositionX = (float) ((double) currentPosition1.X + (double) size.Value.X - (double) this.m_textboxLogoutAgeTrash.Size.X - 0.0450000017881393);
      this.m_textboxLogoutAgeTrash.PositionY = currentPosition1.Y - 0.01f;
      this.m_textboxLogoutAgeTrash.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_PlayerLogoutAge_Tooltip));
      currentPosition1.Y += 0.045f;
      controlSeparatorList.AddHorizontal(currentPosition1 - new Vector2(1f / 500f, 0.0f), size.Value.X * 0.73f);
      currentPosition1.Y += 0.02f;
      float usableWidth = 0.14f;
      Vector2 currentPosition2 = currentPosition1 + new Vector2(usableWidth * 0.5f, 0.0f);
      this.Control.Controls.Add((MyGuiControlBase) this.CreateDebugButton(ref currentPosition2, usableWidth, !MySession.Static.Settings.TrashRemovalEnabled ? MyCommonTexts.ScreenDebugAdminMenu_ResumeTrashButton : MyCommonTexts.ScreenDebugAdminMenu_PauseTrashButton, new Action<MyGuiControlButton>(this.OnTrashButtonClicked), tooltip: new MyStringId?(MyCommonTexts.ScreenDebugAdminMenu_PauseTrashButtonTooltip), increaseSpacing: false, addToControls: false));
      this.Control.Controls.Add((MyGuiControlBase) controlSeparatorList);
    }

    protected virtual void CreateTrashCheckBoxes(ref Vector2 currentPosition)
    {
      MyTrashRemovalFlags flag1 = MyTrashRemovalFlags.Fixed;
      string str1 = string.Format(MySessionComponentTrash.GetName(flag1), (object) string.Empty);
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
      myGuiControlLabel1.Position = currentPosition + new Vector2(1f / 1000f, 0.0f);
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel1.Text = str1;
      MyGuiControlLabel myGuiControlLabel2 = myGuiControlLabel1;
      this.m_checkboxFixed = new MyGuiControlCheckbox(new Vector2?(new Vector2(currentPosition.X + 0.293f, currentPosition.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.m_checkboxFixed.IsChecked = (MySession.Static.Settings.TrashFlags & flag1) == flag1;
      this.m_checkboxFixed.UserData = (object) flag1;
      this.Control.Controls.Add((MyGuiControlBase) this.m_checkboxFixed);
      this.Control.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      MyTrashRemovalFlags flag2 = MyTrashRemovalFlags.Stationary;
      string str2 = string.Format(MySessionComponentTrash.GetName(flag2), (object) string.Empty);
      currentPosition.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel();
      myGuiControlLabel3.Position = currentPosition + new Vector2(1f / 1000f, 0.0f);
      myGuiControlLabel3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel3.Text = str2;
      MyGuiControlLabel myGuiControlLabel4 = myGuiControlLabel3;
      this.m_checkboxStationary = new MyGuiControlCheckbox(new Vector2?(new Vector2(currentPosition.X + 0.293f, currentPosition.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.m_checkboxStationary.IsChecked = (MySession.Static.Settings.TrashFlags & flag2) == flag2;
      this.m_checkboxStationary.UserData = (object) flag2;
      this.Control.Controls.Add((MyGuiControlBase) this.m_checkboxStationary);
      this.Control.Controls.Add((MyGuiControlBase) myGuiControlLabel4);
      MyTrashRemovalFlags flag3 = MyTrashRemovalFlags.Linear;
      string str3 = string.Format(MySessionComponentTrash.GetName(flag3), (object) string.Empty);
      currentPosition.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel5 = new MyGuiControlLabel();
      myGuiControlLabel5.Position = currentPosition + new Vector2(1f / 1000f, 0.0f);
      myGuiControlLabel5.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel5.Text = str3;
      MyGuiControlLabel myGuiControlLabel6 = myGuiControlLabel5;
      this.m_checkboxLinear = new MyGuiControlCheckbox(new Vector2?(new Vector2(currentPosition.X + 0.293f, currentPosition.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.m_checkboxLinear.IsChecked = (MySession.Static.Settings.TrashFlags & flag3) == flag3;
      this.m_checkboxLinear.UserData = (object) flag3;
      this.Control.Controls.Add((MyGuiControlBase) this.m_checkboxLinear);
      this.Control.Controls.Add((MyGuiControlBase) myGuiControlLabel6);
      MyTrashRemovalFlags flag4 = MyTrashRemovalFlags.Accelerating;
      string str4 = string.Format(MySessionComponentTrash.GetName(flag4), (object) string.Empty);
      currentPosition.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel7 = new MyGuiControlLabel();
      myGuiControlLabel7.Position = currentPosition + new Vector2(1f / 1000f, 0.0f);
      myGuiControlLabel7.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel7.Text = str4;
      MyGuiControlLabel myGuiControlLabel8 = myGuiControlLabel7;
      this.m_chkeckboxAccelerating = new MyGuiControlCheckbox(new Vector2?(new Vector2(currentPosition.X + 0.293f, currentPosition.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.m_chkeckboxAccelerating.IsChecked = (MySession.Static.Settings.TrashFlags & flag4) == flag4;
      this.m_chkeckboxAccelerating.UserData = (object) flag4;
      this.Control.Controls.Add((MyGuiControlBase) this.m_chkeckboxAccelerating);
      this.Control.Controls.Add((MyGuiControlBase) myGuiControlLabel8);
      MyTrashRemovalFlags flag5 = MyTrashRemovalFlags.Powered;
      string str5 = string.Format(MySessionComponentTrash.GetName(flag5), (object) string.Empty);
      currentPosition.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel9 = new MyGuiControlLabel();
      myGuiControlLabel9.Position = currentPosition + new Vector2(1f / 1000f, 0.0f);
      myGuiControlLabel9.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel9.Text = str5;
      MyGuiControlLabel myGuiControlLabel10 = myGuiControlLabel9;
      this.m_checkboxPowered = new MyGuiControlCheckbox(new Vector2?(new Vector2(currentPosition.X + 0.293f, currentPosition.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.m_checkboxPowered.IsChecked = (MySession.Static.Settings.TrashFlags & flag5) == flag5;
      this.m_checkboxPowered.UserData = (object) flag5;
      this.Control.Controls.Add((MyGuiControlBase) this.m_checkboxPowered);
      this.Control.Controls.Add((MyGuiControlBase) myGuiControlLabel10);
      MyTrashRemovalFlags flag6 = MyTrashRemovalFlags.Controlled;
      string str6 = string.Format(MySessionComponentTrash.GetName(flag6), (object) string.Empty);
      currentPosition.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel11 = new MyGuiControlLabel();
      myGuiControlLabel11.Position = currentPosition + new Vector2(1f / 1000f, 0.0f);
      myGuiControlLabel11.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel11.Text = str6;
      MyGuiControlLabel myGuiControlLabel12 = myGuiControlLabel11;
      this.m_checkboxControlled = new MyGuiControlCheckbox(new Vector2?(new Vector2(currentPosition.X + 0.293f, currentPosition.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.m_checkboxControlled.IsChecked = (MySession.Static.Settings.TrashFlags & flag6) == flag6;
      this.m_checkboxControlled.UserData = (object) flag6;
      this.Control.Controls.Add((MyGuiControlBase) this.m_checkboxControlled);
      this.Control.Controls.Add((MyGuiControlBase) myGuiControlLabel12);
      MyTrashRemovalFlags flag7 = MyTrashRemovalFlags.WithProduction;
      string str7 = string.Format(MySessionComponentTrash.GetName(flag7), (object) string.Empty);
      currentPosition.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel13 = new MyGuiControlLabel();
      myGuiControlLabel13.Position = currentPosition + new Vector2(1f / 1000f, 0.0f);
      myGuiControlLabel13.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel13.Text = str7;
      MyGuiControlLabel myGuiControlLabel14 = myGuiControlLabel13;
      this.m_checkboxWithProduction = new MyGuiControlCheckbox(new Vector2?(new Vector2(currentPosition.X + 0.293f, currentPosition.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.m_checkboxWithProduction.IsChecked = (MySession.Static.Settings.TrashFlags & flag7) == flag7;
      this.m_checkboxWithProduction.UserData = (object) flag7;
      this.Control.Controls.Add((MyGuiControlBase) this.m_checkboxWithProduction);
      this.Control.Controls.Add((MyGuiControlBase) myGuiControlLabel14);
      MyTrashRemovalFlags flag8 = MyTrashRemovalFlags.WithMedBay;
      string str8 = string.Format(MySessionComponentTrash.GetName(flag8), (object) string.Empty);
      currentPosition.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel15 = new MyGuiControlLabel();
      myGuiControlLabel15.Position = currentPosition + new Vector2(1f / 1000f, 0.0f);
      myGuiControlLabel15.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel15.Text = str8;
      MyGuiControlLabel myGuiControlLabel16 = myGuiControlLabel15;
      this.m_checkboxMedbay = new MyGuiControlCheckbox(new Vector2?(new Vector2(currentPosition.X + 0.293f, currentPosition.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.m_checkboxMedbay.IsChecked = (MySession.Static.Settings.TrashFlags & flag8) == flag8;
      this.m_checkboxMedbay.UserData = (object) flag8;
      this.m_checkboxMedbay.IsCheckedChanged = (Action<MyGuiControlCheckbox>) (c => this.OnTrashFlagChanged(c.IsChecked));
      this.Control.Controls.Add((MyGuiControlBase) this.m_checkboxMedbay);
      this.Control.Controls.Add((MyGuiControlBase) myGuiControlLabel16);
    }

    private void OnTrashButtonClicked(MyGuiControlButton obj)
    {
      MySession.Static.Settings.TrashRemovalEnabled = !MySession.Static.Settings.TrashRemovalEnabled;
      obj.Text = MySession.Static.Settings.TrashRemovalEnabled ? MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_PauseTrashButton) : MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_ResumeTrashButton);
      MyGuiScreenAdminMenu.RecalcTrash();
    }

    private void OnTrashFlagChanged(bool value)
    {
      if (!value || !this.m_showMedbayNotification)
        return;
      MyScreenManager.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_MedbayNotification)));
      this.m_showMedbayNotification = false;
    }

    internal override bool GetSettings(ref MyGuiScreenAdminMenu.AdminSettings newSettings)
    {
      if (this.m_textboxBlockCount == null || this.m_textboxDistanceTrash == null || this.m_textboxLogoutAgeTrash == null)
        return false;
      int result1;
      int.TryParse(this.m_textboxBlockCount.Text, out result1);
      float result2;
      float.TryParse(this.m_textboxDistanceTrash.Text, out result2);
      float result3;
      float.TryParse(this.m_textboxLogoutAgeTrash.Text, out result3);
      int num1 = 0 | (MySession.Static.Settings.BlockCountThreshold != result1 ? 1 : 0) | ((double) MySession.Static.Settings.PlayerDistanceThreshold != (double) result2 ? 1 : 0) | ((double) MySession.Static.Settings.PlayerInactivityThreshold != (double) result3 ? 1 : 0);
      newSettings.BlockCount = result1;
      newSettings.PlayerDistance = result2;
      newSettings.PlayerInactivity = result3;
      newSettings.Enable = MySession.Static.Settings.TrashRemovalEnabled;
      newSettings.Flags = this.m_checkboxFixed.IsChecked ? newSettings.Flags | (MyTrashRemovalFlags) this.m_checkboxFixed.UserData : newSettings.Flags & ~(MyTrashRemovalFlags) this.m_checkboxFixed.UserData;
      newSettings.Flags = this.m_checkboxLinear.IsChecked ? newSettings.Flags | (MyTrashRemovalFlags) this.m_checkboxLinear.UserData : newSettings.Flags & ~(MyTrashRemovalFlags) this.m_checkboxLinear.UserData;
      newSettings.Flags = this.m_checkboxMedbay.IsChecked ? newSettings.Flags | (MyTrashRemovalFlags) this.m_checkboxMedbay.UserData : newSettings.Flags & ~(MyTrashRemovalFlags) this.m_checkboxMedbay.UserData;
      newSettings.Flags = this.m_checkboxPowered.IsChecked ? newSettings.Flags | (MyTrashRemovalFlags) this.m_checkboxPowered.UserData : newSettings.Flags & ~(MyTrashRemovalFlags) this.m_checkboxPowered.UserData;
      newSettings.Flags = this.m_checkboxStationary.IsChecked ? newSettings.Flags | (MyTrashRemovalFlags) this.m_checkboxStationary.UserData : newSettings.Flags & ~(MyTrashRemovalFlags) this.m_checkboxStationary.UserData;
      newSettings.Flags = this.m_checkboxWithProduction.IsChecked ? newSettings.Flags | (MyTrashRemovalFlags) this.m_checkboxWithProduction.UserData : newSettings.Flags & ~(MyTrashRemovalFlags) this.m_checkboxWithProduction.UserData;
      newSettings.Flags = this.m_checkboxControlled.IsChecked ? newSettings.Flags | (MyTrashRemovalFlags) this.m_checkboxControlled.UserData : newSettings.Flags & ~(MyTrashRemovalFlags) this.m_checkboxControlled.UserData;
      newSettings.Flags = this.m_chkeckboxAccelerating.IsChecked ? newSettings.Flags | (MyTrashRemovalFlags) this.m_chkeckboxAccelerating.UserData : newSettings.Flags & ~(MyTrashRemovalFlags) this.m_chkeckboxAccelerating.UserData;
      int num2 = MySession.Static.Settings.TrashFlags != newSettings.Flags ? 1 : 0;
      return (num1 | num2) != 0;
    }
  }
}
