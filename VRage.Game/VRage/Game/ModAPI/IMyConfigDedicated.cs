// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.IMyConfigDedicated
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;

namespace VRage.Game.ModAPI
{
  public interface IMyConfigDedicated
  {
    List<string> Administrators { get; set; }

    int AsteroidAmount { get; set; }

    List<ulong> Banned { get; set; }

    List<ulong> Reserved { get; set; }

    string GetFilePath();

    ulong GroupID { get; set; }

    bool IgnoreLastSession { get; set; }

    string IP { get; set; }

    void Load(string path = null);

    string LoadWorld { get; set; }

    string WorldPlatform { get; set; }

    bool VerboseNetworkLogging { get; set; }

    bool PauseGameWhenEmpty { get; set; }

    string MessageOfTheDay { get; set; }

    string MessageOfTheDayUrl { get; set; }

    bool AutoRestartEnabled { get; set; }

    int AutoRestatTimeInMin { get; set; }

    bool AutoUpdateEnabled { get; set; }

    int AutoUpdateCheckIntervalInMin { get; set; }

    int AutoUpdateRestartDelayInMin { get; set; }

    bool AutoRestartSave { get; set; }

    string AutoUpdateSteamBranch { get; set; }

    string AutoUpdateBranchPassword { get; set; }

    void Save(string path = null);

    string ServerName { get; set; }

    int ServerPort { get; set; }

    MyObjectBuilder_SessionSettings SessionSettings { get; set; }

    int SteamPort { get; set; }

    string WorldName { get; set; }

    string PremadeCheckpointPath { get; set; }

    string ServerDescription { get; set; }

    string ServerPasswordHash { get; set; }

    string ServerPasswordSalt { get; set; }

    void SetPassword(string password);

    bool RemoteApiEnabled { get; set; }

    string RemoteSecurityKey { get; set; }

    void GenerateRemoteSecurityKey();

    int RemoteApiPort { get; set; }

    List<string> Plugins { get; set; }

    float WatcherInterval { get; set; }

    float WatcherSimulationSpeedMinimum { get; set; }

    int ManualActionDelay { get; set; }

    string ManualActionChatMessage { get; set; }

    bool AutodetectDependencies { get; set; }

    bool SaveChatToLog { get; set; }

    string NetworkType { get; set; }

    List<string> NetworkParameters { get; set; }

    bool ConsoleCompatibility { get; set; }
  }
}
