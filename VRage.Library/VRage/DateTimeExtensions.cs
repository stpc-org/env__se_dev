// Decompiled with JetBrains decompiler
// Type: VRage.DateTimeExtensions
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;

namespace VRage
{
  public static class DateTimeExtensions
  {
    public static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

    public static DateTime ToDateTimeFromUnixTimestamp(this uint timestamp) => DateTimeExtensions.Epoch.AddSeconds((double) timestamp).ToUniversalTime();

    public static uint ToUnixTimestamp(this DateTime time) => (uint) time.Subtract(DateTimeExtensions.Epoch).TotalSeconds;
  }
}
