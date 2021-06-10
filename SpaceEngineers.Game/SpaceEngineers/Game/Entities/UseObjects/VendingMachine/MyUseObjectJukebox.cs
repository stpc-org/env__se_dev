// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.UseObjects.VendingMachine.MyUseObjectJukebox
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using SpaceEngineers.Game.Entities.Blocks;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.ModAPI;
using VRage.Input;
using VRage.ModAPI;
using VRageMath;
using VRageRender.Import;

namespace SpaceEngineers.Game.Entities.UseObjects.VendingMachine
{
  [MyUseObject("jukebox")]
  public class MyUseObjectJukebox : MyUseObjectBase
  {
    public override MatrixD ActivationMatrix => this.Dummy.Matrix * this.Owner.WorldMatrix;

    public override MatrixD WorldMatrix => this.Owner.WorldMatrix;

    public override uint RenderObjectID => this.Owner.Render.GetRenderObjectID();

    public override int InstanceID => -1;

    public override bool ShowOverlay => true;

    public override UseActionEnum SupportedActions => !(this.Owner is MyJukebox) ? UseActionEnum.None : this.PrimaryAction | this.SecondaryAction;

    public override UseActionEnum PrimaryAction => UseActionEnum.OpenTerminal;

    public override UseActionEnum SecondaryAction => UseActionEnum.None;

    public override bool ContinuousUsage => false;

    public override bool PlayIndicatorSound => true;

    public override float InteractiveDistance => MyConstants.DEFAULT_INTERACTIVE_DISTANCE;

    public MyUseObjectJukebox(IMyEntity owner, string dummyName, MyModelDummy dummyData, uint key)
      : base(owner, dummyData)
    {
    }

    public override MyActionDescription GetActionInfo(UseActionEnum actionEnum)
    {
      if (actionEnum != UseActionEnum.OpenTerminal)
        return new MyActionDescription();
      return new MyActionDescription()
      {
        Text = MySpaceTexts.NotificationHintPressToOpenTerminal,
        FormatParams = new object[1]
        {
          (object) ("[" + (object) MyInput.Static.GetGameControl(MyControlsSpace.USE) + "]")
        },
        IsTextControlHint = true,
        JoystickFormatParams = new object[1]
        {
          (object) MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.USE)
        },
        ShowForGamepad = true
      };
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
      if (!(this.Owner is MyJukebox) || actionEnum != UseActionEnum.OpenTerminal)
        return;
      MyGuiScreenTerminal.Show(MyTerminalPageEnum.ControlPanel, myCharacter, (MyEntity) this.Owner);
    }
  }
}
