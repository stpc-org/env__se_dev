// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyLoadListResult
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Game.GUI;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using VRage.Utils;

namespace Sandbox.Game.Screens.Helpers
{
  public abstract class MyLoadListResult : IMyAsyncResult
  {
    public List<Tuple<string, MyWorldInfo>> AvailableSaves = new List<Tuple<string, MyWorldInfo>>();
    public bool ContainsCorruptedWorlds;
    public readonly List<string> CustomPaths;

    public bool IsCompleted => this.Task.IsComplete;

    public Task Task { get; private set; }

    public MyLoadListResult(List<string> customPaths = null)
    {
      this.CustomPaths = customPaths;
      this.Task = Parallel.Start((Action) (() => this.LoadListAsync()));
    }

    private void LoadListAsync()
    {
      this.AvailableSaves = this.GetAvailableSaves();
      this.ContainsCorruptedWorlds = false;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (Tuple<string, MyWorldInfo> availableSave in this.AvailableSaves)
      {
        if (availableSave.Item2 != null && availableSave.Item2.IsCorrupted)
        {
          stringBuilder.Append(Path.GetFileNameWithoutExtension(availableSave.Item1)).Append("\n");
          this.ContainsCorruptedWorlds = true;
        }
      }
      this.AvailableSaves.RemoveAll((Predicate<Tuple<string, MyWorldInfo>>) (x => x == null || x.Item2 == null));
      if (this.ContainsCorruptedWorlds && MyLog.Default != null)
      {
        MyLog.Default.WriteLine("Corrupted worlds: ");
        MyLog.Default.WriteLine(stringBuilder.ToString());
      }
      if (this.AvailableSaves.Count == 0)
        return;
      this.AvailableSaves.Sort((Comparison<Tuple<string, MyWorldInfo>>) ((a, b) => b.Item2.LastSaveTime.CompareTo(a.Item2.LastSaveTime)));
    }

    protected abstract List<Tuple<string, MyWorldInfo>> GetAvailableSaves();

    [Conditional("DEBUG")]
    private void VerifyUniqueWorldID(List<Tuple<string, MyWorldInfo>> availableWorlds)
    {
      if (MyLog.Default == null)
        return;
      HashSet<string> stringSet = new HashSet<string>();
      foreach (Tuple<string, MyWorldInfo> availableWorld in availableWorlds)
      {
        MyWorldInfo myWorldInfo = availableWorld.Item2;
        if (stringSet.Contains(availableWorld.Item1))
          MyLog.Default.WriteLine(string.Format("Non-unique WorldID detected. WorldID = {0}; World Folder Path = '{2}', World Name = '{1}'", (object) availableWorld.Item1, (object) myWorldInfo.SessionName, (object) availableWorld.Item1));
        stringSet.Add(availableWorld.Item1);
      }
    }
  }
}
