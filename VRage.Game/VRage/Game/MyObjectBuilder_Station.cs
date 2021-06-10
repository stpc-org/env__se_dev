// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_Station
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.Serialization;

namespace VRage.Game
{
  [ProtoContract]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_Station
  {
    [ProtoMember(1)]
    public long Id;
    [ProtoMember(2)]
    public SerializableVector3D Position;
    [ProtoMember(3)]
    public SerializableVector3 Up;
    [ProtoMember(5)]
    public SerializableVector3 Forward;
    [ProtoMember(7)]
    public MyStationTypeEnum StationType;
    [ProtoMember(9)]
    public bool IsDeepSpaceStation;
    [ProtoMember(11)]
    public long StationEntityId;
    [ProtoMember(13)]
    public long FactionId;
    [ProtoMember(15)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public string PrefabName;
    [ProtoMember(19)]
    public long SafeZoneEntityId;
    [ProtoMember(21)]
    public List<MyObjectBuilder_StoreItem> StoreItems;
    [ProtoMember(23)]
    public bool IsOnPlanetWithAtmosphere;

    protected class VRage_Game_MyObjectBuilder_Station\u003C\u003EId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Station, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Station owner, in long value) => owner.Id = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Station owner, out long value) => value = owner.Id;
    }

    protected class VRage_Game_MyObjectBuilder_Station\u003C\u003EPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Station, SerializableVector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Station owner, in SerializableVector3D value) => owner.Position = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Station owner, out SerializableVector3D value) => value = owner.Position;
    }

    protected class VRage_Game_MyObjectBuilder_Station\u003C\u003EUp\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Station, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Station owner, in SerializableVector3 value) => owner.Up = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Station owner, out SerializableVector3 value) => value = owner.Up;
    }

    protected class VRage_Game_MyObjectBuilder_Station\u003C\u003EForward\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Station, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Station owner, in SerializableVector3 value) => owner.Forward = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Station owner, out SerializableVector3 value) => value = owner.Forward;
    }

    protected class VRage_Game_MyObjectBuilder_Station\u003C\u003EStationType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Station, MyStationTypeEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Station owner, in MyStationTypeEnum value) => owner.StationType = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Station owner, out MyStationTypeEnum value) => value = owner.StationType;
    }

    protected class VRage_Game_MyObjectBuilder_Station\u003C\u003EIsDeepSpaceStation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Station, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Station owner, in bool value) => owner.IsDeepSpaceStation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Station owner, out bool value) => value = owner.IsDeepSpaceStation;
    }

    protected class VRage_Game_MyObjectBuilder_Station\u003C\u003EStationEntityId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Station, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Station owner, in long value) => owner.StationEntityId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Station owner, out long value) => value = owner.StationEntityId;
    }

    protected class VRage_Game_MyObjectBuilder_Station\u003C\u003EFactionId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Station, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Station owner, in long value) => owner.FactionId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Station owner, out long value) => value = owner.FactionId;
    }

    protected class VRage_Game_MyObjectBuilder_Station\u003C\u003EPrefabName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Station, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Station owner, in string value) => owner.PrefabName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Station owner, out string value) => value = owner.PrefabName;
    }

    protected class VRage_Game_MyObjectBuilder_Station\u003C\u003ESafeZoneEntityId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Station, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Station owner, in long value) => owner.SafeZoneEntityId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Station owner, out long value) => value = owner.SafeZoneEntityId;
    }

    protected class VRage_Game_MyObjectBuilder_Station\u003C\u003EStoreItems\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Station, List<MyObjectBuilder_StoreItem>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Station owner,
        in List<MyObjectBuilder_StoreItem> value)
      {
        owner.StoreItems = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Station owner,
        out List<MyObjectBuilder_StoreItem> value)
      {
        value = owner.StoreItems;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Station\u003C\u003EIsOnPlanetWithAtmosphere\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Station, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Station owner, in bool value) => owner.IsOnPlanetWithAtmosphere = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Station owner, out bool value) => value = owner.IsOnPlanetWithAtmosphere;
    }

    private class VRage_Game_MyObjectBuilder_Station\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Station>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_Station();

      MyObjectBuilder_Station IActivator<MyObjectBuilder_Station>.CreateInstance() => new MyObjectBuilder_Station();
    }
  }
}
