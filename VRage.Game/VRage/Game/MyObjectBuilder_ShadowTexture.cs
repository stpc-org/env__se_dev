// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_ShadowTexture
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  public class MyObjectBuilder_ShadowTexture
  {
    [ProtoMember(4)]
    public string Texture = "";
    [ProtoMember(7)]
    public float MinWidth;
    [ProtoMember(10)]
    public float GrowFactorWidth = 1f;
    [ProtoMember(13)]
    public float GrowFactorHeight = 1f;
    [ProtoMember(16)]
    public float DefaultAlpha = 1f;

    protected class VRage_Game_MyObjectBuilder_ShadowTexture\u003C\u003ETexture\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ShadowTexture, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShadowTexture owner, in string value) => owner.Texture = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShadowTexture owner, out string value) => value = owner.Texture;
    }

    protected class VRage_Game_MyObjectBuilder_ShadowTexture\u003C\u003EMinWidth\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ShadowTexture, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShadowTexture owner, in float value) => owner.MinWidth = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShadowTexture owner, out float value) => value = owner.MinWidth;
    }

    protected class VRage_Game_MyObjectBuilder_ShadowTexture\u003C\u003EGrowFactorWidth\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ShadowTexture, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShadowTexture owner, in float value) => owner.GrowFactorWidth = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShadowTexture owner, out float value) => value = owner.GrowFactorWidth;
    }

    protected class VRage_Game_MyObjectBuilder_ShadowTexture\u003C\u003EGrowFactorHeight\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ShadowTexture, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShadowTexture owner, in float value) => owner.GrowFactorHeight = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShadowTexture owner, out float value) => value = owner.GrowFactorHeight;
    }

    protected class VRage_Game_MyObjectBuilder_ShadowTexture\u003C\u003EDefaultAlpha\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ShadowTexture, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ShadowTexture owner, in float value) => owner.DefaultAlpha = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ShadowTexture owner, out float value) => value = owner.DefaultAlpha;
    }

    private class VRage_Game_MyObjectBuilder_ShadowTexture\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ShadowTexture>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ShadowTexture();

      MyObjectBuilder_ShadowTexture IActivator<MyObjectBuilder_ShadowTexture>.CreateInstance() => new MyObjectBuilder_ShadowTexture();
    }
  }
}
