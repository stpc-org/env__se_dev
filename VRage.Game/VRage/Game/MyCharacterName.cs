// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyCharacterName
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
  public class MyCharacterName
  {
    [XmlAttribute]
    [ProtoMember(1)]
    public string Name;

    protected class VRage_Game_MyCharacterName\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyCharacterName, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyCharacterName owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyCharacterName owner, out string value) => value = owner.Name;
    }

    private class VRage_Game_MyCharacterName\u003C\u003EActor : IActivator, IActivator<MyCharacterName>
    {
      object IActivator.CreateInstance() => (object) new MyCharacterName();

      MyCharacterName IActivator<MyCharacterName>.CreateInstance() => new MyCharacterName();
    }
  }
}
