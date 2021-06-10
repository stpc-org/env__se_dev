// Decompiled with JetBrains decompiler
// Type: VRage.Game.Definitions.Reputation.MyReputationSettingsDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Game.ObjectBuilders.Definitions.Reputation;
using VRage.Network;

namespace VRage.Game.Definitions.Reputation
{
  [MyDefinitionType(typeof (MyObjectBuilder_ReputationSettingsDefinition), null)]
  public class MyReputationSettingsDefinition : MyDefinitionBase
  {
    public int MaxReputationGainInTime;
    public int ResetTimeMinForRepGain;
    public MyObjectBuilder_ReputationSettingsDefinition.MyReputationDamageSettings DamageSettings;
    public MyObjectBuilder_ReputationSettingsDefinition.MyReputationDamageSettings PirateDamageSettings;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_ReputationSettingsDefinition settingsDefinition = builder as MyObjectBuilder_ReputationSettingsDefinition;
      this.DamageSettings = settingsDefinition.DamageSettings;
      this.PirateDamageSettings = settingsDefinition.PirateDamageSettings;
      this.MaxReputationGainInTime = settingsDefinition.MaxReputationGainInTime;
      this.ResetTimeMinForRepGain = settingsDefinition.ResetTimeMinForRepGain;
    }

    private class VRage_Game_Definitions_Reputation_MyReputationSettingsDefinition\u003C\u003EActor : IActivator, IActivator<MyReputationSettingsDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyReputationSettingsDefinition();

      MyReputationSettingsDefinition IActivator<MyReputationSettingsDefinition>.CreateInstance() => new MyReputationSettingsDefinition();
    }
  }
}
