// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyResourceDistributionGroupDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ResourceDistributionGroup), null)]
  public class MyResourceDistributionGroupDefinition : MyDefinitionBase
  {
    public int Priority;
    public bool IsSource;
    public bool IsAdaptible;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_ResourceDistributionGroup distributionGroup = builder as MyObjectBuilder_ResourceDistributionGroup;
      this.IsSource = distributionGroup.IsSource;
      this.Priority = distributionGroup.Priority;
      this.IsAdaptible = distributionGroup.IsAdaptible;
    }

    private class Sandbox_Definitions_MyResourceDistributionGroupDefinition\u003C\u003EActor : IActivator, IActivator<MyResourceDistributionGroupDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyResourceDistributionGroupDefinition();

      MyResourceDistributionGroupDefinition IActivator<MyResourceDistributionGroupDefinition>.CreateInstance() => new MyResourceDistributionGroupDefinition();
    }
  }
}
