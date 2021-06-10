// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MySessionComponentTrash
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Game.World.Generator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Groups;
using VRage.Input;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.SessionComponents
{
  [StaticEventOwner]
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation, 2000)]
  public class MySessionComponentTrash : MySessionComponentBase
  {
    private static MyDistributedUpdater<CachingList<MyCubeGrid>, MyCubeGrid> m_gridsToCheck = new MyDistributedUpdater<CachingList<MyCubeGrid>, MyCubeGrid>(100);
    private static float m_playerDistanceHysteresis = 0.0f;
    private static HashSet<long> m_entitiesInCenter = new HashSet<long>();
    private static bool m_worldHasPlanets;
    private int m_identityCheckIndex;
    private List<MyIdentity> m_allIdentities = new List<MyIdentity>();
    private int m_trashedGridsCount;
    private bool m_voxelTrash_StartFromBegining = true;
    private List<long> m_voxel_BaseIds = new List<long>();
    private int m_voxel_BaseCurrentIndex;
    private MyVoxelBase m_voxel_CurrentBase;
    private MyStorageBase m_voxel_CurrentStorage;
    private IEnumerator<KeyValuePair<Vector3I, MyTimeSpan>> m_voxel_CurrentAccessEnumerator;
    private KeyValuePair<Vector3I, MyTimeSpan>? m_voxel_CurrentChunk;
    private int m_voxel_Timer;
    private static int CONST_VOXEL_WAIT_CYCLE = 600;
    private static int CONST_VOXEL_WAIT_CHUNK = 10;
    private TimeSpan m_afkTimer;
    private bool m_afkTimerInitialized;
    private bool m_kicked;
    private double m_lastSecondsLeft = double.MaxValue;
    private MyHudNotification m_kickNotification;
    private TimeSpan m_stopGridsTimer;
    private static MySessionComponentTrash m_static;
    private List<MyEntity> m_entityQueryCache;
    private static Dictionary<MyTrashRemovalFlags, MyStringId> m_names = new Dictionary<MyTrashRemovalFlags, MyStringId>()
    {
      {
        MyTrashRemovalFlags.Fixed,
        MySpaceTexts.ScreenDebugAdminMenu_Stations
      },
      {
        MyTrashRemovalFlags.Stationary,
        MySpaceTexts.ScreenDebugAdminMenu_Stationary
      },
      {
        MyTrashRemovalFlags.Linear,
        MyCommonTexts.ScreenDebugAdminMenu_Linear
      },
      {
        MyTrashRemovalFlags.Accelerating,
        MyCommonTexts.ScreenDebugAdminMenu_Accelerating
      },
      {
        MyTrashRemovalFlags.Powered,
        MySpaceTexts.ScreenDebugAdminMenu_Powered
      },
      {
        MyTrashRemovalFlags.Controlled,
        MySpaceTexts.ScreenDebugAdminMenu_Controlled
      },
      {
        MyTrashRemovalFlags.WithProduction,
        MySpaceTexts.ScreenDebugAdminMenu_WithProduction
      },
      {
        MyTrashRemovalFlags.WithMedBay,
        MySpaceTexts.ScreenDebugAdminMenu_WithMedBay
      },
      {
        MyTrashRemovalFlags.RevertCloseToNPCGrids,
        MySpaceTexts.ScreenDebugAdminMenu_RevertCloseToNPCGrids
      },
      {
        MyTrashRemovalFlags.WithBlockCount,
        MyCommonTexts.ScreenDebugAdminMenu_WithBlockCount
      },
      {
        MyTrashRemovalFlags.DistanceFromPlayer,
        MyCommonTexts.ScreenDebugAdminMenu_DistanceFromPlayer
      },
      {
        MyTrashRemovalFlags.RevertMaterials,
        MyCommonTexts.ScreenDebugAdminMenu_RevertMaterials
      },
      {
        MyTrashRemovalFlags.RevertAsteroids,
        MyCommonTexts.ScreenDebugAdminMenu_RevertAsteroids
      },
      {
        MyTrashRemovalFlags.RevertWithFloatingsPresent,
        MyCommonTexts.ScreenDebugAdminMenu_RevertWithFloatingsPresent
      },
      {
        MyTrashRemovalFlags.RevertBoulders,
        MyCommonTexts.ScreenDebugAdminMenu_RevertBoulders
      }
    };
    private List<long> m_voxelIds = new List<long>();
    private int m_voxelBoulderIndex = -1;
    private bool m_clearBoulders = true;

    public static float PlayerDistanceHysteresis => MySessionComponentTrash.m_playerDistanceHysteresis;

    private MyTrashRemovalFlags TrashFlags
    {
      get
      {
        MyTrashRemovalFlags trashRemovalFlags = MySession.Static.Settings.TrashFlags;
        if (MySandboxGame.Static.MemoryState >= MySandboxGame.MemState.Low)
          trashRemovalFlags = trashRemovalFlags | MyTrashRemovalFlags.RevertAsteroids | MyTrashRemovalFlags.RevertWithFloatingsPresent;
        return trashRemovalFlags;
      }
    }

    public override void LoadData()
    {
      MySessionComponentTrash.m_static = this;
      if (!Sync.IsServer)
      {
        this.m_kickNotification = new MyHudNotification(MySpaceTexts.Trash_KickAFKWarning, 10000);
      }
      else
      {
        this.UpdateVoxelTrashRemoval();
        MyCampaignManager.OnActiveCampaignChanged += new Action(this.OnActiveCampaignChanged);
        Sandbox.Game.Entities.MyEntities.OnEntityAdd += new Action<MyEntity>(this.MyEntities_OnEntityAdd);
        Sandbox.Game.Entities.MyEntities.OnEntityRemove += new Action<MyEntity>(this.MyEntities_OnEntityRemove);
        MySession.Static.Players.IdentitiesChanged += new Action(this.Players_IdentitiesChanged);
        this.m_trashedGridsCount = 0;
      }
    }

    private void UpdateVoxelTrashRemoval()
    {
      if (!MyCampaignManager.Static.IsScenarioRunning && !MyCampaignManager.Static.IsNewCampaignLevelLoading || (MyCampaignManager.Static.ActiveCampaign == null || !MyCampaignManager.Static.ActiveCampaign.ForceDisableTrashRemoval))
        return;
      MySession.Static.Settings.VoxelTrashRemovalEnabled = false;
    }

    private void OnActiveCampaignChanged() => this.UpdateVoxelTrashRemoval();

    protected override void UnloadData()
    {
      MySessionComponentTrash.m_static = (MySessionComponentTrash) null;
      if (!Sync.IsServer)
        return;
      MyCampaignManager.OnActiveCampaignChanged -= new Action(this.OnActiveCampaignChanged);
      Sandbox.Game.Entities.MyEntities.OnEntityAdd -= new Action<MyEntity>(this.MyEntities_OnEntityAdd);
      Sandbox.Game.Entities.MyEntities.OnEntityRemove -= new Action<MyEntity>(this.MyEntities_OnEntityRemove);
      MySession.Static.Players.IdentitiesChanged -= new Action(this.Players_IdentitiesChanged);
      this.m_trashedGridsCount = 0;
      MySessionComponentTrash.m_entitiesInCenter.Clear();
      MySessionComponentTrash.m_gridsToCheck.List.ClearImmediate();
      MySessionComponentTrash.m_worldHasPlanets = false;
    }

    public override void BeforeStart()
    {
      base.BeforeStart();
      this.m_stopGridsTimer = TimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds) + TimeSpan.FromMinutes((double) MySession.Static.Settings.StopGridsPeriodMin);
      MySessionComponentTrash.m_worldHasPlanets = MyPlanets.GetPlanets().Count > 0;
    }

    private void Players_IdentitiesChanged() => this.m_allIdentities = MySession.Static.Players.GetAllIdentities().ToList<MyIdentity>();

    private void MyEntities_OnEntityAdd(MyEntity entity)
    {
      if (!(entity is MyCubeGrid))
        return;
      MySessionComponentTrash.m_gridsToCheck.List.Add(entity as MyCubeGrid);
    }

    private void MyEntities_OnEntityRemove(MyEntity entity)
    {
      if (!(entity is MyCubeGrid entity1))
        return;
      MySessionComponentTrash.m_gridsToCheck.List.Remove(entity1);
      if (!entity1.MarkedAsTrash)
        return;
      --this.m_trashedGridsCount;
    }

    public override void UpdateAfterSimulation()
    {
      if (!Sync.IsServer)
      {
        if (!MySession.Static.Ready || MySession.Static.Settings.AFKTimeountMin <= 0 || this.m_kicked)
          return;
        if (!this.m_afkTimerInitialized)
        {
          this.m_afkTimerInitialized = true;
          this.m_afkTimer = MySession.Static.ElapsedPlayTime + TimeSpan.FromMinutes((double) MySession.Static.Settings.AFKTimeountMin);
        }
        TimeSpan timeSpan = this.m_afkTimer - MySession.Static.ElapsedPlayTime;
        if (timeSpan.TotalSeconds <= 60.0 && this.m_lastSecondsLeft > 60.0)
          MyHud.Notifications.Add((MyHudNotificationBase) this.m_kickNotification);
        if (timeSpan.Ticks <= 0L)
        {
          MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (x => new Action(MySessionComponentTrash.AFKKickRequest_Server)));
          this.m_kicked = true;
        }
        this.m_lastSecondsLeft = timeSpan.TotalSeconds;
      }
      else
      {
        MySessionComponentTrash.m_gridsToCheck.List.ApplyChanges();
        if (MySession.Static.Settings.TrashRemovalEnabled && (Sync.IsDedicated || !MySession.Static.IsCameraUserAnySpectator()))
        {
          bool trashFound = false;
          MySessionComponentTrash.m_gridsToCheck.Update();
          MySessionComponentTrash.m_gridsToCheck.Iterate((Action<MyCubeGrid>) (x =>
          {
            if (x == null)
              return;
            trashFound |= this.UpdateTrash(x);
          }));
          if (MySession.Static.Settings.OptimalGridCount > 0 && !trashFound)
          {
            if (MySessionComponentTrash.m_gridsToCheck.List.Count - this.m_trashedGridsCount > MySession.Static.Settings.OptimalGridCount)
              --MySessionComponentTrash.m_playerDistanceHysteresis;
            else if (MySessionComponentTrash.m_gridsToCheck.List.Count - this.m_trashedGridsCount < MySession.Static.Settings.OptimalGridCount && (double) MySessionComponentTrash.m_playerDistanceHysteresis < 0.0)
              ++MySessionComponentTrash.m_playerDistanceHysteresis;
            MySessionComponentTrash.m_playerDistanceHysteresis = MathHelper.Clamp(MySessionComponentTrash.m_playerDistanceHysteresis, -MySession.Static.Settings.PlayerDistanceThreshold, 0.0f);
          }
          this.CheckIdentitiesTrash();
        }
        this.VoxelRevertor_Update();
      }
    }

    private void CheckIdentitiesTrash()
    {
      int index = -1;
      if (this.m_identityCheckIndex < this.m_allIdentities.Count)
        index = this.m_identityCheckIndex++;
      else
        this.m_identityCheckIndex = 0;
      this.CheckIdentity(index);
    }

    private void CheckIdentity(int index)
    {
      if (index >= this.m_allIdentities.Count || index < 0)
        return;
      MyIdentity allIdentity = this.m_allIdentities[index];
      MyPlayer.PlayerId result;
      if (MySession.Static.Players.TryGetPlayerId(allIdentity.IdentityId, out result))
      {
        if (MySession.Static.Players.TryGetPlayerById(result, out MyPlayer _))
          return;
        int num = (int) ((double) MySession.GetIdentityLogoutTimeSeconds(allIdentity.IdentityId) / 60.0);
        bool flag = false;
        if (MySession.Static.Settings.RemoveOldIdentitiesH > 0 && num >= MySession.Static.Settings.RemoveOldIdentitiesH * 60)
          flag = this.TryRemoveAbandonedIdentity(allIdentity, result);
        int removalThreshold = MySession.Static.Settings.PlayerCharacterRemovalThreshold;
        if (!flag && (removalThreshold == 0 || num < removalThreshold))
          return;
        this.TryRemoveAbandonedCharacter(allIdentity);
      }
      else
        MySessionComponentTrash.CloseAbandonedRespawnShip(allIdentity);
    }

    private void TryRemoveAbandonedCharacter(MyIdentity identity)
    {
      if (identity.Character != null)
      {
        if (this.RemoveCharacter(identity.Character))
          MySessionComponentTrash.CloseAbandonedRespawnShip(identity);
      }
      else
        MySessionComponentTrash.CloseAbandonedRespawnShip(identity);
      foreach (long savedCharacter in identity.SavedCharacters)
      {
        MyCharacter entity;
        if (Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyCharacter>(savedCharacter, out entity, true) && (!entity.Closed || entity.MarkedForClose))
          this.RemoveCharacter(entity);
      }
    }

    private bool TryRemoveAbandonedIdentity(MyIdentity identity, MyPlayer.PlayerId playerId)
    {
      if (identity.BlockLimits.BlocksBuilt != 0)
        return false;
      this.TryRemoveAbandonedFaction(identity);
      MySession.Static.Players.RemoveIdentity(identity.IdentityId, playerId);
      this.m_allIdentities.Remove(identity);
      --this.m_identityCheckIndex;
      return true;
    }

    private void TryRemoveAbandonedFaction(MyIdentity identity)
    {
      MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(identity.IdentityId);
      if (playerFaction == null)
        return;
      MyFactionCollection.KickMember(playerFaction.FactionId, identity.IdentityId);
    }

    public static void CloseAbandonedRespawnShip(MyIdentity identity)
    {
      if (!MySession.Static.Settings.RespawnShipDelete)
        return;
      MySessionComponentTrash.CloseRespawnShip(identity);
    }

    private static void CloseRespawnShip(MyIdentity identity)
    {
      if (identity.RespawnShips == null)
        return;
      foreach (long respawnShip in identity.RespawnShips)
      {
        MyCubeGrid entity;
        if (Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyCubeGrid>(respawnShip, out entity))
        {
          foreach (MySlimBlock block in entity.GetBlocks())
          {
            if (block.FatBlock is MyCockpit fatBlock && fatBlock.Pilot != null)
              fatBlock.Use();
          }
          MyLog.Default.Info(string.Format("CloseRespawnShip removed entity '{0}:{1}' with entity id '{2}' Block Count: '{3}'", (object) entity.Name, (object) entity.DisplayName, (object) entity.EntityId, (object) entity.BlockCounter));
          Sandbox.Game.Entities.MyEntities.SendCloseRequest((IMyEntity) entity);
        }
      }
      identity.RespawnShips.Clear();
    }

    public static void CloseRespawnShip(MyPlayer player)
    {
      if (player.RespawnShip == null)
        return;
      MySessionComponentTrash.CloseRespawnShip(player.Identity);
    }

    private bool RemoveCharacter(MyCharacter character)
    {
      if (character.IsUsing is MyCryoChamber)
        return false;
      if (character.IsUsing is MyCockpit)
        (character.IsUsing as MyCockpit).RemovePilot();
      MyLog.Default.Info(string.Format("Trash collector removed character '{0}' with entity id '{1}'", (object) character.GetPlayerIdentityId(), (object) character.EntityId));
      character.Close();
      return true;
    }

    private bool UpdateTrash(MyCubeGrid grid)
    {
      if (grid == null || MySession.Static == null || (grid.MarkedForClose || grid.MarkedAsTrash) || (grid.IsPreview || grid.Projector != null || grid.IsGenerated))
        return false;
      if (!Sandbox.Game.Entities.MyEntities.IsInsideWorld(grid.PositionComp.GetPosition()))
      {
        MySessionComponentTrash.RemoveGrid(grid);
        return true;
      }
      if (MyEncounterGenerator.Static != null && MyEncounterGenerator.Static.IsEncounter((MyEntity) grid))
        return false;
      MyNeutralShipSpawner neutralShipSpawner = MySession.Static != null ? MySession.Static.GetComponent<MyNeutralShipSpawner>() : (MyNeutralShipSpawner) null;
      if (neutralShipSpawner != null && neutralShipSpawner.IsEncounter(grid.EntityId))
        return false;
      if (MySessionComponentTrash.m_worldHasPlanets && (grid.PositionComp.GetPosition().LengthSquared() < 100.0 && !MySessionComponentTrash.m_entitiesInCenter.Contains(grid.EntityId)))
      {
        MySessionComponentTrash.m_entitiesInCenter.Add(grid.EntityId);
        MyLog.Default.Info(string.Format("Trash cleaner reports that '{0}:{1}' with entity id '{2}' Landed in the center of the UNIVERSE!'", (object) grid.Name, (object) grid.DisplayName, (object) grid.EntityId));
      }
      MyTrashRemovalFlags trashState = this.GetTrashState(grid);
      if (trashState == MyTrashRemovalFlags.None)
      {
        if ((double) MySessionComponentTrash.PlayerDistanceHysteresis == 0.0)
        {
          MySessionComponentTrash.RemoveGrid(grid);
        }
        else
        {
          grid.MarkAsTrash();
          ++this.m_trashedGridsCount;
        }
        return true;
      }
      double num = this.m_stopGridsTimer.TotalMilliseconds - (double) MySandboxGame.TotalGamePlayTimeInMilliseconds;
      if (MySession.Static != null && MySession.Static.Settings.StopGridsPeriodMin > 0)
      {
        if (num > (double) (MySession.Static.Settings.StopGridsPeriodMin * 60 * 1000))
          this.m_stopGridsTimer = TimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds) + TimeSpan.FromMinutes((double) MySession.Static.Settings.StopGridsPeriodMin);
        if (!grid.IsStatic && grid.Physics != null && (grid.Physics.IsMoving && !trashState.HasFlags(MyTrashRemovalFlags.DistanceFromPlayer)) && num <= 0.0)
        {
          this.m_stopGridsTimer = TimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds) + TimeSpan.FromMinutes((double) MySession.Static.Settings.StopGridsPeriodMin);
          MyEntityList.ProceedEntityAction((MyEntity) grid, MyEntityList.EntityListAction.Stop);
        }
      }
      else if (num <= 0.0)
        this.m_stopGridsTimer = TimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds);
      return false;
    }

    public static string GetName(MyTrashRemovalFlags flag)
    {
      MyStringId id;
      return MySessionComponentTrash.m_names.TryGetValue(flag, out id) ? MyTexts.GetString(id) : MyEnum<MyTrashRemovalFlags>.GetName(flag);
    }

    private void VoxelRevertor_Update()
    {
      if (!MySession.Static.Settings.VoxelTrashRemovalEnabled)
        return;
      this.BoulderReversion();
      this.VoxelReversion();
    }

    private void VoxelReversion()
    {
      if (this.m_voxel_Timer >= 0)
      {
        --this.m_voxel_Timer;
      }
      else
      {
        if (this.m_voxelTrash_StartFromBegining)
        {
          this.m_voxelTrash_StartFromBegining = false;
          this.m_voxel_BaseIds.Clear();
          MySession.Static.VoxelMaps.GetAllIds(ref this.m_voxel_BaseIds);
          this.m_voxel_BaseCurrentIndex = -1;
          this.m_voxel_CurrentBase = (MyVoxelBase) null;
          this.m_voxel_CurrentStorage = (MyStorageBase) null;
          this.m_voxel_CurrentAccessEnumerator = (IEnumerator<KeyValuePair<Vector3I, MyTimeSpan>>) null;
        }
        if (!this.VoxelRevertor_AdvanceToNext())
        {
          this.m_voxelTrash_StartFromBegining = true;
        }
        else
        {
          if (!this.m_voxel_CurrentChunk.HasValue || !this.VoxelRevertor_CanRevertCurrent())
            return;
          if (this.m_voxel_CurrentBase.BoulderInfo.HasValue)
          {
            MyPlanet.RevertBoulderServer(this.m_voxel_CurrentBase);
            this.m_voxel_CurrentBase.Close();
          }
          else
          {
            Vector3I pos = this.m_voxel_CurrentChunk.Value.Key;
            MyStorageDataTypeFlags flags = (this.TrashFlags & MyTrashRemovalFlags.RevertMaterials) != MyTrashRemovalFlags.None ? MyStorageDataTypeFlags.ContentAndMaterial : MyStorageDataTypeFlags.Content;
            MyVoxelBase currentBase = this.m_voxel_CurrentBase;
            MyStorageBase storage = this.m_voxel_CurrentStorage;
            bool isAsteroid = this.IsStorageAsteroid(this.m_voxel_CurrentStorage);
            bool placedByPlayer = currentBase.CreatedByUser;
            if (storage.DataProvider == null)
              return;
            Parallel.Start((Action) (() => storage.AccessDelete(ref pos, flags, false)), (Action) (() =>
            {
              Vector3I min;
              Vector3I max;
              storage.GetAccessRange(pos, out min, out max);
              storage.NotifyChanged(min, max, flags);
              MyMultiplayer.RaiseEvent<MyVoxelBase, Vector3I, MyStorageDataTypeFlags>(currentBase.RootVoxel, (Func<MyVoxelBase, Action<Vector3I, MyStorageDataTypeFlags>>) (x => new Action<Vector3I, MyStorageDataTypeFlags>(x.RevertVoxelAccess)), pos, flags);
              if (!isAsteroid)
                return;
              currentBase.RevertProceduralAsteroidVoxelSettings();
              MyVoxelBase.StorageChanged OnStorageRangeChanged = (MyVoxelBase.StorageChanged) null;
              OnStorageRangeChanged = (MyVoxelBase.StorageChanged) ((voxel, minVoxelChanged, maxVoxelChanged, changedData) =>
              {
                voxel.Save = true;
                voxel.RangeChanged -= OnStorageRangeChanged;
              });
              currentBase.RangeChanged += OnStorageRangeChanged;
              if (placedByPlayer || currentBase.IsSeedOpen.HasValue && currentBase.IsSeedOpen.Value)
                return;
              currentBase.Close();
            }));
          }
        }
      }
    }

    private void BoulderReversion()
    {
      if ((MySession.Static.Settings.TrashFlags & MyTrashRemovalFlags.RevertBoulders) != MyTrashRemovalFlags.None && this.ClearBoulders && (MySession.Static.ElapsedPlayTime.TotalMinutes > (double) MySession.Static.Settings.VoxelAgeThreshold && !this.BoulderClearingInProgress))
      {
        this.ClearBoulders = false;
        this.BoulderClearingInProgress = true;
        this.m_voxelBoulderIndex = -1;
      }
      if (!this.BoulderClearingInProgress)
        return;
      if (this.m_voxelBoulderIndex == -1)
      {
        MySession.Static.VoxelMaps.GetAllIds(ref this.m_voxelIds);
        if (this.m_voxelIds.Count > 0)
        {
          this.m_voxelBoulderIndex = 0;
        }
        else
        {
          this.BoulderClearingInProgress = false;
          return;
        }
      }
      while (this.m_voxelBoulderIndex < this.m_voxelIds.Count)
      {
        MyVoxelBase entityById = Sandbox.Game.Entities.MyEntities.GetEntityById(this.m_voxelIds[this.m_voxelBoulderIndex]) as MyVoxelBase;
        ++this.m_voxelBoulderIndex;
        if (this.CanRevertBoulder(entityById))
        {
          MyPlanet.RevertBoulderServer(entityById);
          entityById.Close();
          break;
        }
      }
      if (this.m_voxelBoulderIndex < this.m_voxelIds.Count)
        return;
      this.m_voxelIds.Clear();
      this.m_voxelBoulderIndex = -1;
      this.BoulderClearingInProgress = false;
    }

    public bool ClearBoulders
    {
      get => this.m_clearBoulders;
      set
      {
        if (this.m_clearBoulders == value)
          return;
        this.m_clearBoulders = value;
      }
    }

    public bool BoulderClearingInProgress { get; private set; }

    private bool VoxelRevertor_CanRevertCurrent()
    {
      Vector3I key = this.m_voxel_CurrentChunk.Value.Key;
      MyTimeSpan myTimeSpan = this.m_voxel_CurrentChunk.Value.Value;
      MyObjectBuilder_SessionSettings settings = MySession.Static.Settings;
      int voxelAgeThreshold = settings.VoxelAgeThreshold;
      if (MySandboxGame.Static.MemoryState >= MySandboxGame.MemState.Low)
        voxelAgeThreshold /= 20;
      if (MyTimeSpan.FromTicks(Stopwatch.GetTimestamp() - myTimeSpan.Ticks).Minutes < (double) voxelAgeThreshold)
        return false;
      BoundingBoxD bb;
      this.m_voxel_CurrentStorage.ConvertAccessCoordinates(ref key, out bb);
      Vector3D leftBottomCorner = this.m_voxel_CurrentBase.PositionLeftBottomCorner;
      bb.Translate(leftBottomCorner);
      if (this.m_voxel_CurrentBase.RootVoxel != this.m_voxel_CurrentBase)
        bb.Translate(-leftBottomCorner);
      using (MyUtils.ReuseCollection<MyEntity>(ref this.m_entityQueryCache))
      {
        float distanceThreshold1 = settings.VoxelGridDistanceThreshold;
        float distanceThreshold2 = settings.VoxelPlayerDistanceThreshold;
        if (MySandboxGame.Static.MemoryState >= MySandboxGame.MemState.Low)
        {
          distanceThreshold1 /= 10f;
          distanceThreshold2 /= 10f;
        }
        BoundingBoxD inflated = bb.GetInflated((double) Math.Max(distanceThreshold1, distanceThreshold2));
        MyGamePruningStructure.GetTopmostEntitiesInBox(ref inflated, this.m_entityQueryCache);
        MatrixD matrixD;
        foreach (MyEntity myEntity in this.m_entityQueryCache)
        {
          if (myEntity is MyCubeGrid grid)
          {
            bool flag = (settings.TrashFlags & MyTrashRemovalFlags.RevertCloseToNPCGrids) != MyTrashRemovalFlags.None && MySessionComponentTrash.IsGridNpc(grid);
            double num1 = myEntity.PositionComp.WorldAABB.DistanceSquared(ref bb);
            float num2 = flag ? 1f : distanceThreshold1;
            double num3 = (double) num2 * (double) num2;
            if (num1 < num3)
            {
              if (flag)
              {
                MyGridPhysics physics = grid.Physics;
                if ((physics != null ? (physics.IsStatic ? 1 : 0) : 1) == 0)
                {
                  BoundingBoxD worldAabb = grid.PositionComp.WorldAABB;
                  Vector3I voxelCoord1;
                  MyVoxelCoordSystems.WorldPositionToVoxelCoord(leftBottomCorner, ref worldAabb.Min, out voxelCoord1);
                  Vector3I voxelCoord2;
                  MyVoxelCoordSystems.WorldPositionToVoxelCoord(leftBottomCorner, ref worldAabb.Max, out voxelCoord2);
                  BoundingBoxI box = new BoundingBoxI(voxelCoord1, voxelCoord2);
                  if (!this.m_voxel_CurrentStorage.IsRangeModified(ref box))
                    continue;
                }
                else
                  continue;
              }
              return false;
            }
          }
          else if (myEntity is MyCharacter)
          {
            ref BoundingBoxD local = ref bb;
            matrixD = myEntity.PositionComp.WorldMatrixRef;
            Vector3D translation = matrixD.Translation;
            if (local.DistanceSquared(translation) < (double) distanceThreshold2 * (double) distanceThreshold2)
              return false;
          }
          else if ((settings.TrashFlags & MyTrashRemovalFlags.RevertWithFloatingsPresent) == MyTrashRemovalFlags.None && (myEntity is MyFloatingObject || myEntity is MyInventoryBagEntity))
          {
            matrixD = myEntity.PositionComp.WorldMatrixRef;
            Vector3D translation = matrixD.Translation;
            ContainmentType result;
            bb.Contains(ref translation, out result);
            if (result != ContainmentType.Disjoint)
              return false;
          }
        }
        return true;
      }
    }

    private bool CanRevertBoulder(MyVoxelBase voxels)
    {
      MyObjectBuilder_SessionSettings settings = MySession.Static.Settings;
      if ((settings.TrashFlags & MyTrashRemovalFlags.RevertBoulders) == MyTrashRemovalFlags.None || voxels == null || (!voxels.BoulderInfo.HasValue || !voxels.Save))
        return false;
      BoundingBoxD worldAabb = voxels.PositionComp.WorldAABB;
      using (MyUtils.ReuseCollection<MyEntity>(ref this.m_entityQueryCache))
      {
        float distanceThreshold1 = settings.VoxelGridDistanceThreshold;
        float distanceThreshold2 = settings.VoxelPlayerDistanceThreshold;
        if (MySandboxGame.Static.MemoryState >= MySandboxGame.MemState.Low)
        {
          distanceThreshold1 /= 10f;
          distanceThreshold2 /= 10f;
        }
        BoundingBoxD inflated = worldAabb.GetInflated((double) Math.Max(distanceThreshold1, distanceThreshold2));
        MyGamePruningStructure.GetTopmostEntitiesInBox(ref inflated, this.m_entityQueryCache);
        foreach (MyEntity myEntity in this.m_entityQueryCache)
        {
          if (myEntity is MyCubeGrid)
          {
            if (myEntity.PositionComp.WorldAABB.DistanceSquared(ref worldAabb) < (double) distanceThreshold1 * (double) distanceThreshold1)
              return false;
          }
          else if (myEntity is MyCharacter)
          {
            if (worldAabb.DistanceSquared(myEntity.PositionComp.WorldMatrixRef.Translation) < (double) distanceThreshold2 * (double) distanceThreshold2)
              return false;
          }
          else if ((settings.TrashFlags & MyTrashRemovalFlags.RevertWithFloatingsPresent) == MyTrashRemovalFlags.None && (myEntity is MyFloatingObject || myEntity is MyInventoryBagEntity))
          {
            Vector3D translation = myEntity.PositionComp.WorldMatrixRef.Translation;
            ContainmentType result;
            worldAabb.Contains(ref translation, out result);
            if (result != ContainmentType.Disjoint)
              return false;
          }
        }
        return true;
      }
    }

    private bool VoxelRevertor_AdvanceToNext()
    {
      try
      {
        while (true)
        {
          do
          {
            if ((this.m_voxel_CurrentBase == null ? 0 : (this.m_voxel_CurrentStorage != null ? 1 : 0)) == 0)
            {
              ++this.m_voxel_BaseCurrentIndex;
              if (this.m_voxel_BaseCurrentIndex >= this.m_voxel_BaseIds.Count)
              {
                this.m_voxel_Timer = MySessionComponentTrash.CONST_VOXEL_WAIT_CYCLE;
                return false;
              }
              MyVoxelBase voxelBaseById = MySession.Static.VoxelMaps.TryGetVoxelBaseById(this.m_voxel_BaseIds[this.m_voxel_BaseCurrentIndex]);
              if (voxelBaseById?.Storage is MyStorageBase storage && this.VoxelsAreSuitableForReversion(voxelBaseById, storage))
              {
                this.m_voxel_CurrentBase = voxelBaseById;
                this.m_voxel_CurrentStorage = storage;
                this.m_voxel_CurrentAccessEnumerator = (IEnumerator<KeyValuePair<Vector3I, MyTimeSpan>>) null;
                this.m_voxel_CurrentChunk = new KeyValuePair<Vector3I, MyTimeSpan>?();
              }
            }
          }
          while (this.m_voxel_CurrentBase == null || this.m_voxel_CurrentStorage == null);
          if (this.m_voxel_CurrentAccessEnumerator == null)
            this.m_voxel_CurrentAccessEnumerator = this.m_voxel_CurrentStorage.AccessEnumerator;
          if (!this.m_voxel_CurrentAccessEnumerator.MoveNext())
          {
            this.m_voxel_CurrentBase = (MyVoxelBase) null;
            this.m_voxel_CurrentStorage = (MyStorageBase) null;
            this.m_voxel_CurrentChunk = new KeyValuePair<Vector3I, MyTimeSpan>?();
          }
          else
            break;
        }
        this.m_voxel_CurrentChunk = new KeyValuePair<Vector3I, MyTimeSpan>?(this.m_voxel_CurrentAccessEnumerator.Current);
        this.m_voxel_Timer = MySessionComponentTrash.CONST_VOXEL_WAIT_CHUNK;
        return true;
      }
      finally
      {
      }
    }

    private bool IsStorageAsteroid(MyStorageBase storage) => storage.DataProvider is MyCompositeShapeProvider;

    private bool VoxelsAreSuitableForReversion(MyVoxelBase vox, MyStorageBase storage)
    {
      bool flag = vox.BoulderInfo.HasValue && (uint) (this.TrashFlags & MyTrashRemovalFlags.RevertBoulders) > 0U;
      if (vox.Closed || storage.DataProvider == null && !flag || vox.RootVoxel != vox)
        return false;
      IMyStorageDataProvider dataProvider = storage.DataProvider;
      return !this.IsStorageAsteroid(storage) || (this.TrashFlags & MyTrashRemovalFlags.RevertAsteroids) != MyTrashRemovalFlags.None;
    }

    public static void RemoveGrid(MyCubeGrid grid)
    {
      MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group group = MyCubeGridGroups.Static.Physical.GetGroup(grid);
      if (group != null)
      {
        foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node node in group.Nodes)
        {
          node.NodeData.DismountAllCockpits();
          node.NodeData.Close();
        }
      }
      MyLog.Default.Info(string.Format("Trash collector removed grid '{0}:{1}' with entity id '{2}'", (object) grid.Name, (object) grid.DisplayName, (object) grid.EntityId));
      grid.Close();
      if (grid.BigOwners.Count <= 0)
        return;
      long bigOwner = grid.BigOwners[0];
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(bigOwner);
      MyPlayer.PlayerId result;
      MyPlayer player;
      if (!MySession.Static.Players.TryGetPlayerId(bigOwner, out result) || !MySession.Static.Players.TryGetPlayerById(result, out player) || (MySession.Static.Players.GetOnlinePlayers().Contains(player) || identity == null) || identity.BlockLimits.BlocksBuilt != 0)
        return;
      MySession.Static.Players.RemoveIdentity(bigOwner);
    }

    public MyTrashRemovalFlags GetTrashState(MyCubeGrid grid) => this.GetTrashState(grid, out float _, true);

    private MyTrashRemovalFlags GetTrashState(
      MyCubeGrid grid,
      out float metric,
      bool checkGroup = false)
    {
      metric = -1f;
      if ((double) (float) grid.GridGeneralDamageModifier == 0.0)
        return MyTrashRemovalFlags.Indestructible;
      float num = MySession.GetOwnerLogoutTimeSeconds(grid) / 3600f;
      if (((double) num <= 0.0 || (double) MySession.Static.Settings.PlayerInactivityThreshold <= 0.0 ? 0 : ((double) num > (double) MySession.Static.Settings.PlayerInactivityThreshold ? 1 : 0)) != 0)
      {
        if (!checkGroup)
          return MyTrashRemovalFlags.None;
        float metric1 = 0.0f;
        return this.GetPhysicalGroupTrashState(grid, ref metric1);
      }
      MyTrashRemovalFlags trashRemovalFlags = MyTrashRemovalFlags.None;
      if (MySessionComponentTrash.IsCloseToPlayerOrCamera((MyEntity) grid, MySession.Static.Settings.PlayerDistanceThreshold + MySessionComponentTrash.PlayerDistanceHysteresis))
      {
        metric = MySession.Static.Settings.PlayerDistanceThreshold;
        trashRemovalFlags = MyTrashRemovalFlags.DistanceFromPlayer;
      }
      if ((double) MySessionComponentTrash.PlayerDistanceHysteresis == 0.0)
      {
        if (grid.Physics == null)
          return MyTrashRemovalFlags.Default | trashRemovalFlags;
        bool flag1 = (double) grid.Physics.AngularVelocity.AbsMax() < 0.0500000007450581 && (double) grid.Physics.LinearVelocity.AbsMax() < 0.0500000007450581;
        bool flag2 = !flag1 && ((double) grid.Physics.AngularAcceleration.AbsMax() > 0.0500000007450581 || (double) grid.Physics.LinearAcceleration.AbsMax() > 0.0500000007450581);
        bool flag3 = !flag2 && !flag1;
        if (!this.TrashFlags.HasFlags(MyTrashRemovalFlags.Stationary) & flag1)
          return MyTrashRemovalFlags.Stationary | trashRemovalFlags;
        if (!this.TrashFlags.HasFlags(MyTrashRemovalFlags.Linear) & flag3)
          return MyTrashRemovalFlags.Linear | trashRemovalFlags;
        if (!this.TrashFlags.HasFlags(MyTrashRemovalFlags.Accelerating) & flag2)
          return MyTrashRemovalFlags.Accelerating | trashRemovalFlags;
      }
      HashSet<MySlimBlock> blocks = grid.GetBlocks();
      if ((double) MySessionComponentTrash.PlayerDistanceHysteresis == 0.0)
      {
        if (blocks != null && blocks.Count >= MySession.Static.Settings.BlockCountThreshold)
        {
          metric = (float) MySession.Static.Settings.BlockCountThreshold;
          return MyTrashRemovalFlags.WithBlockCount | trashRemovalFlags;
        }
        if (!this.TrashFlags.HasFlags(MyTrashRemovalFlags.Fixed) && grid.IsStatic)
          return MyTrashRemovalFlags.Fixed | trashRemovalFlags;
        if (grid.GridSystems != null)
        {
          bool flag1 = grid.GridSystems.ResourceDistributor.ResourceStateByType(MyResourceDistributorComponent.ElectricityId) != MyResourceStateEnum.NoPower;
          if (!this.TrashFlags.HasFlags(MyTrashRemovalFlags.Powered) & flag1)
          {
            bool flag2 = true;
            long piratesId = MyPirateAntennas.GetPiratesId();
            MyIdentity identity = MySession.Static.Players.TryGetIdentity(piratesId);
            if (identity != null && !identity.BlockLimits.HasRemainingPCU && (grid.BigOwners.Contains(piratesId) && grid.Save))
            {
              bool flag3 = false;
              foreach (long smallOwner in grid.SmallOwners)
              {
                if (!MySession.Static.Players.IdentityIsNpc(smallOwner))
                {
                  flag3 = true;
                  break;
                }
              }
              if (!flag3)
                flag2 = false;
            }
            if (flag2)
              return MyTrashRemovalFlags.Powered | trashRemovalFlags;
          }
        }
      }
      if (grid.GridSystems != null)
      {
        if (!this.TrashFlags.HasFlags(MyTrashRemovalFlags.Controlled) && grid.GridSystems.ControlSystem != null && grid.GridSystems.ControlSystem.IsControlled)
          return MyTrashRemovalFlags.Controlled | trashRemovalFlags;
        if (!this.TrashFlags.HasFlags(MyTrashRemovalFlags.WithProduction) && (grid.BlocksCounters.GetValueOrDefault<MyObjectBuilderType, int>((MyObjectBuilderType) typeof (MyObjectBuilder_ProductionBlock)) > 0 || grid.BlocksCounters.GetValueOrDefault<MyObjectBuilderType, int>((MyObjectBuilderType) typeof (MyObjectBuilder_Assembler)) > 0 || grid.BlocksCounters.GetValueOrDefault<MyObjectBuilderType, int>((MyObjectBuilderType) typeof (MyObjectBuilder_Refinery)) > 0))
          return MyTrashRemovalFlags.WithProduction | trashRemovalFlags;
      }
      if (!this.TrashFlags.HasFlags(MyTrashRemovalFlags.WithMedBay))
      {
        int valueOrDefault1 = grid.BlocksCounters.GetValueOrDefault<MyObjectBuilderType, int>((MyObjectBuilderType) typeof (MyObjectBuilder_MedicalRoom));
        int valueOrDefault2 = grid.BlocksCounters.GetValueOrDefault<MyObjectBuilderType, int>((MyObjectBuilderType) typeof (MyObjectBuilder_SurvivalKit));
        if (valueOrDefault1 > 0 || valueOrDefault2 > 0)
          return MyTrashRemovalFlags.WithMedBay;
      }
      if (checkGroup && MyCubeGridGroups.Static.Physical != null)
        trashRemovalFlags |= this.GetPhysicalGroupTrashState(grid, ref metric);
      return trashRemovalFlags;
    }

    private MyTrashRemovalFlags GetPhysicalGroupTrashState(
      MyCubeGrid grid,
      ref float metric)
    {
      MyTrashRemovalFlags trashRemovalFlags = MyTrashRemovalFlags.None;
      MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group group = MyCubeGridGroups.Static.Physical.GetGroup(grid);
      if (group != null)
      {
        foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node node in group.Nodes)
        {
          if (node.NodeData != null && node.NodeData.Physics != null && (node.NodeData.Physics.Shape != null && node.NodeData != grid))
          {
            MyTrashRemovalFlags trashState = this.GetTrashState(node.NodeData, out metric);
            if (trashState != MyTrashRemovalFlags.None)
              return trashState | trashRemovalFlags;
          }
        }
      }
      return trashRemovalFlags;
    }

    public static bool IsCloseToPlayerOrCamera(MyEntity entity, float distanceThreshold)
    {
      Vector3D translation = entity.WorldMatrix.Translation;
      float num1 = distanceThreshold * distanceThreshold;
      if (Sync.Players.GetOnlinePlayers().Count > 0)
      {
        int num2 = 0;
        foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) Sync.Players.GetOnlinePlayers())
        {
          IMyControllableEntity controlledEntity = onlinePlayer.Controller.ControlledEntity;
          if (controlledEntity != null)
          {
            ++num2;
            if (Vector3D.DistanceSquared(controlledEntity.Entity.WorldMatrix.Translation, translation) < (double) num1)
              return true;
          }
        }
        if (num2 > 0)
          return false;
      }
      return true;
    }

    private static bool IsGridNpc(MyCubeGrid grid)
    {
      if (grid.SmallOwners.Count == 0)
        return false;
      MyPlayerCollection players = MySession.Static.Players;
      foreach (long smallOwner in grid.SmallOwners)
      {
        if (!players.IdentityIsNpc(smallOwner))
          return false;
      }
      return true;
    }

    public void SetPlayerAFKTimeout(int min) => MyMultiplayer.RaiseStaticEvent<int>((Func<IMyEventOwner, Action<int>>) (x => new Action<int>(MySessionComponentTrash.SetPlayerAFKTimeout_Server)), min);

    [Event(null, 1272)]
    [Reliable]
    [Server]
    public static void SetPlayerAFKTimeout_Server(int min)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
      {
        MyEventContext.ValidationFailed();
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        if (MySession.Static.Settings.AFKTimeountMin != min)
          MyLog.Default.Info(string.Format("Trash AFK Timeount changed by {0} to {1}", (object) MyEventContext.Current.Sender, (object) min));
        MySession.Static.Settings.AFKTimeountMin = min;
        MyMultiplayer.RaiseStaticEvent<int>((Func<IMyEventOwner, Action<int>>) (x => new Action<int>(MySessionComponentTrash.SetPlayerAFKTimeout_Broadcast)), min);
      }
    }

    [Event(null, 1292)]
    [Reliable]
    [Broadcast]
    public static void SetPlayerAFKTimeout_Broadcast(int min)
    {
      MySession.Static.Settings.AFKTimeountMin = min;
      if (Sync.IsServer)
        return;
      if (min > 0)
        MySessionComponentTrash.m_static.m_afkTimer = MySession.Static.ElapsedPlayTime + TimeSpan.FromMinutes((double) MySession.Static.Settings.AFKTimeountMin);
      else
        MySessionComponentTrash.m_static.m_afkTimer = TimeSpan.MaxValue;
    }

    [Event(null, 1310)]
    [Reliable]
    [Server]
    private static void AFKKickRequest_Server() => MyMultiplayer.Static.KickClient(MyEventContext.Current.Sender.Value, add: false);

    public override void HandleInput()
    {
      base.HandleInput();
      if (Sync.IsServer || MySession.Static.Settings.AFKTimeountMin <= 0 || !MyInput.Static.IsAnyKeyPress() && !MyInput.Static.IsAnyNewMousePressed() && (MyInput.Static.GetMouseX() == 0 && MyInput.Static.GetMouseY() == 0) && MyInput.Static.IsJoystickIdle())
        return;
      this.m_afkTimer = MySession.Static.ElapsedPlayTime + TimeSpan.FromMinutes((double) MySession.Static.Settings.AFKTimeountMin);
      MyHud.Notifications.Remove((MyHudNotificationBase) this.m_kickNotification);
    }

    protected sealed class SetPlayerAFKTimeout_Server\u003C\u003ESystem_Int32 : ICallSite<IMyEventOwner, int, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in int min,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentTrash.SetPlayerAFKTimeout_Server(min);
      }
    }

    protected sealed class SetPlayerAFKTimeout_Broadcast\u003C\u003ESystem_Int32 : ICallSite<IMyEventOwner, int, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in int min,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentTrash.SetPlayerAFKTimeout_Broadcast(min);
      }
    }

    protected sealed class AFKKickRequest_Server\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentTrash.AFKKickRequest_Server();
      }
    }
  }
}
