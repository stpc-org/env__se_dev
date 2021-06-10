// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Analytics.MyAnalyticsSpecificationException
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;

namespace Sandbox.Engine.Analytics
{
  internal class MyAnalyticsSpecificationException : Exception
  {
    public MyAnalyticsSpecificationException()
    {
    }

    public MyAnalyticsSpecificationException(string message)
      : base(message)
    {
    }

    public MyAnalyticsSpecificationException(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}
