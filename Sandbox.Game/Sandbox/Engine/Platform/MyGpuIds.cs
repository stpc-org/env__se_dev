﻿// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Platform.MyGpuIds
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using VRageRender;

namespace Sandbox.Engine.Platform
{
  internal static class MyGpuIds
  {
    private static readonly int[] UnsupportedIntels = new int[23]
    {
      9602,
      10114,
      9618,
      10130,
      10098,
      10102,
      10146,
      10150,
      10158,
      10706,
      10707,
      10674,
      10675,
      10690,
      10691,
      40961,
      40962,
      40977,
      40978,
      10610,
      10611,
      10642,
      10643
    };
    private static readonly int[] UnderMinimumIntels = new int[33]
    {
      10658,
      10659,
      10626,
      10627,
      10754,
      10755,
      10770,
      10771,
      11842,
      11843,
      11922,
      11923,
      11794,
      11795,
      11826,
      11827,
      11810,
      11811,
      10818,
      10819,
      66,
      70,
      258,
      262,
      274,
      278,
      290,
      294,
      266,
      338,
      354,
      358,
      1026
    };
    private static readonly int[] UnsupportedRadeons = new int[3]
    {
      31006,
      31007,
      28997
    };
    private static readonly Dictionary<VendorIds, int[]> Unsupported = new Dictionary<VendorIds, int[]>()
    {
      {
        VendorIds.Amd,
        MyGpuIds.UnsupportedRadeons
      },
      {
        VendorIds.Intel,
        MyGpuIds.UnsupportedIntels
      }
    };
    private static readonly Dictionary<VendorIds, int[]> UnderMinimum = new Dictionary<VendorIds, int[]>()
    {
      {
        VendorIds.VMWare,
        new int[1]{ 1029 }
      },
      {
        VendorIds.Parallels,
        new int[1]{ 16389 }
      },
      {
        VendorIds.Intel,
        MyGpuIds.UnderMinimumIntels
      }
    };

    public static bool IsUnsupported(VendorIds vendorId, int deviceId)
    {
      int[] array;
      return MyGpuIds.Unsupported.TryGetValue(vendorId, out array) && array.Contains<int>(deviceId);
    }

    public static bool IsUnderMinimum(VendorIds vendorId, int deviceId)
    {
      if (MyGpuIds.IsUnsupported(vendorId, deviceId))
        return true;
      int[] array;
      return MyGpuIds.UnderMinimum.TryGetValue(vendorId, out array) && array.Contains<int>(deviceId);
    }
  }
}
