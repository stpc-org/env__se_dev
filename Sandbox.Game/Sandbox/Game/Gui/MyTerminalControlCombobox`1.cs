// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyTerminalControlCombobox`1
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Interfaces.Terminal;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Library.Collections;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public class MyTerminalControlCombobox<TBlock> : MyTerminalValueControl<TBlock, long>, IMyTerminalControlCombobox, IMyTerminalControl, IMyTerminalValueControl<long>, ITerminalProperty, IMyTerminalControlTitleTooltip
    where TBlock : MyTerminalBlock
  {
    private static List<MyTerminalControlComboBoxItem> m_handlerItems = new List<MyTerminalControlComboBoxItem>();
    public MyStringId Title;
    public MyStringId Tooltip;
    private MyGuiControlCombobox m_comboBox;
    public MyTerminalControlCombobox<TBlock>.ComboBoxContentDelegate ComboBoxContentWithBlock;
    public Action<List<MyTerminalControlComboBoxItem>> ComboBoxContent;

    public MyTerminalControlCombobox(string id, MyStringId title, MyStringId tooltip)
      : base(id)
    {
      this.Title = title;
      this.Tooltip = tooltip;
      this.SetSerializerDefault();
    }

    public void SetSerializerDefault() => this.Serializer = (MyTerminalValueControl<TBlock, long>.SerializerDelegate) ((BitStream stream, ref long value) => stream.Serialize(ref value));

    public void SetSerializerBit() => this.Serializer = (MyTerminalValueControl<TBlock, long>.SerializerDelegate) ((BitStream stream, ref long value) =>
    {
      if (stream.Reading)
        value = stream.ReadBool() ? 1L : 0L;
      else
        stream.WriteBool((ulong) value > 0UL);
    });

    public void SetSerializerRange(int minInclusive, int maxInclusive)
    {
      int bitCount = MathHelper.Log2(MathHelper.GetNearestBiggerPowerOfTwo((uint) ((ulong) maxInclusive - (ulong) minInclusive + 1UL)));
      this.Serializer = (MyTerminalValueControl<TBlock, long>.SerializerDelegate) ((BitStream stream, ref long value) =>
      {
        if (stream.Reading)
          value = (long) stream.ReadUInt64() + (long) minInclusive;
        else
          stream.WriteUInt64((ulong) value - (ulong) minInclusive, bitCount);
      });
    }

    public void SetSerializerVariant(bool usesNegativeValues = false)
    {
      if (usesNegativeValues)
        this.Serializer = (MyTerminalValueControl<TBlock, long>.SerializerDelegate) ((BitStream stream, ref long value) => stream.SerializeVariant(ref value));
      else
        this.Serializer = (MyTerminalValueControl<TBlock, long>.SerializerDelegate) ((BitStream stream, ref long value) =>
        {
          if (stream.Reading)
            value = stream.ReadInt64();
          else
            stream.WriteInt64(value);
        });
    }

    protected override MyGuiControlBase CreateGui()
    {
      Vector2? position = new Vector2?();
      string str = MyTexts.GetString(this.Tooltip);
      Vector2? size = new Vector2?(new Vector2(0.23f, 0.04f));
      Vector4? backgroundColor = new Vector4?();
      Vector2? textOffset = new Vector2?();
      Vector2? iconSize = new Vector2?();
      string toolTip = str;
      Vector4? textColor = new Vector4?();
      this.m_comboBox = new MyGuiControlCombobox(position, size, backgroundColor, textOffset, iconSize: iconSize, toolTip: toolTip, textColor: textColor);
      this.m_comboBox.VisualStyle = MyGuiControlComboboxStyleEnum.Terminal;
      this.m_comboBox.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.OnItemSelected);
      return (MyGuiControlBase) new MyGuiControlBlockProperty(MyTexts.GetString(this.Title), MyTexts.GetString(this.Tooltip), (MyGuiControlBase) this.m_comboBox);
    }

    public override void SetValue(TBlock block, long value)
    {
      if (this.Getter(block) == value)
        return;
      base.SetValue(block, value);
    }

    private void OnItemSelected()
    {
      if (this.m_comboBox.GetItemsCount() <= 0)
        return;
      long selectedKey = this.m_comboBox.GetSelectedKey();
      foreach (TBlock targetBlock in this.TargetBlocks)
        this.SetValue(targetBlock, selectedKey);
    }

    protected override void OnUpdateVisual()
    {
      base.OnUpdateVisual();
      TBlock firstBlock = this.FirstBlock;
      if (this.m_comboBox.IsOpen || (object) firstBlock == null)
        return;
      this.m_comboBox.ClearItems();
      MyTerminalControlCombobox<TBlock>.m_handlerItems.Clear();
      if (this.ComboBoxContent != null)
      {
        this.ComboBoxContent(MyTerminalControlCombobox<TBlock>.m_handlerItems);
        foreach (MyTerminalControlComboBoxItem handlerItem in MyTerminalControlCombobox<TBlock>.m_handlerItems)
          this.m_comboBox.AddItem(handlerItem.Key, handlerItem.Value);
        long key = this.GetValue(firstBlock);
        if (this.m_comboBox.GetSelectedKey() != key)
        {
          this.m_comboBox.ItemSelected -= new MyGuiControlCombobox.ItemSelectedDelegate(this.OnItemSelected);
          this.m_comboBox.SelectItemByKey(key);
          this.m_comboBox.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.OnItemSelected);
        }
      }
      if (this.ComboBoxContentWithBlock == null)
        return;
      this.ComboBoxContentWithBlock(firstBlock, (ICollection<MyTerminalControlComboBoxItem>) MyTerminalControlCombobox<TBlock>.m_handlerItems);
      foreach (MyTerminalControlComboBoxItem handlerItem in MyTerminalControlCombobox<TBlock>.m_handlerItems)
        this.m_comboBox.AddItem(handlerItem.Key, handlerItem.Value);
      long key1 = this.GetValue(firstBlock);
      if (this.m_comboBox.GetSelectedKey() == key1)
        return;
      this.m_comboBox.SelectItemByKey(key1);
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

    Action<List<MyTerminalControlComboBoxItem>> IMyTerminalControlCombobox.ComboBoxContent
    {
      get
      {
        Action<List<MyTerminalControlComboBoxItem>> oldComboBoxContent = this.ComboBoxContent;
        return (Action<List<MyTerminalControlComboBoxItem>>) (x => oldComboBoxContent(x));
      }
      set => this.ComboBoxContent = value;
    }

    private Action<IMyTerminalBlock, List<MyTerminalControlComboBoxItem>> ComboBoxContentWithBlockAction
    {
      set => this.ComboBoxContentWithBlock = (MyTerminalControlCombobox<TBlock>.ComboBoxContentDelegate) ((block, comboBoxContent) =>
      {
        List<MyTerminalControlComboBoxItem> controlComboBoxItemList = new List<MyTerminalControlComboBoxItem>();
        value((IMyTerminalBlock) block, controlComboBoxItemList);
        foreach (MyTerminalControlComboBoxItem controlComboBoxItem1 in controlComboBoxItemList)
        {
          MyTerminalControlComboBoxItem controlComboBoxItem2 = new MyTerminalControlComboBoxItem()
          {
            Key = controlComboBoxItem1.Key,
            Value = controlComboBoxItem1.Value
          };
          comboBoxContent.Add(controlComboBoxItem2);
        }
      });
    }

    public override long GetDefaultValue(TBlock block) => this.GetMinimum(block);

    public override long GetMinimum(TBlock block)
    {
      long num = 0;
      if (this.ComboBoxContent != null)
      {
        MyTerminalControlCombobox<TBlock>.m_handlerItems.Clear();
        this.ComboBoxContent(MyTerminalControlCombobox<TBlock>.m_handlerItems);
        if (MyTerminalControlCombobox<TBlock>.m_handlerItems.Count > 0)
          num = MyTerminalControlCombobox<TBlock>.m_handlerItems[0].Key;
      }
      return num;
    }

    public override long GetMaximum(TBlock block)
    {
      long num = 0;
      if (this.ComboBoxContent != null)
      {
        MyTerminalControlCombobox<TBlock>.m_handlerItems.Clear();
        this.ComboBoxContent(MyTerminalControlCombobox<TBlock>.m_handlerItems);
        if (MyTerminalControlCombobox<TBlock>.m_handlerItems.Count > 0)
          num = MyTerminalControlCombobox<TBlock>.m_handlerItems[MyTerminalControlCombobox<TBlock>.m_handlerItems.Count - 1].Key;
      }
      return num;
    }

    public delegate void ComboBoxContentDelegate(
      TBlock block,
      ICollection<MyTerminalControlComboBoxItem> comboBoxContent)
      where TBlock : MyTerminalBlock;
  }
}
