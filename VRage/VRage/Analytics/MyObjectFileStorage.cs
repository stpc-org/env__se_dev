// Decompiled with JetBrains decompiler
// Type: VRage.Analytics.MyObjectFileStorage
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using VRage.Utils;

namespace VRage.Analytics
{
  public class MyObjectFileStorage
  {
    public string StoragePath { get; private set; }

    public int MaxStoredFilesPerType { get; private set; }

    public MyObjectFileStorage(string storagePath, int maxStoredFilesPerType = -1)
    {
      if (string.IsNullOrEmpty(storagePath))
        throw new ArgumentNullException("storagePath can't be null");
      Directory.CreateDirectory(storagePath);
      this.StoragePath = storagePath;
      this.MaxStoredFilesPerType = maxStoredFilesPerType;
    }

    public bool StoreObject<T>(T objectToStore, DateTime timestamp) where T : class
    {
      string contents = this.SerializeObject<T>(objectToStore);
      string newFilePath = this.GetNewFilePath<T>(timestamp);
      Exception exception = (Exception) null;
      try
      {
        File.WriteAllText(newFilePath, contents);
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLine(string.Format("ObjectFileStorage error: {0}", (object) ex));
        exception = ex;
      }
      if (exception != null)
        return false;
      this.PruneExcessFilesOfType<T>();
      return true;
    }

    public List<T> RetrieveStoredObjectsByType<T>(bool shouldWipeAfter = false) where T : class
    {
      List<T> objList = new List<T>();
      try
      {
        foreach (string file in Directory.GetFiles(this.StoragePath, this.GetFilePrefixForType<T>() + "*"))
        {
          T obj = this.DeserializeObject<T>(File.ReadAllText(file));
          objList.Add(obj);
        }
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLine(string.Format("Analytics storage error: {0}", (object) ex));
        return (List<T>) null;
      }
      if (shouldWipeAfter)
        this.WipeStoredObjectsByType<T>();
      return objList;
    }

    public int WipeStoredObjectsByType<T>() where T : class => this.DeleteStoredObjectsByType<T>();

    private string SerializeObject<T>(T objectToSerialize) where T : class => JsonMapper.ToJson((object) objectToSerialize);

    private T DeserializeObject<T>(string serializedObject) where T : class => JsonMapper.ToObject<T>(serializedObject);

    private string GetNewFilePath<T>(DateTime timestamp) => Path.Combine(this.StoragePath, this.GetFilePrefixForType<T>() + timestamp.ToString("o").Replace(':', '-') + "_" + Guid.NewGuid().ToString());

    private string GetFilePrefixForType<T>() => typeof (T).Name + "_";

    private void PruneExcessFilesOfType<T>() where T : class => this.DeleteStoredObjectsByType<T>(this.MaxStoredFilesPerType);

    private int DeleteStoredObjectsByType<T>(int amountToKeep = 0) where T : class
    {
      if (amountToKeep < 0)
        return 0;
      int num = 0;
      try
      {
        string[] files = Directory.GetFiles(this.StoragePath, this.GetFilePrefixForType<T>() + "*");
        Array.Sort<string>(files);
        for (int index = 0; index < files.Length - amountToKeep; ++index)
        {
          File.Delete(files[index]);
          ++num;
        }
        return num;
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLine(string.Format("ObjectFileStorage error: {0}", (object) ex));
        return num;
      }
    }
  }
}
