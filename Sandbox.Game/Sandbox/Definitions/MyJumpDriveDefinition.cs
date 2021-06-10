// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyJumpDriveDefinition
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
  [MyDefinitionType(typeof (MyObjectBuilder_JumpDriveDefinition), null)]
  public class MyJumpDriveDefinition : MyCubeBlockDefinition
  {
    public MyStringHash ResourceSinkGroup;
    public float RequiredPowerInput;
    public float PowerNeededForJump;
    public double MaxJumpDistance;
    public double MaxJumpMass;
    public float JumpDelay;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_JumpDriveDefinition jumpDriveDefinition = builder as MyObjectBuilder_JumpDriveDefinition;
      this.ResourceSinkGroup = MyStringHash.GetOrCompute(jumpDriveDefinition.ResourceSinkGroup);
      this.RequiredPowerInput = jumpDriveDefinition.RequiredPowerInput;
      this.PowerNeededForJump = jumpDriveDefinition.PowerNeededForJump;
      this.MaxJumpDistance = jumpDriveDefinition.MaxJumpDistance;
      this.MaxJumpMass = jumpDriveDefinition.MaxJumpMass;
      this.JumpDelay = jumpDriveDefinition.JumpDelay;
    }

    private class Sandbox_Definitions_MyJumpDriveDefinition\u003C\u003EActor : IActivator, IActivator<MyJumpDriveDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyJumpDriveDefinition();

      MyJumpDriveDefinition IActivator<MyJumpDriveDefinition>.CreateInstance() => new MyJumpDriveDefinition();
    }
  }
}
