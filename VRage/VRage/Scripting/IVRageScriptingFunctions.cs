// Decompiled with JetBrains decompiler
// Type: VRage.Scripting.IVRageScriptingFunctions
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace VRage.Scripting
{
  public static class IVRageScriptingFunctions
  {
    public static Task<Assembly> CompileIngameScriptAsync(
      this IVRageScripting thiz,
      string assemblyName,
      string program,
      out List<Message> diagnostics,
      string friendlyName,
      string typeName,
      string baseType)
    {
      Script ingameScript = thiz.GetIngameScript(program, typeName, baseType);
      return thiz.CompileAsync(MyApiTarget.Ingame, assemblyName, (IEnumerable<Script>) new Script[1]
      {
        ingameScript
      }, out diagnostics, friendlyName);
    }
  }
}
