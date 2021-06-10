// Decompiled with JetBrains decompiler
// Type: VRageRender.MyViewport
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRageMath;

namespace VRageRender
{
  public struct MyViewport
  {
    public float OffsetX;
    public float OffsetY;
    public float Width;
    public float Height;

    public MyViewport(float width, float height)
    {
      this.OffsetX = 0.0f;
      this.OffsetY = 0.0f;
      this.Width = width;
      this.Height = height;
    }

    public MyViewport(Vector2I resolution)
    {
      this.OffsetX = 0.0f;
      this.OffsetY = 0.0f;
      this.Width = (float) resolution.X;
      this.Height = (float) resolution.Y;
    }

    public MyViewport(float x, float y, float width, float height)
    {
      this.OffsetX = x;
      this.OffsetY = y;
      this.Width = width;
      this.Height = height;
    }
  }
}
