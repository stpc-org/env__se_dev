// Decompiled with JetBrains decompiler
// Type: Sandbox.MyInitializer
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using ParallelTasks;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using VRage;
using VRage.Common.Utils;
using VRage.Cryptography;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Utils;
using VRageRender;

namespace Sandbox
{
  public static class MyInitializer
  {
    private static string m_appName;
    private static HashSet<string> m_ignoreList = new HashSet<string>();
    private static object m_exceptionSyncRoot = new object();

    private static void ChecksumFailed(string filename, string hash)
    {
      if (MyInitializer.m_ignoreList.Contains(filename))
        return;
      MyInitializer.m_ignoreList.Add(filename);
      MySandboxGame.Log.WriteLine("Error: checksum of file '" + filename + "' failed " + hash);
    }

    private static void ChecksumNotFound(IFileVerifier verifier, string filename)
    {
      MyChecksumVerifier checksumVerifier = (MyChecksumVerifier) verifier;
      if (MyInitializer.m_ignoreList.Contains(filename) || !filename.StartsWith(checksumVerifier.BaseChecksumDir, StringComparison.InvariantCultureIgnoreCase) || !filename.Substring(Math.Min(filename.Length, checksumVerifier.BaseChecksumDir.Length + 1)).StartsWith("Data", StringComparison.InvariantCultureIgnoreCase))
        return;
      MySandboxGame.Log.WriteLine("Error: no checksum found for file '" + filename + "'");
      MyInitializer.m_ignoreList.Add(filename);
    }

    public static void InvokeBeforeRun(
      uint appId,
      string appName,
      string userDataPath,
      bool addDateToLog = false,
      int maxLogAge = -1,
      Action onConfigChangedCallback = null)
    {
      MyInitializer.m_appName = appName;
      MyInitializer.InitFileSystem(userDataPath);
      MySandboxGame.Log.InitWithDate(appName, MyFinalBuildConstants.APP_VERSION_STRING, maxLogAge);
      MyLog.Default = MySandboxGame.Log;
      MyInitializer.InitExceptionHandling();
      string rootPath = MyFileSystem.RootPath;
      bool flag1 = SteamHelpers.IsSteamPath(rootPath);
      bool flag2 = SteamHelpers.IsAppManifestPresent(rootPath, appId);
      Sandbox.Engine.Platform.Game.IsPirated = !flag1 && !flag2;
      MySandboxGame.Log.WriteLineAndConsole(string.Format("Is official: {0} {1}{2}{3}", (object) true, MyObfuscation.Enabled ? (object) "[O]" : (object) "[NO]", flag1 ? (object) "[IS]" : (object) "[NIS]", flag2 ? (object) "[AMP]" : (object) "[NAMP]"));
      MySandboxGame.Log.WriteLine("Branch / Sandbox: " + MyGameService.BranchName);
      MySandboxGame.Log.WriteLineAndConsole("Environment.ProcessorCount: " + (object) VRage.Library.MyEnvironment.ProcessorCount);
      MySandboxGame.Log.WriteLineAndConsole("Environment.OSVersion: " + MyVRage.Platform.System.GetOsName());
      MySandboxGame.Log.WriteLineAndConsole("Environment.CommandLine: " + Environment.CommandLine);
      MyLog log1 = MySandboxGame.Log;
      bool flag3 = VRage.Library.MyEnvironment.Is64BitProcess;
      string msg1 = "Environment.Is64BitProcess: " + flag3.ToString();
      log1.WriteLineAndConsole(msg1);
      MyLog log2 = MySandboxGame.Log;
      flag3 = Environment.Is64BitOperatingSystem;
      string msg2 = "Environment.Is64BitOperatingSystem: " + flag3.ToString();
      log2.WriteLineAndConsole(msg2);
      MySandboxGame.Log.WriteLineAndConsole("Environment.Version: " + RuntimeInformation.FrameworkDescription);
      MySandboxGame.Log.WriteLineAndConsole("Environment.CurrentDirectory: " + Environment.CurrentDirectory);
      uint frequency;
      MySandboxGame.Log.WriteLineAndConsole("CPU Info: " + MyVRage.Platform.System.GetInfoCPU(out frequency, out uint _));
      MySandboxGame.CPUFrequency = frequency;
      MySandboxGame.Log.WriteLine("IntPtr.Size: " + IntPtr.Size.ToString());
      MySandboxGame.Log.WriteLine("Default Culture: " + CultureInfo.CurrentCulture.Name);
      MySandboxGame.Log.WriteLine("Default UI Culture: " + CultureInfo.CurrentUICulture.Name);
      MyVRage.Platform.System.LogRuntimeInfo(new Action<string>(MySandboxGame.Log.WriteLineAndConsole));
      MySandboxGame.Config = new MyConfig(appName + ".cfg");
      if (onConfigChangedCallback != null)
        MySandboxGame.Config.OnSettingChanged += onConfigChangedCallback;
      MySandboxGame.Config.Load();
    }

    public static void InitFileSystem(string userDataPath) => MyFileSystem.Init(Path.Combine(MyFileSystem.RootPath, "Content"), userDataPath);

    private static IMyCrashReporting ErrorPlatform => MyVRage.Platform.CrashReporting;

    public static void InitExceptionHandling()
    {
      HkCrashHunter.Init((Func<int>) (() =>
      {
        MySession mySession = MySession.Static;
        return mySession == null ? -1 : mySession.GameplayFrameCounter;
      }));
      AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyInitializer.UnhandledExceptionHandler);
      MyInitializer.ErrorPlatform.SetNativeExceptionHandler((Action<IntPtr>) (x => MyInitializer.ProcessUnhandledException((Exception) new MyInitializer.MyNativeException(), x)));
      if (string.IsNullOrEmpty(Thread.CurrentThread.Name))
        Thread.CurrentThread.Name = "Main thread";
      Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
      Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
      if (MyFakes.ENABLE_MINIDUMP_SENDING && MyFileSystem.IsInitialized)
      {
        if (MyFakes.COLLECT_SUSPEND_DUMPS)
          MyVRage.Platform.Render.OnSuspending += new Action(MyMiniDump.CollectStateDump);
        MyMiniDump.CleanupOldDumps();
      }
      MyInitializer.ErrorPlatform.CleanupCrashAnalytics();
      MyErrorReporter.UpdateHangAnalytics();
    }

    public static void InvokeAfterRun() => MySandboxGame.Log.Close();

    public static void InitCheckSum()
    {
      try
      {
        string path = Path.Combine(MyFileSystem.ContentPath, "checksum.xml");
        if (!File.Exists(path))
        {
          MySandboxGame.Log.WriteLine("Checksum file is missing, game will run as usual but file integrity won't be verified");
        }
        else
        {
          using (FileStream fileStream = File.OpenRead(path))
          {
            MyChecksums checksums = (MyChecksums) new XmlSerializer(typeof (MyChecksums)).Deserialize((Stream) fileStream);
            MyChecksumVerifier checksumVerifier = new MyChecksumVerifier(checksums, MyFileSystem.ContentPath);
            checksumVerifier.ChecksumFailed += new Action<string, string>(MyInitializer.ChecksumFailed);
            checksumVerifier.ChecksumNotFound += new Action<IFileVerifier, string>(MyInitializer.ChecksumNotFound);
            fileStream.Position = 0L;
            SHA256 shA256 = MySHA256.Create();
            shA256.Initialize();
            byte[] hash = shA256.ComputeHash((Stream) fileStream);
            string str = "BgIAAACkAABSU0ExAAQAAAEAAQClSibD83Y6Akok8tAtkbMz4IpueWFra0QkkKcodwe2pV/RJAfyq5mLUGsF3JdTdu3VWn93VM+ZpL9CcMKS8HaaHmBZJn7k2yxNvU4SD+8PhiZ87iPqpkN2V+rz9nyPWTHDTgadYMmenKk2r7w4oYOooo5WXdkTVjAD50MroAONuQ==";
            MySandboxGame.Log.WriteLine("Checksum file hash: " + Convert.ToBase64String(hash));
            MySandboxGame.Log.WriteLine(string.Format("Checksum public key valid: {0}, Key: {1}", (object) (checksums.PublicKey == str), (object) checksums.PublicKey));
            MyFileSystem.FileVerifier = (IFileVerifier) checksumVerifier;
          }
        }
      }
      catch
      {
      }
    }

    [HandleProcessCorruptedStateExceptions]
    [SecurityCritical]
    private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args) => MyInitializer.ProcessUnhandledException(args.ExceptionObject as Exception, IntPtr.Zero);

    private static void ProcessUnhandledException(Exception exception, IntPtr exceptionPointers)
    {
      lock (MyInitializer.m_exceptionSyncRoot)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        bool oom = MyInitializer.ErrorPlatform.IsCriticalMemory || MyInitializer.\u003C\u003Ec.\u003C\u003E9.\u003CProcessUnhandledException\u003Eg__IsOom\u007C15_0(exception);
        if (oom)
        {
          MyInitializer.MyOutOfMemoryException ofMemoryException = new MyInitializer.MyOutOfMemoryException();
          LogException((Exception) ofMemoryException);
          if (exception != null)
          {
            MySandboxGame.Log.AppendToClosedLog("Exception source:");
            MySandboxGame.Log.AppendToClosedLog(Convert.ToBase64String(Encoding.UTF8.GetBytes(exception.ToString())));
          }
          exception = (Exception) ofMemoryException;
        }
        else
          LogException(exception);
        MySandboxGame.Log.AppendToClosedLog("Showing message");
        if (!Sandbox.Engine.Platform.Game.IsDedicated || MyPerGameSettings.SendLogToKeen)
          MyInitializer.OnCrash(MySandboxGame.Log.GetFilePath(), MyPerGameSettings.GameName, MyPerGameSettings.MinimumRequirementsPage, MyPerGameSettings.RequiresDX11, exception, exceptionPointers, oom);
        MySandboxGame.Log.Close();
        MySimpleProfiler.LogPerformanceTestResults();
        if (Debugger.IsAttached)
          return;
        AppDomain.CurrentDomain.UnhandledException -= new UnhandledExceptionEventHandler(MyInitializer.UnhandledExceptionHandler);
        MyInitializer.ErrorPlatform.SetNativeExceptionHandler((Action<IntPtr>) null);
        MyInitializer.ErrorPlatform.ExitProcessOnCrash(exception);
      }

      void LogException(Exception e)
      {
        Exception exception = e;
        if (exception != null)
        {
          if (!(exception is TaskException taskException))
          {
            if (exception is MyInitializer.MyNativeException myNativeException)
            {
              MyVRage.Platform.System.LogToExternalDebugger("Unhandled native exception: " + Environment.NewLine + (object) myNativeException);
              MySandboxGame.Log.AppendToClosedLog("Native exception occured: " + Environment.NewLine + (object) myNativeException);
              return;
            }
          }
          else
          {
            foreach (Exception innerException in taskException.InnerExceptions)
              LogException(innerException);
            MyVRage.Platform.System.LogToExternalDebugger("Task Exception Stack:" + Environment.NewLine + taskException.StackTrace);
            MySandboxGame.Log.AppendToClosedLog("Task Exception Stack:" + Environment.NewLine + taskException.StackTrace);
            return;
          }
        }
        if (e == null)
          return;
        MyVRage.Platform.System.LogToExternalDebugger("Unhandled managed exception: " + Environment.NewLine + (object) e);
        MySandboxGame.Log.AppendToClosedLog(e);
        MyInitializer.HandleSpecialExceptions(e);
      }
    }

    private static void HandleSpecialExceptions(Exception exception)
    {
      switch (exception)
      {
        case null:
          return;
        case ReflectionTypeLoadException typeLoadException:
          foreach (Exception loaderException in typeLoadException.LoaderExceptions)
            MySandboxGame.Log.AppendToClosedLog(loaderException);
          break;
        case OutOfMemoryException ofMemoryException:
          MySandboxGame.Log.AppendToClosedLog("Handling out of memory exception... " + (object) MySandboxGame.Config);
          if (MySandboxGame.Config.LowMemSwitchToLow == MyConfig.LowMemSwitch.ARMED && !MySandboxGame.Config.IsSetToLowQuality())
          {
            MySandboxGame.Log.AppendToClosedLog("Creating switch to low request");
            MySandboxGame.Config.LowMemSwitchToLow = MyConfig.LowMemSwitch.TRIGGERED;
            MySandboxGame.Config.Save();
            MySandboxGame.Log.AppendToClosedLog("Switch to low request created");
          }
          MySandboxGame.Log.AppendToClosedLog((Exception) ofMemoryException);
          break;
      }
      MyInitializer.HandleSpecialExceptions(exception.InnerException);
    }

    private static bool IsModCrash(Exception e) => e is ModCrashedException;

    private static void OnCrash(
      string logPath,
      string gameName,
      string minimumRequirementsPage,
      bool requiresDX11,
      Exception exception,
      IntPtr exceptionPointers,
      bool oom)
    {
      try
      {
        ExceptionType exceptionType = MyInitializer.ErrorPlatform.GetExceptionType(exception);
        if (MyVideoSettingsManager.GpuUnderMinimum)
          MyErrorReporter.ReportGpuUnderMinimumCrash(gameName, logPath, minimumRequirementsPage);
        else if (!Sandbox.Engine.Platform.Game.IsDedicated && exceptionType == ExceptionType.OutOfMemory)
          MyErrorReporter.ReportOutOfMemory(gameName, logPath, minimumRequirementsPage);
        else if (!Sandbox.Engine.Platform.Game.IsDedicated && exceptionType == ExceptionType.OutOfVideoMemory)
        {
          MyErrorReporter.ReportOutOfVideoMemory(gameName, logPath, minimumRequirementsPage);
        }
        else
        {
          bool flag = false;
          if (exception != null && exception.Data.Contains((object) "Silent"))
            flag = Convert.ToBoolean(exception.Data[(object) "Silent"]);
          if (MyFakes.ENABLE_MINIDUMP_SENDING && exceptionType != ExceptionType.OutOfMemory)
            MyMiniDump.CollectCrashDump(exceptionPointers);
          if (flag || Debugger.IsAttached)
            return;
          if (MyInitializer.IsModCrash(exception))
          {
            ModCrashedException crashedException = (ModCrashedException) exception;
            MyModCrashScreenTexts texts = new MyModCrashScreenTexts()
            {
              ModName = crashedException.ModContext.ModName,
              ModId = crashedException.ModContext.ModId,
              ModServiceName = crashedException.ModContext.ModServiceName,
              LogPath = logPath,
              Close = MyTexts.GetString(MyCommonTexts.Close),
              Text = MyTexts.GetString(MyCommonTexts.ModCrashedTheGame),
              Info = MyTexts.GetString(MyCommonTexts.ModCrashedTheGameInfo)
            };
            MyInitializer.ErrorPlatform.MessageBoxModCrashForm(ref texts);
          }
          else
          {
            CrashInfo crashInfo = MyInitializer.GetCrashInfo(exception, oom, exceptionType);
            MySandboxGame.Log.AppendToClosedLog("\n" + (object) crashInfo);
            CLoseLog(MySandboxGame.Log);
            CLoseLog(MyRenderProxy.Log);
            MyConfig config = MySandboxGame.Config;
            bool GDPRConsent = config == null || config.GDPRConsent.GetValueOrDefault(false);
            MyInitializer.ErrorPlatform.PrepareCrashAnalyticsReporting(logPath, GDPRConsent, crashInfo, requiresDX11 && exceptionType == ExceptionType.UnsupportedGpu);
          }
        }
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLineAndConsole("Exception while reporting crash.");
        MyLog.Default.WriteLineAndConsole(ex.ToString());
      }
      finally
      {
        if (!Debugger.IsAttached)
        {
          try
          {
            if (MySpaceAnalytics.Instance != null)
              MySpaceAnalytics.Instance.ReportSessionEndByCrash(exception);
          }
          catch
          {
          }
        }
      }

      void CLoseLog(MyLog log)
      {
        try
        {
          log.Flush();
          log.Close();
        }
        catch
        {
        }
      }
    }

    private static CrashInfo GetCrashInfo(Exception exception, bool oom, ExceptionType et)
    {
      CrashInfo crashInfo = MyErrorReporter.BuildCrashInfo();
      crashInfo.IsGPU = et == ExceptionType.DriverCrash;
      crashInfo.IsOutOfMemory = oom;
      crashInfo.IsNative = exception is MyInitializer.MyNativeException;
      crashInfo.IsTask = exception is TaskException;
      return crashInfo;
    }

    private class MyOutOfMemoryException : Exception
    {
      public override string StackTrace => string.Empty;

      public MyOutOfMemoryException()
        : base("Game is at critically low memory")
      {
      }
    }

    private class MyNativeException : Exception
    {
      private StackTrace m_trace = new StackTrace(1, false);

      public MyNativeException()
        : base("Exception in unmanaged code.")
      {
      }

      public override string StackTrace => this.m_trace.ToString();

      public override string ToString() => this.StackTrace;
    }
  }
}
