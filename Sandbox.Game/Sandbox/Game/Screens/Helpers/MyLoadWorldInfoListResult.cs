// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyLoadWorldInfoListResult
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Networking;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;

namespace Sandbox.Game.Screens.Helpers
{
  public class MyLoadWorldInfoListResult : MyLoadListResult
  {
    public MyLoadWorldInfoListResult(List<string> customPaths = null)
      : base(customPaths)
    {
    }

    protected override List<Tuple<string, MyWorldInfo>> GetAvailableSaves() => MyLocalCache.GetAvailableWorldInfos(this.CustomPaths);
  }
}
