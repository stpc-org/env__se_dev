// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.IMyUGCService
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.GameServices
{
  public interface IMyUGCService
  {
    string ServiceName { get; }

    string LegalUrl { get; }

    bool IsConsoleCompatible { get; }

    bool IsConsentGiven { get; set; }

    MyWorkshopItem CreateWorkshopItem();

    MyWorkshopItemPublisher CreateWorkshopPublisher();

    MyWorkshopItemPublisher CreateWorkshopPublisher(MyWorkshopItem item);

    MyWorkshopQuery CreateWorkshopQuery();

    void SuspendWorkshopDownloads();

    void ResumeWorkshopDownloads();

    string GetItemListUrl(string requiredTag);

    void SetTestEnvironment(bool testEnabled);

    void Update();
  }
}
