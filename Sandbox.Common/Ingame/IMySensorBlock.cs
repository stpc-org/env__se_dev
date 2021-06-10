// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.IMySensorBlock
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System.Collections.Generic;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
  public interface IMySensorBlock : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
  {
    float MaxRange { get; }

    float LeftExtend { get; set; }

    float RightExtend { get; set; }

    float TopExtend { get; set; }

    float BottomExtend { get; set; }

    float FrontExtend { get; set; }

    float BackExtend { get; set; }

    bool PlayProximitySound { get; set; }

    bool DetectPlayers { get; set; }

    bool DetectFloatingObjects { get; set; }

    bool DetectSmallShips { get; set; }

    bool DetectLargeShips { get; set; }

    bool DetectStations { get; set; }

    bool DetectSubgrids { get; set; }

    bool DetectAsteroids { get; set; }

    bool DetectOwner { get; set; }

    bool DetectFriendly { get; set; }

    bool DetectNeutral { get; set; }

    bool DetectEnemy { get; set; }

    bool IsActive { get; }

    MyDetectedEntityInfo LastDetectedEntity { get; }

    void DetectedEntities(List<MyDetectedEntityInfo> entities);
  }
}
