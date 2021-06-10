// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.MyGameServerItem
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Collections.Generic;

namespace VRage.GameServices
{
  public class MyGameServerItem : IMyMultiplayerGame
  {
    public object LobbyHandle;

    public uint AppID { get; set; }

    public int BotPlayers { get; set; }

    public bool DoNotRefresh { get; set; }

    public bool Experimental { get; set; }

    public string GameDescription { get; set; }

    public string GameDir { get; set; }

    public List<string> GameTagList { get; set; }

    public string GameTags { get; set; }

    public bool HadSuccessfulResponse { get; set; }

    public string Map { get; set; }

    public int MaxPlayers { get; set; }

    public string Name { get; set; }

    public string ConnectionString { get; set; }

    public bool Password { get; set; }

    public int Ping { get; set; }

    public int Players { get; set; }

    public bool Secure { get; set; }

    public int ServerVersion { get; set; }

    public ulong GameID => this.SteamID;

    public ulong SteamID { get; set; }

    public uint TimeLastPlayed { get; set; }

    public bool IsRanked { get; set; }

    public MyGameServerItem() => this.GameTagList = new List<string>();

    public string GetGameTagByPrefix(string prefix)
    {
      foreach (string gameTag in this.GameTagList)
      {
        if (gameTag.StartsWith(prefix))
          return gameTag.Substring(prefix.Length, gameTag.Length - prefix.Length);
      }
      return string.Empty;
    }

    public ulong GetGameTagByPrefixUlong(string prefix)
    {
      string gameTagByPrefix = this.GetGameTagByPrefix(prefix);
      if (string.IsNullOrEmpty(gameTagByPrefix))
        return 0;
      ulong result;
      ulong.TryParse(gameTagByPrefix, out result);
      return result;
    }
  }
}
