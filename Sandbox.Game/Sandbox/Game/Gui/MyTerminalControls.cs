// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyTerminalControls
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.ObjectBuilders;
using VRage.Plugins;
using VRage.Utils;

namespace Sandbox.Game.Gui
{
  [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
  public class MyTerminalControls : MySessionComponentBase, IMyTerminalControls
  {
    private static MyTerminalControls m_instance;
    private Dictionary<Type, Type> m_interfaceCache = new Dictionary<Type, Type>();

    public static MyTerminalControls Static => MyTerminalControls.m_instance;

    public MyTerminalControls()
    {
      MyTerminalControls.m_instance = this;
      MyTerminalControls.m_instance.ScanAssembly(MyPlugins.SandboxGameAssembly);
      MyTerminalControls.m_instance.ScanAssembly(MyPlugins.GameAssembly);
    }

    protected override void UnloadData()
    {
      this.m_customControlGetter = (CustomControlGetDelegate) null;
      MyTerminalControls.m_instance = (MyTerminalControls) null;
    }

    private event CustomControlGetDelegate m_customControlGetter;

    public event CustomControlGetDelegate CustomControlGetter
    {
      remove => this.m_customControlGetter -= value;
      add => this.m_customControlGetter += value;
    }

    private event CustomActionGetDelegate m_customActionGetter;

    public event CustomActionGetDelegate CustomActionGetter
    {
      remove => this.m_customActionGetter -= value;
      add => this.m_customActionGetter += value;
    }

    public List<ITerminalControl> GetControls(Sandbox.ModAPI.IMyTerminalBlock block)
    {
      if (this.m_customControlGetter == null)
        return MyTerminalControlFactory.GetControls(block.GetType()).ToList<ITerminalControl>();
      List<IMyTerminalControl> list = MyTerminalControlFactory.GetControls(block.GetType()).Cast<IMyTerminalControl>().ToList<IMyTerminalControl>();
      this.m_customControlGetter(block, list);
      return list.Cast<ITerminalControl>().ToList<ITerminalControl>();
    }

    public void GetControls<TBlock>(out List<IMyTerminalControl> items)
    {
      items = new List<IMyTerminalControl>();
      if (!this.IsTypeValid<TBlock>())
        return;
      Type producedType = this.GetProducedType<TBlock>();
      if (producedType == (Type) null)
        return;
      foreach (ITerminalControl control in MyTerminalControlFactory.GetList(producedType).Controls)
        items.Add((IMyTerminalControl) control);
    }

    public void AddControl<TBlock>(IMyTerminalControl item)
    {
      if (!this.IsTypeValid<TBlock>())
        return;
      Type producedType = this.GetProducedType<TBlock>();
      if (producedType == (Type) null)
        return;
      MyTerminalControlFactory.AddControl(producedType, (ITerminalControl) item);
      MyTerminalControlFactory.AddActions(producedType, (ITerminalControl) item);
    }

    public void RemoveControl<TBlock>(IMyTerminalControl item)
    {
      if (!this.IsTypeValid<TBlock>())
        return;
      Type producedType = this.GetProducedType<TBlock>();
      if (producedType == (Type) null)
        return;
      MyTerminalControlFactory.RemoveControl(producedType, item);
    }

    public TControl CreateControl<TControl, TBlock>(string id)
    {
      if (!this.IsTypeValid<TBlock>())
        return default (TControl);
      Type producedType = this.GetProducedType<TBlock>();
      if (producedType == (Type) null)
        return default (TControl);
      if (!typeof (MyTerminalBlock).IsAssignableFrom(producedType))
        return default (TControl);
      if (!typeof (IMyTerminalControl).IsAssignableFrom(typeof (TControl)))
        return default (TControl);
      if (!MyTerminalControlFactory.AreControlsCreated(producedType))
        MyTerminalControlFactory.EnsureControlsAreCreated(producedType);
      Type type = typeof (TControl);
      if (type == typeof (IMyTerminalControlTextbox))
        return this.CreateGenericControl<TControl>(typeof (MyTerminalControlTextbox<>), producedType, new object[3]
        {
          (object) id,
          (object) MyStringId.NullOrEmpty,
          (object) MyStringId.NullOrEmpty
        });
      if (type == typeof (IMyTerminalControlButton))
        return this.CreateGenericControl<TControl>(typeof (MyTerminalControlButton<>), producedType, new object[5]
        {
          (object) id,
          (object) MyStringId.NullOrEmpty,
          (object) MyStringId.NullOrEmpty,
          null,
          (object) false
        });
      if (type == typeof (IMyTerminalControlCheckbox))
        return this.CreateGenericControl<TControl>(typeof (MyTerminalControlCheckbox<>), producedType, new object[9]
        {
          (object) id,
          (object) MyStringId.NullOrEmpty,
          (object) MyStringId.NullOrEmpty,
          (object) MyStringId.NullOrEmpty,
          (object) MyStringId.NullOrEmpty,
          (object) false,
          (object) false,
          (object) false,
          (object) 1f
        });
      if (type == typeof (IMyTerminalControlColor))
        return this.CreateGenericControl<TControl>(typeof (MyTerminalControlColor<>), producedType, new object[5]
        {
          (object) id,
          (object) MyStringId.NullOrEmpty,
          (object) false,
          (object) 1f,
          (object) false
        });
      if (type == typeof (IMyTerminalControlCombobox))
        return this.CreateGenericControl<TControl>(typeof (MyTerminalControlCombobox<>), producedType, new object[3]
        {
          (object) id,
          (object) MyStringId.NullOrEmpty,
          (object) MyStringId.NullOrEmpty
        });
      if (type == typeof (IMyTerminalControlListbox))
        return this.CreateGenericControl<TControl>(typeof (MyTerminalControlListbox<>), producedType, new object[5]
        {
          (object) id,
          (object) MyStringId.NullOrEmpty,
          (object) MyStringId.NullOrEmpty,
          (object) false,
          (object) 0
        });
      if (type == typeof (IMyTerminalControlOnOffSwitch))
        return this.CreateGenericControl<TControl>(typeof (MyTerminalControlOnOffSwitch<>), producedType, new object[8]
        {
          (object) id,
          (object) MyStringId.NullOrEmpty,
          (object) MyStringId.NullOrEmpty,
          (object) MyStringId.NullOrEmpty,
          (object) MyStringId.NullOrEmpty,
          (object) float.PositiveInfinity,
          (object) false,
          (object) false
        });
      if (type == typeof (IMyTerminalControlSeparator))
        return this.CreateGenericControl<TControl>(typeof (MyTerminalControlSeparator<>), producedType, new object[0]);
      if (type == typeof (IMyTerminalControlSlider))
        return this.CreateGenericControl<TControl>(typeof (MyTerminalControlSlider<>), producedType, new object[5]
        {
          (object) id,
          (object) MyStringId.NullOrEmpty,
          (object) MyStringId.NullOrEmpty,
          (object) false,
          (object) false
        });
      if (!(type == typeof (IMyTerminalControlLabel)))
        return default (TControl);
      return this.CreateGenericControl<TControl>(typeof (MyTerminalControlLabel<>), producedType, new object[1]
      {
        (object) MyStringId.NullOrEmpty
      });
    }

    public IMyTerminalControlProperty<TValue> CreateProperty<TValue, TBlock>(
      string id)
    {
      if (!this.IsTypeValid<TBlock>())
        return (IMyTerminalControlProperty<TValue>) null;
      Type producedType = this.GetProducedType<TBlock>();
      if (producedType == (Type) null)
        return (IMyTerminalControlProperty<TValue>) null;
      return (IMyTerminalControlProperty<TValue>) Activator.CreateInstance(typeof (MyTerminalControlProperty<,>).MakeGenericType(producedType, typeof (TValue)), (object) id);
    }

    private Type GetProducedType<TBlock>() => !typeof (TBlock).IsInterface ? MyCubeBlockFactory.GetProducedType((MyObjectBuilderType) typeof (TBlock)) : this.FindTerminalTypeFromInterface<TBlock>();

    private Type FindTerminalTypeFromInterface<TBlock>()
    {
      Type key = typeof (TBlock);
      if (!key.IsInterface)
        throw new ArgumentException("Given type is not an interface!");
      Type type;
      if (this.m_interfaceCache.TryGetValue(key, out type))
        return type;
      this.ScanAssembly(Assembly.GetExecutingAssembly());
      return this.m_interfaceCache.TryGetValue(key, out type) ? type : (Type) null;
    }

    private void ScanAssembly(Assembly assembly)
    {
      foreach (Type type in assembly.GetTypes())
      {
        MyTerminalInterfaceAttribute customAttribute = type.GetCustomAttribute<MyTerminalInterfaceAttribute>();
        if (customAttribute != null)
        {
          foreach (Type linkedType in customAttribute.LinkedTypes)
            this.m_interfaceCache[linkedType] = type;
        }
      }
    }

    private bool IsTypeValid<TBlock>()
    {
      if (!typeof (TBlock).IsInterface)
      {
        if (!typeof (MyObjectBuilder_TerminalBlock).IsAssignableFrom(typeof (TBlock)))
          return true;
      }
      else if (typeof (Sandbox.ModAPI.Ingame.IMyTerminalBlock).IsAssignableFrom(typeof (TBlock)))
        return true;
      return false;
    }

    private TControl CreateGenericControl<TControl>(
      Type controlType,
      Type blockType,
      object[] args)
    {
      return (TControl) Activator.CreateInstance(controlType.MakeGenericType(blockType), args);
    }

    public List<ITerminalAction> GetActions(Sandbox.ModAPI.IMyTerminalBlock block)
    {
      if (this.m_customActionGetter == null)
        return MyTerminalControlFactory.GetActions(block.GetType()).ToList<ITerminalAction>();
      List<IMyTerminalAction> list = MyTerminalControlFactory.GetActions(block.GetType()).Cast<IMyTerminalAction>().ToList<IMyTerminalAction>();
      this.m_customActionGetter(block, list);
      return list.Cast<ITerminalAction>().ToList<ITerminalAction>();
    }

    public void GetActions<TBlock>(out List<IMyTerminalAction> items)
    {
      items = new List<IMyTerminalAction>();
      if (!this.IsTypeValid<TBlock>())
        return;
      Type producedType = this.GetProducedType<TBlock>();
      if (producedType == (Type) null)
        return;
      foreach (ITerminalAction action in MyTerminalControlFactory.GetList(producedType).Actions)
        items.Add((IMyTerminalAction) action);
    }

    public void AddAction<TBlock>(IMyTerminalAction action)
    {
      if (!this.IsTypeValid<TBlock>())
        return;
      Type producedType = this.GetProducedType<TBlock>();
      if (producedType == (Type) null)
        return;
      MyTerminalControlFactory.GetList(producedType).Actions.Add((ITerminalAction) action);
    }

    public void RemoveAction<TBlock>(IMyTerminalAction action)
    {
      if (!this.IsTypeValid<TBlock>())
        return;
      Type producedType = this.GetProducedType<TBlock>();
      if (producedType == (Type) null)
        return;
      MyTerminalControlFactory.GetList(producedType).Actions.Remove((ITerminalAction) action);
    }

    public IMyTerminalAction CreateAction<TBlock>(string id)
    {
      if (!this.IsTypeValid<TBlock>())
        return (IMyTerminalAction) null;
      Type producedType = this.GetProducedType<TBlock>();
      if (producedType == (Type) null)
        return (IMyTerminalAction) null;
      return (IMyTerminalAction) Activator.CreateInstance(typeof (MyTerminalAction<>).MakeGenericType(producedType), (object) id, (object) new StringBuilder(""), (object) "");
    }
  }
}
