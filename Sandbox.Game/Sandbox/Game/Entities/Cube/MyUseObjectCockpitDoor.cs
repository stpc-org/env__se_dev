// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyUseObjectCockpitDoor
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
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

namespace Sandbox.Game.Entities.Cube
{
  [MyUseObject("cockpit")]
  internal class MyUseObjectCockpitDoor : MyUseObjectBase
  {
    public readonly IMyEntity Cockpit;
    public readonly Matrix LocalMatrix;

    public MyUseObjectCockpitDoor(
      IMyEntity owner,
      string dummyName,
      MyModelDummy dummyData,
      uint key)
      : base(owner, dummyData)
    {
      this.Cockpit = owner;
      this.LocalMatrix = dummyData.Matrix;
    }

    public override float InteractiveDistance => MyConstants.DEFAULT_INTERACTIVE_DISTANCE;

    public override MatrixD ActivationMatrix => this.LocalMatrix * this.Cockpit.WorldMatrix;

    public override MatrixD WorldMatrix => this.Cockpit.WorldMatrix;

    public override uint RenderObjectID => this.Cockpit.Render.GetRenderObjectID();

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

    public override UseActionEnum PrimaryAction => UseActionEnum.Manipulate;

    public override UseActionEnum SecondaryAction => UseActionEnum.None;

    public override void Use(UseActionEnum actionEnum, IMyEntity entity)
    {
      MyCockpit cockpit = this.Cockpit as MyCockpit;
      MyCharacter user = entity as MyCharacter;
      switch (actionEnum)
      {
        case UseActionEnum.Manipulate:
          cockpit?.RequestUse(actionEnum, user);
          break;
        case UseActionEnum.OpenInventory:
          if (cockpit == null)
            break;
          if (!cockpit.GetUserRelationToOwner(user.ControllerInfo.Controller.Player.Identity.IdentityId).IsFriendly() && !MySession.Static.AdminSettings.HasFlag((Enum) AdminSettingsEnum.UseTerminals))
          {
            if (!user.ControllerInfo.IsLocallyHumanControlled())
              break;
            MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
            break;
          }
          MyGuiScreenTerminal.Show(MyTerminalPageEnum.Inventory, user, this.Owner as MyEntity);
          break;
      }
    }

    public override MyActionDescription GetActionInfo(UseActionEnum actionEnum)
    {
      string str = this.Owner is MyCubeBlock owner ? owner.DefinitionDisplayNameText : ((MyEntity) this.Owner).DisplayNameText;
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      MyStringId context = controlledEntity != null ? controlledEntity.ControlContext : MySpaceBindingCreator.CX_BASE;
      if (actionEnum == UseActionEnum.Manipulate || actionEnum != UseActionEnum.OpenInventory)
        return new MyActionDescription()
        {
          Text = MySpaceTexts.NotificationHintPressToEnterCockpit,
          FormatParams = new object[2]
          {
            (object) ("[" + MyGuiSandbox.GetKeyName(MyControlsSpace.USE) + "]"),
            (object) ((MyEntity) this.Cockpit).DisplayNameText
          },
          IsTextControlHint = true,
          JoystickText = new MyStringId?(MySpaceTexts.NotificationHintPressToEnterCockpit),
          JoystickFormatParams = new object[2]
          {
            (object) MyControllerHelper.GetCodeForControl(context, MyControlsSpace.USE),
            (object) ((MyEntity) this.Cockpit).DisplayNameText
          },
          ShowForGamepad = true
        };
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
        JoystickFormatParams = new object[1]{ (object) str },
        ShowForGamepad = true
      };
    }

    public override bool ContinuousUsage => false;

    public override bool HandleInput() => false;

    public override void OnSelectionLost()
    {
    }

    public override bool PlayIndicatorSound => !(this.Cockpit is MyShipController) || (this.Cockpit as MyShipController).PlayDefaultUseSound;
  }
}
