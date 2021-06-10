// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.EnvironmentItems.MyEnvironmentSector
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Game.Models;
using VRage.Utils;
using VRageMath;
using VRageMath.PackedVector;
using VRageRender;
using VRageRender.Import;
using VRageRender.Messages;

namespace Sandbox.Game.Entities.EnvironmentItems
{
  public class MyEnvironmentSector
  {
    private readonly Vector3I m_id;
    private MatrixD m_sectorMatrix;
    private MatrixD m_sectorInvMatrix;
    private FastResourceLock m_instancePartsLock = new FastResourceLock();
    private Dictionary<int, MyEnvironmentSector.MyModelInstanceData> m_instanceParts = new Dictionary<int, MyEnvironmentSector.MyModelInstanceData>();
    private List<MyInstanceData> m_tmpInstanceData = new List<MyInstanceData>();
    private BoundingBox m_AABB = BoundingBox.CreateInvalid();
    private bool m_invalidateAABB;
    private int m_sectorItemCount;

    public Vector3I SectorId => this.m_id;

    public MatrixD SectorMatrix => this.m_sectorMatrix;

    public bool IsValid => this.m_sectorItemCount > 0;

    public BoundingBox SectorBox
    {
      get
      {
        if (this.m_invalidateAABB)
        {
          this.m_invalidateAABB = false;
          this.m_AABB = this.GetSectorBoundingBox();
        }
        return this.m_AABB;
      }
    }

    public BoundingBoxD SectorWorldBox => this.SectorBox.Transform(this.m_sectorMatrix);

    public int SectorItemCount => this.m_sectorItemCount;

    public MyEnvironmentSector(Vector3I id, Vector3D sectorOffset)
    {
      this.m_id = id;
      this.m_sectorMatrix = MatrixD.CreateTranslation(sectorOffset);
      this.m_sectorInvMatrix = MatrixD.Invert(this.m_sectorMatrix);
    }

    public void UnloadRenderObjects()
    {
      using (this.m_instancePartsLock.AcquireExclusiveUsing())
      {
        foreach (KeyValuePair<int, MyEnvironmentSector.MyModelInstanceData> instancePart in this.m_instanceParts)
          instancePart.Value.UnloadRenderObjects();
      }
    }

    public void ClearInstanceData()
    {
      this.m_tmpInstanceData.Clear();
      this.m_AABB = BoundingBox.CreateInvalid();
      this.m_sectorItemCount = 0;
      using (this.m_instancePartsLock.AcquireExclusiveUsing())
      {
        foreach (KeyValuePair<int, MyEnvironmentSector.MyModelInstanceData> instancePart in this.m_instanceParts)
          instancePart.Value.InstanceData.Clear();
      }
    }

    public int AddInstance(
      MyStringHash subtypeId,
      int modelId,
      int localId,
      ref Matrix localMatrix,
      BoundingBox localAabb,
      MyInstanceFlagsEnum instanceFlags,
      float maxViewDistance,
      Vector3 colorMaskHsv = default (Vector3))
    {
      MyEnvironmentSector.MyModelInstanceData modelInstanceData;
      using (this.m_instancePartsLock.AcquireExclusiveUsing())
      {
        if (!this.m_instanceParts.TryGetValue(modelId, out modelInstanceData))
        {
          modelInstanceData = new MyEnvironmentSector.MyModelInstanceData(this, subtypeId, modelId, instanceFlags, maxViewDistance, localAabb);
          this.m_instanceParts.Add(modelId, modelInstanceData);
        }
      }
      MyEnvironmentSector.MySectorInstanceData instanceData = new MyEnvironmentSector.MySectorInstanceData()
      {
        LocalId = localId,
        InstanceData = new MyInstanceData()
        {
          ColorMaskHSV = new HalfVector4(colorMaskHsv.X, colorMaskHsv.Y, colorMaskHsv.Z, 0.0f),
          LocalMatrix = localMatrix
        }
      };
      int key = modelInstanceData.AddInstanceData(ref instanceData);
      localMatrix = modelInstanceData.InstanceData[key].LocalMatrix;
      this.m_AABB = this.m_AABB.Include(localAabb.Transform(localMatrix));
      ++this.m_sectorItemCount;
      this.m_invalidateAABB = true;
      return key;
    }

    public bool DisableInstance(int sectorInstanceId, int modelId)
    {
      MyEnvironmentSector.MyModelInstanceData modelInstanceData = (MyEnvironmentSector.MyModelInstanceData) null;
      this.m_instanceParts.TryGetValue(modelId, out modelInstanceData);
      if (modelInstanceData == null || !modelInstanceData.DisableInstance(sectorInstanceId))
        return false;
      --this.m_sectorItemCount;
      this.m_invalidateAABB = true;
      return true;
    }

    public void UpdateRenderInstanceData()
    {
      using (this.m_instancePartsLock.AcquireSharedUsing())
      {
        foreach (KeyValuePair<int, MyEnvironmentSector.MyModelInstanceData> instancePart in this.m_instanceParts)
          instancePart.Value.UpdateRenderInstanceData();
      }
    }

    public void UpdateRenderInstanceData(int modelId)
    {
      using (this.m_instancePartsLock.AcquireSharedUsing())
      {
        MyEnvironmentSector.MyModelInstanceData modelInstanceData = (MyEnvironmentSector.MyModelInstanceData) null;
        this.m_instanceParts.TryGetValue(modelId, out modelInstanceData);
        modelInstanceData?.UpdateRenderInstanceData();
      }
    }

    public void UpdateRenderEntitiesData(
      MatrixD worldMatrixD,
      bool useTransparency = false,
      float transparency = 0.0f)
    {
      foreach (MyEnvironmentSector.MyModelInstanceData modelInstanceData in this.m_instanceParts.Values)
        modelInstanceData.UpdateRenderEntitiesData(ref worldMatrixD, useTransparency, transparency);
    }

    public static Vector3I GetSectorId(Vector3D position, float sectorSize) => Vector3I.Floor(position / (double) sectorSize);

    internal void DebugDraw(Vector3I sectorPos, float sectorSize)
    {
      using (this.m_instancePartsLock.AcquireSharedUsing())
      {
        foreach (MyEnvironmentSector.MyModelInstanceData modelInstanceData in this.m_instanceParts.Values)
        {
          using (modelInstanceData.InstanceBufferLock.AcquireSharedUsing())
          {
            foreach (KeyValuePair<int, MyInstanceData> keyValuePair in modelInstanceData.InstanceData)
            {
              MyInstanceData myInstanceData = keyValuePair.Value;
              Matrix matrix = myInstanceData.LocalMatrix;
              Vector3D worldCoord = Vector3D.Transform(matrix.Translation, this.m_sectorMatrix);
              MatrixD matrixD = myInstanceData.LocalMatrix * this.m_sectorMatrix;
              matrix = Matrix.Rescale((Matrix) ref matrixD, modelInstanceData.ModelBox.HalfExtents * 2f);
              MyRenderProxy.DebugDrawOBB((MatrixD) ref matrix, Color.OrangeRed, 0.5f, true, true);
              if (Vector3D.Distance(MySector.MainCamera.Position, worldCoord) < 250.0)
                MyRenderProxy.DebugDrawText3D(worldCoord, modelInstanceData.SubtypeId.ToString(), Color.White, 0.7f, true);
            }
          }
        }
      }
      MyRenderProxy.DebugDrawAABB(this.SectorWorldBox, Color.OrangeRed);
    }

    internal void GetItems(List<Vector3D> output)
    {
      foreach (KeyValuePair<int, MyEnvironmentSector.MyModelInstanceData> instancePart in this.m_instanceParts)
      {
        MyEnvironmentSector.MyModelInstanceData modelInstanceData = instancePart.Value;
        using (modelInstanceData.InstanceBufferLock.AcquireSharedUsing())
        {
          foreach (KeyValuePair<int, MyInstanceData> keyValuePair in modelInstanceData.InstanceData)
          {
            MyInstanceData myInstanceData = keyValuePair.Value;
            if (!myInstanceData.LocalMatrix.EqualsFast(ref Matrix.Zero))
              output.Add(Vector3D.Transform(myInstanceData.LocalMatrix.Translation, this.m_sectorMatrix));
          }
        }
      }
    }

    internal void GetItemsInRadius(Vector3D position, float radius, List<Vector3D> output)
    {
      Vector3D vector3D1 = Vector3D.Transform(position, this.m_sectorInvMatrix);
      foreach (KeyValuePair<int, MyEnvironmentSector.MyModelInstanceData> instancePart in this.m_instanceParts)
      {
        using (instancePart.Value.InstanceBufferLock.AcquireSharedUsing())
        {
          foreach (KeyValuePair<int, MyInstanceData> keyValuePair in instancePart.Value.InstanceData)
          {
            Matrix localMatrix = keyValuePair.Value.LocalMatrix;
            if (Vector3D.DistanceSquared((Vector3D) localMatrix.Translation, vector3D1) < (double) radius * (double) radius)
            {
              List<Vector3D> vector3DList = output;
              localMatrix = keyValuePair.Value.LocalMatrix;
              Vector3D vector3D2 = Vector3D.Transform(localMatrix.Translation, this.m_sectorMatrix);
              vector3DList.Add(vector3D2);
            }
          }
        }
      }
    }

    internal void GetItemsInRadius(
      Vector3 position,
      float radius,
      List<MyEnvironmentItems.ItemInfo> output)
    {
      double num = (double) radius * (double) radius;
      foreach (KeyValuePair<int, MyEnvironmentSector.MyModelInstanceData> instancePart in this.m_instanceParts)
      {
        MyEnvironmentSector.MyModelInstanceData modelInstanceData = instancePart.Value;
        using (modelInstanceData.InstanceBufferLock.AcquireSharedUsing())
        {
          foreach (KeyValuePair<int, MyInstanceData> keyValuePair in modelInstanceData.InstanceData)
          {
            MyInstanceData myInstanceData = keyValuePair.Value;
            Matrix localMatrix = myInstanceData.LocalMatrix;
            if (!localMatrix.EqualsFast(ref Matrix.Zero))
            {
              localMatrix = myInstanceData.LocalMatrix;
              Vector3D position1 = Vector3.Transform(localMatrix.Translation, this.m_sectorMatrix);
              if ((position1 - position).LengthSquared() < num)
                output.Add(new MyEnvironmentItems.ItemInfo()
                {
                  LocalId = modelInstanceData.InstanceIds[keyValuePair.Key],
                  SubtypeId = instancePart.Value.SubtypeId,
                  Transform = new MyTransformD(position1)
                });
            }
          }
        }
      }
    }

    internal void GetItems(List<MyEnvironmentItems.ItemInfo> output)
    {
      foreach (KeyValuePair<int, MyEnvironmentSector.MyModelInstanceData> instancePart in this.m_instanceParts)
      {
        MyEnvironmentSector.MyModelInstanceData modelInstanceData = instancePart.Value;
        using (modelInstanceData.InstanceBufferLock.AcquireSharedUsing())
        {
          foreach (KeyValuePair<int, MyInstanceData> keyValuePair in modelInstanceData.InstanceData)
          {
            Matrix localMatrix = keyValuePair.Value.LocalMatrix;
            if (!localMatrix.EqualsFast(ref Matrix.Zero))
            {
              Vector3D position = Vector3.Transform(localMatrix.Translation, this.m_sectorMatrix);
              output.Add(new MyEnvironmentItems.ItemInfo()
              {
                LocalId = modelInstanceData.InstanceIds[keyValuePair.Key],
                SubtypeId = instancePart.Value.SubtypeId,
                Transform = new MyTransformD(position)
              });
            }
          }
        }
      }
    }

    private BoundingBox GetSectorBoundingBox()
    {
      if (!this.IsValid)
        return new BoundingBox(Vector3.Zero, Vector3.Zero);
      BoundingBox invalid = BoundingBox.CreateInvalid();
      using (this.m_instancePartsLock.AcquireSharedUsing())
      {
        foreach (KeyValuePair<int, MyEnvironmentSector.MyModelInstanceData> instancePart in this.m_instanceParts)
        {
          MyEnvironmentSector.MyModelInstanceData modelInstanceData = instancePart.Value;
          using (modelInstanceData.InstanceBufferLock.AcquireSharedUsing())
          {
            BoundingBox modelBox = modelInstanceData.ModelBox;
            foreach (KeyValuePair<int, MyInstanceData> keyValuePair in modelInstanceData.InstanceData)
            {
              MyInstanceData myInstanceData = keyValuePair.Value;
              if (!myInstanceData.LocalMatrix.EqualsFast(ref Matrix.Zero))
                invalid.Include(modelBox.Transform(myInstanceData.LocalMatrix));
            }
          }
        }
      }
      return invalid;
    }

    private struct MySectorInstanceData
    {
      public int LocalId;
      public MyInstanceData InstanceData;
    }

    private class MyModelInstanceData
    {
      public MyEnvironmentSector Parent;
      public int Model;
      public readonly MyStringHash SubtypeId;
      public readonly MyInstanceFlagsEnum Flags = MyInstanceFlagsEnum.CastShadows | MyInstanceFlagsEnum.ShowLod1 | MyInstanceFlagsEnum.EnableColorMask;
      public readonly float MaxViewDistance = float.MaxValue;
      public readonly Dictionary<int, MyInstanceData> InstanceData = new Dictionary<int, MyInstanceData>();
      public readonly Dictionary<int, int> InstanceIds = new Dictionary<int, int>();
      private int m_keyIndex;
      public readonly BoundingBox ModelBox;
      public uint InstanceBuffer = uint.MaxValue;
      public uint RenderObjectId = uint.MaxValue;
      public FastResourceLock InstanceBufferLock = new FastResourceLock();
      private bool m_changed;

      public int InstanceCount => this.InstanceData.Count;

      public MyModelInstanceData(
        MyEnvironmentSector parent,
        MyStringHash subtypeId,
        int model,
        MyInstanceFlagsEnum flags,
        float maxViewDistance,
        BoundingBox modelBox)
      {
        this.Parent = parent;
        this.SubtypeId = subtypeId;
        this.Flags = flags;
        this.MaxViewDistance = maxViewDistance;
        this.ModelBox = modelBox;
        this.Model = model;
      }

      public int AddInstanceData(
        ref MyEnvironmentSector.MySectorInstanceData instanceData)
      {
        using (this.InstanceBufferLock.AcquireExclusiveUsing())
        {
          while (this.InstanceData.ContainsKey(this.m_keyIndex) && this.InstanceData.Count < int.MaxValue)
            ++this.m_keyIndex;
          if (this.InstanceData.ContainsKey(this.m_keyIndex))
            throw new Exception("No available keys to add new instance data to sector!");
          this.InstanceData.Add(this.m_keyIndex, instanceData.InstanceData);
          this.InstanceIds.Add(this.m_keyIndex, instanceData.LocalId);
          return this.m_keyIndex;
        }
      }

      public void UnloadRenderObjects()
      {
        if (this.InstanceBuffer != uint.MaxValue)
        {
          MyRenderProxy.RemoveRenderObject(this.InstanceBuffer, MyRenderProxy.ObjectType.InstanceBuffer);
          this.InstanceBuffer = uint.MaxValue;
        }
        if (this.RenderObjectId == uint.MaxValue)
          return;
        MyRenderProxy.RemoveRenderObject(this.RenderObjectId, MyRenderProxy.ObjectType.Entity, true);
        this.RenderObjectId = uint.MaxValue;
      }

      public void UpdateRenderInstanceData()
      {
        if (this.InstanceData.Count == 0)
          return;
        if (this.InstanceBuffer == uint.MaxValue)
          this.InstanceBuffer = MyRenderProxy.CreateRenderInstanceBuffer(string.Format("EnvironmentSector{0} - {1}", (object) this.Parent.SectorId, (object) this.SubtypeId), MyRenderInstanceBufferType.Generic);
        MyRenderProxy.UpdateRenderInstanceBufferRange(this.InstanceBuffer, this.InstanceData.Values.ToArray<MyInstanceData>());
      }

      public bool DisableInstance(int sectorInstanceId)
      {
        using (this.InstanceBufferLock.AcquireExclusiveUsing())
        {
          if (!this.InstanceData.ContainsKey(sectorInstanceId))
          {
            int num = MyFakes.ENABLE_FLORA_COMPONENT_DEBUG ? 1 : 0;
            return false;
          }
          this.InstanceData.Remove(sectorInstanceId);
          this.InstanceIds.Remove(sectorInstanceId);
        }
        return true;
      }

      internal void UpdateRenderEntitiesData(
        ref MatrixD worldMatrixD,
        bool useTransparency,
        float transparency)
      {
        int model = this.Model;
        int num = this.InstanceCount > 0 ? 1 : 0;
        bool flag = this.RenderObjectId != uint.MaxValue;
        if (num == 0)
        {
          if (!flag)
            return;
          this.UnloadRenderObjects();
        }
        else
        {
          RenderFlags flags = RenderFlags.CastShadows | RenderFlags.Visible;
          if (!flag)
          {
            string byId = MyModel.GetById(model);
            this.RenderObjectId = MyRenderProxy.CreateRenderEntity("Instance parts, part: " + (object) model, byId, this.Parent.SectorMatrix, MyMeshDrawTechnique.MESH, flags, CullingOptions.Default, (Color) Vector3.One, Vector3.Zero, useTransparency ? transparency : 0.0f, this.MaxViewDistance, fadeIn: true);
          }
          MyRenderProxy.SetInstanceBuffer(this.RenderObjectId, this.InstanceBuffer, 0, this.InstanceData.Count, this.Parent.SectorBox);
          MyRenderProxy.UpdateRenderEntity(this.RenderObjectId, new Color?((Color) Vector3.One), new Vector3?(Vector3.Zero), new float?(useTransparency ? transparency : 0.0f), true);
          MyRenderProxy.UpdateRenderObject(this.RenderObjectId, new MatrixD?(this.Parent.SectorMatrix));
        }
      }
    }
  }
}
