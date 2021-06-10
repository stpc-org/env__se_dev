// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.UseObjects.MyUseObjectInventory
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens;
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

namespace SpaceEngineers.Game.Entities.UseObjects
{
  [MyUseObject("inventory")]
  [MyUseObject("conveyor")]
  internal class MyUseObjectInventory : MyUseObjectBase
  {
    public readonly MyEntity Entity;
    public readonly Matrix LocalMatrix;

    public MyUseObjectInventory(
      IMyEntity owner,
      string dummyName,
      MyModelDummy dummyData,
      uint key)
      : base(owner, dummyData)
    {
      this.Entity = owner as MyEntity;
      this.LocalMatrix = dummyData.Matrix;
    }

    public override float InteractiveDistance => MyConstants.DEFAULT_INTERACTIVE_DISTANCE;

    public override MatrixD ActivationMatrix => this.LocalMatrix * this.Entity.WorldMatrix;

    public override MatrixD WorldMatrix => this.Entity.WorldMatrix;

    public override uint RenderObjectID => this.Entity.Render.GetRenderObjectID();

    public override int InstanceID => -1;

    public override bool ShowOverlay => true;

    public override UseActionEnum SupportedActions => this.PrimaryAction | this.SecondaryAction | UseActionEnum.BuildPlanner;

    public override UseActionEnum PrimaryAction => UseActionEnum.OpenInventory;

    public override UseActionEnum SecondaryAction => UseActionEnum.OpenTerminal;

    public override void Use(UseActionEnum actionEnum, IMyEntity entity)
    {
      MyCharacter user = entity as MyCharacter;
      if (this.Entity is MyCubeBlock entity1 && !entity1.GetUserRelationToOwner(user.ControllerInfo.ControllingIdentityId).IsFriendly() && !MySession.Static.AdminSettings.HasFlag((Enum) AdminSettingsEnum.UseTerminals))
      {
        if (!user.ControllerInfo.IsLocallyHumanControlled())
          return;
        MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
      }
      else
      {
        MyContainerDropComponent component;
        if (this.Entity.Components.TryGet<MyContainerDropComponent>(out component))
          MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = (MyGuiScreenBase) new MyGuiScreenClaimGameItem(component, user.GetPlayerIdentityId()));
        else
          MyGuiScreenTerminal.Show(actionEnum == UseActionEnum.OpenTerminal ? MyTerminalPageEnum.ControlPanel : MyTerminalPageEnum.Inventory, user, this.Entity);
      }
    }

    public override MyActionDescription GetActionInfo(UseActionEnum actionEnum)
    {
      string str = this.Entity is MyCubeBlock entity ? entity.DefinitionDisplayNameText : this.Entity.DisplayNameText;
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      MyStringId context = controlledEntity != null ? controlledEntity.ControlContext : MySpaceBindingCreator.CX_BASE;
      if (actionEnum != UseActionEnum.OpenTerminal)
      {
        if (actionEnum != UseActionEnum.OpenInventory)
        {
          if (actionEnum == UseActionEnum.BuildPlanner)
            return new MyActionDescription()
            {
              Text = MySpaceTexts.NotificationHintPressToWithdraw,
              FormatParams = new object[2]
              {
                (object) ("[" + (object) MyInput.Static.GetGameControl(MyControlsSpace.BUILD_PLANNER) + "]"),
                (object) str
              },
              IsTextControlHint = true,
              JoystickText = new MyStringId?(MySpaceTexts.NotificationHintPressToWithdraw),
              JoystickFormatParams = new object[2]
              {
                (object) MyControllerHelper.GetCodeForControl(context, MyControlsSpace.BUILD_PLANNER_WITHDRAW_COMPONENTS),
                (object) str
              },
              ShowForGamepad = true
            };
        }
        else
          return new MyActionDescription()
          {
            Text = MySpaceTexts.NotificationHintPressToOpenInventory,
            FormatParams = new object[2]
            {
              (object) ("[" + (object) MyInput.Static.GetGameControl(MyControlsSpace.INVENTORY) + "]"),
              (object) str
            },
            IsTextControlHint = true,
            JoystickText = new MyStringId?(MySpaceTexts.NotificationHintPressToOpenInventory),
            JoystickFormatParams = new object[2]
            {
              (object) MyControllerHelper.GetCodeForControl(context, MyControlsSpace.INVENTORY),
              (object) str
            },
            ShowForGamepad = true
          };
      }
      return new MyActionDescription()
      {
        Text = MySpaceTexts.NotificationHintPressToOpenTerminal,
        FormatParams = new object[2]
        {
          (object) ("[" + (object) MyInput.Static.GetGameControl(MyControlsSpace.TERMINAL) + "]"),
          (object) str
        },
        IsTextControlHint = true,
        JoystickText = new MyStringId?(MySpaceTexts.NotificationHintPressToOpenTerminal),
        JoystickFormatParams = new object[2]
        {
          (object) MyControllerHelper.GetCodeForControl(context, MyControlsSpace.TERMINAL),
          (object) str
        },
        ShowForGamepad = true
      };
    }

    public override bool ContinuousUsage => false;

    public override bool HandleInput() => false;

    public override void OnSelectionLost()
    {
    }

    public override bool PlayIndicatorSound => true;
  }
}
