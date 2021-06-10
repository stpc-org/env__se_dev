// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyPlanetSurfaceDetail
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
  public class MyPlanetSurfaceDetail
  {
    [ProtoMember(33)]
    public string Texture;
    [ProtoMember(34)]
    public float Size;
    [ProtoMember(35)]
    public float Scale;
    [ProtoMember(36)]
    public SerializableRange Slope;
    [ProtoMember(37)]
    public float Transition;

    protected class VRage_Game_MyPlanetSurfaceDetail\u003C\u003ETexture\u003C\u003EAccessor : IMemberAccessor<MyPlanetSurfaceDetail, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetSurfaceDetail owner, in string value) => owner.Texture = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetSurfaceDetail owner, out string value) => value = owner.Texture;
    }

    protected class VRage_Game_MyPlanetSurfaceDetail\u003C\u003ESize\u003C\u003EAccessor : IMemberAccessor<MyPlanetSurfaceDetail, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetSurfaceDetail owner, in float value) => owner.Size = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetSurfaceDetail owner, out float value) => value = owner.Size;
    }

    protected class VRage_Game_MyPlanetSurfaceDetail\u003C\u003EScale\u003C\u003EAccessor : IMemberAccessor<MyPlanetSurfaceDetail, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetSurfaceDetail owner, in float value) => owner.Scale = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetSurfaceDetail owner, out float value) => value = owner.Scale;
    }

    protected class VRage_Game_MyPlanetSurfaceDetail\u003C\u003ESlope\u003C\u003EAccessor : IMemberAccessor<MyPlanetSurfaceDetail, SerializableRange>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetSurfaceDetail owner, in SerializableRange value) => owner.Slope = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetSurfaceDetail owner, out SerializableRange value) => value = owner.Slope;
    }

    protected class VRage_Game_MyPlanetSurfaceDetail\u003C\u003ETransition\u003C\u003EAccessor : IMemberAccessor<MyPlanetSurfaceDetail, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetSurfaceDetail owner, in float value) => owner.Transition = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetSurfaceDetail owner, out float value) => value = owner.Transition;
    }

    private class VRage_Game_MyPlanetSurfaceDetail\u003C\u003EActor : IActivator, IActivator<MyPlanetSurfaceDetail>
    {
      object IActivator.CreateInstance() => (object) new MyPlanetSurfaceDetail();

      MyPlanetSurfaceDetail IActivator<MyPlanetSurfaceDetail>.CreateInstance() => new MyPlanetSurfaceDetail();
    }
  }
}
