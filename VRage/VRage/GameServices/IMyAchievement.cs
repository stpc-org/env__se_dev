// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.IMyAchievement
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRage.GameServices
{
  public interface IMyAchievement
  {
    event Action OnStatValueChanged;

    event Action OnUnlocked;

    bool IsUnlocked { get; }

    int StatValueInt { get; set; }

    float StatValueFloat { get; set; }

    int StatValueConditionBitField { get; set; }

    void Unlock();

    void IndicateProgress(uint value);
  }
}
