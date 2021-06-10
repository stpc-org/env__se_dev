// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_FootsPosition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  public class MyObjectBuilder_FootsPosition
  {
    [ProtoMember(1)]
    public MyCharacterMovementEnum Animation;
    [ProtoMember(2)]
    public Vector3 LeftFoot;
    [ProtoMember(3)]
    public Vector3 RightFoot;

    protected class VRage_Game_MyObjectBuilder_FootsPosition\u003C\u003EAnimation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FootsPosition, MyCharacterMovementEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FootsPosition owner,
        in MyCharacterMovementEnum value)
      {
        owner.Animation = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FootsPosition owner,
        out MyCharacterMovementEnum value)
      {
        value = owner.Animation;
      }
    }

    protected class VRage_Game_MyObjectBuilder_FootsPosition\u003C\u003ELeftFoot\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FootsPosition, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FootsPosition owner, in Vector3 value) => owner.LeftFoot = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FootsPosition owner, out Vector3 value) => value = owner.LeftFoot;
    }

    protected class VRage_Game_MyObjectBuilder_FootsPosition\u003C\u003ERightFoot\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FootsPosition, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FootsPosition owner, in Vector3 value) => owner.RightFoot = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FootsPosition owner, out Vector3 value) => value = owner.RightFoot;
    }

    private class VRage_Game_MyObjectBuilder_FootsPosition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_FootsPosition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_FootsPosition();

      MyObjectBuilder_FootsPosition IActivator<MyObjectBuilder_FootsPosition>.CreateInstance() => new MyObjectBuilder_FootsPosition();
    }
  }
}
