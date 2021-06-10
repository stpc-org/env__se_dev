// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenWorkshopTags
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.ViewModels;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
  internal class MyGuiScreenWorkshopTags : MyGuiScreenBase
  {
    private MyGuiControlButton m_okButton;
    private MyGuiControlButton m_cancelButton;
    private static readonly List<MyGuiControlCheckbox> m_cbTags = new List<MyGuiControlCheckbox>();
    private static readonly List<MyGuiControlCheckbox> m_cbServices = new List<MyGuiControlCheckbox>();
    private readonly Action<MyGuiScreenMessageBox.ResultEnum, string[], string[]> m_callback;
    private readonly string m_typeTag;
    private static string m_ugcConsentServiceName;
    private static MyGuiScreenWorkshopTags Static;
    private const int TAGS_MAX_LENGTH = 128;
    private static Dictionary<string, MyStringId> m_activeTags;

    public MyGuiScreenWorkshopTags(
      string typeTag,
      MyWorkshop.Category[] categories,
      string[] tags,
      Action<MyGuiScreenMessageBox.ResultEnum, string[], string[]> callback)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(0.6535714f, 0.7633588f)), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
    {
      MyGuiScreenWorkshopTags.Static = this;
      this.m_typeTag = typeTag ?? "";
      MyGuiScreenWorkshopTags.m_activeTags = new Dictionary<string, MyStringId>(categories.Length);
      foreach (MyWorkshop.Category category in categories)
        MyGuiScreenWorkshopTags.m_activeTags.Add(category.Id, category.LocalizableName);
      this.m_callback = callback;
      this.EnabledBackgroundFade = true;
      this.CanHideOthers = true;
      this.RecreateControls(true);
      this.SetSelectedTags(tags);
      this.m_okButton.Enabled = false;
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.AddCaption(MyCommonTexts.ScreenCaptionWorkshopTags, captionOffset: new Vector2?(new Vector2(0.0f, 3f / 1000f)));
      MyGuiControlSeparatorList controlSeparatorList1 = new MyGuiControlSeparatorList();
      controlSeparatorList1.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.829999983310699 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0750000029802322)), this.m_size.Value.X * 0.83f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList1);
      MyGuiControlSeparatorList controlSeparatorList2 = new MyGuiControlSeparatorList();
      Vector2 start = new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.829999983310699 / 2.0), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.123000003397465));
      controlSeparatorList2.AddHorizontal(start, this.m_size.Value.X * 0.83f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList2);
      Vector2 vector2_1 = new Vector2(-0.125f, -0.3f);
      Vector2 vector2_2 = new Vector2(0.0f, 0.05f);
      MyGuiScreenWorkshopTags.m_cbTags.Clear();
      MyGuiScreenWorkshopTags.m_cbServices.Clear();
      int num = 0;
      Vector2 vector2_3 = new Vector2(0.125f, vector2_1.Y) + vector2_2;
      Vector2 vector2_4 = new Vector2(-0.05f, 0.0f);
      Vector2 vector2_5 = new Vector2(-0.03f, 0.0f);
      foreach (KeyValuePair<string, MyStringId> activeTag in MyGuiScreenWorkshopTags.m_activeTags)
      {
        ++num;
        if (num == 11)
          vector2_1 = vector2_3;
        if (num == 1)
        {
          vector2_1 += vector2_2;
          this.AddLabel(vector2_1 + vector2_5, MyCommonTexts.WorkshopTagsHeader);
          ++num;
        }
        this.AddLabeledCheckbox(vector2_1 += vector2_2, activeTag.Key, activeTag.Value);
        if (this.m_typeTag == "mod")
          Path.Combine(MyFileSystem.ContentPath, "Textures", "GUI", "Icons", "buttons", activeTag.Key.Replace(" ", string.Empty) + ".dds");
      }
      string[] ugcNamesList = MyGameService.GetUGCNamesList();
      if (ugcNamesList.Length != 0)
      {
        vector2_1 = (num <= 0 || num >= 11 ? vector2_3 + vector2_2 * (float) (11 - ugcNamesList.Length - 3) : vector2_3) + vector2_2;
        this.AddLabel(vector2_1 + vector2_5, MyCommonTexts.WorkshopTagsServicesHeader);
        foreach (string str in ugcNamesList)
          this.AddLabeledCheckbox(vector2_1 += vector2_2, str, MyStringId.GetOrCompute(str), false);
      }
      Vector2 vector2_6 = vector2_1 + vector2_2;
      Vector2 position;
      this.Controls.Add((MyGuiControlBase) (this.m_okButton = this.MakeButton(position = vector2_6 + vector2_2, MyCommonTexts.Ok, MySpaceTexts.ToolTipNewsletter_Ok, new Action<MyGuiControlButton>(this.OnOkClick), MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER)));
      this.Controls.Add((MyGuiControlBase) (this.m_cancelButton = this.MakeButton(position, MyCommonTexts.Cancel, MySpaceTexts.ToolTipOptionsSpace_Cancel, new Action<MyGuiControlButton>(this.OnCancelClick), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER)));
      Vector2 vector2_7 = (this.m_size.Value / 2f - new Vector2(54f) / MyGuiConstants.GUI_OPTIMAL_SIZE) * new Vector2(0.0f, 1f);
      float x = 25f;
      this.m_okButton.Position = vector2_7 + new Vector2(-x, 0.0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      this.m_okButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
      this.m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_cancelButton.Position = vector2_7 + new Vector2(x, 0.0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      this.m_cancelButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
      this.m_cancelButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(new Vector2(start.X, this.m_okButton.Position.Y - minSizeGui.Y / 2f)));
      myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      this.CloseButtonEnabled = true;
      this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.WorkshopTagsScreen_Help_Screen);
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenWorkshopTags);

    private void AddLabel(Vector2 position, MyStringId text) => this.Controls.Add((MyGuiControlBase) this.MakeLabel(position, text));

    private MyGuiControlCheckbox AddLabeledCheckbox(
      Vector2 position,
      string tag,
      MyStringId text,
      bool isTag = true)
    {
      MyGuiControlLabel myGuiControlLabel = this.MakeLabel(position, text);
      MyGuiControlCheckbox checkbox = this.MakeCheckbox(position, text);
      MyGuiScreenWorkshopTags.m_ugcConsentServiceName = (string) null;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      this.Controls.Add((MyGuiControlBase) checkbox);
      checkbox.UserData = (object) tag;
      if (isTag)
      {
        MyGuiScreenWorkshopTags.m_cbTags.Add(checkbox);
      }
      else
      {
        checkbox.IsCheckedChanged += (Action<MyGuiControlCheckbox>) (controlCheckbox =>
        {
          if (!checkbox.IsChecked || MyGameService.WorkshopService.GetAggregate(tag).IsConsentGiven)
            return;
          checkbox.IsChecked = false;
          MyGuiScreenWorkshopTags.m_ugcConsentServiceName = tag;
          MyModIoConsentViewModel consentViewModel = new MyModIoConsentViewModel(new Action(this.UGCConsentGained));
          ServiceManager.Instance.GetService<IMyGuiScreenFactoryService>().CreateScreen((ViewModelBase) consentViewModel);
        });
        if (checkbox.IsChecked)
          checkbox.IsChecked = false;
        MyGuiScreenWorkshopTags.m_cbServices.Add(checkbox);
      }
      return checkbox;
    }

    private void UGCConsentGained()
    {
      foreach (MyGuiControlCheckbox cbService in MyGuiScreenWorkshopTags.m_cbServices)
      {
        if (cbService.UserData.ToString() == MyGuiScreenWorkshopTags.m_ugcConsentServiceName)
        {
          cbService.IsChecked = true;
          break;
        }
      }
    }

    private MyGuiControlImage AddIcon(
      Vector2 position,
      string texture,
      Vector2 size)
    {
      MyGuiControlImage myGuiControlImage1 = new MyGuiControlImage();
      myGuiControlImage1.Position = position;
      myGuiControlImage1.Size = size;
      MyGuiControlImage myGuiControlImage2 = myGuiControlImage1;
      myGuiControlImage2.SetTexture(texture);
      this.Controls.Add((MyGuiControlBase) myGuiControlImage2);
      return myGuiControlImage2;
    }

    private MyGuiControlLabel MakeLabel(Vector2 position, MyStringId text) => new MyGuiControlLabel(new Vector2?(position), text: MyTexts.GetString(text));

    private MyGuiControlCheckbox MakeCheckbox(
      Vector2 position,
      MyStringId tooltip)
    {
      MyGuiControlCheckbox guiControlCheckbox = new MyGuiControlCheckbox(new Vector2?(position), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
      guiControlCheckbox.IsCheckedChanged += new Action<MyGuiControlCheckbox>(this.OnCheckboxChanged);
      return guiControlCheckbox;
    }

    private MyGuiControlButton MakeButton(
      Vector2 position,
      MyStringId text,
      MyStringId toolTip,
      Action<MyGuiControlButton> onClick,
      MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
    {
      Vector2? position1 = new Vector2?(position);
      Vector2? size = new Vector2?();
      Vector4? colorMask = new Vector4?();
      StringBuilder stringBuilder = MyTexts.Get(text);
      string str = MyTexts.GetString(toolTip);
      Action<MyGuiControlButton> action = onClick;
      int num = (int) originAlign;
      string toolTip1 = str;
      StringBuilder text1 = stringBuilder;
      Action<MyGuiControlButton> onButtonClick = action;
      int? buttonIndex = new int?();
      return new MyGuiControlButton(position1, size: size, colorMask: colorMask, originAlign: ((MyGuiDrawAlignEnum) num), toolTip: toolTip1, text: text1, onButtonClick: onButtonClick, buttonIndex: buttonIndex);
    }

    private void OnCheckboxChanged(MyGuiControlCheckbox obj)
    {
      if (obj == null)
        return;
      if (obj.IsChecked && MyGuiScreenWorkshopTags.Static.GetSelectedTagsLength() >= 128)
        obj.IsChecked = false;
      bool flag = MyGuiScreenWorkshopTags.m_cbServices.Any<MyGuiControlCheckbox>((Func<MyGuiControlCheckbox, bool>) (sc => sc.IsChecked));
      if (this.m_okButton != null)
        this.m_okButton.Enabled = flag;
      this.GamepadHelpTextId = new MyStringId?(flag ? MySpaceTexts.WorkshopTagsScreen_Help_Screen : MySpaceTexts.WorkshopTagsScreen_HelpNoOk_Screen);
      this.UpdateGamepadHelp(this.FocusedControl);
    }

    private void OnOkClick(MyGuiControlButton obj)
    {
      if (!this.m_okButton.Enabled)
        return;
      this.CloseScreen();
      this.m_callback(MyGuiScreenMessageBox.ResultEnum.YES, this.GetSelectedTags(), this.GetSelectedServiceNames());
    }

    private void OnCancelClick(MyGuiControlButton obj)
    {
      this.CloseScreen();
      this.m_callback(MyGuiScreenMessageBox.ResultEnum.CANCEL, this.GetSelectedTags(), this.GetSelectedServiceNames());
    }

    protected override void Canceling()
    {
      base.Canceling();
      this.m_callback(MyGuiScreenMessageBox.ResultEnum.CANCEL, this.GetSelectedTags(), this.GetSelectedServiceNames());
    }

    private string[] GetSelectedServiceNames()
    {
      if (MyGuiScreenWorkshopTags.m_cbServices.Count == 0)
        return new string[1]
        {
          MyGameService.GetDefaultUGC().ServiceName
        };
      List<string> stringList = new List<string>();
      foreach (MyGuiControlCheckbox cbService in MyGuiScreenWorkshopTags.m_cbServices)
      {
        if (cbService.IsChecked)
          stringList.Add((string) cbService.UserData);
      }
      return stringList.ToArray();
    }

    public int GetSelectedTagsLength()
    {
      int length = this.m_typeTag.Length;
      foreach (MyGuiControlCheckbox cbTag in MyGuiScreenWorkshopTags.m_cbTags)
      {
        if (cbTag.IsChecked)
          length += ((string) cbTag.UserData).Length;
      }
      return length;
    }

    public string[] GetSelectedTags()
    {
      List<string> stringList = new List<string>();
      if (!string.IsNullOrEmpty(this.m_typeTag))
        stringList.Add(this.m_typeTag);
      foreach (MyGuiControlCheckbox cbTag in MyGuiScreenWorkshopTags.m_cbTags)
      {
        if (cbTag.IsChecked)
          stringList.Add((string) cbTag.UserData);
      }
      return stringList.ToArray();
    }

    public void SetSelectedTags(string[] tags)
    {
      if (tags == null)
        return;
      foreach (string tag in tags)
      {
        foreach (MyGuiControlCheckbox cbTag in MyGuiScreenWorkshopTags.m_cbTags)
        {
          if (tag.Equals((string) cbTag.UserData, StringComparison.InvariantCultureIgnoreCase))
            cbTag.IsChecked = true;
        }
      }
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      base.HandleInput(receivedFocusInThisUpdate);
      if (receivedFocusInThisUpdate)
        return;
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
        this.OnOkClick((MyGuiControlButton) null);
      this.m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_cancelButton.Visible = !MyInput.Static.IsJoystickLastUsed;
    }
  }
}
