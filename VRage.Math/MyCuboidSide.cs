// Decompiled with JetBrains decompiler
// Type: VRageMath.MyCuboidSide
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

namespace VRageMath
{
  public class MyCuboidSide
  {
    public Plane Plane;
    public Line[] Lines = new Line[4];

    public MyCuboidSide()
    {
      this.Lines[0] = new Line();
      this.Lines[1] = new Line();
      this.Lines[2] = new Line();
      this.Lines[3] = new Line();
    }

    public void CreatePlaneFromLines() => this.Plane = new Plane(this.Lines[0].From, Vector3.Cross(this.Lines[1].Direction, this.Lines[0].Direction));
  }
}
