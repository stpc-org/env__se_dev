// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyConveyor
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems.Conveyors;
using VRage.Game;
using VRage.Game.Components;
using VRage.Network;

namespace Sandbox.Game.Entities
{
  [MyCubeBlockType(typeof (MyObjectBuilder_Conveyor))]
  public class MyConveyor : MyCubeBlock, IMyConveyorEndpointBlock, Sandbox.ModAPI.IMyConveyor, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyConveyor
  {
    private MyMultilineConveyorEndpoint m_conveyorEndpoint;

    public IMyConveyorEndpoint ConveyorEndpoint => (IMyConveyorEndpoint) this.m_conveyorEndpoint;

    public void InitializeConveyorEndpoint()
    {
      this.m_conveyorEndpoint = new MyMultilineConveyorEndpoint((MyCubeBlock) this);
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentDrawConveyorEndpoint((IMyConveyorEndpoint) this.m_conveyorEndpoint));
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid) => base.Init(objectBuilder, cubeGrid);

    public PullInformation GetPullInformation() => (PullInformation) null;

    public PullInformation GetPushInformation() => (PullInformation) null;

    public bool AllowSelfPulling() => false;

    private class Sandbox_Game_Entities_MyConveyor\u003C\u003EActor : IActivator, IActivator<MyConveyor>
    {
      object IActivator.CreateInstance() => (object) new MyConveyor();

      MyConveyor IActivator<MyConveyor>.CreateInstance() => new MyConveyor();
    }
  }
}
