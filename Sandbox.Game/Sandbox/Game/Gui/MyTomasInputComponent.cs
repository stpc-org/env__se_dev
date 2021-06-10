// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyTomasInputComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Input;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public class MyTomasInputComponent : MyDebugComponent
  {
    public static float USE_WHEEL_ANIMATION_SPEED = 1f;
    private long m_previousSpectatorGridId;
    public static string ClipboardText = string.Empty;

    public override string GetName() => "Tomas";

    public MyTomasInputComponent()
    {
      this.AddShortcut(MyKeys.Delete, true, true, false, false, (Func<string>) (() => "Delete all characters"), (Func<bool>) (() =>
      {
        foreach (MyCharacter myCharacter in Sandbox.Game.Entities.MyEntities.GetEntities().OfType<MyCharacter>())
        {
          if (myCharacter == MySession.Static.ControlledEntity)
            MySession.Static.SetCameraController(MyCameraControllerEnum.Spectator, (IMyEntity) null, new Vector3D?());
          myCharacter.Close();
        }
        foreach (MyCubeGrid myCubeGrid in Sandbox.Game.Entities.MyEntities.GetEntities().OfType<MyCubeGrid>())
        {
          foreach (MySlimBlock block in myCubeGrid.GetBlocks())
          {
            if (block.FatBlock is MyCockpit)
            {
              MyCockpit fatBlock = block.FatBlock as MyCockpit;
              if (fatBlock.Pilot != null)
                fatBlock.Pilot.Close();
            }
          }
        }
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad4, true, false, false, false, (Func<string>) (() => "Spawn cargo ship or barbarians"), (Func<bool>) (() =>
      {
        MyGlobalEventBase globalEvent = MyGlobalEvents.GetEventById(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GlobalEventBase), "SpawnCargoShip")) ?? MyGlobalEvents.GetEventById(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GlobalEventBase), "SpawnBarbarians"));
        if (globalEvent != null)
        {
          MyGlobalEvents.RemoveGlobalEvent(globalEvent);
          globalEvent.SetActivationTime(TimeSpan.FromSeconds(1.0));
          MyGlobalEvents.AddGlobalEvent(globalEvent);
        }
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad5, true, false, false, false, (Func<string>) (() => "Spawn random meteor"), (Func<bool>) (() =>
      {
        MyMeteor.SpawnRandom(MySector.MainCamera.Position + MySector.MainCamera.ForwardVector * 20f + MySector.DirectionToSunNormalized * 1000f, -MySector.DirectionToSunNormalized);
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad8, true, false, false, false, (Func<string>) (() => "Switch control to next entity"), (Func<bool>) (() =>
      {
        if (MySession.Static.ControlledEntity != null)
        {
          switch (MySession.Static.GetCameraControllerEnum())
          {
            case MyCameraControllerEnum.Entity:
            case MyCameraControllerEnum.ThirdPersonSpectator:
              List<MyEntity> list = Sandbox.Game.Entities.MyEntities.GetEntities().ToList<MyEntity>();
              int num = list.IndexOf(MySession.Static.ControlledEntity.Entity);
              List<MyEntity> myEntityList = new List<MyEntity>();
              if (num + 1 < list.Count)
                myEntityList.AddRange((IEnumerable<MyEntity>) list.GetRange(num + 1, list.Count - num - 1));
              if (num != -1)
                myEntityList.AddRange((IEnumerable<MyEntity>) list.GetRange(0, num + 1));
              MyCharacter myCharacter1 = (MyCharacter) null;
              for (int index = 0; index < myEntityList.Count; ++index)
              {
                if (myEntityList[index] is MyCharacter myCharacter2)
                {
                  myCharacter1 = myCharacter2;
                  break;
                }
              }
              if (myCharacter1 != null)
              {
                MySession.Static.LocalHumanPlayer.Controller.TakeControl((IMyControllableEntity) myCharacter1);
                break;
              }
              break;
            default:
              MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, (IMyEntity) MySession.Static.ControlledEntity.Entity, new Vector3D?());
              break;
          }
        }
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad7, true, false, false, false, (Func<string>) (() => "Use next ship"), (Func<bool>) (() =>
      {
        MyCharacterInputComponent.UseNextShip();
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad9, true, false, false, false, (Func<string>) (() => "Debug new grid screen"), (Func<bool>) (() =>
      {
        MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyTomasInputComponent.DebugNewGridScreen());
        return true;
      }));
      this.AddShortcut(MyKeys.N, true, false, false, false, (Func<string>) (() => "Refill all batteries"), (Func<bool>) (() =>
      {
        foreach (MyEntity entity in Sandbox.Game.Entities.MyEntities.GetEntities())
        {
          if (entity is MyCubeGrid myCubeGrid)
          {
            foreach (MySlimBlock block in myCubeGrid.GetBlocks())
            {
              if (block.FatBlock is MyBatteryBlock fatBlock)
                fatBlock.CurrentStoredPower = fatBlock.MaxStoredPower;
            }
          }
        }
        return true;
      }));
      this.AddShortcut(MyKeys.U, true, false, false, false, (Func<string>) (() => "Spawn new character"), (Func<bool>) (() =>
      {
        MyCharacterInputComponent.SpawnCharacter();
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad2, true, false, false, false, (Func<string>) (() => "Merge static grids"), (Func<bool>) (() =>
      {
        HashSet<MyCubeGrid> myCubeGridSet = new HashSet<MyCubeGrid>();
        bool flag;
        do
        {
          flag = false;
          foreach (MyEntity entity in Sandbox.Game.Entities.MyEntities.GetEntities())
          {
            if (entity is MyCubeGrid myCubeGrid && myCubeGrid.IsStatic && myCubeGrid.GridSizeEnum == MyCubeSize.Large)
            {
              if (!myCubeGridSet.Contains(myCubeGrid))
              {
                foreach (MySlimBlock block in myCubeGrid.GetBlocks().ToList<MySlimBlock>())
                {
                  MyCubeGrid myCubeGrid = myCubeGrid.DetectMerge(block);
                  if (myCubeGrid != null)
                  {
                    flag = true;
                    if (myCubeGrid != myCubeGrid)
                      break;
                  }
                }
                if (!flag)
                  myCubeGridSet.Add(myCubeGrid);
              }
              else
                continue;
            }
            if (flag)
              break;
          }
        }
        while (flag);
        return true;
      }));
      this.AddShortcut(MyKeys.Add, true, false, false, false, (Func<string>) (() => "Increase wheel animation speed"), (Func<bool>) (() =>
      {
        MyTomasInputComponent.USE_WHEEL_ANIMATION_SPEED += 0.05f;
        return true;
      }));
      this.AddShortcut(MyKeys.Subtract, true, false, false, false, (Func<string>) (() => "Decrease wheel animation speed"), (Func<bool>) (() =>
      {
        MyTomasInputComponent.USE_WHEEL_ANIMATION_SPEED -= 0.05f;
        return true;
      }));
      this.AddShortcut(MyKeys.Divide, true, false, false, false, (Func<string>) (() => "Show model texture names"), (Func<bool>) (() =>
      {
        MyFakes.ENABLE_DEBUG_DRAW_TEXTURE_NAMES = !MyFakes.ENABLE_DEBUG_DRAW_TEXTURE_NAMES;
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad1, true, false, false, false, (Func<string>) (() => "Throw from spectator: " + MySessionComponentThrower.USE_SPECTATOR_FOR_THROW.ToString()), (Func<bool>) (() =>
      {
        MySessionComponentThrower.USE_SPECTATOR_FOR_THROW = !MySessionComponentThrower.USE_SPECTATOR_FOR_THROW;
        return true;
      }));
      this.AddShortcut(MyKeys.F2, true, false, false, false, (Func<string>) (() => "Spectator to next small grid"), (Func<bool>) (() => this.SpectatorToNextGrid(MyCubeSize.Small)));
      this.AddShortcut(MyKeys.F3, true, false, false, false, (Func<string>) (() => "Spectator to next large grid"), (Func<bool>) (() => this.SpectatorToNextGrid(MyCubeSize.Large)));
      this.AddShortcut(MyKeys.Multiply, true, false, false, false, (Func<string>) (() => "Show model texture names"), new Func<bool>(this.CopyAssetToClipboard));
    }

    private bool CopyAssetToClipboard()
    {
      if (!string.IsNullOrEmpty(MyTomasInputComponent.ClipboardText))
        MyVRage.Platform.System.Clipboard = MyTomasInputComponent.ClipboardText;
      return true;
    }

    public override bool HandleInput() => MySession.Static != null && base.HandleInput();

    public bool SpectatorToNextGrid(MyCubeSize size)
    {
      MyCubeGrid myCubeGrid1 = (MyCubeGrid) null;
      MyCubeGrid myCubeGrid2 = (MyCubeGrid) null;
      foreach (MyEntity entity in Sandbox.Game.Entities.MyEntities.GetEntities())
      {
        if (entity is MyCubeGrid myCubeGrid3 && myCubeGrid3.GridSizeEnum == size)
        {
          if (this.m_previousSpectatorGridId == 0L)
          {
            myCubeGrid1 = myCubeGrid3;
            break;
          }
          if (myCubeGrid2 != null)
          {
            myCubeGrid1 = myCubeGrid3;
            break;
          }
          if (myCubeGrid3.EntityId == this.m_previousSpectatorGridId)
            myCubeGrid2 = myCubeGrid3;
          if (myCubeGrid1 == null)
            myCubeGrid1 = myCubeGrid3;
        }
      }
      if (myCubeGrid1 == null)
        return false;
      BoundingSphere worldVolume = (BoundingSphere) myCubeGrid1.PositionComp.WorldVolume;
      Vector3D vector3D = Vector3D.Transform(Vector3D.Forward, MySpectator.Static.Orientation);
      MySpectator.Static.Position = myCubeGrid1.PositionComp.GetPosition() - vector3D * (double) worldVolume.Radius * 2.0;
      this.m_previousSpectatorGridId = myCubeGrid1.EntityId;
      return true;
    }

    private class DebugNewGridScreen : MyGuiScreenBase
    {
      private MyGuiControlCombobox m_sizeCombobox;
      private MyGuiControlCheckbox m_staticCheckbox;

      public override string GetFriendlyName() => nameof (DebugNewGridScreen);

      public DebugNewGridScreen()
        : base()
      {
        this.EnabledBackgroundFade = true;
        this.RecreateControls(true);
      }

      public override void RecreateControls(bool constructor)
      {
        base.RecreateControls(constructor);
        MyGuiControlCombobox guiControlCombobox = new MyGuiControlCombobox();
        guiControlCombobox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
        guiControlCombobox.Position = Vector2.Zero;
        this.m_sizeCombobox = guiControlCombobox;
        foreach (object enumValue in typeof (MyCubeSize).GetEnumValues())
          this.m_sizeCombobox.AddItem((long) (MyCubeSize) enumValue, new StringBuilder(enumValue.ToString()));
        this.m_sizeCombobox.SelectItemByKey(0L);
        MyGuiControlCheckbox guiControlCheckbox = new MyGuiControlCheckbox();
        guiControlCheckbox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
        guiControlCheckbox.IsChecked = true;
        this.m_staticCheckbox = guiControlCheckbox;
        MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
        myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
        myGuiControlLabel1.Position = new Vector2(this.m_staticCheckbox.Size.X, 0.0f);
        myGuiControlLabel1.Text = "Static grid";
        MyGuiControlLabel myGuiControlLabel2 = myGuiControlLabel1;
        MyGuiControlButton guiControlButton1 = new MyGuiControlButton();
        guiControlButton1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
        guiControlButton1.Text = "Ok";
        guiControlButton1.Position = new Vector2(0.0f, 0.05f);
        MyGuiControlButton guiControlButton2 = guiControlButton1;
        guiControlButton2.ButtonClicked += new Action<MyGuiControlButton>(this.okButton_ButtonClicked);
        this.Elements.Add((MyGuiControlBase) this.m_sizeCombobox);
        this.Elements.Add((MyGuiControlBase) this.m_staticCheckbox);
        this.Elements.Add((MyGuiControlBase) myGuiControlLabel2);
        this.Elements.Add((MyGuiControlBase) guiControlButton2);
      }

      private void okButton_ButtonClicked(MyGuiControlButton obj)
      {
        MyCubeBuilder.Static.StartStaticGridPlacement((MyCubeSize) this.m_sizeCombobox.GetSelectedKey(), this.m_staticCheckbox.IsChecked);
        this.CloseScreen();
      }
    }
  }
}
