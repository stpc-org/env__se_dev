// Decompiled with JetBrains decompiler
// Type: VRage.Generics.MyStateMachineNode
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Collections.Generic;
using VRage.Collections;
using VRage.Utils;

namespace VRage.Generics
{
  public class MyStateMachineNode
  {
    private readonly string m_name;
    protected internal List<MyStateMachineTransition> OutTransitions = new List<MyStateMachineTransition>();
    protected internal List<MyStateMachineTransition> InTransitions = new List<MyStateMachineTransition>();
    protected internal HashSet<MyStateMachineCursor> Cursors = new HashSet<MyStateMachineCursor>();
    public bool PassThrough;

    public string Name => this.m_name;

    public MyStateMachineNode(string name) => this.m_name = name;

    internal void TransitionAdded(MyStateMachineTransition transition) => this.TransitionAddedInternal(transition);

    protected virtual void TransitionAddedInternal(MyStateMachineTransition transition)
    {
    }

    internal void TransitionRemoved(MyStateMachineTransition transition) => this.TransitionRemovedInternal(transition);

    protected virtual void TransitionRemovedInternal(MyStateMachineTransition transition)
    {
    }

    internal void Expand(MyStateMachineCursor cursor, MyConcurrentHashSet<MyStringId> enquedActions) => this.ExpandInternal(cursor, enquedActions, 100);

    protected virtual void ExpandInternal(
      MyStateMachineCursor cursor,
      MyConcurrentHashSet<MyStringId> enquedActions,
      int passThrough)
    {
      MyStateMachineTransition transition;
      do
      {
        transition = (MyStateMachineTransition) null;
        List<MyStateMachineTransition> outTransitions = cursor.Node.OutTransitions;
        MyStringId action = MyStringId.NullOrEmpty;
        if (enquedActions.Count > 0)
        {
          int num1 = int.MaxValue;
          for (int index = 0; index < outTransitions.Count; ++index)
          {
            int num2 = outTransitions[index].Priority ?? int.MaxValue;
            enquedActions.Contains(outTransitions[index].Name);
            bool flag = false;
            foreach (MyStringId enquedAction in enquedActions)
            {
              if (enquedAction.String.ToLower() == outTransitions[index].Name.ToString().ToLower())
                flag = true;
            }
            if (flag && num2 <= num1 && (outTransitions[index].Conditions.Count == 0 || outTransitions[index].Evaluate()))
            {
              transition = outTransitions[index];
              num1 = num2;
              action = outTransitions[index].Name;
            }
          }
        }
        if (transition == null)
        {
          transition = cursor.Node.QueryNextTransition();
          foreach (MyStringId enquedAction in enquedActions)
            action = enquedAction;
        }
        if (transition != null)
          cursor.FollowTransition(transition, action);
      }
      while (transition != null && cursor.Node.PassThrough && passThrough-- > 0);
    }

    protected virtual MyStateMachineTransition QueryNextTransition()
    {
      for (int index = 0; index < this.OutTransitions.Count; ++index)
      {
        if (this.OutTransitions[index].Evaluate())
          return this.OutTransitions[index];
      }
      return (MyStateMachineTransition) null;
    }

    public virtual void OnUpdate(MyStateMachine stateMachine, List<string> eventCollection)
    {
    }
  }
}
