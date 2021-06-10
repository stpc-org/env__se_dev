// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Achievements.MyAchievement_PersonalityCrisis
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Game.Gui;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;

namespace SpaceEngineers.Game.Achievements
{
  public class MyAchievement_PersonalityCrisis : MySteamAchievementBase
  {
    private const int NUMBER_OF_CHANGES_REQUIRED = 20;
    private const int MAXIMUM_TIMER = 600;
    private uint[] m_startS;
    private int m_timerIndex;

    protected override (string, string, float) GetAchievementInfo() => (nameof (MyAchievement_PersonalityCrisis), (string) null, 0.0f);

    public override bool NeedsUpdate => this.m_startS != null && this.m_startS[this.m_timerIndex] != uint.MaxValue;

    public override void SessionLoad()
    {
      if (this.IsAchieved)
        return;
      this.m_startS = new uint[20];
      for (int index = 0; index < this.m_startS.Length; ++index)
        this.m_startS[index] = uint.MaxValue;
      MyGuiScreenLoadInventory.LookChanged += new MyLookChangeDelegate(this.MyGuiScreenWardrobeOnLookChanged);
    }

    public override void SessionUnload()
    {
      base.SessionUnload();
      MyGuiScreenLoadInventory.LookChanged -= new MyLookChangeDelegate(this.MyGuiScreenWardrobeOnLookChanged);
    }

    private void MyGuiScreenWardrobeOnLookChanged()
    {
      this.m_startS[this.m_timerIndex] = (uint) MySession.Static.ElapsedPlayTime.TotalSeconds;
      ++this.m_timerIndex;
      this.m_timerIndex %= 20;
      if (this.m_startS[this.m_timerIndex] == uint.MaxValue)
        return;
      MyGuiScreenLoadInventory.LookChanged -= new MyLookChangeDelegate(this.MyGuiScreenWardrobeOnLookChanged);
      this.NotifyAchieved();
    }

    public override void SessionUpdate()
    {
      if (this.IsAchieved)
        return;
      uint totalSeconds = (uint) MySession.Static.ElapsedPlayTime.TotalSeconds;
      for (int index = 0; index < this.m_startS.Length; ++index)
      {
        if (this.m_startS[index] != uint.MaxValue && totalSeconds - this.m_startS[index] > 600U)
          this.m_startS[index] = uint.MaxValue;
      }
    }
  }
}
