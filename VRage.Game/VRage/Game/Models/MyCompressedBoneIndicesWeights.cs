// Decompiled with JetBrains decompiler
// Type: VRage.Game.Models.MyCompressedBoneIndicesWeights
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Runtime.InteropServices;
using VRageMath.PackedVector;

namespace VRage.Game.Models
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct MyCompressedBoneIndicesWeights
  {
    public HalfVector4 Weights;
    public Byte4 Indices;
  }
}
