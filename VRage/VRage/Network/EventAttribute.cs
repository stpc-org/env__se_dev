// Decompiled with JetBrains decompiler
// Type: VRage.Network.EventAttribute
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Runtime.CompilerServices;

namespace VRage.Network
{
  [AttributeUsage(AttributeTargets.Method)]
  public class EventAttribute : Attribute
  {
    public readonly int Order;
    public readonly string Serialization;

    public EventAttribute(string serializationMethod = null, [CallerLineNumber] int order = 0)
    {
      this.Order = order;
      this.Serialization = serializationMethod;
    }
  }
}
