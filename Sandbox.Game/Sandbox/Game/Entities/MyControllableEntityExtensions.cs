// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyControllableEntityExtensions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Weapons;
using VRage.Game.Components;
using VRageMath;

namespace Sandbox.Game.Entities
{
  internal static class MyControllableEntityExtensions
  {
    public static void SwitchControl(
      this IMyControllableEntity entity,
      IMyControllableEntity newControlledEntity)
    {
      if (entity.ControllerInfo.Controller == null)
        return;
      entity.ControllerInfo.Controller.TakeControl(newControlledEntity);
    }

    public static MyPhysicsComponentBase Physics(
      this IMyControllableEntity entity)
    {
      if (entity.Entity == null)
        return (MyPhysicsComponentBase) null;
      if (entity.Entity.Physics != null)
        return entity.Entity.Physics;
      return entity.Entity is MyCockpit entity1 && entity1.CubeGrid != null && entity1.CubeGrid.Physics != null ? (MyPhysicsComponentBase) entity1.CubeGrid.Physics : (MyPhysicsComponentBase) null;
    }

    public static void GetLinearVelocity(
      this IMyControllableEntity controlledEntity,
      ref Vector3 velocityVector,
      bool useRemoteControlVelocity = true)
    {
      if (controlledEntity.Entity.Physics == null)
      {
        if (controlledEntity is MyCockpit myCockpit)
        {
          velocityVector = myCockpit.CubeGrid.Physics != null ? myCockpit.CubeGrid.Physics.LinearVelocity : Vector3.Zero;
        }
        else
        {
          MyRemoteControl myRemoteControl = controlledEntity as MyRemoteControl;
          if (myRemoteControl != null & useRemoteControlVelocity)
          {
            velocityVector = myRemoteControl.CubeGrid.Physics != null ? myRemoteControl.CubeGrid.Physics.LinearVelocity : Vector3.Zero;
          }
          else
          {
            if (!(controlledEntity is MyLargeTurretBase myLargeTurretBase))
              return;
            velocityVector = myLargeTurretBase.CubeGrid.Physics != null ? myLargeTurretBase.CubeGrid.Physics.LinearVelocity : Vector3.Zero;
          }
        }
      }
      else
        velocityVector = controlledEntity.Entity.Physics != null ? controlledEntity.Entity.Physics.LinearVelocity : Vector3.Zero;
    }
  }
}
