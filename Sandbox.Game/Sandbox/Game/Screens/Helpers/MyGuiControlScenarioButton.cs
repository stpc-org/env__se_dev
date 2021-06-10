// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyGuiControlScenarioButton
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Graphics.GUI;
using System;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
  public class MyGuiControlScenarioButton : MyGuiControlRadioButton
  {
    private MyGuiControlLabel m_titleLabel;
    private MyGuiControlImage m_previewImage;

    public string Title => this.m_titleLabel.Text.ToString();

    public MyScenarioDefinition Scenario { get; private set; }

    public MyGuiControlScenarioButton(MyScenarioDefinition scenario)
      : base(key: MyDefinitionManager.Static.GetScenarioDefinitions().IndexOf(scenario))
    {
      this.VisualStyle = MyGuiControlRadioButtonStyleEnum.ScenarioButton;
      this.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.Scenario = scenario;
      this.m_titleLabel = new MyGuiControlLabel(text: scenario.DisplayNameText, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      this.m_previewImage = new MyGuiControlImage(textures: scenario.Icons, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      this.m_previewImage.Size = new MyGuiSizedTexture()
      {
        SizePx = new Vector2(229f, 128f)
      }.SizeGui;
      this.m_previewImage.BorderEnabled = true;
      this.m_previewImage.BorderColor = MyGuiConstants.THEMED_GUI_LINE_BORDER.ToVector4();
      this.SetToolTip(scenario.DescriptionText);
      this.Size = new Vector2(Math.Max(this.m_titleLabel.Size.X, this.m_previewImage.Size.X), this.m_titleLabel.Size.Y + this.m_previewImage.Size.Y);
      this.Elements.Add((MyGuiControlBase) this.m_titleLabel);
      this.Elements.Add((MyGuiControlBase) this.m_previewImage);
    }

    protected override void OnSizeChanged()
    {
      base.OnSizeChanged();
      this.UpdatePositions();
    }

    private void UpdatePositions()
    {
      this.m_titleLabel.Position = this.Size * -0.5f;
      this.m_previewImage.Position = this.m_titleLabel.Position + new Vector2(0.0f, this.m_titleLabel.Size.Y);
    }

    protected override void OnHasHighlightChanged()
    {
      base.OnHasHighlightChanged();
      if (this.HasHighlight)
      {
        this.m_titleLabel.Font = "White";
        this.m_previewImage.BorderColor = Vector4.One;
      }
      else
      {
        this.m_titleLabel.Font = "Blue";
        this.m_previewImage.BorderColor = MyGuiConstants.THEMED_GUI_LINE_BORDER.ToVector4();
      }
    }
  }
}
