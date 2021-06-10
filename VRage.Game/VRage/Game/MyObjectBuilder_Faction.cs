// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_Faction
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.Serialization;

namespace VRage.Game
{
  [ProtoContract]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_Faction
  {
    [ProtoMember(10)]
    public long FactionId;
    [ProtoMember(13)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public string Tag;
    [ProtoMember(16)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public string Name;
    [ProtoMember(19)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public string Description;
    [ProtoMember(22)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public string PrivateInfo;
    [ProtoMember(25)]
    public List<MyObjectBuilder_FactionMember> Members;
    [ProtoMember(28)]
    public List<MyObjectBuilder_FactionMember> JoinRequests;
    [ProtoMember(31)]
    public bool AutoAcceptMember;
    [ProtoMember(34)]
    public bool AutoAcceptPeace;
    [ProtoMember(37)]
    public bool AcceptHumans = true;
    [ProtoMember(40)]
    public bool EnableFriendlyFire = true;
    [ProtoMember(43)]
    public MyFactionTypes FactionType;
    [ProtoMember(46)]
    public List<MyObjectBuilder_Station> Stations;
    [ProtoMember(49)]
    public SerializableVector3 CustomColor;
    [ProtoMember(51)]
    public SerializableVector3 IconColor;
    [ProtoMember(52)]
    public string FactionIcon;
    [ProtoMember(53)]
    public int TransferedPCUDelta;
    [ProtoMember(56)]
    public int Score;
    [ProtoMember(59)]
    public float ObjectivePercentageCompleted;

    protected class VRage_Game_MyObjectBuilder_Faction\u003C\u003EFactionId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Faction owner, in long value) => owner.FactionId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Faction owner, out long value) => value = owner.FactionId;
    }

    protected class VRage_Game_MyObjectBuilder_Faction\u003C\u003ETag\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Faction owner, in string value) => owner.Tag = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Faction owner, out string value) => value = owner.Tag;
    }

    protected class VRage_Game_MyObjectBuilder_Faction\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Faction owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Faction owner, out string value) => value = owner.Name;
    }

    protected class VRage_Game_MyObjectBuilder_Faction\u003C\u003EDescription\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Faction owner, in string value) => owner.Description = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Faction owner, out string value) => value = owner.Description;
    }

    protected class VRage_Game_MyObjectBuilder_Faction\u003C\u003EPrivateInfo\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Faction owner, in string value) => owner.PrivateInfo = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Faction owner, out string value) => value = owner.PrivateInfo;
    }

    protected class VRage_Game_MyObjectBuilder_Faction\u003C\u003EMembers\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, List<MyObjectBuilder_FactionMember>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Faction owner,
        in List<MyObjectBuilder_FactionMember> value)
      {
        owner.Members = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Faction owner,
        out List<MyObjectBuilder_FactionMember> value)
      {
        value = owner.Members;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Faction\u003C\u003EJoinRequests\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, List<MyObjectBuilder_FactionMember>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Faction owner,
        in List<MyObjectBuilder_FactionMember> value)
      {
        owner.JoinRequests = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Faction owner,
        out List<MyObjectBuilder_FactionMember> value)
      {
        value = owner.JoinRequests;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Faction\u003C\u003EAutoAcceptMember\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Faction owner, in bool value) => owner.AutoAcceptMember = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Faction owner, out bool value) => value = owner.AutoAcceptMember;
    }

    protected class VRage_Game_MyObjectBuilder_Faction\u003C\u003EAutoAcceptPeace\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Faction owner, in bool value) => owner.AutoAcceptPeace = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Faction owner, out bool value) => value = owner.AutoAcceptPeace;
    }

    protected class VRage_Game_MyObjectBuilder_Faction\u003C\u003EAcceptHumans\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Faction owner, in bool value) => owner.AcceptHumans = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Faction owner, out bool value) => value = owner.AcceptHumans;
    }

    protected class VRage_Game_MyObjectBuilder_Faction\u003C\u003EEnableFriendlyFire\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Faction owner, in bool value) => owner.EnableFriendlyFire = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Faction owner, out bool value) => value = owner.EnableFriendlyFire;
    }

    protected class VRage_Game_MyObjectBuilder_Faction\u003C\u003EFactionType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, MyFactionTypes>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Faction owner, in MyFactionTypes value) => owner.FactionType = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Faction owner, out MyFactionTypes value) => value = owner.FactionType;
    }

    protected class VRage_Game_MyObjectBuilder_Faction\u003C\u003EStations\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, List<MyObjectBuilder_Station>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Faction owner,
        in List<MyObjectBuilder_Station> value)
      {
        owner.Stations = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Faction owner,
        out List<MyObjectBuilder_Station> value)
      {
        value = owner.Stations;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Faction\u003C\u003ECustomColor\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Faction owner, in SerializableVector3 value) => owner.CustomColor = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Faction owner, out SerializableVector3 value) => value = owner.CustomColor;
    }

    protected class VRage_Game_MyObjectBuilder_Faction\u003C\u003EIconColor\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Faction owner, in SerializableVector3 value) => owner.IconColor = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Faction owner, out SerializableVector3 value) => value = owner.IconColor;
    }

    protected class VRage_Game_MyObjectBuilder_Faction\u003C\u003EFactionIcon\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Faction owner, in string value) => owner.FactionIcon = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Faction owner, out string value) => value = owner.FactionIcon;
    }

    protected class VRage_Game_MyObjectBuilder_Faction\u003C\u003ETransferedPCUDelta\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Faction owner, in int value) => owner.TransferedPCUDelta = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Faction owner, out int value) => value = owner.TransferedPCUDelta;
    }

    protected class VRage_Game_MyObjectBuilder_Faction\u003C\u003EScore\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Faction owner, in int value) => owner.Score = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Faction owner, out int value) => value = owner.Score;
    }

    protected class VRage_Game_MyObjectBuilder_Faction\u003C\u003EObjectivePercentageCompleted\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Faction, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Faction owner, in float value) => owner.ObjectivePercentageCompleted = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Faction owner, out float value) => value = owner.ObjectivePercentageCompleted;
    }

    private class VRage_Game_MyObjectBuilder_Faction\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Faction>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_Faction();

      MyObjectBuilder_Faction IActivator<MyObjectBuilder_Faction>.CreateInstance() => new MyObjectBuilder_Faction();
    }
  }
}
