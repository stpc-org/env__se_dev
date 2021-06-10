// Decompiled with JetBrains decompiler
// Type: VRage.Generics.MyStateMachineTransitionWithStart
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.Generics
{
  public struct MyStateMachineTransitionWithStart
  {
    public MyStateMachineNode StartNode;
    public MyStateMachineTransition Transition;

    public MyStateMachineTransitionWithStart(
      MyStateMachineNode startNode,
      MyStateMachineTransition transition)
    {
      this.StartNode = startNode;
      this.Transition = transition;
    }
  }
}
