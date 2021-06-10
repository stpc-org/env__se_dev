// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.MyReplicableRightsValidator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Ingame;
using VRage.Groups;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Replication
{
  internal static class MyReplicableRightsValidator
  {
    private static float ALLOWED_PHYSICAL_DISTANCE_SQUARED = (float) ((double) MyConstants.DEFAULT_INTERACTIVE_DISTANCE * 3.0 * ((double) MyConstants.DEFAULT_INTERACTIVE_DISTANCE * 3.0));

    public static ValidationResult GetControlled(
      MyEntity controlledEntity,
      EndpointId endpointId)
    {
      if (controlledEntity == null)
        return ValidationResult.Kick | ValidationResult.Controlled;
      MyPlayer controllingPlayer1 = MySession.Static.Players.GetControllingPlayer(controlledEntity);
      if (controllingPlayer1 != null && (long) controllingPlayer1.Client.SteamUserId == (long) endpointId.Value || MySession.Static.IsUserAdmin(endpointId.Value))
        return ValidationResult.Passed;
      MyPlayer controllingPlayer2 = MySession.Static.Players.GetPreviousControllingPlayer(controlledEntity);
      return (controllingPlayer2 == null || (long) controllingPlayer2.Client.SteamUserId != (long) endpointId.Value) && !MySession.Static.IsUserAdmin(endpointId.Value) ? ValidationResult.Kick | ValidationResult.Controlled : ValidationResult.Controlled;
    }

    public static bool GetBigOwner(
      MyCubeGrid grid,
      EndpointId endpointId,
      long identityId,
      bool spaceMaster)
    {
      if (grid == null)
        return false;
      bool flag = grid.BigOwners.Count == 0 || grid.BigOwners.Contains(identityId);
      if (spaceMaster)
        flag |= MySession.Static.IsUserSpaceMaster(endpointId.Value);
      return flag;
    }

    public static bool GetAccess(
      MyCharacterReplicable characterReplicable,
      Vector3D characterPosition,
      MyCubeGrid grid,
      MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group,
      bool physical)
    {
      if (characterReplicable == null || grid == null)
        return false;
      if (group != null)
      {
        foreach (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node node in group.Nodes)
        {
          if (physical && node.NodeData.PositionComp.WorldAABB.DistanceSquared(characterPosition) <= (double) MyReplicableRightsValidator.ALLOWED_PHYSICAL_DISTANCE_SQUARED || characterReplicable.CachedParentDependencies.Contains((IMyEntity) node.NodeData))
            return true;
        }
      }
      else if (physical && grid.PositionComp.WorldAABB.DistanceSquared(characterPosition) <= (double) MyReplicableRightsValidator.ALLOWED_PHYSICAL_DISTANCE_SQUARED || characterReplicable.CachedParentDependencies.Contains((IMyEntity) grid))
        return true;
      return false;
    }
  }
}
