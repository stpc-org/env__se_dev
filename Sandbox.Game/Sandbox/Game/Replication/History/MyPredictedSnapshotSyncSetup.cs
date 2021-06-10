// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.History.MyPredictedSnapshotSyncSetup
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;

namespace Sandbox.Game.Replication.History
{
  public class MyPredictedSnapshotSyncSetup : MySnapshotSyncSetup
  {
    public float MaxPositionFactor;
    public float MinPositionFactor = 1f;
    public float MaxLinearFactor;
    public float MinLinearFactor = 1f;
    public float MaxRotationFactor;
    public float MaxAngularFactor;
    public float MinAngularFactor = 1f;
    public float IterationsFactor;
    public bool UpdateAlways;
    public bool AllowForceStop;
    public bool IsControlled;
    public bool Smoothing = true;
    private MyPredictedSnapshotSyncSetup m_notSmoothed;

    public MyPredictedSnapshotSyncSetup NotSmoothed
    {
      get
      {
        if (this.m_notSmoothed == null)
        {
          this.m_notSmoothed = this.MemberwiseClone() as MyPredictedSnapshotSyncSetup;
          this.m_notSmoothed.MaxPositionFactor = Math.Min(1f, this.MaxPositionFactor);
          this.m_notSmoothed.MaxLinearFactor = Math.Min(1f, this.MaxLinearFactor);
          this.m_notSmoothed.MaxRotationFactor = Math.Min(1f, this.MaxRotationFactor);
          this.m_notSmoothed.MaxAngularFactor = Math.Min(1f, this.MaxAngularFactor);
          this.m_notSmoothed.Smoothing = false;
        }
        return this.m_notSmoothed;
      }
    }
  }
}
