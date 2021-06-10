// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyGuiDatapadEditScreen
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Input;
using VRage.Library.Collections;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GUI
{
  public class MyGuiDatapadEditScreen : MyGuiScreenBase
  {
    private static BitStream m_stream = new BitStream();
    private static Vector2 m_defaultWindowSize = new Vector2(0.6f, 0.7f);
    private static Vector2 m_windowSizeMulti = new Vector2(1.333f, 1f);
    private MyObjectBuilder_Datapad m_datapad;
    private MyPhysicalInventoryItem m_item;
    private MyInventory m_inventory;
    private MyCharacter m_character;
    private MyGuiControlTextbox m_textboxName;
    private MyGuiControlMultilineEditableText m_textbox;
    private MyGuiControlButton m_okButton;
    private MyGuiControlLabel m_characterCountLabel;
    private MyGuiControlButton m_createGpsCoord;
    private string m_lastValidText = "";

    public static event Action OnDatapadOpened;

    public MyGuiDatapadEditScreen(
      MyObjectBuilder_Datapad pad,
      MyPhysicalInventoryItem item,
      MyInventory inventory,
      MyCharacter character)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(MyGuiDatapadEditScreen.m_defaultWindowSize * MyGuiDatapadEditScreen.m_windowSizeMulti))
    {
      this.m_datapad = pad;
      this.m_item = item;
      this.m_inventory = inventory;
      this.m_character = character;
      this.RecreateControls(true);
    }

    static MyGuiDatapadEditScreen() => MyVRage.RegisterExitCallback((Action) (() => MyGuiDatapadEditScreen.m_stream.Dispose()));

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.AddCaption(MySpaceTexts.DatapadEditEcreen_Caption, captionOffset: new Vector2?(new Vector2(0.0f, 3f / 1000f)));
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(-0.25f, -0.23f) * MyGuiDatapadEditScreen.m_windowSizeMulti), text: MyTexts.GetString(MySpaceTexts.DatapadEditScreen_Name)));
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2(this.m_size.Value.X * 0.417f, this.m_size.Value.Y * 0.385f), this.m_size.Value.X * 0.834f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList);
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(-0.25f, -0.17f) * MyGuiDatapadEditScreen.m_windowSizeMulti), text: MyTexts.GetString(MySpaceTexts.DatapadEditScreen_Content)));
      this.m_characterCountLabel = new MyGuiControlLabel(new Vector2?(new Vector2(-0.25f, 0.28f) * MyGuiDatapadEditScreen.m_windowSizeMulti), text: string.Empty);
      this.Controls.Add((MyGuiControlBase) this.m_characterCountLabel);
      this.m_textboxName = new MyGuiControlTextbox(new Vector2?(new Vector2(0.05f, -0.23f) * MyGuiDatapadEditScreen.m_windowSizeMulti), this.m_datapad != null ? this.m_datapad.Name : string.Empty, this.m_datapad != null ? MyObjectBuilder_Datapad.NAME_CHAR_LIMIT : 0);
      MyGuiControlTextbox textboxName1 = this.m_textboxName;
      textboxName1.Size = textboxName1.Size + new Vector2(0.08f, 0.0f);
      MyGuiControlTextbox textboxName2 = this.m_textboxName;
      textboxName2.Size = textboxName2.Size * MyGuiDatapadEditScreen.m_windowSizeMulti;
      this.Controls.Add((MyGuiControlBase) this.m_textboxName);
      MyGuiControlCompositePanel controlCompositePanel = new MyGuiControlCompositePanel();
      controlCompositePanel.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER;
      controlCompositePanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
      controlCompositePanel.Position = new Vector2(0.0f, 0.05f) * MyGuiDatapadEditScreen.m_windowSizeMulti;
      controlCompositePanel.Size = new Vector2(0.5f, 0.4f) * MyGuiDatapadEditScreen.m_windowSizeMulti;
      this.Controls.Add((MyGuiControlBase) controlCompositePanel);
      this.m_textbox = new MyGuiControlMultilineEditableText(new Vector2?(new Vector2(0.0f, 0.05f) * MyGuiDatapadEditScreen.m_windowSizeMulti), new Vector2?(new Vector2(0.5f, 0.4f) * MyGuiDatapadEditScreen.m_windowSizeMulti), new Vector4?(Color.White.ToVector4()), "White", textBoxAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      this.m_textbox.TextWrap = true;
      this.m_textbox.TextPadding = new MyGuiBorderThickness(0.01f);
      this.m_textbox.Text = new StringBuilder(this.m_datapad != null ? this.m_datapad.Data : string.Empty);
      this.m_textbox.TextChanged += new Action<MyGuiControlMultilineEditableText>(this.MultilineTextChanged);
      if (MyPlatformGameSettings.IsMultilineEditableByGamepad)
        this.m_textbox.GamepadHelpTextId = MyCommonTexts.Gamepad_Help_MultiLineTextbox;
      this.Controls.Add((MyGuiControlBase) this.m_textbox);
      Vector2? position1 = new Vector2?(new Vector2(0.186f, 0.3f) * MyGuiDatapadEditScreen.m_windowSizeMulti);
      Vector2? size1 = new Vector2?(MyGuiConstants.BACK_BUTTON_SIZE);
      Vector4? colorMask1 = new Vector4?();
      StringBuilder stringBuilder1 = MyTexts.Get(MyCommonTexts.Ok);
      Action<MyGuiControlButton> action1 = new Action<MyGuiControlButton>(this.OkButtonClicked);
      string toolTip1 = MyTexts.GetString(MySpaceTexts.ProgrammableBlock_CodeEditor_SaveExit_Tooltip);
      StringBuilder text1 = stringBuilder1;
      Action<MyGuiControlButton> onButtonClick1 = action1;
      int? buttonIndex1 = new int?();
      this.m_okButton = new MyGuiControlButton(position1, size: size1, colorMask: colorMask1, toolTip: toolTip1, text: text1, onButtonClick: onButtonClick1, buttonIndex: buttonIndex1);
      this.Controls.Add((MyGuiControlBase) this.m_okButton);
      Vector2? position2 = new Vector2?(this.m_okButton.Position - new Vector2(MyGuiConstants.BACK_BUTTON_SIZE.X + 0.02f, 0.0f));
      Vector2? size2 = new Vector2?(MyGuiConstants.BACK_BUTTON_SIZE);
      Vector4? colorMask2 = new Vector4?();
      StringBuilder stringBuilder2 = MyTexts.Get(MySpaceTexts.GUI_Datapad_CreateGPSCoord);
      Action<MyGuiControlButton> action2 = new Action<MyGuiControlButton>(this.OnCreateGpsCoordClicked);
      string toolTip2 = MyTexts.GetString(MySpaceTexts.GUI_Datapad_CreateGPSCoord_TTIP);
      StringBuilder text2 = stringBuilder2;
      Action<MyGuiControlButton> onButtonClick2 = action2;
      int? buttonIndex2 = new int?();
      this.m_createGpsCoord = new MyGuiControlButton(position2, size: size2, colorMask: colorMask2, toolTip: toolTip2, text: text2, onButtonClick: onButtonClick2, buttonIndex: buttonIndex2);
      this.Controls.Add((MyGuiControlBase) this.m_createGpsCoord);
      MyGuiControlButton guiControlButton1 = new MyGuiControlButton();
      guiControlButton1.Position = new Vector2(0.27f, -0.32f) * MyGuiDatapadEditScreen.m_windowSizeMulti;
      guiControlButton1.Size = new Vector2(0.045f, 0.05666667f);
      guiControlButton1.Name = "Close";
      guiControlButton1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      guiControlButton1.VisualStyle = MyGuiControlButtonStyleEnum.Close;
      guiControlButton1.ActivateOnMouseRelease = true;
      MyGuiControlButton guiControlButton2 = guiControlButton1;
      guiControlButton2.ButtonClicked += new Action<MyGuiControlButton>(this.CloseButtonClicked);
      this.Controls.Add((MyGuiControlBase) guiControlButton2);
      this.MultilineTextChanged(this.m_textbox);
      Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(new Vector2(this.m_characterCountLabel.PositionX + 0.2f, this.m_characterCountLabel.PositionY)));
      myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.DatapadEdit_Help_Screen);
      this.FocusedControl = (MyGuiControlBase) this.m_textboxName;
    }

    private void MultilineTextChanged(MyGuiControlMultilineEditableText obj)
    {
      if (this.m_textbox == null)
        return;
      string str1 = this.m_textbox.Text.ToString();
      MyGuiDatapadEditScreen.m_stream.ResetWrite();
      MyGuiDatapadEditScreen.m_stream.WritePrefixLengthString(str1, 0, Math.Min(str1.Length, MyObjectBuilder_Datapad.DATA_CHAR_LIMIT - 1), Encoding.UTF8);
      int num = MyGuiDatapadEditScreen.m_stream.BytePosition - 1;
      MyGuiDatapadEditScreen.m_stream.ResetRead();
      string str2 = MyGuiDatapadEditScreen.m_stream.ReadString();
      if (str2.Length != str1.Length)
        this.m_textbox.Text = this.m_textbox.Text.Clear().Append(str2);
      this.m_characterCountLabel.Text = string.Format(MyTexts.GetString(MySpaceTexts.DatapadEditScreen_ContentUsage), (object) num, (object) (this.m_datapad != null ? MyObjectBuilder_Datapad.DATA_CHAR_LIMIT : 0));
      if (this.m_textbox.Text.Length == 0)
      {
        this.m_createGpsCoord.Visible = false;
      }
      else
      {
        Vector3D coords = new Vector3D();
        this.m_createGpsCoord.Visible = MyGpsCollection.ParseOneGPS(this.m_textbox.Text.ToString(), new StringBuilder(), ref coords);
      }
    }

    private void OkButtonClicked(MyGuiControlButton button)
    {
      if (this.m_datapad != null)
      {
        string text = this.m_textboxName.Text;
        string str = this.m_textbox.Text.ToString();
        if (this.m_inventory == null || this.m_inventory.Owner == null)
        {
          this.CloseScreen();
          return;
        }
        int num = -1;
        for (int index = 0; index < this.m_inventory.Owner.InventoryCount; ++index)
        {
          if (MyEntityExtensions.GetInventory(this.m_inventory.Owner, index) == this.m_inventory)
          {
            num = index;
            break;
          }
        }
        MyMultiplayer.RaiseStaticEvent<long, int, uint, string, string>((Func<IMyEventOwner, Action<long, int, uint, string, string>>) (x => new Action<long, int, uint, string, string>(MyInventory.ModifyDatapad)), this.m_inventory.Owner.EntityId, num, this.m_item.ItemId, text, str);
      }
      this.CloseScreen();
    }

    private void CloseButtonClicked(MyGuiControlButton button) => this.CloseScreen();

    private void OnCreateGpsCoordClicked(MyGuiControlButton button)
    {
      if (this.m_textbox.Text.Length == 0)
        return;
      MySession.Static.Gpss.ScanText(this.m_textbox.Text.ToString(), MyTexts.Get(MySpaceTexts.TerminalTab_GPS_NewFromClipboard_Desc));
      this.m_createGpsCoord.Enabled = false;
    }

    protected override void OnShow()
    {
      base.OnShow();
      Action onDatapadOpened = MyGuiDatapadEditScreen.OnDatapadOpened;
      if (onDatapadOpened == null)
        return;
      onDatapadOpened();
    }

    public override string GetFriendlyName() => nameof (MyGuiDatapadEditScreen);

    public override bool Update(bool hasFocus)
    {
      this.m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_createGpsCoord.Visible = !MyInput.Static.IsJoystickLastUsed;
      return base.Update(hasFocus);
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      base.HandleInput(receivedFocusInThisUpdate);
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
        this.OkButtonClicked((MyGuiControlButton) null);
      if (!MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y))
        return;
      this.OnCreateGpsCoordClicked((MyGuiControlButton) null);
    }
  }
}
