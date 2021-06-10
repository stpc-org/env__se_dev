// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyMovementAnimationMapping
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
  public class MyMovementAnimationMapping
  {
    [ProtoMember(12)]
    [XmlAttribute]
    public string Name;
    [ProtoMember(13)]
    [XmlAttribute]
    public string AnimationSubtypeName;

    protected class VRage_Game_MyMovementAnimationMapping\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyMovementAnimationMapping, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyMovementAnimationMapping owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyMovementAnimationMapping owner, out string value) => value = owner.Name;
    }

    protected class VRage_Game_MyMovementAnimationMapping\u003C\u003EAnimationSubtypeName\u003C\u003EAccessor : IMemberAccessor<MyMovementAnimationMapping, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyMovementAnimationMapping owner, in string value) => owner.AnimationSubtypeName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyMovementAnimationMapping owner, out string value) => value = owner.AnimationSubtypeName;
    }

    private class VRage_Game_MyMovementAnimationMapping\u003C\u003EActor : IActivator, IActivator<MyMovementAnimationMapping>
    {
      object IActivator.CreateInstance() => (object) new MyMovementAnimationMapping();

      MyMovementAnimationMapping IActivator<MyMovementAnimationMapping>.CreateInstance() => new MyMovementAnimationMapping();
    }
  }
}
