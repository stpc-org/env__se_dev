// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyOxygenCube
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Concurrent;
using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Game.GameSystems
{
  public class MyOxygenCube
  {
    private readonly Vector3I m_defaultCellSize = new Vector3I(10, 10, 10);
    private readonly Vector3I m_defaultBaseOffset = new Vector3I(5, 5, 5);
    private Vector3I m_cellSize;
    private Vector3I m_baseOffset;
    private ConcurrentDictionary<Vector3I, MyOxygenBlock[]> m_dictionary;

    public MyOxygenBlock this[int x, int y, int z]
    {
      get
      {
        MyOxygenBlock myOxygenBlock;
        this.TryGetValue(new Vector3I(x, y, z), out myOxygenBlock);
        return myOxygenBlock;
      }
      set => this.Add(new Vector3I(x, y, z), value);
    }

    public MyOxygenCube()
    {
      this.m_cellSize = this.m_defaultCellSize;
      this.m_baseOffset = this.m_defaultBaseOffset;
      this.m_dictionary = new ConcurrentDictionary<Vector3I, MyOxygenBlock[]>((IEqualityComparer<Vector3I>) new Vector3I.EqualityComparer());
    }

    public MyOxygenCube(Vector3I cellSize)
    {
      this.m_cellSize = cellSize;
      this.m_baseOffset = cellSize / 2;
      this.m_dictionary = new ConcurrentDictionary<Vector3I, MyOxygenBlock[]>((IEqualityComparer<Vector3I>) new Vector3I.EqualityComparer());
    }

    public void Add(Vector3I key, MyOxygenBlock value)
    {
      Vector3I cellPosition;
      this.GetCellPosition(key, out cellPosition);
      MyOxygenBlock[] myOxygenBlockArray;
      if (!this.m_dictionary.TryGetValue(cellPosition, out myOxygenBlockArray))
      {
        myOxygenBlockArray = new MyOxygenBlock[this.m_cellSize.Volume()];
        this.m_dictionary.TryAdd(cellPosition, myOxygenBlockArray);
      }
      Vector3I vector3I = key - cellPosition;
      int index = vector3I.X + vector3I.Y * this.m_cellSize.X + vector3I.Z * this.m_cellSize.X * this.m_cellSize.Y;
      myOxygenBlockArray[index] = value;
    }

    private void GetCellPosition(Vector3I key, out Vector3I cellPosition)
    {
      if (-this.m_baseOffset.X > key.X)
        key.X -= this.m_cellSize.X - 1;
      if (-this.m_baseOffset.Y > key.Y)
        key.Y -= this.m_cellSize.Y - 1;
      if (-this.m_baseOffset.Z > key.Z)
        key.Z -= this.m_cellSize.Z - 1;
      Vector3I vector3I = (key + this.m_baseOffset) / this.m_cellSize;
      cellPosition = this.m_baseOffset + (vector3I - 1) * this.m_cellSize;
    }

    public bool TryGetValue(Vector3I key, out MyOxygenBlock value)
    {
      Vector3I cellPosition;
      this.GetCellPosition(key, out cellPosition);
      MyOxygenBlock[] myOxygenBlockArray;
      if (!this.m_dictionary.TryGetValue(cellPosition, out myOxygenBlockArray))
      {
        value = (MyOxygenBlock) null;
        return false;
      }
      Vector3I vector3I = key - cellPosition;
      int index = vector3I.X + vector3I.Y * this.m_cellSize.X + vector3I.Z * this.m_cellSize.X * this.m_cellSize.Y;
      value = myOxygenBlockArray[index];
      return value != null;
    }

    private class Cell
    {
      public MyOxygenBlock[] Data;
    }
  }
}
