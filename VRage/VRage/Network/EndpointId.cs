// Decompiled with JetBrains decompiler
// Type: VRage.Network.EndpointId
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Runtime.CompilerServices;

namespace VRage.Network
{
  [Serializable]
  public struct EndpointId
  {
    public readonly ulong Value;
    public static EndpointId Null = new EndpointId(0UL);

    public bool IsNull => this.Value == 0UL;

    public bool IsValid => !this.IsNull;

    public EndpointId(ulong value) => this.Value = value;

    public override string ToString() => EndpointId.Format(this.Value);

    public static bool operator ==(EndpointId a, EndpointId b) => (long) a.Value == (long) b.Value;

    public static bool operator !=(EndpointId a, EndpointId b) => (long) a.Value != (long) b.Value;

    public bool Equals(EndpointId other) => (long) this.Value == (long) other.Value;

    public override bool Equals(object obj) => obj is EndpointId other && this.Equals(other);

    public override int GetHashCode() => this.Value.GetHashCode();

    public static string Format(in EndpointId id) => EndpointId.Format(id.Value);

    public static string Format(ulong endpointId) => endpointId == 0UL ? "[Null]" : string.Format("[{0:000}...{1:000}]", (object) (endpointId / 10000000000000000UL), (object) (endpointId % 1000UL));

    protected class VRage_Network_EndpointId\u003C\u003EValue\u003C\u003EAccessor : IMemberAccessor<EndpointId, ulong>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref EndpointId owner, in ulong value) => owner.Value = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref EndpointId owner, out ulong value) => value = owner.Value;
    }
  }
}
