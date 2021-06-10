// Decompiled with JetBrains decompiler
// Type: VRage.Game.VisualScripting.Missions.MyVSStateMachineSpreadNode
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Collections;
using VRage.Generics;
using VRage.Utils;

namespace VRage.Game.VisualScripting.Missions
{
  public class MyVSStateMachineSpreadNode : MyStateMachineNode
  {
    public MyVSStateMachineSpreadNode(string nodeName)
      : base(nodeName)
    {
    }

    protected override void ExpandInternal(
      MyStateMachineCursor cursor,
      MyConcurrentHashSet<MyStringId> enquedActions,
      int passThrough)
    {
      if (this.OutTransitions.Count == 0)
        return;
      MyStateMachine stateMachine = cursor.StateMachine;
      stateMachine.DeleteCursor(cursor.Id);
      for (int index = 0; index < this.OutTransitions.Count; ++index)
        stateMachine.CreateCursor(this.OutTransitions[index].TargetNode.Name);
    }
  }
}
