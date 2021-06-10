// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyTutorialDescription
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
  public struct MyTutorialDescription
  {
    [ProtoMember(1)]
    public string Name;
    [ProtoMember(4)]
    [XmlArrayItem("Tutorial")]
    public string[] UnlockedBy;

    protected class VRage_Game_MyTutorialDescription\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyTutorialDescription, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyTutorialDescription owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyTutorialDescription owner, out string value) => value = owner.Name;
    }

    protected class VRage_Game_MyTutorialDescription\u003C\u003EUnlockedBy\u003C\u003EAccessor : IMemberAccessor<MyTutorialDescription, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyTutorialDescription owner, in string[] value) => owner.UnlockedBy = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyTutorialDescription owner, out string[] value) => value = owner.UnlockedBy;
    }

    private class VRage_Game_MyTutorialDescription\u003C\u003EActor : IActivator, IActivator<MyTutorialDescription>
    {
      object IActivator.CreateInstance() => (object) new MyTutorialDescription();

      MyTutorialDescription IActivator<MyTutorialDescription>.CreateInstance() => new MyTutorialDescription();
    }
  }
}
