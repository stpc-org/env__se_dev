// Decompiled with JetBrains decompiler
// Type: VRage.Network.NetworkId
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRage.Network
{
  public struct NetworkId : IComparable<NetworkId>, IEquatable<NetworkId>
  {
    public static readonly NetworkId Invalid = new NetworkId(0U);
    internal uint Value;

    public bool IsInvalid => this.Value == 0U;

    public bool IsValid => this.Value > 0U;

    internal NetworkId(uint value) => this.Value = value;

    public int CompareTo(NetworkId other) => this.Value.CompareTo(other.Value);

    public bool Equals(NetworkId other) => (int) this.Value == (int) other.Value;

    public override string ToString() => this.Value.ToString();
  }
}
