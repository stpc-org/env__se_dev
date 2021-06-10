// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyStationsListDefinition
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
  [MyDefinitionType(typeof (MyObjectBuilder_StationsListDefinition), null)]
  public class MyStationsListDefinition : MyDefinitionBase
  {
    public List<MyStringId> StationNames { get; set; }

    public int SpawnDistance { get; set; }

    public MyDefinitionId? GeneratedItemsContainerType { get; set; }

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_StationsListDefinition stationsListDefinition = (MyObjectBuilder_StationsListDefinition) builder;
      if (stationsListDefinition.StationNames != null)
      {
        this.StationNames = new List<MyStringId>(stationsListDefinition.StationNames.Count);
        foreach (string stationName in stationsListDefinition.StationNames)
          this.StationNames.Add(MyStringId.GetOrCompute(stationName));
      }
      else
        this.StationNames = new List<MyStringId>();
      this.SpawnDistance = stationsListDefinition.SpawnDistance;
      SerializableDefinitionId? itemsContainerType = stationsListDefinition.GeneratedItemsContainerType;
      this.GeneratedItemsContainerType = itemsContainerType.HasValue ? new MyDefinitionId?((MyDefinitionId) itemsContainerType.GetValueOrDefault()) : new MyDefinitionId?();
    }

    private class Sandbox_Definitions_MyStationsListDefinition\u003C\u003EActor : IActivator, IActivator<MyStationsListDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyStationsListDefinition();

      MyStationsListDefinition IActivator<MyStationsListDefinition>.CreateInstance() => new MyStationsListDefinition();
    }
  }
}
