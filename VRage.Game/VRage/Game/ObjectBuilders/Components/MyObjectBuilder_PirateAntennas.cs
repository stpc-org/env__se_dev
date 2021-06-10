// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Components.MyObjectBuilder_PirateAntennas
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Components
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_PirateAntennas : MyObjectBuilder_SessionComponent
  {
    [ProtoMember(10)]
    public long PiratesIdentity;
    [ProtoMember(13)]
    public MyObjectBuilder_PirateAntennas.MyPirateDrone[] Drones;

    [ProtoContract]
    public class MyPirateDrone
    {
      [ProtoMember(1)]
      [XmlAttribute("EntityId")]
      public long EntityId;
      [ProtoMember(4)]
      [XmlAttribute("AntennaEntityId")]
      public long AntennaEntityId;
      [ProtoMember(7)]
      [XmlAttribute("DespawnTimer")]
      public int DespawnTimer;

      protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_PirateAntennas\u003C\u003EMyPirateDrone\u003C\u003EEntityId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PirateAntennas.MyPirateDrone, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_PirateAntennas.MyPirateDrone owner,
          in long value)
        {
          owner.EntityId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_PirateAntennas.MyPirateDrone owner,
          out long value)
        {
          value = owner.EntityId;
        }
      }

      protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_PirateAntennas\u003C\u003EMyPirateDrone\u003C\u003EAntennaEntityId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PirateAntennas.MyPirateDrone, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_PirateAntennas.MyPirateDrone owner,
          in long value)
        {
          owner.AntennaEntityId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_PirateAntennas.MyPirateDrone owner,
          out long value)
        {
          value = owner.AntennaEntityId;
        }
      }

      protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_PirateAntennas\u003C\u003EMyPirateDrone\u003C\u003EDespawnTimer\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PirateAntennas.MyPirateDrone, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_PirateAntennas.MyPirateDrone owner,
          in int value)
        {
          owner.DespawnTimer = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_PirateAntennas.MyPirateDrone owner,
          out int value)
        {
          value = owner.DespawnTimer;
        }
      }

      private class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_PirateAntennas\u003C\u003EMyPirateDrone\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_PirateAntennas.MyPirateDrone>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_PirateAntennas.MyPirateDrone();

        MyObjectBuilder_PirateAntennas.MyPirateDrone IActivator<MyObjectBuilder_PirateAntennas.MyPirateDrone>.CreateInstance() => new MyObjectBuilder_PirateAntennas.MyPirateDrone();
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_PirateAntennas\u003C\u003EPiratesIdentity\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PirateAntennas, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PirateAntennas owner, in long value) => owner.PiratesIdentity = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PirateAntennas owner, out long value) => value = owner.PiratesIdentity;
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_PirateAntennas\u003C\u003EDrones\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PirateAntennas, MyObjectBuilder_PirateAntennas.MyPirateDrone[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PirateAntennas owner,
        in MyObjectBuilder_PirateAntennas.MyPirateDrone[] value)
      {
        owner.Drones = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PirateAntennas owner,
        out MyObjectBuilder_PirateAntennas.MyPirateDrone[] value)
      {
        value = owner.Drones;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_PirateAntennas\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PirateAntennas, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PirateAntennas owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PirateAntennas owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_PirateAntennas\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PirateAntennas, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PirateAntennas owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PirateAntennas owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_PirateAntennas\u003C\u003EDefinition\u003C\u003EAccessor : MyObjectBuilder_SessionComponent.VRage_Game_MyObjectBuilder_SessionComponent\u003C\u003EDefinition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PirateAntennas, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PirateAntennas owner,
        in SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_SessionComponent&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PirateAntennas owner,
        out SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_SessionComponent&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_PirateAntennas\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PirateAntennas, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PirateAntennas owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PirateAntennas owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_PirateAntennas\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PirateAntennas, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PirateAntennas owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PirateAntennas owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_PirateAntennas\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_PirateAntennas>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_PirateAntennas();

      MyObjectBuilder_PirateAntennas IActivator<MyObjectBuilder_PirateAntennas>.CreateInstance() => new MyObjectBuilder_PirateAntennas();
    }
  }
}
