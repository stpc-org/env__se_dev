// Decompiled with JetBrains decompiler
// Type: VRage.Generics.MySingleStateMachine
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRage.Utils;

namespace VRage.Generics
{
  public class MySingleStateMachine : MyStateMachine
  {
    public event MySingleStateMachine.StateChangedHandler OnStateChanged;

    protected void NotifyStateChanged(
      MyStateMachineTransitionWithStart transitionWithStart,
      MyStringId action)
    {
      if (this.OnStateChanged == null)
        return;
      this.OnStateChanged(transitionWithStart, action);
    }

    public override bool DeleteCursor(int id) => false;

    public override MyStateMachineCursor CreateCursor(string nodeName) => (MyStateMachineCursor) null;

    public MyStateMachineNode CurrentNode => this.m_activeCursors.Count == 0 ? (MyStateMachineNode) null : this.m_activeCursors[0].Node;

    public bool SetState(string nameOfNewState)
    {
      if (this.m_activeCursors.Count == 0)
      {
        if (base.CreateCursor(nameOfNewState) == null)
          return false;
        this.m_activeCursors.ApplyChanges();
        this.m_activeCursors[0].OnCursorStateChanged += new MyStateMachineCursor.CursorStateChanged(this.CursorStateChanged);
      }
      else
        this.m_activeCursors[0].Node = this.FindNode(nameOfNewState);
      return true;
    }

    private void CursorStateChanged(
      int transitionId,
      MyStringId action,
      MyStateMachineNode node,
      MyStateMachine stateMachine)
    {
      this.NotifyStateChanged(this.m_transitions[transitionId], action);
    }

    public delegate void StateChangedHandler(
      MyStateMachineTransitionWithStart transition,
      MyStringId action);
  }
}
