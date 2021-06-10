// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MySessionComponentResearch
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components;
using VRage.Network;
using VRage.ObjectBuilders;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.SessionComponents
{
  [StaticEventOwner]
  [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate, 666, typeof (MyObjectBuilder_SessionComponentResearch), null, false)]
  public class MySessionComponentResearch : MySessionComponentBase
  {
    public bool DEBUG_SHOW_RESEARCH;
    public bool DEBUG_SHOW_RESEARCH_PRETTY = true;
    public static MySessionComponentResearch Static;
    private Dictionary<long, HashSet<MyDefinitionId>> m_unlockedResearch;
    private Dictionary<long, HashSet<MyDefinitionId>> m_unlockedBlocks;
    public List<MyDefinitionId> m_requiredResearch;
    private Dictionary<MyDefinitionId, List<MyDefinitionId>> m_unlocks;
    private Dictionary<long, bool> m_failedSent;
    private MyHudNotification m_unlockedResearchNotification;
    private MyHudNotification m_factionUnlockedResearchNotification;
    private MyHudNotification m_factionFailedResearchNotification;
    private MyHudNotification m_sharedResearchNotification;
    private MyHudNotification m_knownResearchNotification;

    public bool WhitelistMode { get; private set; }

    public override bool IsRequiredByGame => MyPerGameSettings.EnableResearch;

    public override Type[] Dependencies => base.Dependencies;

    public MySessionComponentResearch()
    {
      MySessionComponentResearch.Static = this;
      this.m_unlockedResearch = new Dictionary<long, HashSet<MyDefinitionId>>();
      this.m_unlockedBlocks = new Dictionary<long, HashSet<MyDefinitionId>>();
      this.m_requiredResearch = new List<MyDefinitionId>();
      this.m_unlocks = new Dictionary<MyDefinitionId, List<MyDefinitionId>>();
      this.m_failedSent = new Dictionary<long, bool>();
      this.m_unlockedResearchNotification = new MyHudNotification(MyCommonTexts.NotificationResearchUnlocked, font: "White", priority: 2);
      this.m_factionUnlockedResearchNotification = new MyHudNotification(MyCommonTexts.NotificationFactionResearchUnlocked, font: "White", priority: 2);
      this.m_factionFailedResearchNotification = new MyHudNotification(MyCommonTexts.NotificationFactionResearchFailed, font: "White", priority: 2);
      this.m_sharedResearchNotification = new MyHudNotification(MyCommonTexts.NotificationSharedResearch, font: "White", priority: 2);
      this.m_knownResearchNotification = new MyHudNotification(MyCommonTexts.NotificationResearchKnown, font: "Red", priority: 2);
    }

    public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
    {
      base.Init(sessionComponent);
      if (MySessionComponentResearch.Static != null)
      {
        foreach (MyCubeBlockDefinition cubeBlockDefinition in MyDefinitionManager.Static.GetDefinitionsOfType<MyCubeBlockDefinition>())
        {
          MyResearchBlockDefinition researchBlock1 = MyDefinitionManager.Static.GetResearchBlock(cubeBlockDefinition.Id);
          if (researchBlock1 != null)
          {
            if (cubeBlockDefinition.CubeSize == MyCubeSize.Large)
            {
              MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(cubeBlockDefinition.BlockPairName);
              if (definitionGroup.Small != null)
              {
                MyResearchBlockDefinition researchBlock2 = MyDefinitionManager.Static.GetResearchBlock(definitionGroup.Small.Id);
                if (researchBlock2 != null)
                {
                  string[] unlockedByGroups = researchBlock2.UnlockedByGroups;
                }
              }
            }
            foreach (string unlockedByGroup in researchBlock1.UnlockedByGroups)
            {
              MyResearchGroupDefinition researchGroup = MyDefinitionManager.Static.GetResearchGroup(unlockedByGroup);
              if (researchGroup != null && !researchGroup.Members.IsNullOrEmpty<SerializableDefinitionId>())
              {
                this.m_requiredResearch.Add(cubeBlockDefinition.Id);
                foreach (SerializableDefinitionId member in researchGroup.Members)
                {
                  if (MyDefinitionManager.Static.TryGetCubeBlockDefinition((MyDefinitionId) member, out MyCubeBlockDefinition _))
                  {
                    List<MyDefinitionId> myDefinitionIdList;
                    if (!this.m_unlocks.TryGetValue((MyDefinitionId) member, out myDefinitionIdList))
                    {
                      myDefinitionIdList = new List<MyDefinitionId>();
                      this.m_unlocks.Add((MyDefinitionId) member, myDefinitionIdList);
                    }
                    myDefinitionIdList.Add(cubeBlockDefinition.Id);
                  }
                }
              }
            }
          }
        }
      }
      if (sessionComponent is MyObjectBuilder_SessionComponentResearch componentResearch && componentResearch.Researches != null)
      {
        foreach (MyObjectBuilder_SessionComponentResearch.ResearchData research in componentResearch.Researches)
        {
          HashSet<MyDefinitionId> myDefinitionIdSet1 = new HashSet<MyDefinitionId>();
          HashSet<MyDefinitionId> myDefinitionIdSet2 = new HashSet<MyDefinitionId>();
          foreach (SerializableDefinitionId definition in research.Definitions)
          {
            myDefinitionIdSet1.Add((MyDefinitionId) definition);
            List<MyDefinitionId> myDefinitionIdList;
            if (MySessionComponentResearch.Static.m_unlocks.TryGetValue((MyDefinitionId) definition, out myDefinitionIdList))
            {
              foreach (MyDefinitionId myDefinitionId in myDefinitionIdList)
                myDefinitionIdSet2.Add(myDefinitionId);
            }
          }
          this.m_unlockedResearch.Add(research.IdentityId, myDefinitionIdSet1);
          this.m_unlockedBlocks.Add(research.IdentityId, myDefinitionIdSet2);
        }
        this.WhitelistMode = componentResearch.WhitelistMode;
        if (this.WhitelistMode)
          this.m_requiredResearch.Clear();
      }
      if (!Sync.IsServer || !MySession.Static.ResearchEnabled)
        return;
      MyCubeGrids.BlockFunctional += new Action<MyCubeGrid, MySlimBlock, bool>(this.OnBlockBuilt);
    }

    private void OnBlockBuilt(MyCubeGrid grid, MySlimBlock block, bool handWelded)
    {
      if (!handWelded)
        return;
      long builtBy = block.BuiltBy;
      MyDefinitionId id = block.BlockDefinition.Id;
      IMyFaction playerFaction = MySession.Static.Factions.TryGetPlayerFaction(builtBy);
      if (playerFaction != null)
      {
        foreach (long key in playerFaction.Members.Keys)
        {
          if (MySession.Static.Players.IsPlayerOnline(key))
            this.UnlockResearch(key, id, builtBy);
        }
      }
      else
        this.UnlockResearch(builtBy, id, builtBy);
    }

    public override MyObjectBuilder_SessionComponent GetObjectBuilder()
    {
      MyObjectBuilder_SessionComponentResearch componentResearch = new MyObjectBuilder_SessionComponentResearch();
      componentResearch.Researches = new List<MyObjectBuilder_SessionComponentResearch.ResearchData>();
      foreach (KeyValuePair<long, HashSet<MyDefinitionId>> keyValuePair in this.m_unlockedResearch)
      {
        if (keyValuePair.Value.Count != 0)
        {
          List<SerializableDefinitionId> serializableDefinitionIdList = new List<SerializableDefinitionId>();
          foreach (MyDefinitionId myDefinitionId in keyValuePair.Value)
            serializableDefinitionIdList.Add((SerializableDefinitionId) myDefinitionId);
          componentResearch.Researches.Add(new MyObjectBuilder_SessionComponentResearch.ResearchData()
          {
            IdentityId = keyValuePair.Key,
            Definitions = serializableDefinitionIdList
          });
        }
      }
      componentResearch.WhitelistMode = this.WhitelistMode;
      return (MyObjectBuilder_SessionComponent) componentResearch;
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      this.m_unlockedResearch = (Dictionary<long, HashSet<MyDefinitionId>>) null;
      this.m_unlockedBlocks = (Dictionary<long, HashSet<MyDefinitionId>>) null;
      this.m_requiredResearch = (List<MyDefinitionId>) null;
      this.m_unlocks = (Dictionary<MyDefinitionId, List<MyDefinitionId>>) null;
      if (Sync.IsServer && MySession.Static.ResearchEnabled)
        MyCubeGrids.BlockFunctional -= new Action<MyCubeGrid, MySlimBlock, bool>(this.OnBlockBuilt);
      MySessionComponentResearch.Static = (MySessionComponentResearch) null;
    }

    public void UnlockResearch(long identityId, MyDefinitionId id, long unlockerId)
    {
      HashSet<MyDefinitionId> myDefinitionIdSet;
      if (!this.m_unlockedResearch.TryGetValue(identityId, out myDefinitionIdSet))
      {
        myDefinitionIdSet = new HashSet<MyDefinitionId>();
        this.m_unlockedResearch.Add(identityId, myDefinitionIdSet);
        this.m_unlockedBlocks.Add(identityId, new HashSet<MyDefinitionId>());
      }
      if (myDefinitionIdSet.Contains(id))
        return;
      SerializableDefinitionId serializableDefinitionId = (SerializableDefinitionId) id;
      if (!this.CanUse(identityId, id))
      {
        if (unlockerId != identityId)
        {
          bool flag;
          if (!this.m_failedSent.TryGetValue(identityId, out flag) || !flag)
          {
            ulong steamId = MySession.Static.Players.TryGetSteamId(identityId);
            if (steamId == 0UL)
              return;
            MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (x => new Action(MySessionComponentResearch.FactionUnlockFailed)), new EndpointId(steamId));
            this.m_failedSent[identityId] = true;
            return;
          }
        }
        else if (!MySession.Static.HasPlayerCreativeRights(MySession.Static.Players.TryGetSteamId(identityId)))
          return;
      }
      MyMultiplayer.RaiseStaticEvent<long, SerializableDefinitionId, long>((Func<IMyEventOwner, Action<long, SerializableDefinitionId, long>>) (x => new Action<long, SerializableDefinitionId, long>(MySessionComponentResearch.UnlockResearchSuccess)), identityId, serializableDefinitionId, unlockerId);
    }

    [Event(null, 290)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void UnlockResearchSuccess(
      long identityId,
      SerializableDefinitionId id,
      long unlockerId)
    {
      MyDefinitionBase definition;
      if (!MyDefinitionManager.Static.TryGetDefinition<MyDefinitionBase>((MyDefinitionId) id, out definition) || !MySessionComponentResearch.Static.UnlockBlocks(identityId, (MyDefinitionId) id) || (MySession.Static.LocalCharacter == null || MySession.Static.LocalCharacter.GetPlayerIdentityId() != identityId))
        return;
      if (unlockerId != identityId)
      {
        MyIdentity identity = MySession.Static.Players.TryGetIdentity(unlockerId);
        if (identity == null)
          return;
        MySessionComponentResearch.Static.m_factionUnlockedResearchNotification.SetTextFormatArguments((object) definition.DisplayNameText, (object) identity.DisplayName);
        MyHud.Notifications.Add((MyHudNotificationBase) MySessionComponentResearch.Static.m_factionUnlockedResearchNotification);
      }
      else
      {
        MySessionComponentResearch.Static.m_unlockedResearchNotification.SetTextFormatArguments((object) definition.DisplayNameText);
        MyHud.Notifications.Add((MyHudNotificationBase) MySessionComponentResearch.Static.m_unlockedResearchNotification);
      }
    }

    [Event(null, 326)]
    [Reliable]
    [Client]
    private static void FactionUnlockFailed() => MyHud.Notifications.Add((MyHudNotificationBase) MySessionComponentResearch.Static.m_factionFailedResearchNotification);

    private bool UnlockBlocks(long identityId, MyDefinitionId researchedBlockId)
    {
      HashSet<MyDefinitionId> myDefinitionIdSet1;
      if (!this.m_unlockedBlocks.TryGetValue(identityId, out myDefinitionIdSet1))
      {
        myDefinitionIdSet1 = new HashSet<MyDefinitionId>();
        this.m_unlockedBlocks[identityId] = myDefinitionIdSet1;
      }
      HashSet<MyDefinitionId> myDefinitionIdSet2;
      if (!this.m_unlockedResearch.TryGetValue(identityId, out myDefinitionIdSet2))
      {
        myDefinitionIdSet2 = new HashSet<MyDefinitionId>();
        this.m_unlockedResearch[identityId] = myDefinitionIdSet2;
      }
      List<MyDefinitionId> myDefinitionIdList;
      this.m_unlocks.TryGetValue(researchedBlockId, out myDefinitionIdList);
      bool flag = false;
      if (myDefinitionIdList != null)
      {
        foreach (MyDefinitionId myDefinitionId in myDefinitionIdList)
        {
          if (!myDefinitionIdSet1.Contains(myDefinitionId))
          {
            flag = true;
            myDefinitionIdSet1.Add(myDefinitionId);
          }
        }
      }
      myDefinitionIdSet2.Add(researchedBlockId);
      return flag;
    }

    public bool CanUse(MyCharacter character, MyDefinitionId id) => character == null || this.CanUse(character.GetPlayerIdentityId(), id);

    public bool CanUse(long identityId, MyDefinitionId id) => !MySession.Static.ResearchEnabled || !this.RequiresResearch(id) || this.IsBlockUnlocked(identityId, id);

    public bool RequiresResearch(MyDefinitionId id) => this.WhitelistMode ? !this.m_requiredResearch.Contains(id) : this.m_requiredResearch.Contains(id);

    public bool IsBlockUnlocked(long identityId, MyDefinitionId id)
    {
      HashSet<MyDefinitionId> myDefinitionIdSet;
      return this.m_unlockedBlocks.TryGetValue(identityId, out myDefinitionIdSet) && myDefinitionIdSet.Contains(id);
    }

    [Event(null, 398)]
    [Reliable]
    [Server]
    public static void CallShareResearch(long toIdentity)
    {
      long identityId = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value, 0);
      MyMultiplayer.RaiseStaticEvent<long, long>((Func<IMyEventOwner, Action<long, long>>) (x => new Action<long, long>(MySessionComponentResearch.ShareResearch)), toIdentity, identityId);
    }

    [Event(null, 405)]
    [Reliable]
    [ServerInvoked]
    [Broadcast]
    private static void ShareResearch(long toIdentity, long fromIdentityId)
    {
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(fromIdentityId);
      if (identity == null)
        return;
      HashSet<MyDefinitionId> myDefinitionIdSet;
      if (MySessionComponentResearch.Static.m_unlockedResearch.TryGetValue(fromIdentityId, out myDefinitionIdSet))
      {
        foreach (MyDefinitionId researchedBlockId in myDefinitionIdSet)
          MySessionComponentResearch.Static.UnlockBlocks(toIdentity, researchedBlockId);
      }
      if (MySession.Static.LocalCharacter == null || MySession.Static.LocalCharacter.GetPlayerIdentityId() != toIdentity)
        return;
      MySessionComponentResearch.Static.m_sharedResearchNotification.SetTextFormatArguments((object) identity.DisplayName);
      MyHud.Notifications.Add((MyHudNotificationBase) MySessionComponentResearch.Static.m_sharedResearchNotification);
    }

    public void ResetResearch(MyCharacter character)
    {
      if (character == null)
        return;
      this.ResetResearch(character.GetPlayerIdentityId());
    }

    public void ResetResearch(long identityId) => MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (x => new Action<long>(MySessionComponentResearch.ResetResearchSync)), identityId);

    public void DebugUnlockAllResearch(long identityId) => MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (x => new Action<long>(MySessionComponentResearch.DebugUnlockAllResearchSync)), identityId);

    [Event(null, 449)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void DebugUnlockAllResearchSync(long identityId)
    {
      foreach (MyDefinitionId myDefinitionId in MySessionComponentResearch.Static.m_requiredResearch)
      {
        HashSet<MyDefinitionId> myDefinitionIdSet1;
        if (!MySessionComponentResearch.Static.m_unlockedBlocks.TryGetValue(identityId, out myDefinitionIdSet1))
        {
          myDefinitionIdSet1 = new HashSet<MyDefinitionId>();
          MySessionComponentResearch.Static.m_unlockedBlocks[identityId] = myDefinitionIdSet1;
        }
        HashSet<MyDefinitionId> myDefinitionIdSet2;
        if (!MySessionComponentResearch.Static.m_unlockedResearch.TryGetValue(identityId, out myDefinitionIdSet2))
        {
          myDefinitionIdSet2 = new HashSet<MyDefinitionId>();
          MySessionComponentResearch.Static.m_unlockedResearch[identityId] = myDefinitionIdSet2;
        }
        if (!myDefinitionIdSet1.Contains(myDefinitionId))
          myDefinitionIdSet1.Add(myDefinitionId);
        myDefinitionIdSet2.Add(myDefinitionId);
      }
    }

    public override void Draw()
    {
      base.Draw();
      if (!this.DEBUG_SHOW_RESEARCH)
        return;
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      HashSet<MyDefinitionId> myDefinitionIdSet;
      if (localCharacter == null || !this.m_unlockedResearch.TryGetValue(localCharacter.GetPlayerIdentityId(), out myDefinitionIdSet))
        return;
      MyRenderProxy.DebugDrawText2D(new Vector2(10f, 180f), string.Format("=== {0}'s Research ===", (object) MySession.Static.LocalHumanPlayer.DisplayName), Color.DarkViolet, 0.8f);
      int num = 200;
      foreach (MyDefinitionId id in myDefinitionIdSet)
      {
        if (this.DEBUG_SHOW_RESEARCH_PRETTY)
        {
          MyDefinitionBase definition = MyDefinitionManager.Static.GetDefinition(id);
          if (definition is MyResearchDefinition)
            MyRenderProxy.DebugDrawText2D(new Vector2(10f, (float) num), string.Format("[R] {0}", (object) definition.DisplayNameText), Color.DarkViolet, 0.7f);
          else
            MyRenderProxy.DebugDrawText2D(new Vector2(10f, (float) num), definition.DisplayNameText, Color.DarkViolet, 0.7f);
        }
        else
          MyRenderProxy.DebugDrawText2D(new Vector2(10f, (float) num), id.ToString(), Color.DarkViolet, 0.7f);
        num += 16;
      }
    }

    public void AddRequiredResearch(MyDefinitionId itemId)
    {
      if (itemId.TypeId.IsNull)
        return;
      MyMultiplayer.RaiseStaticEvent<SerializableDefinitionId>((Func<IMyEventOwner, Action<SerializableDefinitionId>>) (x => new Action<SerializableDefinitionId>(MySessionComponentResearch.AddRequiredResearchSync)), (SerializableDefinitionId) itemId);
    }

    [Event(null, 523)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void AddRequiredResearchSync(SerializableDefinitionId itemId)
    {
      MyDefinitionBase definition;
      if (!MyDefinitionManager.Static.TryGetDefinition<MyDefinitionBase>((MyDefinitionId) itemId, out definition) || MySessionComponentResearch.Static.m_requiredResearch.Contains(definition.Id))
        return;
      MySessionComponentResearch.Static.m_requiredResearch.Add(definition.Id);
    }

    public void RemoveRequiredResearch(MyDefinitionId itemId)
    {
      if (itemId.TypeId.IsNull)
        return;
      MyMultiplayer.RaiseStaticEvent<SerializableDefinitionId>((Func<IMyEventOwner, Action<SerializableDefinitionId>>) (x => new Action<SerializableDefinitionId>(MySessionComponentResearch.RemoveRequiredResearchSync)), (SerializableDefinitionId) itemId);
    }

    [Event(null, 545)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void RemoveRequiredResearchSync(SerializableDefinitionId itemId)
    {
      MyDefinitionBase definition;
      if (!MyDefinitionManager.Static.TryGetDefinition<MyDefinitionBase>((MyDefinitionId) itemId, out definition))
        return;
      MySessionComponentResearch.Static.m_requiredResearch.Remove(definition.Id);
    }

    public void ClearRequiredResearch() => MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (x => new Action(MySessionComponentResearch.ClearRequiredResearchSync)));

    [Event(null, 560)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void ClearRequiredResearchSync() => MySessionComponentResearch.Static.m_requiredResearch.Clear();

    public void ResetResearchForAll() => MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (x => new Action(MySessionComponentResearch.ResetResearchForAllSync)));

    [Event(null, 571)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void ResetResearchForAllSync()
    {
      MySessionComponentResearch.Static.m_unlockedResearch.Clear();
      MySessionComponentResearch.Static.m_unlockedBlocks.Clear();
    }

    public void LockResearch(long characterId, MyDefinitionId itemId)
    {
      if (itemId.TypeId.IsNull)
        return;
      SerializableDefinitionId serializableDefinitionId = (SerializableDefinitionId) itemId;
      MyMultiplayer.RaiseStaticEvent<long, SerializableDefinitionId>((Func<IMyEventOwner, Action<long, SerializableDefinitionId>>) (x => new Action<long, SerializableDefinitionId>(MySessionComponentResearch.LockResearchSync)), characterId, serializableDefinitionId);
    }

    [Event(null, 589)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void LockResearchSync(long characterId, SerializableDefinitionId itemId)
    {
      MyDefinitionBase definition;
      if (!MyDefinitionManager.Static.TryGetDefinition<MyDefinitionBase>((MyDefinitionId) itemId, out definition) || !MySessionComponentResearch.Static.m_unlockedResearch.ContainsKey(characterId))
        return;
      MySessionComponentResearch.Static.m_unlockedResearch[characterId].Remove(definition.Id);
    }

    public void UnlockResearchDirect(long characterId, MyDefinitionId itemId)
    {
      if (itemId.TypeId.IsNull)
        return;
      SerializableDefinitionId serializableDefinitionId = (SerializableDefinitionId) itemId;
      MyMultiplayer.RaiseStaticEvent<long, SerializableDefinitionId>((Func<IMyEventOwner, Action<long, SerializableDefinitionId>>) (x => new Action<long, SerializableDefinitionId>(MySessionComponentResearch.UnlockResearchDirectSync)), characterId, serializableDefinitionId);
    }

    [Event(null, 610)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void UnlockResearchDirectSync(long characterId, SerializableDefinitionId itemId)
    {
      MyDefinitionBase definition;
      if (!MyDefinitionManager.Static.TryGetDefinition<MyDefinitionBase>((MyDefinitionId) itemId, out definition) || MySessionComponentResearch.Static.m_unlockedResearch.ContainsKey(characterId) && MySessionComponentResearch.Static.m_unlockedResearch[characterId].Contains(definition.Id))
        return;
      MySessionComponentResearch.Static.UnlockBlocks(characterId, (MyDefinitionId) itemId);
    }

    [Event(null, 622)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void ResetResearchSync(long identityId)
    {
      if (MySessionComponentResearch.Static.m_unlockedResearch.ContainsKey(identityId))
        MySessionComponentResearch.Static.m_unlockedResearch[identityId].Clear();
      if (!MySessionComponentResearch.Static.m_unlockedBlocks.ContainsKey(identityId))
        return;
      MySessionComponentResearch.Static.m_unlockedBlocks[identityId].Clear();
    }

    public void SwitchWhitelistMode(bool whitelist) => MyMultiplayer.RaiseStaticEvent<bool>((Func<IMyEventOwner, Action<bool>>) (x => new Action<bool>(MySessionComponentResearch.SwitchWhitelistModeSync)), whitelist);

    [Event(null, 643)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void SwitchWhitelistModeSync(bool whitelist)
    {
      MySessionComponentResearch.Static.m_requiredResearch.Clear();
      MySessionComponentResearch.Static.WhitelistMode = whitelist;
    }

    protected sealed class UnlockResearchSuccess\u003C\u003ESystem_Int64\u0023VRage_ObjectBuilders_SerializableDefinitionId\u0023System_Int64 : ICallSite<IMyEventOwner, long, SerializableDefinitionId, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long identityId,
        in SerializableDefinitionId id,
        in long unlockerId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentResearch.UnlockResearchSuccess(identityId, id, unlockerId);
      }
    }

    protected sealed class FactionUnlockFailed\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
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
        MySessionComponentResearch.FactionUnlockFailed();
      }
    }

    protected sealed class CallShareResearch\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long toIdentity,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentResearch.CallShareResearch(toIdentity);
      }
    }

    protected sealed class ShareResearch\u003C\u003ESystem_Int64\u0023System_Int64 : ICallSite<IMyEventOwner, long, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long toIdentity,
        in long fromIdentityId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentResearch.ShareResearch(toIdentity, fromIdentityId);
      }
    }

    protected sealed class DebugUnlockAllResearchSync\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
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
        MySessionComponentResearch.DebugUnlockAllResearchSync(identityId);
      }
    }

    protected sealed class AddRequiredResearchSync\u003C\u003EVRage_ObjectBuilders_SerializableDefinitionId : ICallSite<IMyEventOwner, SerializableDefinitionId, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in SerializableDefinitionId itemId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentResearch.AddRequiredResearchSync(itemId);
      }
    }

    protected sealed class RemoveRequiredResearchSync\u003C\u003EVRage_ObjectBuilders_SerializableDefinitionId : ICallSite<IMyEventOwner, SerializableDefinitionId, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in SerializableDefinitionId itemId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentResearch.RemoveRequiredResearchSync(itemId);
      }
    }

    protected sealed class ClearRequiredResearchSync\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
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
        MySessionComponentResearch.ClearRequiredResearchSync();
      }
    }

    protected sealed class ResetResearchForAllSync\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
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
        MySessionComponentResearch.ResetResearchForAllSync();
      }
    }

    protected sealed class LockResearchSync\u003C\u003ESystem_Int64\u0023VRage_ObjectBuilders_SerializableDefinitionId : ICallSite<IMyEventOwner, long, SerializableDefinitionId, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long characterId,
        in SerializableDefinitionId itemId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentResearch.LockResearchSync(characterId, itemId);
      }
    }

    protected sealed class UnlockResearchDirectSync\u003C\u003ESystem_Int64\u0023VRage_ObjectBuilders_SerializableDefinitionId : ICallSite<IMyEventOwner, long, SerializableDefinitionId, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long characterId,
        in SerializableDefinitionId itemId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentResearch.UnlockResearchDirectSync(characterId, itemId);
      }
    }

    protected sealed class ResetResearchSync\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
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
        MySessionComponentResearch.ResetResearchSync(identityId);
      }
    }

    protected sealed class SwitchWhitelistModeSync\u003C\u003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in bool whitelist,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentResearch.SwitchWhitelistModeSync(whitelist);
      }
    }
  }
}
