// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MyCubemapData`1
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using System;
using VRage.Library;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
  public class MyCubemapData<TPixel> : IMyWrappedCubemapFace, IDisposable
    where TPixel : unmanaged
  {
    private static NativeArrayAllocator BufferAllocator = new NativeArrayAllocator(MyPlanet.MemoryTracker.RegisterSubsystem("CubemapDataBuffers"));
    public unsafe TPixel* Data;
    private NativeArray m_dataBuffer;
    private readonly int m_realResolution;

    public int Resolution { get; set; }

    public int ResolutionMinusOne { get; set; }

    public int RowStride => this.m_realResolution;

    public unsafe void SetMaterial(int x, int y, TPixel value) => this.Data[(y + 1) * this.m_realResolution + (x + 1)] = value;

    public unsafe void SetValue(int x, int y, ref TPixel value) => this.Data[(y + 1) * this.m_realResolution + (x + 1)] = value;

    public unsafe void GetValue(int x, int y, out TPixel value) => value = this.Data[(y + 1) * this.m_realResolution + (x + 1)];

    public unsafe TPixel GetValue(float x, float y)
    {
      int num = (int) ((double) this.Resolution * (double) x);
      return this.Data[((int) ((double) this.Resolution * (double) y) + 1) * this.m_realResolution + (num + 1)];
    }

    public unsafe MyCubemapData(int resolution)
    {
      this.m_realResolution = resolution + 2;
      this.Resolution = resolution;
      this.ResolutionMinusOne = resolution - 1;
      this.m_dataBuffer = MyCubemapData<TPixel>.BufferAllocator.Allocate(this.m_realResolution * this.m_realResolution * sizeof (TPixel));
      this.Data = (TPixel*) (void*) this.m_dataBuffer.Ptr;
    }

    public int GetRowStart(int y) => (y + 1) * this.m_realResolution + 1;

    public void CopyRange(
      Vector2I start,
      Vector2I end,
      MyCubemapData<TPixel> other,
      Vector2I oStart,
      Vector2I oEnd)
    {
      Vector2I step1 = MyCubemapHelpers.GetStep(ref start, ref end);
      Vector2I step2 = MyCubemapHelpers.GetStep(ref oStart, ref oEnd);
      TPixel pixel;
      while (start != end)
      {
        other.GetValue(oStart.X, oStart.Y, out pixel);
        this.SetValue(start.X, start.Y, ref pixel);
        start += step1;
        oStart += step2;
      }
      other.GetValue(oStart.X, oStart.Y, out pixel);
      this.SetValue(start.X, start.Y, ref pixel);
    }

    public void CopyRange(
      Vector2I start,
      Vector2I end,
      IMyWrappedCubemapFace other,
      Vector2I oStart,
      Vector2I oEnd)
    {
      if (!(other is MyCubemapData<TPixel> other1))
        return;
      this.CopyRange(start, end, other1, oStart, oEnd);
    }

    public void FinishFace(string name)
    {
      TPixel pixel = default (TPixel);
      this.SetPixel(-1, -1, ref pixel);
      this.SetPixel(this.Resolution, -1, ref pixel);
      this.SetPixel(-1, this.Resolution, ref pixel);
      this.SetPixel(this.Resolution, this.Resolution, ref pixel);
    }

    internal unsafe void SetPixel(int y, int x, ref TPixel pixel) => this.Data[(y + 1) * this.m_realResolution + (x + 1)] = pixel;

    public unsafe void Dispose()
    {
      this.m_dataBuffer.Dispose();
      this.m_dataBuffer = (NativeArray) null;
      this.Data = (TPixel*) null;
    }
  }
}
