// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Utils.MyLoadingPerformance
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sandbox.Engine.Utils
{
  internal class MyLoadingPerformance
  {
    private static MyLoadingPerformance m_instance;
    private Dictionary<uint, Tuple<int, string>> m_voxelCounts = new Dictionary<uint, Tuple<int, string>>();
    private TimeSpan m_loadingTime;
    private Stopwatch m_stopwatch;

    public static MyLoadingPerformance Instance => MyLoadingPerformance.m_instance ?? (MyLoadingPerformance.m_instance = new MyLoadingPerformance());

    public string LoadingName { get; set; }

    public bool IsTiming { get; private set; }

    private void Reset()
    {
      this.LoadingName = (string) null;
      this.m_loadingTime = TimeSpan.Zero;
      this.m_voxelCounts.Clear();
    }

    public void StartTiming()
    {
      if (this.IsTiming)
        return;
      this.Reset();
      this.IsTiming = true;
      this.m_stopwatch = Stopwatch.StartNew();
    }

    public void AddVoxelHandCount(int count, uint entityID, string name)
    {
      if (!this.IsTiming || this.m_voxelCounts.ContainsKey(entityID))
        return;
      this.m_voxelCounts.Add(entityID, new Tuple<int, string>(count, name));
    }

    public void FinishTiming()
    {
      this.m_stopwatch.Stop();
      this.IsTiming = false;
      this.m_loadingTime = this.m_stopwatch.Elapsed;
      this.WriteToLog();
    }

    public void WriteToLog()
    {
      MySandboxGame.Log.WriteLine("LOADING REPORT FOR: " + this.LoadingName);
      MySandboxGame.Log.IncreaseIndent();
      MySandboxGame.Log.WriteLine("Loading time: " + (object) this.m_loadingTime);
      MySandboxGame.Log.IncreaseIndent();
      foreach (KeyValuePair<uint, Tuple<int, string>> voxelCount in this.m_voxelCounts)
      {
        if (voxelCount.Value.Item1 > 0)
          MySandboxGame.Log.WriteLine("Asteroid: " + (object) voxelCount.Key + " voxel hands: " + (object) voxelCount.Value.Item1 + ". Voxel File: " + voxelCount.Value.Item2);
      }
      MySandboxGame.Log.DecreaseIndent();
      MySandboxGame.Log.DecreaseIndent();
      MySandboxGame.Log.WriteLine("END OF LOADING REPORT");
    }
  }
}
