// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.ViewModels.MyWorkshopBrowserViewModel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Engine.Networking;
using Sandbox.Game.Screens.Models;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.GameServices;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Screens.ViewModels
{
  public class MyWorkshopBrowserViewModel : MyViewModelBase
  {
    private readonly uint m_maxCachedPages = 5;
    private readonly int m_textureCheckFrame = 5;
    private int m_selectedSortIndex;
    private int m_selectedCategoryIndex;
    private int m_loadedImages;
    private int m_currentFrame;
    private List<string> m_textureList = new List<string>();
    private ConcurrentQueue<string> m_loadingTexturesQueue;
    private ConcurrentDictionary<string, MyWorkshopItemModel> m_textureModelsCache = new ConcurrentDictionary<string, MyWorkshopItemModel>();
    private MyWorkshopQuery m_query;
    private uint m_queryStartPage;
    private uint m_currentPage;
    private uint m_totalPages;
    private bool m_isQueryFinished;
    private bool m_isDetailVisible;
    private bool m_isSearchLabelVisible;
    private bool m_isFilterDirty;
    private bool m_isNotFoundTextVisible;
    private bool m_isPagingInfoVisible;
    private string m_searchText;
    private ConcurrentDictionary<uint, List<MyWorkshopItemModel>> m_pagesData = new ConcurrentDictionary<uint, List<MyWorkshopItemModel>>();
    private ObservableCollection<MyWorkshopItemModel> m_workshopItems;
    private MyWorkshopItemModel m_selectedWorkshopItem;
    private List<MyModCategoryModel> m_categories;
    private ICommand m_previousPageCommand;
    private ICommand m_nextPageCommand;
    private ICommand m_refreshCommand;
    private ICommand m_serviceCommand;
    private ICommand m_subscribeCommand;
    private ICommand m_browseWorkshopCommand;
    private ICommand m_toggleSubscriptionCommand;
    private ICommand m_clearSearchTextCommand;
    private ICommand m_openItemInWorkshopCommand;
    private IMyUGCService m_service;
    private bool m_isWorkshopAggregator;
    private int m_categoryControlTabIndexRight;
    private int m_searchControlTabIndexLeft;
    private bool m_isService0Checked;
    private bool m_isService1Checked;
    public Action OnItemSubscriptionChanged;
    private bool m_service0;

    public string AdditionalTag { get; set; }

    public int SelectedCategoryIndex
    {
      get => this.m_selectedCategoryIndex;
      set => this.SetProperty<int>(ref this.m_selectedCategoryIndex, value, nameof (SelectedCategoryIndex));
    }

    public int SelectedSortIndex
    {
      get => this.m_selectedSortIndex;
      set
      {
        if (this.m_selectedSortIndex != value)
          this.SetProperty<int>(ref this.m_selectedSortIndex, value, nameof (SelectedSortIndex));
        if (this.IsQueryFinished)
          this.RefreshData();
        else
          this.m_isFilterDirty = true;
      }
    }

    public uint CurrentPage
    {
      get => this.m_currentPage;
      set => this.SetProperty<uint>(ref this.m_currentPage, value, nameof (CurrentPage));
    }

    public uint TotalPages
    {
      get => this.m_totalPages;
      set => this.SetProperty<uint>(ref this.m_totalPages, value, nameof (TotalPages));
    }

    public bool IsDetailVisible
    {
      get => this.m_isDetailVisible;
      set => this.SetProperty<bool>(ref this.m_isDetailVisible, value, nameof (IsDetailVisible));
    }

    public bool IsSearchLabelVisible
    {
      get => this.m_isSearchLabelVisible;
      set => this.SetProperty<bool>(ref this.m_isSearchLabelVisible, value, nameof (IsSearchLabelVisible));
    }

    public bool IsQueryFinished
    {
      get => this.m_isQueryFinished;
      set
      {
        this.SetProperty<bool>(ref this.m_isQueryFinished, value, nameof (IsQueryFinished));
        this.RaisePropertyChanged("IsRefreshing");
      }
    }

    public bool IsRefreshing => !this.IsQueryFinished;

    public bool IsWorkshopAggregator
    {
      get => this.m_isWorkshopAggregator;
      private set
      {
        this.m_isWorkshopAggregator = value;
        this.RaisePropertyChanged(nameof (IsWorkshopAggregator));
      }
    }

    public int CategoryControlTabIndexRight
    {
      get => this.m_categoryControlTabIndexRight;
      private set
      {
        this.m_categoryControlTabIndexRight = value;
        this.RaisePropertyChanged(nameof (CategoryControlTabIndexRight));
      }
    }

    public int SearchControlTabIndexLeft
    {
      get => this.m_searchControlTabIndexLeft;
      private set
      {
        this.m_searchControlTabIndexLeft = value;
        this.RaisePropertyChanged(nameof (SearchControlTabIndexLeft));
      }
    }

    public bool Service0IsChecked
    {
      get => this.m_isService0Checked;
      set => this.SetProperty<bool>(ref this.m_isService0Checked, value, nameof (Service0IsChecked));
    }

    public bool Service1IsChecked
    {
      get => this.m_isService1Checked;
      set => this.SetProperty<bool>(ref this.m_isService1Checked, value, nameof (Service1IsChecked));
    }

    public string SearchText
    {
      get => this.m_searchText;
      set
      {
        this.SetProperty<string>(ref this.m_searchText, value, nameof (SearchText));
        this.IsSearchLabelVisible = string.IsNullOrEmpty(this.m_searchText);
        if (this.IsQueryFinished)
          this.RefreshData();
        else
          this.m_isFilterDirty = true;
      }
    }

    public MyWorkshopItemModel SelectedWorkshopItem
    {
      get => this.m_selectedWorkshopItem;
      set
      {
        this.SetProperty<MyWorkshopItemModel>(ref this.m_selectedWorkshopItem, value, nameof (SelectedWorkshopItem));
        this.IsDetailVisible = this.m_selectedWorkshopItem != null;
      }
    }

    public ObservableCollection<MyWorkshopItemModel> WorkshopItems
    {
      get => this.m_workshopItems;
      set => this.SetProperty<ObservableCollection<MyWorkshopItemModel>>(ref this.m_workshopItems, value, nameof (WorkshopItems));
    }

    public List<MyModCategoryModel> Categories
    {
      get => this.m_categories;
      set => this.SetProperty<List<MyModCategoryModel>>(ref this.m_categories, value, nameof (Categories));
    }

    public ICommand NextPageCommand
    {
      get => this.m_nextPageCommand;
      set => this.SetProperty<ICommand>(ref this.m_nextPageCommand, value, nameof (NextPageCommand));
    }

    public ICommand PreviousPageCommand
    {
      get => this.m_previousPageCommand;
      set => this.SetProperty<ICommand>(ref this.m_previousPageCommand, value, nameof (PreviousPageCommand));
    }

    public ICommand RefreshCommand
    {
      get => this.m_refreshCommand;
      set => this.SetProperty<ICommand>(ref this.m_refreshCommand, value, nameof (RefreshCommand));
    }

    public ICommand ServiceCommand
    {
      get => this.m_serviceCommand;
      set => this.SetProperty<ICommand>(ref this.m_serviceCommand, value, nameof (ServiceCommand));
    }

    public ICommand SubscribeCommand
    {
      get => this.m_subscribeCommand;
      set => this.SetProperty<ICommand>(ref this.m_subscribeCommand, value, nameof (SubscribeCommand));
    }

    public ICommand BrowseWorkshopCommand
    {
      get => this.m_browseWorkshopCommand;
      set => this.SetProperty<ICommand>(ref this.m_browseWorkshopCommand, value, nameof (BrowseWorkshopCommand));
    }

    public ICommand ToggleSubscriptionCommand
    {
      get => this.m_toggleSubscriptionCommand;
      set => this.SetProperty<ICommand>(ref this.m_toggleSubscriptionCommand, value, nameof (ToggleSubscriptionCommand));
    }

    public ICommand ClearSearchTextCommand
    {
      get => this.m_clearSearchTextCommand;
      set => this.SetProperty<ICommand>(ref this.m_clearSearchTextCommand, value, nameof (ClearSearchTextCommand));
    }

    public ICommand OpenItemInWorkshopCommand
    {
      get => this.m_openItemInWorkshopCommand;
      set => this.SetProperty<ICommand>(ref this.m_openItemInWorkshopCommand, value, nameof (OpenItemInWorkshopCommand));
    }

    public bool IsNotFoundTextVisible
    {
      get => this.m_isNotFoundTextVisible;
      set
      {
        this.SetProperty<bool>(ref this.m_isNotFoundTextVisible, value, nameof (IsNotFoundTextVisible));
        this.IsPagingInfoVisible = !this.IsNotFoundTextVisible;
      }
    }

    public bool IsPagingInfoVisible
    {
      get => this.m_isPagingInfoVisible;
      set => this.SetProperty<bool>(ref this.m_isPagingInfoVisible, value, nameof (IsPagingInfoVisible));
    }

    public MyWorkshopBrowserViewModel()
      : base()
    {
      if (!MyGameService.AtLeastOneUGCServiceConsented)
        this.OnWorkshopConsentClick(new Action(this.OnAgree), new Action(this.OnDisagree));
      this.CurrentPage = 1U;
      this.IsSearchLabelVisible = true;
      this.NextPageCommand = (ICommand) new RelayCommand(new Action<object>(this.OnNextPage));
      this.PreviousPageCommand = (ICommand) new RelayCommand(new Action<object>(this.OnPreviousPage));
      this.RefreshCommand = (ICommand) new RelayCommand(new Action<object>(this.OnRefresh));
      this.ServiceCommand = (ICommand) new RelayCommand(new Action<object>(this.OnService));
      this.SubscribeCommand = (ICommand) new RelayCommand(new Action<object>(this.OnSubscribe));
      this.BrowseWorkshopCommand = (ICommand) new RelayCommand(new Action<object>(this.OnBrowseWorkshop));
      this.OpenItemInWorkshopCommand = (ICommand) new RelayCommand(new Action<object>(this.OnOpenItemInWorkshop));
      this.ToggleSubscriptionCommand = (ICommand) new RelayCommand(new Action<object>(this.OnToggleSubscription));
      this.ClearSearchTextCommand = (ICommand) new RelayCommand(new Action<object>(this.OnClearSearchText));
    }

    public override void InitializeData()
    {
      base.InitializeData();
      if (!MyGameService.IsActive)
        return;
      this.SelectedSortIndex = 0;
      this.FillCategories();
      this.SelectedCategoryIndex = 0;
      this.TotalPages = 1U;
      this.IsNotFoundTextVisible = false;
      this.m_isFilterDirty = false;
      this.m_service0 = true;
      this.SetupService();
      this.UpdateQuery();
      this.m_queryStartPage = 1U;
      this.IsQueryFinished = false;
      if (this.m_query == null)
        return;
      this.m_query.Run(this.m_queryStartPage);
    }

    private void FillCategories()
    {
      List<MyModCategoryModel> modCategoryModelList = new List<MyModCategoryModel>();
      MyWorkshop.Category[] categoryArray = MyWorkshop.ModCategories;
      if (this.AdditionalTag == MySteamConstants.TAG_WORLDS)
        categoryArray = MyWorkshop.WorldCategories;
      else if (this.AdditionalTag == MySteamConstants.TAG_BLUEPRINTS)
        categoryArray = MyWorkshop.BlueprintCategories;
      else if (this.AdditionalTag == MySteamConstants.TAG_SCENARIOS)
        categoryArray = MyWorkshop.ScenarioCategories;
      foreach (MyWorkshop.Category category in categoryArray)
      {
        if (category.IsVisibleForFilter)
        {
          MyModCategoryModel modCategoryModel = new MyModCategoryModel()
          {
            Id = category.Id,
            LocalizedName = MyTexts.GetString(category.LocalizableName)
          };
          modCategoryModel.PropertyChanged += new PropertyChangedEventHandler(this.Model_PropertyChanged);
          modCategoryModelList.Add(modCategoryModel);
        }
      }
      this.Categories = modCategoryModelList;
    }

    private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (!(e.PropertyName == "IsChecked"))
        return;
      if (this.IsQueryFinished)
        this.RefreshData();
      else
        this.m_isFilterDirty = true;
    }

    private void OnPageQueryCompleted(MyGameServiceCallResult result, uint page)
    {
      if (this.m_query.ItemsPerPage == 0U || this.m_query.TotalResults == 0U || (result != MyGameServiceCallResult.OK || this.m_isFilterDirty))
      {
        this.m_query.Stop();
        this.WorkshopItems = (ObservableCollection<MyWorkshopItemModel>) null;
        this.TotalPages = 0U;
        this.IsNotFoundTextVisible = true;
      }
      else
      {
        this.TotalPages = Math.Max(1U, this.m_query.TotalResults / this.m_query.ItemsPerPage + (this.m_query.TotalResults % this.m_query.ItemsPerPage == 0U ? 0U : 1U));
        if ((int) page == (int) this.m_maxCachedPages + (int) this.m_currentPage)
        {
          this.m_query.Stop();
        }
        else
        {
          List<MyWorkshopItemModel> workshopItemModelList = (List<MyWorkshopItemModel>) null;
          if (!this.m_pagesData.TryGetValue(page, out workshopItemModelList))
          {
            workshopItemModelList = new List<MyWorkshopItemModel>((int) this.m_query.ItemsPerPage);
            this.m_pagesData.TryAdd(page, workshopItemModelList);
          }
          int num1 = (int) this.m_query.ItemsPerPage * ((int) page - (int) this.m_queryStartPage);
          int num2 = (int) this.m_query.ItemsPerPage * ((int) page - (int) this.m_queryStartPage + 1);
          for (int index = num1; index < num2 && index < this.m_query.Items.Count; ++index)
          {
            MyWorkshopItem myWorkshopItem = this.m_query.Items[index];
            MyWorkshopItemModel workshopItemModel = new MyWorkshopItemModel()
            {
              WorkshopItem = myWorkshopItem
            };
            workshopItemModel.OnIsSubscribedChanged += new Action(this.ItemSubscriptionChangedCallback);
            workshopItemModelList.Add(workshopItemModel);
          }
          if ((int) this.CurrentPage != (int) page)
            return;
          this.WorkshopItems = new ObservableCollection<MyWorkshopItemModel>(workshopItemModelList);
          this.DownloadPreviewImages(workshopItemModelList);
        }
      }
    }

    private void ItemSubscriptionChangedCallback()
    {
      if (this.OnItemSubscriptionChanged == null)
        return;
      this.OnItemSubscriptionChanged();
    }

    private void DownloadPreviewImages(List<MyWorkshopItemModel> pageList)
    {
      this.m_loadedImages = pageList.Count;
      this.m_textureList.Clear();
      this.m_loadingTexturesQueue = new ConcurrentQueue<string>();
      this.m_textureModelsCache.Clear();
      foreach (MyWorkshopItemModel page in pageList)
      {
        MyWorkshopItemModel item = page;
        string directory = Path.Combine(MyFileSystem.UserDataPath, "WorkshopBrowser");
        item.WorkshopItem.DownloadPreviewImage(directory, (Action<MyWorkshopItem, bool>) ((workshopItem, result) =>
        {
          if (result)
          {
            this.m_textureList.Add(workshopItem.PreviewImageFile);
            this.m_loadingTexturesQueue.Enqueue(workshopItem.PreviewImageFile);
            this.m_textureModelsCache.TryAdd(workshopItem.PreviewImageFile, item);
          }
          --this.m_loadedImages;
          if (this.m_loadedImages > 0)
            return;
          MyRenderProxy.PreloadTextures((IEnumerable<string>) this.m_textureList, TextureType.GUIWithoutPremultiplyAlpha);
        }));
      }
    }

    public override void Update()
    {
      base.Update();
      ++this.m_currentFrame;
      string result;
      if (this.m_loadingTexturesQueue == null || !this.IsQueryFinished || (this.m_currentFrame % this.m_textureCheckFrame != 0 || !this.m_loadingTexturesQueue.TryDequeue(out result)))
        return;
      if (MyRenderProxy.IsTextureLoaded(result))
      {
        MyWorkshopItemModel workshopItemModel;
        if (!this.m_textureModelsCache.TryGetValue(result, out workshopItemModel))
          return;
        workshopItemModel.OnDownloadPreviewImageCompleted(workshopItemModel.WorkshopItem, true);
      }
      else
        this.m_loadingTexturesQueue.Enqueue(result);
    }

    private void OnQueryCompleted(MyGameServiceCallResult result)
    {
      this.IsQueryFinished = true;
      if (!this.m_isFilterDirty)
        return;
      this.RefreshData();
    }

    private void OnPreviousPage(object obj)
    {
      if (this.CurrentPage <= 1U)
        return;
      --this.CurrentPage;
      this.UpdateWorkshopItemsList(this.CurrentPage);
    }

    private void OnNextPage(object obj)
    {
      if (this.TotalPages <= this.CurrentPage)
        return;
      ++this.CurrentPage;
      this.UpdateWorkshopItemsList(this.CurrentPage);
    }

    private void UpdateWorkshopItemsList(uint page)
    {
      this.m_isFilterDirty = false;
      List<MyWorkshopItemModel> workshopItemModelList = (List<MyWorkshopItemModel>) null;
      if (this.m_pagesData.TryGetValue(page, out workshopItemModelList))
      {
        this.WorkshopItems = new ObservableCollection<MyWorkshopItemModel>(workshopItemModelList);
        this.DownloadPreviewImages(workshopItemModelList);
      }
      else
      {
        if (this.m_query.IsRunning)
          return;
        this.m_queryStartPage = this.CurrentPage;
        this.UpdateQuery();
        this.IsNotFoundTextVisible = false;
        this.m_query.Run(this.m_queryStartPage);
      }
    }

    private void OnRefresh(object obj) => this.RefreshData();

    public void OnService(object obj)
    {
      this.m_service0 = !this.m_service0;
      this.SetupService();
    }

    private void SetupService()
    {
      this.DisposeQuery();
      this.Service0IsChecked = this.m_service0;
      this.Service1IsChecked = !this.m_service0;
      if (MyGameService.WorkshopService.GetAggregates().Count > 1)
      {
        this.m_service = MyGameService.WorkshopService.GetAggregates()[this.m_service0 ? 0 : 1];
        this.IsWorkshopAggregator = true;
        this.CategoryControlTabIndexRight = 11;
        this.SearchControlTabIndexLeft = 12;
      }
      else
      {
        this.m_service = MyGameService.GetDefaultUGC();
        this.IsWorkshopAggregator = false;
        this.CategoryControlTabIndexRight = 4;
        this.SearchControlTabIndexLeft = 3;
      }
      if (this.m_currentFrame > 1 && !this.m_service.IsConsentGiven)
      {
        this.m_service0 = !this.m_service0;
        if (!(this.m_service.ServiceName == "mod.io"))
          return;
        this.OnWorkshopConsentClick(new Action(this.OnAgree), new Action(this.OnDisagree));
      }
      else
        this.SetupServiceRunQuery();
    }

    private void OnAgree()
    {
      MyScreenManager.CloseScreenNow(ServiceManager.Instance.GetService<IMyGuiScreenFactoryService>().GetMyGuiScreenBase(typeof (MyWorkshopBrowserViewModel)));
      MyWorkshop.OpenWorkshopBrowser(MySteamConstants.TAG_WORLDS);
    }

    private void OnDisagree()
    {
      MyScreenManager.CloseScreenNow(ServiceManager.Instance.GetService<IMyGuiScreenFactoryService>().GetMyGuiScreenBase(typeof (MyWorkshopBrowserViewModel)));
      MyWorkshop.OpenWorkshopBrowser(MySteamConstants.TAG_WORLDS);
    }

    private void OnWorkshopConsentClick(Action onConsentAgree = null, Action onConsentOptOut = null)
    {
      MyModIoConsentViewModel consentViewModel = new MyModIoConsentViewModel(onConsentAgree, onConsentOptOut);
      IMyGuiScreenFactoryService service = ServiceManager.Instance.GetService<IMyGuiScreenFactoryService>();
      MyScreenManager.CloseScreenNow(service.GetMyGuiScreenBase(typeof (MyWorkshopBrowserViewModel)));
      service.CreateScreen((ViewModelBase) consentViewModel);
    }

    private void SetupServiceRunQueryFromConsentScreen()
    {
      this.m_service0 = !this.m_service0;
      this.SetupServiceRunQuery();
    }

    private void SetupServiceRunQuery()
    {
      this.m_query = this.m_service.CreateWorkshopQuery();
      this.m_query.ItemsPerPage = MyPlatformGameSettings.WORKSHOP_BROWSER_ITEMS_PER_PAGE;
      this.m_query.ItemType = WorkshopItemType.Mod;
      this.m_query.QueryCompleted += new MyWorkshopQuery.QueryCompletedDelegate(this.OnQueryCompleted);
      this.m_query.PageQueryCompleted += new MyWorkshopQuery.PageQueryCompletedDelegate(this.OnPageQueryCompleted);
      if (!MySandboxGame.Config.ExperimentalMode)
      {
        if (this.m_query.ExcludedTags == null)
          this.m_query.ExcludedTags = new List<string>();
        this.m_query.ExcludedTags.Add(MySteamConstants.TAG_EXPERIMENTAL);
      }
      this.RefreshData();
    }

    private void UpdateQuery()
    {
      if (this.m_query == null)
        return;
      this.m_query.SearchString = this.SearchText;
      if (this.SelectedSortIndex == 3)
      {
        this.m_query.UserId = MyGameService.UserId;
      }
      else
      {
        this.m_query.QueryType = (MyWorkshopQueryType) this.SelectedSortIndex;
        this.m_query.UserId = 0UL;
      }
      if (this.m_query.RequiredTags == null)
        this.m_query.RequiredTags = new List<string>();
      else
        this.m_query.RequiredTags.Clear();
      foreach (MyModCategoryModel category in this.Categories)
      {
        if (category.IsChecked)
          this.m_query.RequiredTags.Add(category.Id);
      }
      if (string.IsNullOrEmpty(this.AdditionalTag))
        return;
      this.m_query.RequiredTags.Add(this.AdditionalTag);
      this.m_query.RequireAllTags = true;
      if (MyVRage.Platform.Scripting.IsRuntimeCompilationSupported || !(this.AdditionalTag == "mod"))
        return;
      this.m_query.RequiredTags.Add(MySteamConstants.TAG_NO_SCRIPTS);
    }

    private void RefreshData()
    {
      if (this.m_query.IsRunning)
        return;
      this.CleanUpPagesData();
      this.CurrentPage = 1U;
      this.IsQueryFinished = false;
      this.UpdateWorkshopItemsList(this.CurrentPage);
    }

    private void CleanUpPagesData()
    {
      if (this.m_pagesData == null)
        return;
      foreach (List<MyWorkshopItemModel> workshopItemModelList in (IEnumerable<List<MyWorkshopItemModel>>) this.m_pagesData.Values)
      {
        foreach (MyWorkshopItemModel workshopItemModel in workshopItemModelList)
        {
          if (workshopItemModel.PreviewImage != null && workshopItemModel.PreviewImage.Texture != null)
            workshopItemModel.PreviewImage.Texture.Dispose();
        }
      }
      this.m_pagesData.Clear();
    }

    private void OnSubscribe(object obj)
    {
      if (this.SelectedWorkshopItem == null)
        return;
      this.SelectedWorkshopItem.WorkshopItem.Subscribe();
    }

    private void OnToggleSubscription(object obj)
    {
      if (this.SelectedWorkshopItem == null)
        return;
      this.SelectedWorkshopItem.IsSubscribed = !this.SelectedWorkshopItem.IsSubscribed;
    }

    private void OnBrowseWorkshop(object obj) => MyGuiSandbox.OpenUrlWithFallback(this.m_service.GetItemListUrl(this.AdditionalTag), this.m_service.ServiceName + " Workshop");

    private void OnOpenItemInWorkshop(object obj)
    {
      if (this.SelectedWorkshopItem == null)
        return;
      MyGuiSandbox.OpenUrlWithFallback(this.SelectedWorkshopItem.WorkshopItem.GetItemUrl(), this.SelectedWorkshopItem.WorkshopItem.ServiceName + " Workshop");
    }

    private void OnClearSearchText(object obj) => this.SearchText = string.Empty;

    private void DisposeQuery()
    {
      if (this.m_query == null)
        return;
      this.m_query.QueryCompleted -= new MyWorkshopQuery.QueryCompletedDelegate(this.OnQueryCompleted);
      this.m_query.PageQueryCompleted -= new MyWorkshopQuery.PageQueryCompletedDelegate(this.OnPageQueryCompleted);
      this.m_query.Stop();
      this.m_query.Dispose();
    }

    public override void OnScreenClosing()
    {
      this.CleanUpPagesData();
      this.DisposeQuery();
      base.OnScreenClosing();
    }
  }
}
