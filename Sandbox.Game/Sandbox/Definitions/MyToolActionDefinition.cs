// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyToolActionDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game.Gui;
using VRage.Utils;

namespace Sandbox.Definitions
{
  public struct MyToolActionDefinition
  {
    public MyStringId Name;
    public float StartTime;
    public float EndTime;
    public float Efficiency;
    public string StatsEfficiency;
    public string SwingSound;
    public float SwingSoundStart;
    public float HitStart;
    public float HitDuration;
    public string HitSound;
    public float CustomShapeRadius;
    public MyHudTexturesEnum Crosshair;
    public MyToolHitCondition[] HitConditions;

    public override string ToString() => this.Name.ToString();
  }
}
