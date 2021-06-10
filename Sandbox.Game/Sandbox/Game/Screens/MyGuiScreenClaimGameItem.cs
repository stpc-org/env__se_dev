// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenClaimGameItem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Analytics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
  public class MyGuiScreenClaimGameItem : MyGuiScreenBase
  {
    private long m_playerId;
    private MyContainerDropComponent m_container;
    private MyGuiControlButton m_okButton;

    public MyGuiScreenClaimGameItem(MyContainerDropComponent container, long playerId)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(0.41f, 0.4f)), true)
    {
      this.m_playerId = playerId;
      this.m_container = container;
      this.EnabledBackgroundFade = true;
      this.CloseButtonEnabled = true;
      this.RecreateControls(true);
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.AddCaption(MyCommonTexts.ScreenCaptionClaimGameItem, captionOffset: new Vector2?(new Vector2(0.0f, 3f / 1000f)));
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.740000009536743 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0750000029802322)), this.m_size.Value.X * 0.74f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList);
      MyGuiControlImage myGuiControlImage = new MyGuiControlImage(new Vector2?(new Vector2(-0.15f, -0.107f)), new Vector2?(new Vector2(0.3f, 0.17f)), textures: new string[1]
      {
        "Textures\\GUI\\ClaimItem.png"
      }, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      myGuiControlImage.BorderEnabled = true;
      myGuiControlImage.BorderSize = 2;
      myGuiControlImage.BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f);
      this.Controls.Add((MyGuiControlBase) myGuiControlImage);
      this.Elements.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(0.0f, 0.085f)), text: MyTexts.GetString(MyCommonTexts.ScreenClaimItemText), colorMask: new Vector4?(Vector4.One), originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER)
      {
        Font = "White"
      });
      this.m_okButton = new MyGuiControlButton(new Vector2?(new Vector2(0.0f, 0.168f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM, text: MyTexts.Get(MyCommonTexts.Ok), onButtonClick: new Action<MyGuiControlButton>(this.OnClaimButtonClick));
      this.m_okButton.GamepadHelpTextId = MyStringId.NullOrEmpty;
      this.Controls.Add((MyGuiControlBase) this.m_okButton);
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(new Vector2(this.m_okButton.PositionX, this.m_okButton.Position.Y - MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui.Y / 2f)));
      myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM;
      myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.ClaimSkin_Help_Screen);
      this.FocusedControl = (MyGuiControlBase) null;
      this.UpdateGamepadHelp((MyGuiControlBase) null);
    }

    private void OnClaimButtonClick(MyGuiControlButton obj)
    {
      MySessionComponentContainerDropSystem component = MySession.Static.GetComponent<MySessionComponentContainerDropSystem>();
      if (component != null)
      {
        MySpaceAnalytics.Instance.ReportDropContainer(this.m_container.Competetive);
        component.ContainerOpened(this.m_container, this.m_playerId);
      }
      this.CloseScreen();
    }

    protected override void OnClosed()
    {
      base.OnClosed();
      MyGuiScreenGamePlay.ActiveGameplayScreen = (MyGuiScreenBase) null;
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenClaimGameItem);

    public override bool Update(bool hasFocus)
    {
      this.m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      return base.Update(hasFocus);
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      base.HandleInput(receivedFocusInThisUpdate);
      if (!MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
        return;
      this.OnClaimButtonClick((MyGuiControlButton) null);
    }
  }
}
