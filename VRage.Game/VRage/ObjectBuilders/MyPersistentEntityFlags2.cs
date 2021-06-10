// Decompiled with JetBrains decompiler
// Type: VRage.ObjectBuilders.MyPersistentEntityFlags2
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;

namespace VRage.ObjectBuilders
{
  [Flags]
  public enum MyPersistentEntityFlags2
  {
    None = 0,
    Enabled = 2,
    CastShadows = 4,
    InScene = 16, // 0x00000010
  }
}
