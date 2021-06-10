// Decompiled with JetBrains decompiler
// Type: VRage.Game.GUI.TextPanel.MySerializableSprite
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.Serialization;

namespace VRage.Game.GUI.TextPanel
{
  [ProtoContract]
  public struct MySerializableSprite
  {
    [ProtoMember(1)]
    [DefaultValue(SpriteType.TEXTURE)]
    public SpriteType Type;
    [ProtoMember(4)]
    [Nullable]
    [DefaultValue(null)]
    public SerializableVector2? Position;
    [ProtoMember(7)]
    [Nullable]
    [DefaultValue(null)]
    public SerializableVector2? Size;
    [ProtoMember(10)]
    [Nullable]
    [DefaultValue(null)]
    public uint? Color;
    [ProtoMember(13)]
    [Nullable]
    [DefaultValue(null)]
    public string Data;
    [ProtoMember(16)]
    [Nullable]
    [DefaultValue(null)]
    public string FontId;
    [ProtoMember(19)]
    [DefaultValue(TextAlignment.CENTER)]
    public TextAlignment Alignment;
    [ProtoMember(22)]
    [DefaultValue(0.0f)]
    public float RotationOrScale;
    [ProtoMember(25)]
    public int Index;

    protected class VRage_Game_GUI_TextPanel_MySerializableSprite\u003C\u003EType\u003C\u003EAccessor : IMemberAccessor<MySerializableSprite, SpriteType>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySerializableSprite owner, in SpriteType value) => owner.Type = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySerializableSprite owner, out SpriteType value) => value = owner.Type;
    }

    protected class VRage_Game_GUI_TextPanel_MySerializableSprite\u003C\u003EPosition\u003C\u003EAccessor : IMemberAccessor<MySerializableSprite, SerializableVector2?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySerializableSprite owner, in SerializableVector2? value) => owner.Position = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySerializableSprite owner, out SerializableVector2? value) => value = owner.Position;
    }

    protected class VRage_Game_GUI_TextPanel_MySerializableSprite\u003C\u003ESize\u003C\u003EAccessor : IMemberAccessor<MySerializableSprite, SerializableVector2?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySerializableSprite owner, in SerializableVector2? value) => owner.Size = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySerializableSprite owner, out SerializableVector2? value) => value = owner.Size;
    }

    protected class VRage_Game_GUI_TextPanel_MySerializableSprite\u003C\u003EColor\u003C\u003EAccessor : IMemberAccessor<MySerializableSprite, uint?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySerializableSprite owner, in uint? value) => owner.Color = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySerializableSprite owner, out uint? value) => value = owner.Color;
    }

    protected class VRage_Game_GUI_TextPanel_MySerializableSprite\u003C\u003EData\u003C\u003EAccessor : IMemberAccessor<MySerializableSprite, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySerializableSprite owner, in string value) => owner.Data = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySerializableSprite owner, out string value) => value = owner.Data;
    }

    protected class VRage_Game_GUI_TextPanel_MySerializableSprite\u003C\u003EFontId\u003C\u003EAccessor : IMemberAccessor<MySerializableSprite, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySerializableSprite owner, in string value) => owner.FontId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySerializableSprite owner, out string value) => value = owner.FontId;
    }

    protected class VRage_Game_GUI_TextPanel_MySerializableSprite\u003C\u003EAlignment\u003C\u003EAccessor : IMemberAccessor<MySerializableSprite, TextAlignment>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySerializableSprite owner, in TextAlignment value) => owner.Alignment = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySerializableSprite owner, out TextAlignment value) => value = owner.Alignment;
    }

    protected class VRage_Game_GUI_TextPanel_MySerializableSprite\u003C\u003ERotationOrScale\u003C\u003EAccessor : IMemberAccessor<MySerializableSprite, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySerializableSprite owner, in float value) => owner.RotationOrScale = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySerializableSprite owner, out float value) => value = owner.RotationOrScale;
    }

    protected class VRage_Game_GUI_TextPanel_MySerializableSprite\u003C\u003EIndex\u003C\u003EAccessor : IMemberAccessor<MySerializableSprite, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySerializableSprite owner, in int value) => owner.Index = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySerializableSprite owner, out int value) => value = owner.Index;
    }

    private class VRage_Game_GUI_TextPanel_MySerializableSprite\u003C\u003EActor : IActivator, IActivator<MySerializableSprite>
    {
      object IActivator.CreateInstance() => (object) new MySerializableSprite();

      MySerializableSprite IActivator<MySerializableSprite>.CreateInstance() => new MySerializableSprite();
    }
  }
}
