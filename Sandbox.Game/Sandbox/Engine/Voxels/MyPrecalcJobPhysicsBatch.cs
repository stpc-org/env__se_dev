// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MyPrecalcJobPhysicsBatch
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Game.World;
using System.Collections.Generic;
using VRage.Game.Voxels;
using VRage.Generics;
using VRage.Network;
using VRage.Utils;
using VRage.Voxels;
using VRage.Voxels.Mesh;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
  internal sealed class MyPrecalcJobPhysicsBatch : MyPrecalcJob
  {
    private static readonly MyDynamicObjectPool<MyPrecalcJobPhysicsBatch> m_instancePool = new MyDynamicObjectPool<MyPrecalcJobPhysicsBatch>(8);
    private MyVoxelPhysicsBody m_targetPhysics;
    internal HashSet<Vector3I> CellBatch = new HashSet<Vector3I>((IEqualityComparer<Vector3I>) Vector3I.Comparer);
    private Dictionary<Vector3I, HkBvCompressedMeshShape> m_newShapes = new Dictionary<Vector3I, HkBvCompressedMeshShape>((IEqualityComparer<Vector3I>) Vector3I.Comparer);
    private volatile bool m_isCancelled;
    public int Lod;

    public MyPrecalcJobPhysicsBatch()
      : base(true)
    {
    }

    public static void Start(
      MyVoxelPhysicsBody targetPhysics,
      ref HashSet<Vector3I> cellBatchForSwap,
      int lod)
    {
      MyPrecalcJobPhysicsBatch precalcJobPhysicsBatch = MyPrecalcJobPhysicsBatch.m_instancePool.Allocate();
      precalcJobPhysicsBatch.Lod = lod;
      precalcJobPhysicsBatch.m_targetPhysics = targetPhysics;
      MyUtils.Swap<HashSet<Vector3I>>(ref precalcJobPhysicsBatch.CellBatch, ref cellBatchForSwap);
      targetPhysics.RunningBatchTask[lod] = precalcJobPhysicsBatch;
      MyPrecalcComponent.EnqueueBack((MyPrecalcJob) precalcJobPhysicsBatch);
    }

    public override void DoWork()
    {
      try
      {
        foreach (Vector3I vector3I in this.CellBatch)
        {
          if (this.m_isCancelled)
            break;
          VrVoxelMesh mesh = this.m_targetPhysics.CreateMesh(new MyCellCoord(this.Lod, vector3I));
          try
          {
            if (this.m_isCancelled)
              break;
            if (mesh.IsEmpty())
            {
              this.m_newShapes.Add(vector3I, (HkBvCompressedMeshShape) HkShape.Empty);
            }
            else
            {
              HkBvCompressedMeshShape shape = this.m_targetPhysics.CreateShape(mesh, this.Lod);
              this.m_newShapes.Add(vector3I, shape);
            }
          }
          finally
          {
            mesh?.Dispose();
          }
        }
      }
      finally
      {
      }
    }

    protected override void OnComplete()
    {
      base.OnComplete();
      if (MySession.Static != null && MySession.Static.GetComponent<MyPrecalcComponent>().Loaded && !this.m_isCancelled)
        this.m_targetPhysics.OnBatchTaskComplete(this.m_newShapes, this.Lod);
      foreach (HkBvCompressedMeshShape compressedMeshShape in this.m_newShapes.Values)
      {
        HkShape hkShape = compressedMeshShape.Base;
        if (!hkShape.IsZero)
        {
          hkShape = compressedMeshShape.Base;
          hkShape.RemoveReference();
        }
      }
      if (this.m_targetPhysics.RunningBatchTask[this.Lod] == this)
        this.m_targetPhysics.RunningBatchTask[this.Lod] = (MyPrecalcJobPhysicsBatch) null;
      this.m_targetPhysics = (MyVoxelPhysicsBody) null;
      this.CellBatch.Clear();
      this.m_newShapes.Clear();
      this.m_isCancelled = false;
      MyPrecalcJobPhysicsBatch.m_instancePool.Deallocate(this);
    }

    public override void Cancel() => this.m_isCancelled = true;

    private class Sandbox_Engine_Voxels_MyPrecalcJobPhysicsBatch\u003C\u003EActor : IActivator, IActivator<MyPrecalcJobPhysicsBatch>
    {
      object IActivator.CreateInstance() => (object) new MyPrecalcJobPhysicsBatch();

      MyPrecalcJobPhysicsBatch IActivator<MyPrecalcJobPhysicsBatch>.CreateInstance() => new MyPrecalcJobPhysicsBatch();
    }
  }
}
