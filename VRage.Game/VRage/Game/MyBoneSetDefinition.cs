// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyBoneSetDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  public class MyBoneSetDefinition
  {
    [ProtoMember(9)]
    public string Name;
    [ProtoMember(10)]
    public string Bones;

    protected class VRage_Game_MyBoneSetDefinition\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyBoneSetDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyBoneSetDefinition owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyBoneSetDefinition owner, out string value) => value = owner.Name;
    }

    protected class VRage_Game_MyBoneSetDefinition\u003C\u003EBones\u003C\u003EAccessor : IMemberAccessor<MyBoneSetDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyBoneSetDefinition owner, in string value) => owner.Bones = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyBoneSetDefinition owner, out string value) => value = owner.Bones;
    }

    private class VRage_Game_MyBoneSetDefinition\u003C\u003EActor : IActivator, IActivator<MyBoneSetDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyBoneSetDefinition();

      MyBoneSetDefinition IActivator<MyBoneSetDefinition>.CreateInstance() => new MyBoneSetDefinition();
    }
  }
}
