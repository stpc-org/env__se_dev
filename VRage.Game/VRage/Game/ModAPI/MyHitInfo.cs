// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.MyHitInfo
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace VRage.Game.ModAPI
{
  [ProtoContract]
  public struct MyHitInfo
  {
    [ProtoMember(1)]
    public Vector3D Position;
    [ProtoMember(4)]
    public Vector3 Normal;
    [ProtoMember(7)]
    public Vector3D Velocity;
    [ProtoMember(10)]
    public uint ShapeKey;

    protected class VRage_Game_ModAPI_MyHitInfo\u003C\u003EPosition\u003C\u003EAccessor : IMemberAccessor<MyHitInfo, Vector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyHitInfo owner, in Vector3D value) => owner.Position = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyHitInfo owner, out Vector3D value) => value = owner.Position;
    }

    protected class VRage_Game_ModAPI_MyHitInfo\u003C\u003ENormal\u003C\u003EAccessor : IMemberAccessor<MyHitInfo, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyHitInfo owner, in Vector3 value) => owner.Normal = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyHitInfo owner, out Vector3 value) => value = owner.Normal;
    }

    protected class VRage_Game_ModAPI_MyHitInfo\u003C\u003EVelocity\u003C\u003EAccessor : IMemberAccessor<MyHitInfo, Vector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyHitInfo owner, in Vector3D value) => owner.Velocity = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyHitInfo owner, out Vector3D value) => value = owner.Velocity;
    }

    protected class VRage_Game_ModAPI_MyHitInfo\u003C\u003EShapeKey\u003C\u003EAccessor : IMemberAccessor<MyHitInfo, uint>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyHitInfo owner, in uint value) => owner.ShapeKey = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyHitInfo owner, out uint value) => value = owner.ShapeKey;
    }

    private class VRage_Game_ModAPI_MyHitInfo\u003C\u003EActor : IActivator, IActivator<MyHitInfo>
    {
      object IActivator.CreateInstance() => (object) new MyHitInfo();

      MyHitInfo IActivator<MyHitInfo>.CreateInstance() => new MyHitInfo();
    }
  }
}
