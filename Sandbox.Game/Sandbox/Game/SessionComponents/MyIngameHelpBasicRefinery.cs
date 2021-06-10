// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpBasicRefinery
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using System;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_BasicRefinery", 550)]
  internal class MyIngameHelpBasicRefinery : MyIngameHelpObjective
  {
    private bool m_refineryAdded;

    public MyIngameHelpBasicRefinery()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_BasicRefinery_Title;
      this.RequiredIds = new string[1]
      {
        "IngameHelp_Camera"
      };
      this.Details = new MyIngameHelpDetail[1]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_BasicRefinery_Detail1
        }
      };
      this.RequiredCondition = new Func<bool>(this.BlockAddedCondition);
      this.DelayToHide = 4f * MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
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

    private void Static_OnBlockAdded(MyCubeBlockDefinition definition) => this.m_refineryAdded = definition.Id.SubtypeName == "Blast Furnace" || definition.Id.SubtypeName == "BasicAssembler";

    private bool BlockAddedCondition() => this.m_refineryAdded;
  }
}
