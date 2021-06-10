// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.MyMappedId
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders
{
  [ProtoContract]
  public class MyMappedId
  {
    [ProtoMember(1)]
    [XmlAttribute]
    public string Group;
    [ProtoMember(4)]
    [XmlAttribute]
    public string TypeId;
    [ProtoMember(7)]
    [XmlAttribute]
    public string SubtypeName;

    [XmlIgnore]
    public MyStringHash GroupId => MyStringHash.GetOrCompute(this.Group);

    [XmlIgnore]
    public MyStringHash SubtypeId => MyStringHash.GetOrCompute(this.SubtypeName);

    protected class VRage_Game_ObjectBuilders_MyMappedId\u003C\u003EGroup\u003C\u003EAccessor : IMemberAccessor<MyMappedId, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyMappedId owner, in string value) => owner.Group = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyMappedId owner, out string value) => value = owner.Group;
    }

    protected class VRage_Game_ObjectBuilders_MyMappedId\u003C\u003ETypeId\u003C\u003EAccessor : IMemberAccessor<MyMappedId, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyMappedId owner, in string value) => owner.TypeId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyMappedId owner, out string value) => value = owner.TypeId;
    }

    protected class VRage_Game_ObjectBuilders_MyMappedId\u003C\u003ESubtypeName\u003C\u003EAccessor : IMemberAccessor<MyMappedId, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyMappedId owner, in string value) => owner.SubtypeName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyMappedId owner, out string value) => value = owner.SubtypeName;
    }

    private class VRage_Game_ObjectBuilders_MyMappedId\u003C\u003EActor : IActivator, IActivator<MyMappedId>
    {
      object IActivator.CreateInstance() => (object) new MyMappedId();

      MyMappedId IActivator<MyMappedId>.CreateInstance() => new MyMappedId();
    }
  }
}
