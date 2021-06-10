// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyBBMemoryString
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
  [XmlType("MyBBMemoryString")]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyBBMemoryString : MyBBMemoryValue
  {
    [ProtoMember(1)]
    public string StringValue;

    protected class VRage_Game_MyBBMemoryString\u003C\u003EStringValue\u003C\u003EAccessor : IMemberAccessor<MyBBMemoryString, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyBBMemoryString owner, in string value) => owner.StringValue = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyBBMemoryString owner, out string value) => value = owner.StringValue;
    }

    private class VRage_Game_MyBBMemoryString\u003C\u003EActor : IActivator, IActivator<MyBBMemoryString>
    {
      object IActivator.CreateInstance() => (object) new MyBBMemoryString();

      MyBBMemoryString IActivator<MyBBMemoryString>.CreateInstance() => new MyBBMemoryString();
    }
  }
}
