// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenNewGameItems
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.GameServices;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
  public class MyGuiScreenNewGameItems : MyGuiScreenBase
  {
    private List<MyGameInventoryItem> m_items;
    private MyGuiControlLabel m_itemName;
    private MyGuiControlImage m_itemBackground;
    private MyGuiControlImage m_itemImage;
    private MyGuiControlButton m_okButton;

    public MyGuiScreenNewGameItems(List<MyGameInventoryItem> newItems)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(0.41f, 0.4f)), true)
    {
      this.m_items = newItems;
      MyAudio.Static.PlaySound(MySoundPair.GetCueId("ArcNewItemImpact"));
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
      this.Elements.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(0.0f, -0.096f)), text: MyTexts.GetString(MyCommonTexts.ScreenCaptionNewItem), colorMask: new Vector4?(Vector4.One), originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER)
      {
        Font = "White"
      });
      Vector2? position = new Vector2?(new Vector2(0.0f, 0.03f));
      Vector2? size1 = new Vector2?();
      Vector2? size2 = size1;
      Vector4? colorMask = new Vector4?(Vector4.One);
      this.m_itemName = new MyGuiControlLabel(position, size2, "Item Name", colorMask, originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
      this.m_itemName.Font = "Blue";
      this.Controls.Add((MyGuiControlBase) this.m_itemName);
      size1 = new Vector2?(new Vector2(0.07f, 0.09f));
      this.m_itemBackground = new MyGuiControlImage(new Vector2?(new Vector2(0.0f, -0.025f)), size1, textures: new string[1]
      {
        "Textures\\GUI\\blank.dds"
      });
      this.m_itemBackground.Margin = new Thickness(0.005f);
      this.Controls.Add((MyGuiControlBase) this.m_itemBackground);
      size1 = new Vector2?(new Vector2(0.06f, 0.08f));
      this.m_itemImage = new MyGuiControlImage(new Vector2?(new Vector2(0.0f, -0.025f)), size1);
      this.m_itemImage.Margin = new Thickness(0.005f);
      this.Controls.Add((MyGuiControlBase) this.m_itemImage);
      this.Elements.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(0.0f, 0.085f)), text: MyTexts.GetString(MyCommonTexts.ScreenNewItemVisit), colorMask: new Vector4?(Vector4.One), originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER)
      {
        Font = "White"
      });
      this.m_okButton = new MyGuiControlButton(new Vector2?(new Vector2(0.0f, 0.168f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM, text: MyTexts.Get(MyCommonTexts.Ok));
      this.m_okButton.ButtonClicked += new Action<MyGuiControlButton>(this.OnOkButtonClick);
      this.m_okButton.GamepadHelpTextId = MyStringId.NullOrEmpty;
      this.Controls.Add((MyGuiControlBase) this.m_okButton);
      this.LoadFirstItem();
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(new Vector2(this.m_okButton.PositionX, this.m_okButton.Position.Y - MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui.Y / 2f)));
      myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM;
      myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.ClaimSkin_Help_Screen);
      this.UpdateGamepadHelp((MyGuiControlBase) null);
    }

    private void LoadFirstItem()
    {
      MyGameInventoryItem gameInventoryItem = this.m_items.FirstOrDefault<MyGameInventoryItem>();
      if (gameInventoryItem == null)
        return;
      this.m_itemName.Text = gameInventoryItem.ItemDefinition.Name;
      this.m_itemBackground.ColorMask = string.IsNullOrEmpty(gameInventoryItem.ItemDefinition.BackgroundColor) ? Vector4.One : ColorExtensions.HexToVector4(gameInventoryItem.ItemDefinition.BackgroundColor);
      string[] textures = new string[1]
      {
        "Textures\\GUI\\Blank.dds"
      };
      if (!string.IsNullOrEmpty(gameInventoryItem.ItemDefinition.IconTexture))
        textures[0] = gameInventoryItem.ItemDefinition.IconTexture;
      this.m_itemImage.SetTextures(textures);
    }

    private void OnOkButtonClick(MyGuiControlButton obj)
    {
      if (this.m_items.Count<MyGameInventoryItem>() > 1)
      {
        this.m_items.RemoveAt(0);
        this.LoadFirstItem();
      }
      else
        this.CloseScreen();
    }

    protected override void OnClosed()
    {
      base.OnClosed();
      MyGuiScreenGamePlay.ActiveGameplayScreen = (MyGuiScreenBase) null;
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenNewGameItems);

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
      this.OnOkButtonClick((MyGuiControlButton) null);
    }
  }
}
