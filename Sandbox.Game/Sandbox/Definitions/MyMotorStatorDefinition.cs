// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyMotorStatorDefinition
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
  [MyDefinitionType(typeof (MyObjectBuilder_MotorStatorDefinition), null)]
  public class MyMotorStatorDefinition : MyMechanicalConnectionBlockBaseDefinition
  {
    public MyStringHash ResourceSinkGroup;
    public float RequiredPowerInput;
    public float MaxForceMagnitude;
    public float RotorDisplacementMin;
    public float RotorDisplacementMax;
    public float RotorDisplacementMinSmall;
    public float RotorDisplacementMaxSmall;
    public float RotorDisplacementInModel;
    public float UnsafeTorqueThreshold;
    public float? MinAngleDeg;
    public float? MaxAngleDeg;
    public MyRotorType RotorType;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_MotorStatorDefinition statorDefinition = (MyObjectBuilder_MotorStatorDefinition) builder;
      this.ResourceSinkGroup = MyStringHash.GetOrCompute(statorDefinition.ResourceSinkGroup);
      this.RequiredPowerInput = statorDefinition.RequiredPowerInput;
      this.MaxForceMagnitude = statorDefinition.MaxForceMagnitude;
      this.RotorDisplacementMin = statorDefinition.RotorDisplacementMin;
      this.RotorDisplacementMax = statorDefinition.RotorDisplacementMax;
      this.RotorDisplacementMinSmall = statorDefinition.RotorDisplacementMinSmall;
      this.RotorDisplacementMaxSmall = statorDefinition.RotorDisplacementMaxSmall;
      this.RotorDisplacementInModel = statorDefinition.RotorDisplacementInModel;
      this.UnsafeTorqueThreshold = statorDefinition.DangerousTorqueThreshold;
      this.MinAngleDeg = statorDefinition.MinAngleDeg;
      this.MaxAngleDeg = statorDefinition.MaxAngleDeg;
      this.RotorType = statorDefinition.RotorType;
    }

    private class Sandbox_Definitions_MyMotorStatorDefinition\u003C\u003EActor : IActivator, IActivator<MyMotorStatorDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyMotorStatorDefinition();

      MyMotorStatorDefinition IActivator<MyMotorStatorDefinition>.CreateInstance() => new MyMotorStatorDefinition();
    }
  }
}
