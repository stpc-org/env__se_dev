// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Character.MyFeetIKSettings
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRageMath;

namespace Sandbox.Game.Entities.Character
{
  public struct MyFeetIKSettings
  {
    public bool Enabled;
    public float BelowReachableDistance;
    public float AboveReachableDistance;
    public float VerticalShiftUpGain;
    public float VerticalShiftDownGain;
    public Vector3 FootSize;
  }
}
