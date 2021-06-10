// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Models.MyInventoryItemModel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Definitions;
using Sandbox.Game.Screens.Helpers;
using System.Text;
using VRage;
using VRage.Game.Entity;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Screens.Models
{
  public class MyInventoryItemModel : BindableBase
  {
    private string m_name;
    private string m_iconSymbol;
    private float m_amount;
    private BitmapImage m_icon;

    public string Name
    {
      get => this.m_name;
      set => this.SetProperty<string>(ref this.m_name, value, nameof (Name));
    }

    public string IconSymbol
    {
      get => this.m_iconSymbol;
      set => this.SetProperty<string>(ref this.m_iconSymbol, value, nameof (IconSymbol));
    }

    public float Amount
    {
      get => this.m_amount;
      set
      {
        this.SetProperty<float>(ref this.m_amount, value, nameof (Amount));
        MyPhysicalInventoryItem inventoryItem = this.InventoryItem;
        inventoryItem.Amount = (MyFixedPoint) this.m_amount;
        this.InventoryItem = inventoryItem;
        this.RaisePropertyChanged("AmountFormatted");
      }
    }

    public string AmountFormatted
    {
      get
      {
        StringBuilder text = new StringBuilder();
        MyGuiControlInventoryOwner.FormatItemAmount(this.InventoryItem, text);
        return text.ToString();
      }
    }

    public BitmapImage Icon
    {
      get => this.m_icon;
      set => this.SetProperty<BitmapImage>(ref this.m_icon, value, nameof (Icon));
    }

    public MyPhysicalInventoryItem InventoryItem { get; private set; }

    public MyInventoryBase Inventory { get; private set; }

    public MyInventoryItemModel()
    {
    }

    public MyInventoryItemModel(MyPhysicalInventoryItem inventoryItem, MyInventoryBase inventory)
    {
      MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition((MyObjectBuilder_Base) inventoryItem.Content);
      this.Name = physicalItemDefinition.DisplayNameText;
      BitmapImage bitmapImage = new BitmapImage();
      string[] icons = physicalItemDefinition.Icons;
      if ((icons != null ? ((uint) icons.Length > 0U ? 1 : 0) : 0) != 0)
        bitmapImage.TextureAsset = physicalItemDefinition.Icons[0];
      this.Icon = bitmapImage;
      this.Amount = (float) inventoryItem.Amount;
      this.IconSymbol = physicalItemDefinition.IconSymbol.HasValue ? MyTexts.GetString(physicalItemDefinition.IconSymbol.Value) : string.Empty;
      this.InventoryItem = inventoryItem;
      this.Inventory = inventory;
    }
  }
}
