// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyGuiControlSaveBrowser
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Networking;
using Sandbox.Game.GUI;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.Gui.DirectoryBrowser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VRage;
using VRage.FileSystem;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
  public class MyGuiControlSaveBrowser : MyGuiControlDirectoryBrowser
  {
    private readonly List<FileInfo> m_saveEntriesToCreate = new List<FileInfo>();
    private readonly Dictionary<string, MyWorldInfo> m_loadedWorldsByFilePaths = new Dictionary<string, MyWorldInfo>();
    public string SearchTextFilter;

    public bool InBackupsFolder { get; private set; }

    public MyGuiControlSaveBrowser()
      : base(MyFileSystem.SavesPath, MyFileSystem.SavesPath)
    {
      this.SetColumnName(1, MyTexts.Get(MyCommonTexts.Date));
      this.SetColumnComparison(1, (Comparison<MyGuiControlTable.Cell>) ((cellA, cellB) =>
      {
        if (cellA == null || cellB == null)
          return -1;
        FileInfo userData1 = cellA.UserData as FileInfo;
        FileInfo userData2 = cellB.UserData as FileInfo;
        if (userData1 == userData2)
        {
          if (userData1 == null)
            return 0;
        }
        else
        {
          if (userData1 == null)
            return -1;
          if (userData2 == null)
            return 1;
        }
        return this.m_loadedWorldsByFilePaths[userData1.DirectoryName].LastSaveTime.CompareTo(this.m_loadedWorldsByFilePaths[userData2.DirectoryName].LastSaveTime);
      }));
    }

    public DirectoryInfo GetDirectory(MyGuiControlTable.Row row) => row == null ? (DirectoryInfo) null : row.UserData as DirectoryInfo;

    public void GetSave(MyGuiControlTable.Row row, out MySaveInfo info)
    {
      info = new MySaveInfo();
      if (row == null)
        return;
      if (row.UserData is MyCloudFile userData)
      {
        MyWorldInfo myWorldInfo;
        if (!this.m_loadedWorldsByFilePaths.TryGetValue(userData.CloudName, out myWorldInfo))
          return;
        info = new MySaveInfo()
        {
          Valid = true,
          Name = userData.CloudName,
          WorldInfo = myWorldInfo,
          IsCloud = true
        };
      }
      else
      {
        if (!(row.UserData is FileInfo userData))
          return;
        string directoryName = Path.GetDirectoryName(userData.FullName);
        MyWorldInfo myWorldInfo;
        if (!this.m_loadedWorldsByFilePaths.TryGetValue(directoryName, out myWorldInfo))
          return;
        info = new MySaveInfo()
        {
          Valid = true,
          Name = directoryName,
          WorldInfo = myWorldInfo
        };
      }
    }

    public void AccessBackups()
    {
      MySaveInfo info;
      this.GetSave(this.SelectedRow, out info);
      if (!info.Valid)
        return;
      DirectoryInfo directoryInfo1 = !info.IsCloud ? new DirectoryInfo(info.Name) : new DirectoryInfo(MyCloudHelper.CloudToLocalWorldPath(info.Name));
      DirectoryInfo directoryInfo2 = (DirectoryInfo) null;
      if (directoryInfo1.Exists)
        directoryInfo2 = ((IEnumerable<DirectoryInfo>) directoryInfo1.GetDirectories()).FirstOrDefault<DirectoryInfo>((Func<DirectoryInfo, bool>) (dir => dir.Name.StartsWith("Backup")));
      if (directoryInfo2 == null)
      {
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.SaveBrowserMissingBackup), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
      }
      else
      {
        this.InBackupsFolder = true;
        this.CurrentDirectory = directoryInfo2.FullName;
      }
    }

    protected override void AddFolderRow(DirectoryInfo dir)
    {
      if (!this.SearchFilterTest(dir.Name))
        return;
      FileInfo[] files = dir.GetFiles();
      bool flag = false;
      foreach (FileInfo fileInfo in files)
      {
        if (fileInfo.Name == "Sandbox.sbc")
        {
          if (this.m_loadedWorldsByFilePaths.ContainsKey(fileInfo.DirectoryName))
            this.m_saveEntriesToCreate.Add(fileInfo);
          flag = true;
          break;
        }
      }
      if (flag)
        return;
      base.AddFolderRow(dir);
    }

    public override void Refresh() => this.RefreshTheWorldInfos();

    public void ForceRefresh() => MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, new MyStringId?(), new Func<IMyAsyncResult>(this.StartLoadingWorldInfos), new Action<IMyAsyncResult, MyGuiScreenProgressAsync>(this.OnLoadingFinished)));

    public void RefreshAfterLoaded()
    {
      if (this.IsCurrentDirectoryCloud)
      {
        this.Clear();
        List<string> list = this.m_loadedWorldsByFilePaths.Keys.ToList<string>();
        list.Sort((Comparison<string>) ((fileA, fileB) => this.m_loadedWorldsByFilePaths[fileB].LastSaveTime.CompareTo(this.m_loadedWorldsByFilePaths[fileA].LastSaveTime)));
        foreach (string str in list)
          this.AddSavedGame(str, (object) new MyCloudFile(str));
        this.ScrollToSelection();
      }
      else
      {
        base.Refresh();
        this.m_saveEntriesToCreate.Sort((Comparison<FileInfo>) ((fileA, fileB) => this.m_loadedWorldsByFilePaths[fileB.DirectoryName].LastSaveTime.CompareTo(this.m_loadedWorldsByFilePaths[fileA.DirectoryName].LastSaveTime)));
        foreach (FileInfo fileInfo in this.m_saveEntriesToCreate)
          this.AddSavedGame(fileInfo.DirectoryName, (object) fileInfo);
        this.m_saveEntriesToCreate.Clear();
      }
    }

    private void AddSavedGame(string saveName, object userData)
    {
      MyWorldInfo worldsByFilePath = this.m_loadedWorldsByFilePaths[saveName];
      if (!this.SearchFilterTest(worldsByFilePath.SessionName))
        return;
      MyGuiControlTable.Row row = new MyGuiControlTable.Row(userData);
      MyGuiControlTable.Cell cell = new MyGuiControlTable.Cell(worldsByFilePath.SessionName, userData, icon: new MyGuiHighlightTexture?(this.FileCellIconTexture), iconOriginAlign: this.FileCellIconAlign);
      if (worldsByFilePath.IsCorrupted)
        cell.TextColor = new Color?(Color.Red);
      row.AddCell(cell);
      row.AddCell(new MyGuiControlTable.Cell(worldsByFilePath.LastSaveTime.ToString("g"), userData));
      row.AddCell(new MyGuiControlTable.Cell(MyValueFormatter.GetFormattedFileSizeInMB(worldsByFilePath.StorageSize), userData));
      this.Add(row);
    }

    private void RefreshTheWorldInfos()
    {
      this.m_loadedWorldsByFilePaths.Clear();
      MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, new MyStringId?(), new Func<IMyAsyncResult>(this.StartLoadingWorldInfos), new Action<IMyAsyncResult, MyGuiScreenProgressAsync>(this.OnLoadingFinished)));
    }

    private bool SearchFilterTest(string testString)
    {
      if (this.SearchTextFilter != null && this.SearchTextFilter.Length != 0)
      {
        string[] strArray = this.SearchTextFilter.Split(' ');
        string lower = testString.ToLower();
        foreach (string str in strArray)
        {
          if (!lower.Contains(str.ToLower()))
            return false;
        }
      }
      return true;
    }

    private bool IsCurrentDirectoryCloud => MyPlatformGameSettings.GAME_SAVES_TO_CLOUD && !this.InBackupsFolder;

    private IMyAsyncResult StartLoadingWorldInfos()
    {
      if (this.IsCurrentDirectoryCloud)
        return (IMyAsyncResult) new MyLoadWorldInfoListFromCloudResult(new List<string>()
        {
          this.GetLocalPath()
        });
      return (IMyAsyncResult) new MyLoadWorldInfoListResult(new List<string>()
      {
        this.CurrentDirectory
      });
    }

    private void OnLoadingFinished(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
    {
      MyLoadListResult myLoadListResult = (MyLoadListResult) result;
      this.m_loadedWorldsByFilePaths.Clear();
      foreach (Tuple<string, MyWorldInfo> availableSave in myLoadListResult.AvailableSaves)
        this.m_loadedWorldsByFilePaths[availableSave.Item1] = availableSave.Item2;
      if (myLoadListResult.ContainsCorruptedWorlds)
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.SomeWorldFilesCouldNotBeLoaded), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
      this.RefreshAfterLoaded();
      screen.CloseScreen();
    }

    protected override void OnBackDoubleclicked()
    {
      if (this.m_currentDir.Name.StartsWith("Backup"))
      {
        this.CurrentDirectory = this.m_currentDir.Parent.Parent.FullName;
        this.InBackupsFolder = false;
        this.IgnoreFirstRowForSort = false;
      }
      else
        base.OnBackDoubleclicked();
    }
  }
}
