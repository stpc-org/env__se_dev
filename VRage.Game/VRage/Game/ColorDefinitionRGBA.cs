// Decompiled with JetBrains decompiler
// Type: VRage.Game.ColorDefinitionRGBA
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public struct ColorDefinitionRGBA
  {
    [XmlAttribute]
    [ProtoMember(1)]
    public byte R;
    [ProtoMember(4)]
    [XmlAttribute]
    public byte G;
    [ProtoMember(7)]
    [XmlAttribute]
    public byte B;
    [ProtoMember(10)]
    [XmlAttribute]
    public byte A;

    [XmlAttribute]
    public byte Red
    {
      get => this.R;
      set => this.R = value;
    }

    [XmlAttribute]
    public byte Green
    {
      get => this.G;
      set => this.G = value;
    }

    [XmlAttribute]
    public byte Blue
    {
      get => this.B;
      set => this.B = value;
    }

    [XmlAttribute]
    public byte Alpha
    {
      get => this.A;
      set => this.A = value;
    }

    [XmlAttribute]
    public string Hex
    {
      get => this.GetHex();
      set => this.SetHex(value);
    }

    public bool ShouldSerializeRed() => false;

    public bool ShouldSerializeGreen() => false;

    public bool ShouldSerializeBlue() => false;

    public bool ShouldSerializeAlpha() => false;

    public bool ShouldSerializeHex() => false;

    private string GetHex() => string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", (object) this.A, (object) this.R, (object) this.G, (object) this.B);

    private void SetHex(string hex)
    {
      if (string.IsNullOrEmpty(hex))
        return;
      hex = hex.Trim(' ', '#');
      uint result;
      uint.TryParse(hex, NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture, out result);
      if (hex.Length < 8)
        result |= 4278190080U;
      this.A = (byte) ((4278190080U & result) >> 24);
      this.R = (byte) ((16711680U & result) >> 16);
      this.G = (byte) ((65280U & result) >> 8);
      this.B = (byte) ((uint) byte.MaxValue & result);
    }

    public ColorDefinitionRGBA(string hex)
      : this((byte) 0, (byte) 0, (byte) 0)
      => this.Hex = hex;

    public ColorDefinitionRGBA(byte red, byte green, byte blue, byte alpha = 255)
    {
      this.R = red;
      this.G = green;
      this.B = blue;
      this.A = alpha;
    }

    public static implicit operator Color(ColorDefinitionRGBA definitionRgba) => new Color((int) definitionRgba.R, (int) definitionRgba.G, (int) definitionRgba.B, (int) definitionRgba.A);

    public static implicit operator ColorDefinitionRGBA(Color color) => new ColorDefinitionRGBA()
    {
      A = color.A,
      B = color.B,
      G = color.G,
      R = color.R
    };

    public static implicit operator Vector4(ColorDefinitionRGBA definitionRgba) => new Vector4((float) definitionRgba.R / (float) byte.MaxValue, (float) definitionRgba.G / (float) byte.MaxValue, (float) definitionRgba.B / (float) byte.MaxValue, (float) definitionRgba.A / (float) byte.MaxValue);

    public static implicit operator ColorDefinitionRGBA(Vector4 vector) => new ColorDefinitionRGBA()
    {
      A = (byte) ((double) vector.W * (double) byte.MaxValue),
      B = (byte) ((double) vector.Z * (double) byte.MaxValue),
      G = (byte) ((double) vector.Y * (double) byte.MaxValue),
      R = (byte) ((double) vector.X * (double) byte.MaxValue)
    };

    public override string ToString() => string.Format("R:{0} G:{1} B:{2} A:{3}", (object) this.R, (object) this.G, (object) this.B, (object) this.A);

    protected class VRage_Game_ColorDefinitionRGBA\u003C\u003ER\u003C\u003EAccessor : IMemberAccessor<ColorDefinitionRGBA, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ColorDefinitionRGBA owner, in byte value) => owner.R = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ColorDefinitionRGBA owner, out byte value) => value = owner.R;
    }

    protected class VRage_Game_ColorDefinitionRGBA\u003C\u003EG\u003C\u003EAccessor : IMemberAccessor<ColorDefinitionRGBA, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ColorDefinitionRGBA owner, in byte value) => owner.G = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ColorDefinitionRGBA owner, out byte value) => value = owner.G;
    }

    protected class VRage_Game_ColorDefinitionRGBA\u003C\u003EB\u003C\u003EAccessor : IMemberAccessor<ColorDefinitionRGBA, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ColorDefinitionRGBA owner, in byte value) => owner.B = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ColorDefinitionRGBA owner, out byte value) => value = owner.B;
    }

    protected class VRage_Game_ColorDefinitionRGBA\u003C\u003EA\u003C\u003EAccessor : IMemberAccessor<ColorDefinitionRGBA, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ColorDefinitionRGBA owner, in byte value) => owner.A = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ColorDefinitionRGBA owner, out byte value) => value = owner.A;
    }

    protected class VRage_Game_ColorDefinitionRGBA\u003C\u003ERed\u003C\u003EAccessor : IMemberAccessor<ColorDefinitionRGBA, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ColorDefinitionRGBA owner, in byte value) => owner.Red = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ColorDefinitionRGBA owner, out byte value) => value = owner.Red;
    }

    protected class VRage_Game_ColorDefinitionRGBA\u003C\u003EGreen\u003C\u003EAccessor : IMemberAccessor<ColorDefinitionRGBA, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ColorDefinitionRGBA owner, in byte value) => owner.Green = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ColorDefinitionRGBA owner, out byte value) => value = owner.Green;
    }

    protected class VRage_Game_ColorDefinitionRGBA\u003C\u003EBlue\u003C\u003EAccessor : IMemberAccessor<ColorDefinitionRGBA, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ColorDefinitionRGBA owner, in byte value) => owner.Blue = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ColorDefinitionRGBA owner, out byte value) => value = owner.Blue;
    }

    protected class VRage_Game_ColorDefinitionRGBA\u003C\u003EAlpha\u003C\u003EAccessor : IMemberAccessor<ColorDefinitionRGBA, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ColorDefinitionRGBA owner, in byte value) => owner.Alpha = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ColorDefinitionRGBA owner, out byte value) => value = owner.Alpha;
    }

    protected class VRage_Game_ColorDefinitionRGBA\u003C\u003EHex\u003C\u003EAccessor : IMemberAccessor<ColorDefinitionRGBA, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref ColorDefinitionRGBA owner, in string value) => owner.Hex = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref ColorDefinitionRGBA owner, out string value) => value = owner.Hex;
    }

    private class VRage_Game_ColorDefinitionRGBA\u003C\u003EActor : IActivator, IActivator<ColorDefinitionRGBA>
    {
      object IActivator.CreateInstance() => (object) new ColorDefinitionRGBA();

      ColorDefinitionRGBA IActivator<ColorDefinitionRGBA>.CreateInstance() => new ColorDefinitionRGBA();
    }
  }
}
