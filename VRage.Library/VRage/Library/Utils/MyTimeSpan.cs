// Decompiled with JetBrains decompiler
// Type: VRage.Library.Utils.MyTimeSpan
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;

namespace VRage.Library.Utils
{
  public struct MyTimeSpan
  {
    public static readonly MyTimeSpan Zero = new MyTimeSpan();
    public static readonly MyTimeSpan MaxValue = new MyTimeSpan(long.MaxValue);
    public readonly long Ticks;

    public double Nanoseconds => (double) this.Ticks / ((double) MyGameTimer.Frequency / 1000000000.0);

    public double Microseconds => (double) this.Ticks / ((double) MyGameTimer.Frequency / 1000000.0);

    public double Milliseconds => (double) this.Ticks / ((double) MyGameTimer.Frequency / 1000.0);

    public double Seconds => (double) this.Ticks / (double) MyGameTimer.Frequency;

    public double Minutes => (double) this.Ticks / ((double) MyGameTimer.Frequency * 60.0);

    public TimeSpan TimeSpan => TimeSpan.FromTicks((long) Math.Round((double) this.Ticks * (10000000.0 / (double) MyGameTimer.Frequency)));

    public MyTimeSpan(long stopwatchTicks) => this.Ticks = stopwatchTicks;

    public override bool Equals(object obj) => this.Ticks == ((MyTimeSpan) obj).Ticks;

    public override int GetHashCode() => this.Ticks.GetHashCode();

    public static MyTimeSpan FromTicks(long ticks) => new MyTimeSpan(ticks);

    public static MyTimeSpan FromSeconds(double seconds) => new MyTimeSpan((long) (seconds * (double) MyGameTimer.Frequency));

    public static MyTimeSpan FromMinutes(double minutes) => MyTimeSpan.FromSeconds(minutes * 60.0);

    public static MyTimeSpan FromMilliseconds(double milliseconds) => MyTimeSpan.FromSeconds(milliseconds * 0.001);

    public static MyTimeSpan operator +(MyTimeSpan a, MyTimeSpan b) => new MyTimeSpan(a.Ticks + b.Ticks);

    public static MyTimeSpan operator -(MyTimeSpan a, MyTimeSpan b) => new MyTimeSpan(a.Ticks - b.Ticks);

    public static bool operator !=(MyTimeSpan a, MyTimeSpan b) => a.Ticks != b.Ticks;

    public static bool operator ==(MyTimeSpan a, MyTimeSpan b) => a.Ticks == b.Ticks;

    public static bool operator >(MyTimeSpan a, MyTimeSpan b) => a.Ticks > b.Ticks;

    public static bool operator <(MyTimeSpan a, MyTimeSpan b) => a.Ticks < b.Ticks;

    public static bool operator >=(MyTimeSpan a, MyTimeSpan b) => a.Ticks >= b.Ticks;

    public static bool operator <=(MyTimeSpan a, MyTimeSpan b) => a.Ticks <= b.Ticks;

    public override string ToString() => ((int) Math.Round(this.Milliseconds)).ToString();
  }
}
