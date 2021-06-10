// Decompiled with JetBrains decompiler
// Type: VRage.Generics.MyStateMachineCursor
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Threading;
using VRage.Utils;

namespace VRage.Generics
{
  public class MyStateMachineCursor
  {
    private static int m_idCounter;
    private readonly MyStateMachine m_stateMachine;
    private MyStateMachineNode m_node;
    public readonly int Id;

    public MyStateMachineNode Node
    {
      get => this.m_node;
      internal set => this.m_node = value;
    }

    public int LastTransitionTakenId { get; private set; }

    public event MyStateMachineCursor.CursorStateChanged OnCursorStateChanged;

    public MyStateMachineCursor(MyStateMachineNode node, MyStateMachine stateMachine)
    {
      this.m_stateMachine = stateMachine;
      this.Id = Interlocked.Increment(ref MyStateMachineCursor.m_idCounter);
      this.m_node = node;
      this.m_node.Cursors.Add(this);
      this.OnCursorStateChanged = (MyStateMachineCursor.CursorStateChanged) null;
    }

    public MyStateMachine StateMachine => this.m_stateMachine;

    private void NotifyCursorChanged(MyStateMachineTransition transition, MyStringId action)
    {
      if (this.OnCursorStateChanged == null)
        return;
      this.OnCursorStateChanged(transition.Id, action, this.Node, this.StateMachine);
    }

    public void FollowTransition(MyStateMachineTransition transition, MyStringId action)
    {
      this.Node.Cursors.Remove(this);
      transition.TargetNode.Cursors.Add(this);
      this.Node = transition.TargetNode;
      this.LastTransitionTakenId = transition.Id;
      this.NotifyCursorChanged(transition, action);
    }

    public delegate void CursorStateChanged(
      int transitionId,
      MyStringId action,
      MyStateMachineNode node,
      MyStateMachine stateMachine);
  }
}
