// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyMotorSuspensionDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_MotorSuspensionDefinition), null)]
  public class MyMotorSuspensionDefinition : MyMotorStatorDefinition
  {
    public float MaxSteer;
    public float SteeringSpeed;
    public float PropulsionForce;
    public float MinHeight;
    public float MaxHeight;
    public float AxleFriction;
    public float AirShockMinSpeed;
    public float AirShockMaxSpeed;
    public int AirShockActivationDelay;
    public float RequiredIdlePowerInput;
    public MyDefinitionId? SoundDefinitionId;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_MotorSuspensionDefinition suspensionDefinition = (MyObjectBuilder_MotorSuspensionDefinition) builder;
      this.MaxSteer = suspensionDefinition.MaxSteer;
      this.SteeringSpeed = suspensionDefinition.SteeringSpeed;
      this.PropulsionForce = suspensionDefinition.PropulsionForce;
      this.MinHeight = suspensionDefinition.MinHeight;
      this.MaxHeight = suspensionDefinition.MaxHeight;
      this.AxleFriction = suspensionDefinition.AxleFriction;
      this.AirShockMinSpeed = suspensionDefinition.AirShockMinSpeed;
      this.AirShockMaxSpeed = suspensionDefinition.AirShockMaxSpeed;
      this.AirShockActivationDelay = suspensionDefinition.AirShockActivationDelay;
      this.RequiredIdlePowerInput = (double) suspensionDefinition.RequiredIdlePowerInput != 0.0 ? suspensionDefinition.RequiredIdlePowerInput : suspensionDefinition.RequiredPowerInput;
      MyDefinitionId definitionId;
      if (suspensionDefinition.SoundDefinitionId != null && MyDefinitionId.TryParse(suspensionDefinition.SoundDefinitionId.DefinitionTypeName, suspensionDefinition.SoundDefinitionId.DefinitionSubtypeName, out definitionId))
        this.SoundDefinitionId = new MyDefinitionId?(definitionId);
      else
        this.SoundDefinitionId = new MyDefinitionId?();
    }

    private class Sandbox_Definitions_MyMotorSuspensionDefinition\u003C\u003EActor : IActivator, IActivator<MyMotorSuspensionDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyMotorSuspensionDefinition();

      MyMotorSuspensionDefinition IActivator<MyMotorSuspensionDefinition>.CreateInstance() => new MyMotorSuspensionDefinition();
    }
  }
}
