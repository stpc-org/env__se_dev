// Decompiled with JetBrains decompiler
// Type: VRage.Game.CollectionDictExtensions
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;
using System.Linq;

namespace VRage.Game
{
  internal static class CollectionDictExtensions
  {
    public static IEnumerable<TVal> GetOrEmpty<TKey, TValCol, TVal>(
      this Dictionary<TKey, TValCol> self,
      TKey key)
      where TValCol : IEnumerable<TVal>
    {
      TValCol valCol;
      return !self.TryGetValue(key, out valCol) ? Enumerable.Empty<TVal>() : (IEnumerable<TVal>) valCol;
    }

    public static IEnumerable<TVal> GetOrEmpty<TKey, TKey2, TVal>(
      this Dictionary<TKey, Dictionary<TKey2, TVal>> self,
      TKey key)
    {
      Dictionary<TKey2, TVal> dictionary;
      return !self.TryGetValue(key, out dictionary) ? Enumerable.Empty<TVal>() : (IEnumerable<TVal>) dictionary.Values;
    }
  }
}
