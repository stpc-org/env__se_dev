// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.MyNullMicrophone
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRage.GameServices
{
  public class MyNullMicrophone : IMyMicrophoneService
  {
    public bool FiltersSilence => false;

    public void InitializeVoiceRecording()
    {
    }

    public void DisposeVoiceRecording()
    {
    }

    public void StartVoiceRecording()
    {
    }

    public void StopVoiceRecording()
    {
    }

    public byte[] GetVoiceDataFormat() => throw new NotImplementedException();

    public MyVoiceResult GetAvailableVoice(ref byte[] buffer, out uint size)
    {
      size = 0U;
      return MyVoiceResult.NoData;
    }
  }
}
