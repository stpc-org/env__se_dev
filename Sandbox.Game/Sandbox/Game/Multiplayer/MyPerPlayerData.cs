// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Multiplayer.MyPerPlayerData
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.World;
using System.Collections.Generic;
using VRage.Utils;

namespace Sandbox.Game.Multiplayer
{
  public class MyPerPlayerData
  {
    private Dictionary<MyPlayer.PlayerId, Dictionary<MyStringId, object>> m_playerDataByPlayerId;

    public MyPerPlayerData() => this.m_playerDataByPlayerId = new Dictionary<MyPlayer.PlayerId, Dictionary<MyStringId, object>>((IEqualityComparer<MyPlayer.PlayerId>) MyPlayer.PlayerId.Comparer);

    public void SetPlayerData<T>(MyPlayer.PlayerId playerId, MyStringId dataId, T data) => this.GetOrAllocatePlayerDataDictionary(playerId)[dataId] = (object) data;

    public T GetPlayerData<T>(MyPlayer.PlayerId playerId, MyStringId dataId, T defaultValue)
    {
      Dictionary<MyStringId, object> dictionary = (Dictionary<MyStringId, object>) null;
      if (!this.m_playerDataByPlayerId.TryGetValue(playerId, out dictionary))
        return defaultValue;
      object obj = (object) null;
      return !dictionary.TryGetValue(dataId, out obj) ? defaultValue : (T) obj;
    }

    private Dictionary<MyStringId, object> GetOrAllocatePlayerDataDictionary(
      MyPlayer.PlayerId playerId)
    {
      Dictionary<MyStringId, object> dictionary = (Dictionary<MyStringId, object>) null;
      if (!this.m_playerDataByPlayerId.TryGetValue(playerId, out dictionary))
      {
        dictionary = new Dictionary<MyStringId, object>((IEqualityComparer<MyStringId>) MyStringId.Comparer);
        this.m_playerDataByPlayerId[playerId] = dictionary;
      }
      return dictionary;
    }
  }
}
