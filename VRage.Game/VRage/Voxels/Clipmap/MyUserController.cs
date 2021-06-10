// Decompiled with JetBrains decompiler
// Type: VRage.Voxels.Clipmap.MyUserController
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using VRage.Collections;
using VRageMath;
using VRageRender;
using VRageRender.Voxels;

namespace VRage.Voxels.Clipmap
{
  public class MyUserController : IMyLodController
  {
    private uint m_nextMeshId = 1;
    private readonly Dictionary<uint, IMyVoxelActorCell> m_cells = new Dictionary<uint, IMyVoxelActorCell>();
    private readonly MyConcurrentQueue<MyUserController.IMessage> m_messageQueue = new MyConcurrentQueue<MyUserController.IMessage>();

    public IMyVoxelRenderDataProcessorProvider VoxelRenderDataProcessorProvider { get; set; }

    public IEnumerable<IMyVoxelActorCell> Cells => (IEnumerable<IMyVoxelActorCell>) this.m_cells.Values;

    public IMyVoxelActor Actor { get; private set; }

    public Vector3I Size { get; private set; }

    public event Action<IMyLodController> Loaded;

    public void Update(
      ref MatrixD view,
      BoundingFrustumD viewFrustum,
      float farClipping,
      bool smoothMotion)
    {
      MyUserController.IMessage instance;
      while (this.m_messageQueue.TryDequeue(out instance))
        instance.Do(this);
      if (this.Loaded == null)
        return;
      MyRenderProxy.EnqueueMainThreadCallback((Action) (() =>
      {
        if (this.Loaded == null)
          return;
        this.Loaded((IMyLodController) this);
      }));
    }

    public void BindToActor(IMyVoxelActor actor) => this.Actor = this.Actor == null ? actor : throw new InvalidOperationException();

    public void Unload()
    {
      foreach (IMyVoxelActorCell cell in this.m_cells.Values)
        this.Actor.DeleteCell(cell);
      this.m_cells.Clear();
    }

    public void InvalidateRange(Vector3I min, Vector3I max)
    {
    }

    public void InvalidateAll()
    {
    }

    public void DebugDraw(ref MatrixD cameraMatrix)
    {
    }

    public uint CreateCell(Vector3D offset, int lod)
    {
      uint id = this.m_nextMeshId++;
      this.m_messageQueue.Enqueue((MyUserController.IMessage) new MyUserController.MCreateCell(id, offset, lod));
      return id;
    }

    public void DeleteCell(uint id) => this.m_messageQueue.Enqueue((MyUserController.IMessage) new MyUserController.MDeleteCell(id));

    public float? SpherizeRadius => new float?();

    public Vector3D SpherizePosition => Vector3D.Zero;

    private interface IMessage
    {
      void Do(MyUserController controller);
    }

    private class MCreateCell : MyUserController.IMessage
    {
      private readonly Vector3D m_offset;
      private readonly int m_lod;
      private readonly uint m_id;

      public MCreateCell(uint id, Vector3D offset, int lod)
      {
        this.m_offset = offset;
        this.m_lod = lod;
        this.m_id = id;
      }

      public void Do(MyUserController controller) => controller.m_cells.Add(this.m_id, controller.Actor.CreateCell(this.m_offset, this.m_lod));
    }

    private class MDeleteCell : MyUserController.IMessage
    {
      private readonly uint m_id;

      public MDeleteCell(uint id) => this.m_id = id;

      public void Do(MyUserController controller)
      {
        IMyVoxelActorCell cell;
        if (!controller.m_cells.TryGetValue(this.m_id, out cell))
          return;
        controller.Actor.DeleteCell(cell);
        controller.m_cells.Remove(this.m_id);
      }
    }
  }
}
