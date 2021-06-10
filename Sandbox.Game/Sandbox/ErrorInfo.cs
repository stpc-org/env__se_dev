// Decompiled with JetBrains decompiler
// Type: Sandbox.ErrorInfo
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

namespace Sandbox
{
  internal class ErrorInfo
  {
    public string Match;
    public string Caption;
    public string Message;

    public ErrorInfo(string match, string caption, string message)
    {
      this.Match = match;
      this.Caption = caption;
      this.Message = message;
    }
  }
}
