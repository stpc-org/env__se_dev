// Decompiled with JetBrains decompiler
// Type: VRageMath.PackedVector.IPackedVector`1
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

namespace VRageMath.PackedVector
{
  public interface IPackedVector<TPacked> : IPackedVector
  {
    TPacked PackedValue { get; set; }
  }
}
