// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.IMyScriptBlacklist
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using VRage.Collections;
using VRage.Scripting;

namespace Sandbox.ModAPI
{
  public interface IMyScriptBlacklist
  {
    DictionaryReader<string, MyWhitelistTarget> GetWhitelist();

    HashSetReader<string> GetBlacklistedIngameEntries();

    IMyScriptBlacklistBatch OpenIngameBlacklistBatch();
  }
}
