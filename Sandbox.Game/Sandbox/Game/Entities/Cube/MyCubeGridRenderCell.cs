// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyCubeGridRenderCell
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VRage;
using VRage.Game;
using VRage.Game.Models;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;
using VRageRender.Messages;

namespace Sandbox.Game.Entities.Cube
{
  public class MyCubeGridRenderCell
  {
    private static readonly MyObjectBuilderType m_edgeDefinitionType = new MyObjectBuilderType(typeof (MyObjectBuilder_EdgesDefinition));
    public readonly MyRenderComponentCubeGrid m_gridRenderComponent;
    public readonly float EdgeViewDistance;
    public string DebugName;
    private BoundingBox m_boundingBox = BoundingBox.CreateInvalid();
    private static List<MyCubeInstanceData> m_tmpInstanceData = new List<MyCubeInstanceData>();
    private static readonly Dictionary<MyCubeGridRenderCell.MyInstanceBucket, (List<MyCubeGridRenderCell.MyCubeInstanceMergedData>, MyInstanceInfo)> m_tmpInstanceParts = new Dictionary<MyCubeGridRenderCell.MyInstanceBucket, (List<MyCubeGridRenderCell.MyCubeInstanceMergedData>, MyInstanceInfo)>();
    private static List<MyCubeInstanceDecalData> m_tmpDecalData = new List<MyCubeInstanceDecalData>();
    private uint m_parentCullObject = uint.MaxValue;
    private uint m_instanceBufferId = uint.MaxValue;
    private readonly Dictionary<MyCubeGridRenderCell.MyInstanceBucket, MyRenderInstanceInfo> m_instanceInfo = new Dictionary<MyCubeGridRenderCell.MyInstanceBucket, MyRenderInstanceInfo>();
    private readonly Dictionary<MyCubeGridRenderCell.MyInstanceBucket, uint> m_instanceGroupRenderObjects = new Dictionary<MyCubeGridRenderCell.MyInstanceBucket, uint>();
    private readonly ConcurrentDictionary<MyCubePart, ConcurrentDictionary<uint, bool>> m_cubeParts = new ConcurrentDictionary<MyCubePart, ConcurrentDictionary<uint, bool>>();
    private readonly ConcurrentDictionary<long, MyCubeGridRenderCell.MyEdgeRenderData> m_edgesToRender = new ConcurrentDictionary<long, MyCubeGridRenderCell.MyEdgeRenderData>();
    private readonly ConcurrentDictionary<long, MyFourEdgeInfo> m_dirtyEdges = new ConcurrentDictionary<long, MyFourEdgeInfo>();
    private readonly ConcurrentDictionary<long, MyFourEdgeInfo> m_edgeInfosNew = new ConcurrentDictionary<long, MyFourEdgeInfo>();
    private static readonly int m_edgeTypeCount = MyUtils.GetMaxValueFromEnum<MyCubeEdgeType>() + 1;
    private static readonly Dictionary<MyStringHash, int[]> m_edgeModelIdCache = new Dictionary<MyStringHash, int[]>((IEqualityComparer<MyStringHash>) MyStringHash.Comparer);
    private static readonly List<MyCubeGridRenderCell.EdgeInfoNormal> m_edgesToCompare = new List<MyCubeGridRenderCell.EdgeInfoNormal>();

    public ConcurrentDictionary<MyCubePart, ConcurrentDictionary<uint, bool>> CubeParts => this.m_cubeParts;

    public MyCubeGridRenderCell(MyRenderComponentCubeGrid gridRender)
    {
      this.m_gridRenderComponent = gridRender;
      this.EdgeViewDistance = gridRender.GridSizeEnum == MyCubeSize.Large ? 130f : 35f;
    }

    public void AddCubePart(MyCubePart part) => this.m_cubeParts.TryAdd(part, (ConcurrentDictionary<uint, bool>) null);

    public bool HasCubePart(MyCubePart part) => this.m_cubeParts.ContainsKey(part);

    public bool RemoveCubePart(MyCubePart part) => this.m_cubeParts.TryRemove(part, out ConcurrentDictionary<uint, bool> _);

    internal void AddCubePartDecal(MyCubePart part, uint decalId) => this.m_cubeParts.GetOrAdd(part, (Func<MyCubePart, ConcurrentDictionary<uint, bool>>) (x => new ConcurrentDictionary<uint, bool>())).TryAdd(decalId, true);

    internal void RemoveCubePartDecal(MyCubePart part, uint decalId)
    {
      ConcurrentDictionary<uint, bool> concurrentDictionary;
      if (!this.m_cubeParts.TryGetValue(part, out concurrentDictionary))
        return;
      concurrentDictionary.TryRemove(decalId, out bool _);
    }

    public bool AddEdgeInfo(long hash, MyEdgeInfo info, MySlimBlock owner)
    {
      MyFourEdgeInfo orAdd;
      if (!this.m_edgeInfosNew.TryGetValue(hash, out orAdd))
      {
        MyFourEdgeInfo myFourEdgeInfo = new MyFourEdgeInfo(info.LocalOrthoMatrix, info.EdgeType);
        orAdd = this.m_edgeInfosNew.GetOrAdd(hash, myFourEdgeInfo);
      }
      bool flag;
      lock (orAdd)
      {
        flag = orAdd.AddInstance(owner.Position * owner.CubeGrid.GridSize, info.Color, owner.SkinSubtypeId, info.EdgeModel, info.PackedNormal0, info.PackedNormal1);
        if (flag)
        {
          if (orAdd.Full)
          {
            this.m_dirtyEdges.Remove<long, MyFourEdgeInfo>(hash);
            this.m_edgesToRender.Remove<long, MyCubeGridRenderCell.MyEdgeRenderData>(hash);
          }
          else
            this.m_dirtyEdges[hash] = orAdd;
        }
      }
      return flag;
    }

    public bool RemoveEdgeInfo(long hash, MySlimBlock owner)
    {
      MyFourEdgeInfo myFourEdgeInfo;
      if (!this.m_edgeInfosNew.TryGetValue(hash, out myFourEdgeInfo))
        return false;
      bool flag;
      lock (myFourEdgeInfo)
      {
        flag = myFourEdgeInfo.RemoveInstance(owner.Position * owner.CubeGrid.GridSize);
        if (flag)
        {
          if (myFourEdgeInfo.Empty)
          {
            this.m_dirtyEdges.Remove<long, MyFourEdgeInfo>(hash);
            this.m_edgeInfosNew.Remove<long, MyFourEdgeInfo>(hash);
            this.m_edgesToRender.Remove<long, MyCubeGridRenderCell.MyEdgeRenderData>(hash);
          }
          else
            this.m_dirtyEdges[hash] = myFourEdgeInfo;
        }
      }
      return flag;
    }

    private bool InstanceDataCleared(
      Dictionary<MyCubeGridRenderCell.MyInstanceBucket, (List<MyCubeGridRenderCell.MyCubeInstanceMergedData>, MyInstanceInfo)> instanceParts)
    {
      foreach (KeyValuePair<MyCubeGridRenderCell.MyInstanceBucket, (List<MyCubeGridRenderCell.MyCubeInstanceMergedData>, MyInstanceInfo)> instancePart in instanceParts)
      {
        if (instancePart.Value.Item1.Count > 0)
          return false;
      }
      return true;
    }

    public void RebuildInstanceParts(RenderFlags renderFlags)
    {
      Thread updateThread = MySandboxGame.Static.UpdateThread;
      Thread currentThread = Thread.CurrentThread;
      Dictionary<MyCubeGridRenderCell.MyInstanceBucket, (List<MyCubeGridRenderCell.MyCubeInstanceMergedData>, MyInstanceInfo)> tmpInstanceParts = MyCubeGridRenderCell.m_tmpInstanceParts;
      foreach (KeyValuePair<MyCubePart, ConcurrentDictionary<uint, bool>> cubePart in this.m_cubeParts)
      {
        MyCubePart k;
        ConcurrentDictionary<uint, bool> v;
        cubePart.Deconstruct<MyCubePart, ConcurrentDictionary<uint, bool>>(out k, out v);
        MyCubePart myCubePart = k;
        ConcurrentDictionary<uint, bool> decals = v;
        this.AddInstancePart(tmpInstanceParts, myCubePart.Model.UniqueId, myCubePart.SkinSubtypeId, ref myCubePart.InstanceData, decals, MyInstanceFlagsEnum.CastShadows | MyInstanceFlagsEnum.ShowLod1 | MyInstanceFlagsEnum.EnableColorMask);
      }
      this.UpdateDirtyEdges();
      this.AddEdgeParts(tmpInstanceParts);
      this.UpdateRenderInstanceData(tmpInstanceParts, renderFlags);
      this.ClearInstanceParts(tmpInstanceParts);
      if (this.m_gridRenderComponent == null)
        return;
      this.m_gridRenderComponent.FadeIn = false;
    }

    private bool IsEdgeVisible(MyFourEdgeInfo edgeInfo, out int modelId)
    {
      lock (edgeInfo)
      {
        modelId = 0;
        MyCubeGridRenderCell.m_edgesToCompare.Clear();
        if (edgeInfo.Full)
          return false;
        for (int index = 0; index < 4; ++index)
        {
          Color color;
          MyStringHash skinSubtypeId;
          MyStringHash edgeModel;
          Base27Directions.Direction normal0;
          Base27Directions.Direction normal1;
          if (edgeInfo.GetNormalInfo(index, out color, out skinSubtypeId, out edgeModel, out normal0, out normal1))
          {
            int num = edgeModel == MyStringHash.NullOrEmpty ? 1 : 0;
            List<MyCubeGridRenderCell.EdgeInfoNormal> edgesToCompare1 = MyCubeGridRenderCell.m_edgesToCompare;
            MyCubeGridRenderCell.EdgeInfoNormal edgeInfoNormal1 = new MyCubeGridRenderCell.EdgeInfoNormal();
            edgeInfoNormal1.Normal = Base27Directions.GetVector(normal0);
            edgeInfoNormal1.Color = color;
            edgeInfoNormal1.SkinSubtypeId = skinSubtypeId;
            edgeInfoNormal1.EdgeModel = edgeModel;
            MyCubeGridRenderCell.EdgeInfoNormal edgeInfoNormal2 = edgeInfoNormal1;
            edgesToCompare1.Add(edgeInfoNormal2);
            List<MyCubeGridRenderCell.EdgeInfoNormal> edgesToCompare2 = MyCubeGridRenderCell.m_edgesToCompare;
            edgeInfoNormal1 = new MyCubeGridRenderCell.EdgeInfoNormal();
            edgeInfoNormal1.Normal = Base27Directions.GetVector(normal1);
            edgeInfoNormal1.Color = color;
            edgeInfoNormal1.SkinSubtypeId = skinSubtypeId;
            edgeInfoNormal1.EdgeModel = edgeModel;
            MyCubeGridRenderCell.EdgeInfoNormal edgeInfoNormal3 = edgeInfoNormal1;
            edgesToCompare2.Add(edgeInfoNormal3);
          }
        }
        if (MyCubeGridRenderCell.m_edgesToCompare.Count == 0)
          return false;
        bool flag1 = MyCubeGridRenderCell.m_edgesToCompare.Count == 4;
        MyStringHash edgeModel1 = MyCubeGridRenderCell.m_edgesToCompare[0].EdgeModel;
        for (int index1 = 0; index1 < MyCubeGridRenderCell.m_edgesToCompare.Count; ++index1)
        {
          for (int index2 = index1 + 1; index2 < MyCubeGridRenderCell.m_edgesToCompare.Count; ++index2)
          {
            if (MyUtils.IsZero(MyCubeGridRenderCell.m_edgesToCompare[index1].Normal + MyCubeGridRenderCell.m_edgesToCompare[index2].Normal, 0.1f))
            {
              MyCubeGridRenderCell.m_edgesToCompare.RemoveAt(index2);
              MyCubeGridRenderCell.m_edgesToCompare.RemoveAt(index1);
              --index1;
              break;
            }
          }
        }
        if (MyCubeGridRenderCell.m_edgesToCompare.Count == 1)
          return false;
        bool flag2 = false;
        bool flag3 = false;
        bool flag4 = false;
        if (MyCubeGridRenderCell.m_edgesToCompare.Count > 0)
        {
          Color color = MyCubeGridRenderCell.m_edgesToCompare[0].Color;
          MyStringHash skinSubtypeId = MyCubeGridRenderCell.m_edgesToCompare[0].SkinSubtypeId;
          edgeModel1 = MyCubeGridRenderCell.m_edgesToCompare[0].EdgeModel;
          for (int index = 1; index < MyCubeGridRenderCell.m_edgesToCompare.Count; ++index)
          {
            MyCubeGridRenderCell.EdgeInfoNormal edgeInfoNormal = MyCubeGridRenderCell.m_edgesToCompare[index];
            flag2 |= edgeInfoNormal.Color != color;
            flag4 |= edgeInfoNormal.SkinSubtypeId != skinSubtypeId;
            flag3 |= edgeModel1 != edgeInfoNormal.EdgeModel;
            if (flag2 | flag3 | flag4)
              break;
          }
        }
        if (MyCubeGridRenderCell.m_edgesToCompare.Count == 1 || !(flag2 | flag3 | flag4) && (MyCubeGridRenderCell.m_edgesToCompare.Count <= 2 && !(MyCubeGridRenderCell.m_edgesToCompare.Count != 0 ? (double) Math.Abs(Vector3.Dot(MyCubeGridRenderCell.m_edgesToCompare[0].Normal, MyCubeGridRenderCell.m_edgesToCompare[1].Normal)) < 0.850000023841858 : flag1)))
          return false;
        int edgeTypeCount = MyCubeGridRenderCell.m_edgeTypeCount;
        int[] numArray;
        if (!MyCubeGridRenderCell.m_edgeModelIdCache.TryGetValue(edgeModel1, out numArray))
        {
          MyEdgesDefinition edgesDefinition = MyDefinitionManager.Static.GetEdgesDefinition(new MyDefinitionId(MyCubeGridRenderCell.m_edgeDefinitionType, edgeModel1));
          MyEdgesModelSet small = edgesDefinition.Small;
          MyEdgesModelSet large = edgesDefinition.Large;
          numArray = new int[MyCubeGridRenderCell.m_edgeTypeCount * 2];
          foreach (MyCubeEdgeType myCubeEdgeType in MyEnum<MyCubeEdgeType>.Values)
          {
            int num1;
            int num2;
            switch (myCubeEdgeType)
            {
              case MyCubeEdgeType.Vertical:
                num1 = MyModel.GetId(small.Vertical);
                num2 = MyModel.GetId(large.Vertical);
                break;
              case MyCubeEdgeType.Vertical_Diagonal:
                num1 = MyModel.GetId(small.VerticalDiagonal);
                num2 = MyModel.GetId(large.VerticalDiagonal);
                break;
              case MyCubeEdgeType.Horizontal:
                num1 = MyModel.GetId(small.Horisontal);
                num2 = MyModel.GetId(large.Horisontal);
                break;
              case MyCubeEdgeType.Horizontal_Diagonal:
                num1 = MyModel.GetId(small.HorisontalDiagonal);
                num2 = MyModel.GetId(large.HorisontalDiagonal);
                break;
              case MyCubeEdgeType.Hidden:
                num1 = 0;
                num2 = 0;
                break;
              default:
                throw new Exception("Unhandled edge type");
            }
            int index = (int) myCubeEdgeType;
            numArray[index] = num1;
            numArray[index + edgeTypeCount] = num2;
          }
          MyCubeGridRenderCell.m_edgeModelIdCache.Add(edgeModel1, numArray);
        }
        int edgeType = (int) edgeInfo.EdgeType;
        int num3 = this.m_gridRenderComponent.GridSizeEnum == MyCubeSize.Large ? edgeTypeCount : 0;
        modelId = numArray[num3 + edgeType];
      }
      return true;
    }

    private void UpdateDirtyEdges()
    {
      foreach (KeyValuePair<long, MyFourEdgeInfo> dirtyEdge in this.m_dirtyEdges)
      {
        long k;
        MyFourEdgeInfo v;
        dirtyEdge.Deconstruct<long, MyFourEdgeInfo>(out k, out v);
        long key = k;
        MyFourEdgeInfo edgeInfo = v;
        int timestamp1 = edgeInfo.Timestamp;
        int modelId;
        if (this.IsEdgeVisible(edgeInfo, out modelId))
          this.m_edgesToRender[key] = new MyCubeGridRenderCell.MyEdgeRenderData(modelId, edgeInfo);
        else
          this.m_edgesToRender.Remove<long, MyCubeGridRenderCell.MyEdgeRenderData>(key);
        this.m_dirtyEdges.Remove<long, MyFourEdgeInfo>(key);
        int timestamp2 = edgeInfo.Timestamp;
        if (timestamp1 != timestamp2)
          this.m_dirtyEdges.TryAdd(key, edgeInfo);
      }
    }

    private void AddEdgeParts(
      Dictionary<MyCubeGridRenderCell.MyInstanceBucket, (List<MyCubeGridRenderCell.MyCubeInstanceMergedData>, MyInstanceInfo)> instanceParts)
    {
      MyCubeInstanceData instance = new MyCubeInstanceData();
      instance.ResetBones();
      instance.SetTextureOffset(new Vector4UByte((byte) 0, (byte) 0, (byte) 1, (byte) 1));
      foreach (KeyValuePair<long, MyCubeGridRenderCell.MyEdgeRenderData> keyValuePair in this.m_edgesToRender)
      {
        int modelId = keyValuePair.Value.ModelId;
        MyFourEdgeInfo edgeInfo = keyValuePair.Value.EdgeInfo;
        Color color;
        MyStringHash skinSubtypeId;
        edgeInfo.GetNormalInfo(edgeInfo.FirstAvailable, out color, out skinSubtypeId, out MyStringHash _, out Base27Directions.Direction _, out Base27Directions.Direction _);
        instance.PackedOrthoMatrix = edgeInfo.LocalOrthoMatrix;
        instance.ColorMaskHSV = new Vector4(color.ColorToHSVDX11(), 0.0f);
        this.AddInstancePart(instanceParts, modelId, skinSubtypeId, ref instance, (ConcurrentDictionary<uint, bool>) null, (MyInstanceFlagsEnum) 0, this.EdgeViewDistance);
      }
    }

    private void ClearInstanceParts(
      Dictionary<MyCubeGridRenderCell.MyInstanceBucket, (List<MyCubeGridRenderCell.MyCubeInstanceMergedData>, MyInstanceInfo)> instanceParts)
    {
      this.m_boundingBox = BoundingBox.CreateInvalid();
      foreach (KeyValuePair<MyCubeGridRenderCell.MyInstanceBucket, (List<MyCubeGridRenderCell.MyCubeInstanceMergedData>, MyInstanceInfo)> instancePart in instanceParts)
        instancePart.Value.Item1.Clear();
    }

    private void AddInstancePart(
      Dictionary<MyCubeGridRenderCell.MyInstanceBucket, (List<MyCubeGridRenderCell.MyCubeInstanceMergedData>, MyInstanceInfo)> instanceParts,
      int modelId,
      MyStringHash skinSubtypeId,
      ref MyCubeInstanceData instance,
      ConcurrentDictionary<uint, bool> decals,
      MyInstanceFlagsEnum flags,
      float maxViewDistance = 3.402823E+38f)
    {
      MyCubeGridRenderCell.MyInstanceBucket key = new MyCubeGridRenderCell.MyInstanceBucket(modelId, skinSubtypeId);
      (List<MyCubeGridRenderCell.MyCubeInstanceMergedData>, MyInstanceInfo) valueTuple;
      if (!instanceParts.TryGetValue(key, out valueTuple))
      {
        valueTuple = (new List<MyCubeGridRenderCell.MyCubeInstanceMergedData>(), new MyInstanceInfo(flags, maxViewDistance));
        instanceParts.Add(key, valueTuple);
      }
      Vector3 translation = instance.LocalMatrix.Translation;
      BoundingBox box = new BoundingBox(translation - new Vector3(this.m_gridRenderComponent.GridSize), translation + new Vector3(this.m_gridRenderComponent.GridSize));
      this.m_boundingBox.Include(ref box);
      valueTuple.Item1.Add(new MyCubeGridRenderCell.MyCubeInstanceMergedData()
      {
        CubeInstanceData = instance,
        Decals = decals
      });
    }

    private void AddRenderObjectId(
      bool registerForPositionUpdates,
      out uint renderObjectId,
      uint newId)
    {
      renderObjectId = newId;
      Sandbox.Game.Entities.MyEntities.AddRenderObjectToMap(newId, this.m_gridRenderComponent.Container.Entity);
      if (!registerForPositionUpdates)
        return;
      try
      {
        while (true)
        {
          uint[] renderObjectIds = this.m_gridRenderComponent.RenderObjectIDs;
          for (int index = 0; index < renderObjectIds.Length; ++index)
          {
            if (renderObjectIds[index] == uint.MaxValue)
            {
              renderObjectIds[index] = renderObjectId;
              return;
            }
          }
          this.m_gridRenderComponent.ResizeRenderObjectArray(renderObjectIds.Length + 3);
        }
      }
      finally
      {
        this.m_gridRenderComponent.SetVisibilityUpdates(true);
      }
    }

    private void RemoveRenderObjectId(
      bool unregisterFromPositionUpdates,
      ref uint renderObjectId,
      MyRenderProxy.ObjectType type)
    {
      Sandbox.Game.Entities.MyEntities.RemoveRenderObjectFromMap(renderObjectId);
      MyRenderProxy.RemoveRenderObject(renderObjectId, type, this.m_gridRenderComponent.FadeOut);
      if (unregisterFromPositionUpdates)
      {
        uint[] renderObjectIds = this.m_gridRenderComponent.RenderObjectIDs;
        for (int index = 0; index < renderObjectIds.Length; ++index)
        {
          if ((int) renderObjectIds[index] == (int) renderObjectId)
          {
            renderObjectIds[index] = uint.MaxValue;
            break;
          }
        }
      }
      renderObjectId = uint.MaxValue;
    }

    private void UpdateRenderInstanceData(
      Dictionary<MyCubeGridRenderCell.MyInstanceBucket, (List<MyCubeGridRenderCell.MyCubeInstanceMergedData>, MyInstanceInfo)> instanceParts,
      RenderFlags renderFlags)
    {
      if (this.m_parentCullObject == uint.MaxValue)
        this.AddRenderObjectId(true, out this.m_parentCullObject, MyRenderProxy.CreateManualCullObject(this.m_gridRenderComponent.Container.Entity.DisplayName + " " + (object) this.m_gridRenderComponent.Container.Entity.EntityId + ", cull object", this.m_gridRenderComponent.Container.Entity.PositionComp.WorldMatrixRef));
      if (this.m_instanceBufferId == uint.MaxValue)
        this.AddRenderObjectId(false, out this.m_instanceBufferId, MyRenderProxy.CreateRenderInstanceBuffer(this.m_gridRenderComponent.Container.Entity.DisplayName + " " + (object) this.m_gridRenderComponent.Container.Entity.EntityId + ", instance buffer " + this.DebugName, MyRenderInstanceBufferType.Cube, this.m_gridRenderComponent.GetRenderObjectID()));
      this.m_instanceInfo.Clear();
      MyCubeGridRenderCell.m_tmpDecalData.AssertEmpty<MyCubeInstanceDecalData>();
      MyCubeGridRenderCell.m_tmpInstanceData.AssertEmpty<MyCubeInstanceData>();
      int num = -1;
      foreach (KeyValuePair<MyCubeGridRenderCell.MyInstanceBucket, (List<MyCubeGridRenderCell.MyCubeInstanceMergedData>, MyInstanceInfo)> instancePart in instanceParts)
      {
        MyCubeGridRenderCell.MyInstanceBucket key1 = instancePart.Key;
        (List<MyCubeGridRenderCell.MyCubeInstanceMergedData>, MyInstanceInfo) tuple = instancePart.Value;
        this.m_instanceInfo.Add(key1, new MyRenderInstanceInfo(this.m_instanceBufferId, MyCubeGridRenderCell.m_tmpInstanceData.Count, tuple.Item1.Count, tuple.Item2.MaxViewDistance, tuple.Item2.Flags));
        List<MyCubeGridRenderCell.MyCubeInstanceMergedData> instanceMergedDataList = tuple.Item1;
        for (int index = 0; index < instanceMergedDataList.Count; ++index)
        {
          ++num;
          MyCubeGridRenderCell.m_tmpInstanceData.Add(instanceMergedDataList[index].CubeInstanceData);
          ConcurrentDictionary<uint, bool> decals = instanceMergedDataList[index].Decals;
          if (decals != null)
          {
            foreach (uint key2 in (IEnumerable<uint>) decals.Keys)
              MyCubeGridRenderCell.m_tmpDecalData.Add(new MyCubeInstanceDecalData()
              {
                DecalId = key2,
                InstanceIndex = num
              });
          }
        }
      }
      if (MyCubeGridRenderCell.m_tmpInstanceData.Count > 0)
        MyRenderProxy.UpdateRenderCubeInstanceBuffer(this.m_instanceBufferId, ref MyCubeGridRenderCell.m_tmpInstanceData, (int) ((double) MyCubeGridRenderCell.m_tmpInstanceData.Count * 1.20000004768372), ref MyCubeGridRenderCell.m_tmpDecalData);
      MyCubeGridRenderCell.m_tmpDecalData.AssertEmpty<MyCubeInstanceDecalData>();
      MyCubeGridRenderCell.m_tmpInstanceData.AssertEmpty<MyCubeInstanceData>();
      this.UpdateRenderEntitiesInstanceData(renderFlags, this.m_parentCullObject);
    }

    private void UpdateRenderEntitiesInstanceData(RenderFlags renderFlags, uint parentCullObject)
    {
      foreach (KeyValuePair<MyCubeGridRenderCell.MyInstanceBucket, MyRenderInstanceInfo> keyValuePair in this.m_instanceInfo)
      {
        uint renderObjectId;
        bool flag1 = this.m_instanceGroupRenderObjects.TryGetValue(keyValuePair.Key, out renderObjectId);
        int num;
        if (keyValuePair.Value.InstanceCount > 0)
        {
          IMyEntity entity = this.m_gridRenderComponent.Entity;
          num = entity != null ? (entity.InScene ? 1 : 0) : 0;
        }
        else
          num = 0;
        bool flag2 = num != 0;
        RenderFlags flags = renderFlags;
        if (!flag1 & flag2)
        {
          MyDefinitionManager.MyAssetModifiers myAssetModifiers = new MyDefinitionManager.MyAssetModifiers();
          if (keyValuePair.Key.SkinSubtypeId != MyStringHash.NullOrEmpty)
          {
            myAssetModifiers = MyDefinitionManager.Static.GetAssetModifierDefinitionForRender(keyValuePair.Key.SkinSubtypeId);
            if (myAssetModifiers.MetalnessColorable)
              flags |= RenderFlags.MetalnessColorable;
            else
              flags &= ~RenderFlags.MetalnessColorable;
          }
          this.AddRenderObjectId((!MyFakes.MANUAL_CULL_OBJECTS ? 1 : 0) != 0, out renderObjectId, MyRenderProxy.CreateRenderEntity("CubeGridRenderCell " + this.m_gridRenderComponent.Container.Entity.DisplayName + " " + (object) this.m_gridRenderComponent.Container.Entity.EntityId + ", part: " + (object) keyValuePair.Key, MyModel.GetById(keyValuePair.Key.ModelId), this.m_gridRenderComponent.Container.Entity.PositionComp.WorldMatrixRef, MyMeshDrawTechnique.MESH, flags, CullingOptions.Default, this.m_gridRenderComponent.GetDiffuseColor(), Vector3.Zero, this.m_gridRenderComponent.Transparency, keyValuePair.Value.MaxViewDistance, rescale: this.m_gridRenderComponent.CubeGrid.GridScale, fadeIn: ((double) this.m_gridRenderComponent.Transparency == 0.0 && this.m_gridRenderComponent.FadeIn)));
          if (myAssetModifiers.SkinTextureChanges != null)
            MyRenderProxy.ChangeMaterialTexture(renderObjectId, myAssetModifiers.SkinTextureChanges);
          this.m_instanceGroupRenderObjects[keyValuePair.Key] = renderObjectId;
          if (MyFakes.MANUAL_CULL_OBJECTS)
            MyRenderProxy.SetParentCullObject(renderObjectId, parentCullObject, new Matrix?(Matrix.Identity));
        }
        else if (flag1 && !flag2)
        {
          uint groupRenderObject = this.m_instanceGroupRenderObjects[keyValuePair.Key];
          this.RemoveRenderObjectId(!MyFakes.MANUAL_CULL_OBJECTS, ref groupRenderObject, MyRenderProxy.ObjectType.Entity);
          this.m_instanceGroupRenderObjects.Remove(keyValuePair.Key);
          continue;
        }
        if (flag2)
          MyRenderProxy.SetInstanceBuffer(renderObjectId, keyValuePair.Value.InstanceBufferId, keyValuePair.Value.InstanceStart, keyValuePair.Value.InstanceCount, this.m_boundingBox);
      }
    }

    public void OnRemovedFromRender()
    {
      this.UpdateRenderEntitiesInstanceData((RenderFlags) 0, this.m_parentCullObject);
      if (this.m_parentCullObject != uint.MaxValue)
        this.RemoveRenderObjectId(true, ref this.m_parentCullObject, MyRenderProxy.ObjectType.ManualCull);
      if (this.m_instanceBufferId == uint.MaxValue)
        return;
      this.RemoveRenderObjectId(false, ref this.m_instanceBufferId, MyRenderProxy.ObjectType.InstanceBuffer);
    }

    internal void DebugDraw()
    {
      MyRenderProxy.DebugDrawText3D(this.m_boundingBox.Center + this.m_gridRenderComponent.Container.Entity.PositionComp.WorldMatrixRef.Translation, string.Format("CubeParts:{0}, EdgeParts{1}", (object) this.m_cubeParts.Count, (object) this.m_edgeInfosNew.Count), Color.Red, 0.75f, false);
      MyRenderProxy.DebugDrawOBB(Matrix.CreateScale(this.m_boundingBox.Size) * Matrix.CreateTranslation(this.m_boundingBox.Center) * this.m_gridRenderComponent.Container.Entity.PositionComp.WorldMatrixRef, (Color) Color.Red.ToVector3(), 0.25f, true, true);
    }

    internal uint ParentCullObject => this.m_parentCullObject;

    private struct EdgeInfoNormal
    {
      public Vector3 Normal;
      public Color Color;
      public MyStringHash SkinSubtypeId;
      public MyStringHash EdgeModel;
    }

    private struct MyInstanceBucket : IEquatable<MyCubeGridRenderCell.MyInstanceBucket>
    {
      public int ModelId;
      public MyStringHash SkinSubtypeId;

      public MyInstanceBucket(int modelId, MyStringHash skinSubtypeId)
      {
        this.ModelId = modelId;
        this.SkinSubtypeId = skinSubtypeId;
      }

      public bool Equals(MyCubeGridRenderCell.MyInstanceBucket other) => this.ModelId == other.ModelId && this.SkinSubtypeId == other.SkinSubtypeId;
    }

    private struct MyCubeInstanceMergedData
    {
      public MyCubeInstanceData CubeInstanceData;
      public ConcurrentDictionary<uint, bool> Decals;
    }

    private struct MyEdgeRenderData
    {
      public readonly int ModelId;
      public readonly MyFourEdgeInfo EdgeInfo;

      public MyEdgeRenderData(int modelId, MyFourEdgeInfo edgeInfo)
      {
        this.ModelId = modelId;
        this.EdgeInfo = edgeInfo;
      }
    }
  }
}
