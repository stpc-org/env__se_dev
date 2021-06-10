// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyBrushGUIPropertyNumberSelect
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.SessionComponents
{
  public class MyBrushGUIPropertyNumberSelect : IMyVoxelBrushGUIProperty
  {
    private MyGuiControlButton m_lowerValue;
    private MyGuiControlButton m_upperValue;
    private MyGuiControlLabel m_label;
    private MyGuiControlLabel m_labelValue;
    public Action ValueIncreased;
    public Action ValueDecreased;
    public float Value;
    public float ValueMin;
    public float ValueMax;
    public float ValueStep;

    public MyBrushGUIPropertyNumberSelect(
      float value,
      float valueMin,
      float valueMax,
      float valueStep,
      MyVoxelBrushGUIPropertyOrder order,
      MyStringId labelText)
    {
      Vector2 vector2_1 = new Vector2(-0.1f, -0.15f);
      Vector2 vector2_2 = new Vector2(0.035f, -0.15f);
      Vector2 vector2_3 = new Vector2(0.0f, -0.1475f);
      Vector2 vector2_4 = new Vector2(0.08f, -0.1475f);
      switch (order)
      {
        case MyVoxelBrushGUIPropertyOrder.Second:
          vector2_1.Y = -0.07f;
          vector2_2.Y = -0.07f;
          vector2_3.Y = -0.0675f;
          vector2_4.Y = -0.0675f;
          break;
        case MyVoxelBrushGUIPropertyOrder.Third:
          vector2_1.Y = 0.01f;
          vector2_2.Y = 0.01f;
          vector2_3.Y = 0.0125f;
          vector2_4.Y = 0.0125f;
          break;
      }
      this.Value = value;
      this.ValueMin = valueMin;
      this.ValueMax = valueMax;
      this.ValueStep = valueStep;
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
      myGuiControlLabel1.Position = vector2_1;
      myGuiControlLabel1.TextEnum = labelText;
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_label = myGuiControlLabel1;
      MyGuiControlButton guiControlButton1 = new MyGuiControlButton();
      guiControlButton1.Position = vector2_3;
      guiControlButton1.VisualStyle = MyGuiControlButtonStyleEnum.ArrowLeft;
      guiControlButton1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_lowerValue = guiControlButton1;
      MyGuiControlButton guiControlButton2 = new MyGuiControlButton();
      guiControlButton2.Position = vector2_4;
      guiControlButton2.VisualStyle = MyGuiControlButtonStyleEnum.ArrowRight;
      guiControlButton2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_upperValue = guiControlButton2;
      MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel();
      myGuiControlLabel2.Position = vector2_2;
      myGuiControlLabel2.Text = this.Value.ToString();
      myGuiControlLabel2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_labelValue = myGuiControlLabel2;
      this.m_lowerValue.ButtonClicked += new Action<MyGuiControlButton>(this.LowerClicked);
      this.m_upperValue.ButtonClicked += new Action<MyGuiControlButton>(this.UpperClicked);
    }

    private void LowerClicked(MyGuiControlButton sender)
    {
      this.Value = MathHelper.Clamp(this.Value - this.ValueStep, this.ValueMin, this.ValueMax);
      this.m_labelValue.Text = this.Value.ToString();
      if (this.ValueDecreased == null)
        return;
      this.ValueDecreased();
    }

    private void UpperClicked(MyGuiControlButton sender)
    {
      this.Value = MathHelper.Clamp(this.Value + this.ValueStep, this.ValueMin, this.ValueMax);
      this.m_labelValue.Text = this.Value.ToString();
      if (this.ValueIncreased == null)
        return;
      this.ValueIncreased();
    }

    public void AddControlsToList(List<MyGuiControlBase> list)
    {
      list.Add((MyGuiControlBase) this.m_lowerValue);
      list.Add((MyGuiControlBase) this.m_upperValue);
      list.Add((MyGuiControlBase) this.m_label);
      list.Add((MyGuiControlBase) this.m_labelValue);
    }
  }
}
