// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatControlledIsStatic
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
  public class MyStatControlledIsStatic : MyStatBase
  {
    public MyStatControlledIsStatic() => this.Id = MyStringHash.GetOrCompute("controlled_is_static");

    public override void Update()
    {
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      if (controlledEntity != null)
      {
        if (controlledEntity.Entity is MyCubeGrid entity)
        {
          this.CurrentValue = entity.IsStatic ? 1f : 0.0f;
          return;
        }
        if (controlledEntity.Entity is MyCockpit entity)
        {
          this.CurrentValue = entity.CubeGrid.IsStatic ? 1f : 0.0f;
          return;
        }
        if (controlledEntity is MyLargeTurretBase)
          this.CurrentValue = (controlledEntity as MyLargeTurretBase).CubeGrid.IsStatic ? 1f : 0.0f;
      }
      this.CurrentValue = 0.0f;
    }
  }
}
