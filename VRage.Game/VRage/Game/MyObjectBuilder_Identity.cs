// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_Identity
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_Identity : MyObjectBuilder_Base
  {
    [ProtoMember(1)]
    public long IdentityId;
    [ProtoMember(4)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public string DisplayName;
    [ProtoMember(7)]
    public long CharacterEntityId;
    [ProtoMember(10)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public string Model;
    [ProtoMember(13)]
    public SerializableVector3? ColorMask;
    [ProtoMember(16)]
    public int BlockLimitModifier;
    [ProtoMember(19)]
    public DateTime LastLoginTime;
    [ProtoMember(22, IsRequired = false)]
    public HashSet<long> SavedCharacters;
    [ProtoMember(25)]
    public DateTime LastLogoutTime;
    [ProtoMember(28)]
    public List<long> RespawnShips;
    [ProtoMember(31, IsRequired = false)]
    [NoSerialize]
    public Vector3D? LastDeathPosition;
    [ProtoMember(33)]
    public List<long> ActiveContracts;
    [ProtoMember(35)]
    public int TransferedPCUDelta;

    [NoSerialize]
    public long PlayerId
    {
      get => this.IdentityId;
      set => this.IdentityId = value;
    }

    public bool ShouldSerializePlayerId() => false;

    public bool ShouldSerializeColorMask() => this.ColorMask.HasValue;

    protected class VRage_Game_MyObjectBuilder_Identity\u003C\u003EIdentityId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Identity, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Identity owner, in long value) => owner.IdentityId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Identity owner, out long value) => value = owner.IdentityId;
    }

    protected class VRage_Game_MyObjectBuilder_Identity\u003C\u003EDisplayName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Identity, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Identity owner, in string value) => owner.DisplayName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Identity owner, out string value) => value = owner.DisplayName;
    }

    protected class VRage_Game_MyObjectBuilder_Identity\u003C\u003ECharacterEntityId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Identity, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Identity owner, in long value) => owner.CharacterEntityId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Identity owner, out long value) => value = owner.CharacterEntityId;
    }

    protected class VRage_Game_MyObjectBuilder_Identity\u003C\u003EModel\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Identity, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Identity owner, in string value) => owner.Model = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Identity owner, out string value) => value = owner.Model;
    }

    protected class VRage_Game_MyObjectBuilder_Identity\u003C\u003EColorMask\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Identity, SerializableVector3?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Identity owner, in SerializableVector3? value) => owner.ColorMask = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Identity owner, out SerializableVector3? value) => value = owner.ColorMask;
    }

    protected class VRage_Game_MyObjectBuilder_Identity\u003C\u003EBlockLimitModifier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Identity, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Identity owner, in int value) => owner.BlockLimitModifier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Identity owner, out int value) => value = owner.BlockLimitModifier;
    }

    protected class VRage_Game_MyObjectBuilder_Identity\u003C\u003ELastLoginTime\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Identity, DateTime>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Identity owner, in DateTime value) => owner.LastLoginTime = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Identity owner, out DateTime value) => value = owner.LastLoginTime;
    }

    protected class VRage_Game_MyObjectBuilder_Identity\u003C\u003ESavedCharacters\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Identity, HashSet<long>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Identity owner, in HashSet<long> value) => owner.SavedCharacters = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Identity owner, out HashSet<long> value) => value = owner.SavedCharacters;
    }

    protected class VRage_Game_MyObjectBuilder_Identity\u003C\u003ELastLogoutTime\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Identity, DateTime>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Identity owner, in DateTime value) => owner.LastLogoutTime = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Identity owner, out DateTime value) => value = owner.LastLogoutTime;
    }

    protected class VRage_Game_MyObjectBuilder_Identity\u003C\u003ERespawnShips\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Identity, List<long>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Identity owner, in List<long> value) => owner.RespawnShips = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Identity owner, out List<long> value) => value = owner.RespawnShips;
    }

    protected class VRage_Game_MyObjectBuilder_Identity\u003C\u003ELastDeathPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Identity, Vector3D?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Identity owner, in Vector3D? value) => owner.LastDeathPosition = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Identity owner, out Vector3D? value) => value = owner.LastDeathPosition;
    }

    protected class VRage_Game_MyObjectBuilder_Identity\u003C\u003EActiveContracts\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Identity, List<long>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Identity owner, in List<long> value) => owner.ActiveContracts = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Identity owner, out List<long> value) => value = owner.ActiveContracts;
    }

    protected class VRage_Game_MyObjectBuilder_Identity\u003C\u003ETransferedPCUDelta\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Identity, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Identity owner, in int value) => owner.TransferedPCUDelta = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Identity owner, out int value) => value = owner.TransferedPCUDelta;
    }

    protected class VRage_Game_MyObjectBuilder_Identity\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Identity, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Identity owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Identity owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Identity\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Identity, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Identity owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Identity owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Identity\u003C\u003EPlayerId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Identity, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Identity owner, in long value) => owner.PlayerId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Identity owner, out long value) => value = owner.PlayerId;
    }

    protected class VRage_Game_MyObjectBuilder_Identity\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Identity, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Identity owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Identity owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Identity\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Identity, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Identity owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Identity owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_Identity\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Identity>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_Identity();

      MyObjectBuilder_Identity IActivator<MyObjectBuilder_Identity>.CreateInstance() => new MyObjectBuilder_Identity();
    }
  }
}
