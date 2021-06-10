// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.IMyGridProgram
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using Sandbox.ModAPI.Ingame;
using System;

namespace Sandbox.ModAPI
{
  public interface IMyGridProgram
  {
    Func<IMyIntergridCommunicationSystem> IGC_ContextGetter { set; }

    Sandbox.ModAPI.Ingame.IMyGridTerminalSystem GridTerminalSystem { get; set; }

    Sandbox.ModAPI.Ingame.IMyProgrammableBlock Me { get; set; }

    [Obsolete("Use Runtime.TimeSinceLastRun instead")]
    TimeSpan ElapsedTime { get; set; }

    string Storage { get; set; }

    IMyGridProgramRuntimeInfo Runtime { get; set; }

    Action<string> Echo { get; set; }

    bool HasMainMethod { get; }

    [Obsolete("Use overload Main(String, UpdateType)")]
    void Main(string argument);

    void Main(string argument, UpdateType updateSource);

    bool HasSaveMethod { get; }

    void Save();
  }
}
