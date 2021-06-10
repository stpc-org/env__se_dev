// Decompiled with JetBrains decompiler
// Type: BulletXNA.LinearMath.UShortVector3
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

namespace BulletXNA.LinearMath
{
  public struct UShortVector3
  {
    public ushort X;
    public ushort Y;
    public ushort Z;

    public ushort this[int i]
    {
      get
      {
        switch (i)
        {
          case 0:
            return this.X;
          case 1:
            return this.Y;
          case 2:
            return this.Z;
          default:
            return 0;
        }
      }
      set
      {
        switch (i)
        {
          case 0:
            this.X = value;
            break;
          case 1:
            this.Y = value;
            break;
          case 2:
            this.Z = value;
            break;
        }
      }
    }
  }
}
