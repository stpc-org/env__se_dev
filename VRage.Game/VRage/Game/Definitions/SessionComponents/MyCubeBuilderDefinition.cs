// Decompiled with JetBrains decompiler
// Type: VRage.Game.Definitions.SessionComponents.MyCubeBuilderDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Game.Components.Session;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using VRage.Network;

namespace VRage.Game.Definitions.SessionComponents
{
  [MyDefinitionType(typeof (MyObjectBuilder_CubeBuilderDefinition), null)]
  public class MyCubeBuilderDefinition : MySessionComponentDefinition
  {
    public float DefaultBlockBuildingDistance;
    public float MaxBlockBuildingDistance;
    public float MinBlockBuildingDistance;
    public double BuildingDistSmallSurvivalCharacter;
    public double BuildingDistLargeSurvivalCharacter;
    public double BuildingDistSmallSurvivalShip;
    public double BuildingDistLargeSurvivalShip;
    public MyPlacementSettings BuildingSettings;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_CubeBuilderDefinition builderDefinition = (MyObjectBuilder_CubeBuilderDefinition) builder;
      this.DefaultBlockBuildingDistance = builderDefinition.DefaultBlockBuildingDistance;
      this.MaxBlockBuildingDistance = builderDefinition.MaxBlockBuildingDistance;
      this.MinBlockBuildingDistance = builderDefinition.MinBlockBuildingDistance;
      this.BuildingDistSmallSurvivalCharacter = builderDefinition.BuildingDistSmallSurvivalCharacter;
      this.BuildingDistLargeSurvivalCharacter = builderDefinition.BuildingDistLargeSurvivalCharacter;
      this.BuildingDistSmallSurvivalShip = builderDefinition.BuildingDistSmallSurvivalShip;
      this.BuildingDistLargeSurvivalShip = builderDefinition.BuildingDistLargeSurvivalShip;
      this.BuildingSettings = builderDefinition.BuildingSettings;
    }

    private class VRage_Game_Definitions_SessionComponents_MyCubeBuilderDefinition\u003C\u003EActor : IActivator, IActivator<MyCubeBuilderDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyCubeBuilderDefinition();

      MyCubeBuilderDefinition IActivator<MyCubeBuilderDefinition>.CreateInstance() => new MyCubeBuilderDefinition();
    }
  }
}
