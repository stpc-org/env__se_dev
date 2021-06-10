// Decompiled with JetBrains decompiler
// Type: VRage.Factory.MyDependencyAttribute
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;

namespace VRage.Factory
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
  public class MyDependencyAttribute : Attribute
  {
    public readonly Type Dependency;
    public bool Recursive;
    public bool Critical;

    public MyDependencyAttribute(Type dependency) => this.Dependency = dependency;
  }
}
