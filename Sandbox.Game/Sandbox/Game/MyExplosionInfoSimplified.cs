// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.MyExplosionInfoSimplified
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game
{
  [Serializable]
  public struct MyExplosionInfoSimplified
  {
    public float Damage;
    public Vector3D Center;
    public float Radius;
    public MyExplosionTypeEnum Type;
    public MyExplosionFlags Flags;
    public Vector3D VoxelCenter;
    public float ParticleScale;
    public Vector3 Velocity;
    public bool IgnoreFriendlyFireSetting;

    protected class Sandbox_Game_MyExplosionInfoSimplified\u003C\u003EDamage\u003C\u003EAccessor : IMemberAccessor<MyExplosionInfoSimplified, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyExplosionInfoSimplified owner, in float value) => owner.Damage = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyExplosionInfoSimplified owner, out float value) => value = owner.Damage;
    }

    protected class Sandbox_Game_MyExplosionInfoSimplified\u003C\u003ECenter\u003C\u003EAccessor : IMemberAccessor<MyExplosionInfoSimplified, Vector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyExplosionInfoSimplified owner, in Vector3D value) => owner.Center = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyExplosionInfoSimplified owner, out Vector3D value) => value = owner.Center;
    }

    protected class Sandbox_Game_MyExplosionInfoSimplified\u003C\u003ERadius\u003C\u003EAccessor : IMemberAccessor<MyExplosionInfoSimplified, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyExplosionInfoSimplified owner, in float value) => owner.Radius = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyExplosionInfoSimplified owner, out float value) => value = owner.Radius;
    }

    protected class Sandbox_Game_MyExplosionInfoSimplified\u003C\u003EType\u003C\u003EAccessor : IMemberAccessor<MyExplosionInfoSimplified, MyExplosionTypeEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyExplosionInfoSimplified owner, in MyExplosionTypeEnum value) => owner.Type = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyExplosionInfoSimplified owner, out MyExplosionTypeEnum value) => value = owner.Type;
    }

    protected class Sandbox_Game_MyExplosionInfoSimplified\u003C\u003EFlags\u003C\u003EAccessor : IMemberAccessor<MyExplosionInfoSimplified, MyExplosionFlags>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyExplosionInfoSimplified owner, in MyExplosionFlags value) => owner.Flags = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyExplosionInfoSimplified owner, out MyExplosionFlags value) => value = owner.Flags;
    }

    protected class Sandbox_Game_MyExplosionInfoSimplified\u003C\u003EVoxelCenter\u003C\u003EAccessor : IMemberAccessor<MyExplosionInfoSimplified, Vector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyExplosionInfoSimplified owner, in Vector3D value) => owner.VoxelCenter = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyExplosionInfoSimplified owner, out Vector3D value) => value = owner.VoxelCenter;
    }

    protected class Sandbox_Game_MyExplosionInfoSimplified\u003C\u003EParticleScale\u003C\u003EAccessor : IMemberAccessor<MyExplosionInfoSimplified, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyExplosionInfoSimplified owner, in float value) => owner.ParticleScale = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyExplosionInfoSimplified owner, out float value) => value = owner.ParticleScale;
    }

    protected class Sandbox_Game_MyExplosionInfoSimplified\u003C\u003EVelocity\u003C\u003EAccessor : IMemberAccessor<MyExplosionInfoSimplified, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyExplosionInfoSimplified owner, in Vector3 value) => owner.Velocity = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyExplosionInfoSimplified owner, out Vector3 value) => value = owner.Velocity;
    }

    protected class Sandbox_Game_MyExplosionInfoSimplified\u003C\u003EIgnoreFriendlyFireSetting\u003C\u003EAccessor : IMemberAccessor<MyExplosionInfoSimplified, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyExplosionInfoSimplified owner, in bool value) => owner.IgnoreFriendlyFireSetting = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyExplosionInfoSimplified owner, out bool value) => value = owner.IgnoreFriendlyFireSetting;
    }
  }
}
