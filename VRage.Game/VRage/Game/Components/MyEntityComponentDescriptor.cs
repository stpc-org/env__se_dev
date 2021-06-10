// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.MyEntityComponentDescriptor
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;

namespace VRage.Game.Components
{
  [AttributeUsage(AttributeTargets.Class)]
  public class MyEntityComponentDescriptor : Attribute
  {
    public Type EntityBuilderType;
    public string[] EntityBuilderSubTypeNames;
    public bool? EntityUpdate;

    [Obsolete("Use the 3 parameter overload instead!")]
    public MyEntityComponentDescriptor(
      Type entityBuilderType,
      params string[] entityBuilderSubTypeNames)
    {
      this.EntityBuilderType = entityBuilderType;
      this.EntityBuilderSubTypeNames = entityBuilderSubTypeNames;
      this.EntityUpdate = new bool?();
    }

    public MyEntityComponentDescriptor(
      Type entityBuilderType,
      bool useEntityUpdate,
      params string[] entityBuilderSubTypeNames)
    {
      this.EntityBuilderType = entityBuilderType;
      this.EntityUpdate = new bool?(useEntityUpdate);
      this.EntityBuilderSubTypeNames = entityBuilderSubTypeNames;
    }
  }
}
