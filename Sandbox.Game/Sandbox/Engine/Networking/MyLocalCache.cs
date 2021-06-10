// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Networking.MyLocalCache
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GUI;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders;
using VRage.Game.ObjectBuilders.Campaign;
using VRage.Game.ObjectBuilders.VisualScripting;
using VRage.GameServices;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Engine.Networking
{
  public class MyLocalCache
  {
    public const string CHECKPOINT_FILE = "Sandbox.sbc";
    private const string WORLD_CONFIGURATION_FILE = "Sandbox_config.sbc";
    private const string LAST_SESSION_FILE = "LastSession.sbl";
    private const string INVENTORY_FILE = "ActiveInventory.sbl";
    private static bool m_initialized;
    public static MyObjectBuilder_LastSession LastSessionOverride;

    public static string LastSessionPath => Path.Combine(MyFileSystem.SavesPath, "LastSession.sbl");

    public static string LastSessionCloudPath => "Session/cloud/LastSession.sbl";

    public static string ContentSessionsPath => "Worlds";

    private static string GetSectorPath(string sessionPath, Vector3I sectorPosition)
    {
      if (!sessionPath.EndsWith("/"))
        sessionPath += "/";
      return sessionPath + MyLocalCache.GetSectorName(sectorPosition) + ".sbs";
    }

    private static string GetSectorName(Vector3I sectorPosition) => string.Format("{0}_{1}_{2}_{3}_", (object) "SANDBOX", (object) sectorPosition.X, (object) sectorPosition.Y, (object) sectorPosition.Z);

    public static string GetSessionSavesPath(
      string sessionUniqueName,
      bool contentFolder,
      bool createIfNotExists = true,
      bool isCloud = false)
    {
      if (isCloud)
      {
        string str = "Worlds/cloud/" + sessionUniqueName;
        if (!str.EndsWith("/"))
          str += "/";
        return str;
      }
      string path = !contentFolder ? Path.Combine(MyFileSystem.SavesPath, sessionUniqueName) : Path.Combine(MyFileSystem.ContentPath, MyLocalCache.ContentSessionsPath, sessionUniqueName);
      if (createIfNotExists)
        Directory.CreateDirectory(path);
      return path;
    }

    private static MyWorldInfo LoadWorldInfoFromCloud(string containerName)
    {
      try
      {
        using (Stream stream = new MemoryStream(MyGameService.LoadFromCloud(MyCloudHelper.Combine(containerName, "Sandbox.sbc"))).UnwrapGZip())
        {
          MyWorldInfo myWorldInfo = MyLocalCache.LoadWorldInfo(stream);
          myWorldInfo.StorageSize = MyCloudHelper.GetStorageSize(containerName);
          return myWorldInfo;
        }
      }
      catch (Exception ex)
      {
        MySandboxGame.Log.WriteLine(ex);
        return new MyWorldInfo() { IsCorrupted = true };
      }
    }

    public static string GetSessionPathFromScenario(
      string path,
      bool forceConsoleCompatible,
      out bool isCompatible)
    {
      string[] files = Directory.GetFiles(path, "*.scf");
      isCompatible = true;
      MyObjectBuilder_VSFiles objectBuilder;
      return files.Length != 0 && MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_VSFiles>(files[0], out objectBuilder) ? MyLocalCache.GetSessionPathFromScenarioObjectBuilder(objectBuilder.Campaign, path, forceConsoleCompatible, out isCompatible) : (string) null;
    }

    public static string GetSessionPathFromScenarioObjectBuilder(
      MyObjectBuilder_Campaign ob,
      string path,
      bool forceConsoleCompatible,
      out bool isCompatible)
    {
      isCompatible = true;
      if (ob.SupportedPlatforms == null || ob.SupportedPlatforms.Length == 0)
        return (string) null;
      if (MyPlatformGameSettings.CONSOLE_COMPATIBLE | forceConsoleCompatible)
      {
        MyObjectBuilder_Campaign.MySupportedPlatform platform = ((IEnumerable<MyObjectBuilder_Campaign.MySupportedPlatform>) ob.SupportedPlatforms).FirstOrDefault<MyObjectBuilder_Campaign.MySupportedPlatform>((Func<MyObjectBuilder_Campaign.MySupportedPlatform, bool>) (x => x.Name == "XBox"));
        MyObjectBuilder_CampaignSM builderCampaignSm = ((IEnumerable<MyObjectBuilder_CampaignSM>) ob.StateMachines).FirstOrDefault<MyObjectBuilder_CampaignSM>((Func<MyObjectBuilder_CampaignSM, bool>) (x => x.Name == platform.StateMachine));
        if (builderCampaignSm != null)
          return Path.Combine(path, builderCampaignSm.Nodes[0].SaveFilePath);
        isCompatible = false;
        return (string) null;
      }
      MyObjectBuilder_Campaign.MySupportedPlatform platform1 = ((IEnumerable<MyObjectBuilder_Campaign.MySupportedPlatform>) ob.SupportedPlatforms).FirstOrDefault<MyObjectBuilder_Campaign.MySupportedPlatform>((Func<MyObjectBuilder_Campaign.MySupportedPlatform, bool>) (x => x.Name == "PC"));
      MyObjectBuilder_CampaignSM builderCampaignSm1 = ((IEnumerable<MyObjectBuilder_CampaignSM>) ob.StateMachines).FirstOrDefault<MyObjectBuilder_CampaignSM>((Func<MyObjectBuilder_CampaignSM, bool>) (x => x.Name == platform1.StateMachine));
      if (builderCampaignSm1 != null)
        return Path.Combine(path, builderCampaignSm1.Nodes[0].SaveFilePath);
      isCompatible = false;
      return (string) null;
    }

    public static MyWorldInfo GetWorldInfoFromScenario(
      string sessionPath,
      out bool isCompatible)
    {
      string str = sessionPath;
      if (str.ToLower().EndsWith(".sbc"))
        str = Path.GetDirectoryName(str);
      string[] files = Directory.GetFiles(str, "*.scf");
      isCompatible = true;
      MyObjectBuilder_VSFiles objectBuilder;
      if (files.Length == 0 || !MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_VSFiles>(files[0], out objectBuilder))
        return (MyWorldInfo) null;
      MyWorldInfo myWorldInfo1 = new MyWorldInfo();
      myWorldInfo1.SessionName = MyStatControlText.SubstituteTexts(objectBuilder.Campaign.Name);
      myWorldInfo1.Description = objectBuilder.Campaign.Description;
      myWorldInfo1.StorageSize = DirectoryExtensions.GetStorageSize(str);
      myWorldInfo1.SessionPath = MyLocalCache.GetSessionPathFromScenarioObjectBuilder(objectBuilder.Campaign, str, false, out isCompatible);
      if (!isCompatible)
        return (MyWorldInfo) null;
      if (!string.IsNullOrEmpty(myWorldInfo1.SessionPath))
      {
        MyWorldInfo myWorldInfo2 = MyLocalCache.LoadWorldInfoFromCheckpoint(myWorldInfo1.SessionPath);
        myWorldInfo1.IsExperimental = myWorldInfo2.IsExperimental;
      }
      MyObjectBuilder_Checkpoint builderCheckpoint = MyLocalCache.LoadCheckpoint(myWorldInfo1.SessionPath, out ulong _);
      if (builderCheckpoint != null)
      {
        MyObjectBuilder_CampaignSessionComponent ob = builderCheckpoint.SessionComponents.OfType<MyObjectBuilder_CampaignSessionComponent>().FirstOrDefault<MyObjectBuilder_CampaignSessionComponent>();
        if (ob != null)
        {
          MyWorldInfo myWorldInfo2 = myWorldInfo1;
          myWorldInfo2.IsCampaign = ((myWorldInfo2.IsCampaign ? 1 : 0) | (MyCampaignManager.Static == null ? 0 : (MyCampaignManager.Static.IsCampaign(ob) ? 1 : 0))) != 0;
        }
      }
      return myWorldInfo1;
    }

    public static MyWorldInfo LoadWorldInfoFromFile(string sessionPath)
    {
      bool isCompatible;
      MyWorldInfo infoFromScenario = MyLocalCache.GetWorldInfoFromScenario(sessionPath, out isCompatible);
      return infoFromScenario != null || !isCompatible ? infoFromScenario : MyLocalCache.LoadWorldInfoFromCheckpoint(sessionPath);
    }

    private static MyWorldInfo LoadWorldInfoFromCheckpoint(string sessionPath)
    {
      try
      {
        if (sessionPath.ToLower().EndsWith(".sbc"))
          sessionPath = Path.GetDirectoryName(sessionPath);
        string path = Path.Combine(sessionPath, "Sandbox.sbc");
        if (!File.Exists(path))
          return (MyWorldInfo) null;
        using (Stream stream = MyFileSystem.OpenRead(path).UnwrapGZip())
        {
          MyWorldInfo myWorldInfo = MyLocalCache.LoadWorldInfo(stream);
          myWorldInfo.StorageSize = DirectoryExtensions.GetStorageSize(sessionPath);
          return myWorldInfo;
        }
      }
      catch (Exception ex)
      {
        MySandboxGame.Log.WriteLine(ex);
        return new MyWorldInfo() { IsCorrupted = true };
      }
    }

    private static MyWorldInfo LoadWorldInfo(Stream stream)
    {
      MyWorldInfo myWorldInfo1 = new MyWorldInfo();
      try
      {
        XDocument xdocument = (XDocument) null;
        XmlReaderSettings settings = new XmlReaderSettings()
        {
          CheckCharacters = false
        };
        using (XmlReader reader = XmlReader.Create(stream, settings))
          xdocument = XDocument.Load(reader);
        XElement root = xdocument.Root;
        XElement xelement1 = root.Element((XName) "SessionName");
        XElement xelement2 = root.Element((XName) "Description");
        XElement xelement3 = root.Element((XName) "LastSaveTime");
        root.Element((XName) "WorldID");
        XElement xelement4 = root.Element((XName) "WorkshopId");
        XElement xelement5 = root.Element((XName) "WorkshopServiceName");
        XElement xelement6 = root.Element((XName) "WorkshopId1");
        XElement xelement7 = root.Element((XName) "WorkshopServiceName1");
        XElement xelement8 = root.Element((XName) "Briefing");
        XElement xelement9 = root.Element((XName) "Settings");
        XElement xelement10 = xelement9 != null ? root.Element((XName) "Settings").Element((XName) "ScenarioEditMode") : (XElement) null;
        XElement xelement11 = xelement9 != null ? root.Element((XName) "Settings").Element((XName) "ExperimentalMode") : (XElement) null;
        XElement xelement12 = xelement9 != null ? root.Element((XName) "Settings").Element((XName) "HasPlanets") : (XElement) null;
        XElement xelement13 = root.Element((XName) "SessionComponents");
        XElement xelement14;
        if (xelement13 == null)
        {
          xelement14 = (XElement) null;
        }
        else
        {
          IEnumerable<XElement> source = xelement13.Elements((XName) "MyObjectBuilder_SessionComponent");
          xelement14 = source != null ? source.FirstOrDefault<XElement>((Func<XElement, bool>) (e => e.FirstAttribute?.Value == "MyObjectBuilder_CampaignSessionComponent")) : (XElement) null;
        }
        XElement xelement15 = xelement14;
        XElement xelement16 = xelement15?.Element((XName) "Mod");
        if (xelement11 != null)
          bool.TryParse(xelement11.Value, out myWorldInfo1.IsExperimental);
        if (xelement1 != null)
          myWorldInfo1.SessionName = MyStatControlText.SubstituteTexts(xelement1.Value);
        if (xelement2 != null)
          myWorldInfo1.Description = xelement2.Value;
        if (xelement3 != null)
          DateTime.TryParse(xelement3.Value, out myWorldInfo1.LastSaveTime);
        List<WorkshopId> workshopIdList = new List<WorkshopId>();
        ulong result1;
        if (xelement4 != null && ulong.TryParse(xelement4.Value, out result1))
          workshopIdList.Add(new WorkshopId(result1, xelement5?.Value ?? MyGameService.GetDefaultUGC().ServiceName));
        if (xelement6 != null && ulong.TryParse(xelement6.Value, out result1))
          workshopIdList.Add(new WorkshopId(result1, xelement7.Value));
        myWorldInfo1.WorkshopIds = workshopIdList.ToArray();
        if (xelement8 != null)
          myWorldInfo1.Briefing = xelement8.Value;
        if (xelement10 != null)
          bool.TryParse(xelement10.Value, out myWorldInfo1.ScenarioEditMode);
        if (xelement12 != null)
          bool.TryParse(xelement12.Value, out myWorldInfo1.HasPlanets);
        ulong result2;
        ulong.TryParse(xelement16?.Element((XName) "PublishedFileId")?.Value, out result2);
        MyWorldInfo myWorldInfo2 = myWorldInfo1;
        int num;
        if (xelement15 != null)
        {
          MyCampaignManager myCampaignManager = MyCampaignManager.Static;
          num = myCampaignManager != null ? (myCampaignManager.IsCampaign(xelement15.Element((XName) "CampaignName")?.Value, xelement15?.Element((XName) "IsVanilla")?.Value.ToLower() == "true", result2) ? 1 : 0) : 0;
        }
        else
          num = 0;
        myWorldInfo2.IsCampaign = num != 0;
      }
      catch (Exception ex)
      {
        MySandboxGame.Log.WriteLine(ex);
        myWorldInfo1.IsCorrupted = true;
      }
      return myWorldInfo1;
    }

    public static MyObjectBuilder_Checkpoint LoadCheckpointFromCloud(
      string fileName,
      out ulong sizeInBytes,
      MyGameModeEnum? forceGameMode = null,
      MyOnlineModeEnum? forceOnlineMode = null)
    {
      sizeInBytes = 0UL;
      string str = MyCloudHelper.Combine(fileName, "Sandbox.sbc");
      byte[] xmlData = MyGameService.LoadFromCloud(str);
      if (xmlData == null)
        return (MyObjectBuilder_Checkpoint) null;
      MyObjectBuilder_Checkpoint objectBuilder;
      MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_Checkpoint>(xmlData, out objectBuilder, out sizeInBytes);
      if (objectBuilder != null && string.IsNullOrEmpty(objectBuilder.SessionName))
        objectBuilder.SessionName = Path.GetFileNameWithoutExtension(str);
      ulong sizeInBytes1;
      MyObjectBuilder_WorldConfiguration worldConfiguration = MyLocalCache.LoadWorldConfigurationFromCloud(fileName, out sizeInBytes1);
      if (worldConfiguration != null)
      {
        MyLog.Default.WriteLineAndConsole("Sandbox world configuration file found, overriding checkpoint settings.");
        objectBuilder.Settings = worldConfiguration.Settings;
        objectBuilder.Mods = worldConfiguration.Mods;
        if (!string.IsNullOrEmpty(worldConfiguration.SessionName))
          objectBuilder.SessionName = worldConfiguration.SessionName;
        sizeInBytes += sizeInBytes1;
      }
      if (objectBuilder != null)
      {
        MySession.PerformPlatformPatchBeforeLoad(objectBuilder, forceGameMode, forceOnlineMode);
        MyLocalCache.CheckExperimental(objectBuilder);
      }
      return objectBuilder;
    }

    public static MyObjectBuilder_Checkpoint LoadCheckpoint(
      string sessionDirectory,
      out ulong sizeInBytes,
      MyGameModeEnum? forceGameMode = null,
      MyOnlineModeEnum? forceOnlineMode = null)
    {
      sizeInBytes = 0UL;
      if (sessionDirectory.ToLower().EndsWith(".sbc"))
        sessionDirectory = Path.GetDirectoryName(sessionDirectory);
      string path = Path.Combine(sessionDirectory, "Sandbox.sbc");
      if (!File.Exists(path))
        return (MyObjectBuilder_Checkpoint) null;
      MyObjectBuilder_Checkpoint objectBuilder = (MyObjectBuilder_Checkpoint) null;
      MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_Checkpoint>(path, out objectBuilder, out sizeInBytes);
      if (objectBuilder != null)
      {
        if (string.IsNullOrEmpty(objectBuilder.SessionName))
          objectBuilder.SessionName = Path.GetFileNameWithoutExtension(path);
        ulong sizeInBytes1 = 0;
        MyObjectBuilder_WorldConfiguration worldConfiguration = MyLocalCache.LoadWorldConfiguration(sessionDirectory, out sizeInBytes1);
        if (worldConfiguration != null)
        {
          MyLog.Default.WriteLineAndConsole("Sandbox world configuration file found, overriding checkpoint settings.");
          objectBuilder.Settings = worldConfiguration.Settings;
          objectBuilder.Mods = worldConfiguration.Mods;
          if (!string.IsNullOrEmpty(worldConfiguration.SessionName))
            objectBuilder.SessionName = worldConfiguration.SessionName;
          sizeInBytes += sizeInBytes1;
        }
        MySession.PerformPlatformPatchBeforeLoad(objectBuilder, forceGameMode, forceOnlineMode);
        MyLocalCache.CheckExperimental(objectBuilder);
      }
      return objectBuilder;
    }

    private static void CheckExperimental(MyObjectBuilder_Checkpoint checkpoint)
    {
      MyObjectBuilder_SessionSettings settings = checkpoint.Settings;
      if (settings == null || settings.ExperimentalMode || !checkpoint.IsSettingsExperimental(false) && (MySandboxGame.ConfigDedicated == null || MySandboxGame.ConfigDedicated.Plugins == null || MySandboxGame.ConfigDedicated.Plugins.Count == 0) && (!MySandboxGame.Config.ExperimentalMode || MySandboxGame.ConfigDedicated != null))
        return;
      settings.ExperimentalMode = true;
    }

    public static MyObjectBuilder_WorldConfiguration LoadWorldConfiguration(
      string sessionPath)
    {
      ulong sizeInBytes = 0;
      MyObjectBuilder_WorldConfiguration worldConfiguration1 = MyLocalCache.LoadWorldConfiguration(sessionPath, out sizeInBytes);
      if (worldConfiguration1 != null)
        return worldConfiguration1;
      MyObjectBuilder_Checkpoint builderCheckpoint = MyLocalCache.LoadCheckpoint(sessionPath, out sizeInBytes);
      if (builderCheckpoint == null)
        return (MyObjectBuilder_WorldConfiguration) null;
      MyObjectBuilder_WorldConfiguration worldConfiguration2 = new MyObjectBuilder_WorldConfiguration();
      worldConfiguration2.LastSaveTime = new DateTime?(builderCheckpoint.LastSaveTime);
      worldConfiguration2.Mods = builderCheckpoint.Mods;
      worldConfiguration2.SessionName = builderCheckpoint.SessionName;
      worldConfiguration2.Settings = builderCheckpoint.Settings;
      worldConfiguration2.SubtypeName = builderCheckpoint.SubtypeName;
      return worldConfiguration2;
    }

    public static MyObjectBuilder_WorldConfiguration LoadWorldConfiguration(
      string sessionPath,
      out ulong sizeInBytes)
    {
      if (sessionPath.ToLower().EndsWith(".sbc"))
        sessionPath = Path.GetDirectoryName(sessionPath);
      string path = Path.Combine(sessionPath, "Sandbox_config.sbc");
      if (!File.Exists(path))
      {
        sizeInBytes = 0UL;
        return (MyObjectBuilder_WorldConfiguration) null;
      }
      MyLog.Default.WriteLineAndConsole("Loading Sandbox world configuration file " + path);
      MyObjectBuilder_WorldConfiguration objectBuilder = (MyObjectBuilder_WorldConfiguration) null;
      MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_WorldConfiguration>(path, out objectBuilder, out sizeInBytes);
      MySession.PerformPlatformPatchBeforeLoad(objectBuilder, new MyGameModeEnum?(), new MyOnlineModeEnum?());
      return objectBuilder;
    }

    private static MyObjectBuilder_WorldConfiguration LoadWorldConfigurationFromCloud(
      string fileName,
      out ulong sizeInBytes)
    {
      sizeInBytes = 0UL;
      string fileName1 = MyCloudHelper.Combine(fileName, "Sandbox_config.sbc");
      MyLog.Default.WriteLineAndConsole("Loading Sandbox world configuration file " + fileName1);
      byte[] xmlData = MyGameService.LoadFromCloud(fileName1);
      if (xmlData == null)
        return (MyObjectBuilder_WorldConfiguration) null;
      MyObjectBuilder_WorldConfiguration objectBuilder;
      MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_WorldConfiguration>(xmlData, out objectBuilder, out sizeInBytes);
      MySession.PerformPlatformPatchBeforeLoad(objectBuilder, new MyGameModeEnum?(), new MyOnlineModeEnum?());
      return objectBuilder;
    }

    public static MyObjectBuilder_Sector LoadSector(
      string sessionPath,
      Vector3I sectorPosition,
      bool allowXml,
      out ulong sizeInBytes,
      out bool needsXml)
    {
      return MyLocalCache.LoadSector(MyLocalCache.GetSectorPath(sessionPath, sectorPosition), allowXml, out sizeInBytes, out needsXml);
    }

    private static MyObjectBuilder_Sector LoadSector(
      string path,
      bool allowXml,
      out ulong sizeInBytes,
      out bool needsXml)
    {
      sizeInBytes = 0UL;
      needsXml = false;
      MyObjectBuilder_Sector objectBuilder = (MyObjectBuilder_Sector) null;
      string path1 = path + "B5";
      if (MyFileSystem.FileExists(path1))
      {
        MyObjectBuilderSerializer.DeserializePB<MyObjectBuilder_Sector>(path1, out objectBuilder, out sizeInBytes);
        if (objectBuilder == null || objectBuilder.SectorObjects == null)
        {
          if (allowXml)
          {
            MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_Sector>(path, out objectBuilder, out sizeInBytes);
            if (objectBuilder != null)
              MyObjectBuilderSerializer.SerializePB(path1, false, (MyObjectBuilder_Base) objectBuilder);
          }
          else
            needsXml = true;
        }
      }
      else if (allowXml)
      {
        MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_Sector>(path, out objectBuilder, out sizeInBytes);
        if (!MyFileSystem.FileExists(path1))
          MyObjectBuilderSerializer.SerializePB(path + "B5", false, (MyObjectBuilder_Base) objectBuilder);
      }
      else
        needsXml = true;
      if (objectBuilder != null)
        return objectBuilder;
      MySandboxGame.Log.WriteLine("Incorrect save data");
      return (MyObjectBuilder_Sector) null;
    }

    public static MyObjectBuilder_CubeGrid LoadCubeGrid(
      string sessionPath,
      string fileName,
      out ulong sizeInBytes)
    {
      MyObjectBuilder_CubeGrid objectBuilder;
      MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_CubeGrid>(Path.Combine(sessionPath, fileName), out objectBuilder, out sizeInBytes);
      if (objectBuilder != null)
        return objectBuilder;
      MySandboxGame.Log.WriteLine("Incorrect save data");
      return (MyObjectBuilder_CubeGrid) null;
    }

    public static bool SaveSector(
      MyObjectBuilder_Sector sector,
      string sessionPath,
      Vector3I sectorPosition,
      out ulong sizeInBytes,
      List<MyCloudFile> fileList)
    {
      string sectorPath = MyLocalCache.GetSectorPath(sessionPath, sectorPosition);
      int num = MyObjectBuilderSerializer.SerializeXML(sectorPath, MyPlatformGameSettings.GAME_SAVES_COMPRESSED_BY_DEFAULT, (MyObjectBuilder_Base) sector, out sizeInBytes) ? 1 : 0;
      fileList.Add(new MyCloudFile(sectorPath));
      string str = sectorPath + "B5";
      MyObjectBuilderSerializer.SerializePB(str, MyPlatformGameSettings.GAME_SAVES_COMPRESSED_BY_DEFAULT, (MyObjectBuilder_Base) sector, out sizeInBytes);
      fileList.Add(new MyCloudFile(str));
      return num != 0;
    }

    public static CloudResult SaveCheckpointToCloud(
      MyObjectBuilder_Checkpoint checkpoint,
      string cloudPath)
    {
      List<MyCloudFile> myCloudFileList = new List<MyCloudFile>();
      MyLocalCache.SaveCheckpoint(checkpoint, MyFileSystem.TempPath, myCloudFileList);
      return MyGameService.SaveToCloud(cloudPath, myCloudFileList);
    }

    public static bool SaveCheckpoint(
      MyObjectBuilder_Checkpoint checkpoint,
      string sessionPath,
      List<MyCloudFile> fileList = null)
    {
      return MyLocalCache.SaveCheckpoint(checkpoint, sessionPath, out ulong _, fileList);
    }

    public static bool SaveCheckpoint(
      MyObjectBuilder_Checkpoint checkpoint,
      string sessionPath,
      out ulong sizeInBytes,
      List<MyCloudFile> fileList)
    {
      string str = Path.Combine(sessionPath, "Sandbox.sbc");
      int num1 = MyObjectBuilderSerializer.SerializeXML(str, MyPlatformGameSettings.GAME_SAVES_COMPRESSED_BY_DEFAULT, (MyObjectBuilder_Base) checkpoint, out sizeInBytes) ? 1 : 0;
      fileList?.Add(new MyCloudFile(str));
      MyObjectBuilder_WorldConfiguration newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_WorldConfiguration>();
      newObject.Settings = checkpoint.Settings;
      newObject.Mods = checkpoint.Mods;
      newObject.SessionName = checkpoint.SessionName;
      newObject.LastSaveTime = new DateTime?(checkpoint.LastSaveTime);
      ulong sizeInBytes1 = 0;
      int num2 = MyLocalCache.SaveWorldConfiguration(newObject, sessionPath, out sizeInBytes1, fileList) ? 1 : 0;
      int num3 = num1 & num2;
      sizeInBytes += sizeInBytes1;
      return num3 != 0;
    }

    public static bool SaveWorldConfiguration(
      MyObjectBuilder_WorldConfiguration configuration,
      string sessionPath,
      List<MyCloudFile> fileList)
    {
      return MyLocalCache.SaveWorldConfiguration(configuration, sessionPath, out ulong _, fileList);
    }

    private static bool SaveWorldConfiguration(
      MyObjectBuilder_WorldConfiguration configuration,
      string sessionPath,
      out ulong sizeInBytes,
      List<MyCloudFile> fileList)
    {
      string str = Path.Combine(sessionPath, "Sandbox_config.sbc");
      MyLog.Default.WriteLineAndConsole("Saving Sandbox world configuration file " + str);
      fileList?.Add(new MyCloudFile(str));
      return MyObjectBuilderSerializer.SerializeXML(str, false, (MyObjectBuilder_Base) configuration, out sizeInBytes);
    }

    public static bool SaveRespawnShip(
      MyObjectBuilder_CubeGrid cubegrid,
      string sessionPath,
      string fileName,
      out ulong sizeInBytes)
    {
      return MyObjectBuilderSerializer.SerializeXML(Path.Combine(sessionPath, fileName), MyPlatformGameSettings.GAME_SAVES_COMPRESSED_BY_DEFAULT, (MyObjectBuilder_Base) cubegrid, out sizeInBytes);
    }

    public static List<Tuple<string, MyWorldInfo>> GetAvailableWorldInfos(
      List<string> customPaths = null)
    {
      MySandboxGame.Log.WriteLine("Loading available saves - START");
      List<Tuple<string, MyWorldInfo>> result = new List<Tuple<string, MyWorldInfo>>();
      using (MySandboxGame.Log.IndentUsing(LoggingOptions.ALL))
      {
        if (customPaths == null)
        {
          MyLocalCache.GetWorldInfoFromDirectory(MyFileSystem.SavesPath, result);
        }
        else
        {
          foreach (string customPath in customPaths)
            MyLocalCache.GetWorldInfoFromDirectory(customPath, result);
        }
      }
      MySandboxGame.Log.WriteLine("Loading available saves - END");
      return result;
    }

    public static List<Tuple<string, MyWorldInfo>> GetAvailableWorldInfosFromCloud(
      List<string> customPaths = null)
    {
      MySandboxGame.Log.WriteLine("Loading available saves - START");
      List<Tuple<string, MyWorldInfo>> tupleList = new List<Tuple<string, MyWorldInfo>>();
      using (MySandboxGame.Log.IndentUsing(LoggingOptions.ALL))
      {
        if (customPaths == null)
        {
          MyLocalCache.GetWorldInfoFromCloud(string.Empty, tupleList);
        }
        else
        {
          foreach (string customPath in customPaths)
            MyLocalCache.GetWorldInfoFromCloud(customPath, tupleList);
        }
      }
      List<Tuple<string, MyWorldInfo>> list = tupleList.Distinct<Tuple<string, MyWorldInfo>>((IEqualityComparer<Tuple<string, MyWorldInfo>>) MyLocalCache.SameCloudWorldComparer.Static).ToList<Tuple<string, MyWorldInfo>>();
      MySandboxGame.Log.WriteLine("Loading available saves - END");
      return list;
    }

    private static List<Tuple<string, MyWorldInfo>> GetAvailableInfosFromDirectory(
      string worldCategory,
      string worldDirectoryPath)
    {
      string str = "Loading available " + worldCategory;
      MySandboxGame.Log.WriteLine(str + " - START");
      List<Tuple<string, MyWorldInfo>> result = new List<Tuple<string, MyWorldInfo>>();
      using (MySandboxGame.Log.IndentUsing(LoggingOptions.ALL))
        MyLocalCache.GetWorldInfoFromDirectory(Path.Combine(MyFileSystem.ContentPath, worldDirectoryPath), result);
      MySandboxGame.Log.WriteLine(str + " - END");
      return result;
    }

    public static void GetWorldInfoFromDirectory(
      string path,
      List<Tuple<string, MyWorldInfo>> result)
    {
      bool flag = Directory.Exists(path);
      MySandboxGame.Log.WriteLine(string.Format("GetWorldInfoFromDirectory (Exists: {0}) '{1}'", (object) flag, (object) path));
      if (!flag)
        return;
      foreach (string directory in Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly))
      {
        MyWorldInfo myWorldInfo = MyLocalCache.LoadWorldInfoFromFile(directory);
        string str = directory;
        if (myWorldInfo != null)
        {
          if (string.IsNullOrEmpty(myWorldInfo.SessionName))
            myWorldInfo.SessionName = Path.GetFileName(directory);
          if (!string.IsNullOrEmpty(myWorldInfo.SessionPath))
            str = myWorldInfo.SessionPath;
        }
        result.Add(Tuple.Create<string, MyWorldInfo>(str, myWorldInfo));
      }
    }

    public static void GetWorldInfoFromCloud(string path, List<Tuple<string, MyWorldInfo>> result)
    {
      MySandboxGame.Log.WriteLine(string.Format("GetWorldInfoFromCloud '{0}'", (object) path));
      List<MyCloudFileInfo> cloudFiles = MyGameService.GetCloudFiles(MyLocalCache.GetSessionSavesPath(path, false, false, true));
      if (cloudFiles == null)
        return;
      foreach (MyCloudFileInfo myCloudFileInfo in cloudFiles)
      {
        if (myCloudFileInfo.Name.EndsWith("Sandbox.sbc"))
        {
          string str = myCloudFileInfo.Name.Replace("Sandbox.sbc", "");
          MyWorldInfo myWorldInfo = MyLocalCache.LoadWorldInfoFromCloud(str);
          if (myWorldInfo != null && string.IsNullOrEmpty(myWorldInfo.SessionName))
            myWorldInfo.SessionName = Path.GetFileName(str);
          result.Add(Tuple.Create<string, MyWorldInfo>(str, myWorldInfo));
        }
      }
    }

    public static string GetLastSessionPath() => MyLocalCache.CheckLastSession(MyLocalCache.GetLastSession());

    private static string CheckLastSession(MyObjectBuilder_LastSession lastSession)
    {
      if (lastSession == null)
        return (string) null;
      if (!string.IsNullOrEmpty(lastSession.Path))
      {
        string path = Path.Combine(lastSession.IsContentWorlds ? MyFileSystem.ContentPath : MyFileSystem.SavesPath, lastSession.Path);
        if (MyPlatformGameSettings.GAME_SAVES_TO_CLOUD || Directory.Exists(path))
          return path;
      }
      return (string) null;
    }

    public static void UpdateLastSessionFromCloud()
    {
      if (!MyPlatformGameSettings.GAME_LAST_SESSION_TO_CLOUD)
        return;
      byte[] bytes = MyGameService.LoadFromCloud(MyLocalCache.LastSessionCloudPath);
      if (bytes != null)
        File.WriteAllBytes(MyLocalCache.LastSessionPath, bytes);
      else
        File.Delete(MyLocalCache.LastSessionPath);
    }

    public static MyObjectBuilder_LastSession GetLastSession()
    {
      if (MyLocalCache.LastSessionOverride != null && MyLocalCache.CheckLastSession(MyLocalCache.LastSessionOverride) != null)
        return MyLocalCache.LastSessionOverride;
      if (!File.Exists(MyLocalCache.LastSessionPath))
        return (MyObjectBuilder_LastSession) null;
      MyObjectBuilder_LastSession objectBuilder = (MyObjectBuilder_LastSession) null;
      MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_LastSession>(MyLocalCache.LastSessionPath, out objectBuilder);
      return objectBuilder;
    }

    public static bool SaveLastSessionInfo(
      string sessionPath,
      bool isOnline,
      bool isLobby,
      string gameName,
      string serverIP,
      int serverPort)
    {
      MyObjectBuilder_LastSession newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_LastSession>();
      newObject.IsOnline = isOnline;
      newObject.IsLobby = isLobby;
      if (isOnline)
      {
        if (isLobby)
        {
          newObject.GameName = gameName;
          newObject.ServerIP = serverIP;
        }
        else
        {
          newObject.GameName = gameName;
          newObject.ServerIP = serverIP;
          newObject.ServerPort = serverPort;
        }
      }
      else if (sessionPath != null)
      {
        newObject.Path = sessionPath;
        newObject.GameName = gameName;
        newObject.IsContentWorlds = sessionPath.StartsWith(MyFileSystem.ContentPath, StringComparison.InvariantCultureIgnoreCase);
      }
      int num = MyObjectBuilderSerializer.SerializeXML(MyLocalCache.LastSessionPath, false, (MyObjectBuilder_Base) newObject, out ulong _) ? 1 : 0;
      if (num == 0)
        return num != 0;
      if (!MyPlatformGameSettings.GAME_LAST_SESSION_TO_CLOUD)
        return num != 0;
      int cloud = (int) MyGameService.SaveToCloud(MyLocalCache.LastSessionCloudPath, File.ReadAllBytes(MyLocalCache.LastSessionPath));
      return num != 0;
    }

    public static bool SaveLastSessionInfo(
      string sessionPath,
      bool isOnline,
      bool isLobby,
      string gameName,
      string serverConnectionString)
    {
      MyObjectBuilder_LastSession newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_LastSession>();
      newObject.IsOnline = isOnline;
      newObject.IsLobby = isLobby;
      if (isOnline)
      {
        newObject.GameName = gameName;
        newObject.ConnectionString = serverConnectionString;
      }
      else if (sessionPath != null)
      {
        newObject.Path = sessionPath;
        newObject.GameName = gameName;
        newObject.IsContentWorlds = sessionPath.StartsWith(MyFileSystem.ContentPath, StringComparison.InvariantCultureIgnoreCase);
      }
      int num = MyObjectBuilderSerializer.SerializeXML(MyLocalCache.LastSessionPath, false, (MyObjectBuilder_Base) newObject, out ulong _) ? 1 : 0;
      if (num == 0)
        return num != 0;
      if (!MyPlatformGameSettings.GAME_LAST_SESSION_TO_CLOUD)
        return num != 0;
      int cloud = (int) MyGameService.SaveToCloud(MyLocalCache.LastSessionCloudPath, File.ReadAllBytes(MyLocalCache.LastSessionPath));
      return num != 0;
    }

    public static void ClearLastSessionInfo()
    {
      string lastSessionPath = MyLocalCache.LastSessionPath;
      if (File.Exists(lastSessionPath))
        File.Delete(lastSessionPath);
      if (!MyPlatformGameSettings.GAME_LAST_SESSION_TO_CLOUD)
        return;
      MyGameService.DeleteFromCloud(MyLocalCache.LastSessionCloudPath);
    }

    private static string GetInventoryFile(MyCharacter character) => MyLocalCache.GetInventoryFile(MySession.Static.SavedCharacters.ToList<MyCharacter>().IndexOf(character));

    private static string GetInventoryFile(int index)
    {
      string withoutExtension = Path.GetFileNameWithoutExtension("ActiveInventory.sbl");
      string path1 = MyFileSystem.SavesPath;
      if (index > 0)
      {
        withoutExtension += index.ToString();
        path1 = MySession.Static.CurrentPath;
      }
      string path2 = withoutExtension + Path.GetExtension("ActiveInventory.sbl");
      return Path.Combine(path1, path2);
    }

    private static MyObjectBuilder_SkinInventory GetInventoryBuilder(
      string filename)
    {
      if (!MyFileSystem.FileExists(filename))
        return (MyObjectBuilder_SkinInventory) null;
      MyObjectBuilder_SkinInventory objectBuilder;
      return !MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_SkinInventory>(filename, out objectBuilder) ? (MyObjectBuilder_SkinInventory) null : objectBuilder;
    }

    public static void PreloadLocalInventoryConfig()
    {
      if (!MyGameService.IsActive)
        return;
      List<MyGameInventoryItem> gameInventoryItemList = new List<MyGameInventoryItem>();
      try
      {
        MyObjectBuilder_SkinInventory inventoryBuilder = MyLocalCache.GetInventoryBuilder(MyLocalCache.GetInventoryFile(0));
        if (inventoryBuilder == null)
          return;
        if (inventoryBuilder.Character != null)
        {
          foreach (ulong num in inventoryBuilder.Character)
          {
            ulong itemId = num;
            MyGameInventoryItem gameInventoryItem = MyGameService.InventoryItems.FirstOrDefault<MyGameInventoryItem>((Func<MyGameInventoryItem, bool>) (i => (long) i.ID == (long) itemId));
            if (gameInventoryItem != null)
              gameInventoryItemList.Add(gameInventoryItem);
          }
        }
        if (inventoryBuilder.Tools != null)
        {
          foreach (ulong tool in inventoryBuilder.Tools)
          {
            ulong itemId = tool;
            MyGameInventoryItem gameInventoryItem = MyGameService.InventoryItems.FirstOrDefault<MyGameInventoryItem>((Func<MyGameInventoryItem, bool>) (i => (long) i.ID == (long) itemId));
            if (gameInventoryItem != null)
              gameInventoryItemList.Add(gameInventoryItem);
          }
        }
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLine(ex);
        return;
      }
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      List<string> stringList3 = new List<string>();
      List<string> stringList4 = new List<string>();
      foreach (MyGameInventoryItem gameInventoryItem in gameInventoryItemList)
      {
        foreach (KeyValuePair<string, MyTextureChange> skinTextureChange in MyDefinitionManager.Static.GetAssetModifierDefinitionForRender(gameInventoryItem.ItemDefinition.AssetModifierId).SkinTextureChanges)
        {
          if (!string.IsNullOrEmpty(skinTextureChange.Value.ColorMetalFileName))
            stringList1.Add(skinTextureChange.Value.ColorMetalFileName);
          if (!string.IsNullOrEmpty(skinTextureChange.Value.NormalGlossFileName))
            stringList2.Add(skinTextureChange.Value.NormalGlossFileName);
          if (!string.IsNullOrEmpty(skinTextureChange.Value.ExtensionsFileName))
            stringList3.Add(skinTextureChange.Value.ExtensionsFileName);
          if (!string.IsNullOrEmpty(skinTextureChange.Value.AlphamaskFileName))
            stringList4.Add(skinTextureChange.Value.AlphamaskFileName);
        }
      }
      if (stringList1.Count > 0)
        MyRenderProxy.PreloadTextures((IEnumerable<string>) stringList1, TextureType.ColorMetal);
      if (stringList2.Count > 0)
        MyRenderProxy.PreloadTextures((IEnumerable<string>) stringList2, TextureType.NormalGloss);
      if (stringList3.Count > 0)
        MyRenderProxy.PreloadTextures((IEnumerable<string>) stringList3, TextureType.Extensions);
      if (stringList4.Count <= 0)
        return;
      MyRenderProxy.PreloadTextures((IEnumerable<string>) stringList4, TextureType.AlphaMask);
    }

    public static void LoadInventoryConfig(MyCharacter character, bool setModel = true, bool setColor = true)
    {
      if (character == null)
        throw new ArgumentNullException(nameof (character));
      if (!MyGameService.IsActive)
        return;
      MyObjectBuilder_SkinInventory inventoryBuilder = MyLocalCache.GetInventoryBuilder(MyLocalCache.GetInventoryFile(character));
      if (inventoryBuilder == null)
      {
        MyLocalCache.ResetAllInventorySlots(character);
      }
      else
      {
        if (inventoryBuilder.Character != null && MyGameService.InventoryItems != null)
        {
          List<MyGameInventoryItem> items = new List<MyGameInventoryItem>();
          List<MyGameInventoryItemSlot> list = Enum.GetValues(typeof (MyGameInventoryItemSlot)).Cast<MyGameInventoryItemSlot>().ToList<MyGameInventoryItemSlot>();
          list.Remove(MyGameInventoryItemSlot.None);
          foreach (ulong num in inventoryBuilder.Character)
          {
            ulong itemId = num;
            MyGameInventoryItem gameInventoryItem = MyGameService.InventoryItems.FirstOrDefault<MyGameInventoryItem>((Func<MyGameInventoryItem, bool>) (i => (long) i.ID == (long) itemId));
            if (gameInventoryItem != null)
            {
              gameInventoryItem.UsingCharacters.Add(character.EntityId);
              items.Add(gameInventoryItem);
              list.Remove(gameInventoryItem.ItemDefinition.ItemSlot);
            }
          }
          MyAssetModifierComponent comp;
          if (character.Components.TryGet<MyAssetModifierComponent>(out comp))
          {
            MyGameService.GetItemsCheckData(items, (Action<byte[]>) (checkDataResult => comp.TryAddAssetModifier(checkDataResult)));
            foreach (MyGameInventoryItemSlot slot in list)
              comp.ResetSlot(slot);
          }
        }
        else
          MyLocalCache.ResetAllInventorySlots(character);
        if (setModel && !string.IsNullOrEmpty(inventoryBuilder.Model))
          character.ModelName = inventoryBuilder.Model;
        if (!setColor)
          return;
        character.ColorMask = (Vector3) inventoryBuilder.Color;
      }
    }

    public static void ResetAllInventorySlots(MyCharacter character)
    {
      MyAssetModifierComponent component;
      if (!character.Components.TryGet<MyAssetModifierComponent>(out component))
        return;
      foreach (MyGameInventoryItemSlot slot in Enum.GetValues(typeof (MyGameInventoryItemSlot)))
      {
        if (slot != MyGameInventoryItemSlot.None)
          component.ResetSlot(slot);
      }
    }

    public static void LoadInventoryConfig(
      MyCharacter character,
      MyEntity toolEntity,
      MyAssetModifierComponent skinComponent)
    {
      if (toolEntity == null)
        throw new ArgumentNullException(nameof (toolEntity));
      if (skinComponent == null)
        throw new ArgumentNullException(nameof (skinComponent));
      if (!MyGameService.IsActive)
        return;
      string inventoryFile = MyLocalCache.GetInventoryFile(character);
      MyObjectBuilder_SkinInventory objectBuilder;
      if (!MyFileSystem.FileExists(inventoryFile) || !MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_SkinInventory>(inventoryFile, out objectBuilder) || (objectBuilder.Tools == null || MyGameService.InventoryItems == null))
        return;
      IMyHandheldGunObject<MyDeviceBase> handheldGunObject = toolEntity as IMyHandheldGunObject<MyDeviceBase>;
      MyPhysicalItemDefinition physicalItemDefinition = handheldGunObject.PhysicalItemDefinition;
      MyGameInventoryItemSlot inventoryItemSlot = MyGameInventoryItemSlot.None;
      switch (handheldGunObject)
      {
        case MyHandDrill _:
          inventoryItemSlot = MyGameInventoryItemSlot.Drill;
          break;
        case MyAutomaticRifleGun _:
          inventoryItemSlot = MyGameInventoryItemSlot.Rifle;
          break;
        case MyWelder _:
          inventoryItemSlot = MyGameInventoryItemSlot.Welder;
          break;
        case MyAngleGrinder _:
          inventoryItemSlot = MyGameInventoryItemSlot.Grinder;
          break;
      }
      if (inventoryItemSlot == MyGameInventoryItemSlot.None)
        return;
      List<MyGameInventoryItem> items = new List<MyGameInventoryItem>();
      foreach (ulong tool in objectBuilder.Tools)
      {
        ulong itemId = tool;
        MyGameInventoryItem gameInventoryItem = MyGameService.InventoryItems.FirstOrDefault<MyGameInventoryItem>((Func<MyGameInventoryItem, bool>) (i => (long) i.ID == (long) itemId));
        if (gameInventoryItem != null && physicalItemDefinition != null && (physicalItemDefinition == null || gameInventoryItem.ItemDefinition.ItemSlot == inventoryItemSlot))
        {
          gameInventoryItem.UsingCharacters.Add(character.EntityId);
          items.Add(gameInventoryItem);
        }
      }
      MyGameService.GetItemsCheckData(items, (Action<byte[]>) (checkDataResult => skinComponent.TryAddAssetModifier(checkDataResult)));
    }

    public static bool GetCharacterInfoFromInventoryConfig(ref string model, ref Color color)
    {
      if (!MyGameService.IsActive)
        return false;
      string path = Path.Combine(MyFileSystem.SavesPath, "ActiveInventory.sbl");
      MyObjectBuilder_SkinInventory objectBuilder;
      if (!MyFileSystem.FileExists(path) || !MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_SkinInventory>(path, out objectBuilder))
        return false;
      model = objectBuilder.Model;
      color = new Color(objectBuilder.Color.X, objectBuilder.Color.Y, objectBuilder.Color.Z);
      return true;
    }

    public static void SaveInventoryConfig(MyCharacter character)
    {
      if (character == null || !MyGameService.IsActive)
        return;
      MyObjectBuilder_SkinInventory newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_SkinInventory>();
      newObject.Character = new List<ulong>();
      newObject.Color = (SerializableVector3) character.ColorMask;
      newObject.Model = character.ModelName;
      newObject.Tools = new List<ulong>();
      if (MyGameService.InventoryItems != null)
      {
        foreach (MyGameInventoryItem inventoryItem in (IEnumerable<MyGameInventoryItem>) MyGameService.InventoryItems)
        {
          if (inventoryItem.UsingCharacters.Contains(character.EntityId))
          {
            switch (inventoryItem.ItemDefinition.ItemSlot)
            {
              case MyGameInventoryItemSlot.None:
              case MyGameInventoryItemSlot.Face:
              case MyGameInventoryItemSlot.Helmet:
              case MyGameInventoryItemSlot.Gloves:
              case MyGameInventoryItemSlot.Boots:
              case MyGameInventoryItemSlot.Suit:
                newObject.Character.Add(inventoryItem.ID);
                continue;
              case MyGameInventoryItemSlot.Rifle:
              case MyGameInventoryItemSlot.Welder:
              case MyGameInventoryItemSlot.Grinder:
              case MyGameInventoryItemSlot.Drill:
                newObject.Tools.Add(inventoryItem.ID);
                continue;
              default:
                continue;
            }
          }
        }
      }
      MyObjectBuilderSerializer.SerializeXML(MyLocalCache.GetInventoryFile(character), false, (MyObjectBuilder_Base) newObject, out ulong _);
    }

    private class SameWorldComparer : EqualityComparer<Tuple<string, MyWorldInfo>>
    {
      private static MyLocalCache.SameWorldComparer m_static;

      public static MyLocalCache.SameWorldComparer Static
      {
        get
        {
          if (MyLocalCache.SameWorldComparer.m_static == null)
            MyLocalCache.SameWorldComparer.m_static = new MyLocalCache.SameWorldComparer();
          return MyLocalCache.SameWorldComparer.m_static;
        }
      }

      public override bool Equals(Tuple<string, MyWorldInfo> t1, Tuple<string, MyWorldInfo> t2) => Path.GetFileName(t1.Item1).Equals(Path.GetFileName(t2.Item1));

      public override int GetHashCode(Tuple<string, MyWorldInfo> t) => Path.GetFileName(t.Item1).GetHashCode();
    }

    private class SameCloudWorldComparer : EqualityComparer<Tuple<string, MyWorldInfo>>
    {
      private static MyLocalCache.SameCloudWorldComparer m_static;

      public static MyLocalCache.SameCloudWorldComparer Static
      {
        get
        {
          if (MyLocalCache.SameCloudWorldComparer.m_static == null)
            MyLocalCache.SameCloudWorldComparer.m_static = new MyLocalCache.SameCloudWorldComparer();
          return MyLocalCache.SameCloudWorldComparer.m_static;
        }
      }

      public override bool Equals(Tuple<string, MyWorldInfo> t1, Tuple<string, MyWorldInfo> t2) => t1.Item1.Equals(t2.Item1);

      public override int GetHashCode(Tuple<string, MyWorldInfo> t) => Path.GetFileName(t.Item1).GetHashCode();
    }
  }
}
