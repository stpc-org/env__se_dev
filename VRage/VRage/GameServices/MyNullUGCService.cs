// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.MyNullUGCService
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.GameServices
{
  public class MyNullUGCService : IMyUGCService
  {
    public string ServiceName { get; }

    public string LegalUrl { get; }

    public bool IsConsoleCompatible { get; }

    public bool IsConsentGiven
    {
      get => true;
      set
      {
      }
    }

    public MyWorkshopItem CreateWorkshopItem() => new MyWorkshopItem();

    public MyWorkshopItemPublisher CreateWorkshopPublisher() => (MyWorkshopItemPublisher) new MyNullWorkshopItemPublisher();

    public MyWorkshopItemPublisher CreateWorkshopPublisher(
      MyWorkshopItem item)
    {
      return (MyWorkshopItemPublisher) new MyNullWorkshopItemPublisher();
    }

    public MyWorkshopQuery CreateWorkshopQuery() => (MyWorkshopQuery) new MyNullWorkshopQuery();

    public void SuspendWorkshopDownloads()
    {
    }

    public void ResumeWorkshopDownloads()
    {
    }

    public string GetItemListUrl(string requiredTag) => string.Empty;

    public void SetTestEnvironment(bool testEnabled)
    {
    }

    public void Update()
    {
    }

    public bool IsConsentRequired() => false;
  }
}
