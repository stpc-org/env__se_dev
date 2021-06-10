// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyOreDepositGroup
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Voxels;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
  internal class MyOreDepositGroup
  {
    private readonly MyVoxelBase m_voxelMap;
    private readonly MyOreDetectorComponent m_orderDetectorComponent;
    private readonly Action<List<MyEntityOreDeposit>, List<Vector3I>> m_onDepositQueryComplete;
    private Dictionary<Vector3I, MyEntityOreDeposit> m_depositsByCellCoord_Main = new Dictionary<Vector3I, MyEntityOreDeposit>((IEqualityComparer<Vector3I>) Vector3I.Comparer);
    private Dictionary<Vector3I, MyEntityOreDeposit> m_depositsByCellCoord_Swap = new Dictionary<Vector3I, MyEntityOreDeposit>((IEqualityComparer<Vector3I>) Vector3I.Comparer);
    private Vector3I m_lastDetectionMin;
    private Vector3I m_lastDetectionMax;
    private int m_tasksRunning;

    public void ClearMinMax() => this.m_lastDetectionMin = this.m_lastDetectionMax = Vector3I.Zero;

    public ICollection<MyEntityOreDeposit> Deposits => (ICollection<MyEntityOreDeposit>) this.m_depositsByCellCoord_Main.Values;

    public MyOreDepositGroup(MyVoxelBase voxelMap, MyOreDetectorComponent oreDetector)
    {
      this.m_voxelMap = voxelMap;
      this.m_orderDetectorComponent = oreDetector;
      this.m_onDepositQueryComplete = new Action<List<MyEntityOreDeposit>, List<Vector3I>>(this.OnDepositQueryComplete);
      this.m_lastDetectionMax = new Vector3I(int.MinValue);
      this.m_lastDetectionMin = new Vector3I(int.MaxValue);
    }

    private void OnDepositQueryComplete(
      List<MyEntityOreDeposit> deposits,
      List<Vector3I> emptyCells)
    {
      foreach (MyEntityOreDeposit deposit in deposits)
        this.m_depositsByCellCoord_Swap[deposit.CellCoord] = deposit;
      --this.m_tasksRunning;
      if (this.m_tasksRunning != 0)
        return;
      if (this.m_orderDetectorComponent.WillDiscardNextQuery)
      {
        foreach (MyEntityOreDeposit deposit in this.m_depositsByCellCoord_Main.Values)
          MyHud.OreMarkers.UnregisterMarker(deposit);
        foreach (MyEntityOreDeposit deposit in this.m_depositsByCellCoord_Swap.Values)
          MyHud.OreMarkers.UnregisterMarker(deposit);
        this.m_depositsByCellCoord_Main.Clear();
        this.m_depositsByCellCoord_Swap.Clear();
      }
      else
      {
        Dictionary<Vector3I, MyEntityOreDeposit> depositsByCellCoordMain = this.m_depositsByCellCoord_Main;
        this.m_depositsByCellCoord_Main = this.m_depositsByCellCoord_Swap;
        this.m_depositsByCellCoord_Swap = depositsByCellCoordMain;
        foreach (MyEntityOreDeposit deposit in this.m_depositsByCellCoord_Swap.Values)
          MyHud.OreMarkers.UnregisterMarker(deposit);
        this.m_depositsByCellCoord_Swap.Clear();
        foreach (MyEntityOreDeposit deposit in this.m_depositsByCellCoord_Main.Values)
          MyHud.OreMarkers.RegisterMarker(deposit);
      }
    }

    public void UpdateDeposits(ref BoundingSphereD worldDetectionSphere, long detectorId)
    {
      if (this.m_tasksRunning != 0)
        return;
      MySession mySession = MySession.Static;
      if (mySession == null || !mySession.Ready)
        return;
      Vector3D worldPosition1 = worldDetectionSphere.Center - worldDetectionSphere.Radius;
      Vector3D worldPosition2 = worldDetectionSphere.Center + worldDetectionSphere.Radius;
      Vector3I voxelCoord1;
      MyVoxelCoordSystems.WorldPositionToVoxelCoord(this.m_voxelMap.PositionLeftBottomCorner, ref worldPosition1, out voxelCoord1);
      Vector3I voxelCoord2;
      MyVoxelCoordSystems.WorldPositionToVoxelCoord(this.m_voxelMap.PositionLeftBottomCorner, ref worldPosition2, out voxelCoord2);
      Vector3I voxelCoord3 = voxelCoord1 + this.m_voxelMap.StorageMin;
      Vector3I voxelCoord4 = voxelCoord2 + this.m_voxelMap.StorageMin;
      this.m_voxelMap.Storage.ClampVoxelCoord(ref voxelCoord3);
      this.m_voxelMap.Storage.ClampVoxelCoord(ref voxelCoord4);
      Vector3I vector3I1 = voxelCoord3 >> 5;
      Vector3I vector3I2 = voxelCoord4 >> 5;
      if (vector3I1 == this.m_lastDetectionMin && vector3I2 == this.m_lastDetectionMax)
        return;
      this.m_lastDetectionMin = vector3I1;
      this.m_lastDetectionMax = vector3I2;
      int num1 = Math.Max((vector3I2.X - vector3I1.X) / 2, 1);
      int num2 = Math.Max((vector3I2.Y - vector3I1.Y) / 2, 1);
      Vector3I min;
      min.Z = vector3I1.Z;
      Vector3I max;
      max.Z = vector3I2.Z;
      for (int index1 = 0; index1 < 2; ++index1)
      {
        for (int index2 = 0; index2 < 2; ++index2)
        {
          min.X = vector3I1.X + index1 * num1;
          min.Y = vector3I1.Y + index2 * num2;
          max.X = min.X + num1;
          max.Y = min.Y + num2;
          MyDepositQuery.Start(min, max, detectorId, this.m_voxelMap, this.m_onDepositQueryComplete);
          ++this.m_tasksRunning;
        }
      }
    }

    internal void RemoveMarks()
    {
      foreach (MyEntityOreDeposit deposit in this.m_depositsByCellCoord_Main.Values)
        MyHud.OreMarkers.UnregisterMarker(deposit);
    }
  }
}
