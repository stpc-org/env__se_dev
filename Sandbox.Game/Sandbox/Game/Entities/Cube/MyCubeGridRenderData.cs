// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyCubeGridRenderData
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Components;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Library.Collections;
using VRage.Library.Threading;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Cube
{
  public class MyCubeGridRenderData
  {
    public const int SplitCellCubeCount = 30;
    private const int MAX_DECALS_PER_CUBE = 30;
    private ConcurrentDictionary<Vector3I, ConcurrentQueue<MyCubeGridRenderData.MyDecalPartIdentity>> m_cubeDecals = new ConcurrentDictionary<Vector3I, ConcurrentQueue<MyCubeGridRenderData.MyDecalPartIdentity>>();
    private Vector3 m_basePos;
    private readonly ConcurrentDictionary<Vector3I, MyCubeGridRenderCell> m_cells = new ConcurrentDictionary<Vector3I, MyCubeGridRenderCell>();
    private readonly object m_cellLock = new object();
    private readonly object m_cellsUpdateLock = new object();
    private MyConcurrentHashSet<MyCubeGridRenderCell> m_dirtyCells = new MyConcurrentHashSet<MyCubeGridRenderCell>();
    private MyRenderComponentCubeGrid m_gridRender;
    [ThreadStatic]
    private static List<MyCubeGridRenderCell> m_dirtyCellsBuffer;

    public bool HasDirtyCells => this.m_dirtyCells.Count > 0;

    public ConcurrentDictionary<Vector3I, MyCubeGridRenderCell> Cells => this.m_cells;

    public MyCubeGridRenderData(MyRenderComponentCubeGrid grid) => this.m_gridRender = grid;

    public void AddCubePart(MyCubePart part)
    {
      MyCubeGridRenderCell orAddCell = this.GetOrAddCell(part.InstanceData.Translation);
      orAddCell.AddCubePart(part);
      this.m_dirtyCells.Add(orAddCell);
    }

    public void RemoveCubePart(MyCubePart part)
    {
      Vector3 translation = part.InstanceData.Translation;
      MyCubeGridRenderCell cell = this.GetCell(ref translation, false);
      if (cell == null || !cell.RemoveCubePart(part))
        return;
      this.m_dirtyCells.Add(cell);
    }

    private long CalculateEdgeHash(Vector3 point0, Vector3 point1) => point0.GetHash() * point1.GetHash();

    public void AddEdgeInfo(
      ref Vector3 point0,
      ref Vector3 point1,
      ref Vector3 normal0,
      ref Vector3 normal1,
      Color color,
      MySlimBlock owner)
    {
      long edgeHash = this.CalculateEdgeHash(point0, point1);
      Vector3 pos = (point0 + point1) * 0.5f;
      Vector3I edgeDirection = Vector3I.Round((point0 - point1) / this.m_gridRender.GridSize);
      MyEdgeInfo info = new MyEdgeInfo(ref pos, ref edgeDirection, ref normal0, ref normal1, ref color, MyStringHash.GetOrCompute(owner.BlockDefinition.EdgeType));
      MyCubeGridRenderCell orAddCell = this.GetOrAddCell(pos);
      if (!orAddCell.AddEdgeInfo(edgeHash, info, owner))
        return;
      this.m_dirtyCells.Add(orAddCell);
    }

    public void RemoveEdgeInfo(Vector3 point0, Vector3 point1, MySlimBlock owner)
    {
      long edgeHash = this.CalculateEdgeHash(point0, point1);
      MyCubeGridRenderCell orAddCell = this.GetOrAddCell((point0 + point1) * 0.5f);
      if (!orAddCell.RemoveEdgeInfo(edgeHash, owner))
        return;
      this.m_dirtyCells.Add(orAddCell);
    }

    public void RebuildDirtyCells(RenderFlags renderFlags)
    {
      using (MyUtils.ReuseCollection<MyCubeGridRenderCell>(ref MyCubeGridRenderData.m_dirtyCellsBuffer))
      {
        using (ConcurrentEnumerator<SpinLockRef.Token, MyCubeGridRenderCell, HashSet<MyCubeGridRenderCell>.Enumerator> enumerator = this.m_dirtyCells.GetEnumerator())
        {
          while (enumerator.MoveNext())
            MyCubeGridRenderData.m_dirtyCellsBuffer.Add(enumerator.Current);
          this.m_dirtyCells.Clear();
        }
        lock (this.m_cellsUpdateLock)
        {
          foreach (MyCubeGridRenderCell cubeGridRenderCell in MyCubeGridRenderData.m_dirtyCellsBuffer)
            cubeGridRenderCell.RebuildInstanceParts(renderFlags);
        }
      }
    }

    public void OnRemovedFromRender()
    {
      IMyEntity entity = this.m_gridRender.Entity;
      bool flag = entity != null && !entity.MarkedForClose;
      lock (this.m_cellsUpdateLock)
      {
        foreach (KeyValuePair<Vector3I, MyCubeGridRenderCell> cell in this.m_cells)
        {
          MyCubeGridRenderCell instance = cell.Value;
          instance.OnRemovedFromRender();
          if (flag)
            this.m_dirtyCells.Add(instance);
        }
      }
    }

    public void SetBasePositionHint(Vector3 basePos)
    {
      if (this.m_cells.Count != 0)
        return;
      this.m_basePos = basePos;
    }

    internal MyCubeGridRenderCell GetOrAddCell(Vector3 pos) => this.GetCell(ref pos);

    internal MyCubeGridRenderCell GetCell(ref Vector3 pos, bool create = true)
    {
      Vector3 v = (pos - this.m_basePos) / (30f * this.m_gridRender.GridSize);
      Vector3I r;
      Vector3I.Round(ref v, out r);
      MyCubeGridRenderCell cubeGridRenderCell;
      if (this.m_cells.TryGetValue(r, out cubeGridRenderCell) || !create)
        return cubeGridRenderCell;
      lock (this.m_cellLock)
      {
        cubeGridRenderCell = new MyCubeGridRenderCell(this.m_gridRender)
        {
          DebugName = r.ToString()
        };
        this.m_cells.TryAdd(r, cubeGridRenderCell);
      }
      return cubeGridRenderCell;
    }

    public void AddDecal(Vector3I position, MyCubeGrid.MyCubeGridHitInfo gridHitInfo, uint decalId)
    {
      MyCube cube;
      if (!this.m_gridRender.CubeGrid.TryGetCube(position, out cube))
        return;
      if (gridHitInfo.CubePartIndex != -1)
      {
        MyCubePart part = cube.Parts[gridHitInfo.CubePartIndex];
        this.GetOrAddCell(part.InstanceData.Translation).AddCubePartDecal(part, decalId);
      }
      ConcurrentQueue<MyCubeGridRenderData.MyDecalPartIdentity> orAdd = this.m_cubeDecals.GetOrAdd(position, (Func<Vector3I, ConcurrentQueue<MyCubeGridRenderData.MyDecalPartIdentity>>) (x => new ConcurrentQueue<MyCubeGridRenderData.MyDecalPartIdentity>()));
      if (orAdd.Count > 30)
        this.RemoveDecal(position, orAdd, cube);
      orAdd.Enqueue(new MyCubeGridRenderData.MyDecalPartIdentity()
      {
        DecalId = decalId,
        CubePartIndex = gridHitInfo.CubePartIndex
      });
    }

    public void RemoveDecals(Vector3I position)
    {
      ConcurrentQueue<MyCubeGridRenderData.MyDecalPartIdentity> decals;
      if (!this.m_cubeDecals.TryGetValue(position, out decals))
        return;
      MyCube cube;
      this.m_gridRender.CubeGrid.TryGetCube(position, out cube);
      while (!decals.IsEmpty)
        this.RemoveDecal(position, decals, cube);
    }

    private void RemoveDecal(
      Vector3I position,
      ConcurrentQueue<MyCubeGridRenderData.MyDecalPartIdentity> decals,
      MyCube cube)
    {
      MyCubeGridRenderData.MyDecalPartIdentity result;
      decals.TryDequeue(out result);
      MyDecals.RemoveDecal(result.DecalId);
      if (result.CubePartIndex == -1)
        return;
      MyCubePart part = cube.Parts[result.CubePartIndex];
      this.GetOrAddCell((Vector3) position).RemoveCubePartDecal(part, result.DecalId);
    }

    internal void DebugDraw()
    {
      foreach (KeyValuePair<Vector3I, MyCubeGridRenderCell> cell in this.m_cells)
        cell.Value.DebugDraw();
    }

    private struct MyDecalPartIdentity
    {
      public uint DecalId;
      public int CubePartIndex;
    }
  }
}
