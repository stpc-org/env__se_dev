// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyMusicCategory
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
  public class MyMusicCategory
  {
    [ProtoMember(51)]
    [XmlAttribute(AttributeName = "Category")]
    public string Category;
    [ProtoMember(52)]
    [XmlAttribute(AttributeName = "Frequency")]
    public float Frequency = 1f;

    protected class VRage_Game_MyMusicCategory\u003C\u003ECategory\u003C\u003EAccessor : IMemberAccessor<MyMusicCategory, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyMusicCategory owner, in string value) => owner.Category = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyMusicCategory owner, out string value) => value = owner.Category;
    }

    protected class VRage_Game_MyMusicCategory\u003C\u003EFrequency\u003C\u003EAccessor : IMemberAccessor<MyMusicCategory, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyMusicCategory owner, in float value) => owner.Frequency = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyMusicCategory owner, out float value) => value = owner.Frequency;
    }

    private class VRage_Game_MyMusicCategory\u003C\u003EActor : IActivator, IActivator<MyMusicCategory>
    {
      object IActivator.CreateInstance() => (object) new MyMusicCategory();

      MyMusicCategory IActivator<MyMusicCategory>.CreateInstance() => new MyMusicCategory();
    }
  }
}
