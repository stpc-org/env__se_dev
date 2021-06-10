// Decompiled with JetBrains decompiler
// Type: Medieval.ObjectBuilders.Definitions.TilingSetup
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace Medieval.ObjectBuilders.Definitions
{
  [ProtoContract]
  public class TilingSetup
  {
    [ProtoMember(133)]
    public float InitialScale = 2f;
    [ProtoMember(136)]
    public float ScaleMultiplier = 4f;
    [ProtoMember(139)]
    public float InitialDistance = 5f;
    [ProtoMember(142)]
    public float DistanceMultiplier = 4f;
    [ProtoMember(145)]
    public float TilingScale = 32f;
    [ProtoMember(148)]
    public float Far1Distance;
    [ProtoMember(151)]
    public float Far2Distance;
    [ProtoMember(154)]
    public float Far3Distance;
    [ProtoMember(157)]
    public float Far1Scale = 400f;
    [ProtoMember(160)]
    public float Far2Scale = 2000f;
    [ProtoMember(163)]
    public float Far3Scale = 7000f;
    [ProtoMember(166)]
    public float ExtDetailScale;

    protected class Medieval_ObjectBuilders_Definitions_TilingSetup\u003C\u003EInitialScale\u003C\u003EAccessor : IMemberAccessor<TilingSetup, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref TilingSetup owner, in float value) => owner.InitialScale = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref TilingSetup owner, out float value) => value = owner.InitialScale;
    }

    protected class Medieval_ObjectBuilders_Definitions_TilingSetup\u003C\u003EScaleMultiplier\u003C\u003EAccessor : IMemberAccessor<TilingSetup, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref TilingSetup owner, in float value) => owner.ScaleMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref TilingSetup owner, out float value) => value = owner.ScaleMultiplier;
    }

    protected class Medieval_ObjectBuilders_Definitions_TilingSetup\u003C\u003EInitialDistance\u003C\u003EAccessor : IMemberAccessor<TilingSetup, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref TilingSetup owner, in float value) => owner.InitialDistance = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref TilingSetup owner, out float value) => value = owner.InitialDistance;
    }

    protected class Medieval_ObjectBuilders_Definitions_TilingSetup\u003C\u003EDistanceMultiplier\u003C\u003EAccessor : IMemberAccessor<TilingSetup, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref TilingSetup owner, in float value) => owner.DistanceMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref TilingSetup owner, out float value) => value = owner.DistanceMultiplier;
    }

    protected class Medieval_ObjectBuilders_Definitions_TilingSetup\u003C\u003ETilingScale\u003C\u003EAccessor : IMemberAccessor<TilingSetup, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref TilingSetup owner, in float value) => owner.TilingScale = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref TilingSetup owner, out float value) => value = owner.TilingScale;
    }

    protected class Medieval_ObjectBuilders_Definitions_TilingSetup\u003C\u003EFar1Distance\u003C\u003EAccessor : IMemberAccessor<TilingSetup, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref TilingSetup owner, in float value) => owner.Far1Distance = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref TilingSetup owner, out float value) => value = owner.Far1Distance;
    }

    protected class Medieval_ObjectBuilders_Definitions_TilingSetup\u003C\u003EFar2Distance\u003C\u003EAccessor : IMemberAccessor<TilingSetup, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref TilingSetup owner, in float value) => owner.Far2Distance = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref TilingSetup owner, out float value) => value = owner.Far2Distance;
    }

    protected class Medieval_ObjectBuilders_Definitions_TilingSetup\u003C\u003EFar3Distance\u003C\u003EAccessor : IMemberAccessor<TilingSetup, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref TilingSetup owner, in float value) => owner.Far3Distance = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref TilingSetup owner, out float value) => value = owner.Far3Distance;
    }

    protected class Medieval_ObjectBuilders_Definitions_TilingSetup\u003C\u003EFar1Scale\u003C\u003EAccessor : IMemberAccessor<TilingSetup, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref TilingSetup owner, in float value) => owner.Far1Scale = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref TilingSetup owner, out float value) => value = owner.Far1Scale;
    }

    protected class Medieval_ObjectBuilders_Definitions_TilingSetup\u003C\u003EFar2Scale\u003C\u003EAccessor : IMemberAccessor<TilingSetup, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref TilingSetup owner, in float value) => owner.Far2Scale = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref TilingSetup owner, out float value) => value = owner.Far2Scale;
    }

    protected class Medieval_ObjectBuilders_Definitions_TilingSetup\u003C\u003EFar3Scale\u003C\u003EAccessor : IMemberAccessor<TilingSetup, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref TilingSetup owner, in float value) => owner.Far3Scale = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref TilingSetup owner, out float value) => value = owner.Far3Scale;
    }

    protected class Medieval_ObjectBuilders_Definitions_TilingSetup\u003C\u003EExtDetailScale\u003C\u003EAccessor : IMemberAccessor<TilingSetup, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref TilingSetup owner, in float value) => owner.ExtDetailScale = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref TilingSetup owner, out float value) => value = owner.ExtDetailScale;
    }

    private class Medieval_ObjectBuilders_Definitions_TilingSetup\u003C\u003EActor : IActivator, IActivator<TilingSetup>
    {
      object IActivator.CreateInstance() => (object) new TilingSetup();

      TilingSetup IActivator<TilingSetup>.CreateInstance() => new TilingSetup();
    }
  }
}
