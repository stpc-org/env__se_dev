// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MySessionComponentDLC
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders.Components;
using VRage.Network;

namespace Sandbox.Game.SessionComponents
{
  [StaticEventOwner]
  [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate, 2000, typeof (MyObjectBuilder_MySessionComponentDLC), null, false)]
  public class MySessionComponentDLC : MySessionComponentBase
  {
    private HashSet<uint> m_availableDLCs;
    private Dictionary<ulong, HashSet<uint>> m_clientAvailableDLCs;
    private Dictionary<MyDLCs.MyDLC, int> m_usedUnownedDLCs;

    public DictionaryReader<MyDLCs.MyDLC, int> UsedUnownedDLCs => (DictionaryReader<MyDLCs.MyDLC, int>) this.m_usedUnownedDLCs;

    public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
    {
      base.Init(sessionComponent);
      this.m_availableDLCs = new HashSet<uint>();
      this.m_usedUnownedDLCs = new Dictionary<MyDLCs.MyDLC, int>();
      if (!Sync.IsDedicated)
      {
        this.UpdateLocalPlayerDLC();
        MyGameService.OnDLCInstalled += new Action<uint>(this.OnDLCInstalled);
      }
      if (MyMultiplayer.Static == null || !Sync.IsServer)
        return;
      this.m_clientAvailableDLCs = new Dictionary<ulong, HashSet<uint>>();
      MyMultiplayer.Static.ClientJoined += new Action<ulong, string>(this.UpdateClientDLC);
    }

    public List<ulong> GetAvailableClientDLCsIds()
    {
      if (this.m_availableDLCs == null)
        return new List<ulong>();
      List<ulong> ulongList = new List<ulong>();
      foreach (uint availableDlC in this.m_availableDLCs)
        ulongList.Add((ulong) availableDlC);
      return ulongList;
    }

    protected override void UnloadData()
    {
      if (!Sync.IsDedicated)
        MyGameService.OnDLCInstalled -= new Action<uint>(this.OnDLCInstalled);
      if (MyMultiplayer.Static != null && Sync.IsServer)
        MyMultiplayer.Static.ClientJoined -= new Action<ulong, string>(this.UpdateClientDLC);
      base.UnloadData();
    }

    private void UpdateLocalPlayerDLC()
    {
      int dlcCount = MyGameService.GetDLCCount();
      for (int index = 0; index < dlcCount; ++index)
      {
        uint dlcId;
        MyGameService.GetDLCDataByIndex(index, out dlcId, out bool _, out string _, 128);
        if (MyGameService.IsDlcInstalled(dlcId))
        {
          this.m_availableDLCs.Add(dlcId);
          if ((int) dlcId == (int) MyFakes.SWITCH_DLC_FROM)
            this.m_availableDLCs.Add(MyFakes.SWITCH_DLC_TO);
        }
      }
    }

    [Event(null, 106)]
    [Reliable]
    [Server]
    public static void RequestUpdateClientDLC() => MySession.Static.GetComponent<MySessionComponentDLC>().UpdateClientDLC(MyEventContext.Current.Sender.Value);

    private void UpdateClientDLC(ulong steamId, string userName) => this.UpdateClientDLC(steamId);

    private void UpdateClientDLC(ulong steamId)
    {
      HashSet<uint> uintSet;
      if (!this.m_clientAvailableDLCs.TryGetValue(steamId, out uintSet))
      {
        uintSet = new HashSet<uint>();
        this.m_clientAvailableDLCs.Add(steamId, uintSet);
      }
      foreach (uint key in MyDLCs.DLCs.Keys)
      {
        if (!Sync.IsDedicated || MyGameService.GameServer.UserHasLicenseForApp(steamId, key))
        {
          uintSet.Add(key);
          if ((int) key == (int) MyFakes.SWITCH_DLC_FROM)
            uintSet.Add(MyFakes.SWITCH_DLC_TO);
        }
      }
    }

    public bool HasDLC(string DLCName, ulong steamId)
    {
      if (MyFakes.OWN_ALL_DLCS)
        return true;
      if (steamId == 0UL)
        return false;
      MyDLCs.MyDLC dlc1;
      MyDLCs.MyDLC dlc2;
      return (long) steamId == (long) Sync.MyId ? !Sync.IsDedicated && MyDLCs.TryGetDLC(DLCName, out dlc1) && this.m_availableDLCs.Contains(dlc1.AppId) : Sync.IsServer && MyDLCs.TryGetDLC(DLCName, out dlc2) && this.HasClientDLC(dlc2.AppId, steamId);
    }

    private bool HasClientDLC(uint DLCId, ulong steamId)
    {
      HashSet<uint> clientAvailableDlC;
      if (!this.m_clientAvailableDLCs.TryGetValue(steamId, out clientAvailableDlC))
      {
        this.UpdateClientDLC(steamId);
        clientAvailableDlC = this.m_clientAvailableDLCs[steamId];
      }
      return clientAvailableDlC.Contains(DLCId);
    }

    public bool HasDefinitionDLC(MyDefinitionId definitionId, ulong steamId) => this.HasDefinitionDLC(MyDefinitionManager.Static.GetDefinition(definitionId), steamId);

    public bool HasDefinitionDLC(MyDefinitionBase definition, ulong steamId)
    {
      if (definition.DLCs == null)
        return true;
      foreach (string dlC in definition.DLCs)
      {
        if (!this.HasDLC(dlC, steamId))
          return false;
      }
      return true;
    }

    public bool ContainsRequiredDLC(MyDefinitionBase definition, List<ulong> dlcs)
    {
      if (definition.DLCs == null || definition != null && ((IEnumerable<string>) definition.DLCs).Count<string>() == 0)
        return true;
      if (dlcs == null)
        return false;
      foreach (string dlC in definition.DLCs)
      {
        MyDLCs.MyDLC dlc;
        if (MyDLCs.TryGetDLC(dlC, out dlc) && !dlcs.Contains((ulong) dlc.AppId))
          return false;
      }
      return true;
    }

    public MyDLCs.MyDLC GetFirstMissingDefinitionDLC(
      MyDefinitionBase definition,
      ulong steamId)
    {
      if (definition.DLCs == null)
        return (MyDLCs.MyDLC) null;
      foreach (string dlC in definition.DLCs)
      {
        if (!this.HasDLC(dlC, steamId))
        {
          MyDLCs.MyDLC dlc;
          MyDLCs.TryGetDLC(dlC, out dlc);
          return dlc;
        }
      }
      return (MyDLCs.MyDLC) null;
    }

    private void OnDLCInstalled(uint dlcId)
    {
      this.m_availableDLCs.Add(dlcId);
      if (Sync.IsServer)
        return;
      MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (x => new Action(MySessionComponentDLC.RequestUpdateClientDLC)));
    }

    public void PushUsedUnownedDLC(MyDLCs.MyDLC dlc)
    {
      if (this.m_usedUnownedDLCs.ContainsKey(dlc))
        this.m_usedUnownedDLCs[dlc]++;
      else
        this.m_usedUnownedDLCs[dlc] = 1;
    }

    public void PopUsedUnownedDLC(MyDLCs.MyDLC dlc)
    {
      int num;
      if (!this.m_usedUnownedDLCs.TryGetValue(dlc, out num) || num <= 0)
        return;
      if (num > 1)
        this.m_usedUnownedDLCs[dlc]--;
      else
        this.m_usedUnownedDLCs.Remove(dlc);
    }

    protected sealed class RequestUpdateClientDLC\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
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
        MySessionComponentDLC.RequestUpdateClientDLC();
      }
    }
  }
}
