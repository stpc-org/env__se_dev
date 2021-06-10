// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyDataReceiver
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Gui;
using VRage.Groups;

namespace Sandbox.Game.Entities
{
  public abstract class MyDataReceiver : MyEntityComponentBase
  {
    protected List<MyDataBroadcaster> m_tmpBroadcasters = new List<MyDataBroadcaster>();
    protected HashSet<MyDataBroadcaster> m_broadcastersInRange = new HashSet<MyDataBroadcaster>();
    protected List<MyDataBroadcaster> m_lastBroadcastersInRange = new List<MyDataBroadcaster>();
    private HashSet<MyGridLogicalGroupData> m_broadcastersInRange_TopGrids = new HashSet<MyGridLogicalGroupData>();
    private HashSet<long> m_entitiesOnHud = new HashSet<long>();

    public event MyDataReceiver.BroadcasterChangeInfo OnBroadcasterFound;

    public event MyDataReceiver.BroadcasterChangeInfo OnBroadcasterLost;

    public bool Enabled { get; set; }

    public HashSet<MyDataBroadcaster> BroadcastersInRange => this.m_broadcastersInRange;

    public MyDataBroadcaster Broadcaster
    {
      get
      {
        MyDataBroadcaster component = (MyDataBroadcaster) null;
        if (this.Container != null)
          this.Container.TryGet<MyDataBroadcaster>(out component);
        return component;
      }
    }

    public void UpdateBroadcastersInRange()
    {
      this.m_broadcastersInRange.Clear();
      if (!MyFakes.ENABLE_RADIO_HUD || !this.Enabled)
        return;
      MyDataBroadcaster component;
      if (this.Entity.Components.TryGet<MyDataBroadcaster>(out component))
        this.m_broadcastersInRange.Add(component);
      this.GetBroadcastersInMyRange(ref this.m_broadcastersInRange);
      for (int index = this.m_lastBroadcastersInRange.Count - 1; index >= 0; --index)
      {
        MyDataBroadcaster broadcaster = this.m_lastBroadcastersInRange[index];
        if (!this.m_broadcastersInRange.Contains(broadcaster))
        {
          this.m_lastBroadcastersInRange.RemoveAtFast<MyDataBroadcaster>(index);
          MyDataReceiver.BroadcasterChangeInfo onBroadcasterLost = this.OnBroadcasterLost;
          if (onBroadcasterLost != null)
            onBroadcasterLost(broadcaster);
        }
      }
      foreach (MyDataBroadcaster broadcaster in this.m_broadcastersInRange)
      {
        if (!this.m_lastBroadcastersInRange.Contains(broadcaster))
        {
          this.m_lastBroadcastersInRange.Add(broadcaster);
          MyDataReceiver.BroadcasterChangeInfo broadcasterFound = this.OnBroadcasterFound;
          if (broadcasterFound != null)
            broadcasterFound(broadcaster);
        }
      }
    }

    public bool CanBeUsedByPlayer(long playerId) => MyDataBroadcaster.CanBeUsedByPlayer(playerId, this.Entity);

    protected abstract void GetBroadcastersInMyRange(
      ref HashSet<MyDataBroadcaster> broadcastersInRange);

    public void UpdateHud(bool showMyself = false)
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated || MyHud.MinimalHud || MyHud.CutsceneHud)
        return;
      this.Clear();
      foreach (MyDataBroadcaster relayedBroadcaster in MyAntennaSystem.Static.GetAllRelayedBroadcasters(this, MySession.Static.LocalPlayerId, false))
      {
        bool allowBlink = relayedBroadcaster.CanBeUsedByPlayer(MySession.Static.LocalPlayerId);
        if (relayedBroadcaster.Entity.GetTopMostParent() is MyCubeGrid topMostParent)
        {
          MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(topMostParent);
          if (group != null)
            this.m_broadcastersInRange_TopGrids.Add(group.GroupData);
        }
        if (relayedBroadcaster.ShowOnHud)
        {
          foreach (MyHudEntityParams hudParam in relayedBroadcaster.GetHudParams(allowBlink))
          {
            if (!this.m_entitiesOnHud.Contains(hudParam.EntityId))
            {
              this.m_entitiesOnHud.Add(hudParam.EntityId);
              if ((double) hudParam.BlinkingTime > 0.0)
                MyHud.HackingMarkers.RegisterMarker(hudParam.EntityId, hudParam);
              else if (!MyHud.HackingMarkers.MarkerEntities.ContainsKey(hudParam.EntityId))
                MyHud.LocationMarkers.RegisterMarker(hudParam.EntityId, hudParam);
            }
          }
        }
      }
      if (!MySession.Static.AdminSettings.HasFlag((Enum) AdminSettingsEnum.ShowPlayers))
        return;
      foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) MySession.Static.Players.GetOnlinePlayers())
      {
        MyCharacter character = onlinePlayer.Character;
        if (character != null)
        {
          foreach (MyHudEntityParams hudParam in character.GetHudParams(false))
          {
            if (!this.m_entitiesOnHud.Contains(hudParam.EntityId))
            {
              this.m_entitiesOnHud.Add(hudParam.EntityId);
              MyHud.LocationMarkers.RegisterMarker(hudParam.EntityId, hudParam);
            }
          }
        }
      }
    }

    public bool HasAccessToLogicalGroup(MyGridLogicalGroupData group) => this.m_broadcastersInRange_TopGrids.Contains(group);

    public void Clear()
    {
      foreach (long entityId in this.m_entitiesOnHud)
        MyHud.LocationMarkers.UnregisterMarker(entityId);
      this.m_entitiesOnHud.Clear();
      this.m_broadcastersInRange_TopGrids.Clear();
    }

    public override string ComponentTypeDebugString => "MyDataReciever";

    public delegate void BroadcasterChangeInfo(MyDataBroadcaster broadcaster);
  }
}
