// Decompiled with JetBrains decompiler
// Type: VRage.Game.MySunProperties
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Xml.Serialization;
using VRageMath;
using VRageRender;

namespace VRage.Game
{
  public struct MySunProperties
  {
    [StructDefault]
    public static readonly MySunProperties Default = new MySunProperties()
    {
      SunIntensity = 1f,
      EnvironmentLight = MyEnvironmentLightData.Default,
      EnvironmentProbe = MyEnvironmentProbeData.Default,
      SunDirectionNormalized = MySunProperties.Defaults.SunDirectionNormalized,
      BaseSunDirectionNormalized = MySunProperties.Defaults.BaseSunDirectionNormalized,
      TextureMultipliers = MyTextureDebugMultipliers.Defaults,
      EnvMapResolution = 512,
      EnvMapFilteredResolution = 256
    };
    public float SunIntensity;
    [XmlElement(Type = typeof (MyStructXmlSerializer<MyEnvironmentLightData>))]
    public MyEnvironmentLightData EnvironmentLight;
    [XmlElement(Type = typeof (MyStructXmlSerializer<MyEnvironmentProbeData>))]
    public MyEnvironmentProbeData EnvironmentProbe;
    [XmlElement(Type = typeof (MyStructXmlSerializer<MyTextureDebugMultipliers>))]
    public MyTextureDebugMultipliers TextureMultipliers;
    public Vector3 BaseSunDirectionNormalized;
    public Vector3 SunDirectionNormalized;
    public int EnvMapResolution;
    public int EnvMapFilteredResolution;

    public MyEnvironmentData EnvironmentData
    {
      get
      {
        MyEnvironmentData myEnvironmentData = new MyEnvironmentData()
        {
          EnvironmentLight = this.EnvironmentLight,
          EnvironmentProbe = this.EnvironmentProbe,
          TextureMultipliers = this.TextureMultipliers,
          EnvMapResolution = this.EnvMapResolution,
          EnvMapFilteredResolution = this.EnvMapFilteredResolution
        };
        myEnvironmentData.EnvironmentLight.SunColorRaw = this.EnvironmentLight.SunColorRaw * this.SunIntensity;
        return myEnvironmentData;
      }
    }

    public Vector3 SunRotationAxis
    {
      get
      {
        Vector3 vector3 = (double) Math.Abs(Vector3.Dot(this.BaseSunDirectionNormalized, Vector3.Up)) <= 0.949999988079071 ? Vector3.Cross(Vector3.Cross(this.BaseSunDirectionNormalized, Vector3.Up), this.BaseSunDirectionNormalized) : Vector3.Cross(Vector3.Cross(this.BaseSunDirectionNormalized, Vector3.Left), this.BaseSunDirectionNormalized);
        double num = (double) vector3.Normalize();
        return vector3;
      }
    }

    private static class Defaults
    {
      public const float SunIntensity = 1f;
      public static readonly Vector3 SunDirectionNormalized = new Vector3(0.3394673f, 0.7097954f, -0.6172134f);
      public static readonly Vector3 BaseSunDirectionNormalized = new Vector3(0.3394673f, 0.7097954f, -0.6172134f);
      public const int EnvMapResolution = 512;
      public const int EnvMapFilteredResolution = 256;
    }
  }
}
