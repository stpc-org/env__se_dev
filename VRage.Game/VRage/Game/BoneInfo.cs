// Decompiled with JetBrains decompiler
// Type: VRage.Game.BoneInfo
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  public struct BoneInfo
  {
    [ProtoMember(1)]
    public SerializableVector3I BonePosition;
    [ProtoMember(2)]
    public SerializableVector3UByte BoneOffset;

    protected class VRage_Game_BoneInfo\u003C\u003EBonePosition\u003C\u003EAccessor : IMemberAccessor<BoneInfo, SerializableVector3I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref BoneInfo owner, in SerializableVector3I value) => owner.BonePosition = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref BoneInfo owner, out SerializableVector3I value) => value = owner.BonePosition;
    }

    protected class VRage_Game_BoneInfo\u003C\u003EBoneOffset\u003C\u003EAccessor : IMemberAccessor<BoneInfo, SerializableVector3UByte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref BoneInfo owner, in SerializableVector3UByte value) => owner.BoneOffset = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref BoneInfo owner, out SerializableVector3UByte value) => value = owner.BoneOffset;
    }

    private class VRage_Game_BoneInfo\u003C\u003EActor : IActivator, IActivator<BoneInfo>
    {
      object IActivator.CreateInstance() => (object) new BoneInfo();

      BoneInfo IActivator<BoneInfo>.CreateInstance() => new BoneInfo();
    }
  }
}
