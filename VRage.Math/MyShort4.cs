// Decompiled with JetBrains decompiler
// Type: VRageMath.MyShort4
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

namespace VRageMath
{
  public struct MyShort4
  {
    public short X;
    public short Y;
    public short Z;
    public short W;

    public static unsafe explicit operator ulong(MyShort4 val) => (ulong) *(long*) &val;

    public static unsafe explicit operator MyShort4(ulong val) => *(MyShort4*) &val;
  }
}
