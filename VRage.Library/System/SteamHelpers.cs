// Decompiled with JetBrains decompiler
// Type: System.SteamHelpers
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace System
{
  public static class SteamHelpers
  {
    public static bool IsSteamPath(string path)
    {
      try
      {
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        return directoryInfo.Parent != null && directoryInfo.Parent.Parent != null && directoryInfo.Parent.Name.Equals("Common", StringComparison.InvariantCultureIgnoreCase) && directoryInfo.Parent.Parent.Name.Equals("SteamApps", StringComparison.InvariantCultureIgnoreCase);
      }
      catch
      {
        return false;
      }
    }

    public static bool IsAppManifestPresent(string path, uint appId)
    {
      try
      {
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        return SteamHelpers.IsSteamPath(path) && ((IEnumerable<string>) Directory.GetFiles(directoryInfo.Parent.Parent.FullName)).Contains<string>("AppManifest_" + (object) appId + ".acf", (IEqualityComparer<string>) StringComparer.InvariantCultureIgnoreCase);
      }
      catch
      {
        return false;
      }
    }
  }
}
