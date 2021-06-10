// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyPistonBaseDefinition
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
  [MyDefinitionType(typeof (MyObjectBuilder_PistonBaseDefinition), null)]
  public class MyPistonBaseDefinition : MyMechanicalConnectionBlockBaseDefinition
  {
    public float Minimum;
    public float Maximum;
    public float MaxVelocity;
    public MyStringHash ResourceSinkGroup;
    public float RequiredPowerInput;
    public float MaxImpulse;
    public float DefaultMaxImpulseAxis;
    public float DefaultMaxImpulseNonAxis;
    public float UnsafeImpulseThreshold;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_PistonBaseDefinition pistonBaseDefinition = (MyObjectBuilder_PistonBaseDefinition) builder;
      this.Minimum = pistonBaseDefinition.Minimum;
      this.Maximum = pistonBaseDefinition.Maximum;
      this.MaxVelocity = pistonBaseDefinition.MaxVelocity;
      this.ResourceSinkGroup = MyStringHash.GetOrCompute(pistonBaseDefinition.ResourceSinkGroup);
      this.RequiredPowerInput = pistonBaseDefinition.RequiredPowerInput;
      this.MaxImpulse = pistonBaseDefinition.MaxImpulse;
      this.DefaultMaxImpulseAxis = pistonBaseDefinition.DefaultMaxImpulseAxis;
      this.DefaultMaxImpulseNonAxis = pistonBaseDefinition.DefaultMaxImpulseNonAxis;
      this.UnsafeImpulseThreshold = pistonBaseDefinition.DangerousImpulseThreshold;
    }

    private class Sandbox_Definitions_MyPistonBaseDefinition\u003C\u003EActor : IActivator, IActivator<MyPistonBaseDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyPistonBaseDefinition();

      MyPistonBaseDefinition IActivator<MyPistonBaseDefinition>.CreateInstance() => new MyPistonBaseDefinition();
    }
  }
}
