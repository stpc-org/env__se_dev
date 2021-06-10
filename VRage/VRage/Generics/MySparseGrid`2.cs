// Decompiled with JetBrains decompiler
// Type: VRage.Generics.MySparseGrid`2
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections;
using System.Collections.Generic;
using VRage.Collections;
using VRageMath;

namespace VRage.Generics
{
  public class MySparseGrid<TItemData, TCellData> : IDictionary<Vector3I, TItemData>, ICollection<KeyValuePair<Vector3I, TItemData>>, IEnumerable<KeyValuePair<Vector3I, TItemData>>, IEnumerable
  {
    private int m_itemCount;
    private Dictionary<Vector3I, MySparseGrid<TItemData, TCellData>.Cell> m_cells = new Dictionary<Vector3I, MySparseGrid<TItemData, TCellData>.Cell>();
    private HashSet<Vector3I> m_dirtyCells = new HashSet<Vector3I>();
    public readonly int CellSize;

    public DictionaryReader<Vector3I, MySparseGrid<TItemData, TCellData>.Cell> Cells => new DictionaryReader<Vector3I, MySparseGrid<TItemData, TCellData>.Cell>(this.m_cells);

    public HashSetReader<Vector3I> DirtyCells => (HashSetReader<Vector3I>) this.m_dirtyCells;

    public int ItemCount => this.m_itemCount;

    public int CellCount => this.m_cells.Count;

    public MySparseGrid(int cellSize) => this.CellSize = cellSize;

    public Vector3I Add(Vector3I pos, TItemData data)
    {
      Vector3I cell = pos / this.CellSize;
      this.GetCell(cell, true).m_items.Add(pos, data);
      this.MarkDirty(cell);
      ++this.m_itemCount;
      return cell;
    }

    public bool Contains(Vector3I pos)
    {
      MySparseGrid<TItemData, TCellData>.Cell cell = this.GetCell(pos / this.CellSize, false);
      return cell != null && cell.m_items.ContainsKey(pos);
    }

    public bool Remove(Vector3I pos, bool removeEmptyCell = true)
    {
      Vector3I vector3I = pos / this.CellSize;
      MySparseGrid<TItemData, TCellData>.Cell cell = this.GetCell(vector3I, false);
      if (cell == null || !cell.m_items.Remove(pos))
        return false;
      this.MarkDirty(vector3I);
      --this.m_itemCount;
      if (removeEmptyCell && cell.m_items.Count == 0)
        this.m_cells.Remove(vector3I);
      return true;
    }

    public void Clear()
    {
      this.m_cells.Clear();
      this.m_itemCount = 0;
    }

    public void ClearCells()
    {
      foreach (KeyValuePair<Vector3I, MySparseGrid<TItemData, TCellData>.Cell> cell in this.m_cells)
        cell.Value.m_items.Clear();
      this.m_itemCount = 0;
    }

    public TItemData Get(Vector3I pos) => this.GetCell(pos / this.CellSize, false).m_items[pos];

    public bool TryGet(Vector3I pos, out TItemData data)
    {
      MySparseGrid<TItemData, TCellData>.Cell cell = this.GetCell(pos / this.CellSize, false);
      if (cell != null)
        return cell.m_items.TryGetValue(pos, out data);
      data = default (TItemData);
      return false;
    }

    public MySparseGrid<TItemData, TCellData>.Cell GetCell(Vector3I cell) => this.m_cells[cell];

    public bool TryGetCell(Vector3I cell, out MySparseGrid<TItemData, TCellData>.Cell result) => this.m_cells.TryGetValue(cell, out result);

    private MySparseGrid<TItemData, TCellData>.Cell GetCell(
      Vector3I cell,
      bool createIfNotExists)
    {
      MySparseGrid<TItemData, TCellData>.Cell cell1;
      if (!this.m_cells.TryGetValue(cell, out cell1) & createIfNotExists)
      {
        cell1 = new MySparseGrid<TItemData, TCellData>.Cell();
        this.m_cells[cell] = cell1;
      }
      return cell1;
    }

    public bool IsDirty(Vector3I cell) => this.m_dirtyCells.Contains(cell);

    public void MarkDirty(Vector3I cell) => this.m_dirtyCells.Add(cell);

    public void UnmarkDirty(Vector3I cell) => this.m_dirtyCells.Remove(cell);

    public void UnmarkDirtyAll() => this.m_dirtyCells.Clear();

    public Dictionary<Vector3I, MySparseGrid<TItemData, TCellData>.Cell>.Enumerator GetEnumerator() => this.m_cells.GetEnumerator();

    void IDictionary<Vector3I, TItemData>.Add(Vector3I key, TItemData value) => this.Add(key, value);

    bool IDictionary<Vector3I, TItemData>.ContainsKey(Vector3I key) => this.Contains(key);

    ICollection<Vector3I> IDictionary<Vector3I, TItemData>.Keys => throw new InvalidOperationException("Operation not supported");

    bool IDictionary<Vector3I, TItemData>.Remove(Vector3I key) => this.Remove(key);

    bool IDictionary<Vector3I, TItemData>.TryGetValue(
      Vector3I key,
      out TItemData value)
    {
      return this.TryGet(key, out value);
    }

    ICollection<TItemData> IDictionary<Vector3I, TItemData>.Values => throw new InvalidOperationException("Operation not supported");

    TItemData IDictionary<Vector3I, TItemData>.this[Vector3I key]
    {
      get => this.Get(key);
      set
      {
        this.Remove(key);
        this.Add(key, value);
      }
    }

    void ICollection<KeyValuePair<Vector3I, TItemData>>.Add(
      KeyValuePair<Vector3I, TItemData> item)
    {
      this.Add(item.Key, item.Value);
    }

    void ICollection<KeyValuePair<Vector3I, TItemData>>.Clear() => this.Clear();

    bool ICollection<KeyValuePair<Vector3I, TItemData>>.Contains(
      KeyValuePair<Vector3I, TItemData> item)
    {
      throw new InvalidOperationException("Operation not supported");
    }

    void ICollection<KeyValuePair<Vector3I, TItemData>>.CopyTo(
      KeyValuePair<Vector3I, TItemData>[] array,
      int arrayIndex)
    {
      throw new InvalidOperationException("Operation not supported");
    }

    int ICollection<KeyValuePair<Vector3I, TItemData>>.Count => this.m_itemCount;

    bool ICollection<KeyValuePair<Vector3I, TItemData>>.IsReadOnly => false;

    bool ICollection<KeyValuePair<Vector3I, TItemData>>.Remove(
      KeyValuePair<Vector3I, TItemData> item)
    {
      throw new InvalidOperationException("Operation not supported");
    }

    IEnumerator<KeyValuePair<Vector3I, TItemData>> IEnumerable<KeyValuePair<Vector3I, TItemData>>.GetEnumerator() => throw new InvalidOperationException("Operation not supported");

    IEnumerator IEnumerable.GetEnumerator() => throw new InvalidOperationException("Operation not supported");

    public class Cell
    {
      internal Dictionary<Vector3I, TItemData> m_items = new Dictionary<Vector3I, TItemData>();
      public TCellData CellData;

      public DictionaryReader<Vector3I, TItemData> Items => new DictionaryReader<Vector3I, TItemData>(this.m_items);
    }
  }
}
