// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyBlueprintUtils
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.ViewModels;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.GameServices;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GUI
{
  public class MyBlueprintUtils
  {
    public static readonly string THUMB_IMAGE_NAME = "thumb.png";
    public static readonly string DEFAULT_SCRIPT_NAME = "Script";
    public static readonly string SCRIPT_EXTENSION = ".cs";
    public static readonly string BLUEPRINT_WORKSHOP_EXTENSION = ".sbb";
    public static readonly string BLUEPRINT_LOCAL_NAME = "bp.sbc";
    public static readonly string STEAM_THUMBNAIL_NAME = "Textures\\GUI\\Icons\\IngameProgrammingIcon.png";
    public static readonly string SCRIPTS_DIRECTORY = "IngameScripts";
    public static readonly string BLUEPRINT_DIRECTORY = "Blueprints";
    public static readonly string BLUEPRINT_DEFAULT_DIRECTORY = Path.Combine(MyFileSystem.ContentPath, "Data", "Blueprints");
    public static readonly string SCRIPT_FOLDER_LOCAL = Path.Combine(MyFileSystem.UserDataPath, MyBlueprintUtils.SCRIPTS_DIRECTORY, "local");
    public static readonly string SCRIPT_FOLDER_TEMP = Path.Combine(MyFileSystem.UserDataPath, MyBlueprintUtils.SCRIPTS_DIRECTORY, "temp");
    public static readonly string SCRIPT_FOLDER_WORKSHOP = Path.Combine(MyFileSystem.UserDataPath, MyBlueprintUtils.SCRIPTS_DIRECTORY, "workshop");
    public static readonly string BLUEPRINT_FOLDER_LOCAL = Path.Combine(MyFileSystem.UserDataPath, MyBlueprintUtils.BLUEPRINT_DIRECTORY, "local");
    public static readonly string BLUEPRINT_FOLDER_TEMP = Path.Combine(MyFileSystem.UserDataPath, MyBlueprintUtils.BLUEPRINT_DIRECTORY, "temp");
    public static readonly string BLUEPRINT_FOLDER_WORKSHOP = Path.Combine(MyFileSystem.UserDataPath, MyBlueprintUtils.BLUEPRINT_DIRECTORY, "workshop");
    public static readonly string BLUEPRINT_WORKSHOP_TEMP = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_WORKSHOP, "temp");
    private const int FILE_PATH_MAX_LENGTH = 260;

    public static MyObjectBuilder_Definitions LoadPrefab(string filePath)
    {
      MyObjectBuilder_Definitions objectBuilder = (MyObjectBuilder_Definitions) null;
      bool flag = false;
      string path = filePath + "B5";
      if (MyFileSystem.FileExists(path))
      {
        flag = MyObjectBuilderSerializer.DeserializePB<MyObjectBuilder_Definitions>(path, out objectBuilder);
        if (objectBuilder == null || objectBuilder.ShipBlueprints == null)
        {
          flag = MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_Definitions>(filePath, out objectBuilder);
          if (objectBuilder != null)
            MyObjectBuilderSerializer.SerializePB(path, false, (MyObjectBuilder_Base) objectBuilder);
        }
      }
      else if (MyFileSystem.FileExists(filePath))
      {
        flag = MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_Definitions>(filePath, out objectBuilder);
        if (flag)
          MyObjectBuilderSerializer.SerializePB(path, false, (MyObjectBuilder_Base) objectBuilder);
      }
      return !flag ? (MyObjectBuilder_Definitions) null : objectBuilder;
    }

    public static MyObjectBuilder_Definitions LoadPrefabFromCloud(
      MyBlueprintItemInfo info)
    {
      MyObjectBuilder_Definitions objectBuilder = (MyObjectBuilder_Definitions) null;
      if (!string.IsNullOrEmpty(info.CloudPathPB))
      {
        byte[] buffer = MyGameService.LoadFromCloud(info.CloudPathPB);
        if (buffer != null)
        {
          using (MemoryStream memoryStream = new MemoryStream(buffer))
            MyObjectBuilderSerializer.DeserializePB<MyObjectBuilder_Definitions>((Stream) memoryStream, out objectBuilder);
        }
      }
      else if (!string.IsNullOrEmpty(info.CloudPathXML))
      {
        byte[] buffer = MyGameService.LoadFromCloud(info.CloudPathXML);
        if (buffer != null)
        {
          using (MemoryStream stream = new MemoryStream(buffer))
          {
            using (Stream reader = stream.UnwrapGZip())
              MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_Definitions>(reader, out objectBuilder);
          }
        }
      }
      return objectBuilder;
    }

    public static bool CopyFileFromCloud(string pathFull, string pathRel)
    {
      byte[] buffer = MyGameService.LoadFromCloud(pathRel);
      if (buffer == null)
        return false;
      using (MemoryStream memoryStream = new MemoryStream(buffer))
      {
        memoryStream.Seek(0L, SeekOrigin.Begin);
        MyFileSystem.CreateDirectoryRecursive(Path.GetDirectoryName(pathFull));
        using (FileStream fileStream = new FileStream(pathFull, FileMode.OpenOrCreate))
        {
          memoryStream.CopyTo((Stream) fileStream);
          fileStream.Flush();
        }
      }
      return true;
    }

    public static MyObjectBuilder_Definitions LoadWorkshopPrefab(
      string archive,
      ulong? publishedItemId,
      string publishedServiceName,
      bool isOldBlueprintScreen)
    {
      if (!File.Exists(archive) && !MyFileSystem.DirectoryExists(archive) || !publishedItemId.HasValue)
        return (MyObjectBuilder_Definitions) null;
      MyWorkshopItem myWorkshopItem;
      if (isOldBlueprintScreen)
      {
        myWorkshopItem = MyGuiBlueprintScreen.m_subscribedItemsList.Find((Predicate<MyWorkshopItem>) (item =>
        {
          long id = (long) item.Id;
          ulong? nullable = publishedItemId;
          long valueOrDefault = (long) nullable.GetValueOrDefault();
          return id == valueOrDefault & nullable.HasValue && item.ServiceName == publishedServiceName;
        }));
      }
      else
      {
        using (MyGuiBlueprintScreen_Reworked.SubscribedItemsLock.AcquireSharedUsing())
          myWorkshopItem = MyGuiBlueprintScreen_Reworked.GetSubscribedItemsList(Content.Blueprint).Find((Predicate<MyWorkshopItem>) (item =>
          {
            long id = (long) item.Id;
            ulong? nullable = publishedItemId;
            long valueOrDefault = (long) nullable.GetValueOrDefault();
            return id == valueOrDefault & nullable.HasValue && item.ServiceName == publishedServiceName;
          }));
      }
      if (myWorkshopItem == null)
        return (MyObjectBuilder_Definitions) null;
      string str1 = Path.Combine(archive, MyBlueprintUtils.BLUEPRINT_LOCAL_NAME);
      string str2 = str1 + "B5";
      if (!MyFileSystem.FileExists(str2) && publishedItemId.HasValue)
      {
        string str3 = Path.Combine(MyBlueprintUtils.BLUEPRINT_WORKSHOP_TEMP, myWorkshopItem.ServiceName, publishedItemId.Value.ToString());
        MyFileSystem.EnsureDirectoryExists(str3);
        str2 = Path.Combine(str3, MyBlueprintUtils.BLUEPRINT_LOCAL_NAME) + "B5";
      }
      bool flag1 = false;
      MyObjectBuilder_Definitions objectBuilder = (MyObjectBuilder_Definitions) null;
      int num1 = MyFileSystem.FileExists(str2) ? 1 : 0;
      bool flag2 = MyFileSystem.FileExists(str1);
      bool flag3 = false;
      int num2 = flag2 ? 1 : 0;
      if ((num1 & num2) != 0 && new FileInfo(str2).LastWriteTimeUtc >= new FileInfo(str1).LastWriteTimeUtc)
        flag3 = true;
      if (flag3)
      {
        flag1 = MyObjectBuilderSerializer.DeserializePB<MyObjectBuilder_Definitions>(str2, out objectBuilder);
        if (objectBuilder == null || objectBuilder.ShipBlueprints == null)
          flag1 = MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_Definitions>(str1, out objectBuilder);
      }
      else if (flag2)
      {
        flag1 = MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_Definitions>(str1, out objectBuilder);
        if (flag1 && publishedItemId.HasValue)
          MyObjectBuilderSerializer.SerializePB(str2, false, (MyObjectBuilder_Base) objectBuilder);
      }
      if (!flag1)
        return (MyObjectBuilder_Definitions) null;
      objectBuilder.ShipBlueprints[0].Description = myWorkshopItem.Description;
      objectBuilder.ShipBlueprints[0].CubeGrids[0].DisplayName = myWorkshopItem.Title;
      objectBuilder.ShipBlueprints[0].DLCs = new string[myWorkshopItem.DLCs.Count];
      int index = 0;
      while (true)
      {
        int num3 = index;
        ListReader<uint> dlCs = myWorkshopItem.DLCs;
        int count = dlCs.Count;
        if (num3 < count)
        {
          dlCs = myWorkshopItem.DLCs;
          MyDLCs.MyDLC dlc;
          if (MyDLCs.TryGetDLC(dlCs[index], out dlc))
            objectBuilder.ShipBlueprints[0].DLCs[index] = dlc.Name;
          ++index;
        }
        else
          break;
      }
      return objectBuilder;
    }

    public static void PublishBlueprint(
      MyObjectBuilder_Definitions prefab,
      string blueprintName,
      string currentLocalDirectory,
      string sourceFile,
      MyBlueprintTypeEnum type)
    {
      string file = sourceFile ?? Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, currentLocalDirectory, blueprintName);
      string title = prefab.ShipBlueprints[0].CubeGrids[0].DisplayName;
      string description = prefab.ShipBlueprints[0].Description;
      WorkshopId[] publishIds = prefab.ShipBlueprints[0].WorkshopIds;
      if (publishIds == null)
        publishIds = new WorkshopId[1]
        {
          new WorkshopId(prefab.ShipBlueprints[0].WorkshopId, MyGameService.GetDefaultUGC().ServiceName)
        };
      MyCubeSize gridSize = prefab.ShipBlueprints[0].CubeGrids[0].GridSizeEnum;
      StringBuilder messageCaption = MyTexts.Get(MySpaceTexts.PublishBlueprint_Caption);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MySpaceTexts.PublishBlueprint_Question), messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (val =>
      {
        if (val != MyGuiScreenMessageBox.ResultEnum.YES)
          return;
        Action<MyGuiScreenMessageBox.ResultEnum, string[], string[]> callback = (Action<MyGuiScreenMessageBox.ResultEnum, string[], string[]>) ((tagsResult, outTags, serviceNames) =>
        {
          if (tagsResult != MyGuiScreenMessageBox.ResultEnum.YES)
            return;
          publishIds = MyWorkshop.FilterWorkshopIds(publishIds, serviceNames);
          HashSet<uint> source = new HashSet<uint>();
          foreach (MyObjectBuilder_ShipBlueprintDefinition shipBlueprint in prefab.ShipBlueprints)
          {
            if (shipBlueprint.DLCs != null)
            {
              foreach (string dlC in shipBlueprint.DLCs)
              {
                MyDLCs.MyDLC dlc;
                if (MyDLCs.TryGetDLC(dlC, out dlc))
                {
                  source.Add(dlc.AppId);
                }
                else
                {
                  uint result;
                  if (uint.TryParse(dlC, out result))
                    source.Add(result);
                }
              }
            }
          }
          Array.Resize<string>(ref outTags, outTags.Length + 1 + 1);
          outTags[outTags.Length - 1] = gridSize == MyCubeSize.Large ? "large_grid" : "small_grid";
          outTags[outTags.Length - 2] = true ? MySteamConstants.TAG_SAFE : MySteamConstants.TAG_EXPERIMENTAL;
          MyWorkshop.PublishBlueprintAsync(file, title, description, publishIds, outTags, source.ToArray<uint>(), MyPublishedFileVisibility.Public, (Action<bool, MyGameServiceCallResult, string, MyWorkshopItem[]>) ((success, result, resultServiceName, publishedItems) =>
          {
            if (publishedItems.Length != 0)
            {
              WorkshopId[] workshopIds = publishedItems.ToWorkshopIds();
              prefab.ShipBlueprints[0].WorkshopId = 0UL;
              prefab.ShipBlueprints[0].WorkshopIds = workshopIds;
              MyBlueprintUtils.SavePrefabToFile(prefab, blueprintName, currentLocalDirectory, true, type, true);
            }
            MyWorkshop.ReportPublish(publishedItems, result, resultServiceName);
          }));
        });
        if (MyWorkshop.BlueprintCategories.Length != 0)
          MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenWorkshopTags("blueprint", MyWorkshop.BlueprintCategories, (string[]) null, callback));
        else
          callback(MyGuiScreenMessageBox.ResultEnum.YES, new string[1]
          {
            "blueprint"
          }, MyGameService.GetUGCNamesList());
      }))));
    }

    public static void SavePrefabToFile(
      MyObjectBuilder_Definitions prefab,
      string name,
      string currentDirectory,
      bool replace = false,
      MyBlueprintTypeEnum type = MyBlueprintTypeEnum.LOCAL,
      bool forceLocal = false)
    {
      if (type == MyBlueprintTypeEnum.LOCAL && MySandboxGame.Config.EnableSteamCloud && (MyGameService.IsActive && !forceLocal))
        type = MyBlueprintTypeEnum.CLOUD;
      string str = string.Empty;
      switch (type)
      {
        case MyBlueprintTypeEnum.WORKSHOP:
        case MyBlueprintTypeEnum.SHARED:
        case MyBlueprintTypeEnum.DEFAULT:
          str = Path.Combine(MyBlueprintUtils.BLUEPRINT_WORKSHOP_TEMP, name);
          break;
        case MyBlueprintTypeEnum.LOCAL:
          str = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, currentDirectory, name);
          break;
        case MyBlueprintTypeEnum.CLOUD:
          str = Path.Combine("Blueprints/cloud", name);
          break;
      }
      string filePath = string.Empty;
      try
      {
        if (type == MyBlueprintTypeEnum.CLOUD)
        {
          filePath = Path.Combine(str, MyBlueprintUtils.BLUEPRINT_LOCAL_NAME);
          MyBlueprintUtils.SaveToCloud(prefab, filePath, replace);
        }
        else
          MyBlueprintUtils.SaveToDisk(prefab, name, replace, type, str, currentDirectory, ref filePath);
      }
      catch (Exception ex)
      {
        MySandboxGame.Log.WriteLine(string.Format("Failed to write prefab at file {0}, message: {1}, stack:{2}", (object) filePath, (object) ex.Message, (object) ex.StackTrace));
      }
    }

    public static void SaveToCloud(
      MyObjectBuilder_Definitions prefab,
      string filePath,
      bool replace,
      Action<string, CloudResult> onCompleted = null)
    {
      using (MemoryStream memoryStream1 = new MemoryStream())
      {
        int num = MyObjectBuilderSerializer.SerializeXML((Stream) memoryStream1, (MyObjectBuilder_Base) prefab, MyObjectBuilderSerializer.XmlCompression.Gzip) ? 1 : 0;
        if (num != 0)
        {
          byte[] array1 = memoryStream1.ToArray();
          string filePathCorrect = filePath.Replace('\\', '/');
          MyGameService.SaveToCloudAsync(filePathCorrect, array1, (Action<CloudResult>) (result =>
          {
            if (result != CloudResult.Ok)
            {
              Action<string, CloudResult> action = onCompleted;
              if (action == null)
                return;
              action(filePath, result);
            }
            else
            {
              using (MemoryStream memoryStream = new MemoryStream())
              {
                if (MyObjectBuilderSerializer.SerializePB((Stream) memoryStream, (MyObjectBuilder_Base) prefab))
                {
                  byte[] array = memoryStream.ToArray();
                  filePathCorrect += "B5";
                  CloudResult cloud = MyGameService.SaveToCloud(filePathCorrect, array);
                  if (cloud != CloudResult.Ok)
                  {
                    Action<string, CloudResult> action = onCompleted;
                    if (action == null)
                      return;
                    action(filePath, cloud);
                    return;
                  }
                }
              }
              Action<string, CloudResult> action1 = onCompleted;
              if (action1 == null)
                return;
              action1(filePath, CloudResult.Ok);
            }
          }));
        }
        if (num != 0)
          return;
        MyBlueprintUtils.ShowBlueprintSaveError();
      }
    }

    public static CloudResult SaveToCloudFile(string pathFull, string pathRel)
    {
      try
      {
        using (MemoryStream memoryStream = new MemoryStream())
        {
          using (FileStream fileStream = new FileStream(pathFull, FileMode.Open, FileAccess.Read))
          {
            fileStream.CopyTo((Stream) memoryStream);
            byte[] array = memoryStream.ToArray();
            return MyGameService.SaveToCloud(pathRel.Replace('\\', '/'), array);
          }
        }
      }
      catch (IOException ex)
      {
        return CloudResult.Failed;
      }
    }

    private static void ShowBlueprintSaveError() => MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder("There was a problem with saving blueprint/script"), messageCaption: new StringBuilder("Error")));

    public static void SaveToDisk(
      MyObjectBuilder_Definitions prefab,
      string name,
      bool replace,
      MyBlueprintTypeEnum type,
      string file,
      string currentDirectory,
      ref string filePath)
    {
      if (!replace)
      {
        int num = 1;
        while (MyFileSystem.DirectoryExists(file))
        {
          file = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, currentDirectory, name + "_" + (object) num);
          ++num;
        }
        if (num > 1)
          name += (string) (object) new StringBuilder("_" + (object) (num - 1));
      }
      filePath = Path.Combine(file, MyBlueprintUtils.BLUEPRINT_LOCAL_NAME);
      if (filePath.Length > 260)
      {
        StringBuilder messageText = new StringBuilder();
        messageText.AppendFormat(MyTexts.GetString(MySpaceTexts.BlueprintScreen_FilePathTooLong_Description), (object) filePath.Length, (object) 260, (object) filePath);
        StringBuilder messageCaption = MyTexts.Get(MySpaceTexts.BlueprintScreen_FilePathTooLong_Caption);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: messageText, messageCaption: messageCaption));
      }
      else
      {
        int num = MyObjectBuilderSerializer.SerializeXML(filePath, false, (MyObjectBuilder_Base) prefab) ? 1 : 0;
        if (num != 0 && type == MyBlueprintTypeEnum.LOCAL)
          MyObjectBuilderSerializer.SerializePB(filePath + "B5", false, (MyObjectBuilder_Base) prefab);
        if (num != 0)
          return;
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder("There was a problem with saving blueprint"), messageCaption: new StringBuilder("Error")));
        if (!Directory.Exists(file))
          return;
        Directory.Delete(file, true);
      }
    }

    public static int GetNumberOfBlocks(ref MyObjectBuilder_Definitions prefab)
    {
      int num = 0;
      foreach (MyObjectBuilder_CubeGrid cubeGrid in prefab.ShipBlueprints[0].CubeGrids)
        num += cubeGrid.CubeBlocks.Count;
      return num;
    }

    public static MyGuiControlButton CreateButton(
      MyGuiScreenDebugBase screen,
      float usableWidth,
      StringBuilder text,
      Action<MyGuiControlButton> onClick,
      bool enabled = true,
      MyStringId? tooltip = null,
      float textScale = 1f)
    {
      MyGuiControlButton guiControlButton = screen.AddButton(text, onClick);
      guiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.Rectangular;
      guiControlButton.TextScale = textScale;
      guiControlButton.Size = new Vector2(usableWidth, guiControlButton.Size.Y);
      guiControlButton.Position = guiControlButton.Position + new Vector2(-0.02f, 0.0f);
      guiControlButton.Enabled = enabled;
      if (tooltip.HasValue)
        guiControlButton.SetToolTip(tooltip.Value);
      return guiControlButton;
    }

    public static MyGuiControlButton CreateButtonString(
      MyGuiScreenDebugBase screen,
      float usableWidth,
      StringBuilder text,
      Action<MyGuiControlButton> onClick,
      bool enabled = true,
      string tooltip = null,
      float textScale = 1f)
    {
      MyGuiControlButton guiControlButton = screen.AddButton(text, onClick);
      guiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.Rectangular;
      guiControlButton.TextScale = textScale;
      guiControlButton.Size = new Vector2(usableWidth, guiControlButton.Size.Y);
      guiControlButton.Position = guiControlButton.Position + new Vector2(-0.02f, 0.0f);
      guiControlButton.Enabled = enabled;
      if (tooltip != null)
        guiControlButton.SetToolTip(tooltip);
      return guiControlButton;
    }

    public static void PublishScript(
      string localPath,
      MyBlueprintItemInfo script,
      Action OnPublished)
    {
      StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.LoadScreenButtonPublish);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MySpaceTexts.ProgrammableBlock_PublishScriptDialogText), messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (val =>
      {
        if (val != MyGuiScreenMessageBox.ResultEnum.YES)
          return;
        MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenWorkshopTags("ingameScript", MyWorkshop.ScriptCategories, (string[]) null, (Action<MyGuiScreenMessageBox.ResultEnum, string[], string[]>) ((x, y, z) => MyBlueprintUtils.OnPublishScriptTagsResult(localPath, script, OnPublished, x, y, z))));
      }))));
    }

    private static void OnPublishScriptTagsResult(
      string localPath,
      MyBlueprintItemInfo script,
      Action OnPublished,
      MyGuiScreenMessageBox.ResultEnum tagScreenResult,
      string[] outTags,
      string[] serviceNames)
    {
      if (tagScreenResult != MyGuiScreenMessageBox.ResultEnum.YES)
        return;
      string name = script.Data.Name;
      MyWorkshopItem myWorkshopItem = script.Item;
      WorkshopId additionalWorkshopId = new WorkshopId(myWorkshopItem != null ? myWorkshopItem.Id : 0UL, script.Item?.ServiceName);
      WorkshopId[] workshopIds = MyWorkshop.FilterWorkshopIds(MyWorkshop.GetWorkshopIdFromLocalScript(name, additionalWorkshopId), serviceNames);
      MyWorkshop.PublishIngameScriptAsync(localPath, script.Data.Name, script.Data.Description ?? "", workshopIds, outTags, MyPublishedFileVisibility.Public, (Action<bool, MyGameServiceCallResult, string, MyWorkshopItem[]>) ((success, result, resultServiceName, publishedFiles) =>
      {
        if (publishedFiles.Length != 0)
          MyWorkshop.GenerateModInfo(localPath, publishedFiles, Sync.MyId);
        MyWorkshop.ReportPublish(publishedFiles, result, resultServiceName, OnPublished);
      }));
    }

    public static bool IsItem_Blueprint(string path) => File.Exists(path + "\\bp.sbc");

    public static bool IsItem_Script(string path) => File.Exists(path + "\\Script.cs");

    private MyBlueprintUtils()
    {
    }

    public static void CreateModIoConsentScreen(Action onConsentAgree = null, Action onConsentOptOut = null)
    {
      MyModIoConsentViewModel consentViewModel = new MyModIoConsentViewModel(onConsentAgree, onConsentOptOut);
      ServiceManager.Instance.GetService<IMyGuiScreenFactoryService>().CreateScreen((ViewModelBase) consentViewModel);
    }

    public static void OpenBlueprintScreen(
      MyGridClipboard clipboard,
      bool allowCopyToClipboard,
      MyBlueprintAccessType accessType,
      Action<MyGuiBlueprintScreen_Reworked> onOpened)
    {
      MyGuiBlueprintScreen_Reworked blueprintScreen = MyGuiBlueprintScreen_Reworked.CreateBlueprintScreen(clipboard, allowCopyToClipboard, accessType);
      if (onOpened != null)
        onOpened(blueprintScreen);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) blueprintScreen);
    }

    public static void OpenBlueprintScreen() => MyBlueprintUtils.OpenBlueprintScreen(MyClipboardComponent.Static.Clipboard, MySession.Static.CreativeMode || MySession.Static.CreativeToolsEnabled(Sync.MyId), MyBlueprintAccessType.NORMAL, (Action<MyGuiBlueprintScreen_Reworked>) null);

    public static void OpenScriptScreen(
      Action<string> scriptSelected,
      Func<string> getCode,
      Action workshopWindowClosed)
    {
      if (MyFakes.I_AM_READY_FOR_NEW_SCRIPT_SCREEN)
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiBlueprintScreen_Reworked.CreateScriptScreen(scriptSelected, getCode, workshopWindowClosed));
      else
        MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiIngameScriptsPage(scriptSelected, getCode, workshopWindowClosed));
    }
  }
}
