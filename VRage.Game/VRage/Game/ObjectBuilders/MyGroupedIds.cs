// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.MyGroupedIds
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders
{
  [ProtoContract]
  public class MyGroupedIds
  {
    [ProtoMember(7)]
    [XmlAttribute]
    public string Tag;
    [ProtoMember(10)]
    [DefaultValue(null)]
    [XmlArrayItem("GroupEntry")]
    public MyGroupedIds.GroupedId[] Entries;

    [ProtoContract]
    public struct GroupedId
    {
      [ProtoMember(1)]
      [XmlAttribute]
      public string TypeId;
      [ProtoMember(4)]
      [XmlAttribute]
      public string SubtypeName;

      [XmlIgnore]
      public MyStringHash SubtypeId => MyStringHash.GetOrCompute(this.SubtypeName);

      protected class VRage_Game_ObjectBuilders_MyGroupedIds\u003C\u003EGroupedId\u003C\u003ETypeId\u003C\u003EAccessor : IMemberAccessor<MyGroupedIds.GroupedId, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGroupedIds.GroupedId owner, in string value) => owner.TypeId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGroupedIds.GroupedId owner, out string value) => value = owner.TypeId;
      }

      protected class VRage_Game_ObjectBuilders_MyGroupedIds\u003C\u003EGroupedId\u003C\u003ESubtypeName\u003C\u003EAccessor : IMemberAccessor<MyGroupedIds.GroupedId, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGroupedIds.GroupedId owner, in string value) => owner.SubtypeName = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGroupedIds.GroupedId owner, out string value) => value = owner.SubtypeName;
      }

      private class VRage_Game_ObjectBuilders_MyGroupedIds\u003C\u003EGroupedId\u003C\u003EActor : IActivator, IActivator<MyGroupedIds.GroupedId>
      {
        object IActivator.CreateInstance() => (object) new MyGroupedIds.GroupedId();

        MyGroupedIds.GroupedId IActivator<MyGroupedIds.GroupedId>.CreateInstance() => new MyGroupedIds.GroupedId();
      }
    }

    protected class VRage_Game_ObjectBuilders_MyGroupedIds\u003C\u003ETag\u003C\u003EAccessor : IMemberAccessor<MyGroupedIds, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyGroupedIds owner, in string value) => owner.Tag = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyGroupedIds owner, out string value) => value = owner.Tag;
    }

    protected class VRage_Game_ObjectBuilders_MyGroupedIds\u003C\u003EEntries\u003C\u003EAccessor : IMemberAccessor<MyGroupedIds, MyGroupedIds.GroupedId[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyGroupedIds owner, in MyGroupedIds.GroupedId[] value) => owner.Entries = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyGroupedIds owner, out MyGroupedIds.GroupedId[] value) => value = owner.Entries;
    }

    private class VRage_Game_ObjectBuilders_MyGroupedIds\u003C\u003EActor : IActivator, IActivator<MyGroupedIds>
    {
      object IActivator.CreateInstance() => (object) new MyGroupedIds();

      MyGroupedIds IActivator<MyGroupedIds>.CreateInstance() => new MyGroupedIds();
    }
  }
}
