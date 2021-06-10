// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.MyObjectBuilder_NeutralShipSpawner
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_NeutralShipSpawner : MyObjectBuilder_SessionComponent
  {
    [ProtoMember(1)]
    public List<MyObjectBuilder_NeutralShipSpawner.ShipTimePair> ShipsInProgress;

    [ProtoContract]
    public struct ShipTimePair
    {
      [ProtoMember(1)]
      public List<long> EntityIds;
      public long TimeTicks;

      protected class VRage_Game_ObjectBuilders_MyObjectBuilder_NeutralShipSpawner\u003C\u003EShipTimePair\u003C\u003EEntityIds\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_NeutralShipSpawner.ShipTimePair, List<long>>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_NeutralShipSpawner.ShipTimePair owner,
          in List<long> value)
        {
          owner.EntityIds = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_NeutralShipSpawner.ShipTimePair owner,
          out List<long> value)
        {
          value = owner.EntityIds;
        }
      }

      protected class VRage_Game_ObjectBuilders_MyObjectBuilder_NeutralShipSpawner\u003C\u003EShipTimePair\u003C\u003ETimeTicks\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_NeutralShipSpawner.ShipTimePair, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_NeutralShipSpawner.ShipTimePair owner,
          in long value)
        {
          owner.TimeTicks = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_NeutralShipSpawner.ShipTimePair owner,
          out long value)
        {
          value = owner.TimeTicks;
        }
      }

      private class VRage_Game_ObjectBuilders_MyObjectBuilder_NeutralShipSpawner\u003C\u003EShipTimePair\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_NeutralShipSpawner.ShipTimePair>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_NeutralShipSpawner.ShipTimePair();

        MyObjectBuilder_NeutralShipSpawner.ShipTimePair IActivator<MyObjectBuilder_NeutralShipSpawner.ShipTimePair>.CreateInstance() => new MyObjectBuilder_NeutralShipSpawner.ShipTimePair();
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_NeutralShipSpawner\u003C\u003EShipsInProgress\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_NeutralShipSpawner, List<MyObjectBuilder_NeutralShipSpawner.ShipTimePair>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_NeutralShipSpawner owner,
        in List<MyObjectBuilder_NeutralShipSpawner.ShipTimePair> value)
      {
        owner.ShipsInProgress = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_NeutralShipSpawner owner,
        out List<MyObjectBuilder_NeutralShipSpawner.ShipTimePair> value)
      {
        value = owner.ShipsInProgress;
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_NeutralShipSpawner\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_NeutralShipSpawner, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_NeutralShipSpawner owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_NeutralShipSpawner owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_NeutralShipSpawner\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_NeutralShipSpawner, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_NeutralShipSpawner owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_NeutralShipSpawner owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_NeutralShipSpawner\u003C\u003EDefinition\u003C\u003EAccessor : MyObjectBuilder_SessionComponent.VRage_Game_MyObjectBuilder_SessionComponent\u003C\u003EDefinition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_NeutralShipSpawner, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_NeutralShipSpawner owner,
        in SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_SessionComponent&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_NeutralShipSpawner owner,
        out SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_SessionComponent&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_NeutralShipSpawner\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_NeutralShipSpawner, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_NeutralShipSpawner owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_NeutralShipSpawner owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_NeutralShipSpawner\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_NeutralShipSpawner, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_NeutralShipSpawner owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_NeutralShipSpawner owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_MyObjectBuilder_NeutralShipSpawner\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_NeutralShipSpawner>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_NeutralShipSpawner();

      MyObjectBuilder_NeutralShipSpawner IActivator<MyObjectBuilder_NeutralShipSpawner>.CreateInstance() => new MyObjectBuilder_NeutralShipSpawner();
    }
  }
}
