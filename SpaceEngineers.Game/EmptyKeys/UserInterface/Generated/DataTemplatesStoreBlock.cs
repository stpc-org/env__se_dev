// Decompiled with JetBrains decompiler
// Type: EmptyKeys.UserInterface.Generated.DataTemplatesStoreBlock
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Generated.DataTemplatesStoreBlock_Bindings;
using EmptyKeys.UserInterface.Media;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.Screens.ViewModels;
using System;
using System.CodeDom.Compiler;

namespace EmptyKeys.UserInterface.Generated
{
  [GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
  public sealed class DataTemplatesStoreBlock : ResourceDictionary
  {
    private static DataTemplatesStoreBlock singleton = new DataTemplatesStoreBlock();

    public DataTemplatesStoreBlock() => this.InitializeResources();

    public static DataTemplatesStoreBlock Instance => DataTemplatesStoreBlock.singleton;

    private void InitializeResources()
    {
      this.MergedDictionaries.Add((ResourceDictionary) Styles.Instance);
      this.Add((object) typeof (MyInventoryItemModel), (object) new DataTemplate((object) typeof (MyInventoryItemModel), new Func<UIElement, UIElement>(DataTemplatesStoreBlock.r_0_dtMethod)));
      this.Add((object) typeof (MyInventoryTargetModel), (object) new DataTemplate((object) typeof (MyInventoryTargetModel), new Func<UIElement, UIElement>(DataTemplatesStoreBlock.r_1_dtMethod)));
      this.Add((object) typeof (MyOrderItemModel), (object) new DataTemplate((object) typeof (MyOrderItemModel), new Func<UIElement, UIElement>(DataTemplatesStoreBlock.r_2_dtMethod)));
      this.Add((object) typeof (MyStoreItemModel), (object) new DataTemplate((object) typeof (MyStoreItemModel), new Func<UIElement, UIElement>(DataTemplatesStoreBlock.r_3_dtMethod)));
      this.Add((object) "StoreBlockViewModelLocator", (object) new MyStoreBlockViewModelLocator());
      FontManager.Instance.AddFont("InventorySmall", 10f, FontStyle.Regular, "InventorySmall_7.5_Regular");
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryItemModel), "Name", typeof (MyInventoryItemModel_Name_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryItemModel), "Icon", typeof (MyInventoryItemModel_Icon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryItemModel), "AmountFormatted", typeof (MyInventoryItemModel_AmountFormatted_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryItemModel), "IconSymbol", typeof (MyInventoryItemModel_IconSymbol_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryTargetModel), "Icon", typeof (MyInventoryTargetModel_Icon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyInventoryTargetModel), "Name", typeof (MyInventoryTargetModel_Name_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyOrderItemModel), "Icon", typeof (MyOrderItemModel_Icon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyOrderItemModel), "Name", typeof (MyOrderItemModel_Name_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreItemModel), "Icon", typeof (MyStoreItemModel_Icon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreItemModel), "Name", typeof (MyStoreItemModel_Name_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreItemModel), "IsOffer", typeof (MyStoreItemModel_IsOffer_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreItemModel), "IsOrder", typeof (MyStoreItemModel_IsOrder_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreItemModel), "PricePerUnitFormatted", typeof (MyStoreItemModel_PricePerUnitFormatted_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreItemModel), "CurrencyIcon", typeof (MyStoreItemModel_CurrencyIcon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyStoreItemModel), "AmountFormatted", typeof (MyStoreItemModel_AmountFormatted_PropertyInfo));
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
      toolTip.Content = (object) DataTemplatesStoreBlock.tt_e_0_Method();
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
      StackPanel stackPanel = new StackPanel();
      stackPanel.Parent = parent;
      stackPanel.Name = "e_5";
      stackPanel.Orientation = Orientation.Horizontal;
      Image image = new Image();
      stackPanel.Children.Add((UIElement) image);
      image.Name = "e_6";
      image.Height = 24f;
      image.Margin = new Thickness(5f, 5f, 0.0f, 5f);
      image.VerticalAlignment = VerticalAlignment.Center;
      image.Stretch = Stretch.Uniform;
      image.SetBinding(Image.SourceProperty, new Binding("Icon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock);
      textBlock.Name = "e_7";
      textBlock.Margin = new Thickness(5f, 5f, 5f, 5f);
      textBlock.VerticalAlignment = VerticalAlignment.Center;
      textBlock.SetBinding(TextBlock.TextProperty, new Binding("Name")
      {
        UseGeneratedBindings = true
      });
      return (UIElement) stackPanel;
    }

    private static UIElement r_2_dtMethod(UIElement parent)
    {
      Grid grid = new Grid();
      grid.Parent = parent;
      grid.Name = "e_8";
      grid.HorizontalAlignment = HorizontalAlignment.Stretch;
      grid.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid.ColumnDefinitions.Add(new ColumnDefinition());
      Image image = new Image();
      grid.Children.Add((UIElement) image);
      image.Name = "e_9";
      image.Height = 24f;
      image.Margin = new Thickness(5f, 5f, 0.0f, 5f);
      image.Stretch = Stretch.Uniform;
      image.SetBinding(Image.SourceProperty, new Binding("Icon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock = new TextBlock();
      grid.Children.Add((UIElement) textBlock);
      textBlock.Name = "e_10";
      textBlock.Margin = new Thickness(5f, 5f, 5f, 5f);
      textBlock.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock, 1);
      textBlock.SetBinding(TextBlock.TextProperty, new Binding("Name")
      {
        UseGeneratedBindings = true
      });
      return (UIElement) grid;
    }

    private static UIElement r_3_dtMethod(UIElement parent)
    {
      Grid grid = new Grid();
      grid.Parent = parent;
      grid.Name = "e_11";
      grid.Margin = new Thickness(2f, 2f, 2f, 2f);
      grid.HorizontalAlignment = HorizontalAlignment.Stretch;
      grid.VerticalAlignment = VerticalAlignment.Center;
      grid.RowDefinitions.Add(new RowDefinition());
      grid.RowDefinitions.Add(new RowDefinition());
      grid.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      grid.ColumnDefinitions.Add(new ColumnDefinition());
      grid.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      Image image1 = new Image();
      grid.Children.Add((UIElement) image1);
      image1.Name = "e_12";
      image1.Width = 64f;
      image1.Margin = new Thickness(2f, 0.0f, 2f, 0.0f);
      image1.Stretch = Stretch.Uniform;
      Grid.SetRowSpan((UIElement) image1, 2);
      image1.SetBinding(Image.SourceProperty, new Binding("Icon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock1 = new TextBlock();
      grid.Children.Add((UIElement) textBlock1);
      textBlock1.Name = "e_13";
      textBlock1.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock1.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock1, 1);
      textBlock1.SetBinding(TextBlock.TextProperty, new Binding("Name")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock2 = new TextBlock();
      grid.Children.Add((UIElement) textBlock2);
      textBlock2.Name = "e_14";
      textBlock2.Margin = new Thickness(2f, 2f, 4f, 2f);
      textBlock2.HorizontalAlignment = HorizontalAlignment.Right;
      textBlock2.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock2, 2);
      textBlock2.SetBinding(UIElement.VisibilityProperty, new Binding("IsOffer")
      {
        UseGeneratedBindings = true
      });
      textBlock2.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlockView_OfferItem");
      TextBlock textBlock3 = new TextBlock();
      grid.Children.Add((UIElement) textBlock3);
      textBlock3.Name = "e_15";
      textBlock3.Margin = new Thickness(2f, 2f, 4f, 2f);
      textBlock3.HorizontalAlignment = HorizontalAlignment.Right;
      textBlock3.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock3, 2);
      textBlock3.SetBinding(UIElement.VisibilityProperty, new Binding("IsOrder")
      {
        UseGeneratedBindings = true
      });
      textBlock3.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlockView_OrderItem");
      StackPanel stackPanel = new StackPanel();
      grid.Children.Add((UIElement) stackPanel);
      stackPanel.Name = "e_16";
      stackPanel.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel, 1);
      Grid.SetRow((UIElement) stackPanel, 1);
      Grid.SetColumnSpan((UIElement) stackPanel, 2);
      TextBlock textBlock4 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock4);
      textBlock4.Name = "e_17";
      textBlock4.Margin = new Thickness(1f, 1f, 1f, 1f);
      textBlock4.VerticalAlignment = VerticalAlignment.Center;
      textBlock4.FontFamily = new FontFamily("InventorySmall");
      textBlock4.FontSize = 10f;
      textBlock4.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlock_Column_PricePerUnit");
      TextBlock textBlock5 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock5);
      textBlock5.Name = "e_18";
      textBlock5.Margin = new Thickness(4f, 1f, 1f, 1f);
      textBlock5.VerticalAlignment = VerticalAlignment.Center;
      textBlock5.FontFamily = new FontFamily("InventorySmall");
      textBlock5.FontSize = 10f;
      textBlock5.SetBinding(TextBlock.TextProperty, new Binding("PricePerUnitFormatted")
      {
        UseGeneratedBindings = true
      });
      Image image2 = new Image();
      stackPanel.Children.Add((UIElement) image2);
      image2.Name = "e_19";
      image2.Height = 14f;
      image2.Margin = new Thickness(1f, 1f, 1f, 1f);
      image2.HorizontalAlignment = HorizontalAlignment.Right;
      image2.VerticalAlignment = VerticalAlignment.Center;
      image2.Stretch = Stretch.Uniform;
      image2.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock6 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock6);
      textBlock6.Name = "e_20";
      textBlock6.Margin = new Thickness(4f, 1f, 4f, 1f);
      textBlock6.VerticalAlignment = VerticalAlignment.Center;
      textBlock6.Text = "|";
      textBlock6.FontFamily = new FontFamily("InventorySmall");
      textBlock6.FontSize = 10f;
      TextBlock textBlock7 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock7);
      textBlock7.Name = "e_21";
      textBlock7.Margin = new Thickness(1f, 1f, 1f, 1f);
      textBlock7.VerticalAlignment = VerticalAlignment.Center;
      textBlock7.FontFamily = new FontFamily("InventorySmall");
      textBlock7.FontSize = 10f;
      textBlock7.SetResourceReference(TextBlock.TextProperty, (object) "StoreBlock_Column_Amount");
      TextBlock textBlock8 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock8);
      textBlock8.Name = "e_22";
      textBlock8.Margin = new Thickness(4f, 1f, 1f, 1f);
      textBlock8.VerticalAlignment = VerticalAlignment.Center;
      textBlock8.FontFamily = new FontFamily("InventorySmall");
      textBlock8.FontSize = 10f;
      textBlock8.SetBinding(TextBlock.TextProperty, new Binding("AmountFormatted")
      {
        UseGeneratedBindings = true
      });
      return (UIElement) grid;
    }
  }
}
