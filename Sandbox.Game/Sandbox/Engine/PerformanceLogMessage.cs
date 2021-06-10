// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.PerformanceLogMessage
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace Sandbox.Engine
{
  [Serializable]
  public class PerformanceLogMessage
  {
    public double Time { get; set; }

    public float ReceivedPerSecond { get; set; }

    public float SentPerSecond { get; set; }

    public float PeakReceivedPerSecond { get; set; }

    public float PeakSentPerSecond { get; set; }

    public float OverallReceived { get; set; }

    public float OverallSent { get; set; }

    public float CPULoadSmooth { get; set; }

    public float ThreadLoadSmooth { get; set; }

    public int GetOnlinePlayerCount { get; set; }

    public float Ping { get; set; }

    public float GCMemoryUsed { get; set; }

    public float ProcessMemory { get; set; }

    public float PCUBuilt { get; set; }

    public float PCU { get; set; }

    public float GridsCount { get; set; }

    public float RenderCPULoadSmooth { get; set; }

    public float RenderGPULoadSmooth { get; set; }

    public float HardwareCPULoad { get; set; }

    public float HardwareAvailableMemory { get; set; }

    public float FrameTime { get; set; }

    public float LowSimQuality { get; set; }

    public float FrameTimeLimit { get; set; }

    public float FrameTimeCPU { get; set; }

    public float FrameTimeGPU { get; set; }

    public float CPULoadLimit { get; set; }

    public float TrackedMemory { get; set; }

    public float GCMemoryAllocated { get; set; }

    public int GameVersion { get; set; }

    public string ExePath { get; set; }

    public string SavePath { get; set; }

    public static string GetEndOfTestString() => "EndOfTest_WriteResults";

    public static string GetPerfLognamePrefix() => "perfLog_";

    public static string SerializeObject<T>(T toSerialize)
    {
      using (StringWriter stringWriter = new StringWriter())
      {
        new XmlSerializer(toSerialize.GetType()).Serialize((TextWriter) stringWriter, (object) toSerialize);
        stringWriter.ToString();
        return stringWriter.ToString();
      }
    }

    public static PerformanceLogMessage DeserializeObject(string str)
    {
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (PerformanceLogMessage));
      if (!str.StartsWith("<" + typeof (PerformanceLogMessage).Name))
        return (PerformanceLogMessage) null;
      using (StringReader stringReader = new StringReader(str))
        return (PerformanceLogMessage) xmlSerializer.Deserialize((TextReader) stringReader);
    }

    public static PerformanceLogMessage DeserializeCSVline(
      string propertyNames,
      string values)
    {
      string[] strArray1 = values.Split(',');
      string[] strArray2 = propertyNames.Split(',');
      if (((IEnumerable<string>) strArray1).Count<string>() != ((IEnumerable<string>) strArray2).Count<string>())
        return (PerformanceLogMessage) null;
      PerformanceLogMessage performanceLogMessage = new PerformanceLogMessage();
      int index = 0;
      foreach (string str in strArray1)
      {
        PropertyInfo property = typeof (PerformanceLogMessage).GetProperty(strArray2[index]);
        string name = property.PropertyType.Name;
        if (!(name == "Single"))
        {
          if (!(name == "Float"))
          {
            if (!(name == "Int32"))
            {
              if (!(name == "Nullable`1"))
              {
                if (name == "Double")
                  property.SetValue((object) performanceLogMessage, (object) Convert.ToDouble(str), (object[]) null);
                else
                  property.SetValue((object) performanceLogMessage, (object) str, (object[]) null);
              }
              else
                property.SetValue((object) performanceLogMessage, (object) Convert.ToInt32(str), (object[]) null);
            }
            else
              property.SetValue((object) performanceLogMessage, (object) Convert.ToInt32(str), (object[]) null);
          }
          else
            property.SetValue((object) performanceLogMessage, (object) Convert.ToDecimal(str), (object[]) null);
        }
        else
          property.SetValue((object) performanceLogMessage, (object) Convert.ToSingle(str), (object[]) null);
        ++index;
      }
      return performanceLogMessage;
    }

    protected class Sandbox_Engine_PerformanceLogMessage\u003C\u003ETime\u003C\u003EAccessor : IMemberAccessor<PerformanceLogMessage, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerformanceLogMessage owner, in double value) => owner.Time = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerformanceLogMessage owner, out double value) => value = owner.Time;
    }

    protected class Sandbox_Engine_PerformanceLogMessage\u003C\u003EReceivedPerSecond\u003C\u003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerformanceLogMessage owner, in float value) => owner.ReceivedPerSecond = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerformanceLogMessage owner, out float value) => value = owner.ReceivedPerSecond;
    }

    protected class Sandbox_Engine_PerformanceLogMessage\u003C\u003ESentPerSecond\u003C\u003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerformanceLogMessage owner, in float value) => owner.SentPerSecond = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerformanceLogMessage owner, out float value) => value = owner.SentPerSecond;
    }

    protected class Sandbox_Engine_PerformanceLogMessage\u003C\u003EPeakReceivedPerSecond\u003C\u003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerformanceLogMessage owner, in float value) => owner.PeakReceivedPerSecond = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerformanceLogMessage owner, out float value) => value = owner.PeakReceivedPerSecond;
    }

    protected class Sandbox_Engine_PerformanceLogMessage\u003C\u003EPeakSentPerSecond\u003C\u003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerformanceLogMessage owner, in float value) => owner.PeakSentPerSecond = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerformanceLogMessage owner, out float value) => value = owner.PeakSentPerSecond;
    }

    protected class Sandbox_Engine_PerformanceLogMessage\u003C\u003EOverallReceived\u003C\u003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerformanceLogMessage owner, in float value) => owner.OverallReceived = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerformanceLogMessage owner, out float value) => value = owner.OverallReceived;
    }

    protected class Sandbox_Engine_PerformanceLogMessage\u003C\u003EOverallSent\u003C\u003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerformanceLogMessage owner, in float value) => owner.OverallSent = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerformanceLogMessage owner, out float value) => value = owner.OverallSent;
    }

    protected class Sandbox_Engine_PerformanceLogMessage\u003C\u003ECPULoadSmooth\u003C\u003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerformanceLogMessage owner, in float value) => owner.CPULoadSmooth = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerformanceLogMessage owner, out float value) => value = owner.CPULoadSmooth;
    }

    protected class Sandbox_Engine_PerformanceLogMessage\u003C\u003EThreadLoadSmooth\u003C\u003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerformanceLogMessage owner, in float value) => owner.ThreadLoadSmooth = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerformanceLogMessage owner, out float value) => value = owner.ThreadLoadSmooth;
    }

    protected class Sandbox_Engine_PerformanceLogMessage\u003C\u003EGetOnlinePlayerCount\u003C\u003EAccessor : IMemberAccessor<PerformanceLogMessage, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerformanceLogMessage owner, in int value) => owner.GetOnlinePlayerCount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerformanceLogMessage owner, out int value) => value = owner.GetOnlinePlayerCount;
    }

    protected class Sandbox_Engine_PerformanceLogMessage\u003C\u003EPing\u003C\u003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerformanceLogMessage owner, in float value) => owner.Ping = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerformanceLogMessage owner, out float value) => value = owner.Ping;
    }

    protected class Sandbox_Engine_PerformanceLogMessage\u003C\u003EGCMemoryUsed\u003C\u003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerformanceLogMessage owner, in float value) => owner.GCMemoryUsed = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerformanceLogMessage owner, out float value) => value = owner.GCMemoryUsed;
    }

    protected class Sandbox_Engine_PerformanceLogMessage\u003C\u003EProcessMemory\u003C\u003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerformanceLogMessage owner, in float value) => owner.ProcessMemory = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerformanceLogMessage owner, out float value) => value = owner.ProcessMemory;
    }

    protected class Sandbox_Engine_PerformanceLogMessage\u003C\u003EPCUBuilt\u003C\u003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerformanceLogMessage owner, in float value) => owner.PCUBuilt = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerformanceLogMessage owner, out float value) => value = owner.PCUBuilt;
    }

    protected class Sandbox_Engine_PerformanceLogMessage\u003C\u003EPCU\u003C\u003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerformanceLogMessage owner, in float value) => owner.PCU = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerformanceLogMessage owner, out float value) => value = owner.PCU;
    }

    protected class Sandbox_Engine_PerformanceLogMessage\u003C\u003EGridsCount\u003C\u003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerformanceLogMessage owner, in float value) => owner.GridsCount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerformanceLogMessage owner, out float value) => value = owner.GridsCount;
    }

    protected class Sandbox_Engine_PerformanceLogMessage\u003C\u003ERenderCPULoadSmooth\u003C\u003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerformanceLogMessage owner, in float value) => owner.RenderCPULoadSmooth = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerformanceLogMessage owner, out float value) => value = owner.RenderCPULoadSmooth;
    }

    protected class Sandbox_Engine_PerformanceLogMessage\u003C\u003ERenderGPULoadSmooth\u003C\u003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerformanceLogMessage owner, in float value) => owner.RenderGPULoadSmooth = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerformanceLogMessage owner, out float value) => value = owner.RenderGPULoadSmooth;
    }

    protected class Sandbox_Engine_PerformanceLogMessage\u003C\u003EHardwareCPULoad\u003C\u003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerformanceLogMessage owner, in float value) => owner.HardwareCPULoad = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerformanceLogMessage owner, out float value) => value = owner.HardwareCPULoad;
    }

    protected class Sandbox_Engine_PerformanceLogMessage\u003C\u003EHardwareAvailableMemory\u003C\u003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerformanceLogMessage owner, in float value) => owner.HardwareAvailableMemory = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerformanceLogMessage owner, out float value) => value = owner.HardwareAvailableMemory;
    }

    protected class Sandbox_Engine_PerformanceLogMessage\u003C\u003EFrameTime\u003C\u003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerformanceLogMessage owner, in float value) => owner.FrameTime = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerformanceLogMessage owner, out float value) => value = owner.FrameTime;
    }

    protected class Sandbox_Engine_PerformanceLogMessage\u003C\u003ELowSimQuality\u003C\u003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerformanceLogMessage owner, in float value) => owner.LowSimQuality = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerformanceLogMessage owner, out float value) => value = owner.LowSimQuality;
    }

    protected class Sandbox_Engine_PerformanceLogMessage\u003C\u003EFrameTimeLimit\u003C\u003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerformanceLogMessage owner, in float value) => owner.FrameTimeLimit = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerformanceLogMessage owner, out float value) => value = owner.FrameTimeLimit;
    }

    protected class Sandbox_Engine_PerformanceLogMessage\u003C\u003EFrameTimeCPU\u003C\u003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerformanceLogMessage owner, in float value) => owner.FrameTimeCPU = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerformanceLogMessage owner, out float value) => value = owner.FrameTimeCPU;
    }

    protected class Sandbox_Engine_PerformanceLogMessage\u003C\u003EFrameTimeGPU\u003C\u003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerformanceLogMessage owner, in float value) => owner.FrameTimeGPU = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerformanceLogMessage owner, out float value) => value = owner.FrameTimeGPU;
    }

    protected class Sandbox_Engine_PerformanceLogMessage\u003C\u003ECPULoadLimit\u003C\u003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerformanceLogMessage owner, in float value) => owner.CPULoadLimit = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerformanceLogMessage owner, out float value) => value = owner.CPULoadLimit;
    }

    protected class Sandbox_Engine_PerformanceLogMessage\u003C\u003ETrackedMemory\u003C\u003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerformanceLogMessage owner, in float value) => owner.TrackedMemory = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerformanceLogMessage owner, out float value) => value = owner.TrackedMemory;
    }

    protected class Sandbox_Engine_PerformanceLogMessage\u003C\u003EGCMemoryAllocated\u003C\u003EAccessor : IMemberAccessor<PerformanceLogMessage, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerformanceLogMessage owner, in float value) => owner.GCMemoryAllocated = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerformanceLogMessage owner, out float value) => value = owner.GCMemoryAllocated;
    }

    protected class Sandbox_Engine_PerformanceLogMessage\u003C\u003EGameVersion\u003C\u003EAccessor : IMemberAccessor<PerformanceLogMessage, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerformanceLogMessage owner, in int value) => owner.GameVersion = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerformanceLogMessage owner, out int value) => value = owner.GameVersion;
    }

    protected class Sandbox_Engine_PerformanceLogMessage\u003C\u003EExePath\u003C\u003EAccessor : IMemberAccessor<PerformanceLogMessage, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerformanceLogMessage owner, in string value) => owner.ExePath = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerformanceLogMessage owner, out string value) => value = owner.ExePath;
    }

    protected class Sandbox_Engine_PerformanceLogMessage\u003C\u003ESavePath\u003C\u003EAccessor : IMemberAccessor<PerformanceLogMessage, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerformanceLogMessage owner, in string value) => owner.SavePath = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerformanceLogMessage owner, out string value) => value = owner.SavePath;
    }
  }
}
