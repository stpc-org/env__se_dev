// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MyBlockLimits
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Network;
using VRage.Serialization;

namespace Sandbox.Game.World
{
  [StaticEventOwner]
  public class MyBlockLimits
  {
    public static readonly MyBlockLimits Empty = new MyBlockLimits(0, 0);
    private int m_blocksBuilt;
    private int m_PCUBuilt;
    private int m_PCU;
    private int m_PCUDirty;
    private int m_transferedDelta;

    public int BlockLimitModifier { get; set; }

    public ConcurrentDictionary<string, MyBlockLimits.MyTypeLimitData> BlockTypeBuilt { get; private set; }

    public ConcurrentDictionary<long, MyBlockLimits.MyGridLimitData> BlocksBuiltByGrid { get; private set; }

    public ConcurrentDictionary<long, MyBlockLimits.MyGridLimitData> GridsRemoved { get; private set; }

    public event Action BlockLimitsChanged;

    public int BlocksBuilt => this.m_blocksBuilt;

    public int PCU => this.m_PCU;

    public int PCUBuilt => this.m_PCUBuilt;

    public int TransferedDelta => this.m_transferedDelta;

    public int MaxBlocks => MySession.Static.MaxBlocksPerPlayer + this.BlockLimitModifier;

    public bool HasRemainingPCU => this.m_PCU > 0;

    public bool IsOverLimits
    {
      get
      {
        if (this.m_PCU < 0)
          return true;
        foreach (KeyValuePair<string, short> blockTypeLimit in MySession.Static.BlockTypeLimits)
        {
          string k;
          short v;
          blockTypeLimit.Deconstruct<string, short>(out k, out v);
          string key = k;
          short num = v;
          MyBlockLimits.MyTypeLimitData myTypeLimitData;
          if (this.BlockTypeBuilt.TryGetValue(key, out myTypeLimitData) && myTypeLimitData.BlocksBuilt > (int) num)
            return true;
        }
        return false;
      }
    }

    public static int GetInitialPCU(long identityId = -1)
    {
      switch (MySession.Static.BlockLimitsEnabled)
      {
        case MyBlockLimitsEnabledEnum.NONE:
          return int.MaxValue;
        case MyBlockLimitsEnabledEnum.PER_FACTION:
          if (MySession.Static.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.PER_FACTION && identityId != -1L && MySession.Static.Factions.GetPlayerFaction(identityId) == null)
            return 0;
          return MySession.Static.MaxFactionsCount == 0 ? MySession.Static.TotalPCU : MySession.Static.TotalPCU / MySession.Static.MaxFactionsCount;
        case MyBlockLimitsEnabledEnum.PER_PLAYER:
          return MySession.Static.TotalPCU / (int) MySession.Static.MaxPlayers;
        default:
          return MySession.Static.TotalPCU;
      }
    }

    public static int GetMaxPCU(MyIdentity identity) => MyBlockLimits.GetInitialPCU(identity.IdentityId) + identity.BlockLimits.m_transferedDelta;

    public MyBlockLimits(int initialPCU, int blockLimitModifier, int transferedDelta = 0)
    {
      this.BlockLimitModifier = blockLimitModifier;
      this.BlockTypeBuilt = new ConcurrentDictionary<string, MyBlockLimits.MyTypeLimitData>();
      foreach (string key in MySession.Static.BlockTypeLimits.Keys)
        this.BlockTypeBuilt.TryAdd(key, new MyBlockLimits.MyTypeLimitData()
        {
          BlockPairName = key
        });
      this.BlocksBuiltByGrid = new ConcurrentDictionary<long, MyBlockLimits.MyGridLimitData>();
      this.GridsRemoved = new ConcurrentDictionary<long, MyBlockLimits.MyGridLimitData>();
      this.m_transferedDelta = transferedDelta;
      this.m_PCU = initialPCU + transferedDelta;
    }

    public static bool IsFactionChangePossible(long playerId, long newFaction)
    {
      if (MySession.Static.BlockLimitsEnabled != MyBlockLimitsEnabledEnum.PER_FACTION)
        return true;
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(playerId);
      if (identity != null)
      {
        HashSet<MySlimBlock> blocksBuiltByPlayer = identity.BlockLimits.GetBlocksBuiltByPlayer(playerId);
        int count = blocksBuiltByPlayer.Count;
        int num = blocksBuiltByPlayer.Sum<MySlimBlock>((Func<MySlimBlock, int>) (x => x.BlockDefinition.PCU));
        if (!(MySession.Static.Factions.TryGetFactionById(newFaction) is MyFaction factionById))
          return false;
        MyBlockLimits blockLimits = factionById.BlockLimits;
        if (num > blockLimits.PCU && MySession.Static.Settings.TotalPCU > 0 || count > blockLimits.MaxBlocks && blockLimits.MaxBlocks > 0)
          return false;
        foreach (KeyValuePair<string, short> blockTypeLimit in MySession.Static.BlockTypeLimits)
        {
          MyBlockLimits.MyTypeLimitData myTypeLimitData1;
          MyBlockLimits.MyTypeLimitData myTypeLimitData2;
          if (identity.BlockLimits.BlockTypeBuilt.TryGetValue(blockTypeLimit.Key, out myTypeLimitData1) && blockLimits.BlockTypeBuilt.TryGetValue(blockTypeLimit.Key, out myTypeLimitData2) && myTypeLimitData1.BlocksBuilt + myTypeLimitData2.BlocksBuilt > (int) blockTypeLimit.Value)
            return false;
        }
      }
      return true;
    }

    public static void TransferBlockLimits(
      long playerId,
      MyBlockLimits oldLimits,
      MyBlockLimits newLimits)
    {
      foreach (KeyValuePair<long, MyBlockLimits.MyGridLimitData> keyValuePair in oldLimits.BlocksBuiltByGrid)
      {
        MyCubeGrid entity = (MyCubeGrid) null;
        if (Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyCubeGrid>(keyValuePair.Key, out entity))
          entity.TransferBlockLimitsBuiltByID(playerId, oldLimits, newLimits);
      }
    }

    public static void TransferBlockLimits(long oldOwner, long newOwner)
    {
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(oldOwner);
      if (identity == null)
        return;
      foreach (KeyValuePair<long, MyBlockLimits.MyGridLimitData> keyValuePair in identity.BlockLimits.BlocksBuiltByGrid)
        MyMultiplayer.RaiseStaticEvent<long, long, long>((Func<IMyEventOwner, Action<long, long, long>>) (x => new Action<long, long, long>(MyBlockLimits.TransferBlocksBuiltByID)), keyValuePair.Key, oldOwner, newOwner, new EndpointId(Sync.MyId));
    }

    public static bool IsTransferBlocksBuiltByIDPossible(
      long gridEntityId,
      long oldOwner,
      long newOwner,
      out int blocksCount,
      out int pcu)
    {
      MyIdentity identity1 = MySession.Static.Players.TryGetIdentity(oldOwner);
      MyIdentity identity2 = MySession.Static.Players.TryGetIdentity(newOwner);
      blocksCount = 0;
      pcu = 0;
      if (identity1 == null || identity2 == null)
        return false;
      if (MySession.Static.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.NONE)
        return true;
      if (!identity1.BlockLimits.BlocksBuiltByGrid.TryGetValue(gridEntityId, out MyBlockLimits.MyGridLimitData _))
        return false;
      MyCubeGrid gridFromId = MyBlockLimits.GetGridFromId(gridEntityId);
      if (gridFromId == null)
        return false;
      HashSet<MySlimBlock> blocksBuiltById = gridFromId.FindBlocksBuiltByID(oldOwner);
      blocksCount = blocksBuiltById.Count;
      pcu = blocksBuiltById.Sum<MySlimBlock>((Func<MySlimBlock, int>) (x => x.BlockDefinition.PCU));
      if (MySession.Static.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.GLOBALLY)
        return true;
      if (identity2.BlockLimits.MaxBlocks > 0 && blocksCount + identity2.BlockLimits.BlocksBuilt > identity2.BlockLimits.MaxBlocks || pcu > identity2.BlockLimits.PCU)
        return false;
      Dictionary<string, short> dictionary = new Dictionary<string, short>();
      foreach (MySlimBlock mySlimBlock in gridFromId.FindBlocksBuiltByID(oldOwner))
      {
        if (MySession.Static.BlockTypeLimits.ContainsKey(mySlimBlock.BlockDefinition.BlockPairName))
        {
          if (dictionary.ContainsKey(mySlimBlock.BlockDefinition.BlockPairName))
            dictionary[mySlimBlock.BlockDefinition.BlockPairName]++;
          else
            dictionary[mySlimBlock.BlockDefinition.BlockPairName] = (short) 1;
        }
      }
      foreach (KeyValuePair<string, MyBlockLimits.MyTypeLimitData> keyValuePair in identity2.BlockLimits.BlockTypeBuilt)
      {
        if (dictionary.ContainsKey(keyValuePair.Key))
          dictionary[keyValuePair.Key] += (short) keyValuePair.Value.BlocksBuilt;
        else
          dictionary[keyValuePair.Key] = (short) keyValuePair.Value.BlocksBuilt;
      }
      foreach (KeyValuePair<string, short> keyValuePair in dictionary)
      {
        if ((int) keyValuePair.Value > (int) MySession.Static.BlockTypeLimits[keyValuePair.Key])
          return false;
      }
      return true;
    }

    private static MyCubeGrid GetGridFromId(long gridEntityId)
    {
      MyEntity entityById = Sandbox.Game.Entities.MyEntities.GetEntityById(gridEntityId);
      if (entityById == null)
        return (MyCubeGrid) null;
      return !(entityById is MyCubeGrid myCubeGrid) ? (MyCubeGrid) null : myCubeGrid;
    }

    [Event(null, 349)]
    [Reliable]
    [Server]
    public static void SendTransferRequestMessage(
      MyBlockLimits.MyGridLimitData gridData,
      long oldOwner,
      long newOwner,
      ulong newOwnerSteamId)
    {
      int blocksCount;
      int pcu;
      if (!MyBlockLimits.IsTransferBlocksBuiltByIDPossible(gridData.EntityId, oldOwner, newOwner, out blocksCount, out pcu))
      {
        MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (x => new Action<long>(MyBlockLimits.ReceiveTransferNotPossibleMessage)), newOwner, MyEventContext.Current.Sender);
      }
      else
      {
        MyBlockLimits.MyGridLimitData myGridLimitData = MySession.Static.Players.TryGetIdentity(oldOwner).BlockLimits.BlocksBuiltByGrid[gridData.EntityId];
        if (MyEventContext.Current.IsLocallyInvoked)
          MyBlockLimits.ReceiveTransferRequestMessage(new MyBlockLimits.TransferMessageData()
          {
            EntityId = myGridLimitData.EntityId,
            GridName = myGridLimitData.GridName,
            BlocksBuilt = myGridLimitData.BlocksBuilt,
            PCUBuilt = myGridLimitData.PCUBuilt
          }, oldOwner, newOwner, blocksCount, pcu);
        else
          MyMultiplayer.RaiseStaticEvent<MyBlockLimits.TransferMessageData, long, long, int, int>((Func<IMyEventOwner, Action<MyBlockLimits.TransferMessageData, long, long, int, int>>) (x => new Action<MyBlockLimits.TransferMessageData, long, long, int, int>(MyBlockLimits.ReceiveTransferRequestMessage)), new MyBlockLimits.TransferMessageData()
          {
            EntityId = myGridLimitData.EntityId,
            GridName = myGridLimitData.GridName,
            BlocksBuilt = myGridLimitData.BlocksBuilt,
            PCUBuilt = myGridLimitData.PCUBuilt
          }, oldOwner, newOwner, blocksCount, pcu, new EndpointId(newOwnerSteamId));
      }
    }

    [Event(null, 395)]
    [Reliable]
    [Client]
    private static void ReceiveTransferRequestMessage(
      MyBlockLimits.TransferMessageData gridData,
      long oldOwner,
      long newOwner,
      int blocksCount,
      int pcu)
    {
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(oldOwner);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.MessageBoxTextConfirmAcceptTransferGrid), (object) identity.DisplayName, (object) blocksCount.ToString(), (object) pcu, (object) gridData.GridName), MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result =>
      {
        if (result != MyGuiScreenMessageBox.ResultEnum.YES)
          return;
        MyMultiplayer.RaiseStaticEvent<long, long, long>((Func<IMyEventOwner, Action<long, long, long>>) (x => new Action<long, long, long>(MyBlockLimits.TransferBlocksBuiltByID)), gridData.EntityId, oldOwner, newOwner);
      })), canHideOthers: false));
    }

    [Event(null, 415)]
    [Reliable]
    [Client]
    private static void ReceiveTransferNotPossibleMessage(long identityId)
    {
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(identityId);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder().AppendFormat(MyCommonTexts.MessageBoxTextNotEnoughFreeBlocksForTransfer, (object) identity.DisplayName), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), canHideOthers: false));
    }

    [Event(null, 428)]
    [Reliable]
    [Server]
    private static void TransferBlocksBuiltByID(long gridEntityId, long oldOwner, long newOwner)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && (long) MySession.Static.Players.TryGetSteamId(newOwner) != (long) MyEventContext.Current.Sender.Value)
      {
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        MyCubeGrid gridFromId = MyBlockLimits.GetGridFromId(gridEntityId);
        MyIdentity identity = MySession.Static.Players.TryGetIdentity(newOwner);
        if (gridFromId == null || identity == null)
          return;
        gridFromId.TransferBlocksBuiltByID(oldOwner, newOwner);
        MyMultiplayer.RaiseStaticEvent<long, long, long>((Func<IMyEventOwner, Action<long, long, long>>) (x => new Action<long, long, long>(MyBlockLimits.TransferBlocksBuiltByIDClient)), gridFromId.EntityId, oldOwner, newOwner);
        ulong steamId = MySession.Static.Players.TryGetSteamId(identity.IdentityId);
        if (steamId == 0UL)
          return;
        MyMultiplayer.RaiseStaticEvent<long, string>((Func<IMyEventOwner, Action<long, string>>) (x => new Action<long, string>(MyBlockLimits.SetGridNameFromServer)), gridFromId.EntityId, gridFromId.DisplayName, new EndpointId(steamId));
      }
    }

    [Event(null, 457)]
    [Reliable]
    [BroadcastExcept]
    public static void TransferBlocksBuiltByIDClient(
      long gridEntityId,
      long oldOwner,
      long newOwner)
    {
      MyEntity entity;
      Sandbox.Game.Entities.MyEntities.TryGetEntityById(gridEntityId, out entity);
      if (!(entity is MyCubeGrid myCubeGrid))
        return;
      myCubeGrid.TransferBlocksBuiltByIDClient(oldOwner, newOwner);
    }

    [Event(null, 471)]
    [Reliable]
    [Server]
    public static void RemoveBlocksBuiltByID(long gridEntityId, long identityID)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && (long) MySession.Static.Players.TryGetSteamId(identityID) != (long) MyEventContext.Current.Sender.Value)
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      else
        MyBlockLimits.GetGridFromId(gridEntityId)?.RemoveBlocksBuiltByID(identityID);
    }

    [Event(null, 489)]
    [Reliable]
    [Client]
    public static void SetGridNameFromServer(long gridEntityId, string newName)
    {
      MyIdentity identity = MySession.Static.LocalHumanPlayer.Identity;
      MyBlockLimits.MyGridLimitData myGridLimitData;
      if (identity.BlockLimits.BlocksBuiltByGrid.TryGetValue(gridEntityId, out myGridLimitData))
        myGridLimitData.GridName = newName;
      identity.BlockLimits.CallLimitsChanged();
    }

    private void OnGridNameChangedServer(MyCubeGrid grid)
    {
      MyBlockLimits.MyGridLimitData myGridLimitData;
      if (!this.BlocksBuiltByGrid.TryGetValue(grid.EntityId, out myGridLimitData))
        return;
      myGridLimitData.GridName = grid.DisplayName;
      myGridLimitData.NameDirty = 1;
    }

    public void IncreaseBlocksBuilt(string type, int pcu, MyCubeGrid grid, bool modifyBlockCount = true)
    {
      if (MyBlockLimits.Empty == this || !Sync.IsServer)
        return;
      if (modifyBlockCount)
        Interlocked.Increment(ref this.m_blocksBuilt);
      if (grid != null)
      {
        Interlocked.Add(ref this.m_PCUBuilt, pcu);
        Interlocked.Add(ref this.m_PCU, -pcu);
      }
      MyBlockLimits.MyTypeLimitData myTypeLimitData;
      if (modifyBlockCount && type != null && this.BlockTypeBuilt.TryGetValue(type, out myTypeLimitData))
      {
        Interlocked.Increment(ref myTypeLimitData.BlocksBuilt);
        myTypeLimitData.Dirty = 1;
      }
      if (grid == null)
        return;
      long entityId = grid.EntityId;
      bool flag = false;
      do
      {
        MyBlockLimits.MyGridLimitData myGridLimitData;
        if (this.BlocksBuiltByGrid.TryGetValue(entityId, out myGridLimitData))
        {
          if (modifyBlockCount)
            Interlocked.Increment(ref myGridLimitData.BlocksBuilt);
          Interlocked.Add(ref myGridLimitData.PCUBuilt, pcu);
          myGridLimitData.Dirty = 1;
        }
        else if (this.BlocksBuiltByGrid.TryAdd(entityId, new MyBlockLimits.MyGridLimitData()
        {
          EntityId = grid.EntityId,
          BlocksBuilt = 1,
          PCUBuilt = pcu,
          GridName = grid.DisplayName ?? "Unknown grid",
          Dirty = 1
        }))
          grid.OnNameChanged += new Action<MyCubeGrid>(this.OnGridNameChangedServer);
        else
          flag = true;
      }
      while (flag);
      this.GridsRemoved.Remove<long, MyBlockLimits.MyGridLimitData>(entityId);
    }

    public void DecreaseBlocksBuilt(string type, int pcu, MyCubeGrid grid, bool modifyBlockCount = true)
    {
      if (MyBlockLimits.Empty == this || !Sync.IsServer)
        return;
      if (modifyBlockCount)
        Interlocked.Decrement(ref this.m_blocksBuilt);
      if (grid != null)
      {
        Interlocked.Add(ref this.m_PCUBuilt, -pcu);
        Interlocked.Add(ref this.m_PCU, pcu);
      }
      MyBlockLimits.MyTypeLimitData myTypeLimitData;
      if (type != null & modifyBlockCount && this.BlockTypeBuilt.TryGetValue(type, out myTypeLimitData))
      {
        Interlocked.Decrement(ref myTypeLimitData.BlocksBuilt);
        myTypeLimitData.Dirty = 1;
      }
      if (grid == null)
        return;
      long entityId = grid.EntityId;
      MyBlockLimits.MyGridLimitData myGridLimitData;
      if (!this.BlocksBuiltByGrid.TryGetValue(entityId, out myGridLimitData))
        return;
      if (modifyBlockCount)
        Interlocked.Decrement(ref myGridLimitData.BlocksBuilt);
      Interlocked.Add(ref myGridLimitData.PCUBuilt, -pcu);
      myGridLimitData.Dirty = 1;
      if (myGridLimitData.BlocksBuilt != 0 || myGridLimitData.PCUBuilt != 0)
        return;
      this.BlocksBuiltByGrid.Remove<long, MyBlockLimits.MyGridLimitData>(entityId);
      this.GridsRemoved.TryAdd(entityId, myGridLimitData);
      grid.OnNameChanged -= new Action<MyCubeGrid>(this.OnGridNameChangedServer);
    }

    internal void AddPCU(int pcuToAdd)
    {
      if (!Sync.IsServer)
        return;
      Interlocked.Add(ref this.m_PCU, pcuToAdd);
      Interlocked.Add(ref this.m_transferedDelta, pcuToAdd);
      this.m_PCUDirty = 1;
    }

    internal bool CompareExchangePCUDirty() => Sync.IsServer && Interlocked.CompareExchange(ref this.m_PCUDirty, 0, 1) > 0;

    public void SetAllDirty()
    {
      foreach (MyBlockLimits.MyTypeLimitData myTypeLimitData in (IEnumerable<MyBlockLimits.MyTypeLimitData>) this.BlockTypeBuilt.Values)
        myTypeLimitData.Dirty = 1;
      foreach (MyBlockLimits.MyGridLimitData myGridLimitData in (IEnumerable<MyBlockLimits.MyGridLimitData>) this.BlocksBuiltByGrid.Values)
      {
        myGridLimitData.Dirty = 1;
        myGridLimitData.NameDirty = 1;
      }
    }

    public void SetTypeLimitsFromServer(MyBlockLimits.MyTypeLimitData newLimit)
    {
      if (!this.BlockTypeBuilt.ContainsKey(newLimit.BlockPairName))
        this.BlockTypeBuilt[newLimit.BlockPairName] = new MyBlockLimits.MyTypeLimitData();
      this.BlockTypeBuilt[newLimit.BlockPairName].BlocksBuilt = newLimit.BlocksBuilt;
      this.CallLimitsChanged();
    }

    public void SetGridLimitsFromServer(
      MyBlockLimits.MyGridLimitData newLimit,
      int pcu,
      int pcuBuilt,
      int blocksBuilt,
      int transferedDelta)
    {
      Interlocked.Exchange(ref this.m_PCU, pcu);
      Interlocked.Exchange(ref this.m_PCUBuilt, pcuBuilt);
      Interlocked.Exchange(ref this.m_blocksBuilt, blocksBuilt);
      Interlocked.Exchange(ref this.m_transferedDelta, transferedDelta);
      if (newLimit.BlocksBuilt == 0)
        this.BlocksBuiltByGrid.TryRemove(newLimit.EntityId, out MyBlockLimits.MyGridLimitData _);
      else if (!this.BlocksBuiltByGrid.TryAdd(newLimit.EntityId, newLimit))
      {
        MyBlockLimits.MyGridLimitData myGridLimitData = this.BlocksBuiltByGrid[newLimit.EntityId];
        myGridLimitData.BlocksBuilt = newLimit.BlocksBuilt;
        myGridLimitData.PCUBuilt = newLimit.PCUBuilt;
      }
      this.CallLimitsChanged();
    }

    public void SetPCUFromServer(int pcu, int transferedDelta)
    {
      Interlocked.Exchange(ref this.m_PCU, pcu);
      Interlocked.Exchange(ref this.m_transferedDelta, transferedDelta);
    }

    public void CallLimitsChanged()
    {
      if (this.BlockLimitsChanged == null)
        return;
      this.BlockLimitsChanged();
    }

    private HashSet<MySlimBlock> GetBlocksBuiltByPlayer(long playerId)
    {
      HashSet<MySlimBlock> builtBlocks = new HashSet<MySlimBlock>();
      foreach (KeyValuePair<long, MyBlockLimits.MyGridLimitData> keyValuePair in this.BlocksBuiltByGrid)
      {
        MyCubeGrid entity;
        if (Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyCubeGrid>(keyValuePair.Key, out entity))
          entity.FindBlocksBuiltByID(playerId, builtBlocks);
      }
      return builtBlocks;
    }

    [Serializable]
    public class MyGridLimitData
    {
      public long EntityId;
      [NoSerialize]
      public string GridName;
      public int BlocksBuilt;
      public int PCUBuilt;
      [NoSerialize]
      public int Dirty;
      [NoSerialize]
      public int NameDirty;

      protected class Sandbox_Game_World_MyBlockLimits\u003C\u003EMyGridLimitData\u003C\u003EEntityId\u003C\u003EAccessor : IMemberAccessor<MyBlockLimits.MyGridLimitData, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyBlockLimits.MyGridLimitData owner, in long value) => owner.EntityId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyBlockLimits.MyGridLimitData owner, out long value) => value = owner.EntityId;
      }

      protected class Sandbox_Game_World_MyBlockLimits\u003C\u003EMyGridLimitData\u003C\u003EGridName\u003C\u003EAccessor : IMemberAccessor<MyBlockLimits.MyGridLimitData, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyBlockLimits.MyGridLimitData owner, in string value) => owner.GridName = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyBlockLimits.MyGridLimitData owner, out string value) => value = owner.GridName;
      }

      protected class Sandbox_Game_World_MyBlockLimits\u003C\u003EMyGridLimitData\u003C\u003EBlocksBuilt\u003C\u003EAccessor : IMemberAccessor<MyBlockLimits.MyGridLimitData, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyBlockLimits.MyGridLimitData owner, in int value) => owner.BlocksBuilt = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyBlockLimits.MyGridLimitData owner, out int value) => value = owner.BlocksBuilt;
      }

      protected class Sandbox_Game_World_MyBlockLimits\u003C\u003EMyGridLimitData\u003C\u003EPCUBuilt\u003C\u003EAccessor : IMemberAccessor<MyBlockLimits.MyGridLimitData, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyBlockLimits.MyGridLimitData owner, in int value) => owner.PCUBuilt = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyBlockLimits.MyGridLimitData owner, out int value) => value = owner.PCUBuilt;
      }

      protected class Sandbox_Game_World_MyBlockLimits\u003C\u003EMyGridLimitData\u003C\u003EDirty\u003C\u003EAccessor : IMemberAccessor<MyBlockLimits.MyGridLimitData, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyBlockLimits.MyGridLimitData owner, in int value) => owner.Dirty = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyBlockLimits.MyGridLimitData owner, out int value) => value = owner.Dirty;
      }

      protected class Sandbox_Game_World_MyBlockLimits\u003C\u003EMyGridLimitData\u003C\u003ENameDirty\u003C\u003EAccessor : IMemberAccessor<MyBlockLimits.MyGridLimitData, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyBlockLimits.MyGridLimitData owner, in int value) => owner.NameDirty = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyBlockLimits.MyGridLimitData owner, out int value) => value = owner.NameDirty;
      }
    }

    [Serializable]
    public class MyTypeLimitData
    {
      public string BlockPairName;
      public int BlocksBuilt;
      [NoSerialize]
      public int Dirty;

      protected class Sandbox_Game_World_MyBlockLimits\u003C\u003EMyTypeLimitData\u003C\u003EBlockPairName\u003C\u003EAccessor : IMemberAccessor<MyBlockLimits.MyTypeLimitData, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyBlockLimits.MyTypeLimitData owner, in string value) => owner.BlockPairName = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyBlockLimits.MyTypeLimitData owner, out string value) => value = owner.BlockPairName;
      }

      protected class Sandbox_Game_World_MyBlockLimits\u003C\u003EMyTypeLimitData\u003C\u003EBlocksBuilt\u003C\u003EAccessor : IMemberAccessor<MyBlockLimits.MyTypeLimitData, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyBlockLimits.MyTypeLimitData owner, in int value) => owner.BlocksBuilt = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyBlockLimits.MyTypeLimitData owner, out int value) => value = owner.BlocksBuilt;
      }

      protected class Sandbox_Game_World_MyBlockLimits\u003C\u003EMyTypeLimitData\u003C\u003EDirty\u003C\u003EAccessor : IMemberAccessor<MyBlockLimits.MyTypeLimitData, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyBlockLimits.MyTypeLimitData owner, in int value) => owner.Dirty = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyBlockLimits.MyTypeLimitData owner, out int value) => value = owner.Dirty;
      }
    }

    [Serializable]
    private struct TransferMessageData
    {
      public long EntityId;
      public string GridName;
      public int BlocksBuilt;
      public int PCUBuilt;

      protected class Sandbox_Game_World_MyBlockLimits\u003C\u003ETransferMessageData\u003C\u003EEntityId\u003C\u003EAccessor : IMemberAccessor<MyBlockLimits.TransferMessageData, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyBlockLimits.TransferMessageData owner, in long value) => owner.EntityId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyBlockLimits.TransferMessageData owner, out long value) => value = owner.EntityId;
      }

      protected class Sandbox_Game_World_MyBlockLimits\u003C\u003ETransferMessageData\u003C\u003EGridName\u003C\u003EAccessor : IMemberAccessor<MyBlockLimits.TransferMessageData, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyBlockLimits.TransferMessageData owner, in string value) => owner.GridName = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyBlockLimits.TransferMessageData owner, out string value) => value = owner.GridName;
      }

      protected class Sandbox_Game_World_MyBlockLimits\u003C\u003ETransferMessageData\u003C\u003EBlocksBuilt\u003C\u003EAccessor : IMemberAccessor<MyBlockLimits.TransferMessageData, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyBlockLimits.TransferMessageData owner, in int value) => owner.BlocksBuilt = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyBlockLimits.TransferMessageData owner, out int value) => value = owner.BlocksBuilt;
      }

      protected class Sandbox_Game_World_MyBlockLimits\u003C\u003ETransferMessageData\u003C\u003EPCUBuilt\u003C\u003EAccessor : IMemberAccessor<MyBlockLimits.TransferMessageData, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyBlockLimits.TransferMessageData owner, in int value) => owner.PCUBuilt = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyBlockLimits.TransferMessageData owner, out int value) => value = owner.PCUBuilt;
      }
    }

    protected sealed class SendTransferRequestMessage\u003C\u003ESandbox_Game_World_MyBlockLimits\u003C\u003EMyGridLimitData\u0023System_Int64\u0023System_Int64\u0023System_UInt64 : ICallSite<IMyEventOwner, MyBlockLimits.MyGridLimitData, long, long, ulong, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyBlockLimits.MyGridLimitData gridData,
        in long oldOwner,
        in long newOwner,
        in ulong newOwnerSteamId,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyBlockLimits.SendTransferRequestMessage(gridData, oldOwner, newOwner, newOwnerSteamId);
      }
    }

    protected sealed class ReceiveTransferRequestMessage\u003C\u003ESandbox_Game_World_MyBlockLimits\u003C\u003ETransferMessageData\u0023System_Int64\u0023System_Int64\u0023System_Int32\u0023System_Int32 : ICallSite<IMyEventOwner, MyBlockLimits.TransferMessageData, long, long, int, int, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyBlockLimits.TransferMessageData gridData,
        in long oldOwner,
        in long newOwner,
        in int blocksCount,
        in int pcu,
        in DBNull arg6)
      {
        MyBlockLimits.ReceiveTransferRequestMessage(gridData, oldOwner, newOwner, blocksCount, pcu);
      }
    }

    protected sealed class ReceiveTransferNotPossibleMessage\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long identityId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyBlockLimits.ReceiveTransferNotPossibleMessage(identityId);
      }
    }

    protected sealed class TransferBlocksBuiltByID\u003C\u003ESystem_Int64\u0023System_Int64\u0023System_Int64 : ICallSite<IMyEventOwner, long, long, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long gridEntityId,
        in long oldOwner,
        in long newOwner,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyBlockLimits.TransferBlocksBuiltByID(gridEntityId, oldOwner, newOwner);
      }
    }

    protected sealed class TransferBlocksBuiltByIDClient\u003C\u003ESystem_Int64\u0023System_Int64\u0023System_Int64 : ICallSite<IMyEventOwner, long, long, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long gridEntityId,
        in long oldOwner,
        in long newOwner,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyBlockLimits.TransferBlocksBuiltByIDClient(gridEntityId, oldOwner, newOwner);
      }
    }

    protected sealed class RemoveBlocksBuiltByID\u003C\u003ESystem_Int64\u0023System_Int64 : ICallSite<IMyEventOwner, long, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long gridEntityId,
        in long identityID,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyBlockLimits.RemoveBlocksBuiltByID(gridEntityId, identityID);
      }
    }

    protected sealed class SetGridNameFromServer\u003C\u003ESystem_Int64\u0023System_String : ICallSite<IMyEventOwner, long, string, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long gridEntityId,
        in string newName,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyBlockLimits.SetGridNameFromServer(gridEntityId, newName);
      }
    }
  }
}
