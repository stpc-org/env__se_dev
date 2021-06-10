// Decompiled with JetBrains decompiler
// Type: VRage.Generics.MyWeightDictionary`1
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;

namespace VRage.Generics
{
  public class MyWeightDictionary<T>
  {
    private Dictionary<T, float> m_data;
    private float m_sum;

    public MyWeightDictionary(Dictionary<T, float> data)
    {
      this.m_data = data;
      this.m_sum = 0.0f;
      foreach (KeyValuePair<T, float> keyValuePair in data)
        this.m_sum += keyValuePair.Value;
    }

    public int Count => this.m_data.Count;

    public float GetSum() => this.m_sum;

    public T GetItemByWeightNormalized(float weightNormalized) => this.GetItemByWeight(weightNormalized * this.m_sum);

    public T GetItemByWeight(float weight)
    {
      float num = 0.0f;
      T obj = default (T);
      foreach (KeyValuePair<T, float> keyValuePair in this.m_data)
      {
        obj = keyValuePair.Key;
        num += keyValuePair.Value;
        if ((double) num > (double) weight)
          return obj;
      }
      return obj;
    }

    public T GetRandomItem(Random rnd) => this.GetItemByWeight((float) rnd.NextDouble() * this.m_sum);
  }
}
