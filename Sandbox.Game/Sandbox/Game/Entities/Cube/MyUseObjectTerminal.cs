// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyUseObjectTerminal
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
  [MyUseObject("terminal")]
  public class MyUseObjectTerminal : MyUseObjectBase
  {
    public readonly MyCubeBlock Block;
    public readonly Matrix LocalMatrix;

    public MyUseObjectTerminal(
      IMyEntity owner,
      string dummyName,
      MyModelDummy dummyData,
      uint key)
      : base(owner, dummyData)
    {
      this.Block = owner as MyCubeBlock;
      this.LocalMatrix = dummyData.Matrix;
    }

    public override float InteractiveDistance => MyConstants.DEFAULT_INTERACTIVE_DISTANCE;

    public override MatrixD ActivationMatrix => this.LocalMatrix * this.Block.WorldMatrix;

    public override MatrixD WorldMatrix => this.Block.WorldMatrix;

    public override uint RenderObjectID => this.Block.Render.GetRenderObjectID();

    public override int InstanceID => -1;

    public override bool ShowOverlay => true;

    public override UseActionEnum SupportedActions
    {
      get
      {
        UseActionEnum useActionEnum = UseActionEnum.OpenTerminal;
        if (MyEntityExtensions.GetInventory(this.Block) != null)
          useActionEnum |= UseActionEnum.OpenInventory;
        return useActionEnum;
      }
    }

    public override UseActionEnum PrimaryAction => UseActionEnum.OpenTerminal;

    public override UseActionEnum SecondaryAction => MyEntityExtensions.GetInventory(this.Block) == null ? UseActionEnum.None : UseActionEnum.OpenInventory;

    public override void Use(UseActionEnum actionEnum, IMyEntity entity)
    {
      MyCharacter user = entity as MyCharacter;
      if (!this.Block.GetUserRelationToOwner(user.ControllerInfo.ControllingIdentityId).IsFriendly() && !MySession.Static.AdminSettings.HasFlag((Enum) AdminSettingsEnum.UseTerminals))
      {
        if (!user.ControllerInfo.IsLocallyHumanControlled())
          return;
        MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
      }
      else
      {
        switch (actionEnum)
        {
          case UseActionEnum.OpenTerminal:
            MyGuiScreenTerminal.Show(MyTerminalPageEnum.ControlPanel, user, (MyEntity) this.Block);
            break;
          case UseActionEnum.OpenInventory:
            if (MyEntityExtensions.GetInventory(this.Block) == null)
              break;
            MyGuiScreenTerminal.Show(MyTerminalPageEnum.Inventory, user, (MyEntity) this.Block);
            break;
        }
      }
    }

    public override MyActionDescription GetActionInfo(UseActionEnum actionEnum)
    {
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      MyStringId context = controlledEntity != null ? controlledEntity.ControlContext : MySpaceBindingCreator.CX_BASE;
      if (actionEnum != UseActionEnum.OpenTerminal && actionEnum == UseActionEnum.OpenInventory)
        return new MyActionDescription()
        {
          Text = MySpaceTexts.NotificationHintPressToOpenInventory,
          FormatParams = new object[2]
          {
            (object) ("[" + (object) MyInput.Static.GetGameControl(MyControlsSpace.INVENTORY) + "]"),
            (object) this.Block.DefinitionDisplayNameText
          },
          IsTextControlHint = true,
          JoystickText = new MyStringId?(MySpaceTexts.NotificationHintPressToOpenInventory),
          JoystickFormatParams = new object[2]
          {
            (object) MyControllerHelper.GetCodeForControl(context, MyControlsSpace.INVENTORY),
            (object) this.Block.DefinitionDisplayNameText
          },
          ShowForGamepad = true
        };
      return new MyActionDescription()
      {
        Text = MySpaceTexts.NotificationHintPressToOpenControlPanel,
        FormatParams = new object[2]
        {
          (object) ("[" + (object) MyInput.Static.GetGameControl(MyControlsSpace.TERMINAL) + "]"),
          (object) this.Block.DefinitionDisplayNameText
        },
        IsTextControlHint = true,
        JoystickText = new MyStringId?(MySpaceTexts.NotificationHintPressToOpenControlPanel),
        JoystickFormatParams = new object[2]
        {
          (object) MyControllerHelper.GetCodeForControl(context, MyControlsSpace.TERMINAL),
          (object) this.Block.DefinitionDisplayNameText
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
