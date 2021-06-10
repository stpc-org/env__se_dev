// Decompiled with JetBrains decompiler
// Type: VRage.Game.VisualScripting.Missions.MyVSStateMachineBarrierNode
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;
using VRage.Collections;
using VRage.Generics;
using VRage.Utils;

namespace VRage.Game.VisualScripting.Missions
{
  public class MyVSStateMachineBarrierNode : MyStateMachineNode
  {
    private readonly List<bool> m_cursorsFromInEdgesReceived = new List<bool>();

    public MyVSStateMachineBarrierNode(string name)
      : base(name)
    {
    }

    protected override void ExpandInternal(
      MyStateMachineCursor cursor,
      MyConcurrentHashSet<MyStringId> enquedActions,
      int passThrough)
    {
      MyStateMachine stateMachine = cursor.StateMachine;
      int index = 0;
      while (index < this.InTransitions.Count && this.InTransitions[index].Id != cursor.LastTransitionTakenId)
        ++index;
      this.m_cursorsFromInEdgesReceived[index] = true;
      stateMachine.DeleteCursor(cursor.Id);
      foreach (bool flag in this.m_cursorsFromInEdgesReceived)
      {
        if (!flag)
          return;
      }
      if (this.OutTransitions.Count <= 0)
        return;
      stateMachine.CreateCursor(this.OutTransitions[0].TargetNode.Name);
    }

    protected override void TransitionAddedInternal(MyStateMachineTransition transition)
    {
      if (transition.TargetNode != this)
        return;
      this.m_cursorsFromInEdgesReceived.Add(false);
    }

    protected override void TransitionRemovedInternal(MyStateMachineTransition transition)
    {
      if (transition.TargetNode != this)
        return;
      this.m_cursorsFromInEdgesReceived.RemoveAt(this.InTransitions.IndexOf(transition));
    }
  }
}
