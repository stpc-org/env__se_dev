// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyDefinitionErrors
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
  public static class MyDefinitionErrors
  {
    private static readonly object m_lockObject = new object();
    private static readonly List<MyDefinitionErrors.Error> m_errors = new List<MyDefinitionErrors.Error>();
    private static readonly MyDefinitionErrors.ErrorComparer m_comparer = new MyDefinitionErrors.ErrorComparer();

    public static bool ShouldShowModErrors { get; set; }

    public static void Clear()
    {
      lock (MyDefinitionErrors.m_lockObject)
        MyDefinitionErrors.m_errors.Clear();
    }

    public static void Add(
      MyModContext context,
      string message,
      TErrorSeverity severity,
      bool writeToLog = true)
    {
      MyDefinitionErrors.Error e = new MyDefinitionErrors.Error()
      {
        ModName = context.ModName,
        ErrorFile = context.CurrentFile,
        Message = message,
        Severity = severity
      };
      lock (MyDefinitionErrors.m_lockObject)
        MyDefinitionErrors.m_errors.Add(e);
      string modName = context.ModName;
      if (writeToLog)
        MyDefinitionErrors.WriteError(e);
      if (severity != TErrorSeverity.Critical)
        return;
      MyDefinitionErrors.ShouldShowModErrors = true;
    }

    public static ListReader<MyDefinitionErrors.Error> GetErrors()
    {
      lock (MyDefinitionErrors.m_lockObject)
      {
        MyDefinitionErrors.m_errors.Sort((IComparer<MyDefinitionErrors.Error>) MyDefinitionErrors.m_comparer);
        return new ListReader<MyDefinitionErrors.Error>(MyDefinitionErrors.m_errors);
      }
    }

    public static void WriteError(MyDefinitionErrors.Error e)
    {
      MyLog.Default.WriteLine(string.Format("{0}: {1}", (object) e.ErrorSeverity, (object) (e.ModName ?? string.Empty)));
      MyLog.Default.WriteLine("  in file: " + e.ErrorFile);
      MyLog.Default.WriteLine("  " + e.Message);
    }

    public class Error
    {
      public string ModName;
      public string ErrorFile;
      public string Message;
      public TErrorSeverity Severity;
      private static Color[] severityColors = new Color[4]
      {
        Color.Gray,
        Color.Gray,
        Color.White,
        new Color(1f, 0.25f, 0.1f)
      };
      private static string[] severityName = new string[4]
      {
        "notice",
        "warning",
        "error",
        "critical error"
      };
      private static string[] severityNamePlural = new string[4]
      {
        "notices",
        "warnings",
        "errors",
        "critical errors"
      };

      public string ErrorId => this.ModName != null ? "mod_" : "definition_";

      public string ErrorSeverity
      {
        get
        {
          string str = this.ErrorId;
          switch (this.Severity)
          {
            case TErrorSeverity.Notice:
              str += "notice";
              break;
            case TErrorSeverity.Warning:
              str += "warning";
              break;
            case TErrorSeverity.Error:
              str = (str + "error").ToUpperInvariant();
              break;
            case TErrorSeverity.Critical:
              str = (str + "critical_error").ToUpperInvariant();
              break;
          }
          return str;
        }
      }

      public override string ToString() => string.Format("{0}: {1}, in file: {2}\n{3}", (object) this.ErrorSeverity, (object) (this.ModName ?? string.Empty), (object) this.ErrorFile, (object) this.Message);

      public static Color GetSeverityColor(TErrorSeverity severity)
      {
        try
        {
          return MyDefinitionErrors.Error.severityColors[(int) severity];
        }
        catch (Exception ex)
        {
          MyLog.Default.WriteLine(string.Format("Error type does not have color assigned: message: {0}, stack:{1}", (object) ex.Message, (object) ex.StackTrace));
          return Color.White;
        }
      }

      public static string GetSeverityName(TErrorSeverity severity, bool plural)
      {
        try
        {
          return plural ? MyDefinitionErrors.Error.severityNamePlural[(int) severity] : MyDefinitionErrors.Error.severityName[(int) severity];
        }
        catch (Exception ex)
        {
          MyLog.Default.WriteLine(string.Format("Error type does not have name assigned: message: {0}, stack:{1}", (object) ex.Message, (object) ex.StackTrace));
          return plural ? "Errors" : nameof (Error);
        }
      }

      public Color GetSeverityColor() => MyDefinitionErrors.Error.GetSeverityColor(this.Severity);
    }

    public class ErrorComparer : IComparer<MyDefinitionErrors.Error>
    {
      public int Compare(MyDefinitionErrors.Error x, MyDefinitionErrors.Error y) => y.Severity - x.Severity;
    }
  }
}
