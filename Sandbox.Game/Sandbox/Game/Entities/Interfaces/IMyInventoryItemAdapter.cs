// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Interfaces.IMyInventoryItemAdapter
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage;
using VRage.Utils;

namespace Sandbox.Game.Entities.Interfaces
{
  public interface IMyInventoryItemAdapter
  {
    float Mass { get; }

    float Volume { get; }

    bool HasIntegralAmounts { get; }

    MyFixedPoint MaxStackAmount { get; }

    string DisplayNameText { get; }

    string[] Icons { get; }

    MyStringId? IconSymbol { get; }
  }
}
