// Decompiled with JetBrains decompiler
// Type: VRage.Scripting.IVSTAssemblyProvider
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Collections.Generic;
using System.Reflection;

namespace VRage.Scripting
{
  public interface IVSTAssemblyProvider
  {
    bool DebugEnabled { get; set; }

    bool TryLoad(string assemblyName, bool checkFileExists);

    void Init(IEnumerable<string> fileNames, string localModPath);

    Assembly GetAssembly();
  }
}
