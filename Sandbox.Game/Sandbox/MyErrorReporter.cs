// Decompiled with JetBrains decompiler
// Type: Sandbox.MyErrorReporter
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using LitJson;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Http;
using VRage.Utils;
using VRageRender;

namespace Sandbox
{
  public class MyErrorReporter
  {
    public static string SUPPORT_EMAIL = "support@keenswh.com";
    public static string MESSAGE_BOX_CAPTION = "{LOCG:Error_Message_Caption}";
    public static string APP_ALREADY_RUNNING = "{LOCG:Error_AlreadyRunning}";
    public static string APP_ERROR_CAPTION = "{LOCG:Error_Error_Caption}";
    public static string APP_LOG_REPORT_FAILED = "{LOCG:Error_Failed}";
    public static string APP_LOG_REPORT_THANK_YOU = "{LOCG:Error_ThankYou}";
    public static string APP_ERROR_MESSAGE = "{LOCG:Error_Error_Message}";
    public static string APP_ERROR_MESSAGE_DX11_NOT_AVAILABLE = "{LOCG:Error_DX11}";
    public static string APP_ERROR_MESSAGE_LOW_GPU = "{LOCG:Error_GPU_Low}";
    public static string APP_ERROR_MESSAGE_NOT_DX11_GPU = "{LOCG:Error_GPU_NotDX11}";
    public static string APP_ERROR_MESSAGE_DRIVER_NOT_INSTALLED = "{LOCG:Error_GPU_Drivers}";
    public static string APP_WARNING_MESSAGE_OLD_DRIVER = "{LOCG:Error_GPU_OldDriver}";
    public static string APP_WARNING_MESSAGE_UNSUPPORTED_GPU = "{LOCG:Error_GPU_Unsupported}";
    public static string APP_ERROR_OUT_OF_MEMORY = "{LOCG:Error_OutOfMemmory}";
    public static string APP_ERROR_OUT_OF_VIDEO_MEMORY = "{LOCG:Error_GPU_OutOfMemory}";

    private static bool AllowSendDialog(string gameName, string logfile, string errorMessage) => MyMessageBox.Show(string.Format(errorMessage, (object) gameName, (object) logfile), gameName, MessageBoxOptions.IconExclamation | MessageBoxOptions.YesNo) == MessageBoxResult.Yes;

    public static void ReportRendererCrash(
      string logfile,
      string gameName,
      string minimumRequirementsPage,
      MyRenderExceptionEnum type)
    {
      string format;
      switch (type)
      {
        case MyRenderExceptionEnum.DriverNotInstalled:
          format = MyTexts.SubstituteTexts(MyErrorReporter.APP_ERROR_MESSAGE_DRIVER_NOT_INSTALLED).ToString().Replace("\\n", "\r\n");
          break;
        case MyRenderExceptionEnum.GpuNotSupported:
          format = MyTexts.SubstituteTexts(MyErrorReporter.APP_ERROR_MESSAGE_LOW_GPU).ToString().Replace("\\n", "\r\n");
          break;
        default:
          format = MyTexts.SubstituteTexts(MyErrorReporter.APP_ERROR_MESSAGE_LOW_GPU).ToString().Replace("\\n", "\r\n");
          break;
      }
      int num = (int) MyMessageBox.Show(string.Format(format, (object) logfile, (object) gameName, (object) minimumRequirementsPage), gameName, MessageBoxOptions.IconExclamation);
    }

    public static MessageBoxResult ReportOldDrivers(
      string gameName,
      string cardName,
      string driverUpdateLink)
    {
      return MyMessageBox.Show(string.Format(MyTexts.SubstituteTexts(MyErrorReporter.APP_WARNING_MESSAGE_OLD_DRIVER).ToString().Replace("\\n", "\r\n"), (object) gameName, (object) cardName, (object) driverUpdateLink), gameName, MessageBoxOptions.YesNoCancel | MessageBoxOptions.IconExclamation);
    }

    public static void ReportNotCompatibleGPU(
      string gameName,
      string logfile,
      string minimumRequirementsPage)
    {
      int num = (int) MyMessageBox.Show(string.Format(MyTexts.SubstituteTexts(MyErrorReporter.APP_WARNING_MESSAGE_UNSUPPORTED_GPU).ToString().Replace("\\n", "\r\n"), (object) logfile, (object) gameName, (object) minimumRequirementsPage), gameName, MessageBoxOptions.IconExclamation);
    }

    public static void ReportNotDX11GPUCrash(
      string gameName,
      string logfile,
      string minimumRequirementsPage)
    {
      int num = (int) MyMessageBox.Show(string.Format(MyTexts.SubstituteTexts(MyErrorReporter.APP_ERROR_MESSAGE_NOT_DX11_GPU).ToString().Replace("\\n", "\r\n"), (object) logfile, (object) gameName, (object) minimumRequirementsPage), gameName, MessageBoxOptions.IconExclamation);
    }

    public static void ReportGpuUnderMinimumCrash(
      string gameName,
      string logfile,
      string minimumRequirementsPage)
    {
      int num = (int) MyMessageBox.Show(string.Format(MyTexts.SubstituteTexts(MyErrorReporter.APP_ERROR_MESSAGE_LOW_GPU).ToString().Replace("\\n", "\r\n"), (object) logfile, (object) gameName, (object) minimumRequirementsPage), gameName, MessageBoxOptions.IconExclamation);
    }

    public static void ReportOutOfMemory(
      string gameName,
      string logfile,
      string minimumRequirementsPage)
    {
      int num = (int) MyMessageBox.Show(string.Format(MyTexts.SubstituteTexts(MyErrorReporter.APP_ERROR_OUT_OF_MEMORY).ToString().Replace("\\n", "\r\n"), (object) logfile, (object) gameName, (object) minimumRequirementsPage), gameName, MessageBoxOptions.IconExclamation);
    }

    public static void ReportOutOfVideoMemory(
      string gameName,
      string logfile,
      string minimumRequirementsPage)
    {
      int num = (int) MyMessageBox.Show(string.Format(MyTexts.SubstituteTexts(MyErrorReporter.APP_ERROR_OUT_OF_VIDEO_MEMORY).ToString().Replace("\\n", "\r\n"), (object) logfile, (object) gameName, (object) minimumRequirementsPage), gameName, MessageBoxOptions.IconExclamation);
    }

    private static void MessageBox(string caption, string text)
    {
      int num = (int) MyMessageBox.Show(text, caption, MessageBoxOptions.OkOnly);
    }

    private static bool DisplayCommonError(string logContent)
    {
      foreach (ErrorInfo info in MyErrorTexts.Infos)
      {
        if (logContent.Contains(info.Match))
        {
          MyErrorReporter.MessageBox(info.Caption, info.Message);
          return true;
        }
      }
      return false;
    }

    private static bool LoadAndDisplayCommonError(string logName)
    {
      try
      {
        if (logName != null)
        {
          if (System.IO.File.Exists(logName))
          {
            using (FileStream fileStream = System.IO.File.Open(logName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
              using (StreamReader streamReader = new StreamReader((Stream) fileStream))
                return MyErrorReporter.DisplayCommonError(streamReader.ReadToEnd());
            }
          }
        }
      }
      catch
      {
      }
      return false;
    }

    private static void ReportInternal(
      string logName,
      string id,
      CrashInfo info,
      string email,
      string feedback)
    {
      if (MyErrorReporter.TrySendReport(logName, id, email, feedback, info, "crash"))
        MyErrorReporter.MessageBox(info.GameName, MyTexts.SubstituteTexts(MyErrorReporter.APP_LOG_REPORT_THANK_YOU).Replace("\\n", "\r\n"));
      else
        MyErrorReporter.MessageBox(string.Format(MyTexts.SubstituteTexts(MyErrorReporter.APP_ERROR_CAPTION).Replace("\\n", "\r\n"), (object) info.GameName), string.Format(MyTexts.SubstituteTexts(MyErrorReporter.APP_LOG_REPORT_FAILED).Replace("\\n", "\r\n"), (object) info.GameName, (object) logName, (object) MyTexts.SubstituteTexts(MyErrorReporter.SUPPORT_EMAIL)));
    }

    private static HashSet<string> EnumerateLogs()
    {
      HashSet<string> stringSet = new HashSet<string>();
      IEnumerable<string> strings = MyVRage.Platform.CrashReporting.AdditionalReportFiles();
      if (strings != null)
      {
        foreach (string str in strings)
          stringSet.Add(str);
      }
      FileInfo fileInfo = (FileInfo) null;
      foreach (FileInfo enumerateFile in new DirectoryInfo(MyFileSystem.UserDataPath).EnumerateFiles("*.log"))
      {
        if (enumerateFile.Name.StartsWith("VRageRender") && enumerateFile.Exists && (fileInfo == null || fileInfo.LastWriteTime < enumerateFile.LastWriteTime))
          fileInfo = enumerateFile;
        stringSet.Add(enumerateFile.FullName);
      }
      if (fileInfo != null)
        stringSet.Remove(fileInfo.FullName);
      stringSet.Remove(MySandboxGame.Log.GetFilePath());
      return stringSet;
    }

    public static void ReportNotInteractive(
      string logName,
      string id,
      bool includeAdditionalLogs,
      IEnumerable<string> additionalFiles,
      bool isCrash,
      string email,
      string feedback,
      CrashInfo info)
    {
      HashSet<string> stringSet = includeAdditionalLogs ? MyErrorReporter.EnumerateLogs() : (HashSet<string>) null;
      if (additionalFiles != null)
      {
        if (stringSet == null)
          stringSet = new HashSet<string>();
        foreach (string additionalFile in additionalFiles)
          stringSet.Add(additionalFile);
      }
      MyErrorReporter.TrySendReport(logName, id, email, feedback, info, isCrash ? "crash" : "log", (IEnumerable<string>) stringSet);
    }

    public static void ReportGeneral(string logName, string id, CrashInfo info)
    {
      if (string.IsNullOrWhiteSpace(logName) || MyErrorReporter.LoadAndDisplayCommonError(logName))
        return;
      MyCrashScreenTexts texts;
      if (MyTexts.Exists(MyCoreTexts.CrashScreen_MainText))
        texts = new MyCrashScreenTexts()
        {
          GameName = info.GameName,
          LogName = logName,
          MainText = MyTexts.Get(MyCoreTexts.CrashScreen_MainText).ToString(),
          Log = MyTexts.Get(MyCoreTexts.CrashScreen_Log).ToString(),
          EmailText = string.Format(MyTexts.Get(MyCoreTexts.CrashScreen_EmailText).ToString(), (object) (MyGameService.Service?.ServiceName ?? "Steam")),
          Email = MyTexts.Get(MyCoreTexts.CrashScreen_Email).ToString(),
          Detail = MyTexts.Get(MyCoreTexts.CrashScreen_Detail).ToString(),
          Yes = MyTexts.Get(MyCoreTexts.CrashScreen_Yes).ToString()
        };
      else
        texts = new MyCrashScreenTexts()
        {
          GameName = info.GameName,
          LogName = logName,
          MainText = "Space Engineers had a problem and crashed! We apologize for the inconvenience. Please click Send Log if you would like to help us analyze and fix the problem. For more information, check the log below",
          Log = "log",
          EmailText = "Additionally, you can send us your email in case a member of our support staff needs more information about this error. \r\n \r\n If you would not mind being contacted about this issue please provide your e-mail address below. By sending the log, I grant my consent to the processing of my personal data (E-mail, Steam ID and IP address) to Keen SWH LTD. United Kingdom and it subsidiaries, in order for these data to be processed for the purpose of tracking the crash and requesting feedback with the intent to improve the game performance. I grant this consent for an indefinite term until my express revocation thereof. I confirm that I have been informed that the provision of these data is voluntary, and that I have the right to request their deletion. Registration is non-transferable. More information about the processing of my personal data in the scope required by legal regulations, in particular Regulation (EU) 2016/679 of the European Parliament and of the Council, can be found as of 25 May 2018 here. \r\n",
          Email = "Email (optional)",
          Detail = "To help us resolve the problem, please provide a description of what you were doing when it occurred (optional)",
          Yes = "Send Log"
        };
      string message;
      string email;
      if (!MyVRage.Platform.CrashReporting.MessageBoxCrashForm(ref texts, out message, out email))
        return;
      MyErrorReporter.ReportInternal(logName, id, info, email, message);
    }

    public static void Report(string logName, string id, CrashInfo info, string errorMessage)
    {
      if (MyErrorReporter.LoadAndDisplayCommonError(logName) || !MyErrorReporter.AllowSendDialog(info.GameName, logName, errorMessage) || logName == null)
        return;
      MyErrorReporter.ReportInternal(logName, id, info, string.Empty, string.Empty);
    }

    public static void ReportAppAlreadyRunning(string gameName)
    {
      int num = (int) MyVRage.Platform.Windows.MessageBox(string.Format(MyTexts.SubstituteTexts(MyErrorReporter.APP_ALREADY_RUNNING).Replace("\\n", "\r\n"), (object) gameName), string.Format(MyTexts.SubstituteTexts(MyErrorReporter.MESSAGE_BOX_CAPTION).Replace("\\n", "\r\n"), (object) gameName), MessageBoxOptions.OkOnly);
    }

    private static bool TrySendReport(
      string logName,
      string gameId,
      string email,
      string feedback,
      CrashInfo info,
      string reportType,
      IEnumerable<string> files = null)
    {
      Dictionary<string, byte[]> additionalFiles = new Dictionary<string, byte[]>();
      StringBuilder stringBuilder = new StringBuilder();
      if (!string.IsNullOrWhiteSpace(email))
        stringBuilder.AppendLine("Email: " + email);
      if (!string.IsNullOrWhiteSpace(feedback))
        stringBuilder.AppendLine("Feedback: " + feedback);
      if (stringBuilder.Length > 0)
        stringBuilder.AppendLine();
      MyErrorReporter.SessionMetadata metadata = MyErrorReporter.SessionMetadata.DEFAULT;
      Tuple<string, string> tuple;
      try
      {
        string str1 = stringBuilder.ToString();
        if (logName != null && System.IO.File.Exists(logName))
        {
          using (FileStream fileStream = System.IO.File.Open(logName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
          {
            using (StreamReader streamReader = new StreamReader((Stream) fileStream))
              str1 += streamReader.ReadToEnd();
          }
        }
        string str2 = Path.GetFileName(logName);
        int length = str2.IndexOf("_");
        if (length >= 0)
          str2 = str2.Substring(0, length) + ".log";
        tuple = new Tuple<string, string>(str2, str1);
        MyErrorReporter.TryExtractMetadataFromLog(tuple, ref metadata);
      }
      catch
      {
        return false;
      }
      try
      {
        string s = stringBuilder.ToString();
        FileInfo fileInfo = (FileInfo) null;
        foreach (FileInfo enumerateFile in new DirectoryInfo(Path.GetDirectoryName(logName)).EnumerateFiles("VRageRender*.log"))
        {
          if (enumerateFile.Exists && (fileInfo == null || fileInfo.LastWriteTime < enumerateFile.LastWriteTime))
            fileInfo = enumerateFile;
        }
        if (fileInfo != null)
        {
          using (FileStream fileStream = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
          {
            using (StreamReader streamReader = new StreamReader((Stream) fileStream))
              s += streamReader.ReadToEnd();
          }
        }
        additionalFiles["VRageRender.log"] = Encoding.UTF8.GetBytes(s);
      }
      catch
      {
      }
      try
      {
        string path = Path.Combine(Path.GetDirectoryName(logName), MyPerGameSettings.BasicGameInfo.ApplicationName + ".cfg");
        string s = stringBuilder.ToString();
        if (System.IO.File.Exists(path))
        {
          using (FileStream fileStream = System.IO.File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
          {
            using (StreamReader streamReader = new StreamReader((Stream) fileStream))
              s += streamReader.ReadToEnd();
          }
          additionalFiles[Path.GetFileName(path)] = Encoding.UTF8.GetBytes(s);
        }
      }
      catch
      {
      }
      try
      {
        if (files != null)
        {
          using (MemoryStream memoryStream = new MemoryStream())
          {
            bool flag = false;
            using (ZipArchive zipArchive = new ZipArchive((Stream) memoryStream, ZipArchiveMode.Create, true))
            {
              foreach (string file in files)
              {
                flag = true;
                using (Stream destination = zipArchive.CreateEntry(Path.GetFileName(file)).Open())
                {
                  using (FileStream fileStream = System.IO.File.Open(file, FileMode.Open))
                    fileStream.CopyTo(destination);
                }
              }
            }
            if (flag)
            {
              byte[] array = memoryStream.ToArray();
              additionalFiles["AdditionalFiles.zip"] = array;
            }
          }
        }
      }
      catch
      {
      }
      if (MyFakes.ENABLE_MINIDUMP_SENDING)
      {
        try
        {
          using (MemoryStream memoryStream = new MemoryStream())
          {
            bool flag = false;
            using (ZipArchive zipArchive = new ZipArchive((Stream) memoryStream, ZipArchiveMode.Create, true))
            {
              foreach (string activeDump in MyMiniDump.FindActiveDumps(Path.GetDirectoryName(logName)))
              {
                flag = true;
                using (Stream destination = zipArchive.CreateEntry(Path.GetFileName(activeDump)).Open())
                {
                  using (FileStream fileStream = System.IO.File.Open(activeDump, FileMode.Open))
                    fileStream.CopyTo(destination);
                }
              }
            }
            if (flag)
            {
              byte[] array = memoryStream.ToArray();
              additionalFiles["Minidumps.zip"] = array;
            }
          }
        }
        catch
        {
        }
      }
      return MyErrorReporter.TrySendToOpicka(ref metadata, gameId, tuple, additionalFiles, email, feedback, info, reportType);
    }

    private static void TryExtractMetadataFromLog(
      Tuple<string, string> mainLog,
      ref MyErrorReporter.SessionMetadata metadata)
    {
      string str1 = metadata.UniqueUserIdentifier;
      string str2 = metadata.SessionId;
      try
      {
        using (StringReader stringReader = new StringReader(mainLog.Item2))
        {
          for (string str3 = stringReader.ReadLine(); str3 != null; str3 = stringReader.ReadLine())
          {
            int num1 = str3.IndexOf("Analytics uuid:");
            if (num1 >= 0)
              str1 = str3.Substring(num1 + "Analytics uuid:".Length).Trim();
            int num2 = str3.IndexOf("Analytics session:");
            if (num2 >= 0)
              str2 = str3.Substring(num2 + "Analytics session:".Length).Trim();
          }
        }
      }
      catch
      {
      }
      metadata.UniqueUserIdentifier = str1;
      metadata.SessionId = str2;
    }

    private static bool TrySendToHetzner(
      ref MyErrorReporter.SessionMetadata metadata,
      string gameId,
      Tuple<string, string> log,
      Dictionary<string, byte[]> additionalFiles,
      string email,
      string feedback)
    {
      if (log == null || string.IsNullOrWhiteSpace(log.Item1) || (string.IsNullOrWhiteSpace(log.Item2) || string.IsNullOrWhiteSpace(gameId)))
        return false;
      string str = "";
      byte[] bytes;
      if (additionalFiles.TryGetValue("VRageRender.log", out bytes))
        str = Encoding.UTF8.GetString(bytes);
      byte[] buffer;
      additionalFiles.TryGetValue("Minidumps.zip", out buffer);
      try
      {
        string url = "https://minerwars.keenswh.com/SubmitLog.aspx?id=" + gameId;
        HttpStatusCode httpStatusCode = (HttpStatusCode) 0;
        using (MemoryStream memoryStream = new MemoryStream())
        {
          using (BinaryWriter binaryWriter = new BinaryWriter((Stream) memoryStream))
          {
            binaryWriter.Write(log.Item2);
            binaryWriter.Write(str);
            if (MyFakes.ENABLE_MINIDUMP_SENDING && buffer != null)
            {
              binaryWriter.Write(buffer.Length);
              binaryWriter.Write(buffer);
            }
            HttpData[] parameters = new HttpData[2]
            {
              new HttpData("Content-Type", (object) "application/octet-stream", HttpDataType.HttpHeader),
              new HttpData("application/octet-stream", (object) memoryStream.ToArray(), HttpDataType.RequestBody)
            };
            httpStatusCode = MyVRage.Platform.Http.SendRequest(url, parameters, HttpMethod.POST, out string _);
          }
        }
        return httpStatusCode == HttpStatusCode.OK;
      }
      catch
      {
      }
      return false;
    }

    private static bool TrySendToOpicka(
      ref MyErrorReporter.SessionMetadata metadata,
      string id,
      Tuple<string, string> log,
      Dictionary<string, byte[]> additionalFiles,
      string email,
      string feedback,
      CrashInfo generalInfo,
      string reportType)
    {
      if (log != null && !string.IsNullOrWhiteSpace(log.Item1) && !string.IsNullOrWhiteSpace(log.Item2))
      {
        if (!string.IsNullOrWhiteSpace(id))
        {
          try
          {
            string json = JsonMapper.ToJson((object) new MyErrorReporter.MyCrashInfo()
            {
              UniqueUserIdentifier = metadata.UniqueUserIdentifier,
              SessionID = 0L,
              GameID = id,
              GameVersion = generalInfo.AppVersion,
              Email = email,
              Feedback = feedback,
              ReportType = reportType,
              IsHang = generalInfo.IsHang,
              OOM = generalInfo.IsOutOfMemory,
              IsNative = generalInfo.IsNative,
              IsGPU = generalInfo.IsGPU,
              IsTask = generalInfo.IsTask,
              PCUCount = generalInfo.PCUCount,
              ProcessRunTime = generalInfo.ProcessRunTime,
              IsOfficial = true,
              BranchName = MyGameService.BranchName
            });
            HttpStatusCode httpStatusCode = (HttpStatusCode) 0;
            using (MemoryStream memoryStream = new MemoryStream())
            {
              using (BinaryWriter binaryWriter = new BinaryWriter((Stream) memoryStream))
              {
                binaryWriter.Write(nameof (metadata));
                binaryWriter.Write(json);
                binaryWriter.Write(nameof (log));
                binaryWriter.Write(log.Item1);
                binaryWriter.Write(log.Item2);
                foreach (KeyValuePair<string, byte[]> additionalFile in additionalFiles)
                {
                  binaryWriter.Write("file");
                  binaryWriter.Write(additionalFile.Key);
                  binaryWriter.Write(additionalFile.Value.Length);
                  binaryWriter.Write(additionalFile.Value);
                }
                httpStatusCode = MyVRage.Platform.Http.SendRequest("https://crashlogs.keenswh.com/api/Report", new HttpData[2]
                {
                  new HttpData("Content-Type", (object) "application/octet-stream", HttpDataType.HttpHeader),
                  new HttpData("application/octet-stream", (object) memoryStream.ToArray(), HttpDataType.RequestBody)
                }, HttpMethod.POST, out string _);
              }
            }
            return httpStatusCode == HttpStatusCode.OK;
          }
          catch
          {
          }
          return false;
        }
      }
      return false;
    }

    public static CrashInfo BuildCrashInfo()
    {
      MySession mySession = MySession.Static;
      float allocated;
      float used;
      MyVRage.Platform.System.GetGCMemory(out allocated, out used);
      return new CrashInfo(MyFinalBuildConstants.APP_VERSION_STRING.ToString(), MyPerGameSettings.BasicGameInfo.GameName, MyPerGameSettings.BasicGameInfo.AnalyticId)
      {
        GCMemory = (long) used,
        GCMemoryAllocated = (long) allocated,
        HWAvailableMemory = (long) MyVRage.Platform.System.RAMCounter,
        ProcessPrivateMemory = MyVRage.Platform.System.ProcessPrivateMemory / 1024L / 1024L,
        PCUCount = mySession != null ? mySession.TotalSessionPCU : 0,
        IsExperimental = mySession != null && mySession.IsSettingsExperimental(),
        ProcessRunTime = (long) (DateTime.Now - Process.GetCurrentProcess().StartTime).TotalSeconds
      };
    }

    public static void UpdateHangAnalytics()
    {
      CrashInfo hangInfo = MyErrorReporter.BuildCrashInfo();
      hangInfo.IsHang = true;
      MyConfig config = MySandboxGame.Config;
      bool GDPRConsent = config == null || config.GDPRConsent.GetValueOrDefault(false);
      MyVRage.Platform.CrashReporting.UpdateHangAnalytics(hangInfo, MyLog.Default.GetFilePath(), GDPRConsent);
    }

    public struct SessionMetadata
    {
      public static readonly MyErrorReporter.SessionMetadata DEFAULT = new MyErrorReporter.SessionMetadata()
      {
        UniqueUserIdentifier = Guid.NewGuid().ToString(),
        SessionId = string.Empty
      };
      public string UniqueUserIdentifier;
      public string SessionId;
    }

    private class MyCrashInfo
    {
      public string ReportVersion = "1.0";
      public string UniqueUserIdentifier = string.Empty;
      public long SessionID;
      public string GameID = string.Empty;
      public string GameVersion = string.Empty;
      public string Feedback = string.Empty;
      public string Email = string.Empty;
      public string ReportType = string.Empty;
      public bool IsOfficial;
      public string BranchName = string.Empty;
      public bool OOM;
      public bool IsGPU;
      public bool IsNative;
      public bool IsTask;
      public bool IsHang;
      public long ProcessRunTime;
      public int PCUCount;
    }
  }
}
