// Decompiled with JetBrains decompiler
// Type: ParallelTasks.TaskException
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Text;

namespace ParallelTasks
{
  public class TaskException : Exception
  {
    public Exception[] InnerExceptions { get; }

    public TaskException(Exception[] inner)
      : base("An exception(s) was thrown while executing a task.", (Exception) null)
      => this.InnerExceptions = inner ?? Array.Empty<Exception>();

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(base.ToString());
      int index = 0;
      while (true)
      {
        int num1 = index;
        Exception[] innerExceptions = this.InnerExceptions;
        int num2 = innerExceptions != null ? innerExceptions.Length : 0;
        if (num1 < num2)
        {
          stringBuilder.AppendFormat("Task exception, inner exception {0}:", (object) index.ToString());
          stringBuilder.AppendLine();
          Exception innerException = this.InnerExceptions[index];
          try
          {
            stringBuilder.Append(innerException.ToString());
          }
          catch
          {
            try
            {
              stringBuilder.Append(innerException.StackTrace);
            }
            catch
            {
              stringBuilder.Append("Inner exception dump failed");
            }
          }
          stringBuilder.AppendLine();
          ++index;
        }
        else
          break;
      }
      return stringBuilder.ToString();
    }
  }
}
