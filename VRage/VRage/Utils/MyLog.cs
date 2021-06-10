// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyLog
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using VRage.FileSystem;
using VRage.Library;

namespace VRage.Utils
{
  public class MyLog
  {
    public static Action<MyLogSeverity, StringBuilder> OnLog;
    private bool m_alwaysFlush;
    public static MyLogSeverity AssertLevel = (MyLogSeverity) 255;
    private bool LogForMemoryProfiler;
    private bool m_enabled;
    private Stream m_stream;
    private StreamWriter m_streamWriter;
    private readonly FastResourceLock m_lock = new FastResourceLock();
    private Dictionary<int, int> m_indentsByThread;
    private Dictionary<MyLog.MyLogIndentKey, MyLog.MyLogIndentValue> m_indents;
    private string m_filepath;
    private StringBuilder m_stringBuilder = new StringBuilder(2048);
    private char[] m_tmpWrite = new char[2048];
    private LoggingOptions m_loggingOptions = LoggingOptions.NONE | LoggingOptions.ENUM_CHECKING | LoggingOptions.LOADING_TEXTURES | LoggingOptions.LOADING_CUSTOM_ASSETS | LoggingOptions.LOADING_SPRITE_VIDEO | LoggingOptions.VALIDATING_CUE_PARAMS | LoggingOptions.CONFIG_ACCESS | LoggingOptions.SIMPLE_NETWORKING | LoggingOptions.VOXEL_MAPS | LoggingOptions.MISC_RENDER_ASSETS | LoggingOptions.AUDIO | LoggingOptions.TRAILERS | LoggingOptions.SESSION_SETTINGS;
    private Action<string> m_normalWriter;
    private Action<string> m_closedLogWriter;
    private static MyLog m_default;
    private readonly FastResourceLock m_consoleStringBuilderLock = new FastResourceLock();
    private StringBuilder m_consoleStringBuilder = new StringBuilder();

    public static MyLog Default
    {
      get => MyLog.m_default;
      set => MyLog.m_default = value;
    }

    public LoggingOptions Options
    {
      get => this.m_loggingOptions;
      set => value = this.m_loggingOptions;
    }

    public bool LogEnabled => this.m_enabled;

    public MyLog(bool alwaysFlush = false) => this.m_alwaysFlush = alwaysFlush;

    public void InitWithDate(
      string logNameBaseName,
      StringBuilder appVersionString,
      int maxLogAgeInDays)
    {
      StringBuilder logName = MyLog.GetLogName(logNameBaseName);
      MyLog.RemoveExcessLogs(logNameBaseName, maxLogAgeInDays);
      this.Init(logName.ToString(), appVersionString);
    }

    private static StringBuilder GetLogName(string appName)
    {
      StringBuilder stringBuilder = new StringBuilder(appName);
      stringBuilder.Append("_");
      stringBuilder.Append((object) new StringBuilder().GetFormatedDateTimeForFilename(DateTime.Now, true));
      stringBuilder.Append(".log");
      return stringBuilder;
    }

    private static void RemoveExcessLogs(string appName, int maxLogAgeInDays)
    {
      if (maxLogAgeInDays < 0)
        return;
      DateTime now = DateTime.Now;
      List<FileInfo> fileInfoList = (List<FileInfo>) null;
      foreach (FileInfo enumerateFile in new DirectoryInfo(MyFileSystem.UserDataPath).EnumerateFiles(appName + "*.log"))
      {
        if ((now - enumerateFile.LastWriteTime).TotalDays > (double) maxLogAgeInDays)
        {
          fileInfoList = fileInfoList ?? new List<FileInfo>();
          fileInfoList.Add(enumerateFile);
        }
      }
      if (fileInfoList == null)
        return;
      foreach (FileSystemInfo fileSystemInfo in fileInfoList)
        fileSystemInfo.Delete();
    }

    public void Init(string logFileName, StringBuilder appVersionString)
    {
      int num;
      using (this.m_lock.AcquireExclusiveUsing())
      {
        try
        {
          this.m_filepath = Path.IsPathRooted(logFileName) ? logFileName : Path.Combine(MyFileSystem.UserDataPath, logFileName);
          this.m_stream = MyFileSystem.OpenWrite(this.m_filepath);
          this.m_streamWriter = new StreamWriter(this.m_stream, (Encoding) new UTF8Encoding(false, false));
          this.m_normalWriter = new Action<string>(this.WriteLine);
          this.m_closedLogWriter = (Action<string>) (text =>
          {
            try
            {
              using (this.m_lock.AcquireExclusiveUsing())
                File.AppendAllText(this.m_filepath, text + MyEnvironment.NewLine);
            }
            catch
            {
            }
          });
          this.m_enabled = true;
        }
        catch (Exception ex)
        {
          Trace.Fail("Cannot create log file: " + ex.ToString());
        }
        this.m_indentsByThread = new Dictionary<int, int>();
        this.m_indents = new Dictionary<MyLog.MyLogIndentKey, MyLog.MyLogIndentValue>();
        num = (int) Math.Round((DateTime.Now - DateTime.UtcNow).TotalHours);
      }
      this.WriteLine("Log Started");
      this.WriteLine(string.Format("Timezone (local - UTC): {0}h", (object) num));
      this.WriteLineAndConsole("App Version: " + (object) appVersionString);
    }

    public string GetFilePath()
    {
      using (this.m_lock.AcquireExclusiveUsing())
        return this.m_filepath;
    }

    public MyLog.IndentToken IndentUsing(LoggingOptions options = LoggingOptions.NONE) => new MyLog.IndentToken(this, options);

    public void IncreaseIndent(LoggingOptions option)
    {
      if (!this.LogFlag(option))
        return;
      this.IncreaseIndent();
    }

    public void IncreaseIndent()
    {
      if (!this.m_enabled)
        return;
      using (this.m_lock.AcquireExclusiveUsing())
      {
        if (!this.m_enabled)
          return;
        int threadId = this.GetThreadId();
        this.m_indentsByThread[threadId] = this.GetIdentByThread(threadId) + 1;
        this.m_indents[new MyLog.MyLogIndentKey(threadId, this.m_indentsByThread[threadId])] = new MyLog.MyLogIndentValue(this.GetManagedMemory(), this.GetSystemMemory(), DateTimeOffset.Now);
        if (!this.LogForMemoryProfiler)
          return;
        MyMemoryLogs.StartEvent();
      }
    }

    public bool IsIndentKeyIncreased()
    {
      if (!this.m_enabled)
        return false;
      using (this.m_lock.AcquireExclusiveUsing())
      {
        if (!this.m_enabled)
          return false;
        int threadId = this.GetThreadId();
        return this.m_indents.ContainsKey(new MyLog.MyLogIndentKey(threadId, this.GetIdentByThread(threadId)));
      }
    }

    public void DecreaseIndent(LoggingOptions option)
    {
      if (!this.LogFlag(option))
        return;
      this.DecreaseIndent();
    }

    public void DecreaseIndent()
    {
      if (!this.m_enabled)
        return;
      using (this.m_lock.AcquireExclusiveUsing())
      {
        if (!this.m_enabled)
          return;
        int threadId = this.GetThreadId();
        MyLog.MyLogIndentValue indent = this.m_indents[new MyLog.MyLogIndentKey(threadId, this.GetIdentByThread(threadId))];
        if (this.LogForMemoryProfiler)
          MyMemoryLogs.EndEvent(new MyMemoryLogs.MyMemoryEvent()
          {
            DeltaTime = (float) (DateTimeOffset.Now - indent.LastDateTimeOffset).TotalMilliseconds / 1000f,
            ManagedEndSize = (float) this.GetManagedMemory(),
            ProcessEndSize = (float) this.GetSystemMemory(),
            ManagedStartSize = (float) indent.LastGcTotalMemory,
            ProcessStartSize = (float) indent.LastWorkingSet
          });
      }
      using (this.m_lock.AcquireExclusiveUsing())
      {
        int threadId = this.GetThreadId();
        this.m_indentsByThread[threadId] = this.GetIdentByThread(threadId) - 1;
      }
    }

    private string GetFormatedMemorySize(long bytesCount) => MyValueFormatter.GetFormatedFloat((float) ((double) bytesCount / 1024.0 / 1024.0), 3) + " Mb (" + MyValueFormatter.GetFormatedLong(bytesCount) + " bytes)";

    private long GetManagedMemory()
    {
      float used;
      MyVRage.Platform.System.GetGCMemory(out float _, out used);
      return (long) used;
    }

    private long GetSystemMemory() => MyEnvironment.WorkingSetForMyLog;

    public void Close()
    {
      if (!this.m_enabled)
        return;
      this.WriteLine("Log Closed");
      using (this.m_lock.AcquireExclusiveUsing())
      {
        if (!this.m_enabled)
          return;
        this.m_streamWriter.Close();
        this.m_stream.Close();
        this.m_stream = (Stream) null;
        this.m_streamWriter = (StreamWriter) null;
        this.m_enabled = false;
      }
    }

    public void AppendToClosedLog(string text)
    {
      if (this.m_enabled)
      {
        this.WriteLine(text);
      }
      else
      {
        if (this.m_filepath == null)
          return;
        this.m_closedLogWriter(text);
      }
    }

    public void AppendToClosedLog(Exception e)
    {
      if (this.m_enabled)
      {
        this.WriteLine(e);
      }
      else
      {
        if (this.m_filepath == null)
          return;
        MyLog.WriteLine(this.m_closedLogWriter, e);
      }
    }

    public bool LogFlag(LoggingOptions option) => (uint) (this.m_loggingOptions & option) > 0U;

    public void WriteLine(string message, LoggingOptions option)
    {
      if (!this.LogFlag(option))
        return;
      this.WriteLine(message);
    }

    private static void WriteLine(Action<string> writer, Exception ex)
    {
      writer("Exception occurred: " + (ex == null ? "null" : ex.ToString()));
      if (ex is ReflectionTypeLoadException typeLoadException)
      {
        writer("LoaderExceptions: ");
        foreach (Exception loaderException in typeLoadException.LoaderExceptions)
          MyLog.WriteLine(writer, loaderException);
      }
      if (ex != null && ex.Data.Count > 0)
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("Exception Data:");
        foreach (object key in (IEnumerable) ex.Data.Keys)
          stringBuilder.AppendFormat("\n\t{0}: {1}", key, ex.Data[key]);
        writer(stringBuilder.ToString());
      }
      if (ex?.InnerException == null)
        return;
      writer("InnerException: ");
      MyLog.WriteLine(writer, ex.InnerException);
    }

    public void WriteLine(Exception ex)
    {
      if (!this.m_enabled)
        return;
      MyLog.WriteLine(this.m_normalWriter, ex);
      this.m_streamWriter.Flush();
    }

    public void WriteLineAndConsole(string msg)
    {
      this.WriteLine(msg);
      this.WriteLineToConsole(msg);
    }

    public void WriteLineToConsole(string msg)
    {
      using (this.m_consoleStringBuilderLock.AcquireExclusiveUsing())
      {
        this.m_consoleStringBuilder.Clear();
        this.AppendDateAndTime(this.m_consoleStringBuilder);
        this.m_consoleStringBuilder.Append(": ");
        this.m_consoleStringBuilder.Append(msg);
        MyVRage.Platform?.System.WriteLineToConsole(this.m_consoleStringBuilder.ToString());
      }
    }

    public void WriteLine(string msg)
    {
      if (this.m_enabled)
      {
        using (this.m_lock.AcquireExclusiveUsing())
        {
          if (this.m_enabled)
          {
            this.WriteDateTimeAndThreadId();
            this.WriteString(msg);
            this.m_streamWriter.WriteLine();
            if (this.m_alwaysFlush)
              this.m_streamWriter.Flush();
          }
        }
      }
      if (!this.LogForMemoryProfiler)
        return;
      MyMemoryLogs.AddConsoleLine(msg);
    }

    public void WriteToLogAndAssert(string message) => this.WriteLine(message);

    public TextWriter GetTextWriter() => (TextWriter) this.m_streamWriter;

    private string GetGCMemoryString(string prependText = "") => string.Format("{0}: GC Memory: {1} B", (object) prependText, (object) this.GetManagedMemory().ToString("##,#"));

    public void WriteMemoryUsage(string prefixText) => this.WriteLine(this.GetGCMemoryString(prefixText));

    public void LogThreadPoolInfo()
    {
      if (!this.m_enabled)
        return;
      this.WriteLine("LogThreadPoolInfo - START");
      this.IncreaseIndent();
      int workerThreads;
      int completionPortThreads;
      ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);
      this.WriteLine("GetMaxThreads.WorkerThreads: " + (object) workerThreads);
      this.WriteLine("GetMaxThreads.CompletionPortThreads: " + (object) completionPortThreads);
      ThreadPool.GetMinThreads(out workerThreads, out completionPortThreads);
      this.WriteLine("GetMinThreads.WorkerThreads: " + (object) workerThreads);
      this.WriteLine("GetMinThreads.CompletionPortThreads: " + (object) completionPortThreads);
      ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
      this.WriteLine("GetAvailableThreads.WorkerThreads: " + (object) workerThreads);
      this.WriteLine("GetAvailableThreads.WompletionPortThreads: " + (object) completionPortThreads);
      this.DecreaseIndent();
      this.WriteLine("LogThreadPoolInfo - END");
    }

    private void WriteDateTimeAndThreadId()
    {
      this.m_stringBuilder.Clear();
      this.AppendDateAndTime(this.m_stringBuilder);
      this.m_stringBuilder.Append(" - ");
      this.m_stringBuilder.Append("Thread: ");
      this.m_stringBuilder.Concat(this.GetThreadId(), 3U, ' ');
      this.m_stringBuilder.Append(" ->  ");
      this.m_stringBuilder.Append(' ', this.GetIdentByThread(this.GetThreadId()) * 3);
      this.WriteStringBuilder(this.m_stringBuilder);
    }

    private void AppendDateAndTime(StringBuilder sb)
    {
      DateTimeOffset now = DateTimeOffset.Now;
      sb.Concat(now.Year, 4U, '0', 10U, false).Append('-');
      sb.Concat(now.Month, 2U).Append('-');
      sb.Concat(now.Day, 2U).Append(' ');
      sb.Concat(now.Hour, 2U).Append(':');
      sb.Concat(now.Minute, 2U).Append(':');
      sb.Concat(now.Second, 2U).Append('.');
      sb.Concat(now.Millisecond, 3U);
    }

    private void WriteString(string text)
    {
      if (text == null)
        text = "UNKNOWN ERROR: Text is null in MyLog.WriteString()!";
      try
      {
        this.m_streamWriter.Write(text);
      }
      catch (Exception ex)
      {
        this.m_streamWriter.Write("Error: The string is corrupted and cannot be displayed");
      }
    }

    private void WriteStringBuilder(StringBuilder sb)
    {
      if (sb == null || this.m_tmpWrite == null || this.m_streamWriter == null)
        return;
      if (this.m_tmpWrite.Length < sb.Length)
        Array.Resize<char>(ref this.m_tmpWrite, Math.Max(this.m_tmpWrite.Length * 2, sb.Length));
      sb.CopyTo(0, this.m_tmpWrite, 0, sb.Length);
      try
      {
        this.m_streamWriter.Write(this.m_tmpWrite, 0, sb.Length);
        Array.Clear((Array) this.m_tmpWrite, 0, sb.Length);
      }
      catch (Exception ex)
      {
        this.m_streamWriter.Write("Error: The string is corrupted and cannot be written");
        Array.Clear((Array) this.m_tmpWrite, 0, this.m_tmpWrite.Length);
      }
    }

    private int GetThreadId() => Thread.CurrentThread.ManagedThreadId;

    private int GetIdentByThread(int threadId)
    {
      int num;
      if (!this.m_indentsByThread.TryGetValue(threadId, out num))
        num = 0;
      return num;
    }

    public void Log(MyLogSeverity severity, string format, params object[] args)
    {
      if (!this.m_enabled)
        return;
      using (this.m_lock.AcquireExclusiveUsing())
      {
        if (!this.m_enabled)
          return;
        this.WriteDateTimeAndThreadId();
        StringBuilder stringBuilder = this.m_stringBuilder;
        stringBuilder.Clear();
        stringBuilder.AppendFormat("{0}: ", (object) severity);
        try
        {
          stringBuilder.AppendFormat(format, args);
        }
        catch
        {
          stringBuilder.Append(format).Append(' ');
          stringBuilder.Append(string.Join(";", args ?? Array.Empty<object>()));
        }
        stringBuilder.Append('\n');
        this.WriteStringBuilder(stringBuilder);
        Action<MyLogSeverity, StringBuilder> onLog = MyLog.OnLog;
        if (onLog != null)
          onLog(severity, stringBuilder);
        if (severity >= MyLogSeverity.Warning)
          MyVRage.Platform.System.LogToExternalDebugger(stringBuilder.ToString());
        if (severity < MyLog.AssertLevel)
          return;
        Trace.Fail(stringBuilder.ToString());
      }
    }

    public void Log(MyLogSeverity severity, StringBuilder builder)
    {
      Action<MyLogSeverity, StringBuilder> onLog = MyLog.OnLog;
      if (onLog != null)
        onLog(severity, builder);
      if (!this.m_enabled)
        return;
      using (this.m_lock.AcquireExclusiveUsing())
      {
        if (!this.m_enabled)
          return;
        this.WriteDateTimeAndThreadId();
        StringBuilder stringBuilder = this.m_stringBuilder;
        stringBuilder.Clear();
        stringBuilder.AppendFormat("{0}: ", (object) severity);
        stringBuilder.AppendStringBuilder(builder);
        stringBuilder.Append('\n');
        this.WriteStringBuilder(stringBuilder);
        if (severity >= MyLogSeverity.Warning)
          MyVRage.Platform.System.LogToExternalDebugger(stringBuilder.ToString());
        if (severity < MyLog.AssertLevel)
          return;
        Trace.Fail(stringBuilder.ToString());
      }
    }

    public void Flush() => this.m_streamWriter.Flush();

    public struct IndentToken : IDisposable
    {
      private MyLog m_log;
      private LoggingOptions m_options;

      internal IndentToken(MyLog log, LoggingOptions options)
      {
        this.m_log = log;
        this.m_options = options;
        this.m_log.IncreaseIndent(options);
      }

      public void Dispose()
      {
        if (this.m_log == null)
          return;
        this.m_log.DecreaseIndent(this.m_options);
        this.m_log = (MyLog) null;
      }
    }

    private struct MyLogIndentKey
    {
      public int ThreadId;
      public int Indent;

      public MyLogIndentKey(int threadId, int indent)
      {
        this.ThreadId = threadId;
        this.Indent = indent;
      }
    }

    private struct MyLogIndentValue
    {
      public long LastGcTotalMemory;
      public long LastWorkingSet;
      public DateTimeOffset LastDateTimeOffset;

      public MyLogIndentValue(
        long lastGcTotalMemory,
        long lastWorkingSet,
        DateTimeOffset lastDateTimeOffset)
      {
        this.LastGcTotalMemory = lastGcTotalMemory;
        this.LastWorkingSet = lastWorkingSet;
        this.LastDateTimeOffset = lastDateTimeOffset;
      }
    }
  }
}
