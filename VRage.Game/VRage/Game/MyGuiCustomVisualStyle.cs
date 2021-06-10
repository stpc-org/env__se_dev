// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyGuiCustomVisualStyle
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
  public struct MyGuiCustomVisualStyle
  {
    [ProtoMember(1)]
    public string NormalTexture;
    [ProtoMember(4)]
    public string HighlightTexture;
    [ProtoMember(7)]
    public Vector2 Size;
    [ProtoMember(10)]
    public string NormalFont;
    [ProtoMember(13)]
    public string HighlightFont;
    [ProtoMember(16)]
    public float HorizontalPadding;
    [ProtoMember(19)]
    public float VerticalPadding;

    protected class VRage_Game_MyGuiCustomVisualStyle\u003C\u003ENormalTexture\u003C\u003EAccessor : IMemberAccessor<MyGuiCustomVisualStyle, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyGuiCustomVisualStyle owner, in string value) => owner.NormalTexture = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyGuiCustomVisualStyle owner, out string value) => value = owner.NormalTexture;
    }

    protected class VRage_Game_MyGuiCustomVisualStyle\u003C\u003EHighlightTexture\u003C\u003EAccessor : IMemberAccessor<MyGuiCustomVisualStyle, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyGuiCustomVisualStyle owner, in string value) => owner.HighlightTexture = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyGuiCustomVisualStyle owner, out string value) => value = owner.HighlightTexture;
    }

    protected class VRage_Game_MyGuiCustomVisualStyle\u003C\u003ESize\u003C\u003EAccessor : IMemberAccessor<MyGuiCustomVisualStyle, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyGuiCustomVisualStyle owner, in Vector2 value) => owner.Size = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyGuiCustomVisualStyle owner, out Vector2 value) => value = owner.Size;
    }

    protected class VRage_Game_MyGuiCustomVisualStyle\u003C\u003ENormalFont\u003C\u003EAccessor : IMemberAccessor<MyGuiCustomVisualStyle, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyGuiCustomVisualStyle owner, in string value) => owner.NormalFont = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyGuiCustomVisualStyle owner, out string value) => value = owner.NormalFont;
    }

    protected class VRage_Game_MyGuiCustomVisualStyle\u003C\u003EHighlightFont\u003C\u003EAccessor : IMemberAccessor<MyGuiCustomVisualStyle, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyGuiCustomVisualStyle owner, in string value) => owner.HighlightFont = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyGuiCustomVisualStyle owner, out string value) => value = owner.HighlightFont;
    }

    protected class VRage_Game_MyGuiCustomVisualStyle\u003C\u003EHorizontalPadding\u003C\u003EAccessor : IMemberAccessor<MyGuiCustomVisualStyle, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyGuiCustomVisualStyle owner, in float value) => owner.HorizontalPadding = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyGuiCustomVisualStyle owner, out float value) => value = owner.HorizontalPadding;
    }

    protected class VRage_Game_MyGuiCustomVisualStyle\u003C\u003EVerticalPadding\u003C\u003EAccessor : IMemberAccessor<MyGuiCustomVisualStyle, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyGuiCustomVisualStyle owner, in float value) => owner.VerticalPadding = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyGuiCustomVisualStyle owner, out float value) => value = owner.VerticalPadding;
    }

    private class VRage_Game_MyGuiCustomVisualStyle\u003C\u003EActor : IActivator, IActivator<MyGuiCustomVisualStyle>
    {
      object IActivator.CreateInstance() => (object) new MyGuiCustomVisualStyle();

      MyGuiCustomVisualStyle IActivator<MyGuiCustomVisualStyle>.CreateInstance() => new MyGuiCustomVisualStyle();
    }
  }
}
