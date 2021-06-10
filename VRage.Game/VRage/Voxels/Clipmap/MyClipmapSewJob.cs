// Decompiled with JetBrains decompiler
// Type: VRage.Voxels.Clipmap.MyClipmapSewJob
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Diagnostics;
using VRage.Collections;
using VRage.Game.Voxels;
using VRage.Network;
using VRage.Utils;
using VRage.Voxels.Mesh;
using VRage.Voxels.Sewing;
using VRageMath;
using VRageRender;
using VRageRender.Voxels;

namespace VRage.Voxels.Clipmap
{
  public sealed class MyClipmapSewJob : MyPrecalcJob
  {
    public static bool DebugDrawDependencies = false;
    public static bool DebugDrawGeneration = false;
    public static VrTailor.GeneratedVertexProtocol GeneratedVertexProtocol = VrTailor.GeneratedVertexProtocol.Dynamic;
    private static readonly MyConcurrentPool<MyClipmapSewJob> m_instancePool = new MyConcurrentPool<MyClipmapSewJob>(16);
    private MyVoxelRenderCellData m_cellData;
    private bool m_forceStitchCommit;
    private volatile bool m_isCancelled;

    public MyVoxelClipmap Clipmap { get; private set; }

    public MyCellCoord Cell { get; private set; }

    internal MyVoxelClipmap.StitchOperation Operation { get; private set; }

    public MyWorkTracker<MyCellCoord, MyClipmapSewJob> WorkTracker { get; private set; }

    public MyClipmapSewJob()
      : base(true)
    {
    }

    internal static bool Start(
      MyWorkTracker<MyCellCoord, MyClipmapSewJob> tracker,
      MyVoxelClipmap clipmap,
      MyVoxelClipmap.StitchOperation operation)
    {
      if (tracker == null)
        throw new ArgumentNullException(nameof (tracker));
      if (clipmap == null)
        throw new ArgumentNullException(nameof (clipmap));
      if (operation == null)
        throw new ArgumentNullException(nameof (operation));
      if (tracker.Exists(operation.Cell))
      {
        MyLog.Default.Error("A Stitch job for cell {0} is already scheduled.", (object) operation.Cell);
        return false;
      }
      MyClipmapSewJob myClipmapSewJob = MyClipmapSewJob.m_instancePool.Get();
      myClipmapSewJob.m_isCancelled = false;
      myClipmapSewJob.Clipmap = clipmap;
      myClipmapSewJob.WorkTracker = tracker;
      myClipmapSewJob.Operation = operation;
      myClipmapSewJob.Cell = operation.Cell;
      return myClipmapSewJob.Enqueue();
    }

    private bool Enqueue()
    {
      this.m_forceStitchCommit = true;
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
        if (this.m_isCancelled || !this.IsValid)
        {
          this.Clipmap.CommitStitchOperation(this.Operation);
          this.WorkTracker.Complete(this.Cell);
        }
        else
        {
          MyIsoMeshTaylor.NativeInstance.SetDebug(MyClipmapSewJob.DebugDrawGeneration || MyClipmapSewJob.GeneratedVertexProtocol != VrTailor.GeneratedVertexProtocol.Dynamic);
          MyIsoMeshTaylor.NativeInstance.SetGenerate(MyClipmapSewJob.GeneratedVertexProtocol);
          MyVoxelClipmap.StitchOperation operation = this.Operation;
          if (this.Clipmap.InstanceStitchMode == MyVoxelClipmap.StitchMode.Stitch && this.Operation.Operation != VrSewOperation.None)
          {
            if (this.Operation.Guides[0] == null)
              return;
            if (this.Operation.Guides[0].Sewn)
              this.Operation.Guides[0].Reset();
            MyIsoMeshTaylor.NativeInstance.Sew(this.Operation.Guides, this.Operation.Operation);
            this.Operation.SetState(MyVoxelClipmap.StitchOperation.OpState.Ready);
            if (MyClipmapSewJob.DebugDrawDependencies)
              this.DebugDrawStitch(this.Operation.Guides);
            MyVoxelClipmap.CompoundStitchOperation compound = this.Operation.GetCompound();
            if (compound != null && compound.Children.Count > 0)
              this.SewChildren(compound);
          }
          if (this.Operation.Guides[0].Mesh != null)
            MyRenderDataBuilder.Instance.Build(this.Operation.Guides[0].Mesh, out this.m_cellData, this.Clipmap.VoxelRenderDataProcessorProvider);
          this.Clipmap.UpdateCellRender(this.Cell, this.Operation, ref this.m_cellData);
          this.WorkTracker.Complete(this.Cell);
          this.m_forceStitchCommit = false;
        }
      }
      catch
      {
        throw;
      }
      finally
      {
      }
    }

    [Conditional("DEBUG")]
    private static void CheckGuides(MyVoxelClipmap.StitchOperation operation)
    {
      for (int index = 0; index < operation.Guides.Length; ++index)
      {
        VrSewGuide guide = operation.Guides[index];
        if ((guide != null ? (guide.IsDisposed ? 1 : 0) : 0) != 0)
          throw new ObjectDisposedException("VrSewGuide");
      }
    }

    private void SewChildren(MyVoxelClipmap.CompoundStitchOperation compound)
    {
      foreach (MyVoxelClipmap.StitchOperation child in compound.Children)
      {
        for (int index = 0; index < 8; ++index)
        {
          MyCellCoord myCellCoord = MyVoxelClipmap.MakeFulfilled(child.Dependencies[index]);
          if (child.Guides[index] == null)
          {
            int lod1 = myCellCoord.Lod;
            int lod2 = compound.Cell.Lod;
          }
        }
        MyIsoMeshTaylor.NativeInstance.Sew(child.Guides, child.Operation, child.Range.Value.Min, child.Range.Value.Max);
        child.SetState(MyVoxelClipmap.StitchOperation.OpState.Ready);
        if (MyClipmapSewJob.DebugDrawDependencies)
          this.DebugDrawStitch(child.Guides);
      }
    }

    private void DebugDrawStitch(VrSewGuide[] meshes)
    {
      Vector3D center = this.GetCenter(meshes[0]);
      for (int index = 0; index < 8; ++index)
      {
        if (meshes[index] != null)
          MyRenderProxy.DebugDrawArrow3D(center, this.GetCenter(meshes[index]), Color.Red, new Color?(Color.Green), true, persistent: true);
      }
    }

    [Conditional("DEBUG")]
    private unsafe void DebugDrawGenerated(VrSewGuide[] meshes)
    {
      if (!MyClipmapSewJob.DebugDrawGeneration)
        return;
      VrTailor.VertexRef* studiedVertices;
      int count1;
      MyIsoMeshTaylor.NativeInstance.DebugReadStudied(out studiedVertices, out count1);
      ushort* generatedVertices;
      int count2;
      MyIsoMeshTaylor.NativeInstance.DebugReadGenerated(out generatedVertices, out count2);
      VrTailor.RemappedVertex* remappedVertices;
      int count3;
      MyIsoMeshTaylor.NativeInstance.DebugReadRemapped(out remappedVertices, out count3);
      for (int index = 0; index < count1; ++index)
      {
        VrSewGuide mesh = meshes[(int) studiedVertices[index].Mesh];
        VrVoxelVertex vrVoxelVertex = mesh.Mesh.Vertices[studiedVertices[index].Index];
        MyRenderProxy.DebugDrawText3D(this.Position(mesh, (int) studiedVertices[index].Index), vrVoxelVertex.Cell.ToString(), Color.Gray, 0.7f, true, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, persistent: true);
      }
      VrSewGuide mesh1 = meshes[0];
      for (int index = 0; index < count2; ++index)
      {
        Vector3D vector3D = this.Position(mesh1, (int) generatedVertices[index]);
        MyRenderProxy.DebugDrawText3D(vector3D, mesh1.Mesh.Vertices[generatedVertices[index]].Cell.ToString(), Color.Red, 0.7f, true, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM, persistent: true);
        MyRenderProxy.DebugDrawText3D(vector3D, index.ToString(), Color.Red, 0.7f, true, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP, persistent: true);
        MyRenderProxy.DebugDrawSphere(vector3D, 0.25f, Color.Red, persistent: true);
      }
      for (int index1 = 0; index1 < count3; ++index1)
      {
        Vector3I cell = remappedVertices[index1].Cell;
        Vector3D vector3D1 = this.Position(mesh1, cell) + 0.5 * (double) mesh1.Scale;
        Vector3D vector3D2 = this.Position(mesh1, (int) remappedVertices[index1].Index);
        byte generationCorner = remappedVertices[index1].GenerationCorner;
        for (int index2 = 0; index2 < 3; ++index2)
        {
          int num = (int) generationCorner >> index2 & 1;
          Vector3I pos = cell;
          pos[index2] += num == 1 ? -1 : 1;
          MyRenderProxy.DebugDrawText3D(this.Position(mesh1, pos) + 0.5 * (double) mesh1.Scale, pos.ToString(), Color.Orange, 0.7f, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, persistent: true);
        }
        MyRenderProxy.DebugDrawSphere(vector3D1, 0.25f, Color.Blue, persistent: true);
        MyRenderProxy.DebugDrawSphere(vector3D2, 0.25f, Color.Green, persistent: true);
        MyRenderProxy.DebugDrawArrow3D(vector3D1, vector3D2, Color.Blue, new Color?(Color.Green), true, persistent: true);
        MyRenderProxy.DebugDrawText3D(vector3D1, cell.ToString(), Color.Blue, 0.7f, true, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM, persistent: true);
        MyRenderProxy.DebugDrawText3D(vector3D1, index1.ToString(), Color.Blue, 0.7f, true, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP, persistent: true);
        MyRenderProxy.DebugDrawText3D((vector3D1 + vector3D2) / 2.0, string.Format("G{0} : T{1}", (object) index1, (object) remappedVertices[index1].ProducedTriangleCount), Color.Lerp(Color.Blue, Color.Green, 0.5f), 0.7f, true, persistent: true);
      }
      MyIsoMeshTaylor.NativeInstance.ClearBuffers();
    }

    private Vector3D GetCenter(VrSewGuide mesh) => Vector3D.Transform((Vector3D) (mesh.End + mesh.Start) / 2.0 * (double) mesh.Scale, this.Clipmap.LocalToWorld);

    private Vector3D Position(VrSewGuide mesh, Vector3I pos)
    {
      pos += mesh.Start;
      pos <<= mesh.Lod;
      return Vector3D.Transform((Vector3D) pos, this.Clipmap.LocalToWorld);
    }

    private unsafe Vector3D Position(VrSewGuide mesh, int index) => Vector3D.Transform(((Vector3D) mesh.Mesh.Vertices[index].Position + mesh.Start) * (double) mesh.Scale, this.Clipmap.LocalToWorld);

    protected override void OnComplete()
    {
      base.OnComplete();
      if (this.m_forceStitchCommit)
        this.Clipmap.CommitStitchOperation(this.Operation);
      this.m_cellData = new MyVoxelRenderCellData();
      MyClipmapSewJob.m_instancePool.Return(this);
    }

    public override void Cancel() => this.m_isCancelled = true;

    public override bool IsCanceled => this.m_isCancelled;

    public override int Priority => !this.m_isCancelled ? this.Cell.Lod : int.MaxValue;

    public override void DebugDraw(Color c)
    {
    }

    private class VRage_Voxels_Clipmap_MyClipmapSewJob\u003C\u003EActor : IActivator, IActivator<MyClipmapSewJob>
    {
      object IActivator.CreateInstance() => (object) new MyClipmapSewJob();

      MyClipmapSewJob IActivator<MyClipmapSewJob>.CreateInstance() => new MyClipmapSewJob();
    }
  }
}
