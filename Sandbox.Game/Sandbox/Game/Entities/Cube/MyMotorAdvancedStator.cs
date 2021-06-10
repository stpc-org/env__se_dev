// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyMotorAdvancedStator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.ModAPI;
using System;
using VRage.Game;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
  [MyCubeBlockType(typeof (MyObjectBuilder_MotorAdvancedStator))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyMotorAdvancedStator), typeof (Sandbox.ModAPI.Ingame.IMyMotorAdvancedStator)})]
  public class MyMotorAdvancedStator : MyMotorStator, Sandbox.ModAPI.IMyMotorAdvancedStator, Sandbox.ModAPI.IMyMotorStator, Sandbox.ModAPI.Ingame.IMyMotorStator, Sandbox.ModAPI.Ingame.IMyMotorBase, Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyMotorBase, Sandbox.ModAPI.IMyMechanicalConnectionBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyMotorAdvancedStator
  {
    protected override bool Attach(MyAttachableTopBlockBase rotor, bool updateGroup = true)
    {
      if (!(rotor is MyMotorRotor))
        return false;
      int num = base.Attach(rotor, updateGroup) ? 1 : 0;
      if ((num & (updateGroup ? 1 : 0)) == 0)
        return num != 0;
      if (!(this.TopBlock is MyMotorAdvancedRotor))
        return num != 0;
      this.m_conveyorEndpoint.Attach((this.TopBlock as MyMotorAdvancedRotor).ConveyorEndpoint as MyAttachableConveyorEndpoint);
      return num != 0;
    }

    protected override void Detach(MyCubeGrid topGrid, bool updateGroup = true)
    {
      if (this.TopBlock != null & updateGroup)
      {
        MyAttachableTopBlockBase topBlock = this.TopBlock;
        if (topBlock is MyMotorAdvancedRotor)
          this.m_conveyorEndpoint.Detach((topBlock as MyMotorAdvancedRotor).ConveyorEndpoint as MyAttachableConveyorEndpoint);
      }
      base.Detach(topGrid, updateGroup);
    }

    public override void ComputeTopQueryBox(
      out Vector3D pos,
      out Vector3 halfExtents,
      out Quaternion orientation)
    {
      base.ComputeTopQueryBox(out pos, out halfExtents, out orientation);
      if (this.CubeGrid.GridSizeEnum != MyCubeSize.Small)
        return;
      halfExtents.Y *= 2f;
    }

    public MyMotorAdvancedStator() => this.m_canBeDetached = true;

    private class Sandbox_Game_Entities_Cube_MyMotorAdvancedStator\u003C\u003EActor : IActivator, IActivator<MyMotorAdvancedStator>
    {
      object IActivator.CreateInstance() => (object) new MyMotorAdvancedStator();

      MyMotorAdvancedStator IActivator<MyMotorAdvancedStator>.CreateInstance() => new MyMotorAdvancedStator();
    }
  }
}
