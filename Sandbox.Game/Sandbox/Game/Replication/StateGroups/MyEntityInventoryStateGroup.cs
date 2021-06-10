// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.StateGroups.MyEntityInventoryStateGroup
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game.Entity;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;

namespace Sandbox.Game.Replication.StateGroups
{
  internal class MyEntityInventoryStateGroup : IMyStateGroup, IMyNetObject, IMyEventOwner
  {
    private readonly int m_inventoryIndex;
    private Dictionary<Endpoint, MyEntityInventoryStateGroup.InventoryClientData> m_clientInventoryUpdate;
    private List<MyPhysicalInventoryItem> m_itemsToSend;
    private HashSet<uint> m_foundDeltaItems;
    private uint m_nextExpectedPacketId;
    private readonly SortedList<uint, MyEntityInventoryStateGroup.InventoryDeltaInformation> m_buffer;
    private Dictionary<int, MyPhysicalInventoryItem> m_tmpSwappingList;

    public bool IsHighPriority => false;

    private MyInventory Inventory { get; set; }

    public IMyReplicable Owner { get; private set; }

    public bool IsValid => this.Owner != null && this.Owner.IsValid;

    public bool IsStreaming => false;

    public bool NeedsUpdate => false;

    public MyEntityInventoryStateGroup(MyInventory entity, bool attach, IMyReplicable owner)
    {
      this.Inventory = entity;
      if (attach)
        this.Inventory.ContentsChanged += new Action<MyInventoryBase>(this.InventoryChanged);
      this.Owner = owner;
      if (Sync.IsServer)
        return;
      this.m_buffer = new SortedList<uint, MyEntityInventoryStateGroup.InventoryDeltaInformation>();
    }

    private void InventoryChanged(MyInventoryBase obj)
    {
      if (this.m_clientInventoryUpdate == null)
        return;
      foreach (KeyValuePair<Endpoint, MyEntityInventoryStateGroup.InventoryClientData> keyValuePair in this.m_clientInventoryUpdate)
        this.m_clientInventoryUpdate[keyValuePair.Key].Dirty = true;
      MyMultiplayer.GetReplicationServer().AddToDirtyGroups((IMyStateGroup) this);
    }

    public void CreateClientData(MyClientStateBase forClient) => this.CreateClientData(forClient.EndpointId);

    public void RefreshClientData(Endpoint clientEndpoint)
    {
      if (this.m_clientInventoryUpdate == null)
        return;
      this.m_clientInventoryUpdate.Remove(clientEndpoint);
      this.CreateClientData(clientEndpoint);
    }

    private void CreateClientData(Endpoint clientEndpoint)
    {
      if (this.m_clientInventoryUpdate == null)
        this.m_clientInventoryUpdate = new Dictionary<Endpoint, MyEntityInventoryStateGroup.InventoryClientData>();
      MyEntityInventoryStateGroup.InventoryClientData inventoryClientData;
      if (!this.m_clientInventoryUpdate.TryGetValue(clientEndpoint, out inventoryClientData))
      {
        this.m_clientInventoryUpdate[clientEndpoint] = new MyEntityInventoryStateGroup.InventoryClientData();
        inventoryClientData = this.m_clientInventoryUpdate[clientEndpoint];
      }
      inventoryClientData.Dirty = false;
      foreach (MyPhysicalInventoryItem physicalInventoryItem in this.Inventory.GetItems())
      {
        MyFixedPoint myFixedPoint = physicalInventoryItem.Amount;
        if (physicalInventoryItem.Content is MyObjectBuilder_GasContainerObject content)
          myFixedPoint = (MyFixedPoint) content.GasLevel;
        MyEntityInventoryStateGroup.ClientInvetoryData clientInvetoryData = new MyEntityInventoryStateGroup.ClientInvetoryData()
        {
          Item = physicalInventoryItem,
          Amount = myFixedPoint
        };
        inventoryClientData.ClientItemsSorted[physicalInventoryItem.ItemId] = clientInvetoryData;
        inventoryClientData.ClientItems.Add(clientInvetoryData);
      }
    }

    public void DestroyClientData(MyClientStateBase forClient)
    {
      if (this.m_clientInventoryUpdate == null)
        return;
      this.m_clientInventoryUpdate.Remove(forClient.EndpointId);
    }

    public void ClientUpdate(MyTimeSpan clientTimestamp)
    {
    }

    public void Destroy()
    {
    }

    public float GetGroupPriority(int frameCountWithoutSync, MyClientInfo client)
    {
      MyEntityInventoryStateGroup.InventoryClientData inventoryClientData;
      if (this.m_clientInventoryUpdate == null || !this.m_clientInventoryUpdate.TryGetValue(client.EndpointId, out inventoryClientData) || !inventoryClientData.Dirty && inventoryClientData.FailedIncompletePackets.Count == 0)
        return -1f;
      if (inventoryClientData.FailedIncompletePackets.Count > 0)
        return 1f * (float) frameCountWithoutSync;
      MyClientState state = (MyClientState) client.State;
      if (this.Inventory.Owner is MyCharacter)
      {
        MyCharacter owner = this.Inventory.Owner as MyCharacter;
        MyPlayer myPlayer = MyPlayer.GetPlayerFromCharacter(owner);
        if (myPlayer == null && owner.IsUsing != null && (owner.IsUsing is MyShipController isUsing && isUsing.ControllerInfo.Controller != null))
          myPlayer = isUsing.ControllerInfo.Controller.Player;
        if (myPlayer != null && (long) myPlayer.Id.SteamId == (long) client.EndpointId.Id.Value)
          return 1f * (float) frameCountWithoutSync;
      }
      if (state.ContextEntity is MyCharacter && state.ContextEntity == this.Inventory.Owner)
        return 1f * (float) frameCountWithoutSync;
      return state.Context == MyClientState.MyContextKind.Inventory || state.Context == MyClientState.MyContextKind.Building || state.Context == MyClientState.MyContextKind.Production && this.Inventory.Owner is MyAssembler ? this.GetPriorityStateGroup(client) * (float) frameCountWithoutSync : 0.0f;
    }

    private float GetPriorityStateGroup(MyClientInfo client)
    {
      MyClientState state = (MyClientState) client.State;
      if (this.Inventory.ForcedPriority.HasValue)
        return this.Inventory.ForcedPriority.Value;
      if (state.ContextEntity != null)
      {
        if (state.ContextEntity == this.Inventory.Owner)
          return 1f;
        if (state.ContextEntity.GetTopMostParent((Type) null) is MyCubeGrid topMostParent)
        {
          foreach (MyTerminalBlock block in topMostParent.GridSystems.TerminalSystem.Blocks)
          {
            if (block == this.Inventory.Container.Entity && (state.Context != MyClientState.MyContextKind.Production || block is MyAssembler))
              return 1f;
          }
        }
      }
      return 0.0f;
    }

    public void Serialize(
      BitStream stream,
      MyClientInfo forClient,
      MyTimeSpan serverTimestamp,
      MyTimeSpan lastClientTimestamp,
      byte packetId,
      int maxBitPosition,
      HashSet<string> cachedData)
    {
      if (stream.Writing)
      {
        MyEntityInventoryStateGroup.InventoryClientData clientData;
        if (this.m_clientInventoryUpdate == null || !this.m_clientInventoryUpdate.TryGetValue(forClient.EndpointId, out clientData))
        {
          stream.WriteBool(false);
          stream.WriteUInt32(0U);
        }
        else
        {
          bool needsSplit = false;
          if (clientData.FailedIncompletePackets.Count > 0)
          {
            MyEntityInventoryStateGroup.InventoryDeltaInformation incompletePacket = clientData.FailedIncompletePackets[0];
            clientData.FailedIncompletePackets.RemoveAtFast<MyEntityInventoryStateGroup.InventoryDeltaInformation>(0);
            MyEntityInventoryStateGroup.InventoryDeltaInformation sentData = this.WriteInventory(ref incompletePacket, stream, packetId, maxBitPosition, out needsSplit);
            sentData.MessageId = incompletePacket.MessageId;
            if (needsSplit)
              clientData.FailedIncompletePackets.Add(this.CreateSplit(ref incompletePacket, ref sentData));
            clientData.SendPackets[packetId] = sentData;
          }
          else
          {
            MyEntityInventoryStateGroup.InventoryDeltaInformation inventoryDiff = this.CalculateInventoryDiff(ref clientData);
            inventoryDiff.MessageId = clientData.CurrentMessageId;
            clientData.MainSendingInfo = this.WriteInventory(ref inventoryDiff, stream, packetId, maxBitPosition, out needsSplit);
            clientData.SendPackets[packetId] = clientData.MainSendingInfo;
            ++clientData.CurrentMessageId;
            if (needsSplit)
            {
              MyEntityInventoryStateGroup.InventoryDeltaInformation split = this.CreateSplit(ref inventoryDiff, ref clientData.MainSendingInfo);
              split.MessageId = clientData.CurrentMessageId;
              clientData.FailedIncompletePackets.Add(split);
              ++clientData.CurrentMessageId;
            }
            clientData.Dirty = false;
          }
        }
      }
      else
        this.ReadInventory(stream);
    }

    private void ReadInventory(BitStream stream)
    {
      bool flag1 = stream.ReadBool();
      uint key1 = stream.ReadUInt32();
      bool flag2 = true;
      bool flag3 = false;
      MyEntityInventoryStateGroup.InventoryDeltaInformation deltaInformation = new MyEntityInventoryStateGroup.InventoryDeltaInformation();
      if ((int) key1 == (int) this.m_nextExpectedPacketId)
      {
        ++this.m_nextExpectedPacketId;
        if (!flag1)
        {
          this.FlushBuffer();
          return;
        }
      }
      else if (key1 > this.m_nextExpectedPacketId && !this.m_buffer.ContainsKey(key1))
      {
        flag3 = true;
        deltaInformation.MessageId = key1;
      }
      else
        flag2 = false;
      if (!flag1)
      {
        if (!flag3)
          return;
        this.m_buffer.Add(key1, deltaInformation);
      }
      else
      {
        if (stream.ReadBool())
        {
          int num1 = stream.ReadInt32();
          for (int index = 0; index < num1; ++index)
          {
            uint num2 = stream.ReadUInt32();
            MyFixedPoint amount = new MyFixedPoint();
            amount.RawValue = stream.ReadInt64();
            if (flag2)
            {
              if (flag3)
              {
                if (deltaInformation.ChangedItems == null)
                  deltaInformation.ChangedItems = new Dictionary<uint, MyFixedPoint>();
                deltaInformation.ChangedItems.Add(num2, amount);
              }
              else
                this.Inventory.UpdateItemAmoutClient(num2, amount);
            }
          }
        }
        if (stream.ReadBool())
        {
          int num = stream.ReadInt32();
          for (int index = 0; index < num; ++index)
          {
            uint itemId = stream.ReadUInt32();
            if (flag2)
            {
              if (flag3)
              {
                if (deltaInformation.RemovedItems == null)
                  deltaInformation.RemovedItems = new List<uint>();
                deltaInformation.RemovedItems.Add(itemId);
              }
              else
                this.Inventory.RemoveItemClient(itemId);
            }
          }
        }
        if (stream.ReadBool())
        {
          int num1 = stream.ReadInt32();
          for (int index = 0; index < num1; ++index)
          {
            int num2 = stream.ReadInt32();
            MyPhysicalInventoryItem physicalInventoryItem;
            MySerializer.CreateAndRead<MyPhysicalInventoryItem>(stream, out physicalInventoryItem, MyObjectBuilderSerializer.Dynamic);
            if (flag2)
            {
              if (flag3)
              {
                if (deltaInformation.NewItems == null)
                  deltaInformation.NewItems = new SortedDictionary<int, MyPhysicalInventoryItem>();
                deltaInformation.NewItems.Add(num2, physicalInventoryItem);
              }
              else
                this.Inventory.AddItemClient(num2, physicalInventoryItem);
            }
          }
        }
        if (stream.ReadBool())
        {
          if (this.m_tmpSwappingList == null)
            this.m_tmpSwappingList = new Dictionary<int, MyPhysicalInventoryItem>();
          int num1 = stream.ReadInt32();
          for (int index = 0; index < num1; ++index)
          {
            uint num2 = stream.ReadUInt32();
            int key2 = stream.ReadInt32();
            if (flag2)
            {
              if (flag3)
              {
                if (deltaInformation.SwappedItems == null)
                  deltaInformation.SwappedItems = new Dictionary<uint, int>();
                deltaInformation.SwappedItems.Add(num2, key2);
              }
              else
              {
                MyPhysicalInventoryItem? itemById = this.Inventory.GetItemByID(num2);
                if (itemById.HasValue)
                  this.m_tmpSwappingList.Add(key2, itemById.Value);
              }
            }
          }
          foreach (KeyValuePair<int, MyPhysicalInventoryItem> tmpSwapping in this.m_tmpSwappingList)
            this.Inventory.ChangeItemClient(tmpSwapping.Value, tmpSwapping.Key);
          this.m_tmpSwappingList.Clear();
        }
        if (flag3)
          this.m_buffer.Add(key1, deltaInformation);
        else if (flag2)
          this.FlushBuffer();
        this.Inventory.Refresh();
      }
    }

    private void FlushBuffer()
    {
      while (this.m_buffer.Count > 0)
      {
        MyEntityInventoryStateGroup.InventoryDeltaInformation changes = this.m_buffer.Values[0];
        if ((int) changes.MessageId != (int) this.m_nextExpectedPacketId)
          break;
        ++this.m_nextExpectedPacketId;
        this.ApplyChangesOnClient(changes);
        this.m_buffer.RemoveAt(0);
      }
    }

    private void ApplyChangesOnClient(
      MyEntityInventoryStateGroup.InventoryDeltaInformation changes)
    {
      if (changes.ChangedItems != null)
      {
        foreach (KeyValuePair<uint, MyFixedPoint> changedItem in changes.ChangedItems)
          this.Inventory.UpdateItemAmoutClient(changedItem.Key, changedItem.Value);
      }
      if (changes.RemovedItems != null)
      {
        foreach (uint removedItem in changes.RemovedItems)
          this.Inventory.RemoveItemClient(removedItem);
      }
      if (changes.NewItems != null)
      {
        foreach (KeyValuePair<int, MyPhysicalInventoryItem> newItem in changes.NewItems)
          this.Inventory.AddItemClient(newItem.Key, newItem.Value);
      }
      if (changes.SwappedItems == null)
        return;
      if (this.m_tmpSwappingList == null)
        this.m_tmpSwappingList = new Dictionary<int, MyPhysicalInventoryItem>();
      foreach (KeyValuePair<uint, int> swappedItem in changes.SwappedItems)
      {
        MyPhysicalInventoryItem? itemById = this.Inventory.GetItemByID(swappedItem.Key);
        if (itemById.HasValue)
          this.m_tmpSwappingList.Add(swappedItem.Value, itemById.Value);
      }
      foreach (KeyValuePair<int, MyPhysicalInventoryItem> tmpSwapping in this.m_tmpSwappingList)
        this.Inventory.ChangeItemClient(tmpSwapping.Value, tmpSwapping.Key);
      this.m_tmpSwappingList.Clear();
    }

    private MyEntityInventoryStateGroup.InventoryDeltaInformation CalculateInventoryDiff(
      ref MyEntityInventoryStateGroup.InventoryClientData clientData)
    {
      if (this.m_itemsToSend == null)
        this.m_itemsToSend = new List<MyPhysicalInventoryItem>();
      if (this.m_foundDeltaItems == null)
        this.m_foundDeltaItems = new HashSet<uint>();
      this.m_foundDeltaItems.Clear();
      List<MyPhysicalInventoryItem> items = this.Inventory.GetItems();
      MyEntityInventoryStateGroup.InventoryDeltaInformation delta;
      this.CalculateAddsAndRemovals(clientData, out delta, items);
      if (delta.HasChanges)
        MyEntityInventoryStateGroup.ApplyChangesToClientItems(clientData, ref delta);
      for (int index1 = 0; index1 < items.Count; ++index1)
      {
        if (index1 < clientData.ClientItems.Count)
        {
          uint itemId = clientData.ClientItems[index1].Item.ItemId;
          if ((int) itemId != (int) items[index1].ItemId)
          {
            if (delta.SwappedItems == null)
              delta.SwappedItems = new Dictionary<uint, int>();
            for (int index2 = 0; index2 < items.Count; ++index2)
            {
              if ((int) itemId == (int) items[index2].ItemId)
                delta.SwappedItems[itemId] = index2;
            }
          }
        }
      }
      clientData.ClientItemsSorted.Clear();
      clientData.ClientItems.Clear();
      foreach (MyPhysicalInventoryItem physicalInventoryItem in items)
      {
        MyFixedPoint myFixedPoint = physicalInventoryItem.Amount;
        if (physicalInventoryItem.Content is MyObjectBuilder_GasContainerObject content)
          myFixedPoint = (MyFixedPoint) content.GasLevel;
        MyEntityInventoryStateGroup.ClientInvetoryData clientInvetoryData = new MyEntityInventoryStateGroup.ClientInvetoryData()
        {
          Item = physicalInventoryItem,
          Amount = myFixedPoint
        };
        clientData.ClientItemsSorted[physicalInventoryItem.ItemId] = clientInvetoryData;
        clientData.ClientItems.Add(clientInvetoryData);
      }
      return delta;
    }

    private static void ApplyChangesToClientItems(
      MyEntityInventoryStateGroup.InventoryClientData clientData,
      ref MyEntityInventoryStateGroup.InventoryDeltaInformation delta)
    {
      if (delta.RemovedItems != null)
      {
        foreach (uint removedItem in delta.RemovedItems)
        {
          int index1 = -1;
          for (int index2 = 0; index2 < clientData.ClientItems.Count; ++index2)
          {
            if ((int) clientData.ClientItems[index2].Item.ItemId == (int) removedItem)
            {
              index1 = index2;
              break;
            }
          }
          if (index1 != -1)
            clientData.ClientItems.RemoveAt(index1);
        }
      }
      if (delta.NewItems == null)
        return;
      foreach (KeyValuePair<int, MyPhysicalInventoryItem> newItem in delta.NewItems)
      {
        MyEntityInventoryStateGroup.ClientInvetoryData clientInvetoryData = new MyEntityInventoryStateGroup.ClientInvetoryData()
        {
          Item = newItem.Value,
          Amount = newItem.Value.Amount
        };
        if (newItem.Key >= clientData.ClientItems.Count)
          clientData.ClientItems.Add(clientInvetoryData);
        else
          clientData.ClientItems.Insert(newItem.Key, clientInvetoryData);
      }
    }

    private void CalculateAddsAndRemovals(
      MyEntityInventoryStateGroup.InventoryClientData clientData,
      out MyEntityInventoryStateGroup.InventoryDeltaInformation delta,
      List<MyPhysicalInventoryItem> items)
    {
      delta = new MyEntityInventoryStateGroup.InventoryDeltaInformation()
      {
        HasChanges = false
      };
      int key = 0;
      foreach (MyPhysicalInventoryItem physicalInventoryItem in items)
      {
        MyEntityInventoryStateGroup.ClientInvetoryData clientInvetoryData;
        if (clientData.ClientItemsSorted.TryGetValue(physicalInventoryItem.ItemId, out clientInvetoryData))
        {
          if (clientInvetoryData.Item.Content.TypeId == physicalInventoryItem.Content.TypeId && clientInvetoryData.Item.Content.SubtypeId == physicalInventoryItem.Content.SubtypeId)
          {
            this.m_foundDeltaItems.Add(physicalInventoryItem.ItemId);
            MyFixedPoint myFixedPoint1 = physicalInventoryItem.Amount;
            if (physicalInventoryItem.Content is MyObjectBuilder_GasContainerObject content)
              myFixedPoint1 = (MyFixedPoint) content.GasLevel;
            if (clientInvetoryData.Amount != myFixedPoint1)
            {
              MyFixedPoint myFixedPoint2 = myFixedPoint1 - clientInvetoryData.Amount;
              if (delta.ChangedItems == null)
                delta.ChangedItems = new Dictionary<uint, MyFixedPoint>();
              delta.ChangedItems[physicalInventoryItem.ItemId] = myFixedPoint2;
              delta.HasChanges = true;
            }
          }
        }
        else
        {
          if (delta.NewItems == null)
            delta.NewItems = new SortedDictionary<int, MyPhysicalInventoryItem>();
          delta.NewItems[key] = physicalInventoryItem;
          delta.HasChanges = true;
        }
        ++key;
      }
      foreach (KeyValuePair<uint, MyEntityInventoryStateGroup.ClientInvetoryData> keyValuePair in clientData.ClientItemsSorted)
      {
        if (delta.RemovedItems == null)
          delta.RemovedItems = new List<uint>();
        if (!this.m_foundDeltaItems.Contains(keyValuePair.Key))
        {
          delta.RemovedItems.Add(keyValuePair.Key);
          delta.HasChanges = true;
        }
      }
    }

    private MyEntityInventoryStateGroup.InventoryDeltaInformation WriteInventory(
      ref MyEntityInventoryStateGroup.InventoryDeltaInformation packetInfo,
      BitStream stream,
      byte packetId,
      int maxBitPosition,
      out bool needsSplit)
    {
      MyEntityInventoryStateGroup.InventoryDeltaInformation deltaInformation = this.PrepareSendData(ref packetInfo, stream, maxBitPosition, out needsSplit);
      deltaInformation.MessageId = packetInfo.MessageId;
      stream.WriteBool(deltaInformation.HasChanges);
      stream.WriteUInt32(deltaInformation.MessageId);
      if (!deltaInformation.HasChanges)
        return deltaInformation;
      stream.WriteBool(deltaInformation.ChangedItems != null);
      if (deltaInformation.ChangedItems != null)
      {
        stream.WriteInt32(deltaInformation.ChangedItems.Count);
        foreach (KeyValuePair<uint, MyFixedPoint> changedItem in deltaInformation.ChangedItems)
        {
          stream.WriteUInt32(changedItem.Key);
          stream.WriteInt64(changedItem.Value.RawValue);
        }
      }
      stream.WriteBool(deltaInformation.RemovedItems != null);
      if (deltaInformation.RemovedItems != null)
      {
        stream.WriteInt32(deltaInformation.RemovedItems.Count);
        foreach (uint removedItem in deltaInformation.RemovedItems)
          stream.WriteUInt32(removedItem);
      }
      stream.WriteBool(deltaInformation.NewItems != null);
      if (deltaInformation.NewItems != null)
      {
        stream.WriteInt32(deltaInformation.NewItems.Count);
        foreach (KeyValuePair<int, MyPhysicalInventoryItem> newItem in deltaInformation.NewItems)
        {
          stream.WriteInt32(newItem.Key);
          MyPhysicalInventoryItem physicalInventoryItem = newItem.Value;
          MySerializer.Write<MyPhysicalInventoryItem>(stream, ref physicalInventoryItem, MyObjectBuilderSerializer.Dynamic);
        }
      }
      stream.WriteBool(deltaInformation.SwappedItems != null);
      if (deltaInformation.SwappedItems != null)
      {
        stream.WriteInt32(deltaInformation.SwappedItems.Count);
        foreach (KeyValuePair<uint, int> swappedItem in deltaInformation.SwappedItems)
        {
          stream.WriteUInt32(swappedItem.Key);
          stream.WriteInt32(swappedItem.Value);
        }
      }
      return deltaInformation;
    }

    private MyEntityInventoryStateGroup.InventoryDeltaInformation PrepareSendData(
      ref MyEntityInventoryStateGroup.InventoryDeltaInformation packetInfo,
      BitStream stream,
      int maxBitPosition,
      out bool needsSplit)
    {
      needsSplit = false;
      long bitPosition1 = stream.BitPosition;
      MyEntityInventoryStateGroup.InventoryDeltaInformation deltaInformation = new MyEntityInventoryStateGroup.InventoryDeltaInformation()
      {
        HasChanges = false
      };
      stream.WriteBool(false);
      stream.WriteUInt32(packetInfo.MessageId);
      stream.WriteBool(packetInfo.ChangedItems != null);
      if (packetInfo.ChangedItems != null)
      {
        stream.WriteInt32(packetInfo.ChangedItems.Count);
        if (stream.BitPosition > (long) maxBitPosition)
        {
          needsSplit = true;
        }
        else
        {
          deltaInformation.ChangedItems = new Dictionary<uint, MyFixedPoint>();
          foreach (KeyValuePair<uint, MyFixedPoint> changedItem in packetInfo.ChangedItems)
          {
            stream.WriteUInt32(changedItem.Key);
            stream.WriteInt64(changedItem.Value.RawValue);
            if (stream.BitPosition <= (long) maxBitPosition)
            {
              deltaInformation.ChangedItems[changedItem.Key] = changedItem.Value;
              deltaInformation.HasChanges = true;
            }
            else
              needsSplit = true;
          }
        }
      }
      stream.WriteBool(packetInfo.RemovedItems != null);
      if (packetInfo.RemovedItems != null)
      {
        stream.WriteInt32(packetInfo.RemovedItems.Count);
        if (stream.BitPosition > (long) maxBitPosition)
        {
          needsSplit = true;
        }
        else
        {
          deltaInformation.RemovedItems = new List<uint>();
          foreach (uint removedItem in packetInfo.RemovedItems)
          {
            stream.WriteUInt32(removedItem);
            if (stream.BitPosition <= (long) maxBitPosition)
            {
              deltaInformation.RemovedItems.Add(removedItem);
              deltaInformation.HasChanges = true;
            }
            else
              needsSplit = true;
          }
        }
      }
      stream.WriteBool(packetInfo.NewItems != null);
      if (packetInfo.NewItems != null)
      {
        stream.WriteInt32(packetInfo.NewItems.Count);
        if (stream.BitPosition > (long) maxBitPosition)
        {
          needsSplit = true;
        }
        else
        {
          deltaInformation.NewItems = new SortedDictionary<int, MyPhysicalInventoryItem>();
          foreach (KeyValuePair<int, MyPhysicalInventoryItem> newItem in packetInfo.NewItems)
          {
            MyPhysicalInventoryItem physicalInventoryItem = newItem.Value;
            stream.WriteInt32(newItem.Key);
            long bitPosition2 = stream.BitPosition;
            MySerializer.Write<MyPhysicalInventoryItem>(stream, ref physicalInventoryItem, MyObjectBuilderSerializer.Dynamic);
            long bitPosition3 = stream.BitPosition;
            if (stream.BitPosition <= (long) maxBitPosition)
            {
              deltaInformation.NewItems[newItem.Key] = physicalInventoryItem;
              deltaInformation.HasChanges = true;
            }
            else
              needsSplit = true;
          }
        }
      }
      stream.WriteBool(packetInfo.SwappedItems != null);
      if (packetInfo.SwappedItems != null)
      {
        stream.WriteInt32(packetInfo.SwappedItems.Count);
        if (stream.BitPosition > (long) maxBitPosition)
        {
          needsSplit = true;
        }
        else
        {
          deltaInformation.SwappedItems = new Dictionary<uint, int>();
          foreach (KeyValuePair<uint, int> swappedItem in packetInfo.SwappedItems)
          {
            stream.WriteUInt32(swappedItem.Key);
            stream.WriteInt32(swappedItem.Value);
            if (stream.BitPosition <= (long) maxBitPosition)
            {
              deltaInformation.SwappedItems[swappedItem.Key] = swappedItem.Value;
              deltaInformation.HasChanges = true;
            }
            else
              needsSplit = true;
          }
        }
      }
      stream.SetBitPositionWrite(bitPosition1);
      return deltaInformation;
    }

    private MyEntityInventoryStateGroup.InventoryDeltaInformation CreateSplit(
      ref MyEntityInventoryStateGroup.InventoryDeltaInformation originalData,
      ref MyEntityInventoryStateGroup.InventoryDeltaInformation sentData)
    {
      MyEntityInventoryStateGroup.InventoryDeltaInformation deltaInformation = new MyEntityInventoryStateGroup.InventoryDeltaInformation()
      {
        MessageId = sentData.MessageId
      };
      if (originalData.ChangedItems != null)
      {
        if (sentData.ChangedItems == null)
        {
          deltaInformation.ChangedItems = new Dictionary<uint, MyFixedPoint>();
          foreach (KeyValuePair<uint, MyFixedPoint> changedItem in originalData.ChangedItems)
            deltaInformation.ChangedItems[changedItem.Key] = changedItem.Value;
        }
        else if (originalData.ChangedItems.Count != sentData.ChangedItems.Count)
        {
          deltaInformation.ChangedItems = new Dictionary<uint, MyFixedPoint>();
          foreach (KeyValuePair<uint, MyFixedPoint> changedItem in originalData.ChangedItems)
          {
            if (!sentData.ChangedItems.ContainsKey(changedItem.Key))
              deltaInformation.ChangedItems[changedItem.Key] = changedItem.Value;
          }
        }
      }
      if (originalData.RemovedItems != null)
      {
        if (sentData.RemovedItems == null)
        {
          deltaInformation.RemovedItems = new List<uint>();
          foreach (uint removedItem in originalData.RemovedItems)
            deltaInformation.RemovedItems.Add(removedItem);
        }
        else if (originalData.RemovedItems.Count != sentData.RemovedItems.Count)
        {
          deltaInformation.RemovedItems = new List<uint>();
          foreach (uint removedItem in originalData.RemovedItems)
          {
            if (!sentData.RemovedItems.Contains(removedItem))
              deltaInformation.RemovedItems.Add(removedItem);
          }
        }
      }
      if (originalData.NewItems != null)
      {
        if (sentData.NewItems == null)
        {
          deltaInformation.NewItems = new SortedDictionary<int, MyPhysicalInventoryItem>();
          foreach (KeyValuePair<int, MyPhysicalInventoryItem> newItem in originalData.NewItems)
            deltaInformation.NewItems[newItem.Key] = newItem.Value;
        }
        else if (originalData.NewItems.Count != sentData.NewItems.Count)
        {
          deltaInformation.NewItems = new SortedDictionary<int, MyPhysicalInventoryItem>();
          foreach (KeyValuePair<int, MyPhysicalInventoryItem> newItem in originalData.NewItems)
          {
            if (!sentData.NewItems.ContainsKey(newItem.Key))
              deltaInformation.NewItems[newItem.Key] = newItem.Value;
          }
        }
      }
      if (originalData.SwappedItems != null)
      {
        if (sentData.SwappedItems == null)
        {
          deltaInformation.SwappedItems = new Dictionary<uint, int>();
          foreach (KeyValuePair<uint, int> swappedItem in originalData.SwappedItems)
            deltaInformation.SwappedItems[swappedItem.Key] = swappedItem.Value;
        }
        else if (originalData.SwappedItems.Count != sentData.SwappedItems.Count)
        {
          deltaInformation.SwappedItems = new Dictionary<uint, int>();
          foreach (KeyValuePair<uint, int> swappedItem in originalData.SwappedItems)
          {
            if (!sentData.SwappedItems.ContainsKey(swappedItem.Key))
              deltaInformation.SwappedItems[swappedItem.Key] = swappedItem.Value;
          }
        }
      }
      return deltaInformation;
    }

    public void OnAck(MyClientStateBase forClient, byte packetId, bool delivered)
    {
      MyEntityInventoryStateGroup.InventoryClientData inventoryClientData;
      MyEntityInventoryStateGroup.InventoryDeltaInformation deltaInformation;
      if (this.m_clientInventoryUpdate == null || !this.m_clientInventoryUpdate.TryGetValue(forClient.EndpointId, out inventoryClientData) || !inventoryClientData.SendPackets.TryGetValue(packetId, out deltaInformation))
        return;
      if (!delivered)
      {
        inventoryClientData.FailedIncompletePackets.Add(deltaInformation);
        MyMultiplayer.GetReplicationServer().AddToDirtyGroups((IMyStateGroup) this);
      }
      inventoryClientData.SendPackets.Remove(packetId);
    }

    public void ForceSend(MyClientStateBase clientData)
    {
    }

    public void Reset(bool reinit, MyTimeSpan clientTimestamp)
    {
    }

    public bool IsStillDirty(Endpoint forClient)
    {
      MyEntityInventoryStateGroup.InventoryClientData inventoryClientData;
      return this.m_clientInventoryUpdate == null || !this.m_clientInventoryUpdate.TryGetValue(forClient, out inventoryClientData) || inventoryClientData.Dirty || (uint) inventoryClientData.FailedIncompletePackets.Count > 0U;
    }

    public MyStreamProcessingState IsProcessingForClient(Endpoint forClient) => MyStreamProcessingState.None;

    private struct InventoryDeltaInformation
    {
      public bool HasChanges;
      public uint MessageId;
      public List<uint> RemovedItems;
      public Dictionary<uint, MyFixedPoint> ChangedItems;
      public SortedDictionary<int, MyPhysicalInventoryItem> NewItems;
      public Dictionary<uint, int> SwappedItems;
    }

    private struct ClientInvetoryData
    {
      public MyPhysicalInventoryItem Item;
      public MyFixedPoint Amount;
    }

    private class InventoryClientData
    {
      public uint CurrentMessageId;
      public MyEntityInventoryStateGroup.InventoryDeltaInformation MainSendingInfo;
      public bool Dirty;
      public readonly Dictionary<byte, MyEntityInventoryStateGroup.InventoryDeltaInformation> SendPackets = new Dictionary<byte, MyEntityInventoryStateGroup.InventoryDeltaInformation>();
      public readonly List<MyEntityInventoryStateGroup.InventoryDeltaInformation> FailedIncompletePackets = new List<MyEntityInventoryStateGroup.InventoryDeltaInformation>();
      public readonly SortedDictionary<uint, MyEntityInventoryStateGroup.ClientInvetoryData> ClientItemsSorted = new SortedDictionary<uint, MyEntityInventoryStateGroup.ClientInvetoryData>();
      public readonly List<MyEntityInventoryStateGroup.ClientInvetoryData> ClientItems = new List<MyEntityInventoryStateGroup.ClientInvetoryData>();
    }
  }
}
