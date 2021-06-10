// Decompiled with JetBrains decompiler
// Type: VRage.Generics.MyStateMachine
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Utils;

namespace VRage.Generics
{
  public class MyStateMachine
  {
    private int m_transitionIdCounter;
    protected Dictionary<string, MyStateMachineNode> m_nodes = new Dictionary<string, MyStateMachineNode>();
    protected Dictionary<int, MyStateMachineTransitionWithStart> m_transitions = new Dictionary<int, MyStateMachineTransitionWithStart>();
    protected Dictionary<int, MyStateMachineCursor> m_activeCursorsById = new Dictionary<int, MyStateMachineCursor>();
    protected CachingList<MyStateMachineCursor> m_activeCursors = new CachingList<MyStateMachineCursor>();
    protected MyConcurrentHashSet<MyStringId> m_enqueuedActions = new MyConcurrentHashSet<MyStringId>();

    public DictionaryReader<string, MyStateMachineNode> AllNodes => (DictionaryReader<string, MyStateMachineNode>) this.m_nodes;

    public DictionaryReader<int, MyStateMachineTransitionWithStart> AllTransitions => (DictionaryReader<int, MyStateMachineTransitionWithStart>) this.m_transitions;

    public List<MyStateMachineCursor> ActiveCursors => this.m_activeCursors.HasChanges ? this.m_activeCursors.CopyWithChanges() : new List<MyStateMachineCursor>((IEnumerable<MyStateMachineCursor>) this.m_activeCursors);

    public string Name { get; set; }

    public virtual MyStateMachineCursor CreateCursor(string nodeName)
    {
      MyStateMachineNode node = this.FindNode(nodeName);
      if (node == null)
        return (MyStateMachineCursor) null;
      MyStateMachineCursor entity = new MyStateMachineCursor(node, this);
      this.m_activeCursorsById.Add(entity.Id, entity);
      this.m_activeCursors.Add(entity);
      return entity;
    }

    public MyStateMachineCursor FindCursor(int cursorId)
    {
      MyStateMachineCursor stateMachineCursor;
      this.m_activeCursorsById.TryGetValue(cursorId, out stateMachineCursor);
      return stateMachineCursor;
    }

    public virtual bool DeleteCursor(int id)
    {
      if (!this.m_activeCursorsById.ContainsKey(id))
        return false;
      MyStateMachineCursor entity = this.m_activeCursorsById[id];
      this.m_activeCursorsById.Remove(id);
      this.m_activeCursors.Remove(entity);
      return true;
    }

    public virtual bool AddNode(MyStateMachineNode newNode)
    {
      if (this.FindNode(newNode.Name) != null)
        return false;
      this.m_nodes.Add(newNode.Name, newNode);
      return true;
    }

    public MyStateMachineNode FindNode(string nodeName)
    {
      MyStateMachineNode stateMachineNode;
      this.m_nodes.TryGetValue(nodeName, out stateMachineNode);
      return stateMachineNode;
    }

    public virtual bool DeleteNode(string nodeName)
    {
      MyStateMachineNode rtnNode;
      this.m_nodes.TryGetValue(nodeName, out rtnNode);
      if (rtnNode == null)
        return false;
      foreach (KeyValuePair<string, MyStateMachineNode> node in this.m_nodes)
        node.Value.OutTransitions.RemoveAll((Predicate<MyStateMachineTransition>) (x => x.TargetNode == rtnNode));
      this.m_nodes.Remove(nodeName);
      int index = 0;
      while (index < this.m_activeCursors.Count)
      {
        if (this.m_activeCursors[index].Node.Name == nodeName)
        {
          this.m_activeCursors[index].Node = (MyStateMachineNode) null;
          this.m_activeCursorsById.Remove(this.m_activeCursors[index].Id);
          this.m_activeCursors.Remove(this.m_activeCursors[index]);
        }
      }
      return true;
    }

    public virtual MyStateMachineTransition AddTransition(
      string startNodeName,
      string endNodeName,
      MyStateMachineTransition existingInstance = null,
      string name = null)
    {
      MyStateMachineNode node1 = this.FindNode(startNodeName);
      MyStateMachineNode node2 = this.FindNode(endNodeName);
      if (node1 == null || node2 == null)
        return (MyStateMachineTransition) null;
      MyStateMachineTransition transition;
      if (existingInstance == null)
      {
        transition = new MyStateMachineTransition();
        if (name != null)
          transition.Name = MyStringId.GetOrCompute(name);
      }
      else
        transition = existingInstance;
      ++this.m_transitionIdCounter;
      transition._SetId(this.m_transitionIdCounter);
      transition.TargetNode = node2;
      node1.OutTransitions.Add(transition);
      node2.InTransitions.Add(transition);
      this.m_transitions.Add(this.m_transitionIdCounter, new MyStateMachineTransitionWithStart(node1, transition));
      node1.TransitionAdded(transition);
      node2.TransitionAdded(transition);
      return transition;
    }

    public MyStateMachineTransition FindTransition(int transitionId) => this.FindTransitionWithStart(transitionId).Transition;

    public MyStateMachineTransitionWithStart FindTransitionWithStart(
      int transitionId)
    {
      MyStateMachineTransitionWithStart transitionWithStart;
      this.m_transitions.TryGetValue(transitionId, out transitionWithStart);
      return transitionWithStart;
    }

    public virtual bool DeleteTransition(int transitionId)
    {
      MyStateMachineTransitionWithStart transitionWithStart;
      if (!this.m_transitions.TryGetValue(transitionId, out transitionWithStart))
        return false;
      transitionWithStart.StartNode.TransitionRemoved(transitionWithStart.Transition);
      transitionWithStart.Transition.TargetNode.TransitionRemoved(transitionWithStart.Transition);
      this.m_transitions.Remove(transitionId);
      transitionWithStart.StartNode.OutTransitions.Remove(transitionWithStart.Transition);
      transitionWithStart.Transition.TargetNode.InTransitions.Remove(transitionWithStart.Transition);
      return true;
    }

    public virtual bool SetState(int cursorId, string nameOfNewState)
    {
      MyStateMachineNode node = this.FindNode(nameOfNewState);
      MyStateMachineCursor cursor = this.FindCursor(cursorId);
      if (node == null)
        return false;
      cursor.Node = node;
      return true;
    }

    public virtual void Update(List<string> eventCollection)
    {
      this.m_activeCursors.ApplyChanges();
      if (this.m_activeCursorsById.Count == 0)
      {
        this.m_enqueuedActions.Clear();
      }
      else
      {
        foreach (MyStateMachineCursor activeCursor in this.m_activeCursors)
        {
          activeCursor.Node.Expand(activeCursor, this.m_enqueuedActions);
          activeCursor.Node.OnUpdate(this, eventCollection);
        }
        this.m_enqueuedActions.Clear();
      }
    }

    public void TriggerAction(MyStringId actionName) => this.m_enqueuedActions.Add(actionName);

    public void SortTransitions()
    {
      foreach (MyStateMachineNode stateMachineNode in this.m_nodes.Values)
        stateMachineNode.OutTransitions.Sort((Comparison<MyStateMachineTransition>) ((transition1, transition2) =>
        {
          int? priority = transition1.Priority;
          int num1 = priority ?? int.MaxValue;
          priority = transition2.Priority;
          int num2 = priority ?? int.MaxValue;
          return num1.CompareTo(num2);
        }));
    }
  }
}
