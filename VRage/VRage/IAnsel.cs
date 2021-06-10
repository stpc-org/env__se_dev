// Decompiled with JetBrains decompiler
// Type: VRage.IAnsel
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRage
{
  public interface IAnsel
  {
    bool IsSessionEnabled { get; set; }

    bool IsGamePausable { get; set; }

    bool IsCaptureRunning { get; }

    bool IsSessionRunning { get; }

    bool Is360Capturing { get; }

    bool IsMultiresCapturing { get; }

    bool IsOverlayEnabled { get; }

    bool IsInitializedSuccessfuly { get; }

    event Action<int> StartCaptureDelegate;

    event Action StopCaptureDelegate;

    event Action<bool, bool> WarningMessageDelegate;

    event Func<bool> IsSpectatorEnabledDelegate;

    int Init(bool settingsEnableAnselWithSprites);

    void SetCamera(ref MyCameraSetup cameraSetup);

    void GetCamera(out MyCameraSetup cameraSetup);

    void Enable();

    void StopSession();

    void MarkHdrBufferBind();

    void MarkHdrBufferFinished();
  }
}
