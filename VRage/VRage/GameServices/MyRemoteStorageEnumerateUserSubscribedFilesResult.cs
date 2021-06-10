// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.MyRemoteStorageEnumerateUserSubscribedFilesResult
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Collections.Generic;

namespace VRage.GameServices
{
  public struct MyRemoteStorageEnumerateUserSubscribedFilesResult
  {
    public MyGameServiceCallResult Result;
    public int ResultsReturned;
    public int TotalResultCount;
    public List<ulong> FileIds;

    public ulong this[int i] => this.FileIds[i];
  }
}
