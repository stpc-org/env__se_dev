// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiDetailScreenBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  internal abstract class MyGuiDetailScreenBase : MyGuiBlueprintScreenBase
  {
    protected static readonly Vector2 SCREEN_SIZE = new Vector2(0.4f, 1.2f);
    protected float m_textScale;
    protected string m_blueprintName;
    protected MyGuiControlListbox.Item m_selectedItem;
    protected MyObjectBuilder_Definitions m_loadedPrefab;
    protected MyGuiControlMultilineText m_textField;
    protected MyGuiControlMultilineText m_descriptionField;
    protected MyGuiControlImage m_thumbnailImage;
    protected Action<MyGuiControlListbox.Item> callBack;
    protected MyGuiBlueprintScreenBase m_parent;
    protected MyGuiBlueprintTextDialog m_dialog;
    protected bool m_killScreen;
    protected Vector2 m_offset = new Vector2(-0.01f, 0.0f);
    protected int maxNameLenght = 40;

    public MyGuiDetailScreenBase(
      bool isTopMostScreen,
      MyGuiBlueprintScreenBase parent,
      string thumbnailTexture,
      MyGuiControlListbox.Item selectedItem,
      float textScale)
      : base(new Vector2(0.5f, 0.5f), new Vector2(0.778f, 0.594f), MyGuiConstants.SCREEN_BACKGROUND_COLOR * MySandboxGame.Config.UIBkOpacity, isTopMostScreen)
    {
      MyGuiControlImage myGuiControlImage = new MyGuiControlImage();
      myGuiControlImage.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK;
      this.m_thumbnailImage = myGuiControlImage;
      this.m_thumbnailImage.SetPadding(new MyGuiBorderThickness(2f, 2f, 2f, 2f));
      this.m_thumbnailImage.SetTexture(thumbnailTexture);
      this.m_thumbnailImage.BorderEnabled = true;
      this.m_thumbnailImage.BorderSize = 1;
      this.m_thumbnailImage.BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f);
      this.m_selectedItem = selectedItem;
      this.m_blueprintName = selectedItem.Text.ToString();
      this.m_textScale = textScale;
      this.m_parent = parent;
      this.CloseButtonEnabled = true;
    }

    protected int GetNumberOfBlocks()
    {
      int num = 0;
      foreach (MyObjectBuilder_CubeGrid cubeGrid in this.m_loadedPrefab.ShipBlueprints[0].CubeGrids)
        num += cubeGrid.CubeBlocks.Count;
      return num;
    }

    protected int GetNumberOfBattlePoints() => (int) this.m_loadedPrefab.ShipBlueprints[0].Points;

    protected void RefreshTextField()
    {
      if (this.m_textField == null)
        return;
      string str = this.m_blueprintName;
      if (str.Length > 25)
        str = str.Substring(0, 25) + "...";
      this.m_textField.Clear();
      this.m_textField.AppendText(MyTexts.GetString(MySpaceTexts.BlueprintInfo_Name) + str);
      this.m_textField.AppendLine();
      MyCubeSize gridSizeEnum = this.m_loadedPrefab.ShipBlueprints[0].CubeGrids[0].GridSizeEnum;
      this.m_textField.AppendText(MyTexts.GetString(MyCommonTexts.BlockPropertiesText_Type));
      if (this.m_loadedPrefab.ShipBlueprints[0].CubeGrids[0].IsStatic && gridSizeEnum == MyCubeSize.Large)
        this.m_textField.AppendText(MyTexts.GetString(MyCommonTexts.DetailStaticGrid));
      else if (gridSizeEnum == MyCubeSize.Small)
        this.m_textField.AppendText(MyTexts.GetString(MyCommonTexts.DetailSmallGrid));
      else
        this.m_textField.AppendText(MyTexts.GetString(MyCommonTexts.DetailLargeGrid));
      this.m_textField.AppendLine();
      this.m_textField.AppendText(MyTexts.GetString(MySpaceTexts.BlueprintInfo_NumberOfBlocks) + (object) this.GetNumberOfBlocks());
      this.m_textField.AppendLine();
      this.m_textField.AppendText(MyTexts.GetString(MySpaceTexts.BlueprintInfo_Author) + this.m_loadedPrefab.ShipBlueprints[0].DisplayName);
      this.m_textField.AppendLine();
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
      string description = this.m_loadedPrefab.ShipBlueprints[0].Description;
      if (description == null)
        return;
      this.m_descriptionField.AppendText(description);
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

    public override void RecreateControls(bool constructor)
    {
      if (this.m_loadedPrefab == null)
      {
        this.CloseScreen();
      }
      else
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
        this.CreateTextField();
        this.CreateDescription();
        this.CreateButtons();
        this.m_thumbnailImage.Position = new Vector2(-0.035f, -0.112f) + this.m_offset;
        this.m_thumbnailImage.Size = new Vector2(0.2f, 0.175f);
        this.Controls.Add((MyGuiControlBase) this.m_thumbnailImage);
      }
    }

    protected void CallResultCallback(MyGuiControlListbox.Item val) => this.callBack(val);

    protected override void Canceling()
    {
      this.CallResultCallback(this.m_selectedItem);
      base.Canceling();
    }

    protected abstract void CreateButtons();

    protected void OnCloseButton(MyGuiControlButton button)
    {
      this.CloseScreen();
      this.CallResultCallback(this.m_selectedItem);
    }

    public override bool Update(bool hasFocus)
    {
      if (this.m_killScreen)
      {
        this.CallResultCallback((MyGuiControlListbox.Item) null);
        this.CloseScreen();
      }
      return base.Update(hasFocus);
    }
  }
}
