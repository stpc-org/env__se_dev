// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyVisualSettingsDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Xml.Serialization;
using VRage.Game.Definitions;
using VRage.Network;
using VRageRender;

namespace VRage.Game
{
  [MyDefinitionType(typeof (MyObjectBuilder_VisualSettingsDefinition), null)]
  public class MyVisualSettingsDefinition : MyDefinitionBase
  {
    public MyFogProperties FogProperties = MyFogProperties.Default;
    public MySunProperties SunProperties = MySunProperties.Default;
    public MyPostprocessSettings PostProcessSettings = MyPostprocessSettings.Default;

    public MyVisualSettingsDefinition() => this.ShadowSettings = new MyShadowsSettings();

    [XmlIgnore]
    public MyShadowsSettings ShadowSettings { get; private set; }

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_VisualSettingsDefinition settingsDefinition = (MyObjectBuilder_VisualSettingsDefinition) builder;
      this.FogProperties = settingsDefinition.FogProperties;
      this.SunProperties = settingsDefinition.SunProperties;
      this.PostProcessSettings = settingsDefinition.PostProcessSettings;
      this.ShadowSettings.CopyFrom(settingsDefinition.ShadowSettings);
    }

    public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
    {
      MyObjectBuilder_VisualSettingsDefinition settingsDefinition = new MyObjectBuilder_VisualSettingsDefinition();
      settingsDefinition.FogProperties = this.FogProperties;
      settingsDefinition.SunProperties = this.SunProperties;
      settingsDefinition.PostProcessSettings = this.PostProcessSettings;
      settingsDefinition.ShadowSettings.CopyFrom(this.ShadowSettings);
      return (MyObjectBuilder_DefinitionBase) settingsDefinition;
    }

    private class VRage_Game_MyVisualSettingsDefinition\u003C\u003EActor : IActivator, IActivator<MyVisualSettingsDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyVisualSettingsDefinition();

      MyVisualSettingsDefinition IActivator<MyVisualSettingsDefinition>.CreateInstance() => new MyVisualSettingsDefinition();
    }
  }
}
