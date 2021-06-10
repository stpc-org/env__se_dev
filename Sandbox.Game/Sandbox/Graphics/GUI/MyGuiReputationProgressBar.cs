// Decompiled with JetBrains decompiler
// Type: Sandbox.Graphics.GUI.MyGuiReputationProgressBar
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Localization;
using VRage;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
  internal class MyGuiReputationProgressBar : MyGuiControlBase
  {
    public static readonly Color COL_BACKGROUND = new Color(0.25f, 0.3f, 0.35f, 1f);
    public static readonly Color COL_HOSTILE = new Color(228, 62, 62);
    public static readonly Color COL_NEUTRAL = new Color(149, 169, 179);
    public static readonly Color COL_FRIENDLY = new Color(101, 178, 91);
    public static readonly Color COL_BORDER = new Color(100, 120, 130);
    private static readonly string BAR_TEXTURE = "Textures/GUI/Controls/progressRect.dds";
    private static readonly float HEIGHT_BAR = 0.6f;
    private static readonly float HEIGHT_BORDER = 0.65f;
    private static readonly float THICKNESS_BORDER = 0.01f;
    private static readonly float OFFSET_DOWN_TEXT = 0.75f;
    private static readonly float SIZE_TEXT = 0.55f;
    private int m_current;
    private int m_repMin = -10;
    private int m_repMax = 10;
    private int m_border1 = -5;
    private int m_border2 = 5;
    private int m_offerBonus;
    private int m_orderBonus;
    private int m_offerBonusMax;
    private int m_orderBonusMax;
    private string m_tooltipText_Hostile;
    private string m_tooltipText_Neutral;
    private string m_tooltipText_Friendly;
    public Color ColorBackground;
    public Color ColorHostile;
    public Color ColorNeutral;
    public Color ColorFriendly;
    public Color ColorBorder;

    public MyGuiReputationProgressBar(
      Vector2? position = null,
      Vector2? size = null,
      Vector4? colorMask = null,
      float textScale = 0.8f,
      MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER)
      : base(position, size, colorMask, isActiveControl: false)
    {
      this.OriginAlign = originAlign;
      this.ColorBackground = MyGuiReputationProgressBar.COL_BACKGROUND;
      this.ColorHostile = MyGuiReputationProgressBar.COL_HOSTILE;
      this.ColorNeutral = MyGuiReputationProgressBar.COL_NEUTRAL;
      this.ColorFriendly = MyGuiReputationProgressBar.COL_FRIENDLY;
      this.ColorBorder = MyGuiReputationProgressBar.COL_BORDER;
      this.m_tooltipText_Hostile = string.Empty;
      this.m_tooltipText_Neutral = string.Empty;
      this.m_tooltipText_Friendly = string.Empty;
    }

    public void SetBonusValues(
      int offerBonus,
      int orderBonus,
      int offerbonusMax,
      int orderBonusMax)
    {
      this.m_offerBonus = offerBonus;
      this.m_orderBonus = orderBonus;
      this.m_offerBonusMax = offerbonusMax;
      this.m_orderBonusMax = orderBonusMax;
      this.UpdateTexts();
    }

    public void SetBorderValues(int min, int max, int border1, int border2)
    {
      this.m_repMin = min;
      this.m_repMax = max;
      this.m_border1 = border1;
      this.m_border2 = border2;
      this.UpdateTexts();
    }

    private void UpdateTexts()
    {
      this.m_tooltipText_Hostile = string.Format(MyTexts.GetString(MySpaceTexts.ReputationBat_Tooltip_Hostile), (object) this.m_repMin, (object) this.m_border1);
      this.m_tooltipText_Neutral = string.Format(MyTexts.GetString(MySpaceTexts.ReputationBat_Tooltip_Neutral), (object) this.m_border1, (object) this.m_border2);
      this.m_tooltipText_Friendly = string.Format(MyTexts.GetString(MySpaceTexts.ReputationBat_Tooltip_Friendly), (object) this.m_border2, (object) this.m_repMax, (object) this.m_offerBonus, (object) this.m_orderBonus, (object) this.m_offerBonusMax, (object) this.m_orderBonusMax);
    }

    public void SetCurrentValue(int value) => this.m_current = value;

    public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
    {
      base.Draw(transitionAlpha, backgroundTransitionAlpha);
      Vector2 bar1 = new Vector2((float) (this.m_border1 - this.m_repMin) / (float) (this.m_repMax - this.m_repMin) * this.Size.X, 0.0f);
      Vector2 bar2 = new Vector2((float) (this.m_border2 - this.m_repMin) / (float) (this.m_repMax - this.m_repMin) * this.Size.X, 0.0f);
      Color colBackground1 = MyGuiReputationProgressBar.COL_BACKGROUND;
      Color colBackground2 = MyGuiReputationProgressBar.COL_BACKGROUND;
      float num1 = this.m_current < this.m_repMin ? (float) this.m_repMin : (this.m_current > this.m_repMax ? (float) this.m_repMax : (float) this.m_current);
      float num2 = (num1 - (float) this.m_repMin) / (float) (this.m_repMax - this.m_repMin);
      Vector2 normalizedSize1 = new Vector2(this.Size.X, MyGuiReputationProgressBar.HEIGHT_BAR * this.Size.Y);
      Vector2 normalizedSize2 = new Vector2(num2 * this.Size.X, MyGuiReputationProgressBar.HEIGHT_BAR * this.Size.Y);
      Vector2 normalizedSize3 = new Vector2(MyGuiReputationProgressBar.THICKNESS_BORDER * this.Size.X, MyGuiReputationProgressBar.HEIGHT_BORDER * this.Size.Y);
      Vector2 vector2 = new Vector2(0.0f, (float) (0.5 * ((double) MyGuiReputationProgressBar.HEIGHT_BORDER - (double) MyGuiReputationProgressBar.HEIGHT_BAR)) * this.Size.Y);
      Color color = (double) num1 >= (double) this.m_border1 ? ((double) num1 >= (double) this.m_border2 ? MyGuiReputationProgressBar.COL_FRIENDLY : MyGuiReputationProgressBar.COL_NEUTRAL) : MyGuiReputationProgressBar.COL_HOSTILE;
      MyGuiManager.DrawSpriteBatch(MyGuiReputationProgressBar.BAR_TEXTURE, this.GetPositionAbsolute() + vector2, normalizedSize1, colBackground1, this.OriginAlign);
      MyGuiManager.DrawSpriteBatch(MyGuiReputationProgressBar.BAR_TEXTURE, this.GetPositionAbsolute() + vector2, normalizedSize2, color, this.OriginAlign);
      MyGuiManager.DrawSpriteBatch(MyGuiReputationProgressBar.BAR_TEXTURE, this.GetPositionAbsolute() + bar1, normalizedSize3, MyGuiReputationProgressBar.COL_BORDER, this.OriginAlign);
      MyGuiManager.DrawSpriteBatch(MyGuiReputationProgressBar.BAR_TEXTURE, this.GetPositionAbsolute() + bar2, normalizedSize3, MyGuiReputationProgressBar.COL_BORDER, this.OriginAlign);
      MyGuiManager.DrawString("Blue", string.Format("{0}", (object) this.m_repMin), this.GetPositionAbsolute() + new Vector2(0.0f, MyGuiReputationProgressBar.OFFSET_DOWN_TEXT * this.Size.Y), MyGuiReputationProgressBar.SIZE_TEXT, new Color?(MyGuiControlBase.ApplyColorMaskModifiers(this.ColorMask, this.Enabled, transitionAlpha)), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      MyGuiManager.DrawString("Blue", string.Format("{0}", (object) this.m_border1), this.GetPositionAbsolute() + new Vector2(bar1.X, MyGuiReputationProgressBar.OFFSET_DOWN_TEXT * this.Size.Y), MyGuiReputationProgressBar.SIZE_TEXT, new Color?(MyGuiControlBase.ApplyColorMaskModifiers(this.ColorMask, this.Enabled, transitionAlpha)), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
      MyGuiManager.DrawString("Blue", string.Format("{0}", (object) this.m_border2), this.GetPositionAbsolute() + new Vector2(bar2.X, MyGuiReputationProgressBar.OFFSET_DOWN_TEXT * this.Size.Y), MyGuiReputationProgressBar.SIZE_TEXT, new Color?(MyGuiControlBase.ApplyColorMaskModifiers(this.ColorMask, this.Enabled, transitionAlpha)), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
      MyGuiManager.DrawString("Blue", string.Format("{0}", (object) this.m_repMax), this.GetPositionAbsolute() + new Vector2(this.Size.X, MyGuiReputationProgressBar.OFFSET_DOWN_TEXT * this.Size.Y), MyGuiReputationProgressBar.SIZE_TEXT, new Color?(MyGuiControlBase.ApplyColorMaskModifiers(this.ColorMask, this.Enabled, transitionAlpha)), MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
      this.UpdateAndDrawTooltip(bar1, bar2, new Vector2(this.Size.X - bar2.X, 0.0f), MyGuiReputationProgressBar.HEIGHT_BAR);
    }

    private void UpdateAndDrawTooltip(Vector2 bar1, Vector2 bar2, Vector2 bar3, float height)
    {
      if (!MyGuiControlBase.CheckMouseOver(this.Size, this.GetPositionAbsolute(), this.OriginAlign))
      {
        this.m_showToolTip = true;
      }
      else
      {
        Vector2 positionAbsolute = this.GetPositionAbsolute();
        Vector2 size1 = bar1;
        size1.Y = height;
        Vector2 position1 = this.GetPositionAbsolute() + bar1;
        Vector2 size2 = bar2 - bar1;
        size2.Y = height;
        Vector2 position2 = this.GetPositionAbsolute() + bar2;
        Vector2 size3 = bar3;
        size3.Y = height;
        string toolTip = "Tooltip";
        if (MyGuiControlBase.CheckMouseOver(size1, positionAbsolute, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP))
          toolTip = this.m_tooltipText_Hostile;
        if (MyGuiControlBase.CheckMouseOver(size2, position1, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP))
          toolTip = this.m_tooltipText_Neutral;
        if (MyGuiControlBase.CheckMouseOver(size3, position2, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP))
          toolTip = this.m_tooltipText_Friendly;
        this.m_toolTip = new MyToolTips(toolTip);
        this.m_showToolTip = true;
      }
    }

    private void DebugDraw() => MyGuiManager.DrawBorders(this.GetPositionAbsoluteTopLeft() + this.Position, this.Size, Color.White, 1);
  }
}
