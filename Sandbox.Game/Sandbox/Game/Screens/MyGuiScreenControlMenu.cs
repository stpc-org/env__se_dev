// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenControlMenu
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
  public class MyGuiScreenControlMenu : MyGuiScreenBase
  {
    private const float ITEM_SIZE = 0.03f;
    private MyGuiControlScrollablePanel m_scrollPanel;
    private List<MyGuiScreenControlMenu.MyGuiControlItem> m_items;
    private int m_selectedItem;
    private RectangleF m_itemsRect;

    public MyGuiScreenControlMenu()
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(0.4f, 0.7f)))
    {
      this.DrawMouseCursor = false;
      this.CanHideOthers = false;
      this.m_items = new List<MyGuiScreenControlMenu.MyGuiControlItem>();
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.AddCaption(MyCommonTexts.ScreenControlMenu_Title, captionScale: 1.3f);
      MyGuiControlParent guiControlParent = new MyGuiControlParent(size: new Vector2?(new Vector2(this.Size.Value.X - 0.05f, (float) this.m_items.Count * 0.03f)));
      this.m_scrollPanel = new MyGuiControlScrollablePanel((MyGuiControlBase) guiControlParent);
      this.m_scrollPanel.ScrollbarVEnabled = true;
      this.m_scrollPanel.ScrollBarVScale = 1f;
      MyGuiControlScrollablePanel scrollPanel = this.m_scrollPanel;
      Vector2? size = this.Size;
      double num1 = (double) size.Value.X - 0.0500000007450581;
      size = this.Size;
      double num2 = (double) size.Value.Y - 0.100000001490116;
      Vector2 vector2 = new Vector2((float) num1, (float) num2);
      scrollPanel.Size = vector2;
      this.m_scrollPanel.Position = new Vector2(0.0f, 0.05f);
      MyLayoutVertical myLayoutVertical = new MyLayoutVertical((IMyGuiControlsParent) guiControlParent, 20f);
      foreach (MyGuiScreenControlMenu.MyGuiControlItem myGuiControlItem in this.m_items)
        myLayoutVertical.Add((MyGuiControlBase) myGuiControlItem, MyAlignH.Left);
      this.m_itemsRect.Position = this.m_scrollPanel.GetPositionAbsoluteTopLeft();
      this.m_itemsRect.Size = new Vector2(this.Size.Value.X - 0.05f, this.Size.Value.Y - 0.1f);
      this.FocusedControl = (MyGuiControlBase) guiControlParent;
      this.m_selectedItem = this.m_items.Count != 0 ? 0 : -1;
      this.Controls.Add((MyGuiControlBase) this.m_scrollPanel);
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      if (MyInput.Static.IsNewKeyPressed(MyKeys.Up) || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_UP) || MyInput.Static.DeltaMouseScrollWheelValue() > 0)
      {
        this.UpdateSelectedItem(true);
        this.UpdateScroll();
      }
      else if (MyInput.Static.IsNewKeyPressed(MyKeys.Down) || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_DOWN) || MyInput.Static.DeltaMouseScrollWheelValue() < 0)
      {
        this.UpdateSelectedItem(false);
        this.UpdateScroll();
      }
      else if (MyInput.Static.IsNewKeyPressed(MyKeys.Escape) || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.CANCEL) || (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsSpace.CONTROL_MENU) || MyInput.Static.IsNewRightMousePressed()))
        this.Canceling();
      if (this.m_selectedItem == -1)
        return;
      if (MyInput.Static.IsNewKeyPressed(MyKeys.Right) || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_RIGHT))
        this.m_items[this.m_selectedItem].UpdateItem(MyGuiScreenControlMenu.ItemUpdateType.Next);
      else if (MyInput.Static.IsNewKeyPressed(MyKeys.Left) || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_LEFT))
      {
        this.m_items[this.m_selectedItem].UpdateItem(MyGuiScreenControlMenu.ItemUpdateType.Previous);
      }
      else
      {
        if (!MyInput.Static.IsNewKeyReleased(MyKeys.Enter) && !MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.ACCEPT) && !MyInput.Static.IsNewLeftMousePressed())
          return;
        this.m_items[this.m_selectedItem].UpdateItem(MyGuiScreenControlMenu.ItemUpdateType.Activate);
      }
    }

    public override bool Draw()
    {
      base.Draw();
      if (this.m_selectedItem == -1)
        return true;
      MyGuiScreenControlMenu.MyGuiControlItem myGuiControlItem = this.m_items[this.m_selectedItem];
      if (myGuiControlItem != null)
      {
        this.m_itemsRect.Position = this.m_scrollPanel.GetPositionAbsoluteTopLeft();
        using (MyGuiManager.UsingScissorRectangle(ref this.m_itemsRect))
        {
          Vector2 positionAbsoluteTopLeft = myGuiControlItem.GetPositionAbsoluteTopLeft();
          MyGuiManager.DrawSpriteBatch(MyGuiConstants.TEXTURE_HIGHLIGHT_DARK.Center.Texture, positionAbsoluteTopLeft, myGuiControlItem.Size, new Color(1f, 1f, 1f, 0.8f), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
        }
      }
      return true;
    }

    private void UpdateSelectedItem(bool up)
    {
      bool flag = false;
      if (up)
      {
        for (int index = 0; index < this.m_items.Count; ++index)
        {
          --this.m_selectedItem;
          if (this.m_selectedItem < 0)
            this.m_selectedItem = this.m_items.Count - 1;
          if (this.m_items[this.m_selectedItem].IsItemEnabled)
          {
            flag = true;
            break;
          }
        }
      }
      else
      {
        for (int index = 0; index < this.m_items.Count; ++index)
        {
          this.m_selectedItem = (this.m_selectedItem + 1) % this.m_items.Count;
          if (this.m_items[this.m_selectedItem].IsItemEnabled)
          {
            flag = true;
            break;
          }
        }
      }
      if (flag)
        return;
      this.m_selectedItem = -1;
    }

    private void UpdateScroll()
    {
      if (this.m_selectedItem == -1)
        return;
      MyGuiScreenControlMenu.MyGuiControlItem myGuiControlItem1 = this.m_items[this.m_selectedItem];
      MyGuiScreenControlMenu.MyGuiControlItem myGuiControlItem2 = this.m_items[this.m_items.Count - 1];
      Vector2 positionAbsoluteTopLeft = myGuiControlItem1.GetPositionAbsoluteTopLeft();
      Vector2 vector2 = myGuiControlItem2.GetPositionAbsoluteTopLeft() + myGuiControlItem2.Size;
      float y1 = this.m_scrollPanel.GetPositionAbsoluteTopLeft().Y;
      positionAbsoluteTopLeft.Y -= y1;
      vector2.Y -= y1;
      double y2 = (double) positionAbsoluteTopLeft.Y;
      float num1 = positionAbsoluteTopLeft.Y + myGuiControlItem1.Size.Y;
      double y3 = (double) vector2.Y;
      float num2 = (float) (y2 / y3) * this.m_scrollPanel.ScrolledAreaSize.Y;
      float num3 = num1 / vector2.Y * this.m_scrollPanel.ScrolledAreaSize.Y;
      if ((double) num2 < (double) this.m_scrollPanel.ScrollbarVPosition)
        this.m_scrollPanel.ScrollbarVPosition = num2;
      if ((double) num3 <= (double) this.m_scrollPanel.ScrollbarVPosition)
        return;
      this.m_scrollPanel.ScrollbarVPosition = num3;
    }

    public void AddItem(MyAbstractControlMenuItem item) => this.m_items.Add(new MyGuiScreenControlMenu.MyGuiControlItem(item, new Vector2?(new Vector2(this.Size.Value.X - 0.1f, 0.03f))));

    public void AddItems(params MyAbstractControlMenuItem[] items)
    {
      foreach (MyAbstractControlMenuItem abstractControlMenuItem in items)
        this.AddItem(abstractControlMenuItem);
    }

    public void ClearItems() => this.m_items.Clear();

    protected override void OnClosed() => MyGuiScreenGamePlay.ActiveGameplayScreen = (MyGuiScreenBase) null;

    public override string GetFriendlyName() => "Control menu screen";

    private enum ItemUpdateType
    {
      Activate,
      Next,
      Previous,
    }

    private class MyGuiControlItem : MyGuiControlParent
    {
      private MyAbstractControlMenuItem m_item;
      private MyGuiControlLabel m_label;
      private MyGuiControlLabel m_value;

      public bool IsItemEnabled => this.m_item.Enabled;

      public MyGuiControlItem(MyAbstractControlMenuItem item, Vector2? size = null)
        : base(size: size)
      {
        this.m_item = item;
        this.m_item.UpdateValue();
        this.m_label = new MyGuiControlLabel(text: item.ControlLabel);
        this.m_value = new MyGuiControlLabel(text: item.CurrentValue);
        new MyLayoutVertical((IMyGuiControlsParent) this, 28f).Add((MyGuiControlBase) this.m_label, (MyGuiControlBase) this.m_value);
      }

      public override MyGuiControlBase GetNextFocusControl(
        MyGuiControlBase currentFocusControl,
        MyDirection direction,
        bool page)
      {
        return this.HasFocus ? this.Owner.GetNextFocusControl((MyGuiControlBase) this, direction, page) : (MyGuiControlBase) this;
      }

      public override void Update()
      {
        base.Update();
        this.RefreshValueLabel();
        if (this.IsItemEnabled)
        {
          this.m_label.Enabled = true;
          this.m_value.Enabled = true;
        }
        else
        {
          this.m_label.Enabled = false;
          this.m_value.Enabled = false;
        }
      }

      private void RefreshValueLabel()
      {
        this.m_item.UpdateValue();
        this.m_value.Text = this.m_item.CurrentValue;
      }

      internal void UpdateItem(MyGuiScreenControlMenu.ItemUpdateType updateType)
      {
        switch (updateType)
        {
          case MyGuiScreenControlMenu.ItemUpdateType.Activate:
            if (this.m_item.Enabled)
            {
              this.m_item.Activate();
              break;
            }
            break;
          case MyGuiScreenControlMenu.ItemUpdateType.Next:
            this.m_item.Next();
            break;
          case MyGuiScreenControlMenu.ItemUpdateType.Previous:
            this.m_item.Previous();
            break;
        }
        this.RefreshValueLabel();
      }
    }
  }
}
