// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.BehaviorTree.MyPerTreeBotMemory
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using VRage.Collections;
using VRage.Game;
using VRage.Utils;

namespace Sandbox.Game.AI.BehaviorTree
{
  public class MyPerTreeBotMemory
  {
    private readonly List<MyBehaviorTreeNodeMemory> m_nodesMemory;
    private readonly Dictionary<MyStringId, MyBBMemoryValue> m_blackboardMemory;

    public int NodesMemoryCount => this.m_nodesMemory.Count;

    public ListReader<MyBehaviorTreeNodeMemory> NodesMemory => new ListReader<MyBehaviorTreeNodeMemory>(this.m_nodesMemory);

    public IEnumerable<KeyValuePair<MyStringId, MyBBMemoryValue>> BBMemory => (IEnumerable<KeyValuePair<MyStringId, MyBBMemoryValue>>) this.m_blackboardMemory;

    public MyPerTreeBotMemory()
    {
      this.m_nodesMemory = new List<MyBehaviorTreeNodeMemory>(20);
      this.m_blackboardMemory = new Dictionary<MyStringId, MyBBMemoryValue>(20, (IEqualityComparer<MyStringId>) MyStringId.Comparer);
    }

    public void AddNodeMemory(MyBehaviorTreeNodeMemory nodeMemory) => this.m_nodesMemory.Add(nodeMemory);

    public void AddBlackboardMemoryInstance(string name, MyBBMemoryValue obj) => this.m_blackboardMemory.Add(MyStringId.GetOrCompute(name), obj);

    public void RemoveBlackboardMemoryInstance(MyStringId name) => this.m_blackboardMemory.Remove(name);

    public MyBehaviorTreeNodeMemory GetNodeMemoryByIndex(int index) => this.m_nodesMemory[index];

    public void ClearNodesData()
    {
      foreach (MyBehaviorTreeNodeMemory behaviorTreeNodeMemory in this.m_nodesMemory)
        behaviorTreeNodeMemory.ClearNodeState();
    }

    public void Clear()
    {
      this.m_nodesMemory.Clear();
      this.m_blackboardMemory.Clear();
    }

    public bool TryGetFromBlackboard<T>(MyStringId id, out T value) where T : MyBBMemoryValue
    {
      MyBBMemoryValue myBbMemoryValue;
      int num = this.m_blackboardMemory.TryGetValue(id, out myBbMemoryValue) ? 1 : 0;
      value = myBbMemoryValue as T;
      return num != 0;
    }

    public void SaveToBlackboard(MyStringId id, MyBBMemoryValue value)
    {
      if (!(id != MyStringId.NullOrEmpty))
        return;
      this.m_blackboardMemory[id] = value;
    }

    public MyBBMemoryValue TrySaveToBlackboard(MyStringId id, Type type)
    {
      if (!type.IsSubclassOf(typeof (MyBBMemoryValue)) && !(type == typeof (MyBBMemoryValue)))
        return (MyBBMemoryValue) null;
      if (type.GetConstructor(Type.EmptyTypes) == (ConstructorInfo) null)
        return (MyBBMemoryValue) null;
      MyBBMemoryValue instance = Activator.CreateInstance(type) as MyBBMemoryValue;
      this.m_blackboardMemory[id] = instance;
      return instance;
    }
  }
}
