// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_Player
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ModAPI;
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
  public class MyObjectBuilder_Player : MyObjectBuilder_Base
  {
    [ProtoMember(10)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public string DisplayName;
    [ProtoMember(13)]
    public long IdentityId;
    [ProtoMember(16)]
    public bool Connected;
    [ProtoMember(19)]
    public bool ForceRealPlayer;
    [ProtoMember(22)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public MyObjectBuilder_Toolbar Toolbar;
    [ProtoMember(25)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public List<CameraControllerSettings> EntityCameraData;
    [ProtoMember(28)]
    [DefaultValue(null)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public List<Vector3> BuildColorSlots;
    [ProtoMember(30)]
    public bool CreativeToolsEnabled;
    [ProtoMember(33)]
    public int RemoteAdminSettings;
    [ProtoMember(38)]
    public MyPromoteLevel PromoteLevel;
    [NoSerialize]
    public ulong SteamID;
    [NoSerialize]
    private SerializableDictionary<long, CameraControllerSettings> m_cameraData;
    [NoSerialize]
    public long PlayerEntity;
    [NoSerialize]
    public string PlayerModel;
    [NoSerialize]
    public long PlayerId;
    [NoSerialize]
    public long LastActivity;
    [ProtoMember(31)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public string BuildArmorSkin;
    [ProtoMember(35)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public int BuildColorSlot;
    [ProtoMember(40)]
    public bool IsWildlifeAgent;

    public bool ShouldSerializeBuildColorSlots() => this.BuildColorSlots != null;

    public bool ShouldSerializeSteamID() => false;

    [NoSerialize]
    public SerializableDictionary<long, CameraControllerSettings> CameraData
    {
      get => this.m_cameraData;
      set => this.m_cameraData = value;
    }

    public bool ShouldSerializeCameraData() => false;

    public bool ShouldSerializePlayerEntity() => false;

    public bool ShouldSerializePlayerModel() => false;

    public bool ShouldSerializePlayerId() => false;

    public bool ShouldSerializeLastActivity() => false;

    protected class VRage_Game_MyObjectBuilder_Player\u003C\u003EDisplayName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Player, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Player owner, in string value) => owner.DisplayName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Player owner, out string value) => value = owner.DisplayName;
    }

    protected class VRage_Game_MyObjectBuilder_Player\u003C\u003EIdentityId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Player, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Player owner, in long value) => owner.IdentityId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Player owner, out long value) => value = owner.IdentityId;
    }

    protected class VRage_Game_MyObjectBuilder_Player\u003C\u003EConnected\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Player, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Player owner, in bool value) => owner.Connected = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Player owner, out bool value) => value = owner.Connected;
    }

    protected class VRage_Game_MyObjectBuilder_Player\u003C\u003EForceRealPlayer\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Player, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Player owner, in bool value) => owner.ForceRealPlayer = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Player owner, out bool value) => value = owner.ForceRealPlayer;
    }

    protected class VRage_Game_MyObjectBuilder_Player\u003C\u003EToolbar\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Player, MyObjectBuilder_Toolbar>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Player owner, in MyObjectBuilder_Toolbar value) => owner.Toolbar = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Player owner, out MyObjectBuilder_Toolbar value) => value = owner.Toolbar;
    }

    protected class VRage_Game_MyObjectBuilder_Player\u003C\u003EEntityCameraData\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Player, List<CameraControllerSettings>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Player owner,
        in List<CameraControllerSettings> value)
      {
        owner.EntityCameraData = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Player owner,
        out List<CameraControllerSettings> value)
      {
        value = owner.EntityCameraData;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Player\u003C\u003EBuildColorSlots\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Player, List<Vector3>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Player owner, in List<Vector3> value) => owner.BuildColorSlots = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Player owner, out List<Vector3> value) => value = owner.BuildColorSlots;
    }

    protected class VRage_Game_MyObjectBuilder_Player\u003C\u003ECreativeToolsEnabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Player, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Player owner, in bool value) => owner.CreativeToolsEnabled = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Player owner, out bool value) => value = owner.CreativeToolsEnabled;
    }

    protected class VRage_Game_MyObjectBuilder_Player\u003C\u003ERemoteAdminSettings\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Player, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Player owner, in int value) => owner.RemoteAdminSettings = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Player owner, out int value) => value = owner.RemoteAdminSettings;
    }

    protected class VRage_Game_MyObjectBuilder_Player\u003C\u003EPromoteLevel\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Player, MyPromoteLevel>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Player owner, in MyPromoteLevel value) => owner.PromoteLevel = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Player owner, out MyPromoteLevel value) => value = owner.PromoteLevel;
    }

    protected class VRage_Game_MyObjectBuilder_Player\u003C\u003ESteamID\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Player, ulong>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Player owner, in ulong value) => owner.SteamID = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Player owner, out ulong value) => value = owner.SteamID;
    }

    protected class VRage_Game_MyObjectBuilder_Player\u003C\u003Em_cameraData\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Player, SerializableDictionary<long, CameraControllerSettings>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Player owner,
        in SerializableDictionary<long, CameraControllerSettings> value)
      {
        owner.m_cameraData = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Player owner,
        out SerializableDictionary<long, CameraControllerSettings> value)
      {
        value = owner.m_cameraData;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Player\u003C\u003EPlayerEntity\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Player, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Player owner, in long value) => owner.PlayerEntity = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Player owner, out long value) => value = owner.PlayerEntity;
    }

    protected class VRage_Game_MyObjectBuilder_Player\u003C\u003EPlayerModel\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Player, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Player owner, in string value) => owner.PlayerModel = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Player owner, out string value) => value = owner.PlayerModel;
    }

    protected class VRage_Game_MyObjectBuilder_Player\u003C\u003EPlayerId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Player, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Player owner, in long value) => owner.PlayerId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Player owner, out long value) => value = owner.PlayerId;
    }

    protected class VRage_Game_MyObjectBuilder_Player\u003C\u003ELastActivity\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Player, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Player owner, in long value) => owner.LastActivity = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Player owner, out long value) => value = owner.LastActivity;
    }

    protected class VRage_Game_MyObjectBuilder_Player\u003C\u003EBuildArmorSkin\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Player, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Player owner, in string value) => owner.BuildArmorSkin = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Player owner, out string value) => value = owner.BuildArmorSkin;
    }

    protected class VRage_Game_MyObjectBuilder_Player\u003C\u003EBuildColorSlot\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Player, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Player owner, in int value) => owner.BuildColorSlot = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Player owner, out int value) => value = owner.BuildColorSlot;
    }

    protected class VRage_Game_MyObjectBuilder_Player\u003C\u003EIsWildlifeAgent\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Player, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Player owner, in bool value) => owner.IsWildlifeAgent = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Player owner, out bool value) => value = owner.IsWildlifeAgent;
    }

    protected class VRage_Game_MyObjectBuilder_Player\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Player, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Player owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Player owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Player\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Player, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Player owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Player owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Player\u003C\u003ECameraData\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Player, SerializableDictionary<long, CameraControllerSettings>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Player owner,
        in SerializableDictionary<long, CameraControllerSettings> value)
      {
        owner.CameraData = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Player owner,
        out SerializableDictionary<long, CameraControllerSettings> value)
      {
        value = owner.CameraData;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Player\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Player, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Player owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Player owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Player\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Player, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Player owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Player owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_Player\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Player>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_Player();

      MyObjectBuilder_Player IActivator<MyObjectBuilder_Player>.CreateInstance() => new MyObjectBuilder_Player();
    }
  }
}
