// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyTickTimedItem`1
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.Utils
{
  public struct MyTickTimedItem<T>
  {
    private T m_storage;
    private int m_ticksLeft;

    public T Get()
    {
      if (this.m_ticksLeft <= 0)
        return default (T);
      --this.m_ticksLeft;
      return this.m_storage;
    }

    public bool TryGet(out T outStoredItem)
    {
      if (this.m_ticksLeft > 0)
      {
        --this.m_ticksLeft;
        outStoredItem = this.m_storage;
        return true;
      }
      outStoredItem = default (T);
      return false;
    }

    public void Set(int itemTickTimeout, T item)
    {
      this.m_storage = item;
      this.m_ticksLeft = itemTickTimeout;
    }
  }
}
