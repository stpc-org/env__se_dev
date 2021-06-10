// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiFolderScreen
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  internal class MyGuiFolderScreen : MyGuiScreenBase
  {
    private static readonly Vector2 SCREEN_SIZE = new Vector2(0.4f, 0.6f);
    private Action<bool, string> m_onFinishedAction;
    private bool m_visibleFolderSelect;
    private Func<string, bool> m_isItem;
    private string m_rootPath;
    private string m_pathLocalInitial;
    private string m_pathLocalCurrent;
    private MyGuiControlLabel m_pathLabel;
    private MyGuiControlListbox m_fileList;
    private MyGuiControlButton m_buttonOk;
    private MyGuiControlButton m_buttonRefresh;
    private MyGuiControlImage m_refreshButtonIcon;

    public MyGuiFolderScreen(
      bool hideOthers,
      Action<bool, string> OnFinished,
      string rootPath,
      string localPath,
      Func<string, bool> isItem = null,
      bool visibleFolderSelect = false)
    {
      Vector2? position = new Vector2?(new Vector2(0.5f, 0.5f));
      Vector2? nullable = new Vector2?(MyGuiFolderScreen.SCREEN_SIZE);
      Vector4? backgroundColor = new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR * MySandboxGame.Config.UIBkOpacity);
      Vector2? size = nullable;
      int? gamepadSlot = new int?();
      // ISSUE: explicit constructor call
      base.\u002Ector(position, backgroundColor, size, gamepadSlot: gamepadSlot);
      this.m_visibleFolderSelect = visibleFolderSelect;
      if (OnFinished == null)
        this.CloseScreen();
      this.CanHideOthers = hideOthers;
      this.m_onFinishedAction = OnFinished;
      this.m_rootPath = rootPath;
      this.m_pathLocalCurrent = this.m_pathLocalInitial = localPath;
      this.m_isItem = isItem == null ? new Func<string, bool>(MyGuiFolderScreen.IsItem_Default) : isItem;
      this.RecreateControls(true);
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      Vector2 vector2_1 = new Vector2(0.0f, 0.23f);
      Vector2 vector2_2 = new Vector2(0.15f, 0.23f);
      Vector2 vector2_3 = new Vector2(0.0f, 0.02f);
      Vector2 vector2_4 = new Vector2(-0.143f, -0.2f);
      Vector2 size1 = new Vector2(0.143f, 0.035f);
      Vector2 size2 = new Vector2(0.026f, 0.035f);
      Vector2 vector2_5 = new Vector2(0.32f, 0.38f);
      Vector2 vector2_6 = new Vector2(0.5f, 0.5f);
      this.m_fileList = new MyGuiControlListbox(visualStyle: MyGuiControlListboxStyleEnum.Blueprints);
      this.m_fileList.Position = vector2_3;
      this.m_fileList.Size = vector2_5;
      this.m_fileList.ItemDoubleClicked += new Action<MyGuiControlListbox>(this.OnItemDoubleClick);
      this.m_fileList.ItemClicked += new Action<MyGuiControlListbox>(this.OnItemClick);
      this.m_fileList.VisibleRowsCount = 11;
      this.Controls.Add((MyGuiControlBase) this.m_fileList);
      this.AddCaption(MySpaceTexts.ScreenFolders_Caption);
      this.m_buttonOk = this.CreateButton(size1, MyTexts.Get(MySpaceTexts.ScreenFolders_ButOpen), new Action<MyGuiControlButton>(this.OnOk), tooltip: new MyStringId?(MySpaceTexts.ScreenFolders_Tooltip_Open));
      this.m_buttonOk.Position = vector2_1;
      this.m_buttonOk.ShowTooltipWhenDisabled = true;
      this.m_buttonRefresh = this.CreateButton(size2, (StringBuilder) null, new Action<MyGuiControlButton>(this.OnRefresh), tooltip: new MyStringId?(MySpaceTexts.ScreenFolders_Tooltip_Refresh));
      this.m_buttonRefresh.Position = vector2_2;
      this.m_buttonRefresh.ShowTooltipWhenDisabled = true;
      this.m_pathLabel = new MyGuiControlLabel(new Vector2?(vector2_4), new Vector2?(vector2_6));
      this.Controls.Add((MyGuiControlBase) this.m_pathLabel);
      this.UpdatePathLabel();
      this.m_refreshButtonIcon = this.CreateButtonIcon(this.m_buttonRefresh, "Textures\\GUI\\Icons\\Blueprints\\Refresh.png");
      this.CloseButtonEnabled = true;
      this.RepopulateList();
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(new Vector2(this.m_fileList.PositionX, this.m_buttonOk.PositionY + MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui.Y / 2f)));
      myGuiControlLabel.OriginAlign = this.m_fileList.OriginAlign;
      myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.FolderScreen_Help_Screen);
    }

    public void UpdatePathLabel()
    {
      string str = "./" + this.BuildNewPath();
      if (str.Length > 40)
        this.m_pathLabel.Text = str.Substring(str.Length - 41, 40);
      else
        this.m_pathLabel.Text = str;
    }

    private static bool IsItem_Default(string path) => false;

    private void RepopulateList()
    {
      this.m_fileList.Items.Clear();
      List<MyGuiControlListbox.Item> objList1 = new List<MyGuiControlListbox.Item>();
      List<MyGuiControlListbox.Item> objList2 = new List<MyGuiControlListbox.Item>();
      string path = Path.Combine(this.m_rootPath, this.m_pathLocalCurrent);
      if (!Directory.Exists(path))
        return;
      string[] directories = Directory.GetDirectories(path);
      List<string> stringList = new List<string>();
      foreach (string str in directories)
      {
        char[] chArray = new char[1]{ '\\' };
        string[] strArray = str.Split(chArray);
        stringList.Add(strArray[strArray.Length - 1]);
      }
      MyGuiHighlightTexture highlightTexture;
      for (int index = 0; index < stringList.Count; ++index)
      {
        if (this.m_isItem(directories[index]))
        {
          MyFileItem myFileItem = new MyFileItem()
          {
            Type = MyFileItemType.File,
            Name = stringList[index],
            Path = directories[index]
          };
          highlightTexture = MyGuiConstants.TEXTURE_ICON_BLUEPRINTS_FOLDER;
          string normal = highlightTexture.Normal;
          StringBuilder text = new StringBuilder(stringList[index]);
          string toolTip = directories[index];
          object obj1 = (object) myFileItem;
          string icon = normal;
          object userData = obj1;
          MyGuiControlListbox.Item obj2 = new MyGuiControlListbox.Item(text, toolTip, icon, userData);
          objList2.Add(obj2);
        }
        else
        {
          MyFileItem myFileItem = new MyFileItem()
          {
            Type = MyFileItemType.Directory,
            Name = stringList[index],
            Path = directories[index]
          };
          highlightTexture = MyGuiConstants.TEXTURE_ICON_MODS_LOCAL;
          string normal = highlightTexture.Normal;
          StringBuilder text = new StringBuilder(stringList[index]);
          string toolTip = directories[index];
          object obj1 = (object) myFileItem;
          string icon = normal;
          object userData = obj1;
          MyGuiControlListbox.Item obj2 = new MyGuiControlListbox.Item(text, toolTip, icon, userData);
          objList1.Add(obj2);
        }
      }
      if (!string.IsNullOrEmpty(this.m_pathLocalCurrent))
      {
        MyFileItem myFileItem = new MyFileItem()
        {
          Type = MyFileItemType.Directory,
          Name = string.Empty,
          Path = string.Empty
        };
        StringBuilder text = new StringBuilder("[..]");
        string pathLocalCurrent = this.m_pathLocalCurrent;
        object obj = (object) myFileItem;
        highlightTexture = MyGuiConstants.TEXTURE_ICON_MODS_LOCAL;
        string normal = highlightTexture.Normal;
        object userData = obj;
        this.m_fileList.Add(new MyGuiControlListbox.Item(text, pathLocalCurrent, normal, userData));
      }
      foreach (MyGuiControlListbox.Item obj in objList1)
        this.m_fileList.Add(obj);
      foreach (MyGuiControlListbox.Item obj in objList2)
        this.m_fileList.Add(obj);
      this.UpdatePathLabel();
      this.m_fileList.SelectedItems.Clear();
    }

    protected MyGuiControlButton CreateButton(
      Vector2 size,
      StringBuilder text,
      Action<MyGuiControlButton> onClick,
      bool enabled = true,
      MyStringId? tooltip = null,
      float textScale = 1f)
    {
      MyGuiControlButton guiControlButton = new MyGuiControlButton(visualStyle: MyGuiControlButtonStyleEnum.Rectangular, text: text, textScale: (0.8f * MyGuiConstants.DEBUG_BUTTON_TEXT_SCALE), onButtonClick: onClick);
      guiControlButton.TextScale = textScale;
      guiControlButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
      guiControlButton.Size = size;
      if (tooltip.HasValue)
        guiControlButton.SetToolTip(tooltip.Value);
      this.Controls.Add((MyGuiControlBase) guiControlButton);
      return guiControlButton;
    }

    private MyGuiControlImage CreateButtonIcon(
      MyGuiControlButton butt,
      string texture)
    {
      float y = 0.95f * Math.Min(butt.Size.X, butt.Size.Y);
      Vector2? size = new Vector2?(new Vector2(y * 0.75f, y));
      MyGuiControlImage icon = new MyGuiControlImage(new Vector2?(butt.Position + new Vector2(-1f / 625f, 0.015f)), size, textures: new string[1]
      {
        texture
      });
      this.Controls.Add((MyGuiControlBase) icon);
      butt.HighlightChanged += (Action<MyGuiControlBase>) (x => icon.ColorMask = x.HasHighlight ? MyGuiConstants.HIGHLIGHT_TEXT_COLOR : Vector4.One);
      return icon;
    }

    public string BuildNewPath(bool selectVisible = false)
    {
      if (this.m_fileList.SelectedItems.Count != 1 | selectVisible)
        return this.m_pathLocalCurrent;
      MyFileItem userData = (MyFileItem) this.m_fileList.SelectedItems[0].UserData;
      if (userData.Type != MyFileItemType.Directory)
        return this.m_pathLocalCurrent;
      string str;
      if (string.IsNullOrEmpty(userData.Path))
      {
        string[] strArray = this.m_pathLocalCurrent.Split(Path.DirectorySeparatorChar);
        if (strArray.Length > 1)
        {
          strArray[strArray.Length - 1] = string.Empty;
          str = Path.Combine(strArray);
        }
        else
          str = string.Empty;
      }
      else
        str = Path.Combine(this.m_pathLocalCurrent, userData.Name);
      return str;
    }

    private void OnItemClick(MyGuiControlListbox list)
    {
      if (list.SelectedItems == null || list.SelectedItems.Count == 0)
        return;
      MyGuiControlListbox.Item selectedItem = list.SelectedItems[0];
      this.UpdatePathLabel();
    }

    private void OnItemDoubleClick(MyGuiControlListbox list)
    {
      if (list.SelectedItems.Count <= 0)
        return;
      MyGuiControlListbox.Item selectedItem = list.SelectedItems[0];
      this.m_pathLocalCurrent = this.BuildNewPath();
      this.RepopulateList();
    }

    private void OnRefresh(MyGuiControlButton button) => this.RecreateControls(false);

    private void OnOk(MyGuiControlButton button)
    {
      this.m_onFinishedAction(true, this.BuildNewPath(this.m_visibleFolderSelect));
      this.CloseScreen();
    }

    public override string GetFriendlyName() => nameof (MyGuiFolderScreen);

    public override bool Update(bool hasFocus)
    {
      this.m_buttonOk.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_buttonRefresh.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_refreshButtonIcon.Visible = !MyInput.Static.IsJoystickLastUsed;
      return base.Update(hasFocus);
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      base.HandleInput(receivedFocusInThisUpdate);
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
        this.OnOk((MyGuiControlButton) null);
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y))
        this.OnRefresh((MyGuiControlButton) null);
      if (!MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACCEPT))
        return;
      this.OnItemDoubleClick(this.m_fileList);
    }
  }
}
