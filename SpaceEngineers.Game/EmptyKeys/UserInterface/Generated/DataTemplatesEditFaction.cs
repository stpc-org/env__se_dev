// Decompiled with JetBrains decompiler
// Type: EmptyKeys.UserInterface.Generated.DataTemplatesEditFaction
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Generated.DataTemplatesEditFaction_Bindings;
using EmptyKeys.UserInterface.Media;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.Screens.ViewModels;
using System;
using System.CodeDom.Compiler;

namespace EmptyKeys.UserInterface.Generated
{
  [GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
  public sealed class DataTemplatesEditFaction : ResourceDictionary
  {
    private static DataTemplatesEditFaction singleton = new DataTemplatesEditFaction();

    public DataTemplatesEditFaction() => this.InitializeResources();

    public static DataTemplatesEditFaction Instance => DataTemplatesEditFaction.singleton;

    private void InitializeResources()
    {
      this.MergedDictionaries.Add((ResourceDictionary) Styles.Instance);
      this.Add((object) typeof (MyFactionIconModel), (object) new DataTemplate((object) typeof (MyFactionIconModel), new Func<UIElement, UIElement>(DataTemplatesEditFaction.r_0_dtMethod)));
      this.Add((object) "EditFactionIconViewModelLocator", (object) new MyEditFactionIconViewModelLocator());
      Style style = new Style(typeof (ListBox));
      Setter setter1 = new Setter(UIElement.MinHeightProperty, (object) 80f);
      style.Setters.Add(setter1);
      ControlTemplate controlTemplate = new ControlTemplate(typeof (ListBox), new Func<UIElement, UIElement>(DataTemplatesEditFaction.r_2_s_S_1_ctMethod));
      Setter setter2 = new Setter(Control.TemplateProperty, (object) controlTemplate);
      style.Setters.Add(setter2);
      Trigger trigger = new Trigger();
      trigger.Property = UIElement.IsFocusedProperty;
      trigger.Value = (object) true;
      Setter setter3 = new Setter(Control.BackgroundProperty, (object) new SolidColorBrush(new ColorW(67, 81, 92, 160)));
      trigger.Setters.Add(setter3);
      style.Triggers.Add((TriggerBase) trigger);
      this.Add((object) "ListBoxFactionIconItem", (object) style);
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyFactionIconModel), "IsEnabled", typeof (MyFactionIconModel_IsEnabled_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyFactionIconModel), "TooltipText", typeof (MyFactionIconModel_TooltipText_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyFactionIconModel), "Opacity", typeof (MyFactionIconModel_Opacity_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyFactionIconModel), "Icon", typeof (MyFactionIconModel_Icon_PropertyInfo));
      GeneratedPropertyInfo.RegisterGeneratedProperty(typeof (MyFactionIconModel), "IconColor", typeof (MyFactionIconModel_IconColor_PropertyInfo));
    }

    private static UIElement r_0_dtMethod(UIElement parent)
    {
      Grid grid = new Grid();
      grid.Parent = parent;
      grid.Name = "e_0";
      grid.Height = 58f;
      grid.Width = 58f;
      grid.SetBinding(UIElement.IsEnabledProperty, new Binding("IsEnabled")
      {
        UseGeneratedBindings = true
      });
      Border border = new Border();
      grid.Children.Add((UIElement) border);
      border.Name = "e_1";
      border.Margin = new Thickness(2f, 2f, 2f, 2f);
      border.BorderBrush = (Brush) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue));
      border.BorderThickness = new Thickness(1f, 1f, 1f, 1f);
      Image image = new Image();
      border.Child = (UIElement) image;
      image.Name = "e_2";
      image.Stretch = Stretch.Fill;
      image.SetBinding(UIElement.ToolTipProperty, new Binding("TooltipText")
      {
        UseGeneratedBindings = true
      });
      image.SetBinding(UIElement.OpacityProperty, new Binding("Opacity")
      {
        UseGeneratedBindings = true
      });
      image.SetBinding(Image.SourceProperty, new Binding("Icon")
      {
        UseGeneratedBindings = true
      });
      image.SetBinding(ImageBrush.ColorOverlayProperty, new Binding("IconColor")
      {
        UseGeneratedBindings = true
      });
      return (UIElement) grid;
    }

    private static UIElement r_2_s_S_1_ctMethod(UIElement parent)
    {
      Border border = new Border();
      border.Parent = parent;
      border.Name = "e_3";
      border.SnapsToDevicePixels = true;
      border.SetBinding(Control.BorderThicknessProperty, new Binding("BorderThickness")
      {
        Source = (object) parent
      });
      border.SetBinding(Control.BorderBrushProperty, new Binding("BorderBrush")
      {
        Source = (object) parent
      });
      border.SetBinding(Control.BackgroundProperty, new Binding("Background")
      {
        Source = (object) parent
      });
      ScrollViewer scrollViewer = new ScrollViewer();
      border.Child = (UIElement) scrollViewer;
      scrollViewer.Name = "PART_DataScrollViewer";
      scrollViewer.HorizontalContentAlignment = HorizontalAlignment.Center;
      scrollViewer.VerticalContentAlignment = VerticalAlignment.Top;
      UniformGrid uniformGrid = new UniformGrid();
      scrollViewer.Content = (object) uniformGrid;
      uniformGrid.Name = "e_4";
      uniformGrid.Margin = new Thickness(6f, 6f, 6f, 6f);
      uniformGrid.IsItemsHost = true;
      uniformGrid.Columns = 6;
      return (UIElement) border;
    }
  }
}
