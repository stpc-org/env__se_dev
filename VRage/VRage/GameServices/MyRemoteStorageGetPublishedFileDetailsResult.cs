// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.MyRemoteStorageGetPublishedFileDetailsResult
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.GameServices
{
  public struct MyRemoteStorageGetPublishedFileDetailsResult
  {
    public bool AcceptedForUse;
    public bool Banned;
    public uint ConsumerAppID;
    public uint CreatorAppID;
    public string Description;
    public ulong FileHandle;
    public string FileName;
    public int FileSize;
    public ulong PreviewFileHandle;
    public int PreviewFileSize;
    public ulong PublishedFileId;
    public MyGameServiceCallResult Result;
    public ulong SteamIDOwner;
    public string Tags;
    public bool TagsTruncated;
    public uint TimeCreated;
    public uint TimeUpdated;
    public string Title;
    public string URL;
  }
}
