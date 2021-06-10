// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.RigidBodyFlag
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;

namespace VRage.Game.Components
{
  [Flags]
  public enum RigidBodyFlag
  {
    RBF_DEFAULT = 0,
    RBF_KINEMATIC = 2,
    RBF_STATIC = 4,
    RBF_DISABLE_COLLISION_RESPONSE = 64, // 0x00000040
    RBF_DOUBLED_KINEMATIC = 128, // 0x00000080
    RBF_BULLET = 256, // 0x00000100
    RBF_DEBRIS = 512, // 0x00000200
    RBF_KEYFRAMED_REPORTING = 1024, // 0x00000400
    RBF_UNLOCKED_SPEEDS = 2048, // 0x00000800
    RBF_NO_POSITION_UPDATES = 4096, // 0x00001000
  }
}
