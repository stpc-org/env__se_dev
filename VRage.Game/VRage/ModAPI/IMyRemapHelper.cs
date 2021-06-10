// Decompiled with JetBrains decompiler
// Type: VRage.ModAPI.IMyRemapHelper
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

namespace VRage.ModAPI
{
  public interface IMyRemapHelper
  {
    long RemapEntityId(long oldEntityId);

    string RemapEntityName(long newEntityId);

    int RemapGroupId(string group, int oldValue);

    void Clear();
  }
}
