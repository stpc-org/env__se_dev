// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.MyWorkshopItemPublisher
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;

namespace VRage.GameServices
{
  public class MyWorkshopItemPublisher : IDisposable
  {
    public List<string> Tags = new List<string>();
    public List<ulong> Dependencies = new List<ulong>();
    public HashSet<uint> DLCs = new HashSet<uint>();
    public static IComparer<MyWorkshopItem> NameComparer = (IComparer<MyWorkshopItem>) new MyWorkshopItemPublisher.MyNameComparer();

    public string Title { get; set; }

    public string Description { get; set; }

    public string Thumbnail { get; set; }

    public string Folder { get; set; }

    public ulong Id { get; set; }

    public MyModMetadata Metadata { get; set; }

    public MyPublishedFileVisibility Visibility { get; set; }

    public event MyWorkshopItemPublisher.PublishItemResult ItemPublished;

    protected MyWorkshopItemPublisher()
    {
    }

    ~MyWorkshopItemPublisher() => this.Dispose();

    public virtual void Dispose()
    {
      if (this.Tags != null)
        this.Tags.Clear();
      if (this.Dependencies != null)
        this.Dependencies.Clear();
      if (this.DLCs != null)
        this.DLCs.Clear();
      this.Metadata = (MyModMetadata) null;
    }

    protected void Init(MyWorkshopItem item)
    {
      this.Id = item.Id;
      this.Title = item.Title;
      this.Description = item.Description;
      this.Metadata = item.Metadata;
      this.Visibility = item.Visibility;
      this.Tags = new List<string>((IEnumerable<string>) item.Tags);
      this.DLCs = new HashSet<uint>((IEnumerable<uint>) item.DLCs);
      this.Dependencies = new List<ulong>((IEnumerable<ulong>) item.Dependencies);
    }

    public virtual void Publish()
    {
    }

    protected virtual void OnItemPublished(
      MyGameServiceCallResult result,
      MyWorkshopItem publishedItem)
    {
      MyWorkshopItemPublisher.PublishItemResult itemPublished = this.ItemPublished;
      if (itemPublished == null)
        return;
      itemPublished(result, publishedItem);
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

    public delegate void PublishItemResult(
      MyGameServiceCallResult result,
      MyWorkshopItem publishedItem);

    private class MyNameComparer : IComparer<MyWorkshopItem>
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
