// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MyPrecalcJobPhysicsPrefetch
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using VRage.Collections;
using VRage.Game.Voxels;
using VRage.Network;
using VRage.Voxels;
using VRage.Voxels.Mesh;

namespace Sandbox.Engine.Voxels
{
  internal sealed class MyPrecalcJobPhysicsPrefetch : MyPrecalcJob
  {
    private static readonly MyConcurrentPool<MyPrecalcJobPhysicsPrefetch> m_instancePool = new MyConcurrentPool<MyPrecalcJobPhysicsPrefetch>(16);
    private MyPrecalcJobPhysicsPrefetch.Args m_args;
    private volatile bool m_isCancelled;
    public int Taken;
    public volatile bool ResultComplete;
    public HkBvCompressedMeshShape Result;

    public override bool IsCanceled => this.m_isCancelled;

    public MyPrecalcJobPhysicsPrefetch()
      : base(true)
    {
    }

    public static bool Start(MyPrecalcJobPhysicsPrefetch.Args args)
    {
      MyPrecalcJobPhysicsPrefetch jobPhysicsPrefetch = MyPrecalcJobPhysicsPrefetch.m_instancePool.Get();
      if (args.Tracker.TryAdd(args.GeometryCell, jobPhysicsPrefetch))
      {
        jobPhysicsPrefetch.m_args = args;
        MyPrecalcComponent.EnqueueBack((MyPrecalcJob) jobPhysicsPrefetch);
        return true;
      }
      MyPrecalcJobPhysicsPrefetch.m_instancePool.Return(jobPhysicsPrefetch);
      return false;
    }

    public override void DoWork()
    {
      try
      {
        if (this.m_isCancelled)
          return;
        VrVoxelMesh mesh = this.m_args.TargetPhysics.CreateMesh(this.m_args.GeometryCell);
        try
        {
          if (!mesh.IsEmpty())
          {
            if (this.m_isCancelled)
              return;
            this.Result = this.m_args.TargetPhysics.CreateShape(mesh, this.m_args.GeometryCell.Lod);
          }
        }
        finally
        {
          mesh?.Dispose();
        }
        this.ResultComplete = true;
      }
      finally
      {
      }
    }

    protected override void OnComplete()
    {
      base.OnComplete();
      if (!this.m_isCancelled && this.m_args.TargetPhysics.Entity != null)
        this.m_args.TargetPhysics.OnTaskComplete(this.m_args.GeometryCell, this.Result);
      if (!this.m_isCancelled)
        this.m_args.Tracker.Complete(this.m_args.GeometryCell);
      if (!this.Result.Base.IsZero && this.Taken == 0)
        this.Result.Base.RemoveReference();
      this.Taken = 0;
      this.m_args = new MyPrecalcJobPhysicsPrefetch.Args();
      this.m_isCancelled = false;
      this.ResultComplete = false;
      this.Result = (HkBvCompressedMeshShape) HkShape.Empty;
      MyPrecalcJobPhysicsPrefetch.m_instancePool.Return(this);
    }

    public override void Cancel() => this.m_isCancelled = true;

    public struct Args
    {
      public MyWorkTracker<MyCellCoord, MyPrecalcJobPhysicsPrefetch> Tracker;
      public IMyStorage Storage;
      public MyCellCoord GeometryCell;
      public MyVoxelPhysicsBody TargetPhysics;
    }

    private class Sandbox_Engine_Voxels_MyPrecalcJobPhysicsPrefetch\u003C\u003EActor : IActivator, IActivator<MyPrecalcJobPhysicsPrefetch>
    {
      object IActivator.CreateInstance() => (object) new MyPrecalcJobPhysicsPrefetch();

      MyPrecalcJobPhysicsPrefetch IActivator<MyPrecalcJobPhysicsPrefetch>.CreateInstance() => new MyPrecalcJobPhysicsPrefetch();
    }
  }
}
