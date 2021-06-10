// Decompiled with JetBrains decompiler
// Type: VRage.Scripting.IMyScriptBlacklistBatch
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRage.Scripting
{
  public interface IMyScriptBlacklistBatch : IDisposable
  {
    void AddNamespaceOfTypes(params Type[] types);

    void RemoveNamespaceOfTypes(params Type[] types);

    void AddTypes(params Type[] types);

    void RemoveTypes(params Type[] types);

    void AddMembers(Type type, params string[] memberNames);

    void RemoveMembers(Type type, params string[] memberNames);
  }
}
