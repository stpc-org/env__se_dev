// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyShipSoundSystemDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ShipSoundSystemDefinition), null)]
  public class MyShipSoundSystemDefinition : MyDefinitionBase
  {
    public float MaxUpdateRange = 2000f;
    public float MaxUpdateRange_sq = 4000000f;
    public float WheelsCallbackRangeCreate_sq = 250000f;
    public float WheelsCallbackRangeRemove_sq = 562500f;
    public float FullSpeed = 96f;
    public float FullSpeed_sq = 9216f;
    public float SpeedThreshold1 = 32f;
    public float SpeedThreshold2 = 64f;
    public float LargeShipDetectionRadius = 15f;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_ShipSoundSystemDefinition systemDefinition = builder as MyObjectBuilder_ShipSoundSystemDefinition;
      this.MaxUpdateRange = systemDefinition.MaxUpdateRange;
      this.FullSpeed = systemDefinition.FullSpeed;
      this.FullSpeed_sq = systemDefinition.FullSpeed * systemDefinition.FullSpeed;
      this.SpeedThreshold1 = systemDefinition.FullSpeed * 0.33f;
      this.SpeedThreshold2 = systemDefinition.FullSpeed * 0.66f;
      this.LargeShipDetectionRadius = systemDefinition.LargeShipDetectionRadius;
      this.MaxUpdateRange_sq = systemDefinition.MaxUpdateRange * systemDefinition.MaxUpdateRange;
      this.WheelsCallbackRangeCreate_sq = systemDefinition.WheelStartUpdateRange * systemDefinition.WheelStartUpdateRange;
      this.WheelsCallbackRangeRemove_sq = systemDefinition.WheelStopUpdateRange * systemDefinition.WheelStopUpdateRange;
    }

    private class Sandbox_Definitions_MyShipSoundSystemDefinition\u003C\u003EActor : IActivator, IActivator<MyShipSoundSystemDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyShipSoundSystemDefinition();

      MyShipSoundSystemDefinition IActivator<MyShipSoundSystemDefinition>.CreateInstance() => new MyShipSoundSystemDefinition();
    }
  }
}
