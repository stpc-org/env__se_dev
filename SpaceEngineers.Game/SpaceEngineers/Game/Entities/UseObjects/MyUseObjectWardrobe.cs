// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.UseObjects.MyUseObjectWardrobe
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Screens;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using SpaceEngineers.Game.Entities.Blocks;
using System;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace SpaceEngineers.Game.Entities.UseObjects
{
  [MyUseObject("wardrobe")]
  internal class MyUseObjectWardrobe : MyUseObjectBase
  {
    public readonly MyCubeBlock Block;
    public readonly Matrix LocalMatrix;

    public MyUseObjectWardrobe(
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

    public override MatrixD ActivationMatrix => (MatrixD) ref this.LocalMatrix * this.Block.WorldMatrix;

    public override MatrixD WorldMatrix => this.Block.WorldMatrix;

    public override uint RenderObjectID => this.Block.Render.GetRenderObjectID();

    public override int InstanceID => -1;

    public override bool ShowOverlay => true;

    public override UseActionEnum SupportedActions => this.PrimaryAction | this.SecondaryAction;

    public override UseActionEnum PrimaryAction => UseActionEnum.Manipulate;

    public override UseActionEnum SecondaryAction => UseActionEnum.None;

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
        if (actionEnum != UseActionEnum.Manipulate || !(this.Block is MyMedicalRoom block) || !block.IsWorking)
          return;
        if (!block.SuitChangeAllowed)
        {
          MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
        }
        else
        {
          MyHud.SelectedObjectHighlight.HighlightStyle = MyHudObjectHighlightStyle.None;
          bool flag = user.Definition.Skeleton == "Humanoid";
          if (block.CustomWardrobesEnabled)
          {
            if (MyGameService.IsActive & flag)
            {
              MySessionComponentContainerDropSystem component = MySession.Static.GetComponent<MySessionComponentContainerDropSystem>();
              if (component != null)
                component.EnableWindowPopups = false;
              MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = (MyGuiScreenBase) new MyGuiScreenLoadInventory(true, block.CustomWardrobeNames));
              MyGuiScreenGamePlay.ActiveGameplayScreen.Closed += new MyGuiScreenBase.ScreenHandler(this.ActiveGameplayScreen_Closed);
              block.UseWardrobe(user);
            }
            else
              MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = (MyGuiScreenBase) new MyGuiScreenWardrobe(user, block.CustomWardrobeNames));
          }
          else if (MyGameService.IsActive & flag)
          {
            MySessionComponentContainerDropSystem component = MySession.Static.GetComponent<MySessionComponentContainerDropSystem>();
            if (component != null)
              component.EnableWindowPopups = false;
            MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = (MyGuiScreenBase) new MyGuiScreenLoadInventory(true));
            MyGuiScreenGamePlay.ActiveGameplayScreen.Closed += new MyGuiScreenBase.ScreenHandler(this.ActiveGameplayScreen_Closed);
            block.UseWardrobe(user);
          }
          else
            MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = (MyGuiScreenBase) new MyGuiScreenWardrobe(user));
        }
      }
    }

    private void ActiveGameplayScreen_Closed(MyGuiScreenBase source, bool isUnloading)
    {
      if (this.Block is MyMedicalRoom block)
        block.StopUsingWardrobe();
      MySessionComponentContainerDropSystem component = MySession.Static.GetComponent<MySessionComponentContainerDropSystem>();
      if (component != null)
        component.EnableWindowPopups = true;
      if (MyGuiScreenGamePlay.ActiveGameplayScreen != null)
      {
        MyGuiScreenGamePlay.ActiveGameplayScreen.Closed -= new MyGuiScreenBase.ScreenHandler(this.ActiveGameplayScreen_Closed);
        MyGuiScreenGamePlay.ActiveGameplayScreen = (MyGuiScreenBase) null;
      }
      if (MySession.Static.LocalCharacter == null || !(MySession.Static.LocalCharacter.CurrentWeapon is MyEntity currentWeapon))
        return;
      MyAssetModifierComponent skinComponent = currentWeapon.Components.Get<MyAssetModifierComponent>();
      if (skinComponent == null)
        return;
      MyLocalCache.LoadInventoryConfig(MySession.Static.LocalCharacter, currentWeapon, skinComponent);
    }

    public override MyActionDescription GetActionInfo(UseActionEnum actionEnum)
    {
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      MyStringId context = controlledEntity != null ? controlledEntity.ControlContext : MySpaceBindingCreator.CX_BASE;
      return new MyActionDescription()
      {
        Text = MyCommonTexts.NotificationHintPressToUseWardrobe,
        FormatParams = new object[1]
        {
          (object) ("[" + (object) MyInput.Static.GetGameControl(MyControlsSpace.USE) + "]")
        },
        IsTextControlHint = true,
        JoystickText = new MyStringId?(MyCommonTexts.NotificationHintPressToUseWardrobe),
        JoystickFormatParams = new object[1]
        {
          (object) MyControllerHelper.GetCodeForControl(context, MyControlsSpace.USE)
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
