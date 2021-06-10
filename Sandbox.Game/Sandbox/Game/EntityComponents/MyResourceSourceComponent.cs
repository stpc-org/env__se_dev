// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.MyResourceSourceComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.EntityComponents
{
  public class MyResourceSourceComponent : MyResourceSourceComponentBase
  {
    private int m_allocatedTypeCount;
    private MyResourceSourceComponent.PerTypeData[] m_dataPerType;
    private bool m_enabled;
    private readonly StringBuilder m_textCache = new StringBuilder();
    [ThreadStatic]
    private static List<MyResourceSourceInfo> m_singleHelperList;
    public bool CountTowardsRemainingEnergyTime = true;
    private readonly Dictionary<MyDefinitionId, int> m_resourceTypeToIndex = new Dictionary<MyDefinitionId, int>(1, (IEqualityComparer<MyDefinitionId>) MyDefinitionId.Comparer);
    private readonly List<MyDefinitionId> m_resourceIds = new List<MyDefinitionId>(1);

    public event MyResourceCapacityRemainingChangedDelegate HasCapacityRemainingChanged;

    public event MyResourceCapacityRemainingChangedDelegate ProductionEnabledChanged;

    public event MyResourceOutputChangedDelegate OutputChanged;

    public event MyResourceOutputChangedDelegate MaxOutputChanged;

    public MyEntity TemporaryConnectedEntity { get; set; }

    public MyStringHash Group { get; private set; }

    public float CurrentOutput => this.CurrentOutputByType(this.m_resourceTypeToIndex.FirstPair<MyDefinitionId, int>().Key);

    public float MaxOutput => this.MaxOutputByType(this.m_resourceTypeToIndex.FirstPair<MyDefinitionId, int>().Key);

    public float DefinedOutput => this.DefinedOutputByType(this.m_resourceTypeToIndex.FirstPair<MyDefinitionId, int>().Key);

    public bool ProductionEnabled => this.ProductionEnabledByType(this.m_resourceTypeToIndex.FirstPair<MyDefinitionId, int>().Key);

    public float RemainingCapacity => this.RemainingCapacityByType(this.m_resourceTypeToIndex.FirstPair<MyDefinitionId, int>().Key);

    public bool IsInfiniteCapacity => float.IsInfinity(this.RemainingCapacity);

    public float ProductionToCapacityMultiplier => this.ProductionToCapacityMultiplierByType(this.m_resourceTypeToIndex.FirstPair<MyDefinitionId, int>().Key);

    public bool Enabled
    {
      get => this.m_enabled;
      set => this.SetEnabled(value);
    }

    public bool HasCapacityRemaining => this.HasCapacityRemainingByType(this.m_resourceTypeToIndex.FirstPair<MyDefinitionId, int>().Key);

    public ListReader<MyDefinitionId> ResourceTypes => new ListReader<MyDefinitionId>(this.m_resourceIds);

    public MyResourceSourceComponent(int initialAllocationSize = 1) => this.AllocateData(initialAllocationSize);

    public void Init(MyStringHash sourceGroup, MyResourceSourceInfo sourceResourceData)
    {
      MyUtils.Init<List<MyResourceSourceInfo>>(ref MyResourceSourceComponent.m_singleHelperList);
      MyResourceSourceComponent.m_singleHelperList.Add(sourceResourceData);
      this.Init(sourceGroup, MyResourceSourceComponent.m_singleHelperList);
      MyResourceSourceComponent.m_singleHelperList.Clear();
    }

    public void Init(MyStringHash sourceGroup, List<MyResourceSourceInfo> sourceResourceData)
    {
      this.Group = sourceGroup;
      List<MyResourceSourceInfo> resourceSourceInfoList = sourceResourceData;
      int num1 = resourceSourceInfoList == null ? 0 : ((uint) resourceSourceInfoList.Count > 0U ? 1 : 0);
      int allocationSize = num1 != 0 ? resourceSourceInfoList.Count : 1;
      this.Enabled = true;
      if (allocationSize != this.m_allocatedTypeCount)
        this.AllocateData(allocationSize);
      int num2 = 0;
      if (num1 == 0)
      {
        Dictionary<MyDefinitionId, int> resourceTypeToIndex = this.m_resourceTypeToIndex;
        MyDefinitionId electricityId = MyResourceDistributorComponent.ElectricityId;
        int num3 = num2;
        int num4 = num3 + 1;
        resourceTypeToIndex.Add(electricityId, num3);
        this.m_resourceIds.Add(MyResourceDistributorComponent.ElectricityId);
      }
      else
      {
        foreach (MyResourceSourceInfo resourceSourceInfo in resourceSourceInfoList)
        {
          this.m_resourceTypeToIndex.Add(resourceSourceInfo.ResourceTypeId, num2++);
          this.m_resourceIds.Add(resourceSourceInfo.ResourceTypeId);
          this.m_dataPerType[num2 - 1].DefinedOutput = resourceSourceInfo.DefinedOutput;
          this.SetOutputByType(resourceSourceInfo.ResourceTypeId, 0.0f);
          this.SetMaxOutputByType(resourceSourceInfo.ResourceTypeId, this.m_dataPerType[this.GetTypeIndex(resourceSourceInfo.ResourceTypeId)].DefinedOutput);
          this.SetProductionEnabledByType(resourceSourceInfo.ResourceTypeId, true);
          this.m_dataPerType[num2 - 1].ProductionToCapacityMultiplier = (double) resourceSourceInfo.ProductionToCapacityMultiplier != 0.0 ? resourceSourceInfo.ProductionToCapacityMultiplier : 1f;
          if (resourceSourceInfo.IsInfiniteCapacity)
            this.SetRemainingCapacityByType(resourceSourceInfo.ResourceTypeId, float.PositiveInfinity);
        }
      }
    }

    private void AllocateData(int allocationSize)
    {
      this.m_dataPerType = new MyResourceSourceComponent.PerTypeData[allocationSize];
      this.m_allocatedTypeCount = allocationSize;
    }

    public override float CurrentOutputByType(MyDefinitionId resourceTypeId) => this.m_dataPerType[this.GetTypeIndex(resourceTypeId)].CurrentOutput;

    public void SetOutput(float newOutput) => this.SetOutputByType(this.m_resourceTypeToIndex.FirstPair<MyDefinitionId, int>().Key, newOutput);

    public void SetOutputByType(MyDefinitionId resourceTypeId, float newOutput)
    {
      int typeIndex = this.GetTypeIndex(resourceTypeId);
      float currentOutput = this.m_dataPerType[typeIndex].CurrentOutput;
      this.m_dataPerType[typeIndex].CurrentOutput = newOutput;
      if ((double) currentOutput == (double) newOutput || this.OutputChanged == null)
        return;
      this.OutputChanged(resourceTypeId, currentOutput, this);
    }

    public float RemainingCapacityByType(MyDefinitionId resourceTypeId) => this.m_dataPerType[this.GetTypeIndex(resourceTypeId)].RemainingCapacity;

    public void SetRemainingCapacityByType(
      MyDefinitionId resourceTypeId,
      float newRemainingCapacity)
    {
      int typeIndex = this.GetTypeIndex(resourceTypeId);
      double remainingCapacity = (double) this.m_dataPerType[typeIndex].RemainingCapacity;
      float oldOutput = this.MaxOutputLimitedByCapacity(typeIndex);
      this.m_dataPerType[typeIndex].RemainingCapacity = newRemainingCapacity;
      double num = (double) newRemainingCapacity;
      if (remainingCapacity != num)
        this.SetHasCapacityRemainingByType(resourceTypeId, (double) newRemainingCapacity > 0.0);
      if (this.MaxOutputChanged == null || (double) this.MaxOutputLimitedByCapacity(typeIndex) == (double) oldOutput)
        return;
      this.MaxOutputChanged(resourceTypeId, oldOutput, this);
    }

    public override float MaxOutputByType(MyDefinitionId resourceTypeId) => this.MaxOutputLimitedByCapacity(this.GetTypeIndex(resourceTypeId));

    private float MaxOutputLimitedByCapacity(int typeIndex) => Math.Min(this.m_dataPerType[typeIndex].MaxOutput, (float) ((double) this.m_dataPerType[typeIndex].RemainingCapacity * (double) this.m_dataPerType[typeIndex].ProductionToCapacityMultiplier * 60.0));

    public void SetMaxOutput(float newMaxOutput) => this.SetMaxOutputByType(this.m_resourceTypeToIndex.FirstPair<MyDefinitionId, int>().Key, newMaxOutput);

    public void SetMaxOutputByType(MyDefinitionId resourceTypeId, float newMaxOutput)
    {
      int typeIndex = this.GetTypeIndex(resourceTypeId);
      if ((double) this.m_dataPerType[typeIndex].MaxOutput == (double) newMaxOutput)
        return;
      float maxOutput = this.m_dataPerType[typeIndex].MaxOutput;
      this.m_dataPerType[typeIndex].MaxOutput = newMaxOutput;
      if (this.MaxOutputChanged == null)
        return;
      this.MaxOutputChanged(resourceTypeId, maxOutput, this);
    }

    public override float DefinedOutputByType(MyDefinitionId resourceTypeId) => this.m_dataPerType[this.GetTypeIndex(resourceTypeId)].DefinedOutput;

    public float ProductionToCapacityMultiplierByType(MyDefinitionId resourceTypeId) => this.m_dataPerType[this.GetTypeIndex(resourceTypeId)].ProductionToCapacityMultiplier;

    public bool HasCapacityRemainingByType(MyDefinitionId resourceTypeId) => this.IsInfiniteCapacity || MySession.Static.CreativeMode || this.m_dataPerType[this.GetTypeIndex(resourceTypeId)].HasRemainingCapacity;

    private void SetHasCapacityRemainingByType(MyDefinitionId resourceTypeId, bool newHasCapacity)
    {
      if (this.IsInfiniteCapacity)
        return;
      int typeIndex = this.GetTypeIndex(resourceTypeId);
      if (this.m_dataPerType[typeIndex].HasRemainingCapacity == newHasCapacity)
        return;
      this.m_dataPerType[typeIndex].HasRemainingCapacity = newHasCapacity;
      if (this.HasCapacityRemainingChanged != null)
        this.HasCapacityRemainingChanged(resourceTypeId, this);
      if (newHasCapacity)
        return;
      this.m_dataPerType[typeIndex].CurrentOutput = 0.0f;
    }

    public override bool ProductionEnabledByType(MyDefinitionId resourceTypeId) => this.m_dataPerType[this.GetTypeIndex(resourceTypeId)].IsProducerEnabled;

    public void SetProductionEnabledByType(MyDefinitionId resourceTypeId, bool newProducerEnabled)
    {
      int typeIndex = this.GetTypeIndex(resourceTypeId);
      int num = this.m_dataPerType[typeIndex].IsProducerEnabled != newProducerEnabled ? 1 : 0;
      this.m_dataPerType[typeIndex].IsProducerEnabled = newProducerEnabled;
      if (num != 0 && this.ProductionEnabledChanged != null)
        this.ProductionEnabledChanged(resourceTypeId, this);
      if (newProducerEnabled)
        return;
      this.SetOutputByType(resourceTypeId, 0.0f);
    }

    internal void SetEnabled(bool newValue, bool fireEvents = true)
    {
      if (this.m_enabled == newValue)
        return;
      this.m_enabled = newValue;
      if (fireEvents)
        this.OnProductionEnabledChanged();
      if (this.m_enabled)
        return;
      foreach (MyDefinitionId resourceId in this.m_resourceIds)
        this.SetOutputByType(resourceId, 0.0f);
    }

    public void OnProductionEnabledChanged(MyDefinitionId? resId = null)
    {
      if (resId.HasValue)
      {
        if (this.ProductionEnabledChanged == null)
          return;
        this.ProductionEnabledChanged(resId.Value, this);
      }
      else
      {
        foreach (MyDefinitionId resourceId in this.m_resourceIds)
        {
          if (this.ProductionEnabledChanged != null)
            this.ProductionEnabledChanged(resourceId, this);
        }
      }
    }

    protected int GetTypeIndex(MyDefinitionId resourceTypeId)
    {
      int num = 0;
      if (this.m_resourceTypeToIndex.Count > 1)
        num = this.m_resourceTypeToIndex[resourceTypeId];
      return num;
    }

    public override string ToString()
    {
      this.m_textCache.Clear();
      this.m_textCache.AppendFormat("Enabled: {0}", (object) this.Enabled).Append("; \n");
      this.m_textCache.Append("Output: ");
      MyValueFormatter.AppendWorkInBestUnit(this.CurrentOutput, this.m_textCache);
      this.m_textCache.Append("; \n");
      this.m_textCache.Append("Max Output: ");
      MyValueFormatter.AppendWorkInBestUnit(this.MaxOutput, this.m_textCache);
      this.m_textCache.Append("; \n");
      this.m_textCache.AppendFormat("ProductionEnabled: {0}", (object) this.ProductionEnabled);
      return this.m_textCache.ToString();
    }

    public override string ComponentTypeDebugString => "Resource Source";

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
        MyResourceSourceComponent.PerTypeData perTypeData = this.m_dataPerType[index];
        text = string.Format("{0} Max:{1} Current:{2} Remaining:{3}", (object) resourceId.SubtypeName, (object) perTypeData.MaxOutput, (object) perTypeData.CurrentOutput, (object) perTypeData.RemainingCapacity);
      }
      MyRenderProxy.DebugDrawLine3D(origin, origin + vector3D, Color.White, Color.White, false);
      MyRenderProxy.DebugDrawText3D(worldCoord, text, Color.White, textSize, false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
    }

    private struct PerTypeData
    {
      public float CurrentOutput;
      public float MaxOutput;
      public float DefinedOutput;
      public float RemainingCapacity;
      public float ProductionToCapacityMultiplier;
      public bool HasRemainingCapacity;
      public bool IsProducerEnabled;
    }

    private class Sandbox_Game_EntityComponents_MyResourceSourceComponent\u003C\u003EActor
    {
    }
  }
}
