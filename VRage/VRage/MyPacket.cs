// Decompiled with JetBrains decompiler
// Type: VRage.MyPacket
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;

namespace VRage
{
  public abstract class MyPacket
  {
    public BitStream BitStream;
    public ByteStream ByteStream;
    public Endpoint Sender;
    public MyTimeSpan ReceivedTime;

    public abstract void Return();
  }
}
