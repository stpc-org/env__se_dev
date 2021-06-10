// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyHudSinkGroupInfo
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Localization;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Utils;

namespace Sandbox.Game.Gui
{
  public class MyHudSinkGroupInfo
  {
    private bool m_needsRefresh = true;
    private float[] m_missingPowerByGroup;
    private MyStringId[] m_groupNames;
    private float m_missingTotal;
    public int GroupCount;
    private int m_workingGroupCount;
    private bool m_visible;
    private MyHudNameValueData m_data;

    public int WorkingGroupCount
    {
      get => this.m_workingGroupCount;
      set
      {
        if (this.m_workingGroupCount == value)
          return;
        this.m_workingGroupCount = value;
        this.m_needsRefresh = true;
      }
    }

    public bool Visible
    {
      get => this.m_visible && this.WorkingGroupCount != this.GroupCount;
      set => this.m_visible = value;
    }

    public MyHudNameValueData Data
    {
      get
      {
        if (this.m_needsRefresh)
          this.Refresh();
        return this.m_data;
      }
    }

    public MyHudSinkGroupInfo() => this.Reload();

    public void Reload()
    {
      MyResourceDistributorComponent.InitializeMappings();
      if (MyResourceDistributorComponent.SinkGroupPrioritiesTotal != -1 && (this.m_groupNames == null || this.m_groupNames.Length < MyResourceDistributorComponent.SinkGroupPrioritiesTotal))
      {
        this.GroupCount = MyResourceDistributorComponent.SinkGroupPrioritiesTotal;
        this.WorkingGroupCount = this.GroupCount;
        this.m_groupNames = new MyStringId[this.GroupCount];
        this.m_missingPowerByGroup = new float[this.GroupCount];
        this.m_data = new MyHudNameValueData(this.GroupCount + 1, showBackgroundFog: true);
      }
      if (this.m_groupNames == null)
        return;
      ListReader<MyResourceDistributionGroupDefinition> definitionsOfType = MyDefinitionManager.Static.GetDefinitionsOfType<MyResourceDistributionGroupDefinition>();
      DictionaryReader<MyStringHash, int> subtypesToPriority = MyResourceDistributorComponent.SinkSubtypesToPriority;
      foreach (MyResourceDistributionGroupDefinition distributionGroupDefinition in definitionsOfType)
      {
        int index;
        if (!distributionGroupDefinition.IsSource && subtypesToPriority.TryGetValue(distributionGroupDefinition.Id.SubtypeId, out index) && index < this.GroupCount)
          this.m_groupNames[index] = MyStringId.GetOrCompute(distributionGroupDefinition.Id.SubtypeName);
      }
      this.Data[this.GroupCount].NameFont = "Red";
      this.Data[this.GroupCount].ValueFont = "Red";
    }

    internal void SetGroupDeficit(int groupIndex, float missingPower)
    {
      if (this.m_missingPowerByGroup == null)
        this.Reload();
      this.m_missingTotal += missingPower - this.m_missingPowerByGroup[groupIndex];
      this.m_missingPowerByGroup[groupIndex] = missingPower;
      this.m_needsRefresh = true;
    }

    private void Refresh()
    {
      this.m_needsRefresh = false;
      MyHudNameValueData data1 = this.Data;
      for (int i = 0; i < data1.Count - 1; ++i)
        data1[i].Name.Clear().AppendStringBuilder(MyTexts.Get(this.m_groupNames[i]));
      data1[this.GroupCount].Name.Clear().AppendStringBuilder(MyTexts.Get(MySpaceTexts.HudEnergyMissingTotal));
      MyHudNameValueData.Data data2 = data1[this.GroupCount];
      data2.Value.Clear();
      MyValueFormatter.AppendWorkInBestUnit(-this.m_missingTotal, data2.Value);
      for (int i = 0; i < this.GroupCount; ++i)
      {
        MyHudNameValueData.Data data3 = data1[i];
        data3.NameFont = i >= this.m_workingGroupCount ? (data3.ValueFont = "Red") : (data3.ValueFont = (string) null);
        data3.Value.Clear();
        MyValueFormatter.AppendWorkInBestUnit(-this.m_missingPowerByGroup[i], data3.Value);
      }
    }
  }
}
