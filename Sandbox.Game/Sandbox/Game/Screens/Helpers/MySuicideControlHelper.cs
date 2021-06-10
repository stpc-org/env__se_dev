// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MySuicideControlHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Character;
using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage;

namespace Sandbox.Game.Screens.Helpers
{
  public class MySuicideControlHelper : MyAbstractControlMenuItem
  {
    private MyCharacter m_character;

    public MySuicideControlHelper()
      : base(MyControlsSpace.SUICIDE)
    {
    }

    public override string Label => MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_CommitSuicide);

    public override void Activate()
    {
      MyCampaignSessionComponent component = MySession.Static.GetComponent<MyCampaignSessionComponent>();
      if (component != null && component.CustomRespawnEnabled)
      {
        if (MySession.Static.ControlledEntity == null)
          return;
        MyVisualScriptLogicProvider.CustomRespawnRequest(this.m_character.ControllerInfo.ControllingIdentityId);
      }
      else
        this.m_character.Die();
    }

    public void SetCharacter(MyCharacter character) => this.m_character = character;
  }
}
