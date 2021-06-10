// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.BitStreamException
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;

namespace VRage.Library.Collections
{
  public class BitStreamException : Exception
  {
    public BitStreamException(Exception inner)
      : base("Error when reading using BitReader", inner)
    {
    }

    public BitStreamException(string message)
      : base(message)
    {
    }

    public BitStreamException(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}
