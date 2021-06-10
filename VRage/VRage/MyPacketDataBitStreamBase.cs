// Decompiled with JetBrains decompiler
// Type: VRage.MyPacketDataBitStreamBase
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using VRage.Library.Collections;

namespace VRage
{
  public abstract class MyPacketDataBitStreamBase : IPacketData
  {
    private BitStream m_stream = new BitStream();
    protected bool m_returned;

    public BitStream Stream => this.m_stream;

    protected MyPacketDataBitStreamBase() => this.m_stream.ResetWrite();

    public abstract byte[] Data { get; }

    public abstract IntPtr Ptr { get; }

    public abstract int Size { get; }

    public abstract int Offset { get; }

    public abstract void Return();

    public void Dispose()
    {
      this.m_stream.Dispose();
      this.m_stream = (BitStream) null;
    }
  }
}
