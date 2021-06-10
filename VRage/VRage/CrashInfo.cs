// Decompiled with JetBrains decompiler
// Type: VRage.CrashInfo
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.IO;

namespace VRage
{
  public struct CrashInfo
  {
    public const string StartMark = "================================== CRASH INFO ==================================";
    public const string EndMark = "================================== OFNI HSARC ==================================";
    public string AppVersion;
    public string GameName;
    public string AnalyticId;
    public bool IsOutOfMemory;
    public bool IsGPU;
    public bool IsNative;
    public bool IsTask;
    public bool IsHang;
    public bool IsExperimental;
    public long ProcessRunTime;
    public int PCUCount;
    public long GCMemory;
    public long GCMemoryAllocated;
    public long HWAvailableMemory;
    public long ProcessPrivateMemory;

    public CrashInfo(string appVersion, string gameName, string analyticId)
      : this()
    {
      this.AppVersion = appVersion;
      this.GameName = gameName;
      this.AnalyticId = analyticId;
    }

    public void Write(TextWriter writer)
    {
      writer.WriteLine("================================== CRASH INFO ==================================");
      writer.WriteLine("AppVersion: " + this.AppVersion);
      writer.WriteLine("GameName: " + this.GameName);
      writer.WriteLine(string.Format("IsOutOfMemory: {0}", (object) this.IsOutOfMemory));
      writer.WriteLine(string.Format("IsGPU: {0}", (object) this.IsGPU));
      writer.WriteLine(string.Format("IsNative: {0}", (object) this.IsNative));
      writer.WriteLine(string.Format("IsTask: {0}", (object) this.IsTask));
      writer.WriteLine(string.Format("IsExperimental: {0}", (object) this.IsExperimental));
      writer.WriteLine(string.Format("ProcessRunTime: {0}", (object) this.ProcessRunTime));
      writer.WriteLine(string.Format("PCUCount: {0}", (object) this.PCUCount));
      writer.WriteLine(string.Format("IsHang: {0}", (object) this.IsHang));
      writer.WriteLine(string.Format("GCMemory: {0}", (object) this.GCMemory));
      writer.WriteLine(string.Format("GCMemoryAllocated: {0}", (object) this.GCMemoryAllocated));
      writer.WriteLine(string.Format("HWAvailableMemory: {0}", (object) this.HWAvailableMemory));
      writer.WriteLine(string.Format("ProcessPrivateMemory: {0}", (object) this.ProcessPrivateMemory));
      writer.WriteLine("AnalyticId: " + this.AnalyticId);
      writer.WriteLine("================================== OFNI HSARC ==================================");
    }

    public static CrashInfo Read(TextReader reader)
    {
      string str1;
      while ((str1 = reader.ReadLine()) != "================================== CRASH INFO ==================================")
      {
        if (str1 == null)
          return new CrashInfo();
      }
      CrashInfo crashInfo = new CrashInfo();
      string str2;
      while ((str2 = reader.ReadLine()) != "================================== OFNI HSARC ==================================" && str2 != null)
      {
        int length = str2.IndexOf(':');
        string str3 = str2.Substring(0, length);
        string s = str2.Substring(length + 2);
        switch (str3)
        {
          case "AnalyticId":
            crashInfo.AnalyticId = s;
            continue;
          case "AppVersion":
            crashInfo.AppVersion = s;
            continue;
          case "GCMemory":
            crashInfo.GCMemory = long.Parse(s);
            continue;
          case "GCMemoryAllocated":
            crashInfo.GCMemoryAllocated = long.Parse(s);
            continue;
          case "GameName":
            crashInfo.GameName = s;
            continue;
          case "HWAvailableMemory":
            crashInfo.HWAvailableMemory = long.Parse(s);
            continue;
          case "IsExperimental":
            crashInfo.IsExperimental = bool.Parse(s);
            continue;
          case "IsGPU":
            crashInfo.IsGPU = bool.Parse(s);
            continue;
          case "IsHang":
            crashInfo.IsHang = bool.Parse(s);
            continue;
          case "IsNative":
            crashInfo.IsNative = bool.Parse(s);
            continue;
          case "IsOutOfMemory":
            crashInfo.IsOutOfMemory = bool.Parse(s);
            continue;
          case "IsTask":
            crashInfo.IsTask = bool.Parse(s);
            continue;
          case "PCUCount":
            crashInfo.PCUCount = int.Parse(s);
            continue;
          case "ProcessPrivateMemory":
            crashInfo.ProcessPrivateMemory = long.Parse(s);
            continue;
          case "ProcessRunTime":
            crashInfo.ProcessRunTime = long.Parse(s);
            continue;
          default:
            continue;
        }
      }
      return crashInfo;
    }

    public override string ToString()
    {
      StringWriter stringWriter = new StringWriter();
      this.Write((TextWriter) stringWriter);
      return stringWriter.ToString();
    }
  }
}
