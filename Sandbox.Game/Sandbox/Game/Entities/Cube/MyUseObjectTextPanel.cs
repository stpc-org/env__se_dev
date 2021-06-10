// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyUseObjectTextPanel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Entity.UseObject;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Entities.Cube
{
  [MyUseObject("textpanel")]
  public class MyUseObjectTextPanel : MyUseObjectBase
  {
    private MyTextPanel m_textPanel;
    private Matrix m_localMatrix;

    public MyUseObjectTextPanel(
      IMyEntity owner,
      string dummyName,
      MyModelDummy dummyData,
      uint key)
      : base(owner, dummyData)
    {
      this.m_textPanel = (MyTextPanel) owner;
      this.m_localMatrix = dummyData.Matrix;
    }

    public override float InteractiveDistance => MyConstants.DEFAULT_INTERACTIVE_DISTANCE;

    public override MatrixD ActivationMatrix => this.m_localMatrix * this.m_textPanel.WorldMatrix;

    public override MatrixD WorldMatrix => this.m_textPanel.WorldMatrix;

    public override uint RenderObjectID
    {
      get
      {
        if (this.m_textPanel.Render == null)
          return uint.MaxValue;
        uint[] renderObjectIds = this.m_textPanel.Render.RenderObjectIDs;
        return renderObjectIds.Length != 0 ? renderObjectIds[0] : uint.MaxValue;
      }
    }

    public override int InstanceID => -1;

    public override bool ShowOverlay => true;

    public override UseActionEnum SupportedActions
    {
      get
      {
        UseActionEnum useActionEnum = UseActionEnum.None;
        if (this.m_textPanel.GetPlayerRelationToOwner() != MyRelationsBetweenPlayerAndBlock.Enemies)
          useActionEnum = this.PrimaryAction | this.SecondaryAction;
        return useActionEnum;
      }
    }

    public override UseActionEnum PrimaryAction => UseActionEnum.Manipulate;

    public override UseActionEnum SecondaryAction => UseActionEnum.OpenTerminal;

    public override bool ContinuousUsage => false;

    public override void Use(UseActionEnum actionEnum, IMyEntity user) => this.m_textPanel.Use(actionEnum, user);

    public override MyActionDescription GetActionInfo(UseActionEnum actionEnum)
    {
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      MyStringId context = controlledEntity != null ? controlledEntity.ControlContext : MySpaceBindingCreator.CX_BASE;
      if (actionEnum != UseActionEnum.Manipulate)
      {
        if (actionEnum == UseActionEnum.OpenTerminal)
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
      return new MyActionDescription()
      {
        Text = MySpaceTexts.NotificationHintPressToShowScreen,
        FormatParams = new object[1]
        {
          (object) ("[" + (object) MyInput.Static.GetGameControl(MyControlsSpace.USE) + "]")
        },
        IsTextControlHint = true,
        JoystickText = new MyStringId?(MySpaceTexts.NotificationHintPressToShowScreen),
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
