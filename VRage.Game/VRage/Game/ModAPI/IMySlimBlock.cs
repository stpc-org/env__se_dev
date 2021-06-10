// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.IMySlimBlock
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using VRage.Game.ModAPI.Interfaces;
using VRageMath;

namespace VRage.Game.ModAPI
{
  public interface IMySlimBlock : VRage.Game.ModAPI.Ingame.IMySlimBlock, IMyDestroyableObject, IMyDecalProxy
  {
    void AddNeighbours();

    void ApplyAccumulatedDamage(bool addDirtyParts = true);

    string CalculateCurrentModel(out Matrix orientation);

    void ComputeScaledCenter(out Vector3D scaledCenter);

    void ComputeScaledHalfExtents(out Vector3 scaledHalfExtents);

    void ComputeWorldCenter(out Vector3D worldCenter);

    IMyCubeBlock FatBlock { get; }

    void FixBones(float oldDamage, float maxAllowedBoneMovement);

    void FullyDismount(IMyInventory outputInventory);

    [Obsolete("GetCopyObjectBuilder() is deprecated. Call GetObjectBuilder(bool) and pass 'true'.")]
    MyObjectBuilder_CubeBlock GetCopyObjectBuilder();

    MyObjectBuilder_CubeBlock GetObjectBuilder(bool copy = false);

    void InitOrientation(ref Vector3I forward, ref Vector3I up);

    void InitOrientation(Base6Directions.Direction Forward, Base6Directions.Direction Up);

    void InitOrientation(MyBlockOrientation orientation);

    void MoveItemsFromConstructionStockpile(IMyInventory toInventory, MyItemFlags flags = MyItemFlags.None);

    void RemoveNeighbours();

    void SetToConstructionSite();

    void SpawnConstructionStockpile();

    void SpawnFirstItemInConstructionStockpile();

    void UpdateVisual();

    IMyCubeGrid CubeGrid { get; }

    Vector3 GetColorMask();

    void DecreaseMountLevel(
      float grinderAmount,
      IMyInventory outputInventory,
      bool useDefaultDeconstructEfficiency = false);

    void IncreaseMountLevel(
      float welderMountAmount,
      long welderOwnerPlayerId,
      IMyInventory outputInventory = null,
      float maxAllowedBoneMovement = 0.0f,
      bool isHelping = false,
      MyOwnershipShareModeEnum share = MyOwnershipShareModeEnum.Faction);

    int GetConstructionStockpileItemAmount(MyDefinitionId id);

    void MoveItemsToConstructionStockpile(IMyInventory fromInventory);

    void ClearConstructionStockpile(IMyInventory outputInventory);

    void PlayConstructionSound(MyIntegrityChangeEnum integrityChangeType, bool deconstruction = false);

    bool CanContinueBuild(IMyInventory sourceInventory);

    MyDefinitionBase BlockDefinition { get; }

    Vector3I Max { get; }

    Vector3I Min { get; }

    MyBlockOrientation Orientation { get; }

    [Obsolete("Allocates memory, Use GetNeighbours function")]
    List<IMySlimBlock> Neighbours { get; }

    void GetNeighbours(ICollection<IMySlimBlock> collection);

    void GetWorldBoundingBox(out BoundingBoxD aabb, bool useAABBFromBlockCubes = false);

    float Dithering { get; set; }

    long BuiltBy { get; }
  }
}
