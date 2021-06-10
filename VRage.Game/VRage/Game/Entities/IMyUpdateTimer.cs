// Decompiled with JetBrains decompiler
// Type: VRage.Game.Entities.IMyUpdateTimer
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Game.ObjectBuilders.ComponentSystem;

namespace VRage.Game.Entities
{
  public interface IMyUpdateTimer
  {
    void CreateUpdateTimer(uint startingTimeInFrames, MyTimerTypes timerType, bool start);

    bool GetTimerEnabledState();

    uint GetFramesFromLastTrigger();

    void DoUpdateTimerTick();

    void ChangeTimerTick(uint timeTickInFrames);
  }
}
