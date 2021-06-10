// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyLoadingNeedDLCException
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

namespace Sandbox.Game.Screens
{
  public class MyLoadingNeedDLCException : MyLoadingException
  {
    public MyDLCs.MyDLC RequiredDLC { get; }

    public MyLoadingNeedDLCException(MyDLCs.MyDLC requiredDLC)
      : base(string.Empty)
      => this.RequiredDLC = requiredDLC;
  }
}
