// Decompiled with JetBrains decompiler
// Type: VRage.Game.Models.MyTriangleVertexIndices
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

namespace VRage.Game.Models
{
  public struct MyTriangleVertexIndices
  {
    public int I0;
    public int I1;
    public int I2;

    public MyTriangleVertexIndices(int i0, int i1, int i2)
    {
      this.I0 = i0;
      this.I1 = i1;
      this.I2 = i2;
    }

    public void Set(int i0, int i1, int i2)
    {
      this.I0 = i0;
      this.I1 = i1;
      this.I2 = i2;
    }
  }
}
