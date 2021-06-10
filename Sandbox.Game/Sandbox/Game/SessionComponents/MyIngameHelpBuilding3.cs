// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpBuilding3
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using System;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_Building3", 80)]
  internal class MyIngameHelpBuilding3 : MyIngameHelpObjective
  {
    private bool m_blockAdded;

    public MyIngameHelpBuilding3()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_Building_Title;
      this.RequiredIds = new string[1]
      {
        "IngameHelp_Building2"
      };
      this.Details = new MyIngameHelpDetail[2]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Building3_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Building3_Detail2,
          FinishCondition = new Func<bool>(this.BlockAddedCondition)
        }
      };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
      this.FollowingId = "IngameHelp_BuildingTip2";
    }

    public override void OnActivated()
    {
      base.OnActivated();
      MyCubeBuilder.Static.OnBlockAdded += new Action<MyCubeBlockDefinition>(this.Static_OnBlockAdded);
    }

    public override void CleanUp()
    {
      if (MyCubeBuilder.Static == null)
        return;
      MyCubeBuilder.Static.OnBlockAdded -= new Action<MyCubeBlockDefinition>(this.Static_OnBlockAdded);
    }

    private void Static_OnBlockAdded(MyCubeBlockDefinition definition) => this.m_blockAdded = true;

    private bool BlockAddedCondition() => this.m_blockAdded;
  }
}
