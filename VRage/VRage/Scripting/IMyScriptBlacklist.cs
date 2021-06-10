// Decompiled with JetBrains decompiler
// Type: VRage.Scripting.IMyScriptBlacklist
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRage.Collections;

namespace VRage.Scripting
{
  public interface IMyScriptBlacklist
  {
    DictionaryReader<string, MyWhitelistTarget> GetWhitelist();

    HashSetReader<string> GetBlacklistedIngameEntries();

    IMyScriptBlacklistBatch OpenIngameBlacklistBatch();
  }
}
