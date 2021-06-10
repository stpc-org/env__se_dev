// Decompiled with JetBrains decompiler
// Type: VRage.Network.CallSite
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Reflection;
using VRage.Library.Collections;

namespace VRage.Network
{
  public abstract class CallSite
  {
    public readonly MySynchronizedTypeInfo OwnerType;
    public readonly uint Id;
    public readonly MethodInfo MethodInfo;
    public readonly CallSiteFlags CallSiteFlags;
    public readonly ValidationType ValidationFlags;
    public readonly float DistanceRadiusSquared;

    public bool HasClientFlag => (this.CallSiteFlags & CallSiteFlags.Client) == CallSiteFlags.Client;

    public bool HasServerFlag => (this.CallSiteFlags & CallSiteFlags.Server) == CallSiteFlags.Server;

    public bool HasServerInvokedFlag => (this.CallSiteFlags & CallSiteFlags.ServerInvoked) == CallSiteFlags.ServerInvoked;

    public bool HasBroadcastFlag => (this.CallSiteFlags & CallSiteFlags.Broadcast) == CallSiteFlags.Broadcast;

    public bool HasBroadcastExceptFlag => (this.CallSiteFlags & CallSiteFlags.BroadcastExcept) == CallSiteFlags.BroadcastExcept;

    public bool HasRefreshReplicableFlag => (this.CallSiteFlags & CallSiteFlags.RefreshReplicable) == CallSiteFlags.RefreshReplicable;

    public bool IsReliable => (this.CallSiteFlags & CallSiteFlags.Reliable) == CallSiteFlags.Reliable;

    public bool IsBlocking => (this.CallSiteFlags & CallSiteFlags.Blocking) == CallSiteFlags.Blocking;

    public bool HasDistanceRadius => (this.CallSiteFlags & CallSiteFlags.DistanceRadius) == CallSiteFlags.DistanceRadius;

    public CallSite(
      MySynchronizedTypeInfo owner,
      uint id,
      MethodInfo info,
      CallSiteFlags flags,
      ValidationType validationFlags,
      float distanceRadius)
    {
      this.OwnerType = owner;
      this.Id = id;
      this.MethodInfo = info;
      this.CallSiteFlags = flags;
      this.ValidationFlags = validationFlags;
      this.DistanceRadiusSquared = distanceRadius * distanceRadius;
    }

    public abstract bool Invoke(BitStream stream, object obj, bool validate);

    public override string ToString() => string.Format("{0}.{1}", (object) this.MethodInfo.DeclaringType.Name, (object) this.MethodInfo.Name);
  }
}
