// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.TextSurfaceScripts.IMyTextSurfaceScript
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.ModAPI.Ingame;
using System;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace Sandbox.Game.GameSystems.TextSurfaceScripts
{
  public interface IMyTextSurfaceScript : IDisposable
  {
    IMyTextSurface Surface { get; }

    IMyCubeBlock Block { get; }

    ScriptUpdate NeedsUpdate { get; }

    Color ForegroundColor { get; }

    Color BackgroundColor { get; }

    void Run();
  }
}
