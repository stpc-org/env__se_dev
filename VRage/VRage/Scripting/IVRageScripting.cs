// Decompiled with JetBrains decompiler
// Type: VRage.Scripting.IVRageScripting
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using VRage.Utils;

namespace VRage.Scripting
{
  public interface IVRageScripting
  {
    bool IsRuntimeCompilationSupported { get; }

    void Initialize(
      Thread updateThread,
      IEnumerable<string> referencedAssemblies,
      Type[] referencedTypes,
      string[] symbols,
      string diagnosticsPath,
      bool enableScriptsPDBs);

    IMyWhitelistBatch OpenWhitelistBatch();

    void ClearWhitelist();

    Script GetIngameScript(string code, string className, string inheritance);

    Task<Assembly> CompileAsync(
      MyApiTarget target,
      string assemblyName,
      IEnumerable<Script> scripts,
      out List<Message> diagnostics,
      string friendlyName,
      bool enableDebugInformation = false);

    bool ReportIncorrectBehaviour(MyStringId ruleId);

    IEnumerable<MyTuple<string, MyStringId>> GetWatchdogWarnings();

    IMyIngameScripting GetModAPIScriptingHandle();

    IVSTAssemblyProvider VSTAssemblyProvider { get; }
  }
}
