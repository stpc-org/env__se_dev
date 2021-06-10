// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Navigation.MyStuckDetection
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRageMath;

namespace Sandbox.Game.AI.Navigation
{
  public class MyStuckDetection
  {
    private const int STUCK_COUNTDOWN = 60;
    private const int LONGTERM_COUNTDOWN = 300;
    private const double LONGTERM_TOLERANCE = 0.025;
    private Vector3D m_translationStuckDetection;
    private Vector3D m_longTermTranslationStuckDetection;
    private Vector3 m_rotationStuckDetection;
    private readonly float m_positionToleranceSq;
    private readonly float m_rotationToleranceSq;
    private double m_previousDistanceFromTarget;
    private bool m_isRotating;
    private int m_counter;
    private int m_longTermCounter;
    private int m_tickCounter;
    private int m_stoppedTime;
    private BoundingBoxD m_boundingBox;

    public bool IsStuck { get; private set; }

    public MyStuckDetection(float positionTolerance, float rotationTolerance)
    {
      this.m_positionToleranceSq = positionTolerance * positionTolerance;
      this.m_rotationToleranceSq = rotationTolerance * rotationTolerance;
      this.Reset();
    }

    public MyStuckDetection(float positionTolerance, float rotationTolerance, BoundingBoxD box)
      : this(positionTolerance, rotationTolerance)
      => this.m_boundingBox = box;

    public void SetRotating(bool rotating) => this.m_isRotating = rotating;

    public void Update(Vector3D worldPosition, Vector3 rotation, Vector3D targetLocation = default (Vector3D))
    {
      this.m_translationStuckDetection = this.m_translationStuckDetection * 0.8 + worldPosition * 0.2;
      this.m_rotationStuckDetection = this.m_rotationStuckDetection * 0.95f + rotation * 0.05f;
      bool flag = (this.m_translationStuckDetection - worldPosition).LengthSquared() < (double) this.m_positionToleranceSq && (double) (this.m_rotationStuckDetection - rotation).LengthSquared() < (double) this.m_rotationToleranceSq && !this.m_isRotating;
      double num = (worldPosition - targetLocation).Length();
      if (targetLocation != Vector3D.Zero && !flag && num < 2.0 * this.m_boundingBox.Extents.Min())
      {
        if (Math.Abs(this.m_previousDistanceFromTarget - num) > 1.0)
          this.m_previousDistanceFromTarget = num + 1.0;
        this.m_previousDistanceFromTarget = this.m_previousDistanceFromTarget * 0.7 + num * 0.3;
        flag = Math.Abs(num - this.m_previousDistanceFromTarget) < (double) this.m_positionToleranceSq;
      }
      if (this.m_counter <= 0)
      {
        if (flag)
          this.IsStuck = true;
        else
          this.m_counter = 60;
      }
      else
      {
        if (this.m_counter == 60 && !flag)
        {
          this.IsStuck = false;
          return;
        }
        --this.m_counter;
      }
      if (this.m_longTermCounter <= 0)
      {
        if ((this.m_longTermTranslationStuckDetection - worldPosition).LengthSquared() < 0.025)
        {
          this.IsStuck = true;
        }
        else
        {
          this.m_longTermCounter = 300;
          this.m_longTermTranslationStuckDetection = worldPosition;
        }
      }
      else
        --this.m_longTermCounter;
    }

    public void Reset(bool force = false)
    {
      if (!force && this.m_stoppedTime == this.m_tickCounter)
        return;
      this.m_translationStuckDetection = Vector3D.Zero;
      this.m_rotationStuckDetection = Vector3.Zero;
      this.IsStuck = false;
      this.m_counter = 60;
      this.m_longTermCounter = 300;
      this.m_isRotating = false;
    }

    public void Stop() => this.m_stoppedTime = this.m_tickCounter;

    public void SetCurrentTicks(int behaviorTicks) => this.m_tickCounter = behaviorTicks;
  }
}
