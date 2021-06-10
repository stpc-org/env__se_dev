// Decompiled with JetBrains decompiler
// Type: VRageMath.MyUShort4
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

namespace VRageMath
{
  public struct MyUShort4
  {
    public ushort X;
    public ushort Y;
    public ushort Z;
    public ushort W;

    public MyUShort4(uint x, uint y, uint z, uint w)
    {
      this.X = (ushort) x;
      this.Y = (ushort) y;
      this.Z = (ushort) z;
      this.W = (ushort) w;
    }

    public static unsafe explicit operator ulong(MyUShort4 val) => (ulong) *(long*) &val;

    public static unsafe explicit operator MyUShort4(ulong val) => *(MyUShort4*) &val;
  }
}
