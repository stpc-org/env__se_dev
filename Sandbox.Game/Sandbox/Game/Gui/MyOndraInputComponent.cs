// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyOndraInputComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.World;
using System;
using System.Linq;
using System.Threading;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Input;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
  internal class MyOndraInputComponent : MyDebugComponent
  {
    private bool m_gridDebugInfo;

    public override string GetName() => "Ondra";

    private void phantom_Enter(HkPhantomCallbackShape sender, HkRigidBody body)
    {
    }

    private void phantom_Leave(HkPhantomCallbackShape sender, HkRigidBody body)
    {
    }

    public override void Draw() => base.Draw();

    public override bool HandleInput()
    {
      bool flag = false;
      if (this.m_gridDebugInfo)
      {
        LineD line = new LineD(MySector.MainCamera.Position, MySector.MainCamera.Position + MySector.MainCamera.ForwardVector * 1000f);
        MyCubeGrid grid;
        Vector3I position;
        if (MyCubeGrid.GetLineIntersection(ref line, out grid, out position, out double _))
        {
          MatrixD worldMatrix = grid.WorldMatrix;
          MatrixD matrixD = Matrix.CreateTranslation(position * grid.GridSize) * worldMatrix;
          grid.GetCubeBlock(position);
          MyRenderProxy.DebugDrawText2D(new Vector2(), position.ToString(), Color.White, 0.7f);
          MyRenderProxy.DebugDrawOBB(Matrix.CreateScale(new Vector3(grid.GridSize) + new Vector3(0.15f)) * matrixD, (Color) Color.Red.ToVector3(), 0.2f, true, true);
        }
      }
      if (MyInput.Static.IsAnyAltKeyPressed())
        return flag;
      MyInput.Static.IsAnyShiftKeyPressed();
      MyInput.Static.IsAnyCtrlKeyPressed();
      if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad6))
      {
        MatrixD viewMatrix = MySector.MainCamera.ViewMatrix;
        Matrix matrix = Matrix.Invert((Matrix) ref viewMatrix);
        MyFloatingObjects.Spawn(new MyPhysicalInventoryItem((MyFixedPoint) 1, (MyObjectBuilder_PhysicalObject) MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Ore>("Stone")), (Vector3D) (matrix.Translation + matrix.Forward * 1f), (Vector3D) matrix.Forward, (Vector3D) matrix.Up);
      }
      if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad7))
      {
        foreach (MyCubeGrid myCubeGrid in Sandbox.Game.Entities.MyEntities.GetEntities().OfType<MyCubeGrid>())
        {
          foreach (MyMotorStator myMotorStator in myCubeGrid.CubeBlocks.Select<MySlimBlock, MyCubeBlock>((Func<MySlimBlock, MyCubeBlock>) (s => s.FatBlock)).Where<MyCubeBlock>((Func<MyCubeBlock, bool>) (s => s != null)).OfType<MyMotorStator>())
          {
            if (myMotorStator.Rotor != null)
            {
              Quaternion fromAxisAngle = Quaternion.CreateFromAxisAngle((Vector3) myMotorStator.Rotor.WorldMatrix.Up, MathHelper.ToRadians(45f));
              myMotorStator.Rotor.CubeGrid.WorldMatrix = MatrixD.CreateFromQuaternion(fromAxisAngle) * myMotorStator.Rotor.CubeGrid.WorldMatrix;
            }
          }
        }
      }
      if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad8))
      {
        MatrixD viewMatrix = MySector.MainCamera.ViewMatrix;
        Matrix matrix = Matrix.Invert((Matrix) ref viewMatrix);
        MyObjectBuilder_Ore newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Ore>("Stone");
        MyObjectBuilder_FloatingObject builderFloatingObject = new MyObjectBuilder_FloatingObject();
        builderFloatingObject.Item = new MyObjectBuilder_InventoryItem()
        {
          PhysicalContent = (MyObjectBuilder_PhysicalObject) newObject,
          Amount = (MyFixedPoint) 1000
        };
        builderFloatingObject.PositionAndOrientation = new MyPositionAndOrientation?(new MyPositionAndOrientation((Vector3D) (matrix.Translation + 2f * matrix.Forward), matrix.Forward, matrix.Up));
        builderFloatingObject.PersistentFlags = MyPersistentEntityFlags2.InScene;
        Sandbox.Game.Entities.MyEntities.CreateFromObjectBuilderAndAdd((MyObjectBuilder_EntityBase) builderFloatingObject, false).Physics.LinearVelocity = Vector3.Normalize(matrix.Forward) * 50f;
      }
      MyInput.Static.IsNewKeyPressed(MyKeys.Divide);
      if (MyInput.Static.IsNewKeyPressed(MyKeys.Multiply))
      {
        MyDebugDrawSettings.ENABLE_DEBUG_DRAW = !MyDebugDrawSettings.ENABLE_DEBUG_DRAW;
        foreach (MyCubeGrid myCubeGrid in Sandbox.Game.Entities.MyEntities.GetEntities().OfType<MyCubeGrid>())
        {
          int num = myCubeGrid.IsStatic ? 1 : 0;
        }
      }
      if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad1))
      {
        MyCubeGrid myCubeGrid = Sandbox.Game.Entities.MyEntities.GetEntities().OfType<MyCubeGrid>().FirstOrDefault<MyCubeGrid>();
        if (myCubeGrid != null)
        {
          myCubeGrid.Physics.RigidBody.MaxLinearVelocity = 1000f;
          if ((HkReferenceObject) myCubeGrid.Physics.RigidBody2 != (HkReferenceObject) null)
            myCubeGrid.Physics.RigidBody2.MaxLinearVelocity = 1000f;
          myCubeGrid.Physics.LinearVelocity = new Vector3(1000f, 0.0f, 0.0f);
        }
      }
      if (MyInput.Static.IsNewKeyPressed(MyKeys.Decimal))
        MyPrefabManager.Static.SpawnPrefab("respawnship", MySector.MainCamera.Position, MySector.MainCamera.ForwardVector, MySector.MainCamera.UpVector);
      if (MyInput.Static.IsNewKeyPressed(MyKeys.Multiply) && MyInput.Static.IsAnyShiftKeyPressed())
        GC.Collect(2);
      if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad5))
        Thread.Sleep(250);
      if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad9))
      {
        MyEntity myEntity = MySession.Static.ControlledEntity != null ? MySession.Static.ControlledEntity.Entity : (MyEntity) null;
        myEntity?.PositionComp.SetPosition(myEntity.PositionComp.GetPosition() + myEntity.WorldMatrix.Forward * 5.0);
      }
      if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad4))
      {
        if (MySession.Static.ControlledEntity is MyEntity controlledEntity && controlledEntity.HasInventory)
        {
          MyFixedPoint amount = (MyFixedPoint) 20000;
          MyObjectBuilder_Ore newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Ore>("Stone");
          MyEntityExtensions.GetInventory(controlledEntity).AddItems(amount, (MyObjectBuilder_Base) newObject);
        }
        flag = true;
      }
      if (MyInput.Static.IsAnyCtrlKeyPressed() && MyInput.Static.IsNewKeyPressed(MyKeys.Delete))
      {
        Sandbox.Game.Entities.MyEntities.GetEntities().OfType<MyFloatingObject>().Count<MyFloatingObject>();
        foreach (MyFloatingObject myFloatingObject in Sandbox.Game.Entities.MyEntities.GetEntities().OfType<MyFloatingObject>())
        {
          if (myFloatingObject == MySession.Static.ControlledEntity)
            MySession.Static.SetCameraController(MyCameraControllerEnum.Spectator, (IMyEntity) null, new Vector3D?());
          myFloatingObject.Close();
        }
        flag = true;
      }
      if (MyInput.Static.IsAnyCtrlKeyPressed() && MyInput.Static.IsNewKeyPressed(MyKeys.Decimal))
      {
        foreach (MyEntity entity in Sandbox.Game.Entities.MyEntities.GetEntities())
        {
          if (entity != MySession.Static.ControlledEntity && (MySession.Static.ControlledEntity == null || entity != MySession.Static.ControlledEntity.Entity.Parent) && entity != MyCubeBuilder.Static.FindClosestGrid())
            entity.Close();
        }
        flag = true;
      }
      if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad9) || MyInput.Static.IsNewKeyPressed(MyKeys.NumPad5))
      {
        MyPhysicsComponentBase physics = MySession.Static.ControlledEntity.Entity.GetTopMostParent((System.Type) null).Physics;
        if ((HkReferenceObject) physics.RigidBody != (HkReferenceObject) null)
          physics.RigidBody.ApplyLinearImpulse((Vector3) (physics.Entity.WorldMatrix.Forward * (double) physics.Mass * 2.0));
        flag = true;
      }
      if (MyInput.Static.IsAnyCtrlKeyPressed() && MyInput.Static.IsNewKeyPressed(MyKeys.OemComma))
      {
        foreach (MyEntity myEntity in Sandbox.Game.Entities.MyEntities.GetEntities().OfType<MyFloatingObject>().ToArray<MyFloatingObject>())
          myEntity.Close();
      }
      return flag;
    }
  }
}
