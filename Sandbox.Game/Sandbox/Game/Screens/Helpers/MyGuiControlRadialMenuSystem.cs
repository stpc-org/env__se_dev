// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyGuiControlRadialMenuSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
  internal class MyGuiControlRadialMenuSystem : MyGuiControlRadialMenuBase
  {
    public MyGuiControlRadialMenuSystem(
      MyRadialMenu data,
      MyStringId closingControl,
      Func<bool> handleInputCallback)
      : base(data, closingControl, handleInputCallback)
    {
      this.SwitchSection(MyGuiControlRadialMenuBase.m_lastSelectedSection.GetValueOrDefault<MyDefinitionId, int>(data.Id, 0));
    }

    protected override void UpdateTooltip()
    {
      List<MyRadialMenuItem> items = this.m_data.CurrentSections[this.m_currentSection].Items;
      if (this.m_selectedButton >= 0 && this.m_selectedButton < items.Count)
      {
        MyRadialMenuItem myRadialMenuItem = items[this.m_selectedButton];
        MyRadialLabelText label = myRadialMenuItem.Label;
        this.m_tooltipName.Text = MyTexts.GetString(label.Name);
        this.m_tooltipState.Text = MyTexts.GetString(label.State);
        this.m_tooltipShortcut.Text = MyTexts.GetString(label.Shortcut);
        this.m_tooltipName.RecalculateSize();
        this.m_tooltipState.RecalculateSize();
        this.m_tooltipShortcut.RecalculateSize();
        Vector2 vector2_1 = this.m_icons[this.m_selectedButton].Position * 1.92f;
        Vector2 zero1 = Vector2.Zero;
        Vector2 vector2_2 = new Vector2(0.0f, 0.025f);
        Vector2 vector2_3 = Vector2.Zero;
        int num1 = (double) Math.Abs(vector2_1.X) < 0.05 ? 0 : (Math.Sign(vector2_1.X) < 0 ? -1 : 1);
        int num2 = (double) Math.Abs(vector2_1.Y) < 0.05 ? 0 : (Math.Sign(vector2_1.Y) < 0 ? -1 : 1);
        MyGuiDrawAlignEnum guiDrawAlignEnum1 = (MyGuiDrawAlignEnum) (3 * (-num1 + 1) + (-num2 + 1));
        MyGuiDrawAlignEnum guiDrawAlignEnum2 = (MyGuiDrawAlignEnum) (-num2 + 1);
        float num3 = MyGuiManager.MeasureString(this.m_tooltipShortcut.Font, this.m_tooltipShortcut.TextToDraw, this.m_tooltipShortcut.TextScale).Y + 0.005f;
        if (string.IsNullOrEmpty(this.m_tooltipState.Text))
          num3 -= vector2_2.Y;
        Vector2 vector2_4 = new Vector2();
        switch (num2)
        {
          case -1:
            if (!string.IsNullOrEmpty(label.State))
              zero1 -= vector2_2;
            if (!string.IsNullOrEmpty(label.Shortcut))
              zero1 -= vector2_2;
            vector2_4.Y += num3;
            break;
          case 0:
            if (!string.IsNullOrEmpty(label.State))
              zero1 -= vector2_2;
            vector2_4.Y += num3 * 0.75f;
            break;
          case 1:
            vector2_4.Y += num3 * 0.5f;
            break;
        }
        Vector2 vector2_5 = Vector2.Zero;
        Vector2 zero2 = Vector2.Zero;
        switch (num1)
        {
          case -1:
            vector2_3 = new Vector2(-this.m_tooltipName.Size.X, 0.0f);
            if ((double) this.m_tooltipState.Size.X > (double) this.m_tooltipName.Size.X)
              vector2_5 = new Vector2(this.m_tooltipName.Size.X - this.m_tooltipState.Size.X, 0.0f);
            if ((double) this.m_tooltipShortcut.Size.X > (double) this.m_tooltipName.Size.X)
            {
              Vector2 vector2_6 = new Vector2(this.m_tooltipName.Size.X - this.m_tooltipShortcut.Size.X, 0.0f);
              break;
            }
            break;
          case 0:
            vector2_3 = new Vector2(-0.5f * this.m_tooltipName.Size.X, 0.0f);
            break;
          case 1:
            vector2_3 = Vector2.Zero;
            break;
        }
        this.m_tooltipName.Position = vector2_1 + zero1;
        this.m_tooltipState.Position = this.m_tooltipName.Position + vector2_2 + vector2_3 + vector2_5;
        this.m_tooltipShortcut.Position = this.m_tooltipState.Position + vector2_4;
        this.m_tooltipName.OriginAlign = guiDrawAlignEnum1;
        this.m_tooltipState.OriginAlign = guiDrawAlignEnum2;
        this.m_tooltipShortcut.OriginAlign = guiDrawAlignEnum2;
        this.m_tooltipName.Visible = true;
        this.m_tooltipState.Visible = true;
        this.m_tooltipShortcut.Visible = true;
        this.m_tooltipState.ColorMask = (Vector4) (myRadialMenuItem.Enabled() ? Color.White : Color.Red);
      }
      else
      {
        this.m_tooltipName.Visible = false;
        this.m_tooltipState.Visible = false;
        this.m_tooltipShortcut.Visible = false;
      }
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      base.HandleInput(receivedFocusInThisUpdate);
      if (!MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.CANCEL_MOD1))
        return;
      this.Cancel();
    }
  }
}
