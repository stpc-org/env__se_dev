// Decompiled with JetBrains decompiler
// Type: VRage.Game.Definitions.MySafeZoneSettingsDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace VRage.Game.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_SafeZoneSettingsDefinition), null)]
  public class MySafeZoneSettingsDefinition : MyDefinitionBase
  {
    public int EnableAnimationTimeMs;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      this.EnableAnimationTimeMs = (builder as MyObjectBuilder_SafeZoneSettingsDefinition).EnableAnimationTimeMs;
    }

    private class VRage_Game_Definitions_MySafeZoneSettingsDefinition\u003C\u003EActor : IActivator, IActivator<MySafeZoneSettingsDefinition>
    {
      object IActivator.CreateInstance() => (object) new MySafeZoneSettingsDefinition();

      MySafeZoneSettingsDefinition IActivator<MySafeZoneSettingsDefinition>.CreateInstance() => new MySafeZoneSettingsDefinition();
    }
  }
}
