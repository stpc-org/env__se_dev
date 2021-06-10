// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.IMyProductionBlock
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;

namespace Sandbox.ModAPI
{
  public interface IMyProductionBlock : IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyProductionBlock
  {
    event Action StartedProducing;

    event Action StoppedProducing;

    VRage.Game.ModAPI.IMyInventory InputInventory { get; }

    VRage.Game.ModAPI.IMyInventory OutputInventory { get; }

    bool CanUseBlueprint(MyDefinitionBase blueprint);

    void AddQueueItem(MyDefinitionBase blueprint, MyFixedPoint amount);

    void InsertQueueItem(int idx, MyDefinitionBase blueprint, MyFixedPoint amount);

    List<MyProductionQueueItem> GetQueue();
  }
}
