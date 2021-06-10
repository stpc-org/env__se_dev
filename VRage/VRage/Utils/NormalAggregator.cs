// Decompiled with JetBrains decompiler
// Type: VRage.Utils.NormalAggregator
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using VRageMath;

namespace VRage.Utils
{
  public class NormalAggregator
  {
    private Vector3[] m_data;
    private int m_currentDataIndex;
    private Vector3 m_cachedSum = Vector3.Zero;
    private Vector3 m_normalCache;
    private bool? m_isValidCache;

    public NormalAggregator(int averageWindowSize) => this.m_data = new Vector3[averageWindowSize];

    public void PushNext(ref Vector3 normal)
    {
      this.m_isValidCache = new bool?();
      this.m_cachedSum -= this.m_data[this.m_currentDataIndex];
      this.m_data[this.m_currentDataIndex] = normal;
      this.m_cachedSum += normal;
      ++this.m_currentDataIndex;
      if (this.m_currentDataIndex < this.m_data.Length)
        return;
      this.m_currentDataIndex = 0;
    }

    public bool GetAvgNormal(out Vector3 normal)
    {
      if (this.m_isValidCache.HasValue)
      {
        normal = this.m_normalCache;
        return this.m_isValidCache.Value;
      }
      this.m_isValidCache = new bool?(this.GetAvgNormalInternal(out normal));
      this.m_normalCache = normal;
      return this.m_isValidCache.Value;
    }

    public void Clear()
    {
      this.m_isValidCache = new bool?(false);
      this.m_cachedSum = Vector3.Zero;
      Array.Clear((Array) this.m_data, 0, this.m_data.Length);
    }

    private bool GetAvgNormalInternal(out Vector3 normal)
    {
      float num = this.m_cachedSum.Length();
      if (MyUtils.IsZero(num, 1E-05f))
      {
        normal = Vector3.Zero;
        return false;
      }
      normal = this.m_cachedSum / num;
      return true;
    }
  }
}
