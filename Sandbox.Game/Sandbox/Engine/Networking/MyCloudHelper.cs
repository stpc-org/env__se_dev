// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Networking.MyCloudHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using VRage.FileSystem;
using VRage.GameServices;
using VRage.Utils;
using VRageRender;

namespace Sandbox.Engine.Networking
{
  public static class MyCloudHelper
  {
    public const string BLUEPRINT_CLOUD_DIRECTORY = "Blueprints/cloud";
    public const string SCRIPT_CLOUD_DIRECTORY = "Scripts/cloud";
    public const string WORLD_CLOUD_DIRECTORY = "Worlds/cloud";
    public const string CONFIG_CLOUD_DIRECTORY = "Config/cloud";
    public const string LAST_SESSION_CLOUD_DIRECTORY = "Session/cloud";
    private static string[] m_validPrefixes = new string[4]
    {
      "Blueprints/cloud",
      "Worlds/cloud",
      "Config/cloud",
      "Session/cloud"
    };

    public static void FixStorage()
    {
      List<MyCloudFileInfo> cloudFiles = MyGameService.GetCloudFiles("");
      if (cloudFiles == null)
        return;
      bool flag = false;
      foreach (MyCloudFileInfo myCloudFileInfo in cloudFiles)
      {
        string[] strArray = myCloudFileInfo.Name.Split('/');
        if (strArray.Length < 2 || strArray[1] != "cloud")
        {
          if (!flag)
            MyLog.Default.WriteLine("Invalid cloud filenames: (will be removed)");
          MyLog.Default.WriteLine(myCloudFileInfo.Name);
          MyGameService.DeleteFromCloud(myCloudFileInfo.ContainerName);
          flag = true;
        }
      }
    }

    public static CloudResult CopyFiles(string oldSessionPath, string newSessionPath)
    {
      try
      {
        List<MyCloudFileInfo> cloudFiles = MyGameService.GetCloudFiles(oldSessionPath);
        if (cloudFiles == null || cloudFiles.Count == 0)
          return CloudResult.Failed;
        List<MyCloudFile> files = new List<MyCloudFile>();
        foreach (MyCloudFileInfo myCloudFileInfo in cloudFiles)
        {
          byte[] bytes = MyGameService.LoadFromCloud(myCloudFileInfo.Name);
          if (bytes != null)
          {
            string str = Path.Combine(MyFileSystem.TempPath, Path.GetFileName(myCloudFileInfo.Name));
            File.WriteAllBytes(str, bytes);
            files.Add(new MyCloudFile(str));
          }
        }
        return MyGameService.SaveToCloud(newSessionPath, files);
      }
      catch
      {
        return CloudResult.Failed;
      }
    }

    public static bool Delete(string fileName) => MyGameService.DeleteFromCloud(fileName);

    public static CloudResult UploadFiles(
      string cloudPath,
      string sourcePath,
      bool compress)
    {
      return MyCloudHelper.UploadFiles(cloudPath, new DirectoryInfo(sourcePath), compress);
    }

    public static CloudResult UploadFiles(
      string cloudPath,
      DirectoryInfo sourceDirectory,
      bool compress)
    {
      MyCloudHelper.Delete(cloudPath);
      List<MyCloudFile> files = new List<MyCloudFile>();
      foreach (FileInfo file in sourceDirectory.GetFiles())
      {
        string extension = Path.GetExtension(file.FullName);
        files.Add(new MyCloudFile(file.FullName, compress && extension.ToLower() != ".vx2"));
      }
      return MyGameService.SaveToCloud(cloudPath, files);
    }

    public static bool ExtractFilesTo(string cloudPath, string filePath, bool unpack)
    {
      try
      {
        List<MyCloudFileInfo> cloudFiles = MyGameService.GetCloudFiles(cloudPath);
        if (cloudFiles == null || cloudFiles.Count == 0)
          return false;
        if (Directory.Exists(filePath))
        {
          foreach (string file in Directory.GetFiles(filePath))
            File.Delete(file);
        }
        Directory.CreateDirectory(filePath);
        foreach (MyCloudFileInfo myCloudFileInfo in cloudFiles)
        {
          byte[] numArray = MyGameService.LoadFromCloud(myCloudFileInfo.Name);
          if (numArray != null)
          {
            string fileName = Path.GetFileName(myCloudFileInfo.Name);
            string str = Path.Combine(filePath, fileName);
            if (unpack)
            {
              using (MemoryStream stream1 = new MemoryStream(numArray))
              {
                using (Stream stream2 = stream1.UnwrapGZip())
                {
                  using (FileStream fileStream = File.OpenWrite(str))
                    stream2.CopyTo((Stream) fileStream);
                }
              }
            }
            else
              File.WriteAllBytes(str, numArray);
            string lower = Path.GetExtension(str).ToLower();
            if (lower == ".jpg" || lower == ".png")
              MyRenderProxy.UnloadTexture(str);
          }
        }
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static string LocalToCloudWorldPath(string localPath)
    {
      if (!localPath.StartsWith(MyFileSystem.SavesPath))
        return MyLocalCache.GetSessionSavesPath(Path.GetFileName(Path.GetDirectoryName(localPath)), false, false, true);
      return MyLocalCache.GetSessionSavesPath(localPath.Substring(MyFileSystem.SavesPath.Length).Trim('/', '\\').Replace('\\', ' ').Replace('/', ' '), false, false, true);
    }

    public static string CloudToLocalWorldPath(string cloudPath) => MyLocalCache.GetSessionSavesPath(Path.GetFileName(Path.GetDirectoryName(cloudPath)), false);

    public static string Combine(string container, string file) => container.EndsWith("/") ? container + file : container + "/" + file;

    public static ulong GetStorageSize(string containerName)
    {
      List<MyCloudFileInfo> cloudFiles = MyGameService.GetCloudFiles(containerName);
      ulong num = 0;
      foreach (MyCloudFileInfo myCloudFileInfo in cloudFiles)
        num += (ulong) myCloudFileInfo.Size;
      return num;
    }

    public static bool IsError(
      CloudResult error,
      out MyStringId errorMessage,
      MyStringId? defaultErrorMessage = null)
    {
      if (error == CloudResult.Ok)
      {
        errorMessage = MyStringId.NullOrEmpty;
        return false;
      }
      errorMessage = MyCloudHelper.GetErrorMessage(error, defaultErrorMessage);
      return true;
    }

    public static MyStringId GetErrorMessage(
      CloudResult error,
      MyStringId? defaultErrorMessage = null)
    {
      switch (error)
      {
        case CloudResult.QuotaExceeded:
        case CloudResult.OutOfLocalStorage:
          return MySpaceTexts.MessageBoxWorldOperation_Quota;
        case CloudResult.SynchronizationFailure:
          return MySpaceTexts.MessageBoxWorldOperation_CloudSynchronization;
        default:
          return defaultErrorMessage ?? MySpaceTexts.MessageBoxWorldOperation_Error;
      }
    }

    public static string ChangeContainerName(string containerPath, string newName)
    {
      containerPath = containerPath.Trim('/');
      int length = containerPath.LastIndexOf('/');
      containerPath = length == -1 ? newName : containerPath.Substring(0, length) + "/" + newName;
      return containerPath;
    }
  }
}
