// Decompiled with JetBrains decompiler
// Type: VRage.IMyCrashReporting
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;

namespace VRage
{
  public interface IMyCrashReporting
  {
    bool IsCriticalMemory { get; }

    ExceptionType GetExceptionType(Exception e);

    void WriteMiniDump(string dumpPath, MyMiniDump.Options dumpFlags, IntPtr exceptionPointers);

    void SetNativeExceptionHandler(Action<IntPtr> handler);

    void PrepareCrashAnalyticsReporting(
      string logPath,
      bool GDPRConsent,
      CrashInfo info,
      bool isUnsupportedGpu);

    bool ExtractCrashAnalyticsReport(
      string[] args,
      out string logPath,
      out CrashInfo info,
      out bool isUnsupportedGpu,
      out bool exitAfterReport);

    void UpdateHangAnalytics(CrashInfo hangInfo, string logPath, bool GDPRConsent);

    void CleanupCrashAnalytics();

    bool MessageBoxCrashForm(ref MyCrashScreenTexts texts, out string message, out string email);

    void MessageBoxModCrashForm(ref MyModCrashScreenTexts texts);

    void ExitProcessOnCrash(Exception exception);

    event Action<Exception> ExitingProcessOnCrash;

    IEnumerable<string> AdditionalReportFiles();
  }
}
