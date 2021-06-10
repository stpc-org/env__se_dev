// Decompiled with JetBrains decompiler
// Type: VRage.Game.Models.MyModels
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using VRage.FileSystem;
using VRage.Utils;

namespace VRage.Game.Models
{
  public static class MyModels
  {
    private static readonly ConcurrentDictionary<string, MyModel> m_models = new ConcurrentDictionary<string, MyModel>();

    public static void UnloadData()
    {
      MyModels.UnloadModelData();
      MyModels.m_models.Clear();
    }

    public static void UnloadModdedModels() => MyModels.UnloadModelData((Func<MyModel, bool>) (models => !MyFileSystem.IsGameContent(models.AssetName)));

    private static void UnloadModelData(Func<MyModel, bool> condition = null)
    {
      foreach (MyModel myModel in (IEnumerable<MyModel>) MyModels.m_models.Values)
      {
        if (condition == null || condition(myModel))
          myModel.UnloadData();
      }
    }

    public static MyModel GetModelOnlyData(string modelAsset)
    {
      if (string.IsNullOrEmpty(modelAsset))
        return (MyModel) null;
      MyModel orAdd = MyModels.GetOrAdd(modelAsset);
      if (!orAdd.LoadedData)
      {
        lock (orAdd)
        {
          if (!orAdd.LoadedData)
            orAdd.LoadData();
        }
      }
      return orAdd;
    }

    public static MyModel GetModelOnlyAnimationData(string modelAsset, bool forceReloadMwm = false)
    {
      MyModel orAdd = MyModels.GetOrAdd(modelAsset);
      try
      {
        if (!orAdd.LoadedData | forceReloadMwm)
        {
          lock (orAdd)
          {
            if (!orAdd.LoadedData)
              orAdd.LoadAnimationData(forceReloadMwm);
          }
        }
        return orAdd;
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLine(ex);
        return (MyModel) null;
      }
    }

    public static MyModel GetModelOnlyDummies(string modelAsset)
    {
      MyModel orAdd = MyModels.GetOrAdd(modelAsset);
      orAdd.LoadOnlyDummies();
      return orAdd;
    }

    public static MyModel GetModelOnlyModelInfo(string modelAsset)
    {
      MyModel orAdd = MyModels.GetOrAdd(modelAsset);
      orAdd.LoadOnlyModelInfo();
      return orAdd;
    }

    public static MyModel GetModel(string modelAsset)
    {
      if (modelAsset == null)
        return (MyModel) null;
      MyModel myModel;
      MyModels.m_models.TryGetValue(modelAsset, out myModel);
      return myModel;
    }

    private static MyModel GetOrAdd(string modelAsset) => MyModels.m_models.GetOrAdd(modelAsset, (Func<string, MyModel>) (m => new MyModel(m)));
  }
}
