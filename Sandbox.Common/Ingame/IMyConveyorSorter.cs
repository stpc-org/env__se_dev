// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.IMyConveyorSorter
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System.Collections.Generic;
using VRage.Game;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
  public interface IMyConveyorSorter : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
  {
    bool DrainAll { get; set; }

    MyConveyorSorterMode Mode { get; }

    void GetFilterList(List<MyInventoryItemFilter> items);

    void AddItem(MyInventoryItemFilter item);

    void RemoveItem(MyInventoryItemFilter item);

    bool IsAllowed(MyDefinitionId id);

    void SetFilter(MyConveyorSorterMode mode, List<MyInventoryItemFilter> items);
  }
}
