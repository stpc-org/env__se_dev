// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiDetailScreenScriptLocal
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Graphics.GUI;
using System;
using System.IO;
using System.Text;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.GameServices;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public class MyGuiDetailScreenScriptLocal : MyGuiBlueprintScreenBase
  {
    public bool WasPublished;
    protected MyGuiControlMultilineText m_textField;
    protected float m_textScale;
    protected Vector2 m_offset = new Vector2(-0.01f, 0.0f);
    protected int maxNameLenght = 40;
    protected MyGuiControlImage m_thumbnailImage;
    private MyGuiBlueprintTextDialog m_dialog;
    private MyBlueprintItemInfo m_selectedItem;
    private MyGuiIngameScriptsPage m_parent;
    protected MyGuiControlMultilineText m_descriptionField;
    private Action<MyBlueprintItemInfo> callBack;

    public MyGuiDetailScreenScriptLocal(
      Action<MyBlueprintItemInfo> callBack,
      MyBlueprintItemInfo selectedItem,
      MyGuiIngameScriptsPage parent,
      float textScale)
      : base(new Vector2(0.5f, 0.5f), new Vector2(0.778f, 0.594f), MyGuiConstants.SCREEN_BACKGROUND_COLOR * MySandboxGame.Config.UIBkOpacity, true)
    {
      this.WasPublished = false;
      this.callBack = callBack;
      this.m_parent = parent;
      this.m_selectedItem = selectedItem;
      this.m_textScale = textScale;
      this.m_localRoot = Path.Combine(MyFileSystem.UserDataPath, "IngameScripts", "local");
      this.CanBeHidden = true;
      this.CanHideOthers = true;
      this.RecreateControls(true);
      this.EnabledBackgroundFade = true;
    }

    protected void CreateButtons()
    {
      Vector2 vector2_1 = new Vector2(0.148f, -0.197f) + this.m_offset;
      Vector2 vector2_2 = new Vector2(0.132f, 0.045f);
      float num1 = 0.13f;
      if (this.m_selectedItem.Item == null)
      {
        double num2 = (double) num1;
        StringBuilder text1 = MyTexts.Get(MySpaceTexts.ProgrammableBlock_ButtonRename);
        Action<MyGuiControlButton> onClick1 = new Action<MyGuiControlButton>(this.OnRename);
        float textScale1 = this.m_textScale;
        MyStringId? tooltip1 = new MyStringId?(MyCommonTexts.Scripts_RenameTooltip);
        double num3 = (double) textScale1;
        MyBlueprintUtils.CreateButton((MyGuiScreenDebugBase) this, (float) num2, text1, onClick1, tooltip: tooltip1, textScale: ((float) num3)).Position = vector2_1;
        double num4 = (double) num1;
        StringBuilder text2 = MyTexts.Get(MyCommonTexts.LoadScreenButtonPublish);
        Action<MyGuiControlButton> onClick2 = new Action<MyGuiControlButton>(this.OnPublish);
        float textScale2 = this.m_textScale;
        MyStringId? tooltip2 = new MyStringId?(MyCommonTexts.Scripts_PublishTooltip);
        double num5 = (double) textScale2;
        MyGuiControlButton button = MyBlueprintUtils.CreateButton((MyGuiScreenDebugBase) this, (float) num4, text2, onClick2, tooltip: tooltip2, textScale: ((float) num5));
        button.Position = vector2_1 + new Vector2(1f, 0.0f) * vector2_2;
        button.Enabled = MyFakes.ENABLE_WORKSHOP_PUBLISH;
        double num6 = (double) num1;
        StringBuilder text3 = MyTexts.Get(MyCommonTexts.LoadScreenButtonDelete);
        Action<MyGuiControlButton> onClick3 = new Action<MyGuiControlButton>(this.OnDelete);
        float textScale3 = this.m_textScale;
        MyStringId? tooltip3 = new MyStringId?(MyCommonTexts.Scripts_DeleteTooltip);
        double num7 = (double) textScale3;
        MyBlueprintUtils.CreateButton((MyGuiScreenDebugBase) this, (float) num6, text3, onClick3, tooltip: tooltip3, textScale: ((float) num7)).Position = vector2_1 + new Vector2(0.0f, 1f) * vector2_2;
        double num8 = (double) num1;
        StringBuilder text4 = MyTexts.Get(MyCommonTexts.ScreenLoadSubscribedWorldBrowseWorkshop);
        Action<MyGuiControlButton> onClick4 = new Action<MyGuiControlButton>(this.OnOpenWorkshop);
        float textScale4 = this.m_textScale;
        MyStringId? tooltip4 = new MyStringId?(MyCommonTexts.Scripts_OpenWorkshopTooltip);
        double num9 = (double) textScale4;
        MyBlueprintUtils.CreateButton((MyGuiScreenDebugBase) this, (float) num8, text4, onClick4, tooltip: tooltip4, textScale: ((float) num9)).Position = vector2_1 + new Vector2(1f, 1f) * vector2_2;
      }
      else
      {
        double num2 = (double) num1 * 2.0;
        StringBuilder text = MyTexts.Get(MyCommonTexts.ScreenLoadSubscribedWorldOpenInWorkshop);
        Action<MyGuiControlButton> onClick = new Action<MyGuiControlButton>(this.OnOpenInWorkshop);
        float textScale = this.m_textScale;
        MyStringId? tooltip = new MyStringId?(MyCommonTexts.Scripts_OpenWorkshopTooltip);
        double num3 = (double) textScale;
        MyBlueprintUtils.CreateButton((MyGuiScreenDebugBase) this, (float) num2, text, onClick, tooltip: tooltip, textScale: ((float) num3)).Position = new Vector2(0.215f, -0.197f) + this.m_offset;
      }
    }

    protected void CreateDescription()
    {
      Vector2 position = new Vector2(-0.325f, -0.005f) + this.m_offset;
      Vector2 size = new Vector2(0.67f, 0.155f);
      Vector2 vector2 = new Vector2(0.005f, -0.04f);
      this.AddCompositePanel(MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER, position, size, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      Vector2? offset = new Vector2?(position + vector2);
      float textScale = this.m_textScale;
      this.m_descriptionField = this.CreateMultilineText(new Vector2?(size - (vector2 + new Vector2(0.0f, 0.045f))), offset, textScale);
      this.RefreshDescriptionField();
    }

    protected void RefreshDescriptionField()
    {
      if (this.m_descriptionField == null)
        return;
      this.m_descriptionField.Clear();
      if (this.m_selectedItem.Data.Description == null)
        return;
      this.m_descriptionField.AppendText(this.m_selectedItem.Data.Description);
    }

    protected void CreateTextField()
    {
      Vector2 position = new Vector2(-0.325f, -0.2f) + this.m_offset;
      Vector2 size = new Vector2(0.175f, 0.175f);
      Vector2 vector2 = new Vector2(0.005f, -0.04f);
      this.AddCompositePanel(MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER, position, size, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      this.m_textField = new MyGuiControlMultilineText();
      Vector2? offset = new Vector2?(position + vector2);
      float textScale = this.m_textScale;
      this.m_textField = this.CreateMultilineText(new Vector2?(size - vector2), offset, textScale);
      this.RefreshTextField();
    }

    protected void RefreshTextField()
    {
      if (this.m_textField == null)
        return;
      string str = this.m_selectedItem.Data.Name;
      if (str.Length > 25)
        str = str.Substring(0, 25) + "...";
      this.m_textField.Clear();
      this.m_textField.AppendText("Name: " + str);
      this.m_textField.AppendLine();
      this.m_textField.AppendText("Type: IngameScript");
    }

    public override string GetFriendlyName() => "MyDetailScreenScripts";

    private void OnRename(MyGuiControlButton button)
    {
      this.HideScreen();
      Vector2 position = new Vector2(0.5f, 0.5f);
      Action<string> callBack = (Action<string>) (result =>
      {
        if (result == null)
          return;
        this.m_parent.ChangeName(result);
      });
      string str = MyTexts.GetString(MySpaceTexts.ProgrammableBlock_NewScriptName);
      string name = this.m_selectedItem.Data.Name;
      string caption = str;
      int maxNameLenght = this.maxNameLenght;
      this.m_dialog = new MyGuiBlueprintTextDialog(position, callBack, name, caption, maxNameLenght, 0.3f);
      MyScreenManager.AddScreen((MyGuiScreenBase) this.m_dialog);
    }

    private void OnDelete(MyGuiControlButton button)
    {
      this.m_parent.OnDelete(button);
      this.CloseScreen();
    }

    private void OnPublish(MyGuiControlButton button)
    {
      string name = this.m_selectedItem.Data.Name;
      MyWorkshopItem myWorkshopItem = this.m_selectedItem.Item;
      WorkshopId additionalWorkshopId = new WorkshopId(myWorkshopItem != null ? myWorkshopItem.Id : 0UL, this.m_selectedItem.Item?.ServiceName);
      WorkshopId[] modInfo = MyWorkshop.GetWorkshopIdFromLocalBlueprint(name, additionalWorkshopId);
      StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.LoadScreenButtonPublish);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MySpaceTexts.ProgrammableBlock_PublishScriptDialogText), messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (val =>
      {
        if (val != MyGuiScreenMessageBox.ResultEnum.YES)
          return;
        this.WasPublished = true;
        string fullPath = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, this.m_selectedItem.Data.Name);
        MyWorkshop.PublishIngameScriptAsync(fullPath, this.m_selectedItem.Data.Name, this.m_selectedItem.Data.Description ?? "", modInfo, (string[]) null, MyPublishedFileVisibility.Public, (Action<bool, MyGameServiceCallResult, string, MyWorkshopItem[]>) ((success, result, resultServiceName, publishedFiles) =>
        {
          if (publishedFiles.Length != 0)
            MyWorkshop.GenerateModInfo(fullPath, publishedFiles, Sync.MyId);
          MyWorkshop.ReportPublish(publishedFiles, result, resultServiceName);
        }));
      }))));
    }

    private void OnOpenWorkshop(MyGuiControlButton button) => MyWorkshop.OpenWorkshopBrowser(MySteamConstants.TAG_SCRIPTS);

    private void OnOpenInWorkshop(MyGuiControlButton button) => MyGuiSandbox.OpenUrlWithFallback(this.m_selectedItem.Item.GetItemUrl(), this.m_selectedItem.Item.ServiceName + " Workshop");

    protected void OnCloseButton(MyGuiControlButton button) => this.CloseScreen();

    protected void CallResultCallback(MyBlueprintItemInfo val)
    {
      this.UnhideScreen();
      this.callBack(val);
    }

    protected override void OnClosed()
    {
      base.OnClosed();
      if (this.m_dialog != null)
        this.m_dialog.CloseScreen();
      this.CallResultCallback(this.m_selectedItem);
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.AddCaption(MyCommonTexts.ScreenCaptionBlueprintDetails, captionOffset: new Vector2?(new Vector2(0.0f, 3f / 1000f)));
      MyGuiControlSeparatorList controlSeparatorList1 = new MyGuiControlSeparatorList();
      controlSeparatorList1.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.860000014305115 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0750000029802322)), this.m_size.Value.X * 0.86f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList1);
      MyGuiControlSeparatorList controlSeparatorList2 = new MyGuiControlSeparatorList();
      controlSeparatorList2.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.860000014305115 / 2.0), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.123000003397465)), this.m_size.Value.X * 0.86f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList2);
      StringBuilder text = MyTexts.Get(MySpaceTexts.DetailScreen_Button_Close);
      Action<MyGuiControlButton> onClick = new Action<MyGuiControlButton>(this.OnCloseButton);
      float textScale = this.m_textScale;
      MyStringId? tooltip = new MyStringId?(MySpaceTexts.ToolTipNewsletter_Close);
      double num = (double) textScale;
      MyGuiControlButton button = MyBlueprintUtils.CreateButton((MyGuiScreenDebugBase) this, 1f, text, onClick, tooltip: tooltip, textScale: ((float) num));
      button.Position = new Vector2(0.0f, (float) ((double) this.m_size.Value.Y / 2.0 - 0.0970000028610229));
      button.VisualStyle = MyGuiControlButtonStyleEnum.Default;
      MyGuiControlImage myGuiControlImage1 = new MyGuiControlImage();
      myGuiControlImage1.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK;
      this.m_thumbnailImage = myGuiControlImage1;
      this.m_thumbnailImage.SetPadding(new MyGuiBorderThickness(2f, 2f, 2f, 2f));
      this.m_thumbnailImage.BorderEnabled = true;
      this.m_thumbnailImage.BorderSize = 1;
      this.m_thumbnailImage.BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f);
      this.m_thumbnailImage.Position = new Vector2(-0.035f, -0.112f) + this.m_offset;
      this.m_thumbnailImage.Size = new Vector2(0.2f, 0.175f);
      this.Controls.Add((MyGuiControlBase) this.m_thumbnailImage);
      MyGuiControlImage myGuiControlImage2 = new MyGuiControlImage();
      myGuiControlImage2.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK;
      MyGuiControlImage myGuiControlImage3 = myGuiControlImage2;
      myGuiControlImage3.SetPadding(new MyGuiBorderThickness(2f, 2f, 2f, 2f));
      myGuiControlImage3.SetTexture(this.m_selectedItem.Item == null ? MyGuiConstants.TEXTURE_ICON_BLUEPRINTS_LOCAL.Normal : MyGuiConstants.GetWorkshopIcon(this.m_selectedItem.Item).Normal);
      myGuiControlImage3.Position = new Vector2(-0.035f, -0.112f) + this.m_offset;
      myGuiControlImage3.Size = new Vector2(0.027f, 0.029f);
      this.Controls.Add((MyGuiControlBase) myGuiControlImage3);
      this.CreateTextField();
      this.CreateDescription();
      this.CreateButtons();
    }

    protected MyGuiControlMultilineText CreateMultilineText(
      Vector2? size = null,
      Vector2? offset = null,
      float textScale = 1f,
      bool selectable = false)
    {
      Vector2 vector2 = size ?? this.Size ?? new Vector2(0.5f, 0.5f);
      MyGuiControlMultilineText controlMultilineText = new MyGuiControlMultilineText(new Vector2?(this.m_currentPosition + vector2 / 2f + (offset ?? Vector2.Zero)), new Vector2?(vector2), font: "Debug", textScale: (this.m_scale * textScale), textBoxAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, selectable: selectable);
      this.Controls.Add((MyGuiControlBase) controlMultilineText);
      return controlMultilineText;
    }
  }
}
