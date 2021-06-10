// Decompiled with JetBrains decompiler
// Type: System.BoolBlit
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

namespace System
{
  public struct BoolBlit
  {
    private byte m_value;

    internal BoolBlit(byte value) => this.m_value = value;

    public static implicit operator bool(BoolBlit b) => b.m_value > (byte) 0;

    public static implicit operator BoolBlit(bool b) => new BoolBlit(b ? byte.MaxValue : (byte) 0);

    public override string ToString() => ((bool) this).ToString();
  }
}
