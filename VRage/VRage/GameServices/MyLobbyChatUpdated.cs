﻿// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.MyLobbyChatUpdated
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.GameServices
{
  public delegate void MyLobbyChatUpdated(
    IMyLobby lobby,
    ulong changedUserId,
    ulong makingChangeUserId,
    MyChatMemberStateChangeEnum stateChange,
    MyLobbyStatusCode reason);
}
