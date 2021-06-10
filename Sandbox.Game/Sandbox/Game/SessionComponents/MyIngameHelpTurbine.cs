// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpTurbine
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using System;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_Turbine", 500)]
  internal class MyIngameHelpTurbine : MyIngameHelpObjective
  {
    private bool m_turbineAdded;

    public MyIngameHelpTurbine()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_Turbine_Title;
      this.RequiredIds = new string[1]
      {
        "IngameHelp_Camera"
      };
      this.Details = new MyIngameHelpDetail[1]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Turbine_Detail1
        }
      };
      this.RequiredCondition = new Func<bool>(this.BlockAddedCondition);
      this.DelayToHide = 4f * MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
      if (MyCubeBuilder.Static != null)
        MyCubeBuilder.Static.OnBlockAdded += new Action<MyCubeBlockDefinition>(this.Static_OnBlockAdded);
      this.FollowingId = "IngameHelp_Turbine2";
    }

    public override void CleanUp()
    {
      if (MyCubeBuilder.Static == null)
        return;
      MyCubeBuilder.Static.OnBlockAdded -= new Action<MyCubeBlockDefinition>(this.Static_OnBlockAdded);
    }

    private void Static_OnBlockAdded(MyCubeBlockDefinition definition) => this.m_turbineAdded = definition.Id.TypeId.ToString() == "MyObjectBuilder_WindTurbine" || definition.Id.TypeId.ToString() == "MyObjectBuilder_SolarPanel";

    private bool BlockAddedCondition() => this.m_turbineAdded;
  }
}
