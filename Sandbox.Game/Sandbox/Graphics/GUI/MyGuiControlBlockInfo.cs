// Decompiled with JetBrains decompiler
// Type: Sandbox.Graphics.GUI.MyGuiControlBlockInfo
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
  public class MyGuiControlBlockInfo : MyGuiControlParent
  {
    public static bool ShowComponentProgress = true;
    public static bool ShowCriticalComponent = false;
    public static bool ShowCriticalIntegrity = true;
    public static bool ShowOwnershipIntegrity = MyFakes.SHOW_FACTIONS_GUI;
    public static Vector4 CriticalIntegrityColor = Color.Red.ToVector4();
    public static Vector4 CriticalComponentColor = MyGuiControlBlockInfo.CriticalIntegrityColor * new Vector4(1f, 1f, 1f, 0.7f);
    public static Vector4 OwnershipIntegrityColor = Color.Blue.ToVector4();
    private MyGuiControlLabel m_blockTypeLabel;
    private MyGuiControlLabel m_blockNameLabel;
    private MyGuiControlLabel m_componentsLabel;
    private MyGuiControlLabel m_installedRequiredLabel;
    private MyGuiControlLabel m_blockBuiltByLabel;
    private MyGuiControlLabel m_integrityLabel;
    private MyGuiControlLabel m_PCULabel;
    private MyGuiControlImage m_blockIconImage;
    private MyGuiControlImage m_PCUIcon;
    private MyGuiControlPanel m_blockTypePanel;
    private MyGuiControlPanel m_pcuBackground;
    private MyGuiControlPanel m_titleBackground;
    private MyGuiControlPanel m_integrityBackground;
    private MyGuiProgressCompositeTextureAdvanced m_integrityForeground;
    private Color m_integrityForegroundColorMask = Color.White;
    private MyGuiControlLabel m_criticalIntegrityLabel;
    private MyGuiControlLabel m_ownershipIntegrityLabel;
    private MyGuiControlSeparatorList m_separator;
    private List<MyGuiControlBlockInfo.ComponentLineControl> m_componentLines = new List<MyGuiControlBlockInfo.ComponentLineControl>(15);
    public MyHudBlockInfo BlockInfo;
    private bool m_progressMode;
    private MyGuiControlBlockInfo.MyControlBlockInfoStyle m_style;
    private float m_smallerFontSize = 0.83f;
    private int m_lastInfoStamp;

    private float baseScale => !this.m_progressMode ? 0.83f : 1f;

    private float itemHeight => 0.037f * this.baseScale;

    public MyGuiControlBlockInfo(
      MyGuiControlBlockInfo.MyControlBlockInfoStyle style,
      bool progressMode = true,
      bool largeBlockInfo = true)
    {
      this.BackgroundTexture = new MyGuiCompositeTexture("Textures\\GUI\\Blank.dds");
      this.m_style = style;
      this.m_progressMode = progressMode;
      if (this.m_progressMode)
        this.BackgroundTexture = MyGuiConstants.TEXTURE_COMPOSITE_SLOPE_LEFTBOTTOM_30;
      this.ColorMask = this.m_style.BackgroundColormask;
      this.m_titleBackground = new MyGuiControlPanel(backgroundColor: new Vector4?(Color.Red.ToVector4()));
      this.m_titleBackground.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_titleBackground.BackgroundTexture = new MyGuiCompositeTexture("Textures\\GUI\\Blank.dds");
      this.Elements.Add((MyGuiControlBase) this.m_titleBackground);
      if (this.m_progressMode)
      {
        this.m_integrityLabel = new MyGuiControlLabel(text: string.Empty);
        this.m_integrityLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM;
        this.m_integrityLabel.Font = this.m_style.IntegrityLabelFont;
        this.Controls.Add((MyGuiControlBase) this.m_integrityLabel);
        this.m_integrityBackground = new MyGuiControlPanel();
        this.m_integrityBackground.BackgroundTexture = MyGuiConstants.TEXTURE_COMPOSITE_BLOCKINFO_PROGRESSBAR;
        this.m_integrityBackground.ColorMask = this.m_style.IntegrityBackgroundColor;
        this.m_integrityBackground.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
        this.Elements.Add((MyGuiControlBase) this.m_integrityBackground);
        this.m_integrityForeground = new MyGuiProgressCompositeTextureAdvanced(MyGuiConstants.TEXTURE_COMPOSITE_BLOCKINFO_PROGRESSBAR);
        this.m_integrityForeground.IsInverted = true;
        this.m_integrityForeground.Orientation = MyGuiProgressCompositeTexture.BarOrientation.VERTICAL;
        this.m_criticalIntegrityLabel = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.Functional));
        this.m_criticalIntegrityLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM;
        this.m_criticalIntegrityLabel.TextScale = 0.4f * this.baseScale;
        this.m_criticalIntegrityLabel.Font = "Blue";
        this.m_ownershipIntegrityLabel = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.Hack));
        this.m_ownershipIntegrityLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
        this.m_ownershipIntegrityLabel.TextScale = 0.4f * this.baseScale;
        this.m_ownershipIntegrityLabel.Font = "Blue";
      }
      this.m_blockIconImage = new MyGuiControlImage();
      this.m_blockIconImage.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_blockIconImage.BackgroundTexture = this.m_progressMode ? (MyGuiCompositeTexture) null : new MyGuiCompositeTexture(MyGuiConstants.TEXTURE_HUD_BG_MEDIUM_DEFAULT.Texture);
      this.m_blockIconImage.Size = this.m_progressMode ? new Vector2(0.066f) : new Vector2(0.04f);
      MyGuiControlImage blockIconImage = this.m_blockIconImage;
      blockIconImage.Size = blockIconImage.Size * new Vector2(0.75f, 1f);
      this.Elements.Add((MyGuiControlBase) this.m_blockIconImage);
      this.m_blockTypePanel = new MyGuiControlPanel();
      this.m_blockTypePanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_blockTypePanel.Size = this.m_progressMode ? new Vector2(0.088f) : new Vector2(0.02f);
      MyGuiControlPanel blockTypePanel = this.m_blockTypePanel;
      blockTypePanel.Size = blockTypePanel.Size * new Vector2(0.75f, 1f);
      this.m_blockTypePanel.BackgroundTexture = new MyGuiCompositeTexture(largeBlockInfo ? "Textures\\GUI\\Icons\\HUD 2017\\GridSizeLargeFit.png" : "Textures\\GUI\\Icons\\HUD 2017\\GridSizeSmallFit.png");
      this.Elements.Add((MyGuiControlBase) this.m_blockTypePanel);
      this.m_blockNameLabel = new MyGuiControlLabel(text: string.Empty);
      this.m_blockNameLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_blockNameLabel.TextScale = 1f * this.baseScale;
      this.m_blockNameLabel.Font = this.m_style.BlockNameLabelFont;
      this.m_blockNameLabel.IsAutoEllipsisEnabled = true;
      this.m_blockNameLabel.IsAutoScaleEnabled = true;
      this.Elements.Add((MyGuiControlBase) this.m_blockNameLabel);
      string empty = string.Empty;
      if (style.EnableBlockTypeLabel)
        empty = MyTexts.GetString(largeBlockInfo ? MySpaceTexts.HudBlockInfo_LargeShip_Station : MySpaceTexts.HudBlockInfo_SmallShip);
      this.m_blockTypeLabel = new MyGuiControlLabel(text: empty);
      this.m_blockTypeLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_blockTypeLabel.TextScale = 1f * this.baseScale;
      this.m_blockTypeLabel.Font = "White";
      this.Elements.Add((MyGuiControlBase) this.m_blockTypeLabel);
      this.m_componentsLabel = new MyGuiControlLabel(text: MyTexts.GetString(this.m_style.ComponentsLabelText));
      this.m_componentsLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_componentsLabel.TextScale = this.m_smallerFontSize * this.baseScale;
      this.m_componentsLabel.Font = this.m_style.ComponentsLabelFont;
      this.Elements.Add((MyGuiControlBase) this.m_componentsLabel);
      this.m_installedRequiredLabel = new MyGuiControlLabel(text: MyTexts.GetString(this.m_style.InstalledRequiredLabelText));
      this.m_installedRequiredLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      this.m_installedRequiredLabel.TextScale = this.m_smallerFontSize * this.baseScale;
      this.m_installedRequiredLabel.Font = this.m_style.InstalledRequiredLabelFont;
      this.Elements.Add((MyGuiControlBase) this.m_installedRequiredLabel);
      this.m_blockBuiltByLabel = new MyGuiControlLabel(text: string.Empty);
      this.m_blockBuiltByLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      this.m_blockBuiltByLabel.TextScale = this.m_smallerFontSize * this.baseScale;
      this.m_blockBuiltByLabel.Font = this.m_style.InstalledRequiredLabelFont;
      this.Elements.Add((MyGuiControlBase) this.m_blockBuiltByLabel);
      if (!this.m_progressMode && !this.m_style.HiddenPCU)
      {
        this.m_pcuBackground = new MyGuiControlPanel(backgroundColor: new Vector4?((Vector4) new Color(0.21f, 0.26f, 0.3f)));
        this.m_pcuBackground.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
        this.m_pcuBackground.BackgroundTexture = new MyGuiCompositeTexture("Textures\\GUI\\Blank.dds");
        this.m_pcuBackground.Size = new Vector2(0.225f, 0.03f);
        this.Elements.Add((MyGuiControlBase) this.m_pcuBackground);
        this.m_PCUIcon = new MyGuiControlImage(size: new Vector2?(new Vector2(0.022f, 0.029f)), textures: new string[1]
        {
          "Textures\\GUI\\PCU.png"
        });
        this.m_PCUIcon.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
        this.Elements.Add((MyGuiControlBase) this.m_PCUIcon);
        this.m_PCULabel = new MyGuiControlLabel();
        this.m_PCULabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
        this.Elements.Add((MyGuiControlBase) this.m_PCULabel);
      }
      this.m_separator = new MyGuiControlSeparatorList();
      this.EnsureLineControls(this.m_componentLines.Capacity);
      this.Size = this.m_progressMode ? new Vector2(0.225f, 0.4f) : new Vector2(0.225f, 0.4f);
    }

    private void EnsureLineControls(int count)
    {
      while (this.m_componentLines.Count < count)
      {
        MyGuiControlBlockInfo.ComponentLineControl componentLineControl = new MyGuiControlBlockInfo.ComponentLineControl((this.m_progressMode ? new Vector2(0.288f, 0.05f) : new Vector2(0.24f, 0.05f)) * new Vector2(1f, this.baseScale), 0.035f * this.baseScale);
        this.m_componentLines.Add(componentLineControl);
        this.Elements.Add((MyGuiControlBase) componentLineControl);
      }
    }

    public void RecalculateSize()
    {
      if (this.m_progressMode)
      {
        this.Size = new Vector2(this.Size.X, (float) (0.119999997317791 * (double) this.baseScale + (double) this.itemHeight * (double) (this.BlockInfo.Components.Count - 2)));
      }
      else
      {
        this.Size = new Vector2(this.Size.X, (float) (0.100000001490116 * (double) this.baseScale + (double) this.itemHeight * (double) (this.BlockInfo.Components.Count + 1)));
        if (this.m_style.HiddenPCU)
          this.Size = this.Size - new Vector2(0.0f, this.itemHeight);
        if (!this.m_style.HiddenHeader)
          return;
        this.Size = this.Size - new Vector2(0.0f, this.itemHeight);
      }
    }

    private void Reposition()
    {
      this.RecalculateSize();
      Vector2 vector2_1 = -this.Size / 2f;
      Vector2 vector2_2 = new Vector2(this.Size.X / 2f, (float) (-(double) this.Size.Y / 2.0));
      Vector2 vector2_3 = new Vector2((float) (-(double) this.Size.X / 2.0), this.Size.Y / 2f);
      Vector2 vector2_4 = vector2_1 + (this.m_progressMode ? new Vector2(0.06f, 0.0f) : new Vector2(0.036f, 0.0f));
      float num1 = 0.072f * this.baseScale;
      Vector2 vector2_5 = new Vector2(0.0035f) * new Vector2(0.75f, 1f) * this.baseScale;
      if (!this.m_progressMode)
        vector2_5.Y *= 1f;
      this.m_installedRequiredLabel.TextToDraw = (double) this.BlockInfo.BlockIntegrity <= 0.0 ? (!this.BlockInfo.ShowAvailable ? MyTexts.Get(this.m_style.RequiredLabelText) : MyTexts.Get(this.m_style.RequiredAvailableLabelText)) : MyTexts.Get(this.m_style.RequiredLabelText);
      this.m_titleBackground.Position = vector2_1;
      this.m_titleBackground.ColorMask = this.m_style.TitleBackgroundColor;
      float num2;
      if (this.m_progressMode || this.m_style.HiddenHeader)
      {
        num2 = 0.0f;
        this.m_blockIconImage.Visible = false;
        this.m_blockTypeLabel.Visible = false;
        this.m_blockNameLabel.Visible = false;
        this.m_titleBackground.Visible = false;
      }
      else
        num2 = (float) ((double) Math.Abs(vector2_1.Y - this.m_blockIconImage.Position.Y) + (double) this.m_blockIconImage.Size.Y + 3.0 / 1000.0);
      this.m_titleBackground.Size = new Vector2(vector2_2.X - this.m_titleBackground.Position.X, num2 + 3f / 1000f);
      this.m_separator.Clear();
      if (this.m_progressMode)
      {
        this.m_ownershipIntegrityLabel.Visible = false;
        this.m_criticalIntegrityLabel.Visible = false;
        float y = this.itemHeight * (float) this.BlockInfo.Components.Count;
        float num3 = 0.05f;
        Vector2 vector2_6 = new Vector2(3f / 500f, 0.04f);
        Vector2 normalizedCoord = this.GetPositionAbsoluteTopLeft() + vector2_6;
        Vector2 vector2_7 = vector2_1 + vector2_6 + new Vector2(0.0f, y);
        Vector2 normalizedSize = new Vector2(num3, y);
        this.m_integrityBackground.Position = vector2_7;
        this.m_integrityBackground.Size = normalizedSize;
        this.m_integrityLabel.TextToDraw.Clear();
        this.m_integrityLabel.TextToDraw.AppendInt32((int) Math.Floor((double) this.BlockInfo.BlockIntegrity * 100.0)).Append("%");
        this.m_integrityLabel.RecalculateSize();
        this.m_integrityLabel.Position = vector2_1 + vector2_6 + new Vector2(num3 / 2f, 0.0f);
        if ((double) this.BlockInfo.BlockIntegrity > 0.0)
        {
          this.m_integrityForegroundColorMask = (Color) ((double) this.BlockInfo.BlockIntegrity > (double) this.BlockInfo.CriticalIntegrity ? this.m_style.IntegrityForegroundColorOverCritical : this.m_style.IntegrityForegroundColor);
          this.m_integrityForeground.Position = new Vector2I(MyGuiManager.GetScreenCoordinateFromNormalizedCoordinate(normalizedCoord));
          this.m_integrityForeground.Size = new Vector2I(MyGuiManager.GetScreenSizeFromNormalizedSize(normalizedSize));
          float width = 0.004f;
          if (MyGuiControlBlockInfo.ShowCriticalIntegrity)
          {
            this.m_separator.AddHorizontal(normalizedCoord + new Vector2(0.0f, y * (1f - this.BlockInfo.CriticalIntegrity)), num3, width, new Vector4?(MyGuiControlBlockInfo.CriticalIntegrityColor));
            this.m_criticalIntegrityLabel.Position = normalizedCoord + new Vector2(num3 / 2f, y * (1f - this.BlockInfo.CriticalIntegrity));
            this.m_criticalIntegrityLabel.Visible = true;
          }
          if (MyGuiControlBlockInfo.ShowOwnershipIntegrity && (double) this.BlockInfo.OwnershipIntegrity > 0.0)
          {
            this.m_separator.AddHorizontal(normalizedCoord + new Vector2(0.0f, y * (1f - this.BlockInfo.OwnershipIntegrity)), num3, width, new Vector4?(MyGuiControlBlockInfo.OwnershipIntegrityColor));
            this.m_ownershipIntegrityLabel.Position = normalizedCoord + new Vector2(num3 / 2f, (float) ((double) y * (1.0 - (double) this.BlockInfo.OwnershipIntegrity) + 1.0 / 500.0));
            this.m_ownershipIntegrityLabel.Visible = true;
          }
        }
      }
      else if (!this.m_style.HiddenPCU)
      {
        this.m_pcuBackground.Position = vector2_3 + new Vector2(0.0f, -0.03f);
        this.m_pcuBackground.Size = new Vector2(this.Size.X, this.m_pcuBackground.Size.Y);
        this.m_PCUIcon.Position = vector2_3 + new Vector2(0.0085f, -0.03f);
        this.m_PCULabel.Position = this.m_PCUIcon.Position + new Vector2(0.035f, 3f / 1000f);
        this.m_PCULabel.Text = "PCU: " + this.BlockInfo.PCUCost.ToString();
      }
      if (this.m_progressMode)
      {
        this.m_blockNameLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
        this.m_blockNameLabel.TextScale = 0.81f;
        this.m_blockNameLabel.Size = new Vector2(vector2_2.X - (float) ((double) this.m_blockIconImage.Position.X + (double) this.m_blockIconImage.Size.X + 0.00400000018998981), this.m_blockNameLabel.Size.Y);
        this.m_blockNameLabel.Position = new Vector2(vector2_4.X, this.m_blockIconImage.Position.Y + 0.022f);
        this.m_blockBuiltByLabel.Position = this.m_blockNameLabel.Position + new Vector2(0.0f, this.m_blockNameLabel.Size.Y + 0.0f);
        this.m_blockBuiltByLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
        this.m_blockBuiltByLabel.TextScale = 0.6f;
        this.m_componentsLabel.Position = vector2_4 + new Vector2(0.0f, (float) (0.0094999996945262 + (double) num2 * (double) this.baseScale));
        this.m_installedRequiredLabel.Position = vector2_2 + new Vector2(-11f / 1000f, (float) (0.0094999996945262 + (double) num2 * (double) this.baseScale));
        this.m_blockTypeLabel.Visible = false;
        this.m_blockTypePanel.Visible = false;
      }
      else
      {
        this.m_blockTypePanel.Position = vector2_1 + new Vector2(0.01f, 0.012f);
        if (this.m_style.EnableBlockTypePanel)
        {
          this.m_blockTypePanel.Visible = true;
          this.m_blockNameLabel.Size = new Vector2(this.m_blockTypePanel.Position.X - this.m_blockTypePanel.Size.X - this.m_blockNameLabel.Position.X, this.m_blockNameLabel.Size.Y);
        }
        else
        {
          this.m_blockTypePanel.Visible = false;
          this.m_blockNameLabel.Size = new Vector2((float) ((double) vector2_2.X - ((double) this.m_blockIconImage.Position.X + (double) this.m_blockIconImage.Size.X + 0.00400000018998981) - 3.0 / 500.0), this.m_blockNameLabel.Size.Y);
        }
        this.m_blockNameLabel.TextScale = 0.95f * this.baseScale;
        this.m_blockNameLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
        this.m_blockNameLabel.Position = new Vector2(vector2_4.X + 3f / 500f, this.m_blockIconImage.Position.Y + this.m_blockIconImage.Size.Y);
        if (!this.m_style.EnableBlockTypeLabel)
        {
          this.m_blockNameLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
          MyGuiControlLabel blockNameLabel = this.m_blockNameLabel;
          blockNameLabel.Position = blockNameLabel.Position - new Vector2(0.0f, this.m_blockIconImage.Size.Y * 0.5f);
        }
        this.m_blockTypeLabel.Visible = true;
        this.m_blockTypeLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
        this.m_blockTypeLabel.TextScale = this.m_smallerFontSize * this.baseScale;
        this.m_blockTypeLabel.Position = this.m_blockIconImage.Position + new Vector2(this.m_blockIconImage.Size.X, 0.0f) + new Vector2(0.004f, -1f / 400f);
        this.m_componentsLabel.Position = vector2_4 + new Vector2(3f / 500f, (float) (0.0149999996647239 + (double) num2 * (double) this.baseScale));
        this.m_installedRequiredLabel.Position = vector2_2 + new Vector2(-11f / 1000f, (float) (0.0149999996647239 + (double) num2 * (double) this.baseScale));
      }
      this.m_blockIconImage.Position = vector2_1 + new Vector2(0.005f, 0.005f);
      Vector2 vector2_8 = !this.m_progressMode ? vector2_1 + new Vector2(0.008f, (float) (0.0120000001043081 + (double) this.m_componentsLabel.Size.Y + (double) num2 * (double) this.baseScale)) : vector2_4 + new Vector2(0.0f, (float) (0.0120000001043081 + (double) this.m_componentsLabel.Size.Y + (double) num2 * (double) this.baseScale));
      for (int index = 0; index < this.BlockInfo.Components.Count; ++index)
      {
        this.m_componentLines[index].Position = vector2_8 + new Vector2(0.0f, (float) (this.BlockInfo.Components.Count - index - 1) * this.itemHeight);
        this.m_componentLines[index].IconPanelProgress.Visible = MyGuiControlBlockInfo.ShowComponentProgress;
        this.m_componentLines[index].IconImage.BorderColor = MyGuiControlBlockInfo.CriticalComponentColor;
        this.m_componentLines[index].NameLabel.TextScale = (float) ((double) this.m_smallerFontSize * (double) this.baseScale * 0.899999976158142);
        this.m_componentLines[index].NumbersLabel.TextScale = (float) ((double) this.m_smallerFontSize * (double) this.baseScale * 0.899999976158142);
        this.m_componentLines[index].NumbersLabel.PositionX = this.m_installedRequiredLabel.PositionX - this.m_installedRequiredLabel.Size.X * 0.1f;
        if (this.m_progressMode)
        {
          this.m_componentLines[index].IconImage.BackgroundTexture = (MyGuiCompositeTexture) null;
          this.m_componentLines[index].NameLabel.PositionX = (float) (-(double) this.m_componentLines[index].Size.X / 2.0 + (double) this.m_componentLines[index].IconImage.Size.X - 3.0 / 500.0);
        }
      }
    }

    public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
    {
      if (this.BlockInfo != null && this.BlockInfo.Version != this.m_lastInfoStamp)
      {
        this.m_lastInfoStamp = this.BlockInfo.Version;
        this.EnsureLineControls(this.BlockInfo.Components.Count);
        this.Reposition();
        Vector2 vector2 = new Vector2((float) (-(double) this.Size.X / 2.0), 0.0f);
        for (int index = 0; index < this.m_componentLines.Count; ++index)
        {
          if (index < this.BlockInfo.Components.Count)
          {
            MyHudBlockInfo.ComponentInfo component = this.BlockInfo.Components[index];
            Vector4 vector4 = Vector4.One;
            string str;
            if (this.m_progressMode && (double) this.BlockInfo.BlockIntegrity > 0.0)
            {
              if (this.BlockInfo.MissingComponentIndex == index)
                str = this.m_style.ComponentLineMissingFont;
              else if (component.MountedCount == component.TotalCount)
                str = this.m_style.ComponentLineAllMountedFont;
              else if (component.InstalledCount == component.TotalCount)
              {
                str = this.m_style.ComponentLineAllInstalledFont;
              }
              else
              {
                str = this.m_style.ComponentLineDefaultFont;
                vector4 = this.m_style.ComponentLineDefaultColor;
              }
            }
            else
              str = this.m_style.ComponentLineDefaultFont;
            if (this.m_progressMode && (double) this.BlockInfo.BlockIntegrity > 0.0)
              this.m_componentLines[index].SetProgress((float) component.MountedCount / (float) component.TotalCount);
            else
              this.m_componentLines[index].SetProgress(1f);
            this.m_componentLines[index].Visible = true;
            this.m_componentLines[index].NameLabel.Font = str;
            if (this.m_progressMode)
            {
              this.m_componentLines[index].NameLabel.Position = vector2 + new Vector2(-0.005f, 0.0f);
              double x = (double) MyGuiManager.MeasureString(this.m_componentLines[index].NameLabel.Font, this.m_componentLines[index].NameLabel.TextToDraw, this.m_componentLines[index].NameLabel.TextScale).X;
              this.m_componentLines[index].NameLabel.TextScale = 0.6f;
            }
            this.m_componentLines[index].NameLabel.ColorMask = vector4;
            this.m_componentLines[index].NameLabel.TextToDraw.Clear();
            this.m_componentLines[index].NameLabel.TextToDraw.Append(component.ComponentName);
            this.m_componentLines[index].IconImage.SetTextures(component.Icons);
            this.m_componentLines[index].NumbersLabel.Font = str;
            this.m_componentLines[index].NumbersLabel.ColorMask = vector4;
            this.m_componentLines[index].NumbersLabel.TextToDraw.Clear();
            if (this.m_progressMode && (double) this.BlockInfo.BlockIntegrity > 0.0)
            {
              this.m_componentLines[index].NumbersLabel.TextToDraw.AppendInt32(component.InstalledCount).Append(" / ").AppendInt32(component.TotalCount);
              if (this.m_style.ShowAvailableComponents)
                this.m_componentLines[index].NumbersLabel.TextToDraw.Append(" / ").AppendInt32(component.AvailableAmount);
            }
            else if (this.BlockInfo.ShowAvailable)
            {
              this.m_componentLines[index].NumbersLabel.TextToDraw.AppendInt32(component.TotalCount);
              if (this.m_style.ShowAvailableComponents)
                this.m_componentLines[index].NumbersLabel.TextToDraw.Append(" / ").AppendInt32(component.AvailableAmount);
            }
            else
              this.m_componentLines[index].NumbersLabel.TextToDraw.AppendInt32(component.TotalCount);
            float num = 1f;
            if ((double) MyGuiManager.MeasureString(this.m_componentLines[index].NumbersLabel.Font, this.m_componentLines[index].NumbersLabel.TextToDraw, this.m_componentLines[index].NumbersLabel.TextScale).X > 0.0599999986588955)
              num = 0.8f;
            this.m_componentLines[index].NumbersLabel.TextScale = 0.6f;
            this.m_componentLines[index].NumbersLabel.TextScale *= num;
            this.m_componentLines[index].NumbersLabel.Size = this.m_componentLines[index].NumbersLabel.GetTextSize();
            this.m_componentLines[index].IconImage.BorderEnabled = MyGuiControlBlockInfo.ShowCriticalComponent && this.BlockInfo.CriticalComponentIndex == index;
            this.m_componentLines[index].RecalcTextSize(this.m_progressMode);
          }
          else
            this.m_componentLines[index].Visible = false;
        }
        this.m_blockNameLabel.TextToDraw.Clear();
        if (this.BlockInfo.BlockName != null)
          this.m_blockNameLabel.TextToDraw.Append(this.BlockInfo.BlockName);
        this.m_blockNameLabel.TextToDraw.ToUpper();
        this.m_blockNameLabel.Autowrap(0.25f);
        this.m_blockBuiltByLabel.TextToDraw.Clear();
        MyIdentity identity = MySession.Static.Players.TryGetIdentity(this.BlockInfo.BlockBuiltBy);
        if (identity != null)
        {
          this.m_blockBuiltByLabel.TextToDraw.Append(MyTexts.GetString(MyCommonTexts.BuiltBy));
          this.m_blockBuiltByLabel.TextToDraw.Append(": ");
          this.m_blockBuiltByLabel.TextToDraw.Append(identity.DisplayName);
        }
        if (this.m_progressMode)
          this.m_blockBuiltByLabel.Visible = false;
        this.m_blockIconImage.SetTextures(this.BlockInfo.BlockIcons);
        if (this.BlockInfo.Components.Count == 0)
        {
          this.m_installedRequiredLabel.Visible = false;
          this.m_componentsLabel.Visible = false;
        }
        else
        {
          this.m_installedRequiredLabel.Visible = true;
          this.m_componentsLabel.Visible = true;
        }
      }
      base.Draw(transitionAlpha, backgroundTransitionAlpha * MySandboxGame.Config.HUDBkOpacity);
      if (this.BlockInfo != null && this.m_integrityForeground != null)
        this.m_integrityForeground.Draw(this.BlockInfo.BlockIntegrity, this.m_integrityForegroundColorMask);
      if (this.m_separator != null)
        this.m_separator.Draw(transitionAlpha, backgroundTransitionAlpha);
      if (MyGuiControlBlockInfo.ShowCriticalIntegrity && this.m_criticalIntegrityLabel != null && this.m_criticalIntegrityLabel.Visible)
        this.m_criticalIntegrityLabel.Draw(transitionAlpha, backgroundTransitionAlpha);
      if (!MyGuiControlBlockInfo.ShowOwnershipIntegrity || this.m_ownershipIntegrityLabel == null || !this.m_ownershipIntegrityLabel.Visible)
        return;
      this.m_ownershipIntegrityLabel.Draw(transitionAlpha, backgroundTransitionAlpha);
    }

    public struct MyControlBlockInfoStyle
    {
      public Vector4 BackgroundColormask;
      public string BlockNameLabelFont;
      public MyStringId ComponentsLabelText;
      public string ComponentsLabelFont;
      public MyStringId InstalledRequiredLabelText;
      public string InstalledRequiredLabelFont;
      public MyStringId RequiredAvailableLabelText;
      public MyStringId RequiredLabelText;
      public string IntegrityLabelFont;
      public Vector4 IntegrityBackgroundColor;
      public Vector4 IntegrityForegroundColor;
      public Vector4 IntegrityForegroundColorOverCritical;
      public Vector4 LeftColumnBackgroundColor;
      public Vector4 TitleBackgroundColor;
      public Vector4 TitleSeparatorColor;
      public string ComponentLineMissingFont;
      public string ComponentLineAllMountedFont;
      public string ComponentLineAllInstalledFont;
      public string ComponentLineDefaultFont;
      public Vector4 ComponentLineDefaultColor;
      public bool EnableBlockTypeLabel;
      public bool ShowAvailableComponents;
      public bool EnableBlockTypePanel;
      public bool HiddenPCU;
      public bool HiddenHeader;
    }

    private class ComponentLineControl : MyGuiControlBase
    {
      public MyGuiControlImage IconImage;
      public MyGuiControlPanel IconPanelProgress;
      public MyGuiControlLabel NameLabel;
      public MyGuiControlLabel NumbersLabel;

      public ComponentLineControl(Vector2 size, float iconSize)
        : base(size: new Vector2?(size), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
      {
        Vector2 vector2_1 = new Vector2(iconSize) * new Vector2(0.75f, 1f);
        Vector2 vector2_2 = new Vector2((float) (-(double) this.Size.X / 2.0), 0.0f);
        Vector2 vector2_3 = new Vector2(this.Size.X / 2f, 0.0f);
        Vector2 vector2_4 = vector2_2 - new Vector2(0.0f, vector2_1.Y / 2f);
        this.IconImage = new MyGuiControlImage();
        this.IconPanelProgress = new MyGuiControlPanel();
        this.NameLabel = new MyGuiControlLabel(text: string.Empty);
        this.NumbersLabel = new MyGuiControlLabel(text: string.Empty);
        this.IconImage.Size = vector2_1;
        this.IconImage.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
        this.IconImage.Position = vector2_4;
        this.IconPanelProgress.Size = vector2_1;
        this.IconPanelProgress.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
        this.IconPanelProgress.Position = vector2_4;
        this.IconPanelProgress.BackgroundTexture = MyGuiConstants.TEXTURE_GUI_BLANK;
        float num = 0.1f;
        this.IconPanelProgress.ColorMask = new Vector4(num, num, num, 0.5f);
        this.NameLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
        this.NameLabel.Position = vector2_2 + new Vector2(vector2_1.X + 0.01225f, 0.0f);
        this.NameLabel.IsAutoEllipsisEnabled = true;
        this.NameLabel.IsAutoScaleEnabled = true;
        this.NumbersLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
        this.NumbersLabel.Position = vector2_3 + new Vector2(-0.033f, 0.0f);
        this.Elements.Add((MyGuiControlBase) this.IconImage);
        this.Elements.Add((MyGuiControlBase) this.IconPanelProgress);
        this.Elements.Add((MyGuiControlBase) this.NameLabel);
        this.Elements.Add((MyGuiControlBase) this.NumbersLabel);
      }

      public void RecalcTextSize(bool progressMode)
      {
        this.NameLabel.BorderEnabled = false;
        this.NumbersLabel.BorderEnabled = false;
        float num1 = 1f / 500f;
        if (progressMode)
          this.NumbersLabel.Position = new Vector2(this.Size.X / 2f, 0.0f) + new Vector2(-0.133f, 0.0f);
        float num2 = (float) ((double) this.NumbersLabel.Position.X - (double) this.NameLabel.Position.X - (double) this.NumbersLabel.Size.X - 2.0 * (double) num1);
        int num3 = 3;
        for (Vector2 vector2 = MyGuiManager.MeasureString(this.NameLabel.Font, this.NameLabel.TextToDraw, this.NameLabel.TextScale); num3 > 0 && (double) vector2.X > (double) num2; --num3)
        {
          this.NameLabel.TextScale *= 0.9f;
          vector2 = MyGuiManager.MeasureString(this.NameLabel.Font, this.NameLabel.TextToDraw, this.NameLabel.TextScale);
        }
        this.NameLabel.Size = new Vector2(num2 + num1, this.NameLabel.Size.Y);
      }

      public void SetProgress(float val) => this.IconPanelProgress.Size = this.IconImage.Size * new Vector2(1f, 1f - val);
    }
  }
}
