// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.MyPistonTop
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems.Conveyors;
using VRage.Game.Components;
using VRage.Network;

namespace Sandbox.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_PistonTop))]
  public class MyPistonTop : MyAttachableTopBlockBase, IMyConveyorEndpointBlock, Sandbox.ModAPI.IMyPistonTop, Sandbox.ModAPI.IMyAttachableTopBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyAttachableTopBlock, Sandbox.ModAPI.Ingame.IMyPistonTop
  {
    private MyPistonBase m_pistonBlock;
    private MyAttachableConveyorEndpoint m_conveyorEndpoint;

    public override void Attach(MyMechanicalConnectionBlockBase pistonBase)
    {
      base.Attach(pistonBase);
      this.m_pistonBlock = pistonBase as MyPistonBase;
    }

    public override void ContactPointCallback(ref MyGridContactInfo value)
    {
      base.ContactPointCallback(ref value);
      if (this.m_pistonBlock == null || value.CollidingEntity != this.m_pistonBlock.Subpart3)
        return;
      value.EnableDeformation = false;
      value.EnableParticles = false;
    }

    public IMyConveyorEndpoint ConveyorEndpoint => (IMyConveyorEndpoint) this.m_conveyorEndpoint;

    public void InitializeConveyorEndpoint()
    {
      this.m_conveyorEndpoint = new MyAttachableConveyorEndpoint((MyCubeBlock) this);
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentDrawConveyorEndpoint((IMyConveyorEndpoint) this.m_conveyorEndpoint));
    }

    bool Sandbox.ModAPI.Ingame.IMyAttachableTopBlock.IsAttached => this.m_pistonBlock != null;

    Sandbox.ModAPI.IMyMechanicalConnectionBlock Sandbox.ModAPI.IMyAttachableTopBlock.Base => (Sandbox.ModAPI.IMyMechanicalConnectionBlock) this.m_pistonBlock;

    Sandbox.ModAPI.IMyPistonBase Sandbox.ModAPI.IMyPistonTop.Base => (Sandbox.ModAPI.IMyPistonBase) this.m_pistonBlock;

    Sandbox.ModAPI.IMyPistonBase Sandbox.ModAPI.IMyPistonTop.Piston => (Sandbox.ModAPI.IMyPistonBase) this.m_pistonBlock;

    public PullInformation GetPullInformation() => (PullInformation) null;

    public PullInformation GetPushInformation() => (PullInformation) null;

    public bool AllowSelfPulling() => false;

    private class Sandbox_Game_Entities_Blocks_MyPistonTop\u003C\u003EActor : IActivator, IActivator<MyPistonTop>
    {
      object IActivator.CreateInstance() => (object) new MyPistonTop();

      MyPistonTop IActivator<MyPistonTop>.CreateInstance() => new MyPistonTop();
    }
  }
}
