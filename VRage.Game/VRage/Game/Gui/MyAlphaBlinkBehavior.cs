// Decompiled with JetBrains decompiler
// Type: VRage.Game.GUI.MyAlphaBlinkBehavior
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Xml.Serialization;
using VRage.Library.Utils;
using VRageMath;

namespace VRage.Game.GUI
{
  public class MyAlphaBlinkBehavior
  {
    private static readonly MyGameTimer TIMER = new MyGameTimer();
    private int m_intervalLenghtMs = 2000;
    private float m_minAlpha = 0.2f;
    private float m_maxAlpha = 0.8f;
    private float m_currentBlinkAlpha = 1f;

    public float MinAlpha
    {
      get => this.m_minAlpha;
      set => this.m_minAlpha = MathHelper.Clamp(value, 0.0f, 1f);
    }

    public float MaxAlpha
    {
      get => this.m_maxAlpha;
      set => this.m_maxAlpha = MathHelper.Clamp(value, 0.0f, 1f);
    }

    public int IntervalMs
    {
      get => this.m_intervalLenghtMs;
      set => this.m_intervalLenghtMs = MathHelper.Clamp(value, 0, int.MaxValue);
    }

    public Vector4? ColorMask { get; set; }

    [XmlIgnore]
    public float CurrentBlinkAlpha
    {
      get => this.m_currentBlinkAlpha;
      set => this.m_currentBlinkAlpha = MathHelper.Clamp(value, 0.0f, 1f);
    }

    public bool Blink { get; set; }

    public virtual void UpdateBlink()
    {
      if (this.Blink)
        this.CurrentBlinkAlpha = this.m_minAlpha + (float) ((Math.Cos(MyAlphaBlinkBehavior.TIMER.ElapsedTimeSpan.TotalMilliseconds / (double) this.m_intervalLenghtMs * Math.PI * 2.0) + 1.0) * 0.5 * ((double) this.m_maxAlpha - (double) this.m_minAlpha));
      else
        this.CurrentBlinkAlpha = 1f;
    }
  }
}
