// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MyUpdateCallback
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;

namespace Sandbox.Game.World
{
  public class MyUpdateCallback
  {
    private Func<bool> m_action;

    public bool ToBeRemoved { get; private set; }

    public MyUpdateCallback(Func<bool> action)
    {
      this.ToBeRemoved = false;
      this.m_action = action;
    }

    public void Update()
    {
      if (!this.m_action())
        return;
      this.ToBeRemoved = true;
    }
  }
}
