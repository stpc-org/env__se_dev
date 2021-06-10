// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.MySessionComponentDescriptor
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using VRage.ObjectBuilders;

namespace VRage.Game.Components
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
  public class MySessionComponentDescriptor : Attribute
  {
    public MyUpdateOrder UpdateOrder;
    public int Priority;
    public MyObjectBuilderType ObjectBuilderType;
    public Type ComponentType;

    public bool IsServerOnly { get; private set; }

    public MySessionComponentDescriptor(MyUpdateOrder updateOrder)
      : this(updateOrder, 1000)
    {
    }

    public MySessionComponentDescriptor(MyUpdateOrder updateOrder, int priority)
      : this(updateOrder, priority, (Type) null)
    {
    }

    public MySessionComponentDescriptor(
      MyUpdateOrder updateOrder,
      int priority,
      Type obType,
      Type registrationType = null,
      bool serverOnly = false)
    {
      this.UpdateOrder = updateOrder;
      this.Priority = priority;
      this.ObjectBuilderType = (MyObjectBuilderType) obType;
      this.IsServerOnly = serverOnly;
      if (obType != (Type) null && !typeof (MyObjectBuilder_SessionComponent).IsAssignableFrom(obType))
        this.ObjectBuilderType = MyObjectBuilderType.Invalid;
      this.ComponentType = registrationType;
    }
  }
}
