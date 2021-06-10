// Decompiled with JetBrains decompiler
// Type: VRage.Network.ConnectedClientDataMsg
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Runtime.CompilerServices;
using VRage.Serialization;

namespace VRage.Network
{
  [Serializable]
  public struct ConnectedClientDataMsg
  {
    public EndpointId ClientId;
    [Serialize(MyObjectFlags.DefaultZero)]
    public string ServiceName;
    [Serialize(MyObjectFlags.DefaultZero)]
    public string Name;
    public bool IsAdmin;
    public bool Join;
    [Serialize(MyObjectFlags.DefaultZero)]
    public byte[] Token;
    public bool ExperimentalMode;
    public bool IsProfiling;

    protected class VRage_Network_ConnectedClientDataMsg\u003C\u003EClientId\u003C\u003EAccessor : IMemberAccessor<ConnectedClientDataMsg, EndpointId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ConnectedClientDataMsg owner, in EndpointId value) => owner.ClientId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ConnectedClientDataMsg owner, out EndpointId value) => value = owner.ClientId;
    }

    protected class VRage_Network_ConnectedClientDataMsg\u003C\u003EServiceName\u003C\u003EAccessor : IMemberAccessor<ConnectedClientDataMsg, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ConnectedClientDataMsg owner, in string value) => owner.ServiceName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ConnectedClientDataMsg owner, out string value) => value = owner.ServiceName;
    }

    protected class VRage_Network_ConnectedClientDataMsg\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<ConnectedClientDataMsg, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ConnectedClientDataMsg owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ConnectedClientDataMsg owner, out string value) => value = owner.Name;
    }

    protected class VRage_Network_ConnectedClientDataMsg\u003C\u003EIsAdmin\u003C\u003EAccessor : IMemberAccessor<ConnectedClientDataMsg, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ConnectedClientDataMsg owner, in bool value) => owner.IsAdmin = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ConnectedClientDataMsg owner, out bool value) => value = owner.IsAdmin;
    }

    protected class VRage_Network_ConnectedClientDataMsg\u003C\u003EJoin\u003C\u003EAccessor : IMemberAccessor<ConnectedClientDataMsg, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ConnectedClientDataMsg owner, in bool value) => owner.Join = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ConnectedClientDataMsg owner, out bool value) => value = owner.Join;
    }

    protected class VRage_Network_ConnectedClientDataMsg\u003C\u003EToken\u003C\u003EAccessor : IMemberAccessor<ConnectedClientDataMsg, byte[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ConnectedClientDataMsg owner, in byte[] value) => owner.Token = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ConnectedClientDataMsg owner, out byte[] value) => value = owner.Token;
    }

    protected class VRage_Network_ConnectedClientDataMsg\u003C\u003EExperimentalMode\u003C\u003EAccessor : IMemberAccessor<ConnectedClientDataMsg, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ConnectedClientDataMsg owner, in bool value) => owner.ExperimentalMode = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ConnectedClientDataMsg owner, out bool value) => value = owner.ExperimentalMode;
    }

    protected class VRage_Network_ConnectedClientDataMsg\u003C\u003EIsProfiling\u003C\u003EAccessor : IMemberAccessor<ConnectedClientDataMsg, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ConnectedClientDataMsg owner, in bool value) => owner.IsProfiling = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ConnectedClientDataMsg owner, out bool value) => value = owner.IsProfiling;
    }
  }
}
