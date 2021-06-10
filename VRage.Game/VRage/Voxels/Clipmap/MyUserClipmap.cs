// Decompiled with JetBrains decompiler
// Type: VRage.Voxels.Clipmap.MyUserClipmap
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using VRage.Collections;
using VRageMath;
using VRageRender.Voxels;

namespace VRage.Voxels.Clipmap
{
  public class MyUserClipmap : IMyClipmap
  {
    private uint m_nextMeshId = 1;
    private readonly Dictionary<uint, IMyVoxelActorCell> m_cells = new Dictionary<uint, IMyVoxelActorCell>();
    private readonly MyConcurrentQueue<MyUserClipmap.IMessage> m_messageQueue = new MyConcurrentQueue<MyUserClipmap.IMessage>();

    public IEnumerable<IMyVoxelActorCell> Cells => (IEnumerable<IMyVoxelActorCell>) this.m_cells.Values;

    public IMyVoxelActor Actor { get; private set; }

    public Vector3I Size { get; private set; }

    public void Update(ref MatrixD view, BoundingFrustumD viewFrustum, float farClipping)
    {
      MyUserClipmap.IMessage instance;
      while (this.m_messageQueue.TryDequeue(out instance))
        instance.Do(this);
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

    public void DebugDraw()
    {
    }

    public uint CreateCell(Vector3D offset, int lod)
    {
      uint id = this.m_nextMeshId++;
      this.m_messageQueue.Enqueue((MyUserClipmap.IMessage) new MyUserClipmap.MCreateCell(id, offset, lod));
      return id;
    }

    public void DeleteCell(uint id) => this.m_messageQueue.Enqueue((MyUserClipmap.IMessage) new MyUserClipmap.MDeleteCell(id));

    private interface IMessage
    {
      void Do(MyUserClipmap clipmap);
    }

    private class MCreateCell : MyUserClipmap.IMessage
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

      public void Do(MyUserClipmap clipmap) => clipmap.m_cells.Add(this.m_id, clipmap.Actor.CreateCell(this.m_offset, this.m_lod));
    }

    private class MDeleteCell : MyUserClipmap.IMessage
    {
      private readonly uint m_id;

      public MDeleteCell(uint id) => this.m_id = id;

      public void Do(MyUserClipmap clipmap)
      {
        IMyVoxelActorCell cell;
        if (!clipmap.m_cells.TryGetValue(this.m_id, out cell))
          return;
        clipmap.Actor.DeleteCell(cell);
        clipmap.m_cells.Remove(this.m_id);
      }
    }
  }
}
