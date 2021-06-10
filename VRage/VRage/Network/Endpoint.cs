// Decompiled with JetBrains decompiler
// Type: VRage.Network.Endpoint
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.Network
{
  public struct Endpoint
  {
    public readonly EndpointId Id;
    public readonly byte Index;

    public Endpoint(EndpointId id, byte index)
    {
      this.Id = id;
      this.Index = index;
    }

    public Endpoint(ulong id, byte index)
    {
      this.Id = new EndpointId(id);
      this.Index = index;
    }

    public static bool operator ==(Endpoint a, Endpoint b) => a.Id == b.Id && (int) a.Index == (int) b.Index;

    public static bool operator !=(Endpoint a, Endpoint b) => !(a == b);

    private bool Equals(Endpoint other) => this == other;

    public override bool Equals(object obj) => obj is Endpoint other && this.Equals(other);

    public override int GetHashCode() => this.Id.GetHashCode() ^ this.Index.GetHashCode();
  }
}
