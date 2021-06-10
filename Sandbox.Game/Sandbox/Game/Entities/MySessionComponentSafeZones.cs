// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MySessionComponentSafeZones
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Components;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities
{
  [StaticEventOwner]
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation, 2000, typeof (MyObjectBuilder_SessionComponentSafeZones), null, false)]
  public class MySessionComponentSafeZones : MySessionComponentBase
  {
    private static MyConcurrentList<MySafeZone> m_safeZones = new MyConcurrentList<MySafeZone>();
    public static MySafeZoneAction AllowedActions = MySafeZoneAction.All;
    private static HashSet<MyEntity> m_recentlyAddedEntities = new HashSet<MyEntity>();
    private static HashSet<MyEntity> m_recentlyRemovedEntities = new HashSet<MyEntity>();
    private const int FRAMES_TO_REMOVE_RECENT = 100;
    private int m_recentCounter;
    internal const int ADDED_BUILD_PROJECTION_VERSION = 1198000;

    public static event EventHandler OnAddSafeZone;

    public static event EventHandler OnRemoveSafeZone;

    public static event Action<MySafeZone> OnSafeZoneUpdated;

    public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
    {
      base.Init(sessionComponent);
      MySessionComponentSafeZones.AllowedActions = (sessionComponent as MyObjectBuilder_SessionComponentSafeZones).AllowedActions;
      if (MySession.Static.AppVersionFromSave >= 1198000 || !Sync.IsServer || (MySessionComponentSafeZones.AllowedActions & MySafeZoneAction.Building) == (MySafeZoneAction) 0)
        return;
      MySessionComponentSafeZones.AllowedActions |= MySafeZoneAction.BuildingProjections;
    }

    public override bool IsRequiredByGame => true;

    public override MyObjectBuilder_SessionComponent GetObjectBuilder() => (MyObjectBuilder_SessionComponent) new MyObjectBuilder_SessionComponentSafeZones()
    {
      AllowedActions = MySessionComponentSafeZones.AllowedActions
    };

    public static void AddSafeZone(MySafeZone safeZone)
    {
      MySessionComponentSafeZones.m_safeZones.Add(safeZone);
      if (MySessionComponentSafeZones.OnAddSafeZone == null)
        return;
      MySessionComponentSafeZones.OnAddSafeZone((object) safeZone, (EventArgs) null);
    }

    public static void RemoveSafeZone(MySafeZone safeZone)
    {
      MySessionComponentSafeZones.m_safeZones.Remove(safeZone);
      if (MySessionComponentSafeZones.OnRemoveSafeZone == null)
        return;
      MySessionComponentSafeZones.OnRemoveSafeZone((object) safeZone, (EventArgs) null);
    }

    public static ListReader<MySafeZone> SafeZones => (ListReader<MySafeZone>) MySessionComponentSafeZones.m_safeZones.List;

    public static void RequestCreateSafeZone(Vector3D position)
    {
      if (!MySession.Static.IsUserAdmin(Sync.MyId))
        return;
      MyMultiplayer.RaiseStaticEvent<Vector3D>((Func<IMyEventOwner, Action<Vector3D>>) (x => new Action<Vector3D>(MySessionComponentSafeZones.CreateSafeZone_Implementation)), position);
    }

    [Event(null, 125)]
    [Reliable]
    [Server]
    public static void CreateSafeZone_Implementation(Vector3D position)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserAdmin(MyEventContext.Current.Sender.Value))
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      else
        MySessionComponentSafeZones.CrateSafeZone(MatrixD.CreateWorld(position), MySafeZoneShape.Box, MySafeZoneAccess.Whitelist, (long[]) null, (long[]) null, 100f, false);
    }

    public static long CreateSafeZone_ImplementationPlayer(
      long safeZoneBlockId,
      float startRadius,
      bool activate,
      ulong playerSteamId)
    {
      if (!Sync.IsServer)
      {
        MyLog.Default.Error("CreateSafeZone_ImplementationPlayer can be only called by server.");
        return 0;
      }
      MyMultiplayerBase myMultiplayerBase = MyMultiplayer.Static;
      MyIdentity playerIdentity;
      MyCubeBlock beaconBlock;
      if (MySessionComponentSafeZones.IsPlayerValidationFailed(playerSteamId, safeZoneBlockId, out playerIdentity, out beaconBlock))
        return 0;
      long[] factions = (long[]) null;
      MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(beaconBlock.SlimBlock.OwnerId);
      if (playerFaction != null)
        factions = new long[1]{ playerFaction.FactionId };
      long[] players;
      if (playerIdentity.IdentityId != beaconBlock.SlimBlock.OwnerId)
        players = new long[2]
        {
          playerIdentity.IdentityId,
          beaconBlock.SlimBlock.OwnerId
        };
      else
        players = new long[1]{ playerIdentity.IdentityId };
      return MySessionComponentSafeZones.CrateSafeZone(beaconBlock.PositionComp.WorldMatrixRef, MySafeZoneShape.Sphere, MySafeZoneAccess.Whitelist, players, factions, startRadius, activate, color: Color.SkyBlue.ToVector3(), safeZoneBlockId: safeZoneBlockId).EntityId;
    }

    private static bool IsPlayerValidationFailed(
      ulong steamId,
      long safeZoneBlockId,
      out MyIdentity playerIdentity,
      out MyCubeBlock beaconBlock)
    {
      MyMultiplayerServerBase multiplayerServerBase = MyMultiplayer.Static as MyMultiplayerServerBase;
      playerIdentity = (MyIdentity) null;
      beaconBlock = (MyCubeBlock) null;
      MyCubeBlock entity;
      if (!MyEntities.TryGetEntityById<MyCubeBlock>(safeZoneBlockId, out entity))
      {
        multiplayerServerBase?.ValidationFailed(steamId, true, (string) null, true);
        return true;
      }
      beaconBlock = entity;
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(MySession.Static.Players.TryGetIdentityId(steamId, 0));
      if (identity == null || identity.Character == null)
      {
        multiplayerServerBase?.ValidationFailed(steamId, true, (string) null, true);
        return true;
      }
      playerIdentity = identity;
      return false;
    }

    public static MyEntity CrateSafeZone(
      MatrixD transform,
      MySafeZoneShape safeZoneShape,
      MySafeZoneAccess zoneAccess,
      long[] players,
      long[] factions,
      float startRadius,
      bool enable,
      bool isVisible = true,
      Vector3 color = default (Vector3),
      string visualTexture = "",
      long safeZoneBlockId = 0)
    {
      MyObjectBuilder_SafeZone objectBuilderSafeZone = new MyObjectBuilder_SafeZone();
      objectBuilderSafeZone.PositionAndOrientation = new MyPositionAndOrientation?(new MyPositionAndOrientation(transform));
      objectBuilderSafeZone.Radius = startRadius;
      objectBuilderSafeZone.PersistentFlags = MyPersistentEntityFlags2.InScene;
      objectBuilderSafeZone.Shape = safeZoneShape;
      objectBuilderSafeZone.AccessTypePlayers = zoneAccess;
      objectBuilderSafeZone.AccessTypeFactions = zoneAccess;
      objectBuilderSafeZone.AccessTypeGrids = zoneAccess;
      objectBuilderSafeZone.AccessTypeFloatingObjects = zoneAccess;
      objectBuilderSafeZone.IsVisible = isVisible;
      objectBuilderSafeZone.ModelColor = (SerializableVector3) color;
      if (!string.IsNullOrEmpty(visualTexture))
        objectBuilderSafeZone.Texture = visualTexture;
      if (players != null)
      {
        objectBuilderSafeZone.Players = players;
        objectBuilderSafeZone.Factions = factions;
      }
      objectBuilderSafeZone.Enabled = enable;
      objectBuilderSafeZone.SafeZoneBlockId = safeZoneBlockId;
      return MyEntities.CreateFromObjectBuilderAndAdd((MyObjectBuilder_EntityBase) objectBuilderSafeZone, false);
    }

    public static void RequestDeleteSafeZone(long entityId)
    {
      if (!MySession.Static.IsUserAdmin(Sync.MyId))
        return;
      MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (x => new Action<long>(MySessionComponentSafeZones.DeleteSafeZone_Implementation)), entityId);
    }

    [Event(null, 279)]
    [Reliable]
    [Server]
    public static void DeleteSafeZone_Implementation(long entityId)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserAdmin(MyEventContext.Current.Sender.Value))
      {
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        MyEntity entity = (MyEntity) null;
        if (!MyEntities.TryGetEntityById(entityId, out entity))
          return;
        entity.Close();
      }
    }

    public static void DeleteSafeZone_ImplementationPlayer(
      long safeZoneBlockId,
      long safeZoneId,
      ulong steamId)
    {
      if (!Sync.IsServer)
      {
        MyLog.Default.Error("CreateSafeZone_ImplementationPlayer can be only called by server.");
      }
      else
      {
        if (MySessionComponentSafeZones.IsPlayerValidationFailed(steamId, safeZoneBlockId, out MyIdentity _, out MyCubeBlock _))
          return;
        MyEntity entity = (MyEntity) null;
        if (!MyEntities.TryGetEntityById(safeZoneId, out entity))
          return;
        entity.Close();
      }
    }

    public static void RequestUpdateSafeZone(MyObjectBuilder_SafeZone ob)
    {
      if (!MySession.Static.IsUserAdmin(Sync.MyId))
        return;
      MyMultiplayer.RaiseStaticEvent<MyObjectBuilder_SafeZone>((Func<IMyEventOwner, Action<MyObjectBuilder_SafeZone>>) (x => new Action<MyObjectBuilder_SafeZone>(MySessionComponentSafeZones.UpdateSafeZone_Implementation)), ob);
    }

    public static void RequestUpdateSafeZonePlayer(
      long safeZoneBlockId,
      MyObjectBuilder_SafeZone ob)
    {
      MyMultiplayer.RaiseStaticEvent<long, MyObjectBuilder_SafeZone>((Func<IMyEventOwner, Action<long, MyObjectBuilder_SafeZone>>) (x => new Action<long, MyObjectBuilder_SafeZone>(MySessionComponentSafeZones.UpdateSafeZonePlayer_Implementation)), safeZoneBlockId, ob);
    }

    [Event(null, 329)]
    [Reliable]
    [Server]
    [Broadcast]
    public static void UpdateSafeZone_Implementation(MyObjectBuilder_SafeZone ob)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserAdmin(MyEventContext.Current.Sender.Value))
      {
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
        MyEventContext.ValidationFailed();
      }
      else
        MySessionComponentSafeZones.UpdateSafeZone(ob);
    }

    [Event(null, 343)]
    [Reliable]
    [Server]
    [Broadcast]
    public static void UpdateSafeZonePlayer_Implementation(
      long safeZoneBlockId,
      MyObjectBuilder_SafeZone ob)
    {
      MyIdentity playerIdentity;
      MyCubeBlock beaconBlock;
      if (!MyEventContext.Current.IsLocallyInvoked && !MySessionComponentSafeZones.IsPlayerValidationFailed(MyEventContext.Current.Sender.Value, safeZoneBlockId, out playerIdentity, out beaconBlock) && (!beaconBlock.GetUserRelationToOwner(playerIdentity.IdentityId).IsFriendly() && !MySession.Static.AdminSettings.HasFlag((Enum) AdminSettingsEnum.UseTerminals)))
      {
        if (MyMultiplayer.Static is MyMultiplayerServerBase multiplayerServerBase)
        {
          // ISSUE: explicit non-virtual call
          __nonvirtual (multiplayerServerBase.ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true));
        }
        MyEventContext.ValidationFailed();
      }
      else
        MySessionComponentSafeZones.UpdateSafeZone(ob);
    }

    public static void UpdateSafeZone(MyObjectBuilder_SafeZone ob, bool sync = false)
    {
      MySafeZone entity = (MySafeZone) null;
      if (MyEntities.TryGetEntityById<MySafeZone>(ob.EntityId, out entity))
      {
        if (MySessionComponentSafeZones.IsSafeZoneColliding(ob.EntityId, entity.PositionComp.WorldMatrixRef, ob.Shape, ob.Radius, ob.Size))
          return;
        entity.InitInternal(ob);
        Action<MySafeZone> onSafeZoneUpdated = MySessionComponentSafeZones.OnSafeZoneUpdated;
        if (onSafeZoneUpdated != null)
          onSafeZoneUpdated(entity);
      }
      if (!sync)
        return;
      MyMultiplayer.RaiseStaticEvent<MyObjectBuilder_SafeZone>((Func<IMyEventOwner, Action<MyObjectBuilder_SafeZone>>) (x => new Action<MyObjectBuilder_SafeZone>(MySessionComponentSafeZones.UpdateSafeZone_Broadcast)), ob);
    }

    [Event(null, 380)]
    [Reliable]
    [Broadcast]
    private static void UpdateSafeZone_Broadcast(MyObjectBuilder_SafeZone ob) => MySessionComponentSafeZones.UpdateSafeZone(ob);

    public static void RequestUpdateSafeZone_Player(
      long safeZoneBlockId,
      MyObjectBuilder_SafeZone ob)
    {
      MyMultiplayer.RaiseStaticEvent<long, MyObjectBuilder_SafeZone>((Func<IMyEventOwner, Action<long, MyObjectBuilder_SafeZone>>) (x => new Action<long, MyObjectBuilder_SafeZone>(MySessionComponentSafeZones.UpdateSafeZone_ImplementationPlayer)), safeZoneBlockId, ob);
    }

    [Event(null, 396)]
    [Reliable]
    [Server]
    [Broadcast]
    public static void UpdateSafeZone_ImplementationPlayer(
      long safezoneBlockId,
      MyObjectBuilder_SafeZone ob)
    {
      if (Sync.IsServer && MySessionComponentSafeZones.IsPlayerValidationFailed(MyEventContext.Current.Sender.Value, safezoneBlockId, out MyIdentity _, out MyCubeBlock _))
      {
        MyEventContext.ValidationFailed();
      }
      else
      {
        if (Sync.IsServer && ob.Texture != "SafeZone_Texture_Default")
        {
          MySessionComponentDLC component = MySession.Static.GetComponent<MySessionComponentDLC>();
          bool flag = true;
          if (component != null)
            flag = component.HasDLC("Economy", MyEventContext.Current.Sender.Value);
          if (!flag)
          {
            MyEventContext.ValidationFailed();
            return;
          }
        }
        MySessionComponentSafeZones.UpdateSafeZone(ob);
      }
    }

    public static void RequestUpdateSafeZoneRadius_Player(
      long safezoneBlockId,
      long safezoneId,
      float newRadius)
    {
      MyMultiplayer.RaiseStaticEvent<long, long, float>((Func<IMyEventOwner, Action<long, long, float>>) (x => new Action<long, long, float>(MySessionComponentSafeZones.UpdateSafeZoneRadius_ImplementationPlayer)), safezoneBlockId, safezoneId, newRadius);
    }

    [Event(null, 429)]
    [Reliable]
    [Server]
    [Broadcast]
    public static void UpdateSafeZoneRadius_ImplementationPlayer(
      long safezoneBlockId,
      long safezoneId,
      float radius)
    {
      if (Sync.IsServer && MySessionComponentSafeZones.IsPlayerValidationFailed(MyEventContext.Current.Sender.Value, safezoneBlockId, out MyIdentity _, out MyCubeBlock _))
      {
        MyEventContext.ValidationFailed();
      }
      else
      {
        MySafeZone entity = (MySafeZone) null;
        if (!MyEntities.TryGetEntityById<MySafeZone>(safezoneId, out entity) || MySessionComponentSafeZones.IsSafeZoneColliding(safezoneId, entity.PositionComp.WorldMatrixRef, MySafeZoneShape.Sphere, radius))
          return;
        MyObjectBuilder_SafeZone objectBuilder = (MyObjectBuilder_SafeZone) entity.GetObjectBuilder(false);
        objectBuilder.Radius = radius;
        entity.InitInternal(objectBuilder, Sync.IsServer);
        Action<MySafeZone> onSafeZoneUpdated = MySessionComponentSafeZones.OnSafeZoneUpdated;
        if (onSafeZoneUpdated == null)
          return;
        onSafeZoneUpdated(entity);
      }
    }

    public static bool IsSafeZoneColliding(
      long safeZoneId,
      MatrixD safeZoneWorld,
      MySafeZoneShape shape,
      float newRadius = 0.0f,
      Vector3 newSize = default (Vector3))
    {
      BoundingSphereD sphere1 = new BoundingSphereD(safeZoneWorld.Translation, (double) newRadius);
      MyOrientedBoundingBoxD orientedBoundingBoxD = new MyOrientedBoundingBoxD(MatrixD.CreateScale((Vector3D) newSize) * safeZoneWorld);
      foreach (MySafeZone safeZone in MySessionComponentSafeZones.m_safeZones)
      {
        if (safeZone.EntityId != safeZoneId)
        {
          int collisionCase = MySessionComponentSafeZones.GetCollisionCase(shape, safeZone.Shape);
          MatrixD matrixD = safeZone.PositionComp.WorldMatrixRef;
          BoundingSphereD sphere2 = new BoundingSphereD(matrixD.Translation, (double) safeZone.Radius);
          MyOrientedBoundingBoxD other = new MyOrientedBoundingBoxD(MatrixD.CreateScale((Vector3D) safeZone.Size) * matrixD);
          switch (collisionCase)
          {
            case 0:
              if (sphere1.Intersects(sphere2))
                return true;
              continue;
            case 1:
              if (orientedBoundingBoxD.Intersects(ref other))
                return true;
              continue;
            case 2:
              if (shape == MySafeZoneShape.Sphere)
              {
                if (other.Intersects(ref sphere1))
                  return true;
                continue;
              }
              if (orientedBoundingBoxD.Intersects(ref sphere2))
                return true;
              continue;
            default:
              continue;
          }
        }
      }
      return false;
    }

    private static int GetCollisionCase(MySafeZoneShape shape, MySafeZoneShape otherShape)
    {
      if (shape == MySafeZoneShape.Sphere && otherShape == MySafeZoneShape.Sphere)
        return 0;
      return shape == MySafeZoneShape.Box && otherShape == MySafeZoneShape.Box ? 1 : 2;
    }

    public static void RequestUpdateGlobalSafeZone()
    {
      if (MySession.Static.IsUserAdmin(Sync.MyId))
      {
        MyMultiplayer.RaiseStaticEvent<MySafeZoneAction>((Func<IMyEventOwner, Action<MySafeZoneAction>>) (x => new Action<MySafeZoneAction>(MySessionComponentSafeZones.UpdateGlobalSafeZone_Implementation)), MySessionComponentSafeZones.AllowedActions);
      }
      else
      {
        if (MyEventContext.Current.IsLocallyInvoked)
          return;
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
    }

    [Event(null, 529)]
    [Reliable]
    [Server]
    [Broadcast]
    public static void UpdateGlobalSafeZone_Implementation(MySafeZoneAction allowedActions)
    {
      if (!MySession.Static.IsUserAdmin(MyEventContext.Current.Sender.Value))
        MyEventContext.ValidationFailed();
      else
        MySessionComponentSafeZones.AllowedActions = allowedActions;
    }

    public static bool IsActionAllowed(
      MyEntity entity,
      MySafeZoneAction action,
      long sourceEntityId = 0,
      ulong user = 0)
    {
      if (user != 0UL)
      {
        if (MySession.Static.IsUserAdmin(user) && MySafeZone.CheckAdminIgnoreSafezones(user))
          return true;
      }
      else if (entity is MyCharacter myCharacter)
      {
        MyPlayer player = myCharacter.ControllerInfo?.Controller?.Player;
        if (player != null)
        {
          ulong steamId = player.Id.SteamId;
          if (MySession.Static.IsUserAdmin(steamId) && MySafeZone.CheckAdminIgnoreSafezones(steamId))
            return true;
        }
      }
      if ((MySessionComponentSafeZones.AllowedActions & action) != action)
        return false;
      foreach (MySafeZone safeZone in MySessionComponentSafeZones.m_safeZones)
      {
        if (!safeZone.IsActionAllowed(entity, action, sourceEntityId))
          return false;
      }
      return true;
    }

    public static bool IsActionAllowed(
      BoundingBoxD aabb,
      MySafeZoneAction action,
      long sourceEntityId = 0,
      ulong user = 0)
    {
      if (user != 0UL && MySession.Static.IsUserAdmin(user) && MySafeZone.CheckAdminIgnoreSafezones(user))
        return true;
      if ((MySessionComponentSafeZones.AllowedActions & action) != action)
        return false;
      foreach (MySafeZone safeZone in MySessionComponentSafeZones.m_safeZones)
      {
        if (!safeZone.IsActionAllowed(aabb, action, sourceEntityId))
          return false;
      }
      return true;
    }

    public static bool IsActionAllowed(
      Vector3D point,
      MySafeZoneAction action,
      long sourceEntityId = 0,
      ulong user = 0)
    {
      if (user != 0UL && MySession.Static.IsUserAdmin(user) && MySafeZone.CheckAdminIgnoreSafezones(user))
        return true;
      if ((MySessionComponentSafeZones.AllowedActions & action) != action)
        return false;
      foreach (MySafeZone safeZone in MySessionComponentSafeZones.m_safeZones)
      {
        if (!safeZone.IsActionAllowed(point, action, sourceEntityId))
          return false;
      }
      return true;
    }

    public static bool CanPerformAction(MySafeZoneAction action, ulong user = 0) => user != 0UL && MySession.Static.IsUserAdmin(user) && MySafeZone.CheckAdminIgnoreSafezones(user) || (MySessionComponentSafeZones.AllowedActions & action) == action;

    public override void LoadData()
    {
      base.LoadData();
      if (!Sync.IsServer)
        return;
      MyEntities.OnEntityAdd += new Action<MyEntity>(this.MyEntities_OnEntityAdd);
      MyEntities.OnEntityRemove += new Action<MyEntity>(this.MyEntities_OnEntityRemove);
    }

    private void MyEntities_OnEntityAdd(MyEntity obj)
    {
      if (obj.Physics != null && obj.Physics.IsStatic)
      {
        foreach (MySafeZone safeZone in MySessionComponentSafeZones.m_safeZones)
          safeZone.InsertEntity(obj);
      }
      MySessionComponentSafeZones.m_recentlyAddedEntities.Add(obj);
      this.m_recentCounter = 100;
    }

    private void MyEntities_OnEntityRemove(MyEntity obj)
    {
      if (obj.Physics != null && obj.Physics.IsStatic)
      {
        foreach (MySafeZone safeZone in MySessionComponentSafeZones.m_safeZones)
          safeZone.RemoveEntityInternal(obj, true);
      }
      MySessionComponentSafeZones.m_recentlyAddedEntities.Remove(obj);
      if (obj.MarkedForClose)
      {
        MySessionComponentSafeZones.m_recentlyRemovedEntities.Remove(obj);
      }
      else
      {
        MySessionComponentSafeZones.m_recentlyRemovedEntities.Add(obj);
        this.m_recentCounter = 100;
      }
    }

    protected override void UnloadData()
    {
      if (Sync.IsServer)
      {
        MyEntities.OnEntityAdd -= new Action<MyEntity>(this.MyEntities_OnEntityAdd);
        MyEntities.OnEntityRemove -= new Action<MyEntity>(this.MyEntities_OnEntityRemove);
      }
      MySessionComponentSafeZones.m_safeZones.Clear();
      MySessionComponentSafeZones.m_recentlyAddedEntities.Clear();
      MySessionComponentSafeZones.m_recentlyRemovedEntities.Clear();
      base.UnloadData();
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (this.m_recentCounter <= 0)
        return;
      --this.m_recentCounter;
      if (this.m_recentCounter != 0)
        return;
      MySessionComponentSafeZones.m_recentlyAddedEntities.Clear();
    }

    public static bool IsRecentlyAddedOrRemoved(MyEntity obj) => obj.MarkedForClose || MySessionComponentSafeZones.m_recentlyAddedEntities.Contains(obj) || MySessionComponentSafeZones.m_recentlyRemovedEntities.Contains(obj);

    protected sealed class CreateSafeZone_Implementation\u003C\u003EVRageMath_Vector3D : ICallSite<IMyEventOwner, Vector3D, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in Vector3D position,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentSafeZones.CreateSafeZone_Implementation(position);
      }
    }

    protected sealed class DeleteSafeZone_Implementation\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentSafeZones.DeleteSafeZone_Implementation(entityId);
      }
    }

    protected sealed class UpdateSafeZone_Implementation\u003C\u003ESandbox_Common_ObjectBuilders_MyObjectBuilder_SafeZone : ICallSite<IMyEventOwner, MyObjectBuilder_SafeZone, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyObjectBuilder_SafeZone ob,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentSafeZones.UpdateSafeZone_Implementation(ob);
      }
    }

    protected sealed class UpdateSafeZonePlayer_Implementation\u003C\u003ESystem_Int64\u0023Sandbox_Common_ObjectBuilders_MyObjectBuilder_SafeZone : ICallSite<IMyEventOwner, long, MyObjectBuilder_SafeZone, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long safeZoneBlockId,
        in MyObjectBuilder_SafeZone ob,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentSafeZones.UpdateSafeZonePlayer_Implementation(safeZoneBlockId, ob);
      }
    }

    protected sealed class UpdateSafeZone_Broadcast\u003C\u003ESandbox_Common_ObjectBuilders_MyObjectBuilder_SafeZone : ICallSite<IMyEventOwner, MyObjectBuilder_SafeZone, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyObjectBuilder_SafeZone ob,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentSafeZones.UpdateSafeZone_Broadcast(ob);
      }
    }

    protected sealed class UpdateSafeZone_ImplementationPlayer\u003C\u003ESystem_Int64\u0023Sandbox_Common_ObjectBuilders_MyObjectBuilder_SafeZone : ICallSite<IMyEventOwner, long, MyObjectBuilder_SafeZone, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long safezoneBlockId,
        in MyObjectBuilder_SafeZone ob,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentSafeZones.UpdateSafeZone_ImplementationPlayer(safezoneBlockId, ob);
      }
    }

    protected sealed class UpdateSafeZoneRadius_ImplementationPlayer\u003C\u003ESystem_Int64\u0023System_Int64\u0023System_Single : ICallSite<IMyEventOwner, long, long, float, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long safezoneBlockId,
        in long safezoneId,
        in float radius,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentSafeZones.UpdateSafeZoneRadius_ImplementationPlayer(safezoneBlockId, safezoneId, radius);
      }
    }

    protected sealed class UpdateGlobalSafeZone_Implementation\u003C\u003EVRage_Game_ObjectBuilders_Components_MySafeZoneAction : ICallSite<IMyEventOwner, MySafeZoneAction, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MySafeZoneAction allowedActions,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentSafeZones.UpdateGlobalSafeZone_Implementation(allowedActions);
      }
    }
  }
}
