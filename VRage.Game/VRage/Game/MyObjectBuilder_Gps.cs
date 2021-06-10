// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_Gps
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
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_Gps : MyObjectBuilder_Base
  {
    [ProtoMember(31)]
    public List<MyObjectBuilder_Gps.Entry> Entries;

    [ProtoContract]
    public struct Entry
    {
      [ProtoMember(1)]
      public string name;
      [ProtoMember(4)]
      public string description;
      [ProtoMember(7)]
      public Vector3D coords;
      [ProtoMember(10)]
      public bool isFinal;
      [ProtoMember(13)]
      public bool showOnHud;
      [ProtoMember(16)]
      public bool alwaysVisible;
      [ProtoMember(19)]
      public Color color;
      [ProtoMember(22, IsRequired = false)]
      public long entityId;
      [ProtoMember(28)]
      public bool isObjective;
      [ProtoMember(31, IsRequired = false)]
      public long contractId;

      [ProtoMember(25, IsRequired = false)]
      public string DisplayName { get; set; }

      protected class VRage_Game_MyObjectBuilder_Gps\u003C\u003EEntry\u003C\u003Ename\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Gps.Entry, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_Gps.Entry owner, in string value) => owner.name = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_Gps.Entry owner, out string value) => value = owner.name;
      }

      protected class VRage_Game_MyObjectBuilder_Gps\u003C\u003EEntry\u003C\u003Edescription\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Gps.Entry, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_Gps.Entry owner, in string value) => owner.description = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_Gps.Entry owner, out string value) => value = owner.description;
      }

      protected class VRage_Game_MyObjectBuilder_Gps\u003C\u003EEntry\u003C\u003Ecoords\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Gps.Entry, Vector3D>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_Gps.Entry owner, in Vector3D value) => owner.coords = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_Gps.Entry owner, out Vector3D value) => value = owner.coords;
      }

      protected class VRage_Game_MyObjectBuilder_Gps\u003C\u003EEntry\u003C\u003EisFinal\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Gps.Entry, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_Gps.Entry owner, in bool value) => owner.isFinal = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_Gps.Entry owner, out bool value) => value = owner.isFinal;
      }

      protected class VRage_Game_MyObjectBuilder_Gps\u003C\u003EEntry\u003C\u003EshowOnHud\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Gps.Entry, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_Gps.Entry owner, in bool value) => owner.showOnHud = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_Gps.Entry owner, out bool value) => value = owner.showOnHud;
      }

      protected class VRage_Game_MyObjectBuilder_Gps\u003C\u003EEntry\u003C\u003EalwaysVisible\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Gps.Entry, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_Gps.Entry owner, in bool value) => owner.alwaysVisible = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_Gps.Entry owner, out bool value) => value = owner.alwaysVisible;
      }

      protected class VRage_Game_MyObjectBuilder_Gps\u003C\u003EEntry\u003C\u003Ecolor\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Gps.Entry, Color>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_Gps.Entry owner, in Color value) => owner.color = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_Gps.Entry owner, out Color value) => value = owner.color;
      }

      protected class VRage_Game_MyObjectBuilder_Gps\u003C\u003EEntry\u003C\u003EentityId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Gps.Entry, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_Gps.Entry owner, in long value) => owner.entityId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_Gps.Entry owner, out long value) => value = owner.entityId;
      }

      protected class VRage_Game_MyObjectBuilder_Gps\u003C\u003EEntry\u003C\u003EisObjective\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Gps.Entry, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_Gps.Entry owner, in bool value) => owner.isObjective = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_Gps.Entry owner, out bool value) => value = owner.isObjective;
      }

      protected class VRage_Game_MyObjectBuilder_Gps\u003C\u003EEntry\u003C\u003EcontractId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Gps.Entry, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_Gps.Entry owner, in long value) => owner.contractId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_Gps.Entry owner, out long value) => value = owner.contractId;
      }

      protected class VRage_Game_MyObjectBuilder_Gps\u003C\u003EEntry\u003C\u003EDisplayName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Gps.Entry, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_Gps.Entry owner, in string value) => owner.DisplayName = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_Gps.Entry owner, out string value) => value = owner.DisplayName;
      }

      private class VRage_Game_MyObjectBuilder_Gps\u003C\u003EEntry\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Gps.Entry>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_Gps.Entry();

        MyObjectBuilder_Gps.Entry IActivator<MyObjectBuilder_Gps.Entry>.CreateInstance() => new MyObjectBuilder_Gps.Entry();
      }
    }

    protected class VRage_Game_MyObjectBuilder_Gps\u003C\u003EEntries\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Gps, List<MyObjectBuilder_Gps.Entry>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Gps owner,
        in List<MyObjectBuilder_Gps.Entry> value)
      {
        owner.Entries = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Gps owner,
        out List<MyObjectBuilder_Gps.Entry> value)
      {
        value = owner.Entries;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Gps\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Gps, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Gps owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Gps owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Gps\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Gps, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Gps owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Gps owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Gps\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Gps, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Gps owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Gps owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Gps\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Gps, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Gps owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Gps owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_Gps\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Gps>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_Gps();

      MyObjectBuilder_Gps IActivator<MyObjectBuilder_Gps>.CreateInstance() => new MyObjectBuilder_Gps();
    }
  }
}
