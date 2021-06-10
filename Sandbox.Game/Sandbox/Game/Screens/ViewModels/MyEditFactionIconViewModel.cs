// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.ViewModels.MyEditFactionIconViewModel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Definitions;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VRage;
using VRage.Game.Factions.Definitions;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Screens.ViewModels
{
  public class MyEditFactionIconViewModel : MyViewModelBase, IMyEditFactionIconViewModel
  {
    private ColorW m_factionColor;
    private float m_hue;
    private float m_saturation;
    private float m_value;
    private ColorW m_iconColor;
    private float m_hueIcon;
    private float m_saturationIcon;
    private float m_valueIcon;
    private ICommand m_okCommand;
    private ICommand m_cancelCommand;
    private ICommand m_selectIconCommand;
    private BitmapImage m_factionIconBitmap;
    private string m_iconTexturePath;
    private Vector3 m_runtimeBgColorHsv;
    private Vector3 m_runtimeIconColorHsv;
    private ObservableCollection<MyFactionIconModel> m_factionIcons = new ObservableCollection<MyFactionIconModel>();
    private MyFactionIconModel m_selectedIcon;

    public event Action<MyEditFactionIconViewModel> OnFactionEditorOk;

    public ColorW FactionColor
    {
      get => this.m_factionColor;
      set => this.SetProperty<ColorW>(ref this.m_factionColor, value, nameof (FactionColor));
    }

    public ColorW IconColorInternal
    {
      get => this.m_iconColor;
      set => this.SetProperty<ColorW>(ref this.m_iconColor, value, nameof (IconColorInternal));
    }

    public float Hue
    {
      get => this.m_hue;
      set
      {
        this.SetProperty<float>(ref this.m_hue, value, nameof (Hue));
        this.OnHueChanged(value);
      }
    }

    public float Saturation
    {
      get => this.m_saturation;
      set
      {
        this.SetProperty<float>(ref this.m_saturation, value, nameof (Saturation));
        this.OnSaturationChanged(value);
      }
    }

    public float ColorValue
    {
      get => this.m_value;
      set
      {
        this.SetProperty<float>(ref this.m_value, value, nameof (ColorValue));
        this.OnValueChanged(value);
      }
    }

    public float HueIcon
    {
      get => this.m_hueIcon;
      set
      {
        this.SetProperty<float>(ref this.m_hueIcon, value, nameof (HueIcon));
        this.OnHueIconChanged(value);
      }
    }

    public float SaturationIcon
    {
      get => this.m_saturationIcon;
      set
      {
        this.SetProperty<float>(ref this.m_saturationIcon, value, nameof (SaturationIcon));
        this.OnSaturationIconChanged(value);
      }
    }

    public float ColorValueIcon
    {
      get => this.m_valueIcon;
      set
      {
        this.SetProperty<float>(ref this.m_valueIcon, value, nameof (ColorValueIcon));
        this.OnValueIconChanged(value);
      }
    }

    public BitmapImage FactionIconBitmap
    {
      get => this.m_factionIconBitmap;
      set => this.SetProperty<BitmapImage>(ref this.m_factionIconBitmap, value, nameof (FactionIconBitmap));
    }

    public ICommand OkCommand
    {
      get => this.m_okCommand;
      set => this.SetProperty<ICommand>(ref this.m_okCommand, value, nameof (OkCommand));
    }

    public ICommand CancelCommand
    {
      get => this.m_cancelCommand;
      set => this.SetProperty<ICommand>(ref this.m_cancelCommand, value, nameof (CancelCommand));
    }

    public ICommand SelectIconCommand
    {
      get => this.m_selectIconCommand;
      set => this.SetProperty<ICommand>(ref this.m_selectIconCommand, value, nameof (SelectIconCommand));
    }

    public ObservableCollection<MyFactionIconModel> FactionIcons
    {
      get => this.m_factionIcons;
      set => this.SetProperty<ObservableCollection<MyFactionIconModel>>(ref this.m_factionIcons, value, nameof (FactionIcons));
    }

    public MyFactionIconModel SelectedIcon
    {
      get => this.m_selectedIcon;
      set
      {
        this.SetProperty<MyFactionIconModel>(ref this.m_selectedIcon, value, nameof (SelectedIcon));
        if (this.m_selectedIcon == null)
          return;
        this.OnSelectIcon(this.m_selectedIcon);
      }
    }

    public string ImageIconPath { get; private set; }

    public Color BackgroundColor { get; private set; }

    public Color IconColor { get; private set; }

    public SerializableDefinitionId FactionIconGroupId { get; private set; }

    public int FactionIconId { get; private set; }

    public MyEditFactionIconViewModel(
      SerializableDefinitionId iconGroupId,
      int iconId,
      string imageIconPath,
      Color bgColor,
      Color iconColor)
      : base()
    {
      this.FactionIconGroupId = iconGroupId;
      this.FactionIconId = iconId;
      this.m_iconTexturePath = this.ImageIconPath = imageIconPath;
      this.BackgroundColor = bgColor;
      this.IconColor = iconColor;
      this.m_runtimeBgColorHsv = bgColor.ColorToHSV();
      this.m_runtimeIconColorHsv = iconColor.ColorToHSV();
      this.RefreshIcon(bgColor, iconColor, imageIconPath);
      this.Hue = this.m_runtimeBgColorHsv.X;
      this.Saturation = this.m_runtimeBgColorHsv.Y;
      this.ColorValue = this.m_runtimeBgColorHsv.Z;
      this.HueIcon = this.m_runtimeIconColorHsv.X;
      this.SaturationIcon = this.m_runtimeIconColorHsv.Y;
      this.ColorValueIcon = this.m_runtimeIconColorHsv.Z;
      IEnumerable<MyFactionIconsDefinition> allDefinitions = MyDefinitionManager.Static.GetAllDefinitions<MyFactionIconsDefinition>();
      List<MyFactionIconModel> list = new List<MyFactionIconModel>();
      foreach (MyFactionIconsDefinition factionIconsDefinition in allDefinitions)
      {
        string tooltipText = MyTexts.GetString(MySpaceTexts.Economy_FactionIcon_Tooltip_Allowed);
        bool isEnabled;
        if (factionIconsDefinition.Id.SubtypeId.String == "Other")
        {
          isEnabled = MySession.Static.GetComponent<MySessionComponentDLC>().HasDLC("Economy", Sync.MyId);
          if (!isEnabled)
            tooltipText = MyTexts.GetString(MySpaceTexts.Economy_FactionIcon_Tooltip_BuyEconomy);
        }
        else
          isEnabled = true;
        int id = 0;
        foreach (string icon in factionIconsDefinition.Icons)
        {
          MyFactionIconModel factionIconModel = new MyFactionIconModel((SerializableDefinitionId) factionIconsDefinition.Id, id, icon, new ColorW((int) iconColor.R, (int) iconColor.G, (int) iconColor.B), isEnabled, tooltipText);
          list.Add(factionIconModel);
          ++id;
        }
      }
      this.FactionIcons = new ObservableCollection<MyFactionIconModel>(list);
      this.OkCommand = (ICommand) new RelayCommand(new Action<object>(this.OnOK));
      this.CancelCommand = (ICommand) new RelayCommand(new Action<object>(this.OnCancel));
      this.SelectIconCommand = (ICommand) new RelayCommand<MyFactionIconModel>(new Action<MyFactionIconModel>(this.OnSelectIcon));
      ServiceManager.Instance.AddService<IMyEditFactionIconViewModel>((IMyEditFactionIconViewModel) this);
    }

    private void OnValueChanged(float obj)
    {
      this.m_runtimeBgColorHsv.Z = obj;
      this.RefreshIcon(this.m_runtimeBgColorHsv.HSVtoColor(), this.m_runtimeIconColorHsv.HSVtoColor(), this.m_iconTexturePath);
    }

    private void OnSaturationChanged(float obj)
    {
      this.m_runtimeBgColorHsv.Y = obj;
      this.RefreshIcon(this.m_runtimeBgColorHsv.HSVtoColor(), this.m_runtimeIconColorHsv.HSVtoColor(), this.m_iconTexturePath);
    }

    private void OnHueChanged(float obj)
    {
      float num = obj;
      Vector3 runtimeBgColorHsv = this.m_runtimeBgColorHsv;
      this.m_runtimeBgColorHsv.X = num;
      this.RefreshIcon(this.m_runtimeBgColorHsv.HSVtoColor(), this.m_runtimeIconColorHsv.HSVtoColor(), this.m_iconTexturePath);
    }

    private void OnValueIconChanged(float obj)
    {
      this.m_runtimeIconColorHsv.Z = obj;
      this.RefreshIcon(this.m_runtimeBgColorHsv.HSVtoColor(), this.m_runtimeIconColorHsv.HSVtoColor(), this.m_iconTexturePath);
    }

    private void OnSaturationIconChanged(float obj)
    {
      this.m_runtimeIconColorHsv.Y = obj;
      this.RefreshIcon(this.m_runtimeBgColorHsv.HSVtoColor(), this.m_runtimeIconColorHsv.HSVtoColor(), this.m_iconTexturePath);
    }

    private void OnHueIconChanged(float obj)
    {
      float num = obj;
      Vector3 runtimeIconColorHsv = this.m_runtimeIconColorHsv;
      this.m_runtimeIconColorHsv.X = num;
      this.RefreshIcon(this.m_runtimeBgColorHsv.HSVtoColor(), this.m_runtimeIconColorHsv.HSVtoColor(), this.m_iconTexturePath);
    }

    private void OnOK(object obj)
    {
      this.BackgroundColor = this.m_runtimeBgColorHsv.HSVtoColor();
      this.IconColor = this.m_runtimeIconColorHsv.HSVtoColor();
      this.ImageIconPath = this.m_iconTexturePath;
      MyScreenManager.GetScreenWithFocus().CloseScreen();
      this.OnFactionEditorOk(this);
    }

    private void OnCancel(object obj) => MyScreenManager.GetScreenWithFocus().CloseScreen();

    private void OnSelectIcon(MyFactionIconModel iconItem)
    {
      if (!iconItem.IsEnabled)
        return;
      this.m_iconTexturePath = iconItem.Icon.TextureAsset;
      this.FactionIconGroupId = iconItem.GroupId;
      this.FactionIconId = iconItem.Id;
      this.RefreshIcon(this.m_runtimeBgColorHsv.HSVtoColor(), this.m_runtimeIconColorHsv.HSVtoColor(), this.m_iconTexturePath);
    }

    private void RefreshIcon(Color color, Color iconColor, string imagePath)
    {
      this.FactionColor = new ColorW((int) color.R, (int) color.G, (int) color.B);
      ColorW colorW = new ColorW((int) iconColor.R, (int) iconColor.G, (int) iconColor.B);
      this.IconColorInternal = colorW;
      foreach (MyFactionIconModel factionIcon in (Collection<MyFactionIconModel>) this.FactionIcons)
        factionIcon.IconColor = colorW;
      this.FactionIconBitmap = new BitmapImage()
      {
        TextureAsset = imagePath
      };
    }

    public override void OnScreenClosing() => ServiceManager.Instance.RemoveService<IMyEditFactionIconViewModel>();
  }
}
