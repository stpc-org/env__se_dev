﻿// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MySerializeException
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRage.Serialization
{
  public class MySerializeException : Exception
  {
    public MySerializeErrorEnum Error;

    public MySerializeException(MySerializeErrorEnum error) => this.Error = error;
  }
}
