// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyBBMemoryInt
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
  [XmlType("MyBBMemoryInt")]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyBBMemoryInt : MyBBMemoryValue
  {
    [ProtoMember(1)]
    public int IntValue;

    protected class VRage_Game_MyBBMemoryInt\u003C\u003EIntValue\u003C\u003EAccessor : IMemberAccessor<MyBBMemoryInt, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyBBMemoryInt owner, in int value) => owner.IntValue = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyBBMemoryInt owner, out int value) => value = owner.IntValue;
    }

    private class VRage_Game_MyBBMemoryInt\u003C\u003EActor : IActivator, IActivator<MyBBMemoryInt>
    {
      object IActivator.CreateInstance() => (object) new MyBBMemoryInt();

      MyBBMemoryInt IActivator<MyBBMemoryInt>.CreateInstance() => new MyBBMemoryInt();
    }
  }
}
