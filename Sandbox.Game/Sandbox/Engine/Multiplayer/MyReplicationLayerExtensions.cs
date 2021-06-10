// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Multiplayer.MyReplicationLayerExtensions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VRage.Network;
using VRage.Plugins;

namespace Sandbox.Engine.Multiplayer
{
  public static class MyReplicationLayerExtensions
  {
    public static void RegisterFromGameAssemblies(this MyReplicationLayerBase layer)
    {
      Assembly[] assemblyArray = new Assembly[5]
      {
        typeof (MySandboxGame).Assembly,
        typeof (MyRenderProfiler).Assembly,
        MyPlugins.GameAssembly,
        MyPlugins.SandboxAssembly,
        MyPlugins.SandboxGameAssembly
      };
      layer.RegisterFromAssembly(((IEnumerable<Assembly>) assemblyArray).Where<Assembly>((Func<Assembly, bool>) (s => s != (Assembly) null)).Distinct<Assembly>());
    }
  }
}
