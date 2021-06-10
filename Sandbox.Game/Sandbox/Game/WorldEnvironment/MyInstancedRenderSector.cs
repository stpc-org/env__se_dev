// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.MyInstancedRenderSector
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game.Models;
using VRage.Library.Collections;
using VRageMath;
using VRageRender;
using VRageRender.Import;
using VRageRender.Messages;

namespace Sandbox.Game.WorldEnvironment
{
  public class MyInstancedRenderSector
  {
    private const bool ENABLE_SEPARATE_INSTANCE_LOD = false;
    public MatrixD WorldMatrix;
    private readonly Dictionary<int, MyInstancedRenderSector.InstancedModelBuffer> m_instancedModels = new Dictionary<int, MyInstancedRenderSector.InstancedModelBuffer>();
    private readonly HashSet<int> m_changedBuffers = new HashSet<int>();
    private int m_lod;

    public string Name { get; private set; }

    public int Lod
    {
      get => this.m_lod;
      set
      {
        if (this.m_lod != value && value != -1)
        {
          foreach (MyInstancedRenderSector.InstancedModelBuffer instancedModelBuffer in this.m_instancedModels.Values)
            instancedModelBuffer.SetPerInstanceLod(value == 0);
        }
        this.m_lod = value;
      }
    }

    public MyInstancedRenderSector(string name, MatrixD worldMatrix)
    {
      this.Name = name;
      this.WorldMatrix = worldMatrix;
    }

    private int GetExpandedSize(int size) => size + 5;

    public int AddInstances(int model, MyList<MyInstanceData> instances)
    {
      MyInstancedRenderSector.InstancedModelBuffer instancedModelBuffer;
      if (!this.m_instancedModels.TryGetValue(model, out instancedModelBuffer))
      {
        instancedModelBuffer = new MyInstancedRenderSector.InstancedModelBuffer(this, model);
        instancedModelBuffer.SetPerInstanceLod(this.Lod == 0);
        this.m_instancedModels[model] = instancedModelBuffer;
      }
      instancedModelBuffer.Instances = instances.GetInternalArray();
      int count = instances.Count;
      for (int index = 0; index < count; ++index)
      {
        BoundingBox box = instancedModelBuffer.ModelBb.Transform(instancedModelBuffer.Instances[index].LocalMatrix);
        instancedModelBuffer.Bounds.Include(ref box);
      }
      instancedModelBuffer.UnusedSlots.Clear();
      for (int index = count; index < instances.Capacity; ++index)
        instancedModelBuffer.UnusedSlots.Enqueue((short) index);
      this.m_changedBuffers.Add(model);
      return 0;
    }

    public void CommitChangesToRenderer()
    {
      foreach (int changedBuffer in this.m_changedBuffers)
        this.m_instancedModels[changedBuffer].UpdateRenderObjects();
      this.m_changedBuffers.Clear();
    }

    public void Close()
    {
      foreach (MyInstancedRenderSector.InstancedModelBuffer instancedModelBuffer in this.m_instancedModels.Values)
        instancedModelBuffer.Close();
    }

    public bool HasChanges() => (uint) this.m_changedBuffers.Count > 0U;

    public void DetachEnvironment(MyEnvironmentSector myEnvironmentSector) => this.Close();

    public void RemoveInstance(int modelId, short index)
    {
      MyInstancedRenderSector.InstancedModelBuffer instancedModel = this.m_instancedModels[modelId];
      instancedModel.Instances[(int) index] = new MyInstanceData();
      instancedModel.UnusedSlots.Enqueue(index);
      this.m_changedBuffers.Add(modelId);
    }

    public short AddInstance(int modelId, ref MyInstanceData data)
    {
      MyInstancedRenderSector.InstancedModelBuffer instancedModelBuffer;
      if (!this.m_instancedModels.TryGetValue(modelId, out instancedModelBuffer))
      {
        instancedModelBuffer = new MyInstancedRenderSector.InstancedModelBuffer(this, modelId);
        instancedModelBuffer.SetPerInstanceLod(this.Lod == 0);
        this.m_instancedModels[modelId] = instancedModelBuffer;
      }
      short result;
      if (instancedModelBuffer.UnusedSlots.TryDequeue<short>(out result))
      {
        instancedModelBuffer.Instances[(int) result] = data;
      }
      else
      {
        int size = instancedModelBuffer.Instances != null ? instancedModelBuffer.Instances.Length : 0;
        int expandedSize = this.GetExpandedSize(size);
        Array.Resize<MyInstanceData>(ref instancedModelBuffer.Instances, expandedSize);
        int num = expandedSize - size;
        result = (short) size;
        instancedModelBuffer.Instances[(int) result] = data;
        for (int index = 1; index < num; ++index)
          instancedModelBuffer.UnusedSlots.Enqueue((short) (index + (int) result));
      }
      BoundingBox box = instancedModelBuffer.ModelBb.Transform(data.LocalMatrix);
      instancedModelBuffer.Bounds.Include(ref box);
      this.m_changedBuffers.Add(modelId);
      return result;
    }

    public uint GetRenderEntity(int modelId)
    {
      MyInstancedRenderSector.InstancedModelBuffer instancedModelBuffer;
      return this.m_instancedModels.TryGetValue(modelId, out instancedModelBuffer) ? instancedModelBuffer.RenderObjectId : uint.MaxValue;
    }

    private class InstancedModelBuffer
    {
      public MyInstanceData[] Instances = new MyInstanceData[4];
      public uint[] InstanceOIDs;
      public Queue<short> UnusedSlots = new Queue<short>();
      public uint InstanceBufferId = uint.MaxValue;
      public uint RenderObjectId = uint.MaxValue;
      public BoundingBox Bounds = BoundingBox.CreateInvalid();
      public int Model;
      private readonly MyInstancedRenderSector m_parent;
      public readonly BoundingBox ModelBb;

      public InstancedModelBuffer(MyInstancedRenderSector parent, int modelId)
      {
        this.m_parent = parent;
        this.Model = modelId;
        this.ModelBb = MyModels.GetModelOnlyData(MyModel.GetById(this.Model)).BoundingBox;
      }

      private void UpdateRenderObjectsWithTheNew()
      {
        string byId = MyModel.GetById(this.Model);
        if (this.RenderObjectId != uint.MaxValue)
          MyRenderProxy.RemoveRenderObject(this.RenderObjectId, MyRenderProxy.ObjectType.Entity);
        Vector3D translation = this.m_parent.WorldMatrix.Translation;
        Matrix matrix = (Matrix) ref this.m_parent.WorldMatrix;
        matrix.Translation = Vector3.Zero;
        Matrix[] localMatrices = new Matrix[this.Instances.Length];
        for (int index = 0; index < this.Instances.Length; ++index)
        {
          Matrix localMatrix = this.Instances[index].LocalMatrix;
          localMatrices[index] = localMatrix * matrix;
        }
        this.RenderObjectId = MyRenderProxy.CreateStaticGroup(byId, translation, localMatrices);
      }

      private unsafe void UpdateRenderObjectsWithTheOld()
      {
        string byId = MyModel.GetById(this.Model);
        if (this.InstanceOIDs == null)
        {
          BoundingBox bounds = this.Bounds;
          if (this.RenderObjectId == uint.MaxValue)
            this.RenderObjectId = MyRenderProxy.CreateRenderEntity(string.Format("RO::{0}: {1}", (object) this.m_parent.Name, (object) byId), byId, this.m_parent.WorldMatrix, MyMeshDrawTechnique.MESH, RenderFlags.CastShadows | RenderFlags.Visible | RenderFlags.ForceOldPipeline | RenderFlags.DistanceFade, CullingOptions.Default, (Color) Vector3.One, Vector3.Zero, maxViewDistance: 100000f, fadeIn: true);
          if (this.InstanceBufferId == uint.MaxValue)
            this.InstanceBufferId = MyRenderProxy.CreateRenderInstanceBuffer(string.Format("IB::{0}: {1}", (object) this.m_parent.Name, (object) byId), MyRenderInstanceBufferType.Generic);
          MyRenderProxy.UpdateRenderInstanceBufferRange(this.InstanceBufferId, this.Instances);
          MyRenderProxy.SetInstanceBuffer(this.RenderObjectId, this.InstanceBufferId, 0, this.Instances.Length, bounds);
          MyRenderProxy.UpdateRenderObject(this.RenderObjectId, new MatrixD?(this.m_parent.WorldMatrix));
        }
        else
        {
          if (this.InstanceOIDs.Length != this.Instances.Length)
            this.ResizeActorBuffer();
          fixed (MyInstanceData* myInstanceDataPtr = this.Instances)
          {
            for (int index = 0; index < this.InstanceOIDs.Length; ++index)
            {
              if (this.InstanceOIDs[index] == uint.MaxValue && myInstanceDataPtr[index].m_row0.PackedValue != 0UL)
              {
                MatrixD worldMatrix = myInstanceDataPtr[index].LocalMatrix * this.m_parent.WorldMatrix;
                uint renderEntity = MyRenderProxy.CreateRenderEntity(string.Format("RO::{0}: {1}", (object) this.m_parent.Name, (object) byId), byId, worldMatrix, MyMeshDrawTechnique.MESH, RenderFlags.CastShadows | RenderFlags.Visible, CullingOptions.Default, (Color) Vector3.One, Vector3.Zero, maxViewDistance: 100000f, fadeIn: true);
                MyRenderProxy.UpdateRenderObject(renderEntity, new MatrixD?(worldMatrix), new BoundingBox?(this.ModelBb));
                this.InstanceOIDs[index] = renderEntity;
              }
            }
          }
        }
      }

      public void UpdateRenderObjects()
      {
        if (MyFakes.ENABLE_TREES_IN_THE_NEW_PIPE)
          this.UpdateRenderObjectsWithTheNew();
        else
          this.UpdateRenderObjectsWithTheOld();
      }

      private void ClearRenderObjectsWithTheNew()
      {
        if (this.RenderObjectId == uint.MaxValue)
          return;
        MyRenderProxy.RemoveRenderObject(this.RenderObjectId, MyRenderProxy.ObjectType.Entity);
        this.RenderObjectId = uint.MaxValue;
      }

      private void ClearRenderObjectsWithTheOld()
      {
        if (this.InstanceBufferId != uint.MaxValue)
        {
          MyRenderProxy.RemoveRenderObject(this.InstanceBufferId, MyRenderProxy.ObjectType.InstanceBuffer);
          this.InstanceBufferId = uint.MaxValue;
        }
        if (this.RenderObjectId != uint.MaxValue)
        {
          MyRenderProxy.RemoveRenderObject(this.RenderObjectId, MyRenderProxy.ObjectType.Entity, true);
          this.RenderObjectId = uint.MaxValue;
        }
        if (this.InstanceOIDs == null)
          return;
        for (int index = 0; index < this.InstanceOIDs.Length; ++index)
        {
          if (this.InstanceOIDs[index] != uint.MaxValue)
          {
            MyRenderProxy.RemoveRenderObject(this.InstanceOIDs[index], MyRenderProxy.ObjectType.Entity);
            this.InstanceOIDs[index] = uint.MaxValue;
          }
        }
      }

      public void ClearRenderObjects()
      {
        if (MyFakes.ENABLE_TREES_IN_THE_NEW_PIPE)
          this.ClearRenderObjectsWithTheNew();
        else
          this.ClearRenderObjectsWithTheOld();
      }

      public void Close()
      {
        this.ClearRenderObjects();
        this.Bounds = BoundingBox.CreateInvalid();
        this.Instances = (MyInstanceData[]) null;
        this.InstanceOIDs = (uint[]) null;
        this.UnusedSlots.Clear();
      }

      public void SetPerInstanceLod(bool value)
      {
        if (value == (this.InstanceOIDs != null))
          return;
        MyInstanceData[] instances = this.Instances;
      }

      private void ResizeActorBuffer()
      {
        int num = this.InstanceOIDs != null ? this.InstanceOIDs.Length : 0;
        Array.Resize<uint>(ref this.InstanceOIDs, this.Instances.Length);
        for (int index = num; index < this.InstanceOIDs.Length; ++index)
          this.InstanceOIDs[index] = uint.MaxValue;
      }
    }
  }
}
