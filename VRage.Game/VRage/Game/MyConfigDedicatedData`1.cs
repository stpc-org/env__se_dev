﻿// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyConfigDedicatedData`1
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;
using System.Xml.Serialization;

namespace VRage.Game
{
  [XmlRoot("MyConfigDedicated")]
  public class MyConfigDedicatedData<T> where T : MyObjectBuilder_SessionSettings, new()
  {
    public T SessionSettings = new T();
    public string LoadWorld;
    public string WorldPlatform;
    public string IP = "0.0.0.0";
    public int SteamPort = 8766;
    public int ServerPort = 27016;
    public int AsteroidAmount = 4;
    [XmlArrayItem("unsignedLong")]
    public List<string> Administrators = new List<string>();
    public List<ulong> Banned = new List<ulong>();
    public ulong GroupID;
    public string ServerName = "";
    public string WorldName = "";
    public bool VerboseNetworkLogging;
    public bool PauseGameWhenEmpty;
    public string MessageOfTheDay = string.Empty;
    public string MessageOfTheDayUrl = string.Empty;
    public bool AutoRestartEnabled = true;
    public int AutoRestatTimeInMin;
    public bool AutoRestartSave = true;
    public bool AutoUpdateEnabled;
    public int AutoUpdateCheckIntervalInMin = 10;
    public int AutoUpdateRestartDelayInMin = 15;
    public string AutoUpdateSteamBranch;
    public string AutoUpdateBranchPassword;
    public bool IgnoreLastSession;
    public string PremadeCheckpointPath = "";
    public string ServerDescription;
    public string ServerPasswordHash;
    public string ServerPasswordSalt;
    public List<ulong> Reserved = new List<ulong>();
    public bool RemoteApiEnabled = true;
    public string RemoteSecurityKey;
    public int RemoteApiPort = 8080;
    public List<string> Plugins = new List<string>();
    public float WatcherInterval = 30f;
    public float WatcherSimulationSpeedMinimum = 0.05f;
    public int ManualActionDelay = 5;
    public string ManualActionChatMessage = "Server will be shut down in {0} min(s).";
    public bool AutodetectDependencies = true;
    public bool SaveChatToLog;
    public string NetworkType = "steam";
    public bool ConsoleCompatibility;
    [XmlArrayItem("Parameter")]
    public List<string> NetworkParameters = new List<string>();
  }
}
