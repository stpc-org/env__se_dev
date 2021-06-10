// Decompiled with JetBrains decompiler
// Type: VRage.Game.VisualScripting.Missions.MyVSStateMachine
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game.ObjectBuilders.VisualScripting;
using VRage.Generics;
using VRage.Utils;

namespace VRage.Game.VisualScripting.Missions
{
  public class MyVSStateMachine : MyStateMachine
  {
    private MyObjectBuilder_ScriptSM m_objectBuilder;
    private readonly MyConcurrentHashSet<MyStringId> m_cachedActions = new MyConcurrentHashSet<MyStringId>();
    private readonly List<MyStateMachineCursor> m_cursorsToInit = new List<MyStateMachineCursor>();
    private readonly List<MyStateMachineCursor> m_cursorsToDeserialize = new List<MyStateMachineCursor>();
    private long m_ownerId;

    public int ActiveCursorCount => this.m_activeCursors.Count;

    public event Action<MyVSStateMachineNode, MyVSStateMachineNode> CursorStateChanged;

    public long OwnerId
    {
      get => this.m_ownerId;
      set
      {
        foreach (MyStateMachineNode stateMachineNode1 in this.m_nodes.Values)
        {
          if (stateMachineNode1 is MyVSStateMachineNode stateMachineNode && stateMachineNode.ScriptInstance != null)
            stateMachineNode.ScriptInstance.OwnerId = value;
        }
        this.m_ownerId = value;
      }
    }

    public MyStateMachineCursor RestoreCursor(string nodeName)
    {
      foreach (MyStateMachineCursor stateMachineCursor in this.m_activeCursorsById.Values)
      {
        if (stateMachineCursor.Node.Name == nodeName)
          return (MyStateMachineCursor) null;
      }
      MyStateMachineCursor cursor = base.CreateCursor(nodeName);
      if (cursor != null)
      {
        cursor.OnCursorStateChanged += new MyStateMachineCursor.CursorStateChanged(this.OnCursorStateChanged);
        if (cursor.Node is MyVSStateMachineNode)
          this.m_cursorsToDeserialize.Add(cursor);
      }
      return cursor;
    }

    public override MyStateMachineCursor CreateCursor(string nodeName)
    {
      foreach (MyStateMachineCursor stateMachineCursor in this.m_activeCursorsById.Values)
      {
        if (stateMachineCursor.Node.Name == nodeName)
          return (MyStateMachineCursor) null;
      }
      MyStateMachineCursor cursor = base.CreateCursor(nodeName);
      if (cursor != null)
      {
        cursor.OnCursorStateChanged += new MyStateMachineCursor.CursorStateChanged(this.OnCursorStateChanged);
        if (cursor.Node is MyVSStateMachineNode)
          this.m_cursorsToInit.Add(cursor);
      }
      return cursor;
    }

    private void OnCursorStateChanged(
      int transitionId,
      MyStringId action,
      MyStateMachineNode node,
      MyStateMachine stateMachine)
    {
      if (this.FindTransitionWithStart(transitionId).StartNode is MyVSStateMachineNode startNode)
        startNode.DisposeScript();
      if (node is MyVSStateMachineNode to)
        to.ActivateScript();
      this.NotifyStateChanged(startNode, to);
    }

    public override void Update(List<string> eventCollection)
    {
      this.m_activeCursors.ApplyChanges();
      foreach (MyStateMachineCursor stateMachineCursor in this.m_cursorsToDeserialize)
      {
        if (stateMachineCursor.Node is MyVSStateMachineNode node)
          node.ActivateScript(true);
      }
      this.m_cursorsToDeserialize.Clear();
      foreach (MyStateMachineCursor stateMachineCursor in this.m_cursorsToInit)
      {
        if (stateMachineCursor.Node is MyVSStateMachineNode node)
          node.ActivateScript();
      }
      this.m_cursorsToInit.Clear();
      foreach (MyStringId cachedAction in this.m_cachedActions)
        this.m_enqueuedActions.Add(cachedAction);
      this.m_cachedActions.Clear();
      base.Update(eventCollection);
    }

    public void Init(MyObjectBuilder_ScriptSM ob, long? ownerId = null)
    {
      this.m_objectBuilder = ob;
      this.Name = ob.Name;
      if (ob.Nodes != null)
      {
        foreach (MyObjectBuilder_ScriptSMNode node in ob.Nodes)
        {
          MyStateMachineNode newNode;
          switch (node)
          {
            case MyObjectBuilder_ScriptSMFinalNode _:
              newNode = (MyStateMachineNode) new MyVSStateMachineFinalNode(node.Name, ((MyObjectBuilder_ScriptSMFinalNode) node).ShowCredits, ((MyObjectBuilder_ScriptSMFinalNode) node).CloseSession);
              break;
            case MyObjectBuilder_ScriptSMSpreadNode _:
              newNode = (MyStateMachineNode) new MyVSStateMachineSpreadNode(node.Name);
              break;
            case MyObjectBuilder_ScriptSMBarrierNode _:
              newNode = (MyStateMachineNode) new MyVSStateMachineBarrierNode(node.Name);
              break;
            default:
              Type type = MyVRage.Platform.Scripting.VSTAssemblyProvider.GetType("VisualScripting.CustomScripts." + node.ScriptClassName);
              MyVSStateMachineNode stateMachineNode = new MyVSStateMachineNode(node.Name, type);
              if (stateMachineNode.ScriptInstance != null)
                stateMachineNode.ScriptInstance.OwnerId = ownerId.HasValue ? ownerId.Value : ob.OwnerId;
              newNode = (MyStateMachineNode) stateMachineNode;
              break;
          }
          this.AddNode(newNode);
        }
      }
      if (ob.Transitions != null)
      {
        foreach (MyObjectBuilder_ScriptSMTransition transition in ob.Transitions)
          this.AddTransition(transition.From, transition.To, name: transition.Name);
      }
      if (ob.Cursors == null)
        return;
      foreach (MyObjectBuilder_ScriptSMCursor cursor in ob.Cursors)
        this.CreateCursor(cursor.NodeName);
    }

    public MyObjectBuilder_ScriptSM GetObjectBuilder()
    {
      IReadOnlyList<MyStateMachineCursor> stateMachineCursorList = (IReadOnlyList<MyStateMachineCursor>) this.m_activeCursors;
      if (this.m_activeCursors.HasChanges)
        stateMachineCursorList = (IReadOnlyList<MyStateMachineCursor>) this.m_activeCursors.CopyWithChanges();
      this.m_objectBuilder.Cursors = new MyObjectBuilder_ScriptSMCursor[stateMachineCursorList.Count];
      this.m_objectBuilder.OwnerId = this.m_ownerId;
      for (int index = 0; index < stateMachineCursorList.Count; ++index)
        this.m_objectBuilder.Cursors[index] = new MyObjectBuilder_ScriptSMCursor()
        {
          NodeName = stateMachineCursorList[index].Node.Name
        };
      return this.m_objectBuilder;
    }

    public void Dispose()
    {
      this.m_activeCursors.ApplyChanges();
      for (int index = 0; index < this.m_activeCursors.Count; ++index)
      {
        if (this.m_activeCursors[index].Node is MyVSStateMachineNode node)
          node.DisposeScript();
        this.DeleteCursor(this.m_activeCursors[index].Id);
      }
      this.m_activeCursors.ApplyChanges();
      this.m_activeCursors.Clear();
    }

    public void TriggerCachedAction(MyStringId actionName) => this.m_cachedActions.Add(actionName);

    private void NotifyStateChanged(MyVSStateMachineNode from, MyVSStateMachineNode to)
    {
      if (this.CursorStateChanged == null)
        return;
      this.CursorStateChanged(from, to);
    }
  }
}
