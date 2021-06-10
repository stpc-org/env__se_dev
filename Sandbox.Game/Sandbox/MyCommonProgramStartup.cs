// Decompiled with JetBrains decompiler
// Type: Sandbox.MyCommonProgramStartup
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using VRage;
using VRage.Game;
using VRage.Game.ObjectBuilder;
using VRage.GameServices;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox
{
  public class MyCommonProgramStartup
  {
    private string[] m_args;
    private static IMyRender m_renderer;

    private MyBasicGameInfo GameInfo => MyPerGameSettings.BasicGameInfo;

    public static void MessageBoxWrapper(string caption, string text)
    {
      int num = (int) MyVRage.Platform.Windows.MessageBox(text, caption, MessageBoxOptions.OkOnly);
    }

    public MyCommonProgramStartup(string[] args)
    {
      int? gameVersion = this.GameInfo.GameVersion;
      MyFinalBuildConstants.APP_VERSION = gameVersion.HasValue ? (MyVersion) gameVersion.GetValueOrDefault() : (MyVersion) null;
      this.m_args = args;
    }

    public bool PerformReporting(out CrashInfo crashInfo)
    {
      crashInfo = new CrashInfo();
      try
      {
        string logPath;
        bool isUnsupportedGpu;
        bool exitAfterReport;
        if (MyVRage.Platform.CrashReporting.ExtractCrashAnalyticsReport(this.m_args, out logPath, out crashInfo, out isUnsupportedGpu, out exitAfterReport))
        {
          if (isUnsupportedGpu)
          {
            string errorMessage = string.Format(MyTexts.SubstituteTexts(MyErrorReporter.APP_ERROR_MESSAGE_DX11_NOT_AVAILABLE).Replace("\\n", "\r\n"), (object) this.m_args[1], (object) this.m_args[2], (object) this.GameInfo.MinimumRequirementsWeb);
            MyErrorReporter.Report(logPath, this.GameInfo.GameAcronym, crashInfo, errorMessage);
          }
          else
            MyErrorReporter.ReportGeneral(logPath, this.GameInfo.GameAcronym, crashInfo);
          return exitAfterReport;
        }
      }
      catch (Exception ex)
      {
      }
      return false;
    }

    public void PerformNotInteractiveReport(
      bool includeAdditionalLogs,
      IEnumerable<string> additionalFiles)
    {
      CrashInfo info = MyErrorReporter.BuildCrashInfo();
      MyErrorReporter.ReportNotInteractive(MyLog.Default.GetFilePath(), this.GameInfo.GameAcronym, includeAdditionalLogs, additionalFiles, false, string.Empty, string.Empty, info);
    }

    public void PerformAutoconnect()
    {
      if (!MyFakes.ENABLE_CONNECT_COMMAND_LINE || !this.m_args.Contains<string>("+connect"))
        return;
      int num = Array.IndexOf<string>(this.m_args, "+connect");
      if (num + 1 >= this.m_args.Length)
        return;
      Sandbox.Engine.Platform.Game.ConnectToServer = this.m_args[num + 1];
      if (string.IsNullOrEmpty(Sandbox.Engine.Platform.Game.ConnectToServer))
        return;
      Console.WriteLine(this.GameInfo.GameName + " " + (object) MyFinalBuildConstants.APP_VERSION_STRING);
      Console.WriteLine("Obfuscated: " + MyObfuscation.Enabled.ToString() + ", Platform: " + (VRage.Library.MyEnvironment.Is64BitProcess ? " 64-bit" : " 32-bit"));
      Console.WriteLine("Connecting to: " + this.m_args[num + 1]);
    }

    public bool PerformColdStart()
    {
      if (!this.m_args.Contains<string>("-coldstart"))
        return false;
      MyGlobalTypeMetadata.Static.Init(false);
      Parallel.Scheduler = (IWorkScheduler) new PrioritizedScheduler(1, false);
      int num = -1;
      List<string> stringList = new List<string>();
      Queue<AssemblyName> assemblyNameQueue = new Queue<AssemblyName>();
      assemblyNameQueue.Enqueue(Assembly.GetEntryAssembly().GetName());
      while (assemblyNameQueue.Count > 0)
      {
        AssemblyName assemblyRef = assemblyNameQueue.Dequeue();
        if (!stringList.Contains(assemblyRef.FullName))
        {
          stringList.Add(assemblyRef.FullName);
          try
          {
            Assembly assembly = Assembly.Load(assemblyRef);
            MyCommonProgramStartup.PreloadTypesFrom(assembly);
            ++num;
            foreach (AssemblyName referencedAssembly in assembly.GetReferencedAssemblies())
              assemblyNameQueue.Enqueue(referencedAssembly);
          }
          catch (Exception ex)
          {
          }
        }
      }
      if (MyFakes.ENABLE_NGEN)
      {
        ProcessStartInfo startInfo = new ProcessStartInfo(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "ngen"))
        {
          Verb = "runas"
        };
        startInfo.Arguments = "install SpaceEngineers.exe /silent /nologo";
        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
        try
        {
          Process.Start(startInfo).WaitForExit();
        }
        catch (Exception ex)
        {
          MyLog.Default.WriteLine("NGEN failed: " + (object) ex);
        }
      }
      return true;
    }

    public bool VerboseNetworkLogging() => this.m_args.Contains<string>("-verboseNetworkLogging");

    public bool UseEOS() => this.m_args.Contains<string>("-eos");

    public bool IsRenderUpdateSyncEnabled() => this.m_args.Contains<string>("-render_sync");

    public bool IsVideoRecordingEnabled() => this.m_args.Contains<string>("-video_record");

    public bool IsGenerateDx11MipCache() => this.m_args.Contains<string>("-generateDx11MipCache");

    private static void PreloadTypesFrom(Assembly assembly)
    {
      if (!(assembly != (Assembly) null))
        return;
      MyCommonProgramStartup.ForceStaticCtor(((IEnumerable<Type>) assembly.GetTypes()).Where<Type>((Func<Type, bool>) (type => Attribute.IsDefined((MemberInfo) type, typeof (PreloadRequiredAttribute)))).ToArray<Type>());
    }

    public static void ForceStaticCtor(Type[] types)
    {
      foreach (Type type in types)
        RuntimeHelpers.RunClassConstructor(type.TypeHandle);
    }

    public string GetAppDataPath()
    {
      string str = (string) null;
      int index = Array.IndexOf<string>(this.m_args, "-appdata") + 1;
      if (index != 0 && this.m_args.Length > index)
      {
        string name = this.m_args[index];
        if (!name.StartsWith("-"))
          str = Path.GetFullPath(Environment.ExpandEnvironmentVariables(name));
      }
      return str;
    }

    public void InitSplashScreen()
    {
      if (!MyFakes.ENABLE_SPLASHSCREEN || this.m_args.Contains<string>("-nosplash"))
        return;
      MyVRage.Platform.Windows.ShowSplashScreen(this.GameInfo.SplashScreenImage, new Vector2(0.7f, 0.7f));
    }

    public void DisposeSplashScreen() => MyVRage.Platform.Windows.HideSplashScreen();

    public bool Check64Bit()
    {
      if (VRage.Library.MyEnvironment.Is64BitProcess || AssemblyExtensions.TryGetArchitecture("SteamSDK.dll") != ProcessorArchitecture.Amd64)
        return true;
      switch (MyMessageBox.Show(this.GameInfo.GameName + " cannot be started in 64-bit mode, " + "because 64-bit version of .NET framework is not available or is broken." + VRage.Library.MyEnvironment.NewLine + VRage.Library.MyEnvironment.NewLine + "Do you want to open website with more information about this particular issue?" + VRage.Library.MyEnvironment.NewLine + VRage.Library.MyEnvironment.NewLine + "Press Yes to open website with info" + VRage.Library.MyEnvironment.NewLine + "Press No to run in 32-bit mode (smaller potential of " + this.GameInfo.GameName + "!)" + VRage.Library.MyEnvironment.NewLine + "Press Cancel to close this dialog", ".NET Framework 64-bit error", MessageBoxOptions.YesNoCancel))
      {
        case MessageBoxResult.Yes:
          MyVRage.Platform.System.OpenUrl("http://www.spaceengineersgame.com/64-bit-start-up-issue.html");
          break;
        case MessageBoxResult.No:
          string location = Assembly.GetEntryAssembly().Location;
          string path = Path.Combine(new FileInfo(location).Directory.Parent.FullName, "Bin", Path.GetFileName(location));
          Process.Start(new ProcessStartInfo()
          {
            FileName = path,
            WorkingDirectory = Path.GetDirectoryName(path),
            Arguments = "-fallback",
            UseShellExecute = false,
            WindowStyle = ProcessWindowStyle.Normal
          });
          break;
      }
      return false;
    }

    public bool CheckSteamRunning()
    {
      if (!Sandbox.Engine.Platform.Game.IsDedicated)
      {
        if (MyGameService.IsActive)
        {
          MyGameService.SetNotificationPosition(NotificationPosition.TopLeft);
          MyLog log1 = MySandboxGame.Log;
          bool flag = MyGameService.IsActive;
          string msg1 = "Service.IsActive: " + flag.ToString();
          log1.WriteLineAndConsole(msg1);
          MyLog log2 = MySandboxGame.Log;
          flag = MyGameService.IsOnline;
          string msg2 = "Service.IsOnline: " + flag.ToString();
          log2.WriteLineAndConsole(msg2);
          MyLog log3 = MySandboxGame.Log;
          flag = MyGameService.OwnsGame;
          string msg3 = "Service.OwnsGame: " + flag.ToString();
          log3.WriteLineAndConsole(msg3);
          MySandboxGame.Log.WriteLineAndConsole("Service.UserId: " + (object) MyGameService.UserId);
          MySandboxGame.Log.WriteLineAndConsole("Service.UserName: " + MyGameService.UserName);
          MySandboxGame.Log.WriteLineAndConsole("Service.Branch: " + MyGameService.BranchName);
          MySandboxGame.Log.WriteLineAndConsole("Build date: " + MySandboxGame.BuildDateTime.ToString("yyyy-MM-dd hh:mm", (IFormatProvider) CultureInfo.InvariantCulture));
          MySandboxGame.Log.WriteLineAndConsole("Build version: " + MySandboxGame.BuildVersion.ToString());
        }
        else if ((!MyGameService.IsActive || !MyGameService.OwnsGame) && !MyFakes.ENABLE_RUN_WITHOUT_STEAM)
        {
          MyCommonProgramStartup.MessageBoxWrapper(MyGameService.Service.ServiceName + " is not running!", "Please run this game from " + MyGameService.Service.ServiceName + "." + VRage.Library.MyEnvironment.NewLine + "(restart Steam if already running)");
          return false;
        }
      }
      return true;
    }
  }
}
