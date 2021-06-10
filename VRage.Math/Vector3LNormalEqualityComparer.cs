// Decompiled with JetBrains decompiler
// Type: VRageMath.Vector3LNormalEqualityComparer
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System.Collections.Generic;

namespace VRageMath
{
  public class Vector3LNormalEqualityComparer : IEqualityComparer<Vector3L>
  {
    public bool Equals(Vector3L x, Vector3L y) => x.X + 1L + (x.Y + 1L) * 3L + (x.Z + 1L) * 9L == y.X + 1L + (y.Y + 1L) * 3L + (y.Z + 1L) * 9L;

    public int GetHashCode(Vector3L x) => (int) (x.X + 1L + (x.Y + 1L) * 3L + (x.Z + 1L) * 9L);
  }
}
