// Decompiled with JetBrains decompiler
// Type: VRage.IVRageSystem
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;

namespace VRage
{
  public interface IVRageSystem
  {
    string GetOsName();

    string GetInfoCPU(out uint frequency, out uint physicalCores);

    ulong GetTotalPhysicalMemory();

    void LogEnvironmentInformation();

    float CPUCounter { get; }

    float RAMCounter { get; }

    void GetGCMemory(out float allocated, out float used);

    long RemainingMemoryForGame { get; }

    long ProcessPrivateMemory { get; }

    string Clipboard { get; set; }

    List<string> GetProcessesLockingFile(string path);

    void ResetColdStartRegister();

    bool IsAllocationProfilingReady { get; }

    ulong GetThreadAllocationStamp();

    ulong GetGlobalAllocationsStamp();

    bool IsSingleInstance { get; }

    event Action<string> OnSystemProtocolActivated;

    string GetAppDataPath();

    void WriteLineToConsole(string msg);

    void LogToExternalDebugger(string message);

    bool IsRemoteDebuggingSupported { get; }

    bool OpenUrl(string url);

    SimulationQuality SimulationQuality { get; }

    bool IsDeprecatedOS { get; }

    bool IsMemoryLimited { get; }

    void OnThreadpoolInitialized();

    void LogRuntimeInfo(Action<string> log);

    void OnSessionStarted(SessionType sessionType);

    void OnSessionUnloaded();

    int? GetExperimentalPCULimit(int safePCULimit);

    string ThreeLetterISORegionName { get; }

    string TwoLetterISORegionName { get; }

    string RegionLatitude { get; }

    string RegionLongitude { get; }

    string TempPath { get; }

    DateTime GetNetworkTimeUTC();

    event Action OnResuming;
  }
}
