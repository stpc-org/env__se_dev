// Decompiled with JetBrains decompiler
// Type: VRage.Game.Utils.MyCameraShake
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.Utils
{
  public class MyCameraShake
  {
    public float MaxShake = 15f;
    public float MaxShakePosX = 0.8f;
    public float MaxShakePosY = 0.2f;
    public float MaxShakePosZ = 0.8f;
    public float MaxShakeDir = 0.2f;
    public float Reduction = 0.6f;
    public float Dampening = 0.8f;
    public float OffConstant = 0.01f;
    public float DirReduction = 0.35f;
    private bool m_shakeEnabled;
    private Vector3 m_shakePos;
    private Vector3 m_shakeDir;
    private float m_currentShakePosPower;
    private float m_currentShakeDirPower;

    public MyCameraShake()
    {
      this.m_shakeEnabled = false;
      this.m_currentShakeDirPower = 0.0f;
      this.m_currentShakePosPower = 0.0f;
    }

    public bool ShakeEnabled
    {
      get => this.m_shakeEnabled;
      set => this.m_shakeEnabled = value;
    }

    public Vector3 ShakePos => this.m_shakePos;

    public Vector3 ShakeDir => this.m_shakeDir;

    public bool ShakeActive() => this.m_shakeEnabled;

    public void AddShake(float shakePower)
    {
      if (MyUtils.IsZero(shakePower, 1E-05f) || MyUtils.IsZero(this.MaxShake, 1E-05f))
        return;
      float num = MathHelper.Clamp(shakePower / this.MaxShake, 0.0f, 1f);
      if ((double) this.m_currentShakePosPower < (double) num)
        this.m_currentShakePosPower = num;
      if ((double) this.m_currentShakeDirPower < (double) num * (double) this.DirReduction)
        this.m_currentShakeDirPower = num * this.DirReduction;
      this.m_shakePos = new Vector3(this.m_currentShakePosPower * this.MaxShakePosX, this.m_currentShakePosPower * this.MaxShakePosY, this.m_currentShakePosPower * this.MaxShakePosZ);
      this.m_shakeDir = new Vector3(this.m_currentShakeDirPower * this.MaxShakeDir, this.m_currentShakeDirPower * this.MaxShakeDir, 0.0f);
      this.m_shakeEnabled = true;
    }

    public void UpdateShake(float timeStep, out Vector3 outPos, out Vector3 outDir)
    {
      if (!this.m_shakeEnabled)
      {
        outPos = Vector3.Zero;
        outDir = Vector3.Zero;
      }
      else
      {
        this.m_shakePos.X *= MyUtils.GetRandomSign();
        this.m_shakePos.Y *= MyUtils.GetRandomSign();
        this.m_shakePos.Z *= MyUtils.GetRandomSign();
        outPos.X = this.m_shakePos.X * Math.Abs(this.m_shakePos.X) * this.Reduction;
        outPos.Y = this.m_shakePos.Y * Math.Abs(this.m_shakePos.Y) * this.Reduction;
        outPos.Z = this.m_shakePos.Z * Math.Abs(this.m_shakePos.Z) * this.Reduction;
        this.m_shakeDir.X *= MyUtils.GetRandomSign();
        this.m_shakeDir.Y *= MyUtils.GetRandomSign();
        this.m_shakeDir.Z *= MyUtils.GetRandomSign();
        outDir.X = (float) ((double) this.m_shakeDir.X * (double) Math.Abs(this.m_shakeDir.X) * 100.0);
        outDir.Y = (float) ((double) this.m_shakeDir.Y * (double) Math.Abs(this.m_shakeDir.Y) * 100.0);
        outDir.Z = (float) ((double) this.m_shakeDir.Z * (double) Math.Abs(this.m_shakeDir.Z) * 100.0);
        outDir *= this.DirReduction;
        this.m_currentShakePosPower *= (float) Math.Pow((double) this.Dampening, (double) timeStep * 60.0);
        this.m_currentShakeDirPower *= (float) Math.Pow((double) this.Dampening, (double) timeStep * 60.0);
        if ((double) this.m_currentShakeDirPower < 0.0)
          this.m_currentShakeDirPower = 0.0f;
        if ((double) this.m_currentShakePosPower < 0.0)
          this.m_currentShakePosPower = 0.0f;
        this.m_shakePos = new Vector3(this.m_currentShakePosPower * this.MaxShakePosX, this.m_currentShakePosPower * this.MaxShakePosY, this.m_currentShakePosPower * this.MaxShakePosZ);
        this.m_shakeDir = new Vector3(this.m_currentShakeDirPower * this.MaxShakeDir, this.m_currentShakeDirPower * this.MaxShakeDir, 0.0f);
        if ((double) this.m_currentShakeDirPower >= (double) this.OffConstant || (double) this.m_currentShakePosPower >= (double) this.OffConstant)
          return;
        this.m_currentShakeDirPower = 0.0f;
        this.m_currentShakePosPower = 0.0f;
        this.m_shakeEnabled = false;
      }
    }
  }
}
