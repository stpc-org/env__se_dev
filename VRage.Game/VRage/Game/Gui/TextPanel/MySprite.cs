// Decompiled with JetBrains decompiler
// Type: VRage.Game.GUI.TextPanel.MySprite
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.Serialization;
using VRageMath;

namespace VRage.Game.GUI.TextPanel
{
  [ProtoContract]
  [Serializable]
  public struct MySprite : IEquatable<MySprite>
  {
    [ProtoMember(1)]
    [DefaultValue(SpriteType.TEXTURE)]
    public SpriteType Type;
    [ProtoMember(4)]
    [Nullable]
    [DefaultValue(null)]
    public Vector2? Position;
    [ProtoMember(7)]
    [Nullable]
    [DefaultValue(null)]
    public Vector2? Size;
    [ProtoMember(10)]
    [Nullable]
    [DefaultValue(null)]
    public VRageMath.Color? Color;
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

    public MySprite(
      SpriteType type = SpriteType.TEXTURE,
      string data = null,
      Vector2? position = null,
      Vector2? size = null,
      VRageMath.Color? color = null,
      string fontId = null,
      TextAlignment alignment = TextAlignment.CENTER,
      float rotation = 0.0f)
    {
      this.Type = type;
      this.Data = data;
      this.Position = position;
      this.Size = size;
      this.Color = color;
      this.FontId = fontId;
      this.Alignment = alignment;
      this.RotationOrScale = rotation;
    }

    public static MySprite CreateSprite(string sprite, Vector2 position, Vector2 size) => new MySprite(data: sprite, position: new Vector2?(position), size: new Vector2?(size));

    public static MySprite CreateText(
      string text,
      string fontId,
      VRageMath.Color color,
      float scale = 1f,
      TextAlignment alignment = TextAlignment.CENTER)
    {
      string data = text;
      Vector2? position = new Vector2?();
      Vector2? size = new Vector2?();
      float num1 = scale;
      VRageMath.Color? color1 = new VRageMath.Color?(color);
      string fontId1 = fontId;
      int num2 = (int) alignment;
      double num3 = (double) num1;
      return new MySprite(SpriteType.TEXT, data, position, size, color1, fontId1, (TextAlignment) num2, (float) num3);
    }

    public static MySprite CreateClipRect(Rectangle rect) => new MySprite(SpriteType.CLIP_RECT, position: new Vector2?(new Vector2((float) rect.X, (float) rect.Y)), size: new Vector2?(new Vector2((float) rect.Width, (float) rect.Height)));

    public static MySprite CreateClearClipRect() => new MySprite(SpriteType.CLIP_RECT);

    public static implicit operator MySerializableSprite(MySprite sprite)
    {
      MySerializableSprite serializableSprite = new MySerializableSprite();
      serializableSprite.Type = sprite.Type;
      ref MySerializableSprite local1 = ref serializableSprite;
      Vector2? position = sprite.Position;
      SerializableVector2? nullable1 = position.HasValue ? new SerializableVector2?((SerializableVector2) position.GetValueOrDefault()) : new SerializableVector2?();
      local1.Position = nullable1;
      ref MySerializableSprite local2 = ref serializableSprite;
      Vector2? size = sprite.Size;
      SerializableVector2? nullable2 = size.HasValue ? new SerializableVector2?((SerializableVector2) size.GetValueOrDefault()) : new SerializableVector2?();
      local2.Size = nullable2;
      ref MySerializableSprite local3 = ref serializableSprite;
      ref VRageMath.Color? local4 = ref sprite.Color;
      uint? nullable3 = local4.HasValue ? new uint?(local4.GetValueOrDefault().PackedValue) : new uint?();
      local3.Color = nullable3;
      serializableSprite.Data = sprite.Data;
      serializableSprite.Alignment = sprite.Alignment;
      serializableSprite.FontId = sprite.FontId;
      serializableSprite.RotationOrScale = sprite.RotationOrScale;
      return serializableSprite;
    }

    public static implicit operator MySprite(MySerializableSprite sprite)
    {
      MySprite mySprite = new MySprite();
      mySprite.Type = sprite.Type;
      ref MySprite local1 = ref mySprite;
      SerializableVector2? position = sprite.Position;
      Vector2? nullable1 = position.HasValue ? new Vector2?((Vector2) position.GetValueOrDefault()) : new Vector2?();
      local1.Position = nullable1;
      ref MySprite local2 = ref mySprite;
      SerializableVector2? size = sprite.Size;
      Vector2? nullable2 = size.HasValue ? new Vector2?((Vector2) size.GetValueOrDefault()) : new Vector2?();
      local2.Size = nullable2;
      mySprite.Color = sprite.Color.HasValue ? new VRageMath.Color?(new VRageMath.Color(sprite.Color.Value)) : new VRageMath.Color?();
      mySprite.Data = sprite.Data;
      mySprite.Alignment = sprite.Alignment;
      mySprite.FontId = sprite.FontId;
      mySprite.RotationOrScale = sprite.RotationOrScale;
      return mySprite;
    }

    public bool Equals(MySprite other) => this.Type == other.Type && this.Alignment == other.Alignment && (this.RotationOrScale.Equals(other.RotationOrScale) && this.Position.Equals((object) other.Position)) && (this.Size.Equals((object) other.Size) && this.Color.Equals((object) other.Color) && this.AreStringsEqual(this.Data, other.Data)) && this.AreStringsEqual(this.FontId, other.FontId);

    public override bool Equals(object obj) => obj != null && obj is MySprite other && this.Equals(other);

    public override int GetHashCode() => (int) ((TextAlignment) (((((((int) this.Type * 397 ^ this.Position.GetHashCode()) * 397 ^ this.Size.GetHashCode()) * 397 ^ this.Color.GetHashCode()) * 397 ^ (this.Data != null ? StringComparer.InvariantCulture.GetHashCode(this.Data) : 0)) * 397 ^ (this.FontId != null ? StringComparer.InvariantCulture.GetHashCode(this.FontId) : 0)) * 397) ^ this.Alignment) * 397 ^ this.RotationOrScale.GetHashCode();

    private bool AreStringsEqual(string lhs, string rhs) => string.IsNullOrEmpty(lhs) && string.IsNullOrEmpty(rhs) || string.Equals(lhs, rhs, StringComparison.InvariantCulture);

    protected class VRage_Game_GUI_TextPanel_MySprite\u003C\u003EType\u003C\u003EAccessor : IMemberAccessor<MySprite, SpriteType>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySprite owner, in SpriteType value) => owner.Type = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySprite owner, out SpriteType value) => value = owner.Type;
    }

    protected class VRage_Game_GUI_TextPanel_MySprite\u003C\u003EPosition\u003C\u003EAccessor : IMemberAccessor<MySprite, Vector2?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySprite owner, in Vector2? value) => owner.Position = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySprite owner, out Vector2? value) => value = owner.Position;
    }

    protected class VRage_Game_GUI_TextPanel_MySprite\u003C\u003ESize\u003C\u003EAccessor : IMemberAccessor<MySprite, Vector2?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySprite owner, in Vector2? value) => owner.Size = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySprite owner, out Vector2? value) => value = owner.Size;
    }

    protected class VRage_Game_GUI_TextPanel_MySprite\u003C\u003EColor\u003C\u003EAccessor : IMemberAccessor<MySprite, VRageMath.Color?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySprite owner, in VRageMath.Color? value) => owner.Color = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySprite owner, out VRageMath.Color? value) => value = owner.Color;
    }

    protected class VRage_Game_GUI_TextPanel_MySprite\u003C\u003EData\u003C\u003EAccessor : IMemberAccessor<MySprite, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySprite owner, in string value) => owner.Data = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySprite owner, out string value) => value = owner.Data;
    }

    protected class VRage_Game_GUI_TextPanel_MySprite\u003C\u003EFontId\u003C\u003EAccessor : IMemberAccessor<MySprite, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySprite owner, in string value) => owner.FontId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySprite owner, out string value) => value = owner.FontId;
    }

    protected class VRage_Game_GUI_TextPanel_MySprite\u003C\u003EAlignment\u003C\u003EAccessor : IMemberAccessor<MySprite, TextAlignment>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySprite owner, in TextAlignment value) => owner.Alignment = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySprite owner, out TextAlignment value) => value = owner.Alignment;
    }

    protected class VRage_Game_GUI_TextPanel_MySprite\u003C\u003ERotationOrScale\u003C\u003EAccessor : IMemberAccessor<MySprite, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MySprite owner, in float value) => owner.RotationOrScale = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MySprite owner, out float value) => value = owner.RotationOrScale;
    }

    private class VRage_Game_GUI_TextPanel_MySprite\u003C\u003EActor : IActivator, IActivator<MySprite>
    {
      object IActivator.CreateInstance() => (object) new MySprite();

      MySprite IActivator<MySprite>.CreateInstance() => new MySprite();
    }
  }
}
