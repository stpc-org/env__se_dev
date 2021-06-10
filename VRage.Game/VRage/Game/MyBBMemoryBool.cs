// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyBBMemoryBool
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
  [XmlType("MyBBMemoryBool")]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyBBMemoryBool : MyBBMemoryValue
  {
    [ProtoMember(1)]
    public bool BoolValue;

    protected class VRage_Game_MyBBMemoryBool\u003C\u003EBoolValue\u003C\u003EAccessor : IMemberAccessor<MyBBMemoryBool, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyBBMemoryBool owner, in bool value) => owner.BoolValue = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyBBMemoryBool owner, out bool value) => value = owner.BoolValue;
    }

    private class VRage_Game_MyBBMemoryBool\u003C\u003EActor : IActivator, IActivator<MyBBMemoryBool>
    {
      object IActivator.CreateInstance() => (object) new MyBBMemoryBool();

      MyBBMemoryBool IActivator<MyBBMemoryBool>.CreateInstance() => new MyBBMemoryBool();
    }
  }
}
