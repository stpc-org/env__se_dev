// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyPhysicalModelItem
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyPhysicalModelItem
  {
    [ProtoMember(1)]
    [XmlAttribute(AttributeName = "TypeId")]
    public string TypeId;
    [ProtoMember(4)]
    [XmlAttribute(AttributeName = "SubtypeId")]
    public string SubtypeId;
    [ProtoMember(7)]
    [XmlAttribute(AttributeName = "Weight")]
    public float Weight = 1f;

    protected class VRage_Game_MyPhysicalModelItem\u003C\u003ETypeId\u003C\u003EAccessor : IMemberAccessor<MyPhysicalModelItem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPhysicalModelItem owner, in string value) => owner.TypeId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPhysicalModelItem owner, out string value) => value = owner.TypeId;
    }

    protected class VRage_Game_MyPhysicalModelItem\u003C\u003ESubtypeId\u003C\u003EAccessor : IMemberAccessor<MyPhysicalModelItem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPhysicalModelItem owner, in string value) => owner.SubtypeId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPhysicalModelItem owner, out string value) => value = owner.SubtypeId;
    }

    protected class VRage_Game_MyPhysicalModelItem\u003C\u003EWeight\u003C\u003EAccessor : IMemberAccessor<MyPhysicalModelItem, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPhysicalModelItem owner, in float value) => owner.Weight = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPhysicalModelItem owner, out float value) => value = owner.Weight;
    }

    private class VRage_Game_MyPhysicalModelItem\u003C\u003EActor : IActivator, IActivator<MyPhysicalModelItem>
    {
      object IActivator.CreateInstance() => (object) new MyPhysicalModelItem();

      MyPhysicalModelItem IActivator<MyPhysicalModelItem>.CreateInstance() => new MyPhysicalModelItem();
    }
  }
}
