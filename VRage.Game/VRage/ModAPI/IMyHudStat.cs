// Decompiled with JetBrains decompiler
// Type: VRage.ModAPI.IMyHudStat
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Utils;

namespace VRage.ModAPI
{
  public interface IMyHudStat
  {
    MyStringHash Id { get; }

    float CurrentValue { get; }

    float MaxValue { get; }

    float MinValue { get; }

    void Update();

    string GetValueString();
  }
}
