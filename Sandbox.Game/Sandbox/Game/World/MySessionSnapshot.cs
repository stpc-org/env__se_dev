// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MySessionSnapshot
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Engine.Networking;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Voxels;
using VRage.GameServices;
using VRageMath;

namespace Sandbox.Game.World
{
  public class MySessionSnapshot
  {
    private static FastResourceLock m_savingLock = new FastResourceLock();
    public string TargetDir;
    public string SavingDir;
    public MyObjectBuilder_Checkpoint CheckpointSnapshot;
    public MyObjectBuilder_Sector SectorSnapshot;
    public Task VicinityGatherTask;
    public const int MAX_WINDOWS_PATH = 260;

    public Dictionary<string, byte[]> CompressedVoxelSnapshots { get; set; }

    public Dictionary<string, byte[]> VoxelSnapshots { get; set; }

    public Dictionary<string, IMyStorage> VoxelStorageNameCache { get; set; }

    public ulong SavedSizeInBytes { get; private set; }

    public bool SavingSuccess { get; private set; }

    public bool TooLongPath { get; private set; }

    public CloudResult CloudResult { get; private set; }

    public bool Save(Func<bool> screenshotTaken, string thumbName)
    {
      this.VicinityGatherTask.WaitOrExecute();
      bool gameSavesToCloud = MyPlatformGameSettings.GAME_SAVES_TO_CLOUD;
      bool flag = true;
      using (MySessionSnapshot.m_savingLock.AcquireExclusiveUsing())
      {
        MySandboxGame.Log.WriteLine("Session snapshot save - START");
        using (MySandboxGame.Log.IndentUsing())
        {
          Directory.CreateDirectory(this.TargetDir);
          MySandboxGame.Log.WriteLine("Checking file access for files in target dir.");
          if (!this.CheckAccessToFiles())
          {
            this.SavingSuccess = false;
            return false;
          }
          string savingDir = this.SavingDir;
          if (Directory.Exists(savingDir))
            Directory.Delete(savingDir, true);
          Directory.CreateDirectory(savingDir);
          List<MyCloudFile> myCloudFileList = new List<MyCloudFile>();
          if (thumbName != null)
            myCloudFileList.Add(new MyCloudFile(thumbName));
          try
          {
            ulong sizeInBytes1 = 0;
            ulong sizeInBytes2 = 0;
            ulong num = 0;
            this.TooLongPath = false;
            flag = MyLocalCache.SaveSector(this.SectorSnapshot, this.SavingDir, Vector3I.Zero, out sizeInBytes1, myCloudFileList) && MyLocalCache.SaveCheckpoint(this.CheckpointSnapshot, this.SavingDir, out sizeInBytes2, myCloudFileList);
            if (flag)
            {
              foreach (KeyValuePair<string, byte[]> voxelSnapshot in this.VoxelSnapshots)
              {
                if (Path.Combine(this.SavingDir, voxelSnapshot.Key).Length > 260)
                {
                  this.TooLongPath = true;
                  flag = false;
                  break;
                }
                ulong size = 0;
                flag = flag && this.SaveVoxelSnapshot(voxelSnapshot.Key, voxelSnapshot.Value, true, out size, myCloudFileList);
                if (flag)
                  num += size;
              }
              this.VoxelSnapshots.Clear();
              this.VoxelStorageNameCache.Clear();
              foreach (KeyValuePair<string, byte[]> compressedVoxelSnapshot in this.CompressedVoxelSnapshots)
              {
                if (Path.Combine(this.SavingDir, compressedVoxelSnapshot.Key).Length > 260)
                {
                  this.TooLongPath = true;
                  flag = false;
                  break;
                }
                ulong size = 0;
                flag = flag && this.SaveVoxelSnapshot(compressedVoxelSnapshot.Key, compressedVoxelSnapshot.Value, false, out size, myCloudFileList);
                if (flag)
                  num += size;
              }
              this.CompressedVoxelSnapshots.Clear();
            }
            if (flag && Sync.IsServer)
              flag = MyLocalCache.SaveLastSessionInfo(this.TargetDir, false, false, MySession.Static.Name, (string) null, 0);
            if (flag)
            {
              this.SavedSizeInBytes = sizeInBytes1 + sizeInBytes2 + num;
              if (screenshotTaken != null)
              {
                while (!screenshotTaken())
                  Thread.Sleep(10);
              }
              if (gameSavesToCloud)
              {
                this.CloudResult = MyGameService.SaveToCloud(MyCloudHelper.LocalToCloudWorldPath(this.TargetDir), myCloudFileList);
                flag = this.CloudResult == CloudResult.Ok;
              }
            }
            if (flag)
            {
              HashSet<string> stringSet = new HashSet<string>();
              foreach (string file in Directory.GetFiles(savingDir))
              {
                string fileName = Path.GetFileName(file);
                File.Copy(file, Path.Combine(this.TargetDir, fileName), true);
                stringSet.Add(fileName);
              }
              try
              {
                foreach (string file in Directory.GetFiles(this.TargetDir))
                {
                  string fileName = Path.GetFileName(file);
                  if (!stringSet.Contains(fileName) && !(fileName == MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION))
                    File.Delete(file);
                }
                Directory.Delete(savingDir, true);
              }
              catch (Exception ex)
              {
                MySandboxGame.Log.WriteLine("There was an error while cleaning the snapshot.");
                MySandboxGame.Log.WriteLine(ex);
              }
              this.Backup(this.TargetDir, this.TargetDir);
            }
          }
          catch (Exception ex)
          {
            MySandboxGame.Log.WriteLine("There was an error while saving snapshot.");
            MySandboxGame.Log.WriteLine(ex);
            flag = false;
          }
          if (!flag)
          {
            try
            {
              if (Directory.Exists(savingDir))
                Directory.Delete(savingDir, true);
            }
            catch (Exception ex)
            {
              MySandboxGame.Log.WriteLine("There was an error while cleaning snapshot.");
              MySandboxGame.Log.WriteLine(ex);
            }
          }
        }
        MySandboxGame.Log.WriteLine("Session snapshot save - END");
      }
      this.SavingSuccess = flag;
      return flag;
    }

    private void Backup(string targetDir, string backupDir)
    {
      if (MySession.Static.MaxBackupSaves > (short) 0)
      {
        string path3 = DateTime.Now.ToString("yyyy-MM-dd HHmmss");
        string str = Path.Combine(backupDir, MyTextConstants.SESSION_SAVE_BACKUP_FOLDER, path3);
        Directory.CreateDirectory(str);
        foreach (string file in Directory.GetFiles(targetDir))
        {
          string destFileName = Path.Combine(str, Path.GetFileName(file));
          if (destFileName.Length < 260 && file.Length < 260)
            File.Copy(file, destFileName, true);
        }
        string[] directories = Directory.GetDirectories(Path.Combine(backupDir, MyTextConstants.SESSION_SAVE_BACKUP_FOLDER));
        if (!MySessionSnapshot.IsSorted(directories))
          Array.Sort<string>(directories);
        if (directories.Length <= (int) MySession.Static.MaxBackupSaves)
          return;
        int num = directories.Length - (int) MySession.Static.MaxBackupSaves;
        for (int index = 0; index < num; ++index)
          Directory.Delete(directories[index], true);
      }
      else
      {
        if (MySession.Static.MaxBackupSaves != (short) 0 || !Directory.Exists(Path.Combine(backupDir, MyTextConstants.SESSION_SAVE_BACKUP_FOLDER)))
          return;
        Directory.Delete(Path.Combine(backupDir, MyTextConstants.SESSION_SAVE_BACKUP_FOLDER), true);
      }
    }

    public static bool IsSorted(string[] arr)
    {
      for (int index = 1; index < arr.Length; ++index)
      {
        if (arr[index - 1].CompareTo(arr[index]) > 0)
          return false;
      }
      return true;
    }

    private bool SaveVoxelSnapshot(
      string storageName,
      byte[] snapshotData,
      bool compress,
      out ulong size,
      List<MyCloudFile> fileList)
    {
      string str = Path.Combine(this.SavingDir, storageName + ".vx2");
      fileList.Add(new MyCloudFile(str));
      try
      {
        if (compress)
        {
          using (MemoryStream memoryStream = new MemoryStream(16384))
          {
            using (GZipStream gzipStream = new GZipStream((Stream) memoryStream, CompressionMode.Compress))
              gzipStream.Write(snapshotData, 0, snapshotData.Length);
            byte[] array = memoryStream.ToArray();
            File.WriteAllBytes(str, array);
            size = (ulong) array.Length;
            if (this.VoxelStorageNameCache != null)
            {
              IMyStorage myStorage = (IMyStorage) null;
              if (this.VoxelStorageNameCache.TryGetValue(storageName, out myStorage))
              {
                if (!myStorage.Closed)
                  myStorage.SetDataCache(array, true);
              }
            }
          }
        }
        else
        {
          File.WriteAllBytes(str, snapshotData);
          size = (ulong) snapshotData.Length;
        }
      }
      catch (Exception ex)
      {
        MySandboxGame.Log.WriteLine(string.Format("Failed to write voxel file '{0}'", (object) str));
        MySandboxGame.Log.WriteLine(ex);
        size = 0UL;
        return false;
      }
      return true;
    }

    private bool CheckAccessToFiles()
    {
      foreach (string file in Directory.GetFiles(this.TargetDir, "*", SearchOption.TopDirectoryOnly))
      {
        if (!(file == MySession.Static.ThumbPath) && !MyFileSystem.CheckFileWriteAccess(file))
        {
          MySandboxGame.Log.WriteLine(string.Format("Couldn't access file '{0}'.", (object) Path.GetFileName(file)));
          return false;
        }
      }
      return true;
    }

    public void SaveParallel(
      Func<bool> screenshotTaken,
      string screenshotPath,
      Action completionCallback = null)
    {
      MySessionSnapshot copy = this;
      Action action = (Action) (() => copy.Save(screenshotTaken, screenshotPath));
      if (completionCallback != null)
        Parallel.Start(action, completionCallback, WorkPriority.Low);
      else
        Parallel.Start(action, WorkPriority.Low);
    }

    public static void WaitForSaving()
    {
      int num = 0;
      do
      {
        using (MySessionSnapshot.m_savingLock.AcquireExclusiveUsing())
          num = MySessionSnapshot.m_savingLock.ExclusiveWaiters;
      }
      while (num > 0);
    }
  }
}
