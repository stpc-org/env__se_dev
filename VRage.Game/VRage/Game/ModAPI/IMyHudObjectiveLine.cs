// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.IMyHudObjectiveLine
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;

namespace VRage.Game.ModAPI
{
  public interface IMyHudObjectiveLine
  {
    bool Visible { get; }

    string Title { get; set; }

    string CurrentObjective { get; }

    void Show();

    void Hide();

    void AdvanceObjective();

    List<string> Objectives { get; set; }
  }
}
