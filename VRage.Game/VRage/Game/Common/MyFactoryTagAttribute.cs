// Decompiled with JetBrains decompiler
// Type: VRage.Game.Common.MyFactoryTagAttribute
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;

namespace VRage.Game.Common
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
  public class MyFactoryTagAttribute : Attribute
  {
    public readonly Type ObjectBuilderType;
    public Type ProducedType;
    public bool IsMain;

    public MyFactoryTagAttribute(Type objectBuilderType, bool mainBuilder = true)
    {
      this.ObjectBuilderType = objectBuilderType;
      this.IsMain = mainBuilder;
    }
  }
}
