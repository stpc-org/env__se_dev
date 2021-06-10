// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyRespawnComponentBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Game.World.Generator;
using System;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.SessionComponents
{
  public abstract class MyRespawnComponentBase : MySessionComponentBase
  {
    public static event Action<MyPlayer> RespawnRequested;

    public abstract void InitFromCheckpoint(MyObjectBuilder_Checkpoint checkpoint);

    public abstract void SaveToCheckpoint(MyObjectBuilder_Checkpoint checkpoint);

    public abstract bool HandleRespawnRequest(
      bool joinGame,
      bool newIdentity,
      long respawnEntityId,
      string respawnShipId,
      MyPlayer.PlayerId playerId,
      Vector3D? spawnPosition,
      Vector3? direction,
      Vector3? up,
      SerializableDefinitionId? botDefinitionId,
      bool realPlayer,
      string modelName,
      Color color);

    public abstract MyIdentity CreateNewIdentity(
      string identityName,
      MyPlayer.PlayerId playerId,
      string modelName,
      bool initialPlayer = false);

    public abstract void AfterRemovePlayer(MyPlayer player);

    public abstract void SetupCharacterDefault(MyPlayer player, MyWorldGenerator.Args args);

    public abstract bool IsInRespawnScreen();

    public abstract void CloseRespawnScreen();

    public abstract void CloseRespawnScreenNow();

    public abstract void SetNoRespawnText(StringBuilder text, int timeSec);

    public abstract void SetupCharacterFromStarts(
      MyPlayer player,
      MyWorldGeneratorStartingStateBase[] playerStarts,
      MyWorldGenerator.Args args);

    protected static bool ShowPermaWarning { get; set; }

    public void ResetPlayerIdentity(MyPlayer player, string modelName, Color color)
    {
      if (player.Identity == null || !MySession.Static.Settings.PermanentDeath.Value)
        return;
      if (!player.Identity.IsDead)
        Sync.Players.KillPlayer(player);
      IMyFaction playerFaction = MySession.Static.Factions.TryGetPlayerFaction(player.Identity.IdentityId);
      if (playerFaction != null)
        MyFactionCollection.KickMember(playerFaction.FactionId, player.Identity.IdentityId);
      MySession.Static.ChatSystem.ChatHistory.ClearNonGlobalHistory();
      MyIdentity newIdentity = Sync.Players.CreateNewIdentity(player.DisplayName, modelName, new Vector3?((Vector3) color), false, false);
      player.Identity = newIdentity;
    }

    protected static void NotifyRespawnRequested(MyPlayer player)
    {
      if (MyRespawnComponentBase.RespawnRequested == null)
        return;
      MyRespawnComponentBase.RespawnRequested(player);
    }

    public static Vector3D? FindPositionAbovePlanet(
      Vector3D friendPosition,
      ref SpawnInfo info,
      bool testFreeZone,
      int distanceIteration,
      int maxDistanceIterations,
      float? optimalSpawnDistance = null,
      MyEntity ignoreEntity = null)
    {
      MyPlanet planet = info.Planet;
      float collisionRadius = info.CollisionRadius;
      Vector3D center = planet.PositionComp.WorldAABB.Center;
      Vector3D axis1 = Vector3D.Normalize(friendPosition - center);
      float num1 = !optimalSpawnDistance.HasValue ? MySession.Static.Settings.OptimalSpawnDistance : optimalSpawnDistance.Value;
      float num2 = num1 * 0.9f;
      for (int index1 = 0; index1 < 20; ++index1)
      {
        Vector3D perpendicularVector1 = MyUtils.GetRandomPerpendicularVector(ref axis1);
        float num3 = num1 * (MyUtils.GetRandomFloat(1.05f, 1.15f) + (float) distanceIteration * 0.05f);
        Vector3D globalPos = friendPosition + perpendicularVector1 * (double) num3;
        globalPos = planet.GetClosestSurfacePointGlobal(ref globalPos);
        if (!testFreeZone || (double) info.MinimalAirDensity <= 0.0 || (double) planet.GetAirDensity(globalPos) >= (double) info.MinimalAirDensity)
        {
          Vector3D axis2 = Vector3D.Normalize(globalPos - center);
          Vector3D perpendicularVector2 = MyUtils.GetRandomPerpendicularVector(ref axis2);
          bool flag = true;
          Vector3 vector1 = (Vector3) perpendicularVector2 * collisionRadius;
          Vector3 vector3 = Vector3.Cross(vector1, (Vector3) axis2);
          MyOrientedBoundingBoxD orientedBoundingBoxD = new MyOrientedBoundingBoxD(globalPos, new Vector3D((double) collisionRadius * 2.0, (double) Math.Min(10f, collisionRadius * 0.5f), (double) collisionRadius * 2.0), Quaternion.CreateFromForwardUp((Vector3) perpendicularVector2, (Vector3) axis2));
          int num4 = -1;
          for (int index2 = 0; index2 < 4; ++index2)
          {
            num4 = -num4;
            int num5 = index2 > 1 ? -1 : 1;
            Vector3D surfacePointGlobal = planet.GetClosestSurfacePointGlobal(globalPos + vector1 * (float) num4 + vector3 * (float) num5);
            if (!orientedBoundingBoxD.Contains(ref surfacePointGlobal))
            {
              flag = false;
              break;
            }
          }
          if (flag)
          {
            if (testFreeZone && !MyProceduralWorldModule.IsZoneFree(new BoundingSphereD(globalPos, (double) num2)))
            {
              ++distanceIteration;
              if (distanceIteration > maxDistanceIterations)
                break;
            }
            else
            {
              Vector3D vector3D = Vector3D.Normalize(globalPos - center);
              Vector3D? freePlace = MyEntities.FindFreePlace(globalPos + vector3D * (double) info.PlanetDeployAltitude, collisionRadius, ignoreEnt: ignoreEntity);
              if (freePlace.HasValue)
                return new Vector3D?(freePlace.Value);
            }
          }
        }
      }
      return new Vector3D?();
    }
  }
}
