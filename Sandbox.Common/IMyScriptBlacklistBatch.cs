// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.IMyScriptBlacklistBatch
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;

namespace Sandbox.ModAPI
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
