// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.MyWorkshopItem
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using VRage.Collections;

namespace VRage.GameServices
{
  public class MyWorkshopItem
  {
    protected List<string> m_tags = new List<string>();
    protected List<ulong> m_dependencies = new List<ulong>();
    protected List<uint> m_DLCs = new List<uint>();
    public static IComparer<MyWorkshopItem> NameComparer = (IComparer<MyWorkshopItem>) new MyWorkshopItem.MyWorkshopItemComparer();

    public string Title { get; set; }

    public string Description { get; protected set; }

    public string Thumbnail { get; protected set; }

    public string Folder { get; protected set; }

    public MyWorkshopItemType ItemType { get; protected set; }

    public ulong Id { get; set; }

    public ulong OwnerId { get; protected set; }

    public DateTime TimeUpdated { get; protected set; }

    public DateTime LocalTimeUpdated { get; protected set; }

    public DateTime TimeCreated { get; protected set; }

    public virtual ulong BytesDownloaded { get; protected set; }

    public virtual ulong BytesTotal { get; protected set; }

    public virtual float DownloadProgress { get; protected set; }

    public ulong Size { get; protected set; }

    public MyModMetadata Metadata { get; protected set; }

    public MyPublishedFileVisibility Visibility { get; protected set; }

    public MyWorkshopItemState State { get; protected set; }

    public MyModCompatibility Compatibility { get; set; }

    public ListReader<string> Tags => (ListReader<string>) this.m_tags;

    public ListReader<ulong> Dependencies => (ListReader<ulong>) this.m_dependencies;

    public ListReader<uint> DLCs => (ListReader<uint>) this.m_DLCs;

    public float Score { get; protected set; }

    public int MyRating { get; protected set; }

    public ulong NumSubscriptions { get; set; }

    public string PreviewImageFile { get; protected set; }

    public virtual string ServiceName { get; }

    public event MyWorkshopItem.DownloadItemResult ItemDownloaded;

    public virtual MyWorkshopItemPublisher GetPublisher() => (MyWorkshopItemPublisher) null;

    public virtual void Download()
    {
    }

    public virtual void Rate(bool positive)
    {
    }

    public virtual void UpdateRating()
    {
    }

    public virtual void DownloadPreviewImage(
      string directory,
      Action<MyWorkshopItem, bool> completeCallback)
    {
      throw new NotImplementedException();
    }

    public virtual void UpdateState()
    {
    }

    public virtual void Subscribe() => throw new NotImplementedException();

    public virtual void Unsubscribe() => throw new NotImplementedException();

    public virtual bool IsUpToDate() => this.State.HasFlag((Enum) MyWorkshopItemState.Installed) && !this.State.HasFlag((Enum) MyWorkshopItemState.NeedsUpdate);

    protected virtual void OnItemDownloaded(MyGameServiceCallResult result, ulong publishedId)
    {
      MyWorkshopItem.DownloadItemResult itemDownloaded = this.ItemDownloaded;
      if (itemDownloaded == null)
        return;
      itemDownloaded(result, publishedId);
    }

    protected bool Equals(MyWorkshopItem other) => (long) this.Id == (long) other.Id;

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (this == obj)
        return true;
      return !(obj.GetType() != this.GetType()) && this.Equals((MyWorkshopItem) obj);
    }

    public override int GetHashCode() => this.Id.GetHashCode();

    public override string ToString() => string.Format("[{0}] {1}", (object) this.Id, (object) (this.Title ?? "N/A"));

    public virtual void UpdateDependencyBlocking()
    {
    }

    public virtual void Report(string reason)
    {
    }

    public void ChangeRatingValue(bool positive) => this.MyRating = positive ? 1 : -1;

    public virtual string GetItemUrl() => string.Empty;

    public void Init(MyWorkshopItemPublisher publisher)
    {
      this.Id = publisher.Id;
      this.Title = publisher.Title;
      this.Description = publisher.Description;
      this.Metadata = publisher.Metadata;
      this.Visibility = publisher.Visibility;
      this.m_tags = new List<string>((IEnumerable<string>) publisher.Tags);
      this.m_DLCs = new List<uint>((IEnumerable<uint>) publisher.DLCs);
      this.m_dependencies = new List<ulong>((IEnumerable<ulong>) publisher.Dependencies);
    }

    public delegate void DownloadItemResult(MyGameServiceCallResult result, ulong publishedId);

    private class MyWorkshopItemComparer : IComparer<MyWorkshopItem>
    {
      public int Compare(MyWorkshopItem x, MyWorkshopItem y)
      {
        if (x == null && y == null)
          return 0;
        if (x != null && y == null)
          return 1;
        return x == null && y != null ? -1 : string.CompareOrdinal(x.Title, y.Title);
      }
    }
  }
}
