﻿// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.CharacterMovementStateChangedDelegate
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

namespace VRage.Game.ModAPI
{
  public delegate void CharacterMovementStateChangedDelegate(
    IMyCharacter character,
    MyCharacterMovementEnum oldState,
    MyCharacterMovementEnum newState);
}