// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyRadioAntennaDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_RadioAntennaDefinition), null)]
  public class MyRadioAntennaDefinition : MyCubeBlockDefinition
  {
    public MyStringHash ResourceSinkGroup;
    public float MaxBroadcastRadius;
    public float LightningRodRadiusLarge;
    public float LightningRodRadiusSmall;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_RadioAntennaDefinition antennaDefinition = (MyObjectBuilder_RadioAntennaDefinition) builder;
      this.ResourceSinkGroup = MyStringHash.GetOrCompute(antennaDefinition.ResourceSinkGroup);
      this.MaxBroadcastRadius = antennaDefinition.MaxBroadcastRadius;
      this.LightningRodRadiusLarge = antennaDefinition.LightningRodRadiusLarge;
      this.LightningRodRadiusSmall = antennaDefinition.LightningRodRadiusSmall;
    }

    private class Sandbox_Definitions_MyRadioAntennaDefinition\u003C\u003EActor : IActivator, IActivator<MyRadioAntennaDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyRadioAntennaDefinition();

      MyRadioAntennaDefinition IActivator<MyRadioAntennaDefinition>.CreateInstance() => new MyRadioAntennaDefinition();
    }
  }
}
