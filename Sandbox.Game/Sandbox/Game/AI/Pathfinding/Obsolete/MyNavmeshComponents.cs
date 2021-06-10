// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.Obsolete.MyNavmeshComponents
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Collections;
using VRageMath;

namespace Sandbox.Game.AI.Pathfinding.Obsolete
{
  public class MyNavmeshComponents
  {
    private readonly Dictionary<ulong, MyNavmeshComponents.CellInfo> m_cellInfos;
    private readonly Dictionary<int, ulong> m_componentCells;
    private bool m_cellOpen;
    private bool m_componentOpen;
    private readonly Dictionary<ushort, List<int>> m_componentIndicesAvailable;
    private int m_nextComponentIndex;
    private readonly List<Vector3> m_lastCellComponentCenters;
    private ulong m_cellCoord;
    private ushort m_componentNum;
    private List<MyIntervalList> m_components;

    public MyNavmeshComponents()
    {
      this.m_cellOpen = false;
      this.m_componentOpen = false;
      this.m_cellInfos = new Dictionary<ulong, MyNavmeshComponents.CellInfo>();
      this.m_componentCells = new Dictionary<int, ulong>();
      this.m_componentIndicesAvailable = new Dictionary<ushort, List<int>>();
      this.m_nextComponentIndex = 0;
      this.m_components = (List<MyIntervalList>) null;
      this.m_lastCellComponentCenters = new List<Vector3>();
    }

    public void OpenCell(ulong cellCoord)
    {
      this.m_cellOpen = true;
      this.m_cellCoord = cellCoord;
      this.m_componentNum = (ushort) 0;
      this.m_components = new List<MyIntervalList>();
      this.m_lastCellComponentCenters.Clear();
    }

    public void CloseAndCacheCell(ref MyNavmeshComponents.ClosedCellInfo output)
    {
      MyNavmeshComponents.CellInfo cellInfo = new MyNavmeshComponents.CellInfo();
      bool flag = true;
      if (this.m_cellInfos.TryGetValue(this.m_cellCoord, out cellInfo))
      {
        output.NewCell = false;
        output.OldComponentNum = cellInfo.ComponentNum;
        output.OldStartingIndex = cellInfo.StartingIndex;
        if ((int) cellInfo.ComponentNum == (int) this.m_componentNum)
        {
          flag = false;
          cellInfo.ComponentNum = output.OldComponentNum;
          cellInfo.StartingIndex = output.OldStartingIndex;
        }
      }
      else
        output.NewCell = true;
      if (flag)
      {
        cellInfo.ComponentNum = this.m_componentNum;
        cellInfo.StartingIndex = this.AllocateComponentStartingIndex(this.m_componentNum);
        if (!output.NewCell)
        {
          this.DeallocateComponentStartingIndex(output.OldStartingIndex, output.OldComponentNum);
          for (int index = 0; index < (int) output.OldComponentNum; ++index)
            this.m_componentCells.Remove(output.OldStartingIndex + index);
        }
        for (int index = 0; index < (int) cellInfo.ComponentNum; ++index)
          this.m_componentCells[cellInfo.StartingIndex + index] = this.m_cellCoord;
      }
      this.m_cellInfos[this.m_cellCoord] = cellInfo;
      output.ComponentNum = cellInfo.ComponentNum;
      output.ExploredDirections = cellInfo.ExploredDirections;
      output.StartingIndex = cellInfo.StartingIndex;
      this.m_components = (List<MyIntervalList>) null;
      this.m_componentNum = (ushort) 0;
      this.m_cellOpen = false;
    }

    public void OpenComponent()
    {
      this.m_componentOpen = true;
      this.m_lastCellComponentCenters.Add(Vector3.Zero);
      this.m_components.Add(new MyIntervalList());
    }

    public void AddComponentTriangle(MyNavigationTriangle triangle, Vector3 center)
    {
      int index = triangle.Index;
      MyIntervalList component = this.m_components[(int) this.m_componentNum];
      component.Add(index);
      float num = 1f / (float) component.Count;
      this.m_lastCellComponentCenters[(int) this.m_componentNum] = center * num + this.m_lastCellComponentCenters[(int) this.m_componentNum] * (1f - num);
    }

    public void CloseComponent()
    {
      ++this.m_componentNum;
      this.m_componentOpen = false;
    }

    public Vector3 GetComponentCenter(int index) => this.m_lastCellComponentCenters[index];

    public bool TryGetComponentCell(int componentIndex, out ulong cellIndex) => this.m_componentCells.TryGetValue(componentIndex, out cellIndex);

    public bool GetComponentCell(int componentIndex, out ulong cellIndex) => this.m_componentCells.TryGetValue(componentIndex, out cellIndex);

    public bool GetComponentInfo(
      int componentIndex,
      ulong cellIndex,
      out Base6Directions.DirectionFlags exploredDirections)
    {
      exploredDirections = (Base6Directions.DirectionFlags) 0;
      MyNavmeshComponents.CellInfo cellInfo;
      this.TryGetCell(cellIndex, out cellInfo);
      int num = componentIndex - cellInfo.StartingIndex;
      if (num < 0 || num >= (int) cellInfo.ComponentNum)
        return false;
      exploredDirections = cellInfo.ExploredDirections;
      return true;
    }

    public void MarkExplored(ulong otherCell, Base6Directions.Direction direction)
    {
      MyNavmeshComponents.CellInfo cellInfo = new MyNavmeshComponents.CellInfo();
      if (!this.m_cellInfos.TryGetValue(otherCell, out cellInfo))
        return;
      cellInfo.ExploredDirections |= Base6Directions.GetDirectionFlag(direction);
      this.m_cellInfos[otherCell] = cellInfo;
    }

    public void SetExplored(ulong packedCoord, Base6Directions.DirectionFlags directionFlags)
    {
      MyNavmeshComponents.CellInfo cellInfo = new MyNavmeshComponents.CellInfo();
      if (!this.m_cellInfos.TryGetValue(packedCoord, out cellInfo))
        return;
      cellInfo.ExploredDirections = directionFlags;
      this.m_cellInfos[packedCoord] = cellInfo;
    }

    public bool TryGetCell(ulong packedCoord, out MyNavmeshComponents.CellInfo cellInfo) => this.m_cellInfos.TryGetValue(packedCoord, out cellInfo);

    public ICollection<ulong> GetPresentCells() => (ICollection<ulong>) this.m_cellInfos.Keys;

    public void ClearCell(ulong packedCoord, ref MyNavmeshComponents.CellInfo cellInfo)
    {
      for (int index = 0; index < (int) cellInfo.ComponentNum; ++index)
        this.m_componentCells.Remove(cellInfo.StartingIndex + index);
      this.m_cellInfos.Remove(packedCoord);
      this.DeallocateComponentStartingIndex(cellInfo.StartingIndex, cellInfo.ComponentNum);
    }

    private int AllocateComponentStartingIndex(ushort componentNum)
    {
      List<int> intList = (List<int>) null;
      if (this.m_componentIndicesAvailable.TryGetValue(componentNum, out intList) && intList.Count > 0)
      {
        int num = intList[intList.Count - 1];
        intList.RemoveAt(intList.Count - 1);
        return num;
      }
      int nextComponentIndex = this.m_nextComponentIndex;
      this.m_nextComponentIndex += (int) componentNum;
      return nextComponentIndex;
    }

    private void DeallocateComponentStartingIndex(int index, ushort componentNum)
    {
      List<int> intList = (List<int>) null;
      if (!this.m_componentIndicesAvailable.TryGetValue(componentNum, out intList))
      {
        intList = new List<int>();
        this.m_componentIndicesAvailable[componentNum] = intList;
      }
      intList.Add(index);
    }

    public struct CellInfo
    {
      public int StartingIndex;
      public ushort ComponentNum;
      public Base6Directions.DirectionFlags ExploredDirections;

      public override string ToString() => this.ComponentNum.ToString() + " components: " + (object) this.StartingIndex + " - " + (object) (this.StartingIndex + (int) this.ComponentNum - 1) + ", Expl.: " + (object) this.ExploredDirections;
    }

    public struct ClosedCellInfo
    {
      public bool NewCell;
      public int OldStartingIndex;
      public ushort OldComponentNum;
      public int StartingIndex;
      public ushort ComponentNum;
      public Base6Directions.DirectionFlags ExploredDirections;
    }
  }
}
