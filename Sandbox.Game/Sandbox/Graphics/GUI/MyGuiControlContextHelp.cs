// Decompiled with JetBrains decompiler
// Type: Sandbox.Graphics.GUI.MyGuiControlContextHelp
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
  public class MyGuiControlContextHelp : MyGuiControlBase
  {
    private MyGuiControlLabel m_blockTypeLabel;
    private MyGuiControlLabel m_blockNameLabel;
    private MyGuiControlImage m_blockIconImage;
    private MyGuiControlPanel m_blockTypePanelLarge;
    private MyGuiControlPanel m_blockTypePanelSmall;
    private MyGuiControlLabel m_blockBuiltByLabel;
    private MyGuiControlPanel m_titleBackground;
    private MyGuiControlPanel m_pcuBackground;
    private MyGuiControlImage m_PCUIcon;
    private MyGuiControlLabel m_PCULabel;
    private MyGuiControlMultilineText m_helpText;
    private bool m_progressMode;
    private MyGuiControlBlockInfo.MyControlBlockInfoStyle m_style;
    private float m_smallerFontSize = 0.83f;
    private bool m_dirty;
    private MyHudBlockInfo m_blockInfo;
    private bool m_showBuildInfo;

    private float baseScale => 0.83f;

    private float itemHeight => 0.037f * this.baseScale;

    public bool ShowJustTitle
    {
      set
      {
        if (value)
        {
          this.Size = new Vector2(0.225f, 0.1f);
          this.m_helpText.Visible = false;
        }
        else
        {
          this.Size = new Vector2(0.225f, 0.32f);
          this.m_helpText.Visible = true;
        }
      }
    }

    public MyHudBlockInfo BlockInfo
    {
      get => this.m_blockInfo;
      set
      {
        if (this.m_blockInfo == value)
          return;
        if (this.m_blockInfo != null)
          this.m_blockInfo.ContextHelpChanged -= new Action<string>(this.BlockInfoOnContextHelpChanged);
        this.m_blockInfo = value;
        if (this.m_blockInfo != null)
          this.m_blockInfo.ContextHelpChanged += new Action<string>(this.BlockInfoOnContextHelpChanged);
        this.m_dirty = true;
      }
    }

    protected override void OnPositionChanged()
    {
      base.OnPositionChanged();
      this.m_dirty = true;
    }

    public bool ShowBuildInfo
    {
      get => this.m_showBuildInfo;
      set
      {
        if (this.m_showBuildInfo == value)
          return;
        this.m_showBuildInfo = value;
        this.m_dirty = true;
      }
    }

    public MyGuiControlContextHelp(
      MyGuiControlBlockInfo.MyControlBlockInfoStyle style,
      bool progressMode = true,
      bool largeBlockInfo = true)
      : base(backgroundTexture: new MyGuiCompositeTexture("Textures\\GUI\\Blank.dds"))
    {
      this.m_style = style;
      this.m_progressMode = true;
      this.ColorMask = this.m_style.BackgroundColormask;
      this.m_titleBackground = new MyGuiControlPanel(backgroundColor: new Vector4?(Color.Red.ToVector4()));
      this.m_titleBackground.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_titleBackground.BackgroundTexture = new MyGuiCompositeTexture("Textures\\GUI\\Blank.dds");
      this.Elements.Add((MyGuiControlBase) this.m_titleBackground);
      this.m_blockIconImage = new MyGuiControlImage();
      this.m_blockIconImage.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_blockIconImage.BackgroundTexture = this.m_progressMode ? (MyGuiCompositeTexture) null : new MyGuiCompositeTexture(MyGuiConstants.TEXTURE_HUD_BG_MEDIUM_DEFAULT.Texture);
      this.m_blockIconImage.Size = this.m_progressMode ? new Vector2(0.066f) : new Vector2(0.04f);
      MyGuiControlImage blockIconImage = this.m_blockIconImage;
      blockIconImage.Size = blockIconImage.Size * new Vector2(0.75f, 1f);
      this.Elements.Add((MyGuiControlBase) this.m_blockIconImage);
      this.m_blockTypePanelLarge = new MyGuiControlPanel();
      this.m_blockTypePanelLarge.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      this.Elements.Add((MyGuiControlBase) this.m_blockTypePanelLarge);
      this.m_blockTypePanelSmall = new MyGuiControlPanel();
      this.m_blockTypePanelSmall.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      this.Elements.Add((MyGuiControlBase) this.m_blockTypePanelSmall);
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
      this.m_blockBuiltByLabel = new MyGuiControlLabel(text: string.Empty);
      this.m_blockBuiltByLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      this.m_blockBuiltByLabel.TextScale = this.m_smallerFontSize * this.baseScale;
      this.m_blockBuiltByLabel.Font = this.m_style.InstalledRequiredLabelFont;
      this.Elements.Add((MyGuiControlBase) this.m_blockBuiltByLabel);
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
      MyGuiControlMultilineText controlMultilineText = new MyGuiControlMultilineText(textScale: 0.68f, textBoxAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      controlMultilineText.Name = "HelpText";
      controlMultilineText.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_helpText = controlMultilineText;
      this.Elements.Add((MyGuiControlBase) this.m_helpText);
      this.Size = new Vector2(0.225f, 0.32f);
    }

    public void RecalculateSize()
    {
      this.m_helpText.Position = -this.Size / 2f + new Vector2(0.0f, this.m_titleBackground.Size.Y) + new Vector2(0.01f, 0.01f);
      this.m_helpText.Size = new Vector2(this.Size.X, (float) ((double) this.Size.Y - (double) this.m_titleBackground.Size.Y - 3.0 / 500.0));
      this.m_helpText.RefreshText(false);
      this.m_helpText.Text.Clear();
      this.m_helpText.Text.Append(this.m_blockInfo.ContextHelp);
      this.m_helpText.Parse();
    }

    private void Reposition()
    {
      Vector2 vector2_1 = -this.Size / 2f;
      Vector2 vector2_2 = new Vector2(this.Size.X / 2f, (float) (-(double) this.Size.Y / 2.0));
      Vector2 vector2_3 = new Vector2((float) (-(double) this.Size.X / 2.0), this.Size.Y / 2f);
      Vector2 vector2_4 = vector2_1 + (this.m_progressMode ? new Vector2(0.06f, 0.0f) : new Vector2(0.036f, 0.0f));
      float num1 = 0.072f * this.baseScale;
      this.m_blockIconImage.Position = vector2_1 + new Vector2(0.005f, 0.005f);
      Vector2 vector2_5 = new Vector2(0.0035f) * new Vector2(0.75f, 1f) * this.baseScale;
      if (!this.m_progressMode)
        vector2_5.Y *= 1f;
      this.m_titleBackground.Position = vector2_1;
      this.m_titleBackground.ColorMask = this.m_style.TitleBackgroundColor;
      float num2 = !this.m_progressMode ? (float) ((double) Math.Abs(vector2_1.Y - this.m_blockIconImage.Position.Y) + (double) this.m_blockIconImage.Size.Y + 3.0 / 1000.0) : Math.Abs(vector2_1.Y - this.m_blockIconImage.Position.Y) + this.m_blockIconImage.Size.Y;
      this.m_titleBackground.Size = new Vector2(vector2_2.X - this.m_titleBackground.Position.X, num2 + 3f / 1000f);
      this.RecalculateSize();
      if (this.m_progressMode)
      {
        this.m_blockNameLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
        float num3 = 0.16f;
        float scale = 0.81f;
        Vector2 vector2_6 = MyGuiManager.MeasureString(this.m_blockNameLabel.Font, this.m_blockNameLabel.TextToDraw, scale);
        this.m_blockNameLabel.TextScale = (double) vector2_6.X <= (double) num3 ? scale : scale * (num3 / vector2_6.X);
        this.m_blockNameLabel.Size = new Vector2(vector2_2.X - (float) ((double) this.m_blockIconImage.Position.X + (double) this.m_blockIconImage.Size.X + 0.00400000018998981), this.m_blockNameLabel.Size.Y);
        this.m_blockNameLabel.Position = new Vector2(vector2_4.X, this.m_blockIconImage.Position.Y + 0.022f);
        this.m_blockTypeLabel.Visible = false;
        if (this.ShowBuildInfo)
        {
          this.m_blockTypePanelLarge.Position = vector2_2 + new Vector2(-0.005f, 0.032f);
          this.m_blockTypePanelLarge.Size = this.m_progressMode ? new Vector2(0.05f) : new Vector2(0.04f);
          MyGuiControlPanel blockTypePanelLarge = this.m_blockTypePanelLarge;
          blockTypePanelLarge.Size = blockTypePanelLarge.Size * new Vector2(0.75f, 1f);
          this.m_blockTypePanelLarge.BackgroundTexture = MyGuiConstants.TEXTURE_HUD_GRID_LARGE;
          this.m_blockTypePanelLarge.Visible = true;
          this.m_blockTypePanelLarge.Enabled = this.m_blockInfo.GridSize == MyCubeSize.Large;
          this.m_blockTypePanelSmall.Position = vector2_2 + new Vector2(-0.005f, 0.032f);
          this.m_blockTypePanelSmall.Size = this.m_progressMode ? new Vector2(0.05f) : new Vector2(0.04f);
          MyGuiControlPanel blockTypePanelSmall = this.m_blockTypePanelSmall;
          blockTypePanelSmall.Size = blockTypePanelSmall.Size * new Vector2(0.75f, 1f);
          this.m_blockTypePanelSmall.BackgroundTexture = MyGuiConstants.TEXTURE_HUD_GRID_SMALL;
          this.m_blockTypePanelSmall.Visible = true;
          this.m_blockTypePanelSmall.Enabled = this.m_blockInfo.GridSize == MyCubeSize.Small;
        }
        else
        {
          this.m_blockTypePanelLarge.Visible = false;
          this.m_blockTypePanelSmall.Visible = false;
        }
      }
      if (this.ShowBuildInfo)
      {
        this.m_pcuBackground.Visible = true;
        this.m_PCUIcon.Visible = true;
        this.m_PCULabel.Visible = true;
        this.m_pcuBackground.Position = vector2_3 + new Vector2(0.0f, -0.03f);
        this.m_PCUIcon.Position = vector2_3 + new Vector2(0.0085f, -0.03f);
        this.m_PCULabel.Position = this.m_PCUIcon.Position + new Vector2(0.035f, 3f / 1000f);
        this.m_PCULabel.Text = "PCU: " + this.m_blockInfo.PCUCost.ToString();
      }
      else
      {
        this.m_pcuBackground.Visible = false;
        this.m_PCUIcon.Visible = false;
        this.m_PCULabel.Visible = false;
      }
    }

    public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
    {
      if (this.m_dirty)
      {
        this.m_dirty = false;
        this.m_pcuBackground.Visible = false;
        this.m_PCUIcon.Visible = false;
        this.m_PCULabel.Visible = false;
        if (this.m_blockInfo != null)
        {
          this.m_blockNameLabel.TextToDraw.Clear();
          if (this.m_blockInfo.BlockName != null)
            this.m_blockNameLabel.TextToDraw.Append(this.m_blockInfo.BlockName);
          this.m_blockNameLabel.TextToDraw.ToUpper();
          this.m_blockNameLabel.Autowrap(0.25f);
          this.Reposition();
          this.m_blockIconImage.SetTextures(this.m_blockInfo.BlockIcons);
          if (this.ShowBuildInfo)
          {
            this.m_blockBuiltByLabel.Visible = true;
            this.m_blockBuiltByLabel.Position = this.m_blockNameLabel.Position + new Vector2(0.0f, this.m_blockNameLabel.Size.Y + 0.0f);
            this.m_blockBuiltByLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
            this.m_blockBuiltByLabel.TextScale = 0.6f;
            this.m_blockBuiltByLabel.TextToDraw.Clear();
            MyIdentity identity = MySession.Static.Players.TryGetIdentity(this.m_blockInfo.BlockBuiltBy);
            if (identity != null)
            {
              this.m_blockBuiltByLabel.TextToDraw.Append(MyTexts.GetString(MyCommonTexts.BuiltBy));
              this.m_blockBuiltByLabel.TextToDraw.Append(": ");
              this.m_blockBuiltByLabel.TextToDraw.Append(identity.DisplayName);
            }
          }
          else
            this.m_blockBuiltByLabel.Visible = false;
        }
      }
      base.Draw(transitionAlpha, backgroundTransitionAlpha * MySandboxGame.Config.HUDBkOpacity);
    }

    private void BlockInfoOnContextHelpChanged(string obj) => this.m_dirty = true;
  }
}
