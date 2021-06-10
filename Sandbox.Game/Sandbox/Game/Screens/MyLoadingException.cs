// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyLoadingException
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRage;
using VRage.Utils;

namespace Sandbox.Game.Screens
{
  public class MyLoadingException : Exception
  {
    public MyLoadingException(MyStringId message, Exception innerException = null)
      : base(MyTexts.GetString(message), innerException)
    {
    }

    public MyLoadingException(string message, Exception innerException = null)
      : base(message.ToString(), innerException)
    {
    }
  }
}
