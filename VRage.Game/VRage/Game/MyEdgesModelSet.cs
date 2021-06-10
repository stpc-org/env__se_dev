// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyEdgesModelSet
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Data;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  public class MyEdgesModelSet
  {
    [ProtoMember(1)]
    [ModdableContentFile("mwm")]
    public string Vertical;
    [ProtoMember(4)]
    [ModdableContentFile("mwm")]
    public string VerticalDiagonal;
    [ProtoMember(7)]
    [ModdableContentFile("mwm")]
    public string Horisontal;
    [ProtoMember(10)]
    [ModdableContentFile("mwm")]
    public string HorisontalDiagonal;

    protected class VRage_Game_MyEdgesModelSet\u003C\u003EVertical\u003C\u003EAccessor : IMemberAccessor<MyEdgesModelSet, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyEdgesModelSet owner, in string value) => owner.Vertical = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyEdgesModelSet owner, out string value) => value = owner.Vertical;
    }

    protected class VRage_Game_MyEdgesModelSet\u003C\u003EVerticalDiagonal\u003C\u003EAccessor : IMemberAccessor<MyEdgesModelSet, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyEdgesModelSet owner, in string value) => owner.VerticalDiagonal = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyEdgesModelSet owner, out string value) => value = owner.VerticalDiagonal;
    }

    protected class VRage_Game_MyEdgesModelSet\u003C\u003EHorisontal\u003C\u003EAccessor : IMemberAccessor<MyEdgesModelSet, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyEdgesModelSet owner, in string value) => owner.Horisontal = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyEdgesModelSet owner, out string value) => value = owner.Horisontal;
    }

    protected class VRage_Game_MyEdgesModelSet\u003C\u003EHorisontalDiagonal\u003C\u003EAccessor : IMemberAccessor<MyEdgesModelSet, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyEdgesModelSet owner, in string value) => owner.HorisontalDiagonal = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyEdgesModelSet owner, out string value) => value = owner.HorisontalDiagonal;
    }

    private class VRage_Game_MyEdgesModelSet\u003C\u003EActor : IActivator, IActivator<MyEdgesModelSet>
    {
      object IActivator.CreateInstance() => (object) new MyEdgesModelSet();

      MyEdgesModelSet IActivator<MyEdgesModelSet>.CreateInstance() => new MyEdgesModelSet();
    }
  }
}
