// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.World.Generator.MyRespawnShipState
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.World;
using VRage.Game;
using VRageMath;

namespace SpaceEngineers.Game.World.Generator
{
  [MyWorldGenerator.StartingStateType(typeof (MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip))]
  public class MyRespawnShipState : MyWorldGeneratorStartingStateBase
  {
    private string m_respawnShipId;

    public override void Init(
      MyObjectBuilder_WorldGeneratorPlayerStartingState builder)
    {
      base.Init(builder);
      this.m_respawnShipId = (builder as MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip).RespawnShip;
    }

    public override MyObjectBuilder_WorldGeneratorPlayerStartingState GetObjectBuilder()
    {
      MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip objectBuilder = base.GetObjectBuilder() as MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip;
      objectBuilder.RespawnShip = this.m_respawnShipId;
      return (MyObjectBuilder_WorldGeneratorPlayerStartingState) objectBuilder;
    }

    public override void SetupCharacter(MyWorldGenerator.Args generatorArgs)
    {
      string respawnShipId = this.m_respawnShipId;
      if (!MyDefinitionManager.Static.HasRespawnShip(this.m_respawnShipId))
        respawnShipId = MyDefinitionManager.Static.GetFirstRespawnShip();
      if (MySession.Static.LocalHumanPlayer == null)
        return;
      this.CreateAndSetPlayerFaction();
      MySpaceRespawnComponent.Static.SpawnAtShip(MySession.Static.LocalHumanPlayer, respawnShipId, (MyBotDefinition) null, (string) null, new Color?());
    }

    public override Vector3D? GetStartingLocation() => new Vector3D?();
  }
}
