// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyTerminalControlFactory
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Cube;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Interfaces.Terminal;
using System;
using System.Collections.Generic;
using System.Reflection;
using VRage;
using VRage.Collections;

namespace Sandbox.Game.Gui
{
  public static class MyTerminalControlFactory
  {
    private static Dictionary<Type, MyTerminalControlFactory.BlockData> m_controls = new Dictionary<Type, MyTerminalControlFactory.BlockData>();
    private static FastResourceLock m_controlsLock = new FastResourceLock();

    public static bool AreControlsCreated<TBlock>() => MyTerminalControlFactory.m_controls.ContainsKey(typeof (TBlock));

    public static bool AreControlsCreated(Type blockType) => MyTerminalControlFactory.m_controls.ContainsKey(blockType);

    public static void EnsureControlsAreCreated(Type blockType)
    {
      MethodInfo method = blockType.GetMethod("CreateTerminalControls", BindingFlags.Static | BindingFlags.NonPublic);
      if (method == (MethodInfo) null)
        return;
      method.Invoke((object) null, new object[0]);
    }

    public static void AddBaseClass<TBlock, TBase>()
      where TBlock : TBase
      where TBase : MyTerminalBlock => MyTerminalControlFactory.AddBaseClass(typeof (TBase), MyTerminalControlFactory.GetList<TBlock>());

    public static void RemoveBaseClass<TBlock, TBase>()
      where TBlock : TBase
      where TBase : MyTerminalBlock => MyTerminalControlFactory.RemoveBaseClass(typeof (TBase), MyTerminalControlFactory.GetList<TBlock>());

    public static void RemoveAllBaseClass<TBlock>() where TBlock : MyTerminalBlock
    {
      MyTerminalControlFactory.BlockData list = MyTerminalControlFactory.GetList<TBlock>();
      for (Type baseType = typeof (TBlock).BaseType; baseType != (Type) null; baseType = baseType.BaseType)
        MyTerminalControlFactory.RemoveBaseClass(baseType, list);
    }

    public static void AddControl<TBlock>(int index, MyTerminalControl<TBlock> control) where TBlock : MyTerminalBlock
    {
      MyTerminalControlFactory.GetList<TBlock>().Controls.Insert(index, (ITerminalControl) control);
      MyTerminalControlFactory.AddActions<TBlock>(index, control);
    }

    public static void AddControl<TBlock>(MyTerminalControl<TBlock> control) where TBlock : MyTerminalBlock
    {
      MyTerminalControlFactory.GetList<TBlock>().Controls.Add((ITerminalControl) control);
      MyTerminalControlFactory.AddActions<TBlock>(control);
    }

    public static void AddControl<TBase, TBlock>(MyTerminalControl<TBase> control)
      where TBase : MyTerminalBlock
      where TBlock : TBase
    {
      MyTerminalControlFactory.GetList<TBlock>().Controls.Add((ITerminalControl) control);
      MyTerminalControlFactory.AddActions<TBase>(control);
    }

    public static void AddControl(Type blockType, ITerminalControl control) => MyTerminalControlFactory.GetList(blockType).Controls.Add(control);

    public static void AddAction<TBlock>(int index, MyTerminalAction<TBlock> Action) where TBlock : MyTerminalBlock => MyTerminalControlFactory.GetList<TBlock>().Actions.Insert(index, (ITerminalAction) Action);

    public static void AddAction<TBlock>(MyTerminalAction<TBlock> Action) where TBlock : MyTerminalBlock => MyTerminalControlFactory.GetList<TBlock>().Actions.Add((ITerminalAction) Action);

    public static void AddAction<TBase, TBlock>(MyTerminalAction<TBase> Action)
      where TBase : MyTerminalBlock
      where TBlock : TBase => MyTerminalControlFactory.GetList<TBlock>().Actions.Add((ITerminalAction) Action);

    public static void AddActions(Type blockType, ITerminalControl control)
    {
      if (control.Actions == null)
        return;
      foreach (ITerminalAction action in control.Actions)
        MyTerminalControlFactory.GetList(blockType).Actions.Add(action);
    }

    private static void AddActions<TBlock>(MyTerminalControl<TBlock> block) where TBlock : MyTerminalBlock
    {
      if (block.Actions == null)
        return;
      foreach (MyTerminalAction<TBlock> action in block.Actions)
        MyTerminalControlFactory.AddAction<TBlock>(action);
    }

    public static void RemoveControl<TBlock>(IMyTerminalControl item) => MyTerminalControlFactory.RemoveControl(typeof (TBlock), item);

    public static void RemoveControl(Type blockType, IMyTerminalControl controlItem)
    {
      MyUniqueList<ITerminalControl> controls = MyTerminalControlFactory.GetList(blockType).Controls;
      foreach (ITerminalControl terminalControl in controls)
      {
        if (terminalControl == (ITerminalControl) controlItem)
        {
          controls.Remove(terminalControl);
          break;
        }
      }
      ITerminalControl terminalControl1 = (ITerminalControl) controlItem;
      if (terminalControl1.Actions == null)
        return;
      foreach (ITerminalAction action in terminalControl1.Actions)
        MyTerminalControlFactory.GetList(blockType).Actions.Remove(action);
    }

    private static void AddActions<TBlock>(int index, MyTerminalControl<TBlock> block) where TBlock : MyTerminalBlock
    {
      if (block.Actions == null)
        return;
      foreach (MyTerminalAction<TBlock> action in block.Actions)
        MyTerminalControlFactory.AddAction<TBlock>(index++, action);
    }

    public static UniqueListReader<ITerminalControl> GetControls(
      Type blockType)
    {
      return MyTerminalControlFactory.GetList(blockType).Controls.Items;
    }

    public static UniqueListReader<ITerminalAction> GetActions(
      Type blockType)
    {
      return MyTerminalControlFactory.GetList(blockType).Actions.Items;
    }

    public static void GetControls(Type blockType, List<ITerminalControl> resultList)
    {
      foreach (ITerminalControl terminalControl in MyTerminalControlFactory.GetList(blockType).Controls.Items)
        resultList.Add(terminalControl);
    }

    public static void GetValueControls(Type blockType, List<ITerminalProperty> resultList)
    {
      foreach (ITerminalControl terminalControl in MyTerminalControlFactory.GetList(blockType).Controls.Items)
      {
        if (terminalControl is ITerminalProperty terminalProperty)
          resultList.Add(terminalProperty);
      }
    }

    public static void GetActions(Type blockType, List<ITerminalAction> resultList)
    {
      foreach (ITerminalAction terminalAction in MyTerminalControlFactory.GetList(blockType).Actions.Items)
        resultList.Add(terminalAction);
    }

    public static void GetControls<TBlock>(List<MyTerminalControl<TBlock>> resultList) where TBlock : MyTerminalBlock
    {
      foreach (ITerminalControl terminalControl in MyTerminalControlFactory.GetList<TBlock>().Controls.Items)
        resultList.Add((MyTerminalControl<TBlock>) terminalControl);
    }

    public static void GetValueControls<TBlock>(Type blockType, List<ITerminalProperty> resultList) where TBlock : MyTerminalBlock
    {
      foreach (ITerminalControl terminalControl in MyTerminalControlFactory.GetList<TBlock>().Controls.Items)
      {
        if (terminalControl is ITerminalProperty terminalProperty)
          resultList.Add(terminalProperty);
      }
    }

    public static void GetActions<TBlock>(List<MyTerminalAction<TBlock>> resultList) where TBlock : MyTerminalBlock
    {
      foreach (ITerminalAction terminalAction in MyTerminalControlFactory.GetList<TBlock>().Actions.Items)
        resultList.Add((MyTerminalAction<TBlock>) terminalAction);
    }

    public static void Unload() => MyTerminalControlFactory.m_controls.Clear();

    private static void RemoveBaseClass(
      Type baseClass,
      MyTerminalControlFactory.BlockData resultList)
    {
      MyTerminalControlFactory.BlockData blockData;
      if (!MyTerminalControlFactory.m_controls.TryGetValue(baseClass, out blockData))
        return;
      foreach (ITerminalControl terminalControl in blockData.Controls.Items)
        resultList.Controls.Remove(terminalControl);
      foreach (ITerminalAction terminalAction in blockData.Actions.Items)
        resultList.Actions.Remove(terminalAction);
    }

    private static void AddBaseClass(Type baseClass, MyTerminalControlFactory.BlockData resultList)
    {
      MethodInfo method = baseClass.GetMethod("CreateTerminalControls", BindingFlags.Static | BindingFlags.NonPublic);
      if (method != (MethodInfo) null)
        method.Invoke((object) null, new object[0]);
      MyTerminalControlFactory.BlockData blockData;
      if (!MyTerminalControlFactory.m_controls.TryGetValue(baseClass, out blockData))
        return;
      foreach (ITerminalControl terminalControl in blockData.Controls.Items)
        resultList.Controls.Add(terminalControl);
      foreach (ITerminalAction terminalAction in blockData.Actions.Items)
        resultList.Actions.Add(terminalAction);
    }

    private static MyTerminalControlFactory.BlockData GetList<TBlock>() => MyTerminalControlFactory.GetList(typeof (TBlock));

    internal static MyTerminalControlFactory.BlockData GetList(Type type)
    {
      MyTerminalControlFactory.BlockData blockData;
      if (!MyTerminalControlFactory.m_controls.TryGetValue(type, out blockData))
        blockData = MyTerminalControlFactory.InitializeControls(type);
      return blockData;
    }

    internal static MyTerminalControlFactory.BlockData InitializeControls(
      Type type)
    {
      MyTerminalControlFactory.BlockData resultList = new MyTerminalControlFactory.BlockData();
      Type key = type;
      using (MyTerminalControlFactory.m_controlsLock.AcquireExclusiveUsing())
        MyTerminalControlFactory.m_controls[key] = resultList;
      for (Type baseType = type.BaseType; baseType != (Type) null; baseType = baseType.BaseType)
        MyTerminalControlFactory.AddBaseClass(baseType, resultList);
      return resultList;
    }

    internal class BlockData
    {
      public MyUniqueList<ITerminalControl> Controls = new MyUniqueList<ITerminalControl>();
      public MyUniqueList<ITerminalAction> Actions = new MyUniqueList<ITerminalAction>();
    }
  }
}
