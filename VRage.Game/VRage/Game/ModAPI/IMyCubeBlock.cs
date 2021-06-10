// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.IMyCubeBlock
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRageMath;

namespace VRage.Game.ModAPI
{
  public interface IMyCubeBlock : VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity
  {
    event Action<IMyCubeBlock> IsWorkingChanged;

    void CalcLocalMatrix(out Matrix localMatrix, out string currModel);

    string CalculateCurrentModel(out Matrix orientation);

    new bool CheckConnectionAllowed { get; set; }

    IMyCubeGrid CubeGrid { get; }

    bool DebugDraw();

    MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false);

    new MyRelationsBetweenPlayerAndBlock GetPlayerRelationToOwner();

    new MyRelationsBetweenPlayerAndBlock GetUserRelationToOwner(
      long playerId);

    void Init();

    void Init(MyObjectBuilder_CubeBlock builder, IMyCubeGrid cubeGrid);

    void OnBuildSuccess(long builtBy);

    void OnBuildSuccess(long builtBy, bool instantBuild);

    void OnDestroy();

    void OnModelChange();

    void OnRegisteredToGridSystems();

    void OnRemovedByCubeBuilder();

    void OnUnregisteredFromGridSystems();

    string RaycastDetectors(Vector3D worldFrom, Vector3D worldTo);

    void ReloadDetectors(bool refreshNetworks = true);

    MyResourceSinkComponentBase ResourceSink { get; set; }

    new void UpdateIsWorking();

    new void UpdateVisual();

    void SetDamageEffect(bool start);

    bool SetEffect(string effectName, bool stopPrevious = false);

    bool SetEffect(
      string effectName,
      float parameter,
      bool stopPrevious = false,
      bool ignoreParameter = false,
      bool removeSameNameEffects = false);

    int RemoveEffect(string effectName, int exception = -1);

    Dictionary<string, float> UpgradeValues { get; }

    void AddUpgradeValue(string upgrade, float defaultValue);

    IMySlimBlock SlimBlock { get; }

    event Action OnUpgradeValuesChanged;
  }
}
