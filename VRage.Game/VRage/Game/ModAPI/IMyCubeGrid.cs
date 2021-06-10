// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.IMyCubeGrid
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using VRageMath;

namespace VRage.Game.ModAPI
{
  public interface IMyCubeGrid : VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.Game.ModAPI.Ingame.IMyCubeGrid
  {
    void ApplyDestructionDeformation(IMySlimBlock block);

    List<long> BigOwners { get; }

    List<long> SmallOwners { get; }

    bool IsRespawnGrid { get; set; }

    new bool IsStatic { get; set; }

    void ChangeGridOwnership(long playerId, MyOwnershipShareModeEnum shareMode);

    void ClearSymmetries();

    void ColorBlocks(Vector3I min, Vector3I max, Vector3 newHSV);

    void SkinBlocks(Vector3I min, Vector3I max, Vector3? newHSV, string newSkin);

    [Obsolete("Use IMyCubeGrid.Static instead.")]
    void OnConvertToDynamic();

    void FixTargetCube(out Vector3I cube, Vector3 fractionalGridPosition);

    Vector3 GetClosestCorner(Vector3I gridPos, Vector3 position);

    IMySlimBlock GetCubeBlock(Vector3I pos);

    new string CustomName { get; set; }

    Vector3D? GetLineIntersectionExactAll(
      ref LineD line,
      out double distance,
      out IMySlimBlock intersectedBlock);

    bool GetLineIntersectionExactGrid(
      ref LineD line,
      ref Vector3I position,
      ref double distanceSquared);

    bool IsTouchingAnyNeighbor(Vector3I min, Vector3I max);

    bool CanMergeCubes(IMyCubeGrid gridToMerge, Vector3I gridOffset);

    MatrixI CalculateMergeTransform(IMyCubeGrid gridToMerge, Vector3I gridOffset);

    IMyCubeGrid MergeGrid_MergeBlock(IMyCubeGrid gridToMerge, Vector3I gridOffset);

    Vector3I? RayCastBlocks(Vector3D worldStart, Vector3D worldEnd);

    void RayCastCells(
      Vector3D worldStart,
      Vector3D worldEnd,
      List<Vector3I> outHitPositions,
      Vector3I? gridSizeInflate = null,
      bool havokWorld = false);

    void RazeBlock(Vector3I position);

    void RazeBlocks(ref Vector3I pos, ref Vector3UByte size);

    void RazeBlocks(List<Vector3I> locations);

    void RemoveBlock(IMySlimBlock block, bool updatePhysics = false);

    void RemoveDestroyedBlock(IMySlimBlock block);

    void UpdateBlockNeighbours(IMySlimBlock block);

    new Vector3I WorldToGridInteger(Vector3D coords);

    void GetBlocks(List<IMySlimBlock> blocks, Func<IMySlimBlock, bool> collect = null);

    List<IMySlimBlock> GetBlocksInsideSphere(ref BoundingSphereD sphere);

    event Action<IMySlimBlock> OnBlockAdded;

    event Action<IMySlimBlock> OnBlockRemoved;

    event Action<IMyCubeGrid> OnBlockOwnershipChanged;

    event Action<IMyCubeGrid> OnGridChanged;

    event Action<IMyCubeGrid, IMyCubeGrid> OnGridSplit;

    event Action<IMyCubeGrid, bool> OnIsStaticChanged;

    event Action<IMySlimBlock> OnBlockIntegrityChanged;

    void UpdateOwnership(long ownerId, bool isFunctional);

    IMySlimBlock AddBlock(MyObjectBuilder_CubeBlock objectBuilder, bool testMerge);

    bool WillRemoveBlockSplitGrid(IMySlimBlock testBlock);

    bool CanAddCube(Vector3I pos);

    bool CanAddCubes(Vector3I min, Vector3I max);

    IMyCubeGrid SplitByPlane(PlaneD plane);

    IMyCubeGrid Split(List<IMySlimBlock> blocks, bool sync = true);

    Vector3I? XSymmetryPlane { get; set; }

    Vector3I? YSymmetryPlane { get; set; }

    Vector3I? ZSymmetryPlane { get; set; }

    bool XSymmetryOdd { get; set; }

    bool YSymmetryOdd { get; set; }

    bool ZSymmetryOdd { get; set; }

    bool IsInSameLogicalGroupAs(IMyCubeGrid other);

    bool IsSameConstructAs(IMyCubeGrid other);

    bool IsRoomAtPositionAirtight(Vector3I vector3I);

    MyUpdateTiersGridPresence GridPresenceTier { get; }

    MyUpdateTiersPlayerPresence PlayerPresenceTier { get; }

    event Action<IMyCubeGrid> GridPresenceTierChanged;

    event Action<IMyCubeGrid> PlayerPresenceTierChanged;
  }
}
