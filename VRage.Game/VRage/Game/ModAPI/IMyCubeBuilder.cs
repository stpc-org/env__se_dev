// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.IMyCubeBuilder
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.ModAPI;

namespace VRage.Game.ModAPI
{
  public interface IMyCubeBuilder
  {
    void Activate(MyDefinitionId? blockDefinitionId = null);

    bool AddConstruction(IMyEntity buildingEntity);

    bool BlockCreationIsActivated { get; }

    void Deactivate();

    void DeactivateBlockCreation();

    bool FreezeGizmo { get; set; }

    bool ShowRemoveGizmo { get; set; }

    void StartNewGridPlacement(MyCubeSize cubeSize, bool isStatic);

    bool UseSymmetry { get; set; }

    bool UseTransparency { get; set; }

    IMyCubeGrid FindClosestGrid();

    bool IsActivated { get; }
  }
}
