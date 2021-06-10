// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGenerator
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.ModAPI.Ingame;
using System;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace SpaceEngineers.Game.ModAPI.Ingame
{
  public interface IMyGravityGenerator : IMyGravityGeneratorBase, IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
  {
    [Obsolete("Use FieldSize.X")]
    float FieldWidth { get; }

    [Obsolete("Use FieldSize.Y")]
    float FieldHeight { get; }

    [Obsolete("Use FieldSize.Z")]
    float FieldDepth { get; }

    Vector3 FieldSize { get; set; }
  }
}
