// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyUseObjectCryoChamberDoor
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage.Game;
using VRage.Game.Entity.UseObject;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Entities.Cube
{
  [MyUseObject("cryopod")]
  internal class MyUseObjectCryoChamberDoor : MyUseObjectBase
  {
    public readonly MyCryoChamber CryoChamber;
    public readonly Matrix LocalMatrix;

    public MyUseObjectCryoChamberDoor(
      IMyEntity owner,
      string dummyName,
      MyModelDummy dummyData,
      uint key)
      : base(owner, dummyData)
    {
      this.CryoChamber = owner as MyCryoChamber;
      this.LocalMatrix = dummyData.Matrix;
    }

    public override float InteractiveDistance => MyConstants.DEFAULT_INTERACTIVE_DISTANCE;

    public override MatrixD ActivationMatrix => this.LocalMatrix * this.CryoChamber.WorldMatrix;

    public override MatrixD WorldMatrix => this.CryoChamber.WorldMatrix;

    public override uint RenderObjectID => this.CryoChamber.Render.GetRenderObjectID();

    public override int InstanceID => -1;

    public override bool ShowOverlay => true;

    public override UseActionEnum SupportedActions => this.PrimaryAction | this.SecondaryAction;

    public override UseActionEnum PrimaryAction => UseActionEnum.Manipulate;

    public override UseActionEnum SecondaryAction => UseActionEnum.None;

    public override void Use(UseActionEnum actionEnum, IMyEntity entity)
    {
      MyCharacter user = entity as MyCharacter;
      this.CryoChamber.RequestUse(actionEnum, user);
    }

    public override MyActionDescription GetActionInfo(UseActionEnum actionEnum)
    {
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      MyStringId context = controlledEntity != null ? controlledEntity.ControlContext : MySpaceBindingCreator.CX_BASE;
      return new MyActionDescription()
      {
        Text = MySpaceTexts.NotificationHintPressToEnterCryochamber,
        FormatParams = new object[2]
        {
          (object) ("[" + MyGuiSandbox.GetKeyName(MyControlsSpace.USE) + "]"),
          (object) this.CryoChamber.DisplayNameText
        },
        IsTextControlHint = true,
        JoystickFormatParams = new object[2]
        {
          (object) MyControllerHelper.GetCodeForControl(context, MyControlsSpace.USE),
          (object) this.CryoChamber.DisplayNameText
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
