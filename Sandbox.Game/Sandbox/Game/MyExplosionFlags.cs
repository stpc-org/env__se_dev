// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.MyExplosionFlags
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;

namespace Sandbox.Game
{
  [Flags]
  public enum MyExplosionFlags
  {
    CREATE_DEBRIS = 1,
    AFFECT_VOXELS = 2,
    APPLY_FORCE_AND_DAMAGE = 4,
    CREATE_DECALS = 8,
    FORCE_DEBRIS = 16, // 0x00000010
    CREATE_PARTICLE_EFFECT = 32, // 0x00000020
    CREATE_SHRAPNELS = 64, // 0x00000040
    APPLY_DEFORMATION = 128, // 0x00000080
    CREATE_PARTICLE_DEBRIS = 256, // 0x00000100
  }
}
