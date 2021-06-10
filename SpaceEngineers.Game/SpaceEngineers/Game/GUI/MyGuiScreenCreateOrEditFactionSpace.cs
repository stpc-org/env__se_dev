// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.GUI.MyGuiScreenCreateOrEditFactionSpace
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using EmptyKeys.UserInterface.Mvvm;
using Sandbox;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.ViewModels;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using Sandbox.Gui;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Factions.Definitions;
using VRage.Game.ModAPI;
using VRage.Input;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.GUI
{
  public class MyGuiScreenCreateOrEditFactionSpace : MyGuiScreenCreateOrEditFaction
  {
    private const string FALLBACK_DEFINITION_FACTIONICON = "Textures\\FactionLogo\\Empty.dds";
    private MyGuiControlButton m_btnOk;

    public MyGuiScreenCreateOrEditFactionSpace(ref IMyFaction editData)
      : base(ref editData)
      => this.CloseButtonEnabled = true;

    public MyGuiScreenCreateOrEditFactionSpace() => this.CloseButtonEnabled = true;

    public override string GetFriendlyName() => nameof (MyGuiScreenCreateOrEditFactionSpace);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.AddCaption(MySpaceTexts.TerminalTab_Factions_EditFaction, captionOffset: new Vector2?(new Vector2(0.0f, 3f / 1000f)));
      MyGuiControlSeparatorList controlSeparatorList1 = new MyGuiControlSeparatorList();
      controlSeparatorList1.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.779999971389771 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0750000029802322)), this.m_size.Value.X * 0.78f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList1);
      MyGuiControlSeparatorList controlSeparatorList2 = new MyGuiControlSeparatorList();
      Vector2 start = new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.779999971389771 / 2.0), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.123000003397465));
      controlSeparatorList2.AddHorizontal(start, this.m_size.Value.X * 0.78f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList2);
      float x1 = -0.2285f;
      float num1 = -0.273f;
      float num2 = 0.07f;
      Vector2 vector2_1 = new Vector2(0.29f, 0.052f);
      Vector2? size1 = new Vector2?(vector2_1);
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel(new Vector2?(new Vector2(x1, num1 + num2)), size1, MyTexts.GetString(MySpaceTexts.TerminalTab_Factions_CreateFactionTag), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      size1 = new Vector2?(vector2_1);
      MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(new Vector2?(new Vector2(x1, num1 + 2f * num2)), size1, MyTexts.GetString(MySpaceTexts.TerminalTab_Factions_CreateFactionName), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      size1 = new Vector2?(vector2_1);
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(new Vector2?(new Vector2(x1, num1 + 3f * num2)), size1, MyTexts.GetString(MySpaceTexts.TerminalTab_Factions_CreateFactionDescription), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, isAutoEllipsisEnabled: true, maxWidth: 0.12f, isAutoScaleEnabled: true);
      size1 = new Vector2?(vector2_1);
      MyGuiControlLabel myGuiControlLabel4 = new MyGuiControlLabel(new Vector2?(new Vector2(x1, num1 + 4.8f * num2)), size1, MyTexts.GetString(MySpaceTexts.TerminalTab_Factions_CreateFactionPrivateInfo), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, isAutoEllipsisEnabled: true, maxWidth: 0.12f, isAutoScaleEnabled: true);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel1);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel3);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel4);
      float x2 = x1 + 0.1165f;
      float y1 = num1 + 0.065f;
      Vector2 vector2_2 = new Vector2(0.2485f, 0.1f);
      this.m_shortcut = new MyGuiControlTextbox(new Vector2?(new Vector2(x2, y1)), this.m_editFaction != null ? this.m_editFaction.Tag : "", 3);
      this.m_shortcut.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_shortcut.Enabled = this.m_editFaction == null || this.m_editFaction != null && !MySession.Static.Factions.IsNpcFaction(this.m_editFaction.Tag);
      this.m_name = new MyGuiControlTextbox(new Vector2?(new Vector2(x2, y1 + num2)), this.m_editFaction != null ? MyStatControlText.SubstituteTexts(this.m_editFaction.Name) : "", 64);
      this.m_name.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      float val = 0.0075f;
      this.m_desc = new MyGuiControlMultilineEditableText(new Vector2?(new Vector2(x2, y1 + 2f * num2)));
      this.m_desc.TextWrap = true;
      this.m_desc.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_desc.Text = this.m_editFaction != null ? new StringBuilder(this.m_editFaction.Description) : new StringBuilder("");
      this.m_desc.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_BORDER;
      this.m_desc.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_desc.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_desc.TextPadding = new MyGuiBorderThickness(val);
      this.m_desc.TextChanged += new Action<MyGuiControlMultilineEditableText>(this.OnTextChanged);
      Vector2? position1 = new Vector2?(new Vector2(x2, y1 + 3.8f * num2));
      Vector2? nullable = new Vector2?();
      Vector2? size2 = nullable;
      Vector4? backgroundColor = new Vector4?();
      int? visibleLinesCount = new int?();
      MyGuiBorderThickness? textPadding = new MyGuiBorderThickness?();
      this.m_privInfo = new MyGuiControlMultilineEditableText(position1, size2, backgroundColor, visibleLinesCount: visibleLinesCount, textPadding: textPadding);
      this.m_privInfo.TextWrap = true;
      this.m_privInfo.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_privInfo.Text = this.m_editFaction != null ? new StringBuilder(this.m_editFaction.PrivateInfo) : new StringBuilder("");
      this.m_privInfo.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_BORDER;
      this.m_privInfo.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_privInfo.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_privInfo.TextPadding = new MyGuiBorderThickness(val);
      this.m_privInfo.TextChanged += new Action<MyGuiControlMultilineEditableText>(this.OnTextChanged);
      this.m_shortcut.Size = vector2_2;
      this.m_name.Size = vector2_2;
      Rectangle safeGuiRectangle = MyGuiManager.GetSafeGuiRectangle();
      float num3 = (float) safeGuiRectangle.Height / (float) safeGuiRectangle.Width;
      float y2 = 0.03f;
      Vector2 vector2_3 = new Vector2(y2 * num3, y2);
      float y3 = 0.11f;
      float x3 = y3 * num3;
      float num4 = 0.01f;
      Vector2 vector2_4 = new Vector2(vector2_2.X + x3 + num4, 0.1f);
      this.m_desc.Size = vector2_4;
      this.m_privInfo.Size = vector2_4;
      MyGuiControlImage myGuiControlImage = new MyGuiControlImage();
      myGuiControlImage.Position = this.m_shortcut.Position + new Vector2(this.m_shortcut.Size.X + num4, 0.0f);
      myGuiControlImage.Size = new Vector2(x3, y3);
      myGuiControlImage.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlImage.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER;
      myGuiControlImage.Padding = new MyGuiBorderThickness(1f);
      this.m_factionIcon = myGuiControlImage;
      if (this.m_editFaction != null)
      {
        MyStringId? factionIcon = this.m_editFaction.FactionIcon;
        string empty;
        if (!factionIcon.HasValue)
        {
          empty = string.Empty;
        }
        else
        {
          factionIcon = this.m_editFaction.FactionIcon;
          empty = factionIcon.Value.ToString();
        }
        MyGuiControlImage.MyDrawTexture backgroundTexture;
        MyGuiControlImage.MyDrawTexture iconFaction;
        this.GetFactionIconTextures(this.m_editFaction.CustomColor, this.m_editFaction.IconColor, empty, out backgroundTexture, out iconFaction);
        this.m_factionIcon.SetTextures(new MyGuiControlImage.MyDrawTexture[2]
        {
          backgroundTexture,
          iconFaction
        });
        SerializableDefinitionId? factionIconGroupId;
        if (MyFactionCollection.GetDefinitionIdsByIconName(iconFaction.Texture, out factionIconGroupId, out this.m_factionIconId))
        {
          this.m_factionIconGroupId = factionIconGroupId.Value;
        }
        else
        {
          MyFactionCollection.GetDefinitionIdsByIconName("Textures\\FactionLogo\\Empty.dds", out factionIconGroupId, out this.m_factionIconId);
          this.m_factionIconGroupId = factionIconGroupId.Value;
        }
        this.m_factionColor = this.m_editFaction.CustomColor;
        this.m_factionIconColor = this.m_editFaction.IconColor;
      }
      else
      {
        MyGuiControlImage.MyDrawTexture backgroundTexture;
        MyGuiControlImage.MyDrawTexture iconFaction;
        this.GetRandomIcon(out backgroundTexture, out iconFaction);
        this.m_factionIcon.SetTextures(new MyGuiControlImage.MyDrawTexture[2]
        {
          backgroundTexture,
          iconFaction
        });
        SerializableDefinitionId? factionIconGroupId;
        if (MyFactionCollection.GetDefinitionIdsByIconName(iconFaction.Texture, out factionIconGroupId, out this.m_factionIconId))
          this.m_factionIconGroupId = factionIconGroupId.Value;
        this.m_factionColor = MyColorPickerConstants.HSVToHSVOffset(new Color(backgroundTexture.ColorMask.Value).ColorToHSV());
        this.m_factionIconColor = MyColorPickerConstants.HSVToHSVOffset(Color.White.ColorToHSV());
      }
      this.m_editFactionIconBtn = new MyGuiControlImageButton("", new Vector2?(this.m_factionIcon.Position + new Vector2(this.m_factionIcon.Size.X, 0.0f)), new Vector2?(vector2_3));
      this.m_editFactionIconBtn.ApplyStyle(new MyGuiControlImageButton.StyleDefinition()
      {
        Active = new MyGuiControlImageButton.StateDefinition()
        {
          Texture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_BORDER
        },
        Disabled = new MyGuiControlImageButton.StateDefinition()
        {
          Texture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_BORDER
        },
        Normal = new MyGuiControlImageButton.StateDefinition()
        {
          Texture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_BORDER
        },
        Highlight = new MyGuiControlImageButton.StateDefinition()
        {
          Texture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_HIGHLIGHTED_BORDER
        },
        ActiveHighlight = new MyGuiControlImageButton.StateDefinition()
        {
          Texture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_HIGHLIGHTED_BORDER
        },
        Padding = new MyGuiBorderThickness(0.005f * num3, 0.005f)
      });
      this.m_editFactionIconBtn.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      this.m_editFactionIconBtn.Size = vector2_3;
      this.m_editFactionIconBtn.Icon = new MyGuiControlImageButton.ButtonIcon()
      {
        Normal = "Textures\\GUI\\Icons\\Blueprints\\ThumbnailsON.dds",
        Active = "Textures\\GUI\\Icons\\Blueprints\\ThumbnailsON.dds",
        Highlight = "Textures\\GUI\\Icons\\Blueprints\\ThumbnailsON_Highlight.dds"
      };
      this.m_editFactionIconBtn.ButtonClicked += new Action<MyGuiControlImageButton>(this.OnEditIconButtonPressed);
      this.m_shortcut.SetToolTip(MySpaceTexts.TerminalTab_Factions_CreateFactionTagToolTip);
      this.m_privInfo.SetToolTip(MySpaceTexts.TerminalTab_Factions_CreateFactionPrivateInfoToolTip);
      this.m_name.SetToolTip(MyCommonTexts.MessageBoxErrorFactionsNameTooShort);
      this.m_desc.SetToolTip(MySpaceTexts.TerminalTab_Factions_CreateFactionPublicInfoToolTip);
      this.Controls.Add((MyGuiControlBase) this.m_shortcut);
      this.Controls.Add((MyGuiControlBase) this.m_name);
      this.Controls.Add((MyGuiControlBase) this.m_desc);
      this.Controls.Add((MyGuiControlBase) this.m_privInfo);
      this.Controls.Add((MyGuiControlBase) this.m_factionIcon);
      this.Controls.Add((MyGuiControlBase) this.m_editFactionIconBtn);
      float num5 = y1 - 3f / 1000f;
      Vector2 vector2_5 = new Vector2(1f / 500f, (float) ((double) this.m_size.Value.Y / 2.0 - 0.0410000011324883));
      Vector2 vector2_6 = new Vector2(0.229f, 0.0f);
      nullable = new Vector2?(vector2_1);
      Vector2? position2 = new Vector2?(vector2_5 + vector2_6);
      Vector2? size3 = nullable;
      Vector4? colorMask = new Vector4?();
      StringBuilder stringBuilder = MyTexts.Get(MyCommonTexts.Ok);
      Action<MyGuiControlButton> action = new Action<MyGuiControlButton>(((MyGuiScreenCreateOrEditFaction) this).OnOkClick);
      string toolTip = MyTexts.GetString(MySpaceTexts.ToolTipNewsletter_Ok);
      StringBuilder text = stringBuilder;
      Action<MyGuiControlButton> onButtonClick = action;
      int? buttonIndex = new int?();
      this.m_btnOk = new MyGuiControlButton(position2, size: size3, colorMask: colorMask, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, toolTip: toolTip, text: text, onButtonClick: onButtonClick, buttonIndex: buttonIndex);
      this.Controls.Add((MyGuiControlBase) this.m_btnOk);
      MyGuiControlLabel myGuiControlLabel5 = new MyGuiControlLabel(new Vector2?(new Vector2(start.X, this.m_btnOk.Position.Y - vector2_1.Y / 2f)));
      myGuiControlLabel5.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel5);
      this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.FactionCreateEdit_Help_Screen);
      this.FocusedControl = (MyGuiControlBase) this.m_shortcut;
    }

    private void OnTextChanged(MyGuiControlMultilineEditableText obj)
    {
      if (obj.GetTextLength() <= 512)
        return;
      obj.Text = new StringBuilder(obj.Text.ToString().Substring(0, 512));
    }

    private void OnEditIconButtonPressed(MyGuiControlImageButton obj)
    {
      Vector3 hsv1 = MyColorPickerConstants.HSVOffsetToHSV(this.m_factionColor);
      Vector3 hsv2 = MyColorPickerConstants.HSVOffsetToHSV(this.m_factionIconColor);
      MyEditFactionIconViewModel factionIconViewModel = new MyEditFactionIconViewModel(this.m_factionIconGroupId, this.m_factionIconId, MyFactionCollection.GetFactionIcon(this.m_factionIconGroupId, this.m_factionIconId), hsv1.HSVtoColor(), hsv2.HSVtoColor());
      factionIconViewModel.OnFactionEditorOk += new Action<MyEditFactionIconViewModel>(this.OnFactionEditorOk);
      ServiceManager.Instance.GetService<IMyGuiScreenFactoryService>().CreateScreen((ViewModelBase) factionIconViewModel);
    }

    private void OnFactionEditorOk(MyEditFactionIconViewModel viewModel)
    {
      this.m_factionColor = MyColorPickerConstants.HSVToHSVOffset(viewModel.BackgroundColor.ColorToHSV());
      this.m_factionIconColor = MyColorPickerConstants.HSVToHSVOffset(viewModel.IconColor.ColorToHSV());
      MyGuiControlImage.MyDrawTexture backgroundTexture;
      MyGuiControlImage.MyDrawTexture iconFaction;
      this.GetFactionIconTextures(this.m_factionColor, this.m_factionIconColor, viewModel.ImageIconPath, out backgroundTexture, out iconFaction);
      this.m_factionIcon.SetTextures(new MyGuiControlImage.MyDrawTexture[2]
      {
        backgroundTexture,
        iconFaction
      });
      this.m_factionIconGroupId = viewModel.FactionIconGroupId;
      this.m_factionIconId = viewModel.FactionIconId;
    }

    private void GetFactionIconTextures(
      Vector3 hsvOffsetColor,
      string iconPath,
      out MyGuiControlImage.MyDrawTexture backgroundTexture,
      out MyGuiControlImage.MyDrawTexture iconFaction)
    {
      Vector3 hsv = MyColorPickerConstants.HSVOffsetToHSV(hsvOffsetColor);
      backgroundTexture = new MyGuiControlImage.MyDrawTexture()
      {
        Texture = "Textures\\GUI\\Blank.dds",
        ColorMask = new Vector4?(hsv.HSVtoColor().ToVector4())
      };
      iconFaction = new MyGuiControlImage.MyDrawTexture()
      {
        Texture = iconPath,
        ColorMask = new Vector4?(new Vector4(1f))
      };
    }

    private void GetFactionIconTextures(
      Vector3 hsvOffsetColor,
      Vector3 hsvIconColor,
      string iconPath,
      out MyGuiControlImage.MyDrawTexture backgroundTexture,
      out MyGuiControlImage.MyDrawTexture iconFaction)
    {
      Vector3 hsv1 = MyColorPickerConstants.HSVOffsetToHSV(hsvOffsetColor);
      backgroundTexture = new MyGuiControlImage.MyDrawTexture()
      {
        Texture = "Textures\\GUI\\Blank.dds",
        ColorMask = new Vector4?(hsv1.HSVtoColor().ToVector4())
      };
      Vector3 hsv2 = MyColorPickerConstants.HSVOffsetToHSV(hsvIconColor);
      iconFaction = new MyGuiControlImage.MyDrawTexture()
      {
        Texture = iconPath,
        ColorMask = new Vector4?(hsv2.HSVtoColor().ToVector4())
      };
    }

    private void GetRandomIcon(
      out MyGuiControlImage.MyDrawTexture backgroundTexture,
      out MyGuiControlImage.MyDrawTexture iconFaction)
    {
      IEnumerable<MyFactionIconsDefinition> allDefinitions = MyDefinitionManager.Static.GetAllDefinitions<MyFactionIconsDefinition>();
      List<string> stringList = new List<string>();
      MySessionComponentDLC component = MySession.Static.GetComponent<MySessionComponentDLC>();
      foreach (MyFactionIconsDefinition factionIconsDefinition in allDefinitions)
      {
        if (factionIconsDefinition.Id.SubtypeId.String == "Other")
        {
          if (component.HasDLC("Economy", Sync.MyId))
          {
            foreach (string icon in factionIconsDefinition.Icons)
              stringList.Add(icon);
          }
        }
        else
        {
          foreach (string icon in factionIconsDefinition.Icons)
            stringList.Add(icon);
        }
      }
      int index = MyRandom.Instance.Next(0, stringList.Count);
      ref MyGuiControlImage.MyDrawTexture local1 = ref iconFaction;
      MyGuiControlImage.MyDrawTexture myDrawTexture1 = new MyGuiControlImage.MyDrawTexture();
      myDrawTexture1.Texture = stringList[index];
      myDrawTexture1.ColorMask = new Vector4?(Vector4.One);
      MyGuiControlImage.MyDrawTexture myDrawTexture2 = myDrawTexture1;
      local1 = myDrawTexture2;
      float x = MyRandom.Instance.NextFloat(0.0f, 1f);
      float y = MyRandom.Instance.NextFloat(0.0f, 1f);
      float z = MyRandom.Instance.NextFloat(0.0f, 1f);
      ref MyGuiControlImage.MyDrawTexture local2 = ref backgroundTexture;
      myDrawTexture1 = new MyGuiControlImage.MyDrawTexture();
      myDrawTexture1.Texture = "Textures\\GUI\\Blank.dds";
      myDrawTexture1.ColorMask = new Vector4?(new Vector4(x, y, z, 1f));
      MyGuiControlImage.MyDrawTexture myDrawTexture3 = myDrawTexture1;
      local2 = myDrawTexture3;
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      base.HandleInput(receivedFocusInThisUpdate);
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
        this.OnOkClick((MyGuiControlButton) null);
      if (!MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y))
        return;
      this.OnEditIconButtonPressed((MyGuiControlImageButton) null);
    }

    public override bool Update(bool hasFocus)
    {
      this.m_btnOk.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_editFactionIconBtn.Visible = !MyInput.Static.IsJoystickLastUsed;
      return base.Update(hasFocus);
    }
  }
}
