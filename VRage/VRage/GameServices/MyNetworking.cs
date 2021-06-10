// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.MyNetworking
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;

namespace VRage.GameServices
{
  public static class MyNetworking
  {
    public static List<string> CollectNetworkParameters(string[] args)
    {
      List<string> stringList = new List<string>();
      int startIndex = 0;
      int num;
      while ((num = Array.IndexOf<string>(args, "-np", startIndex)) > 0)
      {
        startIndex = num + 1;
        if (startIndex < args.Length)
          stringList.Add(args[startIndex]);
      }
      return stringList;
    }
  }
}
