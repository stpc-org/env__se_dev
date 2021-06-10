// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyTestersInputComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Input;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Gui
{
  internal class MyTestersInputComponent : MyDebugComponent
  {
    public MyTestersInputComponent()
    {
      this.AddShortcut(MyKeys.Back, true, true, false, false, (Func<string>) (() => "Freeze cube builder gizmo"), (Func<bool>) (() =>
      {
        MyCubeBuilder.Static.FreezeGizmo = !MyCubeBuilder.Static.FreezeGizmo;
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad0, false, false, false, false, (Func<string>) (() => "Add items to inventory (continuous)"), (Func<bool>) (() =>
      {
        this.AddItemsToInventory(0);
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad1, true, false, false, false, (Func<string>) (() => "Add items to inventory"), (Func<bool>) (() =>
      {
        this.AddItemsToInventory(1);
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad2, true, false, false, false, (Func<string>) (() => "Add components to inventory"), (Func<bool>) (() =>
      {
        this.AddItemsToInventory(2);
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad3, true, false, false, false, (Func<string>) (() => "Fill inventory with iron"), new Func<bool>(MyTestersInputComponent.FillInventoryWithIron));
      this.AddShortcut(MyKeys.NumPad4, true, false, false, false, (Func<string>) (() => "Add to inventory dialog..."), (Func<bool>) (() =>
      {
        MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenDialogInventoryCheat());
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad5, true, false, false, false, (Func<string>) (() => "Set container type"), new Func<bool>(MyTestersInputComponent.SetContainerType));
      this.AddShortcut(MyKeys.NumPad6, true, false, false, false, (Func<string>) (() => "Toggle debug draw"), new Func<bool>(MyTestersInputComponent.ToggleDebugDraw));
      this.AddShortcut(MyKeys.NumPad8, true, false, false, false, (Func<string>) (() => "Save the game"), (Func<bool>) (() =>
      {
        MyAsyncSaving.Start();
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad9, true, false, false, false, (Func<string>) (() => "SpawnCargoShip"), new Func<bool>(this.SpawnCargoShip));
    }

    private bool SpawnCargoShip()
    {
      MyNeutralShipSpawner.OnGlobalSpawnEvent((object) null);
      return true;
    }

    public override string GetName() => "Testers";

    public bool AddItems(
      MyInventory inventory,
      MyObjectBuilder_PhysicalObject obj,
      bool overrideCheck)
    {
      return this.AddItems(inventory, obj, overrideCheck, (MyFixedPoint) 1);
    }

    public bool AddItems(
      MyInventory inventory,
      MyObjectBuilder_PhysicalObject obj,
      bool overrideCheck,
      MyFixedPoint amount)
    {
      if (!overrideCheck && inventory.ContainItems(amount, obj) || !inventory.CanItemsBeAdded(amount, obj.GetId()))
        return false;
      inventory.AddItems(amount, (MyObjectBuilder_Base) obj);
      return true;
    }

    public override bool HandleInput() => MySession.Static != null && !(MyScreenManager.GetScreenWithFocus() is MyGuiScreenDialogInventoryCheat) && base.HandleInput();

    private static bool ToggleDebugDraw()
    {
      if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
      {
        MyDebugDrawSettings.ENABLE_DEBUG_DRAW = false;
        MyDebugDrawSettings.DEBUG_DRAW_EVENTS = false;
      }
      else
      {
        MyDebugDrawSettings.ENABLE_DEBUG_DRAW = true;
        MyDebugDrawSettings.DEBUG_DRAW_EVENTS = true;
      }
      return true;
    }

    private static bool SetContainerType()
    {
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      if (localCharacter == null)
        return false;
      MatrixD headMatrix = localCharacter.GetHeadMatrix(true, true, false, false, false);
      Matrix matrix = (Matrix) ref headMatrix;
      List<MyPhysics.HitInfo> hitInfoList = new List<MyPhysics.HitInfo>();
      MyPhysics.CastRay((Vector3D) matrix.Translation, (Vector3D) (matrix.Translation + matrix.Forward * 100f), hitInfoList);
      if (hitInfoList.Count == 0)
        return false;
      MyPhysics.HitInfo hitInfo = hitInfoList.FirstOrDefault<MyPhysics.HitInfo>();
      if ((HkReferenceObject) hitInfo.HkHitInfo.Body == (HkReferenceObject) null)
        return false;
      IMyEntity hitEntity = hitInfo.HkHitInfo.GetHitEntity();
      if (!(hitEntity is MyCargoContainer))
        return false;
      MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenDialogContainerType(hitEntity as MyCargoContainer));
      return true;
    }

    private static bool FillInventoryWithIron()
    {
      if (MySession.Static.ControlledEntity is MyEntity controlledEntity && controlledEntity.HasInventory)
      {
        MyFixedPoint myFixedPoint = (MyFixedPoint) 20000;
        MyObjectBuilder_Ore newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Ore>("Iron");
        MyInventory inventory = MyEntityExtensions.GetInventory(controlledEntity);
        inventory.AddItems(inventory.ComputeAmountThatFits(newObject.GetId(), 0.0f, 0.0f), (MyObjectBuilder_Base) newObject);
      }
      return true;
    }

    private void AddItemsToInventory(int variant)
    {
      bool overrideCheck = (uint) variant > 0U;
      bool flag1 = (uint) variant > 0U;
      bool flag2 = variant == 2;
      if (!(MySession.Static.ControlledEntity is MyEntity controlledEntity) || !controlledEntity.HasInventory)
        return;
      MyInventory inventory = MyEntityExtensions.GetInventory(controlledEntity);
      if (!flag2)
      {
        MyObjectBuilder_AmmoMagazine builderAmmoMagazine1 = new MyObjectBuilder_AmmoMagazine();
        builderAmmoMagazine1.SubtypeName = "NATO_5p56x45mm";
        builderAmmoMagazine1.ProjectilesCount = 50;
        this.AddItems(inventory, (MyObjectBuilder_PhysicalObject) builderAmmoMagazine1, false, (MyFixedPoint) 5);
        MyObjectBuilder_AmmoMagazine builderAmmoMagazine2 = new MyObjectBuilder_AmmoMagazine();
        builderAmmoMagazine2.SubtypeName = "NATO_25x184mm";
        builderAmmoMagazine2.ProjectilesCount = 50;
        this.AddItems(inventory, (MyObjectBuilder_PhysicalObject) builderAmmoMagazine2, false);
        MyObjectBuilder_AmmoMagazine builderAmmoMagazine3 = new MyObjectBuilder_AmmoMagazine();
        builderAmmoMagazine3.SubtypeName = "Missile200mm";
        builderAmmoMagazine3.ProjectilesCount = 50;
        this.AddItems(inventory, (MyObjectBuilder_PhysicalObject) builderAmmoMagazine3, false);
        this.AddItems(inventory, (MyObjectBuilder_PhysicalObject) this.CreateGunContent("AutomaticRifleItem"), false);
        this.AddItems(inventory, (MyObjectBuilder_PhysicalObject) this.CreateGunContent("WelderItem"), false);
        this.AddItems(inventory, (MyObjectBuilder_PhysicalObject) this.CreateGunContent("AngleGrinderItem"), false);
        this.AddItems(inventory, (MyObjectBuilder_PhysicalObject) this.CreateGunContent("HandDrillItem"), false);
      }
      foreach (MyDefinitionBase allDefinition in MyDefinitionManager.Static.GetAllDefinitions())
      {
        if ((!(allDefinition.Id.TypeId != typeof (MyObjectBuilder_Component)) || !(allDefinition.Id.TypeId != typeof (MyObjectBuilder_Ingot))) && (!flag2 || !(allDefinition.Id.TypeId != typeof (MyObjectBuilder_Component))) && (!flag2 || (double) ((MyPhysicalItemDefinition) allDefinition).Volume <= 0.0500000007450581))
        {
          MyObjectBuilder_PhysicalObject newObject = (MyObjectBuilder_PhysicalObject) MyObjectBuilderSerializer.CreateNewObject(allDefinition.Id.TypeId);
          newObject.SubtypeName = allDefinition.Id.SubtypeName;
          if (!this.AddItems(inventory, newObject, overrideCheck, (MyFixedPoint) 1) & flag1)
          {
            MatrixD headMatrix = MySession.Static.ControlledEntity.GetHeadMatrix(true);
            Matrix matrix = (Matrix) ref headMatrix;
            MyFloatingObjects.Spawn(new MyPhysicalInventoryItem((MyFixedPoint) 1, newObject), (Vector3D) (matrix.Translation + matrix.Forward * 0.2f), (Vector3D) matrix.Forward, (Vector3D) matrix.Up, MySession.Static.ControlledEntity.Entity.Physics);
          }
        }
      }
      if (flag2)
        return;
      string[] outNames;
      MyDefinitionManager.Static.GetOreTypeNames(out outNames);
      foreach (string subtypeName in outNames)
      {
        MyObjectBuilder_Ore newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Ore>(subtypeName);
        if (!this.AddItems(inventory, (MyObjectBuilder_PhysicalObject) newObject, overrideCheck, (MyFixedPoint) 1) & flag1)
        {
          MatrixD headMatrix = MySession.Static.ControlledEntity.GetHeadMatrix(true);
          Matrix matrix = (Matrix) ref headMatrix;
          MyFloatingObjects.Spawn(new MyPhysicalInventoryItem((MyFixedPoint) 1, (MyObjectBuilder_PhysicalObject) newObject), (Vector3D) (matrix.Translation + matrix.Forward * 0.2f), (Vector3D) matrix.Forward, (Vector3D) matrix.Up, MySession.Static.ControlledEntity.Entity.Physics);
        }
      }
    }

    private MyObjectBuilder_PhysicalGunObject CreateGunContent(
      string subtypeName)
    {
      return (MyObjectBuilder_PhysicalGunObject) MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_PhysicalGunObject), subtypeName));
    }
  }
}
