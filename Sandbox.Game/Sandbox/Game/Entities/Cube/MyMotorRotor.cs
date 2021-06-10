// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyMotorRotor
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities.Blocks;
using System;
using VRage.Network;

namespace Sandbox.Game.Entities.Cube
{
  [MyCubeBlockType(typeof (MyObjectBuilder_MotorRotor))]
  public class MyMotorRotor : MyAttachableTopBlockBase, Sandbox.ModAPI.IMyMotorRotor, Sandbox.ModAPI.IMyAttachableTopBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyAttachableTopBlock, Sandbox.ModAPI.Ingame.IMyMotorRotor
  {
    [Obsolete("Use MyAttachableTopBlockBase.Base")]
    Sandbox.ModAPI.IMyMotorBase Sandbox.ModAPI.IMyMotorRotor.Stator => (Sandbox.ModAPI.IMyMotorBase) (this.Stator as MyMotorStator);

    Sandbox.ModAPI.IMyMotorBase Sandbox.ModAPI.IMyMotorRotor.Base => (Sandbox.ModAPI.IMyMotorBase) this.Stator;

    private class Sandbox_Game_Entities_Cube_MyMotorRotor\u003C\u003EActor : IActivator, IActivator<MyMotorRotor>
    {
      object IActivator.CreateInstance() => (object) new MyMotorRotor();

      MyMotorRotor IActivator<MyMotorRotor>.CreateInstance() => new MyMotorRotor();
    }
  }
}
