// Decompiled with JetBrains decompiler
// Type: ParallelTasks.HashtableNode`2
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

namespace ParallelTasks
{
  public struct HashtableNode<TKey, TData>
  {
    public TKey Key;
    public TData Data;
    public HashtableToken Token;

    public HashtableNode(TKey key, TData data, HashtableToken token)
    {
      this.Key = key;
      this.Data = data;
      this.Token = token;
    }
  }
}
