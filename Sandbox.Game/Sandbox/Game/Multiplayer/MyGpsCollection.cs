// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Multiplayer.MyGpsCollection
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRageMath;

namespace Sandbox.Game.Multiplayer
{
  [StaticEventOwner]
  public class MyGpsCollection : IMyGpsCollection
  {
    private const int GPS_POSITION_UPDATE = 100;
    private int m_gpsUpdateCounter = 100;
    private Dictionary<long, Dictionary<int, MyGps>> m_playerGpss = new Dictionary<long, Dictionary<int, MyGps>>();
    private List<MyGps> m_gpsToUpdate = new List<MyGps>();
    private List<long> m_gpsIdentityToUpdate = new List<long>();
    private StringBuilder m_NamingSearch = new StringBuilder();
    private long lastPlayerId;
    private static readonly int PARSE_MAX_COUNT = 20;
    private static readonly string m_ScanPattern = "GPS:([^:]{0,32}):([\\d\\.-]*):([\\d\\.-]*):([\\d\\.-]*):";
    private static readonly string m_ColorScanPattern = "GPS:([^:]{0,32}):([\\d\\.-]*):([\\d\\.-]*):([\\d\\.-]*):(#[A-Fa-f0-9]{6}(?:[A-Fa-f0-9]{2})?):";
    private static readonly string m_ScanPatternExtended = "GPS:([^:]{0,32}):([\\d\\.-]*):([\\d\\.-]*):([\\d\\.-]*):(.*)";
    private static readonly string m_ColorScanPatternExtended = "GPS:([^:]{0,32}):([\\d\\.-]*):([\\d\\.-]*):([\\d\\.-]*):(#[A-Fa-f0-9]{6}(?:[A-Fa-f0-9]{2})?):(.*)";
    private static List<IMyGps> reusableList = new List<IMyGps>();

    public event Action<long> ListChanged;

    public event Action<long, int> GpsChanged;

    public event Action<long, int> GpsAdded;

    public Dictionary<int, MyGps> this[long id] => this.m_playerGpss[id];

    public bool ExistsForPlayer(long id) => this.m_playerGpss.TryGetValue(id, out Dictionary<int, MyGps> _);

    public void SendAddGps(
      long identityId,
      ref MyGps gps,
      long entityId = 0,
      bool playSoundOnCreation = true)
    {
      if (identityId == 0L)
        return;
      MyMultiplayer.RaiseStaticEvent<MyGpsCollection.AddMsg>((Func<IMyEventOwner, Action<MyGpsCollection.AddMsg>>) (s => new Action<MyGpsCollection.AddMsg>(MyGpsCollection.OnAddGps)), new MyGpsCollection.AddMsg()
      {
        IdentityId = identityId,
        Name = gps.Name,
        DisplayName = gps.DisplayName,
        Description = gps.Description,
        Coords = gps.Coords,
        ShowOnHud = gps.ShowOnHud,
        IsFinal = !gps.DiscardAt.HasValue,
        AlwaysVisible = gps.AlwaysVisible,
        EntityId = entityId,
        ContractId = gps.ContractId,
        GPSColor = gps.GPSColor,
        IsContainerGPS = gps.IsContainerGPS,
        PlaySoundOnCreation = playSoundOnCreation,
        IsObjective = gps.IsObjective
      });
    }

    [Event(null, 128)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void OnAddGps(MyGpsCollection.AddMsg msg)
    {
      if (Sync.IsServer && (!MyEventContext.Current.IsLocallyInvoked && (long) MySession.Static.Players.TryGetSteamId(msg.IdentityId) != (long) MyEventContext.Current.Sender.Value))
      {
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
        MyEventContext.ValidationFailed();
      }
      else
      {
        MyGps gps = new MyGps();
        gps.Name = msg.Name;
        gps.DisplayName = msg.DisplayName;
        gps.Description = msg.Description;
        gps.Coords = msg.Coords;
        gps.ShowOnHud = msg.ShowOnHud;
        gps.AlwaysVisible = msg.AlwaysVisible;
        gps.DiscardAt = new TimeSpan?();
        gps.GPSColor = msg.GPSColor;
        gps.IsContainerGPS = msg.IsContainerGPS;
        gps.IsObjective = msg.IsObjective;
        gps.ContractId = msg.ContractId;
        if (!msg.IsFinal)
          gps.SetDiscardAt();
        if (msg.EntityId != 0L)
        {
          MyEntity entityById = MyEntities.GetEntityById(msg.EntityId);
          if (entityById != null)
            gps.SetEntity((IMyEntity) entityById);
          else
            gps.SetEntityId(msg.EntityId);
        }
        gps.UpdateHash();
        if (MySession.Static.Gpss.AddPlayerGps(msg.IdentityId, ref gps) && gps.ShowOnHud && msg.IdentityId == MySession.Static.LocalPlayerId)
        {
          MyHud.GpsMarkers.RegisterMarker(gps);
          if (msg.PlaySoundOnCreation)
            MyAudio.Static.PlaySound(MySoundPair.GetCueId("HudGPSNotification3"));
        }
        MySession.Static.Gpss.ListChanged.InvokeIfNotNull<long>(msg.IdentityId);
        MySession.Static.Gpss.GpsAdded.InvokeIfNotNull<long, int>(msg.IdentityId, gps.Hash);
      }
    }

    public void SendDelete(long identityId, int gpsHash) => MyMultiplayer.RaiseStaticEvent<long, int>((Func<IMyEventOwner, Action<long, int>>) (s => new Action<long, int>(MyGpsCollection.DeleteRequest)), identityId, gpsHash);

    [Event(null, 198)]
    [Reliable]
    [Server]
    private static void DeleteRequest(long identityId, int gpsHash)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && (long) MySession.Static.Players.TryGetSteamId(identityId) != (long) MyEventContext.Current.Sender.Value)
      {
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        Dictionary<int, MyGps> dictionary;
        if (!MySession.Static.Gpss.m_playerGpss.TryGetValue(identityId, out dictionary) || !dictionary.ContainsKey(gpsHash))
          return;
        MyMultiplayer.RaiseStaticEvent<long, int>((Func<IMyEventOwner, Action<long, int>>) (s => new Action<long, int>(MyGpsCollection.DeleteSuccess)), identityId, gpsHash);
      }
    }

    [Event(null, 214)]
    [Reliable]
    [ServerInvoked]
    [Broadcast]
    private static void DeleteSuccess(long identityId, int gpsHash)
    {
      Dictionary<int, MyGps> dictionary;
      MyGps ins;
      if (!MySession.Static.Gpss.m_playerGpss.TryGetValue(identityId, out dictionary) || !dictionary.TryGetValue(gpsHash, out ins))
        return;
      if (ins.ShowOnHud)
        MyHud.GpsMarkers.UnregisterMarker(ins);
      dictionary.Remove(gpsHash);
      ins.Close();
      Action<long> listChanged = MySession.Static.Gpss.ListChanged;
      if (listChanged == null)
        return;
      listChanged(identityId);
    }

    public void SendModifyGps(long identityId, MyGps gps) => MyMultiplayer.RaiseStaticEvent<MyGpsCollection.ModifyMsg>((Func<IMyEventOwner, Action<MyGpsCollection.ModifyMsg>>) (s => new Action<MyGpsCollection.ModifyMsg>(MyGpsCollection.ModifyRequest)), new MyGpsCollection.ModifyMsg()
    {
      IdentityId = identityId,
      Name = gps.Name,
      Description = gps.Description,
      Coords = gps.Coords,
      Hash = gps.Hash,
      GPSColor = gps.GPSColor,
      ContractId = gps.ContractId
    });

    [Event(null, 254)]
    [Reliable]
    [Server]
    private static void ModifyRequest(MyGpsCollection.ModifyMsg msg)
    {
      ulong steamId = MySession.Static.Players.TryGetSteamId(msg.IdentityId);
      if (!MyEventContext.Current.IsLocallyInvoked && (long) steamId != (long) MyEventContext.Current.Sender.Value)
      {
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        Dictionary<int, MyGps> dictionary;
        if (!MySession.Static.Gpss.m_playerGpss.TryGetValue(msg.IdentityId, out dictionary) || !dictionary.ContainsKey(msg.Hash))
          return;
        MyMultiplayer.RaiseStaticEvent<MyGpsCollection.ModifyMsg>((Func<IMyEventOwner, Action<MyGpsCollection.ModifyMsg>>) (s => new Action<MyGpsCollection.ModifyMsg>(MyGpsCollection.ModifySuccess)), msg, new EndpointId(steamId));
      }
    }

    [Event(null, 271)]
    [Reliable]
    [Server]
    [Client]
    private static void ModifySuccess(MyGpsCollection.ModifyMsg msg)
    {
      Dictionary<int, MyGps> dictionary;
      MyGps ins1;
      if (!MySession.Static.Gpss.m_playerGpss.TryGetValue(msg.IdentityId, out dictionary) || !dictionary.TryGetValue(msg.Hash, out ins1))
        return;
      ins1.Name = msg.Name;
      ins1.Description = msg.Description;
      ins1.Coords = msg.Coords;
      ins1.DiscardAt = new TimeSpan?();
      ins1.GPSColor = msg.GPSColor;
      ins1.ContractId = msg.ContractId;
      Action<long, int> gpsChanged = MySession.Static.Gpss.GpsChanged;
      if (gpsChanged != null)
        gpsChanged(msg.IdentityId, ins1.Hash);
      dictionary.Remove(ins1.Hash);
      MyHud.GpsMarkers.UnregisterMarker(ins1);
      ins1.UpdateHash();
      if (dictionary.ContainsKey(ins1.Hash))
      {
        MyGps ins2;
        dictionary.TryGetValue(ins1.Hash, out ins2);
        MyHud.GpsMarkers.UnregisterMarker(ins2);
        dictionary.Remove(ins1.Hash);
        dictionary.Add(ins1.Hash, ins1);
        Action<long> listChanged = MySession.Static.Gpss.ListChanged;
        if (listChanged != null)
          listChanged(msg.IdentityId);
      }
      else
        dictionary.Add(ins1.Hash, ins1);
      if (msg.IdentityId != MySession.Static.LocalPlayerId || !ins1.ShowOnHud)
        return;
      MyHud.GpsMarkers.RegisterMarker(ins1);
    }

    public void ChangeShowOnHud(long identityId, int gpsHash, bool show) => this.SendChangeShowOnHud(identityId, gpsHash, show);

    private void SendChangeShowOnHud(long identityId, int gpsHash, bool show) => MyMultiplayer.RaiseStaticEvent<long, int, bool>((Func<IMyEventOwner, Action<long, int, bool>>) (s => new Action<long, int, bool>(MyGpsCollection.ShowOnHudRequest)), identityId, gpsHash, show);

    [Event(null, 331)]
    [Reliable]
    [Server]
    private static void ShowOnHudRequest(long identityId, int gpsHash, bool show)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && (long) MySession.Static.Players.TryGetSteamId(identityId) != (long) MyEventContext.Current.Sender.Value)
      {
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        if (!MySession.Static.Gpss.m_playerGpss.TryGetValue(identityId, out Dictionary<int, MyGps> _))
          return;
        MyMultiplayer.RaiseStaticEvent<long, int, bool>((Func<IMyEventOwner, Action<long, int, bool>>) (s => new Action<long, int, bool>(MyGpsCollection.ShowOnHudSuccess)), identityId, gpsHash, show);
      }
    }

    [Event(null, 346)]
    [Reliable]
    [ServerInvoked]
    [Broadcast]
    private static void ShowOnHudSuccess(long identityId, int gpsHash, bool show)
    {
      Dictionary<int, MyGps> dictionary;
      MyGps ins;
      if (!MySession.Static.Gpss.m_playerGpss.TryGetValue(identityId, out dictionary) || !dictionary.TryGetValue(gpsHash, out ins))
        return;
      ins.ShowOnHud = show;
      if (!show)
        ins.AlwaysVisible = false;
      ins.DiscardAt = new TimeSpan?();
      Action<long, int> gpsChanged = MySession.Static.Gpss.GpsChanged;
      if (gpsChanged != null)
        gpsChanged(identityId, gpsHash);
      if (identityId != MySession.Static.LocalPlayerId)
        return;
      if (ins.ShowOnHud)
        MyHud.GpsMarkers.RegisterMarker(ins);
      else
        MyHud.GpsMarkers.UnregisterMarker(ins);
    }

    public void ChangeAlwaysVisible(long identityId, int gpsHash, bool alwaysVisible) => this.SendChangeAlwaysVisible(identityId, gpsHash, alwaysVisible);

    private void SendChangeAlwaysVisible(long identityId, int gpsHash, bool alwaysVisible) => MyMultiplayer.RaiseStaticEvent<long, int, bool>((Func<IMyEventOwner, Action<long, int, bool>>) (s => new Action<long, int, bool>(MyGpsCollection.AlwaysVisibleRequest)), identityId, gpsHash, alwaysVisible);

    [Event(null, 389)]
    [Reliable]
    [Server]
    private static void AlwaysVisibleRequest(long identityId, int gpsHash, bool alwaysVisible)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && (long) MySession.Static.Players.TryGetSteamId(identityId) != (long) MyEventContext.Current.Sender.Value)
      {
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        if (!MySession.Static.Gpss.m_playerGpss.TryGetValue(identityId, out Dictionary<int, MyGps> _))
          return;
        MyMultiplayer.RaiseStaticEvent<long, int, bool>((Func<IMyEventOwner, Action<long, int, bool>>) (s => new Action<long, int, bool>(MyGpsCollection.AlwaysVisibleSuccess)), identityId, gpsHash, alwaysVisible);
      }
    }

    [Event(null, 404)]
    [Reliable]
    [ServerInvoked]
    [Broadcast]
    private static void AlwaysVisibleSuccess(long identityId, int gpsHash, bool alwaysVisible)
    {
      Dictionary<int, MyGps> dictionary;
      MyGps ins;
      if (!MySession.Static.Gpss.m_playerGpss.TryGetValue(identityId, out dictionary) || !dictionary.TryGetValue(gpsHash, out ins))
        return;
      ins.AlwaysVisible = alwaysVisible;
      ins.ShowOnHud |= alwaysVisible;
      ins.DiscardAt = new TimeSpan?();
      Action<long, int> gpsChanged = MySession.Static.Gpss.GpsChanged;
      if (gpsChanged != null)
        gpsChanged(identityId, gpsHash);
      if (identityId != MySession.Static.LocalPlayerId)
        return;
      if (ins.ShowOnHud)
        MyHud.GpsMarkers.RegisterMarker(ins);
      else
        MyHud.GpsMarkers.UnregisterMarker(ins);
    }

    public void ChangeColor(long identityId, int gpsHash, Color color) => this.SendChangeColor(identityId, gpsHash, color);

    private void SendChangeColor(long identityId, int gpsHash, Color color) => MyMultiplayer.RaiseStaticEvent<long, int, Color>((Func<IMyEventOwner, Action<long, int, Color>>) (s => new Action<long, int, Color>(MyGpsCollection.ColorRequest)), identityId, gpsHash, color);

    [Event(null, 447)]
    [Reliable]
    [Server]
    private static void ColorRequest(long identityId, int gpsHash, Color color)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && (long) MySession.Static.Players.TryGetSteamId(identityId) != (long) MyEventContext.Current.Sender.Value)
      {
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        if (!MySession.Static.Gpss.m_playerGpss.TryGetValue(identityId, out Dictionary<int, MyGps> _))
          return;
        MyMultiplayer.RaiseStaticEvent<long, int, Color>((Func<IMyEventOwner, Action<long, int, Color>>) (s => new Action<long, int, Color>(MyGpsCollection.ColorSuccess)), identityId, gpsHash, color);
      }
    }

    [Event(null, 462)]
    [Reliable]
    [ServerInvoked]
    [Broadcast]
    private static void ColorSuccess(long identityId, int gpsHash, Color color)
    {
      Dictionary<int, MyGps> dictionary;
      MyGps myGps;
      if (!MySession.Static.Gpss.m_playerGpss.TryGetValue(identityId, out dictionary) || !dictionary.TryGetValue(gpsHash, out myGps))
        return;
      myGps.GPSColor = color;
      myGps.DiscardAt = new TimeSpan?();
      Action<long, int> gpsChanged = MySession.Static.Gpss.GpsChanged;
      if (gpsChanged == null)
        return;
      gpsChanged(identityId, gpsHash);
    }

    public bool AddPlayerGps(long identityId, ref MyGps gps)
    {
      if (gps == null)
        return false;
      Dictionary<int, MyGps> dictionary;
      if (!this.m_playerGpss.TryGetValue(identityId, out dictionary))
      {
        dictionary = new Dictionary<int, MyGps>();
        this.m_playerGpss.Add(identityId, dictionary);
      }
      if (dictionary.ContainsKey(gps.Hash))
      {
        MyGps myGps;
        dictionary.TryGetValue(gps.Hash, out myGps);
        if (myGps.DiscardAt.HasValue)
          myGps.SetDiscardAt();
        return false;
      }
      dictionary.Add(gps.Hash, gps);
      return true;
    }

    public void RemovePlayerGpss(long identityId)
    {
      if (!Sync.IsServer || !this.m_playerGpss.ContainsKey(identityId))
        return;
      this.m_playerGpss.Remove(identityId);
    }

    public MyGps GetGps(int hash)
    {
      foreach (Dictionary<int, MyGps> dictionary in MySession.Static.Gpss.m_playerGpss.Values)
      {
        MyGps myGps;
        if (dictionary.TryGetValue(hash, out myGps))
          return myGps;
      }
      return (MyGps) null;
    }

    public MyGps GetGpsByEntityId(long identityId, long entityId)
    {
      Dictionary<int, MyGps> dictionary;
      if (!this.m_playerGpss.TryGetValue(identityId, out dictionary))
        return (MyGps) null;
      foreach (MyGps myGps in dictionary.Values)
      {
        if (myGps.EntityId == entityId)
          return myGps;
      }
      return (MyGps) null;
    }

    public MyGps GetGpsByContractId(long identityId, long contractId)
    {
      Dictionary<int, MyGps> dictionary;
      if (!this.m_playerGpss.TryGetValue(identityId, out dictionary))
        return (MyGps) null;
      foreach (MyGps myGps in dictionary.Values)
      {
        if (myGps.ContractId == contractId)
          return myGps;
      }
      return (MyGps) null;
    }

    public void GetNameForNewCurrent(StringBuilder name)
    {
      int num1 = 0;
      name.Clear().Append(MySession.Static.LocalHumanPlayer.DisplayName).Append(" #");
      Dictionary<int, MyGps> dictionary;
      if (this.m_playerGpss.TryGetValue(MySession.Static.LocalPlayerId, out dictionary))
      {
        foreach (KeyValuePair<int, MyGps> keyValuePair in dictionary)
        {
          if (keyValuePair.Value.Name.StartsWith(name.ToString()))
          {
            this.m_NamingSearch.Clear().Append(keyValuePair.Value.Name, name.Length, keyValuePair.Value.Name.Length - name.Length);
            int num2;
            try
            {
              num2 = int.Parse(this.m_NamingSearch.ToString());
            }
            catch (SystemException ex)
            {
              continue;
            }
            if (num2 > num1)
              num1 = num2;
          }
        }
      }
      int num3 = num1 + 1;
      name.Append(num3);
    }

    public void LoadGpss(MyObjectBuilder_Checkpoint checkpoint)
    {
      if (!MyFakes.ENABLE_GPS || checkpoint.Gps == null)
        return;
      foreach (KeyValuePair<long, MyObjectBuilder_Gps> keyValuePair in checkpoint.Gps.Dictionary)
      {
        foreach (MyObjectBuilder_Gps.Entry entry in keyValuePair.Value.Entries)
        {
          MyGps ins = new MyGps(entry);
          Dictionary<int, MyGps> dictionary;
          if (!this.m_playerGpss.TryGetValue(keyValuePair.Key, out dictionary))
          {
            dictionary = new Dictionary<int, MyGps>();
            this.m_playerGpss.Add(keyValuePair.Key, dictionary);
          }
          if (!dictionary.ContainsKey(ins.GetHashCode()))
          {
            dictionary.Add(ins.GetHashCode(), ins);
            if (ins.ShowOnHud && keyValuePair.Key == MySession.Static.LocalPlayerId && MySession.Static.LocalPlayerId != 0L)
              MyHud.GpsMarkers.RegisterMarker(ins);
          }
        }
      }
    }

    public void updateForHud()
    {
      if (this.lastPlayerId != MySession.Static.LocalPlayerId)
      {
        Dictionary<int, MyGps> dictionary;
        if (this.m_playerGpss.TryGetValue(this.lastPlayerId, out dictionary))
        {
          foreach (KeyValuePair<int, MyGps> keyValuePair in dictionary)
            MyHud.GpsMarkers.UnregisterMarker(keyValuePair.Value);
        }
        this.lastPlayerId = MySession.Static.LocalPlayerId;
        if (this.m_playerGpss.TryGetValue(this.lastPlayerId, out dictionary))
        {
          foreach (KeyValuePair<int, MyGps> keyValuePair in dictionary)
          {
            if (keyValuePair.Value.ShowOnHud)
              MyHud.GpsMarkers.RegisterMarker(keyValuePair.Value);
          }
        }
      }
      this.DiscardOld();
    }

    public void SaveGpss(MyObjectBuilder_Checkpoint checkpoint)
    {
      if (!MyFakes.ENABLE_GPS)
        return;
      this.DiscardOld();
      if (checkpoint.Gps == null)
        checkpoint.Gps = new SerializableDictionary<long, MyObjectBuilder_Gps>();
      foreach (KeyValuePair<long, Dictionary<int, MyGps>> keyValuePair1 in this.m_playerGpss)
      {
        MyObjectBuilder_Gps newObject;
        if (!checkpoint.Gps.Dictionary.TryGetValue(keyValuePair1.Key, out newObject))
          newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Gps>();
        if (newObject.Entries == null)
          newObject.Entries = new List<MyObjectBuilder_Gps.Entry>();
        foreach (KeyValuePair<int, MyGps> keyValuePair2 in keyValuePair1.Value)
        {
          if (!keyValuePair2.Value.IsLocal && (!Sync.IsServer || keyValuePair2.Value.EntityId == 0L || MyEntities.GetEntityById(keyValuePair2.Value.EntityId) != null))
            newObject.Entries.Add(this.GetObjectBuilderEntry(keyValuePair2.Value));
        }
        checkpoint.Gps.Dictionary.Add(keyValuePair1.Key, newObject);
      }
    }

    public MyObjectBuilder_Gps.Entry GetObjectBuilderEntry(MyGps gps) => new MyObjectBuilder_Gps.Entry()
    {
      name = gps.Name,
      description = gps.Description,
      coords = gps.Coords,
      isFinal = !gps.DiscardAt.HasValue,
      showOnHud = gps.ShowOnHud,
      alwaysVisible = gps.AlwaysVisible,
      color = gps.GPSColor,
      entityId = gps.EntityId,
      contractId = gps.ContractId,
      DisplayName = gps.DisplayName,
      isObjective = gps.IsObjective
    };

    public void DiscardOld()
    {
      List<int> intList = new List<int>();
      foreach (KeyValuePair<long, Dictionary<int, MyGps>> keyValuePair1 in this.m_playerGpss)
      {
        foreach (KeyValuePair<int, MyGps> keyValuePair2 in keyValuePair1.Value)
        {
          if (keyValuePair2.Value.DiscardAt.HasValue && TimeSpan.Compare(MySession.Static.ElapsedPlayTime, keyValuePair2.Value.DiscardAt.Value) > 0)
            intList.Add(keyValuePair2.Value.Hash);
        }
        foreach (int key in intList)
        {
          MyGps ins = keyValuePair1.Value[key];
          if (ins.ShowOnHud)
            MyHud.GpsMarkers.UnregisterMarker(ins);
          keyValuePair1.Value.Remove(key);
        }
        intList.Clear();
      }
    }

    internal void RegisterChat(MyMultiplayerBase multiplayer)
    {
      if (Sync.IsDedicated || !MyFakes.ENABLE_GPS)
        return;
      multiplayer.ChatMessageReceived += new Action<ulong, string, ChatChannel, long, string>(this.ParseChat);
    }

    internal void UnregisterChat(MyMultiplayerBase multiplayer)
    {
      if (Sync.IsDedicated || !MyFakes.ENABLE_GPS)
        return;
      multiplayer.ChatMessageReceived -= new Action<ulong, string, ChatChannel, long, string>(this.ParseChat);
    }

    private void ParseChat(
      ulong steamUserId,
      string messageText,
      ChatChannel channel,
      long targetId,
      string customAuthorName = null)
    {
      StringBuilder desc = new StringBuilder();
      desc.Append(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_FromChatDescPrefix)).Append(MyMultiplayer.Static.GetMemberName(steamUserId));
      this.ScanText(messageText, desc);
    }

    public static bool ParseOneGPS(string input, StringBuilder name, ref Vector3D coords)
    {
      foreach (Match match in Regex.Matches(input, MyGpsCollection.m_ScanPattern))
      {
        double num1;
        double num2;
        double num3;
        try
        {
          num1 = Math.Round(double.Parse(match.Groups[2].Value, (IFormatProvider) CultureInfo.InvariantCulture), 2);
          num2 = Math.Round(double.Parse(match.Groups[3].Value, (IFormatProvider) CultureInfo.InvariantCulture), 2);
          num3 = Math.Round(double.Parse(match.Groups[4].Value, (IFormatProvider) CultureInfo.InvariantCulture), 2);
        }
        catch (SystemException ex)
        {
          continue;
        }
        name.Clear().Append(match.Groups[1].Value);
        coords.X = num1;
        coords.Y = num2;
        coords.Z = num3;
        return true;
      }
      return false;
    }

    public static bool ParseOneGPSExtended(
      string input,
      StringBuilder name,
      ref Vector3D coords,
      StringBuilder additionalData)
    {
      MatchCollection matchCollection = Regex.Matches(input, MyGpsCollection.m_ColorScanPatternExtended);
      if (matchCollection == null || matchCollection != null && matchCollection.Count == 0)
        matchCollection = Regex.Matches(input, MyGpsCollection.m_ScanPatternExtended);
      Color color = new Color(117, 201, 241);
      foreach (Match match in matchCollection)
      {
        double num1;
        double num2;
        double num3;
        try
        {
          num1 = Math.Round(double.Parse(match.Groups[2].Value, (IFormatProvider) CultureInfo.InvariantCulture), 2);
          num2 = Math.Round(double.Parse(match.Groups[3].Value, (IFormatProvider) CultureInfo.InvariantCulture), 2);
          num3 = Math.Round(double.Parse(match.Groups[4].Value, (IFormatProvider) CultureInfo.InvariantCulture), 2);
        }
        catch (SystemException ex)
        {
          continue;
        }
        name.Clear().Append(match.Groups[1].Value);
        coords.X = num1;
        coords.Y = num2;
        coords.Z = num3;
        additionalData.Clear();
        if (match.Groups.Count == 7 && !string.IsNullOrWhiteSpace(match.Groups[6].Value))
          additionalData.Append(match.Groups[6].Value);
        return true;
      }
      return false;
    }

    public int ScanText(string input, StringBuilder desc) => this.ScanText(input, desc.ToString());

    public int ScanText(string input, string desc = null)
    {
      int num = 0;
      bool flag = true;
      MatchCollection matchCollection = Regex.Matches(input, MyGpsCollection.m_ColorScanPattern);
      if (matchCollection == null || matchCollection != null && matchCollection.Count == 0)
      {
        matchCollection = Regex.Matches(input, MyGpsCollection.m_ScanPattern);
        flag = false;
      }
      Color color = new Color(117, 201, 241);
      foreach (Match match in matchCollection)
      {
        string str = match.Groups[1].Value;
        double x;
        double y;
        double z;
        try
        {
          x = Math.Round(double.Parse(match.Groups[2].Value, (IFormatProvider) CultureInfo.InvariantCulture), 2);
          y = Math.Round(double.Parse(match.Groups[3].Value, (IFormatProvider) CultureInfo.InvariantCulture), 2);
          z = Math.Round(double.Parse(match.Groups[4].Value, (IFormatProvider) CultureInfo.InvariantCulture), 2);
          if (flag)
            color = (Color) new ColorDefinitionRGBA(match.Groups[5].Value);
        }
        catch (SystemException ex)
        {
          continue;
        }
        MyGps gps = new MyGps()
        {
          Name = str,
          Description = desc,
          Coords = new Vector3D(x, y, z),
          GPSColor = color,
          ShowOnHud = false
        };
        gps.UpdateHash();
        MySession.Static.Gpss.SendAddGps(MySession.Static.LocalPlayerId, ref gps);
        ++num;
        if (num == MyGpsCollection.PARSE_MAX_COUNT)
          break;
      }
      return num;
    }

    public void Update()
    {
      if (!Sync.IsServer || this.m_gpsUpdateCounter-- != 0)
        return;
      foreach (KeyValuePair<long, Dictionary<int, MyGps>> keyValuePair1 in this.m_playerGpss)
      {
        foreach (KeyValuePair<int, MyGps> keyValuePair2 in keyValuePair1.Value)
        {
          if (!keyValuePair2.Value.IsLocal && (keyValuePair2.Value.Hash != keyValuePair2.Value.CalculateHash() || keyValuePair2.Value.EntityId != 0L))
          {
            this.m_gpsToUpdate.Add(keyValuePair2.Value);
            this.m_gpsIdentityToUpdate.Add(keyValuePair1.Key);
          }
        }
      }
      for (int index = 0; index < this.m_gpsToUpdate.Count; ++index)
      {
        MyGps gps = this.m_gpsToUpdate[index];
        long identityId = this.m_gpsIdentityToUpdate[index];
        if (MySession.Static.Players.IsPlayerOnline(identityId))
        {
          this.SendModifyGps(identityId, gps);
          gps.UpdateHash();
        }
      }
      this.m_gpsToUpdate.Clear();
      this.m_gpsIdentityToUpdate.Clear();
      this.m_gpsUpdateCounter = 100;
    }

    IMyGps IMyGpsCollection.Create(
      string name,
      string description,
      Vector3D coords,
      bool showOnHud,
      bool temporary)
    {
      MyGps myGps = new MyGps();
      myGps.Name = name;
      myGps.Description = description;
      myGps.Coords = coords;
      myGps.ShowOnHud = showOnHud;
      myGps.GPSColor = new Color(117, 201, 241);
      if (temporary)
        myGps.SetDiscardAt();
      else
        myGps.DiscardAt = new TimeSpan?();
      myGps.UpdateHash();
      return (IMyGps) myGps;
    }

    List<IMyGps> IMyGpsCollection.GetGpsList(long identityId)
    {
      MyGpsCollection.reusableList.Clear();
      this.GetGpsList(identityId, MyGpsCollection.reusableList);
      return MyGpsCollection.reusableList;
    }

    public void GetGpsList(long identityId, List<IMyGps> list)
    {
      Dictionary<int, MyGps> dictionary;
      if (!this.m_playerGpss.TryGetValue(identityId, out dictionary))
        return;
      foreach (MyGps myGps in dictionary.Values)
        list.Add((IMyGps) myGps);
    }

    public IMyGps GetGpsByName(long identityId, string gpsName)
    {
      Dictionary<int, MyGps> dictionary;
      if (!this.m_playerGpss.TryGetValue(identityId, out dictionary))
        return (IMyGps) null;
      foreach (MyGps myGps in dictionary.Values)
      {
        if (myGps.Name == gpsName)
          return (IMyGps) myGps;
      }
      return (IMyGps) null;
    }

    void IMyGpsCollection.AddGps(long identityId, IMyGps gps)
    {
      MyGps gps1 = (MyGps) gps;
      this.SendAddGps(identityId, ref gps1);
    }

    void IMyGpsCollection.RemoveGps(long identityId, IMyGps gps) => this.SendDelete(identityId, (gps as MyGps).Hash);

    void IMyGpsCollection.RemoveGps(long identityId, int gpsHash) => this.SendDelete(identityId, gpsHash);

    void IMyGpsCollection.ModifyGps(long identityId, IMyGps gps)
    {
      MyGps gps1 = (MyGps) gps;
      this.SendModifyGps(identityId, gps1);
    }

    void IMyGpsCollection.SetShowOnHud(long identityId, int gpsHash, bool show) => this.SendChangeShowOnHud(identityId, gpsHash, show);

    void IMyGpsCollection.SetShowOnHud(long identityId, IMyGps gps, bool show) => this.SendChangeShowOnHud(identityId, (gps as MyGps).Hash, show);

    void IMyGpsCollection.AddLocalGps(IMyGps gps)
    {
      MyGps gps1 = (MyGps) gps;
      gps1.IsLocal = true;
      if (!this.AddPlayerGps(MySession.Static.LocalPlayerId, ref gps1) || !gps.ShowOnHud)
        return;
      MyHud.GpsMarkers.RegisterMarker(gps1);
    }

    void IMyGpsCollection.RemoveLocalGps(IMyGps gps) => this.RemovePlayerGps(gps.Hash);

    void IMyGpsCollection.RemoveLocalGps(int gpsHash) => this.RemovePlayerGps(gpsHash);

    private void RemovePlayerGps(int gpsHash)
    {
      Dictionary<int, MyGps> dictionary;
      MyGps ins;
      if (!MySession.Static.Gpss.m_playerGpss.TryGetValue(MySession.Static.LocalPlayerId, out dictionary) || !dictionary.TryGetValue(gpsHash, out ins))
        return;
      if (ins.ShowOnHud)
        MyHud.GpsMarkers.UnregisterMarker(ins);
      dictionary.Remove(gpsHash);
      Action<long> listChanged = MySession.Static.Gpss.ListChanged;
      if (listChanged == null)
        return;
      listChanged(MySession.Static.LocalPlayerId);
    }

    [Serializable]
    private struct AddMsg
    {
      public long IdentityId;
      [Serialize(MyObjectFlags.DefaultZero)]
      public string Name;
      [Serialize(MyObjectFlags.DefaultZero)]
      public string DisplayName;
      [Serialize(MyObjectFlags.DefaultZero)]
      public string Description;
      public Vector3D Coords;
      public bool ShowOnHud;
      public bool IsFinal;
      public bool AlwaysVisible;
      public long EntityId;
      public long ContractId;
      public Color GPSColor;
      public bool IsContainerGPS;
      public bool PlaySoundOnCreation;
      public bool IsObjective;

      protected class Sandbox_Game_Multiplayer_MyGpsCollection\u003C\u003EAddMsg\u003C\u003EIdentityId\u003C\u003EAccessor : IMemberAccessor<MyGpsCollection.AddMsg, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGpsCollection.AddMsg owner, in long value) => owner.IdentityId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGpsCollection.AddMsg owner, out long value) => value = owner.IdentityId;
      }

      protected class Sandbox_Game_Multiplayer_MyGpsCollection\u003C\u003EAddMsg\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyGpsCollection.AddMsg, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGpsCollection.AddMsg owner, in string value) => owner.Name = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGpsCollection.AddMsg owner, out string value) => value = owner.Name;
      }

      protected class Sandbox_Game_Multiplayer_MyGpsCollection\u003C\u003EAddMsg\u003C\u003EDisplayName\u003C\u003EAccessor : IMemberAccessor<MyGpsCollection.AddMsg, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGpsCollection.AddMsg owner, in string value) => owner.DisplayName = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGpsCollection.AddMsg owner, out string value) => value = owner.DisplayName;
      }

      protected class Sandbox_Game_Multiplayer_MyGpsCollection\u003C\u003EAddMsg\u003C\u003EDescription\u003C\u003EAccessor : IMemberAccessor<MyGpsCollection.AddMsg, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGpsCollection.AddMsg owner, in string value) => owner.Description = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGpsCollection.AddMsg owner, out string value) => value = owner.Description;
      }

      protected class Sandbox_Game_Multiplayer_MyGpsCollection\u003C\u003EAddMsg\u003C\u003ECoords\u003C\u003EAccessor : IMemberAccessor<MyGpsCollection.AddMsg, Vector3D>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGpsCollection.AddMsg owner, in Vector3D value) => owner.Coords = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGpsCollection.AddMsg owner, out Vector3D value) => value = owner.Coords;
      }

      protected class Sandbox_Game_Multiplayer_MyGpsCollection\u003C\u003EAddMsg\u003C\u003EShowOnHud\u003C\u003EAccessor : IMemberAccessor<MyGpsCollection.AddMsg, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGpsCollection.AddMsg owner, in bool value) => owner.ShowOnHud = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGpsCollection.AddMsg owner, out bool value) => value = owner.ShowOnHud;
      }

      protected class Sandbox_Game_Multiplayer_MyGpsCollection\u003C\u003EAddMsg\u003C\u003EIsFinal\u003C\u003EAccessor : IMemberAccessor<MyGpsCollection.AddMsg, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGpsCollection.AddMsg owner, in bool value) => owner.IsFinal = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGpsCollection.AddMsg owner, out bool value) => value = owner.IsFinal;
      }

      protected class Sandbox_Game_Multiplayer_MyGpsCollection\u003C\u003EAddMsg\u003C\u003EAlwaysVisible\u003C\u003EAccessor : IMemberAccessor<MyGpsCollection.AddMsg, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGpsCollection.AddMsg owner, in bool value) => owner.AlwaysVisible = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGpsCollection.AddMsg owner, out bool value) => value = owner.AlwaysVisible;
      }

      protected class Sandbox_Game_Multiplayer_MyGpsCollection\u003C\u003EAddMsg\u003C\u003EEntityId\u003C\u003EAccessor : IMemberAccessor<MyGpsCollection.AddMsg, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGpsCollection.AddMsg owner, in long value) => owner.EntityId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGpsCollection.AddMsg owner, out long value) => value = owner.EntityId;
      }

      protected class Sandbox_Game_Multiplayer_MyGpsCollection\u003C\u003EAddMsg\u003C\u003EContractId\u003C\u003EAccessor : IMemberAccessor<MyGpsCollection.AddMsg, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGpsCollection.AddMsg owner, in long value) => owner.ContractId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGpsCollection.AddMsg owner, out long value) => value = owner.ContractId;
      }

      protected class Sandbox_Game_Multiplayer_MyGpsCollection\u003C\u003EAddMsg\u003C\u003EGPSColor\u003C\u003EAccessor : IMemberAccessor<MyGpsCollection.AddMsg, Color>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGpsCollection.AddMsg owner, in Color value) => owner.GPSColor = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGpsCollection.AddMsg owner, out Color value) => value = owner.GPSColor;
      }

      protected class Sandbox_Game_Multiplayer_MyGpsCollection\u003C\u003EAddMsg\u003C\u003EIsContainerGPS\u003C\u003EAccessor : IMemberAccessor<MyGpsCollection.AddMsg, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGpsCollection.AddMsg owner, in bool value) => owner.IsContainerGPS = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGpsCollection.AddMsg owner, out bool value) => value = owner.IsContainerGPS;
      }

      protected class Sandbox_Game_Multiplayer_MyGpsCollection\u003C\u003EAddMsg\u003C\u003EPlaySoundOnCreation\u003C\u003EAccessor : IMemberAccessor<MyGpsCollection.AddMsg, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGpsCollection.AddMsg owner, in bool value) => owner.PlaySoundOnCreation = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGpsCollection.AddMsg owner, out bool value) => value = owner.PlaySoundOnCreation;
      }

      protected class Sandbox_Game_Multiplayer_MyGpsCollection\u003C\u003EAddMsg\u003C\u003EIsObjective\u003C\u003EAccessor : IMemberAccessor<MyGpsCollection.AddMsg, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGpsCollection.AddMsg owner, in bool value) => owner.IsObjective = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGpsCollection.AddMsg owner, out bool value) => value = owner.IsObjective;
      }
    }

    [Serializable]
    private struct ModifyMsg
    {
      public long IdentityId;
      public int Hash;
      [Serialize(MyObjectFlags.DefaultZero)]
      public string Name;
      [Serialize(MyObjectFlags.DefaultZero)]
      public string Description;
      public Vector3D Coords;
      public Color GPSColor;
      public long ContractId;

      protected class Sandbox_Game_Multiplayer_MyGpsCollection\u003C\u003EModifyMsg\u003C\u003EIdentityId\u003C\u003EAccessor : IMemberAccessor<MyGpsCollection.ModifyMsg, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGpsCollection.ModifyMsg owner, in long value) => owner.IdentityId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGpsCollection.ModifyMsg owner, out long value) => value = owner.IdentityId;
      }

      protected class Sandbox_Game_Multiplayer_MyGpsCollection\u003C\u003EModifyMsg\u003C\u003EHash\u003C\u003EAccessor : IMemberAccessor<MyGpsCollection.ModifyMsg, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGpsCollection.ModifyMsg owner, in int value) => owner.Hash = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGpsCollection.ModifyMsg owner, out int value) => value = owner.Hash;
      }

      protected class Sandbox_Game_Multiplayer_MyGpsCollection\u003C\u003EModifyMsg\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyGpsCollection.ModifyMsg, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGpsCollection.ModifyMsg owner, in string value) => owner.Name = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGpsCollection.ModifyMsg owner, out string value) => value = owner.Name;
      }

      protected class Sandbox_Game_Multiplayer_MyGpsCollection\u003C\u003EModifyMsg\u003C\u003EDescription\u003C\u003EAccessor : IMemberAccessor<MyGpsCollection.ModifyMsg, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGpsCollection.ModifyMsg owner, in string value) => owner.Description = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGpsCollection.ModifyMsg owner, out string value) => value = owner.Description;
      }

      protected class Sandbox_Game_Multiplayer_MyGpsCollection\u003C\u003EModifyMsg\u003C\u003ECoords\u003C\u003EAccessor : IMemberAccessor<MyGpsCollection.ModifyMsg, Vector3D>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGpsCollection.ModifyMsg owner, in Vector3D value) => owner.Coords = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGpsCollection.ModifyMsg owner, out Vector3D value) => value = owner.Coords;
      }

      protected class Sandbox_Game_Multiplayer_MyGpsCollection\u003C\u003EModifyMsg\u003C\u003EGPSColor\u003C\u003EAccessor : IMemberAccessor<MyGpsCollection.ModifyMsg, Color>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGpsCollection.ModifyMsg owner, in Color value) => owner.GPSColor = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGpsCollection.ModifyMsg owner, out Color value) => value = owner.GPSColor;
      }

      protected class Sandbox_Game_Multiplayer_MyGpsCollection\u003C\u003EModifyMsg\u003C\u003EContractId\u003C\u003EAccessor : IMemberAccessor<MyGpsCollection.ModifyMsg, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyGpsCollection.ModifyMsg owner, in long value) => owner.ContractId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyGpsCollection.ModifyMsg owner, out long value) => value = owner.ContractId;
      }
    }

    protected sealed class OnAddGps\u003C\u003ESandbox_Game_Multiplayer_MyGpsCollection\u003C\u003EAddMsg : ICallSite<IMyEventOwner, MyGpsCollection.AddMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyGpsCollection.AddMsg msg,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGpsCollection.OnAddGps(msg);
      }
    }

    protected sealed class DeleteRequest\u003C\u003ESystem_Int64\u0023System_Int32 : ICallSite<IMyEventOwner, long, int, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long identityId,
        in int gpsHash,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGpsCollection.DeleteRequest(identityId, gpsHash);
      }
    }

    protected sealed class DeleteSuccess\u003C\u003ESystem_Int64\u0023System_Int32 : ICallSite<IMyEventOwner, long, int, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long identityId,
        in int gpsHash,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGpsCollection.DeleteSuccess(identityId, gpsHash);
      }
    }

    protected sealed class ModifyRequest\u003C\u003ESandbox_Game_Multiplayer_MyGpsCollection\u003C\u003EModifyMsg : ICallSite<IMyEventOwner, MyGpsCollection.ModifyMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyGpsCollection.ModifyMsg msg,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGpsCollection.ModifyRequest(msg);
      }
    }

    protected sealed class ModifySuccess\u003C\u003ESandbox_Game_Multiplayer_MyGpsCollection\u003C\u003EModifyMsg : ICallSite<IMyEventOwner, MyGpsCollection.ModifyMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyGpsCollection.ModifyMsg msg,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGpsCollection.ModifySuccess(msg);
      }
    }

    protected sealed class ShowOnHudRequest\u003C\u003ESystem_Int64\u0023System_Int32\u0023System_Boolean : ICallSite<IMyEventOwner, long, int, bool, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long identityId,
        in int gpsHash,
        in bool show,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGpsCollection.ShowOnHudRequest(identityId, gpsHash, show);
      }
    }

    protected sealed class ShowOnHudSuccess\u003C\u003ESystem_Int64\u0023System_Int32\u0023System_Boolean : ICallSite<IMyEventOwner, long, int, bool, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long identityId,
        in int gpsHash,
        in bool show,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGpsCollection.ShowOnHudSuccess(identityId, gpsHash, show);
      }
    }

    protected sealed class AlwaysVisibleRequest\u003C\u003ESystem_Int64\u0023System_Int32\u0023System_Boolean : ICallSite<IMyEventOwner, long, int, bool, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long identityId,
        in int gpsHash,
        in bool alwaysVisible,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGpsCollection.AlwaysVisibleRequest(identityId, gpsHash, alwaysVisible);
      }
    }

    protected sealed class AlwaysVisibleSuccess\u003C\u003ESystem_Int64\u0023System_Int32\u0023System_Boolean : ICallSite<IMyEventOwner, long, int, bool, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long identityId,
        in int gpsHash,
        in bool alwaysVisible,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGpsCollection.AlwaysVisibleSuccess(identityId, gpsHash, alwaysVisible);
      }
    }

    protected sealed class ColorRequest\u003C\u003ESystem_Int64\u0023System_Int32\u0023VRageMath_Color : ICallSite<IMyEventOwner, long, int, Color, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long identityId,
        in int gpsHash,
        in Color color,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGpsCollection.ColorRequest(identityId, gpsHash, color);
      }
    }

    protected sealed class ColorSuccess\u003C\u003ESystem_Int64\u0023System_Int32\u0023VRageMath_Color : ICallSite<IMyEventOwner, long, int, Color, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long identityId,
        in int gpsHash,
        in Color color,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGpsCollection.ColorSuccess(identityId, gpsHash, color);
      }
    }
  }
}
