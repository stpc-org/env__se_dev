// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.MyRealWheel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities.Cube;
using VRage.Network;

namespace Sandbox.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_RealWheel))]
  public class MyRealWheel : MyMotorRotor
  {
    public override void ContactPointCallback(ref MyGridContactInfo value)
    {
      HkContactPointProperties contactProperties = value.Event.ContactProperties;
      contactProperties.Friction = 0.85f;
      contactProperties.Restitution = 0.2f;
      value.EnableParticles = false;
      value.RubberDeformation = true;
    }

    private class Sandbox_Game_Entities_Blocks_MyRealWheel\u003C\u003EActor : IActivator, IActivator<MyRealWheel>
    {
      object IActivator.CreateInstance() => (object) new MyRealWheel();

      MyRealWheel IActivator<MyRealWheel>.CreateInstance() => new MyRealWheel();
    }
  }
}
