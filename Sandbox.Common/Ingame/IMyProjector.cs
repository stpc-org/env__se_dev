// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.IMyProjector
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace Sandbox.ModAPI.Ingame
{
  public interface IMyProjector : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity, IMyTextSurfaceProvider
  {
    [Obsolete("Use ProjectionOffset vector instead.")]
    int ProjectionOffsetX { get; }

    [Obsolete("Use ProjectionOffset vector instead.")]
    int ProjectionOffsetY { get; }

    [Obsolete("Use ProjectionOffset vector instead.")]
    int ProjectionOffsetZ { get; }

    [Obsolete("Use ProjectionRotation vector instead.")]
    int ProjectionRotX { get; }

    [Obsolete("Use ProjectionRotation vector instead.")]
    int ProjectionRotY { get; }

    [Obsolete("Use ProjectionRotation vector instead.")]
    int ProjectionRotZ { get; }

    bool IsProjecting { get; }

    int TotalBlocks { get; }

    int RemainingBlocks { get; }

    Dictionary<MyDefinitionBase, int> RemainingBlocksPerType { get; }

    int RemainingArmorBlocks { get; }

    int BuildableBlocksCount { get; }

    Vector3I ProjectionOffset { get; set; }

    Vector3I ProjectionRotation { get; set; }

    void UpdateOffsetAndRotation();

    bool ShowOnlyBuildable { get; set; }
  }
}
