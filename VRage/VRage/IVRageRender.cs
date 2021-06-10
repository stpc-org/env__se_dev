// Decompiled with JetBrains decompiler
// Type: VRage.IVRageRender
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using VRageRender;

namespace VRage
{
  public interface IVRageRender
  {
    void CreateRenderDevice(
      ref MyRenderDeviceSettings? settings,
      out object deviceInstance,
      out object swapChain);

    void DisposeRenderDevice();

    void SuspendRenderContext();

    void ResumeRenderContext();

    MyRenderPresetEnum GetRenderQualityHint();

    MyAdapterInfo[] GetRenderAdapterList();

    void ApplyRenderSettings(MyRenderDeviceSettings? settings);

    object CreateRenderAnnotation(object deviceContext);

    ulong GetMemoryBudgetForStreamedResources();

    bool UseParallelRenderInit { get; }

    event Action OnResuming;

    event Action OnSuspending;

    void RequestSuspendWait();

    bool IsRenderOutputDebugSupported { get; }

    void SetMemoryUsedForImprovedGFX(long bytes);

    void FlushIndirectArgsFromComputeShader(object deviceContext);

    ulong GetMemoryBudgetForGeneratedTextures();

    ulong GetMemoryBudgetForVoxelTextureArrays();
  }
}
