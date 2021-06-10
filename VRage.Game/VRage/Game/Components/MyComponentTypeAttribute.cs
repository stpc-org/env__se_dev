// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.MyComponentTypeAttribute
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;

namespace VRage.Game.Components
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
  public class MyComponentTypeAttribute : Attribute
  {
    public readonly Type ComponentType;

    public MyComponentTypeAttribute(Type componentType) => this.ComponentType = componentType;
  }
}
