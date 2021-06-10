// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.Ingame.IMyCubeBlock
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using VRage.ObjectBuilders;
using VRageMath;

namespace VRage.Game.ModAPI.Ingame
{
  public interface IMyCubeBlock : IMyEntity
  {
    SerializableDefinitionId BlockDefinition { get; }

    bool CheckConnectionAllowed { get; }

    IMyCubeGrid CubeGrid { get; }

    string DefinitionDisplayNameText { get; }

    float DisassembleRatio { get; }

    string DisplayNameText { get; }

    string GetOwnerFactionTag();

    [Obsolete("GetPlayerRelationToOwner() is useless ingame. Mods should use the one in ModAPI.IMyCubeBlock")]
    MyRelationsBetweenPlayerAndBlock GetPlayerRelationToOwner();

    MyRelationsBetweenPlayerAndBlock GetUserRelationToOwner(
      long playerId);

    bool IsBeingHacked { get; }

    bool IsFunctional { get; }

    bool IsWorking { get; }

    Vector3I Max { get; }

    float Mass { get; }

    Vector3I Min { get; }

    int NumberInGrid { get; }

    MyBlockOrientation Orientation { get; }

    long OwnerId { get; }

    Vector3I Position { get; }

    [Obsolete]
    void UpdateIsWorking();

    [Obsolete]
    void UpdateVisual();
  }
}
