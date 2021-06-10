// Decompiled with JetBrains decompiler
// Type: VRage.Network.JoinResult
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.Network
{
  public enum JoinResult
  {
    OK,
    AlreadyJoined,
    TicketInvalid,
    SteamServersOffline,
    NotInGroup,
    GroupIdInvalid,
    ServerFull,
    BannedByAdmins,
    KickedRecently,
    TicketCanceled,
    TicketAlreadyUsed,
    LoggedInElseWhere,
    NoLicenseOrExpired,
    UserNotConnected,
    VACBanned,
    VACCheckTimedOut,
    PasswordRequired,
    WrongPassword,
    ExperimentalMode,
    ProfilingNotAllowed,
    FamilySharing,
    NotFound,
    IncorrectTime,
  }
}
