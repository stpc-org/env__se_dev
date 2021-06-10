// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Components.AnimationData
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.Serialization;

namespace VRage.Game.ObjectBuilders.Components
{
  [ProtoContract]
  public struct AnimationData
  {
    [ProtoMember(34)]
    public string Animation;
    [ProtoMember(35)]
    [Nullable]
    public string Animation2;

    protected class VRage_Game_ObjectBuilders_Components_AnimationData\u003C\u003EAnimation\u003C\u003EAccessor : IMemberAccessor<AnimationData, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref AnimationData owner, in string value) => owner.Animation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref AnimationData owner, out string value) => value = owner.Animation;
    }

    protected class VRage_Game_ObjectBuilders_Components_AnimationData\u003C\u003EAnimation2\u003C\u003EAccessor : IMemberAccessor<AnimationData, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref AnimationData owner, in string value) => owner.Animation2 = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref AnimationData owner, out string value) => value = owner.Animation2;
    }

    private class VRage_Game_ObjectBuilders_Components_AnimationData\u003C\u003EActor : IActivator, IActivator<AnimationData>
    {
      object IActivator.CreateInstance() => (object) new AnimationData();

      AnimationData IActivator<AnimationData>.CreateInstance() => new AnimationData();
    }
  }
}
