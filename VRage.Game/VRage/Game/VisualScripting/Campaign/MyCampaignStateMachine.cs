// Decompiled with JetBrains decompiler
// Type: VRage.Game.VisualScripting.Campaign.MyCampaignStateMachine
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Game.ObjectBuilders.Campaign;
using VRage.Generics;

namespace VRage.Game.VisualScripting.Campaign
{
  public class MyCampaignStateMachine : MySingleStateMachine
  {
    private MyObjectBuilder_CampaignSM m_objectBuilder;

    public bool Initialized => this.m_objectBuilder != null;

    public bool Finished => ((MyCampaignStateMachineNode) this.CurrentNode).Finished;

    public void Deserialize(MyObjectBuilder_CampaignSM ob)
    {
      if (this.m_objectBuilder != null)
        return;
      this.m_objectBuilder = ob;
      foreach (MyObjectBuilder_CampaignSMNode node in this.m_objectBuilder.Nodes)
        this.AddNode((MyStateMachineNode) new MyCampaignStateMachineNode(node.Name)
        {
          SavePath = node.SaveFilePath
        });
      foreach (MyObjectBuilder_CampaignSMTransition transition in this.m_objectBuilder.Transitions)
        this.AddTransition(transition.From, transition.To, name: transition.Name);
    }

    public void ResetToStart()
    {
      foreach (MyStateMachineNode stateMachineNode1 in this.m_nodes.Values)
      {
        if (stateMachineNode1 is MyCampaignStateMachineNode stateMachineNode && stateMachineNode.InTransitionCount == 0)
        {
          this.SetState(stateMachineNode.Name);
          break;
        }
      }
    }
  }
}
