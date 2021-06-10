// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyUseObjectLadder
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
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
  [MyUseObject("ladder")]
  public class MyUseObjectLadder : MyUseObjectBase
  {
    private MyLadder m_ladder;
    private Matrix m_localMatrix;

    public MyUseObjectLadder(IMyEntity owner, string dummyName, MyModelDummy dummyData, uint key)
      : base(owner, dummyData)
    {
      this.m_ladder = (MyLadder) owner;
      this.m_localMatrix = dummyData.Matrix;
    }

    public override float InteractiveDistance => MyConstants.DEFAULT_INTERACTIVE_DISTANCE;

    public override MatrixD ActivationMatrix => this.m_localMatrix * this.m_ladder.WorldMatrix;

    public override MatrixD WorldMatrix => this.m_ladder.WorldMatrix;

    public override uint RenderObjectID
    {
      get
      {
        if (this.m_ladder.Render == null)
          return uint.MaxValue;
        uint[] renderObjectIds = this.m_ladder.Render.RenderObjectIDs;
        return renderObjectIds.Length != 0 ? renderObjectIds[0] : uint.MaxValue;
      }
    }

    public override int InstanceID => -1;

    public override bool ShowOverlay => true;

    public override UseActionEnum SupportedActions => this.PrimaryAction | this.SecondaryAction;

    public override UseActionEnum PrimaryAction => UseActionEnum.Manipulate;

    public override UseActionEnum SecondaryAction => UseActionEnum.None;

    public override bool ContinuousUsage => false;

    public override void Use(UseActionEnum actionEnum, IMyEntity user) => this.m_ladder.Use(actionEnum, user);

    public override MyActionDescription GetActionInfo(UseActionEnum actionEnum)
    {
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      MyStringId context = controlledEntity != null ? controlledEntity.ControlContext : MySpaceBindingCreator.CX_BASE;
      return new MyActionDescription()
      {
        Text = MySpaceTexts.NotificationHintPressToGetOnLadder,
        FormatParams = new object[1]
        {
          (object) ("[" + MyGuiSandbox.GetKeyName(MyControlsSpace.USE) + "]")
        },
        IsTextControlHint = true,
        JoystickFormatParams = new object[1]
        {
          (object) MyControllerHelper.GetCodeForControl(context, MyControlsSpace.USE)
        },
        ShowForGamepad = true
      };
    }

    public override bool HandleInput() => false;

    public override void OnSelectionLost()
    {
    }

    public override bool PlayIndicatorSound => true;
  }
}
