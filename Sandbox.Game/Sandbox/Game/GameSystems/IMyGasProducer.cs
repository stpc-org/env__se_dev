// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.IMyGasProducer
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.GameSystems.Conveyors;

namespace Sandbox.Game.GameSystems
{
  internal interface IMyGasProducer : IMyGasBlock, IMyConveyorEndpointBlock
  {
    float ProductionCapacity(float deltaTime);

    void Produce(float amount);

    int GetPriority();
  }
}
