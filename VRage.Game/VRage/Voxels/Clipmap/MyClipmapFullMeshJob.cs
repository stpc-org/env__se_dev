// Decompiled with JetBrains decompiler
// Type: VRage.Voxels.Clipmap.MyClipmapFullMeshJob
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using VRage.Collections;
using VRage.Game.Voxels;
using VRage.Network;
using VRage.Utils;
using VRage.Voxels.DualContouring;
using VRageMath;
using VRageRender.Voxels;

namespace VRage.Voxels.Clipmap
{
  public class MyClipmapFullMeshJob : MyPrecalcJob
  {
    private static readonly MyConcurrentPool<MyClipmapFullMeshJob> m_instancePool = new MyConcurrentPool<MyClipmapFullMeshJob>(16);
    private MyVoxelRenderCellData m_cellData;
    private volatile bool m_isCancelled;

    public MyVoxelClipmap Clipmap { get; private set; }

    public MyCellCoord Cell { get; private set; }

    public MyWorkTracker<MyCellCoord, MyClipmapFullMeshJob> WorkTracker { get; private set; }

    public MyClipmapFullMeshJob()
      : base(true)
    {
    }

    public static bool Start(
      MyWorkTracker<MyCellCoord, MyClipmapFullMeshJob> tracker,
      MyVoxelClipmap clipmap,
      MyCellCoord cell)
    {
      if (tracker == null)
        throw new ArgumentNullException(nameof (tracker));
      if (clipmap == null)
        throw new ArgumentNullException(nameof (clipmap));
      if (tracker.Exists(cell))
      {
        MyLog.Default.Error("A Stitch job for cell {0} is already scheduled.", (object) cell);
        return false;
      }
      MyClipmapFullMeshJob clipmapFullMeshJob = MyClipmapFullMeshJob.m_instancePool.Get();
      clipmapFullMeshJob.m_isCancelled = false;
      clipmapFullMeshJob.Clipmap = clipmap;
      clipmapFullMeshJob.Cell = cell;
      clipmapFullMeshJob.WorkTracker = tracker;
      return clipmapFullMeshJob.Enqueue();
    }

    private bool Enqueue()
    {
      this.WorkTracker.Add(this.Cell, this);
      if (MyPrecalcComponent.EnqueueBack((MyPrecalcJob) this))
        return true;
      this.WorkTracker.Complete(this.Cell);
      return false;
    }

    public override void DoWork()
    {
      try
      {
        if (this.m_isCancelled)
          return;
        BoundingBoxI cellBounds = this.Clipmap.GetCellBounds(this.Cell);
        MyMesherResult mesh = this.Clipmap.Mesher.CalculateMesh(this.Cell.Lod, cellBounds.Min, cellBounds.Max);
        if (this.m_isCancelled || !mesh.MeshProduced)
          this.m_cellData = new MyVoxelRenderCellData();
        else
          MyRenderDataBuilder.Instance.Build(mesh.Mesh, out this.m_cellData, this.Clipmap.VoxelRenderDataProcessorProvider);
        if (!mesh.MeshProduced)
          return;
        mesh.Mesh.Dispose();
      }
      finally
      {
      }
    }

    protected override void OnComplete()
    {
      base.OnComplete();
      bool flag = false;
      if (!this.m_isCancelled && this.Clipmap.Mesher != null)
      {
        this.Clipmap.UpdateCellRender(this.Cell, (MyVoxelClipmap.StitchOperation) null, ref this.m_cellData);
        if (!this.IsValid)
          flag = true;
      }
      if (!this.m_isCancelled)
        this.WorkTracker.Complete(this.Cell);
      this.m_cellData = new MyVoxelRenderCellData();
      if (flag)
      {
        this.Enqueue();
      }
      else
      {
        this.Clipmap = (MyVoxelClipmap) null;
        this.WorkTracker = (MyWorkTracker<MyCellCoord, MyClipmapFullMeshJob>) null;
        MyClipmapFullMeshJob.m_instancePool.Return(this);
      }
    }

    public override void Cancel() => this.m_isCancelled = true;

    public override bool IsCanceled => this.m_isCancelled;

    public override int Priority => !this.m_isCancelled ? this.Cell.Lod : int.MaxValue;

    public override void DebugDraw(Color c)
    {
    }

    private class VRage_Voxels_Clipmap_MyClipmapFullMeshJob\u003C\u003EActor : IActivator, IActivator<MyClipmapFullMeshJob>
    {
      object IActivator.CreateInstance() => (object) new MyClipmapFullMeshJob();

      MyClipmapFullMeshJob IActivator<MyClipmapFullMeshJob>.CreateInstance() => new MyClipmapFullMeshJob();
    }
  }
}
