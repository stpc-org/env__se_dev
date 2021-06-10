// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Models.MyWorkshopItemModel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Mvvm;
using System;
using System.Collections.Generic;
using VRage.GameServices;

namespace Sandbox.Game.Screens.Models
{
  public class MyWorkshopItemModel : BindableBase
  {
    private readonly int m_numberOfStars = 5;
    private readonly string m_fullStar = "Textures\\GUI\\Icons\\Rating\\FullStar.png";
    private readonly string m_halfStar = "Textures\\GUI\\Icons\\Rating\\HalfStar.png";
    private readonly string m_noStar = "Textures\\GUI\\Icons\\Rating\\NoStar.png";
    private bool m_isSubscribed;
    private BitmapImage m_previewImage;
    private MyWorkshopItem m_workshopItem;
    private List<BitmapImage> m_rating;
    public Action OnIsSubscribedChanged;

    public ulong Id => this.WorkshopItem == null ? 0UL : this.WorkshopItem.Id;

    public string Title => this.WorkshopItem == null ? string.Empty : this.WorkshopItem.Title;

    public string Description => this.WorkshopItem == null ? string.Empty : this.WorkshopItem.Description;

    public DateTime TimeUpdated => this.WorkshopItem == null ? DateTime.MinValue : this.WorkshopItem.TimeUpdated;

    public DateTime TimeCreated => this.WorkshopItem == null ? DateTime.MinValue : this.WorkshopItem.TimeCreated;

    public ulong Size => this.WorkshopItem == null ? 0UL : this.WorkshopItem.Size;

    public float Score => this.WorkshopItem == null ? 0.0f : this.WorkshopItem.Score;

    public ulong NumSubscriptions => this.WorkshopItem == null ? 0UL : this.WorkshopItem.NumSubscriptions;

    public bool IsSubscribed
    {
      get => this.m_isSubscribed;
      set
      {
        this.SetProperty<bool>(ref this.m_isSubscribed, value, nameof (IsSubscribed));
        if (this.WorkshopItem == null)
          return;
        bool flag = this.WorkshopItem.State.HasFlag((Enum) MyWorkshopItemState.Subscribed);
        if (this.m_isSubscribed && !flag)
          this.WorkshopItem.Subscribe();
        else if (!this.m_isSubscribed & flag)
          this.WorkshopItem.Unsubscribe();
        if (this.OnIsSubscribedChanged == null)
          return;
        this.OnIsSubscribedChanged();
      }
    }

    public MyWorkshopItem WorkshopItem
    {
      get => this.m_workshopItem;
      set
      {
        this.SetProperty<MyWorkshopItem>(ref this.m_workshopItem, value, nameof (WorkshopItem));
        if (this.m_workshopItem == null)
          return;
        this.CreateRating();
        this.IsSubscribed = this.WorkshopItem != null && this.WorkshopItem.State.HasFlag((Enum) MyWorkshopItemState.Subscribed);
      }
    }

    public BitmapImage PreviewImage
    {
      get => this.m_previewImage;
      set => this.SetProperty<BitmapImage>(ref this.m_previewImage, value, nameof (PreviewImage));
    }

    public List<BitmapImage> Rating
    {
      get => this.m_rating;
      set => this.SetProperty<List<BitmapImage>>(ref this.m_rating, value, nameof (Rating));
    }

    internal void OnDownloadPreviewImageCompleted(MyWorkshopItem item, bool success)
    {
      if (item != this.WorkshopItem || !success)
        return;
      ImageManager.Instance.AddImage(item.PreviewImageFile);
      ImageManager.Instance.LoadImages((object) null);
      this.PreviewImage = new BitmapImage()
      {
        TextureAsset = item.PreviewImageFile
      };
    }

    private void CreateRating()
    {
      List<BitmapImage> bitmapImageList = new List<BitmapImage>(this.m_numberOfStars);
      float num1 = this.m_workshopItem.Score * (float) this.m_numberOfStars;
      for (int index = 0; index < (int) num1; ++index)
      {
        BitmapImage bitmapImage = new BitmapImage()
        {
          TextureAsset = this.m_fullStar
        };
        bitmapImageList.Add(bitmapImage);
      }
      if (Math.Round((double) num1, 0, MidpointRounding.AwayFromZero) >= (double) num1)
      {
        BitmapImage bitmapImage = new BitmapImage()
        {
          TextureAsset = this.m_fullStar
        };
        bitmapImageList.Add(bitmapImage);
      }
      else
      {
        BitmapImage bitmapImage = new BitmapImage()
        {
          TextureAsset = this.m_halfStar
        };
        bitmapImageList.Add(bitmapImage);
      }
      int num2 = this.m_numberOfStars - bitmapImageList.Count;
      for (int index = 0; index < num2; ++index)
      {
        BitmapImage bitmapImage = new BitmapImage()
        {
          TextureAsset = this.m_noStar
        };
        bitmapImageList.Add(bitmapImage);
      }
      this.Rating = bitmapImageList;
    }
  }
}
