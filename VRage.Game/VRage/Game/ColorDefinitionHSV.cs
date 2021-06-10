// Decompiled with JetBrains decompiler
// Type: VRage.Game.ColorDefinitionHSV
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Xml.Serialization;
using VRageMath;

namespace VRage.Game
{
  public struct ColorDefinitionHSV
  {
    [XmlAttribute]
    public int H;
    [XmlAttribute]
    public int S;
    [XmlAttribute]
    public int V;

    [XmlAttribute]
    public int Hue
    {
      get => this.H;
      set => this.H = value;
    }

    [XmlAttribute]
    public int Saturation
    {
      get => this.S;
      set => this.S = value;
    }

    [XmlAttribute]
    public int Value
    {
      get => this.V;
      set => this.V = value;
    }

    public bool ShouldSerializeHue() => false;

    public bool ShouldSerializeSaturation() => false;

    public bool ShouldSerializeValue() => false;

    public bool IsValid() => this.H >= 0 && this.H <= 360 && (this.S >= -100 && this.S <= 100) && this.V >= -100 && this.V <= 100;

    public static implicit operator Vector3(ColorDefinitionHSV definition)
    {
      definition.H %= 360;
      if (definition.H < 0)
        definition.H += 360;
      return new Vector3((float) definition.H / 360f, MathHelper.Clamp((float) definition.S / 100f, -1f, 1f), MathHelper.Clamp((float) definition.V / 100f, -1f, 1f));
    }

    public static implicit operator ColorDefinitionHSV(Vector3 vector) => new ColorDefinitionHSV()
    {
      H = (int) MathHelper.Clamp(vector.Z * 100f, -100f, 100f),
      S = (int) MathHelper.Clamp(vector.Y * 100f, -100f, 100f),
      V = (int) MathHelper.Clamp(vector.Z * 360f, 0.0f, 360f)
    };

    public override string ToString() => string.Format("H:{0} S:{1} V:{2}", (object) this.H, (object) this.S, (object) this.V);
  }
}
