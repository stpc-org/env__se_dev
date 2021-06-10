// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRage.ModAPI;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
  public abstract class MyStatBase : IMyHudStat
  {
    private float m_currentValue;
    private string m_valueStringCache;

    public MyStringHash Id { get; protected set; }

    public float CurrentValue
    {
      get => this.m_currentValue;
      protected set
      {
        if (this.m_currentValue.IsEqual(value))
          return;
        this.m_currentValue = value;
        this.m_valueStringCache = (string) null;
      }
    }

    public virtual float MaxValue => 1f;

    public virtual float MinValue => 0.0f;

    public abstract void Update();

    public override string ToString() => string.Format("{0:0.00}", (object) this.CurrentValue);

    public string GetValueString()
    {
      if (this.m_valueStringCache == null)
        this.m_valueStringCache = this.ToString();
      return this.m_valueStringCache;
    }
  }
}
