// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyPirateAntennaDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_PirateAntennaDefinition), null)]
  public class MyPirateAntennaDefinition : MyDefinitionBase
  {
    public string Name;
    public float SpawnDistance;
    public int SpawnTimeMs;
    public int FirstSpawnTimeMs;
    public int MaxDrones;
    public MyDiscreteSampler<MySpawnGroupDefinition> SpawnGroupSampler;
    private List<string> m_spawnGroups;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_PirateAntennaDefinition antennaDefinition = builder as MyObjectBuilder_PirateAntennaDefinition;
      this.Name = antennaDefinition.Name;
      this.SpawnDistance = antennaDefinition.SpawnDistance;
      this.SpawnTimeMs = antennaDefinition.SpawnTimeMs;
      this.FirstSpawnTimeMs = antennaDefinition.FirstSpawnTimeMs;
      this.MaxDrones = antennaDefinition.MaxDrones;
      this.m_spawnGroups = new List<string>();
      foreach (string spawnGroup in antennaDefinition.SpawnGroups)
        this.m_spawnGroups.Add(spawnGroup);
    }

    public new void Postprocess()
    {
      List<MySpawnGroupDefinition> values = new List<MySpawnGroupDefinition>();
      List<float> floatList = new List<float>();
      foreach (string spawnGroup in this.m_spawnGroups)
      {
        MySpawnGroupDefinition definition = (MySpawnGroupDefinition) null;
        MyDefinitionManager.Static.TryGetDefinition<MySpawnGroupDefinition>(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_SpawnGroupDefinition), spawnGroup), out definition);
        if (definition != null)
        {
          values.Add(definition);
          floatList.Add(definition.Frequency);
        }
      }
      this.m_spawnGroups = (List<string>) null;
      if (floatList.Count == 0)
        return;
      this.SpawnGroupSampler = new MyDiscreteSampler<MySpawnGroupDefinition>(values, (IEnumerable<float>) floatList);
    }

    private class Sandbox_Definitions_MyPirateAntennaDefinition\u003C\u003EActor : IActivator, IActivator<MyPirateAntennaDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyPirateAntennaDefinition();

      MyPirateAntennaDefinition IActivator<MyPirateAntennaDefinition>.CreateInstance() => new MyPirateAntennaDefinition();
    }
  }
}
