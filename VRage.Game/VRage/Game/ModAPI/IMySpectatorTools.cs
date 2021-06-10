// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.IMySpectatorTools
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;
using VRage.ModAPI;

namespace VRage.Game.ModAPI
{
  public interface IMySpectatorTools
  {
    void SetTarget(IMyEntity ent);

    IMyEntity GetTarget();

    void SetMode(MyCameraMode mode);

    MyCameraMode GetMode();

    void LockHitEntity();

    IReadOnlyList<MyLockEntityState> TrackedSlots { get; }

    void ClearTrackedSlot(int slotIndex);

    void SaveTrackedSlot(int slotIndex);

    void SelectTrackedSlot(int slotIndex);

    void NextPlayer();

    void PreviousPlayer();
  }
}
