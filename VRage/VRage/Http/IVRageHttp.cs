// Decompiled with JetBrains decompiler
// Type: VRage.Http.IVRageHttp
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Net;

namespace VRage.Http
{
  public interface IVRageHttp
  {
    HttpStatusCode SendRequest(
      string url,
      HttpData[] parameters,
      HttpMethod method,
      out string content);

    void SendRequestAsync(
      string url,
      HttpData[] parameters,
      HttpMethod method,
      Action<HttpStatusCode, string> onDone);

    void DownloadAsync(
      string url,
      string filename,
      Action<ulong> onProgress,
      Action<HttpStatusCode> action);
  }
}
