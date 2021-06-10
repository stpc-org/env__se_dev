// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MyTileTexture`1
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using System;
using System.Runtime.CompilerServices;
using VRage;
using VRage.Library;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
  public class MyTileTexture<TPixel> : IDisposable where TPixel : unmanaged
  {
    private static NativeArrayAllocator BufferAllocator = new NativeArrayAllocator(MyPlanet.MemoryTracker.RegisterSubsystem("TileTextures"));
    private int m_stride;
    private Vector2I m_cellSize;
    private unsafe TPixel* m_data;
    private NativeArray m_nativeBuffer;
    private static readonly Vector2B[] s_baseCellCoords = new Vector2B[16]
    {
      new Vector2B((byte) 0, (byte) 0),
      new Vector2B((byte) 1, (byte) 0),
      new Vector2B((byte) 2, (byte) 0),
      new Vector2B((byte) 3, (byte) 0),
      new Vector2B((byte) 0, (byte) 1),
      new Vector2B((byte) 1, (byte) 1),
      new Vector2B((byte) 2, (byte) 1),
      new Vector2B((byte) 3, (byte) 1),
      new Vector2B((byte) 0, (byte) 2),
      new Vector2B((byte) 1, (byte) 2),
      new Vector2B((byte) 2, (byte) 2),
      new Vector2B((byte) 3, (byte) 2),
      new Vector2B((byte) 0, (byte) 3),
      new Vector2B((byte) 1, (byte) 3),
      new Vector2B((byte) 2, (byte) 3),
      new Vector2B((byte) 3, (byte) 3)
    };
    private Vector2I[] m_cellCoords = new Vector2I[16];
    public static readonly MyTileTexture<TPixel> Default = new MyTileTexture<TPixel>();

    static MyTileTexture() => MyVRage.RegisterExitCallback((Action) (() => MyTileTexture<TPixel>.Default.Dispose()));

    private MyTileTexture()
      : this(4, new TPixel[16], 1)
    {
    }

    public MyTileTexture(Vector2I size, int stride, TPixel[] data, int cellSize)
      : this(stride, data, cellSize)
    {
    }

    private unsafe MyTileTexture(int stride, TPixel[] data, int cellSize)
    {
      this.m_stride = stride;
      this.m_cellSize = new Vector2I(cellSize);
      this.m_nativeBuffer = MyTileTexture<TPixel>.BufferAllocator.Allocate(data.Length * sizeof (TPixel));
      this.m_data = (TPixel*) (void*) this.m_nativeBuffer.Ptr;
      fixed (TPixel* pixelPtr = data)
        Unsafe.CopyBlockUnaligned((void*) this.m_data, (void*) pixelPtr, (uint) this.m_nativeBuffer.Size);
      this.PrepareCellCoords();
    }

    private void PrepareCellCoords()
    {
      for (int index = 0; index < 16; ++index)
        this.m_cellCoords[index] = MyTileTexture<TPixel>.s_baseCellCoords[index] * this.m_cellSize.X;
    }

    public unsafe void GetValue(int corners, Vector2I coords, out TPixel value)
    {
      if (corners > 15)
      {
        value = default (TPixel);
      }
      else
      {
        coords += this.m_cellCoords[corners];
        value = this.m_data[coords.X + coords.Y * this.m_stride];
      }
    }

    public unsafe void GetValue(int corners, Vector2 coords, out TPixel value)
    {
      if (corners > 15)
      {
        value = default (TPixel);
      }
      else
      {
        Vector2I vector2I = new Vector2I(coords * (float) this.m_cellSize.X) + this.m_cellCoords[corners];
        value = this.m_data[vector2I.X + vector2I.Y * this.m_stride];
      }
    }

    public unsafe void Dispose()
    {
      MyTileTexture<TPixel>.BufferAllocator.Dispose(this.m_nativeBuffer);
      this.m_nativeBuffer = (NativeArray) null;
      this.m_data = (TPixel*) null;
    }
  }
}
