// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyComponentStack
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Library.Utils;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Entities
{
  public class MyComponentStack
  {
    public const float MOUNT_THRESHOLD = 1.525902E-05f;
    private readonly MyCubeBlockDefinition m_blockDefinition;
    private float m_buildIntegrity;
    private float m_integrity;
    private bool m_yieldLastComponent = true;
    private ushort m_topGroupIndex;
    private ushort m_topComponentIndex;

    public int LastChangeStamp { get; private set; }

    public bool YieldLastComponent => this.m_yieldLastComponent;

    public bool IsFullIntegrity => (double) this.m_integrity >= (double) this.MaxIntegrity;

    public bool IsFullyDismounted => (double) this.m_integrity < 1.52590218931437E-05;

    public bool IsDestroyed => (double) this.m_integrity < 1.52590218931437E-05;

    public float Integrity
    {
      get => this.m_integrity;
      private set
      {
        if ((double) this.m_integrity == (double) value)
          return;
        bool isFunctional = this.IsFunctional;
        this.m_integrity = value;
        this.CheckFunctionalState(isFunctional);
      }
    }

    public float IntegrityRatio => this.Integrity / this.MaxIntegrity;

    public float MaxIntegrity => this.m_blockDefinition.MaxIntegrity;

    public float BuildRatio => this.m_buildIntegrity / this.MaxIntegrity;

    public float BuildIntegrity
    {
      get => this.m_buildIntegrity;
      private set
      {
        if ((double) this.m_buildIntegrity == (double) value)
          return;
        bool isFunctional = this.IsFunctional;
        this.m_buildIntegrity = value;
        this.CheckFunctionalState(isFunctional);
      }
    }

    public static float NewBlockIntegrity => !MySession.Static.SurvivalMode ? 1f : 1.525902E-05f;

    public bool IsFunctional => this.IsBuilt && (double) this.Integrity > (double) this.MaxIntegrity * (double) this.m_blockDefinition.CriticalIntegrityRatio;

    public bool? WillFunctionalityRise(float mountAmount)
    {
      int num1 = (double) this.Integrity > (double) this.MaxIntegrity * (double) this.m_blockDefinition.CriticalIntegrityRatio ? 1 : 0;
      bool flag = (double) this.Integrity + (double) mountAmount * (double) this.m_blockDefinition.IntegrityPointsPerSec > (double) this.MaxIntegrity * (double) this.m_blockDefinition.CriticalIntegrityRatio;
      int num2 = flag ? 1 : 0;
      return num1 == num2 ? new bool?() : new bool?(flag);
    }

    public bool IsBuilt => (double) this.BuildIntegrity > (double) this.MaxIntegrity * (double) this.m_blockDefinition.FinalModelThreshold() || (double) this.BuildIntegrity == (double) this.MaxIntegrity;

    public event Action IsFunctionalChanged;

    private void CheckFunctionalState(bool oldFunctionalState)
    {
      if (this.IsFunctional == oldFunctionalState)
        return;
      this.IsFunctionalChanged.InvokeIfNotNull();
    }

    public MyComponentStack(
      MyCubeBlockDefinition BlockDefinition,
      float integrityPercent,
      float buildPercent)
    {
      this.m_blockDefinition = BlockDefinition;
      float maxIntegrity = BlockDefinition.MaxIntegrity;
      this.BuildIntegrity = maxIntegrity * buildPercent;
      this.Integrity = maxIntegrity * integrityPercent;
      this.UpdateIndices();
      if ((double) this.Integrity != 0.0)
      {
        float componentIntegrity = this.GetTopComponentIntegrity();
        if ((double) componentIntegrity < 1.52590218931437E-05)
          this.Integrity += 1.525902E-05f - componentIntegrity;
        if ((double) componentIntegrity > (double) BlockDefinition.Components[(int) this.m_topGroupIndex].Definition.MaxIntegrity)
          this.Integrity -= componentIntegrity - (float) BlockDefinition.Components[(int) this.m_topGroupIndex].Definition.MaxIntegrity;
      }
      this.LastChangeStamp = 1;
    }

    private float GetTopComponentIntegrity()
    {
      float integrity = this.Integrity;
      MyCubeBlockDefinition.Component[] components = this.m_blockDefinition.Components;
      for (int index = 0; index < (int) this.m_topGroupIndex; ++index)
        integrity -= (float) (components[index].Definition.MaxIntegrity * components[index].Count);
      return integrity - (float) (components[(int) this.m_topGroupIndex].Definition.MaxIntegrity * (int) this.m_topComponentIndex);
    }

    private void SetTopIndex(int newTopGroupIndex, int newTopComponentIndex)
    {
      this.m_topGroupIndex = (ushort) newTopGroupIndex;
      this.m_topComponentIndex = (ushort) newTopComponentIndex;
    }

    private void UpdateIndices()
    {
      double integrity = (double) this.Integrity;
      MyCubeBlockDefinition blockDefinition = this.m_blockDefinition;
      int newTopGroupIndex = 0;
      int newTopComponentIndex = 0;
      MyCubeBlockDefinition blockDef = blockDefinition;
      ref int local1 = ref newTopGroupIndex;
      ref int local2 = ref newTopComponentIndex;
      MyComponentStack.CalculateIndicesInternal((float) integrity, blockDef, ref local1, ref local2);
      this.SetTopIndex(newTopGroupIndex, newTopComponentIndex);
    }

    private static void CalculateIndicesInternal(
      float integrity,
      MyCubeBlockDefinition blockDef,
      ref int topGroupIndex,
      ref int topComponentIndex)
    {
      float num1 = integrity;
      MyCubeBlockDefinition.Component[] components = blockDef.Components;
      for (int index = 0; index < components.Length; ++index)
      {
        float num2 = (float) (components[index].Definition.MaxIntegrity * components[index].Count);
        if ((double) num1 >= (double) num2)
        {
          num1 -= num2;
          if ((double) num1 <= 1.52590218931437E-05)
          {
            topGroupIndex = index;
            topComponentIndex = components[index].Count - 1;
            break;
          }
        }
        else
        {
          int num3 = (int) ((double) num1 / (double) components[index].Definition.MaxIntegrity);
          if ((double) num1 - (double) (components[index].Definition.MaxIntegrity * num3) < 7.62951094657183E-06 && num3 != 0)
          {
            topGroupIndex = index;
            topComponentIndex = num3 - 1;
            break;
          }
          topGroupIndex = index;
          topComponentIndex = num3;
          break;
        }
      }
    }

    public void UpdateBuildIntegrityUp()
    {
      if ((double) this.BuildIntegrity >= (double) this.Integrity)
        return;
      this.BuildIntegrity = this.Integrity;
      ++this.LastChangeStamp;
    }

    public void UpdateBuildIntegrityDown(float ratio)
    {
      if ((double) this.BuildIntegrity <= (double) this.Integrity * (double) ratio)
        return;
      this.BuildIntegrity = this.Integrity * ratio;
      ++this.LastChangeStamp;
    }

    public bool CanContinueBuild(MyInventoryBase inventory, MyConstructionStockpile stockpile)
    {
      if (this.IsFullIntegrity)
        return false;
      if ((double) this.GetTopComponentIntegrity() < (double) this.m_blockDefinition.Components[(int) this.m_topGroupIndex].Definition.MaxIntegrity)
        return true;
      int topGroupIndex = (int) this.m_topGroupIndex;
      if ((int) this.m_topComponentIndex == this.m_blockDefinition.Components[topGroupIndex].Count - 1)
        ++topGroupIndex;
      MyComponentDefinition definition = this.m_blockDefinition.Components[topGroupIndex].Definition;
      return stockpile != null && stockpile.GetItemAmount(definition.Id) > 0 || inventory != null && MyCubeBuilder.BuildComponent.GetItemAmountCombined(inventory, definition.Id) > (MyFixedPoint) 0;
    }

    public void GetMissingInfo(out int groupIndex, out int componentCount)
    {
      if (this.IsFullIntegrity)
      {
        groupIndex = 0;
        componentCount = 0;
      }
      else if ((double) this.GetTopComponentIntegrity() < (double) this.m_blockDefinition.Components[(int) this.m_topGroupIndex].Definition.MaxIntegrity)
      {
        groupIndex = 0;
        componentCount = 0;
      }
      else
      {
        int num = (int) this.m_topComponentIndex + 1;
        groupIndex = (int) this.m_topGroupIndex;
        if (num == this.m_blockDefinition.Components[groupIndex].Count)
        {
          ++groupIndex;
          num = 0;
        }
        componentCount = this.m_blockDefinition.Components[groupIndex].Count - num;
      }
    }

    public void DestroyCompletely()
    {
      this.BuildIntegrity = 0.0f;
      this.Integrity = 0.0f;
      this.UpdateIndices();
    }

    private bool CheckOrMountFirstComponent(MyConstructionStockpile stockpile = null)
    {
      if ((double) this.Integrity > 7.62951094657183E-06)
        return true;
      MyComponentDefinition definition = this.m_blockDefinition.Components[0].Definition;
      if (stockpile != null && !stockpile.RemoveItems(1, definition.Id))
        return false;
      this.Integrity = 1.525902E-05f;
      this.UpdateBuildIntegrityUp();
      return true;
    }

    public void GetMissingComponents(
      Dictionary<string, int> addToDictionary,
      MyConstructionStockpile availableItems = null)
    {
      int topGroupIndex = (int) this.m_topGroupIndex;
      MyCubeBlockDefinition.Component component1 = this.m_blockDefinition.Components[topGroupIndex];
      int num = (int) this.m_topComponentIndex + 1;
      if (this.IsFullyDismounted)
        --num;
      if (num < component1.Count)
      {
        string subtypeName = component1.Definition.Id.SubtypeName;
        if (addToDictionary.ContainsKey(subtypeName))
          addToDictionary[subtypeName] += component1.Count - num;
        else
          addToDictionary[subtypeName] = component1.Count - num;
      }
      for (int index = topGroupIndex + 1; index < this.m_blockDefinition.Components.Length; ++index)
      {
        MyCubeBlockDefinition.Component component2 = this.m_blockDefinition.Components[index];
        string subtypeName = component2.Definition.Id.SubtypeName;
        if (addToDictionary.ContainsKey(subtypeName))
          addToDictionary[subtypeName] += component2.Count;
        else
          addToDictionary[subtypeName] = component2.Count;
      }
      if (availableItems == null)
        return;
      for (int index = 0; index < addToDictionary.Keys.Count; ++index)
      {
        string str = addToDictionary.Keys.ElementAt<string>(index);
        addToDictionary[str] -= availableItems.GetItemAmount(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_Component), str));
        if (addToDictionary[str] <= 0)
        {
          addToDictionary.Remove(str);
          --index;
        }
      }
    }

    public static void GetMountedComponents(
      MyComponentList addToList,
      MyObjectBuilder_CubeBlock block)
    {
      int topGroupIndex = 0;
      int topComponentIndex = 0;
      MyCubeBlockDefinition blockDefinition = (MyCubeBlockDefinition) null;
      MyDefinitionManager.Static.TryGetCubeBlockDefinition(block.GetId(), out blockDefinition);
      if (blockDefinition == null || block == null)
        return;
      float integrity = block.IntegrityPercent * blockDefinition.MaxIntegrity;
      MyComponentStack.CalculateIndicesInternal(integrity, blockDefinition, ref topGroupIndex, ref topComponentIndex);
      if (topGroupIndex >= blockDefinition.Components.Length || topComponentIndex >= blockDefinition.Components[topGroupIndex].Count)
        return;
      int num = topComponentIndex;
      if ((double) integrity >= 1.52590218931437E-05)
        ++num;
      for (int index = 0; index < topGroupIndex; ++index)
      {
        MyCubeBlockDefinition.Component component = blockDefinition.Components[index];
        addToList.AddMaterial(component.Definition.Id, component.Count, component.Count, false);
      }
      MyDefinitionId id = blockDefinition.Components[topGroupIndex].Definition.Id;
      addToList.AddMaterial(id, num, num, false);
    }

    public void IncreaseMountLevel(float mountAmount, MyConstructionStockpile stockpile = null)
    {
      int num = this.IsFunctional ? 1 : 0;
      this.IncreaseMountLevelInternal(mountAmount, stockpile);
      ++this.LastChangeStamp;
    }

    private void IncreaseMountLevelInternal(float mountAmount, MyConstructionStockpile stockpile = null)
    {
      if (!this.CheckOrMountFirstComponent(stockpile))
        return;
      float num1 = this.GetTopComponentIntegrity();
      float maxIntegrity = (float) this.m_blockDefinition.Components[(int) this.m_topGroupIndex].Definition.MaxIntegrity;
      int topGroupIndex = (int) this.m_topGroupIndex;
      int newTopComponentIndex = (int) this.m_topComponentIndex;
      while ((double) mountAmount > 0.0)
      {
        float num2 = maxIntegrity - num1;
        if ((double) mountAmount < (double) num2)
        {
          this.Integrity += mountAmount;
          this.UpdateBuildIntegrityUp();
          break;
        }
        this.Integrity += num2 + 1.525902E-05f;
        mountAmount -= num2 + 1.525902E-05f;
        ++newTopComponentIndex;
        if (newTopComponentIndex >= this.m_blockDefinition.Components[(int) this.m_topGroupIndex].Count)
        {
          ++topGroupIndex;
          newTopComponentIndex = 0;
        }
        if (topGroupIndex == this.m_blockDefinition.Components.Length)
        {
          this.Integrity = this.MaxIntegrity;
          this.UpdateBuildIntegrityUp();
          break;
        }
        MyComponentDefinition definition = this.m_blockDefinition.Components[topGroupIndex].Definition;
        if (stockpile != null && !stockpile.RemoveItems(1, definition.Id))
        {
          this.Integrity -= 1.525902E-05f;
          this.UpdateBuildIntegrityUp();
          break;
        }
        this.UpdateBuildIntegrityUp();
        this.SetTopIndex(topGroupIndex, newTopComponentIndex);
        num1 = 1.525902E-05f;
        maxIntegrity = (float) this.m_blockDefinition.Components[topGroupIndex].Definition.MaxIntegrity;
      }
    }

    public void DecreaseMountLevel(
      float unmountAmount,
      MyConstructionStockpile outputStockpile = null,
      bool useDefaultDeconstructEfficiency = false)
    {
      float ratio = this.BuildIntegrity / this.Integrity;
      this.UnmountInternal(unmountAmount, outputStockpile, useDefaultDeconstructEfficiency: useDefaultDeconstructEfficiency);
      this.UpdateBuildIntegrityDown(ratio);
      ++this.LastChangeStamp;
    }

    public void ApplyDamage(float damage, MyConstructionStockpile outputStockpile = null)
    {
      this.UnmountInternal(damage, outputStockpile, true);
      this.UpdateBuildIntegrityDown(this.BuildIntegrity / this.Integrity);
      ++this.LastChangeStamp;
    }

    private float GetDeconstructionEfficiency(int groupIndex, bool useDefault) => !useDefault ? this.m_blockDefinition.Components[groupIndex].Definition.DeconstructionEfficiency : 1f;

    private void UnmountInternal(
      float unmountAmount,
      MyConstructionStockpile outputStockpile = null,
      bool damageItems = false,
      bool useDefaultDeconstructEfficiency = false)
    {
      float num1 = this.GetTopComponentIntegrity();
      int topGroupIndex = (int) this.m_topGroupIndex;
      int newTopComponentIndex = (int) this.m_topComponentIndex;
      MyObjectBuilder_Ore scrapBuilder = MyFloatingObject.ScrapBuilder;
      while ((double) unmountAmount * (double) this.GetDeconstructionEfficiency(topGroupIndex, damageItems | useDefaultDeconstructEfficiency) >= (double) num1)
      {
        this.Integrity -= num1;
        unmountAmount -= num1;
        if (outputStockpile != null && MySession.Static.SurvivalMode)
        {
          bool flag = damageItems && MyFakes.ENABLE_DAMAGED_COMPONENTS;
          if (!damageItems || flag && (double) MyRandom.Instance.NextFloat() <= (double) this.m_blockDefinition.Components[topGroupIndex].Definition.DropProbability)
          {
            MyObjectBuilder_PhysicalObject newObject = (MyObjectBuilder_PhysicalObject) MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) this.m_blockDefinition.Components[topGroupIndex].DeconstructItem.Id);
            if (flag)
              newObject.Flags |= MyItemFlags.Damaged;
            if ((double) this.Integrity > 0.0 || this.m_yieldLastComponent)
              outputStockpile.AddItems(1, newObject);
          }
          MyComponentDefinition definition = this.m_blockDefinition.Components[topGroupIndex].Definition;
          if (MyFakes.ENABLE_SCRAP & damageItems && (double) MyRandom.Instance.NextFloat() < (double) definition.DropProbability && ((double) this.Integrity > 0.0 || this.m_yieldLastComponent))
            outputStockpile.AddItems((int) (0.800000011920929 * (double) definition.Mass), (MyObjectBuilder_PhysicalObject) scrapBuilder);
        }
        --newTopComponentIndex;
        if (newTopComponentIndex < 0)
        {
          --topGroupIndex;
          if (topGroupIndex < 0)
          {
            this.SetTopIndex(0, 0);
            this.Integrity = 0.0f;
            return;
          }
          newTopComponentIndex = this.m_blockDefinition.Components[topGroupIndex].Count - 1;
        }
        num1 = (float) this.m_blockDefinition.Components[topGroupIndex].Definition.MaxIntegrity;
        this.SetTopIndex(topGroupIndex, newTopComponentIndex);
      }
      this.Integrity -= unmountAmount * this.GetDeconstructionEfficiency(topGroupIndex, damageItems | useDefaultDeconstructEfficiency);
      float num2 = num1 - unmountAmount * this.GetDeconstructionEfficiency(topGroupIndex, damageItems | useDefaultDeconstructEfficiency);
      if ((double) num2 >= 1.52590218931437E-05)
        return;
      this.Integrity += 1.525902E-05f - num2;
    }

    internal void SetIntegrity(float buildIntegrity, float integrity)
    {
      this.Integrity = integrity;
      this.BuildIntegrity = buildIntegrity;
      this.UpdateIndices();
      ++this.LastChangeStamp;
    }

    public int GroupCount => this.m_blockDefinition.Components.Length;

    public MyComponentStack.GroupInfo GetGroupInfo(int index)
    {
      MyCubeBlockDefinition.Component component = this.m_blockDefinition.Components[index];
      MyComponentStack.GroupInfo groupInfo = new MyComponentStack.GroupInfo()
      {
        Component = component.Definition,
        TotalCount = component.Count,
        MountedCount = 0,
        AvailableCount = 0,
        Integrity = 0.0f,
        MaxIntegrity = (float) (component.Count * component.Definition.MaxIntegrity)
      };
      if (index < (int) this.m_topGroupIndex)
      {
        groupInfo.MountedCount = component.Count;
        groupInfo.Integrity = (float) (component.Count * component.Definition.MaxIntegrity);
      }
      else if (index == (int) this.m_topGroupIndex)
      {
        groupInfo.MountedCount = (int) this.m_topComponentIndex + 1;
        groupInfo.Integrity = this.GetTopComponentIntegrity() + (float) ((int) this.m_topComponentIndex * component.Definition.MaxIntegrity);
      }
      return groupInfo;
    }

    public void DisableLastComponentYield() => this.m_yieldLastComponent = false;

    public struct GroupInfo
    {
      public int MountedCount;
      public int TotalCount;
      public int AvailableCount;
      public float Integrity;
      public float MaxIntegrity;
      public MyComponentDefinition Component;
    }
  }
}
