// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyMotorAdvancedRotor
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Components;
using Sandbox.Game.GameSystems.Conveyors;
using VRage.Game.Components;
using VRage.Network;

namespace Sandbox.Game.Entities.Cube
{
  [MyCubeBlockType(typeof (MyObjectBuilder_MotorAdvancedRotor))]
  public class MyMotorAdvancedRotor : MyMotorRotor, IMyConveyorEndpointBlock, Sandbox.ModAPI.IMyMotorAdvancedRotor, Sandbox.ModAPI.IMyMotorRotor, Sandbox.ModAPI.IMyAttachableTopBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyAttachableTopBlock, Sandbox.ModAPI.Ingame.IMyMotorRotor, Sandbox.ModAPI.Ingame.IMyMotorAdvancedRotor
  {
    private MyAttachableConveyorEndpoint m_conveyorEndpoint;

    public void InitializeConveyorEndpoint()
    {
      this.m_conveyorEndpoint = new MyAttachableConveyorEndpoint((MyCubeBlock) this);
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentDrawConveyorEndpoint((IMyConveyorEndpoint) this.m_conveyorEndpoint));
    }

    public IMyConveyorEndpoint ConveyorEndpoint => (IMyConveyorEndpoint) this.m_conveyorEndpoint;

    public PullInformation GetPullInformation() => (PullInformation) null;

    public PullInformation GetPushInformation() => (PullInformation) null;

    public bool AllowSelfPulling() => false;

    private class Sandbox_Game_Entities_Cube_MyMotorAdvancedRotor\u003C\u003EActor : IActivator, IActivator<MyMotorAdvancedRotor>
    {
      object IActivator.CreateInstance() => (object) new MyMotorAdvancedRotor();

      MyMotorAdvancedRotor IActivator<MyMotorAdvancedRotor>.CreateInstance() => new MyMotorAdvancedRotor();
    }
  }
}
