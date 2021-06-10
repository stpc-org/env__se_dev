// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyEntityIdRemapHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage;
using VRage.Library.Utils;
using VRage.ModAPI;

namespace Sandbox.Game.Entities
{
  internal class MyEntityIdRemapHelper : IMyRemapHelper
  {
    private static int DEFAULT_REMAPPER_SIZE = 512;
    private Dictionary<long, long> m_oldToNewMap = new Dictionary<long, long>(MyEntityIdRemapHelper.DEFAULT_REMAPPER_SIZE);
    private Dictionary<string, Dictionary<int, int>> m_groupMap = new Dictionary<string, Dictionary<int, int>>();

    public long RemapEntityId(long oldEntityId)
    {
      long num;
      if (!this.m_oldToNewMap.TryGetValue(oldEntityId, out num))
      {
        num = MyEntityIdentifier.AllocateId();
        this.m_oldToNewMap.Add(oldEntityId, num);
      }
      return num;
    }

    public string RemapEntityName(long newEntityId) => newEntityId.ToString();

    public int RemapGroupId(string group, int oldValue)
    {
      Dictionary<int, int> dictionary;
      if (!this.m_groupMap.TryGetValue(group, out dictionary))
      {
        dictionary = new Dictionary<int, int>();
        this.m_groupMap.Add(group, dictionary);
      }
      int num;
      if (!dictionary.TryGetValue(oldValue, out num))
      {
        num = MyRandom.Instance.Next();
        dictionary.Add(oldValue, num);
      }
      return num;
    }

    public void Clear()
    {
      this.m_oldToNewMap.Clear();
      this.m_groupMap.Clear();
    }
  }
}
