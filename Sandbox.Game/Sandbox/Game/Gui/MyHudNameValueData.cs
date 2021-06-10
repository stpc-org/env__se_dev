// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyHudNameValueData
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public class MyHudNameValueData
  {
    private readonly List<MyHudNameValueData.Data> m_items;
    private int m_count;
    public string DefaultNameFont;
    public string DefaultValueFont;
    public float LineSpacing;
    public bool ShowBackgroundFog;

    public int Count
    {
      get => this.m_count;
      set
      {
        this.m_count = value;
        this.EnsureItemsExist();
      }
    }

    public int GetVisibleCount()
    {
      int num = 0;
      for (int index = 0; index < this.m_count; ++index)
      {
        if (this.m_items[index].Visible)
          ++num;
      }
      return num;
    }

    public float GetGuiHeight() => (float) (this.GetVisibleCount() + 1) * this.LineSpacing;

    public MyHudNameValueData.Data this[int i] => this.m_items[i];

    public MyHudNameValueData(
      int itemCount,
      string defaultNameFont = "Blue",
      string defaultValueFont = "White",
      float lineSpacing = 0.025f,
      bool showBackgroundFog = false)
    {
      this.DefaultNameFont = defaultNameFont;
      this.DefaultValueFont = defaultValueFont;
      this.LineSpacing = lineSpacing;
      this.m_count = itemCount;
      this.m_items = new List<MyHudNameValueData.Data>(itemCount);
      this.ShowBackgroundFog = showBackgroundFog;
      this.EnsureItemsExist();
    }

    public void DrawTopDown(Vector2 namesTopLeft, Vector2 valuesTopRight, float textScale)
    {
      Color white = Color.White;
      if (this.ShowBackgroundFog)
        this.DrawBackgroundFog(namesTopLeft, valuesTopRight, true);
      for (int index = 0; index < this.Count; ++index)
      {
        MyHudNameValueData.Data data = this.m_items[index];
        if (data.Visible)
        {
          MyGuiManager.DrawString(data.NameFont ?? this.DefaultNameFont, data.Name.ToString(), namesTopLeft, textScale, new Color?(white));
          MyGuiManager.DrawString(data.ValueFont ?? this.DefaultValueFont, data.Value.ToString(), valuesTopRight, textScale, new Color?(white), MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
          namesTopLeft.Y += this.LineSpacing;
          valuesTopRight.Y += this.LineSpacing;
        }
      }
    }

    public void DrawBottomUp(Vector2 namesBottomLeft, Vector2 valuesBottomRight, float textScale)
    {
      Color white = Color.White;
      if (this.ShowBackgroundFog)
        this.DrawBackgroundFog(namesBottomLeft, valuesBottomRight, false);
      for (int index = this.Count - 1; index >= 0; --index)
      {
        MyHudNameValueData.Data data = this.m_items[index];
        if (data.Visible)
        {
          MyGuiManager.DrawString(data.NameFont ?? this.DefaultNameFont, data.Name.ToString(), namesBottomLeft, textScale, new Color?(white), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
          MyGuiManager.DrawString(data.ValueFont ?? this.DefaultValueFont, data.Value.ToString(), valuesBottomRight, textScale, new Color?(white), MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM);
          namesBottomLeft.Y -= this.LineSpacing;
          valuesBottomRight.Y -= this.LineSpacing;
        }
      }
    }

    internal float ComputeMaxLineWidth(float textScale)
    {
      float val1 = 0.0f;
      for (int index = 0; index < this.Count; ++index)
      {
        MyHudNameValueData.Data data = this.m_items[index];
        string font1 = data.NameFont ?? this.DefaultNameFont;
        string font2 = data.ValueFont ?? this.DefaultValueFont;
        Vector2 vector2_1 = MyGuiManager.MeasureString(font1, data.Name, textScale);
        Vector2 vector2_2 = MyGuiManager.MeasureString(font2, data.Value, textScale);
        val1 = Math.Max(val1, vector2_1.X + vector2_2.X);
      }
      return val1;
    }

    private void DrawBackgroundFog(Vector2 namesTopLeft, Vector2 valuesTopRight, bool topDown)
    {
      float num1;
      int num2;
      int num3;
      int num4;
      if (topDown)
      {
        num1 = this.LineSpacing;
        num2 = 0;
        num3 = this.Count;
        num4 = 1;
      }
      else
      {
        num1 = -this.LineSpacing;
        num2 = this.Count - 1;
        num3 = -1;
        num4 = -1;
      }
      for (int index = num2; index != num3; index += num4)
      {
        if (this.m_items[index].Visible)
        {
          Vector2 position = new Vector2((float) (((double) namesTopLeft.X + (double) valuesTopRight.X) * 0.5), namesTopLeft.Y + 0.5f * num1);
          Vector2 textSize = new Vector2(Math.Abs(namesTopLeft.X - valuesTopRight.X), this.LineSpacing);
          MyGuiTextShadows.DrawShadow(ref position, ref textSize);
          namesTopLeft.Y += num1;
          valuesTopRight.Y += num1;
        }
      }
    }

    private void EnsureItemsExist()
    {
      this.m_items.Capacity = Math.Max(this.Count, this.m_items.Capacity);
      while (this.m_items.Count < this.Count)
        this.m_items.Add(new MyHudNameValueData.Data());
    }

    public class Data
    {
      public StringBuilder Name;
      public StringBuilder Value;
      public string NameFont;
      public string ValueFont;
      public bool Visible;

      public Data()
      {
        this.Name = new StringBuilder();
        this.Value = new StringBuilder();
        this.Visible = true;
      }
    }
  }
}
