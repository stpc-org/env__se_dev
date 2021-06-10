// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.InputRecording.MyBlockSnapshot
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;

namespace Sandbox.Game.Screens.Helpers.InputRecording
{
  public class MyBlockSnapshot
  {
    public MyCubeSize Grid { get; set; }

    public string CurrentBlockName { get; set; }

    public int? Stage { get; set; }

    public int? LOD { get; set; }
  }
}
