// Decompiled with JetBrains decompiler
// Type: EmptyKeys.UserInterface.Generated.DataTemplatesContracts
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Generated.DataTemplatesContracts_Bindings;
using EmptyKeys.UserInterface.Media;
using Sandbox.Game.Screens.Models;
using System;
using System.CodeDom.Compiler;

namespace EmptyKeys.UserInterface.Generated
{
  [GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
  public sealed class DataTemplatesContracts : ResourceDictionary
  {
    private static DataTemplatesContracts singleton = new DataTemplatesContracts();

    public DataTemplatesContracts() => this.InitializeResources();

    public static DataTemplatesContracts Instance => DataTemplatesContracts.singleton;

    private void InitializeResources()
    {
      this.MergedDictionaries.Add((ResourceDictionary) Styles.Instance);
      this.Add((object) typeof (MyAdminSelectionItemModel), (object) new DataTemplate((object) typeof (MyAdminSelectionItemModel), new Func<UIElement, UIElement>(DataTemplatesContracts.r_0_dtMethod)));
      this.Add((object) typeof (MyContractConditionDeliverItemModel), (object) new DataTemplate((object) typeof (MyContractConditionDeliverItemModel), new Func<UIElement, UIElement>(DataTemplatesContracts.r_1_dtMethod)));
      this.Add((object) typeof (MyContractModelCustom), (object) new DataTemplate((object) typeof (MyContractModelCustom), new Func<UIElement, UIElement>(DataTemplatesContracts.r_2_dtMethod)));
      this.Add((object) typeof (MyContractModelDeliver), (object) new DataTemplate((object) typeof (MyContractModelDeliver), new Func<UIElement, UIElement>(DataTemplatesContracts.r_3_dtMethod)));
      this.Add((object) typeof (MyContractModelEscort), (object) new DataTemplate((object) typeof (MyContractModelEscort), new Func<UIElement, UIElement>(DataTemplatesContracts.r_4_dtMethod)));
      this.Add((object) typeof (MyContractModelFind), (object) new DataTemplate((object) typeof (MyContractModelFind), new Func<UIElement, UIElement>(DataTemplatesContracts.r_5_dtMethod)));
      this.Add((object) typeof (MyContractModelHunt), (object) new DataTemplate((object) typeof (MyContractModelHunt), new Func<UIElement, UIElement>(DataTemplatesContracts.r_6_dtMethod)));
      this.Add((object) typeof (MyContractModelObtainAndDeliver), (object) new DataTemplate((object) typeof (MyContractModelObtainAndDeliver), new Func<UIElement, UIElement>(DataTemplatesContracts.r_7_dtMethod)));
      this.Add((object) typeof (MyContractModelRepair), (object) new DataTemplate((object) typeof (MyContractModelRepair), new Func<UIElement, UIElement>(DataTemplatesContracts.r_8_dtMethod)));
      this.Add((object) typeof (MyContractTypeFilterItemModel), (object) new DataTemplate((object) typeof (MyContractTypeFilterItemModel), new Func<UIElement, UIElement>(DataTemplatesContracts.r_9_dtMethod)));
      this.Add((object) typeof (MyContractTypeModel), (object) new DataTemplate((object) typeof (MyContractTypeModel), new Func<UIElement, UIElement>(DataTemplatesContracts.r_10_dtMethod)));
      this.Add((object) typeof (MyDeliverItemModel), (object) new DataTemplate((object) typeof (MyDeliverItemModel), new Func<UIElement, UIElement>(DataTemplatesContracts.r_11_dtMethod)));
      this.Add((object) typeof (MySimpleSelectableItemModel), (object) new DataTemplate((object) typeof (MySimpleSelectableItemModel), new Func<UIElement, UIElement>(DataTemplatesContracts.r_12_dtMethod)));
      this.Add((object) "SelectableGridTemplate", (object) new DataTemplate(new Func<UIElement, UIElement>(DataTemplatesContracts.r_13_dtMethod)));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyAdminSelectionItemModel), "NameCombined", typeof (MyAdminSelectionItemModel_NameCombined_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractConditionDeliverItemModel), "Icon", typeof (MyContractConditionDeliverItemModel_Icon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractConditionDeliverItemModel), "Name", typeof (MyContractConditionDeliverItemModel_Name_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractConditionDeliverItemModel), "ItemAmount", typeof (MyContractConditionDeliverItemModel_ItemAmount_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractConditionDeliverItemModel), "ItemVolume_Formated", typeof (MyContractConditionDeliverItemModel_ItemVolume_Formated_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelCustom), "Header", typeof (MyContractModelCustom_Header_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelCustom), "Icon", typeof (MyContractModelCustom_Icon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelCustom), "Name", typeof (MyContractModelCustom_Name_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelCustom), "RewardMoney_Formatted", typeof (MyContractModelCustom_RewardMoney_Formatted_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelCustom), "CurrencyIcon", typeof (MyContractModelCustom_CurrencyIcon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelCustom), "RewardReputation", typeof (MyContractModelCustom_RewardReputation_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelCustom), "FailReputationPenalty_Formated", typeof (MyContractModelCustom_FailReputationPenalty_Formated_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelCustom), "InitialDeposit_Formated", typeof (MyContractModelCustom_InitialDeposit_Formated_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelCustom), "TimeLeft", typeof (MyContractModelCustom_TimeLeft_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelCustom), "Conditions", typeof (MyContractModelCustom_Conditions_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelCustom), "Description", typeof (MyContractModelCustom_Description_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelDeliver), "Header", typeof (MyContractModelDeliver_Header_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelDeliver), "Icon", typeof (MyContractModelDeliver_Icon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelDeliver), "Name", typeof (MyContractModelDeliver_Name_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelDeliver), "RewardMoney_Formatted", typeof (MyContractModelDeliver_RewardMoney_Formatted_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelDeliver), "CurrencyIcon", typeof (MyContractModelDeliver_CurrencyIcon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelDeliver), "RewardReputation", typeof (MyContractModelDeliver_RewardReputation_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelDeliver), "FailReputationPenalty_Formated", typeof (MyContractModelDeliver_FailReputationPenalty_Formated_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelDeliver), "InitialDeposit_Formated", typeof (MyContractModelDeliver_InitialDeposit_Formated_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelDeliver), "DeliverDistance_Formatted", typeof (MyContractModelDeliver_DeliverDistance_Formatted_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelDeliver), "TimeLeft", typeof (MyContractModelDeliver_TimeLeft_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelDeliver), "Conditions", typeof (MyContractModelDeliver_Conditions_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelDeliver), "Description", typeof (MyContractModelDeliver_Description_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelEscort), "Header", typeof (MyContractModelEscort_Header_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelEscort), "Icon", typeof (MyContractModelEscort_Icon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelEscort), "Name", typeof (MyContractModelEscort_Name_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelEscort), "RewardMoney_Formatted", typeof (MyContractModelEscort_RewardMoney_Formatted_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelEscort), "CurrencyIcon", typeof (MyContractModelEscort_CurrencyIcon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelEscort), "RewardReputation", typeof (MyContractModelEscort_RewardReputation_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelEscort), "FailReputationPenalty_Formated", typeof (MyContractModelEscort_FailReputationPenalty_Formated_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelEscort), "PathLength_Formatted", typeof (MyContractModelEscort_PathLength_Formatted_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelEscort), "TimeLeft", typeof (MyContractModelEscort_TimeLeft_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelEscort), "Conditions", typeof (MyContractModelEscort_Conditions_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelEscort), "Description", typeof (MyContractModelEscort_Description_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelFind), "Header", typeof (MyContractModelFind_Header_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelFind), "Icon", typeof (MyContractModelFind_Icon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelFind), "Name", typeof (MyContractModelFind_Name_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelFind), "RewardMoney_Formatted", typeof (MyContractModelFind_RewardMoney_Formatted_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelFind), "CurrencyIcon", typeof (MyContractModelFind_CurrencyIcon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelFind), "RewardReputation", typeof (MyContractModelFind_RewardReputation_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelFind), "FailReputationPenalty_Formated", typeof (MyContractModelFind_FailReputationPenalty_Formated_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelFind), "TimeLeft", typeof (MyContractModelFind_TimeLeft_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelFind), "MaxGpsOffset_Formatted", typeof (MyContractModelFind_MaxGpsOffset_Formatted_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelFind), "Conditions", typeof (MyContractModelFind_Conditions_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelFind), "Description", typeof (MyContractModelFind_Description_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelHunt), "Header", typeof (MyContractModelHunt_Header_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelHunt), "Icon", typeof (MyContractModelHunt_Icon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelHunt), "Name", typeof (MyContractModelHunt_Name_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelHunt), "RewardMoney_Formatted", typeof (MyContractModelHunt_RewardMoney_Formatted_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelHunt), "CurrencyIcon", typeof (MyContractModelHunt_CurrencyIcon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelHunt), "RewardReputation", typeof (MyContractModelHunt_RewardReputation_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelHunt), "TargetName_Formatted", typeof (MyContractModelHunt_TargetName_Formatted_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelHunt), "TimeLeft", typeof (MyContractModelHunt_TimeLeft_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelHunt), "Conditions", typeof (MyContractModelHunt_Conditions_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelHunt), "Description", typeof (MyContractModelHunt_Description_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelObtainAndDeliver), "Header", typeof (MyContractModelObtainAndDeliver_Header_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelObtainAndDeliver), "Icon", typeof (MyContractModelObtainAndDeliver_Icon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelObtainAndDeliver), "Name", typeof (MyContractModelObtainAndDeliver_Name_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelObtainAndDeliver), "RewardMoney_Formatted", typeof (MyContractModelObtainAndDeliver_RewardMoney_Formatted_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelObtainAndDeliver), "CurrencyIcon", typeof (MyContractModelObtainAndDeliver_CurrencyIcon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelObtainAndDeliver), "RewardReputation", typeof (MyContractModelObtainAndDeliver_RewardReputation_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelObtainAndDeliver), "Conditions", typeof (MyContractModelObtainAndDeliver_Conditions_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelObtainAndDeliver), "Description", typeof (MyContractModelObtainAndDeliver_Description_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelRepair), "Header", typeof (MyContractModelRepair_Header_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelRepair), "Icon", typeof (MyContractModelRepair_Icon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelRepair), "Name", typeof (MyContractModelRepair_Name_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelRepair), "RewardMoney_Formatted", typeof (MyContractModelRepair_RewardMoney_Formatted_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelRepair), "CurrencyIcon", typeof (MyContractModelRepair_CurrencyIcon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelRepair), "RewardReputation", typeof (MyContractModelRepair_RewardReputation_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelRepair), "FailReputationPenalty_Formated", typeof (MyContractModelRepair_FailReputationPenalty_Formated_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelRepair), "TimeLeft", typeof (MyContractModelRepair_TimeLeft_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelRepair), "Conditions", typeof (MyContractModelRepair_Conditions_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractModelRepair), "Description", typeof (MyContractModelRepair_Description_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractTypeFilterItemModel), "Icon", typeof (MyContractTypeFilterItemModel_Icon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractTypeFilterItemModel), "LocalizedName", typeof (MyContractTypeFilterItemModel_LocalizedName_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyContractTypeModel), "Name", typeof (MyContractTypeModel_Name_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyDeliverItemModel), "Icon", typeof (MyDeliverItemModel_Icon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyDeliverItemModel), "Name", typeof (MyDeliverItemModel_Name_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MySimpleSelectableItemModel), "DisplayName", typeof (MySimpleSelectableItemModel_DisplayName_PropertyInfo));
    }

    private static UIElement r_0_dtMethod(UIElement parent)
    {
      StackPanel stackPanel = new StackPanel();
      stackPanel.Parent = parent;
      stackPanel.Name = "e_0";
      stackPanel.Orientation = Orientation.Horizontal;
      TextBlock textBlock = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock);
      textBlock.Name = "e_1";
      textBlock.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock.VerticalAlignment = VerticalAlignment.Center;
      textBlock.SetBinding(TextBlock.TextProperty, new Binding("NameCombined")
      {
        UseGeneratedBindings = true
      });
      return (UIElement) stackPanel;
    }

    private static UIElement r_1_dtMethod(UIElement parent)
    {
      StackPanel stackPanel = new StackPanel();
      stackPanel.Parent = parent;
      stackPanel.Name = "e_2";
      stackPanel.Orientation = Orientation.Horizontal;
      Image image = new Image();
      stackPanel.Children.Add((UIElement) image);
      image.Name = "e_3";
      image.Width = 64f;
      image.Stretch = Stretch.Uniform;
      image.SetBinding(Image.SourceProperty, new Binding("Icon")
      {
        UseGeneratedBindings = true
      });
      Grid grid = new Grid();
      stackPanel.Children.Add((UIElement) grid);
      grid.Name = "e_4";
      grid.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition1 = new ColumnDefinition();
      grid.ColumnDefinitions.Add(columnDefinition1);
      ColumnDefinition columnDefinition2 = new ColumnDefinition();
      grid.ColumnDefinitions.Add(columnDefinition2);
      TextBlock textBlock1 = new TextBlock();
      grid.Children.Add((UIElement) textBlock1);
      textBlock1.Name = "e_5";
      textBlock1.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock1.VerticalAlignment = VerticalAlignment.Top;
      textBlock1.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock1, 0);
      Grid.SetRow((UIElement) textBlock1, 0);
      textBlock1.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Tooltip_Name");
      TextBlock textBlock2 = new TextBlock();
      grid.Children.Add((UIElement) textBlock2);
      textBlock2.Name = "e_6";
      textBlock2.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock2.VerticalAlignment = VerticalAlignment.Top;
      textBlock2.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock2, 1);
      Grid.SetRow((UIElement) textBlock2, 0);
      textBlock2.SetBinding(TextBlock.TextProperty, new Binding("Name")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock3 = new TextBlock();
      grid.Children.Add((UIElement) textBlock3);
      textBlock3.Name = "e_7";
      textBlock3.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock3.VerticalAlignment = VerticalAlignment.Top;
      textBlock3.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock3, 0);
      Grid.SetRow((UIElement) textBlock3, 1);
      textBlock3.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_ObtainDeliver_ItemAmount");
      TextBlock textBlock4 = new TextBlock();
      grid.Children.Add((UIElement) textBlock4);
      textBlock4.Name = "e_8";
      textBlock4.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock4.VerticalAlignment = VerticalAlignment.Top;
      textBlock4.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock4, 1);
      Grid.SetRow((UIElement) textBlock4, 1);
      textBlock4.SetBinding(TextBlock.TextProperty, new Binding("ItemAmount")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock5 = new TextBlock();
      grid.Children.Add((UIElement) textBlock5);
      textBlock5.Name = "e_9";
      textBlock5.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock5.VerticalAlignment = VerticalAlignment.Top;
      textBlock5.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock5, 0);
      Grid.SetRow((UIElement) textBlock5, 2);
      textBlock5.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_ObtainDeliver_ItemVolume");
      TextBlock textBlock6 = new TextBlock();
      grid.Children.Add((UIElement) textBlock6);
      textBlock6.Name = "e_10";
      textBlock6.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock6.VerticalAlignment = VerticalAlignment.Top;
      textBlock6.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock6, 1);
      Grid.SetRow((UIElement) textBlock6, 2);
      textBlock6.SetBinding(TextBlock.TextProperty, new Binding("ItemVolume_Formated")
      {
        UseGeneratedBindings = true
      });
      return (UIElement) stackPanel;
    }

    private static UIElement r_2_dtMethod(UIElement parent)
    {
      Grid grid1 = new Grid();
      grid1.Parent = parent;
      grid1.Name = "e_11";
      grid1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid1.RowDefinitions.Add(new RowDefinition());
      Image image1 = new Image();
      grid1.Children.Add((UIElement) image1);
      image1.Name = "e_12";
      image1.Height = 192f;
      image1.HorizontalAlignment = HorizontalAlignment.Right;
      image1.VerticalAlignment = VerticalAlignment.Top;
      image1.Stretch = Stretch.Uniform;
      image1.SetBinding(Image.SourceProperty, new Binding("Header")
      {
        UseGeneratedBindings = true
      });
      Grid grid2 = new Grid();
      grid1.Children.Add((UIElement) grid2);
      grid2.Name = "e_13";
      grid2.Margin = new Thickness(10f, 10f, 10f, 0.0f);
      grid2.HorizontalAlignment = HorizontalAlignment.Left;
      grid2.VerticalAlignment = VerticalAlignment.Top;
      grid2.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid2.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      Image image2 = new Image();
      grid2.Children.Add((UIElement) image2);
      image2.Name = "e_14";
      image2.Width = 80f;
      image2.HorizontalAlignment = HorizontalAlignment.Left;
      image2.Stretch = Stretch.Uniform;
      ImageBrush.SetColorOverlay((DependencyObject) image2, new ColorW(131, 201, 226, (int) byte.MaxValue));
      image2.SetBinding(Image.SourceProperty, new Binding("Icon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock1 = new TextBlock();
      grid2.Children.Add((UIElement) textBlock1);
      textBlock1.Name = "e_15";
      textBlock1.Margin = new Thickness(0.0f, 5f, 0.0f, 0.0f);
      textBlock1.VerticalAlignment = VerticalAlignment.Center;
      textBlock1.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetRow((UIElement) textBlock1, 1);
      textBlock1.SetBinding(TextBlock.TextProperty, new Binding("Name")
      {
        UseGeneratedBindings = true
      });
      Grid grid3 = new Grid();
      grid1.Children.Add((UIElement) grid3);
      grid3.Name = "e_16";
      grid3.Margin = new Thickness(10f, 5f, 10f, 5f);
      RowDefinition rowDefinition1 = new RowDefinition();
      grid3.RowDefinitions.Add(rowDefinition1);
      RowDefinition rowDefinition2 = new RowDefinition();
      grid3.RowDefinitions.Add(rowDefinition2);
      RowDefinition rowDefinition3 = new RowDefinition();
      grid3.RowDefinitions.Add(rowDefinition3);
      RowDefinition rowDefinition4 = new RowDefinition();
      grid3.RowDefinitions.Add(rowDefinition4);
      RowDefinition rowDefinition5 = new RowDefinition();
      grid3.RowDefinitions.Add(rowDefinition5);
      ColumnDefinition columnDefinition1 = new ColumnDefinition();
      grid3.ColumnDefinitions.Add(columnDefinition1);
      ColumnDefinition columnDefinition2 = new ColumnDefinition();
      grid3.ColumnDefinitions.Add(columnDefinition2);
      Grid.SetRow((UIElement) grid3, 1);
      TextBlock textBlock2 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock2);
      textBlock2.Name = "e_17";
      textBlock2.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock2.VerticalAlignment = VerticalAlignment.Center;
      textBlock2.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock2, 0);
      Grid.SetRow((UIElement) textBlock2, 0);
      textBlock2.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Tooltip_Currency");
      StackPanel stackPanel = new StackPanel();
      grid3.Children.Add((UIElement) stackPanel);
      stackPanel.Name = "e_18";
      stackPanel.Margin = new Thickness(2f, 2f, 2f, 2f);
      stackPanel.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel, 1);
      Grid.SetRow((UIElement) stackPanel, 0);
      TextBlock textBlock3 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock3);
      textBlock3.Name = "e_19";
      textBlock3.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock3.VerticalAlignment = VerticalAlignment.Center;
      textBlock3.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock3, 1);
      Grid.SetRow((UIElement) textBlock3, 0);
      textBlock3.SetBinding(TextBlock.TextProperty, new Binding("RewardMoney_Formatted")
      {
        UseGeneratedBindings = true
      });
      Image image3 = new Image();
      stackPanel.Children.Add((UIElement) image3);
      image3.Name = "e_20";
      image3.Height = 20f;
      image3.Margin = new Thickness(4f, 2f, 2f, 2f);
      image3.HorizontalAlignment = HorizontalAlignment.Left;
      image3.VerticalAlignment = VerticalAlignment.Center;
      image3.Stretch = Stretch.Uniform;
      Grid.SetColumn((UIElement) image3, 2);
      image3.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock4 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock4);
      textBlock4.Name = "e_21";
      textBlock4.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock4.VerticalAlignment = VerticalAlignment.Center;
      textBlock4.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock4, 0);
      Grid.SetRow((UIElement) textBlock4, 1);
      textBlock4.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Tooltip_Reputation");
      TextBlock textBlock5 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock5);
      textBlock5.Name = "e_22";
      textBlock5.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock5.VerticalAlignment = VerticalAlignment.Center;
      textBlock5.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock5, 1);
      Grid.SetRow((UIElement) textBlock5, 1);
      textBlock5.SetBinding(TextBlock.TextProperty, new Binding("RewardReputation")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock6 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock6);
      textBlock6.Name = "e_23";
      textBlock6.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock6.VerticalAlignment = VerticalAlignment.Center;
      textBlock6.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock6, 0);
      Grid.SetRow((UIElement) textBlock6, 2);
      textBlock6.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Tooltip_FailReputationPenalty");
      TextBlock textBlock7 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock7);
      textBlock7.Name = "e_24";
      textBlock7.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock7.VerticalAlignment = VerticalAlignment.Center;
      textBlock7.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock7, 1);
      Grid.SetRow((UIElement) textBlock7, 2);
      textBlock7.SetBinding(TextBlock.TextProperty, new Binding("FailReputationPenalty_Formated")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock8 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock8);
      textBlock8.Name = "e_25";
      textBlock8.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock8.VerticalAlignment = VerticalAlignment.Center;
      textBlock8.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock8, 0);
      Grid.SetRow((UIElement) textBlock8, 3);
      textBlock8.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Tooltip_InitialDeposit");
      TextBlock textBlock9 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock9);
      textBlock9.Name = "e_26";
      textBlock9.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock9.VerticalAlignment = VerticalAlignment.Center;
      textBlock9.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock9, 1);
      Grid.SetRow((UIElement) textBlock9, 3);
      textBlock9.SetBinding(TextBlock.TextProperty, new Binding("InitialDeposit_Formated")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock10 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock10);
      textBlock10.Name = "e_27";
      textBlock10.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock10.VerticalAlignment = VerticalAlignment.Center;
      textBlock10.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock10, 0);
      Grid.SetRow((UIElement) textBlock10, 4);
      textBlock10.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Tooltip_Duration");
      TextBlock textBlock11 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock11);
      textBlock11.Name = "e_28";
      textBlock11.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock11.VerticalAlignment = VerticalAlignment.Center;
      textBlock11.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock11, 1);
      Grid.SetRow((UIElement) textBlock11, 4);
      textBlock11.SetBinding(TextBlock.TextProperty, new Binding("TimeLeft")
      {
        UseGeneratedBindings = true
      });
      ItemsControl itemsControl = new ItemsControl();
      grid1.Children.Add((UIElement) itemsControl);
      itemsControl.Name = "e_29";
      itemsControl.Margin = new Thickness(10f, 0.0f, 10f, 0.0f);
      Grid.SetRow((UIElement) itemsControl, 2);
      itemsControl.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("Conditions")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock12 = new TextBlock();
      grid1.Children.Add((UIElement) textBlock12);
      textBlock12.Name = "e_30";
      textBlock12.UseLayoutRounding = true;
      textBlock12.Margin = new Thickness(10f, 0.0f, 10f, 0.0f);
      textBlock12.TextAlignment = TextAlignment.Left;
      textBlock12.TextWrapping = TextWrapping.Wrap;
      Grid.SetRow((UIElement) textBlock12, 3);
      textBlock12.SetBinding(TextBlock.TextProperty, new Binding("Description")
      {
        UseGeneratedBindings = true
      });
      return (UIElement) grid1;
    }

    private static UIElement r_3_dtMethod(UIElement parent)
    {
      Grid grid1 = new Grid();
      grid1.Parent = parent;
      grid1.Name = "e_31";
      grid1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid1.RowDefinitions.Add(new RowDefinition());
      Image image1 = new Image();
      grid1.Children.Add((UIElement) image1);
      image1.Name = "e_32";
      image1.Height = 192f;
      image1.HorizontalAlignment = HorizontalAlignment.Right;
      image1.VerticalAlignment = VerticalAlignment.Top;
      image1.Stretch = Stretch.Uniform;
      image1.SetBinding(Image.SourceProperty, new Binding("Header")
      {
        UseGeneratedBindings = true
      });
      Grid grid2 = new Grid();
      grid1.Children.Add((UIElement) grid2);
      grid2.Name = "e_33";
      grid2.Margin = new Thickness(10f, 10f, 10f, 0.0f);
      grid2.HorizontalAlignment = HorizontalAlignment.Left;
      grid2.VerticalAlignment = VerticalAlignment.Top;
      grid2.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid2.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      Image image2 = new Image();
      grid2.Children.Add((UIElement) image2);
      image2.Name = "e_34";
      image2.Width = 80f;
      image2.HorizontalAlignment = HorizontalAlignment.Left;
      image2.Stretch = Stretch.Uniform;
      ImageBrush.SetColorOverlay((DependencyObject) image2, new ColorW(131, 201, 226, (int) byte.MaxValue));
      image2.SetBinding(Image.SourceProperty, new Binding("Icon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock1 = new TextBlock();
      grid2.Children.Add((UIElement) textBlock1);
      textBlock1.Name = "e_35";
      textBlock1.Margin = new Thickness(0.0f, 5f, 0.0f, 0.0f);
      textBlock1.VerticalAlignment = VerticalAlignment.Center;
      textBlock1.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetRow((UIElement) textBlock1, 1);
      textBlock1.SetBinding(TextBlock.TextProperty, new Binding("Name")
      {
        UseGeneratedBindings = true
      });
      Grid grid3 = new Grid();
      grid1.Children.Add((UIElement) grid3);
      grid3.Name = "e_36";
      grid3.Margin = new Thickness(10f, 0.0f, 10f, 5f);
      RowDefinition rowDefinition1 = new RowDefinition();
      grid3.RowDefinitions.Add(rowDefinition1);
      RowDefinition rowDefinition2 = new RowDefinition();
      grid3.RowDefinitions.Add(rowDefinition2);
      RowDefinition rowDefinition3 = new RowDefinition();
      grid3.RowDefinitions.Add(rowDefinition3);
      RowDefinition rowDefinition4 = new RowDefinition();
      grid3.RowDefinitions.Add(rowDefinition4);
      RowDefinition rowDefinition5 = new RowDefinition();
      grid3.RowDefinitions.Add(rowDefinition5);
      RowDefinition rowDefinition6 = new RowDefinition();
      grid3.RowDefinitions.Add(rowDefinition6);
      ColumnDefinition columnDefinition1 = new ColumnDefinition();
      grid3.ColumnDefinitions.Add(columnDefinition1);
      ColumnDefinition columnDefinition2 = new ColumnDefinition();
      grid3.ColumnDefinitions.Add(columnDefinition2);
      Grid.SetRow((UIElement) grid3, 1);
      TextBlock textBlock2 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock2);
      textBlock2.Name = "e_37";
      textBlock2.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock2.VerticalAlignment = VerticalAlignment.Center;
      textBlock2.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock2, 0);
      Grid.SetRow((UIElement) textBlock2, 0);
      textBlock2.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Tooltip_Currency");
      StackPanel stackPanel = new StackPanel();
      grid3.Children.Add((UIElement) stackPanel);
      stackPanel.Name = "e_38";
      stackPanel.Margin = new Thickness(2f, 2f, 2f, 2f);
      stackPanel.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel, 1);
      Grid.SetRow((UIElement) stackPanel, 0);
      TextBlock textBlock3 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock3);
      textBlock3.Name = "e_39";
      textBlock3.VerticalAlignment = VerticalAlignment.Center;
      textBlock3.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      textBlock3.SetBinding(TextBlock.TextProperty, new Binding("RewardMoney_Formatted")
      {
        UseGeneratedBindings = true
      });
      Image image3 = new Image();
      stackPanel.Children.Add((UIElement) image3);
      image3.Name = "e_40";
      image3.Height = 20f;
      image3.Margin = new Thickness(4f, 2f, 2f, 2f);
      image3.HorizontalAlignment = HorizontalAlignment.Left;
      image3.VerticalAlignment = VerticalAlignment.Center;
      image3.Stretch = Stretch.Uniform;
      Grid.SetColumn((UIElement) image3, 2);
      image3.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock4 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock4);
      textBlock4.Name = "e_41";
      textBlock4.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock4.VerticalAlignment = VerticalAlignment.Center;
      textBlock4.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock4, 0);
      Grid.SetRow((UIElement) textBlock4, 1);
      textBlock4.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Tooltip_Reputation");
      TextBlock textBlock5 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock5);
      textBlock5.Name = "e_42";
      textBlock5.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock5.VerticalAlignment = VerticalAlignment.Center;
      textBlock5.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock5, 1);
      Grid.SetRow((UIElement) textBlock5, 1);
      textBlock5.SetBinding(TextBlock.TextProperty, new Binding("RewardReputation")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock6 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock6);
      textBlock6.Name = "e_43";
      textBlock6.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock6.VerticalAlignment = VerticalAlignment.Center;
      textBlock6.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock6, 0);
      Grid.SetRow((UIElement) textBlock6, 2);
      textBlock6.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Tooltip_FailReputationPenalty");
      TextBlock textBlock7 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock7);
      textBlock7.Name = "e_44";
      textBlock7.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock7.VerticalAlignment = VerticalAlignment.Center;
      textBlock7.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock7, 1);
      Grid.SetRow((UIElement) textBlock7, 2);
      textBlock7.SetBinding(TextBlock.TextProperty, new Binding("FailReputationPenalty_Formated")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock8 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock8);
      textBlock8.Name = "e_45";
      textBlock8.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock8.VerticalAlignment = VerticalAlignment.Center;
      textBlock8.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock8, 0);
      Grid.SetRow((UIElement) textBlock8, 3);
      textBlock8.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Tooltip_InitialDeposit");
      TextBlock textBlock9 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock9);
      textBlock9.Name = "e_46";
      textBlock9.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock9.VerticalAlignment = VerticalAlignment.Center;
      textBlock9.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock9, 1);
      Grid.SetRow((UIElement) textBlock9, 3);
      textBlock9.SetBinding(TextBlock.TextProperty, new Binding("InitialDeposit_Formated")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock10 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock10);
      textBlock10.Name = "e_47";
      textBlock10.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock10.VerticalAlignment = VerticalAlignment.Center;
      textBlock10.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock10, 0);
      Grid.SetRow((UIElement) textBlock10, 4);
      textBlock10.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Deliver_Distance");
      TextBlock textBlock11 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock11);
      textBlock11.Name = "e_48";
      textBlock11.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock11.VerticalAlignment = VerticalAlignment.Center;
      textBlock11.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock11, 1);
      Grid.SetRow((UIElement) textBlock11, 4);
      textBlock11.SetBinding(TextBlock.TextProperty, new Binding("DeliverDistance_Formatted")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock12 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock12);
      textBlock12.Name = "e_49";
      textBlock12.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock12.VerticalAlignment = VerticalAlignment.Center;
      textBlock12.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock12, 0);
      Grid.SetRow((UIElement) textBlock12, 5);
      textBlock12.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Tooltip_Duration");
      TextBlock textBlock13 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock13);
      textBlock13.Name = "e_50";
      textBlock13.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock13.VerticalAlignment = VerticalAlignment.Center;
      textBlock13.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock13, 1);
      Grid.SetRow((UIElement) textBlock13, 5);
      textBlock13.SetBinding(TextBlock.TextProperty, new Binding("TimeLeft")
      {
        UseGeneratedBindings = true
      });
      ItemsControl itemsControl = new ItemsControl();
      grid1.Children.Add((UIElement) itemsControl);
      itemsControl.Name = "e_51";
      itemsControl.Margin = new Thickness(10f, 0.0f, 10f, 0.0f);
      Grid.SetRow((UIElement) itemsControl, 2);
      itemsControl.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("Conditions")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock14 = new TextBlock();
      grid1.Children.Add((UIElement) textBlock14);
      textBlock14.Name = "e_52";
      textBlock14.UseLayoutRounding = true;
      textBlock14.Margin = new Thickness(10f, 0.0f, 10f, 0.0f);
      textBlock14.TextAlignment = TextAlignment.Left;
      textBlock14.TextWrapping = TextWrapping.Wrap;
      Grid.SetRow((UIElement) textBlock14, 3);
      textBlock14.SetBinding(TextBlock.TextProperty, new Binding("Description")
      {
        UseGeneratedBindings = true
      });
      return (UIElement) grid1;
    }

    private static UIElement r_4_dtMethod(UIElement parent)
    {
      Grid grid1 = new Grid();
      grid1.Parent = parent;
      grid1.Name = "e_53";
      grid1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid1.RowDefinitions.Add(new RowDefinition());
      Image image1 = new Image();
      grid1.Children.Add((UIElement) image1);
      image1.Name = "e_54";
      image1.Height = 192f;
      image1.HorizontalAlignment = HorizontalAlignment.Right;
      image1.VerticalAlignment = VerticalAlignment.Top;
      image1.Stretch = Stretch.Uniform;
      image1.SetBinding(Image.SourceProperty, new Binding("Header")
      {
        UseGeneratedBindings = true
      });
      Grid grid2 = new Grid();
      grid1.Children.Add((UIElement) grid2);
      grid2.Name = "e_55";
      grid2.Margin = new Thickness(10f, 10f, 10f, 0.0f);
      grid2.HorizontalAlignment = HorizontalAlignment.Left;
      grid2.VerticalAlignment = VerticalAlignment.Top;
      grid2.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid2.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      Image image2 = new Image();
      grid2.Children.Add((UIElement) image2);
      image2.Name = "e_56";
      image2.Width = 80f;
      image2.HorizontalAlignment = HorizontalAlignment.Left;
      image2.Stretch = Stretch.Uniform;
      ImageBrush.SetColorOverlay((DependencyObject) image2, new ColorW(131, 201, 226, (int) byte.MaxValue));
      image2.SetBinding(Image.SourceProperty, new Binding("Icon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock1 = new TextBlock();
      grid2.Children.Add((UIElement) textBlock1);
      textBlock1.Name = "e_57";
      textBlock1.Margin = new Thickness(0.0f, 5f, 0.0f, 0.0f);
      textBlock1.VerticalAlignment = VerticalAlignment.Center;
      textBlock1.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetRow((UIElement) textBlock1, 1);
      textBlock1.SetBinding(TextBlock.TextProperty, new Binding("Name")
      {
        UseGeneratedBindings = true
      });
      Grid grid3 = new Grid();
      grid1.Children.Add((UIElement) grid3);
      grid3.Name = "e_58";
      grid3.Margin = new Thickness(10f, 5f, 10f, 5f);
      RowDefinition rowDefinition1 = new RowDefinition();
      grid3.RowDefinitions.Add(rowDefinition1);
      RowDefinition rowDefinition2 = new RowDefinition();
      grid3.RowDefinitions.Add(rowDefinition2);
      RowDefinition rowDefinition3 = new RowDefinition();
      grid3.RowDefinitions.Add(rowDefinition3);
      RowDefinition rowDefinition4 = new RowDefinition();
      grid3.RowDefinitions.Add(rowDefinition4);
      RowDefinition rowDefinition5 = new RowDefinition();
      grid3.RowDefinitions.Add(rowDefinition5);
      ColumnDefinition columnDefinition1 = new ColumnDefinition();
      grid3.ColumnDefinitions.Add(columnDefinition1);
      ColumnDefinition columnDefinition2 = new ColumnDefinition();
      grid3.ColumnDefinitions.Add(columnDefinition2);
      Grid.SetRow((UIElement) grid3, 1);
      TextBlock textBlock2 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock2);
      textBlock2.Name = "e_59";
      textBlock2.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock2.VerticalAlignment = VerticalAlignment.Center;
      textBlock2.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock2, 0);
      Grid.SetRow((UIElement) textBlock2, 0);
      textBlock2.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Tooltip_Currency");
      StackPanel stackPanel = new StackPanel();
      grid3.Children.Add((UIElement) stackPanel);
      stackPanel.Name = "e_60";
      stackPanel.Margin = new Thickness(2f, 2f, 2f, 2f);
      stackPanel.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel, 1);
      Grid.SetRow((UIElement) stackPanel, 0);
      TextBlock textBlock3 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock3);
      textBlock3.Name = "e_61";
      textBlock3.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock3.VerticalAlignment = VerticalAlignment.Center;
      textBlock3.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock3, 1);
      Grid.SetRow((UIElement) textBlock3, 0);
      textBlock3.SetBinding(TextBlock.TextProperty, new Binding("RewardMoney_Formatted")
      {
        UseGeneratedBindings = true
      });
      Image image3 = new Image();
      stackPanel.Children.Add((UIElement) image3);
      image3.Name = "e_62";
      image3.Height = 20f;
      image3.Margin = new Thickness(4f, 2f, 2f, 2f);
      image3.HorizontalAlignment = HorizontalAlignment.Left;
      image3.VerticalAlignment = VerticalAlignment.Center;
      image3.Stretch = Stretch.Uniform;
      Grid.SetColumn((UIElement) image3, 2);
      image3.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock4 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock4);
      textBlock4.Name = "e_63";
      textBlock4.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock4.VerticalAlignment = VerticalAlignment.Center;
      textBlock4.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock4, 0);
      Grid.SetRow((UIElement) textBlock4, 1);
      textBlock4.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Tooltip_Reputation");
      TextBlock textBlock5 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock5);
      textBlock5.Name = "e_64";
      textBlock5.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock5.VerticalAlignment = VerticalAlignment.Center;
      textBlock5.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock5, 1);
      Grid.SetRow((UIElement) textBlock5, 1);
      textBlock5.SetBinding(TextBlock.TextProperty, new Binding("RewardReputation")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock6 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock6);
      textBlock6.Name = "e_65";
      textBlock6.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock6.VerticalAlignment = VerticalAlignment.Center;
      textBlock6.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock6, 0);
      Grid.SetRow((UIElement) textBlock6, 2);
      textBlock6.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Tooltip_FailReputationPenalty");
      TextBlock textBlock7 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock7);
      textBlock7.Name = "e_66";
      textBlock7.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock7.VerticalAlignment = VerticalAlignment.Center;
      textBlock7.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock7, 1);
      Grid.SetRow((UIElement) textBlock7, 2);
      textBlock7.SetBinding(TextBlock.TextProperty, new Binding("FailReputationPenalty_Formated")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock8 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock8);
      textBlock8.Name = "e_67";
      textBlock8.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock8.VerticalAlignment = VerticalAlignment.Center;
      textBlock8.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock8, 0);
      Grid.SetRow((UIElement) textBlock8, 3);
      textBlock8.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Deliver_Distance");
      TextBlock textBlock9 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock9);
      textBlock9.Name = "e_68";
      textBlock9.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock9.VerticalAlignment = VerticalAlignment.Center;
      textBlock9.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock9, 1);
      Grid.SetRow((UIElement) textBlock9, 3);
      textBlock9.SetBinding(TextBlock.TextProperty, new Binding("PathLength_Formatted")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock10 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock10);
      textBlock10.Name = "e_69";
      textBlock10.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock10.VerticalAlignment = VerticalAlignment.Center;
      textBlock10.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock10, 0);
      Grid.SetRow((UIElement) textBlock10, 4);
      textBlock10.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Tooltip_Duration");
      TextBlock textBlock11 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock11);
      textBlock11.Name = "e_70";
      textBlock11.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock11.VerticalAlignment = VerticalAlignment.Center;
      textBlock11.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock11, 1);
      Grid.SetRow((UIElement) textBlock11, 4);
      textBlock11.SetBinding(TextBlock.TextProperty, new Binding("TimeLeft")
      {
        UseGeneratedBindings = true
      });
      ItemsControl itemsControl = new ItemsControl();
      grid1.Children.Add((UIElement) itemsControl);
      itemsControl.Name = "e_71";
      itemsControl.Margin = new Thickness(10f, 0.0f, 10f, 0.0f);
      Grid.SetRow((UIElement) itemsControl, 2);
      itemsControl.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("Conditions")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock12 = new TextBlock();
      grid1.Children.Add((UIElement) textBlock12);
      textBlock12.Name = "e_72";
      textBlock12.UseLayoutRounding = true;
      textBlock12.Margin = new Thickness(10f, 0.0f, 10f, 0.0f);
      textBlock12.TextAlignment = TextAlignment.Left;
      textBlock12.TextWrapping = TextWrapping.Wrap;
      Grid.SetRow((UIElement) textBlock12, 3);
      textBlock12.SetBinding(TextBlock.TextProperty, new Binding("Description")
      {
        UseGeneratedBindings = true
      });
      return (UIElement) grid1;
    }

    private static UIElement r_5_dtMethod(UIElement parent)
    {
      Grid grid1 = new Grid();
      grid1.Parent = parent;
      grid1.Name = "e_73";
      grid1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid1.RowDefinitions.Add(new RowDefinition());
      Image image1 = new Image();
      grid1.Children.Add((UIElement) image1);
      image1.Name = "e_74";
      image1.Height = 192f;
      image1.HorizontalAlignment = HorizontalAlignment.Right;
      image1.VerticalAlignment = VerticalAlignment.Top;
      image1.Stretch = Stretch.Uniform;
      image1.SetBinding(Image.SourceProperty, new Binding("Header")
      {
        UseGeneratedBindings = true
      });
      Grid grid2 = new Grid();
      grid1.Children.Add((UIElement) grid2);
      grid2.Name = "e_75";
      grid2.Margin = new Thickness(10f, 10f, 10f, 0.0f);
      grid2.HorizontalAlignment = HorizontalAlignment.Left;
      grid2.VerticalAlignment = VerticalAlignment.Top;
      grid2.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid2.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      Image image2 = new Image();
      grid2.Children.Add((UIElement) image2);
      image2.Name = "e_76";
      image2.Width = 80f;
      image2.HorizontalAlignment = HorizontalAlignment.Left;
      image2.Stretch = Stretch.Uniform;
      ImageBrush.SetColorOverlay((DependencyObject) image2, new ColorW(131, 201, 226, (int) byte.MaxValue));
      image2.SetBinding(Image.SourceProperty, new Binding("Icon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock1 = new TextBlock();
      grid2.Children.Add((UIElement) textBlock1);
      textBlock1.Name = "e_77";
      textBlock1.Margin = new Thickness(0.0f, 5f, 0.0f, 0.0f);
      textBlock1.VerticalAlignment = VerticalAlignment.Center;
      textBlock1.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetRow((UIElement) textBlock1, 1);
      textBlock1.SetBinding(TextBlock.TextProperty, new Binding("Name")
      {
        UseGeneratedBindings = true
      });
      Grid grid3 = new Grid();
      grid1.Children.Add((UIElement) grid3);
      grid3.Name = "e_78";
      grid3.Margin = new Thickness(10f, 5f, 10f, 5f);
      RowDefinition rowDefinition1 = new RowDefinition();
      grid3.RowDefinitions.Add(rowDefinition1);
      RowDefinition rowDefinition2 = new RowDefinition();
      grid3.RowDefinitions.Add(rowDefinition2);
      RowDefinition rowDefinition3 = new RowDefinition();
      grid3.RowDefinitions.Add(rowDefinition3);
      RowDefinition rowDefinition4 = new RowDefinition();
      grid3.RowDefinitions.Add(rowDefinition4);
      RowDefinition rowDefinition5 = new RowDefinition();
      grid3.RowDefinitions.Add(rowDefinition5);
      ColumnDefinition columnDefinition1 = new ColumnDefinition();
      grid3.ColumnDefinitions.Add(columnDefinition1);
      ColumnDefinition columnDefinition2 = new ColumnDefinition();
      grid3.ColumnDefinitions.Add(columnDefinition2);
      Grid.SetRow((UIElement) grid3, 1);
      TextBlock textBlock2 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock2);
      textBlock2.Name = "e_79";
      textBlock2.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock2.VerticalAlignment = VerticalAlignment.Center;
      textBlock2.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock2, 0);
      Grid.SetRow((UIElement) textBlock2, 0);
      textBlock2.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Tooltip_Currency");
      StackPanel stackPanel = new StackPanel();
      grid3.Children.Add((UIElement) stackPanel);
      stackPanel.Name = "e_80";
      stackPanel.Margin = new Thickness(2f, 2f, 2f, 2f);
      stackPanel.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel, 1);
      Grid.SetRow((UIElement) stackPanel, 0);
      TextBlock textBlock3 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock3);
      textBlock3.Name = "e_81";
      textBlock3.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock3.VerticalAlignment = VerticalAlignment.Center;
      textBlock3.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock3, 1);
      Grid.SetRow((UIElement) textBlock3, 0);
      textBlock3.SetBinding(TextBlock.TextProperty, new Binding("RewardMoney_Formatted")
      {
        UseGeneratedBindings = true
      });
      Image image3 = new Image();
      stackPanel.Children.Add((UIElement) image3);
      image3.Name = "e_82";
      image3.Height = 20f;
      image3.Margin = new Thickness(4f, 2f, 2f, 2f);
      image3.HorizontalAlignment = HorizontalAlignment.Left;
      image3.VerticalAlignment = VerticalAlignment.Center;
      image3.Stretch = Stretch.Uniform;
      Grid.SetColumn((UIElement) image3, 2);
      image3.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock4 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock4);
      textBlock4.Name = "e_83";
      textBlock4.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock4.VerticalAlignment = VerticalAlignment.Center;
      textBlock4.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock4, 0);
      Grid.SetRow((UIElement) textBlock4, 1);
      textBlock4.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Tooltip_Reputation");
      TextBlock textBlock5 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock5);
      textBlock5.Name = "e_84";
      textBlock5.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock5.VerticalAlignment = VerticalAlignment.Center;
      textBlock5.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock5, 1);
      Grid.SetRow((UIElement) textBlock5, 1);
      textBlock5.SetBinding(TextBlock.TextProperty, new Binding("RewardReputation")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock6 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock6);
      textBlock6.Name = "e_85";
      textBlock6.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock6.VerticalAlignment = VerticalAlignment.Center;
      textBlock6.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock6, 0);
      Grid.SetRow((UIElement) textBlock6, 2);
      textBlock6.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Tooltip_FailReputationPenalty");
      TextBlock textBlock7 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock7);
      textBlock7.Name = "e_86";
      textBlock7.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock7.VerticalAlignment = VerticalAlignment.Center;
      textBlock7.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock7, 1);
      Grid.SetRow((UIElement) textBlock7, 2);
      textBlock7.SetBinding(TextBlock.TextProperty, new Binding("FailReputationPenalty_Formated")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock8 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock8);
      textBlock8.Name = "e_87";
      textBlock8.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock8.VerticalAlignment = VerticalAlignment.Center;
      textBlock8.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock8, 0);
      Grid.SetRow((UIElement) textBlock8, 3);
      textBlock8.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Tooltip_Duration");
      TextBlock textBlock9 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock9);
      textBlock9.Name = "e_88";
      textBlock9.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock9.VerticalAlignment = VerticalAlignment.Center;
      textBlock9.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock9, 1);
      Grid.SetRow((UIElement) textBlock9, 3);
      textBlock9.SetBinding(TextBlock.TextProperty, new Binding("TimeLeft")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock10 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock10);
      textBlock10.Name = "e_89";
      textBlock10.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock10.VerticalAlignment = VerticalAlignment.Center;
      textBlock10.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock10, 0);
      Grid.SetRow((UIElement) textBlock10, 4);
      textBlock10.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Tooltip_MaxGpsOffset");
      TextBlock textBlock11 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock11);
      textBlock11.Name = "e_90";
      textBlock11.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock11.VerticalAlignment = VerticalAlignment.Center;
      textBlock11.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock11, 1);
      Grid.SetRow((UIElement) textBlock11, 4);
      textBlock11.SetBinding(TextBlock.TextProperty, new Binding("MaxGpsOffset_Formatted")
      {
        UseGeneratedBindings = true
      });
      ItemsControl itemsControl = new ItemsControl();
      grid1.Children.Add((UIElement) itemsControl);
      itemsControl.Name = "e_91";
      itemsControl.Margin = new Thickness(10f, 0.0f, 10f, 0.0f);
      Grid.SetRow((UIElement) itemsControl, 2);
      itemsControl.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("Conditions")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock12 = new TextBlock();
      grid1.Children.Add((UIElement) textBlock12);
      textBlock12.Name = "e_92";
      textBlock12.UseLayoutRounding = true;
      textBlock12.Margin = new Thickness(10f, 0.0f, 10f, 0.0f);
      textBlock12.TextAlignment = TextAlignment.Left;
      textBlock12.TextWrapping = TextWrapping.Wrap;
      Grid.SetRow((UIElement) textBlock12, 3);
      textBlock12.SetBinding(TextBlock.TextProperty, new Binding("Description")
      {
        UseGeneratedBindings = true
      });
      return (UIElement) grid1;
    }

    private static UIElement r_6_dtMethod(UIElement parent)
    {
      Grid grid1 = new Grid();
      grid1.Parent = parent;
      grid1.Name = "e_93";
      grid1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid1.RowDefinitions.Add(new RowDefinition());
      Image image1 = new Image();
      grid1.Children.Add((UIElement) image1);
      image1.Name = "e_94";
      image1.Height = 192f;
      image1.HorizontalAlignment = HorizontalAlignment.Right;
      image1.VerticalAlignment = VerticalAlignment.Top;
      image1.Stretch = Stretch.Uniform;
      image1.SetBinding(Image.SourceProperty, new Binding("Header")
      {
        UseGeneratedBindings = true
      });
      Grid grid2 = new Grid();
      grid1.Children.Add((UIElement) grid2);
      grid2.Name = "e_95";
      grid2.Margin = new Thickness(10f, 10f, 10f, 0.0f);
      grid2.HorizontalAlignment = HorizontalAlignment.Left;
      grid2.VerticalAlignment = VerticalAlignment.Top;
      grid2.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid2.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      Image image2 = new Image();
      grid2.Children.Add((UIElement) image2);
      image2.Name = "e_96";
      image2.Width = 80f;
      image2.HorizontalAlignment = HorizontalAlignment.Left;
      image2.Stretch = Stretch.Uniform;
      ImageBrush.SetColorOverlay((DependencyObject) image2, new ColorW(131, 201, 226, (int) byte.MaxValue));
      image2.SetBinding(Image.SourceProperty, new Binding("Icon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock1 = new TextBlock();
      grid2.Children.Add((UIElement) textBlock1);
      textBlock1.Name = "e_97";
      textBlock1.Margin = new Thickness(0.0f, 5f, 0.0f, 0.0f);
      textBlock1.VerticalAlignment = VerticalAlignment.Center;
      textBlock1.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetRow((UIElement) textBlock1, 1);
      textBlock1.SetBinding(TextBlock.TextProperty, new Binding("Name")
      {
        UseGeneratedBindings = true
      });
      Grid grid3 = new Grid();
      grid1.Children.Add((UIElement) grid3);
      grid3.Name = "e_98";
      grid3.Margin = new Thickness(10f, 5f, 10f, 5f);
      RowDefinition rowDefinition1 = new RowDefinition();
      grid3.RowDefinitions.Add(rowDefinition1);
      RowDefinition rowDefinition2 = new RowDefinition();
      grid3.RowDefinitions.Add(rowDefinition2);
      RowDefinition rowDefinition3 = new RowDefinition();
      grid3.RowDefinitions.Add(rowDefinition3);
      RowDefinition rowDefinition4 = new RowDefinition();
      grid3.RowDefinitions.Add(rowDefinition4);
      ColumnDefinition columnDefinition1 = new ColumnDefinition();
      grid3.ColumnDefinitions.Add(columnDefinition1);
      ColumnDefinition columnDefinition2 = new ColumnDefinition();
      grid3.ColumnDefinitions.Add(columnDefinition2);
      Grid.SetRow((UIElement) grid3, 1);
      TextBlock textBlock2 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock2);
      textBlock2.Name = "e_99";
      textBlock2.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock2.VerticalAlignment = VerticalAlignment.Center;
      textBlock2.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock2, 0);
      Grid.SetRow((UIElement) textBlock2, 0);
      textBlock2.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Tooltip_Currency");
      StackPanel stackPanel = new StackPanel();
      grid3.Children.Add((UIElement) stackPanel);
      stackPanel.Name = "e_100";
      stackPanel.Margin = new Thickness(2f, 2f, 2f, 2f);
      stackPanel.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel, 1);
      Grid.SetRow((UIElement) stackPanel, 0);
      TextBlock textBlock3 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock3);
      textBlock3.Name = "e_101";
      textBlock3.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock3.VerticalAlignment = VerticalAlignment.Center;
      textBlock3.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock3, 1);
      Grid.SetRow((UIElement) textBlock3, 0);
      textBlock3.SetBinding(TextBlock.TextProperty, new Binding("RewardMoney_Formatted")
      {
        UseGeneratedBindings = true
      });
      Image image3 = new Image();
      stackPanel.Children.Add((UIElement) image3);
      image3.Name = "e_102";
      image3.Height = 20f;
      image3.Margin = new Thickness(4f, 2f, 2f, 2f);
      image3.HorizontalAlignment = HorizontalAlignment.Left;
      image3.VerticalAlignment = VerticalAlignment.Center;
      image3.Stretch = Stretch.Uniform;
      Grid.SetColumn((UIElement) image3, 2);
      image3.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock4 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock4);
      textBlock4.Name = "e_103";
      textBlock4.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock4.VerticalAlignment = VerticalAlignment.Center;
      textBlock4.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock4, 0);
      Grid.SetRow((UIElement) textBlock4, 1);
      textBlock4.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Tooltip_Reputation");
      TextBlock textBlock5 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock5);
      textBlock5.Name = "e_104";
      textBlock5.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock5.VerticalAlignment = VerticalAlignment.Center;
      textBlock5.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock5, 1);
      Grid.SetRow((UIElement) textBlock5, 1);
      textBlock5.SetBinding(TextBlock.TextProperty, new Binding("RewardReputation")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock6 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock6);
      textBlock6.Name = "e_105";
      textBlock6.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock6.VerticalAlignment = VerticalAlignment.Center;
      textBlock6.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock6, 0);
      Grid.SetRow((UIElement) textBlock6, 2);
      textBlock6.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Hunt_Target");
      TextBlock textBlock7 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock7);
      textBlock7.Name = "e_106";
      textBlock7.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock7.VerticalAlignment = VerticalAlignment.Center;
      textBlock7.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock7, 1);
      Grid.SetRow((UIElement) textBlock7, 2);
      textBlock7.SetBinding(TextBlock.TextProperty, new Binding("TargetName_Formatted")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock8 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock8);
      textBlock8.Name = "e_107";
      textBlock8.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock8.VerticalAlignment = VerticalAlignment.Center;
      textBlock8.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock8, 0);
      Grid.SetRow((UIElement) textBlock8, 3);
      textBlock8.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Tooltip_Duration");
      TextBlock textBlock9 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock9);
      textBlock9.Name = "e_108";
      textBlock9.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock9.VerticalAlignment = VerticalAlignment.Center;
      textBlock9.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock9, 1);
      Grid.SetRow((UIElement) textBlock9, 3);
      textBlock9.SetBinding(TextBlock.TextProperty, new Binding("TimeLeft")
      {
        UseGeneratedBindings = true
      });
      ItemsControl itemsControl = new ItemsControl();
      grid1.Children.Add((UIElement) itemsControl);
      itemsControl.Name = "e_109";
      itemsControl.Margin = new Thickness(10f, 0.0f, 10f, 0.0f);
      Grid.SetRow((UIElement) itemsControl, 2);
      itemsControl.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("Conditions")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock10 = new TextBlock();
      grid1.Children.Add((UIElement) textBlock10);
      textBlock10.Name = "e_110";
      textBlock10.UseLayoutRounding = true;
      textBlock10.Margin = new Thickness(10f, 0.0f, 10f, 0.0f);
      textBlock10.TextAlignment = TextAlignment.Left;
      textBlock10.TextWrapping = TextWrapping.Wrap;
      Grid.SetRow((UIElement) textBlock10, 3);
      textBlock10.SetBinding(TextBlock.TextProperty, new Binding("Description")
      {
        UseGeneratedBindings = true
      });
      return (UIElement) grid1;
    }

    private static UIElement r_7_dtMethod(UIElement parent)
    {
      Grid grid1 = new Grid();
      grid1.Parent = parent;
      grid1.Name = "e_111";
      grid1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid1.RowDefinitions.Add(new RowDefinition());
      Image image1 = new Image();
      grid1.Children.Add((UIElement) image1);
      image1.Name = "e_112";
      image1.Height = 192f;
      image1.HorizontalAlignment = HorizontalAlignment.Right;
      image1.VerticalAlignment = VerticalAlignment.Top;
      image1.Stretch = Stretch.Uniform;
      image1.SetBinding(Image.SourceProperty, new Binding("Header")
      {
        UseGeneratedBindings = true
      });
      Grid grid2 = new Grid();
      grid1.Children.Add((UIElement) grid2);
      grid2.Name = "e_113";
      grid2.Margin = new Thickness(10f, 10f, 10f, 0.0f);
      grid2.HorizontalAlignment = HorizontalAlignment.Left;
      grid2.VerticalAlignment = VerticalAlignment.Top;
      grid2.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid2.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      Image image2 = new Image();
      grid2.Children.Add((UIElement) image2);
      image2.Name = "e_114";
      image2.Width = 80f;
      image2.HorizontalAlignment = HorizontalAlignment.Left;
      image2.Stretch = Stretch.Uniform;
      ImageBrush.SetColorOverlay((DependencyObject) image2, new ColorW(131, 201, 226, (int) byte.MaxValue));
      image2.SetBinding(Image.SourceProperty, new Binding("Icon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock1 = new TextBlock();
      grid2.Children.Add((UIElement) textBlock1);
      textBlock1.Name = "e_115";
      textBlock1.Margin = new Thickness(0.0f, 5f, 0.0f, 0.0f);
      textBlock1.VerticalAlignment = VerticalAlignment.Center;
      textBlock1.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetRow((UIElement) textBlock1, 1);
      textBlock1.SetBinding(TextBlock.TextProperty, new Binding("Name")
      {
        UseGeneratedBindings = true
      });
      Grid grid3 = new Grid();
      grid1.Children.Add((UIElement) grid3);
      grid3.Name = "e_116";
      grid3.Margin = new Thickness(10f, 5f, 10f, 5f);
      RowDefinition rowDefinition1 = new RowDefinition();
      grid3.RowDefinitions.Add(rowDefinition1);
      RowDefinition rowDefinition2 = new RowDefinition();
      grid3.RowDefinitions.Add(rowDefinition2);
      ColumnDefinition columnDefinition1 = new ColumnDefinition();
      grid3.ColumnDefinitions.Add(columnDefinition1);
      ColumnDefinition columnDefinition2 = new ColumnDefinition();
      grid3.ColumnDefinitions.Add(columnDefinition2);
      Grid.SetRow((UIElement) grid3, 1);
      TextBlock textBlock2 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock2);
      textBlock2.Name = "e_117";
      textBlock2.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock2.VerticalAlignment = VerticalAlignment.Center;
      textBlock2.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock2, 0);
      Grid.SetRow((UIElement) textBlock2, 0);
      textBlock2.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Tooltip_Currency");
      StackPanel stackPanel = new StackPanel();
      grid3.Children.Add((UIElement) stackPanel);
      stackPanel.Name = "e_118";
      stackPanel.Margin = new Thickness(2f, 2f, 2f, 2f);
      stackPanel.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel, 1);
      Grid.SetRow((UIElement) stackPanel, 0);
      TextBlock textBlock3 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock3);
      textBlock3.Name = "e_119";
      textBlock3.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock3.VerticalAlignment = VerticalAlignment.Center;
      textBlock3.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock3, 1);
      Grid.SetRow((UIElement) textBlock3, 0);
      textBlock3.SetBinding(TextBlock.TextProperty, new Binding("RewardMoney_Formatted")
      {
        UseGeneratedBindings = true
      });
      Image image3 = new Image();
      stackPanel.Children.Add((UIElement) image3);
      image3.Name = "e_120";
      image3.Height = 20f;
      image3.Margin = new Thickness(4f, 2f, 2f, 2f);
      image3.HorizontalAlignment = HorizontalAlignment.Left;
      image3.VerticalAlignment = VerticalAlignment.Center;
      image3.Stretch = Stretch.Uniform;
      Grid.SetColumn((UIElement) image3, 2);
      image3.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock4 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock4);
      textBlock4.Name = "e_121";
      textBlock4.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock4.VerticalAlignment = VerticalAlignment.Center;
      textBlock4.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock4, 0);
      Grid.SetRow((UIElement) textBlock4, 1);
      textBlock4.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Tooltip_Reputation");
      TextBlock textBlock5 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock5);
      textBlock5.Name = "e_122";
      textBlock5.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock5.VerticalAlignment = VerticalAlignment.Center;
      textBlock5.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock5, 1);
      Grid.SetRow((UIElement) textBlock5, 1);
      textBlock5.SetBinding(TextBlock.TextProperty, new Binding("RewardReputation")
      {
        UseGeneratedBindings = true
      });
      ItemsControl itemsControl = new ItemsControl();
      grid1.Children.Add((UIElement) itemsControl);
      itemsControl.Name = "e_123";
      itemsControl.Margin = new Thickness(10f, 0.0f, 10f, 0.0f);
      Grid.SetRow((UIElement) itemsControl, 2);
      itemsControl.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("Conditions")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock6 = new TextBlock();
      grid1.Children.Add((UIElement) textBlock6);
      textBlock6.Name = "e_124";
      textBlock6.UseLayoutRounding = true;
      textBlock6.Margin = new Thickness(10f, 0.0f, 10f, 0.0f);
      textBlock6.TextAlignment = TextAlignment.Left;
      textBlock6.TextWrapping = TextWrapping.Wrap;
      Grid.SetRow((UIElement) textBlock6, 3);
      textBlock6.SetBinding(TextBlock.TextProperty, new Binding("Description")
      {
        UseGeneratedBindings = true
      });
      return (UIElement) grid1;
    }

    private static UIElement r_8_dtMethod(UIElement parent)
    {
      Grid grid1 = new Grid();
      grid1.Parent = parent;
      grid1.Name = "e_125";
      grid1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid1.RowDefinitions.Add(new RowDefinition());
      Image image1 = new Image();
      grid1.Children.Add((UIElement) image1);
      image1.Name = "e_126";
      image1.Height = 192f;
      image1.HorizontalAlignment = HorizontalAlignment.Right;
      image1.VerticalAlignment = VerticalAlignment.Top;
      image1.Stretch = Stretch.Uniform;
      image1.SetBinding(Image.SourceProperty, new Binding("Header")
      {
        UseGeneratedBindings = true
      });
      Grid grid2 = new Grid();
      grid1.Children.Add((UIElement) grid2);
      grid2.Name = "e_127";
      grid2.Margin = new Thickness(10f, 10f, 10f, 0.0f);
      grid2.HorizontalAlignment = HorizontalAlignment.Left;
      grid2.VerticalAlignment = VerticalAlignment.Top;
      grid2.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid2.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      Image image2 = new Image();
      grid2.Children.Add((UIElement) image2);
      image2.Name = "e_128";
      image2.Width = 80f;
      image2.HorizontalAlignment = HorizontalAlignment.Left;
      image2.Stretch = Stretch.Uniform;
      ImageBrush.SetColorOverlay((DependencyObject) image2, new ColorW(131, 201, 226, (int) byte.MaxValue));
      image2.SetBinding(Image.SourceProperty, new Binding("Icon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock1 = new TextBlock();
      grid2.Children.Add((UIElement) textBlock1);
      textBlock1.Name = "e_129";
      textBlock1.Margin = new Thickness(0.0f, 5f, 0.0f, 0.0f);
      textBlock1.VerticalAlignment = VerticalAlignment.Center;
      textBlock1.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetRow((UIElement) textBlock1, 1);
      textBlock1.SetBinding(TextBlock.TextProperty, new Binding("Name")
      {
        UseGeneratedBindings = true
      });
      Grid grid3 = new Grid();
      grid1.Children.Add((UIElement) grid3);
      grid3.Name = "e_130";
      grid3.Margin = new Thickness(10f, 5f, 10f, 5f);
      RowDefinition rowDefinition1 = new RowDefinition();
      grid3.RowDefinitions.Add(rowDefinition1);
      RowDefinition rowDefinition2 = new RowDefinition();
      grid3.RowDefinitions.Add(rowDefinition2);
      RowDefinition rowDefinition3 = new RowDefinition();
      grid3.RowDefinitions.Add(rowDefinition3);
      RowDefinition rowDefinition4 = new RowDefinition();
      grid3.RowDefinitions.Add(rowDefinition4);
      ColumnDefinition columnDefinition1 = new ColumnDefinition();
      grid3.ColumnDefinitions.Add(columnDefinition1);
      ColumnDefinition columnDefinition2 = new ColumnDefinition();
      grid3.ColumnDefinitions.Add(columnDefinition2);
      Grid.SetRow((UIElement) grid3, 1);
      TextBlock textBlock2 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock2);
      textBlock2.Name = "e_131";
      textBlock2.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock2.VerticalAlignment = VerticalAlignment.Center;
      textBlock2.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock2, 0);
      Grid.SetRow((UIElement) textBlock2, 0);
      textBlock2.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Tooltip_Currency");
      StackPanel stackPanel = new StackPanel();
      grid3.Children.Add((UIElement) stackPanel);
      stackPanel.Name = "e_132";
      stackPanel.Margin = new Thickness(2f, 2f, 2f, 2f);
      stackPanel.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel, 1);
      Grid.SetRow((UIElement) stackPanel, 0);
      TextBlock textBlock3 = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock3);
      textBlock3.Name = "e_133";
      textBlock3.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock3.VerticalAlignment = VerticalAlignment.Center;
      textBlock3.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock3, 1);
      Grid.SetRow((UIElement) textBlock3, 0);
      textBlock3.SetBinding(TextBlock.TextProperty, new Binding("RewardMoney_Formatted")
      {
        UseGeneratedBindings = true
      });
      Image image3 = new Image();
      stackPanel.Children.Add((UIElement) image3);
      image3.Name = "e_134";
      image3.Height = 20f;
      image3.Margin = new Thickness(4f, 2f, 2f, 2f);
      image3.HorizontalAlignment = HorizontalAlignment.Left;
      image3.VerticalAlignment = VerticalAlignment.Center;
      image3.Stretch = Stretch.Uniform;
      Grid.SetColumn((UIElement) image3, 2);
      image3.SetBinding(Image.SourceProperty, new Binding("CurrencyIcon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock4 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock4);
      textBlock4.Name = "e_135";
      textBlock4.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock4.VerticalAlignment = VerticalAlignment.Center;
      textBlock4.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock4, 0);
      Grid.SetRow((UIElement) textBlock4, 1);
      textBlock4.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Tooltip_Reputation");
      TextBlock textBlock5 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock5);
      textBlock5.Name = "e_136";
      textBlock5.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock5.VerticalAlignment = VerticalAlignment.Center;
      textBlock5.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock5, 1);
      Grid.SetRow((UIElement) textBlock5, 1);
      textBlock5.SetBinding(TextBlock.TextProperty, new Binding("RewardReputation")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock6 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock6);
      textBlock6.Name = "e_137";
      textBlock6.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock6.VerticalAlignment = VerticalAlignment.Center;
      textBlock6.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock6, 0);
      Grid.SetRow((UIElement) textBlock6, 2);
      textBlock6.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Tooltip_FailReputationPenalty");
      TextBlock textBlock7 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock7);
      textBlock7.Name = "e_138";
      textBlock7.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock7.VerticalAlignment = VerticalAlignment.Center;
      textBlock7.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock7, 1);
      Grid.SetRow((UIElement) textBlock7, 2);
      textBlock7.SetBinding(TextBlock.TextProperty, new Binding("FailReputationPenalty_Formated")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock8 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock8);
      textBlock8.Name = "e_139";
      textBlock8.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock8.VerticalAlignment = VerticalAlignment.Center;
      textBlock8.Foreground = (Brush) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock8, 0);
      Grid.SetRow((UIElement) textBlock8, 3);
      textBlock8.SetResourceReference(TextBlock.TextProperty, (object) "ContractScreen_Tooltip_Duration");
      TextBlock textBlock9 = new TextBlock();
      grid3.Children.Add((UIElement) textBlock9);
      textBlock9.Name = "e_140";
      textBlock9.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock9.VerticalAlignment = VerticalAlignment.Center;
      textBlock9.Foreground = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
      Grid.SetColumn((UIElement) textBlock9, 1);
      Grid.SetRow((UIElement) textBlock9, 3);
      textBlock9.SetBinding(TextBlock.TextProperty, new Binding("TimeLeft")
      {
        UseGeneratedBindings = true
      });
      ItemsControl itemsControl = new ItemsControl();
      grid1.Children.Add((UIElement) itemsControl);
      itemsControl.Name = "e_141";
      itemsControl.Margin = new Thickness(10f, 0.0f, 10f, 0.0f);
      Grid.SetRow((UIElement) itemsControl, 2);
      itemsControl.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("Conditions")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock10 = new TextBlock();
      grid1.Children.Add((UIElement) textBlock10);
      textBlock10.Name = "e_142";
      textBlock10.UseLayoutRounding = true;
      textBlock10.Margin = new Thickness(10f, 0.0f, 10f, 0.0f);
      textBlock10.TextAlignment = TextAlignment.Left;
      textBlock10.TextWrapping = TextWrapping.Wrap;
      Grid.SetRow((UIElement) textBlock10, 3);
      textBlock10.SetBinding(TextBlock.TextProperty, new Binding("Description")
      {
        UseGeneratedBindings = true
      });
      return (UIElement) grid1;
    }

    private static UIElement r_9_dtMethod(UIElement parent)
    {
      StackPanel stackPanel = new StackPanel();
      stackPanel.Parent = parent;
      stackPanel.Name = "e_143";
      stackPanel.Margin = new Thickness(5f, 5f, 5f, 5f);
      stackPanel.Orientation = Orientation.Horizontal;
      Image image = new Image();
      stackPanel.Children.Add((UIElement) image);
      image.Name = "e_144";
      image.Width = 24f;
      image.Margin = new Thickness(0.0f, 0.0f, 5f, 0.0f);
      image.VerticalAlignment = VerticalAlignment.Center;
      image.Stretch = Stretch.Uniform;
      image.SetBinding(Image.SourceProperty, new Binding("Icon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock);
      textBlock.Name = "e_145";
      textBlock.VerticalAlignment = VerticalAlignment.Center;
      textBlock.SetBinding(TextBlock.TextProperty, new Binding("LocalizedName")
      {
        UseGeneratedBindings = true
      });
      return (UIElement) stackPanel;
    }

    private static UIElement r_10_dtMethod(UIElement parent)
    {
      TextBlock textBlock = new TextBlock();
      textBlock.Parent = parent;
      textBlock.Name = "e_146";
      textBlock.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock.SetBinding(TextBlock.TextProperty, new Binding("Name")
      {
        UseGeneratedBindings = true
      });
      return (UIElement) textBlock;
    }

    private static UIElement r_11_dtMethod(UIElement parent)
    {
      StackPanel stackPanel = new StackPanel();
      stackPanel.Parent = parent;
      stackPanel.Name = "e_147";
      stackPanel.Orientation = Orientation.Horizontal;
      Image image = new Image();
      stackPanel.Children.Add((UIElement) image);
      image.Name = "e_148";
      image.Width = 24f;
      image.Margin = new Thickness(2f, 2f, 2f, 2f);
      image.VerticalAlignment = VerticalAlignment.Center;
      image.Stretch = Stretch.Uniform;
      image.SetBinding(Image.SourceProperty, new Binding("Icon")
      {
        UseGeneratedBindings = true
      });
      TextBlock textBlock = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock);
      textBlock.Name = "e_149";
      textBlock.VerticalAlignment = VerticalAlignment.Center;
      textBlock.SetBinding(TextBlock.TextProperty, new Binding("Name")
      {
        UseGeneratedBindings = true
      });
      return (UIElement) stackPanel;
    }

    private static UIElement r_12_dtMethod(UIElement parent)
    {
      TextBlock textBlock = new TextBlock();
      textBlock.Parent = parent;
      textBlock.Name = "e_150";
      textBlock.VerticalAlignment = VerticalAlignment.Center;
      textBlock.SetBinding(TextBlock.TextProperty, new Binding("DisplayName")
      {
        UseGeneratedBindings = true
      });
      return (UIElement) textBlock;
    }

    private static UIElement r_13_dtMethod(UIElement parent)
    {
      StackPanel stackPanel = new StackPanel();
      stackPanel.Parent = parent;
      stackPanel.Name = "e_151";
      stackPanel.Orientation = Orientation.Horizontal;
      TextBlock textBlock = new TextBlock();
      stackPanel.Children.Add((UIElement) textBlock);
      textBlock.Name = "e_152";
      textBlock.Margin = new Thickness(2f, 2f, 2f, 2f);
      textBlock.VerticalAlignment = VerticalAlignment.Center;
      Binding binding = new Binding("DisplayName");
      textBlock.SetBinding(TextBlock.TextProperty, binding);
      return (UIElement) stackPanel;
    }
  }
}
