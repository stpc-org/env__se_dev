// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyDelayedRazeBatch
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Game.Entities
{
  public class MyDelayedRazeBatch
  {
    public Vector3I Pos;
    public Vector3UByte Size;
    public HashSet<MyCockpit> Occupied;

    public MyDelayedRazeBatch(Vector3I pos, Vector3UByte size)
    {
      this.Pos = pos;
      this.Size = size;
    }
  }
}
