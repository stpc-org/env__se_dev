// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyOreDetectorComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using System;
using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
  public class MyOreDetectorComponent
  {
    public const int QUERY_LOD = 2;
    public const int CELL_SIZE_IN_VOXELS_BITS = 3;
    public const int CELL_SIZE_IN_LOD_VOXELS = 8;
    public const float CELL_SIZE_IN_METERS = 32f;
    public const float CELL_SIZE_IN_METERS_HALF = 16f;
    private static readonly List<MyVoxelBase> m_inRangeCache = new List<MyVoxelBase>();
    private static readonly List<MyVoxelBase> m_notInRangeCache = new List<MyVoxelBase>();
    public MyOreDetectorComponent.CheckControlDelegate OnCheckControl;
    private readonly Dictionary<MyVoxelBase, MyOreDepositGroup> m_depositGroupsByEntity = new Dictionary<MyVoxelBase, MyOreDepositGroup>();
    private bool m_discardQueryResult;

    public float DetectionRadius { get; set; }

    public bool BroadcastUsingAntennas { get; set; }

    public MyOreDetectorComponent()
    {
      this.DetectionRadius = 50f;
      this.SetRelayedRequest = false;
      this.BroadcastUsingAntennas = false;
    }

    public bool SetRelayedRequest { get; set; }

    public void Update(Vector3D position, long detectorId, bool checkControl = true)
    {
      if (!this.SetRelayedRequest & checkControl && !this.OnCheckControl())
      {
        this.Clear();
      }
      else
      {
        this.SetRelayedRequest = false;
        BoundingSphereD sphere = new BoundingSphereD(position, (double) this.DetectionRadius);
        MyGamePruningStructure.GetAllVoxelMapsInSphere(ref sphere, MyOreDetectorComponent.m_inRangeCache);
        this.RemoveVoxelMapsOutOfRange();
        this.AddVoxelMapsInRange();
        this.UpdateDeposits(ref sphere, detectorId);
        MyOreDetectorComponent.m_inRangeCache.Clear();
      }
    }

    private void UpdateDeposits(ref BoundingSphereD sphere, long detectorId)
    {
      foreach (MyOreDepositGroup myOreDepositGroup in this.m_depositGroupsByEntity.Values)
        myOreDepositGroup.UpdateDeposits(ref sphere, detectorId);
    }

    private void AddVoxelMapsInRange()
    {
      foreach (MyVoxelBase myVoxelBase in MyOreDetectorComponent.m_inRangeCache)
      {
        if (!(myVoxelBase is MyVoxelPhysics) && !(myVoxelBase.GetTopMostParent((Type) null) is MyVoxelPhysics) && !this.m_depositGroupsByEntity.ContainsKey(myVoxelBase.GetTopMostParent((Type) null) as MyVoxelBase))
          this.m_depositGroupsByEntity.Add(myVoxelBase, new MyOreDepositGroup(myVoxelBase, this));
      }
      MyOreDetectorComponent.m_inRangeCache.Clear();
    }

    private void RemoveVoxelMapsOutOfRange()
    {
      foreach (MyVoxelBase key in this.m_depositGroupsByEntity.Keys)
      {
        if (!MyOreDetectorComponent.m_inRangeCache.Contains(key.GetTopMostParent((Type) null) as MyVoxelBase))
          MyOreDetectorComponent.m_notInRangeCache.Add(key);
      }
      foreach (MyVoxelBase key in MyOreDetectorComponent.m_notInRangeCache)
      {
        MyOreDepositGroup myOreDepositGroup;
        if (this.m_depositGroupsByEntity.TryGetValue(key, out myOreDepositGroup))
          myOreDepositGroup.RemoveMarks();
        this.m_depositGroupsByEntity.Remove(key);
      }
      MyOreDetectorComponent.m_notInRangeCache.Clear();
    }

    public bool WillDiscardNextQuery => this.m_discardQueryResult;

    public void DiscardNextQuery() => this.m_discardQueryResult = true;

    public void EnableNextQuery() => this.m_discardQueryResult = false;

    public void Clear()
    {
      foreach (MyOreDepositGroup myOreDepositGroup in this.m_depositGroupsByEntity.Values)
      {
        myOreDepositGroup.ClearMinMax();
        foreach (MyEntityOreDeposit deposit in (IEnumerable<MyEntityOreDeposit>) myOreDepositGroup.Deposits)
          MyHud.OreMarkers.UnregisterMarker(deposit);
      }
    }

    public delegate bool CheckControlDelegate();
  }
}
