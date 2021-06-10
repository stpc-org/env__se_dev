// Decompiled with JetBrains decompiler
// Type: EmptyKeys.UserInterface.Generated.DataTemplatesContractsDataGrid
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Generated.DataTemplatesContractsDataGrid_Bindings;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Media.Imaging;
using Sandbox.Game.Screens.Models;
using System;
using System.CodeDom.Compiler;

namespace EmptyKeys.UserInterface.Generated
{
  [GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
  public sealed class DataTemplatesContractsDataGrid : ResourceDictionary
  {
    private static DataTemplatesContractsDataGrid singleton = new DataTemplatesContractsDataGrid();

    public DataTemplatesContractsDataGrid() => this.InitializeResources();

    public static DataTemplatesContractsDataGrid Instance => DataTemplatesContractsDataGrid.singleton;

    private void InitializeResources()
    {
      this.MergedDictionaries.Add((ResourceDictionary) Styles.Instance);
      this.Add((object) typeof (MyContractModelCustom), (object) new DataTemplate((object) typeof (MyContractModelCustom), new Func<UIElement, UIElement>(DataTemplatesContractsDataGrid.r_0_dtMethod)));
      this.Add((object) typeof (MyContractModelDeliver), (object) new DataTemplate((object) typeof (MyContractModelDeliver), new Func<UIElement, UIElement>(DataTemplatesContractsDataGrid.r_1_dtMethod)));
      this.Add((object) typeof (MyContractModelEscort), (object) new DataTemplate((object) typeof (MyContractModelEscort), new Func<UIElement, UIElement>(DataTemplatesContractsDataGrid.r_2_dtMethod)));
      this.Add((object) typeof (MyContractModelFind), (object) new DataTemplate((object) typeof (MyContractModelFind), new Func<UIElement, UIElement>(DataTemplatesContractsDataGrid.r_3_dtMethod)));
      this.Add((object) typeof (MyContractModelHunt), (object) new DataTemplate((object) typeof (MyContractModelHunt), new Func<UIElement, UIElement>(DataTemplatesContractsDataGrid.r_4_dtMethod)));
      this.Add((object) typeof (MyContractModelObtainAndDeliver), (object) new DataTemplate((object) typeof (MyContractModelObtainAndDeliver), new Func<UIElement, UIElement>(DataTemplatesContractsDataGrid.r_5_dtMethod)));
      this.Add((object) typeof (MyContractModelRepair), (object) new DataTemplate((object) typeof (MyContractModelRepair), new Func<UIElement, UIElement>(DataTemplatesContractsDataGrid.r_6_dtMethod)));
      ImageManager.Instance.AddImage("Textures\\GUI\\Contracts\\ArrowRepGain.png");
      ImageManager.Instance.AddImage("Textures\\GUI\\Contracts\\ArrowRepLoss.png");
      FontManager.Instance.AddFont("InventorySmall", 10f, FontStyle.Regular, "InventorySmall_7.5_Regular");
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelCustom), "Icon", typeof (MyContractModelCustom_Icon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelCustom), "Name", typeof (MyContractModelCustom_Name_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelCustom), "RewardMoney_Formatted", typeof (MyContractModelCustom_RewardMoney_Formatted_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelCustom), "CurrencyIcon", typeof (MyContractModelCustom_CurrencyIcon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelCustom), "RewardReputation_Formatted", typeof (MyContractModelCustom_RewardReputation_Formatted_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelCustom), "FailReputationPenalty_Formated", typeof (MyContractModelCustom_FailReputationPenalty_Formated_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelCustom), "IsFactionIconVisible", typeof (MyContractModelCustom_IsFactionIconVisible_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelCustom), "FactionIconBackgroundColor", typeof (MyContractModelCustom_FactionIconBackgroundColor_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelCustom), "FactionIconTooltip", typeof (MyContractModelCustom_FactionIconTooltip_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelCustom), "FactionIcon", typeof (MyContractModelCustom_FactionIcon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelCustom), "FactionIconColor", typeof (MyContractModelCustom_FactionIconColor_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelDeliver), "Icon", typeof (MyContractModelDeliver_Icon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelDeliver), "Name", typeof (MyContractModelDeliver_Name_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelDeliver), "RewardMoney_Formatted", typeof (MyContractModelDeliver_RewardMoney_Formatted_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelDeliver), "CurrencyIcon", typeof (MyContractModelDeliver_CurrencyIcon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelDeliver), "RewardReputation_Formatted", typeof (MyContractModelDeliver_RewardReputation_Formatted_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelDeliver), "FailReputationPenalty_Formated", typeof (MyContractModelDeliver_FailReputationPenalty_Formated_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelDeliver), "IsFactionIconVisible", typeof (MyContractModelDeliver_IsFactionIconVisible_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelDeliver), "FactionIconBackgroundColor", typeof (MyContractModelDeliver_FactionIconBackgroundColor_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelDeliver), "FactionIconTooltip", typeof (MyContractModelDeliver_FactionIconTooltip_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelDeliver), "FactionIcon", typeof (MyContractModelDeliver_FactionIcon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelDeliver), "FactionIconColor", typeof (MyContractModelDeliver_FactionIconColor_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelEscort), "Icon", typeof (MyContractModelEscort_Icon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelEscort), "Name", typeof (MyContractModelEscort_Name_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelEscort), "RewardMoney_Formatted", typeof (MyContractModelEscort_RewardMoney_Formatted_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelEscort), "CurrencyIcon", typeof (MyContractModelEscort_CurrencyIcon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelEscort), "RewardReputation_Formatted", typeof (MyContractModelEscort_RewardReputation_Formatted_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelEscort), "FailReputationPenalty_Formated", typeof (MyContractModelEscort_FailReputationPenalty_Formated_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelEscort), "IsFactionIconVisible", typeof (MyContractModelEscort_IsFactionIconVisible_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelEscort), "FactionIconBackgroundColor", typeof (MyContractModelEscort_FactionIconBackgroundColor_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelEscort), "FactionIconTooltip", typeof (MyContractModelEscort_FactionIconTooltip_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelEscort), "FactionIcon", typeof (MyContractModelEscort_FactionIcon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelEscort), "FactionIconColor", typeof (MyContractModelEscort_FactionIconColor_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelFind), "Icon", typeof (MyContractModelFind_Icon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelFind), "Name", typeof (MyContractModelFind_Name_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelFind), "RewardMoney_Formatted", typeof (MyContractModelFind_RewardMoney_Formatted_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelFind), "CurrencyIcon", typeof (MyContractModelFind_CurrencyIcon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelFind), "RewardReputation_Formatted", typeof (MyContractModelFind_RewardReputation_Formatted_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelFind), "FailReputationPenalty_Formated", typeof (MyContractModelFind_FailReputationPenalty_Formated_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelFind), "IsFactionIconVisible", typeof (MyContractModelFind_IsFactionIconVisible_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelFind), "FactionIconBackgroundColor", typeof (MyContractModelFind_FactionIconBackgroundColor_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelFind), "FactionIconTooltip", typeof (MyContractModelFind_FactionIconTooltip_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelFind), "FactionIcon", typeof (MyContractModelFind_FactionIcon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelFind), "FactionIconColor", typeof (MyContractModelFind_FactionIconColor_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelHunt), "Icon", typeof (MyContractModelHunt_Icon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelHunt), "Name", typeof (MyContractModelHunt_Name_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelHunt), "RewardMoney_Formatted", typeof (MyContractModelHunt_RewardMoney_Formatted_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelHunt), "CurrencyIcon", typeof (MyContractModelHunt_CurrencyIcon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelHunt), "RewardReputation_Formatted", typeof (MyContractModelHunt_RewardReputation_Formatted_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelHunt), "IsFactionIconVisible", typeof (MyContractModelHunt_IsFactionIconVisible_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelHunt), "FactionIconBackgroundColor", typeof (MyContractModelHunt_FactionIconBackgroundColor_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelHunt), "FactionIconTooltip", typeof (MyContractModelHunt_FactionIconTooltip_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelHunt), "FactionIcon", typeof (MyContractModelHunt_FactionIcon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelHunt), "FactionIconColor", typeof (MyContractModelHunt_FactionIconColor_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelObtainAndDeliver), "Icon", typeof (MyContractModelObtainAndDeliver_Icon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelObtainAndDeliver), "Name", typeof (MyContractModelObtainAndDeliver_Name_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelObtainAndDeliver), "RewardMoney_Formatted", typeof (MyContractModelObtainAndDeliver_RewardMoney_Formatted_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelObtainAndDeliver), "CurrencyIcon", typeof (MyContractModelObtainAndDeliver_CurrencyIcon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelObtainAndDeliver), "RewardReputation_Formatted", typeof (MyContractModelObtainAndDeliver_RewardReputation_Formatted_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelObtainAndDeliver), "IsFactionIconVisible", typeof (MyContractModelObtainAndDeliver_IsFactionIconVisible_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelObtainAndDeliver), "FactionIconBackgroundColor", typeof (MyContractModelObtainAndDeliver_FactionIconBackgroundColor_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelObtainAndDeliver), "FactionIconTooltip", typeof (MyContractModelObtainAndDeliver_FactionIconTooltip_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelObtainAndDeliver), "FactionIcon", typeof (MyContractModelObtainAndDeliver_FactionIcon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelObtainAndDeliver), "FactionIconColor", typeof (MyContractModelObtainAndDeliver_FactionIconColor_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelRepair), "Icon", typeof (MyContractModelRepair_Icon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelRepair), "Name", typeof (MyContractModelRepair_Name_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelRepair), "RewardMoney_Formatted", typeof (MyContractModelRepair_RewardMoney_Formatted_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelRepair), "CurrencyIcon", typeof (MyContractModelRepair_CurrencyIcon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelRepair), "RewardReputation_Formatted", typeof (MyContractModelRepair_RewardReputation_Formatted_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelRepair), "FailReputationPenalty_Formated", typeof (MyContractModelRepair_FailReputationPenalty_Formated_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelRepair), "IsFactionIconVisible", typeof (MyContractModelRepair_IsFactionIconVisible_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelRepair), "FactionIconBackgroundColor", typeof (MyContractModelRepair_FactionIconBackgroundColor_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelRepair), "FactionIconTooltip", typeof (MyContractModelRepair_FactionIconTooltip_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelRepair), "FactionIcon", typeof (MyContractModelRepair_FactionIcon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelRepair), "FactionIconColor", typeof (MyContractModelRepair_FactionIconColor_PropertyInfo));
    }

    private static UIElement r_0_dtMethod(UIElement parent)
    {
      Grid grid = new Grid();
      grid.Parent = parent;
      grid.Name = "e_0";
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
      image1.Name = "e_1";
      image1.Width = 64f;
      image1.Margin = new Thickness(2f, 0.0f, 2f, 0.0f);
      image1.Stretch = Stretch.Uniform;
      Grid.SetRowSpan((UIElement) image1, 2);
      ImageBrush.SetColorOverlay((DependencyObject) image1, new ColorW(131, 201, 226, (int) byte.MaxValue));
      image1.SetBinding(Image.SourceProperty, new Binding("Icon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock1 = new TextBlock();
      grid.Children.Add((UIElement) textBlock1);
      textBlock1.Name = "e_2";
      textBlock1.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock1.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock1, 1);
      textBlock1.SetBinding(TextBlock.TextProperty, new Binding("Name")
      {
        UseGeneratedBindings = true
      });
      StackPanel stackPanel = new StackPanel();
      grid.Children.Add((UIElement) stackPanel);
      stackPanel.Name = "e_3";
      stackPanel.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel, 1);
      Grid.SetRow((UIElement) stackPanel, 1);
      TextBlock textBlock2 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock2);
      textBlock2.Name = "e_4";
      textBlock2.Margin = new Thickness(4f, 1f, 1f, 1f);
      textBlock2.VerticalAlignment = VerticalAlignment.Center;
      textBlock2.FontFamily = new FontFamily("InventorySmall");
      textBlock2.FontSize = 10f;
      textBlock2.SetBinding(TextBlock.TextProperty, new Binding("RewardMoney_Formatted")
      {
        UseGeneratedBindings = true
      });
      Image image2 = new Image();
      stackPanel.Children.Add((UIElement) image2);
      image2.Name = "e_5";
      image2.Height = 14f;
      image2.Margin = new Thickness(1f, 1f, 1f, 1f);
      image2.HorizontalAlignment = HorizontalAlignment.Right;
      image2.VerticalAlignment = VerticalAlignment.Center;
      image2.Stretch = Stretch.Uniform;
      image2.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock3 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock3);
      textBlock3.Name = "e_6";
      textBlock3.Margin = new Thickness(4f, 1f, 4f, 1f);
      textBlock3.VerticalAlignment = VerticalAlignment.Center;
      textBlock3.Text = "|";
      textBlock3.FontFamily = new FontFamily("InventorySmall");
      textBlock3.FontSize = 10f;
      TextBlock textBlock4 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock4);
      textBlock4.Name = "e_7";
      textBlock4.Margin = new Thickness(1f, 1f, 1f, 1f);
      textBlock4.VerticalAlignment = VerticalAlignment.Center;
      textBlock4.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      textBlock4.FontFamily = new FontFamily("InventorySmall");
      textBlock4.FontSize = 10f;
      textBlock4.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_RepChange_Hint");
      TextBlock textBlock5 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock5);
      textBlock5.Name = "e_8";
      textBlock5.Margin = new Thickness(1f, 1f, 1f, 1f);
      textBlock5.VerticalAlignment = VerticalAlignment.Center;
      textBlock5.FontFamily = new FontFamily("InventorySmall");
      textBlock5.FontSize = 10f;
      textBlock5.SetBinding(TextBlock.TextProperty, new Binding("RewardReputation_Formatted")
      {
        UseGeneratedBindings = true
      });
      Image image3 = new Image();
      stackPanel.Children.Add((UIElement) image3);
      image3.Name = "e_9";
      image3.Width = 16f;
      image3.VerticalAlignment = VerticalAlignment.Center;
      image3.Source = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Contracts\\ArrowRepGain.png"
      };
      image3.Stretch = Stretch.Uniform;
      TextBlock textBlock6 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock6);
      textBlock6.Name = "e_10";
      textBlock6.Margin = new Thickness(1f, 1f, 1f, 1f);
      textBlock6.VerticalAlignment = VerticalAlignment.Center;
      textBlock6.FontFamily = new FontFamily("InventorySmall");
      textBlock6.FontSize = 10f;
      textBlock6.SetBinding(TextBlock.TextProperty, new Binding("FailReputationPenalty_Formated")
      {
        UseGeneratedBindings = true
      });
      Image image4 = new Image();
      stackPanel.Children.Add((UIElement) image4);
      image4.Name = "e_11";
      image4.Width = 16f;
      image4.VerticalAlignment = VerticalAlignment.Center;
      image4.Source = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Contracts\\ArrowRepLoss.png"
      };
      image4.Stretch = Stretch.Uniform;
      Border border = new Border();
      grid.Children.Add((UIElement) border);
      border.Name = "e_12";
      border.Margin = new Thickness(2f, 2f, 2f, 2f);
      border.HorizontalAlignment = HorizontalAlignment.Left;
      border.VerticalAlignment = VerticalAlignment.Top;
      border.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      border.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      Grid.SetColumn((UIElement) border, 2);
      Grid.SetRowSpan((UIElement) border, 2);
      border.SetBinding(UIElement.VisibilityProperty, new Binding("IsFactionIconVisible")
      {
        UseGeneratedBindings = true
      });
      border.SetBinding(Control.BackgroundProperty, new Binding("FactionIconBackgroundColor")
      {
        UseGeneratedBindings = true
      });
      Image image5 = new Image();
      border.Child = (UIElement) image5;
      image5.Name = "FactionIcon";
      image5.Height = 32f;
      image5.Margin = new Thickness(0.0f, 0.0f, 0.0f, 0.0f);
      image5.Stretch = Stretch.Uniform;
      image5.SetBinding(UIElement.ToolTipProperty, new Binding("FactionIconTooltip")
      {
        UseGeneratedBindings = true
      });
      image5.SetBinding(Image.SourceProperty, new Binding("FactionIcon")
      {
        UseGeneratedBindings = true
      });
      image5.SetBinding(ImageBrush.ColorOverlayProperty, new Binding("FactionIconColor")
      {
        UseGeneratedBindings = true
      });
      return (UIElement) grid;
    }

    private static UIElement r_1_dtMethod(UIElement parent)
    {
      Grid grid = new Grid();
      grid.Parent = parent;
      grid.Name = "e_13";
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
      image1.Name = "e_14";
      image1.Width = 64f;
      image1.Margin = new Thickness(2f, 0.0f, 2f, 0.0f);
      image1.Stretch = Stretch.Uniform;
      Grid.SetRowSpan((UIElement) image1, 2);
      ImageBrush.SetColorOverlay((DependencyObject) image1, new ColorW(131, 201, 226, (int) byte.MaxValue));
      image1.SetBinding(Image.SourceProperty, new Binding("Icon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock1 = new TextBlock();
      grid.Children.Add((UIElement) textBlock1);
      textBlock1.Name = "e_15";
      textBlock1.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock1.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock1, 1);
      textBlock1.SetBinding(TextBlock.TextProperty, new Binding("Name")
      {
        UseGeneratedBindings = true
      });
      StackPanel stackPanel = new StackPanel();
      grid.Children.Add((UIElement) stackPanel);
      stackPanel.Name = "e_16";
      stackPanel.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel, 1);
      Grid.SetRow((UIElement) stackPanel, 1);
      TextBlock textBlock2 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock2);
      textBlock2.Name = "e_17";
      textBlock2.Margin = new Thickness(4f, 1f, 1f, 1f);
      textBlock2.VerticalAlignment = VerticalAlignment.Center;
      textBlock2.FontFamily = new FontFamily("InventorySmall");
      textBlock2.FontSize = 10f;
      textBlock2.SetBinding(TextBlock.TextProperty, new Binding("RewardMoney_Formatted")
      {
        UseGeneratedBindings = true
      });
      Image image2 = new Image();
      stackPanel.Children.Add((UIElement) image2);
      image2.Name = "e_18";
      image2.Height = 14f;
      image2.Margin = new Thickness(1f, 1f, 1f, 1f);
      image2.HorizontalAlignment = HorizontalAlignment.Right;
      image2.VerticalAlignment = VerticalAlignment.Center;
      image2.Stretch = Stretch.Uniform;
      image2.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock3 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock3);
      textBlock3.Name = "e_19";
      textBlock3.Margin = new Thickness(4f, 1f, 4f, 1f);
      textBlock3.VerticalAlignment = VerticalAlignment.Center;
      textBlock3.Text = "|";
      textBlock3.FontFamily = new FontFamily("InventorySmall");
      textBlock3.FontSize = 10f;
      TextBlock textBlock4 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock4);
      textBlock4.Name = "e_20";
      textBlock4.Margin = new Thickness(1f, 1f, 1f, 1f);
      textBlock4.VerticalAlignment = VerticalAlignment.Center;
      textBlock4.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      textBlock4.FontFamily = new FontFamily("InventorySmall");
      textBlock4.FontSize = 10f;
      textBlock4.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_RepChange_Hint");
      TextBlock textBlock5 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock5);
      textBlock5.Name = "e_21";
      textBlock5.Margin = new Thickness(1f, 1f, 1f, 1f);
      textBlock5.VerticalAlignment = VerticalAlignment.Center;
      textBlock5.FontFamily = new FontFamily("InventorySmall");
      textBlock5.FontSize = 10f;
      textBlock5.SetBinding(TextBlock.TextProperty, new Binding("RewardReputation_Formatted")
      {
        UseGeneratedBindings = true
      });
      Image image3 = new Image();
      stackPanel.Children.Add((UIElement) image3);
      image3.Name = "e_22";
      image3.Width = 16f;
      image3.VerticalAlignment = VerticalAlignment.Center;
      image3.Source = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Contracts\\ArrowRepGain.png"
      };
      image3.Stretch = Stretch.Uniform;
      TextBlock textBlock6 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock6);
      textBlock6.Name = "e_23";
      textBlock6.Margin = new Thickness(1f, 1f, 1f, 1f);
      textBlock6.VerticalAlignment = VerticalAlignment.Center;
      textBlock6.FontFamily = new FontFamily("InventorySmall");
      textBlock6.FontSize = 10f;
      textBlock6.SetBinding(TextBlock.TextProperty, new Binding("FailReputationPenalty_Formated")
      {
        UseGeneratedBindings = true
      });
      Image image4 = new Image();
      stackPanel.Children.Add((UIElement) image4);
      image4.Name = "e_24";
      image4.Width = 16f;
      image4.VerticalAlignment = VerticalAlignment.Center;
      image4.Source = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Contracts\\ArrowRepLoss.png"
      };
      image4.Stretch = Stretch.Uniform;
      Border border = new Border();
      grid.Children.Add((UIElement) border);
      border.Name = "e_25";
      border.Margin = new Thickness(2f, 2f, 2f, 2f);
      border.HorizontalAlignment = HorizontalAlignment.Left;
      border.VerticalAlignment = VerticalAlignment.Top;
      border.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      border.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      Grid.SetColumn((UIElement) border, 2);
      Grid.SetRowSpan((UIElement) border, 2);
      border.SetBinding(UIElement.VisibilityProperty, new Binding("IsFactionIconVisible")
      {
        UseGeneratedBindings = true
      });
      border.SetBinding(Control.BackgroundProperty, new Binding("FactionIconBackgroundColor")
      {
        UseGeneratedBindings = true
      });
      Image image5 = new Image();
      border.Child = (UIElement) image5;
      image5.Name = "FactionIcon";
      image5.Height = 32f;
      image5.Margin = new Thickness(0.0f, 0.0f, 0.0f, 0.0f);
      image5.Stretch = Stretch.Uniform;
      image5.SetBinding(UIElement.ToolTipProperty, new Binding("FactionIconTooltip")
      {
        UseGeneratedBindings = true
      });
      image5.SetBinding(Image.SourceProperty, new Binding("FactionIcon")
      {
        UseGeneratedBindings = true
      });
      image5.SetBinding(ImageBrush.ColorOverlayProperty, new Binding("FactionIconColor")
      {
        UseGeneratedBindings = true
      });
      return (UIElement) grid;
    }

    private static UIElement r_2_dtMethod(UIElement parent)
    {
      Grid grid = new Grid();
      grid.Parent = parent;
      grid.Name = "e_26";
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
      image1.Name = "e_27";
      image1.Width = 64f;
      image1.Margin = new Thickness(2f, 0.0f, 2f, 0.0f);
      image1.Stretch = Stretch.Uniform;
      Grid.SetRowSpan((UIElement) image1, 2);
      ImageBrush.SetColorOverlay((DependencyObject) image1, new ColorW(131, 201, 226, (int) byte.MaxValue));
      image1.SetBinding(Image.SourceProperty, new Binding("Icon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock1 = new TextBlock();
      grid.Children.Add((UIElement) textBlock1);
      textBlock1.Name = "e_28";
      textBlock1.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock1.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock1, 1);
      textBlock1.SetBinding(TextBlock.TextProperty, new Binding("Name")
      {
        UseGeneratedBindings = true
      });
      StackPanel stackPanel = new StackPanel();
      grid.Children.Add((UIElement) stackPanel);
      stackPanel.Name = "e_29";
      stackPanel.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel, 1);
      Grid.SetRow((UIElement) stackPanel, 1);
      TextBlock textBlock2 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock2);
      textBlock2.Name = "e_30";
      textBlock2.Margin = new Thickness(4f, 1f, 1f, 1f);
      textBlock2.VerticalAlignment = VerticalAlignment.Center;
      textBlock2.FontFamily = new FontFamily("InventorySmall");
      textBlock2.FontSize = 10f;
      textBlock2.SetBinding(TextBlock.TextProperty, new Binding("RewardMoney_Formatted")
      {
        UseGeneratedBindings = true
      });
      Image image2 = new Image();
      stackPanel.Children.Add((UIElement) image2);
      image2.Name = "e_31";
      image2.Height = 14f;
      image2.Margin = new Thickness(1f, 1f, 1f, 1f);
      image2.HorizontalAlignment = HorizontalAlignment.Right;
      image2.VerticalAlignment = VerticalAlignment.Center;
      image2.Stretch = Stretch.Uniform;
      image2.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock3 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock3);
      textBlock3.Name = "e_32";
      textBlock3.Margin = new Thickness(4f, 1f, 4f, 1f);
      textBlock3.VerticalAlignment = VerticalAlignment.Center;
      textBlock3.Text = "|";
      textBlock3.FontFamily = new FontFamily("InventorySmall");
      textBlock3.FontSize = 10f;
      TextBlock textBlock4 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock4);
      textBlock4.Name = "e_33";
      textBlock4.Margin = new Thickness(1f, 1f, 1f, 1f);
      textBlock4.VerticalAlignment = VerticalAlignment.Center;
      textBlock4.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      textBlock4.FontFamily = new FontFamily("InventorySmall");
      textBlock4.FontSize = 10f;
      textBlock4.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_RepChange_Hint");
      TextBlock textBlock5 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock5);
      textBlock5.Name = "e_34";
      textBlock5.Margin = new Thickness(1f, 1f, 1f, 1f);
      textBlock5.VerticalAlignment = VerticalAlignment.Center;
      textBlock5.FontFamily = new FontFamily("InventorySmall");
      textBlock5.FontSize = 10f;
      textBlock5.SetBinding(TextBlock.TextProperty, new Binding("RewardReputation_Formatted")
      {
        UseGeneratedBindings = true
      });
      Image image3 = new Image();
      stackPanel.Children.Add((UIElement) image3);
      image3.Name = "e_35";
      image3.Width = 16f;
      image3.VerticalAlignment = VerticalAlignment.Center;
      image3.Source = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Contracts\\ArrowRepGain.png"
      };
      image3.Stretch = Stretch.Uniform;
      TextBlock textBlock6 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock6);
      textBlock6.Name = "e_36";
      textBlock6.Margin = new Thickness(1f, 1f, 1f, 1f);
      textBlock6.VerticalAlignment = VerticalAlignment.Center;
      textBlock6.FontFamily = new FontFamily("InventorySmall");
      textBlock6.FontSize = 10f;
      textBlock6.SetBinding(TextBlock.TextProperty, new Binding("FailReputationPenalty_Formated")
      {
        UseGeneratedBindings = true
      });
      Image image4 = new Image();
      stackPanel.Children.Add((UIElement) image4);
      image4.Name = "e_37";
      image4.Width = 16f;
      image4.VerticalAlignment = VerticalAlignment.Center;
      image4.Source = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Contracts\\ArrowRepLoss.png"
      };
      image4.Stretch = Stretch.Uniform;
      Border border = new Border();
      grid.Children.Add((UIElement) border);
      border.Name = "e_38";
      border.Margin = new Thickness(2f, 2f, 2f, 2f);
      border.HorizontalAlignment = HorizontalAlignment.Left;
      border.VerticalAlignment = VerticalAlignment.Top;
      border.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      border.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      Grid.SetColumn((UIElement) border, 2);
      Grid.SetRowSpan((UIElement) border, 2);
      border.SetBinding(UIElement.VisibilityProperty, new Binding("IsFactionIconVisible")
      {
        UseGeneratedBindings = true
      });
      border.SetBinding(Control.BackgroundProperty, new Binding("FactionIconBackgroundColor")
      {
        UseGeneratedBindings = true
      });
      Image image5 = new Image();
      border.Child = (UIElement) image5;
      image5.Name = "FactionIcon";
      image5.Height = 32f;
      image5.Margin = new Thickness(0.0f, 0.0f, 0.0f, 0.0f);
      image5.Stretch = Stretch.Uniform;
      image5.SetBinding(UIElement.ToolTipProperty, new Binding("FactionIconTooltip")
      {
        UseGeneratedBindings = true
      });
      image5.SetBinding(Image.SourceProperty, new Binding("FactionIcon")
      {
        UseGeneratedBindings = true
      });
      image5.SetBinding(ImageBrush.ColorOverlayProperty, new Binding("FactionIconColor")
      {
        UseGeneratedBindings = true
      });
      return (UIElement) grid;
    }

    private static UIElement r_3_dtMethod(UIElement parent)
    {
      Grid grid = new Grid();
      grid.Parent = parent;
      grid.Name = "e_39";
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
      image1.Name = "e_40";
      image1.Width = 64f;
      image1.Margin = new Thickness(2f, 0.0f, 2f, 0.0f);
      image1.Stretch = Stretch.Uniform;
      Grid.SetRowSpan((UIElement) image1, 2);
      ImageBrush.SetColorOverlay((DependencyObject) image1, new ColorW(131, 201, 226, (int) byte.MaxValue));
      image1.SetBinding(Image.SourceProperty, new Binding("Icon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock1 = new TextBlock();
      grid.Children.Add((UIElement) textBlock1);
      textBlock1.Name = "e_41";
      textBlock1.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock1.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock1, 1);
      textBlock1.SetBinding(TextBlock.TextProperty, new Binding("Name")
      {
        UseGeneratedBindings = true
      });
      StackPanel stackPanel = new StackPanel();
      grid.Children.Add((UIElement) stackPanel);
      stackPanel.Name = "e_42";
      stackPanel.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel, 1);
      Grid.SetRow((UIElement) stackPanel, 1);
      TextBlock textBlock2 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock2);
      textBlock2.Name = "e_43";
      textBlock2.Margin = new Thickness(4f, 1f, 1f, 1f);
      textBlock2.VerticalAlignment = VerticalAlignment.Center;
      textBlock2.FontFamily = new FontFamily("InventorySmall");
      textBlock2.FontSize = 10f;
      textBlock2.SetBinding(TextBlock.TextProperty, new Binding("RewardMoney_Formatted")
      {
        UseGeneratedBindings = true
      });
      Image image2 = new Image();
      stackPanel.Children.Add((UIElement) image2);
      image2.Name = "e_44";
      image2.Height = 14f;
      image2.Margin = new Thickness(1f, 1f, 1f, 1f);
      image2.HorizontalAlignment = HorizontalAlignment.Right;
      image2.VerticalAlignment = VerticalAlignment.Center;
      image2.Stretch = Stretch.Uniform;
      image2.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock3 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock3);
      textBlock3.Name = "e_45";
      textBlock3.Margin = new Thickness(4f, 1f, 4f, 1f);
      textBlock3.VerticalAlignment = VerticalAlignment.Center;
      textBlock3.Text = "|";
      textBlock3.FontFamily = new FontFamily("InventorySmall");
      textBlock3.FontSize = 10f;
      TextBlock textBlock4 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock4);
      textBlock4.Name = "e_46";
      textBlock4.Margin = new Thickness(1f, 1f, 1f, 1f);
      textBlock4.VerticalAlignment = VerticalAlignment.Center;
      textBlock4.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      textBlock4.FontFamily = new FontFamily("InventorySmall");
      textBlock4.FontSize = 10f;
      textBlock4.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_RepChange_Hint");
      TextBlock textBlock5 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock5);
      textBlock5.Name = "e_47";
      textBlock5.Margin = new Thickness(1f, 1f, 1f, 1f);
      textBlock5.VerticalAlignment = VerticalAlignment.Center;
      textBlock5.FontFamily = new FontFamily("InventorySmall");
      textBlock5.FontSize = 10f;
      textBlock5.SetBinding(TextBlock.TextProperty, new Binding("RewardReputation_Formatted")
      {
        UseGeneratedBindings = true
      });
      Image image3 = new Image();
      stackPanel.Children.Add((UIElement) image3);
      image3.Name = "e_48";
      image3.Width = 16f;
      image3.VerticalAlignment = VerticalAlignment.Center;
      image3.Source = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Contracts\\ArrowRepGain.png"
      };
      image3.Stretch = Stretch.Uniform;
      TextBlock textBlock6 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock6);
      textBlock6.Name = "e_49";
      textBlock6.Margin = new Thickness(1f, 1f, 1f, 1f);
      textBlock6.VerticalAlignment = VerticalAlignment.Center;
      textBlock6.FontFamily = new FontFamily("InventorySmall");
      textBlock6.FontSize = 10f;
      textBlock6.SetBinding(TextBlock.TextProperty, new Binding("FailReputationPenalty_Formated")
      {
        UseGeneratedBindings = true
      });
      Image image4 = new Image();
      stackPanel.Children.Add((UIElement) image4);
      image4.Name = "e_50";
      image4.Width = 16f;
      image4.VerticalAlignment = VerticalAlignment.Center;
      image4.Source = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Contracts\\ArrowRepLoss.png"
      };
      image4.Stretch = Stretch.Uniform;
      Border border = new Border();
      grid.Children.Add((UIElement) border);
      border.Name = "e_51";
      border.Margin = new Thickness(2f, 2f, 2f, 2f);
      border.HorizontalAlignment = HorizontalAlignment.Left;
      border.VerticalAlignment = VerticalAlignment.Top;
      border.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      border.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      Grid.SetColumn((UIElement) border, 2);
      Grid.SetRowSpan((UIElement) border, 2);
      border.SetBinding(UIElement.VisibilityProperty, new Binding("IsFactionIconVisible")
      {
        UseGeneratedBindings = true
      });
      border.SetBinding(Control.BackgroundProperty, new Binding("FactionIconBackgroundColor")
      {
        UseGeneratedBindings = true
      });
      Image image5 = new Image();
      border.Child = (UIElement) image5;
      image5.Name = "FactionIcon";
      image5.Height = 32f;
      image5.Margin = new Thickness(0.0f, 0.0f, 0.0f, 0.0f);
      image5.Stretch = Stretch.Uniform;
      image5.SetBinding(UIElement.ToolTipProperty, new Binding("FactionIconTooltip")
      {
        UseGeneratedBindings = true
      });
      image5.SetBinding(Image.SourceProperty, new Binding("FactionIcon")
      {
        UseGeneratedBindings = true
      });
      image5.SetBinding(ImageBrush.ColorOverlayProperty, new Binding("FactionIconColor")
      {
        UseGeneratedBindings = true
      });
      return (UIElement) grid;
    }

    private static UIElement r_4_dtMethod(UIElement parent)
    {
      Grid grid = new Grid();
      grid.Parent = parent;
      grid.Name = "e_52";
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
      image1.Name = "e_53";
      image1.Width = 64f;
      image1.Margin = new Thickness(2f, 0.0f, 2f, 0.0f);
      image1.Stretch = Stretch.Uniform;
      Grid.SetRowSpan((UIElement) image1, 2);
      ImageBrush.SetColorOverlay((DependencyObject) image1, new ColorW(131, 201, 226, (int) byte.MaxValue));
      image1.SetBinding(Image.SourceProperty, new Binding("Icon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock1 = new TextBlock();
      grid.Children.Add((UIElement) textBlock1);
      textBlock1.Name = "e_54";
      textBlock1.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock1.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock1, 1);
      textBlock1.SetBinding(TextBlock.TextProperty, new Binding("Name")
      {
        UseGeneratedBindings = true
      });
      StackPanel stackPanel = new StackPanel();
      grid.Children.Add((UIElement) stackPanel);
      stackPanel.Name = "e_55";
      stackPanel.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel, 1);
      Grid.SetRow((UIElement) stackPanel, 1);
      TextBlock textBlock2 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock2);
      textBlock2.Name = "e_56";
      textBlock2.Margin = new Thickness(4f, 1f, 1f, 1f);
      textBlock2.VerticalAlignment = VerticalAlignment.Center;
      textBlock2.FontFamily = new FontFamily("InventorySmall");
      textBlock2.FontSize = 10f;
      textBlock2.SetBinding(TextBlock.TextProperty, new Binding("RewardMoney_Formatted")
      {
        UseGeneratedBindings = true
      });
      Image image2 = new Image();
      stackPanel.Children.Add((UIElement) image2);
      image2.Name = "e_57";
      image2.Height = 14f;
      image2.Margin = new Thickness(1f, 1f, 1f, 1f);
      image2.HorizontalAlignment = HorizontalAlignment.Right;
      image2.VerticalAlignment = VerticalAlignment.Center;
      image2.Stretch = Stretch.Uniform;
      image2.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock3 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock3);
      textBlock3.Name = "e_58";
      textBlock3.Margin = new Thickness(4f, 1f, 4f, 1f);
      textBlock3.VerticalAlignment = VerticalAlignment.Center;
      textBlock3.Text = "|";
      textBlock3.FontFamily = new FontFamily("InventorySmall");
      textBlock3.FontSize = 10f;
      TextBlock textBlock4 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock4);
      textBlock4.Name = "e_59";
      textBlock4.Margin = new Thickness(1f, 1f, 1f, 1f);
      textBlock4.VerticalAlignment = VerticalAlignment.Center;
      textBlock4.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      textBlock4.FontFamily = new FontFamily("InventorySmall");
      textBlock4.FontSize = 10f;
      textBlock4.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_RepChange_Hint");
      TextBlock textBlock5 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock5);
      textBlock5.Name = "e_60";
      textBlock5.Margin = new Thickness(1f, 1f, 1f, 1f);
      textBlock5.VerticalAlignment = VerticalAlignment.Center;
      textBlock5.FontFamily = new FontFamily("InventorySmall");
      textBlock5.FontSize = 10f;
      textBlock5.SetBinding(TextBlock.TextProperty, new Binding("RewardReputation_Formatted")
      {
        UseGeneratedBindings = true
      });
      Image image3 = new Image();
      stackPanel.Children.Add((UIElement) image3);
      image3.Name = "e_61";
      image3.Width = 16f;
      image3.VerticalAlignment = VerticalAlignment.Center;
      image3.Source = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Contracts\\ArrowRepGain.png"
      };
      image3.Stretch = Stretch.Uniform;
      Border border = new Border();
      grid.Children.Add((UIElement) border);
      border.Name = "e_62";
      border.Margin = new Thickness(2f, 2f, 2f, 2f);
      border.HorizontalAlignment = HorizontalAlignment.Left;
      border.VerticalAlignment = VerticalAlignment.Top;
      border.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      border.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      Grid.SetColumn((UIElement) border, 2);
      Grid.SetRowSpan((UIElement) border, 2);
      border.SetBinding(UIElement.VisibilityProperty, new Binding("IsFactionIconVisible")
      {
        UseGeneratedBindings = true
      });
      border.SetBinding(Control.BackgroundProperty, new Binding("FactionIconBackgroundColor")
      {
        UseGeneratedBindings = true
      });
      Image image4 = new Image();
      border.Child = (UIElement) image4;
      image4.Name = "FactionIcon";
      image4.Height = 32f;
      image4.Margin = new Thickness(0.0f, 0.0f, 0.0f, 0.0f);
      image4.Stretch = Stretch.Uniform;
      image4.SetBinding(UIElement.ToolTipProperty, new Binding("FactionIconTooltip")
      {
        UseGeneratedBindings = true
      });
      image4.SetBinding(Image.SourceProperty, new Binding("FactionIcon")
      {
        UseGeneratedBindings = true
      });
      image4.SetBinding(ImageBrush.ColorOverlayProperty, new Binding("FactionIconColor")
      {
        UseGeneratedBindings = true
      });
      return (UIElement) grid;
    }

    private static UIElement r_5_dtMethod(UIElement parent)
    {
      Grid grid = new Grid();
      grid.Parent = parent;
      grid.Name = "e_63";
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
      image1.Name = "e_64";
      image1.Width = 64f;
      image1.Margin = new Thickness(2f, 0.0f, 2f, 0.0f);
      image1.Stretch = Stretch.Uniform;
      Grid.SetRowSpan((UIElement) image1, 2);
      ImageBrush.SetColorOverlay((DependencyObject) image1, new ColorW(131, 201, 226, (int) byte.MaxValue));
      image1.SetBinding(Image.SourceProperty, new Binding("Icon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock1 = new TextBlock();
      grid.Children.Add((UIElement) textBlock1);
      textBlock1.Name = "e_65";
      textBlock1.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock1.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock1, 1);
      textBlock1.SetBinding(TextBlock.TextProperty, new Binding("Name")
      {
        UseGeneratedBindings = true
      });
      StackPanel stackPanel = new StackPanel();
      grid.Children.Add((UIElement) stackPanel);
      stackPanel.Name = "e_66";
      stackPanel.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel, 1);
      Grid.SetRow((UIElement) stackPanel, 1);
      TextBlock textBlock2 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock2);
      textBlock2.Name = "e_67";
      textBlock2.Margin = new Thickness(4f, 1f, 1f, 1f);
      textBlock2.VerticalAlignment = VerticalAlignment.Center;
      textBlock2.FontFamily = new FontFamily("InventorySmall");
      textBlock2.FontSize = 10f;
      textBlock2.SetBinding(TextBlock.TextProperty, new Binding("RewardMoney_Formatted")
      {
        UseGeneratedBindings = true
      });
      Image image2 = new Image();
      stackPanel.Children.Add((UIElement) image2);
      image2.Name = "e_68";
      image2.Height = 14f;
      image2.Margin = new Thickness(1f, 1f, 1f, 1f);
      image2.HorizontalAlignment = HorizontalAlignment.Right;
      image2.VerticalAlignment = VerticalAlignment.Center;
      image2.Stretch = Stretch.Uniform;
      image2.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock3 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock3);
      textBlock3.Name = "e_69";
      textBlock3.Margin = new Thickness(4f, 1f, 4f, 1f);
      textBlock3.VerticalAlignment = VerticalAlignment.Center;
      textBlock3.Text = "|";
      textBlock3.FontFamily = new FontFamily("InventorySmall");
      textBlock3.FontSize = 10f;
      TextBlock textBlock4 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock4);
      textBlock4.Name = "e_70";
      textBlock4.Margin = new Thickness(1f, 1f, 1f, 1f);
      textBlock4.VerticalAlignment = VerticalAlignment.Center;
      textBlock4.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      textBlock4.FontFamily = new FontFamily("InventorySmall");
      textBlock4.FontSize = 10f;
      textBlock4.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_RepChange_Hint");
      TextBlock textBlock5 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock5);
      textBlock5.Name = "e_71";
      textBlock5.Margin = new Thickness(1f, 1f, 1f, 1f);
      textBlock5.VerticalAlignment = VerticalAlignment.Center;
      textBlock5.FontFamily = new FontFamily("InventorySmall");
      textBlock5.FontSize = 10f;
      textBlock5.SetBinding(TextBlock.TextProperty, new Binding("RewardReputation_Formatted")
      {
        UseGeneratedBindings = true
      });
      Image image3 = new Image();
      stackPanel.Children.Add((UIElement) image3);
      image3.Name = "e_72";
      image3.Width = 16f;
      image3.VerticalAlignment = VerticalAlignment.Center;
      image3.Source = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Contracts\\ArrowRepGain.png"
      };
      image3.Stretch = Stretch.Uniform;
      Border border = new Border();
      grid.Children.Add((UIElement) border);
      border.Name = "e_73";
      border.Margin = new Thickness(2f, 2f, 2f, 2f);
      border.HorizontalAlignment = HorizontalAlignment.Left;
      border.VerticalAlignment = VerticalAlignment.Top;
      border.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      border.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      Grid.SetColumn((UIElement) border, 2);
      Grid.SetRowSpan((UIElement) border, 2);
      border.SetBinding(UIElement.VisibilityProperty, new Binding("IsFactionIconVisible")
      {
        UseGeneratedBindings = true
      });
      border.SetBinding(Control.BackgroundProperty, new Binding("FactionIconBackgroundColor")
      {
        UseGeneratedBindings = true
      });
      Image image4 = new Image();
      border.Child = (UIElement) image4;
      image4.Name = "FactionIcon";
      image4.Height = 32f;
      image4.Margin = new Thickness(0.0f, 0.0f, 0.0f, 0.0f);
      image4.Stretch = Stretch.Uniform;
      image4.SetBinding(UIElement.ToolTipProperty, new Binding("FactionIconTooltip")
      {
        UseGeneratedBindings = true
      });
      image4.SetBinding(Image.SourceProperty, new Binding("FactionIcon")
      {
        UseGeneratedBindings = true
      });
      image4.SetBinding(ImageBrush.ColorOverlayProperty, new Binding("FactionIconColor")
      {
        UseGeneratedBindings = true
      });
      return (UIElement) grid;
    }

    private static UIElement r_6_dtMethod(UIElement parent)
    {
      Grid grid = new Grid();
      grid.Parent = parent;
      grid.Name = "e_74";
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
      image1.Name = "e_75";
      image1.Width = 64f;
      image1.Margin = new Thickness(2f, 0.0f, 2f, 0.0f);
      image1.Stretch = Stretch.Uniform;
      Grid.SetRowSpan((UIElement) image1, 2);
      ImageBrush.SetColorOverlay((DependencyObject) image1, new ColorW(131, 201, 226, (int) byte.MaxValue));
      image1.SetBinding(Image.SourceProperty, new Binding("Icon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock1 = new TextBlock();
      grid.Children.Add((UIElement) textBlock1);
      textBlock1.Name = "e_76";
      textBlock1.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock1.VerticalAlignment = VerticalAlignment.Center;
      Grid.SetColumn((UIElement) textBlock1, 1);
      textBlock1.SetBinding(TextBlock.TextProperty, new Binding("Name")
      {
        UseGeneratedBindings = true
      });
      StackPanel stackPanel = new StackPanel();
      grid.Children.Add((UIElement) stackPanel);
      stackPanel.Name = "e_77";
      stackPanel.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel, 1);
      Grid.SetRow((UIElement) stackPanel, 1);
      TextBlock textBlock2 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock2);
      textBlock2.Name = "e_78";
      textBlock2.Margin = new Thickness(4f, 1f, 1f, 1f);
      textBlock2.VerticalAlignment = VerticalAlignment.Center;
      textBlock2.FontFamily = new FontFamily("InventorySmall");
      textBlock2.FontSize = 10f;
      textBlock2.SetBinding(TextBlock.TextProperty, new Binding("RewardMoney_Formatted")
      {
        UseGeneratedBindings = true
      });
      Image image2 = new Image();
      stackPanel.Children.Add((UIElement) image2);
      image2.Name = "e_79";
      image2.Height = 14f;
      image2.Margin = new Thickness(1f, 1f, 1f, 1f);
      image2.HorizontalAlignment = HorizontalAlignment.Right;
      image2.VerticalAlignment = VerticalAlignment.Center;
      image2.Stretch = Stretch.Uniform;
      image2.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock3 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock3);
      textBlock3.Name = "e_80";
      textBlock3.Margin = new Thickness(4f, 1f, 4f, 1f);
      textBlock3.VerticalAlignment = VerticalAlignment.Center;
      textBlock3.Text = "|";
      textBlock3.FontFamily = new FontFamily("InventorySmall");
      textBlock3.FontSize = 10f;
      TextBlock textBlock4 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock4);
      textBlock4.Name = "e_81";
      textBlock4.Margin = new Thickness(1f, 1f, 1f, 1f);
      textBlock4.VerticalAlignment = VerticalAlignment.Center;
      textBlock4.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      textBlock4.FontFamily = new FontFamily("InventorySmall");
      textBlock4.FontSize = 10f;
      textBlock4.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_RepChange_Hint");
      TextBlock textBlock5 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock5);
      textBlock5.Name = "e_82";
      textBlock5.Margin = new Thickness(1f, 1f, 1f, 1f);
      textBlock5.VerticalAlignment = VerticalAlignment.Center;
      textBlock5.FontFamily = new FontFamily("InventorySmall");
      textBlock5.FontSize = 10f;
      textBlock5.SetBinding(TextBlock.TextProperty, new Binding("RewardReputation_Formatted")
      {
        UseGeneratedBindings = true
      });
      Image image3 = new Image();
      stackPanel.Children.Add((UIElement) image3);
      image3.Name = "e_83";
      image3.Width = 16f;
      image3.VerticalAlignment = VerticalAlignment.Center;
      image3.Source = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Contracts\\ArrowRepGain.png"
      };
      image3.Stretch = Stretch.Uniform;
      TextBlock textBlock6 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock6);
      textBlock6.Name = "e_84";
      textBlock6.Margin = new Thickness(1f, 1f, 1f, 1f);
      textBlock6.VerticalAlignment = VerticalAlignment.Center;
      textBlock6.FontFamily = new FontFamily("InventorySmall");
      textBlock6.FontSize = 10f;
      textBlock6.SetBinding(TextBlock.TextProperty, new Binding("FailReputationPenalty_Formated")
      {
        UseGeneratedBindings = true
      });
      Image image4 = new Image();
      stackPanel.Children.Add((UIElement) image4);
      image4.Name = "e_85";
      image4.Width = 16f;
      image4.VerticalAlignment = VerticalAlignment.Center;
      image4.Source = new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Contracts\\ArrowRepLoss.png"
      };
      image4.Stretch = Stretch.Uniform;
      Border border = new Border();
      grid.Children.Add((UIElement) border);
      border.Name = "e_86";
      border.Margin = new Thickness(2f, 2f, 2f, 2f);
      border.HorizontalAlignment = HorizontalAlignment.Left;
      border.VerticalAlignment = VerticalAlignment.Top;
      border.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      border.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      Grid.SetColumn((UIElement) border, 2);
      Grid.SetRowSpan((UIElement) border, 2);
      border.SetBinding(UIElement.VisibilityProperty, new Binding("IsFactionIconVisible")
      {
        UseGeneratedBindings = true
      });
      border.SetBinding(Control.BackgroundProperty, new Binding("FactionIconBackgroundColor")
      {
        UseGeneratedBindings = true
      });
      Image image5 = new Image();
      border.Child = (UIElement) image5;
      image5.Name = "FactionIcon";
      image5.Height = 32f;
      image5.Margin = new Thickness(0.0f, 0.0f, 0.0f, 0.0f);
      image5.Stretch = Stretch.Uniform;
      image5.SetBinding(UIElement.ToolTipProperty, new Binding("FactionIconTooltip")
      {
        UseGeneratedBindings = true
      });
      image5.SetBinding(Image.SourceProperty, new Binding("FactionIcon")
      {
        UseGeneratedBindings = true
      });
      image5.SetBinding(ImageBrush.ColorOverlayProperty, new Binding("FactionIconColor")
      {
        UseGeneratedBindings = true
      });
      return (UIElement) grid;
    }
  }
}
