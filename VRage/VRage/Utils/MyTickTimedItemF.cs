// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyTickTimedItemF
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.Utils
{
  public struct MyTickTimedItemF
  {
    private float m_storage;
    private int m_ticksLeft;

    public float Get()
    {
      if (this.m_ticksLeft <= 0)
        return 0.0f;
      --this.m_ticksLeft;
      return this.m_storage;
    }

    public bool TryGet(out float outStoredItem)
    {
      if (this.m_ticksLeft > 0)
      {
        outStoredItem = this.m_storage;
        --this.m_ticksLeft;
        return true;
      }
      outStoredItem = 0.0f;
      return false;
    }

    public void Set(int itemTickTimeout, float item)
    {
      this.m_storage = item;
      this.m_ticksLeft = itemTickTimeout;
    }
  }
}
