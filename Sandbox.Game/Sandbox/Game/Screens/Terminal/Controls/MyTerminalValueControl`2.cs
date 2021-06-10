// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Terminal.Controls.MyTerminalValueControl`2
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Interfaces.Terminal;
using System;
using VRage.Game.ModAPI.Ingame;
using VRage.Library.Collections;

namespace Sandbox.Game.Screens.Terminal.Controls
{
  public abstract class MyTerminalValueControl<TBlock, TValue> : MyTerminalControl<TBlock>, ITerminalValueControl<TBlock, TValue>, ITerminalProperty<TValue>, ITerminalProperty, ITerminalControl, ITerminalControlSync, IMyTerminalValueControl<TValue>
    where TBlock : MyTerminalBlock
  {
    public MyTerminalValueControl<TBlock, TValue>.SerializerDelegate Serializer;

    public MyTerminalValueControl<TBlock, TValue>.GetterDelegate Getter { get; set; }

    public MyTerminalValueControl<TBlock, TValue>.SetterDelegate Setter { get; set; }

    public MyTerminalValueControl(string id)
      : base(id)
    {
    }

    public virtual TValue GetValue(TBlock block) => this.Getter(block);

    public virtual void SetValue(TBlock block, TValue value)
    {
      this.Setter(block, value);
      block.NotifyTerminalValueChanged((ITerminalControl) this);
    }

    public virtual void Serialize(BitStream stream, TBlock block)
    {
      if (stream.Reading)
      {
        TValue obj = default (TValue);
        this.Serializer(stream, ref obj);
        this.SetValue(block, obj);
      }
      else
      {
        TValue obj = this.GetValue(block);
        this.Serializer(stream, ref obj);
      }
    }

    public abstract TValue GetDefaultValue(TBlock block);

    [Obsolete("Use GetMinimum instead")]
    public TValue GetMininum(TBlock block) => this.GetMinimum(block);

    public abstract TValue GetMinimum(TBlock block);

    public abstract TValue GetMaximum(TBlock block);

    public TValue GetValue(IMyCubeBlock block) => this.GetValue((TBlock) block);

    public void SetValue(IMyCubeBlock block, TValue value) => this.SetValue((TBlock) block, value);

    public TValue GetDefaultValue(IMyCubeBlock block) => this.GetDefaultValue((TBlock) block);

    [Obsolete("Use GetMinimum instead")]
    public TValue GetMininum(IMyCubeBlock block) => this.GetMinimum((TBlock) block);

    public TValue GetMinimum(IMyCubeBlock block) => this.GetMinimum((TBlock) block);

    public TValue GetMaximum(IMyCubeBlock block) => this.GetMaximum((TBlock) block);

    public void Serialize(BitStream stream, MyTerminalBlock block) => this.Serialize(stream, (TBlock) block);

    string ITerminalProperty.Id => this.Id;

    string ITerminalProperty.TypeName => typeof (TValue).Name;

    Func<IMyTerminalBlock, TValue> IMyTerminalValueControl<TValue>.Getter
    {
      get
      {
        MyTerminalValueControl<TBlock, TValue>.GetterDelegate oldGetter = this.Getter;
        return (Func<IMyTerminalBlock, TValue>) (x => oldGetter((TBlock) x));
      }
      set => this.Getter = new MyTerminalValueControl<TBlock, TValue>.GetterDelegate(value.Invoke);
    }

    Action<IMyTerminalBlock, TValue> IMyTerminalValueControl<TValue>.Setter
    {
      get
      {
        MyTerminalValueControl<TBlock, TValue>.SetterDelegate oldSetter = this.Setter;
        return (Action<IMyTerminalBlock, TValue>) ((x, y) => oldSetter((TBlock) x, y));
      }
      set => this.Setter = new MyTerminalValueControl<TBlock, TValue>.SetterDelegate(value.Invoke);
    }

    public delegate TValue GetterDelegate(TBlock block) where TBlock : MyTerminalBlock;

    public delegate void SetterDelegate(TBlock block, TValue value) where TBlock : MyTerminalBlock;

    public delegate void SerializerDelegate(BitStream stream, ref TValue value) where TBlock : MyTerminalBlock;

    public delegate void ExternalSetterDelegate(IMyTerminalBlock block, TValue value) where TBlock : MyTerminalBlock;
  }
}
