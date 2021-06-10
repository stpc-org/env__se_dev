// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyHudLargeTurretTargets
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Game.Gui;

namespace Sandbox.Game.Gui
{
  public class MyHudLargeTurretTargets
  {
    private Dictionary<MyEntity, MyHudEntityParams> m_markers = new Dictionary<MyEntity, MyHudEntityParams>();

    public bool Visible { get; set; }

    public MyHudLargeTurretTargets() => this.Visible = true;

    internal Dictionary<MyEntity, MyHudEntityParams> Targets => this.m_markers;

    internal void RegisterMarker(MyEntity target, MyHudEntityParams hudParams) => this.m_markers[target] = hudParams;

    internal void UnregisterMarker(MyEntity target) => this.m_markers.Remove(target);
  }
}
