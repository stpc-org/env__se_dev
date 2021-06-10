// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MyHeightDetailTexture
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using System;
using System.Runtime.CompilerServices;
using VRage.Library;

namespace Sandbox.Engine.Voxels
{
  public class MyHeightDetailTexture : IDisposable
  {
    private static NativeArrayAllocator BufferAllocator = new NativeArrayAllocator(MyPlanet.MemoryTracker.RegisterSubsystem("HeightDetailTexture"));
    private NativeArray m_nativeBuffer;

    public uint Resolution { get; }

    public unsafe byte* Data => (byte*) (void*) this.m_nativeBuffer.Ptr;

    public unsafe MyHeightDetailTexture(byte[] data, uint resolution)
    {
      this.Resolution = resolution;
      this.m_nativeBuffer = MyHeightDetailTexture.BufferAllocator.Allocate(data.Length);
      fixed (byte* numPtr = data)
        Unsafe.CopyBlockUnaligned((void*) this.m_nativeBuffer.Ptr, (void*) numPtr, (uint) data.Length);
    }

    public unsafe float GetValue(float x, float y) => (float) this.Data[(long) (int) ((double) y * (double) this.Resolution) * (long) this.Resolution + (long) (int) ((double) x * (double) this.Resolution)] * 0.003921569f;

    public unsafe byte GetValue(int x, int y) => this.Data[(long) y * (long) this.Resolution + (long) x];

    public void Dispose()
    {
      MyHeightDetailTexture.BufferAllocator.Dispose(this.m_nativeBuffer);
      this.m_nativeBuffer = (NativeArray) null;
    }
  }
}
