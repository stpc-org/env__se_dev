// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Interfaces.IMyGasTank
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRage.Game;

namespace Sandbox.Game.Entities.Interfaces
{
  public interface IMyGasTank
  {
    float GasCapacity { get; }

    double FilledRatio { get; }

    Action FilledRatioChanged { get; set; }

    bool IsResourceStorage(MyDefinitionId resourceDefinition);
  }
}
