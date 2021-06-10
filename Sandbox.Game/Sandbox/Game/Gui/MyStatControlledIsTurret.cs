// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatControlledIsTurret
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
  public class MyStatControlledIsTurret : MyStatBase
  {
    public MyStatControlledIsTurret() => this.Id = MyStringHash.GetOrCompute("controlled_is_turret");

    public override void Update()
    {
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      if (controlledEntity == null)
        this.CurrentValue = 0.0f;
      else
        this.CurrentValue = controlledEntity is IMyUserControllableGun ? 1f : 0.0f;
    }
  }
}
