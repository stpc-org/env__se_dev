// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatControlledEntitySpeed
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.World;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GUI
{
  public class MyStatControlledEntitySpeed : MyStatBase
  {
    public override float MaxValue => MyGridPhysics.ShipMaxLinearVelocity() + 7f;

    public MyStatControlledEntitySpeed() => this.Id = MyStringHash.GetOrCompute("controlled_speed");

    public override void Update()
    {
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      float num = 0.0f;
      if (controlledEntity != null)
      {
        Vector3 zero = Vector3.Zero;
        controlledEntity.GetLinearVelocity(ref zero);
        num = zero.Length();
      }
      this.CurrentValue = num;
    }
  }
}
