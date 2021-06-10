// Decompiled with JetBrains decompiler
// Type: VRage.IVideoPlayer
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRage
{
  public interface IVideoPlayer
  {
    int VideoWidth { get; }

    int VideoHeight { get; }

    float Volume { get; set; }

    VideoState CurrentState { get; }

    IntPtr TextureSrv { get; }

    void Init(string filename);

    void Dispose();

    void Play();

    void Stop();

    void Update(object context);
  }
}
