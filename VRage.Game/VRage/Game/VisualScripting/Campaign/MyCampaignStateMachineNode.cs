// Decompiled with JetBrains decompiler
// Type: VRage.Game.VisualScripting.Campaign.MyCampaignStateMachineNode
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;
using VRage.Generics;

namespace VRage.Game.VisualScripting.Campaign
{
  public class MyCampaignStateMachineNode : MyStateMachineNode
  {
    public string SavePath { get; set; }

    public bool Finished { get; private set; }

    public int InTransitionCount => this.InTransitions.Count;

    public MyCampaignStateMachineNode(string name)
      : base(name)
    {
    }

    public override void OnUpdate(MyStateMachine stateMachine, List<string> eventCollection)
    {
      if (this.OutTransitions.Count != 0)
        return;
      this.Finished = true;
    }
  }
}
