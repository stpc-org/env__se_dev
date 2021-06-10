// Decompiled with JetBrains decompiler
// Type: VRage.Voxels.Clipmap.MyVoxelClipmapRing
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;
using System.Linq;
using VRage.Collections;
using VRage.Game;
using VRage.Utils;
using VRage.Voxels.Sewing;
using VRageMath;
using VRageRender;
using VRageRender.Voxels;

namespace VRage.Voxels.Clipmap
{
  internal class MyVoxelClipmapRing
  {
    private readonly MyVoxelClipmap m_clipmap;
    private readonly int m_lod;
    private Vector3L m_max;
    private readonly Dictionary<Vector3I, MyVoxelClipmapRing.CellData> m_cells = new Dictionary<Vector3I, MyVoxelClipmapRing.CellData>((IEqualityComparer<Vector3I>) Vector3I.Comparer);
    private readonly HashSet<Vector3I> m_cellsRemove = new HashSet<Vector3I>((IEqualityComparer<Vector3I>) Vector3I.Comparer);
    private readonly HashSet<Vector3I> m_cellsAdd = new HashSet<Vector3I>((IEqualityComparer<Vector3I>) Vector3I.Comparer);
    private readonly HashSet<Vector3I> m_cellsReStitch = new HashSet<Vector3I>((IEqualityComparer<Vector3I>) Vector3I.Comparer);
    internal BoundingBoxI Bounds;
    internal BoundingBoxI InnerBounds;
    internal bool BoundsChanged;

    public int Lod => this.m_lod;

    public DictionaryReader<Vector3I, MyVoxelClipmapRing.CellData> Cells => (DictionaryReader<Vector3I, MyVoxelClipmapRing.CellData>) this.m_cells;

    public MyVoxelClipmapRing(MyVoxelClipmap clipmap, int lod)
    {
      this.m_clipmap = clipmap;
      this.m_lod = lod;
    }

    internal void UpdateSize(Vector3I size) => this.m_max = (Vector3L) (size + 1 >> 1 << 1);

    public void Update(Vector3L relativePosition)
    {
      this.BoundsChanged = false;
      Vector3L result1 = relativePosition - (long) this.m_clipmap.Ranges[this.m_lod] >> this.m_lod >> this.m_clipmap.CellBits;
      Vector3L result2 = relativePosition + (long) this.m_clipmap.Ranges[this.m_lod] >> this.m_lod >> this.m_clipmap.CellBits;
      Vector3L.Clamp(ref result1, ref Vector3L.Zero, ref this.m_max, out result1);
      Vector3L.Clamp(ref result2, ref Vector3L.Zero, ref this.m_max, out result2);
      BoundingBoxI boundingBoxI1 = new BoundingBoxI((Vector3I) result1, (Vector3I) result2);
      boundingBoxI1.Min = boundingBoxI1.Min >> 1 << 1;
      boundingBoxI1.Max = boundingBoxI1.Max + 1 >> 1 << 1;
      BoundingBoxI innerBounds;
      bool flag;
      if (this.m_lod > 0)
      {
        MyVoxelClipmapRing ring = this.m_clipmap.Rings[this.m_lod - 1];
        innerBounds.Min = ring.Bounds.Min >> 1;
        innerBounds.Max = ring.Bounds.Max >> 1;
        flag = ring.BoundsChanged;
      }
      else
      {
        innerBounds = this.InnerBounds;
        flag = false;
      }
      if (boundingBoxI1 != this.Bounds)
      {
        foreach (Vector3I key in BoundingBoxI.IterateDifference(this.Bounds, boundingBoxI1))
        {
          if (this.m_cells.ContainsKey(key))
            this.m_cellsRemove.Add(key);
        }
        foreach (Vector3I vector3I in BoundingBoxI.IterateDifference(boundingBoxI1, this.Bounds))
        {
          if ((this.m_lod <= 0 || !vector3I.IsInside(ref innerBounds.Min, ref innerBounds.Max)) && !this.m_cells.ContainsKey(vector3I))
          {
            MyVoxelContentConstitution constitution = this.m_clipmap.ApproximateCellConstitution(vector3I, this.m_lod);
            if (constitution == MyVoxelContentConstitution.Mixed)
              this.m_cellsAdd.Add(vector3I);
            else
              this.m_cells.Add(vector3I, new MyVoxelClipmapRing.CellData(MyVoxelClipmapRing.CellStatus.Empty, constitution));
          }
        }
        BoundingBoxI left = boundingBoxI1.Intersect(this.Bounds);
        BoundingBoxI right = new BoundingBoxI(left.Min, left.Max - 1);
        foreach (Vector3I key in BoundingBoxI.IterateDifference(left, right))
        {
          MyVoxelClipmapRing.CellData cellData;
          if (this.m_cells.TryGetValue(key, out cellData) && cellData.Guide != null)
            this.m_cellsReStitch.Add(key);
        }
        this.BoundsChanged = true;
        this.Bounds = boundingBoxI1;
      }
      if (!flag)
        return;
      BoundingBoxI boundingBoxI2 = this.Bounds.Intersect(this.InnerBounds);
      foreach (Vector3I vector3I in BoundingBoxI.IterateDifference(boundingBoxI2, innerBounds))
      {
        MyVoxelContentConstitution constitution = this.m_clipmap.ApproximateCellConstitution(vector3I, this.m_lod);
        if (constitution == MyVoxelContentConstitution.Mixed)
          this.m_cellsAdd.Add(vector3I);
        else
          this.m_cells.Add(vector3I, new MyVoxelClipmapRing.CellData(MyVoxelClipmapRing.CellStatus.Empty, constitution));
      }
      foreach (Vector3I key in BoundingBoxI.IterateDifference(innerBounds, boundingBoxI2))
      {
        if (this.m_cells.ContainsKey(key))
        {
          this.m_cellsRemove.Add(key);
          this.m_cellsReStitch.Remove(key);
        }
      }
      BoundingBoxI left1 = this.Bounds.Intersect(new BoundingBoxI(innerBounds.Min - 1, innerBounds.Max));
      foreach (Vector3I key in BoundingBoxI.IterateDifference(this.Bounds.Intersect(new BoundingBoxI(this.InnerBounds.Min - 1, this.InnerBounds.Max)), this.InnerBounds).Concat<Vector3I>(BoundingBoxI.IterateDifference(left1, innerBounds)))
      {
        MyVoxelClipmapRing.CellData cellData;
        if (!this.m_cellsRemove.Contains(key) && this.m_cells.TryGetValue(key, out cellData) && cellData.Guide != null)
          this.m_cellsReStitch.Add(key);
      }
      this.InnerBounds = innerBounds;
    }

    public void ProcessChanges()
    {
      foreach (Vector3I cell in this.m_cellsAdd)
        this.AddCell(cell);
      this.m_cellsAdd.Clear();
      foreach (Vector3I cell in this.m_cellsRemove)
        this.RemoveCell(cell);
      this.m_cellsRemove.Clear();
    }

    public void DispatchStitchingRefreshes()
    {
      foreach (Vector3I coordInLod in this.m_cellsReStitch)
        this.m_clipmap.Stitch(new MyCellCoord(this.m_lod, coordInLod), MyClipmapCellVicinity.Invalid);
      this.m_cellsReStitch.Clear();
    }

    private void DisposeCell(Vector3I coord, MyVoxelClipmapRing.CellData data)
    {
      data.Dispose(this.m_clipmap);
      if (data.Cell != null)
      {
        this.m_clipmap.Actor.DeleteCell(data.Cell);
        data.Cell = (IMyVoxelActorCell) null;
      }
      if (data.Guide == null)
        return;
      data.Guide.RemoveReference();
      data.Guide = (VrSewGuide) null;
    }

    private void AddCell(Vector3I cell)
    {
      MyVoxelClipmapRing.CellData cellData = new MyVoxelClipmapRing.CellData();
      this.m_cells.Add(cell, cellData);
      this.m_clipmap.RequestCell(cell, this.m_lod);
    }

    private void RemoveCell(Vector3I cell)
    {
      MyVoxelClipmapRing.CellData cell1 = this.m_cells[cell];
      this.RemoveImmediately(cell, cell1);
    }

    private void RemoveImmediately(Vector3I cell, MyVoxelClipmapRing.CellData cellData)
    {
      this.DisposeCell(cell, cellData);
      this.m_cells.Remove(cell);
    }

    public void FinishAdd(Vector3I cell)
    {
      MyVoxelClipmapRing.CellData cellData;
      if (!this.m_cells.TryGetValue(cell, out cellData) || cellData.Cell == null)
        return;
      cellData.Cell.SetVisible(true);
    }

    public void FinishRemove(Vector3I cell)
    {
      MyVoxelClipmapRing.CellData cellData;
      if (!this.m_cells.TryGetValue(cell, out cellData))
        return;
      this.RemoveImmediately(cell, cellData);
    }

    public void UpdateCellData(
      Vector3I cell,
      VrSewGuide guide,
      MyVoxelContentConstitution constitution)
    {
      MyVoxelClipmapRing.CellData cellData;
      if (!this.m_cells.TryGetValue(cell, out cellData))
        return;
      if (cellData.Guide != null && cellData.Guide != guide && cellData.Guide != null)
        cellData.Guide.RemoveReference();
      cellData.Guide = guide;
      cellData.Constitution = constitution;
      if (guide != null && guide.Mesh != null)
      {
        cellData.Status = MyVoxelClipmapRing.CellStatus.Calculated;
      }
      else
      {
        cellData.Status = MyVoxelClipmapRing.CellStatus.Empty;
        if (cellData.Cell == null)
          return;
        this.m_clipmap.Actor.DeleteCell(cellData.Cell);
        cellData.Cell = (IMyVoxelActorCell) null;
      }
    }

    public bool UpdateCellRender(
      Vector3I cell,
      ref MyVoxelRenderCellData updateData,
      ref IMyVoxelUpdateBatch updateBatch)
    {
      bool flag = false;
      MyVoxelClipmapRing.CellData cellData;
      if (this.m_cells.TryGetValue(cell, out cellData))
      {
        if (updateData.Parts != null && updateData.Parts.Length != 0)
        {
          if (cellData.Cell == null)
          {
            cellData.Cell = this.m_clipmap.Actor.CreateCell((Vector3D) (cell << this.m_lod + this.m_clipmap.CellBits), this.m_lod);
            flag = true;
          }
          cellData.Cell.UpdateMesh(ref updateData, ref updateBatch);
          cellData.Status = MyVoxelClipmapRing.CellStatus.Ready;
          cellData.Cell.SetVisible(true);
        }
        else if (cellData.Cell != null)
        {
          this.m_clipmap.Actor.DeleteCell(cellData.Cell);
          cellData.Cell = (IMyVoxelActorCell) null;
        }
      }
      return flag;
    }

    internal bool TryGetCell(Vector3I cell, out MyVoxelClipmapRing.CellData data) => this.m_cells.TryGetValue(cell, out data);

    internal MyClipmapCellVicinity GetCellVicinity(Vector3I cell)
    {
      MyVoxelClipmapRing.CellData cellData;
      return this.m_cells.TryGetValue(cell, out cellData) ? cellData.Vicinity : MyClipmapCellVicinity.Invalid;
    }

    internal bool IsInnerLodEdge(Vector3I cell) => cell.X == this.InnerBounds.Min.X - 1 || cell.Y == this.InnerBounds.Min.Y - 1 || cell.Z == this.InnerBounds.Min.Z - 1;

    internal bool IsInnerLodEdge(Vector3I cell, out int innerCornerIndex)
    {
      int num = cell.X == this.InnerBounds.Min.X - 1 ? 1 : 0;
      bool flag1 = cell.Y == this.InnerBounds.Min.Y - 1;
      bool flag2 = cell.Z == this.InnerBounds.Min.Z - 1;
      innerCornerIndex = 0;
      if (num != 0)
        innerCornerIndex |= 1;
      if (flag1)
        innerCornerIndex |= 2;
      if (flag2)
        innerCornerIndex |= 4;
      return (uint) innerCornerIndex > 0U;
    }

    internal bool IsInsideInnerLod(Vector3I cell) => this.InnerBounds.Contains(cell) == ContainmentType.Contains;

    internal bool IsInBounds(Vector3I cell) => this.Bounds.Contains(cell) == ContainmentType.Contains;

    public bool IsForwardEdge(Vector3I cell) => cell.X == this.Bounds.Max.X - 1 || cell.Y == this.Bounds.Max.Y - 1 || cell.Z == this.Bounds.Max.Z - 1;

    internal void InvalidateRange(BoundingBoxI range)
    {
      Vector3I minRange = range.Min >> this.m_lod;
      range.Min >>= this.m_lod + this.m_clipmap.CellBits;
      range.Max += (1 << this.m_lod + this.m_clipmap.CellBits) - 1;
      range.Max >>= this.m_lod + this.m_clipmap.CellBits;
      range = range.Intersect(this.Bounds);
      foreach (Vector3I enumeratePoint in BoundingBoxI.EnumeratePoints(range))
      {
        MyVoxelClipmapRing.CellData cellData;
        if (this.m_cells.TryGetValue(enumeratePoint, out cellData) && cellData.Status != MyVoxelClipmapRing.CellStatus.MarkedForRemoval)
        {
          if (cellData.Guide != null)
            cellData.Guide.InvalidateGenerated(minRange);
          if (this.m_clipmap.ApproximateCellConstitution(enumeratePoint, this.m_lod) != cellData.Constitution || cellData.Status != MyVoxelClipmapRing.CellStatus.Empty)
          {
            cellData.Status = MyVoxelClipmapRing.CellStatus.Pending;
            this.m_clipmap.RequestCell(enumeratePoint, this.m_lod, cellData.Guide);
          }
        }
      }
    }

    internal void InvalidateAll()
    {
      foreach (KeyValuePair<Vector3I, MyVoxelClipmapRing.CellData> cell in this.m_cells)
        this.DisposeCell(cell.Key, cell.Value);
      this.m_cells.Clear();
      this.Bounds = new BoundingBoxI();
      this.InnerBounds = new BoundingBoxI();
    }

    public void DebugDraw()
    {
      Vector3D translation = MyTransparentGeometry.Camera.Translation;
      int num1 = 100;
      int num2 = num1 * num1;
      Vector4 lodColor = MyClipmap.LodColors[this.m_lod];
      using (IMyDebugDrawBatchAabb debugDrawBatchAabb1 = MyRenderProxy.DebugDrawBatchAABB(this.m_clipmap.LocalToWorld, new Color((Color) (lodColor - new Vector4(0.2f)), 0.07f)))
      {
        using (IMyDebugDrawBatchAabb debugDrawBatchAabb2 = MyRenderProxy.DebugDrawBatchAABB(this.m_clipmap.LocalToWorld, new Color((Color) lodColor, 0.4f), shaded: false))
        {
          foreach (KeyValuePair<Vector3I, MyVoxelClipmapRing.CellData> cell in this.m_cells)
          {
            if (cell.Value.Guide != null && cell.Value.Guide.Mesh != null)
            {
              Vector3I vector3I1 = cell.Key << this.m_clipmap.CellBits << this.m_lod;
              Vector3I vector3I2 = vector3I1 + (this.m_clipmap.CellSize << this.m_lod);
              BoundingBoxD aabb = new BoundingBoxD((Vector3D) vector3I1, (Vector3D) vector3I2);
              debugDrawBatchAabb1.Add(ref aabb);
              debugDrawBatchAabb2.Add(ref aabb);
              Vector3D worldCoord = Vector3D.Transform((Vector3D) vector3I1 + (float) (this.m_clipmap.CellSize << this.m_lod >> 1), this.m_clipmap.LocalToWorld);
              double num3 = Vector3D.DistanceSquared(worldCoord, translation);
              if (num3 < (double) num2)
              {
                float a = (float) (1.0 - num3 / (double) num2);
                MyRenderProxy.DebugDrawText3D(worldCoord, string.Format("{0}:{1}", (object) this.m_lod, (object) cell.Key), new Color((Color) lodColor, a), 0.8f * a, true, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM);
              }
            }
          }
        }
      }
    }

    public enum CellStatus : byte
    {
      Pending,
      Calculated,
      Empty,
      Ready,
      MarkedForRemoval,
    }

    public class CellData
    {
      public IMyVoxelActorCell Cell;
      public MyVoxelClipmapRing.CellStatus Status;
      public MyVoxelContentConstitution Constitution;
      public VrSewGuide Guide;
      public bool Visible;
      public MyClipmapCellVicinity Vicinity = MyClipmapCellVicinity.Invalid;

      public CellData(MyVoxelClipmapRing.CellStatus status, MyVoxelContentConstitution constitution)
      {
        this.Status = status;
        this.Constitution = constitution;
        this.Cell = (IMyVoxelActorCell) null;
      }

      public CellData()
      {
        this.Status = MyVoxelClipmapRing.CellStatus.Pending;
        this.Constitution = MyVoxelContentConstitution.Mixed;
        this.Cell = (IMyVoxelActorCell) null;
      }

      public void Dispose(MyVoxelClipmap clipmap)
      {
      }
    }
  }
}
