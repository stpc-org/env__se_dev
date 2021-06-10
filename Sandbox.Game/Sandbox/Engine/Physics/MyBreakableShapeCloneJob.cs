// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Physics.MyBreakableShapeCloneJob
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Voxels;
using VRage.Generics;
using VRage.Network;
using VRage.Voxels;

namespace Sandbox.Engine.Physics
{
  public class MyBreakableShapeCloneJob : MyPrecalcJob
  {
    private static readonly MyDynamicObjectPool<MyBreakableShapeCloneJob> m_instancePool = new MyDynamicObjectPool<MyBreakableShapeCloneJob>(16);
    private MyBreakableShapeCloneJob.Args m_args;
    private List<HkdBreakableShape> m_clonedShapes = new List<HkdBreakableShape>();
    private bool m_isCanceled;

    public static void Start(MyBreakableShapeCloneJob.Args args)
    {
      MyBreakableShapeCloneJob work = MyBreakableShapeCloneJob.m_instancePool.Allocate();
      work.m_args = args;
      args.Tracker.Add(args.DefId, work);
      MyPrecalcComponent.EnqueueBack((MyPrecalcJob) work);
    }

    public MyBreakableShapeCloneJob()
      : base(true)
    {
    }

    public override void DoWork()
    {
      for (int index = 0; index < this.m_args.Count && (!this.m_isCanceled || index <= 0); ++index)
        this.m_clonedShapes.Add(this.m_args.ShapeToClone.Clone());
    }

    public override void Cancel() => this.m_isCanceled = true;

    protected override void OnComplete()
    {
      base.OnComplete();
      if (MyDestructionData.Static != null && MyDestructionData.Static.BlockShapePool != null)
        MyDestructionData.Static.BlockShapePool.EnqueShapes(this.m_args.Model, this.m_args.DefId, this.m_clonedShapes);
      this.m_clonedShapes.Clear();
      this.m_args.Tracker.Complete(this.m_args.DefId);
      this.m_args = new MyBreakableShapeCloneJob.Args();
      this.m_isCanceled = false;
      MyBreakableShapeCloneJob.m_instancePool.Deallocate(this);
    }

    public struct Args
    {
      public MyWorkTracker<MyDefinitionId, MyBreakableShapeCloneJob> Tracker;
      public string Model;
      public MyDefinitionId DefId;
      public HkdBreakableShape ShapeToClone;
      public int Count;
    }

    private class Sandbox_Engine_Physics_MyBreakableShapeCloneJob\u003C\u003EActor : IActivator, IActivator<MyBreakableShapeCloneJob>
    {
      object IActivator.CreateInstance() => (object) new MyBreakableShapeCloneJob();

      MyBreakableShapeCloneJob IActivator<MyBreakableShapeCloneJob>.CreateInstance() => new MyBreakableShapeCloneJob();
    }
  }
}
