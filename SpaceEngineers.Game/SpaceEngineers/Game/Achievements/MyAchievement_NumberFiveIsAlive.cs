// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Achievements.MyAchievement_NumberFiveIsAlive
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using SpaceEngineers.Game.Entities.Blocks;
using VRage.ModAPI;

namespace SpaceEngineers.Game.Achievements
{
  public class MyAchievement_NumberFiveIsAlive : MySteamAchievementBase
  {
    protected override (string, string, float) GetAchievementInfo() => (nameof (MyAchievement_NumberFiveIsAlive), (string) null, 0.0f);

    public override bool NeedsUpdate => !MySession.Static.CreativeMode;

    public override void SessionUpdate()
    {
      if (this.IsAchieved || MySession.Static.LocalCharacter == null)
        return;
      IMyEntity temporaryConnectedEntity = MySession.Static.LocalCharacter.SuitBattery.ResourceSink.TemporaryConnectedEntity;
      if ((double) MySession.Static.LocalCharacter.SuitEnergyLevel >= 0.01 || temporaryConnectedEntity == null || temporaryConnectedEntity == MySession.Static.LocalCharacter)
        return;
      switch (temporaryConnectedEntity)
      {
        case MyMedicalRoom myMedicalRoom when myMedicalRoom.IsWorking && myMedicalRoom.RefuelAllowed:
          this.NotifyAchieved();
          break;
        case MyCockpit myCockpit when myCockpit.hasPower:
          this.NotifyAchieved();
          break;
      }
    }
  }
}
