// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.MyLobbyStatusCode
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.GameServices
{
  public enum MyLobbyStatusCode
  {
    Success = 1,
    DoesntExist = 2,
    NotAllowed = 3,
    Full = 4,
    Error = 5,
    Banned = 6,
    Limited = 7,
    ClanDisabled = 8,
    CommunityBan = 9,
    MemberBlockedYou = 10, // 0x0000000A
    YouBlockedMember = 11, // 0x0000000B
    FriendsOnly = 100, // 0x00000064
    Cancelled = 200, // 0x000000C8
    LostInternetConnection = 201, // 0x000000C9
    ServiceUnavailable = 202, // 0x000000CA
    NoDirectConnections = 203, // 0x000000CB
    VersionMismatch = 204, // 0x000000CC
    UserMultiplayerRestricted = 205, // 0x000000CD
    ConnectionProblems = 206, // 0x000000CE
    InvalidPasscode = 207, // 0x000000CF
    NoUser = 208, // 0x000000D0
  }
}
