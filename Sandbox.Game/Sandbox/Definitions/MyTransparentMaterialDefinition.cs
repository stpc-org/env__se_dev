// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyTransparentMaterialDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;
using VRageRender;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_TransparentMaterialDefinition), null)]
  public class MyTransparentMaterialDefinition : MyDefinitionBase
  {
    public string Texture;
    public string GlossTexture;
    public MyTransparentMaterialTextureType TextureType;
    public bool CanBeAffectedByLights;
    public bool AlphaMistingEnable;
    public bool UseAtlas;
    public float AlphaMistingStart;
    public float AlphaMistingEnd;
    public float SoftParticleDistanceScale;
    public float AlphaSaturation;
    public float Reflectivity;
    public float Fresnel;
    public bool IsFlareOccluder;
    public bool TriangleFaceCulling;
    public Vector4 Color = Vector4.One;
    public Vector4 ColorAdd = Vector4.Zero;
    public Vector4 ShadowMultiplier = Vector4.Zero;
    public Vector4 LightMultiplier = Vector4.One * 0.1f;
    public bool AlphaCutout;
    public Vector2I TargetSize;
    public float ReflectionShadow = 0.1f;
    public float Gloss = 0.4f;
    public float GlossTextureAdd = 0.55f;
    public float SpecularColorFactor = 20f;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_TransparentMaterialDefinition materialDefinition = builder as MyObjectBuilder_TransparentMaterialDefinition;
      this.Texture = materialDefinition.Texture;
      this.GlossTexture = materialDefinition.GlossTexture;
      if (this.Texture == null)
        this.Texture = string.Empty;
      this.TextureType = materialDefinition.TextureType;
      this.CanBeAffectedByLights = materialDefinition.CanBeAffectedByOtherLights;
      this.AlphaMistingEnable = materialDefinition.AlphaMistingEnable;
      this.UseAtlas = materialDefinition.UseAtlas;
      this.AlphaMistingStart = materialDefinition.AlphaMistingStart;
      this.AlphaMistingEnd = materialDefinition.AlphaMistingEnd;
      this.SoftParticleDistanceScale = materialDefinition.SoftParticleDistanceScale;
      this.AlphaSaturation = materialDefinition.AlphaSaturation;
      this.Reflectivity = materialDefinition.Reflectivity;
      this.Fresnel = materialDefinition.Fresnel;
      this.IsFlareOccluder = materialDefinition.IsFlareOccluder;
      this.TriangleFaceCulling = materialDefinition.TriangleFaceCulling;
      this.Color = materialDefinition.Color;
      this.ColorAdd = materialDefinition.ColorAdd;
      this.ShadowMultiplier = materialDefinition.ShadowMultiplier;
      this.LightMultiplier = materialDefinition.LightMultiplier;
      this.AlphaCutout = materialDefinition.AlphaCutout;
      this.TargetSize = materialDefinition.TargetSize;
      this.ReflectionShadow = materialDefinition.ReflectionShadow;
      this.Gloss = materialDefinition.Gloss;
      this.GlossTextureAdd = materialDefinition.GlossTextureAdd;
      this.SpecularColorFactor = materialDefinition.SpecularColorFactor;
    }

    private class Sandbox_Definitions_MyTransparentMaterialDefinition\u003C\u003EActor : IActivator, IActivator<MyTransparentMaterialDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyTransparentMaterialDefinition();

      MyTransparentMaterialDefinition IActivator<MyTransparentMaterialDefinition>.CreateInstance() => new MyTransparentMaterialDefinition();
    }
  }
}
