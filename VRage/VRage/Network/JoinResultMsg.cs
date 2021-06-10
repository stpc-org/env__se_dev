// Decompiled with JetBrains decompiler
// Type: VRage.Network.JoinResultMsg
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Runtime.CompilerServices;

namespace VRage.Network
{
  [Serializable]
  public struct JoinResultMsg
  {
    public JoinResult JoinResult;
    public bool ServerExperimental;
    public ulong Admin;

    protected class VRage_Network_JoinResultMsg\u003C\u003EJoinResult\u003C\u003EAccessor : IMemberAccessor<JoinResultMsg, JoinResult>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref JoinResultMsg owner, in JoinResult value) => owner.JoinResult = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref JoinResultMsg owner, out JoinResult value) => value = owner.JoinResult;
    }

    protected class VRage_Network_JoinResultMsg\u003C\u003EServerExperimental\u003C\u003EAccessor : IMemberAccessor<JoinResultMsg, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref JoinResultMsg owner, in bool value) => owner.ServerExperimental = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref JoinResultMsg owner, out bool value) => value = owner.ServerExperimental;
    }

    protected class VRage_Network_JoinResultMsg\u003C\u003EAdmin\u003C\u003EAccessor : IMemberAccessor<JoinResultMsg, ulong>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref JoinResultMsg owner, in ulong value) => owner.Admin = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref JoinResultMsg owner, out ulong value) => value = owner.Admin;
    }
  }
}
