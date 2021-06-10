// Decompiled with JetBrains decompiler
// Type: VRage.FileSystem.MyFileHelper
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.IO;

namespace VRage.FileSystem
{
  public class MyFileHelper
  {
    public static bool CanWrite(string path)
    {
      if (!File.Exists(path))
        return true;
      try
      {
        using (File.Open(path, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
          return true;
      }
      catch
      {
        return false;
      }
    }
  }
}
