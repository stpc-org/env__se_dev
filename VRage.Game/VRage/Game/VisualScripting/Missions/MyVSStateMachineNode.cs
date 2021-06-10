// Decompiled with JetBrains decompiler
// Type: VRage.Game.VisualScripting.Missions.MyVSStateMachineNode
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections;
using System.Collections.Generic;
using VRage.Generics;
using VRage.Generics.StateMachine;
using VRage.Library.Utils;
using VRage.Utils;

namespace VRage.Game.VisualScripting.Missions
{
  public class MyVSStateMachineNode : MyStateMachineNode
  {
    private readonly Type m_scriptType;
    private IMyStateMachineScript m_instance;
    private readonly Dictionary<MyStringId, IMyVariableStorage<bool>> m_transitionNamesToVariableStorages = new Dictionary<MyStringId, IMyVariableStorage<bool>>();

    public IMyStateMachineScript ScriptInstance => this.m_instance;

    public MyVSStateMachineNode(string name, Type script)
      : base(name)
      => this.m_scriptType = script;

    public override void OnUpdate(MyStateMachine stateMachine, List<string> eventCollection)
    {
      if (this.m_instance == null)
      {
        foreach (IMyVariableStorage<bool> myVariableStorage in this.m_transitionNamesToVariableStorages.Values)
          myVariableStorage.SetValue(MyStringId.GetOrCompute("Left"), true);
      }
      else
      {
        if (string.IsNullOrEmpty(this.m_instance.TransitionTo))
          this.m_instance.Update();
        if (string.IsNullOrEmpty(this.m_instance.TransitionTo))
          return;
        if (this.OutTransitions.Count == 0)
        {
          HashSet<MyStateMachineCursor>.Enumerator enumerator = this.Cursors.GetEnumerator();
          enumerator.MoveNext();
          MyStateMachineCursor current = enumerator.Current;
          stateMachine.DeleteCursor(current.Id);
        }
        else
        {
          MyStringId orCompute = MyStringId.GetOrCompute(this.m_instance.TransitionTo);
          foreach (MyStateMachineTransition outTransition in this.OutTransitions)
          {
            if (outTransition.Name == orCompute)
              break;
          }
          IMyVariableStorage<bool> myVariableStorage;
          if (!this.m_transitionNamesToVariableStorages.TryGetValue(MyStringId.GetOrCompute(this.m_instance.TransitionTo), out myVariableStorage))
            return;
          myVariableStorage.SetValue(MyStringId.GetOrCompute("Left"), true);
        }
      }
    }

    protected override void TransitionAddedInternal(MyStateMachineTransition transition)
    {
      base.TransitionAddedInternal(transition);
      if (transition.TargetNode == this)
        return;
      MyVSStateMachineNode.VSNodeVariableStorage nodeVariableStorage = new MyVSStateMachineNode.VSNodeVariableStorage();
      transition.Conditions.Add((IMyCondition) new MyCondition<bool>((IMyVariableStorage<bool>) nodeVariableStorage, MyCondition<bool>.MyOperation.Equal, "Left", "Right"));
      this.m_transitionNamesToVariableStorages.Add(transition.Name, (IMyVariableStorage<bool>) nodeVariableStorage);
    }

    public void ActivateScript(bool restored = false)
    {
      if (this.m_scriptType == (Type) null || this.m_instance != null)
        return;
      this.m_instance = Activator.CreateInstance(this.m_scriptType) as IMyStateMachineScript;
      if (restored)
        this.m_instance.Deserialize();
      this.m_instance.Init();
      foreach (IMyVariableStorage<bool> myVariableStorage in this.m_transitionNamesToVariableStorages.Values)
        myVariableStorage.SetValue(MyStringId.GetOrCompute("Left"), false);
    }

    public void DisposeScript()
    {
      if (this.m_instance == null)
        return;
      this.m_instance.Dispose();
      this.m_instance = (IMyStateMachineScript) null;
    }

    private class VSNodeVariableStorage : IMyVariableStorage<bool>, IEnumerable<KeyValuePair<MyStringId, bool>>, IEnumerable
    {
      private MyStringId left;
      private MyStringId right;
      private bool m_leftValue;
      private bool m_rightvalue = true;

      public VSNodeVariableStorage()
      {
        this.left = MyStringId.GetOrCompute("Left");
        this.right = MyStringId.GetOrCompute("Right");
      }

      public void SetValue(MyStringId key, bool newValue)
      {
        if (key == this.left)
          this.m_leftValue = newValue;
        if (!(key == this.right))
          return;
        this.m_rightvalue = newValue;
      }

      public bool GetValue(MyStringId key, out bool value)
      {
        value = false;
        if (key == this.left)
          value = this.m_leftValue;
        if (key == this.right)
          value = this.m_rightvalue;
        return true;
      }

      public IEnumerator<KeyValuePair<MyStringId, bool>> GetEnumerator()
      {
        yield return new KeyValuePair<MyStringId, bool>(this.left, this.m_leftValue);
        yield return new KeyValuePair<MyStringId, bool>(this.right, this.m_rightvalue);
      }

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
    }
  }
}
