// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.MyVRageIngameScriptingAdapter
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;
using VRage.Collections;
using VRage.Scripting;

namespace Sandbox.ModAPI
{
  public class MyVRageIngameScriptingAdapter : IMyIngameScripting, IMyScriptBlacklist
  {
    private readonly VRage.Scripting.IMyIngameScripting m_scripting;

    public IMyScriptBlacklist ScriptBlacklist { get; }

    private VRage.Scripting.IMyScriptBlacklist BlackList => this.m_scripting.ScriptBlacklist;

    public MyVRageIngameScriptingAdapter(VRage.Scripting.IMyIngameScripting impl)
    {
      this.m_scripting = impl;
      this.ScriptBlacklist = (IMyScriptBlacklist) this;
    }

    public void Clean() => this.m_scripting?.Clean();

    public DictionaryReader<string, MyWhitelistTarget> GetWhitelist() => this.BlackList.GetWhitelist();

    public HashSetReader<string> GetBlacklistedIngameEntries() => this.BlackList.GetBlacklistedIngameEntries();

    public IMyScriptBlacklistBatch OpenIngameBlacklistBatch() => (IMyScriptBlacklistBatch) new MyVRageIngameScriptingAdapter.MyScriptBlacklistBatchAdapter(this.BlackList.OpenIngameBlacklistBatch());

    private class MyScriptBlacklistBatchAdapter : IMyScriptBlacklistBatch, IDisposable
    {
      private readonly VRage.Scripting.IMyScriptBlacklistBatch m_batch;

      public MyScriptBlacklistBatchAdapter(VRage.Scripting.IMyScriptBlacklistBatch batch) => this.m_batch = batch;

      public void AddNamespaceOfTypes(params Type[] types) => this.m_batch.AddNamespaceOfTypes(types);

      public void RemoveNamespaceOfTypes(params Type[] types) => this.m_batch.RemoveNamespaceOfTypes(types);

      public void AddTypes(params Type[] types) => this.m_batch.AddTypes(types);

      public void RemoveTypes(params Type[] types) => this.m_batch.RemoveTypes(types);

      public void AddMembers(Type type, params string[] memberNames) => this.m_batch.AddMembers(type, memberNames);

      public void RemoveMembers(Type type, params string[] memberNames) => this.m_batch.RemoveMembers(type, memberNames);

      public void Dispose() => this.m_batch.Dispose();
    }
  }
}
