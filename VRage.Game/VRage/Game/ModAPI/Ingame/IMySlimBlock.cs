// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.Ingame.IMySlimBlock
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.ModAPI.Ingame
{
  public interface IMySlimBlock
  {
    SerializableDefinitionId BlockDefinition { get; }

    float AccumulatedDamage { get; }

    float BuildIntegrity { get; }

    float BuildLevelRatio { get; }

    float CurrentDamage { get; }

    float DamageRatio { get; }

    IMyCubeBlock FatBlock { get; }

    void GetMissingComponents(Dictionary<string, int> addToDictionary);

    bool HasDeformation { get; }

    bool IsDestroyed { get; }

    bool IsFullIntegrity { get; }

    bool IsFullyDismounted { get; }

    float MaxDeformation { get; }

    float MaxIntegrity { get; }

    float Mass { get; }

    long OwnerId { get; }

    bool ShowParts { get; }

    bool StockpileAllocated { get; }

    bool StockpileEmpty { get; }

    Vector3I Position { get; }

    IMyCubeGrid CubeGrid { get; }

    Vector3 ColorMaskHSV { get; }

    MyStringHash SkinSubtypeId { get; }
  }
}
