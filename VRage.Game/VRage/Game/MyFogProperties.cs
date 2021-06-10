// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyFogProperties
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRageMath;

namespace VRage.Game
{
  public struct MyFogProperties
  {
    [StructDefault]
    public static readonly MyFogProperties Default = new MyFogProperties()
    {
      FogMultiplier = 0.13f,
      FogDensity = 3f / 1000f,
      FogColor = MyFogProperties.Defaults.FogColor,
      FogSkybox = 0.0f,
      FogAtmo = 0.0f
    };
    public float FogMultiplier;
    public float FogDensity;
    public Vector3 FogColor;
    public float FogSkybox;
    public float FogAtmo;

    public MyFogProperties Lerp(ref MyFogProperties target, float ratio) => new MyFogProperties()
    {
      FogMultiplier = MathHelper.Lerp(this.FogMultiplier, target.FogMultiplier, ratio),
      FogDensity = MathHelper.Lerp(this.FogDensity, target.FogDensity, ratio),
      FogColor = Vector3.Lerp(this.FogColor, target.FogColor, ratio),
      FogSkybox = MathHelper.Lerp(this.FogSkybox, target.FogSkybox, ratio),
      FogAtmo = MathHelper.Lerp(this.FogAtmo, target.FogAtmo, ratio)
    };

    public override bool Equals(object obj)
    {
      MyFogProperties myFogProperties = (MyFogProperties) obj;
      return MathHelper.IsEqual(this.FogMultiplier, myFogProperties.FogMultiplier) && MathHelper.IsEqual(this.FogDensity, myFogProperties.FogDensity) && (MathHelper.IsEqual(this.FogColor, myFogProperties.FogColor) && MathHelper.IsEqual(this.FogSkybox, myFogProperties.FogSkybox)) && MathHelper.IsEqual(this.FogAtmo, myFogProperties.FogAtmo);
    }

    private static class Defaults
    {
      public static readonly Vector3 FogColor = new Vector3(0.0f, 0.0f, 0.0f);
      public const float FogMultiplier = 0.13f;
      public const float FogDensity = 0.003f;
      public const float FogSkybox = 0.0f;
      public const float FogAtmo = 0.0f;
    }
  }
}
