// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Models.MyInventoryTargetModel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Game.Entities.Blocks;
using VRage.Game.Entity;

namespace Sandbox.Game.Screens.Models
{
  public class MyInventoryTargetModel : BindableBase
  {
    private string m_name;
    private float m_volume;
    private float m_maxVolume;
    private BitmapImage m_icon;

    public string Name
    {
      get => this.m_name;
      set => this.SetProperty<string>(ref this.m_name, value, nameof (Name));
    }

    public long EntityId { get; set; }

    public bool AllInventories { get; set; }

    public float MaxVolume
    {
      get => this.m_maxVolume;
      set => this.SetProperty<float>(ref this.m_maxVolume, value, nameof (MaxVolume));
    }

    public float Volume
    {
      get => this.m_volume;
      set => this.SetProperty<float>(ref this.m_volume, value, nameof (Volume));
    }

    public BitmapImage Icon
    {
      get => this.m_icon;
      set => this.SetProperty<BitmapImage>(ref this.m_icon, value, nameof (Icon));
    }

    public MyInventoryBase Inventory { get; private set; }

    public MyGasTank GasTank { get; set; }

    public MyInventoryTargetModel(MyInventoryBase inventoryBase) => this.Inventory = inventoryBase;
  }
}
