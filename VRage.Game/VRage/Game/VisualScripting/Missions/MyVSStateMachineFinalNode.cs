// Decompiled with JetBrains decompiler
// Type: VRage.Game.VisualScripting.Missions.MyVSStateMachineFinalNode
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Linq;
using VRage.Collections;
using VRage.Generics;
using VRage.Utils;

namespace VRage.Game.VisualScripting.Missions
{
  public class MyVSStateMachineFinalNode : MyStateMachineNode
  {
    public static Action<string, string, bool, bool> Finished;
    private bool m_showCredits;
    private bool m_closeSession;

    public MyVSStateMachineFinalNode(string name, bool showCredits, bool closeSession)
      : base(name)
    {
      this.m_showCredits = showCredits;
      this.m_closeSession = closeSession;
    }

    protected override void ExpandInternal(
      MyStateMachineCursor cursor,
      MyConcurrentHashSet<MyStringId> enquedActions,
      int passThrough)
    {
      MyStateMachineTransition transition = cursor.StateMachine.FindTransition(cursor.LastTransitionTakenId);
      if (transition == null && cursor.StateMachine.AllTransitions.Count > 0)
        transition = cursor.StateMachine.AllTransitions.Values.Last<MyStateMachineTransitionWithStart>().Transition;
      if (MyVSStateMachineFinalNode.Finished != null && transition != null)
        MyVSStateMachineFinalNode.Finished(cursor.StateMachine.Name, transition.Name.ToString(), this.m_showCredits, this.m_closeSession);
      foreach (MyStateMachineCursor activeCursor in cursor.StateMachine.ActiveCursors)
        cursor.StateMachine.DeleteCursor(activeCursor.Id);
    }
  }
}
