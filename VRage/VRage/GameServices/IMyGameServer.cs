// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.IMyGameServer
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Net;
using VRage.Network;

namespace VRage.GameServices
{
  public interface IMyGameServer
  {
    event Action PlatformConnected;

    event Action<string> PlatformDisconnected;

    event Action<string> PlatformConnectionFailed;

    event Action<ulong, JoinResult, ulong, string> ValidateAuthTicketResponse;

    event Action<ulong, ulong, bool, bool> UserGroupStatusResponse;

    event Action<sbyte> PolicyResponse;

    string GameDescription { get; set; }

    ulong ServerId { get; }

    bool Running { get; }

    bool Start(IPEndPoint serverEndpoint, ushort steamPort, string versionString);

    uint GetPublicIP();

    void SetKeyValue(string key, string value);

    void ClearAllKeyValues();

    void SetGameTags(string tags);

    void SetGameData(string data);

    void SetModDir(string directory);

    void SetDedicated(bool isDedicated);

    void SetMapName(string mapName);

    void SetServerName(string serverName);

    void SetMaxPlayerCount(int count);

    void SetBotPlayerCount(int count);

    void SetPasswordProtected(bool passwdProtected);

    void LogOnAnonymous();

    void LogOff();

    void Shutdown();

    bool BeginAuthSession(ulong userId, byte[] token, string serviceName);

    void EndAuthSession(ulong userId);

    void SendUserDisconnect(ulong userId);

    void EnableHeartbeats(bool enable);

    void BrowserUpdateUserData(ulong userId, string playerName, int score);

    bool RequestGroupStatus(ulong userId, ulong groupId);

    bool UserHasLicenseForApp(ulong steamId, uint appId);

    bool WaitStart(int timeOut);

    void SetServerModTemporaryDirectory();

    void SetGameReady(bool state);
  }
}
