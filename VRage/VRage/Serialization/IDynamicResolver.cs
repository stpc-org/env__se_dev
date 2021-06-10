// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.IDynamicResolver
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using VRage.Library.Collections;

namespace VRage.Serialization
{
  public interface IDynamicResolver
  {
    void Serialize(BitStream stream, Type baseType, ref Type obj);
  }
}
