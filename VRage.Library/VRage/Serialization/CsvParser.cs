// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.CsvParser
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VRage.Serialization
{
  public static class CsvParser
  {
    private static Tuple<T, IEnumerable<T>> HeadAndTail<T>(
      this IEnumerable<T> source)
    {
      IEnumerator<T> en = source != null ? source.GetEnumerator() : throw new ArgumentNullException(nameof (source));
      en.MoveNext();
      return Tuple.Create<T, IEnumerable<T>>(en.Current, CsvParser.EnumerateTail<T>(en));
    }

    private static IEnumerable<T> EnumerateTail<T>(IEnumerator<T> en)
    {
      while (en.MoveNext())
        yield return en.Current;
    }

    public static IEnumerable<IList<string>> Parse(
      string content,
      char delimiter,
      char qualifier)
    {
      using (StringReader stringReader = new StringReader(content))
        return CsvParser.Parse((TextReader) stringReader, delimiter, qualifier);
    }

    public static Tuple<IList<string>, IEnumerable<IList<string>>> ParseHeadAndTail(
      TextReader reader,
      char delimiter,
      char qualifier)
    {
      return CsvParser.Parse(reader, delimiter, qualifier).HeadAndTail<IList<string>>();
    }

    public static IEnumerable<IList<string>> Parse(
      TextReader reader,
      char delimiter,
      char qualifier)
    {
      bool inQuote = false;
      List<string> record = new List<string>();
      StringBuilder sb = new StringBuilder();
      while (reader.Peek() != -1)
      {
        char c = (char) reader.Read();
        switch (c)
        {
          case '\n':
            if (c == '\r')
              reader.Read();
            if (inQuote)
            {
              if (c == '\r')
                sb.Append('\r');
              sb.Append('\n');
              continue;
            }
            if (record.Count > 0 || sb.Length > 0)
            {
              record.Add(sb.ToString());
              sb.Clear();
            }
            if (record.Count > 0)
              yield return (IList<string>) record;
            record = new List<string>(record.Count);
            continue;
          case '\r':
            if ((ushort) reader.Peek() != (ushort) 10)
              break;
            goto case '\n';
        }
        if (sb.Length == 0 && !inQuote)
        {
          if ((int) c == (int) qualifier)
            inQuote = true;
          else if ((int) c == (int) delimiter)
          {
            record.Add(sb.ToString());
            sb.Clear();
          }
          else if (!char.IsWhiteSpace(c))
            sb.Append(c);
        }
        else if ((int) c == (int) delimiter)
        {
          if (inQuote)
          {
            sb.Append(delimiter);
          }
          else
          {
            record.Add(sb.ToString());
            sb.Clear();
          }
        }
        else if ((int) c == (int) qualifier)
        {
          if (inQuote)
          {
            if ((int) (ushort) reader.Peek() == (int) qualifier)
            {
              reader.Read();
              sb.Append(qualifier);
            }
            else
              inQuote = false;
          }
          else
            sb.Append(c);
        }
        else
          sb.Append(c);
      }
      if (record.Count > 0 || sb.Length > 0)
        record.Add(sb.ToString());
      if (record.Count > 0)
        yield return (IList<string>) record;
    }
  }
}
