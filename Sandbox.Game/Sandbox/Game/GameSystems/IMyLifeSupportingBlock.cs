// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.IMyLifeSupportingBlock
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Character;

namespace Sandbox.Game.GameSystems
{
  public interface IMyLifeSupportingBlock : VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity
  {
    bool RefuelAllowed { get; }

    bool HealingAllowed { get; }

    MyLifeSupportingBlockType BlockType { get; }

    void ShowTerminal(MyCharacter user);

    void BroadcastSupportRequest(MyCharacter user);
  }
}
