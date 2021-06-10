// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.Ingame.Utilities.MyIniParseResult
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

namespace VRage.Game.ModAPI.Ingame.Utilities
{
  public struct MyIniParseResult
  {
    private int m_lineNo;
    private readonly TextPtr m_ptr;
    public readonly string Error;

    public static bool operator ==(MyIniParseResult a, MyIniParseResult b) => a.Equals(b);

    public static bool operator !=(MyIniParseResult a, MyIniParseResult b) => !a.Equals(b);

    public bool Equals(MyIniParseResult other) => this.LineNo == other.LineNo && string.Equals(this.Error, other.Error);

    public override bool Equals(object obj) => obj != null && obj is MyIniParseResult other && this.Equals(other);

    public override int GetHashCode() => this.LineNo * 397 ^ (this.Error != null ? this.Error.GetHashCode() : 0);

    internal MyIniParseResult(TextPtr ptr, string error)
    {
      this.m_lineNo = 0;
      this.m_ptr = ptr;
      this.Error = error;
    }

    public int LineNo
    {
      get
      {
        if (this.m_lineNo == 0)
          this.m_lineNo = this.m_ptr.FindLineNo();
        return this.m_lineNo;
      }
    }

    public bool Success => this.IsDefined && this.Error == null;

    public bool IsDefined => !this.m_ptr.IsEmpty;

    public override string ToString() => !this.Success ? string.Format("Line {0}: {1}", (object) this.LineNo, (object) this.Error) : "Success";
  }
}
