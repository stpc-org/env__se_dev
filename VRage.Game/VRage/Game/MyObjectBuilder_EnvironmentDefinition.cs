// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_EnvironmentDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Data;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace VRage.Game
{
  [MyObjectBuilderDefinition(null, null)]
  [XmlType("EnvironmentDefinition")]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_EnvironmentDefinition : MyObjectBuilder_DefinitionBase
  {
    [XmlElement(Type = typeof (MyStructXmlSerializer<MyFogProperties>))]
    public MyFogProperties FogProperties = MyFogProperties.Default;
    [XmlElement(Type = typeof (MyStructXmlSerializer<MyPlanetProperties>))]
    public MyPlanetProperties PlanetProperties = MyPlanetProperties.Default;
    [XmlElement(Type = typeof (MyStructXmlSerializer<MySunProperties>))]
    public MySunProperties SunProperties = MySunProperties.Default;
    [XmlElement(Type = typeof (MyStructXmlSerializer<MyPostprocessSettings>))]
    public MyPostprocessSettings PostProcessSettings = MyPostprocessSettings.Default;
    [XmlElement(Type = typeof (MyStructXmlSerializer<MySSAOSettings>))]
    public MySSAOSettings SSAOSettings = MySSAOSettings.Default;
    [XmlElement(Type = typeof (MyStructXmlSerializer<MyHBAOData>))]
    public MyHBAOData HBAOSettings = MyHBAOData.Default;
    public MyShadowsSettings ShadowSettings = new MyShadowsSettings();
    public MyNewLoddingSettings LowLoddingSettings = new MyNewLoddingSettings();
    public MyNewLoddingSettings MediumLoddingSettings = new MyNewLoddingSettings();
    public MyNewLoddingSettings HighLoddingSettings = new MyNewLoddingSettings();
    public MyNewLoddingSettings ExtremeLoddingSettings = new MyNewLoddingSettings();
    [ProtoMember(31)]
    [XmlArrayItem("ParticleType")]
    public List<MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings> EnvironmentalParticles = new List<MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings>();
    public float SmallShipMaxSpeed = 100f;
    public float LargeShipMaxSpeed = 100f;
    public float SmallShipMaxAngularSpeed = 36000f;
    public float LargeShipMaxAngularSpeed = 18000f;
    public Vector4 ContourHighlightColor = MyObjectBuilder_EnvironmentDefinition.Defaults.ContourHighlightColor;
    public Vector4 ContourHighlightColorAccessDenied = MyObjectBuilder_EnvironmentDefinition.Defaults.ContourHighlightColorAccessDenied;
    public float ContourHighlightThickness = 5f;
    public float HighlightPulseInSeconds;
    [ModdableContentFile("dds")]
    public string EnvironmentTexture = "Textures\\BackgroundCube\\Final\\BackgroundCube.dds";
    public MyOrientation EnvironmentOrientation = MyObjectBuilder_EnvironmentDefinition.Defaults.EnvironmentOrientation;

    [ProtoContract]
    public struct EnvironmentalParticleSettings
    {
      [ProtoMember(1)]
      public SerializableDefinitionId Id;
      [ProtoMember(4)]
      public string Material;
      [ProtoMember(7)]
      public Vector4 Color;
      [ProtoMember(10)]
      public string MaterialPlanet;
      [ProtoMember(13)]
      public Vector4 ColorPlanet;
      [ProtoMember(16)]
      public float MaxSpawnDistance;
      [ProtoMember(19)]
      public float DespawnDistance;
      [ProtoMember(22)]
      public float Density;
      [ProtoMember(25)]
      public int MaxLifeTime;
      [ProtoMember(28)]
      public int MaxParticles;

      protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EEnvironmentalParticleSettings\u003C\u003EId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings, SerializableDefinitionId>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings owner,
          in SerializableDefinitionId value)
        {
          owner.Id = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings owner,
          out SerializableDefinitionId value)
        {
          value = owner.Id;
        }
      }

      protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EEnvironmentalParticleSettings\u003C\u003EMaterial\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings owner,
          in string value)
        {
          owner.Material = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings owner,
          out string value)
        {
          value = owner.Material;
        }
      }

      protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EEnvironmentalParticleSettings\u003C\u003EColor\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings, Vector4>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings owner,
          in Vector4 value)
        {
          owner.Color = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings owner,
          out Vector4 value)
        {
          value = owner.Color;
        }
      }

      protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EEnvironmentalParticleSettings\u003C\u003EMaterialPlanet\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings owner,
          in string value)
        {
          owner.MaterialPlanet = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings owner,
          out string value)
        {
          value = owner.MaterialPlanet;
        }
      }

      protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EEnvironmentalParticleSettings\u003C\u003EColorPlanet\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings, Vector4>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings owner,
          in Vector4 value)
        {
          owner.ColorPlanet = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings owner,
          out Vector4 value)
        {
          value = owner.ColorPlanet;
        }
      }

      protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EEnvironmentalParticleSettings\u003C\u003EMaxSpawnDistance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings owner,
          in float value)
        {
          owner.MaxSpawnDistance = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings owner,
          out float value)
        {
          value = owner.MaxSpawnDistance;
        }
      }

      protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EEnvironmentalParticleSettings\u003C\u003EDespawnDistance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings owner,
          in float value)
        {
          owner.DespawnDistance = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings owner,
          out float value)
        {
          value = owner.DespawnDistance;
        }
      }

      protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EEnvironmentalParticleSettings\u003C\u003EDensity\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings owner,
          in float value)
        {
          owner.Density = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings owner,
          out float value)
        {
          value = owner.Density;
        }
      }

      protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EEnvironmentalParticleSettings\u003C\u003EMaxLifeTime\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings owner,
          in int value)
        {
          owner.MaxLifeTime = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings owner,
          out int value)
        {
          value = owner.MaxLifeTime;
        }
      }

      protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EEnvironmentalParticleSettings\u003C\u003EMaxParticles\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings owner,
          in int value)
        {
          owner.MaxParticles = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings owner,
          out int value)
        {
          value = owner.MaxParticles;
        }
      }

      private class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EEnvironmentalParticleSettings\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings();

        MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings IActivator<MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings>.CreateInstance() => new MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings();
      }
    }

    public static class Defaults
    {
      public const float SmallShipMaxSpeed = 100f;
      public const float LargeShipMaxSpeed = 100f;
      public const float SmallShipMaxAngularSpeed = 36000f;
      public const float LargeShipMaxAngularSpeed = 18000f;
      public static readonly Vector4 ContourHighlightColor = new Vector4(1f, 1f, 0.0f, 0.05f);
      public static readonly Vector4 ContourHighlightColorAccessDenied = new Vector4(1f, 0.0f, 0.0f, 0.05f);
      public const float ContourHighlightThickness = 5f;
      public const float HighlightPulseInSeconds = 0.0f;
      public const string EnvironmentTexture = "Textures\\BackgroundCube\\Final\\BackgroundCube.dds";
      public const string ScaryFaceTexture = "Textures\\BackgroundCube\\Final\\BackgroundCube_ScaryFace.dds";
      public static readonly DateTime ScaryFaceFrom = new DateTime(2019, 10, 25);
      public static readonly DateTime ScaryFaceTo = new DateTime(2019, 11, 1);
      public const string Christmas2019Texture = "Textures\\BackgroundCube\\Final\\BackgroundCube_Christmas.dds";
      public static readonly DateTime Christmas2019From = new DateTime(2019, 12, 19);
      public static readonly DateTime Christmas2019To = new DateTime(2020, 1, 3);
      public static readonly MyOrientation EnvironmentOrientation = new MyOrientation(MathHelper.ToRadians(60.39555f), MathHelper.ToRadians(-61.1862f), MathHelper.ToRadians(90.90578f));
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EFogProperties\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, MyFogProperties>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentDefinition owner,
        in MyFogProperties value)
      {
        owner.FogProperties = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentDefinition owner,
        out MyFogProperties value)
      {
        value = owner.FogProperties;
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EPlanetProperties\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, MyPlanetProperties>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentDefinition owner,
        in MyPlanetProperties value)
      {
        owner.PlanetProperties = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentDefinition owner,
        out MyPlanetProperties value)
      {
        value = owner.PlanetProperties;
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003ESunProperties\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, MySunProperties>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentDefinition owner,
        in MySunProperties value)
      {
        owner.SunProperties = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentDefinition owner,
        out MySunProperties value)
      {
        value = owner.SunProperties;
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EPostProcessSettings\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, MyPostprocessSettings>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentDefinition owner,
        in MyPostprocessSettings value)
      {
        owner.PostProcessSettings = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentDefinition owner,
        out MyPostprocessSettings value)
      {
        value = owner.PostProcessSettings;
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003ESSAOSettings\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, MySSAOSettings>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentDefinition owner,
        in MySSAOSettings value)
      {
        owner.SSAOSettings = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentDefinition owner,
        out MySSAOSettings value)
      {
        value = owner.SSAOSettings;
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EHBAOSettings\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, MyHBAOData>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in MyHBAOData value) => owner.HBAOSettings = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out MyHBAOData value) => value = owner.HBAOSettings;
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EShadowSettings\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, MyShadowsSettings>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentDefinition owner,
        in MyShadowsSettings value)
      {
        owner.ShadowSettings = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentDefinition owner,
        out MyShadowsSettings value)
      {
        value = owner.ShadowSettings;
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003ELowLoddingSettings\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, MyNewLoddingSettings>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentDefinition owner,
        in MyNewLoddingSettings value)
      {
        owner.LowLoddingSettings = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentDefinition owner,
        out MyNewLoddingSettings value)
      {
        value = owner.LowLoddingSettings;
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EMediumLoddingSettings\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, MyNewLoddingSettings>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentDefinition owner,
        in MyNewLoddingSettings value)
      {
        owner.MediumLoddingSettings = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentDefinition owner,
        out MyNewLoddingSettings value)
      {
        value = owner.MediumLoddingSettings;
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EHighLoddingSettings\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, MyNewLoddingSettings>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentDefinition owner,
        in MyNewLoddingSettings value)
      {
        owner.HighLoddingSettings = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentDefinition owner,
        out MyNewLoddingSettings value)
      {
        value = owner.HighLoddingSettings;
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EExtremeLoddingSettings\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, MyNewLoddingSettings>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentDefinition owner,
        in MyNewLoddingSettings value)
      {
        owner.ExtremeLoddingSettings = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentDefinition owner,
        out MyNewLoddingSettings value)
      {
        value = owner.ExtremeLoddingSettings;
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EEnvironmentalParticles\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, List<MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentDefinition owner,
        in List<MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings> value)
      {
        owner.EnvironmentalParticles = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentDefinition owner,
        out List<MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings> value)
      {
        value = owner.EnvironmentalParticles;
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003ESmallShipMaxSpeed\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in float value) => owner.SmallShipMaxSpeed = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out float value) => value = owner.SmallShipMaxSpeed;
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003ELargeShipMaxSpeed\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in float value) => owner.LargeShipMaxSpeed = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out float value) => value = owner.LargeShipMaxSpeed;
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003ESmallShipMaxAngularSpeed\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in float value) => owner.SmallShipMaxAngularSpeed = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out float value) => value = owner.SmallShipMaxAngularSpeed;
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003ELargeShipMaxAngularSpeed\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in float value) => owner.LargeShipMaxAngularSpeed = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out float value) => value = owner.LargeShipMaxAngularSpeed;
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EContourHighlightColor\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, Vector4>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in Vector4 value) => owner.ContourHighlightColor = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out Vector4 value) => value = owner.ContourHighlightColor;
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EContourHighlightColorAccessDenied\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, Vector4>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in Vector4 value) => owner.ContourHighlightColorAccessDenied = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out Vector4 value) => value = owner.ContourHighlightColorAccessDenied;
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EContourHighlightThickness\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in float value) => owner.ContourHighlightThickness = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out float value) => value = owner.ContourHighlightThickness;
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EHighlightPulseInSeconds\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in float value) => owner.HighlightPulseInSeconds = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out float value) => value = owner.HighlightPulseInSeconds;
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EEnvironmentTexture\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in string value) => owner.EnvironmentTexture = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out string value) => value = owner.EnvironmentTexture;
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EEnvironmentOrientation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, MyOrientation>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentDefinition owner,
        in MyOrientation value)
      {
        owner.EnvironmentOrientation = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentDefinition owner,
        out MyOrientation value)
      {
        value = owner.EnvironmentOrientation;
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_EnvironmentDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_EnvironmentDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_EnvironmentDefinition();

      MyObjectBuilder_EnvironmentDefinition IActivator<MyObjectBuilder_EnvironmentDefinition>.CreateInstance() => new MyObjectBuilder_EnvironmentDefinition();
    }
  }
}
