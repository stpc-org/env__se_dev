// Decompiled with JetBrains decompiler
// Type: EmptyKeys.UserInterface.Generated.ActiveContractsView_Gamepad
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Controls.Primitives;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Generated.ActiveContractsView_Gamepad_Bindings;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Themes;
using Sandbox.Game.Screens.ViewModels;
using System.CodeDom.Compiler;

namespace EmptyKeys.UserInterface.Generated
{
  [GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
  public class ActiveContractsView_Gamepad : UIRoot
  {
    private Grid rootGrid;
    private Image e_0;
    private Grid e_1;
    private ImageButton e_2;
    private StackPanel e_3;
    private TextBlock e_4;
    private Border e_5;
    private Grid e_6;
    private ListBox e_7;
    private Grid e_8;
    private TextBlock e_9;
    private Border e_10;
    private ContentPresenter e_11;
    private Border e_12;
    private Grid DataGridHelp;
    private TextBlock e_13;
    private TextBlock e_14;
    private TextBlock e_15;

    public ActiveContractsView_Gamepad() => this.Initialize();

    public ActiveContractsView_Gamepad(int width, int height)
      : base(width, height)
      => this.Initialize();

    private void Initialize()
    {
      Style rootStyle = RootStyle.CreateRootStyle();
      rootStyle.TargetType = this.GetType();
      this.Style = rootStyle;
      this.InitializeComponent();
    }

    private void InitializeComponent()
    {
      this.Background = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 0));
      this.MessageBoxOverlay = new ColorW(0, 0, 0, 240);
      ActiveContractsView_Gamepad.InitializeElementResources((UIElement) this);
      this.rootGrid = new Grid();
      this.Content = (object) this.rootGrid;
      this.rootGrid.Name = "rootGrid";
      this.rootGrid.HorizontalAlignment = HorizontalAlignment.Center;
      this.rootGrid.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(0.1f, GridUnitType.Star)
      });
      this.rootGrid.RowDefinitions.Add(new RowDefinition());
      this.rootGrid.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(0.1f, GridUnitType.Star)
      });
      this.rootGrid.SetBinding(UIElement.MaxWidthProperty, new Binding("MaxWidth")
      {
        UseGeneratedBindings = true
      });
      this.e_0 = new Image();
      this.rootGrid.Children.Add((UIElement) this.e_0);
      this.e_0.Name = "e_0";
      this.e_0.Source = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Screens\\screen_background.dds"
      };
      this.e_0.Stretch = Stretch.Fill;
      Grid.SetRow((UIElement) this.e_0, 1);
      this.e_0.SetBinding(ImageBrush.ColorOverlayProperty, new Binding("BackgroundOverlay")
      {
        UseGeneratedBindings = true
      });
      this.e_1 = new Grid();
      this.rootGrid.Children.Add((UIElement) this.e_1);
      this.e_1.Name = "e_1";
      this.e_1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_1.RowDefinitions.Add(new RowDefinition());
      this.e_1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_1.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(75f, GridUnitType.Pixel)
      });
      this.e_1.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(4f, GridUnitType.Star)
      });
      this.e_1.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(75f, GridUnitType.Pixel)
      });
      Grid.SetRow((UIElement) this.e_1, 1);
      this.e_2 = new ImageButton();
      this.e_1.Children.Add((UIElement) this.e_2);
      this.e_2.Name = "e_2";
      this.e_2.Height = 24f;
      this.e_2.Width = 24f;
      this.e_2.Margin = new Thickness(16f, 16f, 16f, 16f);
      this.e_2.HorizontalAlignment = HorizontalAlignment.Right;
      this.e_2.VerticalAlignment = VerticalAlignment.Center;
      this.e_2.IsTabStop = false;
      this.e_2.ImageStretch = Stretch.Uniform;
      this.e_2.ImageNormal = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol.dds"
      };
      this.e_2.ImageHover = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds"
      };
      this.e_2.ImagePressed = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds"
      };
      Grid.SetColumn((UIElement) this.e_2, 2);
      this.e_2.SetBinding(Button.CommandProperty, new Binding("ExitCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_3 = new StackPanel();
      this.e_1.Children.Add((UIElement) this.e_3);
      this.e_3.Name = "e_3";
      Grid.SetColumn((UIElement) this.e_3, 1);
      Grid.SetRow((UIElement) this.e_3, 1);
      this.e_4 = new TextBlock();
      this.e_3.Children.Add((UIElement) this.e_4);
      this.e_4.Name = "e_4";
      this.e_4.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_4.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      this.e_4.SetResourceReference(TextBlock.TextProperty, (object) "ScreenCaptionActiveContracts");
      this.e_5 = new Border();
      this.e_3.Children.Add((UIElement) this.e_5);
      this.e_5.Name = "e_5";
      this.e_5.Height = 2f;
      this.e_5.Margin = new Thickness(0.0f, 10f, 0.0f, 10f);
      this.e_5.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      this.e_6 = new Grid();
      this.e_1.Children.Add((UIElement) this.e_6);
      this.e_6.Name = "e_6";
      GamepadBinding gamepadBinding1 = new GamepadBinding();
      gamepadBinding1.Gesture = (InputGesture) new GamepadGesture(GamepadInput.AButton);
      gamepadBinding1.SetBinding(InputBinding.CommandProperty, new Binding("AbandonCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_6.InputBindings.Add((InputBinding) gamepadBinding1);
      gamepadBinding1.Parent = (UIElement) this.e_6;
      GamepadBinding gamepadBinding2 = new GamepadBinding();
      gamepadBinding2.Gesture = (InputGesture) new GamepadGesture(GamepadInput.DButton);
      gamepadBinding2.SetBinding(InputBinding.CommandProperty, new Binding("RefreshActiveCommand")
      {
        UseGeneratedBindings = true
      });
      this.e_6.InputBindings.Add((InputBinding) gamepadBinding2);
      gamepadBinding2.Parent = (UIElement) this.e_6;
      this.e_6.RowDefinitions.Add(new RowDefinition());
      this.e_6.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_6.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(65f, GridUnitType.Pixel)
      });
      this.e_6.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(2f, GridUnitType.Star)
      });
      this.e_6.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(3f, GridUnitType.Star)
      });
      Grid.SetColumn((UIElement) this.e_6, 1);
      Grid.SetRow((UIElement) this.e_6, 3);
      this.e_7 = new ListBox();
      this.e_6.Children.Add((UIElement) this.e_7);
      this.e_7.Name = "e_7";
      this.e_7.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      this.e_7.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      this.e_7.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      this.e_7.TabIndex = 0;
      this.e_7.SelectionMode = SelectionMode.Single;
      GamepadHelp.SetTargetName((DependencyObject) this.e_7, "DataGridHelp");
      this.e_7.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("ActiveContracts")
      {
        UseGeneratedBindings = true
      });
      this.e_7.SetBinding(Selector.SelectedIndexProperty, new Binding("SelectedActiveContractIndex")
      {
        UseGeneratedBindings = true
      });
      this.e_7.SetBinding(Selector.SelectedItemProperty, new Binding("SelectedActiveContract")
      {
        UseGeneratedBindings = true
      });
      ActiveContractsView_Gamepad.InitializeElemente_7Resources((UIElement) this.e_7);
      this.e_8 = new Grid();
      this.e_6.Children.Add((UIElement) this.e_8);
      this.e_8.Name = "e_8";
      this.e_8.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(30f, GridUnitType.Pixel)
      });
      this.e_8.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      this.e_8.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Star)
      });
      Grid.SetColumn((UIElement) this.e_8, 0);
      Grid.SetRow((UIElement) this.e_8, 0);
      this.e_8.SetBinding(UIElement.VisibilityProperty, new Binding("IsNoActiveContractVisible")
      {
        UseGeneratedBindings = true
      });
      this.e_9 = new TextBlock();
      this.e_8.Children.Add((UIElement) this.e_9);
      this.e_9.Name = "e_9";
      this.e_9.Margin = new Thickness(5f, 5f, 5f, 5f);
      this.e_9.HorizontalAlignment = HorizontalAlignment.Center;
      Grid.SetRow((UIElement) this.e_9, 1);
      this.e_9.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Text_NoActiveContracts");
      this.e_10 = new Border();
      this.e_6.Children.Add((UIElement) this.e_10);
      this.e_10.Name = "e_10";
      this.e_10.VerticalAlignment = VerticalAlignment.Stretch;
      this.e_10.Background = (Brush) new SolidColorBrush(new ColorW(33, 44, 53, (int) byte.MaxValue));
      this.e_10.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      this.e_10.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      Grid.SetColumn((UIElement) this.e_10, 1);
      this.e_11 = new ContentPresenter();
      this.e_10.Child = (UIElement) this.e_11;
      this.e_11.Name = "e_11";
      this.e_11.Margin = new Thickness(5f, 0.0f, 0.0f, 0.0f);
      Grid.SetColumn((UIElement) this.e_11, 1);
      this.e_11.SetBinding(UIElement.DataContextProperty, new Binding("SelectedActiveContract")
      {
        UseGeneratedBindings = true
      });
      this.e_11.SetBinding(ContentPresenter.ContentProperty, new Binding("SelectedActiveContract")
      {
        UseGeneratedBindings = true
      });
      this.e_12 = new Border();
      this.e_6.Children.Add((UIElement) this.e_12);
      this.e_12.Name = "e_12";
      this.e_12.Height = 2f;
      this.e_12.Margin = new Thickness(0.0f, 10f, 0.0f, 0.0f);
      this.e_12.Background = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      Grid.SetRow((UIElement) this.e_12, 1);
      Grid.SetColumnSpan((UIElement) this.e_12, 2);
      this.DataGridHelp = new Grid();
      this.e_6.Children.Add((UIElement) this.DataGridHelp);
      this.DataGridHelp.Name = "DataGridHelp";
      this.DataGridHelp.VerticalAlignment = VerticalAlignment.Center;
      this.DataGridHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.DataGridHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.DataGridHelp.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      this.DataGridHelp.ColumnDefinitions.Add(new ColumnDefinition());
      Grid.SetRow((UIElement) this.DataGridHelp, 2);
      Grid.SetColumnSpan((UIElement) this.DataGridHelp, 2);
      this.e_13 = new TextBlock();
      this.DataGridHelp.Children.Add((UIElement) this.e_13);
      this.e_13.Name = "e_13";
      this.e_13.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      this.e_13.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_13.VerticalAlignment = VerticalAlignment.Center;
      this.e_13.SetResourceReference(TextBlock.TextProperty, (object) "ActiveContractsScreen_Help_Abandon");
      this.e_14 = new TextBlock();
      this.DataGridHelp.Children.Add((UIElement) this.e_14);
      this.e_14.Name = "e_14";
      this.e_14.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      this.e_14.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_14.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) this.e_14, 1);
      this.e_14.SetResourceReference(TextBlock.TextProperty, (object) "ActiveContractsScreen_Help_Refresh");
      this.e_15 = new TextBlock();
      this.DataGridHelp.Children.Add((UIElement) this.e_15);
      this.e_15.Name = "e_15";
      this.e_15.Margin = new Thickness(10f, 0.0f, 5f, 0.0f);
      this.e_15.HorizontalAlignment = HorizontalAlignment.Center;
      this.e_15.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) this.e_15, 2);
      this.e_15.SetResourceReference(TextBlock.TextProperty, (object) "Gamepad_Help_Back");
      ImageManager.Instance.AddImage("Textures\\GUI\\Screens\\screen_background.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol_highlight.dds");
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsActiveViewModel), "MaxWidth", typeof (MyContractsActiveViewModel_MaxWidth_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsActiveViewModel), "BackgroundOverlay", typeof (MyContractsActiveViewModel_BackgroundOverlay_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsActiveViewModel), "ExitCommand", typeof (MyContractsActiveViewModel_ExitCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsActiveViewModel), "AbandonCommand", typeof (MyContractsActiveViewModel_AbandonCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsActiveViewModel), "RefreshActiveCommand", typeof (MyContractsActiveViewModel_RefreshActiveCommand_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsActiveViewModel), "ActiveContracts", typeof (MyContractsActiveViewModel_ActiveContracts_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsActiveViewModel), "SelectedActiveContractIndex", typeof (MyContractsActiveViewModel_SelectedActiveContractIndex_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsActiveViewModel), "SelectedActiveContract", typeof (MyContractsActiveViewModel_SelectedActiveContract_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractsActiveViewModel), "IsNoActiveContractVisible", typeof (MyContractsActiveViewModel_IsNoActiveContractVisible_PropertyInfo));
    }

    private static void InitializeElementResources(UIElement elem)
    {
      elem.Resources.MergedDictionaries.Add((ResourceDictionary) Styles.Instance);
      elem.Resources.MergedDictionaries.Add((ResourceDictionary) DataTemplatesContracts.Instance);
    }

    private static void InitializeElemente_7Resources(UIElement elem) => elem.Resources.MergedDictionaries.Add((ResourceDictionary) DataTemplatesContractsDataGrid.Instance);
  }
}
