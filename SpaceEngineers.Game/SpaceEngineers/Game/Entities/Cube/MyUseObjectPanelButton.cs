// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.Cube.MyUseObjectPanelButton
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using SpaceEngineers.Game.Entities.Blocks;
using System;
using VRage.Game;
using VRage.Game.Entity.UseObject;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace SpaceEngineers.Game.Entities.Cube
{
  [MyUseObject("panel")]
  public class MyUseObjectPanelButton : MyUseObjectBase
  {
    private readonly MyButtonPanel m_buttonPanel;
    private readonly Matrix m_localMatrix;
    private int m_index;
    private MyGps m_buttonDesc;

    public MyUseObjectPanelButton(
      IMyEntity owner,
      string dummyName,
      MyModelDummy dummyData,
      uint key)
      : base(owner, dummyData)
    {
      this.m_buttonPanel = owner as MyButtonPanel;
      this.m_localMatrix = dummyData.Matrix;
      int result = 0;
      string[] strArray = dummyName.Split('_');
      int.TryParse(strArray[strArray.Length - 1], out result);
      this.m_index = result - 1;
      if (this.m_index < this.m_buttonPanel.BlockDefinition.ButtonCount)
        return;
      MyLog.Default.WriteLine(string.Format("{0} Button index higher than defined count.", (object) this.m_buttonPanel.BlockDefinition.Id.SubtypeName));
      this.m_index = this.m_buttonPanel.BlockDefinition.ButtonCount - 1;
    }

    public override float InteractiveDistance => MyConstants.DEFAULT_INTERACTIVE_DISTANCE;

    public override MatrixD ActivationMatrix => this.m_localMatrix * this.m_buttonPanel.WorldMatrix;

    public override MatrixD WorldMatrix => this.m_buttonPanel.WorldMatrix;

    public Vector3D MarkerPosition => this.ActivationMatrix.Translation;

    public override uint RenderObjectID => this.m_buttonPanel.Render.GetRenderObjectID();

    public override int InstanceID => -1;

    public override bool ShowOverlay => true;

    public override UseActionEnum SupportedActions => this.PrimaryAction | this.SecondaryAction;

    public override UseActionEnum PrimaryAction => this.m_buttonPanel.Toolbar.GetItemAtIndex(this.m_index) == null ? UseActionEnum.None : UseActionEnum.Manipulate;

    public override UseActionEnum SecondaryAction => UseActionEnum.OpenTerminal;

    public override bool ContinuousUsage => false;

    public override void Use(UseActionEnum actionEnum, IMyEntity entity)
    {
      MyCharacter myCharacter = entity as MyCharacter;
      MyContainerDropComponent component;
      if (this.m_buttonPanel.Components.TryGet<MyContainerDropComponent>(out component))
      {
        MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = (MyGuiScreenBase) new MyGuiScreenClaimGameItem(component, myCharacter.GetPlayerIdentityId()));
      }
      else
      {
        switch (actionEnum)
        {
          case UseActionEnum.Manipulate:
            if (!this.m_buttonPanel.IsWorking)
              break;
            if (!this.m_buttonPanel.AnyoneCanUse && !this.m_buttonPanel.HasLocalPlayerAccess())
            {
              MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
              break;
            }
            MyMultiplayer.RaiseEvent<MyButtonPanel, int, long>(this.m_buttonPanel, (Func<MyButtonPanel, Action<int, long>>) (x => new Action<int, long>(x.ActivateButton)), this.m_index, myCharacter.EntityId);
            break;
          case UseActionEnum.OpenTerminal:
            if (!this.m_buttonPanel.HasLocalPlayerAccess())
              break;
            MyToolbarComponent.CurrentToolbar = this.m_buttonPanel.Toolbar;
            MyGuiScreenBase screen = (MyGuiScreenBase) MyGuiScreenToolbarConfigBase.Static;
            if (screen == null)
              screen = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.ToolbarConfigScreen, (object) 0, (object) this.m_buttonPanel, null);
            MyToolbarComponent.AutoUpdate = false;
            screen.Closed += (MyGuiScreenBase.ScreenHandler) ((source, isUnloading) => MyToolbarComponent.AutoUpdate = true);
            MyGuiSandbox.AddScreen(screen);
            break;
        }
      }
    }

    public override MyActionDescription GetActionInfo(UseActionEnum actionEnum)
    {
      this.m_buttonPanel.Toolbar.UpdateItem(this.m_index);
      MyToolbarItem itemAtIndex = this.m_buttonPanel.Toolbar.GetItemAtIndex(this.m_index);
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      MyStringId context = controlledEntity != null ? controlledEntity.ControlContext : MySpaceBindingCreator.CX_BASE;
      switch (actionEnum)
      {
        case UseActionEnum.Manipulate:
          if (this.m_buttonDesc == null)
          {
            this.m_buttonDesc = new MyGps();
            this.m_buttonDesc.Description = "";
            this.m_buttonDesc.CoordsFunc = (Func<Vector3D>) (() => this.MarkerPosition);
            this.m_buttonDesc.ShowOnHud = true;
            this.m_buttonDesc.DiscardAt = new TimeSpan?();
            this.m_buttonDesc.AlwaysVisible = true;
          }
          MyHud.ButtonPanelMarkers.RegisterMarker(this.m_buttonDesc);
          this.SetButtonName(this.m_buttonPanel.GetCustomButtonName(this.m_index));
          if (itemAtIndex != null)
            return new MyActionDescription()
            {
              Text = MyCommonTexts.NotificationHintPressToUse,
              FormatParams = new object[2]
              {
                (object) ("[" + (object) MyInput.Static.GetGameControl(MyControlsSpace.USE) + "]"),
                (object) itemAtIndex.DisplayName
              },
              IsTextControlHint = true,
              JoystickText = new MyStringId?(MyCommonTexts.NotificationHintPressToUse),
              JoystickFormatParams = new object[2]
              {
                (object) MyControllerHelper.GetCodeForControl(context, MyControlsSpace.USE),
                (object) itemAtIndex.DisplayName
              },
              ShowForGamepad = true
            };
          return new MyActionDescription()
          {
            Text = MySpaceTexts.Blank
          };
        case UseActionEnum.OpenTerminal:
          return new MyActionDescription()
          {
            Text = MySpaceTexts.NotificationHintPressToOpenButtonPanel,
            FormatParams = new object[1]
            {
              (object) ("[" + (object) MyInput.Static.GetGameControl(MyControlsSpace.TERMINAL) + "]")
            },
            IsTextControlHint = true,
            JoystickText = new MyStringId?(MySpaceTexts.NotificationHintPressToOpenButtonPanel),
            JoystickFormatParams = new object[1]
            {
              (object) MyControllerHelper.GetCodeForControl(context, MyControlsSpace.TERMINAL)
            },
            ShowForGamepad = true
          };
        default:
          return new MyActionDescription()
          {
            Text = MySpaceTexts.NotificationHintPressToOpenButtonPanel,
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

    public override bool PlayIndicatorSound => true;

    public void RemoveButtonMarker()
    {
      if (this.m_buttonDesc == null)
        return;
      MyHud.ButtonPanelMarkers.UnregisterMarker(this.m_buttonDesc);
    }

    public override void OnSelectionLost() => this.RemoveButtonMarker();

    private void SetButtonName(string name)
    {
      if (this.m_buttonPanel.IsFunctional && this.m_buttonPanel.IsWorking && (this.m_buttonPanel.HasLocalPlayerAccess() || this.m_buttonPanel.AnyoneCanUse))
        this.m_buttonDesc.Name = name;
      else
        this.m_buttonDesc.Name = "";
    }
  }
}
