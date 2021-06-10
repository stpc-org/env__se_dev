// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MyWrappedCubemap`1
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
  public class MyWrappedCubemap<TFaceFormat> : IDisposable where TFaceFormat : IMyWrappedCubemapFace
  {
    protected TFaceFormat[] m_faces;
    protected readonly int m_resolution;

    public string Name { get; }

    public TFaceFormat[] Faces => this.m_faces;

    public int Resolution => this.m_resolution;

    public TFaceFormat this[int i] => this.m_faces[i];

    public TFaceFormat Top => this.m_faces[4];

    public TFaceFormat Back => this.m_faces[1];

    public TFaceFormat Left => this.m_faces[2];

    public TFaceFormat Right => this.m_faces[3];

    public TFaceFormat Bottom => this.m_faces[5];

    public TFaceFormat Front => this.m_faces[0];

    public MyWrappedCubemap(string name, int resolution, TFaceFormat[] faces)
    {
      this.Name = name;
      this.m_faces = faces;
      this.m_resolution = resolution;
      this.PrepareSides();
    }

    private void PrepareSides()
    {
      int num = this.m_resolution - 1;
      this.Front.CopyRange(new Vector2I(0, -1), new Vector2I(num, -1), (IMyWrappedCubemapFace) this.Top, new Vector2I(0, num), new Vector2I(num, num));
      this.Front.CopyRange(new Vector2I(0, this.m_resolution), new Vector2I(num, this.m_resolution), (IMyWrappedCubemapFace) this.Bottom, new Vector2I(num, num), new Vector2I(0, num));
      this.Front.CopyRange(new Vector2I(-1, 0), new Vector2I(-1, num), (IMyWrappedCubemapFace) this.Left, new Vector2I(num, 0), new Vector2I(num, num));
      this.Front.CopyRange(new Vector2I(this.m_resolution, 0), new Vector2I(this.m_resolution, num), (IMyWrappedCubemapFace) this.Right, new Vector2I(0, 0), new Vector2I(0, num));
      this.Back.CopyRange(new Vector2I(num, -1), new Vector2I(0, -1), (IMyWrappedCubemapFace) this.Top, new Vector2I(0, 0), new Vector2I(num, 0));
      this.Back.CopyRange(new Vector2I(num, this.m_resolution), new Vector2I(0, this.m_resolution), (IMyWrappedCubemapFace) this.Bottom, new Vector2I(num, 0), new Vector2I(0, 0));
      this.Back.CopyRange(new Vector2I(-1, 0), new Vector2I(-1, num), (IMyWrappedCubemapFace) this.Right, new Vector2I(num, 0), new Vector2I(num, num));
      this.Back.CopyRange(new Vector2I(this.m_resolution, 0), new Vector2I(this.m_resolution, num), (IMyWrappedCubemapFace) this.Left, new Vector2I(0, 0), new Vector2I(0, num));
      this.Left.CopyRange(new Vector2I(num, -1), new Vector2I(0, -1), (IMyWrappedCubemapFace) this.Top, new Vector2I(0, num), new Vector2I(0, 0));
      this.Left.CopyRange(new Vector2I(num, this.m_resolution), new Vector2I(0, this.m_resolution), (IMyWrappedCubemapFace) this.Bottom, new Vector2I(num, num), new Vector2I(num, 0));
      this.Left.CopyRange(new Vector2I(this.m_resolution, 0), new Vector2I(this.m_resolution, num), (IMyWrappedCubemapFace) this.Front, new Vector2I(0, 0), new Vector2I(0, num));
      this.Left.CopyRange(new Vector2I(-1, 0), new Vector2I(-1, num), (IMyWrappedCubemapFace) this.Back, new Vector2I(num, 0), new Vector2I(num, num));
      this.Right.CopyRange(new Vector2I(num, -1), new Vector2I(0, -1), (IMyWrappedCubemapFace) this.Top, new Vector2I(num, 0), new Vector2I(num, num));
      this.Right.CopyRange(new Vector2I(num, this.m_resolution), new Vector2I(0, this.m_resolution), (IMyWrappedCubemapFace) this.Bottom, new Vector2I(0, 0), new Vector2I(0, num));
      this.Right.CopyRange(new Vector2I(this.m_resolution, 0), new Vector2I(this.m_resolution, num), (IMyWrappedCubemapFace) this.Back, new Vector2I(0, 0), new Vector2I(0, num));
      this.Right.CopyRange(new Vector2I(-1, 0), new Vector2I(-1, num), (IMyWrappedCubemapFace) this.Front, new Vector2I(num, 0), new Vector2I(num, num));
      this.Top.CopyRange(new Vector2I(0, this.m_resolution), new Vector2I(num, this.m_resolution), (IMyWrappedCubemapFace) this.Front, new Vector2I(0, 0), new Vector2I(num, 0));
      this.Top.CopyRange(new Vector2I(0, -1), new Vector2I(num, -1), (IMyWrappedCubemapFace) this.Back, new Vector2I(num, 0), new Vector2I(0, 0));
      this.Top.CopyRange(new Vector2I(this.m_resolution, 0), new Vector2I(this.m_resolution, num), (IMyWrappedCubemapFace) this.Right, new Vector2I(num, 0), new Vector2I(0, 0));
      this.Top.CopyRange(new Vector2I(-1, 0), new Vector2I(-1, num), (IMyWrappedCubemapFace) this.Left, new Vector2I(0, 0), new Vector2I(num, 0));
      this.Bottom.CopyRange(new Vector2I(0, this.m_resolution), new Vector2I(num, this.m_resolution), (IMyWrappedCubemapFace) this.Front, new Vector2I(num, num), new Vector2I(0, num));
      this.Bottom.CopyRange(new Vector2I(0, -1), new Vector2I(num, -1), (IMyWrappedCubemapFace) this.Back, new Vector2I(0, num), new Vector2I(num, num));
      this.Bottom.CopyRange(new Vector2I(-1, 0), new Vector2I(-1, num), (IMyWrappedCubemapFace) this.Right, new Vector2I(num, num), new Vector2I(0, num));
      this.Bottom.CopyRange(new Vector2I(this.m_resolution, 0), new Vector2I(this.m_resolution, num), (IMyWrappedCubemapFace) this.Left, new Vector2I(0, num), new Vector2I(num, num));
      for (int i = 0; i < 6; ++i)
        this.m_faces[i].FinishFace(this.Name + "_" + MyCubemapHelpers.GetNameForFace(i));
    }

    ~MyWrappedCubemap()
    {
    }

    public void Dispose()
    {
      foreach (TFaceFormat face in this.m_faces)
        face.Dispose();
      this.m_faces = (TFaceFormat[]) null;
      GC.SuppressFinalize((object) this);
    }
  }
}
