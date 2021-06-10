// Decompiled with JetBrains decompiler
// Type: VRage.Voxels.Clipmap.MyClipmapMeshJob
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using VRage.Collections;
using VRage.Game.Voxels;
using VRage.Network;
using VRage.Utils;
using VRage.Voxels.DualContouring;
using VRage.Voxels.Sewing;
using VRageMath;

namespace VRage.Voxels.Clipmap
{
  public sealed class MyClipmapMeshJob : MyPrecalcJob
  {
    private static readonly MyConcurrentPool<MyClipmapMeshJob> m_instancePool = new MyConcurrentPool<MyClipmapMeshJob>(16);
    private VrSewGuide m_meshAndGuide;
    private MyVoxelContentConstitution m_resultConstitution;
    private volatile bool m_isCancelled;

    public MyVoxelClipmap Clipmap { get; private set; }

    public MyCellCoord Cell { get; private set; }

    public MyWorkTracker<MyCellCoord, MyClipmapMeshJob> WorkTracker { get; private set; }

    public MyClipmapMeshJob()
      : base(true)
    {
    }

    public static bool Start(
      MyWorkTracker<MyCellCoord, MyClipmapMeshJob> tracker,
      MyVoxelClipmap clipmap,
      MyCellCoord cell,
      VrSewGuide existingGuide = null)
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
      MyClipmapMeshJob myClipmapMeshJob = MyClipmapMeshJob.m_instancePool.Get();
      myClipmapMeshJob.m_isCancelled = false;
      myClipmapMeshJob.Clipmap = clipmap;
      myClipmapMeshJob.Cell = cell;
      myClipmapMeshJob.WorkTracker = tracker;
      myClipmapMeshJob.m_meshAndGuide = existingGuide;
      return myClipmapMeshJob.Enqueue();
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
        BoundingBoxI cellBounds = this.Clipmap.GetCellBounds(this.Cell);
        VrSewGuide guide = this.m_meshAndGuide;
        MyMesherResult mesh = this.Clipmap.Mesher.CalculateMesh(this.Cell.Lod, cellBounds.Min, cellBounds.Max, target: guide?.Mesh);
        this.m_resultConstitution = mesh.Constitution;
        if (mesh.Constitution == MyVoxelContentConstitution.Mixed)
        {
          MyStorageData storageData = MyDualContouringMesher.Static.StorageData;
          VrShellDataCache dataCache = VrShellDataCache.FromDataCube(storageData.Size3D, storageData[MyStorageDataTypeEnum.Content], storageData[MyStorageDataTypeEnum.Material]);
          if (guide != null)
            guide.SetMesh(mesh.Mesh, dataCache);
          else
            guide = new VrSewGuide(mesh.Mesh, dataCache);
        }
        else if (guide != null)
        {
          MyStorageData storageData = MyDualContouringMesher.Static.StorageData;
          VrShellDataCache dataCache = VrShellDataCache.FromDataCube(storageData.Size3D, storageData[MyStorageDataTypeEnum.Content], storageData[MyStorageDataTypeEnum.Material]);
          guide.SetMesh(guide.Mesh, dataCache);
        }
        this.Clipmap.UpdateCellData(this, this.Cell, guide, this.m_resultConstitution);
      }
      finally
      {
        this.m_meshAndGuide = (VrSewGuide) null;
      }
    }

    protected override void OnComplete()
    {
      base.OnComplete();
      if (this.IsReusingGuide)
      {
        this.m_meshAndGuide.RemoveReference();
        this.m_meshAndGuide = (VrSewGuide) null;
      }
      this.Clipmap = (MyVoxelClipmap) null;
      this.m_meshAndGuide = (VrSewGuide) null;
      this.WorkTracker = (MyWorkTracker<MyCellCoord, MyClipmapMeshJob>) null;
      MyClipmapMeshJob.m_instancePool.Return(this);
    }

    public override void Cancel() => this.m_isCancelled = true;

    public override bool IsCanceled => this.m_isCancelled;

    public override int Priority => !this.m_isCancelled ? this.Cell.Lod : int.MaxValue;

    public bool IsReusingGuide => this.m_meshAndGuide != null;

    public override void DebugDraw(Color c)
    {
    }

    private class VRage_Voxels_Clipmap_MyClipmapMeshJob\u003C\u003EActor : IActivator, IActivator<MyClipmapMeshJob>
    {
      object IActivator.CreateInstance() => (object) new MyClipmapMeshJob();

      MyClipmapMeshJob IActivator<MyClipmapMeshJob>.CreateInstance() => new MyClipmapMeshJob();
    }
  }
}
