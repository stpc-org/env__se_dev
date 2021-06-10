// Decompiled with JetBrains decompiler
// Type: EmptyKeys.UserInterface.Generated.DataTemplates
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Generated.DataTemplates_Bindings;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.Screens.ViewModels;
using System;
using System.CodeDom.Compiler;

namespace EmptyKeys.UserInterface.Generated
{
  [GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
  public sealed class DataTemplates : ResourceDictionary
  {
    private static DataTemplates singleton = new DataTemplates();

    public DataTemplates() => this.InitializeResources();

    public static DataTemplates Instance => DataTemplates.singleton;

    private void InitializeResources()
    {
      this.MergedDictionaries.Add((ResourceDictionary) Styles.Instance);
      this.Add((object) typeof (MyInventoryItemModel), (object) new DataTemplate((object) typeof (MyInventoryItemModel), new Func<UIElement, UIElement>(DataTemplates.r_0_dtMethod)));
      this.Add((object) "InventoryItemDetailed", (object) new DataTemplate((object) typeof (MyInventoryItemModel), new Func<UIElement, UIElement>(DataTemplates.r_1_dtMethod)));
      this.Add((object) "InventoryItemDetailedWithRemove", (object) new DataTemplate((object) typeof (MyInventoryItemModel), new Func<UIElement, UIElement>(DataTemplates.r_2_dtMethod)));
      this.Add((object) "PlayerTradeViewModelLocator", (object) new MyPlayerTradeViewModelLocator());
      FontManager.Instance.AddFont("InventorySmall", 10f, FontStyle.Regular, "InventorySmall_7.5_Regular");
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryItemModel), "Name", typeof (MyInventoryItemModel_Name_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryItemModel), "Icon", typeof (MyInventoryItemModel_Icon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryItemModel), "AmountFormatted", typeof (MyInventoryItemModel_AmountFormatted_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryItemModel), "IconSymbol", typeof (MyInventoryItemModel_IconSymbol_PropertyInfo));
    }

    private static UIElement tt_e_0_Method()
    {
      TextBlock textBlock = new TextBlock();
      textBlock.Name = "e_1";
      textBlock.Margin = new Thickness(4f, 4f, 4f, 4f);
      textBlock.VerticalAlignment = VerticalAlignment.Center;
      textBlock.SetBinding(TextBlock.TextProperty, new Binding("Name")
      {
        UseGeneratedBindings = true
      });
      return (UIElement) textBlock;
    }

    private static UIElement r_0_dtMethod(UIElement parent)
    {
      Grid grid = new Grid();
      grid.Parent = parent;
      grid.Name = "e_0";
      grid.Height = 64f;
      grid.Width = 64f;
      ToolTip toolTip = new ToolTip();
      grid.ToolTip = (object) toolTip;
      toolTip.Content = (object) DataTemplates.tt_e_0_Method();
      MouseBinding mouseBinding1 = new MouseBinding();
      mouseBinding1.Gesture = (InputGesture) new MouseGesture(MouseAction.LeftDoubleClick, ModifierKeys.None);
      mouseBinding1.SetBinding(InputBinding.CommandProperty, new Binding("ViewModel.AddItemToOfferCommand")
      {
        Source = (object) new MyPlayerTradeViewModelLocator(false)
      });
      Binding binding1 = new Binding();
      mouseBinding1.SetBinding(InputBinding.CommandParameterProperty, binding1);
      grid.InputBindings.Add((InputBinding) mouseBinding1);
      mouseBinding1.Parent = (UIElement) grid;
      MouseBinding mouseBinding2 = new MouseBinding();
      mouseBinding2.Gesture = (InputGesture) new MouseGesture(MouseAction.LeftDoubleClick, ModifierKeys.Control);
      mouseBinding2.SetBinding(InputBinding.CommandProperty, new Binding("ViewModel.AddStackTenToOfferCommand")
      {
        Source = (object) new MyPlayerTradeViewModelLocator(false)
      });
      Binding binding2 = new Binding();
      mouseBinding2.SetBinding(InputBinding.CommandParameterProperty, binding2);
      grid.InputBindings.Add((InputBinding) mouseBinding2);
      mouseBinding2.Parent = (UIElement) grid;
      MouseBinding mouseBinding3 = new MouseBinding();
      mouseBinding3.Gesture = (InputGesture) new MouseGesture(MouseAction.LeftDoubleClick, ModifierKeys.Shift);
      mouseBinding3.SetBinding(InputBinding.CommandProperty, new Binding("ViewModel.AddStackHundredToOfferCommand")
      {
        Source = (object) new MyPlayerTradeViewModelLocator(false)
      });
      Binding binding3 = new Binding();
      mouseBinding3.SetBinding(InputBinding.CommandParameterProperty, binding3);
      grid.InputBindings.Add((InputBinding) mouseBinding3);
      mouseBinding3.Parent = (UIElement) grid;
      Image image = new Image();
      grid.Children.Add((UIElement) image);
      image.Name = "e_2";
      image.Stretch = Stretch.Fill;
      image.SetBinding(Image.SourceProperty, new Binding("Icon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock1 = new TextBlock();
      grid.Children.Add((UIElement) textBlock1);
      textBlock1.Name = "e_3";
      textBlock1.Margin = new Thickness(6f, 0.0f, 0.0f, 4f);
      textBlock1.HorizontalAlignment = HorizontalAlignment.Left;
      textBlock1.VerticalAlignment = VerticalAlignment.Bottom;
      textBlock1.TextAlignment = TextAlignment.Left;
      textBlock1.FontFamily = new FontFamily("InventorySmall");
      textBlock1.FontSize = 10f;
      textBlock1.FontStyle = FontStyle.Regular;
      textBlock1.SetBinding(TextBlock.TextProperty, new Binding("AmountFormatted")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock2 = new TextBlock();
      grid.Children.Add((UIElement) textBlock2);
      textBlock2.Name = "e_4";
      textBlock2.Margin = new Thickness(6f, 4f, 0.0f, 0.0f);
      textBlock2.HorizontalAlignment = HorizontalAlignment.Left;
      textBlock2.VerticalAlignment = VerticalAlignment.Top;
      textBlock2.TextAlignment = TextAlignment.Left;
      textBlock2.FontFamily = new FontFamily("InventorySmall");
      textBlock2.FontSize = 10f;
      textBlock2.SetBinding(TextBlock.TextProperty, new Binding("IconSymbol")
      {
        UseGeneratedBindings = true
      });
      return (UIElement) grid;
    }

    private static UIElement r_1_dtMethod(UIElement parent)
    {
      Grid grid = new Grid();
      grid.Parent = parent;
      grid.Name = "e_5";
      grid.HorizontalAlignment = HorizontalAlignment.Stretch;
      grid.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid.ColumnDefinitions.Add(new ColumnDefinition());
      Border border = new Border();
      grid.Children.Add((UIElement) border);
      border.Name = "e_6";
      border.Height = 64f;
      border.Width = 64f;
      border.SetResourceReference(Control.BackgroundProperty, (object) "GridItem");
      Image image = new Image();
      border.Child = (UIElement) image;
      image.Name = "e_7";
      image.Margin = new Thickness(5f, 5f, 5f, 5f);
      image.Stretch = Stretch.Fill;
      image.SetBinding(Image.SourceProperty, new Binding("Icon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock1 = new TextBlock();
      grid.Children.Add((UIElement) textBlock1);
      textBlock1.Name = "e_8";
      textBlock1.Margin = new Thickness(6f, 0.0f, 0.0f, 4f);
      textBlock1.HorizontalAlignment = HorizontalAlignment.Left;
      textBlock1.VerticalAlignment = VerticalAlignment.Bottom;
      textBlock1.TextAlignment = TextAlignment.Left;
      textBlock1.FontFamily = new FontFamily("InventorySmall");
      textBlock1.FontSize = 10f;
      textBlock1.FontStyle = FontStyle.Regular;
      textBlock1.SetBinding(TextBlock.TextProperty, new Binding("AmountFormatted")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock2 = new TextBlock();
      grid.Children.Add((UIElement) textBlock2);
      textBlock2.Name = "e_9";
      textBlock2.Margin = new Thickness(6f, 4f, 0.0f, 0.0f);
      textBlock2.HorizontalAlignment = HorizontalAlignment.Left;
      textBlock2.VerticalAlignment = VerticalAlignment.Top;
      textBlock2.TextAlignment = TextAlignment.Left;
      textBlock2.FontFamily = new FontFamily("InventorySmall");
      textBlock2.FontSize = 10f;
      textBlock2.SetBinding(TextBlock.TextProperty, new Binding("IconSymbol")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock3 = new TextBlock();
      grid.Children.Add((UIElement) textBlock3);
      textBlock3.Name = "e_10";
      textBlock3.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock3, 1);
      textBlock3.SetBinding(TextBlock.TextProperty, new Binding("Name")
      {
        UseGeneratedBindings = true
      });
      return (UIElement) grid;
    }

    private static UIElement r_2_dtMethod(UIElement parent)
    {
      Grid grid = new Grid();
      grid.Parent = parent;
      grid.Name = "e_11";
      grid.HorizontalAlignment = HorizontalAlignment.Stretch;
      MouseBinding mouseBinding1 = new MouseBinding();
      mouseBinding1.Gesture = (InputGesture) new MouseGesture(MouseAction.LeftDoubleClick, ModifierKeys.None);
      mouseBinding1.SetBinding(InputBinding.CommandProperty, new Binding("ViewModel.RemoveItemFromOfferCommand")
      {
        Source = (object) new MyPlayerTradeViewModelLocator(false)
      });
      Binding binding1 = new Binding();
      mouseBinding1.SetBinding(InputBinding.CommandParameterProperty, binding1);
      grid.InputBindings.Add((InputBinding) mouseBinding1);
      mouseBinding1.Parent = (UIElement) grid;
      MouseBinding mouseBinding2 = new MouseBinding();
      mouseBinding2.Gesture = (InputGesture) new MouseGesture(MouseAction.LeftDoubleClick, ModifierKeys.Control);
      mouseBinding2.SetBinding(InputBinding.CommandProperty, new Binding("ViewModel.RemoveStackTenToOfferCommand")
      {
        Source = (object) new MyPlayerTradeViewModelLocator(false)
      });
      Binding binding2 = new Binding();
      mouseBinding2.SetBinding(InputBinding.CommandParameterProperty, binding2);
      grid.InputBindings.Add((InputBinding) mouseBinding2);
      mouseBinding2.Parent = (UIElement) grid;
      MouseBinding mouseBinding3 = new MouseBinding();
      mouseBinding3.Gesture = (InputGesture) new MouseGesture(MouseAction.LeftDoubleClick, ModifierKeys.Shift);
      mouseBinding3.SetBinding(InputBinding.CommandProperty, new Binding("ViewModel.RemoveStackHundredToOfferCommand")
      {
        Source = (object) new MyPlayerTradeViewModelLocator(false)
      });
      Binding binding3 = new Binding();
      mouseBinding3.SetBinding(InputBinding.CommandParameterProperty, binding3);
      grid.InputBindings.Add((InputBinding) mouseBinding3);
      mouseBinding3.Parent = (UIElement) grid;
      grid.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition = new ColumnDefinition();
      grid.ColumnDefinitions.Add(columnDefinition);
      Border border = new Border();
      grid.Children.Add((UIElement) border);
      border.Name = "e_12";
      border.Height = 64f;
      border.Width = 64f;
      border.SetResourceReference(Control.BackgroundProperty, (object) "GridItem");
      Image image = new Image();
      border.Child = (UIElement) image;
      image.Name = "e_13";
      image.Margin = new Thickness(5f, 5f, 5f, 5f);
      image.Stretch = Stretch.Fill;
      image.SetBinding(Image.SourceProperty, new Binding("Icon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock1 = new TextBlock();
      grid.Children.Add((UIElement) textBlock1);
      textBlock1.Name = "e_14";
      textBlock1.Margin = new Thickness(6f, 0.0f, 0.0f, 4f);
      textBlock1.HorizontalAlignment = HorizontalAlignment.Left;
      textBlock1.VerticalAlignment = VerticalAlignment.Bottom;
      textBlock1.TextAlignment = TextAlignment.Left;
      textBlock1.FontFamily = new FontFamily("InventorySmall");
      textBlock1.FontSize = 10f;
      textBlock1.FontStyle = FontStyle.Regular;
      textBlock1.SetBinding(TextBlock.TextProperty, new Binding("AmountFormatted")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock2 = new TextBlock();
      grid.Children.Add((UIElement) textBlock2);
      textBlock2.Name = "e_15";
      textBlock2.Margin = new Thickness(6f, 4f, 0.0f, 0.0f);
      textBlock2.HorizontalAlignment = HorizontalAlignment.Left;
      textBlock2.VerticalAlignment = VerticalAlignment.Top;
      textBlock2.TextAlignment = TextAlignment.Left;
      textBlock2.FontFamily = new FontFamily("InventorySmall");
      textBlock2.FontSize = 10f;
      textBlock2.SetBinding(TextBlock.TextProperty, new Binding("IconSymbol")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock3 = new TextBlock();
      grid.Children.Add((UIElement) textBlock3);
      textBlock3.Name = "e_16";
      textBlock3.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock3, 1);
      textBlock3.SetBinding(TextBlock.TextProperty, new Binding("Name")
      {
        UseGeneratedBindings = true
      });
      return (UIElement) grid;
    }
  }
}
