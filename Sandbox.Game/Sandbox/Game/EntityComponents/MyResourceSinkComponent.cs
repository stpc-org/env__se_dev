// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.MyResourceSinkComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.EntityComponents
{
  public class MyResourceSinkComponent : MyResourceSinkComponentBase
  {
    private MyEntity m_tmpConnectedEntity;
    private MyResourceSinkComponent.PerTypeData[] m_dataPerType;
    private readonly Dictionary<MyDefinitionId, int> m_resourceTypeToIndex = new Dictionary<MyDefinitionId, int>(1, (IEqualityComparer<MyDefinitionId>) MyDefinitionId.Comparer);
    private readonly List<MyDefinitionId> m_resourceIds = new List<MyDefinitionId>(1);
    [ThreadStatic]
    private static List<MyResourceSinkInfo> m_singleHelperList;
    internal MyStringHash Group;

    public override IMyEntity TemporaryConnectedEntity
    {
      get => (IMyEntity) this.m_tmpConnectedEntity;
      set => this.m_tmpConnectedEntity = (MyEntity) value;
    }

    public MyEntity Entity => base.Entity as MyEntity;

    [Obsolete]
    public float MaxRequiredInput
    {
      get => this.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId);
      set => this.SetMaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId, value);
    }

    [Obsolete]
    public float RequiredInput => this.RequiredInputByType(MyResourceDistributorComponent.ElectricityId);

    [Obsolete]
    public float SuppliedRatio => this.SuppliedRatioByType(MyResourceDistributorComponent.ElectricityId);

    [Obsolete]
    public float CurrentInput => this.CurrentInputByType(MyResourceDistributorComponent.ElectricityId);

    [Obsolete]
    public bool IsPowered => this.IsPoweredByType(MyResourceDistributorComponent.ElectricityId);

    public override ListReader<MyDefinitionId> AcceptedResources => new ListReader<MyDefinitionId>(this.m_resourceIds);

    public event MyRequiredResourceChangeDelegate RequiredInputChanged;

    public event MyResourceAvailableDelegate ResourceAvailable;

    public event MyCurrentResourceInputChangedDelegate CurrentInputChanged;

    public event Action IsPoweredChanged;

    public event Action<MyResourceSinkComponent, MyDefinitionId> OnAddType;

    public event Action<MyResourceSinkComponent, MyDefinitionId> OnRemoveType;

    public MyResourceSinkComponent(int initialAllocationSize = 1) => this.AllocateData(initialAllocationSize);

    public void Init(MyStringHash group, float maxRequiredInput, Func<float> requiredInputFunc)
    {
      using (MyUtils.ReuseCollection<MyResourceSinkInfo>(ref MyResourceSinkComponent.m_singleHelperList))
      {
        MyResourceSinkComponent.m_singleHelperList.Add(new MyResourceSinkInfo()
        {
          ResourceTypeId = MyResourceDistributorComponent.ElectricityId,
          MaxRequiredInput = maxRequiredInput,
          RequiredInputFunc = requiredInputFunc
        });
        this.Init(group, MyResourceSinkComponent.m_singleHelperList);
      }
    }

    public void Init(MyStringHash group, MyResourceSinkInfo sinkData)
    {
      using (MyUtils.ReuseCollection<MyResourceSinkInfo>(ref MyResourceSinkComponent.m_singleHelperList))
      {
        MyResourceSinkComponent.m_singleHelperList.Add(sinkData);
        this.Init(group, MyResourceSinkComponent.m_singleHelperList);
      }
    }

    public void Init(MyStringHash group, List<MyResourceSinkInfo> sinkData)
    {
      this.Group = group;
      if (this.m_dataPerType.Length != sinkData.Count)
        this.AllocateData(sinkData.Count);
      this.m_resourceTypeToIndex.Clear();
      this.m_resourceIds.Clear();
      this.ClearAllCallbacks();
      int num = 0;
      for (int index = 0; index < sinkData.Count; ++index)
      {
        this.m_resourceTypeToIndex.Add(sinkData[index].ResourceTypeId, num++);
        this.m_resourceIds.Add(sinkData[index].ResourceTypeId);
        this.m_dataPerType[num - 1].MaxRequiredInput = sinkData[index].MaxRequiredInput;
        this.m_dataPerType[num - 1].RequiredInputFunc = sinkData[index].RequiredInputFunc;
      }
    }

    public void ClearAllData()
    {
      for (int index = 0; index < this.m_dataPerType.Length; ++index)
      {
        this.m_dataPerType[index].IsPowered = false;
        this.m_dataPerType[index].SuppliedRatio = 0.0f;
      }
    }

    public void AddType(ref MyResourceSinkInfo sinkData)
    {
      if (this.m_resourceIds.Contains(sinkData.ResourceTypeId) || this.m_resourceTypeToIndex.ContainsKey(sinkData.ResourceTypeId))
        return;
      MyResourceSinkComponent.PerTypeData[] perTypeDataArray = new MyResourceSinkComponent.PerTypeData[this.m_resourceIds.Count + 1];
      for (int index = 0; index < this.m_dataPerType.Length; ++index)
        perTypeDataArray[index] = this.m_dataPerType[index];
      this.m_dataPerType = perTypeDataArray;
      this.m_dataPerType[this.m_dataPerType.Length - 1] = new MyResourceSinkComponent.PerTypeData()
      {
        MaxRequiredInput = sinkData.MaxRequiredInput,
        RequiredInputFunc = sinkData.RequiredInputFunc
      };
      this.m_resourceIds.Add(sinkData.ResourceTypeId);
      this.m_resourceTypeToIndex.Add(sinkData.ResourceTypeId, this.m_dataPerType.Length - 1);
      if (this.OnAddType == null)
        return;
      this.OnAddType(this, sinkData.ResourceTypeId);
    }

    public void RemoveType(ref MyDefinitionId resourceType)
    {
      if (!this.m_resourceIds.Contains(resourceType))
        return;
      if (this.OnRemoveType != null)
        this.OnRemoveType(this, resourceType);
      MyResourceSinkComponent.PerTypeData[] perTypeDataArray = new MyResourceSinkComponent.PerTypeData[this.m_resourceIds.Count - 1];
      int typeIndex = this.GetTypeIndex(resourceType);
      int index1 = 0;
      int index2 = 0;
      while (index2 < this.m_dataPerType.Length)
      {
        if (index2 != typeIndex)
          perTypeDataArray[index1] = this.m_dataPerType[index2];
        ++index2;
        ++index1;
      }
      this.m_dataPerType = perTypeDataArray;
      this.m_resourceIds.Remove(resourceType);
      this.m_resourceTypeToIndex.Remove(resourceType);
    }

    private void AllocateData(int allocationSize) => this.m_dataPerType = new MyResourceSinkComponent.PerTypeData[allocationSize];

    public override void SetInputFromDistributor(
      MyDefinitionId resourceTypeId,
      float newResourceInput,
      bool isAdaptible,
      bool fireEvents = true)
    {
      int typeIndex = this.GetTypeIndex(resourceTypeId);
      float currentInput = this.m_dataPerType[typeIndex].CurrentInput;
      float requiredInput = this.m_dataPerType[typeIndex].RequiredInput;
      bool flag1;
      float num;
      if ((double) newResourceInput > 0.0 || (double) requiredInput == 0.0)
      {
        flag1 = isAdaptible || (double) newResourceInput >= (double) requiredInput;
        num = (double) requiredInput <= 0.0 ? 1f : newResourceInput / requiredInput;
      }
      else
      {
        flag1 = false;
        num = 0.0f;
      }
      bool flag2 = !newResourceInput.IsEqual(this.m_dataPerType[typeIndex].CurrentInput, 1E-06f);
      bool flag3 = flag1 != this.m_dataPerType[typeIndex].IsPowered;
      this.m_dataPerType[typeIndex].IsPowered = flag1;
      this.m_dataPerType[typeIndex].SuppliedRatio = num;
      this.m_dataPerType[typeIndex].CurrentInput = newResourceInput;
      if (!fireEvents)
        return;
      if (flag2 && this.CurrentInputChanged != null)
      {
        if (MyEntities.IsAsyncUpdateInProgress)
        {
          MyResourceSinkComponent thiz = this;
          float oldInputCopy = currentInput;
          MyDefinitionId resourceTypeIdCopy = resourceTypeId;
          MyCurrentResourceInputChangedDelegate handler = this.CurrentInputChanged;
          MyEntities.InvokeLater((Action) (() =>
          {
            MyCurrentResourceInputChangedDelegate inputChangedDelegate = handler;
            if (inputChangedDelegate == null)
              return;
            inputChangedDelegate(resourceTypeIdCopy, oldInputCopy, thiz);
          }));
        }
        else
          this.CurrentInputChanged(resourceTypeId, currentInput, this);
      }
      if (!flag3 || this.IsPoweredChanged == null)
        return;
      if (MyEntities.IsAsyncUpdateInProgress)
        MyEntities.InvokeLater(this.IsPoweredChanged);
      else
        this.IsPoweredChanged.InvokeIfNotNull();
    }

    public float ResourceAvailableByType(MyDefinitionId resourceTypeId)
    {
      float num = this.CurrentInputByType(resourceTypeId);
      if (this.ResourceAvailable != null)
        num += this.ResourceAvailable(resourceTypeId, this);
      return num;
    }

    public override bool IsPowerAvailable(MyDefinitionId resourceTypeId, float power) => (double) this.ResourceAvailableByType(resourceTypeId) >= (double) power;

    public void Update()
    {
      foreach (MyDefinitionId key in this.m_resourceTypeToIndex.Keys)
        this.SetRequiredInputByType(key, this.m_dataPerType[this.GetTypeIndex(key)].RequiredInputFunc());
    }

    public override float MaxRequiredInputByType(MyDefinitionId resourceTypeId) => this.m_dataPerType[this.GetTypeIndex(resourceTypeId)].MaxRequiredInput;

    public override void SetMaxRequiredInputByType(
      MyDefinitionId resourceTypeId,
      float newMaxRequiredInput)
    {
      this.m_dataPerType[this.GetTypeIndex(resourceTypeId)].MaxRequiredInput = newMaxRequiredInput;
    }

    public override float CurrentInputByType(MyDefinitionId resourceTypeId) => this.m_dataPerType[this.GetTypeIndex(resourceTypeId)].CurrentInput;

    public override float RequiredInputByType(MyDefinitionId resourceTypeId) => this.m_dataPerType[this.GetTypeIndex(resourceTypeId)].RequiredInput;

    public override bool IsPoweredByType(MyDefinitionId resourceTypeId) => this.m_dataPerType[this.GetTypeIndex(resourceTypeId)].IsPowered;

    public override void SetRequiredInputByType(
      MyDefinitionId resourceTypeId,
      float newRequiredInput)
    {
      int typeIndex = this.GetTypeIndex(resourceTypeId);
      if ((double) this.m_dataPerType[typeIndex].RequiredInput == (double) newRequiredInput)
        return;
      float requiredInput = this.m_dataPerType[typeIndex].RequiredInput;
      this.m_dataPerType[typeIndex].RequiredInput = newRequiredInput;
      if (this.RequiredInputChanged == null)
        return;
      this.RequiredInputChanged(resourceTypeId, this, requiredInput, newRequiredInput);
    }

    public override void SetRequiredInputFuncByType(
      MyDefinitionId resourceTypeId,
      Func<float> newRequiredInputFunc)
    {
      this.m_dataPerType[this.GetTypeIndex(resourceTypeId)].RequiredInputFunc = newRequiredInputFunc;
    }

    public override float SuppliedRatioByType(MyDefinitionId resourceTypeId) => this.m_dataPerType[this.GetTypeIndex(resourceTypeId)].SuppliedRatio;

    protected int GetTypeIndex(MyDefinitionId resourceTypeId)
    {
      int num = 0;
      this.m_resourceTypeToIndex.TryGetValue(resourceTypeId, out num);
      return num;
    }

    public void DebugDraw(Matrix worldMatrix)
    {
      if (!MyDebugDrawSettings.DEBUG_DRAW_RESOURCE_RECEIVERS)
        return;
      double num1 = 2.5;
      double num2 = num1 * 0.045;
      Vector3D worldCoord = (Vector3D) (worldMatrix.Translation + worldMatrix.Up);
      Vector3D position = MySector.MainCamera.Position;
      Vector3D up = MySector.MainCamera.WorldMatrix.Up;
      Vector3D right = MySector.MainCamera.WorldMatrix.Right;
      double num3 = Math.Atan(num1 / Math.Max(Vector3D.Distance(worldCoord, position), 0.001));
      if (num3 <= 0.270000010728836)
        return;
      if (this.Entity != null)
        MyRenderProxy.DebugDrawText3D(worldCoord, this.Entity.ToString(), Color.Yellow, (float) num3, true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      if (this.m_resourceIds == null || this.m_resourceIds.Count == 0)
        return;
      int num4 = -1;
      foreach (MyDefinitionId resourceId in this.m_resourceIds)
      {
        this.DebugDrawResource(resourceId, worldCoord + (double) num4 * up * num2, right, (float) num3);
        --num4;
      }
    }

    private void DebugDrawResource(
      MyDefinitionId resourceId,
      Vector3D origin,
      Vector3D rightVector,
      float textSize)
    {
      Vector3D vector3D = 0.0500000007450581 * rightVector;
      Vector3D worldCoord = origin + vector3D + rightVector * 0.0149999996647239;
      int index = 0;
      string text = resourceId.SubtypeName;
      if (this.m_resourceTypeToIndex.TryGetValue(resourceId, out index))
      {
        MyResourceSinkComponent.PerTypeData perTypeData = this.m_dataPerType[index];
        text = string.Format("{0} Required:{1} Current:{2} Ratio:{3}", (object) resourceId.SubtypeName, (object) perTypeData.RequiredInput, (object) perTypeData.CurrentInput, (object) perTypeData.SuppliedRatio);
      }
      MyRenderProxy.DebugDrawLine3D(origin, origin + vector3D, Color.White, Color.White, false);
      MyRenderProxy.DebugDrawText3D(worldCoord, text, Color.White, textSize, false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
    }

    private void ClearAllCallbacks()
    {
      this.RequiredInputChanged = (MyRequiredResourceChangeDelegate) null;
      this.ResourceAvailable = (MyResourceAvailableDelegate) null;
      this.CurrentInputChanged = (MyCurrentResourceInputChangedDelegate) null;
      this.IsPoweredChanged = (Action) null;
      this.OnAddType = (Action<MyResourceSinkComponent, MyDefinitionId>) null;
      this.OnRemoveType = (Action<MyResourceSinkComponent, MyDefinitionId>) null;
    }

    public override string ComponentTypeDebugString => "Resource Sink";

    public override void OnBeforeRemovedFromContainer()
    {
      base.OnBeforeRemovedFromContainer();
      this.ClearAllCallbacks();
    }

    private struct PerTypeData
    {
      public float CurrentInput;
      public float RequiredInput;
      public float MaxRequiredInput;
      public float SuppliedRatio;
      public Func<float> RequiredInputFunc;
      public bool IsPowered;
    }

    private class Sandbox_Game_EntityComponents_MyResourceSinkComponent\u003C\u003EActor
    {
    }
  }
}
