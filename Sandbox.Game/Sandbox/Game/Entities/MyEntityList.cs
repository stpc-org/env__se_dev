// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyEntityList
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Physics;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Library.Collections;
using VRage.Library.Threading;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities
{
  public static class MyEntityList
  {
    [ThreadStatic]
    private static MyEntityList.MyEntityListInfoItem m_gridItem;

    private static MyEntityList.MyEntityListInfoItem GenerateInfo_Grid(
      MyEntity entity,
      ref ICollection<MyPlayer> players)
    {
      if (!(entity is MyCubeGrid grid))
        return (MyEntityList.MyEntityListInfoItem) null;
      MyCubeGrid mechanicalRootGrid = MyEntityList.GetMechanicalRootGrid(grid);
      if (grid.Closed || grid.Physics == null || mechanicalRootGrid != grid)
        return (MyEntityList.MyEntityListInfoItem) null;
      MyEntityList.CreateListInfoForGrid(grid, out MyEntityList.m_gridItem);
      MyEntityList.AccountChildren(grid);
      return MyEntityList.m_gridItem;
    }

    private static List<MyEntityList.MyEntityListInfoItem> GenerateInfo_Character(
      MyIdentity identity,
      bool? IsReplicatedFilter = null)
    {
      List<MyEntityList.MyEntityListInfoItem> entityListInfoItemList = new List<MyEntityList.MyEntityListInfoItem>();
      string displayName = identity.DisplayName;
      MyPlayer.PlayerId result;
      if (Sync.Players.TryGetPlayerId(identity.IdentityId, out result))
      {
        MyPlayer player = (MyPlayer) null;
        if (!Sync.Players.TryGetPlayerById(result, out player))
          displayName = displayName + " (" + (object) MyTexts.Get(MyCommonTexts.OfflineStatus) + ")";
      }
      if (identity.Character != null)
      {
        if (!IsReplicatedFilter.HasValue || IsReplicatedFilter.Value == identity.Character.IsReplicated)
          entityListInfoItemList.Add(new MyEntityList.MyEntityListInfoItem(displayName, identity.Character.EntityId, 0, new int?(identity.BlockLimits.PCU), identity.Character.CurrentMass, identity.Character.PositionComp.GetPosition(), identity.Character.Physics.LinearVelocity.Length(), 0.0f, identity.DisplayName, identity.IdentityId, (float) (int) (DateTime.Now - identity.LastLoginTime).TotalSeconds, new float?((float) (int) (DateTime.Now - identity.LastLogoutTime).TotalSeconds), isReplicated: new bool?(identity.Character.IsReplicated)));
      }
      else
      {
        foreach (long savedCharacter in identity.SavedCharacters)
        {
          MyCharacter entity;
          if (MyEntities.TryGetEntityById<MyCharacter>(savedCharacter, out entity) && (!IsReplicatedFilter.HasValue || IsReplicatedFilter.Value == entity.IsReplicated))
            entityListInfoItemList.Add(new MyEntityList.MyEntityListInfoItem(displayName, savedCharacter, 0, new int?(), entity.CurrentMass, entity.PositionComp.GetPosition(), entity.Physics.LinearVelocity.Length(), 0.0f, identity.DisplayName, identity.IdentityId, (float) (int) (DateTime.Now - identity.LastLoginTime).TotalSeconds, new float?((float) (int) (DateTime.Now - identity.LastLogoutTime).TotalSeconds), isReplicated: new bool?(entity.IsReplicated)));
        }
      }
      return entityListInfoItemList;
    }

    private static MyEntityList.MyEntityListInfoItem GenerateInfo_FloatingObject(
      MyEntity entity,
      ref ICollection<MyPlayer> players)
    {
      return !(entity is MyFloatingObject myFloatingObject) || myFloatingObject.Closed || myFloatingObject.Physics == null ? (MyEntityList.MyEntityListInfoItem) null : new MyEntityList.MyEntityListInfoItem(myFloatingObject.DisplayName, myFloatingObject.EntityId, 0, new int?(), myFloatingObject.Physics.Mass, myFloatingObject.PositionComp.GetPosition(), myFloatingObject.Physics.LinearVelocity.Length(), MySession.GetPlayerDistance((MyEntity) myFloatingObject, players), "", 0L, 0.0f, new float?(), isReplicated: new bool?(myFloatingObject.IsReplicated));
    }

    private static MyEntityList.MyEntityListInfoItem GenerateInfo_Bag(
      MyEntity entity,
      ref ICollection<MyPlayer> players)
    {
      if (!(entity is MyInventoryBagEntity inventoryBagEntity))
        return (MyEntityList.MyEntityListInfoItem) null;
      if (inventoryBagEntity.Closed || inventoryBagEntity.Physics == null)
        return (MyEntityList.MyEntityListInfoItem) null;
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(inventoryBagEntity.OwnerIdentityId);
      string ownerName = "";
      float ownerLogin = 0.0f;
      float num = 0.0f;
      if (identity != null)
      {
        ownerName = identity.DisplayName;
        ownerLogin = (float) (int) (DateTime.Now - identity.LastLoginTime).TotalSeconds;
        num = (float) (int) (DateTime.Now - identity.LastLogoutTime).TotalSeconds;
      }
      return new MyEntityList.MyEntityListInfoItem(inventoryBagEntity.DisplayName, inventoryBagEntity.EntityId, 0, new int?(), inventoryBagEntity.Physics.Mass, inventoryBagEntity.PositionComp.GetPosition(), inventoryBagEntity.Physics.LinearVelocity.Length(), MySession.GetPlayerDistance((MyEntity) inventoryBagEntity, players), ownerName, inventoryBagEntity.OwnerIdentityId, ownerLogin, new float?(num), isReplicated: new bool?(inventoryBagEntity.IsReplicated));
    }

    private static MyEntityList.MyEntityListInfoItem GenerateInfo_Planet(
      MyEntity entity,
      ref ICollection<MyPlayer> players)
    {
      return !(entity is MyPlanet myPlanet) || myPlanet.Closed ? (MyEntityList.MyEntityListInfoItem) null : new MyEntityList.MyEntityListInfoItem(myPlanet.StorageName, myPlanet.EntityId, 0, new int?(), 0.0f, myPlanet.PositionComp.GetPosition(), 0.0f, MySession.GetPlayerDistance((MyEntity) myPlanet, players), "", 0L, 0.0f, new float?(), isReplicated: new bool?(myPlanet.IsReplicated));
    }

    private static MyEntityList.MyEntityListInfoItem GenerateInfo_Asteroid(
      MyEntity entity,
      ref ICollection<MyPlayer> players)
    {
      return !(entity is MyVoxelBase myVoxelBase) || myVoxelBase is MyPlanet || myVoxelBase.Closed ? (MyEntityList.MyEntityListInfoItem) null : new MyEntityList.MyEntityListInfoItem(myVoxelBase.StorageName, myVoxelBase.EntityId, 0, new int?(), 0.0f, myVoxelBase.PositionComp.GetPosition(), 0.0f, MySession.GetPlayerDistance((MyEntity) myVoxelBase, players), "", 0L, 0.0f, new float?(), isReplicated: new bool?(myVoxelBase.IsReplicated));
    }

    public static List<MyEntityList.MyEntityListInfoItem> GetEntityList(
      MyEntityList.MyEntityTypeEnum selectedType)
    {
      MyConcurrentHashSet<MyEntity> entities = MyEntities.GetEntities();
      List<MyEntityList.MyEntityListInfoItem> entityListInfoItemList = new List<MyEntityList.MyEntityListInfoItem>(entities.Count);
      ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
      switch (selectedType)
      {
        case MyEntityList.MyEntityTypeEnum.Grids:
        case MyEntityList.MyEntityTypeEnum.SmallGrids:
        case MyEntityList.MyEntityTypeEnum.LargeGrids:
          using (ConcurrentEnumerator<SpinLockRef.Token, MyEntity, HashSet<MyEntity>.Enumerator> enumerator = entities.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              MyEntity current = enumerator.Current;
              if (current is MyCubeGrid myCubeGrid && (selectedType != MyEntityList.MyEntityTypeEnum.LargeGrids || myCubeGrid.GridSizeEnum != MyCubeSize.Small) && (selectedType != MyEntityList.MyEntityTypeEnum.SmallGrids || myCubeGrid.GridSizeEnum != MyCubeSize.Large))
              {
                MyEntityList.MyEntityListInfoItem infoGrid = MyEntityList.GenerateInfo_Grid(current, ref onlinePlayers);
                if (infoGrid != null)
                  entityListInfoItemList.Add(infoGrid);
              }
            }
            break;
          }
        case MyEntityList.MyEntityTypeEnum.Characters:
          using (IEnumerator<MyIdentity> enumerator = MySession.Static.Players.GetAllIdentities().GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              List<MyEntityList.MyEntityListInfoItem> infoCharacter = MyEntityList.GenerateInfo_Character(enumerator.Current);
              if (infoCharacter != null && infoCharacter.Count > 0)
              {
                foreach (MyEntityList.MyEntityListInfoItem entityListInfoItem in infoCharacter)
                  entityListInfoItemList.Add(entityListInfoItem);
              }
            }
            break;
          }
        case MyEntityList.MyEntityTypeEnum.FloatingObjects:
          using (ConcurrentEnumerator<SpinLockRef.Token, MyEntity, HashSet<MyEntity>.Enumerator> enumerator = entities.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              MyEntity current = enumerator.Current;
              MyEntityList.MyEntityListInfoItem infoFloatingObject = MyEntityList.GenerateInfo_FloatingObject(current, ref onlinePlayers);
              if (infoFloatingObject != null)
                entityListInfoItemList.Add(infoFloatingObject);
              MyEntityList.MyEntityListInfoItem infoBag = MyEntityList.GenerateInfo_Bag(current, ref onlinePlayers);
              if (infoBag != null)
                entityListInfoItemList.Add(infoBag);
            }
            break;
          }
        case MyEntityList.MyEntityTypeEnum.Planets:
          using (ConcurrentEnumerator<SpinLockRef.Token, MyEntity, HashSet<MyEntity>.Enumerator> enumerator = entities.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              MyEntityList.MyEntityListInfoItem infoPlanet = MyEntityList.GenerateInfo_Planet(enumerator.Current, ref onlinePlayers);
              if (infoPlanet != null)
                entityListInfoItemList.Add(infoPlanet);
            }
            break;
          }
        case MyEntityList.MyEntityTypeEnum.Asteroids:
          using (ConcurrentEnumerator<SpinLockRef.Token, MyEntity, HashSet<MyEntity>.Enumerator> enumerator = entities.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              MyEntityList.MyEntityListInfoItem infoAsteroid = MyEntityList.GenerateInfo_Asteroid(enumerator.Current, ref onlinePlayers);
              if (infoAsteroid != null)
                entityListInfoItemList.Add(infoAsteroid);
            }
            break;
          }
        case MyEntityList.MyEntityTypeEnum.Replicated:
        case MyEntityList.MyEntityTypeEnum.NotReplicated:
          bool? IsReplicatedFilter = new bool?();
          if (selectedType == MyEntityList.MyEntityTypeEnum.Replicated)
            IsReplicatedFilter = new bool?(true);
          else if (selectedType == MyEntityList.MyEntityTypeEnum.NotReplicated)
            IsReplicatedFilter = new bool?(false);
          foreach (MyEntity entity in entities)
          {
            if ((entity.IsReplicated || selectedType != MyEntityList.MyEntityTypeEnum.Replicated) && (!entity.IsReplicated || selectedType != MyEntityList.MyEntityTypeEnum.NotReplicated))
            {
              MyEntityList.MyEntityListInfoItem infoGrid = MyEntityList.GenerateInfo_Grid(entity, ref onlinePlayers);
              if (infoGrid != null)
              {
                entityListInfoItemList.Add(infoGrid);
              }
              else
              {
                MyEntityList.MyEntityListInfoItem infoFloatingObject = MyEntityList.GenerateInfo_FloatingObject(entity, ref onlinePlayers);
                if (infoFloatingObject != null)
                {
                  entityListInfoItemList.Add(infoFloatingObject);
                }
                else
                {
                  MyEntityList.MyEntityListInfoItem infoBag = MyEntityList.GenerateInfo_Bag(entity, ref onlinePlayers);
                  if (infoBag != null)
                  {
                    entityListInfoItemList.Add(infoBag);
                  }
                  else
                  {
                    MyEntityList.MyEntityListInfoItem infoPlanet = MyEntityList.GenerateInfo_Planet(entity, ref onlinePlayers);
                    if (infoPlanet != null)
                    {
                      entityListInfoItemList.Add(infoPlanet);
                    }
                    else
                    {
                      MyEntityList.MyEntityListInfoItem infoAsteroid = MyEntityList.GenerateInfo_Asteroid(entity, ref onlinePlayers);
                      if (infoAsteroid != null)
                        entityListInfoItemList.Add(infoAsteroid);
                    }
                  }
                }
              }
            }
          }
          using (IEnumerator<MyIdentity> enumerator = MySession.Static.Players.GetAllIdentities().GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              List<MyEntityList.MyEntityListInfoItem> infoCharacter = MyEntityList.GenerateInfo_Character(enumerator.Current, IsReplicatedFilter);
              if (infoCharacter != null && infoCharacter.Count > 0)
              {
                foreach (MyEntityList.MyEntityListInfoItem entityListInfoItem in infoCharacter)
                  entityListInfoItemList.Add(entityListInfoItem);
              }
            }
            break;
          }
        default:
          throw new ArgumentOutOfRangeException();
      }
      return entityListInfoItemList;
    }

    private static MyCubeGrid GetMechanicalRootGrid(MyCubeGrid grid)
    {
      MyCubeGrid myCubeGrid = (MyCubeGrid) null;
      foreach (MyCubeGrid groupNode in MyCubeGridGroups.Static.Mechanical.GetGroupNodes(grid))
      {
        if (myCubeGrid == null || groupNode.CubeBlocks.Count > myCubeGrid.CubeBlocks.Count)
          myCubeGrid = groupNode;
      }
      return myCubeGrid;
    }

    public static string GetDescriptionText(MyEntityList.MyEntityListInfoItem item)
    {
      StringBuilder output = new StringBuilder();
      if (!item.IsGrid)
      {
        output.Append(MyEntityList.MyEntitySortOrder.Mass.ToString() + ": ");
        if ((double) item.Mass > 0.0)
          MyValueFormatter.AppendWeightInBestUnit(item.Mass, output);
        else
          output.Append("-");
        output.AppendLine();
        output.Append(MyTexts.Get(MyStringId.GetOrCompute(MyEntityList.MyEntitySortOrder.DistanceFromCenter.ToString())).ToString() + ": ");
        MyValueFormatter.AppendDistanceInBestUnit((float) item.Position.Length(), output);
        output.AppendLine();
        output.Append(MyTexts.Get(MyStringId.GetOrCompute(MyEntityList.MyEntitySortOrder.Speed.ToString())).ToString() + ": " + (object) item.Speed + " m/s");
        output.AppendLine();
        output.Append((object) MyTexts.Get(MySpaceTexts.TieredUpdate_IsReplicated));
        output.Append((object) MyTexts.Get(!item.IsReplicated.HasValue ? MySpaceTexts.TieredUpdate_IsReplicated_NA : (item.IsReplicated.Value ? MySpaceTexts.TieredUpdate_IsReplicated_True : MySpaceTexts.TieredUpdate_IsReplicated_False)));
      }
      else
      {
        output.AppendLine(MyTexts.Get(MyStringId.GetOrCompute(MyEntityList.MyEntitySortOrder.BlockCount.ToString())).ToString() + ": " + (object) item.BlockCount);
        if (item.PCU.HasValue && item.PCU.HasValue)
          output.AppendLine(MyTexts.Get(MyStringId.GetOrCompute(MyEntityList.MyEntitySortOrder.PCU.ToString())).ToString() + ": " + (object) item.PCU.Value);
        output.Append(MyTexts.Get(MyStringId.GetOrCompute(MyEntityList.MyEntitySortOrder.Mass.ToString())).ToString() + ": ");
        if ((double) item.Mass > 0.0)
          MyValueFormatter.AppendWeightInBestUnit(item.Mass, output);
        else
          output.Append("-");
        output.AppendLine();
        output.AppendLine(MyTexts.Get(MyStringId.GetOrCompute(MyEntityList.MyEntitySortOrder.OwnerName.ToString())).ToString() + ": " + item.OwnerName);
        StringBuilder stringBuilder1 = output;
        object[] objArray = new object[4];
        MyEntityList.MyEntitySortOrder myEntitySortOrder = MyEntityList.MyEntitySortOrder.Speed;
        objArray[0] = (object) MyTexts.Get(MyStringId.GetOrCompute(myEntitySortOrder.ToString()));
        objArray[1] = (object) ": ";
        objArray[2] = (object) item.Speed;
        objArray[3] = (object) " m/s";
        string str1 = string.Concat(objArray);
        stringBuilder1.AppendLine(str1);
        StringBuilder stringBuilder2 = output;
        myEntitySortOrder = MyEntityList.MyEntitySortOrder.DistanceFromCenter;
        string str2 = MyTexts.Get(MyStringId.GetOrCompute(myEntitySortOrder.ToString())).ToString() + ": ";
        stringBuilder2.Append(str2);
        MyValueFormatter.AppendDistanceInBestUnit((float) item.Position.Length(), output);
        output.AppendLine();
        StringBuilder stringBuilder3 = output;
        myEntitySortOrder = MyEntityList.MyEntitySortOrder.DistanceFromPlayers;
        string str3 = MyTexts.Get(MyStringId.GetOrCompute(myEntitySortOrder.ToString())).ToString() + ": ";
        stringBuilder3.Append(str3);
        MyValueFormatter.AppendDistanceInBestUnit(item.DistanceFromPlayers, output);
        output.AppendLine();
        StringBuilder stringBuilder4 = output;
        myEntitySortOrder = MyEntityList.MyEntitySortOrder.OwnerLastLogout;
        string str4 = MyTexts.Get(MyStringId.GetOrCompute(myEntitySortOrder.ToString())).ToString() + ": ";
        stringBuilder4.Append(str4);
        if (item.OwnerLogoutTime.HasValue && item.OwnerLogoutTime.HasValue)
          MyValueFormatter.AppendTimeInBestUnit(item.OwnerLogoutTime.Value, output);
        string str5 = item.PlayerPresenceTier.ToString();
        string str6 = item.GridPresenceTier.ToString();
        output.AppendLine();
        output.Append((object) MyTexts.Get(MySpaceTexts.TieredUpdate_PlayerPresenceTier));
        output.Append(str5);
        output.AppendLine();
        output.Append((object) MyTexts.Get(MySpaceTexts.TieredUpdate_GridPresenceTier));
        output.Append(str6);
        output.AppendLine();
        output.Append((object) MyTexts.Get(MySpaceTexts.TieredUpdate_IsReplicated));
        output.Append((object) MyTexts.Get(!item.IsReplicated.HasValue ? MySpaceTexts.TieredUpdate_IsReplicated_NA : (item.IsReplicated.Value ? MySpaceTexts.TieredUpdate_IsReplicated_True : MySpaceTexts.TieredUpdate_IsReplicated_False)));
      }
      return output.ToString();
    }

    public static StringBuilder GetFormattedDisplayName(
      MyEntityList.MyEntitySortOrder selectedOrder,
      MyEntityList.MyEntityListInfoItem item)
    {
      StringBuilder output = new StringBuilder(item.DisplayName);
      switch (selectedOrder)
      {
        case MyEntityList.MyEntitySortOrder.DisplayName:
          return output;
        case MyEntityList.MyEntitySortOrder.BlockCount:
          if (item.IsGrid)
          {
            output.Append(" | " + (object) item.BlockCount);
            goto case MyEntityList.MyEntitySortOrder.DisplayName;
          }
          else
            goto case MyEntityList.MyEntitySortOrder.DisplayName;
        case MyEntityList.MyEntitySortOrder.Mass:
          output.Append(" | ");
          if ((double) item.Mass == 0.0)
          {
            output.Append("-");
            goto case MyEntityList.MyEntitySortOrder.DisplayName;
          }
          else
          {
            MyValueFormatter.AppendWeightInBestUnit(item.Mass, output);
            goto case MyEntityList.MyEntitySortOrder.DisplayName;
          }
        case MyEntityList.MyEntitySortOrder.OwnerName:
          if (item.IsGrid)
          {
            output.Append(" | " + (string.IsNullOrEmpty(item.OwnerName) ? MyTexts.GetString(MySpaceTexts.BlockOwner_Nobody) : item.OwnerName));
            goto case MyEntityList.MyEntitySortOrder.DisplayName;
          }
          else
            goto case MyEntityList.MyEntitySortOrder.DisplayName;
        case MyEntityList.MyEntitySortOrder.DistanceFromCenter:
          output.Append(" | ");
          MyValueFormatter.AppendDistanceInBestUnit((float) item.Position.Length(), output);
          goto case MyEntityList.MyEntitySortOrder.DisplayName;
        case MyEntityList.MyEntitySortOrder.Speed:
          output.Append(" | " + item.Speed.ToString("0.### m/s"));
          goto case MyEntityList.MyEntitySortOrder.DisplayName;
        case MyEntityList.MyEntitySortOrder.DistanceFromPlayers:
          output.Append(" | ");
          MyValueFormatter.AppendDistanceInBestUnit(item.DistanceFromPlayers, output);
          goto case MyEntityList.MyEntitySortOrder.DisplayName;
        case MyEntityList.MyEntitySortOrder.OwnerLastLogout:
          if (item.OwnerLogoutTime.HasValue && item.OwnerLogoutTime.HasValue && (double) item.OwnerLogoutTime.Value >= 0.0)
          {
            if (item.OwnerName != item.DisplayName)
            {
              output.Append(" | " + (string.IsNullOrEmpty(item.OwnerName) ? MyTexts.GetString(MySpaceTexts.BlockOwner_Nobody) : item.OwnerName));
              output.Append(": ");
            }
            else
              output.Append(" | ");
            MyValueFormatter.AppendTimeInBestUnit(item.OwnerLogoutTime.Value, output);
            goto case MyEntityList.MyEntitySortOrder.DisplayName;
          }
          else
            goto case MyEntityList.MyEntitySortOrder.DisplayName;
        case MyEntityList.MyEntitySortOrder.PCU:
          if (item.PCU.HasValue && item.PCU.HasValue)
          {
            string str = item.PCU.Value == int.MaxValue ? "N/A" : item.PCU.Value.ToString();
            output.Append(" | " + str);
            goto case MyEntityList.MyEntitySortOrder.DisplayName;
          }
          else
            goto case MyEntityList.MyEntitySortOrder.DisplayName;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public static void SortEntityList(
      MyEntityList.MyEntitySortOrder selectedOrder,
      ref List<MyEntityList.MyEntityListInfoItem> items,
      bool invertOrder)
    {
      switch (selectedOrder)
      {
        case MyEntityList.MyEntitySortOrder.DisplayName:
          items.Sort((Comparison<MyEntityList.MyEntityListInfoItem>) ((a, b) =>
          {
            int num = string.Compare(a.DisplayName, b.DisplayName, StringComparison.CurrentCultureIgnoreCase);
            return invertOrder ? -num : num;
          }));
          break;
        case MyEntityList.MyEntitySortOrder.BlockCount:
          items.Sort((Comparison<MyEntityList.MyEntityListInfoItem>) ((a, b) =>
          {
            int num = b.BlockCount.CompareTo(a.BlockCount);
            return invertOrder ? -num : num;
          }));
          break;
        case MyEntityList.MyEntitySortOrder.Mass:
          items.Sort((Comparison<MyEntityList.MyEntityListInfoItem>) ((a, b) =>
          {
            if ((double) a.Mass == (double) b.Mass)
              return 0;
            int num = (double) a.Mass != 0.0 ? ((double) b.Mass != 0.0 ? b.Mass.CompareTo(a.Mass) : 1) : -1;
            return invertOrder ? -num : num;
          }));
          break;
        case MyEntityList.MyEntitySortOrder.OwnerName:
          items.Sort((Comparison<MyEntityList.MyEntityListInfoItem>) ((a, b) =>
          {
            int num = string.Compare(a.OwnerName, b.OwnerName, StringComparison.CurrentCultureIgnoreCase);
            return invertOrder ? -num : num;
          }));
          break;
        case MyEntityList.MyEntitySortOrder.DistanceFromCenter:
          items.Sort((Comparison<MyEntityList.MyEntityListInfoItem>) ((a, b) =>
          {
            int num = a.Position.LengthSquared().CompareTo(b.Position.LengthSquared());
            return invertOrder ? -num : num;
          }));
          break;
        case MyEntityList.MyEntitySortOrder.Speed:
          items.Sort((Comparison<MyEntityList.MyEntityListInfoItem>) ((a, b) =>
          {
            int num = b.Speed.CompareTo(a.Speed);
            return invertOrder ? -num : num;
          }));
          break;
        case MyEntityList.MyEntitySortOrder.DistanceFromPlayers:
          items.Sort((Comparison<MyEntityList.MyEntityListInfoItem>) ((a, b) =>
          {
            int num = b.DistanceFromPlayers.CompareTo(a.DistanceFromPlayers);
            return invertOrder ? -num : num;
          }));
          break;
        case MyEntityList.MyEntitySortOrder.OwnerLastLogout:
          items.Sort((Comparison<MyEntityList.MyEntityListInfoItem>) ((a, b) =>
          {
            if (!b.OwnerLogoutTime.HasValue || !b.OwnerLogoutTime.HasValue || (!a.OwnerLogoutTime.HasValue || !a.OwnerLogoutTime.HasValue))
              return 1;
            int num = b.OwnerLogoutTime.Value.CompareTo(a.OwnerLogoutTime.Value);
            return invertOrder ? -num : num;
          }));
          break;
        case MyEntityList.MyEntitySortOrder.PCU:
          items.Sort((Comparison<MyEntityList.MyEntityListInfoItem>) ((a, b) =>
          {
            if (!b.PCU.HasValue || !b.PCU.HasValue || (!a.PCU.HasValue || !a.PCU.HasValue))
              return 1;
            int num = b.PCU.Value.CompareTo(a.PCU.Value);
            return invertOrder ? -num : num;
          }));
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public static void ProceedEntityAction(MyEntity entity, MyEntityList.EntityListAction action)
    {
      if (entity is MyCubeGrid grid)
      {
        if (!(grid.GetTopMostParent((Type) null) is MyCubeGrid myCubeGrid))
          myCubeGrid = grid;
        if (myCubeGrid.IsGenerated)
          return;
        if (action == MyEntityList.EntityListAction.Remove)
          grid.DismountAllCockpits();
        MyEntityList.ProceedEntityActionHierarchy(MyGridPhysicalHierarchy.Static.GetRoot(grid), action);
      }
      else
        MyEntityList.ProceedEntityActionInternal(entity, action);
    }

    private static void ProceedEntityActionHierarchy(
      MyCubeGrid grid,
      MyEntityList.EntityListAction action)
    {
      MyGridPhysicalHierarchy.Static.ApplyOnChildren(grid, (Action<MyCubeGrid>) (x => MyEntityList.ProceedEntityActionHierarchy(x, action)));
      MyEntityList.ProceedEntityActionInternal((MyEntity) grid, action);
    }

    private static void ProceedEntityActionInternal(
      MyEntity entity,
      MyEntityList.EntityListAction action)
    {
      switch (action)
      {
        case MyEntityList.EntityListAction.Remove:
          entity.Close();
          break;
        case MyEntityList.EntityListAction.Stop:
          MyEntityList.Stop(entity);
          break;
        case MyEntityList.EntityListAction.Depower:
          MyEntityList.Depower(entity);
          break;
        case MyEntityList.EntityListAction.Power:
          MyEntityList.Power(entity);
          break;
      }
    }

    private static void Stop(MyEntity entity)
    {
      if (entity.Physics == null)
        return;
      entity.Physics.LinearVelocity = Vector3.Zero;
      entity.Physics.AngularVelocity = Vector3.Zero;
    }

    private static void Depower(MyEntity entity)
    {
      if (!(entity is MyCubeGrid myCubeGrid))
        return;
      myCubeGrid.ChangePowerProducerState(MyMultipleEnabledEnum.AllDisabled, -1L);
    }

    private static void Power(MyEntity entity)
    {
      if (!(entity is MyCubeGrid myCubeGrid))
        return;
      myCubeGrid.ChangePowerProducerState(MyMultipleEnabledEnum.AllEnabled, -1L);
    }

    private static void AccountChildren(MyCubeGrid grid) => MyGridPhysicalHierarchy.Static.ApplyOnChildren(grid, (Action<MyCubeGrid>) (childGrid =>
    {
      MyEntityList.MyEntityListInfoItem entityListInfoItem;
      MyEntityList.CreateListInfoForGrid(childGrid, out entityListInfoItem);
      MyEntityList.m_gridItem.Add(ref entityListInfoItem);
      MyEntityList.AccountChildren(childGrid);
    }));

    private static void CreateListInfoForGrid(
      MyCubeGrid grid,
      out MyEntityList.MyEntityListInfoItem item)
    {
      long owner = 0;
      string ownerName = string.Empty;
      if (grid.BigOwners.Count > 0)
      {
        MyIdentity identity = MySession.Static.Players.TryGetIdentity(grid.BigOwners[0]);
        if (identity != null)
        {
          ownerName = identity.DisplayName;
          owner = grid.BigOwners[0];
        }
      }
      MyUpdateTiersPlayerPresence playerPresenceTier = grid.PlayerPresenceTier;
      MyUpdateTiersGridPresence gridPresenceTier = grid.GridPresenceTier;
      item = new MyEntityList.MyEntityListInfoItem(grid.DisplayName, grid.EntityId, grid.BlocksCount, new int?(grid.BlocksPCU), grid.Physics.Mass, grid.PositionComp.GetPosition(), grid.Physics.LinearVelocity.Length(), MySession.GetPlayerDistance((MyEntity) grid, MySession.Static.Players.GetOnlinePlayers()), ownerName, owner, MySession.GetOwnerLoginTimeSeconds(grid), new float?(MySession.GetOwnerLogoutTimeSeconds(grid)), playerPresenceTier, gridPresenceTier, new bool?(grid.IsReplicated), true);
    }

    [Serializable]
    public class MyEntityListInfoItem
    {
      public string DisplayName;
      public long EntityId;
      public int BlockCount;
      public int? PCU;
      public float Mass;
      public Vector3D Position;
      public string OwnerName;
      public long Owner;
      public float Speed;
      public float DistanceFromPlayers;
      public float OwnerLoginTime;
      public float? OwnerLogoutTime;
      public MyUpdateTiersPlayerPresence PlayerPresenceTier;
      public MyUpdateTiersGridPresence GridPresenceTier;
      public bool? IsReplicated;
      public bool IsGrid;

      public MyEntityListInfoItem()
      {
      }

      public MyEntityListInfoItem(
        string displayName,
        long entityId,
        int blockCount,
        int? pcu,
        float mass,
        Vector3D position,
        float speed,
        float distanceFromPlayers,
        string ownerName,
        long owner,
        float ownerLogin,
        float? ownerLogout,
        MyUpdateTiersPlayerPresence playerPresenceTier = MyUpdateTiersPlayerPresence.Normal,
        MyUpdateTiersGridPresence gridPresenceTier = MyUpdateTiersGridPresence.Normal,
        bool? isReplicated = null,
        bool isGrid = false)
      {
        this.DisplayName = !string.IsNullOrEmpty(displayName) ? (displayName.Length < 50 ? displayName : displayName.Substring(0, 49)) : "----";
        this.EntityId = entityId;
        this.BlockCount = blockCount;
        this.PCU = pcu;
        this.Mass = mass;
        this.Position = position;
        this.OwnerName = ownerName;
        this.Owner = owner;
        this.Speed = speed;
        this.DistanceFromPlayers = distanceFromPlayers;
        this.OwnerLoginTime = ownerLogin;
        this.OwnerLogoutTime = ownerLogout;
        this.PlayerPresenceTier = playerPresenceTier;
        this.GridPresenceTier = gridPresenceTier;
        this.IsReplicated = isReplicated;
        this.IsGrid = isGrid;
      }

      public void Add(ref MyEntityList.MyEntityListInfoItem item)
      {
        this.BlockCount += item.BlockCount;
        if (item.PCU.HasValue && item.PCU.HasValue)
        {
          int? pcu = this.PCU;
          int num = item.PCU.Value;
          this.PCU = pcu.HasValue ? new int?(pcu.GetValueOrDefault() + num) : new int?();
        }
        this.Mass += item.Mass;
        this.OwnerLoginTime = Math.Min(item.OwnerLoginTime, this.OwnerLoginTime);
        if (!item.OwnerLogoutTime.HasValue || !item.OwnerLogoutTime.HasValue)
          return;
        this.OwnerLogoutTime = new float?(Math.Min(item.OwnerLogoutTime.Value, this.OwnerLogoutTime.Value));
      }

      protected class Sandbox_Game_Entities_MyEntityList\u003C\u003EMyEntityListInfoItem\u003C\u003EDisplayName\u003C\u003EAccessor : IMemberAccessor<MyEntityList.MyEntityListInfoItem, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyEntityList.MyEntityListInfoItem owner, in string value) => owner.DisplayName = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyEntityList.MyEntityListInfoItem owner, out string value) => value = owner.DisplayName;
      }

      protected class Sandbox_Game_Entities_MyEntityList\u003C\u003EMyEntityListInfoItem\u003C\u003EEntityId\u003C\u003EAccessor : IMemberAccessor<MyEntityList.MyEntityListInfoItem, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyEntityList.MyEntityListInfoItem owner, in long value) => owner.EntityId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyEntityList.MyEntityListInfoItem owner, out long value) => value = owner.EntityId;
      }

      protected class Sandbox_Game_Entities_MyEntityList\u003C\u003EMyEntityListInfoItem\u003C\u003EBlockCount\u003C\u003EAccessor : IMemberAccessor<MyEntityList.MyEntityListInfoItem, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyEntityList.MyEntityListInfoItem owner, in int value) => owner.BlockCount = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyEntityList.MyEntityListInfoItem owner, out int value) => value = owner.BlockCount;
      }

      protected class Sandbox_Game_Entities_MyEntityList\u003C\u003EMyEntityListInfoItem\u003C\u003EPCU\u003C\u003EAccessor : IMemberAccessor<MyEntityList.MyEntityListInfoItem, int?>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyEntityList.MyEntityListInfoItem owner, in int? value) => owner.PCU = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyEntityList.MyEntityListInfoItem owner, out int? value) => value = owner.PCU;
      }

      protected class Sandbox_Game_Entities_MyEntityList\u003C\u003EMyEntityListInfoItem\u003C\u003EMass\u003C\u003EAccessor : IMemberAccessor<MyEntityList.MyEntityListInfoItem, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyEntityList.MyEntityListInfoItem owner, in float value) => owner.Mass = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyEntityList.MyEntityListInfoItem owner, out float value) => value = owner.Mass;
      }

      protected class Sandbox_Game_Entities_MyEntityList\u003C\u003EMyEntityListInfoItem\u003C\u003EPosition\u003C\u003EAccessor : IMemberAccessor<MyEntityList.MyEntityListInfoItem, Vector3D>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyEntityList.MyEntityListInfoItem owner, in Vector3D value) => owner.Position = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyEntityList.MyEntityListInfoItem owner, out Vector3D value) => value = owner.Position;
      }

      protected class Sandbox_Game_Entities_MyEntityList\u003C\u003EMyEntityListInfoItem\u003C\u003EOwnerName\u003C\u003EAccessor : IMemberAccessor<MyEntityList.MyEntityListInfoItem, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyEntityList.MyEntityListInfoItem owner, in string value) => owner.OwnerName = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyEntityList.MyEntityListInfoItem owner, out string value) => value = owner.OwnerName;
      }

      protected class Sandbox_Game_Entities_MyEntityList\u003C\u003EMyEntityListInfoItem\u003C\u003EOwner\u003C\u003EAccessor : IMemberAccessor<MyEntityList.MyEntityListInfoItem, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyEntityList.MyEntityListInfoItem owner, in long value) => owner.Owner = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyEntityList.MyEntityListInfoItem owner, out long value) => value = owner.Owner;
      }

      protected class Sandbox_Game_Entities_MyEntityList\u003C\u003EMyEntityListInfoItem\u003C\u003ESpeed\u003C\u003EAccessor : IMemberAccessor<MyEntityList.MyEntityListInfoItem, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyEntityList.MyEntityListInfoItem owner, in float value) => owner.Speed = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyEntityList.MyEntityListInfoItem owner, out float value) => value = owner.Speed;
      }

      protected class Sandbox_Game_Entities_MyEntityList\u003C\u003EMyEntityListInfoItem\u003C\u003EDistanceFromPlayers\u003C\u003EAccessor : IMemberAccessor<MyEntityList.MyEntityListInfoItem, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyEntityList.MyEntityListInfoItem owner, in float value) => owner.DistanceFromPlayers = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyEntityList.MyEntityListInfoItem owner, out float value) => value = owner.DistanceFromPlayers;
      }

      protected class Sandbox_Game_Entities_MyEntityList\u003C\u003EMyEntityListInfoItem\u003C\u003EOwnerLoginTime\u003C\u003EAccessor : IMemberAccessor<MyEntityList.MyEntityListInfoItem, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyEntityList.MyEntityListInfoItem owner, in float value) => owner.OwnerLoginTime = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyEntityList.MyEntityListInfoItem owner, out float value) => value = owner.OwnerLoginTime;
      }

      protected class Sandbox_Game_Entities_MyEntityList\u003C\u003EMyEntityListInfoItem\u003C\u003EOwnerLogoutTime\u003C\u003EAccessor : IMemberAccessor<MyEntityList.MyEntityListInfoItem, float?>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyEntityList.MyEntityListInfoItem owner, in float? value) => owner.OwnerLogoutTime = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyEntityList.MyEntityListInfoItem owner, out float? value) => value = owner.OwnerLogoutTime;
      }

      protected class Sandbox_Game_Entities_MyEntityList\u003C\u003EMyEntityListInfoItem\u003C\u003EPlayerPresenceTier\u003C\u003EAccessor : IMemberAccessor<MyEntityList.MyEntityListInfoItem, MyUpdateTiersPlayerPresence>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyEntityList.MyEntityListInfoItem owner,
          in MyUpdateTiersPlayerPresence value)
        {
          owner.PlayerPresenceTier = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyEntityList.MyEntityListInfoItem owner,
          out MyUpdateTiersPlayerPresence value)
        {
          value = owner.PlayerPresenceTier;
        }
      }

      protected class Sandbox_Game_Entities_MyEntityList\u003C\u003EMyEntityListInfoItem\u003C\u003EGridPresenceTier\u003C\u003EAccessor : IMemberAccessor<MyEntityList.MyEntityListInfoItem, MyUpdateTiersGridPresence>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyEntityList.MyEntityListInfoItem owner,
          in MyUpdateTiersGridPresence value)
        {
          owner.GridPresenceTier = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyEntityList.MyEntityListInfoItem owner,
          out MyUpdateTiersGridPresence value)
        {
          value = owner.GridPresenceTier;
        }
      }

      protected class Sandbox_Game_Entities_MyEntityList\u003C\u003EMyEntityListInfoItem\u003C\u003EIsReplicated\u003C\u003EAccessor : IMemberAccessor<MyEntityList.MyEntityListInfoItem, bool?>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyEntityList.MyEntityListInfoItem owner, in bool? value) => owner.IsReplicated = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyEntityList.MyEntityListInfoItem owner, out bool? value) => value = owner.IsReplicated;
      }

      protected class Sandbox_Game_Entities_MyEntityList\u003C\u003EMyEntityListInfoItem\u003C\u003EIsGrid\u003C\u003EAccessor : IMemberAccessor<MyEntityList.MyEntityListInfoItem, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyEntityList.MyEntityListInfoItem owner, in bool value) => owner.IsGrid = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyEntityList.MyEntityListInfoItem owner, out bool value) => value = owner.IsGrid;
      }
    }

    public enum MyEntityTypeEnum
    {
      Grids,
      SmallGrids,
      LargeGrids,
      Characters,
      FloatingObjects,
      Planets,
      Asteroids,
      Replicated,
      NotReplicated,
    }

    public enum EntityListAction
    {
      Remove,
      Stop,
      Depower,
      Power,
    }

    public enum MyEntitySortOrder
    {
      DisplayName,
      BlockCount,
      Mass,
      OwnerName,
      DistanceFromCenter,
      Speed,
      DistanceFromPlayers,
      OwnerLastLogout,
      PCU,
    }
  }
}
