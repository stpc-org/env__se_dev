// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyTerminalControlListbox`1
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Cube;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Library.Collections;
using VRage.ModAPI;
using VRage.Utils;

namespace Sandbox.Game.Gui
{
  public class MyTerminalControlListbox<TBlock> : MyTerminalControl<TBlock>, ITerminalControlSync, IMyTerminalControlTitleTooltip, IMyTerminalControlListbox, IMyTerminalControl
    where TBlock : MyTerminalBlock
  {
    public MyStringId Title;
    public MyStringId Tooltip;
    public MyTerminalControlListbox<TBlock>.ListContentDelegate ListContent;
    public MyTerminalControlListbox<TBlock>.SelectItemDelegate ItemSelected;
    public MyTerminalControlListbox<TBlock>.SelectItemDelegate ItemDoubleClicked;
    private MyGuiControlListbox m_listbox;
    private bool m_enableMultiSelect;
    private int m_visibleRowsCount = 8;
    private bool m_keepScrolling = true;

    private bool KeepScrolling
    {
      get => this.m_keepScrolling;
      set => this.m_keepScrolling = value;
    }

    public MyTerminalControlListbox(
      string id,
      MyStringId title,
      MyStringId tooltip,
      bool multiSelect = false,
      int visibleRowsCount = 8)
      : base(id)
    {
      this.Title = title;
      this.Tooltip = tooltip;
      this.m_enableMultiSelect = multiSelect;
      this.m_visibleRowsCount = visibleRowsCount;
    }

    protected override MyGuiControlBase CreateGui()
    {
      MyGuiControlListbox guiControlListbox = new MyGuiControlListbox();
      guiControlListbox.VisualStyle = MyGuiControlListboxStyleEnum.Terminal;
      guiControlListbox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      guiControlListbox.VisibleRowsCount = this.m_visibleRowsCount;
      guiControlListbox.MultiSelect = this.m_enableMultiSelect;
      this.m_listbox = guiControlListbox;
      this.m_listbox.ItemsSelected += new Action<MyGuiControlListbox>(this.OnItemsSelected);
      this.m_listbox.ItemDoubleClicked += new Action<MyGuiControlListbox>(this.OnItemDoubleClicked);
      return (MyGuiControlBase) new MyGuiControlBlockProperty(MyTexts.GetString(this.Title), MyTexts.GetString(this.Tooltip), (MyGuiControlBase) this.m_listbox);
    }

    private void OnItemsSelected(MyGuiControlListbox obj)
    {
      if (this.ItemSelected == null || obj.SelectedItems.Count <= 0)
        return;
      foreach (TBlock targetBlock in this.TargetBlocks)
        this.ItemSelected(targetBlock, obj.SelectedItems);
    }

    private void OnItemDoubleClicked(MyGuiControlListbox obj)
    {
      if (this.ItemDoubleClicked == null || obj.SelectedItems.Count <= 0)
        return;
      foreach (TBlock targetBlock in this.TargetBlocks)
        this.ItemDoubleClicked(targetBlock, obj.SelectedItems);
    }

    MyStringId IMyTerminalControlTitleTooltip.Title
    {
      get => this.Title;
      set => this.Title = value;
    }

    MyStringId IMyTerminalControlTitleTooltip.Tooltip
    {
      get => this.Tooltip;
      set => this.Tooltip = value;
    }

    bool IMyTerminalControlListbox.Multiselect
    {
      get => this.m_enableMultiSelect;
      set => this.m_enableMultiSelect = value;
    }

    int IMyTerminalControlListbox.VisibleRowsCount
    {
      get => this.m_visibleRowsCount;
      set => this.m_visibleRowsCount = value;
    }

    Action<IMyTerminalBlock, List<MyTerminalControlListBoxItem>, List<MyTerminalControlListBoxItem>> IMyTerminalControlListbox.ListContent
    {
      set => this.ListContent = (MyTerminalControlListbox<TBlock>.ListContentDelegate) ((block, contentList, selectedList, focusedItem) =>
      {
        List<MyTerminalControlListBoxItem> controlListBoxItemList1 = new List<MyTerminalControlListBoxItem>();
        List<MyTerminalControlListBoxItem> controlListBoxItemList2 = new List<MyTerminalControlListBoxItem>();
        value((IMyTerminalBlock) block, controlListBoxItemList1, controlListBoxItemList2);
        foreach (MyTerminalControlListBoxItem controlListBoxItem in controlListBoxItemList1)
        {
          MyStringId myStringId = controlListBoxItem.Text;
          StringBuilder text = new StringBuilder(myStringId.ToString());
          myStringId = controlListBoxItem.Tooltip;
          string toolTip = myStringId.ToString();
          object userData = controlListBoxItem.UserData;
          MyGuiControlListbox.Item obj = new MyGuiControlListbox.Item(text, toolTip, userData: userData);
          contentList.Add(obj);
          if (controlListBoxItemList2.Contains(controlListBoxItem))
            selectedList.Add(obj);
        }
      });
    }

    Action<IMyTerminalBlock, List<MyTerminalControlListBoxItem>> IMyTerminalControlListbox.ItemSelected
    {
      set => this.ItemSelected = (MyTerminalControlListbox<TBlock>.SelectItemDelegate) ((block, selectedList) =>
      {
        List<MyTerminalControlListBoxItem> controlListBoxItemList = new List<MyTerminalControlListBoxItem>();
        foreach (MyGuiControlListbox.Item selected in selectedList)
        {
          string str = selected.ToolTip == null || selected.ToolTip.ToolTips.Count <= 0 ? (string) null : selected.ToolTip.ToolTips.First<MyColoredText>().ToString();
          MyTerminalControlListBoxItem controlListBoxItem = new MyTerminalControlListBoxItem(MyStringId.GetOrCompute(selected.Text.ToString()), MyStringId.GetOrCompute(str), selected.UserData);
          controlListBoxItemList.Add(controlListBoxItem);
        }
        value((IMyTerminalBlock) block, controlListBoxItemList);
      });
    }

    protected override void OnUpdateVisual()
    {
      base.OnUpdateVisual();
      TBlock firstBlock = this.FirstBlock;
      if ((object) firstBlock == null)
        return;
      float scrollPosition = this.m_listbox.GetScrollPosition();
      this.m_listbox.Items.Clear();
      this.m_listbox.SelectedItems.Clear();
      if (this.ListContent != null)
      {
        List<MyGuiControlListbox.Item> objList = new List<MyGuiControlListbox.Item>();
        this.ListContent(firstBlock, (ICollection<MyGuiControlListbox.Item>) this.m_listbox.Items, (ICollection<MyGuiControlListbox.Item>) this.m_listbox.SelectedItems, (ICollection<MyGuiControlListbox.Item>) objList);
        if (objList.Count > 0)
          this.m_listbox.FocusedItem = objList[0];
      }
      if ((double) scrollPosition <= (double) (this.m_listbox.Items.Count - this.m_listbox.VisibleRowsCount) + 1.0)
        this.m_listbox.SetScrollPosition(scrollPosition);
      else
        this.m_listbox.SetScrollPosition(0.0f);
    }

    public void Serialize(BitStream stream, MyTerminalBlock block)
    {
    }

    public delegate void ListContentDelegate(
      TBlock block,
      ICollection<MyGuiControlListbox.Item> listBoxContent,
      ICollection<MyGuiControlListbox.Item> listBoxSelectedItems,
      ICollection<MyGuiControlListbox.Item> lastFocused)
      where TBlock : MyTerminalBlock;

    public delegate void SelectItemDelegate(TBlock block, List<MyGuiControlListbox.Item> items) where TBlock : MyTerminalBlock;
  }
}
