﻿// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_TransparentMaterialDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Data;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_TransparentMaterialDefinition : MyObjectBuilder_DefinitionBase
  {
    [ProtoMember(1)]
    [ModdableContentFile("dds")]
    public string Texture;
    [ProtoMember(4)]
    [ModdableContentFile("dds")]
    public string GlossTexture;
    [ProtoMember(7)]
    public MyTransparentMaterialTextureType TextureType;
    [ProtoMember(10)]
    public bool CanBeAffectedByOtherLights;
    [ProtoMember(13)]
    public bool AlphaMistingEnable;
    [ProtoMember(16)]
    public bool UseAtlas;
    [ProtoMember(19)]
    public float AlphaMistingStart;
    [ProtoMember(22)]
    public float AlphaMistingEnd;
    [ProtoMember(25)]
    public float SoftParticleDistanceScale;
    [ProtoMember(28)]
    public float AlphaSaturation;
    [ProtoMember(31)]
    public Vector4 Color = Vector4.One;
    [ProtoMember(34)]
    public Vector4 ColorAdd = Vector4.Zero;
    [ProtoMember(37)]
    public Vector4 ShadowMultiplier = Vector4.Zero;
    [ProtoMember(40)]
    public Vector4 LightMultiplier = Vector4.One * 0.1f;
    [ProtoMember(43)]
    public float Reflectivity = 0.6f;
    [ProtoMember(46)]
    public float Fresnel = 1f;
    [ProtoMember(49)]
    public float ReflectionShadow = 0.1f;
    [ProtoMember(52)]
    public float Gloss = 0.4f;
    [ProtoMember(55)]
    public float GlossTextureAdd = 0.55f;
    [ProtoMember(58)]
    public float SpecularColorFactor = 20f;
    [ProtoMember(61)]
    public bool IsFlareOccluder;
    [ProtoMember(62)]
    public bool TriangleFaceCulling = true;
    [ProtoMember(64)]
    public bool AlphaCutout;
    [ProtoMember(67)]
    public Vector2I TargetSize = new Vector2I(-1, -1);

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003ETexture\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in string value)
      {
        owner.Texture = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out string value)
      {
        value = owner.Texture;
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003EGlossTexture\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in string value)
      {
        owner.GlossTexture = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out string value)
      {
        value = owner.GlossTexture;
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003ETextureType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, MyTransparentMaterialTextureType>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in MyTransparentMaterialTextureType value)
      {
        owner.TextureType = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out MyTransparentMaterialTextureType value)
      {
        value = owner.TextureType;
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003ECanBeAffectedByOtherLights\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in bool value)
      {
        owner.CanBeAffectedByOtherLights = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out bool value)
      {
        value = owner.CanBeAffectedByOtherLights;
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003EAlphaMistingEnable\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in bool value)
      {
        owner.AlphaMistingEnable = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out bool value)
      {
        value = owner.AlphaMistingEnable;
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003EUseAtlas\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in bool value)
      {
        owner.UseAtlas = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out bool value)
      {
        value = owner.UseAtlas;
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003EAlphaMistingStart\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in float value)
      {
        owner.AlphaMistingStart = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out float value)
      {
        value = owner.AlphaMistingStart;
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003EAlphaMistingEnd\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in float value)
      {
        owner.AlphaMistingEnd = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out float value)
      {
        value = owner.AlphaMistingEnd;
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003ESoftParticleDistanceScale\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in float value)
      {
        owner.SoftParticleDistanceScale = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out float value)
      {
        value = owner.SoftParticleDistanceScale;
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003EAlphaSaturation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in float value)
      {
        owner.AlphaSaturation = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out float value)
      {
        value = owner.AlphaSaturation;
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003EColor\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, Vector4>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in Vector4 value)
      {
        owner.Color = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out Vector4 value)
      {
        value = owner.Color;
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003EColorAdd\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, Vector4>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in Vector4 value)
      {
        owner.ColorAdd = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out Vector4 value)
      {
        value = owner.ColorAdd;
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003EShadowMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, Vector4>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in Vector4 value)
      {
        owner.ShadowMultiplier = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out Vector4 value)
      {
        value = owner.ShadowMultiplier;
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003ELightMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, Vector4>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in Vector4 value)
      {
        owner.LightMultiplier = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out Vector4 value)
      {
        value = owner.LightMultiplier;
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003EReflectivity\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in float value)
      {
        owner.Reflectivity = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out float value)
      {
        value = owner.Reflectivity;
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003EFresnel\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in float value)
      {
        owner.Fresnel = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out float value)
      {
        value = owner.Fresnel;
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003EReflectionShadow\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in float value)
      {
        owner.ReflectionShadow = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out float value)
      {
        value = owner.ReflectionShadow;
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003EGloss\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in float value)
      {
        owner.Gloss = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out float value)
      {
        value = owner.Gloss;
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003EGlossTextureAdd\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in float value)
      {
        owner.GlossTextureAdd = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out float value)
      {
        value = owner.GlossTextureAdd;
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003ESpecularColorFactor\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in float value)
      {
        owner.SpecularColorFactor = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out float value)
      {
        value = owner.SpecularColorFactor;
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003EIsFlareOccluder\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in bool value)
      {
        owner.IsFlareOccluder = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out bool value)
      {
        value = owner.IsFlareOccluder;
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003ETriangleFaceCulling\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in bool value)
      {
        owner.TriangleFaceCulling = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out bool value)
      {
        value = owner.TriangleFaceCulling;
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003EAlphaCutout\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in bool value)
      {
        owner.AlphaCutout = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out bool value)
      {
        value = owner.AlphaCutout;
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003ETargetSize\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, Vector2I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in Vector2I value)
      {
        owner.TargetSize = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out Vector2I value)
      {
        value = owner.TargetSize;
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TransparentMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TransparentMaterialDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_MyObjectBuilder_TransparentMaterialDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_TransparentMaterialDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_TransparentMaterialDefinition();

      MyObjectBuilder_TransparentMaterialDefinition IActivator<MyObjectBuilder_TransparentMaterialDefinition>.CreateInstance() => new MyObjectBuilder_TransparentMaterialDefinition();
    }
  }
}
