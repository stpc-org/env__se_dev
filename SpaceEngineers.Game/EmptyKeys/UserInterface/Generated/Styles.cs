// Decompiled with JetBrains decompiler
// Type: EmptyKeys.UserInterface.Generated.Styles
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Controls.Primitives;
using EmptyKeys.UserInterface.Data;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Media.Imaging;
using System;
using System.CodeDom.Compiler;

namespace EmptyKeys.UserInterface.Generated
{
  [GeneratedCode("Empty Keys UI Generator", "3.2.0.0")]
  public sealed class Styles : ResourceDictionary
  {
    private static Styles singleton = new Styles();

    public Styles() => this.InitializeResources();

    public static Styles Instance => Styles.singleton;

    private void InitializeResources()
    {
      this.Add((object) "AcquisitionContract", (object) new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Icons\\Contracts\\AcquisitionContractHeader.dds"
      });
      this.Add((object) "Background16x9", (object) new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Bg16x9.png"
      });
      this.Add((object) "BountyContract", (object) new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Icons\\Contracts\\BountyContractHeader.dds"
      });
      this.Add((object) "ButtonArrowLeft", (object) new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_arrow_left.dds"
      });
      this.Add((object) "ButtonArrowLeftHighlight", (object) new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_arrow_left_highlight.dds"
      });
      this.Add((object) "ButtonArrowRight", (object) new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_arrow_right.dds"
      });
      this.Add((object) "ButtonArrowRightHighlight", (object) new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_arrow_right_highlight.dds"
      });
      this.Add((object) "ButtonBackgroundBrush", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\button_default.dds"
        }
      });
      this.Add((object) "ButtonBackgroundDisabledBrush", (object) new SolidColorBrush(new ColorW(29, 39, 45, (int) byte.MaxValue)));
      this.Add((object) "ButtonBackgroundFocusedBrush", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\button_default_focus.dds"
        }
      });
      this.Add((object) "ButtonBackgroundHoverBrush", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\button_default_highlight.dds"
        }
      });
      this.Add((object) "ButtonBackgroundPressedBrush", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\button_default_active.dds"
        }
      });
      this.Add((object) "ButtonDecrease", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\button_decrease.dds"
        }
      });
      this.Add((object) "ButtonDecreaseHover", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\button_decrease_highlight.dds"
        }
      });
      this.Add((object) "ButtonIncrease", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\button_increase.dds"
        }
      });
      this.Add((object) "ButtonIncreaseHover", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\button_increase_highlight.dds"
        }
      });
      this.Add((object) "ButtonTextColor", (object) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue)));
      this.Add((object) "ButtonTextDisabledColor", (object) new SolidColorBrush(new ColorW(93, 105, 110, (int) byte.MaxValue)));
      this.Add((object) "ButtonTextFocusedColor", (object) new SolidColorBrush(new ColorW(33, 40, 45, (int) byte.MaxValue)));
      this.Add((object) "ButtonTextHoverColor", (object) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue)));
      this.Add((object) "ButtonTextPressedColor", (object) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue)));
      this.Add((object) "CheckBoxBackgroundBrush", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\checkbox_unchecked.dds"
        }
      });
      this.Add((object) "CheckBoxFocusedBackgroundBrush", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\checkbox_unchecked_focus.dds"
        }
      });
      this.Add((object) "CheckBoxHoverBackgroundBrush", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\checkbox_unchecked_highlight.dds"
        }
      });
      Style style1 = new Style(typeof (CheckBox));
      ControlTemplate controlTemplate1 = new ControlTemplate(typeof (CheckBox), new Func<UIElement, UIElement>(Styles.r_24_s_S_0_ctMethod));
      Trigger trigger1 = new Trigger();
      trigger1.Property = UIElement.IsMouseOverProperty;
      trigger1.Value = (object) true;
      trigger1.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "CheckServiceBackHighlighted"))
      {
        TargetName = "PART_Background"
      });
      controlTemplate1.Triggers.Add((TriggerBase) trigger1);
      Trigger trigger2 = new Trigger();
      trigger2.Property = UIElement.IsFocusedProperty;
      trigger2.Value = (object) true;
      trigger2.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "CheckServiceBackFocused"))
      {
        TargetName = "PART_Background"
      });
      trigger2.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "CheckService0Focused"))
      {
        TargetName = "PART_Icon"
      });
      controlTemplate1.Triggers.Add((TriggerBase) trigger2);
      Trigger trigger3 = new Trigger();
      trigger3.Property = ToggleButton.IsCheckedProperty;
      trigger3.Value = (object) true;
      trigger3.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "CheckServiceBackChecked"))
      {
        TargetName = "PART_Background"
      });
      controlTemplate1.Triggers.Add((TriggerBase) trigger3);
      Setter setter1 = new Setter(Control.TemplateProperty, (object) controlTemplate1);
      style1.Setters.Add(setter1);
      this.Add((object) "CheckBoxServices0", (object) style1);
      Style style2 = new Style(typeof (CheckBox));
      ControlTemplate controlTemplate2 = new ControlTemplate(typeof (CheckBox), new Func<UIElement, UIElement>(Styles.r_25_s_S_0_ctMethod));
      Trigger trigger4 = new Trigger();
      trigger4.Property = UIElement.IsMouseOverProperty;
      trigger4.Value = (object) true;
      trigger4.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "CheckServiceBackHighlighted"))
      {
        TargetName = "PART_Background"
      });
      controlTemplate2.Triggers.Add((TriggerBase) trigger4);
      Trigger trigger5 = new Trigger();
      trigger5.Property = UIElement.IsFocusedProperty;
      trigger5.Value = (object) true;
      trigger5.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Icons\\Browser\\BackgroundFocused.png"
        }
      })
      {
        TargetName = "PART_Background"
      });
      trigger5.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Icons\\Browser\\ModioCBFocused.png"
        }
      })
      {
        TargetName = "PART_Icon"
      });
      controlTemplate2.Triggers.Add((TriggerBase) trigger5);
      Trigger trigger6 = new Trigger();
      trigger6.Property = ToggleButton.IsCheckedProperty;
      trigger6.Value = (object) true;
      trigger6.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "CheckServiceBackChecked"))
      {
        TargetName = "PART_Background"
      });
      controlTemplate2.Triggers.Add((TriggerBase) trigger6);
      Setter setter2 = new Setter(Control.TemplateProperty, (object) controlTemplate2);
      style2.Setters.Add(setter2);
      this.Add((object) "CheckBoxServices1", (object) style2);
      this.Add((object) "CheckImageBrush", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\checkbox_checked.dds"
        }
      });
      this.Add((object) "CheckService0", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Icons\\Browser\\SteamCB.png"
        }
      });
      this.Add((object) "CheckService0Focused", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Icons\\Browser\\SteamCBFocused.png"
        }
      });
      this.Add((object) "CheckService1", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Icons\\Browser\\ModioCB.png"
        }
      });
      this.Add((object) "CheckService1Focused", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Icons\\Browser\\ModioCBFocused.png"
        }
      });
      this.Add((object) "CheckServiceBack", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Icons\\Browser\\Background.png"
        }
      });
      this.Add((object) "CheckServiceBackChecked", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Icons\\Browser\\BackgroundChecked.png"
        }
      });
      this.Add((object) "CheckServiceBackFocused", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Icons\\Browser\\BackgroundFocused.png"
        }
      });
      this.Add((object) "CheckServiceBackHighlighted", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Icons\\Browser\\BackgroundHighlighted.png"
        }
      });
      this.Add((object) "CloseScreenButtonIcon", (object) new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol.dds"
      });
      this.Add((object) "CloseScreenButtonIconHover", (object) new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds"
      });
      this.Add((object) "ComboBoxBackgroundCenter", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\combobox_default_center.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "ComboBoxBackgroundCenterFocus", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\combobox_default_focus_center.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "ComboBoxBackgroundCenterHighlight", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\combobox_default_highlight_center.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "ComboBoxBackgroundLeft", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\combobox_default_left.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "ComboBoxBackgroundLeftFocus", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\combobox_default_focus_left.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "ComboBoxBackgroundLeftHighlight", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\combobox_default_highlight_left.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "ComboBoxBackgroundRight", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\combobox_default_right.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "ComboBoxBackgroundRightFocus", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\combobox_default_focus_right.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "ComboBoxBackgroundRightHighlight", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\combobox_default_highlight_right.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "DataGridHeaderBackground", (object) new SolidColorBrush(new ColorW(33, 44, 53, (int) byte.MaxValue)));
      this.Add((object) "DataGridHeaderBackgroundPressed", (object) new SolidColorBrush(new ColorW(63, 71, 79, (int) byte.MaxValue)));
      this.Add((object) "DataGridHeaderForeground", (object) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue)));
      this.Add((object) "DataGridHeaderForegroundPressed", (object) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue)));
      this.Add((object) "DataGridRowBackgroundSelected", (object) new SolidColorBrush(new ColorW(91, 115, 123, (int) byte.MaxValue)));
      this.Add((object) "DataGridRowForeground", (object) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue)));
      this.Add((object) "DataGridRowForegroundPressed", (object) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue)));
      this.Add((object) "DiscountPrice", (object) new SolidColorBrush(new ColorW(198, 44, 20, (int) byte.MaxValue)));
      Style style3 = new Style(typeof (NumericTextBox));
      Setter setter3 = new Setter(TextBoxBase.CaretBrushProperty, (object) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue)));
      style3.Setters.Add(setter3);
      Setter setter4 = new Setter(UIElement.SnapsToDevicePixelsProperty, (object) true);
      style3.Setters.Add(setter4);
      Setter setter5 = new Setter(TextBoxBase.SelectionBrushProperty, (object) new SolidColorBrush(new ColorW(63, 71, 79, (int) byte.MaxValue)));
      style3.Setters.Add(setter5);
      Setter setter6 = new Setter(TextBoxBase.TextAlignmentProperty, (object) TextAlignment.Right);
      style3.Setters.Add(setter6);
      Setter setter7 = new Setter(Control.HorizontalContentAlignmentProperty, (object) HorizontalAlignment.Right);
      style3.Setters.Add(setter7);
      ControlTemplate controlTemplate3 = new ControlTemplate(typeof (NumericTextBox), new Func<UIElement, UIElement>(Styles.r_54_s_S_5_ctMethod));
      Trigger trigger7 = new Trigger();
      trigger7.Property = UIElement.IsMouseOverProperty;
      trigger7.Value = (object) true;
      trigger7.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "TextBoxLeftHighlight"))
      {
        TargetName = "PART_TextBoxLeft"
      });
      trigger7.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "TextBoxCenterHighlight"))
      {
        TargetName = "PART_TextBoxCenter"
      });
      trigger7.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "TextBoxRightHighlight"))
      {
        TargetName = "PART_TextBoxRight"
      });
      Setter setter8 = new Setter(Control.ForegroundProperty, (object) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue)));
      trigger7.Setters.Add(setter8);
      controlTemplate3.Triggers.Add((TriggerBase) trigger7);
      Trigger trigger8 = new Trigger();
      trigger8.Property = UIElement.IsFocusedProperty;
      trigger8.Value = (object) true;
      trigger8.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "TextBoxLeftFocus"))
      {
        TargetName = "PART_TextBoxLeft"
      });
      trigger8.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "TextBoxCenterFocus"))
      {
        TargetName = "PART_TextBoxCenter"
      });
      trigger8.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "TextBoxRightFocus"))
      {
        TargetName = "PART_TextBoxRight"
      });
      Setter setter9 = new Setter(Control.ForegroundProperty, (object) new SolidColorBrush(new ColorW(33, 40, 45, (int) byte.MaxValue)));
      trigger8.Setters.Add(setter9);
      controlTemplate3.Triggers.Add((TriggerBase) trigger8);
      Setter setter10 = new Setter(Control.TemplateProperty, (object) controlTemplate3);
      style3.Setters.Add(setter10);
      this.Add((object) typeof (NumericTextBox), (object) style3);
      this.Add((object) "EscortContract", (object) new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Icons\\Contracts\\EscortContractHeader.dds"
      });
      this.Add((object) "ExclamationMark", (object) new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Icons\\HUD 2017\\Notification_badge.png"
      });
      this.Add((object) "FullStar", (object) new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Icons\\Rating\\FullStar.png"
      });
      this.Add((object) "GridItem", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\grid_item.dds"
        }
      });
      this.Add((object) "GridItemHover", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\grid_item_highlight.dds"
        }
      });
      this.Add((object) "HalfStar", (object) new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Icons\\Rating\\HalfStar.png"
      });
      this.Add((object) "HaulingContract", (object) new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Icons\\Contracts\\HaulingContractHeader.dds"
      });
      ControlTemplate controlTemplate4 = new ControlTemplate(typeof (Slider), new Func<UIElement, UIElement>(Styles.r_62_ctMethod));
      Trigger trigger9 = new Trigger();
      trigger9.Property = UIElement.IsMouseOverProperty;
      trigger9.Value = (object) true;
      trigger9.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "SliderRailLeftHighlight"))
      {
        TargetName = "PART_SliderRailLeft"
      });
      trigger9.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "SliderRailCenterHighlight"))
      {
        TargetName = "PART_SliderRailCenter"
      });
      trigger9.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "SliderRailRightHighlight"))
      {
        TargetName = "PART_SliderRailRight"
      });
      controlTemplate4.Triggers.Add((TriggerBase) trigger9);
      Trigger trigger10 = new Trigger();
      trigger10.Property = UIElement.IsFocusedProperty;
      trigger10.Value = (object) true;
      trigger10.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "SliderRailLeftFocus"))
      {
        TargetName = "PART_SliderRailLeft"
      });
      trigger10.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "SliderRailCenterFocus"))
      {
        TargetName = "PART_SliderRailCenter"
      });
      trigger10.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "SliderRailRightFocus"))
      {
        TargetName = "PART_SliderRailRight"
      });
      controlTemplate4.Triggers.Add((TriggerBase) trigger10);
      this.Add((object) "HorizontalSlider", (object) controlTemplate4);
      ControlTemplate controlTemplate5 = new ControlTemplate(typeof (Slider), new Func<UIElement, UIElement>(Styles.r_63_ctMethod));
      Trigger trigger11 = new Trigger();
      trigger11.Property = UIElement.IsMouseOverProperty;
      trigger11.Value = (object) true;
      trigger11.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "HueColorGradientLeftHighlight"))
      {
        TargetName = "PART_SliderRailLeft"
      });
      trigger11.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "HueColorGradientCenterHighlight"))
      {
        TargetName = "PART_SliderRailCenter"
      });
      trigger11.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "HueColorGradientRightHighlight"))
      {
        TargetName = "PART_SliderRailRight"
      });
      controlTemplate5.Triggers.Add((TriggerBase) trigger11);
      Trigger trigger12 = new Trigger();
      trigger12.Property = UIElement.IsFocusedProperty;
      trigger12.Value = (object) true;
      trigger12.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "HueColorGradientLeftHighlight"))
      {
        TargetName = "PART_SliderRailLeft"
      });
      trigger12.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "HueColorGradientCenterHighlight"))
      {
        TargetName = "PART_SliderRailCenter"
      });
      trigger12.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "HueColorGradientRightHighlight"))
      {
        TargetName = "PART_SliderRailRight"
      });
      controlTemplate5.Triggers.Add((TriggerBase) trigger12);
      this.Add((object) "HorizontalSliderHuePicker", (object) controlTemplate5);
      Style style4 = new Style(typeof (Thumb));
      ControlTemplate controlTemplate6 = new ControlTemplate(typeof (Thumb), new Func<UIElement, UIElement>(Styles.r_64_s_S_0_ctMethod));
      Trigger trigger13 = new Trigger();
      trigger13.Property = UIElement.IsMouseOverProperty;
      trigger13.Value = (object) true;
      trigger13.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "ScrollbarHorizontalThumbCenterHighlight"))
      {
        TargetName = "PART_Center"
      });
      controlTemplate6.Triggers.Add((TriggerBase) trigger13);
      Setter setter11 = new Setter(Control.TemplateProperty, (object) controlTemplate6);
      style4.Setters.Add(setter11);
      this.Add((object) "HorizontalThumb", (object) style4);
      this.Add((object) "HueColorGradientCenter", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\hue_slider_rail_center.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "HueColorGradientCenterHighlight", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\hue_slider_rail_center_highlight.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "HueColorGradientLeft", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\hue_slider_rail_left.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "HueColorGradientLeftHighlight", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\hue_slider_rail_left_highlight.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "HueColorGradientRight", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\hue_slider_rail_right.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "HueColorGradientRightHighlight", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\hue_slider_rail_right_highlight.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "HydrogenIcon", (object) new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Icons\\HydrogenIcon.dds"
      });
      this.Add((object) "IconColor", (object) new ColorW(131, 201, 226, (int) byte.MaxValue));
      this.Add((object) "InventoryBackground", (object) new SolidColorBrush(new ColorW(33, 44, 53, (int) byte.MaxValue)));
      this.Add((object) "ItemBackgroundHoverBrush", (object) new SolidColorBrush(new ColorW(60, 76, 82, (int) byte.MaxValue)));
      this.Add((object) "ItemSelectedBrush", (object) new SolidColorBrush(new ColorW(91, 115, 123, (int) byte.MaxValue)));
      this.Add((object) "ItemTextColor", (object) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue)));
      this.Add((object) "ItemTextHoverColor", (object) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue)));
      this.Add((object) "ItemTextSelectedColor", (object) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue)));
      this.Add((object) "LighterBackground", (object) new SolidColorBrush(new ColorW(41, 54, 62, (int) byte.MaxValue)));
      Style style5 = new Style(typeof (ListBox));
      Setter setter12 = new Setter(UIElement.MinHeightProperty, (object) 80f);
      style5.Setters.Add(setter12);
      ControlTemplate controlTemplate7 = new ControlTemplate(typeof (ListBox), new Func<UIElement, UIElement>(Styles.r_80_s_S_1_ctMethod));
      Setter setter13 = new Setter(Control.TemplateProperty, (object) controlTemplate7);
      style5.Setters.Add(setter13);
      Trigger trigger14 = new Trigger();
      trigger14.Property = UIElement.IsFocusedProperty;
      trigger14.Value = (object) true;
      Setter setter14 = new Setter(Control.BackgroundProperty, (object) new SolidColorBrush(new ColorW(67, 81, 92, 160)));
      trigger14.Setters.Add(setter14);
      style5.Triggers.Add((TriggerBase) trigger14);
      this.Add((object) "ListBoxGrid", (object) style5);
      Style style6 = new Style(typeof (ListBox));
      Setter setter15 = new Setter(UIElement.MinHeightProperty, (object) 80f);
      style6.Setters.Add(setter15);
      Trigger trigger15 = new Trigger();
      trigger15.Property = UIElement.IsFocusedProperty;
      trigger15.Value = (object) true;
      Setter setter16 = new Setter(Control.BackgroundProperty, (object) new SolidColorBrush(new ColorW(67, 81, 92, 160)));
      trigger15.Setters.Add(setter16);
      style6.Triggers.Add((TriggerBase) trigger15);
      this.Add((object) "ListBoxStandard", (object) style6);
      Style style7 = new Style(typeof (ListBox));
      Setter setter17 = new Setter(UIElement.MinHeightProperty, (object) 80f);
      style7.Setters.Add(setter17);
      ControlTemplate controlTemplate8 = new ControlTemplate(typeof (ListBox), new Func<UIElement, UIElement>(Styles.r_82_s_S_1_ctMethod));
      Setter setter18 = new Setter(Control.TemplateProperty, (object) controlTemplate8);
      style7.Setters.Add(setter18);
      Trigger trigger16 = new Trigger();
      trigger16.Property = UIElement.IsFocusedProperty;
      trigger16.Value = (object) true;
      Setter setter19 = new Setter(Control.BackgroundProperty, (object) new SolidColorBrush(new ColorW(67, 81, 92, 160)));
      trigger16.Setters.Add(setter19);
      style7.Triggers.Add((TriggerBase) trigger16);
      this.Add((object) "ListBoxWrapPanel", (object) style7);
      Style style8 = new Style(typeof (ListBox));
      Setter setter20 = new Setter(UIElement.MinHeightProperty, (object) 80f);
      style8.Setters.Add(setter20);
      ControlTemplate controlTemplate9 = new ControlTemplate(typeof (ListBox), new Func<UIElement, UIElement>(Styles.r_83_s_S_1_ctMethod));
      Setter setter21 = new Setter(Control.TemplateProperty, (object) controlTemplate9);
      style8.Setters.Add(setter21);
      Trigger trigger17 = new Trigger();
      trigger17.Property = UIElement.IsFocusedProperty;
      trigger17.Value = (object) true;
      Setter setter22 = new Setter(Control.BackgroundProperty, (object) new SolidColorBrush(new ColorW(67, 81, 92, 160)));
      trigger17.Setters.Add(setter22);
      style8.Triggers.Add((TriggerBase) trigger17);
      this.Add((object) "ListBoxWrapPanelWorkshopBrowser", (object) style8);
      this.Add((object) "LoadingIconAnimated", (object) new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\LoadingIconAnimated.png"
      });
      this.Add((object) "MessageBackgroundBrush", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Screens\\message_background_red.dds"
        },
        Stretch = Stretch.Fill
      });
      Style style9 = new Style(typeof (Window));
      Setter setter23 = new Setter(Control.ForegroundProperty, (object) new SolidColorBrush(new ColorW((int) byte.MaxValue, 0, 0, (int) byte.MaxValue)));
      style9.Setters.Add(setter23);
      ControlTemplate controlTemplate10 = new ControlTemplate(typeof (Window), new Func<UIElement, UIElement>(Styles.r_86_s_S_1_ctMethod));
      Setter setter24 = new Setter(Control.TemplateProperty, (object) controlTemplate10);
      style9.Setters.Add(setter24);
      this.Add((object) "MessageBoxWindowStyle", (object) style9);
      this.Add((object) "NoStar", (object) new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Icons\\Rating\\NoStar.png"
      });
      this.Add((object) "OxygenIcon", (object) new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Icons\\OxygenIcon.dds"
      });
      this.Add((object) "ProgressBarBackgroundBrush", (object) new SolidColorBrush(new ColorW(33, 44, 53, (int) byte.MaxValue)));
      this.Add((object) "ProgressBarForegroundColor", (object) new SolidColorBrush(new ColorW(63, 71, 79, (int) byte.MaxValue)));
      this.Add((object) "RedButtonBackgroundBrush", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\button_red.dds"
        }
      });
      this.Add((object) "RedButtonBackgroundBrushHighlight", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\button_red_highlight.dds"
        }
      });
      this.Add((object) "RefreshIcon", (object) new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Icons\\Blueprints\\Refresh.png"
      });
      this.Add((object) "RepairContract", (object) new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Icons\\Contracts\\RepairContractHeader.dds"
      });
      Style style10 = new Style(typeof (RepeatButton));
      Setter setter25 = new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "ButtonDecrease"));
      style10.Setters.Add(setter25);
      ControlTemplate controlTemplate11 = new ControlTemplate(typeof (RepeatButton), new Func<UIElement, UIElement>(Styles.r_95_s_S_1_ctMethod));
      Trigger trigger18 = new Trigger();
      trigger18.Property = UIElement.IsMouseOverProperty;
      trigger18.Value = (object) true;
      Setter setter26 = new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "ButtonDecreaseHover"));
      trigger18.Setters.Add(setter26);
      controlTemplate11.Triggers.Add((TriggerBase) trigger18);
      Setter setter27 = new Setter(Control.TemplateProperty, (object) controlTemplate11);
      style10.Setters.Add(setter27);
      this.Add((object) "RepeatButtonDecrease", (object) style10);
      Style style11 = new Style(typeof (RepeatButton));
      Setter setter28 = new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "ButtonIncrease"));
      style11.Setters.Add(setter28);
      ControlTemplate controlTemplate12 = new ControlTemplate(typeof (RepeatButton), new Func<UIElement, UIElement>(Styles.r_96_s_S_1_ctMethod));
      Trigger trigger19 = new Trigger();
      trigger19.Property = UIElement.IsMouseOverProperty;
      trigger19.Value = (object) true;
      Setter setter29 = new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "ButtonIncreaseHover"));
      trigger19.Setters.Add(setter29);
      controlTemplate12.Triggers.Add((TriggerBase) trigger19);
      Setter setter30 = new Setter(Control.TemplateProperty, (object) controlTemplate12);
      style11.Setters.Add(setter30);
      this.Add((object) "RepeatButtonIncrease", (object) style11);
      this.Add((object) "ReputationGainArrow", (object) new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Contracts\\ArrowRepGain.png"
      });
      this.Add((object) "ReputationLossArrow", (object) new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Contracts\\ArrowRepLoss.png"
      });
      this.Add((object) "ScreenBackground", (object) new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Screens\\screen_background.dds"
      });
      this.Add((object) "ScrollableListCenter", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\scrollable_list_center.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "ScrollableListCenterBottom", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\scrollable_list_center_bottom.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "ScrollableListCenterTop", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\scrollable_list_center_top.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "ScrollableListLeftBottom", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\scrollable_list_left_bottom.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "ScrollableListLeftCenter", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\scrollable_list_left_center.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "ScrollableListLeftTop", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\scrollable_list_left_top.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "ScrollableListRightBottom", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\scrollable_list_right_bottom.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "ScrollableListRightCenter", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\scrollable_list_right_center.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "ScrollableListRightTop", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\scrollable_list_right_top.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "ScrollbarHorizontalThumbCenter", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\scrollbar_h_thumb_center.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "ScrollbarHorizontalThumbCenterHighlight", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\scrollbar_h_thumb_center_highlight.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "ScrollbarHorizontalThumbLeft", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\scrollbar_h_thumb_left.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "ScrollbarHorizontalThumbLeftHighlight", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\scrollbar_h_thumb_left_highlight.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "ScrollbarHorizontalThumbRight", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\scrollbar_h_thumb_right.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "ScrollbarHorizontalThumbRightHighlight", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\scrollbar_h_thumb_right_highlight.dds"
        },
        Stretch = Stretch.Fill
      });
      Style style12 = new Style(typeof (RepeatButton));
      ControlTemplate controlTemplate13 = new ControlTemplate(new Func<UIElement, UIElement>(Styles.r_115_s_S_0_ctMethod));
      Setter setter31 = new Setter(Control.TemplateProperty, (object) controlTemplate13);
      style12.Setters.Add(setter31);
      this.Add((object) "ScrollBarPageButton", (object) style12);
      this.Add((object) "ScrollbarVerticalThumbBottom", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\scrollbar_v_thumb_bottom.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "ScrollbarVerticalThumbBottomHighlight", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\scrollbar_v_thumb_bottom_highlight.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "ScrollbarVerticalThumbCenter", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\scrollbar_v_thumb_center.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "ScrollbarVerticalThumbCenterHighlight", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\scrollbar_v_thumb_center_highlight.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "ScrollbarVerticalThumbTop", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\scrollbar_v_thumb_top.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "ScrollbarVerticalThumbTopHighlight", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\scrollbar_v_thumb_top_highlight.dds"
        },
        Stretch = Stretch.Fill
      });
      Style style13 = new Style(typeof (ScrollViewer));
      Setter setter32 = new Setter(UIElement.SnapsToDevicePixelsProperty, (object) true);
      style13.Setters.Add(setter32);
      ControlTemplate controlTemplate14 = new ControlTemplate(typeof (ScrollViewer), new Func<UIElement, UIElement>(Styles.r_122_s_S_1_ctMethod));
      Setter setter33 = new Setter(Control.TemplateProperty, (object) controlTemplate14);
      style13.Setters.Add(setter33);
      this.Add((object) "ScrollViewerStyle", (object) style13);
      Style style14 = new Style(typeof (ScrollViewer));
      Setter setter34 = new Setter(UIElement.SnapsToDevicePixelsProperty, (object) true);
      style14.Setters.Add(setter34);
      Setter setter35 = new Setter(Control.BorderBrushProperty, (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\button_default.dds"
        }
      });
      style14.Setters.Add(setter35);
      ControlTemplate controlTemplate15 = new ControlTemplate(typeof (ScrollViewer), new Func<UIElement, UIElement>(Styles.r_123_s_S_2_ctMethod));
      Trigger trigger20 = new Trigger();
      trigger20.Property = UIElement.IsFocusedProperty;
      trigger20.Value = (object) true;
      Setter setter36 = new Setter(Control.BorderBrushProperty, (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\button_default_focus.dds"
        }
      });
      trigger20.Setters.Add(setter36);
      controlTemplate15.Triggers.Add((TriggerBase) trigger20);
      Trigger trigger21 = new Trigger();
      trigger21.Property = UIElement.IsFocusedProperty;
      trigger21.Value = (object) false;
      Setter setter37 = new Setter(Control.BorderBrushProperty, (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\button_default.dds"
        }
      });
      trigger21.Setters.Add(setter37);
      controlTemplate15.Triggers.Add((TriggerBase) trigger21);
      Setter setter38 = new Setter(Control.TemplateProperty, (object) controlTemplate15);
      style14.Setters.Add(setter38);
      this.Add((object) "ScrollViewerStyleTextBlock", (object) style14);
      this.Add((object) "SearchContract", (object) new BitmapImage()
      {
        TextureAsset = "Textures\\GUI\\Icons\\Contracts\\SearchContractHeader.dds"
      });
      Style style15 = new Style(typeof (RepeatButton));
      Setter setter39 = new Setter(UIElement.SnapsToDevicePixelsProperty, (object) true);
      style15.Setters.Add(setter39);
      Setter setter40 = new Setter(UIElement.FocusableProperty, (object) false);
      style15.Setters.Add(setter40);
      ControlTemplate controlTemplate16 = new ControlTemplate(typeof (RepeatButton), new Func<UIElement, UIElement>(Styles.r_125_s_S_2_ctMethod));
      Setter setter41 = new Setter(Control.TemplateProperty, (object) controlTemplate16);
      style15.Setters.Add(setter41);
      this.Add((object) "SliderButtonStyle", (object) style15);
      this.Add((object) "SliderRailCenter", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\slider_rail_center.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "SliderRailCenterFocus", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\slider_rail_center_focus.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "SliderRailCenterHighlight", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\slider_rail_center_highlight.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "SliderRailLeft", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\slider_rail_left.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "SliderRailLeftFocus", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\slider_rail_left_focus.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "SliderRailLeftHighlight", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\slider_rail_left_highlight.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "SliderRailRight", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\slider_rail_right.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "SliderRailRightFocus", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\slider_rail_right_focus.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "SliderRailRightHighlight", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\slider_rail_right_highlight.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "SliderThumb", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\slider_thumb.dds"
        },
        Stretch = Stretch.Fill
      });
      this.Add((object) "SliderThumbHighlight", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\slider_thumb_highlight.dds"
        },
        Stretch = Stretch.Fill
      });
      Style style16 = new Style(typeof (Thumb));
      Setter setter42 = new Setter(UIElement.SnapsToDevicePixelsProperty, (object) true);
      style16.Setters.Add(setter42);
      ControlTemplate controlTemplate17 = new ControlTemplate(typeof (Thumb), new Func<UIElement, UIElement>(Styles.r_137_s_S_1_ctMethod));
      Trigger trigger22 = new Trigger();
      trigger22.Property = UIElement.IsMouseOverProperty;
      trigger22.Value = (object) true;
      trigger22.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "SliderThumbHighlight"))
      {
        TargetName = "PART_SliderThumb"
      });
      controlTemplate17.Triggers.Add((TriggerBase) trigger22);
      Setter setter43 = new Setter(Control.TemplateProperty, (object) controlTemplate17);
      style16.Setters.Add(setter43);
      this.Add((object) "SliderThumbStyle", (object) style16);
      Style style17 = new Style(typeof (Slider));
      Setter setter44 = new Setter(UIElement.SnapsToDevicePixelsProperty, (object) false);
      style17.Setters.Add(setter44);
      Setter setter45 = new Setter(Control.TemplateProperty, (object) new ResourceReferenceExpression((object) "HorizontalSliderHuePicker"));
      style17.Setters.Add(setter45);
      this.Add((object) "SliderWithHue", (object) style17);
      Style style18 = new Style(typeof (Button));
      Setter setter46 = new Setter(Control.BackgroundProperty, (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\button_default.dds"
        }
      });
      style18.Setters.Add(setter46);
      Setter setter47 = new Setter(Control.ForegroundProperty, (object) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue)));
      style18.Setters.Add(setter47);
      Setter setter48 = new Setter(Control.HorizontalContentAlignmentProperty, (object) HorizontalAlignment.Center);
      style18.Setters.Add(setter48);
      Setter setter49 = new Setter(Control.VerticalContentAlignmentProperty, (object) VerticalAlignment.Center);
      style18.Setters.Add(setter49);
      Setter setter50 = new Setter(UIElement.HeightProperty, (object) 40f);
      style18.Setters.Add(setter50);
      ControlTemplate controlTemplate18 = new ControlTemplate(typeof (Button), new Func<UIElement, UIElement>(Styles.r_139_s_S_5_ctMethod));
      Trigger trigger23 = new Trigger();
      trigger23.Property = Button.IsPressedProperty;
      trigger23.Value = (object) true;
      Setter setter51 = new Setter(Control.BackgroundProperty, (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\button_default_active.dds"
        }
      });
      trigger23.Setters.Add(setter51);
      Setter setter52 = new Setter(Control.ForegroundProperty, (object) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue)));
      trigger23.Setters.Add(setter52);
      controlTemplate18.Triggers.Add((TriggerBase) trigger23);
      Trigger trigger24 = new Trigger();
      trigger24.Property = UIElement.IsFocusedProperty;
      trigger24.Value = (object) true;
      Setter setter53 = new Setter(Control.BackgroundProperty, (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\button_default_focus.dds"
        }
      });
      trigger24.Setters.Add(setter53);
      Setter setter54 = new Setter(Control.ForegroundProperty, (object) new SolidColorBrush(new ColorW(33, 40, 45, (int) byte.MaxValue)));
      trigger24.Setters.Add(setter54);
      controlTemplate18.Triggers.Add((TriggerBase) trigger24);
      MultiTrigger multiTrigger1 = new MultiTrigger();
      multiTrigger1.Conditions.Add(new TriggerCondition()
      {
        Property = UIElement.IsMouseOverProperty,
        Value = (object) true
      });
      multiTrigger1.Conditions.Add(new TriggerCondition()
      {
        Property = UIElement.IsEnabledProperty,
        Value = (object) true
      });
      Setter setter55 = new Setter(Control.BackgroundProperty, (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\button_default_highlight.dds"
        }
      });
      multiTrigger1.Setters.Add(setter55);
      Setter setter56 = new Setter(Control.ForegroundProperty, (object) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue)));
      multiTrigger1.Setters.Add(setter56);
      controlTemplate18.Triggers.Add((TriggerBase) multiTrigger1);
      Trigger trigger25 = new Trigger();
      trigger25.Property = UIElement.IsEnabledProperty;
      trigger25.Value = (object) false;
      Setter setter57 = new Setter(Control.BackgroundProperty, (object) new SolidColorBrush(new ColorW(29, 39, 45, (int) byte.MaxValue)));
      trigger25.Setters.Add(setter57);
      Setter setter58 = new Setter(Control.ForegroundProperty, (object) new SolidColorBrush(new ColorW(93, 105, 110, (int) byte.MaxValue)));
      trigger25.Setters.Add(setter58);
      controlTemplate18.Triggers.Add((TriggerBase) trigger25);
      Setter setter59 = new Setter(Control.TemplateProperty, (object) controlTemplate18);
      style18.Setters.Add(setter59);
      this.Add((object) typeof (Button), (object) style18);
      Style style19 = new Style(typeof (CheckBox));
      ControlTemplate controlTemplate19 = new ControlTemplate(typeof (CheckBox), new Func<UIElement, UIElement>(Styles.r_140_s_S_0_ctMethod));
      Trigger trigger26 = new Trigger();
      trigger26.Property = ToggleButton.IsCheckedProperty;
      trigger26.Value = (object) true;
      trigger26.Setters.Add(new Setter(UIElement.VisibilityProperty, (object) Visibility.Visible)
      {
        TargetName = "PART_CheckMark"
      });
      controlTemplate19.Triggers.Add((TriggerBase) trigger26);
      Trigger trigger27 = new Trigger();
      trigger27.Property = ToggleButton.IsCheckedProperty;
      trigger27.Value = (object) false;
      trigger27.Setters.Add(new Setter(UIElement.VisibilityProperty, (object) Visibility.Collapsed)
      {
        TargetName = "PART_CheckMark"
      });
      controlTemplate19.Triggers.Add((TriggerBase) trigger27);
      Trigger trigger28 = new Trigger();
      trigger28.Property = UIElement.IsMouseOverProperty;
      trigger28.Value = (object) true;
      trigger28.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "CheckBoxHoverBackgroundBrush"))
      {
        TargetName = "PART_NotChecked"
      });
      controlTemplate19.Triggers.Add((TriggerBase) trigger28);
      Trigger trigger29 = new Trigger();
      trigger29.Property = UIElement.IsFocusedProperty;
      trigger29.Value = (object) true;
      trigger29.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\checkbox_unchecked_focus.dds"
        }
      })
      {
        TargetName = "PART_NotChecked"
      });
      controlTemplate19.Triggers.Add((TriggerBase) trigger29);
      Setter setter60 = new Setter(Control.TemplateProperty, (object) controlTemplate19);
      style19.Setters.Add(setter60);
      this.Add((object) typeof (CheckBox), (object) style19);
      Style style20 = new Style(typeof (ComboBox));
      Setter setter61 = new Setter(Control.ForegroundProperty, (object) new ResourceReferenceExpression((object) "ItemTextColor"));
      style20.Setters.Add(setter61);
      Setter setter62 = new Setter(UIElement.HeightProperty, (object) 38f);
      style20.Setters.Add(setter62);
      Setter setter63 = new Setter(ComboBox.MaxDropDownHeightProperty, (object) 150f);
      style20.Setters.Add(setter63);
      Setter setter64 = new Setter(Control.VerticalContentAlignmentProperty, (object) VerticalAlignment.Center);
      style20.Setters.Add(setter64);
      ControlTemplate controlTemplate20 = new ControlTemplate(typeof (ComboBox), new Func<UIElement, UIElement>(Styles.r_141_s_S_4_ctMethod));
      Trigger trigger30 = new Trigger();
      trigger30.Property = UIElement.IsFocusedProperty;
      trigger30.Value = (object) true;
      trigger30.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "ComboBoxBackgroundLeftFocus"))
      {
        TargetName = "PART_ComboBoxLeft"
      });
      trigger30.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "ComboBoxBackgroundCenterFocus"))
      {
        TargetName = "PART_ComboBoxCenter"
      });
      trigger30.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "ComboBoxBackgroundRightFocus"))
      {
        TargetName = "PART_ComboBoxRight"
      });
      trigger30.Setters.Add(new Setter(Control.ForegroundProperty, (object) new ResourceReferenceExpression((object) "ButtonTextFocusedColor"))
      {
        TargetName = "PART_Button"
      });
      controlTemplate20.Triggers.Add((TriggerBase) trigger30);
      MultiTrigger multiTrigger2 = new MultiTrigger();
      multiTrigger2.Conditions.Add(new TriggerCondition()
      {
        Property = UIElement.IsMouseOverProperty,
        Value = (object) true
      });
      multiTrigger2.Conditions.Add(new TriggerCondition()
      {
        Property = UIElement.IsEnabledProperty,
        Value = (object) true
      });
      multiTrigger2.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "ComboBoxBackgroundLeftHighlight"))
      {
        TargetName = "PART_ComboBoxLeft"
      });
      multiTrigger2.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "ComboBoxBackgroundCenterHighlight"))
      {
        TargetName = "PART_ComboBoxCenter"
      });
      multiTrigger2.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "ComboBoxBackgroundRightHighlight"))
      {
        TargetName = "PART_ComboBoxRight"
      });
      multiTrigger2.Setters.Add(new Setter(Control.ForegroundProperty, (object) new ResourceReferenceExpression((object) "ButtonTextHoverColor"))
      {
        TargetName = "PART_Button"
      });
      controlTemplate20.Triggers.Add((TriggerBase) multiTrigger2);
      Setter setter65 = new Setter(Control.TemplateProperty, (object) controlTemplate20);
      style20.Setters.Add(setter65);
      this.Add((object) typeof (ComboBox), (object) style20);
      Style style21 = new Style(typeof (ComboBoxItem));
      ControlTemplate controlTemplate21 = new ControlTemplate(typeof (ComboBoxItem), new Func<UIElement, UIElement>(Styles.r_142_s_S_0_ctMethod));
      Trigger trigger31 = new Trigger();
      trigger31.Property = ComboBoxItem.IsHighlightedProperty;
      trigger31.Value = (object) true;
      Setter setter66 = new Setter(Control.ForegroundProperty, (object) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue)));
      trigger31.Setters.Add(setter66);
      Setter setter67 = new Setter(Control.BackgroundProperty, (object) new SolidColorBrush(new ColorW(60, 76, 82, (int) byte.MaxValue)));
      trigger31.Setters.Add(setter67);
      controlTemplate21.Triggers.Add((TriggerBase) trigger31);
      Trigger trigger32 = new Trigger();
      trigger32.Property = UIElement.IsMouseOverProperty;
      trigger32.Value = (object) true;
      Setter setter68 = new Setter(Control.ForegroundProperty, (object) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue)));
      trigger32.Setters.Add(setter68);
      Setter setter69 = new Setter(Control.BackgroundProperty, (object) new SolidColorBrush(new ColorW(60, 76, 82, (int) byte.MaxValue)));
      trigger32.Setters.Add(setter69);
      controlTemplate21.Triggers.Add((TriggerBase) trigger32);
      Trigger trigger33 = new Trigger();
      trigger33.Property = UIElement.IsFocusedProperty;
      trigger33.Value = (object) true;
      Setter setter70 = new Setter(Control.ForegroundProperty, (object) new SolidColorBrush(new ColorW(33, 40, 45, (int) byte.MaxValue)));
      trigger33.Setters.Add(setter70);
      Setter setter71 = new Setter(Control.BackgroundProperty, (object) new SolidColorBrush(new ColorW(142, 188, 207, (int) byte.MaxValue)));
      trigger33.Setters.Add(setter71);
      controlTemplate21.Triggers.Add((TriggerBase) trigger33);
      Trigger trigger34 = new Trigger();
      trigger34.Property = ListBoxItem.IsSelectedProperty;
      trigger34.Value = (object) true;
      Setter setter72 = new Setter(Control.ForegroundProperty, (object) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue)));
      trigger34.Setters.Add(setter72);
      Setter setter73 = new Setter(Control.BackgroundProperty, (object) new SolidColorBrush(new ColorW(91, 115, 123, (int) byte.MaxValue)));
      trigger34.Setters.Add(setter73);
      controlTemplate21.Triggers.Add((TriggerBase) trigger34);
      Setter setter74 = new Setter(Control.TemplateProperty, (object) controlTemplate21);
      style21.Setters.Add(setter74);
      this.Add((object) typeof (ComboBoxItem), (object) style21);
      Style style22 = new Style(typeof (DataGrid), this[(object) typeof (DataGrid)] as Style);
      Setter setter75 = new Setter(Control.BackgroundProperty, (object) new SolidColorBrush(new ColorW(41, 54, 62, 240)));
      style22.Setters.Add(setter75);
      Trigger trigger35 = new Trigger();
      trigger35.Property = UIElement.IsFocusedProperty;
      trigger35.Value = (object) true;
      Setter setter76 = new Setter(Control.BackgroundProperty, (object) new SolidColorBrush(new ColorW(60, 76, 82, (int) byte.MaxValue)));
      trigger35.Setters.Add(setter76);
      style22.Triggers.Add((TriggerBase) trigger35);
      this.Add((object) typeof (DataGrid), (object) style22);
      Style style23 = new Style(typeof (DataGridRow), this[(object) typeof (DataGridRow)] as Style);
      Setter setter77 = new Setter(Control.BorderThicknessProperty, (object) new Thickness(0.0f));
      style23.Setters.Add(setter77);
      Setter setter78 = new Setter(Control.BorderBrushProperty, (object) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 0)));
      style23.Setters.Add(setter78);
      Setter setter79 = new Setter(Control.IsTabStopProperty, (object) false);
      style23.Setters.Add(setter79);
      this.Add((object) typeof (DataGridRow), (object) style23);
      Style style24 = new Style(typeof (ListBox), this[(object) typeof (ListBox)] as Style);
      Trigger trigger36 = new Trigger();
      trigger36.Property = UIElement.IsFocusedProperty;
      trigger36.Value = (object) true;
      Setter setter80 = new Setter(Control.BackgroundProperty, (object) new SolidColorBrush(new ColorW(67, 81, 92, 192)));
      trigger36.Setters.Add(setter80);
      style24.Triggers.Add((TriggerBase) trigger36);
      this.Add((object) typeof (ListBox), (object) style24);
      Style style25 = new Style(typeof (ListBoxItem), this[(object) typeof (ListBoxItem)] as Style);
      Setter setter81 = new Setter(Control.IsTabStopProperty, (object) false);
      style25.Setters.Add(setter81);
      this.Add((object) typeof (ListBoxItem), (object) style25);
      Style style26 = new Style(typeof (ScrollBar));
      Trigger trigger37 = new Trigger();
      trigger37.Property = ScrollBar.OrientationProperty;
      trigger37.Value = (object) Orientation.Vertical;
      ControlTemplate controlTemplate22 = new ControlTemplate(typeof (ScrollBar), new Func<UIElement, UIElement>(Styles.r_147_s_T_0_S_0_ctMethod));
      Setter setter82 = new Setter(Control.TemplateProperty, (object) controlTemplate22);
      trigger37.Setters.Add(setter82);
      style26.Triggers.Add((TriggerBase) trigger37);
      Trigger trigger38 = new Trigger();
      trigger38.Property = ScrollBar.OrientationProperty;
      trigger38.Value = (object) Orientation.Horizontal;
      ControlTemplate controlTemplate23 = new ControlTemplate(typeof (ScrollBar), new Func<UIElement, UIElement>(Styles.r_147_s_T_1_S_0_ctMethod));
      Setter setter83 = new Setter(Control.TemplateProperty, (object) controlTemplate23);
      trigger38.Setters.Add(setter83);
      style26.Triggers.Add((TriggerBase) trigger38);
      this.Add((object) typeof (ScrollBar), (object) style26);
      Style style27 = new Style(typeof (Slider));
      Setter setter84 = new Setter(UIElement.SnapsToDevicePixelsProperty, (object) false);
      style27.Setters.Add(setter84);
      Setter setter85 = new Setter(Control.TemplateProperty, (object) new ResourceReferenceExpression((object) "HorizontalSlider"));
      style27.Setters.Add(setter85);
      this.Add((object) typeof (Slider), (object) style27);
      Style style28 = new Style(typeof (TabControl), this[(object) typeof (TabControl)] as Style);
      Setter setter86 = new Setter(Control.BorderThicknessProperty, (object) new Thickness(0.0f));
      style28.Setters.Add(setter86);
      Setter setter87 = new Setter(Control.PaddingProperty, (object) new Thickness(0.0f));
      style28.Setters.Add(setter87);
      Setter setter88 = new Setter(Control.BorderBrushProperty, (object) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 0)));
      style28.Setters.Add(setter88);
      this.Add((object) typeof (TabControl), (object) style28);
      Style style29 = new Style(typeof (TabItem));
      Setter setter89 = new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "ButtonBackgroundBrush"));
      style29.Setters.Add(setter89);
      Setter setter90 = new Setter(Control.ForegroundProperty, (object) new ResourceReferenceExpression((object) "ButtonTextColor"));
      style29.Setters.Add(setter90);
      ControlTemplate controlTemplate24 = new ControlTemplate(typeof (TabItem), new Func<UIElement, UIElement>(Styles.r_150_s_S_2_ctMethod));
      Trigger trigger39 = new Trigger();
      trigger39.Property = TabItem.IsSelectedProperty;
      trigger39.Value = (object) true;
      Setter setter91 = new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "ButtonBackgroundPressedBrush"));
      trigger39.Setters.Add(setter91);
      Setter setter92 = new Setter(Control.ForegroundProperty, (object) new ResourceReferenceExpression((object) "ButtonTextPressedColor"));
      trigger39.Setters.Add(setter92);
      controlTemplate24.Triggers.Add((TriggerBase) trigger39);
      Trigger trigger40 = new Trigger();
      trigger40.Property = UIElement.IsFocusedProperty;
      trigger40.Value = (object) true;
      Setter setter93 = new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "ButtonBackgroundFocusedBrush"));
      trigger40.Setters.Add(setter93);
      Setter setter94 = new Setter(Control.ForegroundProperty, (object) new ResourceReferenceExpression((object) "ButtonTextFocusedColor"));
      trigger40.Setters.Add(setter94);
      controlTemplate24.Triggers.Add((TriggerBase) trigger40);
      Trigger trigger41 = new Trigger();
      trigger41.Property = UIElement.IsMouseOverProperty;
      trigger41.Value = (object) true;
      Setter setter95 = new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "ButtonBackgroundHoverBrush"));
      trigger41.Setters.Add(setter95);
      Setter setter96 = new Setter(Control.ForegroundProperty, (object) new ResourceReferenceExpression((object) "ButtonTextHoverColor"));
      trigger41.Setters.Add(setter96);
      controlTemplate24.Triggers.Add((TriggerBase) trigger41);
      Setter setter97 = new Setter(Control.TemplateProperty, (object) controlTemplate24);
      style29.Setters.Add(setter97);
      this.Add((object) typeof (TabItem), (object) style29);
      Style style30 = new Style(typeof (TextBox));
      Setter setter98 = new Setter(TextBoxBase.CaretBrushProperty, (object) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue)));
      style30.Setters.Add(setter98);
      Setter setter99 = new Setter(TextBoxBase.SelectionBrushProperty, (object) new SolidColorBrush(new ColorW(63, 71, 79, (int) byte.MaxValue)));
      style30.Setters.Add(setter99);
      ControlTemplate controlTemplate25 = new ControlTemplate(typeof (TextBox), new Func<UIElement, UIElement>(Styles.r_151_s_S_2_ctMethod));
      Trigger trigger42 = new Trigger();
      trigger42.Property = UIElement.IsMouseOverProperty;
      trigger42.Value = (object) true;
      trigger42.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "TextBoxLeftHighlight"))
      {
        TargetName = "PART_TextBoxLeft"
      });
      trigger42.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "TextBoxCenterHighlight"))
      {
        TargetName = "PART_TextBoxCenter"
      });
      trigger42.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "TextBoxRightHighlight"))
      {
        TargetName = "PART_TextBoxRight"
      });
      Setter setter100 = new Setter(Control.ForegroundProperty, (object) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue)));
      trigger42.Setters.Add(setter100);
      controlTemplate25.Triggers.Add((TriggerBase) trigger42);
      Trigger trigger43 = new Trigger();
      trigger43.Property = UIElement.IsFocusedProperty;
      trigger43.Value = (object) true;
      trigger43.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "TextBoxLeftFocus"))
      {
        TargetName = "PART_TextBoxLeft"
      });
      trigger43.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "TextBoxCenterFocus"))
      {
        TargetName = "PART_TextBoxCenter"
      });
      trigger43.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "TextBoxRightFocus"))
      {
        TargetName = "PART_TextBoxRight"
      });
      Setter setter101 = new Setter(Control.ForegroundProperty, (object) new SolidColorBrush(new ColorW(33, 40, 45, (int) byte.MaxValue)));
      trigger43.Setters.Add(setter101);
      controlTemplate25.Triggers.Add((TriggerBase) trigger43);
      Setter setter102 = new Setter(Control.TemplateProperty, (object) controlTemplate25);
      style30.Setters.Add(setter102);
      this.Add((object) typeof (TextBox), (object) style30);
      Style style31 = new Style(typeof (ToolTip), this[(object) typeof (ToolTip)] as Style);
      Setter setter103 = new Setter(Control.PaddingProperty, (object) new Thickness(0.0f));
      style31.Setters.Add(setter103);
      Setter setter104 = new Setter(Control.BorderThicknessProperty, (object) new Thickness(0.0f));
      style31.Setters.Add(setter104);
      Setter setter105 = new Setter(UIElement.OpacityProperty, (object) 1f);
      style31.Setters.Add(setter105);
      this.Add((object) typeof (ToolTip), (object) style31);
      Style style32 = new Style(typeof (Window));
      ControlTemplate controlTemplate26 = new ControlTemplate(typeof (Window), new Func<UIElement, UIElement>(Styles.r_153_s_S_0_ctMethod));
      Setter setter106 = new Setter(Control.TemplateProperty, (object) controlTemplate26);
      style32.Setters.Add(setter106);
      this.Add((object) typeof (Window), (object) style32);
      this.Add((object) "TextBoxCenter", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\textbox_center.dds"
        }
      });
      this.Add((object) "TextBoxCenterFocus", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\textbox_center_focus.dds"
        }
      });
      this.Add((object) "TextBoxCenterHighlight", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\textbox_center_highlight.dds"
        }
      });
      this.Add((object) "TextBoxLeft", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\textbox_left.dds"
        }
      });
      this.Add((object) "TextBoxLeftFocus", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\textbox_left_focus.dds"
        }
      });
      this.Add((object) "TextBoxLeftHighlight", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\textbox_left_highlight.dds"
        }
      });
      this.Add((object) "TextBoxRight", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\textbox_right.dds"
        }
      });
      this.Add((object) "TextBoxRightFocus", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\textbox_right_focus.dds"
        }
      });
      this.Add((object) "TextBoxRightHighlight", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Controls\\textbox_right_highlight.dds"
        }
      });
      Style style33 = new Style(typeof (ScrollViewer));
      Setter setter107 = new Setter(UIElement.SnapsToDevicePixelsProperty, (object) true);
      style33.Setters.Add(setter107);
      ControlTemplate controlTemplate27 = new ControlTemplate(typeof (ScrollViewer), new Func<UIElement, UIElement>(Styles.r_163_s_S_1_ctMethod));
      Setter setter108 = new Setter(Control.TemplateProperty, (object) controlTemplate27);
      style33.Setters.Add(setter108);
      this.Add((object) "TextBoxScrollViewer", (object) style33);
      this.Add((object) "TextColor", (object) new SolidColorBrush(new ColorW(198, 220, 228, (int) byte.MaxValue)));
      this.Add((object) "TextColorDarkBlue", (object) new SolidColorBrush(new ColorW(94, 115, (int) sbyte.MaxValue, (int) byte.MaxValue)));
      this.Add((object) "TextColorHighlight", (object) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue)));
      this.Add((object) "ThemedGuiLineColor", (object) new SolidColorBrush(new ColorW(77, 99, 113, (int) byte.MaxValue)));
      this.Add((object) "ToolTipBackgroundBrush", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Screens\\screen_background.dds"
        }
      });
      Style style34 = new Style(typeof (Thumb));
      ControlTemplate controlTemplate28 = new ControlTemplate(typeof (Thumb), new Func<UIElement, UIElement>(Styles.r_169_s_S_0_ctMethod));
      Trigger trigger44 = new Trigger();
      trigger44.Property = UIElement.IsMouseOverProperty;
      trigger44.Value = (object) true;
      trigger44.Setters.Add(new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "ScrollbarVerticalThumbCenterHighlight"))
      {
        TargetName = "PART_Center"
      });
      controlTemplate28.Triggers.Add((TriggerBase) trigger44);
      Setter setter109 = new Setter(Control.TemplateProperty, (object) controlTemplate28);
      style34.Setters.Add(setter109);
      this.Add((object) "VerticalThumb", (object) style34);
      this.Add((object) "WindowBackgroundBrush", (object) new ImageBrush()
      {
        ImageSource = new BitmapImage()
        {
          TextureAsset = "Textures\\GUI\\Screens\\screen_background.dds"
        },
        Stretch = Stretch.Fill
      });
      ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Contracts\\AcquisitionContractHeader.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Bg16x9.png");
      ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Contracts\\BountyContractHeader.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_arrow_left.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_arrow_left_highlight.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_arrow_right.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_arrow_right_highlight.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_default.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_default_focus.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_default_highlight.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_default_active.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_decrease.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_decrease_highlight.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_increase.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_increase_highlight.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\checkbox_unchecked.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\checkbox_unchecked_focus.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\checkbox_unchecked_highlight.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Browser\\BackgroundFocused.png");
      ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Browser\\ModioCBFocused.png");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\checkbox_checked.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Browser\\SteamCB.png");
      ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Browser\\SteamCBFocused.png");
      ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Browser\\ModioCB.png");
      ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Browser\\Background.png");
      ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Browser\\BackgroundChecked.png");
      ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Browser\\BackgroundHighlighted.png");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_close_symbol_highlight.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\combobox_default_center.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\combobox_default_focus_center.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\combobox_default_highlight_center.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\combobox_default_left.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\combobox_default_focus_left.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\combobox_default_highlight_left.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\combobox_default_right.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\combobox_default_focus_right.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\combobox_default_highlight_right.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Contracts\\EscortContractHeader.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\HUD 2017\\Notification_badge.png");
      ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Rating\\FullStar.png");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\grid_item.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\grid_item_highlight.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Rating\\HalfStar.png");
      ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Contracts\\HaulingContractHeader.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\hue_slider_rail_center.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\hue_slider_rail_center_highlight.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\hue_slider_rail_left.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\hue_slider_rail_left_highlight.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\hue_slider_rail_right.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\hue_slider_rail_right_highlight.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\HydrogenIcon.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\LoadingIconAnimated.png");
      ImageManager.Instance.AddImage("Textures\\GUI\\Screens\\message_background_red.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Rating\\NoStar.png");
      ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\OxygenIcon.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_red.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\button_red_highlight.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Blueprints\\Refresh.png");
      ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Contracts\\RepairContractHeader.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Contracts\\ArrowRepGain.png");
      ImageManager.Instance.AddImage("Textures\\GUI\\Contracts\\ArrowRepLoss.png");
      ImageManager.Instance.AddImage("Textures\\GUI\\Screens\\screen_background.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollable_list_center.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollable_list_center_bottom.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollable_list_center_top.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollable_list_left_bottom.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollable_list_left_center.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollable_list_left_top.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollable_list_right_bottom.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollable_list_right_center.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollable_list_right_top.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollbar_h_thumb_center.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollbar_h_thumb_center_highlight.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollbar_h_thumb_left.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollbar_h_thumb_left_highlight.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollbar_h_thumb_right.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollbar_h_thumb_right_highlight.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollbar_v_thumb_bottom.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollbar_v_thumb_bottom_highlight.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollbar_v_thumb_center.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollbar_v_thumb_center_highlight.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollbar_v_thumb_top.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\scrollbar_v_thumb_top_highlight.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Icons\\Contracts\\SearchContractHeader.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\slider_rail_center.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\slider_rail_center_focus.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\slider_rail_center_highlight.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\slider_rail_left.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\slider_rail_left_focus.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\slider_rail_left_highlight.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\slider_rail_right.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\slider_rail_right_focus.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\slider_rail_right_highlight.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\slider_thumb.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\slider_thumb_highlight.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\textbox_center.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\textbox_center_focus.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\textbox_center_highlight.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\textbox_left.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\textbox_left_focus.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\textbox_left_highlight.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\textbox_right.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\textbox_right_focus.dds");
      ImageManager.Instance.AddImage("Textures\\GUI\\Controls\\textbox_right_highlight.dds");
    }

    private static UIElement r_24_s_S_0_ctMethod(UIElement parent)
    {
      StackPanel stackPanel = new StackPanel();
      stackPanel.Parent = parent;
      stackPanel.Name = "e_0";
      stackPanel.Orientation = Orientation.Horizontal;
      Grid grid = new Grid();
      stackPanel.Children.Add((UIElement) grid);
      grid.Name = "e_1";
      Border border1 = new Border();
      grid.Children.Add((UIElement) border1);
      border1.Name = "PART_Background";
      border1.Height = 38f;
      border1.Width = 38f;
      border1.SetResourceReference(Control.BackgroundProperty, (object) "CheckServiceBack");
      Border border2 = new Border();
      grid.Children.Add((UIElement) border2);
      border2.Name = "PART_Icon";
      border2.Height = 38f;
      border2.Width = 38f;
      border2.SetResourceReference(Control.BackgroundProperty, (object) "CheckService0");
      ContentPresenter contentPresenter = new ContentPresenter();
      stackPanel.Children.Add((UIElement) contentPresenter);
      contentPresenter.Name = "e_2";
      contentPresenter.VerticalAlignment = VerticalAlignment.Center;
      contentPresenter.SetBinding(ContentPresenter.ContentProperty, new Binding("Content")
      {
        Source = (object) parent
      });
      return (UIElement) stackPanel;
    }

    private static UIElement r_25_s_S_0_ctMethod(UIElement parent)
    {
      StackPanel stackPanel = new StackPanel();
      stackPanel.Parent = parent;
      stackPanel.Name = "e_3";
      stackPanel.Orientation = Orientation.Horizontal;
      Grid grid = new Grid();
      stackPanel.Children.Add((UIElement) grid);
      grid.Name = "e_4";
      Border border1 = new Border();
      grid.Children.Add((UIElement) border1);
      border1.Name = "PART_Background";
      border1.Height = 38f;
      border1.Width = 38f;
      border1.SetResourceReference(Control.BackgroundProperty, (object) "CheckServiceBack");
      Border border2 = new Border();
      grid.Children.Add((UIElement) border2);
      border2.Name = "PART_Icon";
      border2.Height = 38f;
      border2.Width = 38f;
      border2.SetResourceReference(Control.BackgroundProperty, (object) "CheckService1");
      ContentPresenter contentPresenter = new ContentPresenter();
      stackPanel.Children.Add((UIElement) contentPresenter);
      contentPresenter.Name = "e_5";
      contentPresenter.VerticalAlignment = VerticalAlignment.Center;
      contentPresenter.SetBinding(ContentPresenter.ContentProperty, new Binding("Content")
      {
        Source = (object) parent
      });
      return (UIElement) stackPanel;
    }

    private static UIElement r_54_s_S_5_ctMethod(UIElement parent)
    {
      Border border1 = new Border();
      border1.Parent = parent;
      border1.Name = "e_6";
      Grid grid1 = new Grid();
      border1.Child = (UIElement) grid1;
      grid1.Name = "e_7";
      grid1.SnapsToDevicePixels = false;
      grid1.UseLayoutRounding = true;
      ColumnDefinition columnDefinition1 = new ColumnDefinition();
      grid1.ColumnDefinitions.Add(columnDefinition1);
      grid1.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      Grid grid2 = new Grid();
      grid1.Children.Add((UIElement) grid2);
      grid2.Name = "e_8";
      grid2.RowDefinitions.Add(new RowDefinition()
      {
        MinHeight = 36f
      });
      grid2.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(4f, GridUnitType.Pixel)
      });
      ColumnDefinition columnDefinition2 = new ColumnDefinition();
      grid2.ColumnDefinitions.Add(columnDefinition2);
      grid2.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(4f, GridUnitType.Pixel)
      });
      Border border2 = new Border();
      grid2.Children.Add((UIElement) border2);
      border2.Name = "PART_TextBoxLeft";
      border2.IsHitTestVisible = false;
      border2.SnapsToDevicePixels = false;
      border2.SetResourceReference(Control.BackgroundProperty, (object) "TextBoxLeft");
      Border border3 = new Border();
      grid2.Children.Add((UIElement) border3);
      border3.Name = "PART_TextBoxCenter";
      border3.IsHitTestVisible = false;
      border3.SnapsToDevicePixels = false;
      Grid.SetColumn((UIElement) border3, 1);
      border3.SetResourceReference(Control.BackgroundProperty, (object) "TextBoxCenter");
      Border border4 = new Border();
      grid2.Children.Add((UIElement) border4);
      border4.Name = "PART_TextBoxRight";
      border4.IsHitTestVisible = false;
      border4.SnapsToDevicePixels = false;
      Grid.SetColumn((UIElement) border4, 2);
      border4.SetResourceReference(Control.BackgroundProperty, (object) "TextBoxRight");
      ScrollViewer scrollViewer = new ScrollViewer();
      grid2.Children.Add((UIElement) scrollViewer);
      scrollViewer.Name = "PART_ScrollViewer";
      scrollViewer.HorizontalContentAlignment = HorizontalAlignment.Stretch;
      scrollViewer.VerticalContentAlignment = VerticalAlignment.Stretch;
      scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
      Grid.SetColumn((UIElement) scrollViewer, 1);
      TextBlock textBlock = new TextBlock();
      scrollViewer.Content = (object) textBlock;
      textBlock.Name = "e_9";
      textBlock.HorizontalAlignment = HorizontalAlignment.Right;
      textBlock.VerticalAlignment = VerticalAlignment.Center;
      textBlock.TextAlignment = TextAlignment.Right;
      textBlock.SetBinding(Control.PaddingProperty, new Binding("Padding")
      {
        Source = (object) parent
      });
      textBlock.SetBinding(TextBlock.TextProperty, new Binding("Text")
      {
        Source = (object) parent
      });
      StackPanel stackPanel = new StackPanel();
      grid1.Children.Add((UIElement) stackPanel);
      stackPanel.Name = "e_10";
      stackPanel.Orientation = Orientation.Horizontal;
      Grid.SetColumn((UIElement) stackPanel, 1);
      RepeatButton repeatButton1 = new RepeatButton();
      stackPanel.Children.Add((UIElement) repeatButton1);
      repeatButton1.Name = "PART_DecreaseButton";
      repeatButton1.Height = 32f;
      repeatButton1.Width = 32f;
      repeatButton1.Focusable = false;
      repeatButton1.IsTabStop = false;
      repeatButton1.ClickMode = ClickMode.Press;
      repeatButton1.Delay = 100;
      repeatButton1.Interval = 200;
      SoundManager.SetIsSoundEnabled((DependencyObject) repeatButton1, false);
      repeatButton1.SetResourceReference(UIElement.StyleProperty, (object) "RepeatButtonDecrease");
      RepeatButton repeatButton2 = new RepeatButton();
      stackPanel.Children.Add((UIElement) repeatButton2);
      repeatButton2.Name = "PART_IncreaseButton";
      repeatButton2.Height = 32f;
      repeatButton2.Width = 32f;
      repeatButton2.Focusable = false;
      repeatButton2.IsTabStop = false;
      repeatButton2.ClickMode = ClickMode.Press;
      repeatButton2.Delay = 100;
      repeatButton2.Interval = 200;
      SoundManager.SetIsSoundEnabled((DependencyObject) repeatButton2, false);
      repeatButton2.SetResourceReference(UIElement.StyleProperty, (object) "RepeatButtonIncrease");
      return (UIElement) border1;
    }

    private static UIElement r_62_ctMethod(UIElement parent)
    {
      Grid grid = new Grid();
      grid.Parent = parent;
      grid.Name = "e_11";
      grid.Height = 32f;
      grid.SnapsToDevicePixels = false;
      grid.UseLayoutRounding = true;
      grid.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(12f, GridUnitType.Pixel)
      });
      grid.ColumnDefinitions.Add(new ColumnDefinition());
      grid.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(12f, GridUnitType.Pixel)
      });
      Border border1 = new Border();
      grid.Children.Add((UIElement) border1);
      border1.Name = "PART_SliderRailLeft";
      border1.IsHitTestVisible = false;
      border1.SetResourceReference(Control.BackgroundProperty, (object) "SliderRailLeft");
      Border border2 = new Border();
      grid.Children.Add((UIElement) border2);
      border2.Name = "PART_SliderRailCenter";
      border2.IsHitTestVisible = false;
      Grid.SetColumn((UIElement) border2, 1);
      border2.SetResourceReference(Control.BackgroundProperty, (object) "SliderRailCenter");
      Border border3 = new Border();
      grid.Children.Add((UIElement) border3);
      border3.Name = "PART_SliderRailRight";
      border3.IsHitTestVisible = false;
      Grid.SetColumn((UIElement) border3, 2);
      border3.SetResourceReference(Control.BackgroundProperty, (object) "SliderRailRight");
      Track track = new Track();
      grid.Children.Add((UIElement) track);
      track.Name = "PART_Track";
      track.Margin = new Thickness(6f, 0.0f, 6f, 0.0f);
      RepeatButton repeatButton1 = new RepeatButton();
      repeatButton1.Name = "e_12";
      repeatButton1.ClickMode = ClickMode.Press;
      repeatButton1.SetResourceReference(UIElement.StyleProperty, (object) "SliderButtonStyle");
      track.IncreaseRepeatButton = repeatButton1;
      RepeatButton repeatButton2 = new RepeatButton();
      repeatButton2.Name = "e_13";
      repeatButton2.ClickMode = ClickMode.Press;
      repeatButton2.SetResourceReference(UIElement.StyleProperty, (object) "SliderButtonStyle");
      track.DecreaseRepeatButton = repeatButton2;
      Thumb thumb = new Thumb();
      thumb.Name = "e_14";
      thumb.VerticalAlignment = VerticalAlignment.Stretch;
      thumb.VerticalContentAlignment = VerticalAlignment.Center;
      thumb.SetResourceReference(UIElement.StyleProperty, (object) "SliderThumbStyle");
      track.Thumb = thumb;
      Grid.SetColumnSpan((UIElement) track, 3);
      return (UIElement) grid;
    }

    private static UIElement r_63_ctMethod(UIElement parent)
    {
      Grid grid = new Grid();
      grid.Parent = parent;
      grid.Name = "e_15";
      grid.Height = 32f;
      grid.SnapsToDevicePixels = false;
      grid.UseLayoutRounding = true;
      grid.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(12f, GridUnitType.Pixel)
      });
      grid.ColumnDefinitions.Add(new ColumnDefinition());
      grid.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(12f, GridUnitType.Pixel)
      });
      Border border1 = new Border();
      grid.Children.Add((UIElement) border1);
      border1.Name = "PART_SliderRailLeft";
      border1.IsHitTestVisible = false;
      border1.SetResourceReference(Control.BackgroundProperty, (object) "HueColorGradientLeft");
      Border border2 = new Border();
      grid.Children.Add((UIElement) border2);
      border2.Name = "PART_SliderRailCenter";
      border2.IsHitTestVisible = false;
      Grid.SetColumn((UIElement) border2, 1);
      border2.SetResourceReference(Control.BackgroundProperty, (object) "HueColorGradientCenter");
      Border border3 = new Border();
      grid.Children.Add((UIElement) border3);
      border3.Name = "PART_SliderRailRight";
      border3.IsHitTestVisible = false;
      Grid.SetColumn((UIElement) border3, 2);
      border3.SetResourceReference(Control.BackgroundProperty, (object) "HueColorGradientRight");
      Track track = new Track();
      grid.Children.Add((UIElement) track);
      track.Name = "PART_Track";
      track.Margin = new Thickness(6f, 0.0f, 6f, 0.0f);
      RepeatButton repeatButton1 = new RepeatButton();
      repeatButton1.Name = "e_16";
      repeatButton1.ClickMode = ClickMode.Press;
      repeatButton1.SetResourceReference(UIElement.StyleProperty, (object) "SliderButtonStyle");
      track.IncreaseRepeatButton = repeatButton1;
      RepeatButton repeatButton2 = new RepeatButton();
      repeatButton2.Name = "e_17";
      repeatButton2.ClickMode = ClickMode.Press;
      repeatButton2.SetResourceReference(UIElement.StyleProperty, (object) "SliderButtonStyle");
      track.DecreaseRepeatButton = repeatButton2;
      Thumb thumb = new Thumb();
      thumb.Name = "e_18";
      thumb.VerticalAlignment = VerticalAlignment.Stretch;
      thumb.VerticalContentAlignment = VerticalAlignment.Center;
      thumb.SetResourceReference(UIElement.StyleProperty, (object) "SliderThumbStyle");
      track.Thumb = thumb;
      Grid.SetColumnSpan((UIElement) track, 3);
      return (UIElement) grid;
    }

    private static UIElement r_64_s_S_0_ctMethod(UIElement parent)
    {
      Grid grid = new Grid();
      grid.Parent = parent;
      grid.Name = "e_19";
      grid.Margin = new Thickness(2f, 0.0f, 2f, 0.0f);
      grid.ColumnDefinitions.Add(new ColumnDefinition());
      Border border = new Border();
      grid.Children.Add((UIElement) border);
      border.Name = "PART_Center";
      Grid.SetColumn((UIElement) border, 0);
      border.SetResourceReference(Control.BackgroundProperty, (object) "ScrollbarHorizontalThumbCenter");
      return (UIElement) grid;
    }

    private static UIElement r_80_s_S_1_ctMethod(UIElement parent)
    {
      Border border = new Border();
      border.Parent = parent;
      border.Name = "e_20";
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
      scrollViewer.HorizontalContentAlignment = HorizontalAlignment.Stretch;
      scrollViewer.VerticalContentAlignment = VerticalAlignment.Stretch;
      UniformGrid uniformGrid = new UniformGrid();
      scrollViewer.Content = (object) uniformGrid;
      uniformGrid.Name = "e_21";
      uniformGrid.Margin = new Thickness(4f, 4f, 4f, 4f);
      uniformGrid.VerticalAlignment = VerticalAlignment.Top;
      uniformGrid.IsItemsHost = true;
      uniformGrid.Columns = 5;
      return (UIElement) border;
    }

    private static UIElement r_82_s_S_1_ctMethod(UIElement parent)
    {
      Border border = new Border();
      border.Parent = parent;
      border.Name = "e_22";
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
      scrollViewer.HorizontalContentAlignment = HorizontalAlignment.Stretch;
      scrollViewer.VerticalContentAlignment = VerticalAlignment.Stretch;
      WrapPanel wrapPanel = new WrapPanel();
      scrollViewer.Content = (object) wrapPanel;
      wrapPanel.Name = "e_23";
      wrapPanel.Margin = new Thickness(4f, 4f, 4f, 4f);
      wrapPanel.IsItemsHost = true;
      wrapPanel.ItemHeight = 64f;
      wrapPanel.ItemWidth = 64f;
      return (UIElement) border;
    }

    private static UIElement r_83_s_S_1_ctMethod(UIElement parent)
    {
      Border border = new Border();
      border.Parent = parent;
      border.Name = "e_24";
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
      UniformGrid uniformGrid = new UniformGrid();
      border.Child = (UIElement) uniformGrid;
      uniformGrid.Name = "e_25";
      uniformGrid.Margin = new Thickness(5f, 5f, 5f, 5f);
      uniformGrid.IsItemsHost = true;
      uniformGrid.Rows = 3;
      uniformGrid.Columns = 3;
      return (UIElement) border;
    }

    private static void InitializeElemente_27Resources(UIElement elem)
    {
      Style style = new Style(typeof (Button), elem.Resources[(object) typeof (Button)] as Style);
      Setter setter1 = new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "RedButtonBackgroundBrush"));
      style.Setters.Add(setter1);
      Setter setter2 = new Setter(Control.ForegroundProperty, (object) new ResourceReferenceExpression((object) "ButtonTextColor"));
      style.Setters.Add(setter2);
      Setter setter3 = new Setter(UIElement.WidthProperty, (object) 200f);
      style.Setters.Add(setter3);
      ControlTemplate controlTemplate = new ControlTemplate(typeof (Button), new Func<UIElement, UIElement>(Styles.e_27r_0_s_S_3_ctMethod));
      Trigger trigger1 = new Trigger();
      trigger1.Property = Button.IsPressedProperty;
      trigger1.Value = (object) true;
      Setter setter4 = new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "RedButtonBackgroundBrushHighlight"));
      trigger1.Setters.Add(setter4);
      Setter setter5 = new Setter(Control.ForegroundProperty, (object) new SolidColorBrush(new ColorW((int) byte.MaxValue, 0, 0, (int) byte.MaxValue)));
      trigger1.Setters.Add(setter5);
      controlTemplate.Triggers.Add((TriggerBase) trigger1);
      Trigger trigger2 = new Trigger();
      trigger2.Property = UIElement.IsFocusedProperty;
      trigger2.Value = (object) true;
      Setter setter6 = new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "RedButtonBackgroundBrushHighlight"));
      trigger2.Setters.Add(setter6);
      Setter setter7 = new Setter(Control.ForegroundProperty, (object) new ResourceReferenceExpression((object) "ButtonTextFocusedColor"));
      trigger2.Setters.Add(setter7);
      controlTemplate.Triggers.Add((TriggerBase) trigger2);
      Trigger trigger3 = new Trigger();
      trigger3.Property = UIElement.IsMouseOverProperty;
      trigger3.Value = (object) true;
      Setter setter8 = new Setter(Control.BackgroundProperty, (object) new ResourceReferenceExpression((object) "RedButtonBackgroundBrushHighlight"));
      trigger3.Setters.Add(setter8);
      Setter setter9 = new Setter(Control.ForegroundProperty, (object) new SolidColorBrush(new ColorW((int) byte.MaxValue, 0, 0, (int) byte.MaxValue)));
      trigger3.Setters.Add(setter9);
      controlTemplate.Triggers.Add((TriggerBase) trigger3);
      Setter setter10 = new Setter(Control.TemplateProperty, (object) controlTemplate);
      style.Setters.Add(setter10);
      elem.Resources.Add((object) typeof (Button), (object) style);
    }

    private static UIElement e_27r_0_s_S_3_ctMethod(UIElement parent)
    {
      Border border = new Border();
      border.Parent = parent;
      border.Name = "e_28";
      border.SnapsToDevicePixels = true;
      border.Padding = new Thickness(5f, 5f, 5f, 5f);
      border.SetBinding(Control.BackgroundProperty, new Binding("Background")
      {
        Source = (object) parent
      });
      ContentPresenter contentPresenter = new ContentPresenter();
      border.Child = (UIElement) contentPresenter;
      contentPresenter.Name = "e_29";
      contentPresenter.HorizontalAlignment = HorizontalAlignment.Center;
      contentPresenter.VerticalAlignment = VerticalAlignment.Center;
      contentPresenter.SetBinding(ContentPresenter.ContentProperty, new Binding("Content")
      {
        Source = (object) parent
      });
      return (UIElement) border;
    }

    private static UIElement r_86_s_S_1_ctMethod(UIElement parent)
    {
      Grid grid1 = new Grid();
      grid1.Parent = parent;
      grid1.Name = "e_26";
      grid1.ColumnDefinitions.Add(new ColumnDefinition());
      grid1.ColumnDefinitions.Add(new ColumnDefinition());
      grid1.ColumnDefinitions.Add(new ColumnDefinition());
      Grid grid2 = new Grid();
      grid1.Children.Add((UIElement) grid2);
      grid2.Name = "e_27";
      grid2.SnapsToDevicePixels = true;
      grid2.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      RowDefinition rowDefinition = new RowDefinition();
      grid2.RowDefinitions.Add(rowDefinition);
      Grid.SetColumn((UIElement) grid2, 1);
      grid2.SetResourceReference(Control.BackgroundProperty, (object) "MessageBackgroundBrush");
      Styles.InitializeElemente_27Resources((UIElement) grid2);
      ContentPresenter contentPresenter1 = new ContentPresenter();
      grid2.Children.Add((UIElement) contentPresenter1);
      contentPresenter1.Name = "PART_WindowTitle";
      contentPresenter1.IsHitTestVisible = false;
      contentPresenter1.Margin = new Thickness(10f, 10f, 10f, 0.0f);
      contentPresenter1.HorizontalAlignment = HorizontalAlignment.Center;
      contentPresenter1.VerticalAlignment = VerticalAlignment.Center;
      contentPresenter1.SetBinding(ContentPresenter.ContentProperty, new Binding("Title")
      {
        Source = (object) parent
      });
      ContentPresenter contentPresenter2 = new ContentPresenter();
      grid2.Children.Add((UIElement) contentPresenter2);
      contentPresenter2.Name = "e_30";
      contentPresenter2.Margin = new Thickness(10f, 10f, 10f, 10f);
      contentPresenter2.HorizontalAlignment = HorizontalAlignment.Center;
      Grid.SetRow((UIElement) contentPresenter2, 1);
      contentPresenter2.SetBinding(ContentPresenter.ContentProperty, new Binding("Content")
      {
        Source = (object) parent
      });
      return (UIElement) grid1;
    }

    private static UIElement r_95_s_S_1_ctMethod(UIElement parent)
    {
      Border border = new Border();
      border.Parent = parent;
      border.Name = "e_31";
      border.Margin = new Thickness(1f, 1f, 1f, 1f);
      border.SetBinding(Control.BackgroundProperty, new Binding("Background")
      {
        Source = (object) parent
      });
      return (UIElement) border;
    }

    private static UIElement r_96_s_S_1_ctMethod(UIElement parent)
    {
      Border border = new Border();
      border.Parent = parent;
      border.Name = "e_32";
      border.Margin = new Thickness(1f, 1f, 1f, 1f);
      border.SetBinding(Control.BackgroundProperty, new Binding("Background")
      {
        Source = (object) parent
      });
      return (UIElement) border;
    }

    private static UIElement r_115_s_S_0_ctMethod(UIElement parent)
    {
      Border border = new Border();
      border.Parent = parent;
      border.Name = "e_33";
      return (UIElement) border;
    }

    private static UIElement r_122_s_S_1_ctMethod(UIElement parent)
    {
      Grid grid1 = new Grid();
      grid1.Parent = parent;
      grid1.Name = "e_34";
      grid1.RowDefinitions.Add(new RowDefinition());
      grid1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid1.ColumnDefinitions.Add(new ColumnDefinition());
      grid1.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      Grid grid2 = new Grid();
      grid1.Children.Add((UIElement) grid2);
      grid2.Name = "e_35";
      grid2.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(4f, GridUnitType.Pixel)
      });
      RowDefinition rowDefinition = new RowDefinition();
      grid2.RowDefinitions.Add(rowDefinition);
      grid2.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(4f, GridUnitType.Pixel)
      });
      grid2.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(4f, GridUnitType.Pixel)
      });
      ColumnDefinition columnDefinition = new ColumnDefinition();
      grid2.ColumnDefinitions.Add(columnDefinition);
      grid2.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(32f, GridUnitType.Pixel)
      });
      Grid.SetColumnSpan((UIElement) grid2, 2);
      Grid.SetRowSpan((UIElement) grid2, 2);
      Border border1 = new Border();
      grid2.Children.Add((UIElement) border1);
      border1.Name = "e_36";
      border1.IsHitTestVisible = false;
      border1.SnapsToDevicePixels = false;
      border1.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListLeftTop");
      Border border2 = new Border();
      grid2.Children.Add((UIElement) border2);
      border2.Name = "e_37";
      border2.IsHitTestVisible = false;
      border2.SnapsToDevicePixels = false;
      Grid.SetColumn((UIElement) border2, 1);
      border2.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListCenterTop");
      Border border3 = new Border();
      grid2.Children.Add((UIElement) border3);
      border3.Name = "e_38";
      border3.IsHitTestVisible = false;
      border3.SnapsToDevicePixels = false;
      Grid.SetColumn((UIElement) border3, 2);
      border3.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListRightTop");
      Border border4 = new Border();
      grid2.Children.Add((UIElement) border4);
      border4.Name = "e_39";
      border4.IsHitTestVisible = false;
      border4.SnapsToDevicePixels = false;
      Grid.SetRow((UIElement) border4, 1);
      border4.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListLeftCenter");
      Border border5 = new Border();
      grid2.Children.Add((UIElement) border5);
      border5.Name = "e_40";
      border5.IsHitTestVisible = false;
      border5.SnapsToDevicePixels = false;
      Grid.SetColumn((UIElement) border5, 1);
      Grid.SetRow((UIElement) border5, 1);
      border5.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListCenter");
      Border border6 = new Border();
      grid2.Children.Add((UIElement) border6);
      border6.Name = "e_41";
      border6.IsHitTestVisible = false;
      border6.SnapsToDevicePixels = false;
      Grid.SetColumn((UIElement) border6, 2);
      Grid.SetRow((UIElement) border6, 1);
      border6.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListRightCenter");
      Border border7 = new Border();
      grid2.Children.Add((UIElement) border7);
      border7.Name = "e_42";
      border7.IsHitTestVisible = false;
      border7.SnapsToDevicePixels = false;
      Grid.SetRow((UIElement) border7, 2);
      border7.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListLeftBottom");
      Border border8 = new Border();
      grid2.Children.Add((UIElement) border8);
      border8.Name = "e_43";
      border8.IsHitTestVisible = false;
      border8.SnapsToDevicePixels = false;
      Grid.SetColumn((UIElement) border8, 1);
      Grid.SetRow((UIElement) border8, 2);
      border8.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListCenterBottom");
      Border border9 = new Border();
      grid2.Children.Add((UIElement) border9);
      border9.Name = "e_44";
      border9.IsHitTestVisible = false;
      border9.SnapsToDevicePixels = false;
      Grid.SetColumn((UIElement) border9, 2);
      Grid.SetRow((UIElement) border9, 2);
      border9.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListRightBottom");
      ScrollContentPresenter contentPresenter = new ScrollContentPresenter();
      grid1.Children.Add((UIElement) contentPresenter);
      contentPresenter.Name = "PART_ScrollContentPresenter";
      contentPresenter.SnapsToDevicePixels = true;
      contentPresenter.SetBinding(UIElement.MarginProperty, new Binding("Padding")
      {
        Source = (object) parent
      });
      contentPresenter.SetBinding(UIElement.HorizontalAlignmentProperty, new Binding("HorizontalContentAlignment")
      {
        Source = (object) parent
      });
      contentPresenter.SetBinding(UIElement.VerticalAlignmentProperty, new Binding("VerticalContentAlignment")
      {
        Source = (object) parent
      });
      contentPresenter.SetBinding(ContentPresenter.ContentProperty, new Binding("Content")
      {
        Source = (object) parent
      });
      ScrollBar scrollBar1 = new ScrollBar();
      grid1.Children.Add((UIElement) scrollBar1);
      scrollBar1.Name = "PART_VerticalScrollBar";
      scrollBar1.Width = 32f;
      scrollBar1.Minimum = 0.0f;
      scrollBar1.Orientation = Orientation.Vertical;
      Grid.SetColumn((UIElement) scrollBar1, 1);
      scrollBar1.SetBinding(UIElement.VisibilityProperty, new Binding("ComputedVerticalScrollBarVisibility")
      {
        Source = (object) parent
      });
      scrollBar1.SetBinding(ScrollBar.ViewportSizeProperty, new Binding("ViewportHeight")
      {
        Source = (object) parent
      });
      scrollBar1.SetBinding(RangeBase.MaximumProperty, new Binding("ScrollableHeight")
      {
        Source = (object) parent
      });
      scrollBar1.SetBinding(RangeBase.ValueProperty, new Binding("VerticalOffset")
      {
        Source = (object) parent
      });
      ScrollBar scrollBar2 = new ScrollBar();
      grid1.Children.Add((UIElement) scrollBar2);
      scrollBar2.Name = "PART_HorizontalScrollBar";
      scrollBar2.Height = 32f;
      scrollBar2.Minimum = 0.0f;
      scrollBar2.Orientation = Orientation.Horizontal;
      Grid.SetRow((UIElement) scrollBar2, 1);
      scrollBar2.SetBinding(UIElement.VisibilityProperty, new Binding("ComputedHorizontalScrollBarVisibility")
      {
        Source = (object) parent
      });
      scrollBar2.SetBinding(ScrollBar.ViewportSizeProperty, new Binding("ViewportWidth")
      {
        Source = (object) parent
      });
      scrollBar2.SetBinding(RangeBase.MaximumProperty, new Binding("ScrollableWidth")
      {
        Source = (object) parent
      });
      scrollBar2.SetBinding(RangeBase.ValueProperty, new Binding("HorizontalOffset")
      {
        Source = (object) parent
      });
      return (UIElement) grid1;
    }

    private static UIElement r_123_s_S_2_ctMethod(UIElement parent)
    {
      Border border1 = new Border();
      border1.Parent = parent;
      border1.Name = "e_45";
      border1.BorderThickness = new Thickness(2f, 2f, 2f, 2f);
      border1.SetBinding(Control.BorderBrushProperty, new Binding("BorderBrush")
      {
        Source = (object) parent
      });
      Grid grid1 = new Grid();
      border1.Child = (UIElement) grid1;
      grid1.Name = "e_46";
      RowDefinition rowDefinition1 = new RowDefinition();
      grid1.RowDefinitions.Add(rowDefinition1);
      grid1.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      ColumnDefinition columnDefinition1 = new ColumnDefinition();
      grid1.ColumnDefinitions.Add(columnDefinition1);
      grid1.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(1f, GridUnitType.Auto)
      });
      Grid grid2 = new Grid();
      grid1.Children.Add((UIElement) grid2);
      grid2.Name = "e_47";
      grid2.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(4f, GridUnitType.Pixel)
      });
      RowDefinition rowDefinition2 = new RowDefinition();
      grid2.RowDefinitions.Add(rowDefinition2);
      grid2.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(4f, GridUnitType.Pixel)
      });
      grid2.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(4f, GridUnitType.Pixel)
      });
      ColumnDefinition columnDefinition2 = new ColumnDefinition();
      grid2.ColumnDefinitions.Add(columnDefinition2);
      grid2.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(32f, GridUnitType.Pixel)
      });
      Grid.SetColumnSpan((UIElement) grid2, 2);
      Grid.SetRowSpan((UIElement) grid2, 2);
      Border border2 = new Border();
      grid2.Children.Add((UIElement) border2);
      border2.Name = "e_48";
      border2.IsHitTestVisible = false;
      border2.SnapsToDevicePixels = false;
      border2.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListLeftTop");
      Border border3 = new Border();
      grid2.Children.Add((UIElement) border3);
      border3.Name = "e_49";
      border3.IsHitTestVisible = false;
      border3.SnapsToDevicePixels = false;
      Grid.SetColumn((UIElement) border3, 1);
      border3.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListCenterTop");
      Border border4 = new Border();
      grid2.Children.Add((UIElement) border4);
      border4.Name = "e_50";
      border4.IsHitTestVisible = false;
      border4.SnapsToDevicePixels = false;
      Grid.SetColumn((UIElement) border4, 2);
      border4.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListRightTop");
      Border border5 = new Border();
      grid2.Children.Add((UIElement) border5);
      border5.Name = "e_51";
      border5.IsHitTestVisible = false;
      border5.SnapsToDevicePixels = false;
      Grid.SetRow((UIElement) border5, 1);
      border5.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListLeftCenter");
      Border border6 = new Border();
      grid2.Children.Add((UIElement) border6);
      border6.Name = "e_52";
      border6.IsHitTestVisible = false;
      border6.SnapsToDevicePixels = false;
      Grid.SetColumn((UIElement) border6, 1);
      Grid.SetRow((UIElement) border6, 1);
      border6.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListCenter");
      Border border7 = new Border();
      grid2.Children.Add((UIElement) border7);
      border7.Name = "e_53";
      border7.IsHitTestVisible = false;
      border7.SnapsToDevicePixels = false;
      Grid.SetColumn((UIElement) border7, 2);
      Grid.SetRow((UIElement) border7, 1);
      border7.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListRightCenter");
      Border border8 = new Border();
      grid2.Children.Add((UIElement) border8);
      border8.Name = "e_54";
      border8.IsHitTestVisible = false;
      border8.SnapsToDevicePixels = false;
      Grid.SetRow((UIElement) border8, 2);
      border8.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListLeftBottom");
      Border border9 = new Border();
      grid2.Children.Add((UIElement) border9);
      border9.Name = "e_55";
      border9.IsHitTestVisible = false;
      border9.SnapsToDevicePixels = false;
      Grid.SetColumn((UIElement) border9, 1);
      Grid.SetRow((UIElement) border9, 2);
      border9.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListCenterBottom");
      Border border10 = new Border();
      grid2.Children.Add((UIElement) border10);
      border10.Name = "e_56";
      border10.IsHitTestVisible = false;
      border10.SnapsToDevicePixels = false;
      Grid.SetColumn((UIElement) border10, 2);
      Grid.SetRow((UIElement) border10, 2);
      border10.SetResourceReference(Control.BackgroundProperty, (object) "ScrollableListRightBottom");
      ScrollContentPresenter contentPresenter = new ScrollContentPresenter();
      grid1.Children.Add((UIElement) contentPresenter);
      contentPresenter.Name = "PART_ScrollContentPresenter";
      contentPresenter.SnapsToDevicePixels = true;
      contentPresenter.SetBinding(UIElement.MarginProperty, new Binding("Padding")
      {
        Source = (object) parent
      });
      contentPresenter.SetBinding(UIElement.HorizontalAlignmentProperty, new Binding("HorizontalContentAlignment")
      {
        Source = (object) parent
      });
      contentPresenter.SetBinding(UIElement.VerticalAlignmentProperty, new Binding("VerticalContentAlignment")
      {
        Source = (object) parent
      });
      contentPresenter.SetBinding(ContentPresenter.ContentProperty, new Binding("Content")
      {
        Source = (object) parent
      });
      ScrollBar scrollBar1 = new ScrollBar();
      grid1.Children.Add((UIElement) scrollBar1);
      scrollBar1.Name = "PART_VerticalScrollBar";
      scrollBar1.Width = 32f;
      scrollBar1.Minimum = 0.0f;
      scrollBar1.Orientation = Orientation.Vertical;
      Grid.SetColumn((UIElement) scrollBar1, 1);
      scrollBar1.SetBinding(UIElement.VisibilityProperty, new Binding("ComputedVerticalScrollBarVisibility")
      {
        Source = (object) parent
      });
      scrollBar1.SetBinding(ScrollBar.ViewportSizeProperty, new Binding("ViewportHeight")
      {
        Source = (object) parent
      });
      scrollBar1.SetBinding(RangeBase.MaximumProperty, new Binding("ScrollableHeight")
      {
        Source = (object) parent
      });
      scrollBar1.SetBinding(RangeBase.ValueProperty, new Binding("VerticalOffset")
      {
        Source = (object) parent
      });
      ScrollBar scrollBar2 = new ScrollBar();
      grid1.Children.Add((UIElement) scrollBar2);
      scrollBar2.Name = "PART_HorizontalScrollBar";
      scrollBar2.Height = 32f;
      scrollBar2.Minimum = 0.0f;
      scrollBar2.Orientation = Orientation.Horizontal;
      Grid.SetRow((UIElement) scrollBar2, 1);
      scrollBar2.SetBinding(UIElement.VisibilityProperty, new Binding("ComputedHorizontalScrollBarVisibility")
      {
        Source = (object) parent
      });
      scrollBar2.SetBinding(ScrollBar.ViewportSizeProperty, new Binding("ViewportWidth")
      {
        Source = (object) parent
      });
      scrollBar2.SetBinding(RangeBase.MaximumProperty, new Binding("ScrollableWidth")
      {
        Source = (object) parent
      });
      scrollBar2.SetBinding(RangeBase.ValueProperty, new Binding("HorizontalOffset")
      {
        Source = (object) parent
      });
      return (UIElement) border1;
    }

    private static UIElement r_125_s_S_2_ctMethod(UIElement parent)
    {
      Border border = new Border();
      border.Parent = parent;
      border.Name = "e_57";
      border.Background = (Brush) new SolidColorBrush(new ColorW((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 0));
      return (UIElement) border;
    }

    private static UIElement r_137_s_S_1_ctMethod(UIElement parent)
    {
      Border border = new Border();
      border.Parent = parent;
      border.Name = "PART_SliderThumb";
      border.Height = 20f;
      border.Width = 20f;
      border.SnapsToDevicePixels = true;
      border.SetBinding(UIElement.HorizontalAlignmentProperty, new Binding("HorizontalContentAlignment")
      {
        Source = (object) parent
      });
      border.SetBinding(UIElement.VerticalAlignmentProperty, new Binding("VerticalContentAlignment")
      {
        Source = (object) parent
      });
      border.SetResourceReference(Control.BackgroundProperty, (object) "SliderThumb");
      return (UIElement) border;
    }

    private static UIElement r_139_s_S_5_ctMethod(UIElement parent)
    {
      Border border = new Border();
      border.Parent = parent;
      border.Name = "e_58";
      border.SnapsToDevicePixels = true;
      border.SetBinding(Control.BackgroundProperty, new Binding("Background")
      {
        Source = (object) parent
      });
      ContentPresenter contentPresenter = new ContentPresenter();
      border.Child = (UIElement) contentPresenter;
      contentPresenter.Name = "e_59";
      contentPresenter.SetBinding(UIElement.MarginProperty, new Binding("Padding")
      {
        Source = (object) parent
      });
      contentPresenter.SetBinding(UIElement.HorizontalAlignmentProperty, new Binding("HorizontalContentAlignment")
      {
        Source = (object) parent
      });
      contentPresenter.SetBinding(UIElement.VerticalAlignmentProperty, new Binding("VerticalContentAlignment")
      {
        Source = (object) parent
      });
      contentPresenter.SetBinding(ContentPresenter.ContentProperty, new Binding("Content")
      {
        Source = (object) parent
      });
      return (UIElement) border;
    }

    private static UIElement r_140_s_S_0_ctMethod(UIElement parent)
    {
      StackPanel stackPanel = new StackPanel();
      stackPanel.Parent = parent;
      stackPanel.Name = "e_60";
      stackPanel.Orientation = Orientation.Horizontal;
      Grid grid = new Grid();
      stackPanel.Children.Add((UIElement) grid);
      grid.Name = "e_61";
      Border border1 = new Border();
      grid.Children.Add((UIElement) border1);
      border1.Name = "PART_NotChecked";
      border1.Height = 32f;
      border1.Width = 32f;
      border1.SetResourceReference(Control.BackgroundProperty, (object) "CheckBoxBackgroundBrush");
      Border border2 = new Border();
      grid.Children.Add((UIElement) border2);
      border2.Name = "PART_CheckMark";
      border2.Height = 32f;
      border2.Width = 32f;
      border2.Visibility = Visibility.Collapsed;
      border2.SetResourceReference(Control.BackgroundProperty, (object) "CheckImageBrush");
      ContentPresenter contentPresenter = new ContentPresenter();
      stackPanel.Children.Add((UIElement) contentPresenter);
      contentPresenter.Name = "e_62";
      contentPresenter.VerticalAlignment = VerticalAlignment.Center;
      contentPresenter.SetBinding(ContentPresenter.ContentProperty, new Binding("Content")
      {
        Source = (object) parent
      });
      return (UIElement) stackPanel;
    }

    private static UIElement PART_Button_s_S_0_ctMethod(UIElement parent)
    {
      ContentPresenter contentPresenter = new ContentPresenter();
      contentPresenter.Parent = parent;
      contentPresenter.Name = "e_64";
      contentPresenter.VerticalAlignment = VerticalAlignment.Center;
      contentPresenter.SetBinding(ContentPresenter.ContentProperty, new Binding("Content")
      {
        Source = (object) parent
      });
      return (UIElement) contentPresenter;
    }

    private static UIElement r_141_s_S_4_ctMethod(UIElement parent)
    {
      Grid grid = new Grid();
      grid.Parent = parent;
      grid.Name = "e_63";
      grid.SnapsToDevicePixels = false;
      grid.UseLayoutRounding = true;
      grid.RowDefinitions.Add(new RowDefinition());
      grid.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(16f, GridUnitType.Pixel)
      });
      grid.ColumnDefinitions.Add(new ColumnDefinition());
      grid.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(38f, GridUnitType.Pixel)
      });
      Border border1 = new Border();
      grid.Children.Add((UIElement) border1);
      border1.Name = "PART_ComboBoxLeft";
      border1.Height = 38f;
      border1.IsHitTestVisible = false;
      border1.SnapsToDevicePixels = false;
      border1.SetResourceReference(Control.BackgroundProperty, (object) "ComboBoxBackgroundLeft");
      Border border2 = new Border();
      grid.Children.Add((UIElement) border2);
      border2.Name = "PART_ComboBoxCenter";
      border2.IsHitTestVisible = false;
      border2.SnapsToDevicePixels = false;
      Grid.SetColumn((UIElement) border2, 1);
      border2.SetResourceReference(Control.BackgroundProperty, (object) "ComboBoxBackgroundCenter");
      Border border3 = new Border();
      grid.Children.Add((UIElement) border3);
      border3.Name = "PART_ComboBoxRight";
      border3.Height = 38f;
      border3.IsHitTestVisible = false;
      border3.SnapsToDevicePixels = false;
      Grid.SetColumn((UIElement) border3, 2);
      border3.SetResourceReference(Control.BackgroundProperty, (object) "ComboBoxBackgroundRight");
      ToggleButton toggleButton = new ToggleButton();
      grid.Children.Add((UIElement) toggleButton);
      toggleButton.Name = "PART_Button";
      toggleButton.Focusable = false;
      Style style = new Style(typeof (ToggleButton));
      ControlTemplate controlTemplate = new ControlTemplate(typeof (ToggleButton), new Func<UIElement, UIElement>(Styles.PART_Button_s_S_0_ctMethod));
      Setter setter = new Setter(Control.TemplateProperty, (object) controlTemplate);
      style.Setters.Add(setter);
      toggleButton.Style = style;
      toggleButton.IsTabStop = false;
      toggleButton.ClickMode = ClickMode.Press;
      Grid.SetColumnSpan((UIElement) toggleButton, 3);
      toggleButton.SetBinding(ToggleButton.IsCheckedProperty, new Binding("IsDropDownOpen")
      {
        Source = (object) parent
      });
      ContentPresenter contentPresenter = new ContentPresenter();
      toggleButton.Content = (object) contentPresenter;
      contentPresenter.Name = "e_65";
      contentPresenter.IsHitTestVisible = false;
      contentPresenter.Margin = new Thickness(4f, 0.0f, 40f, 0.0f);
      contentPresenter.SetBinding(UIElement.DataContextProperty, new Binding("SelectionBoxItem")
      {
        Source = (object) parent
      });
      contentPresenter.SetBinding(UIElement.VerticalAlignmentProperty, new Binding("VerticalContentAlignment")
      {
        Source = (object) parent
      });
      contentPresenter.SetBinding(ContentPresenter.ContentProperty, new Binding("SelectionBoxItem")
      {
        Source = (object) parent
      });
      contentPresenter.SetBinding(ContentPresenter.ContentTemplateProperty, new Binding("SelectionBoxItemTemplate")
      {
        Source = (object) parent
      });
      Popup popup = new Popup();
      grid.Children.Add((UIElement) popup);
      popup.Name = "PART_ComboBoxPopup";
      Grid.SetRow((UIElement) popup, 1);
      Grid.SetColumnSpan((UIElement) popup, 3);
      popup.SetBinding(UIElement.MaxHeightProperty, new Binding("MaxDropDownHeight")
      {
        Source = (object) parent
      });
      popup.SetBinding(Popup.IsOpenProperty, new Binding("IsDropDownOpen")
      {
        Source = (object) parent
      });
      ScrollViewer scrollViewer = new ScrollViewer();
      popup.Child = (UIElement) scrollViewer;
      scrollViewer.Name = "PART_DataScrollViewer";
      scrollViewer.HorizontalContentAlignment = HorizontalAlignment.Stretch;
      scrollViewer.VerticalContentAlignment = VerticalAlignment.Stretch;
      scrollViewer.SetResourceReference(UIElement.StyleProperty, (object) "ScrollViewerStyle");
      StackPanel stackPanel = new StackPanel();
      scrollViewer.Content = (object) stackPanel;
      stackPanel.Name = "e_66";
      stackPanel.Margin = new Thickness(10f, 0.0f, 0.0f, 0.0f);
      stackPanel.IsItemsHost = true;
      return (UIElement) grid;
    }

    private static UIElement r_142_s_S_0_ctMethod(UIElement parent)
    {
      Border border = new Border();
      border.Parent = parent;
      border.Name = "e_67";
      border.SetBinding(Control.BackgroundProperty, new Binding("Background")
      {
        Source = (object) parent
      });
      ContentPresenter contentPresenter = new ContentPresenter();
      border.Child = (UIElement) contentPresenter;
      contentPresenter.Name = "e_68";
      contentPresenter.SetBinding(UIElement.HorizontalAlignmentProperty, new Binding("HorizontalContentAlignment")
      {
        Source = (object) parent
      });
      contentPresenter.SetBinding(UIElement.VerticalAlignmentProperty, new Binding("VerticalContentAlignment")
      {
        Source = (object) parent
      });
      contentPresenter.SetBinding(ContentPresenter.ContentProperty, new Binding("Content")
      {
        Source = (object) parent
      });
      return (UIElement) border;
    }

    private static UIElement e_71_s_S_0_ctMethod(UIElement parent)
    {
      Border border = new Border();
      border.Parent = parent;
      border.Name = "e_72";
      return (UIElement) border;
    }

    private static UIElement r_147_s_T_0_S_0_ctMethod(UIElement parent)
    {
      Grid grid = new Grid();
      grid.Parent = parent;
      grid.Name = "e_69";
      Track track = new Track();
      grid.Children.Add((UIElement) track);
      track.Name = "PART_Track";
      track.IsDirectionReversed = true;
      RepeatButton repeatButton1 = new RepeatButton();
      repeatButton1.Name = "e_70";
      repeatButton1.IsTabStop = false;
      repeatButton1.Command = (ICommand) ScrollBar.PageDownCommand;
      repeatButton1.ClickMode = ClickMode.Press;
      repeatButton1.SetResourceReference(UIElement.StyleProperty, (object) "ScrollBarPageButton");
      track.IncreaseRepeatButton = repeatButton1;
      RepeatButton repeatButton2 = new RepeatButton();
      repeatButton2.Name = "e_71";
      Style style = new Style(typeof (RepeatButton));
      ControlTemplate controlTemplate = new ControlTemplate(new Func<UIElement, UIElement>(Styles.e_71_s_S_0_ctMethod));
      Setter setter = new Setter(Control.TemplateProperty, (object) controlTemplate);
      style.Setters.Add(setter);
      repeatButton2.Style = style;
      repeatButton2.IsTabStop = false;
      repeatButton2.Command = (ICommand) ScrollBar.PageUpCommand;
      repeatButton2.ClickMode = ClickMode.Press;
      track.DecreaseRepeatButton = repeatButton2;
      Thumb thumb = new Thumb();
      thumb.Name = "e_73";
      thumb.SetResourceReference(UIElement.StyleProperty, (object) "VerticalThumb");
      track.Thumb = thumb;
      return (UIElement) grid;
    }

    private static UIElement e_76_s_S_0_ctMethod(UIElement parent)
    {
      Border border = new Border();
      border.Parent = parent;
      border.Name = "e_77";
      return (UIElement) border;
    }

    private static UIElement r_147_s_T_1_S_0_ctMethod(UIElement parent)
    {
      Grid grid = new Grid();
      grid.Parent = parent;
      grid.Name = "e_74";
      Track track = new Track();
      grid.Children.Add((UIElement) track);
      track.Name = "PART_Track";
      track.IsDirectionReversed = false;
      RepeatButton repeatButton1 = new RepeatButton();
      repeatButton1.Name = "e_75";
      repeatButton1.IsTabStop = false;
      repeatButton1.Command = (ICommand) ScrollBar.PageRightCommand;
      repeatButton1.ClickMode = ClickMode.Press;
      repeatButton1.SetResourceReference(UIElement.StyleProperty, (object) "ScrollBarPageButton");
      track.IncreaseRepeatButton = repeatButton1;
      RepeatButton repeatButton2 = new RepeatButton();
      repeatButton2.Name = "e_76";
      Style style = new Style(typeof (RepeatButton));
      ControlTemplate controlTemplate = new ControlTemplate(new Func<UIElement, UIElement>(Styles.e_76_s_S_0_ctMethod));
      Setter setter = new Setter(Control.TemplateProperty, (object) controlTemplate);
      style.Setters.Add(setter);
      repeatButton2.Style = style;
      repeatButton2.IsTabStop = false;
      repeatButton2.Command = (ICommand) ScrollBar.PageLeftCommand;
      repeatButton2.ClickMode = ClickMode.Press;
      track.DecreaseRepeatButton = repeatButton2;
      Thumb thumb = new Thumb();
      thumb.Name = "e_78";
      thumb.SetResourceReference(UIElement.StyleProperty, (object) "HorizontalThumb");
      track.Thumb = thumb;
      return (UIElement) grid;
    }

    private static UIElement r_150_s_S_2_ctMethod(UIElement parent)
    {
      Border border = new Border();
      border.Parent = parent;
      border.Name = "e_79";
      border.SnapsToDevicePixels = true;
      border.Padding = new Thickness(5f, 5f, 5f, 5f);
      border.SetBinding(Control.BackgroundProperty, new Binding("Background")
      {
        Source = (object) parent
      });
      ContentPresenter contentPresenter = new ContentPresenter();
      border.Child = (UIElement) contentPresenter;
      contentPresenter.Name = "e_80";
      contentPresenter.Margin = new Thickness(10f, 2f, 10f, 2f);
      contentPresenter.HorizontalAlignment = HorizontalAlignment.Center;
      contentPresenter.VerticalAlignment = VerticalAlignment.Center;
      contentPresenter.SetBinding(ContentPresenter.ContentProperty, new Binding("Header")
      {
        Source = (object) parent
      });
      return (UIElement) border;
    }

    private static UIElement r_151_s_S_2_ctMethod(UIElement parent)
    {
      Grid grid = new Grid();
      grid.Parent = parent;
      grid.Name = "e_81";
      grid.SnapsToDevicePixels = false;
      grid.UseLayoutRounding = true;
      grid.RowDefinitions.Add(new RowDefinition()
      {
        MinHeight = 36f
      });
      grid.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(4f, GridUnitType.Pixel)
      });
      grid.ColumnDefinitions.Add(new ColumnDefinition());
      grid.ColumnDefinitions.Add(new ColumnDefinition()
      {
        Width = new GridLength(4f, GridUnitType.Pixel)
      });
      Border border1 = new Border();
      grid.Children.Add((UIElement) border1);
      border1.Name = "PART_TextBoxLeft";
      border1.IsHitTestVisible = false;
      border1.SetResourceReference(Control.BackgroundProperty, (object) "TextBoxLeft");
      Border border2 = new Border();
      grid.Children.Add((UIElement) border2);
      border2.Name = "PART_TextBoxCenter";
      border2.IsHitTestVisible = false;
      Grid.SetColumn((UIElement) border2, 1);
      border2.SetResourceReference(Control.BackgroundProperty, (object) "TextBoxCenter");
      Border border3 = new Border();
      grid.Children.Add((UIElement) border3);
      border3.Name = "PART_TextBoxRight";
      border3.IsHitTestVisible = false;
      Grid.SetColumn((UIElement) border3, 2);
      border3.SetResourceReference(Control.BackgroundProperty, (object) "TextBoxRight");
      ScrollViewer scrollViewer = new ScrollViewer();
      grid.Children.Add((UIElement) scrollViewer);
      scrollViewer.Name = "PART_ScrollViewer";
      scrollViewer.HorizontalContentAlignment = HorizontalAlignment.Stretch;
      scrollViewer.VerticalContentAlignment = VerticalAlignment.Stretch;
      Grid.SetColumn((UIElement) scrollViewer, 1);
      scrollViewer.SetResourceReference(UIElement.StyleProperty, (object) "TextBoxScrollViewer");
      TextBlock textBlock = new TextBlock();
      scrollViewer.Content = (object) textBlock;
      textBlock.Name = "e_82";
      textBlock.VerticalAlignment = VerticalAlignment.Center;
      textBlock.SetBinding(UIElement.HorizontalAlignmentProperty, new Binding("HorizontalContentAlignment")
      {
        Source = (object) parent
      });
      textBlock.SetBinding(Control.PaddingProperty, new Binding("Padding")
      {
        Source = (object) parent
      });
      textBlock.SetBinding(TextBlock.TextAlignmentProperty, new Binding("TextAlignment")
      {
        Source = (object) parent
      });
      textBlock.SetBinding(TextBlock.TextProperty, new Binding("Text")
      {
        Source = (object) parent
      });
      return (UIElement) grid;
    }

    private static UIElement r_153_s_S_0_ctMethod(UIElement parent)
    {
      Grid grid = new Grid();
      grid.Parent = parent;
      grid.Name = "e_83";
      grid.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid.RowDefinitions.Add(new RowDefinition());
      grid.RowDefinitions.Add(new RowDefinition()
      {
        Height = new GridLength(1f, GridUnitType.Auto)
      });
      grid.SetResourceReference(Control.BackgroundProperty, (object) "WindowBackgroundBrush");
      Border border1 = new Border();
      grid.Children.Add((UIElement) border1);
      border1.Name = "PART_WindowTitleBorder";
      ContentPresenter contentPresenter1 = new ContentPresenter();
      border1.Child = (UIElement) contentPresenter1;
      contentPresenter1.Name = "PART_WindowTitle";
      contentPresenter1.IsHitTestVisible = false;
      contentPresenter1.HorizontalAlignment = HorizontalAlignment.Center;
      contentPresenter1.VerticalAlignment = VerticalAlignment.Center;
      contentPresenter1.SetBinding(ContentPresenter.ContentProperty, new Binding("Title")
      {
        Source = (object) parent
      });
      ScrollViewer scrollViewer = new ScrollViewer();
      grid.Children.Add((UIElement) scrollViewer);
      scrollViewer.Name = "e_84";
      scrollViewer.Margin = new Thickness(20f, 20f, 20f, 20f);
      scrollViewer.HorizontalContentAlignment = HorizontalAlignment.Stretch;
      scrollViewer.VerticalContentAlignment = VerticalAlignment.Stretch;
      scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
      scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
      Grid.SetRow((UIElement) scrollViewer, 1);
      ContentPresenter contentPresenter2 = new ContentPresenter();
      scrollViewer.Content = (object) contentPresenter2;
      contentPresenter2.Name = "e_85";
      contentPresenter2.SetBinding(ContentPresenter.ContentProperty, new Binding("Content")
      {
        Source = (object) parent
      });
      Border border2 = new Border();
      grid.Children.Add((UIElement) border2);
      border2.Name = "PART_WindowResizeBorder";
      border2.Height = 16f;
      border2.Width = 16f;
      border2.HorizontalAlignment = HorizontalAlignment.Right;
      border2.Background = (Brush) new SolidColorBrush(new ColorW(0, 0, 0, (int) byte.MaxValue));
      Grid.SetRow((UIElement) border2, 2);
      return (UIElement) grid;
    }

    private static UIElement r_163_s_S_1_ctMethod(UIElement parent)
    {
      ScrollContentPresenter contentPresenter = new ScrollContentPresenter();
      contentPresenter.Parent = parent;
      contentPresenter.Name = "PART_ScrollContentPresenter";
      contentPresenter.SetBinding(UIElement.MarginProperty, new Binding("Padding")
      {
        Source = (object) parent
      });
      contentPresenter.SetBinding(UIElement.HorizontalAlignmentProperty, new Binding("HorizontalContentAlignment")
      {
        Source = (object) parent
      });
      contentPresenter.SetBinding(UIElement.VerticalAlignmentProperty, new Binding("VerticalContentAlignment")
      {
        Source = (object) parent
      });
      contentPresenter.SetBinding(ContentPresenter.ContentProperty, new Binding("Content")
      {
        Source = (object) parent
      });
      return (UIElement) contentPresenter;
    }

    private static UIElement r_169_s_S_0_ctMethod(UIElement parent)
    {
      Grid grid = new Grid();
      grid.Parent = parent;
      grid.Name = "e_86";
      grid.Margin = new Thickness(0.0f, 2f, 0.0f, 2f);
      grid.RowDefinitions.Add(new RowDefinition());
      Border border = new Border();
      grid.Children.Add((UIElement) border);
      border.Name = "PART_Center";
      Grid.SetRow((UIElement) border, 0);
      border.SetResourceReference(Control.BackgroundProperty, (object) "ScrollbarVerticalThumbCenter");
      return (UIElement) grid;
    }
  }
}
