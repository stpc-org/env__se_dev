// Decompiled with JetBrains decompiler
// Type: VRage.Algorithms.MyUFTest
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Diagnostics;
using System.IO;

namespace VRage.Algorithms
{
  public static class MyUFTest
  {
    public static void Test()
    {
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      int count = 10000000;
      MyUnionFind myUnionFind = new MyUnionFind();
      myUnionFind.Resize(count);
      for (int a = 0; a < count; ++a)
        myUnionFind.Union(a, a >> 1);
      int num = myUnionFind.Find(0);
      for (int a = 0; a < count; ++a)
      {
        if (num != myUnionFind.Find(a))
        {
          File.AppendAllText("C:\\Users\\daniel.ilha\\Desktop\\perf.log", "FAIL!\n");
          Environment.Exit(1);
        }
      }
      File.AppendAllText("C:\\Users\\daniel.ilha\\Desktop\\perf.log", string.Format("Test took {0:N}ms\n", (object) stopwatch.ElapsedMilliseconds));
    }
  }
}
