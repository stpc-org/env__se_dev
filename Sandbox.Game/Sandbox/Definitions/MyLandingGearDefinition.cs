// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyLandingGearDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_LandingGearDefinition), null)]
  public class MyLandingGearDefinition : MyCubeBlockDefinition
  {
    public string LockSound;
    public string UnlockSound;
    public string FailedAttachSound;
    public float MaxLockSeparatingVelocity;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_LandingGearDefinition landingGearDefinition = builder as MyObjectBuilder_LandingGearDefinition;
      this.LockSound = landingGearDefinition.LockSound;
      this.UnlockSound = landingGearDefinition.UnlockSound;
      this.FailedAttachSound = landingGearDefinition.FailedAttachSound;
      this.MaxLockSeparatingVelocity = landingGearDefinition.MaxLockSeparatingVelocity;
    }

    private class Sandbox_Definitions_MyLandingGearDefinition\u003C\u003EActor : IActivator, IActivator<MyLandingGearDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyLandingGearDefinition();

      MyLandingGearDefinition IActivator<MyLandingGearDefinition>.CreateInstance() => new MyLandingGearDefinition();
    }
  }
}
