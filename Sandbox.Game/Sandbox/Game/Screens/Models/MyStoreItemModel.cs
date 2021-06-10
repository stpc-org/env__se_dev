// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Models.MyStoreItemModel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using VRage.Game;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Utils;

namespace Sandbox.Game.Screens.Models
{
  public class MyStoreItemModel : BindableBase
  {
    private long m_totalPrice;
    private int m_pricePerUnit;
    private int m_amount;
    private string m_name;
    private string m_description;
    private BitmapImage m_icon;
    private bool m_hasTooltip;
    private BitmapImage m_tooltipImage;
    private BitmapImage m_currencyIcon;
    private ItemTypes m_itemTypes;
    private StoreItemTypes m_storeItemType;
    private bool m_isOre;
    private int m_prefabTotalPcu;
    private float m_perPriceDiscount;
    private bool m_hasPricePerUnitDiscount;
    private bool m_hasNormalPrice;

    public long Id { get; set; }

    public int PricePerUnit
    {
      get => this.m_pricePerUnit;
      set
      {
        this.SetProperty<int>(ref this.m_pricePerUnit, value, nameof (PricePerUnit));
        this.RaisePropertyChanged(nameof (PricePerUnit));
      }
    }

    public string PricePerUnitFormatted => MyBankingSystem.GetFormatedValue((long) this.PricePerUnit);

    public long TotalPrice
    {
      get => this.m_totalPrice;
      set
      {
        this.SetProperty<long>(ref this.m_totalPrice, value, nameof (TotalPrice));
        this.RaisePropertyChanged("TotalPriceFormatted");
      }
    }

    public string TotalPriceFormatted => MyBankingSystem.GetFormatedValue(this.TotalPrice);

    public BitmapImage CurrencyIcon
    {
      get => this.m_currencyIcon;
      set => this.SetProperty<BitmapImage>(ref this.m_currencyIcon, value, nameof (CurrencyIcon));
    }

    public int Amount
    {
      get => this.m_amount;
      set
      {
        this.SetProperty<int>(ref this.m_amount, value, nameof (Amount));
        this.RaisePropertyChanged("AmountFormatted");
      }
    }

    public string AmountFormatted
    {
      get
      {
        switch (this.ItemType)
        {
          case ItemTypes.PhysicalItem:
            return this.IsOre ? MyValueFormatter.GetFormattedOreAmount(this.Amount) : MyValueFormatter.GetFormattedPiecesAmount(this.Amount);
          case ItemTypes.Oxygen:
            return MyValueFormatter.GetFormattedGasAmount(this.Amount);
          case ItemTypes.Hydrogen:
            return MyValueFormatter.GetFormattedGasAmount(this.Amount);
          case ItemTypes.Grid:
            return MyValueFormatter.GetFormattedPiecesAmount(this.Amount);
          default:
            return MyValueFormatter.GetFormatedInt(this.Amount);
        }
      }
    }

    public string Name
    {
      get => this.m_name;
      set => this.SetProperty<string>(ref this.m_name, value, nameof (Name));
    }

    public BitmapImage Icon
    {
      get => this.m_icon;
      set => this.SetProperty<BitmapImage>(ref this.m_icon, value, nameof (Icon));
    }

    public string Description
    {
      get => this.m_description;
      set => this.SetProperty<string>(ref this.m_description, value, nameof (Description));
    }

    public bool HasTooltip
    {
      get => this.m_hasTooltip;
      set => this.SetProperty<bool>(ref this.m_hasTooltip, value, nameof (HasTooltip));
    }

    public BitmapImage TooltipImage
    {
      get => this.m_tooltipImage;
      set => this.SetProperty<BitmapImage>(ref this.m_tooltipImage, value, nameof (TooltipImage));
    }

    public bool IsOre
    {
      get => this.m_isOre;
      set
      {
        this.SetProperty<bool>(ref this.m_isOre, value, nameof (IsOre));
        this.RaisePropertyChanged("AmountFormatted");
      }
    }

    public ItemTypes ItemType
    {
      get => this.m_itemTypes;
      set
      {
        this.SetProperty<ItemTypes>(ref this.m_itemTypes, value, nameof (ItemType));
        this.RaisePropertyChanged("AmountFormatted");
      }
    }

    public int PrefabTotalPcu
    {
      get => this.m_prefabTotalPcu;
      set => this.SetProperty<int>(ref this.m_prefabTotalPcu, value, nameof (PrefabTotalPcu));
    }

    public StoreItemTypes StoreItemType
    {
      get => this.m_storeItemType;
      set => this.SetProperty<StoreItemTypes>(ref this.m_storeItemType, value, nameof (StoreItemType));
    }

    public bool IsOffer => this.m_storeItemType == StoreItemTypes.Offer;

    public bool IsOrder => this.m_storeItemType == StoreItemTypes.Order;

    public MyDefinitionId ItemDefinitionId { get; set; }

    public float PricePerUnitDiscount
    {
      get => this.m_perPriceDiscount;
      set
      {
        this.SetProperty<float>(ref this.m_perPriceDiscount, value, nameof (PricePerUnitDiscount));
        this.HasPricePerUnitDiscount = (double) this.m_perPriceDiscount > 0.0;
        this.HasNormalPrice = !this.HasPricePerUnitDiscount;
      }
    }

    public bool HasPricePerUnitDiscount
    {
      get => this.m_hasPricePerUnitDiscount;
      set => this.SetProperty<bool>(ref this.m_hasPricePerUnitDiscount, value, nameof (HasPricePerUnitDiscount));
    }

    public bool HasNormalPrice
    {
      get => this.m_hasNormalPrice;
      set => this.SetProperty<bool>(ref this.m_hasNormalPrice, value, nameof (HasNormalPrice));
    }

    public MyStoreItemModel()
    {
      BitmapImage bitmapImage = new BitmapImage();
      string[] icons = MyBankingSystem.BankingSystemDefinition.Icons;
      bitmapImage.TextureAsset = (icons != null ? ((uint) icons.Length > 0U ? 1 : 0) : 0) != 0 ? MyBankingSystem.BankingSystemDefinition.Icons[0] : string.Empty;
      this.CurrencyIcon = bitmapImage;
    }

    public void SetIcon(string iconPath) => this.Icon = new BitmapImage()
    {
      TextureAsset = iconPath
    };

    public void SetTooltipImage(string imagePath) => this.TooltipImage = new BitmapImage()
    {
      TextureAsset = imagePath
    };
  }
}
