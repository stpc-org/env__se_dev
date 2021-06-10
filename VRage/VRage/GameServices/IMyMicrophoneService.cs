// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.IMyMicrophoneService
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.GameServices
{
  public interface IMyMicrophoneService
  {
    bool FiltersSilence { get; }

    void InitializeVoiceRecording();

    void DisposeVoiceRecording();

    void StartVoiceRecording();

    void StopVoiceRecording();

    byte[] GetVoiceDataFormat();

    MyVoiceResult GetAvailableVoice(ref byte[] buffer, out uint size);
  }
}
