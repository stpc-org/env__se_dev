// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyFactionDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_FactionDefinition), null)]
  public class MyFactionDefinition : MyDefinitionBase
  {
    public string Tag;
    public string Name;
    public string Founder;
    public MyStringId FactionIcon;
    public bool AcceptHumans;
    public bool AutoAcceptMember;
    public bool EnableFriendlyFire;
    public bool IsDefault;
    public MyRelationsBetweenFactions DefaultRelation;
    public MyRelationsBetweenFactions DefaultRelationToPlayers;
    public long StartingBalance;
    public MyFactionTypes Type;
    public bool DiscoveredByDefault;
    public int Score;
    public float ObjectivePercentageCompleted;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_FactionDefinition factionDefinition = builder as MyObjectBuilder_FactionDefinition;
      this.Tag = factionDefinition.Tag;
      this.Name = factionDefinition.Name;
      this.Founder = factionDefinition.Founder;
      this.AcceptHumans = factionDefinition.AcceptHumans;
      this.AutoAcceptMember = factionDefinition.AutoAcceptMember;
      this.EnableFriendlyFire = factionDefinition.EnableFriendlyFire;
      this.IsDefault = factionDefinition.IsDefault;
      this.DefaultRelation = factionDefinition.DefaultRelation;
      this.StartingBalance = factionDefinition.StartingBalance;
      this.Type = factionDefinition.Type;
      this.DiscoveredByDefault = factionDefinition.DiscoveredByDefault;
      this.FactionIcon = MyStringId.GetOrCompute(factionDefinition.FactionIcon);
    }

    public override void Postprocess()
    {
      base.Postprocess();
      MyDefinitionManager.Static.RegisterFactionDefinition(this);
    }

    private class Sandbox_Definitions_MyFactionDefinition\u003C\u003EActor : IActivator, IActivator<MyFactionDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyFactionDefinition();

      MyFactionDefinition IActivator<MyFactionDefinition>.CreateInstance() => new MyFactionDefinition();
    }
  }
}
