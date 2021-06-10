// Decompiled with JetBrains decompiler
// Type: VRage.Game.AI.MyBehaviorTreeActionAttribute
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;

namespace VRage.Game.AI
{
  [AttributeUsage(AttributeTargets.Method, Inherited = true)]
  public class MyBehaviorTreeActionAttribute : Attribute
  {
    public readonly string ActionName;
    public readonly MyBehaviorTreeActionType ActionType;
    public bool ReturnsRunning;

    public MyBehaviorTreeActionAttribute(string actionName)
      : this(actionName, MyBehaviorTreeActionType.BODY)
    {
    }

    public MyBehaviorTreeActionAttribute(string actionName, MyBehaviorTreeActionType type)
    {
      this.ActionName = actionName;
      this.ActionType = type;
      this.ReturnsRunning = true;
    }
  }
}
