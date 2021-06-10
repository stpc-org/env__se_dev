// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyBeaconDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_BeaconDefinition), null)]
  public class MyBeaconDefinition : MyCubeBlockDefinition
  {
    public string ResourceSinkGroup;
    public string Flare;
    public float MaxBroadcastRadius;
    public float MaxBroadcastPowerDrainkW;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_BeaconDefinition beaconDefinition = (MyObjectBuilder_BeaconDefinition) builder;
      this.ResourceSinkGroup = beaconDefinition.ResourceSinkGroup;
      this.MaxBroadcastRadius = beaconDefinition.MaxBroadcastRadius;
      this.Flare = beaconDefinition.Flare;
      this.MaxBroadcastPowerDrainkW = beaconDefinition.MaxBroadcastPowerDrainkW;
    }

    private class Sandbox_Definitions_MyBeaconDefinition\u003C\u003EActor : IActivator, IActivator<MyBeaconDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyBeaconDefinition();

      MyBeaconDefinition IActivator<MyBeaconDefinition>.CreateInstance() => new MyBeaconDefinition();
    }
  }
}
