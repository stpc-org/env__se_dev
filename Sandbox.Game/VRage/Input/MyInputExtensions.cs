// Decompiled with JetBrains decompiler
// Type: VRage.Input.MyInputExtensions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using VRage.Utils;
using VRageMath;

namespace VRage.Input
{
  public static class MyInputExtensions
  {
    public const float MOUSE_ROTATION_INDICATOR_MULTIPLIER = 0.075f;
    public const float ROTATION_INDICATOR_MULTIPLIER = 0.15f;

    public static float GetRoll(this IMyInput self)
    {
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      MyStringId context = controlledEntity != null ? controlledEntity.ControlContext : MySpaceBindingCreator.CX_BASE;
      return MyControllerHelper.IsControlAnalog(context, MyControlsSpace.ROLL_RIGHT) - MyControllerHelper.IsControlAnalog(context, MyControlsSpace.ROLL_LEFT);
    }

    public static float GetDeveloperRoll(this IMyInput self)
    {
      float num = 0.0f;
      bool flag = false;
      if (self.IsGameControlPressed(MyControlsSpace.ROLL_LEFT))
      {
        num += -1f;
        flag = true;
      }
      if (self.IsGameControlPressed(MyControlsSpace.ROLL_RIGHT))
      {
        ++num;
        flag = true;
      }
      if (!flag)
      {
        IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
        MyStringId context = controlledEntity != null ? controlledEntity.ControlContext : MySpaceBindingCreator.CX_BASE;
        num += MyControllerHelper.IsControlAnalog(context, MyControlsSpace.ROLL_RIGHT) - MyControllerHelper.IsControlAnalog(context, MyControlsSpace.ROLL_LEFT);
      }
      return num;
    }

    public static Vector3 GetPositionDelta(this IMyInput self)
    {
      Vector3 vector3 = Vector3.Zero;
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      MyStringId myStringId = MySpaceBindingCreator.CX_SPECTATOR;
      if (!MySession.Static.IsCameraUserControlledSpectator())
        myStringId = controlledEntity != null ? controlledEntity.ControlContext : MySpaceBindingCreator.CX_BASE;
      if (MyInput.Static.IsJoystickLastUsed && myStringId == MySpaceBindingCreator.CX_SPACESHIP)
      {
        vector3 = MyInputExtensions.GetShipMoveIndicator_Gamepad(myStringId);
      }
      else
      {
        vector3.X = MyControllerHelper.IsControlAnalog(myStringId, MyControlsSpace.STRAFE_RIGHT) - MyControllerHelper.IsControlAnalog(myStringId, MyControlsSpace.STRAFE_LEFT);
        vector3.Y = MyControllerHelper.IsControlAnalog(myStringId, MyControlsSpace.JUMP) - MyControllerHelper.IsControlAnalog(myStringId, MyControlsSpace.CROUCH);
        vector3.Z = MyControllerHelper.IsControlAnalog(myStringId, MyControlsSpace.BACKWARD) - MyControllerHelper.IsControlAnalog(myStringId, MyControlsSpace.FORWARD);
      }
      return vector3;
    }

    private static bool IsControlledEntityCar()
    {
      MyPlayer firstPlayer = Sync.Clients?.LocalClient?.FirstPlayer;
      if (firstPlayer == null)
        return false;
      IMyControllableEntity controlledEntity = firstPlayer.Controller.ControlledEntity;
      if (controlledEntity == null)
        return false;
      MyStringId myStringId = controlledEntity.ControlContext;
      int id1 = myStringId.Id;
      myStringId = MySpaceBindingCreator.CX_SPACESHIP;
      int id2 = myStringId.Id;
      if (id1 != id2)
        return false;
      MyShipController myShipController = (MyShipController) controlledEntity;
      return myShipController != null && myShipController.GridWheels != null && (myShipController.GridWheels.WheelCount > 0 && !(myShipController.GetTotalGravity() == Vector3D.Zero));
    }

    private static Vector3 GetShipMoveIndicator_Gamepad(MyStringId cx)
    {
      Vector3 zero = Vector3.Zero;
      zero.X = MyControllerHelper.IsControlAnalog(cx, MyControlsSpace.STRAFE_RIGHT, true) - MyControllerHelper.IsControlAnalog(cx, MyControlsSpace.STRAFE_LEFT, true);
      zero.Y = MyControllerHelper.IsControlAnalog(cx, MyControlsSpace.JUMP) - MyControllerHelper.IsControlAnalog(cx, MyControlsSpace.CROUCH);
      zero.Z = MyControllerHelper.IsControlAnalog(cx, MyControlsSpace.BACKWARD) - MyControllerHelper.IsControlAnalog(cx, MyControlsSpace.FORWARD);
      return zero;
    }

    public static Vector2 GetRotation(this IMyInput self)
    {
      Vector2 zero = Vector2.Zero;
      Vector2 vector2 = new Vector2(self.GetMouseYForGamePlayF(), self.GetMouseXForGamePlayF()) * 0.075f;
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      MyStringId context = controlledEntity != null ? controlledEntity.ControlContext : MySpaceBindingCreator.CX_BASE;
      vector2.X -= MyControllerHelper.IsControlAnalog(context, MyControlsSpace.ROTATION_UP);
      vector2.X += MyControllerHelper.IsControlAnalog(context, MyControlsSpace.ROTATION_DOWN);
      vector2.Y -= MyControllerHelper.IsControlAnalog(context, MyControlsSpace.ROTATION_LEFT);
      vector2.Y += MyControllerHelper.IsControlAnalog(context, MyControlsSpace.ROTATION_RIGHT);
      vector2 *= 9f;
      return vector2;
    }

    public static Vector2 GetLookAroundRotation(this IMyInput self)
    {
      Vector2 zero = Vector2.Zero;
      Vector2 vector2 = new Vector2(self.GetMouseYForGamePlayF(), self.GetMouseXForGamePlayF()) * 0.075f;
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      bool flag = false;
      if (controlledEntity != null && controlledEntity.Entity != null && (controlledEntity.Entity is Sandbox.ModAPI.IMyShipController && !((Sandbox.ModAPI.Ingame.IMyShipController) controlledEntity.Entity).CanControlShip))
        flag = true;
      MyStringId context = controlledEntity == null ? MySpaceBindingCreator.CX_BASE : controlledEntity.ControlContext;
      vector2.X -= MyControllerHelper.IsControlAnalog(context, flag ? MyControlsSpace.ROTATION_UP : MyControlsSpace.LOOK_UP);
      vector2.X += MyControllerHelper.IsControlAnalog(context, flag ? MyControlsSpace.ROTATION_DOWN : MyControlsSpace.LOOK_DOWN);
      vector2.Y -= MyControllerHelper.IsControlAnalog(context, flag ? MyControlsSpace.ROTATION_LEFT : MyControlsSpace.LOOK_LEFT);
      vector2.Y += MyControllerHelper.IsControlAnalog(context, flag ? MyControlsSpace.ROTATION_RIGHT : MyControlsSpace.LOOK_RIGHT);
      return vector2 * 9f;
    }

    public static Vector2 GetCursorPositionDelta(this IMyInput self)
    {
      Vector2 one = Vector2.One;
      return new Vector2((float) self.GetMouseX(), (float) self.GetMouseY()) * one;
    }

    public static float GetRoll(this VRage.ModAPI.IMyInput self) => ((IMyInput) self).GetRoll();

    public static Vector3 GetPositionDelta(this VRage.ModAPI.IMyInput self) => ((IMyInput) self).GetPositionDelta();

    public static Vector2 GetRotation(this VRage.ModAPI.IMyInput self) => ((IMyInput) self).GetRotation();

    public static Vector2 GetCursorPositionDelta(this VRage.ModAPI.IMyInput self) => ((IMyInput) self).GetCursorPositionDelta();
  }
}
