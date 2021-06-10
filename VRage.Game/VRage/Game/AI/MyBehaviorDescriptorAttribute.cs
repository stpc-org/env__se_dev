// Decompiled with JetBrains decompiler
// Type: VRage.Game.AI.MyBehaviorDescriptorAttribute
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;

namespace VRage.Game.AI
{
  [AttributeUsage(AttributeTargets.Class)]
  public class MyBehaviorDescriptorAttribute : Attribute
  {
    public readonly string DescriptorCategory;

    public MyBehaviorDescriptorAttribute(string category) => this.DescriptorCategory = category;
  }
}
