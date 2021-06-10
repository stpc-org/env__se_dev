// Decompiled with JetBrains decompiler
// Type: VRage.Network.ClientReadyDataMsg
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Runtime.CompilerServices;

namespace VRage.Network
{
  [Serializable]
  public struct ClientReadyDataMsg
  {
    public bool ForcePlayoutDelayBuffer;
    public bool UsePlayoutDelayBufferForCharacter;
    public bool UsePlayoutDelayBufferForJetpack;
    public bool UsePlayoutDelayBufferForGrids;

    protected class VRage_Network_ClientReadyDataMsg\u003C\u003EForcePlayoutDelayBuffer\u003C\u003EAccessor : IMemberAccessor<ClientReadyDataMsg, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ClientReadyDataMsg owner, in bool value) => owner.ForcePlayoutDelayBuffer = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ClientReadyDataMsg owner, out bool value) => value = owner.ForcePlayoutDelayBuffer;
    }

    protected class VRage_Network_ClientReadyDataMsg\u003C\u003EUsePlayoutDelayBufferForCharacter\u003C\u003EAccessor : IMemberAccessor<ClientReadyDataMsg, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ClientReadyDataMsg owner, in bool value) => owner.UsePlayoutDelayBufferForCharacter = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ClientReadyDataMsg owner, out bool value) => value = owner.UsePlayoutDelayBufferForCharacter;
    }

    protected class VRage_Network_ClientReadyDataMsg\u003C\u003EUsePlayoutDelayBufferForJetpack\u003C\u003EAccessor : IMemberAccessor<ClientReadyDataMsg, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ClientReadyDataMsg owner, in bool value) => owner.UsePlayoutDelayBufferForJetpack = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ClientReadyDataMsg owner, out bool value) => value = owner.UsePlayoutDelayBufferForJetpack;
    }

    protected class VRage_Network_ClientReadyDataMsg\u003C\u003EUsePlayoutDelayBufferForGrids\u003C\u003EAccessor : IMemberAccessor<ClientReadyDataMsg, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ClientReadyDataMsg owner, in bool value) => owner.UsePlayoutDelayBufferForGrids = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ClientReadyDataMsg owner, out bool value) => value = owner.UsePlayoutDelayBufferForGrids;
    }
  }
}
