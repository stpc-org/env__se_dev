// Decompiled with JetBrains decompiler
// Type: VRage.Game.EmptyArea
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
  public struct EmptyArea
  {
    [ProtoMember(19)]
    public Vector3D Position;
    [ProtoMember(22)]
    public float Radius;

    protected class VRage_Game_EmptyArea\u003C\u003EPosition\u003C\u003EAccessor : IMemberAccessor<EmptyArea, Vector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref EmptyArea owner, in Vector3D value) => owner.Position = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref EmptyArea owner, out Vector3D value) => value = owner.Position;
    }

    protected class VRage_Game_EmptyArea\u003C\u003ERadius\u003C\u003EAccessor : IMemberAccessor<EmptyArea, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref EmptyArea owner, in float value) => owner.Radius = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref EmptyArea owner, out float value) => value = owner.Radius;
    }

    private class VRage_Game_EmptyArea\u003C\u003EActor : IActivator, IActivator<EmptyArea>
    {
      object IActivator.CreateInstance() => (object) new EmptyArea();

      EmptyArea IActivator<EmptyArea>.CreateInstance() => new EmptyArea();
    }
  }
}
