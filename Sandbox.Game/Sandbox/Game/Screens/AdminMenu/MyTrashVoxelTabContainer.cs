// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.AdminMenu.MyTrashVoxelTabContainer
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
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.AdminMenu
{
  internal class MyTrashVoxelTabContainer : MyTabContainer
  {
    private MyGuiControlTextbox m_textboxVoxelPlayerDistanceTrash;
    private MyGuiControlTextbox m_textboxVoxelGridDistanceTrash;
    private MyGuiControlTextbox m_textboxVoxelAgeTrash;
    private MyGuiControlCheckbox m_checkboxRevertMaterials;
    private MyGuiControlCheckbox m_checkboxRevertAsteroids;
    private MyGuiControlCheckbox m_checkboxRevertBoulders;
    private MyGuiControlCheckbox m_checkboxRevertFloatingPreset;
    private MyGuiControlCheckbox m_checkboxIgnoreNPCGrids;

    public MyTrashVoxelTabContainer(MyGuiScreenBase parentScreen)
      : base(parentScreen)
    {
      this.Control.Size = new Vector2(this.Control.Size.X, 0.4f);
      Vector2 currentPosition1 = -this.Control.Size * 0.5f;
      Vector2? size = parentScreen.GetSize();
      this.CreateVoxelTrashCheckBoxes(ref currentPosition1);
      currentPosition1.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
      myGuiControlLabel1.Position = currentPosition1;
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel1.Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_VoxelDistanceFromPlayer);
      MyGuiControlLabel control1 = myGuiControlLabel1;
      control1.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_VoxelDistanceFromPlayer_Tooltip));
      this.Control.Controls.Add((MyGuiControlBase) control1);
      this.m_textboxVoxelPlayerDistanceTrash = this.AddTextbox(ref currentPosition1, MySession.Static.Settings.VoxelPlayerDistanceThreshold.ToString(), (Action<MyGuiControlTextbox>) null, new Vector4?(MyTabContainer.LABEL_COLOR), 0.9f, MyGuiControlTextboxType.DigitsOnly, addToControls: false);
      this.Control.Controls.Add((MyGuiControlBase) this.m_textboxVoxelPlayerDistanceTrash);
      this.m_textboxVoxelPlayerDistanceTrash.Size = new Vector2(0.07f, this.m_textboxVoxelPlayerDistanceTrash.Size.Y);
      this.m_textboxVoxelPlayerDistanceTrash.PositionX = (float) ((double) currentPosition1.X + (double) size.Value.X - (double) this.m_textboxVoxelPlayerDistanceTrash.Size.X - 0.0450000017881393);
      this.m_textboxVoxelPlayerDistanceTrash.PositionY = currentPosition1.Y;
      this.m_textboxVoxelPlayerDistanceTrash.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_VoxelDistanceFromPlayer_Tooltip));
      currentPosition1.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel();
      myGuiControlLabel2.Position = currentPosition1;
      myGuiControlLabel2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel2.Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_VoxelDistanceFromGrid);
      MyGuiControlLabel control2 = myGuiControlLabel2;
      control2.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_VoxelDistanceFromGrid_Tooltip));
      this.Control.Controls.Add((MyGuiControlBase) control2);
      this.m_textboxVoxelGridDistanceTrash = this.AddTextbox(ref currentPosition1, MySession.Static.Settings.VoxelGridDistanceThreshold.ToString(), (Action<MyGuiControlTextbox>) null, new Vector4?(MyTabContainer.LABEL_COLOR), 0.9f, MyGuiControlTextboxType.DigitsOnly, addToControls: false);
      this.Control.Controls.Add((MyGuiControlBase) this.m_textboxVoxelGridDistanceTrash);
      this.m_textboxVoxelGridDistanceTrash.Size = new Vector2(0.07f, this.m_textboxVoxelGridDistanceTrash.Size.Y);
      this.m_textboxVoxelGridDistanceTrash.PositionX = (float) ((double) currentPosition1.X + (double) size.Value.X - (double) this.m_textboxVoxelGridDistanceTrash.Size.X - 0.0450000017881393);
      this.m_textboxVoxelGridDistanceTrash.PositionY = currentPosition1.Y;
      this.m_textboxVoxelGridDistanceTrash.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_VoxelDistanceFromGrid_Tooltip));
      currentPosition1.Y += 0.045f;
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel();
      myGuiControlLabel3.Position = currentPosition1;
      myGuiControlLabel3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel3.Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_VoxelAge);
      MyGuiControlLabel control3 = myGuiControlLabel3;
      control3.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_VoxelAge_Tooltip));
      this.Control.Controls.Add((MyGuiControlBase) control3);
      this.m_textboxVoxelAgeTrash = this.AddTextbox(ref currentPosition1, MySession.Static.Settings.VoxelAgeThreshold.ToString(), (Action<MyGuiControlTextbox>) null, new Vector4?(MyTabContainer.LABEL_COLOR), 0.9f, MyGuiControlTextboxType.DigitsOnly, addToControls: false);
      this.Control.Controls.Add((MyGuiControlBase) this.m_textboxVoxelAgeTrash);
      this.m_textboxVoxelAgeTrash.Size = new Vector2(0.07f, this.m_textboxVoxelAgeTrash.Size.Y);
      this.m_textboxVoxelAgeTrash.PositionX = (float) ((double) currentPosition1.X + (double) size.Value.X - (double) this.m_textboxVoxelAgeTrash.Size.X - 0.0450000017881393);
      this.m_textboxVoxelAgeTrash.PositionY = currentPosition1.Y;
      this.m_textboxVoxelAgeTrash.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_VoxelAge_Tooltip));
      currentPosition1.Y += 0.045f;
      float usableWidth = 0.14f;
      Vector2 currentPosition2 = currentPosition1 + new Vector2(usableWidth * 0.5f, 0.0f);
      this.Control.Controls.Add((MyGuiControlBase) this.CreateDebugButton(ref currentPosition2, usableWidth, !MySession.Static.Settings.VoxelTrashRemovalEnabled ? MyCommonTexts.ScreenDebugAdminMenu_ResumeTrashButton : MyCommonTexts.ScreenDebugAdminMenu_PauseTrashButton, new Action<MyGuiControlButton>(this.OnTrashVoxelButtonClicked), tooltip: new MyStringId?(MyCommonTexts.ScreenDebugAdminMenu_PauseTrashVoxelButtonTooltip), increaseSpacing: false, addToControls: false));
    }

    protected virtual void CreateVoxelTrashCheckBoxes(ref Vector2 currentPosition)
    {
      MyTrashRemovalFlags flag1 = MyTrashRemovalFlags.RevertMaterials;
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
      myGuiControlLabel1.Position = currentPosition + new Vector2(1f / 1000f, 0.0f);
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel1.Text = MySessionComponentTrash.GetName(flag1);
      MyGuiControlLabel myGuiControlLabel2 = myGuiControlLabel1;
      this.m_checkboxRevertMaterials = new MyGuiControlCheckbox(new Vector2?(new Vector2(currentPosition.X + 0.293f, currentPosition.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.m_checkboxRevertMaterials.IsChecked = (MySession.Static.Settings.TrashFlags & flag1) == flag1;
      this.m_checkboxRevertMaterials.UserData = (object) flag1;
      this.Control.Controls.Add((MyGuiControlBase) this.m_checkboxRevertMaterials);
      this.Control.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      currentPosition.Y += 0.045f;
      MyTrashRemovalFlags flag2 = MyTrashRemovalFlags.RevertAsteroids;
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel();
      myGuiControlLabel3.Position = currentPosition + new Vector2(1f / 1000f, 0.0f);
      myGuiControlLabel3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel3.Text = MySessionComponentTrash.GetName(flag2);
      MyGuiControlLabel myGuiControlLabel4 = myGuiControlLabel3;
      this.m_checkboxRevertAsteroids = new MyGuiControlCheckbox(new Vector2?(new Vector2(currentPosition.X + 0.293f, currentPosition.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.m_checkboxRevertAsteroids.IsChecked = (MySession.Static.Settings.TrashFlags & flag2) == flag2;
      this.m_checkboxRevertAsteroids.UserData = (object) flag2;
      this.Control.Controls.Add((MyGuiControlBase) this.m_checkboxRevertAsteroids);
      this.Control.Controls.Add((MyGuiControlBase) myGuiControlLabel4);
      currentPosition.Y += 0.045f;
      MyTrashRemovalFlags flag3 = MyTrashRemovalFlags.RevertBoulders;
      MyGuiControlLabel myGuiControlLabel5 = new MyGuiControlLabel();
      myGuiControlLabel5.Position = currentPosition + new Vector2(1f / 1000f, 0.0f);
      myGuiControlLabel5.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel5.Text = MySessionComponentTrash.GetName(flag3);
      MyGuiControlLabel myGuiControlLabel6 = myGuiControlLabel5;
      this.m_checkboxRevertBoulders = new MyGuiControlCheckbox(new Vector2?(new Vector2(currentPosition.X + 0.293f, currentPosition.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.m_checkboxRevertBoulders.IsChecked = (MySession.Static.Settings.TrashFlags & flag3) == flag3;
      this.m_checkboxRevertBoulders.UserData = (object) flag3;
      this.m_checkboxRevertBoulders.IsCheckedChanged += new Action<MyGuiControlCheckbox>(this.RevertBoulderCheckboxChanged);
      this.m_checkboxRevertBoulders.SetTooltip(string.Format(MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_RevertBoulderTooltip), (object) MyGuiScreenAdminMenu.BOULDER_REVERT_MINIMUM_PLAYER_DISTANCE));
      this.Control.Controls.Add((MyGuiControlBase) this.m_checkboxRevertBoulders);
      this.Control.Controls.Add((MyGuiControlBase) myGuiControlLabel6);
      currentPosition.Y += 0.045f;
      MyTrashRemovalFlags flag4 = MyTrashRemovalFlags.RevertWithFloatingsPresent;
      MyGuiControlLabel myGuiControlLabel7 = new MyGuiControlLabel();
      myGuiControlLabel7.Position = currentPosition + new Vector2(1f / 1000f, 0.0f);
      myGuiControlLabel7.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel7.Text = MySessionComponentTrash.GetName(flag4);
      myGuiControlLabel7.IsAutoScaleEnabled = true;
      myGuiControlLabel7.IsAutoEllipsisEnabled = true;
      MyGuiControlLabel myGuiControlLabel8 = myGuiControlLabel7;
      myGuiControlLabel8.SetMaxWidth(0.26f);
      myGuiControlLabel8.DoEllipsisAndScaleAdjust(true, myGuiControlLabel8.TextScale, true);
      this.m_checkboxRevertFloatingPreset = new MyGuiControlCheckbox(new Vector2?(new Vector2(currentPosition.X + 0.293f, currentPosition.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.m_checkboxRevertFloatingPreset.IsChecked = (MySession.Static.Settings.TrashFlags & flag4) == flag4;
      this.m_checkboxRevertFloatingPreset.UserData = (object) flag4;
      this.Control.Controls.Add((MyGuiControlBase) this.m_checkboxRevertFloatingPreset);
      this.Control.Controls.Add((MyGuiControlBase) myGuiControlLabel8);
      currentPosition.Y += 0.045f;
      MyTrashRemovalFlags flag5 = MyTrashRemovalFlags.RevertCloseToNPCGrids;
      MyGuiControlLabel myGuiControlLabel9 = new MyGuiControlLabel();
      myGuiControlLabel9.Position = currentPosition + new Vector2(1f / 1000f, 0.0f);
      myGuiControlLabel9.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel9.Text = MySessionComponentTrash.GetName(flag5);
      MyGuiControlLabel myGuiControlLabel10 = myGuiControlLabel9;
      this.m_checkboxIgnoreNPCGrids = new MyGuiControlCheckbox(new Vector2?(new Vector2(currentPosition.X + 0.293f, currentPosition.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.m_checkboxIgnoreNPCGrids.IsChecked = (MySession.Static.Settings.TrashFlags & flag5) == flag5;
      this.m_checkboxIgnoreNPCGrids.UserData = (object) flag5;
      this.Control.Controls.Add((MyGuiControlBase) this.m_checkboxIgnoreNPCGrids);
      this.Control.Controls.Add((MyGuiControlBase) myGuiControlLabel10);
    }

    private void RevertBoulderCheckboxChanged(MyGuiControlCheckbox obj)
    {
      if (!obj.IsChecked)
        return;
      this.PlayerDistanceTextChanged(this.m_textboxVoxelAgeTrash);
    }

    private void PlayerDistanceTextChanged(MyGuiControlTextbox obj)
    {
      if (!this.m_checkboxRevertBoulders.IsChecked)
        return;
      int result;
      int.TryParse(this.m_textboxVoxelPlayerDistanceTrash.Text, out result);
      if (result >= MyGuiScreenAdminMenu.BOULDER_REVERT_MINIMUM_PLAYER_DISTANCE)
        return;
      this.m_textboxVoxelPlayerDistanceTrash.Text = MyGuiScreenAdminMenu.BOULDER_REVERT_MINIMUM_PLAYER_DISTANCE.ToString();
    }

    private void OnTrashVoxelButtonClicked(MyGuiControlButton obj)
    {
      MySession.Static.Settings.VoxelTrashRemovalEnabled = !MySession.Static.Settings.VoxelTrashRemovalEnabled;
      obj.Text = MySession.Static.Settings.VoxelTrashRemovalEnabled ? MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_PauseTrashButton) : MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_ResumeTrashButton);
      MyGuiScreenAdminMenu.RecalcTrash();
    }

    internal override bool GetSettings(ref MyGuiScreenAdminMenu.AdminSettings settings)
    {
      if (this.m_textboxVoxelPlayerDistanceTrash == null || this.m_textboxVoxelGridDistanceTrash == null || this.m_textboxVoxelAgeTrash == null)
        return false;
      float result1;
      float.TryParse(this.m_textboxVoxelPlayerDistanceTrash.Text, out result1);
      if (this.m_checkboxRevertBoulders.IsChecked && (double) result1 < (double) MyGuiScreenAdminMenu.BOULDER_REVERT_MINIMUM_PLAYER_DISTANCE)
      {
        result1 = (float) MyGuiScreenAdminMenu.BOULDER_REVERT_MINIMUM_PLAYER_DISTANCE;
        this.m_textboxVoxelPlayerDistanceTrash.Text = MyGuiScreenAdminMenu.BOULDER_REVERT_MINIMUM_PLAYER_DISTANCE.ToString();
      }
      float result2;
      float.TryParse(this.m_textboxVoxelGridDistanceTrash.Text, out result2);
      int result3;
      int.TryParse(this.m_textboxVoxelAgeTrash.Text, out result3);
      int num1 = 0 | ((double) MySession.Static.Settings.VoxelPlayerDistanceThreshold != (double) result1 ? 1 : 0) | ((double) MySession.Static.Settings.VoxelGridDistanceThreshold != (double) result2 ? 1 : 0) | (MySession.Static.Settings.VoxelAgeThreshold != result3 ? 1 : 0);
      settings.VoxelDistanceFromPlayer = result1;
      settings.VoxelDistanceFromGrid = result2;
      settings.VoxelAge = result3;
      settings.VoxelEnable = MySession.Static.Settings.VoxelTrashRemovalEnabled;
      settings.Flags = this.m_checkboxRevertMaterials.IsChecked ? settings.Flags | (MyTrashRemovalFlags) this.m_checkboxRevertMaterials.UserData : settings.Flags & ~(MyTrashRemovalFlags) this.m_checkboxRevertMaterials.UserData;
      settings.Flags = this.m_checkboxRevertAsteroids.IsChecked ? settings.Flags | (MyTrashRemovalFlags) this.m_checkboxRevertAsteroids.UserData : settings.Flags & ~(MyTrashRemovalFlags) this.m_checkboxRevertAsteroids.UserData;
      settings.Flags = this.m_checkboxRevertBoulders.IsChecked ? settings.Flags | (MyTrashRemovalFlags) this.m_checkboxRevertBoulders.UserData : settings.Flags & ~(MyTrashRemovalFlags) this.m_checkboxRevertBoulders.UserData;
      settings.Flags = this.m_checkboxRevertFloatingPreset.IsChecked ? settings.Flags | (MyTrashRemovalFlags) this.m_checkboxRevertFloatingPreset.UserData : settings.Flags & ~(MyTrashRemovalFlags) this.m_checkboxRevertFloatingPreset.UserData;
      settings.Flags = this.m_checkboxIgnoreNPCGrids.IsChecked ? settings.Flags | (MyTrashRemovalFlags) this.m_checkboxIgnoreNPCGrids.UserData : settings.Flags & ~(MyTrashRemovalFlags) this.m_checkboxIgnoreNPCGrids.UserData;
      int num2 = MySession.Static.Settings.TrashFlags != settings.Flags ? 1 : 0;
      return (num1 | num2) != 0;
    }
  }
}
