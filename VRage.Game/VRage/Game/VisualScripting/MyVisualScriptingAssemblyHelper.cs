// Decompiled with JetBrains decompiler
// Type: VRage.Game.VisualScripting.MyVisualScriptingAssemblyHelper
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using VRage.Scripting;

namespace VRage.Game.VisualScripting
{
  public static class MyVisualScriptingAssemblyHelper
  {
    private static readonly Regex m_assemblyNameCleaner = new Regex("[^A-Za-z_-]*", RegexOptions.Compiled);

    public static string MakeAssemblyName(string scenarioName) => "VS_" + MyVisualScriptingAssemblyHelper.m_assemblyNameCleaner.Replace(scenarioName, "");

    public static Type GetType(this IVSTAssemblyProvider provider, string typeName)
    {
      Assembly assembly = provider.GetAssembly();
      return (object) assembly == null ? (Type) null : assembly.GetType(typeName);
    }

    public static List<IMyLevelScript> GetLevelScriptInstances(
      this IVSTAssemblyProvider provider,
      HashSet<string> scriptNames = null)
    {
      Assembly assembly = provider.GetAssembly();
      List<IMyLevelScript> myLevelScriptList = new List<IMyLevelScript>();
      if (assembly != (Assembly) null)
      {
        foreach (Type type in assembly.GetTypes())
        {
          if (typeof (IMyLevelScript).IsAssignableFrom(type) && (scriptNames == null || scriptNames.Contains(type.Name)))
            myLevelScriptList.Add((IMyLevelScript) Activator.CreateInstance(type));
        }
      }
      return myLevelScriptList;
    }
  }
}
