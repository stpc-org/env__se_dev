// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Models.MyFactionIconModel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Mvvm;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Screens.Models
{
  public class MyFactionIconModel : BindableBase
  {
    private float CONST_OPACITY_ENABLED = 1f;
    private float CONST_OPACITY_DISABLED = 0.8f;
    private BitmapImage m_icon;
    private ColorW m_iconColor;
    private bool m_isEnabled = true;
    private string m_tooltipText;

    public SerializableDefinitionId GroupId { get; set; }

    public int Id { get; set; }

    public BitmapImage Icon
    {
      get => this.m_icon;
      set => this.SetProperty<BitmapImage>(ref this.m_icon, value, nameof (Icon));
    }

    public string TooltipText
    {
      get => this.m_tooltipText;
      set => this.SetProperty<string>(ref this.m_tooltipText, value, nameof (TooltipText));
    }

    public bool IsEnabled
    {
      get => this.m_isEnabled;
      set
      {
        this.SetProperty<bool>(ref this.m_isEnabled, value, nameof (IsEnabled));
        this.RaisePropertyChanged("Opacity");
      }
    }

    public ColorW IconColor
    {
      get => this.m_iconColor;
      set => this.SetProperty<ColorW>(ref this.m_iconColor, value, nameof (IconColor));
    }

    public float Opacity => !this.IsEnabled ? this.CONST_OPACITY_DISABLED : this.CONST_OPACITY_ENABLED;

    public MyFactionIconModel(
      SerializableDefinitionId groupId,
      int id,
      string iconPath,
      ColorW iconColor,
      bool isEnabled = true,
      string tooltipText = "")
    {
      this.GroupId = groupId;
      this.Id = id;
      this.IconColor = iconColor;
      this.Icon = new BitmapImage()
      {
        TextureAsset = iconPath
      };
      this.IsEnabled = isEnabled;
      this.TooltipText = tooltipText;
    }
  }
}
