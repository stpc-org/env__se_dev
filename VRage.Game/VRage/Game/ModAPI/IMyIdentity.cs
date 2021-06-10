// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.IMyIdentity
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using VRageMath;

namespace VRage.Game.ModAPI
{
  public interface IMyIdentity
  {
    event Action<IMyCharacter, IMyCharacter> CharacterChanged;

    [Obsolete("Use IdentityId instead.")]
    long PlayerId { get; }

    long IdentityId { get; }

    string DisplayName { get; }

    string Model { get; }

    Vector3? ColorMask { get; }

    bool IsDead { get; }
  }
}
