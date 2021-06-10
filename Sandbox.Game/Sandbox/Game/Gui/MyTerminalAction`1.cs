// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyTerminalAction`1
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Cube;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces.Terminal;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Collections;
using VRage.Game;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.Game.Gui
{
  public class MyTerminalAction<TBlock> : ITerminalAction, Sandbox.ModAPI.Interfaces.ITerminalAction, IMyTerminalAction
    where TBlock : MyTerminalBlock
  {
    private readonly string m_id;
    private string m_icon;
    private StringBuilder m_name;
    private List<TerminalActionParameter> m_parameterDefinitions = new List<TerminalActionParameter>();
    private System.Action<TBlock> m_action;
    private System.Action<TBlock, ListReader<TerminalActionParameter>> m_actionWithParameters;
    public Func<TBlock, bool> Enabled = (Func<TBlock, bool>) (b => true);
    public Func<TBlock, bool> Callable = (Func<TBlock, bool>) (b => true);
    public List<MyToolbarType> InvalidToolbarTypes;
    public bool ValidForGroups = true;
    public MyTerminalControl<TBlock>.WriterDelegate Writer;
    public System.Action<IList<TerminalActionParameter>, System.Action<bool>> DoUserParameterRequest;

    public MyTerminalAction(string id, StringBuilder name, string icon)
    {
      this.m_id = id;
      this.m_name = name;
      this.m_icon = icon;
    }

    public MyTerminalAction(string id, StringBuilder name, System.Action<TBlock> action, string icon)
    {
      this.m_id = id;
      this.m_name = name;
      this.Action = action;
      this.m_icon = icon;
    }

    public MyTerminalAction(
      string id,
      StringBuilder name,
      System.Action<TBlock, ListReader<TerminalActionParameter>> action,
      string icon)
    {
      this.m_id = id;
      this.m_name = name;
      this.ActionWithParameters = action;
      this.m_icon = icon;
    }

    public MyTerminalAction(
      string id,
      StringBuilder name,
      System.Action<TBlock> action,
      MyTerminalControl<TBlock>.WriterDelegate valueWriter,
      string icon)
    {
      this.m_id = id;
      this.m_name = name;
      this.Action = action;
      this.m_icon = icon;
      this.Writer = valueWriter;
    }

    public MyTerminalAction(
      string id,
      StringBuilder name,
      System.Action<TBlock, ListReader<TerminalActionParameter>> action,
      MyTerminalControl<TBlock>.WriterDelegate valueWriter,
      string icon)
    {
      this.m_id = id;
      this.m_name = name;
      this.ActionWithParameters = action;
      this.m_icon = icon;
      this.Writer = valueWriter;
    }

    public MyTerminalAction(
      string id,
      StringBuilder name,
      System.Action<TBlock> action,
      MyTerminalControl<TBlock>.WriterDelegate valueWriter,
      string icon,
      Func<TBlock, bool> enabled = null,
      Func<TBlock, bool> callable = null)
      : this(id, name, action, valueWriter, icon)
    {
      if (enabled != null)
        this.Enabled = enabled;
      if (callable == null)
        return;
      this.Callable = callable;
    }

    public System.Action<TBlock> Action
    {
      get => this.m_action;
      set
      {
        this.m_action = value;
        this.m_actionWithParameters = (System.Action<TBlock, ListReader<TerminalActionParameter>>) ((block, parameters) => this.m_action(block));
      }
    }

    public System.Action<TBlock, ListReader<TerminalActionParameter>> ActionWithParameters
    {
      get => this.m_actionWithParameters;
      set
      {
        this.m_actionWithParameters = value;
        this.m_action = (System.Action<TBlock>) (block => this.m_actionWithParameters(block, new ListReader<TerminalActionParameter>(this.ParameterDefinitions)));
      }
    }

    public string Id => this.m_id;

    public string Icon => this.m_icon;

    public StringBuilder Name => this.m_name;

    public void Apply(MyTerminalBlock block, ListReader<TerminalActionParameter> parameters)
    {
      TBlock block1 = (TBlock) block;
      if (!this.Enabled(block1) || !this.IsCallable((MyTerminalBlock) block1))
        return;
      this.m_actionWithParameters(block1, parameters);
    }

    public void Apply(MyTerminalBlock block)
    {
      TBlock block1 = (TBlock) block;
      if (!this.Enabled(block1) || !this.IsCallable((MyTerminalBlock) block1))
        return;
      this.m_action(block1);
    }

    public bool IsEnabled(MyTerminalBlock block) => (string.IsNullOrEmpty(this.Id) || !this.Id.Equals("IncreaseWeld speed") && !this.Id.Equals("DecreaseWeld speed") && !this.Id.Equals("Force weld")) && this.Enabled((TBlock) block) && this.IsCallable(block);

    public bool IsCallable(MyTerminalBlock block) => this.Callable == null || this.Callable((TBlock) block);

    public bool IsValidForToolbarType(MyToolbarType type) => this.InvalidToolbarTypes == null || !this.InvalidToolbarTypes.Contains(type);

    public bool IsValidForGroups() => this.ValidForGroups;

    ListReader<TerminalActionParameter> ITerminalAction.GetParameterDefinitions() => (ListReader<TerminalActionParameter>) this.m_parameterDefinitions;

    public void WriteValue(MyTerminalBlock block, StringBuilder appendTo)
    {
      if (this.Writer == null || !this.IsCallable(block))
        return;
      this.Writer((TBlock) block, appendTo);
    }

    string Sandbox.ModAPI.Interfaces.ITerminalAction.Id => this.Id;

    string Sandbox.ModAPI.Interfaces.ITerminalAction.Icon => this.Icon;

    StringBuilder Sandbox.ModAPI.Interfaces.ITerminalAction.Name => this.Name;

    public List<TerminalActionParameter> ParameterDefinitions => this.m_parameterDefinitions;

    public void RequestParameterCollection(
      IList<TerminalActionParameter> parameters,
      System.Action<bool> callback)
    {
      if (parameters == null)
        throw new ArgumentException(nameof (parameters));
      if (callback == null)
        throw new ArgumentNullException(nameof (callback));
      System.Action<IList<TerminalActionParameter>, System.Action<bool>> parameterRequest = this.DoUserParameterRequest;
      List<TerminalActionParameter> parameterDefinitions = this.ParameterDefinitions;
      parameters.Clear();
      foreach (TerminalActionParameter terminalActionParameter in parameterDefinitions)
        parameters.Add(terminalActionParameter);
      if (parameterRequest == null)
        callback(true);
      else
        parameterRequest(parameters, callback);
    }

    Func<Sandbox.ModAPI.IMyTerminalBlock, bool> IMyTerminalAction.Enabled
    {
      get
      {
        Func<TBlock, bool> oldEnabled = this.Enabled;
        return (Func<Sandbox.ModAPI.IMyTerminalBlock, bool>) (x => oldEnabled((TBlock) x));
      }
      set => this.Enabled = (Func<TBlock, bool>) value;
    }

    List<MyToolbarType> IMyTerminalAction.InvalidToolbarTypes
    {
      get => this.InvalidToolbarTypes;
      set => this.InvalidToolbarTypes = value;
    }

    bool IMyTerminalAction.ValidForGroups
    {
      get => this.ValidForGroups;
      set => this.ValidForGroups = value;
    }

    StringBuilder IMyTerminalAction.Name
    {
      get => this.Name;
      set => this.m_name = value;
    }

    string IMyTerminalAction.Icon
    {
      get => this.Icon;
      set => this.m_icon = value;
    }

    System.Action<Sandbox.ModAPI.IMyTerminalBlock> IMyTerminalAction.Action
    {
      get
      {
        System.Action<TBlock> oldAction = this.Action;
        return (System.Action<Sandbox.ModAPI.IMyTerminalBlock>) (x => oldAction((TBlock) x));
      }
      set => this.Action = (System.Action<TBlock>) value;
    }

    System.Action<Sandbox.ModAPI.IMyTerminalBlock, StringBuilder> IMyTerminalAction.Writer
    {
      get
      {
        MyTerminalControl<TBlock>.WriterDelegate oldWriter = this.Writer;
        return (System.Action<Sandbox.ModAPI.IMyTerminalBlock, StringBuilder>) ((x, y) => oldWriter((TBlock) x, y));
      }
      set => this.Writer = new MyTerminalControl<TBlock>.WriterDelegate(value.Invoke);
    }

    void Sandbox.ModAPI.Interfaces.ITerminalAction.Apply(IMyCubeBlock block)
    {
      if (!(block is TBlock))
        return;
      this.Apply(block as MyTerminalBlock);
    }

    void Sandbox.ModAPI.Interfaces.ITerminalAction.Apply(
      IMyCubeBlock block,
      ListReader<TerminalActionParameter> parameters)
    {
      if (!(block is TBlock))
        return;
      this.Apply(block as MyTerminalBlock, parameters);
    }

    void Sandbox.ModAPI.Interfaces.ITerminalAction.WriteValue(
      IMyCubeBlock block,
      StringBuilder appendTo)
    {
      if (!(block is TBlock))
        return;
      this.WriteValue(block as MyTerminalBlock, appendTo);
    }

    bool Sandbox.ModAPI.Interfaces.ITerminalAction.IsEnabled(IMyCubeBlock block) => block is TBlock && this.IsEnabled(block as MyTerminalBlock);
  }
}
