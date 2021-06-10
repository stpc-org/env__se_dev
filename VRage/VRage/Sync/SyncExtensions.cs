// Decompiled with JetBrains decompiler
// Type: VRage.Sync.SyncExtensions
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using VRageMath;

namespace VRage.Sync
{
  public static class SyncExtensions
  {
    public static void AlwaysReject<T, TSyncDirection>(this VRage.Sync.Sync<T, TSyncDirection> sync) where TSyncDirection : SyncDirection => sync.Validate = (SyncValidate<T>) (value => false);

    public static void ValidateRange<TSyncDirection>(
      this VRage.Sync.Sync<float, TSyncDirection> sync,
      float inclusiveMin,
      float inclusiveMax)
      where TSyncDirection : SyncDirection
    {
      sync.Validate = (SyncValidate<float>) (value => (double) value >= (double) inclusiveMin && (double) value <= (double) inclusiveMax);
    }

    public static void ValidateRange<TSyncDirection>(
      this VRage.Sync.Sync<float, TSyncDirection> sync,
      Func<float> inclusiveMin,
      Func<float> inclusiveMax)
      where TSyncDirection : SyncDirection
    {
      sync.Validate = (SyncValidate<float>) (value => (double) value >= (double) inclusiveMin() && (double) value <= (double) inclusiveMax());
    }

    public static void ValidateRange<TSyncDirection>(
      this VRage.Sync.Sync<float, TSyncDirection> sync,
      Func<MyBounds> bounds)
      where TSyncDirection : SyncDirection
    {
      sync.Validate = (SyncValidate<float>) (value =>
      {
        MyBounds myBounds = bounds();
        return (double) value >= (double) myBounds.Min && (double) value <= (double) myBounds.Max;
      });
    }
  }
}
