// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpBattery
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using System;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_Battery", 650)]
  internal class MyIngameHelpBattery : MyIngameHelpObjective
  {
    private bool m_batteryAdded;

    public MyIngameHelpBattery()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_Battery_Title;
      this.RequiredIds = new string[1]
      {
        "IngameHelp_Camera"
      };
      this.Details = new MyIngameHelpDetail[1]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Battery_Detail1
        }
      };
      this.RequiredCondition = new Func<bool>(this.BlockAddedCondition);
      this.DelayToHide = 4f * MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
      this.FollowingId = "IngameHelp_Battery2";
      if (MyCubeBuilder.Static == null)
        return;
      MyCubeBuilder.Static.OnBlockAdded += new Action<MyCubeBlockDefinition>(this.Static_OnBlockAdded);
    }

    public override void CleanUp()
    {
      base.CleanUp();
      if (MyCubeBuilder.Static == null)
        return;
      MyCubeBuilder.Static.OnBlockAdded -= new Action<MyCubeBlockDefinition>(this.Static_OnBlockAdded);
    }

    private void Static_OnBlockAdded(MyCubeBlockDefinition definition) => this.m_batteryAdded = definition.Id.TypeId.ToString() == "MyObjectBuilder_BatteryBlock";

    private bool BlockAddedCondition() => this.m_batteryAdded;
  }
}
