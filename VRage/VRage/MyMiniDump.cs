// Decompiled with JetBrains decompiler
// Type: VRage.MyMiniDump
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using VRage.FileSystem;

namespace VRage
{
  public static class MyMiniDump
  {
    [ThreadStatic]
    private static long m_lastDumpTimestamp;

    public static void CollectExceptionDump(Exception ex)
    {
      DateTime utcNow = DateTime.UtcNow;
      DateTime dateTime = new DateTime(MyMiniDump.m_lastDumpTimestamp);
      if ((utcNow - dateTime).TotalSeconds <= 15.0)
        return;
      MyVRage.Platform.CrashReporting.WriteMiniDump(Path.Combine(MyFileSystem.UserDataPath, "MinidumpT" + (object) Thread.CurrentThread.ManagedThreadId + ".dmp"), MyMiniDump.Options.WithProcessThreadData | MyMiniDump.Options.WithThreadInfo, IntPtr.Zero);
      MyMiniDump.m_lastDumpTimestamp = utcNow.Ticks;
    }

    public static void CollectCrashDump(IntPtr exceptionPointers) => MyVRage.Platform.CrashReporting.WriteMiniDump(Path.Combine(MyFileSystem.UserDataPath, "Minidump.dmp"), MyMiniDump.Options.WithProcessThreadData | MyMiniDump.Options.WithThreadInfo, exceptionPointers);

    public static void CollectStateDump() => MyVRage.Platform.CrashReporting.WriteMiniDump(Path.Combine(MyFileSystem.UserDataPath, string.Format("Minidump_State_{0:yyyy_MM_dd_HH_mm_ss_fff}.dmp", (object) DateTime.Now)), MyMiniDump.Options.WithProcessThreadData | MyMiniDump.Options.WithThreadInfo, IntPtr.Zero);

    public static IEnumerable<string> FindActiveDumps(string directory)
    {
      DateTime now = DateTime.Now;
      string[] strArray = Directory.GetFiles(directory, "Minidump*.dmp", SearchOption.TopDirectoryOnly);
      for (int index = 0; index < strArray.Length; ++index)
      {
        string path = strArray[index];
        if (path != null && File.Exists(path) && (File.GetCreationTime(path) - now).Minutes < 5)
          yield return path;
      }
      strArray = (string[]) null;
    }

    public static void CleanupOldDumps()
    {
      HashSet<FileInfo> source = new HashSet<FileInfo>();
      foreach (FileInfo enumerateFile in new DirectoryInfo(MyFileSystem.UserDataPath).EnumerateFiles("Minidump*.dmp", SearchOption.TopDirectoryOnly))
      {
        if (enumerateFile.Name.StartsWith("Minidump_State_"))
          source.Add(enumerateFile);
        else
          enumerateFile.Delete();
      }
      if (source.Count <= 1)
        return;
      source.Remove(source.MaxBy<FileInfo, DateTime>((Func<FileInfo, DateTime>) (x => x.LastWriteTime)));
      foreach (FileSystemInfo fileSystemInfo in source)
        fileSystemInfo.Delete();
    }

    [Flags]
    public enum Options : uint
    {
      Normal = 0,
      WithDataSegs = 1,
      WithFullMemory = 2,
      WithHandleData = 4,
      FilterMemory = 8,
      ScanMemory = 16, // 0x00000010
      WithUnloadedModules = 32, // 0x00000020
      WithIndirectlyReferencedMemory = 64, // 0x00000040
      FilterModulePaths = 128, // 0x00000080
      WithProcessThreadData = 256, // 0x00000100
      WithPrivateReadWriteMemory = 512, // 0x00000200
      WithoutOptionalData = 1024, // 0x00000400
      WithFullMemoryInfo = 2048, // 0x00000800
      WithThreadInfo = 4096, // 0x00001000
      WithCodeSegs = 8192, // 0x00002000
      WithoutAuxiliaryState = 16384, // 0x00004000
      WithFullAuxiliaryState = 32768, // 0x00008000
      WithPrivateWriteCopyMemory = 65536, // 0x00010000
      IgnoreInaccessibleMemory = 131072, // 0x00020000
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct ExceptionInformation
    {
      public uint ThreadId;
      public IntPtr ExceptionPointers;
      [MarshalAs(UnmanagedType.Bool)]
      public bool ClientPointers;
    }
  }
}
