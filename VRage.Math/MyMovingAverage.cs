// Decompiled with JetBrains decompiler
// Type: VRageMath.MyMovingAverage
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System.Collections.Generic;

namespace VRageMath
{
  public class MyMovingAverage
  {
    private readonly Queue<float> m_queue = new Queue<float>();
    private readonly int m_windowSize;
    private int m_enqueueCounter;
    private readonly int m_enqueueCountToReset;

    public float Avg => this.m_queue.Count <= 0 ? 0.0f : (float) this.Sum / (float) this.m_queue.Count;

    public double Sum { get; private set; }

    public MyMovingAverage(int windowSize, int enqueueCountToReset = 1000)
    {
      this.m_windowSize = windowSize;
      this.m_enqueueCountToReset = enqueueCountToReset;
    }

    private void UpdateSum()
    {
      this.Sum = 0.0;
      foreach (double num in this.m_queue)
        this.Sum += num;
    }

    public void Enqueue(float value)
    {
      this.m_queue.Enqueue(value);
      ++this.m_enqueueCounter;
      if (this.m_enqueueCounter > this.m_enqueueCountToReset)
      {
        this.m_enqueueCounter = 0;
        this.UpdateSum();
      }
      else
        this.Sum += (double) value;
      while (this.m_queue.Count > this.m_windowSize)
        this.Sum -= (double) this.m_queue.Dequeue();
    }

    public void Reset()
    {
      this.Sum = 0.0;
      this.m_queue.Clear();
    }
  }
}
