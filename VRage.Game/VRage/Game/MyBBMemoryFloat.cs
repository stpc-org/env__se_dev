// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyBBMemoryFloat
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
  [XmlType("MyBBMemoryFloat")]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyBBMemoryFloat : MyBBMemoryValue
  {
    [ProtoMember(1)]
    public float FloatValue;

    protected class VRage_Game_MyBBMemoryFloat\u003C\u003EFloatValue\u003C\u003EAccessor : IMemberAccessor<MyBBMemoryFloat, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyBBMemoryFloat owner, in float value) => owner.FloatValue = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyBBMemoryFloat owner, out float value) => value = owner.FloatValue;
    }

    private class VRage_Game_MyBBMemoryFloat\u003C\u003EActor : IActivator, IActivator<MyBBMemoryFloat>
    {
      object IActivator.CreateInstance() => (object) new MyBBMemoryFloat();

      MyBBMemoryFloat IActivator<MyBBMemoryFloat>.CreateInstance() => new MyBBMemoryFloat();
    }
  }
}
