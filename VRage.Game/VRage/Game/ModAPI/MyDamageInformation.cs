// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.MyDamageInformation
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.Utils;

namespace VRage.Game.ModAPI
{
  [ProtoContract]
  public struct MyDamageInformation
  {
    [ProtoMember(1)]
    public bool IsDeformation;
    [ProtoMember(4)]
    public float Amount;
    [ProtoMember(7)]
    public MyStringHash Type;
    [ProtoMember(10)]
    public long AttackerId;

    public MyDamageInformation(
      bool isDeformation,
      float amount,
      MyStringHash type,
      long attackerId)
    {
      this.IsDeformation = isDeformation;
      this.Amount = amount;
      this.Type = type;
      this.AttackerId = attackerId;
    }

    protected class VRage_Game_ModAPI_MyDamageInformation\u003C\u003EIsDeformation\u003C\u003EAccessor : IMemberAccessor<MyDamageInformation, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyDamageInformation owner, in bool value) => owner.IsDeformation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyDamageInformation owner, out bool value) => value = owner.IsDeformation;
    }

    protected class VRage_Game_ModAPI_MyDamageInformation\u003C\u003EAmount\u003C\u003EAccessor : IMemberAccessor<MyDamageInformation, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyDamageInformation owner, in float value) => owner.Amount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyDamageInformation owner, out float value) => value = owner.Amount;
    }

    protected class VRage_Game_ModAPI_MyDamageInformation\u003C\u003EType\u003C\u003EAccessor : IMemberAccessor<MyDamageInformation, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyDamageInformation owner, in MyStringHash value) => owner.Type = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyDamageInformation owner, out MyStringHash value) => value = owner.Type;
    }

    protected class VRage_Game_ModAPI_MyDamageInformation\u003C\u003EAttackerId\u003C\u003EAccessor : IMemberAccessor<MyDamageInformation, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyDamageInformation owner, in long value) => owner.AttackerId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyDamageInformation owner, out long value) => value = owner.AttackerId;
    }

    private class VRage_Game_ModAPI_MyDamageInformation\u003C\u003EActor : IActivator, IActivator<MyDamageInformation>
    {
      object IActivator.CreateInstance() => (object) new MyDamageInformation();

      MyDamageInformation IActivator<MyDamageInformation>.CreateInstance() => new MyDamageInformation();
    }
  }
}
