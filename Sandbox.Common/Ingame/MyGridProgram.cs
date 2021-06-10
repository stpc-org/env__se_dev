// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.MyGridProgram
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;
using System.Reflection;

namespace Sandbox.ModAPI.Ingame
{
  public abstract class MyGridProgram : IMyGridProgram
  {
    private string m_storage;
    private readonly Action<string, UpdateType> m_main;
    private readonly Action m_save;
    private Func<IMyIntergridCommunicationSystem> m_IGC_ContextGetter;

    protected MyGridProgram()
    {
      Type type = this.GetType();
      MethodInfo method1 = type.GetMethod("Main", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, new Type[2]
      {
        typeof (string),
        typeof (UpdateType)
      }, (ParameterModifier[]) null);
      if (method1 != (MethodInfo) null)
      {
        this.m_main = method1.CreateDelegate<Action<string, UpdateType>>((object) this);
      }
      else
      {
        MethodInfo method2 = type.GetMethod("Main", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, new Type[1]
        {
          typeof (string)
        }, (ParameterModifier[]) null);
        if (method2 != (MethodInfo) null)
        {
          Action<string> main = method2.CreateDelegate<Action<string>>((object) this);
          this.m_main = (Action<string, UpdateType>) ((arg, source) => main(arg));
        }
        else
        {
          MethodInfo method3 = type.GetMethod("Main", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Type.EmptyTypes, (ParameterModifier[]) null);
          if (method3 != (MethodInfo) null)
          {
            Action mainWithoutArgument = method3.CreateDelegate<Action>((object) this);
            this.m_main = (Action<string, UpdateType>) ((arg, source) => mainWithoutArgument());
          }
        }
      }
      MethodInfo method4 = type.GetMethod("Save", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      if (!(method4 != (MethodInfo) null))
        return;
      this.m_save = method4.CreateDelegate<Action>((object) this);
    }

    public IMyGridTerminalSystem GridTerminalSystem { get; protected set; }

    public IMyProgrammableBlock Me { get; protected set; }

    [Obsolete("Use Runtime.TimeSinceLastRun instead")]
    public TimeSpan ElapsedTime { get; protected set; }

    public IMyGridProgramRuntimeInfo Runtime { get; protected set; }

    public string Storage
    {
      get => this.m_storage ?? "";
      protected set => this.m_storage = value ?? "";
    }

    public Action<string> Echo { get; protected set; }

    Func<IMyIntergridCommunicationSystem> IMyGridProgram.IGC_ContextGetter
    {
      set => this.m_IGC_ContextGetter = value;
    }

    public IMyIntergridCommunicationSystem IGC => this.m_IGC_ContextGetter();

    IMyGridTerminalSystem IMyGridProgram.GridTerminalSystem
    {
      get => this.GridTerminalSystem;
      set => this.GridTerminalSystem = value;
    }

    IMyProgrammableBlock IMyGridProgram.Me
    {
      get => this.Me;
      set => this.Me = value;
    }

    TimeSpan IMyGridProgram.ElapsedTime
    {
      get => this.ElapsedTime;
      set => this.ElapsedTime = value;
    }

    string IMyGridProgram.Storage
    {
      get => this.Storage;
      set => this.Storage = value;
    }

    Action<string> IMyGridProgram.Echo
    {
      get => this.Echo;
      set => this.Echo = value;
    }

    IMyGridProgramRuntimeInfo IMyGridProgram.Runtime
    {
      get => this.Runtime;
      set => this.Runtime = value;
    }

    bool IMyGridProgram.HasMainMethod => this.m_main != null;

    [Obsolete]
    void IMyGridProgram.Main(string argument)
    {
      if (this.m_main == null)
        throw new InvalidOperationException("No Main method available");
      this.m_main(argument ?? string.Empty, UpdateType.Mod);
    }

    void IMyGridProgram.Main(string argument, UpdateType updateSource)
    {
      if (this.m_main == null)
        throw new InvalidOperationException("No Main method available");
      this.m_main(argument ?? string.Empty, updateSource);
    }

    bool IMyGridProgram.HasSaveMethod => this.m_save != null;

    void IMyGridProgram.Save()
    {
      if (this.m_save == null)
        return;
      this.m_save();
    }
  }
}
