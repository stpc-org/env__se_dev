// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_ProxyAntenna
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_ProxyAntenna : MyObjectBuilder_EntityBase
  {
    [ProtoMember(1)]
    public bool HasReceiver;
    [ProtoMember(2)]
    public bool IsLaser;
    [ProtoMember(3)]
    public bool IsCharacter;
    [ProtoMember(4)]
    public SerializableVector3D Position;
    [ProtoMember(5)]
    public float BroadcastRadius;
    [ProtoMember(6)]
    public List<MyObjectBuilder_HudEntityParams> HudParams;
    [ProtoMember(7)]
    public long Owner;
    [ProtoMember(8)]
    public MyOwnershipShareModeEnum Share;
    [ProtoMember(9)]
    public long InfoEntityId;
    [ProtoMember(10)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public string InfoName;
    [ProtoMember(11)]
    public long AntennaEntityId;
    [ProtoMember(12)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public long? SuccessfullyContacting;
    [ProtoMember(13)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public string StateText;
    [ProtoMember(14)]
    public bool HasRemote;
    [ProtoMember(15)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public long? MainRemoteOwner;
    [ProtoMember(16)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public long? MainRemoteId;
    [ProtoMember(17)]
    public MyOwnershipShareModeEnum MainRemoteSharing;

    protected class VRage_Game_MyObjectBuilder_ProxyAntenna\u003C\u003EHasReceiver\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProxyAntenna, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProxyAntenna owner, in bool value) => owner.HasReceiver = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProxyAntenna owner, out bool value) => value = owner.HasReceiver;
    }

    protected class VRage_Game_MyObjectBuilder_ProxyAntenna\u003C\u003EIsLaser\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProxyAntenna, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProxyAntenna owner, in bool value) => owner.IsLaser = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProxyAntenna owner, out bool value) => value = owner.IsLaser;
    }

    protected class VRage_Game_MyObjectBuilder_ProxyAntenna\u003C\u003EIsCharacter\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProxyAntenna, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProxyAntenna owner, in bool value) => owner.IsCharacter = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProxyAntenna owner, out bool value) => value = owner.IsCharacter;
    }

    protected class VRage_Game_MyObjectBuilder_ProxyAntenna\u003C\u003EPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProxyAntenna, SerializableVector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProxyAntenna owner, in SerializableVector3D value) => owner.Position = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProxyAntenna owner,
        out SerializableVector3D value)
      {
        value = owner.Position;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ProxyAntenna\u003C\u003EBroadcastRadius\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProxyAntenna, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProxyAntenna owner, in float value) => owner.BroadcastRadius = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProxyAntenna owner, out float value) => value = owner.BroadcastRadius;
    }

    protected class VRage_Game_MyObjectBuilder_ProxyAntenna\u003C\u003EHudParams\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProxyAntenna, List<MyObjectBuilder_HudEntityParams>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProxyAntenna owner,
        in List<MyObjectBuilder_HudEntityParams> value)
      {
        owner.HudParams = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProxyAntenna owner,
        out List<MyObjectBuilder_HudEntityParams> value)
      {
        value = owner.HudParams;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ProxyAntenna\u003C\u003EOwner\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProxyAntenna, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProxyAntenna owner, in long value) => owner.Owner = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProxyAntenna owner, out long value) => value = owner.Owner;
    }

    protected class VRage_Game_MyObjectBuilder_ProxyAntenna\u003C\u003EShare\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProxyAntenna, MyOwnershipShareModeEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProxyAntenna owner,
        in MyOwnershipShareModeEnum value)
      {
        owner.Share = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProxyAntenna owner,
        out MyOwnershipShareModeEnum value)
      {
        value = owner.Share;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ProxyAntenna\u003C\u003EInfoEntityId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProxyAntenna, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProxyAntenna owner, in long value) => owner.InfoEntityId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProxyAntenna owner, out long value) => value = owner.InfoEntityId;
    }

    protected class VRage_Game_MyObjectBuilder_ProxyAntenna\u003C\u003EInfoName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProxyAntenna, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProxyAntenna owner, in string value) => owner.InfoName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProxyAntenna owner, out string value) => value = owner.InfoName;
    }

    protected class VRage_Game_MyObjectBuilder_ProxyAntenna\u003C\u003EAntennaEntityId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProxyAntenna, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProxyAntenna owner, in long value) => owner.AntennaEntityId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProxyAntenna owner, out long value) => value = owner.AntennaEntityId;
    }

    protected class VRage_Game_MyObjectBuilder_ProxyAntenna\u003C\u003ESuccessfullyContacting\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProxyAntenna, long?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProxyAntenna owner, in long? value) => owner.SuccessfullyContacting = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProxyAntenna owner, out long? value) => value = owner.SuccessfullyContacting;
    }

    protected class VRage_Game_MyObjectBuilder_ProxyAntenna\u003C\u003EStateText\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProxyAntenna, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProxyAntenna owner, in string value) => owner.StateText = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProxyAntenna owner, out string value) => value = owner.StateText;
    }

    protected class VRage_Game_MyObjectBuilder_ProxyAntenna\u003C\u003EHasRemote\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProxyAntenna, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProxyAntenna owner, in bool value) => owner.HasRemote = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProxyAntenna owner, out bool value) => value = owner.HasRemote;
    }

    protected class VRage_Game_MyObjectBuilder_ProxyAntenna\u003C\u003EMainRemoteOwner\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProxyAntenna, long?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProxyAntenna owner, in long? value) => owner.MainRemoteOwner = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProxyAntenna owner, out long? value) => value = owner.MainRemoteOwner;
    }

    protected class VRage_Game_MyObjectBuilder_ProxyAntenna\u003C\u003EMainRemoteId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProxyAntenna, long?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProxyAntenna owner, in long? value) => owner.MainRemoteId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProxyAntenna owner, out long? value) => value = owner.MainRemoteId;
    }

    protected class VRage_Game_MyObjectBuilder_ProxyAntenna\u003C\u003EMainRemoteSharing\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProxyAntenna, MyOwnershipShareModeEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProxyAntenna owner,
        in MyOwnershipShareModeEnum value)
      {
        owner.MainRemoteSharing = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProxyAntenna owner,
        out MyOwnershipShareModeEnum value)
      {
        value = owner.MainRemoteSharing;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ProxyAntenna\u003C\u003EEntityId\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EEntityId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProxyAntenna, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProxyAntenna owner, in long value) => this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProxyAntenna owner, out long value) => this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProxyAntenna\u003C\u003EPersistentFlags\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EPersistentFlags\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProxyAntenna, MyPersistentEntityFlags2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProxyAntenna owner,
        in MyPersistentEntityFlags2 value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProxyAntenna owner,
        out MyPersistentEntityFlags2 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ProxyAntenna\u003C\u003EName\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProxyAntenna, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProxyAntenna owner, in string value) => this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProxyAntenna owner, out string value) => this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProxyAntenna\u003C\u003EPositionAndOrientation\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EPositionAndOrientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProxyAntenna, MyPositionAndOrientation?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProxyAntenna owner,
        in MyPositionAndOrientation? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProxyAntenna owner,
        out MyPositionAndOrientation? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ProxyAntenna\u003C\u003ELocalPositionAndOrientation\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003ELocalPositionAndOrientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProxyAntenna, MyPositionAndOrientation?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProxyAntenna owner,
        in MyPositionAndOrientation? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProxyAntenna owner,
        out MyPositionAndOrientation? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ProxyAntenna\u003C\u003EComponentContainer\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EComponentContainer\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProxyAntenna, MyObjectBuilder_ComponentContainer>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProxyAntenna owner,
        in MyObjectBuilder_ComponentContainer value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProxyAntenna owner,
        out MyObjectBuilder_ComponentContainer value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ProxyAntenna\u003C\u003EEntityDefinitionId\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EEntityDefinitionId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProxyAntenna, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProxyAntenna owner,
        in SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProxyAntenna owner,
        out SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ProxyAntenna\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProxyAntenna, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProxyAntenna owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProxyAntenna owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProxyAntenna\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProxyAntenna, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProxyAntenna owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProxyAntenna owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProxyAntenna\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProxyAntenna, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProxyAntenna owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProxyAntenna owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProxyAntenna\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProxyAntenna, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProxyAntenna owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProxyAntenna owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_ProxyAntenna\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ProxyAntenna>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ProxyAntenna();

      MyObjectBuilder_ProxyAntenna IActivator<MyObjectBuilder_ProxyAntenna>.CreateInstance() => new MyObjectBuilder_ProxyAntenna();
    }
  }
}
