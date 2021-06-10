// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.RadialMenuActions.MyActionRespawn
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Character;
using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
  public class MyActionRespawn : MyActionBase
  {
    public override void ExecuteAction()
    {
      MyCampaignSessionComponent component = MySession.Static.GetComponent<MyCampaignSessionComponent>();
      if (component != null && component.CustomRespawnEnabled)
      {
        if (MySession.Static.ControlledEntity == null)
          return;
        MyVisualScriptLogicProvider.CustomRespawnRequest(MySession.Static.ControlledEntity.ControllerInfo.ControllingIdentityId);
      }
      else
        MySession.Static.ControlledEntity?.Die();
    }

    public override bool IsEnabled() => MySession.Static.ControlledEntity is MyCharacter;

    public override MyRadialLabelText GetLabel(string shortcut, string name)
    {
      MyRadialLabelText label = base.GetLabel(shortcut, name);
      if (!(MySession.Static.ControlledEntity is MyCharacter))
        label.State = label.State + MyActionBase.AppendingConjunctionState(label) + MyTexts.GetString(MySpaceTexts.RadialMenu_Label_CharacterOnly);
      return label;
    }
  }
}
