// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.UseObjects.VendingMachine.MyUseObjectVendingMachineScreen
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.ViewModels;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using SpaceEngineers.Game.GUI;
using System;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.ModAPI;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace SpaceEngineers.Game.Entities.UseObjects.VendingMachine
{
  [MyUseObject("vendingMachine")]
  public class MyUseObjectVendingMachineScreen : MyUseObjectBase
  {
    private MyGuiScreenStoreBlock m_screen;

    public override MatrixD ActivationMatrix => this.Dummy.Matrix * this.Owner.WorldMatrix;

    public override MatrixD WorldMatrix => this.Owner.WorldMatrix;

    public override uint RenderObjectID => this.Owner.Render.GetRenderObjectID();

    public override int InstanceID => -1;

    public override bool ShowOverlay => true;

    public override UseActionEnum SupportedActions
    {
      get
      {
        UseActionEnum useActionEnum = this.PrimaryAction | this.SecondaryAction;
        if (this.Owner != null && this.Owner.HasInventory)
          useActionEnum |= UseActionEnum.OpenInventory;
        return useActionEnum;
      }
    }

    public override UseActionEnum PrimaryAction
    {
      get
      {
        UseActionEnum useActionEnum = UseActionEnum.Manipulate;
        if (this.Owner != null && !this.Owner.HasInventory)
          useActionEnum &= ~UseActionEnum.Manipulate;
        return useActionEnum;
      }
    }

    public override UseActionEnum SecondaryAction => UseActionEnum.OpenTerminal;

    public override bool ContinuousUsage => false;

    public override bool PlayIndicatorSound => true;

    public override float InteractiveDistance => MyConstants.DEFAULT_INTERACTIVE_DISTANCE;

    public MyUseObjectVendingMachineScreen(
      IMyEntity owner,
      string dummyName,
      MyModelDummy dummyData,
      uint key)
      : base(owner, dummyData)
    {
    }

    public override MyActionDescription GetActionInfo(UseActionEnum actionEnum)
    {
      string str = this.Owner is MyCubeBlock owner ? owner.DefinitionDisplayNameText : ((MyEntity) this.Owner).DisplayNameText;
      switch (actionEnum)
      {
        case UseActionEnum.Manipulate:
          return new MyActionDescription()
          {
            Text = MySpaceTexts.NotificationHintPressToOpenStore,
            FormatParams = new object[1]
            {
              (object) ("[" + (object) MyInput.Static.GetGameControl(MyControlsSpace.USE) + "]")
            },
            IsTextControlHint = true,
            JoystickFormatParams = new object[1]
            {
              (object) (MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.USE) + "]")
            },
            ShowForGamepad = true
          };
        case UseActionEnum.OpenInventory:
          return new MyActionDescription()
          {
            Text = MySpaceTexts.NotificationHintPressToOpenInventory,
            FormatParams = new object[2]
            {
              (object) ("[" + (object) MyInput.Static.GetGameControl(MyControlsSpace.INVENTORY) + "]"),
              (object) str
            },
            IsTextControlHint = true,
            JoystickText = new MyStringId?(MySpaceTexts.NotificationHintJoystickPressToOpenInventory),
            JoystickFormatParams = new object[1]
            {
              (object) str
            },
            ShowForGamepad = true
          };
        default:
          return new MyActionDescription()
          {
            Text = MySpaceTexts.NotificationHintPressToOpenTerminal,
            FormatParams = new object[2]
            {
              (object) ("[" + (object) MyInput.Static.GetGameControl(MyControlsSpace.TERMINAL) + "]"),
              (object) str
            },
            IsTextControlHint = true,
            JoystickText = new MyStringId?(MySpaceTexts.NotificationHintJoystickPressToOpenControlPanel),
            JoystickFormatParams = new object[1]
            {
              (object) str
            },
            ShowForGamepad = true
          };
      }
    }

    public override bool HandleInput() => false;

    public override void OnSelectionLost()
    {
    }

    public override void Use(UseActionEnum actionEnum, IMyEntity userEntity)
    {
      if (!(userEntity is MyCharacter myCharacter))
        return;
      MyPlayer.GetPlayerFromCharacter(myCharacter);
      if (!(this.Owner is MyVendingMachine owner))
        return;
      MyRelationsBetweenPlayerAndBlock userRelationToOwner = owner.GetUserRelationToOwner(myCharacter.ControllerInfo.Controller.Player.Identity.IdentityId);
      switch (actionEnum)
      {
        case UseActionEnum.Manipulate:
          if ((userRelationToOwner == MyRelationsBetweenPlayerAndBlock.Owner ? 1 : (MySession.Static.AdminSettings.HasFlag((Enum) AdminSettingsEnum.UseTerminals) ? 1 : 0)) == 0)
          {
            MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
            break;
          }
          if (!owner.IsWorking || this.m_screen != null)
            break;
          this.m_screen = MyGuiSandbox.CreateScreen<MyGuiScreenStoreBlock>((object) new MyStoreBlockViewModel((MyStoreBlock) owner)
          {
            IsBuyTabVisible = false,
            IsSellTabVisible = false,
            AdministrationViewModel = {
              IsTabNewOrderVisible = false
            }
          });
          this.m_screen.Closed += new MyGuiScreenBase.ScreenHandler(this.Screen_Closed);
          MyGuiSandbox.AddScreen((MyGuiScreenBase) this.m_screen);
          break;
        case UseActionEnum.OpenTerminal:
          if (!userRelationToOwner.IsFriendly() && !MySession.Static.AdminSettings.HasFlag((Enum) AdminSettingsEnum.UseTerminals))
          {
            if (!myCharacter.ControllerInfo.IsLocallyHumanControlled())
              break;
            MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
            break;
          }
          MyGuiScreenTerminal.Show(MyTerminalPageEnum.ControlPanel, myCharacter, (MyEntity) this.Owner);
          break;
        case UseActionEnum.OpenInventory:
          if (!userRelationToOwner.IsFriendly() && !MySession.Static.AdminSettings.HasFlag((Enum) AdminSettingsEnum.UseTerminals))
          {
            if (!myCharacter.ControllerInfo.IsLocallyHumanControlled())
              break;
            MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
            break;
          }
          MyGuiScreenTerminal.Show(MyTerminalPageEnum.Inventory, myCharacter, this.Owner as MyEntity);
          break;
      }
    }

    private void Screen_Closed(MyGuiScreenBase source, bool isUnloading)
    {
      this.m_screen = (MyGuiScreenStoreBlock) null;
      if (!(this.Owner is MyVendingMachine owner))
        return;
      owner.UpdateStoreContent();
    }
  }
}
