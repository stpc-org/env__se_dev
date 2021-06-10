// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyTimedItemCache
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using VRageMath;

namespace VRage.Utils
{
  public class MyTimedItemCache
  {
    private readonly HashSet<long> m_eventHappenedHere = new HashSet<long>();
    private readonly Queue<KeyValuePair<long, int>> m_eventQueue = new Queue<KeyValuePair<long, int>>();

    public MyTimedItemCache(int eventTimeoutMs) => this.EventTimeoutMs = eventTimeoutMs;

    public int EventTimeoutMs { get; set; }

    public bool IsItemPresent(long itemHashCode, int currentTimeMs, bool autoinsert = true)
    {
      while (this.m_eventQueue.Count > 0)
      {
        int num1 = currentTimeMs;
        KeyValuePair<long, int> keyValuePair = this.m_eventQueue.Peek();
        int num2 = keyValuePair.Value;
        if (num1 > num2)
        {
          HashSet<long> eventHappenedHere = this.m_eventHappenedHere;
          keyValuePair = this.m_eventQueue.Dequeue();
          long key = keyValuePair.Key;
          eventHappenedHere.Remove(key);
        }
        else
          break;
      }
      if (this.m_eventHappenedHere.Contains(itemHashCode))
        return true;
      if (autoinsert)
      {
        this.m_eventHappenedHere.Add(itemHashCode);
        this.m_eventQueue.Enqueue(new KeyValuePair<long, int>(itemHashCode, currentTimeMs + this.EventTimeoutMs));
      }
      return false;
    }

    public bool IsPlaceUsed(
      Vector3D position,
      double eventSpaceMapping,
      int currentTimeMs,
      bool autoinsert = true)
    {
      Vector3D vector3D = position * eventSpaceMapping;
      vector3D.X = Math.Floor(vector3D.X);
      vector3D.Y = Math.Floor(vector3D.Y);
      vector3D.Z = Math.Floor(vector3D.Z);
      return this.IsItemPresent(vector3D.GetHash(), currentTimeMs, autoinsert);
    }
  }
}
