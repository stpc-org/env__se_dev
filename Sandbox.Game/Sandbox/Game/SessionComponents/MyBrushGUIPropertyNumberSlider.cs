// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyBrushGUIPropertyNumberSlider
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.SessionComponents
{
  public class MyBrushGUIPropertyNumberSlider : IMyVoxelBrushGUIProperty
  {
    public Action ValueChanged;
    public float Value;
    public float ValueMin;
    public float ValueMax;
    public float ValueStep;

    public MyGuiControlLabel Label { get; set; }

    public MyGuiControlLabel LabelValue { get; set; }

    public MyGuiControlSlider SliderValue { get; set; }

    public MyBrushGUIPropertyNumberSlider(
      float value,
      float valueMin,
      float valueMax,
      float valueStep,
      MyVoxelBrushGUIPropertyOrder order,
      MyStringId labelText)
    {
      Vector2 vector2_1 = new Vector2(-0.1f, -0.2f);
      Vector2 vector2_2 = new Vector2(0.16f, -0.2f);
      Vector2 vector2_3 = new Vector2(-0.1f, -0.173f);
      switch (order)
      {
        case MyVoxelBrushGUIPropertyOrder.Second:
          vector2_1.Y = -0.116f;
          vector2_2.Y = -0.116f;
          vector2_3.Y = -0.089f;
          break;
        case MyVoxelBrushGUIPropertyOrder.Third:
          vector2_1.Y = -0.032f;
          vector2_2.Y = -0.032f;
          vector2_3.Y = -0.005f;
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
      this.Label = myGuiControlLabel1;
      MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel();
      myGuiControlLabel2.Position = vector2_2;
      myGuiControlLabel2.Text = this.Value.ToString();
      myGuiControlLabel2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      this.LabelValue = myGuiControlLabel2;
      MyGuiControlSlider guiControlSlider = new MyGuiControlSlider();
      guiControlSlider.Position = vector2_3;
      guiControlSlider.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.SliderValue = guiControlSlider;
      this.SliderValue.Size = new Vector2(0.263f, 0.1f);
      this.SliderValue.MaxValue = this.ValueMax;
      this.SliderValue.Value = this.Value;
      this.SliderValue.MinValue = this.ValueMin;
      this.SliderValue.ValueChanged += new Action<MyGuiControlSlider>(this.Slider_ValueChanged);
    }

    private void Slider_ValueChanged(MyGuiControlSlider sender)
    {
      float num = 1f / this.ValueStep;
      this.Value = MathHelper.Clamp((float) (int) (this.SliderValue.Value * num) / num, this.ValueMin, this.ValueMax);
      this.LabelValue.Text = this.Value.ToString();
      if (this.ValueChanged == null)
        return;
      this.ValueChanged();
    }

    public void AddControlsToList(List<MyGuiControlBase> list)
    {
      list.Add((MyGuiControlBase) this.Label);
      list.Add((MyGuiControlBase) this.LabelValue);
      list.Add((MyGuiControlBase) this.SliderValue);
    }
  }
}
