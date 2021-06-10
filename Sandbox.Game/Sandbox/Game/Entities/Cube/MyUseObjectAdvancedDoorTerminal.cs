// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyUseObjectAdvancedDoorTerminal
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
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
  [MyUseObject("advanceddoor")]
  public class MyUseObjectAdvancedDoorTerminal : MyUseObjectBase
  {
    public readonly MyAdvancedDoor Door;
    public readonly Matrix LocalMatrix;

    public MyUseObjectAdvancedDoorTerminal(
      IMyEntity owner,
      string dummyName,
      MyModelDummy dummyData,
      uint key)
      : base(owner, dummyData)
    {
      this.Door = (MyAdvancedDoor) owner;
      this.LocalMatrix = dummyData.Matrix;
    }

    public override float InteractiveDistance => MyConstants.DEFAULT_INTERACTIVE_DISTANCE;

    public override MatrixD ActivationMatrix => this.LocalMatrix * this.Door.WorldMatrix;

    public override MatrixD WorldMatrix => this.Door.WorldMatrix;

    public override uint RenderObjectID => this.Door.Render.GetRenderObjectID();

    public override int InstanceID => -1;

    public override bool ShowOverlay => true;

    public override UseActionEnum SupportedActions => this.PrimaryAction | this.SecondaryAction;

    public override UseActionEnum PrimaryAction => UseActionEnum.Manipulate;

    public override UseActionEnum SecondaryAction => UseActionEnum.OpenTerminal;

    public override void Use(UseActionEnum actionEnum, IMyEntity entity)
    {
      MyCharacter user = entity as MyCharacter;
      MyRelationsBetweenPlayerAndBlock userRelationToOwner = this.Door.GetUserRelationToOwner(user.ControllerInfo.ControllingIdentityId);
      switch (actionEnum)
      {
        case UseActionEnum.Manipulate:
          if (!this.Door.AnyoneCanUse && !this.Door.HasLocalPlayerAccess())
          {
            MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
            break;
          }
          this.Door.SetOpenRequest(!this.Door.Open, user.ControllerInfo.ControllingIdentityId);
          break;
        case UseActionEnum.OpenTerminal:
          if (!userRelationToOwner.IsFriendly() && !MySession.Static.AdminSettings.HasFlag((Enum) AdminSettingsEnum.UseTerminals))
          {
            if (!user.ControllerInfo.IsLocallyHumanControlled())
              break;
            MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
            break;
          }
          MyGuiScreenTerminal.Show(MyTerminalPageEnum.ControlPanel, user, (MyEntity) this.Door);
          break;
      }
    }

    public override MyActionDescription GetActionInfo(UseActionEnum actionEnum)
    {
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      MyStringId context = controlledEntity != null ? controlledEntity.ControlContext : MySpaceBindingCreator.CX_BASE;
      if (actionEnum != UseActionEnum.Manipulate)
      {
        if (actionEnum == UseActionEnum.OpenTerminal)
          return new MyActionDescription()
          {
            Text = MySpaceTexts.NotificationHintPressToOpenControlPanel,
            FormatParams = new object[2]
            {
              (object) ("[" + (object) MyInput.Static.GetGameControl(MyControlsSpace.TERMINAL) + "]"),
              (object) this.Door.DefinitionDisplayNameText
            },
            IsTextControlHint = true,
            JoystickText = new MyStringId?(MySpaceTexts.NotificationHintPressToOpenControlPanel),
            JoystickFormatParams = new object[2]
            {
              (object) MyControllerHelper.GetCodeForControl(context, MyControlsSpace.TERMINAL),
              (object) this.Door.DefinitionDisplayNameText
            },
            ShowForGamepad = true
          };
        return new MyActionDescription()
        {
          Text = MySpaceTexts.NotificationHintPressToOpenControlPanel,
          FormatParams = new object[2]
          {
            (object) MyInput.Static.GetGameControl(MyControlsSpace.TERMINAL),
            (object) this.Door.DefinitionDisplayNameText
          },
          IsTextControlHint = true,
          ShowForGamepad = true
        };
      }
      return new MyActionDescription()
      {
        Text = MySpaceTexts.NotificationHintPressToOpenDoor,
        FormatParams = new object[2]
        {
          (object) ("[" + (object) MyInput.Static.GetGameControl(MyControlsSpace.USE) + "]"),
          (object) this.Door.DefinitionDisplayNameText
        },
        IsTextControlHint = true,
        JoystickText = new MyStringId?(MySpaceTexts.NotificationHintPressToOpenDoor),
        JoystickFormatParams = new object[2]
        {
          (object) MyControllerHelper.GetCodeForControl(context, MyControlsSpace.USE),
          (object) this.Door.DefinitionDisplayNameText
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
