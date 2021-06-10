// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_FactionCollection
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, "Factions")]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_FactionCollection : MyObjectBuilder_Base
  {
    [ProtoMember(16)]
    public List<MyObjectBuilder_Faction> Factions;
    [ProtoMember(19)]
    public SerializableDictionary<long, long> Players;
    [ProtoMember(22)]
    public List<MyObjectBuilder_FactionRelation> Relations;
    [ProtoMember(25)]
    public List<MyObjectBuilder_PlayerFactionRelation> RelationsWithPlayers;
    [ProtoMember(28)]
    public List<MyObjectBuilder_FactionRequests> Requests;
    [ProtoMember(31)]
    public List<MyObjectBuilder_FactionsVisEntry> PlayerToFactionsVis;

    protected class VRage_Game_MyObjectBuilder_FactionCollection\u003C\u003EFactions\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionCollection, List<MyObjectBuilder_Faction>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FactionCollection owner,
        in List<MyObjectBuilder_Faction> value)
      {
        owner.Factions = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FactionCollection owner,
        out List<MyObjectBuilder_Faction> value)
      {
        value = owner.Factions;
      }
    }

    protected class VRage_Game_MyObjectBuilder_FactionCollection\u003C\u003EPlayers\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionCollection, SerializableDictionary<long, long>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FactionCollection owner,
        in SerializableDictionary<long, long> value)
      {
        owner.Players = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FactionCollection owner,
        out SerializableDictionary<long, long> value)
      {
        value = owner.Players;
      }
    }

    protected class VRage_Game_MyObjectBuilder_FactionCollection\u003C\u003ERelations\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionCollection, List<MyObjectBuilder_FactionRelation>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FactionCollection owner,
        in List<MyObjectBuilder_FactionRelation> value)
      {
        owner.Relations = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FactionCollection owner,
        out List<MyObjectBuilder_FactionRelation> value)
      {
        value = owner.Relations;
      }
    }

    protected class VRage_Game_MyObjectBuilder_FactionCollection\u003C\u003ERelationsWithPlayers\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionCollection, List<MyObjectBuilder_PlayerFactionRelation>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FactionCollection owner,
        in List<MyObjectBuilder_PlayerFactionRelation> value)
      {
        owner.RelationsWithPlayers = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FactionCollection owner,
        out List<MyObjectBuilder_PlayerFactionRelation> value)
      {
        value = owner.RelationsWithPlayers;
      }
    }

    protected class VRage_Game_MyObjectBuilder_FactionCollection\u003C\u003ERequests\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionCollection, List<MyObjectBuilder_FactionRequests>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FactionCollection owner,
        in List<MyObjectBuilder_FactionRequests> value)
      {
        owner.Requests = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FactionCollection owner,
        out List<MyObjectBuilder_FactionRequests> value)
      {
        value = owner.Requests;
      }
    }

    protected class VRage_Game_MyObjectBuilder_FactionCollection\u003C\u003EPlayerToFactionsVis\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionCollection, List<MyObjectBuilder_FactionsVisEntry>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FactionCollection owner,
        in List<MyObjectBuilder_FactionsVisEntry> value)
      {
        owner.PlayerToFactionsVis = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FactionCollection owner,
        out List<MyObjectBuilder_FactionsVisEntry> value)
      {
        value = owner.PlayerToFactionsVis;
      }
    }

    protected class VRage_Game_MyObjectBuilder_FactionCollection\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionCollection, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionCollection owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionCollection owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_FactionCollection\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionCollection, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionCollection owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionCollection owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_FactionCollection\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionCollection, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionCollection owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionCollection owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_FactionCollection\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionCollection, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionCollection owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionCollection owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_FactionCollection\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_FactionCollection>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_FactionCollection();

      MyObjectBuilder_FactionCollection IActivator<MyObjectBuilder_FactionCollection>.CreateInstance() => new MyObjectBuilder_FactionCollection();
    }
  }
}
