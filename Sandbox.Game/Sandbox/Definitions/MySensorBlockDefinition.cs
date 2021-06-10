// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MySensorBlockDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using System;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_SensorBlockDefinition), null)]
  public class MySensorBlockDefinition : MyCubeBlockDefinition
  {
    public MyStringHash ResourceSinkGroup;
    public float RequiredPowerInput;
    public float MaxRange;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_SensorBlockDefinition sensorBlockDefinition = builder as MyObjectBuilder_SensorBlockDefinition;
      this.ResourceSinkGroup = MyStringHash.GetOrCompute(sensorBlockDefinition.ResourceSinkGroup);
      this.RequiredPowerInput = sensorBlockDefinition.RequiredPowerInput;
      this.MaxRange = Math.Max(sensorBlockDefinition.MaxRange, 1f);
    }

    private class Sandbox_Definitions_MySensorBlockDefinition\u003C\u003EActor : IActivator, IActivator<MySensorBlockDefinition>
    {
      object IActivator.CreateInstance() => (object) new MySensorBlockDefinition();

      MySensorBlockDefinition IActivator<MySensorBlockDefinition>.CreateInstance() => new MySensorBlockDefinition();
    }
  }
}
