// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyTileDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
  public struct MyTileDefinition
  {
    public Matrix LocalMatrix;
    public Vector3 Normal;
    public bool FullQuad;
    public bool IsEmpty;
    public bool IsRounded;
    public bool DontOffsetTexture;
    public Vector3 Up;
    public MyStringId Id;
  }
}
