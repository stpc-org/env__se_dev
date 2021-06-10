// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyPlanetAnimal
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
  public class MyPlanetAnimal
  {
    [ProtoMember(25)]
    [XmlAttribute(AttributeName = "Type")]
    public string AnimalType;

    protected class VRage_Game_MyPlanetAnimal\u003C\u003EAnimalType\u003C\u003EAccessor : IMemberAccessor<MyPlanetAnimal, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetAnimal owner, in string value) => owner.AnimalType = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetAnimal owner, out string value) => value = owner.AnimalType;
    }

    private class VRage_Game_MyPlanetAnimal\u003C\u003EActor : IActivator, IActivator<MyPlanetAnimal>
    {
      object IActivator.CreateInstance() => (object) new MyPlanetAnimal();

      MyPlanetAnimal IActivator<MyPlanetAnimal>.CreateInstance() => new MyPlanetAnimal();
    }
  }
}
