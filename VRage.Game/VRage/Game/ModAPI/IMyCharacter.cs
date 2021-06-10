// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.IMyCharacter
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using VRage.Game.ModAPI.Interfaces;
using VRageMath;

namespace VRage.Game.ModAPI
{
  public interface IMyCharacter : VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity, IMyControllableEntity, IMyCameraController, IMyDestroyableObject, IMyDecalProxy
  {
    event Action<IMyCharacter> CharacterDied;

    Vector3D AimedPoint { get; set; }

    MyDefinitionBase Definition { get; }

    float EnvironmentOxygenLevel { get; }

    float OxygenLevel { get; }

    float BaseMass { get; }

    float CurrentMass { get; }

    float SuitEnergyLevel { get; }

    float GetSuitGasFillLevel(MyDefinitionId gasDefinitionId);

    bool IsDead { get; }

    bool IsPlayer { get; }

    bool IsBot { get; }

    [Obsolete("OnMovementStateChanged is deprecated, use MovementStateChanged")]
    event CharacterMovementStateDelegate OnMovementStateChanged;

    event CharacterMovementStateChangedDelegate MovementStateChanged;

    MyCharacterMovementEnum CurrentMovementState { get; set; }

    MyCharacterMovementEnum PreviousMovementState { get; }

    void Kill(object killData = null);

    void TriggerCharacterAnimationEvent(string eventName, bool sync);

    VRage.ModAPI.IMyEntity EquippedTool { get; }

    float GetOutsideTemperature();
  }
}
