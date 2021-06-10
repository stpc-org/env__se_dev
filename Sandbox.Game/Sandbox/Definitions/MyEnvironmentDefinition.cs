// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyEnvironmentDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_EnvironmentDefinition), typeof (MyEnvironmentDefinition.Postprocessor))]
  public class MyEnvironmentDefinition : MyDefinitionBase
  {
    public MyPlanetProperties PlanetProperties = MyPlanetProperties.Default;
    public MyFogProperties FogProperties = MyFogProperties.Default;
    public MySunProperties SunProperties = MySunProperties.Default;
    public MyPostprocessSettings PostProcessSettings = MyPostprocessSettings.Default;
    public MySSAOSettings SSAOSettings = MySSAOSettings.Default;
    public MyHBAOData HBAOSettings = MyHBAOData.Default;
    public float LargeShipMaxSpeed = 100f;
    public float SmallShipMaxSpeed = 100f;
    public Color ContourHighlightColor = (Color) MyObjectBuilder_EnvironmentDefinition.Defaults.ContourHighlightColor;
    public Color ContourHighlightColorAccessDenied = (Color) MyObjectBuilder_EnvironmentDefinition.Defaults.ContourHighlightColorAccessDenied;
    public float ContourHighlightThickness = 5f;
    public float HighlightPulseInSeconds;
    public List<MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings> EnvironmentalParticles = new List<MyObjectBuilder_EnvironmentDefinition.EnvironmentalParticleSettings>();
    private float m_largeShipMaxAngularSpeed = 18000f;
    private float m_smallShipMaxAngularSpeed = 36000f;
    private float m_largeShipMaxAngularSpeedInRadians = MathHelper.ToRadians(18000f);
    private float m_smallShipMaxAngularSpeedInRadians = MathHelper.ToRadians(36000f);
    public string EnvironmentTexture = "Textures\\BackgroundCube\\Final\\BackgroundCube.dds";
    public MyOrientation EnvironmentOrientation = MyObjectBuilder_EnvironmentDefinition.Defaults.EnvironmentOrientation;

    public MyEnvironmentDefinition()
    {
      this.ShadowSettings = new MyShadowsSettings();
      this.LowLoddingSettings = new MyNewLoddingSettings();
      this.MediumLoddingSettings = new MyNewLoddingSettings();
      this.HighLoddingSettings = new MyNewLoddingSettings();
      this.ExtremeLoddingSettings = new MyNewLoddingSettings();
    }

    public MyShadowsSettings ShadowSettings { get; private set; }

    public MyNewLoddingSettings LowLoddingSettings { get; private set; }

    public MyNewLoddingSettings MediumLoddingSettings { get; private set; }

    public MyNewLoddingSettings HighLoddingSettings { get; private set; }

    public MyNewLoddingSettings ExtremeLoddingSettings { get; private set; }

    public float LargeShipMaxAngularSpeed
    {
      get => this.m_largeShipMaxAngularSpeed;
      private set
      {
        this.m_largeShipMaxAngularSpeed = value;
        this.m_largeShipMaxAngularSpeedInRadians = MathHelper.ToRadians(this.m_largeShipMaxAngularSpeed);
      }
    }

    public float SmallShipMaxAngularSpeed
    {
      get => this.m_smallShipMaxAngularSpeed;
      private set
      {
        this.m_smallShipMaxAngularSpeed = value;
        this.m_smallShipMaxAngularSpeedInRadians = MathHelper.ToRadians(this.m_smallShipMaxAngularSpeed);
      }
    }

    public float LargeShipMaxAngularSpeedInRadians => this.m_largeShipMaxAngularSpeedInRadians;

    public float SmallShipMaxAngularSpeedInRadians => this.m_smallShipMaxAngularSpeedInRadians;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_EnvironmentDefinition environmentDefinition = (MyObjectBuilder_EnvironmentDefinition) builder;
      this.FogProperties = environmentDefinition.FogProperties;
      this.PlanetProperties = environmentDefinition.PlanetProperties;
      this.SunProperties = environmentDefinition.SunProperties;
      this.PostProcessSettings = environmentDefinition.PostProcessSettings;
      this.SSAOSettings = environmentDefinition.SSAOSettings;
      this.HBAOSettings = environmentDefinition.HBAOSettings;
      this.ShadowSettings.CopyFrom(environmentDefinition.ShadowSettings);
      this.LowLoddingSettings.CopyFrom(environmentDefinition.LowLoddingSettings);
      this.MediumLoddingSettings.CopyFrom(environmentDefinition.MediumLoddingSettings);
      this.HighLoddingSettings.CopyFrom(environmentDefinition.HighLoddingSettings);
      this.ExtremeLoddingSettings.CopyFrom(environmentDefinition.ExtremeLoddingSettings);
      this.SmallShipMaxSpeed = environmentDefinition.SmallShipMaxSpeed;
      this.LargeShipMaxSpeed = environmentDefinition.LargeShipMaxSpeed;
      this.SmallShipMaxAngularSpeed = environmentDefinition.SmallShipMaxAngularSpeed;
      this.LargeShipMaxAngularSpeed = environmentDefinition.LargeShipMaxAngularSpeed;
      this.ContourHighlightColor = new Color(environmentDefinition.ContourHighlightColor);
      this.ContourHighlightThickness = environmentDefinition.ContourHighlightThickness;
      this.HighlightPulseInSeconds = environmentDefinition.HighlightPulseInSeconds;
      this.EnvironmentTexture = environmentDefinition.EnvironmentTexture;
      DateTime now = DateTime.Now;
      if (this.EnvironmentTexture == "Textures\\BackgroundCube\\Final\\BackgroundCube.dds" && now >= MyObjectBuilder_EnvironmentDefinition.Defaults.ScaryFaceFrom && now <= MyObjectBuilder_EnvironmentDefinition.Defaults.ScaryFaceTo)
        this.EnvironmentTexture = "Textures\\BackgroundCube\\Final\\BackgroundCube_ScaryFace.dds";
      if (this.EnvironmentTexture == "Textures\\BackgroundCube\\Final\\BackgroundCube.dds" && now >= MyObjectBuilder_EnvironmentDefinition.Defaults.Christmas2019From && now <= MyObjectBuilder_EnvironmentDefinition.Defaults.Christmas2019To)
        this.EnvironmentTexture = "Textures\\BackgroundCube\\Final\\BackgroundCube_Christmas.dds";
      this.EnvironmentOrientation = environmentDefinition.EnvironmentOrientation;
      this.EnvironmentalParticles = environmentDefinition.EnvironmentalParticles;
    }

    public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
    {
      MyObjectBuilder_EnvironmentDefinition environmentDefinition = new MyObjectBuilder_EnvironmentDefinition();
      environmentDefinition.Id = (SerializableDefinitionId) this.Id;
      environmentDefinition.FogProperties = this.FogProperties;
      environmentDefinition.SunProperties = this.SunProperties;
      environmentDefinition.PostProcessSettings = this.PostProcessSettings;
      environmentDefinition.SSAOSettings = this.SSAOSettings;
      environmentDefinition.HBAOSettings = this.HBAOSettings;
      environmentDefinition.ShadowSettings.CopyFrom(this.ShadowSettings);
      environmentDefinition.LowLoddingSettings.CopyFrom(this.LowLoddingSettings);
      environmentDefinition.MediumLoddingSettings.CopyFrom(this.MediumLoddingSettings);
      environmentDefinition.HighLoddingSettings.CopyFrom(this.HighLoddingSettings);
      environmentDefinition.ExtremeLoddingSettings.CopyFrom(this.ExtremeLoddingSettings);
      environmentDefinition.SmallShipMaxSpeed = this.SmallShipMaxSpeed;
      environmentDefinition.LargeShipMaxSpeed = this.LargeShipMaxSpeed;
      environmentDefinition.SmallShipMaxAngularSpeed = this.SmallShipMaxAngularSpeed;
      environmentDefinition.LargeShipMaxAngularSpeed = this.LargeShipMaxAngularSpeed;
      environmentDefinition.ContourHighlightColor = this.ContourHighlightColor.ToVector4();
      environmentDefinition.ContourHighlightThickness = this.ContourHighlightThickness;
      environmentDefinition.HighlightPulseInSeconds = this.HighlightPulseInSeconds;
      environmentDefinition.EnvironmentTexture = this.EnvironmentTexture;
      environmentDefinition.EnvironmentOrientation = this.EnvironmentOrientation;
      environmentDefinition.EnvironmentalParticles = this.EnvironmentalParticles;
      return (MyObjectBuilder_DefinitionBase) environmentDefinition;
    }

    public void Merge(MyEnvironmentDefinition src)
    {
      MyEnvironmentDefinition other = new MyEnvironmentDefinition();
      other.Id = src.Id;
      other.DisplayNameEnum = src.DisplayNameEnum;
      other.DescriptionEnum = src.DescriptionEnum;
      other.DisplayNameString = src.DisplayNameString;
      other.DescriptionString = src.DescriptionString;
      other.Icons = src.Icons;
      other.Enabled = src.Enabled;
      other.Public = src.Public;
      other.AvailableInSurvival = src.AvailableInSurvival;
      other.Context = src.Context;
      MyMergeHelper.Merge<MyEnvironmentDefinition>(this, src, other);
    }

    private class Postprocessor : MyDefinitionPostprocessor
    {
      public override void AfterLoaded(ref MyDefinitionPostprocessor.Bundle definitions)
      {
      }

      public override void AfterPostprocess(
        MyDefinitionSet set,
        Dictionary<MyStringHash, MyDefinitionBase> definitions)
      {
      }

      public override void OverrideBy(
        ref MyDefinitionPostprocessor.Bundle currentDefinitions,
        ref MyDefinitionPostprocessor.Bundle overrideBySet)
      {
        foreach (KeyValuePair<MyStringHash, MyDefinitionBase> definition in overrideBySet.Definitions)
        {
          if (definition.Value.Enabled)
          {
            MyDefinitionBase myDefinitionBase;
            if (currentDefinitions.Definitions.TryGetValue(definition.Key, out myDefinitionBase))
              ((MyEnvironmentDefinition) myDefinitionBase).Merge((MyEnvironmentDefinition) definition.Value);
            else
              currentDefinitions.Definitions.Add(definition.Key, definition.Value);
          }
          else
            currentDefinitions.Definitions.Remove(definition.Key);
        }
      }
    }

    private class Sandbox_Definitions_MyEnvironmentDefinition\u003C\u003EActor : IActivator, IActivator<MyEnvironmentDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyEnvironmentDefinition();

      MyEnvironmentDefinition IActivator<MyEnvironmentDefinition>.CreateInstance() => new MyEnvironmentDefinition();
    }
  }
}
