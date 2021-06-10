// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyDiscreteSampler`1
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VRage.Library.Utils;

namespace VRage.Utils
{
  public class MyDiscreteSampler<T> : IEnumerable<T>, IEnumerable
  {
    private T[] m_values;
    private MyDiscreteSampler m_sampler;

    public bool Initialized => this.m_sampler.Initialized;

    public MyDiscreteSampler(T[] values, IEnumerable<float> densities)
    {
      this.m_values = new T[values.Length];
      Array.Copy((Array) values, (Array) this.m_values, values.Length);
      this.m_sampler = new MyDiscreteSampler();
      this.m_sampler.Prepare(densities);
    }

    public MyDiscreteSampler(List<T> values, IEnumerable<float> densities)
    {
      this.m_values = new T[values.Count];
      for (int index = 0; index < values.Count; ++index)
        this.m_values[index] = values[index];
      this.m_sampler = new MyDiscreteSampler();
      this.m_sampler.Prepare(densities);
    }

    public MyDiscreteSampler(IEnumerable<T> values, IEnumerable<float> densities)
    {
      this.m_values = new T[values.Count<T>()];
      int index = 0;
      foreach (T obj in values)
      {
        this.m_values[index] = obj;
        ++index;
      }
      this.m_sampler = new MyDiscreteSampler();
      this.m_sampler.Prepare(densities);
    }

    public MyDiscreteSampler(Dictionary<T, float> densities)
      : this((IEnumerable<T>) densities.Keys, (IEnumerable<float>) densities.Values)
    {
    }

    public T Sample(MyRandom rng) => this.m_values[this.m_sampler.Sample(rng)];

    public T Sample(float sample) => this.m_values[this.m_sampler.Sample(sample)];

    public T Sample() => this.m_values[this.m_sampler.Sample()];

    public int Count => this.m_values.Length;

    public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>) this.m_values).AsEnumerable<T>().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
