// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.UseObjects.MyUseObjectLifeSupportingBlock
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using SpaceEngineers.Game.EntityComponents.GameLogic;
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

namespace SpaceEngineers.Game.Entities.UseObjects
{
  [MyUseObject("block")]
  internal class MyUseObjectLifeSupportingBlock : MyUseObjectBase
  {
    private Matrix m_localMatrix;

    public IMyLifeSupportingBlock Owner => !(base.Owner is IMyLifeSupportingBlock owner) ? (IMyLifeSupportingBlock) null : owner;

    public MyUseObjectLifeSupportingBlock(
      IMyEntity owner,
      string dummyName,
      MyModelDummy dummyData,
      uint key)
      : base(owner, dummyData)
    {
      this.m_localMatrix = dummyData.Matrix;
    }

    public override float InteractiveDistance => MyConstants.DEFAULT_INTERACTIVE_DISTANCE;

    public override MatrixD ActivationMatrix => this.m_localMatrix * ((IMyEntity) this.Owner).WorldMatrix;

    public override MatrixD WorldMatrix => ((IMyEntity) this.Owner).WorldMatrix;

    public override uint RenderObjectID
    {
      get
      {
        IMyLifeSupportingBlock owner = this.Owner;
        if (owner == null)
          return uint.MaxValue;
        uint[] renderObjectIds = owner.Render.RenderObjectIDs;
        return renderObjectIds.Length != 0 ? renderObjectIds[0] : uint.MaxValue;
      }
    }

    public override int InstanceID => -1;

    public override bool ShowOverlay => true;

    public override UseActionEnum SupportedActions
    {
      get
      {
        UseActionEnum useActionEnum = this.PrimaryAction | this.SecondaryAction;
        IMyLifeSupportingBlock owner = this.Owner;
        if (owner != null && owner.HasInventory)
          useActionEnum |= UseActionEnum.OpenInventory;
        return useActionEnum;
      }
    }

    public override UseActionEnum PrimaryAction => UseActionEnum.Manipulate;

    public override UseActionEnum SecondaryAction => UseActionEnum.OpenTerminal;

    public override bool ContinuousUsage => true;

    public override void Use(UseActionEnum actionEnum, IMyEntity entity)
    {
      if (!(entity is MyCharacter myCharacter))
        return;
      MyPlayer.GetPlayerFromCharacter(myCharacter);
      IMyLifeSupportingBlock owner = this.Owner;
      if (owner == null)
        return;
      if (!((IMyCubeBlock) owner).GetUserRelationToOwner(myCharacter.ControllerInfo.Controller.Player.Identity.IdentityId).IsFriendly() && !MySession.Static.AdminSettings.HasFlag((Enum) AdminSettingsEnum.UseTerminals))
      {
        if (myCharacter.ControllerInfo.Controller.Player != MySession.Static.LocalHumanPlayer)
          return;
        MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
      }
      else
      {
        switch (actionEnum)
        {
          case UseActionEnum.Manipulate:
            ((IMyEntity) this.Owner).Components.Get<MyLifeSupportingComponent>().OnSupportRequested(myCharacter);
            break;
          case UseActionEnum.OpenTerminal:
            this.Owner.ShowTerminal(myCharacter);
            break;
          case UseActionEnum.OpenInventory:
            MyGuiScreenTerminal.Show(MyTerminalPageEnum.Inventory, myCharacter, this.Owner as MyEntity);
            break;
        }
      }
    }

    public override MyActionDescription GetActionInfo(UseActionEnum actionEnum)
    {
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      MyStringId context = controlledEntity != null ? controlledEntity.ControlContext : MySpaceBindingCreator.CX_BASE;
      switch (actionEnum)
      {
        case UseActionEnum.Manipulate:
          return new MyActionDescription()
          {
            Text = MySpaceTexts.NotificationHintPressToRechargeInMedicalRoom,
            FormatParams = new object[1]
            {
              (object) ("[" + (object) MyInput.Static.GetGameControl(MyControlsSpace.USE) + "]")
            },
            IsTextControlHint = true,
            JoystickText = new MyStringId?(MySpaceTexts.NotificationHintPressToRechargeInMedicalRoom),
            JoystickFormatParams = new object[1]
            {
              (object) MyControllerHelper.GetCodeForControl(context, MyControlsSpace.USE)
            },
            ShowForGamepad = true
          };
        case UseActionEnum.OpenTerminal:
          return new MyActionDescription()
          {
            Text = MySpaceTexts.NotificationHintPressToOpenTerminal,
            FormatParams = new object[1]
            {
              (object) ("[" + (object) MyInput.Static.GetGameControl(MyControlsSpace.TERMINAL) + "]")
            },
            IsTextControlHint = true,
            JoystickText = new MyStringId?(MySpaceTexts.NotificationHintPressToOpenTerminal),
            JoystickFormatParams = new object[1]
            {
              (object) MyControllerHelper.GetCodeForControl(context, MyControlsSpace.TERMINAL)
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
              this.Owner is MyCubeBlock ? (object) ((MyCubeBlock) this.Owner).DefinitionDisplayNameText : (object) this.Owner.DisplayNameText
            },
            IsTextControlHint = true,
            JoystickText = new MyStringId?(MySpaceTexts.NotificationHintPressToOpenInventory),
            JoystickFormatParams = new object[2]
            {
              (object) MyControllerHelper.GetCodeForControl(context, MyControlsSpace.INVENTORY),
              this.Owner is MyCubeBlock ? (object) ((MyCubeBlock) this.Owner).DefinitionDisplayNameText : (object) this.Owner.DisplayNameText
            },
            ShowForGamepad = true
          };
        default:
          return new MyActionDescription()
          {
            Text = MySpaceTexts.NotificationHintPressToOpenTerminal,
            FormatParams = new object[1]
            {
              (object) MyInput.Static.GetGameControl(MyControlsSpace.TERMINAL)
            },
            IsTextControlHint = true,
            ShowForGamepad = true
          };
      }
    }

    public override bool HandleInput() => false;

    public override void OnSelectionLost()
    {
    }

    public override bool PlayIndicatorSound => true;
  }
}
