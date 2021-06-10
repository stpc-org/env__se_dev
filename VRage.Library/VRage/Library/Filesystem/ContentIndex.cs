// Decompiled with JetBrains decompiler
// Type: VRage.Library.Filesystem.ContentIndex
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;
using System.IO;

namespace VRage.Library.Filesystem
{
  public static class ContentIndex
  {
    private static readonly Dictionary<string, ContentIndex.FileData> Files = new Dictionary<string, ContentIndex.FileData>((IEqualityComparer<string>) StringComparer.InvariantCultureIgnoreCase);

    public static bool IsLoaded { get; private set; }

    public static bool TryGetImageSize(string path, out int width, out int height)
    {
      ContentIndex.FileData fileData;
      if (!string.IsNullOrEmpty(path) && ContentIndex.Files.TryGetValue(path, out fileData) && fileData.IsImage)
      {
        width = fileData.Width;
        height = fileData.Height;
        return true;
      }
      width = height = 0;
      return false;
    }

    public static void Load(string indexFile)
    {
      using (FileStream fileStream = File.OpenRead(indexFile))
      {
        using (StreamReader streamReader = new StreamReader((Stream) fileStream))
        {
          while (!streamReader.EndOfStream)
          {
            string line = streamReader.ReadLine();
            ContentIndex.FileData data;
            string filePath;
            switch (line[0])
            {
              case 'G':
                ContentIndex.ReadGenericFile(line, out data, out filePath);
                break;
              case 'I':
                ContentIndex.ReadImageFile(line, out data, out filePath);
                break;
              default:
                continue;
            }
            ContentIndex.Files.Add(filePath, data);
          }
        }
      }
      ContentIndex.IsLoaded = true;
    }

    private static void ReadGenericFile(
      string line,
      out ContentIndex.FileData data,
      out string filePath)
    {
      data = new ContentIndex.FileData();
      filePath = line.Substring(2);
    }

    private static void ReadImageFile(
      string line,
      out ContentIndex.FileData data,
      out string filePath)
    {
      int num1 = line.IndexOf(' ', 2);
      int num2 = line.IndexOf(' ', num1 + 1);
      int width = int.Parse(line.Substring(2, num1 - 2));
      int height = int.Parse(line.Substring(num1 + 1, num2 - num1 - 1));
      filePath = line.Substring(num2 + 1);
      data = new ContentIndex.FileData(width, height, true);
    }

    public static bool FileExists(string contentPath) => ContentIndex.Files.ContainsKey(contentPath);

    private readonly struct FileData
    {
      public readonly int Width;
      public readonly int Height;
      public readonly bool IsImage;

      public FileData(int width, int height, bool isImage)
      {
        this.Width = width;
        this.Height = height;
        this.IsImage = isImage;
      }
    }
  }
}
