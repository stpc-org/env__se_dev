// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyEnvironmentItemsDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_EnvironmentItemsDefinition), null)]
  public class MyEnvironmentItemsDefinition : MyDefinitionBase
  {
    private HashSet<MyStringHash> m_itemDefinitions;
    private List<MyStringHash> m_definitionList;
    private List<float> Frequencies;
    private float[] Intervals;
    private MyObjectBuilderType m_itemDefinitionType = MyObjectBuilderType.Invalid;

    public MyObjectBuilderType ItemDefinitionType => this.m_itemDefinitionType;

    public int Channel { get; private set; }

    public float MaxViewDistance { get; private set; }

    public float SectorSize { get; private set; }

    public float ItemSize { get; private set; }

    public MyStringHash Material { get; private set; }

    public int ItemDefinitionCount => this.m_definitionList.Count;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_EnvironmentItemsDefinition environmentItemsDefinition = builder as MyObjectBuilder_EnvironmentItemsDefinition;
      this.m_itemDefinitions = new HashSet<MyStringHash>((IEqualityComparer<MyStringHash>) MyStringHash.Comparer);
      this.m_definitionList = new List<MyStringHash>();
      object[] customAttributes = ((Type) builder.Id.TypeId).GetCustomAttributes(typeof (MyEnvironmentItemsAttribute), false);
      this.m_itemDefinitionType = customAttributes.Length != 1 ? (MyObjectBuilderType) typeof (MyObjectBuilder_EnvironmentItemDefinition) : (MyObjectBuilderType) (customAttributes[0] as MyEnvironmentItemsAttribute).ItemDefinitionType;
      this.Channel = environmentItemsDefinition.Channel;
      this.MaxViewDistance = environmentItemsDefinition.MaxViewDistance;
      this.SectorSize = environmentItemsDefinition.SectorSize;
      this.ItemSize = environmentItemsDefinition.ItemSize;
      this.Material = MyStringHash.GetOrCompute(environmentItemsDefinition.PhysicalMaterial);
      this.Frequencies = new List<float>();
    }

    public void AddItemDefinition(MyStringHash definition, float frequency, bool recompute = true)
    {
      if (this.m_itemDefinitions.Contains(definition))
        return;
      this.m_itemDefinitions.Add(definition);
      this.m_definitionList.Add(definition);
      this.Frequencies.Add(frequency);
      if (!recompute)
        return;
      this.RecomputeFrequencies();
    }

    public void RecomputeFrequencies()
    {
      if (this.m_definitionList.Count == 0)
      {
        this.Intervals = (float[]) null;
      }
      else
      {
        this.Intervals = new float[this.m_definitionList.Count - 1];
        float num1 = 0.0f;
        foreach (float frequency in this.Frequencies)
          num1 += frequency;
        float num2 = 0.0f;
        for (int index = 0; index < this.Intervals.Length; ++index)
        {
          num2 += this.Frequencies[index];
          this.Intervals[index] = num2 / num1;
        }
      }
    }

    public MyEnvironmentItemDefinition GetItemDefinition(
      MyStringHash subtypeId)
    {
      MyEnvironmentItemDefinition definition = (MyEnvironmentItemDefinition) null;
      MyDefinitionManager.Static.TryGetDefinition<MyEnvironmentItemDefinition>(new MyDefinitionId(this.m_itemDefinitionType, subtypeId), out definition);
      return definition;
    }

    public MyEnvironmentItemDefinition GetItemDefinition(int index) => index < 0 || index >= this.m_definitionList.Count ? (MyEnvironmentItemDefinition) null : this.GetItemDefinition(this.m_definitionList[index]);

    public MyEnvironmentItemDefinition GetRandomItemDefinition() => this.m_definitionList.Count == 0 ? (MyEnvironmentItemDefinition) null : this.GetItemDefinition(this.m_definitionList[this.Intervals.BinaryIntervalSearch<float>((float) MyRandom.Instance.Next(0, 65536) / 65536f)]);

    public MyEnvironmentItemDefinition GetRandomItemDefinition(
      MyRandom instance)
    {
      return this.m_definitionList.Count == 0 ? (MyEnvironmentItemDefinition) null : this.GetItemDefinition(this.m_definitionList[this.Intervals.BinaryIntervalSearch<float>((float) instance.Next(0, 65536) / 65536f)]);
    }

    public bool ContainsItemDefinition(MyStringHash subtypeId) => this.m_itemDefinitions.Contains(subtypeId);

    public bool ContainsItemDefinition(MyDefinitionId definitionId) => definitionId.TypeId == this.m_itemDefinitionType && this.m_itemDefinitions.Contains(definitionId.SubtypeId);

    public bool ContainsItemDefinition(MyEnvironmentItemDefinition itemDefinition) => this.ContainsItemDefinition(itemDefinition.Id);

    private class Sandbox_Definitions_MyEnvironmentItemsDefinition\u003C\u003EActor : IActivator, IActivator<MyEnvironmentItemsDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyEnvironmentItemsDefinition();

      MyEnvironmentItemsDefinition IActivator<MyEnvironmentItemsDefinition>.CreateInstance() => new MyEnvironmentItemsDefinition();
    }
  }
}
