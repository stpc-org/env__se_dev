// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyTimedItem`1
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.Utils
{
  public struct MyTimedItem<T>
  {
    private T m_storage;
    private int m_setTime;
    private int m_timeout;

    public T Get(int currentTime, bool autoRefreshTimeout)
    {
      if (currentTime >= this.m_setTime + this.m_timeout)
        return default (T);
      if (autoRefreshTimeout)
        this.m_setTime = currentTime + this.m_timeout;
      return this.m_storage;
    }

    public bool TryGet(int currentTime, bool autoRefreshTimeout, out T outStoredItem)
    {
      if (currentTime < this.m_setTime + this.m_timeout)
      {
        if (autoRefreshTimeout)
          this.m_setTime = currentTime + this.m_timeout;
        outStoredItem = this.m_storage;
        return true;
      }
      outStoredItem = default (T);
      return false;
    }

    public void Set(int currentTime, int itemTimeout, T item)
    {
      this.m_setTime = currentTime;
      this.m_timeout = itemTimeout;
      this.m_storage = item;
    }
  }
}
