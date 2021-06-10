// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyGuiControlComponentList
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Graphics.GUI;
using System;
using System.Globalization;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
  internal class MyGuiControlComponentList : MyGuiControlBase
  {
    private float m_currentOffsetFromTop;
    private MyGuiBorderThickness m_padding;
    private MyGuiControlLabel m_valuesLabel;

    public StringBuilder ValuesText
    {
      get => new StringBuilder(this.m_valuesLabel.Text);
      set => this.m_valuesLabel.Text = value.ToString();
    }

    public MyGuiControlComponentList()
      : base(isActiveControl: false)
    {
      this.m_padding = new MyGuiBorderThickness(0.02f, 0.008f);
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel();
      myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      myGuiControlLabel.TextScale = 0.6f;
      this.m_valuesLabel = myGuiControlLabel;
      this.Elements.Add((MyGuiControlBase) this.m_valuesLabel);
      this.UpdatePositions();
    }

    public MyGuiControlComponentList.ComponentControl this[int i] => (MyGuiControlComponentList.ComponentControl) this.Elements[i + 1];

    public int Count => this.Elements.Count - 1;

    public void Add(MyDefinitionId id, double val1, double val2, string font)
    {
      MyGuiControlComponentList.ComponentControl componentControl = new MyGuiControlComponentList.ComponentControl(id);
      componentControl.Size = new Vector2(this.Size.X - this.m_padding.HorizontalSum, componentControl.Size.Y);
      this.m_currentOffsetFromTop += componentControl.Size.Y;
      componentControl.Position = -0.5f * this.Size + new Vector2(this.m_padding.Left, this.m_currentOffsetFromTop);
      componentControl.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
      componentControl.ValuesFont = font;
      componentControl.SetValues(val1, val2);
      this.Elements.Add((MyGuiControlBase) componentControl);
    }

    public new void Clear()
    {
      this.Elements.Clear();
      this.Elements.Add((MyGuiControlBase) this.m_valuesLabel);
      this.m_currentOffsetFromTop = this.m_valuesLabel.Size.Y + this.m_padding.Top;
    }

    protected override void OnSizeChanged()
    {
      this.UpdatePositions();
      base.OnSizeChanged();
    }

    private void UpdatePositions()
    {
      this.m_valuesLabel.Position = this.Size * new Vector2(0.5f, -0.5f) + this.m_padding.TopRightOffset;
      this.m_currentOffsetFromTop = this.m_valuesLabel.Size.Y + this.m_padding.Top;
      foreach (MyGuiControlBase element in this.Elements)
      {
        if (element != this.m_valuesLabel)
        {
          float y = element.Size.Y;
          this.m_currentOffsetFromTop += y;
          element.Position = -0.5f * this.Size + new Vector2(this.m_padding.Left, this.m_currentOffsetFromTop);
          element.Size = new Vector2(this.Size.X - this.m_padding.HorizontalSum, y);
        }
      }
    }

    private class ItemIconControl : MyGuiControlBase
    {
      private static readonly float SCALE = 0.85f;

      internal ItemIconControl(MyPhysicalItemDefinition def)
        : base(size: new Vector2?(MyGuiConstants.TEXTURE_GRID_ITEM.SizeGui * MyGuiControlComponentList.ItemIconControl.SCALE), backgroundTexture: MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_BORDER, isActiveControl: false)
      {
        this.MinSize = this.MaxSize = this.Size;
        MyGuiBorderThickness guiBorderThickness = new MyGuiBorderThickness(1f / 400f, 1f / 1000f);
        if (def == null)
        {
          this.Elements.Add((MyGuiControlBase) new MyGuiControlPanel(size: new Vector2?(this.Size - guiBorderThickness.SizeChange), texture: MyGuiConstants.TEXTURE_ICON_FAKE.Texture));
        }
        else
        {
          for (int index = 0; index < def.Icons.Length; ++index)
            this.Elements.Add((MyGuiControlBase) new MyGuiControlPanel(size: new Vector2?(this.Size - guiBorderThickness.SizeChange), texture: def.Icons[0]));
          if (!def.IconSymbol.HasValue)
            return;
          this.Elements.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(-0.5f * this.Size + guiBorderThickness.TopLeftOffset), text: MyTexts.GetString(def.IconSymbol.Value), textScale: (MyGuiControlComponentList.ItemIconControl.SCALE * 0.75f), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP));
        }
      }
    }

    internal class ComponentControl : MyGuiControlBase
    {
      public readonly MyDefinitionId Id;
      private MyGuiControlComponentList.ItemIconControl m_iconControl;
      private MyGuiControlLabel m_nameLabel;
      private MyGuiControlLabel m_valuesLabel;

      internal ComponentControl(MyDefinitionId id)
        : base(size: new Vector2?(new Vector2(0.2f, MyGuiConstants.TEXTURE_GRID_ITEM.SizeGui.Y * 0.75f)), isActiveControl: false)
      {
        MyPhysicalItemDefinition def = (MyPhysicalItemDefinition) null;
        if (!id.TypeId.IsNull)
          def = MyDefinitionManager.Static.GetDefinition(id) as MyPhysicalItemDefinition;
        MyGuiControlComponentList.ItemIconControl itemIconControl = new MyGuiControlComponentList.ItemIconControl(def);
        itemIconControl.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
        this.m_iconControl = itemIconControl;
        this.m_nameLabel = new MyGuiControlLabel(text: (def != null ? def.DisplayNameText : "N/A"), textScale: 0.68f);
        this.m_valuesLabel = new MyGuiControlLabel(text: new StringBuilder("{0} / {1}").ToString(), textScale: 0.6f, font: "White", originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
        this.SetValues(99.0, 99.0);
        this.Elements.Add((MyGuiControlBase) this.m_iconControl);
        this.Elements.Add((MyGuiControlBase) this.m_nameLabel);
        this.Elements.Add((MyGuiControlBase) this.m_valuesLabel);
        this.MinSize = new Vector2(this.m_iconControl.MinSize.X + this.m_nameLabel.Size.X + this.m_valuesLabel.Size.X, this.m_iconControl.MinSize.Y);
      }

      protected override void OnSizeChanged()
      {
        this.m_iconControl.Position = this.Size * new Vector2(-0.5f, 0.0f);
        this.m_nameLabel.Position = this.m_iconControl.Position + new Vector2(this.m_iconControl.Size.X + 0.01f, 0.0f);
        this.m_valuesLabel.Position = this.Size * new Vector2(0.5f, 0.0f);
        this.UpdateNameLabelSize();
        base.OnSizeChanged();
      }

      public void SetValues(double val1, double val2)
      {
        this.m_valuesLabel.UpdateFormatParams((object) val1.ToString("N", (IFormatProvider) CultureInfo.InvariantCulture), (object) val2.ToString("N", (IFormatProvider) CultureInfo.InvariantCulture));
        this.UpdateNameLabelSize();
      }

      public string ValuesFont
      {
        set => this.m_valuesLabel.Font = value;
      }

      private void UpdateNameLabelSize() => this.m_nameLabel.Size = new Vector2(this.Size.X - (this.m_iconControl.Size.X + this.m_valuesLabel.Size.X), this.m_nameLabel.Size.Y);
    }
  }
}
