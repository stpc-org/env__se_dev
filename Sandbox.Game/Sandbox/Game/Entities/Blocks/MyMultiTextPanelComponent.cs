// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.MyMultiTextPanelComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ObjectBuilders;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Blocks
{
  public class MyMultiTextPanelComponent : IMyTextSurfaceProvider, IMyTextPanelProvider
  {
    private List<MyTextPanelComponent> m_panels = new List<MyTextPanelComponent>();
    private MyRenderComponentScreenAreas m_render;
    private MyTerminalBlock m_block;
    private int m_selectedPanel;
    private bool m_isOutofRange;
    private bool m_wasInRange;
    private Action<int, int[]> m_addImagesToSelectionRequest;
    private Action<int, int[]> m_removeImagesFromSelectionRequest;
    private Action<int, string> m_changeTextRequest;
    private Action<int, MySerializableSpriteCollection> m_updateSpriteCollection;
    private float m_maxRenderDistanceSquared;
    private MySessionComponentPanels m_panelsComponent;
    private bool m_texturesReleased = true;

    public MyTextPanelComponent PanelComponent => this.m_panels.Count != 0 ? this.m_panels[this.m_selectedPanel] : (MyTextPanelComponent) null;

    public int SurfaceCount => this.m_panels == null ? 0 : this.m_panels.Count;

    public int SelectedPanelIndex => this.m_selectedPanel;

    public MyTextPanelComponent GetPanelComponent(int panelIndex) => this.m_panels[panelIndex];

    public static void CreateTerminalControls<T>() where T : MyTerminalBlock, IMyTextSurfaceProvider, IMyMultiTextPanelComponentOwner
    {
      MyTerminalControlListbox<T> terminalControlListbox1 = new MyTerminalControlListbox<T>("PanelList", MyStringId.GetOrCompute("LCD Panels"), MySpaceTexts.Blank);
      terminalControlListbox1.ListContent = (MyTerminalControlListbox<T>.ListContentDelegate) ((x, list1, list2, focusedItem) => MyMultiTextPanelComponent.FillPanels(x.MultiTextPanel, list1, list2, focusedItem));
      terminalControlListbox1.ItemSelected = (MyTerminalControlListbox<T>.SelectItemDelegate) ((x, y) => x.SelectPanel(y));
      terminalControlListbox1.Visible = (Func<T, bool>) (x => x.SurfaceCount > 1);
      terminalControlListbox1.Enabled = (Func<T, bool>) (x => x.SurfaceCount > 0);
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) terminalControlListbox1);
      MyTerminalControlCombobox<T> terminalControlCombobox1 = new MyTerminalControlCombobox<T>("Content", MySpaceTexts.BlockPropertyTitle_PanelContent, MySpaceTexts.Blank);
      terminalControlCombobox1.Visible = (Func<T, bool>) (x => x.SurfaceCount > 0);
      terminalControlCombobox1.Enabled = (Func<T, bool>) (x => x.SurfaceCount > 0);
      terminalControlCombobox1.ComboBoxContent = (Action<List<MyTerminalControlComboBoxItem>>) (x => MyTextPanelComponent.FillContentComboBoxContent(x));
      terminalControlCombobox1.Getter = (MyTerminalValueControl<T, long>.GetterDelegate) (x => x.PanelComponent == null ? 0L : (long) x.PanelComponent.ContentType);
      terminalControlCombobox1.Setter = (MyTerminalValueControl<T, long>.SetterDelegate) ((x, y) =>
      {
        if (x.PanelComponent == null)
          return;
        x.PanelComponent.ContentType = (ContentType) y;
      });
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) terminalControlCombobox1);
      MyTerminalControlSeparator<T> controlSeparator1 = new MyTerminalControlSeparator<T>();
      controlSeparator1.Visible = (Func<T, bool>) (x => x.SurfaceCount > 0 && (uint) x.PanelComponent.ContentType > 0U);
      controlSeparator1.Enabled = (Func<T, bool>) (x => x.SurfaceCount > 0);
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) controlSeparator1);
      MyTerminalControlListbox<T> terminalControlListbox2 = new MyTerminalControlListbox<T>("Script", MySpaceTexts.BlockPropertyTitle_PanelScript, MySpaceTexts.Blank);
      terminalControlListbox2.Visible = (Func<T, bool>) (x => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.SCRIPT);
      terminalControlListbox2.Enabled = (Func<T, bool>) (x => x.SurfaceCount > 0);
      terminalControlListbox2.ListContent = (MyTerminalControlListbox<T>.ListContentDelegate) ((x, list1, list2, focusedItem) =>
      {
        if (x.PanelComponent == null)
          return;
        x.PanelComponent.FillScriptsContent(list1, list2, focusedItem);
      });
      terminalControlListbox2.ItemSelected = (MyTerminalControlListbox<T>.SelectItemDelegate) ((x, y) =>
      {
        if (x.PanelComponent == null)
          return;
        x.PanelComponent.SelectScriptToDraw(y);
      });
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) terminalControlListbox2);
      MyTerminalControlColor<T> terminalControlColor1 = new MyTerminalControlColor<T>("ScriptForegroundColor", MySpaceTexts.BlockPropertyTitle_FontColor);
      terminalControlColor1.Visible = (Func<T, bool>) (x => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.SCRIPT);
      terminalControlColor1.Enabled = (Func<T, bool>) (x => x.SurfaceCount > 0);
      terminalControlColor1.Getter = (MyTerminalValueControl<T, Color>.GetterDelegate) (x => x.PanelComponent == null ? Color.White : x.PanelComponent.ScriptForegroundColor);
      terminalControlColor1.Setter = (MyTerminalValueControl<T, Color>.SetterDelegate) ((x, v) =>
      {
        if (x.PanelComponent == null)
          return;
        x.PanelComponent.ScriptForegroundColor = v;
      });
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) terminalControlColor1);
      MyTerminalControlColor<T> terminalControlColor2 = new MyTerminalControlColor<T>("ScriptBackgroundColor", MySpaceTexts.BlockPropertyTitle_BackgroundColor, true, 0.055f, true);
      terminalControlColor2.Visible = (Func<T, bool>) (x => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.SCRIPT);
      terminalControlColor2.Enabled = (Func<T, bool>) (x => x.SurfaceCount > 0);
      terminalControlColor2.Getter = (MyTerminalValueControl<T, Color>.GetterDelegate) (x => x.PanelComponent == null ? Color.Black : x.PanelComponent.ScriptBackgroundColor);
      terminalControlColor2.Setter = (MyTerminalValueControl<T, Color>.SetterDelegate) ((x, v) =>
      {
        if (x.PanelComponent == null)
          return;
        x.PanelComponent.ScriptBackgroundColor = v;
      });
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) terminalControlColor2);
      MyTerminalControlButton<T> terminalControlButton1 = new MyTerminalControlButton<T>("ShowTextPanel", MySpaceTexts.BlockPropertyTitle_TextPanelShowPublicTextPanel, MySpaceTexts.Blank, (Action<T>) (x => x.OpenWindow(true, true, true)));
      terminalControlButton1.Visible = (Func<T, bool>) (x => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE);
      terminalControlButton1.Enabled = (Func<T, bool>) (x => x.SurfaceCount > 0 && !x.IsTextPanelOpen);
      terminalControlButton1.SupportsMultipleBlocks = false;
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) terminalControlButton1);
      MyTerminalControlCombobox<T> terminalControlCombobox2 = new MyTerminalControlCombobox<T>("Font", MySpaceTexts.BlockPropertyTitle_Font, MySpaceTexts.Blank);
      terminalControlCombobox2.Visible = (Func<T, bool>) (x => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE);
      terminalControlCombobox2.Enabled = (Func<T, bool>) (x => x.SurfaceCount > 0);
      terminalControlCombobox2.ComboBoxContent = (Action<List<MyTerminalControlComboBoxItem>>) (x => MyTextPanelComponent.FillFontComboBoxContent(x));
      terminalControlCombobox2.Getter = (MyTerminalValueControl<T, long>.GetterDelegate) (x => x.PanelComponent == null ? 0L : (long) (int) x.PanelComponent.Font.SubtypeId);
      terminalControlCombobox2.Setter = (MyTerminalValueControl<T, long>.SetterDelegate) ((x, y) =>
      {
        if (x.PanelComponent == null)
          return;
        x.PanelComponent.Font = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_FontDefinition), MyStringHash.TryGet((int) y));
      });
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) terminalControlCombobox2);
      MyTerminalControlSlider<T> slider1 = new MyTerminalControlSlider<T>("FontSize", MySpaceTexts.BlockPropertyTitle_LCDScreenTextSize, MySpaceTexts.Blank);
      slider1.Visible = (Func<T, bool>) (x => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE);
      slider1.Enabled = (Func<T, bool>) (x => x.SurfaceCount > 0);
      slider1.SetLimits(0.1f, 10f);
      slider1.DefaultValue = new float?(1f);
      slider1.Getter = (MyTerminalValueControl<T, float>.GetterDelegate) (x => x.PanelComponent == null ? 1f : x.PanelComponent.FontSize);
      slider1.Setter = (MyTerminalValueControl<T, float>.SetterDelegate) ((x, v) =>
      {
        if (x.PanelComponent == null)
          return;
        x.PanelComponent.FontSize = v;
      });
      slider1.Writer = (MyTerminalControl<T>.WriterDelegate) ((x, result) =>
      {
        if (x.PanelComponent == null)
          return;
        result.Append(MyValueFormatter.GetFormatedFloat(x.PanelComponent.FontSize, 3));
      });
      slider1.EnableActions<T>(enabled: ((Func<T, bool>) (x => x.SurfaceCount > 0)), callable: ((Func<T, bool>) (x => x.SurfaceCount > 0)));
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) slider1);
      MyTerminalControlColor<T> terminalControlColor3 = new MyTerminalControlColor<T>("FontColor", MySpaceTexts.BlockPropertyTitle_FontColor);
      terminalControlColor3.Visible = (Func<T, bool>) (x => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE);
      terminalControlColor3.Enabled = (Func<T, bool>) (x => x.SurfaceCount > 0);
      terminalControlColor3.Getter = (MyTerminalValueControl<T, Color>.GetterDelegate) (x => x.PanelComponent == null ? Color.White : x.PanelComponent.FontColor);
      terminalControlColor3.Setter = (MyTerminalValueControl<T, Color>.SetterDelegate) ((x, v) =>
      {
        if (x.PanelComponent == null)
          return;
        x.PanelComponent.FontColor = v;
      });
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) terminalControlColor3);
      MyTerminalControlCombobox<T> terminalControlCombobox3 = new MyTerminalControlCombobox<T>("alignment", MySpaceTexts.BlockPropertyTitle_Alignment, MySpaceTexts.Blank);
      terminalControlCombobox3.Visible = (Func<T, bool>) (x => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE);
      terminalControlCombobox3.Enabled = (Func<T, bool>) (x => x.SurfaceCount > 0);
      terminalControlCombobox3.ComboBoxContent = (Action<List<MyTerminalControlComboBoxItem>>) (x => MyTextPanelComponent.FillAlignmentComboBoxContent(x));
      terminalControlCombobox3.Getter = (MyTerminalValueControl<T, long>.GetterDelegate) (x => x.PanelComponent == null ? 0L : (long) x.PanelComponent.Alignment);
      terminalControlCombobox3.Setter = (MyTerminalValueControl<T, long>.SetterDelegate) ((x, y) =>
      {
        if (x.PanelComponent == null)
          return;
        x.PanelComponent.Alignment = (TextAlignment) y;
      });
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) terminalControlCombobox3);
      MyTerminalControlSlider<T> slider2 = new MyTerminalControlSlider<T>("TextPaddingSlider", MySpaceTexts.BlockPropertyTitle_LCDScreenTextPadding, MySpaceTexts.Blank);
      slider2.Visible = (Func<T, bool>) (x => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE);
      slider2.Enabled = (Func<T, bool>) (x => x.SurfaceCount > 0);
      slider2.SetLimits(0.0f, 50f);
      slider2.DefaultValue = new float?(0.0f);
      slider2.Getter = (MyTerminalValueControl<T, float>.GetterDelegate) (x => x.PanelComponent == null ? 0.0f : x.PanelComponent.TextPadding);
      slider2.Setter = (MyTerminalValueControl<T, float>.SetterDelegate) ((x, v) =>
      {
        if (x.PanelComponent == null)
          return;
        x.PanelComponent.TextPadding = v;
      });
      slider2.Writer = (MyTerminalControl<T>.WriterDelegate) ((x, result) =>
      {
        if (x.PanelComponent == null)
          return;
        result.Append(MyValueFormatter.GetFormatedFloat(x.PanelComponent.TextPadding, 1)).Append("%");
      });
      slider2.EnableActions<T>(enabled: ((Func<T, bool>) (x => x.SurfaceCount > 0)), callable: ((Func<T, bool>) (x => x.SurfaceCount > 0)));
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) slider2);
      MyTerminalControlSeparator<T> controlSeparator2 = new MyTerminalControlSeparator<T>();
      controlSeparator2.Visible = (Func<T, bool>) (x =>
      {
        if (x.SurfaceCount <= 0)
          return false;
        return x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE || x.PanelComponent.ContentType == ContentType.SCRIPT;
      });
      controlSeparator2.Enabled = (Func<T, bool>) (x => x.SurfaceCount > 0);
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) controlSeparator2);
      MyTerminalControlColor<T> terminalControlColor4 = new MyTerminalControlColor<T>("BackgroundColor", MySpaceTexts.BlockPropertyTitle_BackgroundColor, true, 0.055f, true);
      terminalControlColor4.Visible = (Func<T, bool>) (x => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE);
      terminalControlColor4.Enabled = (Func<T, bool>) (x => x.SurfaceCount > 0);
      terminalControlColor4.Getter = (MyTerminalValueControl<T, Color>.GetterDelegate) (x => x.PanelComponent == null ? Color.Black : x.PanelComponent.BackgroundColor);
      terminalControlColor4.Setter = (MyTerminalValueControl<T, Color>.SetterDelegate) ((x, v) =>
      {
        if (x.PanelComponent == null)
          return;
        x.PanelComponent.BackgroundColor = v;
      });
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) terminalControlColor4);
      MyTerminalControlListbox<T> terminalControlListbox3 = new MyTerminalControlListbox<T>("ImageList", MySpaceTexts.BlockPropertyTitle_LCDScreenDefinitionsTextures, MySpaceTexts.Blank, true);
      terminalControlListbox3.Visible = (Func<T, bool>) (x => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE);
      terminalControlListbox3.Enabled = (Func<T, bool>) (x => x.SurfaceCount > 0);
      terminalControlListbox3.ListContent = (MyTerminalControlListbox<T>.ListContentDelegate) ((x, list1, list2, focusedItem) =>
      {
        if (x.PanelComponent == null)
          return;
        x.PanelComponent.FillListContent(list1, list2);
      });
      terminalControlListbox3.ItemSelected = (MyTerminalControlListbox<T>.SelectItemDelegate) ((x, y) => x.PanelComponent.SelectImageToDraw(y));
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) terminalControlListbox3);
      MyTerminalControlButton<T> terminalControlButton2 = new MyTerminalControlButton<T>("SelectTextures", MySpaceTexts.BlockPropertyTitle_LCDScreenSelectTextures, MySpaceTexts.Blank, (Action<T>) (x => x.PanelComponent.AddImagesToSelection()));
      terminalControlButton2.Visible = (Func<T, bool>) (x => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE);
      terminalControlButton2.Enabled = (Func<T, bool>) (x => x.SurfaceCount > 0);
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) terminalControlButton2);
      MyTerminalControlSlider<T> slider3 = new MyTerminalControlSlider<T>("ChangeIntervalSlider", MySpaceTexts.BlockPropertyTitle_LCDScreenRefreshInterval, MySpaceTexts.Blank, true, true);
      slider3.Visible = (Func<T, bool>) (x => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE);
      slider3.Enabled = (Func<T, bool>) (x => x.SurfaceCount > 0);
      slider3.SetLimits(0.0f, 30f);
      slider3.DefaultValue = new float?(0.0f);
      slider3.Getter = (MyTerminalValueControl<T, float>.GetterDelegate) (x => x.PanelComponent == null ? 0.0f : x.PanelComponent.ChangeInterval);
      slider3.Setter = (MyTerminalValueControl<T, float>.SetterDelegate) ((x, v) =>
      {
        if (x.PanelComponent == null)
          return;
        x.PanelComponent.ChangeInterval = v;
      });
      slider3.Writer = (MyTerminalControl<T>.WriterDelegate) ((x, result) =>
      {
        if (x.PanelComponent == null)
          return;
        result.Append(MyValueFormatter.GetFormatedFloat(x.PanelComponent.ChangeInterval, 3)).Append(" s");
      });
      slider3.EnableActions<T>(enabled: ((Func<T, bool>) (x => x.SurfaceCount > 0)), callable: ((Func<T, bool>) (x => x.SurfaceCount > 0)));
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) slider3);
      MyTerminalControlListbox<T> terminalControlListbox4 = new MyTerminalControlListbox<T>("SelectedImageList", MySpaceTexts.BlockPropertyTitle_LCDScreenSelectedTextures, MySpaceTexts.Blank, true);
      terminalControlListbox4.Visible = (Func<T, bool>) (x => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE);
      terminalControlListbox4.Enabled = (Func<T, bool>) (x => x.SurfaceCount > 0);
      terminalControlListbox4.ListContent = (MyTerminalControlListbox<T>.ListContentDelegate) ((x, list1, list2, focusedItem) =>
      {
        if (x.PanelComponent == null)
          return;
        x.PanelComponent.FillSelectedListContent(list1, list2);
      });
      terminalControlListbox4.ItemSelected = (MyTerminalControlListbox<T>.SelectItemDelegate) ((x, y) =>
      {
        if (x.PanelComponent == null)
          return;
        x.PanelComponent.SelectImage(y);
      });
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) terminalControlListbox4);
      MyTerminalControlButton<T> terminalControlButton3 = new MyTerminalControlButton<T>("RemoveSelectedTextures", MySpaceTexts.BlockPropertyTitle_LCDScreenRemoveSelectedTextures, MySpaceTexts.Blank, (Action<T>) (x => x.PanelComponent.RemoveImagesFromSelection()));
      terminalControlButton3.Visible = (Func<T, bool>) (x => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE);
      terminalControlButton3.Enabled = (Func<T, bool>) (x => x.SurfaceCount > 0);
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) terminalControlButton3);
      MyTerminalControlCheckbox<T> checkbox = new MyTerminalControlCheckbox<T>("PreserveAspectRatio", MySpaceTexts.BlockPropertyTitle_LCDScreenPreserveAspectRatio, MySpaceTexts.BlockPropertyTitle_LCDScreenPreserveAspectRatio);
      checkbox.Getter = (MyTerminalValueControl<T, bool>.GetterDelegate) (x => x.PanelComponent != null && x.PanelComponent.PreserveAspectRatio);
      checkbox.Setter = (MyTerminalValueControl<T, bool>.SetterDelegate) ((x, v) =>
      {
        if (x.PanelComponent == null)
          return;
        x.PanelComponent.PreserveAspectRatio = v;
      });
      checkbox.Visible = (Func<T, bool>) (x => x.SurfaceCount > 0 && x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE);
      checkbox.Enabled = (Func<T, bool>) (x => x.SurfaceCount > 0);
      checkbox.EnableAction<T>((Func<T, bool>) (x => x.SurfaceCount > 0));
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) checkbox);
    }

    private static void FillPanels(
      MyMultiTextPanelComponent multiText,
      ICollection<MyGuiControlListbox.Item> listBoxContent,
      ICollection<MyGuiControlListbox.Item> listBoxSelectedItems,
      ICollection<MyGuiControlListbox.Item> lastFocused)
    {
      MyGuiControlListbox.Item obj1 = (MyGuiControlListbox.Item) null;
      listBoxContent.Clear();
      listBoxSelectedItems.Clear();
      if (multiText == null)
        return;
      for (int index = 0; index < multiText.m_panels.Count; ++index)
      {
        MyGuiControlListbox.Item obj2 = new MyGuiControlListbox.Item(new StringBuilder(multiText.m_panels[index].DisplayName), userData: ((object) index));
        listBoxContent.Add(obj2);
        if (multiText.m_selectedPanel == index)
        {
          listBoxSelectedItems.Add(obj2);
          obj1 = obj2;
        }
      }
      if (obj1 == null)
        return;
      lastFocused.Add(obj1);
    }

    public MyMultiTextPanelComponent(
      MyTerminalBlock block,
      List<ScreenArea> screens,
      List<MySerializedTextPanelData> panels)
    {
      this.m_block = block;
      this.m_block.OnClosing += new Action<MyEntity>(this.OnClosing);
      this.m_render = block.Render as MyRenderComponentScreenAreas;
      if (screens.Count <= 0)
        return;
      this.m_panels = new List<MyTextPanelComponent>();
      for (int index = 0; index < screens.Count; ++index)
      {
        ScreenArea screen = screens[index];
        string displayName = MyTexts.GetString(screen.DisplayName);
        MyTextPanelComponent textPanelComponent = new MyTextPanelComponent(index, block, screen.Name, displayName, screen.TextureResolution, screen.ScreenWidth, screen.ScreenHeight, false);
        this.m_panels.Add(textPanelComponent);
        block.SyncType.Append((object) textPanelComponent);
        textPanelComponent.Init(panels == null || panels.Count <= index ? new MySerializableSpriteCollection() : panels[index].Sprites, screen.Script, new Action<MyTextPanelComponent, int[]>(this.AddImagesRequest), new Action<MyTextPanelComponent, int[]>(this.RemoveImagesRequest), new Action<MyTextPanelComponent, string>(this.ChangeTextRequest), new Action<MyTextPanelComponent, MySerializableSpriteCollection>(this.SpriteCollectionUpdate));
      }
      if (panels == null)
        return;
      MyDefinitionManager.Static.GetLCDTexturesDefinitions();
      for (int index = 0; index < panels.Count && index < screens.Count; ++index)
        this.SetPanelData(panels[index], index);
    }

    public void SetPanelData(MySerializedTextPanelData serializedData, int panelIndex)
    {
      MyTextPanelComponent.ContentMetadata content = new MyTextPanelComponent.ContentMetadata()
      {
        ContentType = serializedData.ContentType,
        BackgroundColor = serializedData.BackgroundColor,
        ChangeInterval = serializedData.ChangeInterval,
        PreserveAspectRatio = serializedData.PreserveAspectRatio,
        TextPadding = serializedData.TextPadding
      };
      MyTextPanelComponent.FontData font = new MyTextPanelComponent.FontData()
      {
        Alignment = (TextAlignment) serializedData.Alignment,
        Size = serializedData.FontSize,
        TextColor = serializedData.FontColor,
        Name = serializedData.Font.SubtypeName
      };
      MyTextPanelComponent.ScriptData script = new MyTextPanelComponent.ScriptData()
      {
        Script = serializedData.SelectedScript ?? string.Empty,
        CustomizeScript = serializedData.CustomizeScripts,
        BackgroundColor = serializedData.ScriptBackgroundColor,
        ForegroundColor = serializedData.ScriptForegroundColor
      };
      this.m_panels[panelIndex].CurrentSelectedTexture = serializedData.CurrentShownTexture;
      if (serializedData.SelectedImages != null)
      {
        foreach (string selectedImage in serializedData.SelectedImages)
        {
          MyLCDTextureDefinition definition = MyDefinitionManager.Static.GetDefinition<MyLCDTextureDefinition>(selectedImage);
          if (definition != null)
            this.m_panels[panelIndex].SelectedTexturesToDraw.Add(definition);
        }
        this.m_panels[panelIndex].CurrentSelectedTexture = Math.Min(this.m_panels[panelIndex].CurrentSelectedTexture, this.m_panels[panelIndex].SelectedTexturesToDraw.Count);
      }
      this.m_panels[panelIndex].Text = new StringBuilder(serializedData.Text);
      content.ContentType = serializedData.ContentType != ContentType.IMAGE ? serializedData.ContentType : ContentType.TEXT_AND_IMAGE;
      this.m_panels[panelIndex].SetLocalValues(content, font, script);
    }

    public void Init(
      Action<int, int[]> addImagesRequest,
      Action<int, int[]> removeImagesRequest,
      Action<int, string> changeTextRequest,
      Action<int, MySerializableSpriteCollection> updateSpriteCollection,
      float maxRenderDistance = 120f)
    {
      this.m_panelsComponent = MySession.Static.GetComponent<MySessionComponentPanels>();
      this.RangeIndex = -1;
      this.m_addImagesToSelectionRequest = addImagesRequest;
      this.m_removeImagesFromSelectionRequest = removeImagesRequest;
      this.m_changeTextRequest = changeTextRequest;
      this.m_updateSpriteCollection = updateSpriteCollection;
      this.m_maxRenderDistanceSquared = maxRenderDistance * maxRenderDistance;
    }

    private void OnClosing(MyEntity e) => this.m_panelsComponent?.Remove((IMyTextPanelProvider) this);

    private void AddImagesRequest(MyTextPanelComponent panel, int[] selection)
    {
      if (panel == null)
        return;
      int num = this.m_panels.IndexOf(panel);
      if (num == -1 || this.m_addImagesToSelectionRequest == null)
        return;
      this.m_addImagesToSelectionRequest(num, selection);
    }

    public void SelectItems(int panelIndex, int[] selection)
    {
      if (panelIndex < 0 || panelIndex >= this.m_panels.Count)
        return;
      this.m_panels[panelIndex].SelectItems(selection);
    }

    private void RemoveImagesRequest(MyTextPanelComponent panel, int[] selection)
    {
      if (panel == null)
        return;
      int num = this.m_panels.IndexOf(panel);
      if (num == -1 || this.m_removeImagesFromSelectionRequest == null)
        return;
      this.m_removeImagesFromSelectionRequest(num, selection);
    }

    public void RemoveItems(int panelIndex, int[] selection)
    {
      if (panelIndex < 0 || panelIndex >= this.m_panels.Count)
        return;
      this.m_panels[panelIndex].RemoveItems(selection);
    }

    private void ChangeTextRequest(MyTextPanelComponent panel, string text)
    {
      if (panel == null)
        return;
      int num = this.m_panels.IndexOf(panel);
      if (num == -1)
        return;
      Action<int, string> changeTextRequest = this.m_changeTextRequest;
      if (changeTextRequest == null)
        return;
      changeTextRequest(num, text);
    }

    public void ChangeText(int panelIndex, string text)
    {
      if (panelIndex < 0 || panelIndex >= this.m_panels.Count)
        return;
      this.m_panels[panelIndex].Text = new StringBuilder(text);
    }

    private void SpriteCollectionUpdate(
      MyTextPanelComponent panel,
      MySerializableSpriteCollection sprites)
    {
      if (panel == null)
        return;
      int num = this.m_panels.IndexOf(panel);
      if (num == -1)
        return;
      Action<int, MySerializableSpriteCollection> spriteCollection = this.m_updateSpriteCollection;
      if (spriteCollection == null)
        return;
      spriteCollection(num, sprites);
    }

    public void UpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites)
    {
      if (panelIndex < 0 || panelIndex >= this.m_panels.Count)
        return;
      this.m_panels[panelIndex].UpdateSpriteCollection(sprites);
    }

    public void SetRender(MyRenderComponentScreenAreas render)
    {
      this.m_render = render;
      if (this.m_panels == null || this.m_panels.Count == 0)
        return;
      for (int index = 0; index < this.m_panels.Count; ++index)
        this.m_panels[index].SetRender(this.m_render);
    }

    public void AddToScene(int? renderObjectIndex = null)
    {
      foreach (MyTextPanelComponent panel in this.m_panels)
      {
        panel.SetRender(this.m_render);
        panel.Reset();
        this.m_render.AddScreenArea(this.m_render.RenderObjectIDs, panel.Name);
        if (renderObjectIndex.HasValue)
          panel.SetRenderObjectIndex(renderObjectIndex.Value);
      }
    }

    public void Reset()
    {
      foreach (MyTextPanelComponent panel in this.m_panels)
        panel.Reset();
    }

    public List<MySerializedTextPanelData> Serialize()
    {
      if (this.m_panels.Count <= 0)
        return (List<MySerializedTextPanelData>) null;
      List<MySerializedTextPanelData> serializedTextPanelDataList = new List<MySerializedTextPanelData>();
      for (int panelIndex = 0; panelIndex < this.m_panels.Count; ++panelIndex)
      {
        MySerializedTextPanelData serializedTextPanelData = this.SerializePanel(panelIndex);
        serializedTextPanelDataList.Add(serializedTextPanelData);
      }
      return serializedTextPanelDataList;
    }

    public MySerializedTextPanelData SerializePanel(int panelIndex)
    {
      MySerializedTextPanelData serializedTextPanelData = new MySerializedTextPanelData();
      serializedTextPanelData.Alignment = (int) this.m_panels[panelIndex].Alignment;
      serializedTextPanelData.BackgroundColor = this.m_panels[panelIndex].BackgroundColor;
      serializedTextPanelData.ChangeInterval = this.m_panels[panelIndex].ChangeInterval;
      serializedTextPanelData.CurrentShownTexture = this.m_panels[panelIndex].CurrentSelectedTexture;
      serializedTextPanelData.Font = (SerializableDefinitionId) this.m_panels[panelIndex].Font;
      serializedTextPanelData.FontColor = this.m_panels[panelIndex].FontColor;
      serializedTextPanelData.FontSize = this.m_panels[panelIndex].FontSize;
      if (this.m_panels[panelIndex].SelectedTexturesToDraw.Count > 0)
      {
        serializedTextPanelData.SelectedImages = new List<string>();
        foreach (MyLCDTextureDefinition textureDefinition in this.m_panels[panelIndex].SelectedTexturesToDraw)
          serializedTextPanelData.SelectedImages.Add(textureDefinition.Id.SubtypeName);
      }
      serializedTextPanelData.Text = this.m_panels[panelIndex].Text.ToString();
      serializedTextPanelData.TextPadding = this.m_panels[panelIndex].TextPadding;
      serializedTextPanelData.PreserveAspectRatio = this.m_panels[panelIndex].PreserveAspectRatio;
      serializedTextPanelData.ContentType = this.m_panels[panelIndex].ContentType == ContentType.IMAGE ? ContentType.TEXT_AND_IMAGE : this.m_panels[panelIndex].ContentType;
      serializedTextPanelData.SelectedScript = this.m_panels[panelIndex].Script;
      serializedTextPanelData.CustomizeScripts = this.m_panels[panelIndex].CustomizeScripts;
      serializedTextPanelData.ScriptBackgroundColor = this.m_panels[panelIndex].ScriptBackgroundColor;
      serializedTextPanelData.ScriptForegroundColor = this.m_panels[panelIndex].ScriptForegroundColor;
      if (MyReplicationLayer.CurrentSerializingReplicable != null)
        serializedTextPanelData.Sprites = this.m_panels[panelIndex].ExternalSprites;
      return serializedTextPanelData;
    }

    public void SelectPanel(int index) => this.m_selectedPanel = index;

    public void UpdateScreen(bool isWorking)
    {
      if (!this.m_block.IsFunctional)
      {
        this.ReleaseTextures();
      }
      else
      {
        this.m_texturesReleased = false;
        bool isInRange = this.IsInRange();
        if (isInRange)
          this.m_wasInRange = isInRange;
        for (int index = 0; index < this.m_panels.Count; ++index)
          this.m_panels[index].UpdateAfterSimulation(isWorking, isInRange);
      }
    }

    public void UpdateAfterSimulation(bool isWorking = true)
    {
      if (this.m_block.IsFunctional)
        this.UpdateScreen(isWorking);
      else
        this.ReleaseTextures();
    }

    private bool IsInRange()
    {
      if (!this.IsContentStatic())
        return this.m_panelsComponent.IsInRange((IMyTextPanelProvider) this, this.m_maxRenderDistanceSquared);
      this.m_panelsComponent.Remove((IMyTextPanelProvider) this);
      return true;
    }

    private void ReleaseTextures()
    {
      if (this.m_texturesReleased)
        return;
      this.m_texturesReleased = true;
      foreach (MyTextPanelComponent panel in this.m_panels)
        panel.ReleaseTexture(false);
    }

    public IMyTextSurface GetSurface(int index) => index >= 0 && this.m_panels != null && index < this.m_panels.Count ? (IMyTextSurface) this.m_panels[index] : (IMyTextSurface) null;

    private bool IsContentStatic()
    {
      bool flag = (uint) this.m_panels.Count > 0U;
      foreach (MyTextPanelComponent panel in this.m_panels)
      {
        if (panel != null)
          flag &= panel.IsStatic;
      }
      return flag;
    }

    int IMyTextSurfaceProvider.SurfaceCount => this.SurfaceCount;

    public int PanelTexturesByteCount
    {
      get
      {
        int num = 0;
        foreach (MyTextPanelComponent panel in this.m_panels)
          num += panel.TextureByteCount;
        return num;
      }
    }

    public Vector3D WorldPosition => this.m_block.PositionComp.WorldMatrixRef.Translation;

    public int RangeIndex { get; set; }

    IMyTextSurface IMyTextSurfaceProvider.GetSurface(int index) => this.GetSurface(index);
  }
}
