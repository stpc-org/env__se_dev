// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyJetpackThrustDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  public class MyJetpackThrustDefinition
  {
    [ProtoMember(1)]
    public string ThrustBone;
    [ProtoMember(2)]
    public float SideFlameOffset = 0.12f;
    [ProtoMember(3)]
    public float FrontFlameOffset = 0.04f;

    protected class VRage_Game_MyJetpackThrustDefinition\u003C\u003EThrustBone\u003C\u003EAccessor : IMemberAccessor<MyJetpackThrustDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyJetpackThrustDefinition owner, in string value) => owner.ThrustBone = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyJetpackThrustDefinition owner, out string value) => value = owner.ThrustBone;
    }

    protected class VRage_Game_MyJetpackThrustDefinition\u003C\u003ESideFlameOffset\u003C\u003EAccessor : IMemberAccessor<MyJetpackThrustDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyJetpackThrustDefinition owner, in float value) => owner.SideFlameOffset = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyJetpackThrustDefinition owner, out float value) => value = owner.SideFlameOffset;
    }

    protected class VRage_Game_MyJetpackThrustDefinition\u003C\u003EFrontFlameOffset\u003C\u003EAccessor : IMemberAccessor<MyJetpackThrustDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyJetpackThrustDefinition owner, in float value) => owner.FrontFlameOffset = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyJetpackThrustDefinition owner, out float value) => value = owner.FrontFlameOffset;
    }

    private class VRage_Game_MyJetpackThrustDefinition\u003C\u003EActor : IActivator, IActivator<MyJetpackThrustDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyJetpackThrustDefinition();

      MyJetpackThrustDefinition IActivator<MyJetpackThrustDefinition>.CreateInstance() => new MyJetpackThrustDefinition();
    }
  }
}
