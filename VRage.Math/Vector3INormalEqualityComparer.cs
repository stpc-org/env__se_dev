// Decompiled with JetBrains decompiler
// Type: VRageMath.Vector3INormalEqualityComparer
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System.Collections.Generic;

namespace VRageMath
{
  public class Vector3INormalEqualityComparer : IEqualityComparer<Vector3I>
  {
    public bool Equals(Vector3I x, Vector3I y) => x.X + 1 + (x.Y + 1) * 3 + (x.Z + 1) * 9 == y.X + 1 + (y.Y + 1) * 3 + (y.Z + 1) * 9;

    public int GetHashCode(Vector3I x) => x.X + 1 + (x.Y + 1) * 3 + (x.Z + 1) * 9;
  }
}
