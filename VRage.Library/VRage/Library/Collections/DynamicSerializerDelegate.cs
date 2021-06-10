// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.DynamicSerializerDelegate
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;

namespace VRage.Library.Collections
{
  public delegate void DynamicSerializerDelegate(BitStream stream, Type baseType, ref Type obj);
}
