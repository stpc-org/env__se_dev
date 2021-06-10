// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyHudWarningGroup
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;

namespace Sandbox.Game.Gui
{
  internal class MyHudWarningGroup
  {
    private List<MyHudWarning> m_hudWarnings;
    private bool m_canBeTurnedOff;
    private int m_msSinceLastCuePlayed;
    private int m_highestWarnedPriority = int.MaxValue;

    public MyHudWarningGroup(List<MyHudWarning> hudWarnings, bool canBeTurnedOff)
    {
      this.m_hudWarnings = new List<MyHudWarning>((IEnumerable<MyHudWarning>) hudWarnings);
      this.SortByPriority();
      this.m_canBeTurnedOff = canBeTurnedOff;
      this.InitLastCuePlayed();
      foreach (MyHudWarning hudWarning in hudWarnings)
      {
        MyHudWarning warning = hudWarning;
        warning.CanPlay = (Func<bool>) (() =>
        {
          if (this.m_highestWarnedPriority > warning.WarningPriority)
            return true;
          return this.m_msSinceLastCuePlayed > warning.RepeatInterval && this.m_highestWarnedPriority == warning.WarningPriority;
        });
        warning.Played = (Action) (() =>
        {
          this.m_msSinceLastCuePlayed = 0;
          this.m_highestWarnedPriority = warning.WarningPriority;
        });
      }
    }

    private void InitLastCuePlayed()
    {
      foreach (MyHudWarning hudWarning in this.m_hudWarnings)
      {
        if (hudWarning.RepeatInterval > this.m_msSinceLastCuePlayed)
          this.m_msSinceLastCuePlayed = hudWarning.RepeatInterval;
      }
    }

    public void Update()
    {
      if (!MySandboxGame.IsGameReady)
        return;
      this.m_msSinceLastCuePlayed += 16 * MyHudWarnings.FRAMES_BETWEEN_UPDATE;
      bool isWarnedHigherPriority = false;
      foreach (MyHudWarning hudWarning in this.m_hudWarnings)
      {
        if (hudWarning.Update(isWarnedHigherPriority))
          isWarnedHigherPriority = true;
      }
      if (isWarnedHigherPriority)
        return;
      this.m_highestWarnedPriority = int.MaxValue;
    }

    public void Add(MyHudWarning hudWarning)
    {
      this.m_hudWarnings.Add(hudWarning);
      this.SortByPriority();
      this.InitLastCuePlayed();
      hudWarning.CanPlay = (Func<bool>) (() =>
      {
        if (this.m_highestWarnedPriority > hudWarning.WarningPriority)
          return true;
        return this.m_msSinceLastCuePlayed > hudWarning.RepeatInterval && this.m_highestWarnedPriority == hudWarning.WarningPriority;
      });
      hudWarning.Played = (Action) (() =>
      {
        this.m_msSinceLastCuePlayed = 0;
        this.m_highestWarnedPriority = hudWarning.WarningPriority;
      });
    }

    public void Remove(MyHudWarning hudWarning) => this.m_hudWarnings.Remove(hudWarning);

    public void Clear() => this.m_hudWarnings.Clear();

    private void SortByPriority() => this.m_hudWarnings.Sort((Comparison<MyHudWarning>) ((x, y) => x.WarningPriority.CompareTo(y.WarningPriority)));
  }
}
