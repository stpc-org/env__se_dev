// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.IMyProductionBlock
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
  public interface IMyProductionBlock : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
  {
    IMyInventory InputInventory { get; }

    IMyInventory OutputInventory { get; }

    bool IsProducing { get; }

    bool IsQueueEmpty { get; }

    void MoveQueueItemRequest(uint queueItemId, int targetIdx);

    uint NextItemId { get; }

    bool UseConveyorSystem { get; set; }

    bool CanUseBlueprint(MyDefinitionId blueprint);

    void AddQueueItem(MyDefinitionId blueprint, MyFixedPoint amount);

    void AddQueueItem(MyDefinitionId blueprint, Decimal amount);

    void AddQueueItem(MyDefinitionId blueprint, double amount);

    void InsertQueueItem(int idx, MyDefinitionId blueprint, MyFixedPoint amount);

    void InsertQueueItem(int idx, MyDefinitionId blueprint, Decimal amount);

    void InsertQueueItem(int idx, MyDefinitionId blueprint, double amount);

    void RemoveQueueItem(int idx, MyFixedPoint amount);

    void RemoveQueueItem(int idx, Decimal amount);

    void RemoveQueueItem(int idx, double amount);

    void ClearQueue();

    void GetQueue(List<MyProductionItem> items);
  }
}
