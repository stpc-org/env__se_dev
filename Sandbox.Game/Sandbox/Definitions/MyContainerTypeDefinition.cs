// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyContainerTypeDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Library.Utils;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ContainerTypeDefinition), null)]
  public class MyContainerTypeDefinition : MyDefinitionBase
  {
    public int CountMin;
    public int CountMax;
    public float ItemsCumulativeFrequency;
    private float m_tempCumulativeFreq;
    public MyContainerTypeDefinition.ContainerTypeItem[] Items;
    private bool[] m_itemSelection;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_ContainerTypeDefinition containerTypeDefinition = builder as MyObjectBuilder_ContainerTypeDefinition;
      this.CountMin = containerTypeDefinition.CountMin;
      this.CountMax = containerTypeDefinition.CountMax;
      this.ItemsCumulativeFrequency = 0.0f;
      int index = 0;
      this.Items = new MyContainerTypeDefinition.ContainerTypeItem[containerTypeDefinition.Items.Length];
      this.m_itemSelection = new bool[containerTypeDefinition.Items.Length];
      foreach (MyObjectBuilder_ContainerTypeDefinition.ContainerTypeItem containerTypeItem1 in containerTypeDefinition.Items)
      {
        MyContainerTypeDefinition.ContainerTypeItem containerTypeItem2 = new MyContainerTypeDefinition.ContainerTypeItem();
        containerTypeItem2.AmountMax = MyFixedPoint.DeserializeStringSafe(containerTypeItem1.AmountMax);
        containerTypeItem2.AmountMin = MyFixedPoint.DeserializeStringSafe(containerTypeItem1.AmountMin);
        containerTypeItem2.Frequency = Math.Max(containerTypeItem1.Frequency, 0.0f);
        containerTypeItem2.DefinitionId = (MyDefinitionId) containerTypeItem1.Id;
        this.ItemsCumulativeFrequency += containerTypeItem2.Frequency;
        this.Items[index] = containerTypeItem2;
        this.m_itemSelection[index] = false;
        ++index;
      }
      this.m_tempCumulativeFreq = this.ItemsCumulativeFrequency;
    }

    public void DeselectAll()
    {
      for (int index = 0; index < this.Items.Length; ++index)
        this.m_itemSelection[index] = false;
      this.m_tempCumulativeFreq = this.ItemsCumulativeFrequency;
    }

    public MyContainerTypeDefinition.ContainerTypeItem SelectNextRandomItem()
    {
      float num = MyRandom.Instance.NextFloat(0.0f, this.m_tempCumulativeFreq);
      int index = 0;
      while (index < this.Items.Length - 1)
      {
        if (this.m_itemSelection[index])
        {
          ++index;
        }
        else
        {
          num -= this.Items[index].Frequency;
          if ((double) num >= 0.0)
            ++index;
          else
            break;
        }
      }
      this.m_tempCumulativeFreq -= this.Items[index].Frequency;
      this.m_itemSelection[index] = true;
      return this.Items[index];
    }

    public struct ContainerTypeItem
    {
      public MyFixedPoint AmountMin;
      public MyFixedPoint AmountMax;
      public float Frequency;
      public MyDefinitionId DefinitionId;
      public bool HasIntegralAmount;
    }

    private class Sandbox_Definitions_MyContainerTypeDefinition\u003C\u003EActor : IActivator, IActivator<MyContainerTypeDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyContainerTypeDefinition();

      MyContainerTypeDefinition IActivator<MyContainerTypeDefinition>.CreateInstance() => new MyContainerTypeDefinition();
    }
  }
}
