// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatControlledEntityMass
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
  public class MyStatControlledEntityMass : MyStatBase
  {
    public override float MaxValue => 0.0f;

    public MyStatControlledEntityMass() => this.Id = MyStringHash.GetOrCompute("controlled_mass");

    public override void Update()
    {
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      if (controlledEntity != null)
      {
        MyCubeGrid myCubeGrid = (MyCubeGrid) null;
        if (controlledEntity.Entity is MyCockpit entity)
        {
          myCubeGrid = entity.CubeGrid;
        }
        else
        {
          switch (controlledEntity)
          {
            case MyRemoteControl myRemoteControl:
              myCubeGrid = myRemoteControl.CubeGrid;
              break;
            case MyLargeTurretBase myLargeTurretBase:
              myCubeGrid = myLargeTurretBase.CubeGrid;
              break;
          }
        }
        this.CurrentValue = myCubeGrid != null ? (float) myCubeGrid.GetCurrentMass() : 0.0f;
      }
      else
        this.CurrentValue = 0.0f;
    }
  }
}
