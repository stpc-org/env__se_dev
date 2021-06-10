// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyBBMemoryLong
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
  [XmlType("MyBBMemoryLong")]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyBBMemoryLong : MyBBMemoryValue
  {
    [ProtoMember(1)]
    public long LongValue;

    protected class VRage_Game_MyBBMemoryLong\u003C\u003ELongValue\u003C\u003EAccessor : IMemberAccessor<MyBBMemoryLong, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyBBMemoryLong owner, in long value) => owner.LongValue = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyBBMemoryLong owner, out long value) => value = owner.LongValue;
    }

    private class VRage_Game_MyBBMemoryLong\u003C\u003EActor : IActivator, IActivator<MyBBMemoryLong>
    {
      object IActivator.CreateInstance() => (object) new MyBBMemoryLong();

      MyBBMemoryLong IActivator<MyBBMemoryLong>.CreateInstance() => new MyBBMemoryLong();
    }
  }
}
