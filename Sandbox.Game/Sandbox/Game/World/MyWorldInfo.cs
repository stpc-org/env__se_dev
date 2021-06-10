// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MyWorldInfo
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRage.Game;

namespace Sandbox.Game.World
{
  public class MyWorldInfo
  {
    public string SessionName;
    public string SessionPath;
    public string Description;
    public DateTime LastSaveTime;
    public ulong StorageSize;
    public WorkshopId[] WorkshopIds;
    public string Briefing;
    public bool ScenarioEditMode;
    public bool IsCorrupted;
    public bool IsExperimental;
    public bool HasPlanets;
    public bool IsCampaign;

    public string SaveDirectory { get; set; }
  }
}
