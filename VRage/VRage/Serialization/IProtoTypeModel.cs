// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.IProtoTypeModel
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using ProtoBuf.Meta;
using System;
using System.Collections.Generic;

namespace VRage.Serialization
{
  public interface IProtoTypeModel
  {
    TypeModel Model { get; }

    void RegisterTypes(IEnumerable<Type> types);

    void FlushCaches();
  }
}
