﻿// Decompiled with JetBrains decompiler
// Type: System.Reflection.Setter`2
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

namespace System.Reflection
{
  public delegate void Setter<T, TMember>(ref T obj, in TMember value);
}
