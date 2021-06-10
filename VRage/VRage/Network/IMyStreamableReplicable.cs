// Decompiled with JetBrains decompiler
// Type: VRage.Network.IMyStreamableReplicable
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using VRage.Library.Collections;

namespace VRage.Network
{
  public interface IMyStreamableReplicable
  {
    void Serialize(
      BitStream stream,
      HashSet<string> cachedData,
      Endpoint forClient,
      Action writeData);

    void LoadDone(BitStream stream);

    void LoadCancel();

    void CreateStreamingStateGroup();

    IMyStreamingStateGroup GetStreamingStateGroup();

    void OnLoadBegin(Action<bool> loadingDoneHandler);

    bool NeedsToBeStreamed { get; }
  }
}
