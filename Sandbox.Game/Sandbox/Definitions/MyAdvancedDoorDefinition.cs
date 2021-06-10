// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyAdvancedDoorDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_AdvancedDoorDefinition), null)]
  public class MyAdvancedDoorDefinition : MyCubeBlockDefinition
  {
    public MyStringHash ResourceSinkGroup;
    public float PowerConsumptionIdle;
    public float PowerConsumptionMoving;
    public MyObjectBuilder_AdvancedDoorDefinition.SubpartDefinition[] Subparts;
    public MyObjectBuilder_AdvancedDoorDefinition.Opening[] OpeningSequence;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_AdvancedDoorDefinition advancedDoorDefinition = builder as MyObjectBuilder_AdvancedDoorDefinition;
      this.ResourceSinkGroup = MyStringHash.GetOrCompute(advancedDoorDefinition.ResourceSinkGroup);
      this.PowerConsumptionIdle = advancedDoorDefinition.PowerConsumptionIdle;
      this.PowerConsumptionMoving = advancedDoorDefinition.PowerConsumptionMoving;
      this.Subparts = advancedDoorDefinition.Subparts;
      this.OpeningSequence = advancedDoorDefinition.OpeningSequence;
    }

    private class Sandbox_Definitions_MyAdvancedDoorDefinition\u003C\u003EActor : IActivator, IActivator<MyAdvancedDoorDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyAdvancedDoorDefinition();

      MyAdvancedDoorDefinition IActivator<MyAdvancedDoorDefinition>.CreateInstance() => new MyAdvancedDoorDefinition();
    }
  }
}
