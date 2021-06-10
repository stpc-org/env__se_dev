// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyParachuteDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ParachuteDefinition), null)]
  public class MyParachuteDefinition : MyCubeBlockDefinition
  {
    public MyStringHash ResourceSinkGroup;
    public float PowerConsumptionIdle;
    public float PowerConsumptionMoving;
    public MyObjectBuilder_ParachuteDefinition.SubpartDefinition[] Subparts;
    public MyObjectBuilder_ParachuteDefinition.Opening[] OpeningSequence;
    public string ParachuteSubpartName;
    public float DragCoefficient;
    public int MaterialDeployCost;
    public MyDefinitionId MaterialDefinitionId;
    public float ReefAtmosphereLevel;
    public float MinimumAtmosphereLevel;
    public float RadiusMultiplier;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_ParachuteDefinition parachuteDefinition = builder as MyObjectBuilder_ParachuteDefinition;
      this.ResourceSinkGroup = MyStringHash.GetOrCompute(parachuteDefinition.ResourceSinkGroup);
      this.PowerConsumptionIdle = parachuteDefinition.PowerConsumptionIdle;
      this.PowerConsumptionMoving = parachuteDefinition.PowerConsumptionMoving;
      this.Subparts = parachuteDefinition.Subparts;
      this.OpeningSequence = parachuteDefinition.OpeningSequence;
      this.ParachuteSubpartName = parachuteDefinition.ParachuteSubpartName;
      this.DragCoefficient = parachuteDefinition.DragCoefficient;
      this.MaterialDeployCost = parachuteDefinition.MaterialDeployCost;
      this.ReefAtmosphereLevel = parachuteDefinition.ReefAtmosphereLevel;
      this.MinimumAtmosphereLevel = parachuteDefinition.MinimumAtmosphereLevel;
      this.RadiusMultiplier = parachuteDefinition.RadiusMultiplier;
      this.MaterialDefinitionId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_Component), parachuteDefinition.MaterialSubtype);
    }

    private class Sandbox_Definitions_MyParachuteDefinition\u003C\u003EActor : IActivator, IActivator<MyParachuteDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyParachuteDefinition();

      MyParachuteDefinition IActivator<MyParachuteDefinition>.CreateInstance() => new MyParachuteDefinition();
    }
  }
}
