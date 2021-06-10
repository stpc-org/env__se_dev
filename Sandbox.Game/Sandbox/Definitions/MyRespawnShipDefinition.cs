// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyRespawnShipDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_RespawnShipDefinition), null)]
  public class MyRespawnShipDefinition : MyDefinitionBase
  {
    public int Cooldown;
    public MyPrefabDefinition Prefab;
    public bool UseForSpace;
    public float MinimalAirDensity;
    public bool UseForPlanetsWithAtmosphere;
    public bool UseForPlanetsWithoutAtmosphere;
    public float PlanetDeployAltitude;
    public Vector3 InitialLinearVelocity;
    public Vector3 InitialAngularVelocity;
    public bool SpawnNearProceduralAsteroids;
    public string[] PlanetTypes;
    public Vector3D? SpawnPosition;
    public float SpawnPositionDispersionMin;
    public float SpawnPositionDispersionMax;
    public bool SpawnWithDefaultItems;
    public string HelpTextLocalizationId;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_RespawnShipDefinition respawnShipDefinition = (MyObjectBuilder_RespawnShipDefinition) builder;
      this.Cooldown = respawnShipDefinition.CooldownSeconds;
      this.Prefab = MyDefinitionManager.Static.GetPrefabDefinition(respawnShipDefinition.Prefab);
      this.UseForSpace = respawnShipDefinition.UseForSpace;
      this.MinimalAirDensity = respawnShipDefinition.MinimalAirDensity;
      this.SpawnWithDefaultItems = respawnShipDefinition.SpawnWithDefaultItems;
      this.InitialLinearVelocity = (Vector3) respawnShipDefinition.InitialLinearVelocity;
      this.InitialAngularVelocity = (Vector3) respawnShipDefinition.InitialAngularVelocity;
      this.UseForPlanetsWithAtmosphere = respawnShipDefinition.UseForPlanetsWithAtmosphere;
      this.UseForPlanetsWithoutAtmosphere = respawnShipDefinition.UseForPlanetsWithoutAtmosphere;
      float? planetDeployAltitude = respawnShipDefinition.PlanetDeployAltitude;
      this.PlanetDeployAltitude = planetDeployAltitude.HasValue ? planetDeployAltitude.GetValueOrDefault() : (respawnShipDefinition.UseForPlanetsWithAtmosphere || respawnShipDefinition.UseForPlanetsWithoutAtmosphere ? 2000f : 10f);
      this.HelpTextLocalizationId = respawnShipDefinition.HelpTextLocalizationId;
      this.SpawnNearProceduralAsteroids = respawnShipDefinition.SpawnNearProceduralAsteroids;
      this.PlanetTypes = respawnShipDefinition.PlanetTypes;
      SerializableVector3D? spawnPosition = respawnShipDefinition.SpawnPosition;
      this.SpawnPosition = spawnPosition.HasValue ? new Vector3D?((Vector3D) spawnPosition.GetValueOrDefault()) : new Vector3D?();
      this.SpawnPositionDispersionMin = respawnShipDefinition.SpawnPositionDispersionMin;
      this.SpawnPositionDispersionMax = respawnShipDefinition.SpawnPositionDispersionMax;
      this.CorrectInvalidStates();
    }

    private void CorrectInvalidStates()
    {
      bool hasValue = this.SpawnPosition.HasValue;
      bool flag = this.PlanetTypes != null;
      int num = this.UseForPlanetsWithAtmosphere ? 1 : (this.UseForPlanetsWithoutAtmosphere ? 1 : 0);
      if ((num & (hasValue ? 1 : 0) & (flag ? 1 : 0)) != 0)
      {
        this.UseForPlanetsWithAtmosphere = false;
        this.UseForPlanetsWithoutAtmosphere = false;
        this.CorrectInvalidStates();
      }
      if (num == 0 & hasValue & flag)
      {
        this.PlanetTypes = (string[]) null;
        this.CorrectInvalidStates();
      }
      if (((num != 0 ? 0 : (!hasValue ? 1 : 0)) & (flag ? 1 : 0)) != 0)
      {
        this.UseForPlanetsWithAtmosphere = true;
        this.UseForPlanetsWithoutAtmosphere = true;
        this.CorrectInvalidStates();
      }
      if ((num & (hasValue ? 1 : 0)) == 0 || flag)
        return;
      this.UseForPlanetsWithAtmosphere = false;
      this.UseForPlanetsWithoutAtmosphere = false;
      this.CorrectInvalidStates();
    }

    private class Sandbox_Definitions_MyRespawnShipDefinition\u003C\u003EActor : IActivator, IActivator<MyRespawnShipDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyRespawnShipDefinition();

      MyRespawnShipDefinition IActivator<MyRespawnShipDefinition>.CreateInstance() => new MyRespawnShipDefinition();
    }
  }
}
