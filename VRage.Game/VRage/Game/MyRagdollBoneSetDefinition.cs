// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyRagdollBoneSetDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  public class MyRagdollBoneSetDefinition : MyBoneSetDefinition
  {
    [ProtoMember(11)]
    public float CollisionRadius;

    protected class VRage_Game_MyRagdollBoneSetDefinition\u003C\u003ECollisionRadius\u003C\u003EAccessor : IMemberAccessor<MyRagdollBoneSetDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyRagdollBoneSetDefinition owner, in float value) => owner.CollisionRadius = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyRagdollBoneSetDefinition owner, out float value) => value = owner.CollisionRadius;
    }

    protected class VRage_Game_MyRagdollBoneSetDefinition\u003C\u003EName\u003C\u003EAccessor : MyBoneSetDefinition.VRage_Game_MyBoneSetDefinition\u003C\u003EName\u003C\u003EAccessor, IMemberAccessor<MyRagdollBoneSetDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyRagdollBoneSetDefinition owner, in string value) => this.Set((MyBoneSetDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyRagdollBoneSetDefinition owner, out string value) => this.Get((MyBoneSetDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyRagdollBoneSetDefinition\u003C\u003EBones\u003C\u003EAccessor : MyBoneSetDefinition.VRage_Game_MyBoneSetDefinition\u003C\u003EBones\u003C\u003EAccessor, IMemberAccessor<MyRagdollBoneSetDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyRagdollBoneSetDefinition owner, in string value) => this.Set((MyBoneSetDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyRagdollBoneSetDefinition owner, out string value) => this.Get((MyBoneSetDefinition&) ref owner, out value);
    }

    private class VRage_Game_MyRagdollBoneSetDefinition\u003C\u003EActor : IActivator, IActivator<MyRagdollBoneSetDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyRagdollBoneSetDefinition();

      MyRagdollBoneSetDefinition IActivator<MyRagdollBoneSetDefinition>.CreateInstance() => new MyRagdollBoneSetDefinition();
    }
  }
}
