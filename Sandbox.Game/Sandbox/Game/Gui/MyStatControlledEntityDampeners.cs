// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatControlledEntityDampeners
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
  public class MyStatControlledEntityDampeners : MyStatBase
  {
    public MyStatControlledEntityDampeners() => this.Id = MyStringHash.GetOrCompute("controlled_dampeners");

    public override void Update()
    {
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      if (controlledEntity == null)
        return;
      if (controlledEntity is MyLargeTurretBase)
        controlledEntity = (controlledEntity as MyLargeTurretBase).PreviousControlledEntity;
      if (controlledEntity.EnabledDamping)
      {
        if (controlledEntity.RelativeDampeningEntity == null)
          this.CurrentValue = 1f;
        else
          this.CurrentValue = 0.5f;
      }
      else
        this.CurrentValue = 0.0f;
    }
  }
}
