// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyBehaviorTreeState
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

namespace VRage.Game
{
  public enum MyBehaviorTreeState : sbyte
  {
    ERROR = -1, // 0xFF
    NOT_TICKED = 0,
    SUCCESS = 1,
    FAILURE = 2,
    RUNNING = 3,
  }
}
