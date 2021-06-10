// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_GunBase
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_GunBase : MyObjectBuilder_DeviceBase
  {
    private SerializableDictionary<string, int> m_remainingAmmos;
    [ProtoMember(7)]
    [DefaultValue(0)]
    public int RemainingAmmo;
    [ProtoMember(8)]
    [DefaultValue(0)]
    public int RemainingMagazines;
    [ProtoMember(10)]
    [DefaultValue("")]
    public string CurrentAmmoMagazineName = "";
    [ProtoMember(13)]
    public List<MyObjectBuilder_GunBase.RemainingAmmoIns> RemainingAmmosList = new List<MyObjectBuilder_GunBase.RemainingAmmoIns>();
    [ProtoMember(16)]
    public long LastShootTime;

    [NoSerialize]
    public SerializableDictionary<string, int> RemainingAmmos
    {
      get => this.m_remainingAmmos;
      set
      {
        this.m_remainingAmmos = value;
        if (this.RemainingAmmosList == null)
          this.RemainingAmmosList = new List<MyObjectBuilder_GunBase.RemainingAmmoIns>();
        foreach (KeyValuePair<string, int> keyValuePair in value.Dictionary)
          this.RemainingAmmosList.Add(new MyObjectBuilder_GunBase.RemainingAmmoIns()
          {
            SubtypeName = keyValuePair.Key,
            Amount = keyValuePair.Value
          });
      }
    }

    public bool ShouldSerializeRemainingAmmos() => false;

    [ProtoContract]
    public class RemainingAmmoIns
    {
      [ProtoMember(1)]
      [XmlAttribute]
      [Nullable]
      public string SubtypeName;
      [ProtoMember(4)]
      [XmlAttribute]
      public int Amount;

      protected class VRage_Game_MyObjectBuilder_GunBase\u003C\u003ERemainingAmmoIns\u003C\u003ESubtypeName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GunBase.RemainingAmmoIns, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_GunBase.RemainingAmmoIns owner, in string value) => owner.SubtypeName = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_GunBase.RemainingAmmoIns owner,
          out string value)
        {
          value = owner.SubtypeName;
        }
      }

      protected class VRage_Game_MyObjectBuilder_GunBase\u003C\u003ERemainingAmmoIns\u003C\u003EAmount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GunBase.RemainingAmmoIns, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_GunBase.RemainingAmmoIns owner, in int value) => owner.Amount = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_GunBase.RemainingAmmoIns owner, out int value) => value = owner.Amount;
      }

      private class VRage_Game_MyObjectBuilder_GunBase\u003C\u003ERemainingAmmoIns\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_GunBase.RemainingAmmoIns>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_GunBase.RemainingAmmoIns();

        MyObjectBuilder_GunBase.RemainingAmmoIns IActivator<MyObjectBuilder_GunBase.RemainingAmmoIns>.CreateInstance() => new MyObjectBuilder_GunBase.RemainingAmmoIns();
      }
    }

    protected class VRage_Game_MyObjectBuilder_GunBase\u003C\u003Em_remainingAmmos\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GunBase, SerializableDictionary<string, int>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GunBase owner,
        in SerializableDictionary<string, int> value)
      {
        owner.m_remainingAmmos = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GunBase owner,
        out SerializableDictionary<string, int> value)
      {
        value = owner.m_remainingAmmos;
      }
    }

    protected class VRage_Game_MyObjectBuilder_GunBase\u003C\u003ERemainingAmmo\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GunBase, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GunBase owner, in int value) => owner.RemainingAmmo = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GunBase owner, out int value) => value = owner.RemainingAmmo;
    }

    protected class VRage_Game_MyObjectBuilder_GunBase\u003C\u003ERemainingMagazines\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GunBase, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GunBase owner, in int value) => owner.RemainingMagazines = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GunBase owner, out int value) => value = owner.RemainingMagazines;
    }

    protected class VRage_Game_MyObjectBuilder_GunBase\u003C\u003ECurrentAmmoMagazineName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GunBase, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GunBase owner, in string value) => owner.CurrentAmmoMagazineName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GunBase owner, out string value) => value = owner.CurrentAmmoMagazineName;
    }

    protected class VRage_Game_MyObjectBuilder_GunBase\u003C\u003ERemainingAmmosList\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GunBase, List<MyObjectBuilder_GunBase.RemainingAmmoIns>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GunBase owner,
        in List<MyObjectBuilder_GunBase.RemainingAmmoIns> value)
      {
        owner.RemainingAmmosList = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GunBase owner,
        out List<MyObjectBuilder_GunBase.RemainingAmmoIns> value)
      {
        value = owner.RemainingAmmosList;
      }
    }

    protected class VRage_Game_MyObjectBuilder_GunBase\u003C\u003ELastShootTime\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GunBase, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GunBase owner, in long value) => owner.LastShootTime = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GunBase owner, out long value) => value = owner.LastShootTime;
    }

    protected class VRage_Game_MyObjectBuilder_GunBase\u003C\u003EInventoryItemId\u003C\u003EAccessor : MyObjectBuilder_DeviceBase.VRage_Game_MyObjectBuilder_DeviceBase\u003C\u003EInventoryItemId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GunBase, uint?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GunBase owner, in uint? value) => this.Set((MyObjectBuilder_DeviceBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GunBase owner, out uint? value) => this.Get((MyObjectBuilder_DeviceBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GunBase\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GunBase, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GunBase owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GunBase owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GunBase\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GunBase, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GunBase owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GunBase owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GunBase\u003C\u003ERemainingAmmos\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GunBase, SerializableDictionary<string, int>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GunBase owner,
        in SerializableDictionary<string, int> value)
      {
        owner.RemainingAmmos = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GunBase owner,
        out SerializableDictionary<string, int> value)
      {
        value = owner.RemainingAmmos;
      }
    }

    protected class VRage_Game_MyObjectBuilder_GunBase\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GunBase, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GunBase owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GunBase owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GunBase\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GunBase, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GunBase owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GunBase owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_GunBase\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_GunBase>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_GunBase();

      MyObjectBuilder_GunBase IActivator<MyObjectBuilder_GunBase>.CreateInstance() => new MyObjectBuilder_GunBase();
    }
  }
}
