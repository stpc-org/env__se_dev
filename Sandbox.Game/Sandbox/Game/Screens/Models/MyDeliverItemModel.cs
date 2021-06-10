// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Models.MyDeliverItemModel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Definitions;

namespace Sandbox.Game.Screens.Models
{
  public class MyDeliverItemModel : BindableBase
  {
    private string m_name;
    private BitmapImage m_icon;

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

    public MyPhysicalItemDefinition ItemDefinition { get; set; }

    public MyDeliverItemModel(MyPhysicalItemDefinition itemDefinition)
    {
      this.Name = itemDefinition.DisplayNameText;
      BitmapImage bitmapImage = new BitmapImage();
      string[] icons = itemDefinition.Icons;
      if ((icons != null ? ((uint) icons.Length > 0U ? 1 : 0) : 0) != 0)
        bitmapImage.TextureAsset = itemDefinition.Icons[0];
      this.Icon = bitmapImage;
      this.ItemDefinition = itemDefinition;
    }
  }
}
