// Decompiled with JetBrains decompiler
// Type: VRage.Network.SerializeDelegate`7
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRage.Library.Collections;

namespace VRage.Network
{
  public delegate void SerializeDelegate<T1, T2, T3, T4, T5, T6, T7>(
    T1 inst,
    BitStream stream,
    ref T2 arg2,
    ref T3 arg3,
    ref T4 arg4,
    ref T5 arg5,
    ref T6 arg6,
    ref T7 arg7);
}
