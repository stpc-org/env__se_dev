// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyBrushGUIPropertyNumberCombo
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
  public class MyBrushGUIPropertyNumberCombo : IMyVoxelBrushGUIProperty
  {
    private MyGuiControlLabel m_label;
    private MyGuiControlCombobox m_combo;
    public Action ItemSelected;
    public long SelectedKey;

    public MyBrushGUIPropertyNumberCombo(MyVoxelBrushGUIPropertyOrder order, MyStringId labelText)
    {
      Vector2 vector2_1 = new Vector2(-0.1f, -0.2f);
      Vector2 vector2_2 = new Vector2(-0.1f, -0.173f);
      switch (order)
      {
        case MyVoxelBrushGUIPropertyOrder.Second:
          vector2_1.Y = -0.116f;
          vector2_2.Y = -0.089f;
          break;
        case MyVoxelBrushGUIPropertyOrder.Third:
          vector2_1.Y = -0.032f;
          vector2_2.Y = -0.005f;
          break;
      }
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel();
      myGuiControlLabel.Position = vector2_1;
      myGuiControlLabel.TextEnum = labelText;
      myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_label = myGuiControlLabel;
      this.m_combo = new MyGuiControlCombobox();
      this.m_combo.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_combo.Position = vector2_2;
      this.m_combo.Size = new Vector2(0.263f, 0.1f);
      this.m_combo.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.Combo_ItemSelected);
    }

    public void AddItem(long key, MyStringId text) => this.m_combo.AddItem(key, text);

    public void SelectItem(long key) => this.m_combo.SelectItemByKey(key);

    private void Combo_ItemSelected()
    {
      this.SelectedKey = this.m_combo.GetSelectedKey();
      if (this.ItemSelected == null)
        return;
      this.ItemSelected();
    }

    public void AddControlsToList(List<MyGuiControlBase> list)
    {
      list.Add((MyGuiControlBase) this.m_label);
      list.Add((MyGuiControlBase) this.m_combo);
    }
  }
}
