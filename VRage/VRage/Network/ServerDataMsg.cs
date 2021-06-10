// Decompiled with JetBrains decompiler
// Type: VRage.Network.ServerDataMsg
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Runtime.CompilerServices;
using VRage.Library.Utils;
using VRage.Serialization;

namespace VRage.Network
{
  [Serializable]
  public struct ServerDataMsg
  {
    [Serialize(MyObjectFlags.DefaultZero)]
    public string WorldName;
    public MyGameModeEnum GameMode;
    public float InventoryMultiplier;
    public float AssemblerMultiplier;
    public float RefineryMultiplier;
    [Serialize(MyObjectFlags.DefaultZero)]
    public string HostName;
    public ulong WorldSize;
    public int AppVersion;
    public int MembersLimit;
    [Serialize(MyObjectFlags.DefaultZero)]
    public string DataHash;
    public float WelderMultiplier;
    public float GrinderMultiplier;
    public float BlocksInventoryMultiplier;

    [Serialize(MyObjectFlags.DefaultZero)]
    public string ServerPasswordSalt { get; set; }

    [Serialize(MyObjectFlags.DefaultZero)]
    public string ServerAnalyticsId { get; set; }

    protected class VRage_Network_ServerDataMsg\u003C\u003EWorldName\u003C\u003EAccessor : IMemberAccessor<ServerDataMsg, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ServerDataMsg owner, in string value) => owner.WorldName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ServerDataMsg owner, out string value) => value = owner.WorldName;
    }

    protected class VRage_Network_ServerDataMsg\u003C\u003EGameMode\u003C\u003EAccessor : IMemberAccessor<ServerDataMsg, MyGameModeEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ServerDataMsg owner, in MyGameModeEnum value) => owner.GameMode = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ServerDataMsg owner, out MyGameModeEnum value) => value = owner.GameMode;
    }

    protected class VRage_Network_ServerDataMsg\u003C\u003EInventoryMultiplier\u003C\u003EAccessor : IMemberAccessor<ServerDataMsg, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ServerDataMsg owner, in float value) => owner.InventoryMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ServerDataMsg owner, out float value) => value = owner.InventoryMultiplier;
    }

    protected class VRage_Network_ServerDataMsg\u003C\u003EAssemblerMultiplier\u003C\u003EAccessor : IMemberAccessor<ServerDataMsg, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ServerDataMsg owner, in float value) => owner.AssemblerMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ServerDataMsg owner, out float value) => value = owner.AssemblerMultiplier;
    }

    protected class VRage_Network_ServerDataMsg\u003C\u003ERefineryMultiplier\u003C\u003EAccessor : IMemberAccessor<ServerDataMsg, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ServerDataMsg owner, in float value) => owner.RefineryMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ServerDataMsg owner, out float value) => value = owner.RefineryMultiplier;
    }

    protected class VRage_Network_ServerDataMsg\u003C\u003EHostName\u003C\u003EAccessor : IMemberAccessor<ServerDataMsg, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ServerDataMsg owner, in string value) => owner.HostName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ServerDataMsg owner, out string value) => value = owner.HostName;
    }

    protected class VRage_Network_ServerDataMsg\u003C\u003EWorldSize\u003C\u003EAccessor : IMemberAccessor<ServerDataMsg, ulong>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ServerDataMsg owner, in ulong value) => owner.WorldSize = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ServerDataMsg owner, out ulong value) => value = owner.WorldSize;
    }

    protected class VRage_Network_ServerDataMsg\u003C\u003EAppVersion\u003C\u003EAccessor : IMemberAccessor<ServerDataMsg, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ServerDataMsg owner, in int value) => owner.AppVersion = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ServerDataMsg owner, out int value) => value = owner.AppVersion;
    }

    protected class VRage_Network_ServerDataMsg\u003C\u003EMembersLimit\u003C\u003EAccessor : IMemberAccessor<ServerDataMsg, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ServerDataMsg owner, in int value) => owner.MembersLimit = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ServerDataMsg owner, out int value) => value = owner.MembersLimit;
    }

    protected class VRage_Network_ServerDataMsg\u003C\u003EDataHash\u003C\u003EAccessor : IMemberAccessor<ServerDataMsg, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ServerDataMsg owner, in string value) => owner.DataHash = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ServerDataMsg owner, out string value) => value = owner.DataHash;
    }

    protected class VRage_Network_ServerDataMsg\u003C\u003EWelderMultiplier\u003C\u003EAccessor : IMemberAccessor<ServerDataMsg, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ServerDataMsg owner, in float value) => owner.WelderMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ServerDataMsg owner, out float value) => value = owner.WelderMultiplier;
    }

    protected class VRage_Network_ServerDataMsg\u003C\u003EGrinderMultiplier\u003C\u003EAccessor : IMemberAccessor<ServerDataMsg, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ServerDataMsg owner, in float value) => owner.GrinderMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ServerDataMsg owner, out float value) => value = owner.GrinderMultiplier;
    }

    protected class VRage_Network_ServerDataMsg\u003C\u003EBlocksInventoryMultiplier\u003C\u003EAccessor : IMemberAccessor<ServerDataMsg, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ServerDataMsg owner, in float value) => owner.BlocksInventoryMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ServerDataMsg owner, out float value) => value = owner.BlocksInventoryMultiplier;
    }

    protected class VRage_Network_ServerDataMsg\u003C\u003EServerPasswordSalt\u003C\u003EAccessor : IMemberAccessor<ServerDataMsg, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ServerDataMsg owner, in string value) => owner.ServerPasswordSalt = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ServerDataMsg owner, out string value) => value = owner.ServerPasswordSalt;
    }

    protected class VRage_Network_ServerDataMsg\u003C\u003EServerAnalyticsId\u003C\u003EAccessor : IMemberAccessor<ServerDataMsg, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ServerDataMsg owner, in string value) => owner.ServerAnalyticsId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ServerDataMsg owner, out string value) => value = owner.ServerAnalyticsId;
    }
  }
}
