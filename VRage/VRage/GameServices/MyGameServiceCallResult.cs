// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.MyGameServiceCallResult
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.GameServices
{
  public enum MyGameServiceCallResult
  {
    OK = 1,
    Fail = 2,
    NoConnection = 3,
    InvalidPassword = 5,
    LoggedInElsewhere = 6,
    InvalidProtocolVer = 7,
    InvalidParam = 8,
    FileNotFound = 9,
    Busy = 10, // 0x0000000A
    InvalidState = 11, // 0x0000000B
    InvalidName = 12, // 0x0000000C
    InvalidEmail = 13, // 0x0000000D
    DuplicateName = 14, // 0x0000000E
    AccessDenied = 15, // 0x0000000F
    Timeout = 16, // 0x00000010
    Banned = 17, // 0x00000011
    AccountNotFound = 18, // 0x00000012
    InvalidSteamID = 19, // 0x00000013
    ServiceUnavailable = 20, // 0x00000014
    NotLoggedOn = 21, // 0x00000015
    Pending = 22, // 0x00000016
    EncryptionFailure = 23, // 0x00000017
    InsufficientPrivilege = 24, // 0x00000018
    LimitExceeded = 25, // 0x00000019
    Revoked = 26, // 0x0000001A
    Expired = 27, // 0x0000001B
    AlreadyRedeemed = 28, // 0x0000001C
    DuplicateRequest = 29, // 0x0000001D
    AlreadyOwned = 30, // 0x0000001E
    IPNotFound = 31, // 0x0000001F
    PersistFailed = 32, // 0x00000020
    LockingFailed = 33, // 0x00000021
    LogonSessionReplaced = 34, // 0x00000022
    ConnectFailed = 35, // 0x00000023
    HandshakeFailed = 36, // 0x00000024
    IOFailure = 37, // 0x00000025
    RemoteDisconnect = 38, // 0x00000026
    ShoppingCartNotFound = 39, // 0x00000027
    Blocked = 40, // 0x00000028
    Ignored = 41, // 0x00000029
    NoMatch = 42, // 0x0000002A
    AccountDisabled = 43, // 0x0000002B
    ServiceReadOnly = 44, // 0x0000002C
    AccountNotFeatured = 45, // 0x0000002D
    AdministratorOK = 46, // 0x0000002E
    ContentVersion = 47, // 0x0000002F
    TryAnotherCM = 48, // 0x00000030
    PasswordRequiredToKickSession = 49, // 0x00000031
    AlreadyLoggedInElsewhere = 50, // 0x00000032
    Suspended = 51, // 0x00000033
    Cancelled = 52, // 0x00000034
    DataCorruption = 53, // 0x00000035
    DiskFull = 54, // 0x00000036
    RemoteCallFailed = 55, // 0x00000037
    PasswordUnset = 56, // 0x00000038
    ExternalAccountUnlinked = 57, // 0x00000039
    PSNTicketInvalid = 58, // 0x0000003A
    ExternalAccountAlreadyLinked = 59, // 0x0000003B
    RemoteFileConflict = 60, // 0x0000003C
    IllegalPassword = 61, // 0x0000003D
    SameAsPreviousValue = 62, // 0x0000003E
    AccountLogonDenied = 63, // 0x0000003F
    CannotUseOldPassword = 64, // 0x00000040
    InvalidLoginAuthCode = 65, // 0x00000041
    AccountLogonDeniedNoMail = 66, // 0x00000042
    HardwareNotCapableOfIPT = 67, // 0x00000043
    IPTInitError = 68, // 0x00000044
    ParentalControlRestricted = 69, // 0x00000045
    FacebookQueryError = 70, // 0x00000046
    ExpiredLoginAuthCode = 71, // 0x00000047
    IPLoginRestrictionFailed = 72, // 0x00000048
    AccountLockedDown = 73, // 0x00000049
    AccountLogonDeniedVerifiedEmailRequired = 74, // 0x0000004A
    NoMatchingURL = 75, // 0x0000004B
    BadResponse = 76, // 0x0000004C
    RequirePasswordReEntry = 77, // 0x0000004D
    ValueOutOfRange = 78, // 0x0000004E
    NoUser = 79, // 0x0000004F
    SSOUnsupported = 80, // 0x00000050
    PlatformRestricted = 81, // 0x00000051
    PlatformPublishRestricted = 82, // 0x00000052
  }
}
