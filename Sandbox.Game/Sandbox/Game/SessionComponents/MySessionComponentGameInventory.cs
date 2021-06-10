// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MySessionComponentGameInventory
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders.Components;
using VRage.GameServices;
using VRage.Library.Collections;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.SessionComponents
{
  [StaticEventOwner]
  [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation, 2019, typeof (MyObjectBuilder_SessionComponentGameInventory), null, false)]
  public class MySessionComponentGameInventory : MySessionComponentBase
  {
    public static bool DEBUG_REVOKE_ITEM_OWNERSHIP;
    private const int MAX_TRIES = 5;
    private HashSet<MyStringHash> m_availableArmors;
    private MyHashSetDictionary<ulong, MyStringHash> m_clientAvailableArmors;
    private CachingDictionary<ulong, byte[]> m_pendingUpdates;
    private Dictionary<ulong, int> m_triesLeft;

    public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
    {
      if (!MyGameService.Service.IsActive && !Sync.IsDedicated)
        return;
      base.Init(sessionComponent);
      this.m_availableArmors = new HashSet<MyStringHash>();
      if (!Sync.IsDedicated)
      {
        this.UpdateLocalPlayerGameInventory();
        MyGameService.InventoryRefreshed += new EventHandler(this.MyGameServiceOnInventoryRefreshed);
      }
      if (MyMultiplayer.Static == null || !Sync.IsServer)
        return;
      this.m_pendingUpdates = new CachingDictionary<ulong, byte[]>();
      this.m_triesLeft = new Dictionary<ulong, int>();
      this.m_clientAvailableArmors = new MyHashSetDictionary<ulong, MyStringHash>();
    }

    private void MyGameServiceOnInventoryRefreshed(object sender, EventArgs e) => this.UpdateLocalPlayerGameInventory();

    public override void UpdateBeforeSimulation()
    {
      base.UpdateBeforeSimulation();
      if (!Sync.IsServer || this.m_pendingUpdates == null)
        return;
      this.m_pendingUpdates.ApplyChanges();
      foreach (KeyValuePair<ulong, byte[]> pendingUpdate in this.m_pendingUpdates)
      {
        int num;
        if (this.m_triesLeft.TryGetValue(pendingUpdate.Key, out num) && num > 0)
        {
          this.UpdateClientGameInventory(pendingUpdate.Key, pendingUpdate.Value, num > 1);
          this.m_triesLeft[pendingUpdate.Key] = num - 1;
        }
        else
          this.m_triesLeft.Remove(pendingUpdate.Key);
      }
    }

    protected override void UnloadData()
    {
      if (!Sync.IsDedicated && MyGameService.IsActive)
        MyGameService.InventoryRefreshed -= new EventHandler(this.MyGameServiceOnInventoryRefreshed);
      base.UnloadData();
    }

    private void UpdateLocalPlayerGameInventory()
    {
      if (Sync.IsDedicated || MySession.Static == null)
        return;
      this.m_availableArmors.Clear();
      ICollection<MyGameInventoryItem> inventoryItems = MyGameService.InventoryItems;
      if (inventoryItems == null)
        return;
      List<MyGameInventoryItem> items = new List<MyGameInventoryItem>();
      foreach (MyGameInventoryItem gameInventoryItem in (IEnumerable<MyGameInventoryItem>) inventoryItems)
      {
        if (gameInventoryItem.ItemDefinition != null && gameInventoryItem.ItemDefinition.ItemSlot == MyGameInventoryItemSlot.Armor)
        {
          items.Add(gameInventoryItem);
          this.m_availableArmors.Add(MyStringHash.GetOrCompute(gameInventoryItem.ItemDefinition.AssetModifierId));
        }
      }
      if (items.Count <= 0 || Sync.IsServer)
        return;
      MyGameService.GetItemsCheckData(items, (Action<byte[]>) (checkData => MyMultiplayer.RaiseStaticEvent<byte[]>((Func<IMyEventOwner, Action<byte[]>>) (x => new Action<byte[]>(MySessionComponentGameInventory.RequestUpdateClientGameInventory)), checkData)));
    }

    [Event(null, 138)]
    [Reliable]
    [Server]
    public static void RequestUpdateClientGameInventory(byte[] checkData)
    {
      MySessionComponentGameInventory component = MySession.Static.GetComponent<MySessionComponentGameInventory>();
      component.m_triesLeft[MyEventContext.Current.Sender.Value] = 5;
      component.UpdateClientGameInventory(MyEventContext.Current.Sender.Value, checkData, true);
    }

    [Event(null, 149)]
    [Reliable]
    [Client]
    public static void UpdateClientGameInventoryResult(bool success)
    {
      if (success)
        return;
      MySession.Static.GetComponent<MySessionComponentGameInventory>().UpdateLocalPlayerGameInventory();
      MyLog.Default.Log(MyLogSeverity.Warning, "Server failed to update game inventory items.");
    }

    private void UpdateClientGameInventory(ulong steamId, byte[] checkData, bool retry)
    {
      bool checkResult;
      List<MyGameInventoryItem> gameInventoryItemList = MyGameService.CheckItemData(checkData, out checkResult);
      if (!checkResult)
      {
        if (retry)
        {
          this.m_pendingUpdates[steamId] = checkData;
        }
        else
        {
          this.m_pendingUpdates.Remove(steamId);
          MyMultiplayer.RaiseStaticEvent<bool>((Func<IMyEventOwner, Action<bool>>) (x => new Action<bool>(MySessionComponentGameInventory.UpdateClientGameInventoryResult)), false, new EndpointId(steamId));
        }
      }
      else
      {
        this.m_pendingUpdates.Remove(steamId);
        HashSet<MyStringHash> orAdd = this.m_clientAvailableArmors.GetOrAdd(steamId);
        orAdd.Clear();
        foreach (MyGameInventoryItem gameInventoryItem in gameInventoryItemList)
        {
          if (gameInventoryItem.ItemDefinition != null && gameInventoryItem.ItemDefinition.ItemSlot == MyGameInventoryItemSlot.Armor)
            orAdd.Add(MyStringHash.GetOrCompute(gameInventoryItem.ItemDefinition.AssetModifierId));
        }
        MyMultiplayer.RaiseStaticEvent<bool>((Func<IMyEventOwner, Action<bool>>) (x => new Action<bool>(MySessionComponentGameInventory.UpdateClientGameInventoryResult)), true, new EndpointId(steamId));
      }
    }

    public MyStringHash ValidateArmor(MyStringHash armorId, ulong steamId)
    {
      if (!this.HasArmor(armorId, steamId))
        return MyStringHash.NullOrEmpty;
      MyAssetModifierDefinition modifierDefinition = MyDefinitionManager.Static.GetAssetModifierDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_AssetModifierDefinition), armorId));
      return MySession.Static.GetComponent<MySessionComponentDLC>().HasDefinitionDLC((MyDefinitionBase) modifierDefinition, steamId) ? armorId : MyStringHash.NullOrEmpty;
    }

    public bool HasArmor(MyStringHash armorId, ulong steamId)
    {
      if (steamId == 0UL || MySessionComponentGameInventory.DEBUG_REVOKE_ITEM_OWNERSHIP || this.m_availableArmors == null)
        return false;
      return (long) steamId == (long) Sync.MyId ? !Sync.IsDedicated && this.m_availableArmors.Contains(armorId) : Sync.IsServer && this.HasClientArmor(armorId, steamId);
    }

    private bool HasClientArmor(MyStringHash armorId, ulong steamId)
    {
      HashSet<MyStringHash> list;
      return this.m_clientAvailableArmors.TryGet(steamId, out list) && list.Contains(armorId);
    }

    protected sealed class RequestUpdateClientGameInventory\u003C\u003ESystem_Byte\u003C\u0023\u003E : ICallSite<IMyEventOwner, byte[], DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in byte[] checkData,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentGameInventory.RequestUpdateClientGameInventory(checkData);
      }
    }

    protected sealed class UpdateClientGameInventoryResult\u003C\u003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in bool success,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentGameInventory.UpdateClientGameInventoryResult(success);
      }
    }
  }
}
