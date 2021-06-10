// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.MyTextPanelComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ProtoBuf;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems.TextSurfaceScripts;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Blocks
{
  public class MyTextPanelComponent : Sandbox.ModAPI.IMyTextSurface, Sandbox.ModAPI.Ingame.IMyTextSurface
  {
    public static readonly MySerializer<MySpriteCollection> SpriteSerializer = MyFactory.GetSerializer<MySpriteCollection>();
    private static readonly List<MySerializableSprite> m_spritesToSend = new List<MySerializableSprite>();
    public const int NUM_DECIMALS = 3;
    public const int MAX_NUMBER_CHARACTERS = 100000;
    public const string DEFAULT_OFFLINE_TEXTURE = "Offline";
    public const string DEFAULT_ONLINE_TEXTURE = "Online";
    public const string DEFAULT_OFFLINE_WIDE_TEXTURE = "Offline_wide";
    public const string DEFAULT_ONLINE_WIDE_TEXTURE = "Online_wide";
    private const int DEFAULT_RESOLUTION = 512;
    private const int MAX_SPRITE_COLLECTION_BYTE_SIZE = 9504;
    private static readonly StringBuilder m_helperSB = new StringBuilder();
    private int m_currentSelectedTexture;
    private int m_previousUpdateTime;
    private string m_previousTextureID;
    private string m_previousScript = string.Empty;
    private bool m_textureGenerated;
    private bool m_staticContent;
    private MySpriteCollection m_spriteQueue;
    private MySpriteCollection m_lastSpriteQueue;
    private readonly List<MyLCDTextureDefinition> m_definitions = new List<MyLCDTextureDefinition>();
    private readonly List<MySprite> m_renderLayers = new List<MySprite>();
    private readonly List<MySprite> m_lastRenderLayers = new List<MySprite>();
    private readonly List<MySprite> m_textAndImageLayers = new List<MySprite>();
    private readonly List<MyLCDTextureDefinition> m_selectedTexturesToDraw = new List<MyLCDTextureDefinition>();
    private readonly List<MyGuiControlListbox.Item> m_selectedTexturesToAdd = new List<MyGuiControlListbox.Item>();
    private readonly List<MyGuiControlListbox.Item> m_selectedTexturesToRemove = new List<MyGuiControlListbox.Item>();
    private MyTerminalBlock m_block;
    private MyRenderComponentScreenAreas m_render;
    private IMyTextSurfaceScript m_script;
    private VRage.Sync.Sync<MyTextPanelComponent.FontData, SyncDirection.BothWays> m_fontData;
    private VRage.Sync.Sync<MyTextPanelComponent.ContentMetadata, SyncDirection.BothWays> m_contentData;
    private bool m_backgroundChanged;
    private Color m_backgroundColorLast = Color.Black;
    private VRage.Sync.Sync<MyTextPanelComponent.ScriptData, SyncDirection.BothWays> m_scriptData;
    private MySerializableSpriteCollection m_externalSprites;
    private string m_name;
    private string m_displayName;
    private StringBuilder m_text;
    private bool m_failedToRenderTexture;
    private bool m_useOnlineTexture = true;
    private bool m_areSpritesDirty;
    private int m_lastUpdate;
    private int m_renderObjectIndex;
    private int m_area;
    private Vector2I m_textureSize = Vector2I.One;
    private Vector2 m_screenAspectRatio = Vector2.One;
    private Action<MyTextPanelComponent, int[]> m_addImagesToSelectionRequest;
    private Action<MyTextPanelComponent, int[]> m_removeImagesFromSelectionRequest;
    private Action<MyTextPanelComponent, string> m_changeTextRequest;
    private Action<MyTextPanelComponent, MySerializableSpriteCollection> m_spriteCollectionUpdate;
    private int m_randomOffset;
    private readonly MySpriteCollection m_spriteQueueError;
    private StringBuilder m_textHelper = new StringBuilder();
    private bool m_textHelperDirty;

    public Vector3D WorldPosition => this.m_block.PositionComp.WorldMatrixRef.Translation;

    public string Name => this.m_name;

    public string DisplayName => this.m_displayName;

    public StringBuilder Text
    {
      get => this.m_text;
      set
      {
        this.m_text.Clear();
        this.m_text.Append((object) value);
        this.UpdateIsStaticContent();
      }
    }

    public int Area => this.m_area;

    public ContentType ContentType
    {
      get => this.m_contentData.Value.ContentType;
      set
      {
        if (this.m_contentData.Value.ContentType == value)
          return;
        MyTextPanelComponent.ContentMetadata contentMetadata = this.m_contentData.Value;
        contentMetadata.ContentType = value != ContentType.IMAGE ? value : ContentType.TEXT_AND_IMAGE;
        this.m_contentData.Value = contentMetadata;
        this.UpdateIsStaticContent();
      }
    }

    public ShowTextOnScreenFlag ShowTextFlag
    {
      get => this.m_contentData.Value.ContentType == ContentType.TEXT_AND_IMAGE ? ShowTextOnScreenFlag.PUBLIC : ShowTextOnScreenFlag.NONE;
      set
      {
        if (value == this.ShowTextFlag)
          return;
        if (value == ShowTextOnScreenFlag.NONE)
          this.ContentType = ContentType.NONE;
        else
          this.ContentType = ContentType.TEXT_AND_IMAGE;
      }
    }

    public bool ShowTextOnScreen => this.m_contentData.Value.ContentType == ContentType.TEXT_AND_IMAGE;

    public List<MyLCDTextureDefinition> Definitions => this.m_definitions;

    public int CurrentSelectedTexture
    {
      get => this.m_currentSelectedTexture;
      set => this.m_currentSelectedTexture = value;
    }

    internal MyRenderComponentScreenAreas Render => this.m_render;

    public Vector2 SurfaceSize => (Vector2) this.m_textureSize * MyRenderComponentScreenAreas.CalcAspectFactor(this.m_textureSize, this.m_screenAspectRatio);

    public Vector2 TextureSize => (Vector2) this.m_textureSize;

    public List<MyLCDTextureDefinition> SelectedTexturesToDraw => this.m_selectedTexturesToDraw;

    public Color BackgroundColor
    {
      get => this.m_contentData.Value.BackgroundColor;
      set
      {
        if (!(this.m_contentData.Value.BackgroundColor != value))
          return;
        MyTextPanelComponent.ContentMetadata contentMetadata = this.m_contentData.Value;
        contentMetadata.BackgroundColor = value;
        this.m_contentData.Value = contentMetadata;
        this.m_backgroundChanged = true;
        this.UpdateIsStaticContent();
      }
    }

    public byte BackgroundAlpha
    {
      get => this.m_contentData.Value.BackgroundAlpha;
      set
      {
        if ((int) this.m_contentData.Value.BackgroundAlpha == (int) value)
          return;
        MyTextPanelComponent.ContentMetadata contentMetadata = this.m_contentData.Value;
        contentMetadata.BackgroundAlpha = value;
        this.m_contentData.Value = contentMetadata;
        this.m_backgroundChanged = true;
        this.UpdateIsStaticContent();
      }
    }

    public Color FontColor
    {
      get => this.m_fontData.Value.TextColor;
      set
      {
        if (!(this.m_fontData.Value.TextColor != value))
          return;
        MyTextPanelComponent.FontData fontData = this.m_fontData.Value;
        fontData.TextColor = value;
        this.m_fontData.Value = fontData;
      }
    }

    public MyDefinitionId Font
    {
      get => new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_FontDefinition), this.m_fontData.Value.Name);
      set
      {
        if (!(this.m_fontData.Value.Name != value.SubtypeName))
          return;
        MyTextPanelComponent.FontData fontData = this.m_fontData.Value;
        fontData.Name = value.SubtypeName;
        this.m_fontData.Value = fontData;
      }
    }

    public float FontSize
    {
      get => this.m_fontData.Value.Size;
      set
      {
        if ((double) this.m_fontData.Value.Size == (double) value)
          return;
        float num = (float) Math.Round((double) value, 3);
        MyTextPanelComponent.FontData fontData = this.m_fontData.Value;
        fontData.Size = num;
        this.m_fontData.Value = fontData;
      }
    }

    public TextAlignment Alignment
    {
      get => this.m_fontData.Value.Alignment;
      set
      {
        if (this.m_fontData.Value.Alignment == value)
          return;
        MyTextPanelComponent.FontData fontData = this.m_fontData.Value;
        fontData.Alignment = value;
        this.m_fontData.Value = fontData;
        this.m_block.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      }
    }

    public string Script
    {
      get => this.m_scriptData.Value.Script;
      set
      {
        string str = value ?? string.Empty;
        if (!(this.m_scriptData.Value.Script != str))
          return;
        MyTextPanelComponent.ScriptData scriptData = this.m_scriptData.Value;
        scriptData.Script = str;
        scriptData.CustomizeScript = false;
        this.SetScriptData(scriptData);
        this.m_block.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      }
    }

    public bool CustomizeScripts
    {
      get => this.m_scriptData.Value.CustomizeScript;
      set
      {
        if (this.m_scriptData.Value.CustomizeScript == value)
          return;
        MyTextPanelComponent.ScriptData scriptData = this.m_scriptData.Value;
        scriptData.CustomizeScript = value;
        this.SetScriptData(scriptData);
        this.m_block.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      }
    }

    public Color ScriptBackgroundColor
    {
      get => this.m_scriptData.Value.BackgroundColor;
      set
      {
        if (!(this.m_scriptData.Value.BackgroundColor != value))
          return;
        MyTextPanelComponent.ScriptData scriptData = this.m_scriptData.Value;
        scriptData.BackgroundColor = value;
        scriptData.CustomizeScript = true;
        this.SetScriptData(scriptData);
        this.m_backgroundChanged = true;
      }
    }

    public Color ScriptForegroundColor
    {
      get => this.m_scriptData.Value.ForegroundColor;
      set
      {
        if (!(this.m_scriptData.Value.ForegroundColor != value))
          return;
        MyTextPanelComponent.ScriptData scriptData = this.m_scriptData.Value;
        scriptData.ForegroundColor = value;
        scriptData.CustomizeScript = true;
        this.SetScriptData(scriptData);
        this.m_backgroundChanged = true;
      }
    }

    public bool FailedToRenderTexture => this.m_failedToRenderTexture;

    public void SetFailedToRenderTexture(int area, bool failed)
    {
      if (failed)
        this.SetDefaultTexture(false);
      this.m_failedToRenderTexture = failed;
    }

    public void ChangeRenderTexture(int area, string path)
    {
      if (path == this.m_previousTextureID)
        return;
      this.Render.ChangeTexture(area, path);
      this.m_previousTextureID = path;
    }

    public void RemoveTexture(int area) => this.Render.ChangeTexture(area, (string) null);

    public float ChangeInterval
    {
      get => this.m_contentData.Value.ChangeInterval;
      set
      {
        float num = (float) Math.Round((double) value, 3);
        if ((double) this.m_contentData.Value.ChangeInterval == (double) num)
          return;
        MyTextPanelComponent.ContentMetadata contentMetadata = this.m_contentData.Value;
        contentMetadata.ChangeInterval = num;
        this.m_contentData.Value = contentMetadata;
      }
    }

    public bool PreserveAspectRatio
    {
      get => this.m_contentData.Value.PreserveAspectRatio;
      set
      {
        if (this.m_contentData.Value.PreserveAspectRatio == value)
          return;
        MyTextPanelComponent.ContentMetadata contentMetadata = this.m_contentData.Value;
        contentMetadata.PreserveAspectRatio = value;
        this.m_contentData.Value = contentMetadata;
        this.UpdateIsStaticContent();
      }
    }

    public float TextPadding
    {
      get => this.m_contentData.Value.TextPadding;
      set
      {
        float num = (float) Math.Round((double) value, 3);
        if ((double) this.m_contentData.Value.TextPadding == (double) num)
          return;
        MyTextPanelComponent.ContentMetadata contentMetadata = this.m_contentData.Value;
        contentMetadata.TextPadding = num;
        this.m_contentData.Value = contentMetadata;
      }
    }

    public bool IsStatic => this.m_staticContent;

    public MySerializableSpriteCollection ExternalSprites => (MySerializableSpriteCollection) this.m_lastSpriteQueue;

    public int TextureByteCount => MyRenderComponentScreenAreas.GetTextureByteCount(this.m_textureSize);

    public static void CreateTerminalControls<T>() where T : MyTerminalBlock, Sandbox.ModAPI.IMyTextPanel, IMyTextPanelComponentOwner
    {
      MyTerminalControlCombobox<T> terminalControlCombobox1 = new MyTerminalControlCombobox<T>("Content", MySpaceTexts.BlockPropertyTitle_PanelContent, MySpaceTexts.Blank);
      terminalControlCombobox1.ComboBoxContent = (Action<List<MyTerminalControlComboBoxItem>>) (x => MyTextPanelComponent.FillContentComboBoxContent(x));
      terminalControlCombobox1.Getter = (MyTerminalValueControl<T, long>.GetterDelegate) (x => (long) x.PanelComponent.ContentType);
      terminalControlCombobox1.Setter = (MyTerminalValueControl<T, long>.SetterDelegate) ((x, y) => x.PanelComponent.ContentType = (ContentType) y);
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) terminalControlCombobox1);
      MyTerminalControlSeparator<T> controlSeparator1 = new MyTerminalControlSeparator<T>();
      controlSeparator1.Visible = (Func<T, bool>) (x => (uint) x.PanelComponent.ContentType > 0U);
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) controlSeparator1);
      MyTerminalControlListbox<T> terminalControlListbox1 = new MyTerminalControlListbox<T>("Script", MySpaceTexts.BlockPropertyTitle_PanelScript, MySpaceTexts.Blank);
      terminalControlListbox1.Visible = (Func<T, bool>) (x => x.PanelComponent.ContentType == ContentType.SCRIPT);
      terminalControlListbox1.ListContent = (MyTerminalControlListbox<T>.ListContentDelegate) ((x, list1, list2, lastFocused) => x.PanelComponent.FillScriptsContent(list1, list2, lastFocused));
      terminalControlListbox1.ItemSelected = (MyTerminalControlListbox<T>.SelectItemDelegate) ((x, y) => x.PanelComponent.SelectScriptToDraw(y));
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) terminalControlListbox1);
      MyTerminalControlColor<T> terminalControlColor1 = new MyTerminalControlColor<T>("ScriptForegroundColor", MySpaceTexts.BlockPropertyTitle_FontColor);
      terminalControlColor1.Visible = (Func<T, bool>) (x => x.PanelComponent.ContentType == ContentType.SCRIPT);
      terminalControlColor1.Getter = (MyTerminalValueControl<T, Color>.GetterDelegate) (x => x.PanelComponent.ScriptForegroundColor);
      terminalControlColor1.Setter = (MyTerminalValueControl<T, Color>.SetterDelegate) ((x, v) => x.PanelComponent.ScriptForegroundColor = v);
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) terminalControlColor1);
      MyTerminalControlColor<T> terminalControlColor2 = new MyTerminalControlColor<T>("ScriptBackgroundColor", MySpaceTexts.BlockPropertyTitle_BackgroundColor, true, 0.055f, true);
      terminalControlColor2.Visible = (Func<T, bool>) (x => x.PanelComponent.ContentType == ContentType.SCRIPT);
      terminalControlColor2.Getter = (MyTerminalValueControl<T, Color>.GetterDelegate) (x => x.PanelComponent.ScriptBackgroundColor);
      terminalControlColor2.Setter = (MyTerminalValueControl<T, Color>.SetterDelegate) ((x, v) => x.PanelComponent.ScriptBackgroundColor = v);
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) terminalControlColor2);
      MyTerminalControlButton<T> terminalControlButton1 = new MyTerminalControlButton<T>("ShowTextPanel", MySpaceTexts.BlockPropertyTitle_TextPanelShowPublicTextPanel, MySpaceTexts.Blank, (Action<T>) (x => x.OpenWindow(true, true, true)));
      terminalControlButton1.Visible = (Func<T, bool>) (x => x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE);
      terminalControlButton1.Enabled = (Func<T, bool>) (x => !x.IsTextPanelOpen);
      terminalControlButton1.SupportsMultipleBlocks = false;
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) terminalControlButton1);
      MyTerminalControlCombobox<T> terminalControlCombobox2 = new MyTerminalControlCombobox<T>("Font", MySpaceTexts.BlockPropertyTitle_Font, MySpaceTexts.Blank);
      terminalControlCombobox2.Visible = (Func<T, bool>) (x => x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE);
      terminalControlCombobox2.ComboBoxContent = (Action<List<MyTerminalControlComboBoxItem>>) (x => MyTextPanelComponent.FillFontComboBoxContent(x));
      terminalControlCombobox2.Getter = (MyTerminalValueControl<T, long>.GetterDelegate) (x => (long) (int) x.PanelComponent.Font.SubtypeId);
      terminalControlCombobox2.Setter = (MyTerminalValueControl<T, long>.SetterDelegate) ((x, y) => x.PanelComponent.Font = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_FontDefinition), MyStringHash.TryGet((int) y)));
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) terminalControlCombobox2);
      MyTerminalControlSlider<T> slider1 = new MyTerminalControlSlider<T>("FontSize", MySpaceTexts.BlockPropertyTitle_LCDScreenTextSize, MySpaceTexts.Blank);
      slider1.Visible = (Func<T, bool>) (x => x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE);
      slider1.SetLimits(0.1f, 10f);
      slider1.DefaultValue = new float?(1f);
      slider1.Getter = (MyTerminalValueControl<T, float>.GetterDelegate) (x => x.PanelComponent.FontSize);
      slider1.Setter = (MyTerminalValueControl<T, float>.SetterDelegate) ((x, v) => x.PanelComponent.FontSize = v);
      slider1.Writer = (MyTerminalControl<T>.WriterDelegate) ((x, result) => result.Append(MyValueFormatter.GetFormatedFloat(x.PanelComponent.FontSize, 3)));
      slider1.EnableActions<T>();
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) slider1);
      MyTerminalControlColor<T> terminalControlColor3 = new MyTerminalControlColor<T>("FontColor", MySpaceTexts.BlockPropertyTitle_FontColor);
      terminalControlColor3.Visible = (Func<T, bool>) (x => x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE);
      terminalControlColor3.Getter = (MyTerminalValueControl<T, Color>.GetterDelegate) (x => x.PanelComponent.FontColor);
      terminalControlColor3.Setter = (MyTerminalValueControl<T, Color>.SetterDelegate) ((x, v) => x.PanelComponent.FontColor = v);
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) terminalControlColor3);
      MyTerminalControlCombobox<T> terminalControlCombobox3 = new MyTerminalControlCombobox<T>("alignment", MySpaceTexts.BlockPropertyTitle_Alignment, MySpaceTexts.Blank);
      terminalControlCombobox3.Visible = (Func<T, bool>) (x => x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE);
      terminalControlCombobox3.ComboBoxContent = (Action<List<MyTerminalControlComboBoxItem>>) (x => MyTextPanelComponent.FillAlignmentComboBoxContent(x));
      terminalControlCombobox3.Getter = (MyTerminalValueControl<T, long>.GetterDelegate) (x => (long) x.PanelComponent.Alignment);
      terminalControlCombobox3.Setter = (MyTerminalValueControl<T, long>.SetterDelegate) ((x, y) => x.PanelComponent.Alignment = (TextAlignment) y);
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) terminalControlCombobox3);
      MyTerminalControlSlider<T> slider2 = new MyTerminalControlSlider<T>("TextPaddingSlider", MySpaceTexts.BlockPropertyTitle_LCDScreenTextPadding, MySpaceTexts.Blank);
      slider2.Visible = (Func<T, bool>) (x => x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE);
      slider2.SetLimits(0.0f, 50f);
      slider2.DefaultValue = new float?(0.0f);
      slider2.Getter = (MyTerminalValueControl<T, float>.GetterDelegate) (x => x.PanelComponent.TextPadding);
      slider2.Setter = (MyTerminalValueControl<T, float>.SetterDelegate) ((x, v) => x.PanelComponent.TextPadding = v);
      slider2.Writer = (MyTerminalControl<T>.WriterDelegate) ((x, result) => result.Append(MyValueFormatter.GetFormatedFloat(x.PanelComponent.TextPadding, 1)).Append("%"));
      slider2.EnableActions<T>();
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) slider2);
      MyTerminalControlSeparator<T> controlSeparator2 = new MyTerminalControlSeparator<T>();
      controlSeparator2.Visible = (Func<T, bool>) (x => x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE);
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) controlSeparator2);
      MyTerminalControlColor<T> terminalControlColor4 = new MyTerminalControlColor<T>("BackgroundColor", MySpaceTexts.BlockPropertyTitle_BackgroundColor, true, 0.055f, true);
      terminalControlColor4.Visible = (Func<T, bool>) (x => x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE);
      terminalControlColor4.Getter = (MyTerminalValueControl<T, Color>.GetterDelegate) (x => x.PanelComponent.BackgroundColor);
      terminalControlColor4.Setter = (MyTerminalValueControl<T, Color>.SetterDelegate) ((x, v) => x.PanelComponent.BackgroundColor = v);
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) terminalControlColor4);
      MyTerminalControlListbox<T> terminalControlListbox2 = new MyTerminalControlListbox<T>("ImageList", MySpaceTexts.BlockPropertyTitle_LCDScreenDefinitionsTextures, MySpaceTexts.Blank, true);
      terminalControlListbox2.Visible = (Func<T, bool>) (x => x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE);
      terminalControlListbox2.ListContent = (MyTerminalControlListbox<T>.ListContentDelegate) ((x, list1, list2, lastFocused) => x.PanelComponent.FillListContent(list1, list2));
      terminalControlListbox2.ItemSelected = (MyTerminalControlListbox<T>.SelectItemDelegate) ((x, y) => x.PanelComponent.SelectImageToDraw(y));
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) terminalControlListbox2);
      MyTerminalControlButton<T> terminalControlButton2 = new MyTerminalControlButton<T>("SelectTextures", MySpaceTexts.BlockPropertyTitle_LCDScreenSelectTextures, MySpaceTexts.Blank, (Action<T>) (x => x.PanelComponent.AddImagesToSelection()));
      terminalControlButton2.Visible = (Func<T, bool>) (x => x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE);
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) terminalControlButton2);
      MyTerminalControlSlider<T> slider3 = new MyTerminalControlSlider<T>("ChangeIntervalSlider", MySpaceTexts.BlockPropertyTitle_LCDScreenRefreshInterval, MySpaceTexts.Blank, true, true);
      slider3.Visible = (Func<T, bool>) (x => x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE);
      slider3.SetLimits(0.0f, 30f);
      slider3.DefaultValue = new float?(0.0f);
      slider3.Getter = (MyTerminalValueControl<T, float>.GetterDelegate) (x => x.PanelComponent.ChangeInterval);
      slider3.Setter = (MyTerminalValueControl<T, float>.SetterDelegate) ((x, v) => x.PanelComponent.ChangeInterval = v);
      slider3.Writer = (MyTerminalControl<T>.WriterDelegate) ((x, result) => result.Append(MyValueFormatter.GetFormatedFloat(x.PanelComponent.ChangeInterval, 3)).Append(" s"));
      slider3.EnableActions<T>();
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) slider3);
      MyTerminalControlListbox<T> terminalControlListbox3 = new MyTerminalControlListbox<T>("SelectedImageList", MySpaceTexts.BlockPropertyTitle_LCDScreenSelectedTextures, MySpaceTexts.Blank, true);
      terminalControlListbox3.Visible = (Func<T, bool>) (x => x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE);
      terminalControlListbox3.ListContent = (MyTerminalControlListbox<T>.ListContentDelegate) ((x, list1, list2, lastFocused) => x.PanelComponent.FillSelectedListContent(list1, list2));
      terminalControlListbox3.ItemSelected = (MyTerminalControlListbox<T>.SelectItemDelegate) ((x, y) => x.PanelComponent.SelectImage(y));
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) terminalControlListbox3);
      MyTerminalControlButton<T> terminalControlButton3 = new MyTerminalControlButton<T>("RemoveSelectedTextures", MySpaceTexts.BlockPropertyTitle_LCDScreenRemoveSelectedTextures, MySpaceTexts.Blank, (Action<T>) (x => x.PanelComponent.RemoveImagesFromSelection()));
      terminalControlButton3.Visible = (Func<T, bool>) (x => x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE);
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) terminalControlButton3);
      MyTerminalControlCheckbox<T> checkbox = new MyTerminalControlCheckbox<T>("PreserveAspectRatio", MySpaceTexts.BlockPropertyTitle_LCDScreenPreserveAspectRatio, MySpaceTexts.BlockPropertyTitle_LCDScreenPreserveAspectRatio);
      checkbox.Getter = (MyTerminalValueControl<T, bool>.GetterDelegate) (x => x.PanelComponent.PreserveAspectRatio);
      checkbox.Setter = (MyTerminalValueControl<T, bool>.SetterDelegate) ((x, v) => x.PanelComponent.PreserveAspectRatio = v);
      checkbox.Visible = (Func<T, bool>) (x => x.PanelComponent.ContentType == ContentType.TEXT_AND_IMAGE);
      checkbox.EnableAction<T>();
      MyTerminalControlFactory.AddControl<T>((MyTerminalControl<T>) checkbox);
    }

    public static void FillContentComboBoxContent(List<MyTerminalControlComboBoxItem> items)
    {
      items.Add(new MyTerminalControlComboBoxItem()
      {
        Key = 0L,
        Value = MySpaceTexts.BlockPropertyValue_NoContent
      });
      items.Add(new MyTerminalControlComboBoxItem()
      {
        Key = 1L,
        Value = MySpaceTexts.BlockPropertyValue_TextAndImageContent
      });
      items.Add(new MyTerminalControlComboBoxItem()
      {
        Key = 3L,
        Value = MySpaceTexts.BlockPropertyValue_ScriptContent
      });
    }

    public static void FillFontComboBoxContent(List<MyTerminalControlComboBoxItem> items)
    {
      foreach (MyFontDefinition definition in MyDefinitionManager.Static.GetDefinitions<MyFontDefinition>())
      {
        if (definition.Public)
          items.Add(new MyTerminalControlComboBoxItem()
          {
            Key = (long) (int) definition.Id.SubtypeId,
            Value = MyStringId.GetOrCompute(definition.Id.SubtypeName)
          });
      }
    }

    public static void FillAlignmentComboBoxContent(List<MyTerminalControlComboBoxItem> items)
    {
      foreach (object obj in Enum.GetValues(typeof (TextAlignmentEnum)))
        items.Add(new MyTerminalControlComboBoxItem()
        {
          Key = (long) (int) obj,
          Value = MyStringId.GetOrCompute(obj.ToString())
        });
    }

    public void FillScriptsContent(
      ICollection<MyGuiControlListbox.Item> listBoxContent,
      ICollection<MyGuiControlListbox.Item> listBoxSelectedItems,
      ICollection<MyGuiControlListbox.Item> lastFocused)
    {
      MyGuiControlListbox.Item obj1 = (MyGuiControlListbox.Item) null;
      MyGuiControlListbox.Item obj2 = new MyGuiControlListbox.Item(MyTexts.Get(MySpaceTexts.None));
      listBoxContent.Add(obj2);
      if (MyTextSurfaceScriptFactory.Instance == null)
        return;
      foreach (KeyValuePair<string, MyTextSurfaceScriptFactory.ScriptInfo> script in MyTextSurfaceScriptFactory.Instance.Scripts)
      {
        MyGuiControlListbox.Item obj3 = new MyGuiControlListbox.Item(MyTexts.Get(script.Value.DisplayName), userData: ((object) script.Key));
        listBoxContent.Add(obj3);
        if (string.Compare(script.Key, this.m_scriptData.Value.Script, StringComparison.InvariantCultureIgnoreCase) == 0)
        {
          listBoxSelectedItems.Add(obj3);
          obj1 = obj3;
        }
      }
      if (listBoxSelectedItems.Count == 0)
        listBoxSelectedItems.Add(obj2);
      if (obj1 == null)
        return;
      lastFocused.Add(obj1);
    }

    public void FillListContent(
      ICollection<MyGuiControlListbox.Item> listBoxContent,
      ICollection<MyGuiControlListbox.Item> listBoxSelectedItems)
    {
      foreach (MyLCDTextureDefinition definition in this.m_definitions)
      {
        if (definition.Public && definition.Selectable)
        {
          if (!string.IsNullOrEmpty(definition.LocalizationId))
            MyTextPanelComponent.m_helperSB.Clear().Append((object) MyTexts.Get(MyStringId.GetOrCompute(definition.LocalizationId)));
          else
            MyTextPanelComponent.m_helperSB.Clear().Append(definition.Id.SubtypeName);
          MyGuiControlListbox.Item obj = new MyGuiControlListbox.Item(MyTextPanelComponent.m_helperSB, userData: ((object) definition.Id.SubtypeName));
          listBoxContent.Add(obj);
        }
      }
      foreach (MyGuiControlListbox.Item obj in this.m_selectedTexturesToAdd)
        listBoxSelectedItems.Add(obj);
    }

    public void FillSelectedListContent(
      ICollection<MyGuiControlListbox.Item> listBoxContent,
      ICollection<MyGuiControlListbox.Item> listBoxSelectedItems)
    {
      foreach (MyLCDTextureDefinition textureDefinition in this.m_selectedTexturesToDraw)
      {
        if (!string.IsNullOrEmpty(textureDefinition.LocalizationId))
          MyTextPanelComponent.m_helperSB.Clear().Append((object) MyTexts.Get(MyStringId.GetOrCompute(textureDefinition.LocalizationId)));
        else
          MyTextPanelComponent.m_helperSB.Clear().Append(textureDefinition.Id.SubtypeName);
        MyGuiControlListbox.Item obj = new MyGuiControlListbox.Item(MyTextPanelComponent.m_helperSB, userData: ((object) textureDefinition.Id.SubtypeName));
        listBoxContent.Add(obj);
      }
    }

    public void SelectImage(List<MyGuiControlListbox.Item> imageId)
    {
      this.m_selectedTexturesToRemove.Clear();
      for (int index = 0; index < imageId.Count; ++index)
        this.m_selectedTexturesToRemove.Add(imageId[index]);
    }

    public void SelectImageToDraw(List<MyGuiControlListbox.Item> imageIds)
    {
      this.m_selectedTexturesToAdd.Clear();
      for (int index = 0; index < imageIds.Count; ++index)
        this.m_selectedTexturesToAdd.Add(imageIds[index]);
    }

    public void SelectScriptToDraw(List<MyGuiControlListbox.Item> combo)
    {
      if (combo == null || combo.Count == 0)
        return;
      if (!(combo[0].UserData is string str))
        str = string.Empty;
      this.Script = str;
    }

    public MyTextPanelComponent(
      int area,
      MyTerminalBlock block,
      string name,
      string displayName,
      int textureResolution,
      int screenWidth = 1,
      int screenHeight = 1,
      bool useOnlineTexture = true)
    {
      this.m_block = block;
      this.m_name = name;
      this.m_displayName = displayName;
      this.m_text = new StringBuilder();
      this.m_textureSize = MyTextPanelComponent.GetTextureResolutionForAspectRatio(screenWidth, screenHeight, textureResolution);
      this.m_area = area;
      this.m_screenAspectRatio = screenWidth <= screenHeight ? new Vector2(1f * (float) screenWidth / (float) screenHeight, 1f) : new Vector2(1f, 1f * (float) screenHeight / (float) screenWidth);
      this.m_useOnlineTexture = useOnlineTexture;
      this.m_definitions.Clear();
      foreach (MyLCDTextureDefinition texturesDefinition in MyDefinitionManager.Static.GetLCDTexturesDefinitions())
        this.m_definitions.Add(texturesDefinition);
      this.m_spriteQueueError = new MySpriteCollection(new MySprite[2]
      {
        new MySprite(data: "Cross", position: new Vector2?(this.SurfaceSize / 2f), size: new Vector2?(this.SurfaceSize)),
        new MySprite(SpriteType.TEXT, MyTexts.GetString(MyCommonTexts.Scripts_TooManySprites), new Vector2?(this.SurfaceSize / 2f - new Vector2(0.0f, this.SurfaceSize.Y / 2f)), new Vector2?(this.SurfaceSize), new Color?(Color.Yellow), "Debug", rotation: ((float) Math.Min(this.m_textureSize.X, this.m_textureSize.Y) / 512f))
      });
    }

    public void Init(
      MySerializableSpriteCollection initialSprites = default (MySerializableSpriteCollection),
      string initialScript = null,
      Action<MyTextPanelComponent, int[]> addImagesRequest = null,
      Action<MyTextPanelComponent, int[]> removeImagesRequest = null,
      Action<MyTextPanelComponent, string> changeTextRequest = null,
      Action<MyTextPanelComponent, MySerializableSpriteCollection> spriteCollectionUpdate = null)
    {
      this.m_previousTextureID = string.Empty;
      this.SetLocalValues(new MyTextPanelComponent.ContentMetadata()
      {
        ContentType = !string.IsNullOrEmpty(initialScript) ? ContentType.SCRIPT : ContentType.NONE,
        BackgroundColor = Color.Black,
        ChangeInterval = 0.0f,
        BackgroundAlpha = (byte) 0,
        PreserveAspectRatio = false,
        TextPadding = 2f
      }, new MyTextPanelComponent.FontData()
      {
        Name = "Debug",
        TextColor = Color.White,
        Alignment = TextAlignment.LEFT,
        Size = 1f
      }, new MyTextPanelComponent.ScriptData()
      {
        Script = initialScript ?? string.Empty,
        CustomizeScript = false,
        BackgroundColor = MyTextSurfaceScriptBase.DEFAULT_BACKGROUND_COLOR,
        ForegroundColor = MyTextSurfaceScriptBase.DEFAULT_FONT_COLOR
      });
      this.m_fontData.ValueChanged += new Action<SyncBase>(this.m_fontData_ValueChanged);
      this.m_contentData.ValueChanged += new Action<SyncBase>(this.m_contentData_ValueChanged);
      this.m_scriptData.ValueChanged += new Action<SyncBase>(this.m_scriptData_ValueChanged);
      if (initialSprites.Sprites != null)
      {
        for (int index = 0; index < initialSprites.Sprites.Length; ++index)
          initialSprites.Sprites[index].Index = index;
      }
      this.UpdateSpriteCollection(initialSprites);
      this.m_addImagesToSelectionRequest = addImagesRequest;
      this.m_removeImagesFromSelectionRequest = removeImagesRequest;
      this.m_changeTextRequest = changeTextRequest;
      this.m_spriteCollectionUpdate = spriteCollectionUpdate;
      this.m_block.IsWorkingChanged += new Action<MyCubeBlock>(this.m_block_IsWorkingChanged);
      this.m_randomOffset = MyRandom.Instance.Next(10000);
      this.UpdateIsStaticContent();
    }

    public void SetLocalValues(
      MyTextPanelComponent.ContentMetadata content,
      MyTextPanelComponent.FontData font,
      MyTextPanelComponent.ScriptData script)
    {
      this.m_contentData.SetLocalValue(content);
      this.m_fontData.SetLocalValue(font);
      this.m_scriptData.SetLocalValue(script);
      if (!(content.BackgroundColor != Color.Black))
        return;
      this.m_backgroundChanged = true;
    }

    public void GetLocalValues(
      out MyTextPanelComponent.ContentMetadata content,
      out MyTextPanelComponent.FontData font,
      out MyTextPanelComponent.ScriptData script)
    {
      content = this.m_contentData.Value;
      font = this.m_fontData.Value;
      script = this.m_scriptData.Value;
    }

    public void Unload()
    {
    }

    public void UpdateIsStaticContent()
    {
      if (this.ContentType == ContentType.NONE || !this.m_block.IsWorking)
        this.m_staticContent = true;
      else if ((this.m_text == null || this.m_text.Length == 0) && (!this.PreserveAspectRatio && this.BackgroundColor == Color.Black) && this.ContentType == ContentType.TEXT_AND_IMAGE)
        this.m_staticContent = true;
      else
        this.m_staticContent = false;
    }

    public void UpdateAfterSimulation(bool isWorking, bool isInRange)
    {
      if (this.Render == null)
        return;
      this.UpdateModApiText();
      if (Sandbox.Game.Multiplayer.Sync.IsServer && this.m_areSpritesDirty)
        this.SendSpriteQueue();
      if (!this.m_block.IsFunctional)
        this.ReleaseTexture(false);
      else if (((isInRange ? 0 : (!this.m_staticContent ? 1 : 0)) & (isWorking ? 1 : 0)) != 0)
      {
        this.ReleaseTexture();
        this.SetDefaultTexture(isWorking);
      }
      else if (!isWorking)
      {
        this.ReleaseTexture();
        this.SetDefaultTexture(false);
      }
      else
      {
        if (this.m_staticContent)
          this.ReleaseTexture();
        else
          this.EnsureGeneratedTexture();
        switch (this.ContentType)
        {
          case ContentType.NONE:
            this.SetDefaultTexture(true);
            break;
          case ContentType.TEXT_AND_IMAGE:
            this.UpdateRenderTexture();
            break;
          case ContentType.SCRIPT:
            if (this.m_script == null && this.Script != string.Empty)
              this.SelectScriptToDraw(this.Script);
            this.UpdateSpritesTexture();
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }

    private void SendSpriteQueue()
    {
      this.m_areSpritesDirty = false;
      MySerializableSpriteCollection delta = this.GetDelta(this.m_lastSpriteQueue, this.m_spriteQueue);
      if (!delta.Sprites.IsNullOrEmpty<MySerializableSprite>())
      {
        Action<MyTextPanelComponent, MySerializableSpriteCollection> collectionUpdate = this.m_spriteCollectionUpdate;
        if (collectionUpdate != null)
          collectionUpdate(this, delta);
        this.m_lastSpriteQueue = this.m_spriteQueue;
      }
      this.m_spriteQueue = new MySpriteCollection();
    }

    private MySerializableSpriteCollection GetDelta(
      MySpriteCollection original,
      MySpriteCollection current)
    {
      if (original.Sprites == null)
        original.Sprites = new MySprite[0];
      int length = 0;
      if (current.Sprites != null)
        length = current.Sprites.Length;
      MyTextPanelComponent.m_spritesToSend.Clear();
      for (int index = 0; index < length; ++index)
      {
        if (index >= original.Sprites.Length || !original.Sprites[index].Equals(current.Sprites[index]))
        {
          MySerializableSprite sprite = (MySerializableSprite) current.Sprites[index];
          sprite.Index = index;
          MyTextPanelComponent.m_spritesToSend.Add(sprite);
        }
      }
      return new MySerializableSpriteCollection(MyTextPanelComponent.m_spritesToSend.ToArray(), length);
    }

    private void EnsureGeneratedTexture()
    {
      if (this.m_textureGenerated || Sandbox.Game.Multiplayer.Sync.IsDedicated)
        return;
      this.Render.CreateTexture(this.m_area, this.m_textureSize);
      this.m_textureGenerated = true;
      this.m_backgroundChanged = true;
      this.m_externalSprites_ValueChanged((SyncBase) null);
    }

    private void SetDefaultTexture(bool isOnline)
    {
      if (isOnline)
        this.ChangeRenderTexture(this.m_area, !this.m_useOnlineTexture ? this.GetPathForID((string) null) : ((double) this.m_screenAspectRatio.X <= (double) this.m_screenAspectRatio.Y * 4.0 ? this.GetPathForID("Online") : this.GetPathForID("Online_wide")));
      else
        this.ChangeRenderTexture(this.m_area, (double) this.m_screenAspectRatio.X <= (double) this.m_screenAspectRatio.Y * 4.0 ? this.GetPathForID("Offline") : this.GetPathForID("Offline_wide"));
    }

    protected bool UpdateSpritesTexture()
    {
      if (this.Render == null)
        return false;
      if (this.m_script != null && this.NeedsUpdate(this.m_script))
        this.m_script.Run();
      if (!MyTextPanelComponent.AreEqual(this.m_lastRenderLayers, this.m_renderLayers) || this.m_backgroundChanged)
      {
        this.Render.RenderSpritesToTexture(this.m_area, (ListReader<MySprite>) this.m_renderLayers, this.m_textureSize, this.m_screenAspectRatio, this.ScriptBackgroundColor, this.BackgroundAlpha);
        this.m_lastRenderLayers.Clear();
        this.m_lastRenderLayers.AddList<MySprite>(this.m_renderLayers);
        this.m_backgroundChanged = false;
      }
      this.ChangeRenderTexture(this.m_area, this.GetRenderTextureName());
      this.SetFailedToRenderTexture(this.m_area, false);
      return true;
    }

    protected void UpdateTexture()
    {
      if (this.m_selectedTexturesToDraw.Count <= 0)
        return;
      int num1 = (int) ((double) this.ChangeInterval * 1000.0);
      if (num1 > 0)
      {
        int num2 = (int) MySession.Static.ElapsedGameTime.TotalMilliseconds % num1;
        if (this.m_previousUpdateTime - num2 > 0)
          ++this.m_currentSelectedTexture;
        this.m_previousUpdateTime = num2;
      }
      if (this.m_currentSelectedTexture < this.m_selectedTexturesToDraw.Count)
        return;
      this.m_currentSelectedTexture = 0;
    }

    protected bool UpdateRenderTexture()
    {
      if (this.Render == null || !this.Render.IsRenderObjectAssigned(this.m_renderObjectIndex))
        return false;
      Vector2 aspectFactor = MyRenderComponentScreenAreas.CalcAspectFactor(this.m_textureSize, this.m_screenAspectRatio);
      MyRenderComponentScreenAreas.CalcShift(this.m_textureSize, aspectFactor);
      this.m_textAndImageLayers.Clear();
      bool flag = this.m_textureSize.X == this.m_textureSize.Y;
      Vector2 position = new Vector2((float) this.m_textureSize.X, (float) this.m_textureSize.Y) * 0.5f;
      Vector2 size = (Vector2) this.m_textureSize * aspectFactor;
      if (this.m_selectedTexturesToDraw.Count > 0)
      {
        this.UpdateTexture();
        if ((this.m_text == null || this.m_text.Length == 0) && (!this.PreserveAspectRatio && this.BackgroundColor == Color.Black))
        {
          this.ChangeRenderTexture(this.m_area, this.m_selectedTexturesToDraw[this.m_currentSelectedTexture].TexturePath);
          return false;
        }
        MySprite sprite = MySprite.CreateSprite(this.m_selectedTexturesToDraw[this.m_currentSelectedTexture].Id.SubtypeName, position, size);
        if (this.PreserveAspectRatio)
        {
          sprite.Size = new Vector2?(new Vector2((float) Math.Min(this.m_textureSize.X, this.m_textureSize.Y)));
          if (!flag)
          {
            ref Vector2? local = ref sprite.Size;
            Vector2? nullable = local;
            Vector2 vector2 = aspectFactor;
            local = nullable.HasValue ? new Vector2?(nullable.GetValueOrDefault() * vector2) : new Vector2?();
          }
          else
          {
            ref Vector2? local = ref sprite.Size;
            Vector2? nullable = local;
            float num = Math.Min(aspectFactor.X, aspectFactor.Y);
            local = nullable.HasValue ? new Vector2?(nullable.GetValueOrDefault() * num) : new Vector2?();
          }
        }
        this.m_textAndImageLayers.Add(sprite);
      }
      if (this.m_text != null && this.m_text.Length > 0)
      {
        MySprite text = MySprite.CreateText(this.m_text.ToString(), this.Font.SubtypeName, this.FontColor, (float) ((double) this.FontSize * (double) Math.Min(this.m_textureSize.X, this.m_textureSize.Y) / 512.0), this.Alignment);
        Vector2 vector2_1 = new Vector2(this.TextPadding * 0.02f);
        Vector2 vector2_2 = size * (Vector2.One - vector2_1);
        switch (this.Alignment)
        {
          case TextAlignment.LEFT:
            text.Position = new Vector2?(position - vector2_2 * 0.5f);
            break;
          case TextAlignment.RIGHT:
            text.Position = new Vector2?(position + new Vector2(vector2_2.X * 0.5f, (float) (-(double) vector2_2.Y * 0.5)));
            break;
          case TextAlignment.CENTER:
            text.Position = new Vector2?(position - new Vector2(0.0f, vector2_2.Y * 0.5f));
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
        this.m_textAndImageLayers.Add(text);
      }
      else if (this.m_selectedTexturesToDraw.Count == 0 && this.BackgroundColor == Color.Black)
      {
        this.ChangeRenderTexture(this.m_area, "EMPTY");
        return false;
      }
      if (!MyTextPanelComponent.AreEqual(this.m_lastRenderLayers, this.m_textAndImageLayers) || this.m_backgroundChanged)
      {
        this.m_backgroundChanged = false;
        this.Render.RenderSpritesToTexture(this.m_area, (ListReader<MySprite>) this.m_textAndImageLayers, this.m_textureSize, this.m_screenAspectRatio, this.BackgroundColor, this.BackgroundAlpha);
        this.m_lastRenderLayers.Clear();
        this.m_lastRenderLayers.AddRange((IEnumerable<MySprite>) this.m_textAndImageLayers);
      }
      this.ChangeRenderTexture(this.m_area, this.GetRenderTextureName());
      this.SetFailedToRenderTexture(this.m_area, false);
      return true;
    }

    private bool NeedsUpdate(IMyTextSurfaceScript script)
    {
      int num1;
      switch (script.NeedsUpdate)
      {
        case ScriptUpdate.Update10:
          num1 = 10;
          break;
        case ScriptUpdate.Update100:
          num1 = 100;
          break;
        case ScriptUpdate.Update1000:
          num1 = 1000;
          break;
        case ScriptUpdate.Update10000:
          num1 = 10000;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      int num2 = MySession.Static.GameplayFrameCounter + this.m_randomOffset / (10000 / num1);
      if (this.m_lastUpdate == 0)
      {
        this.m_lastUpdate = num2;
        return true;
      }
      int num3 = num2 >= this.m_lastUpdate + num1 ? 1 : 0;
      if (num3 == 0)
        return num3 != 0;
      this.m_lastUpdate = num2;
      return num3 != 0;
    }

    private void m_block_IsWorkingChanged(MyCubeBlock obj) => this.UpdateIsStaticContent();

    public void GetScripts(List<string> scripts)
    {
      if (scripts == null || MyTextSurfaceScriptFactory.Instance == null)
        return;
      scripts.AddRange(MyTextSurfaceScriptFactory.Instance.Scripts.Keys);
    }

    public void SelectScriptToDraw(string id)
    {
      if (this.m_script != null && id == this.m_previousScript)
        return;
      if (this.m_script != null)
        this.m_script.Dispose();
      this.m_script = (IMyTextSurfaceScript) null;
      this.m_lastUpdate = 0;
      if (Sandbox.Game.Multiplayer.Sync.IsDedicated)
        return;
      this.m_renderLayers.Clear();
      if (!string.IsNullOrEmpty(id))
        this.m_script = MyTextSurfaceScriptFactory.CreateScript(id, (Sandbox.ModAPI.Ingame.IMyTextSurface) this, (IMyCubeBlock) this.m_block, (Vector2) this.m_textureSize);
      if (this.m_script == null)
      {
        this.Script = string.Empty;
        using (MySpriteDrawFrame mySpriteDrawFrame = this.DrawFrame())
        {
          MySprite defaultBackground = MyTextSurfaceHelper.DEFAULT_BACKGROUND;
          defaultBackground.Color = new Color?(Color.White);
          mySpriteDrawFrame.Add(defaultBackground);
        }
      }
      else
      {
        if (!this.CustomizeScripts)
          this.SetScriptData(new MyTextPanelComponent.ScriptData(id, true, this.m_script.BackgroundColor, this.m_script.ForegroundColor));
        this.m_script.Run();
      }
      this.m_previousScript = id;
    }

    private void SetScriptData(MyTextPanelComponent.ScriptData scriptData)
    {
      bool flag = this.m_block.HasPlayerAccess(MySession.Static.LocalPlayerId);
      if (Sandbox.Game.Multiplayer.Sync.IsServer || MySession.Static.LocalCharacter != null & flag)
        this.m_scriptData.Value = scriptData;
      else
        this.m_scriptData.SetLocalValue(scriptData);
    }

    public void AddImagesToSelection()
    {
      if (this.m_selectedTexturesToAdd == null || this.m_selectedTexturesToAdd.Count == 0)
        return;
      int[] numArray = new int[this.m_selectedTexturesToAdd.Count];
      for (int index1 = 0; index1 < this.m_selectedTexturesToAdd.Count; ++index1)
      {
        for (int index2 = 0; index2 < this.m_definitions.Count; ++index2)
        {
          if ((string) this.m_selectedTexturesToAdd[index1].UserData == this.m_definitions[index2].Id.SubtypeName)
          {
            numArray[index1] = index2;
            break;
          }
        }
      }
      if (this.m_addImagesToSelectionRequest == null)
        return;
      this.m_addImagesToSelectionRequest(this, numArray);
    }

    public void RemoveImagesFromSelection()
    {
      if (this.m_selectedTexturesToRemove == null || this.m_selectedTexturesToRemove.Count == 0)
        return;
      this.m_previousTextureID = (string) null;
      int[] numArray = new int[this.m_selectedTexturesToRemove.Count];
      for (int index1 = 0; index1 < this.m_selectedTexturesToRemove.Count; ++index1)
      {
        for (int index2 = 0; index2 < this.m_definitions.Count; ++index2)
        {
          if ((string) this.m_selectedTexturesToRemove[index1].UserData == this.m_definitions[index2].Id.SubtypeName)
          {
            numArray[index1] = index2;
            break;
          }
        }
      }
      if (this.m_removeImagesFromSelectionRequest == null)
        return;
      this.m_removeImagesFromSelectionRequest(this, numArray);
    }

    public void SelectItems(int[] selection)
    {
      for (int index = 0; index < selection.Length; ++index)
      {
        if (selection[index] < this.m_definitions.Count)
          this.m_selectedTexturesToDraw.Add(this.m_definitions[selection[index]]);
      }
      this.m_currentSelectedTexture = 0;
      this.m_block.RaisePropertiesChanged();
      this.m_block.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      this.UpdateIsStaticContent();
    }

    public void RemoveItems(int[] selection)
    {
      for (int index = 0; index < selection.Length; ++index)
      {
        if (selection[index] < this.m_definitions.Count)
          this.m_selectedTexturesToDraw.Remove(this.m_definitions[selection[index]]);
      }
      this.m_currentSelectedTexture = 0;
      int count = this.m_selectedTexturesToDraw.Count;
      this.m_block.RaisePropertiesChanged();
      this.m_block.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      this.UpdateIsStaticContent();
    }

    public void UpdateSpriteCollection(MySerializableSpriteCollection sprites)
    {
      this.m_externalSprites = sprites;
      if (Sandbox.Game.Multiplayer.Sync.IsDedicated)
        return;
      this.m_externalSprites_ValueChanged((SyncBase) null);
    }

    public void SetRender(MyRenderComponentScreenAreas render)
    {
      if (this.m_render != null && render == null)
        this.ReleaseTexture();
      this.m_render = render;
    }

    public void ReleaseTexture(bool useEmptyTexture = true)
    {
      if (this.m_render == null || !this.m_textureGenerated)
        return;
      this.m_render.ReleaseTexture(this.m_area, useEmptyTexture);
      this.m_textureGenerated = false;
      this.m_lastRenderLayers.Clear();
    }

    public void SetRenderObjectIndex(int renderObjectIndex) => this.m_renderObjectIndex = renderObjectIndex;

    public void RefreshRenderText(int freeResources = 2147483647)
    {
    }

    public void Reset(bool setOfflineTexture = false)
    {
      this.m_previousTextureID = string.Empty;
      this.m_previousScript = string.Empty;
      this.m_textureGenerated = false;
      if (this.m_script != null)
      {
        this.m_script.Dispose();
        this.m_script = (IMyTextSurfaceScript) null;
      }
      if (!setOfflineTexture)
        return;
      this.SetDefaultTexture(false);
    }

    public MySpriteDrawFrame DrawFrame() => new MySpriteDrawFrame(new Action<MySpriteDrawFrame>(this.DispatchSprites));

    private void DispatchSprites(MySpriteDrawFrame drawFrame)
    {
      this.m_renderLayers.Clear();
      drawFrame.AddToList(this.m_renderLayers);
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      if (this.Script != string.Empty)
      {
        this.m_spriteQueue = new MySpriteCollection();
      }
      else
      {
        this.m_spriteQueue = drawFrame.ToCollection();
        this.m_areSpritesDirty = true;
      }
    }

    public string GetRenderTextureName() => this.m_render.GenerateOffscreenTextureName(this.m_block.EntityId, this.m_area);

    public string GetPathForID(string id) => MyDefinitionManager.Static.GetDefinition<MyLCDTextureDefinition>(id)?.TexturePath;

    private static Vector2I GetTextureResolutionForAspectRatio(
      int width,
      int height,
      int textureSize)
    {
      if (width == height)
        return new Vector2I(textureSize, textureSize);
      if (width > height)
      {
        int num = MathHelper.Pow2(MathHelper.Floor((float) MathHelper.Log2(width / height)));
        return new Vector2I(textureSize * num, textureSize);
      }
      int num1 = MathHelper.Pow2(MathHelper.Floor((float) MathHelper.Log2(height / width)));
      return new Vector2I(textureSize, textureSize * num1);
    }

    private static bool AreEqual(List<MySprite> lhs, List<MySprite> rhs)
    {
      if (lhs.Count == 0 && rhs.Count == 0)
        return true;
      if (lhs.Count != rhs.Count)
        return false;
      for (int index = 0; index < lhs.Count; ++index)
      {
        if (!lhs[index].Equals(rhs[index]))
          return false;
      }
      return true;
    }

    public override string ToString() => string.Format("{0} area:{1}", (object) this.Name, (object) this.m_area);

    private void m_contentData_ValueChanged(SyncBase obj)
    {
      this.m_block.RaisePropertiesChanged();
      this.m_block.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      if (this.m_backgroundColorLast != this.m_contentData.Value.BackgroundColor)
      {
        this.m_backgroundChanged = true;
        this.m_backgroundColorLast = this.m_contentData.Value.BackgroundColor;
      }
      this.UpdateIsStaticContent();
    }

    private void m_fontData_ValueChanged(SyncBase obj)
    {
      this.m_block.RaisePropertiesChanged();
      this.m_block.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    private void m_scriptData_ValueChanged(SyncBase obj)
    {
      this.m_block.RaisePropertiesChanged();
      this.SelectScriptToDraw(this.Script);
    }

    private void m_externalSprites_ValueChanged(SyncBase obj)
    {
      if (this.m_externalSprites.Sprites.IsNullOrEmpty<MySerializableSprite>() || this.m_script != null && this.Script != string.Empty)
        return;
      int num = this.m_renderLayers.Count - this.m_externalSprites.Length;
      if (num > 0)
      {
        while (num-- > 0)
          this.m_renderLayers.RemoveAt(this.m_renderLayers.Count - 1);
      }
      else if (num < 0)
      {
        while (num++ < 0)
          this.m_renderLayers.Add((MySprite) new MySerializableSprite());
      }
      foreach (MySerializableSprite sprite in this.m_externalSprites.Sprites)
        this.m_renderLayers[sprite.Index] = (MySprite) sprite;
    }

    public void UpdateModApiText()
    {
      if (!this.m_textHelperDirty)
        return;
      this.m_textHelperDirty = false;
      if (this.Text != this.m_textHelper)
      {
        Action<MyTextPanelComponent, string> changeTextRequest = this.m_changeTextRequest;
        if (changeTextRequest != null)
          changeTextRequest(this, this.m_textHelper.ToString());
      }
      this.UpdateIsStaticContent();
    }

    bool Sandbox.ModAPI.Ingame.IMyTextSurface.WriteText(string value, bool append)
    {
      if (!append)
        this.m_textHelper.Clear();
      if (value.Length + this.m_textHelper.Length > 100000)
        value = value.Remove(100000 - this.m_textHelper.Length);
      this.m_textHelper.Append(value);
      this.m_textHelperDirty = true;
      return true;
    }

    string Sandbox.ModAPI.Ingame.IMyTextSurface.GetText() => this.m_textHelper.ToString();

    bool Sandbox.ModAPI.Ingame.IMyTextSurface.WriteText(
      StringBuilder value,
      bool append)
    {
      if (!append)
        this.m_textHelper.Clear();
      int count = value.Length + this.m_textHelper.Length - 100000;
      if (count > 0)
        this.m_textHelper.AppendSubstring(value, 0, count);
      else
        this.m_textHelper.Append((object) value);
      this.m_textHelperDirty = true;
      return true;
    }

    void Sandbox.ModAPI.Ingame.IMyTextSurface.ReadText(
      StringBuilder buffer,
      bool append)
    {
      if (!append)
        buffer.Clear();
      buffer.Append((object) this.Text);
    }

    void Sandbox.ModAPI.Ingame.IMyTextSurface.AddImageToSelection(
      string id,
      bool checkExistence)
    {
      if (id == null)
        return;
      for (int index1 = 0; index1 < this.Definitions.Count; ++index1)
      {
        if (this.Definitions[index1].Id.SubtypeName == id)
        {
          if (checkExistence)
          {
            for (int index2 = 0; index2 < this.SelectedTexturesToDraw.Count; ++index2)
            {
              if (this.SelectedTexturesToDraw[index2].Id.SubtypeName == id)
                return;
            }
          }
          if (this.m_addImagesToSelectionRequest == null)
            break;
          this.m_addImagesToSelectionRequest(this, new int[1]
          {
            index1
          });
          break;
        }
      }
    }

    void Sandbox.ModAPI.Ingame.IMyTextSurface.AddImagesToSelection(
      List<string> ids,
      bool checkExistence)
    {
      if (ids == null)
        return;
      List<int> intList = new List<int>();
      foreach (string id in ids)
      {
        for (int index1 = 0; index1 < this.Definitions.Count; ++index1)
        {
          if (this.Definitions[index1].Id.SubtypeName == id)
          {
            bool flag = false;
            if (checkExistence)
            {
              for (int index2 = 0; index2 < this.SelectedTexturesToDraw.Count; ++index2)
              {
                if (this.SelectedTexturesToDraw[index2].Id.SubtypeName == id)
                {
                  flag = true;
                  break;
                }
              }
            }
            if (!flag)
            {
              intList.Add(index1);
              break;
            }
            break;
          }
        }
      }
      if (intList.Count <= 0 || this.m_addImagesToSelectionRequest == null)
        return;
      this.m_addImagesToSelectionRequest(this, intList.ToArray());
    }

    void Sandbox.ModAPI.Ingame.IMyTextSurface.RemoveImageFromSelection(
      string id,
      bool removeDuplicates)
    {
      if (id == null)
        return;
      List<int> intList = new List<int>();
      for (int index1 = 0; index1 < this.Definitions.Count; ++index1)
      {
        if (this.Definitions[index1].Id.SubtypeName == id)
        {
          if (removeDuplicates)
          {
            for (int index2 = 0; index2 < this.SelectedTexturesToDraw.Count; ++index2)
            {
              if (this.SelectedTexturesToDraw[index2].Id.SubtypeName == id)
                intList.Add(index1);
            }
            break;
          }
          intList.Add(index1);
          break;
        }
      }
      if (intList.Count <= 0 || this.m_removeImagesFromSelectionRequest == null)
        return;
      this.m_removeImagesFromSelectionRequest(this, intList.ToArray());
    }

    void Sandbox.ModAPI.Ingame.IMyTextSurface.RemoveImagesFromSelection(
      List<string> ids,
      bool removeDuplicates)
    {
      if (ids == null)
        return;
      List<int> intList = new List<int>();
      foreach (string id in ids)
      {
        for (int index1 = 0; index1 < this.Definitions.Count; ++index1)
        {
          if (this.Definitions[index1].Id.SubtypeName == id)
          {
            if (removeDuplicates)
            {
              for (int index2 = 0; index2 < this.SelectedTexturesToDraw.Count; ++index2)
              {
                if (this.SelectedTexturesToDraw[index2].Id.SubtypeName == id)
                  intList.Add(index1);
              }
              break;
            }
            intList.Add(index1);
            break;
          }
        }
      }
      if (intList.Count <= 0 || this.m_removeImagesFromSelectionRequest == null)
        return;
      this.m_removeImagesFromSelectionRequest(this, intList.ToArray());
    }

    void Sandbox.ModAPI.Ingame.IMyTextSurface.ClearImagesFromSelection()
    {
      if (this.SelectedTexturesToDraw.Count == 0)
        return;
      List<int> intList = new List<int>();
      for (int index1 = 0; index1 < this.SelectedTexturesToDraw.Count; ++index1)
      {
        for (int index2 = 0; index2 < this.Definitions.Count; ++index2)
        {
          if (this.Definitions[index2].Id.SubtypeName == this.SelectedTexturesToDraw[index1].Id.SubtypeName)
          {
            intList.Add(index2);
            break;
          }
        }
      }
      if (intList.Count <= 0 || this.m_removeImagesFromSelectionRequest == null)
        return;
      this.m_removeImagesFromSelectionRequest(this, intList.ToArray());
    }

    void Sandbox.ModAPI.Ingame.IMyTextSurface.GetSelectedImages(List<string> output)
    {
      foreach (MyLCDTextureDefinition textureDefinition in this.SelectedTexturesToDraw)
        output.Add(textureDefinition.Id.SubtypeName);
    }

    string Sandbox.ModAPI.Ingame.IMyTextSurface.CurrentlyShownImage
    {
      get
      {
        if (this.SelectedTexturesToDraw.Count == 0)
          return (string) null;
        return this.CurrentSelectedTexture >= this.SelectedTexturesToDraw.Count ? this.SelectedTexturesToDraw[0].Id.SubtypeName : this.SelectedTexturesToDraw[this.CurrentSelectedTexture].Id.SubtypeName;
      }
    }

    string Sandbox.ModAPI.Ingame.IMyTextSurface.Font
    {
      get => this.Font.SubtypeName;
      set
      {
        if (string.IsNullOrEmpty(value) || MyDefinitionManager.Static.GetDefinition<MyFontDefinition>(value) == null)
          return;
        this.Font = MyDefinitionManager.Static.GetDefinition<MyFontDefinition>(value).Id;
      }
    }

    void Sandbox.ModAPI.Ingame.IMyTextSurface.GetFonts(List<string> fonts)
    {
      if (fonts == null)
        return;
      foreach (MyFontDefinition definition in MyDefinitionManager.Static.GetDefinitions<MyFontDefinition>())
        fonts.Add(definition.Id.SubtypeName);
    }

    public void GetSprites(List<string> sprites)
    {
      foreach (MyLCDTextureDefinition definition in this.m_definitions)
        sprites.Add(definition.Id.SubtypeName);
    }

    TextAlignment Sandbox.ModAPI.Ingame.IMyTextSurface.Alignment
    {
      get => this.Alignment;
      set => this.Alignment = value;
    }

    void Sandbox.ModAPI.Ingame.IMyTextSurface.GetScripts(List<string> scripts) => this.GetScripts(scripts);

    string Sandbox.ModAPI.Ingame.IMyTextSurface.Script
    {
      get => this.Script;
      set => this.Script = value;
    }

    ContentType Sandbox.ModAPI.Ingame.IMyTextSurface.ContentType
    {
      get => this.ContentType;
      set => this.ContentType = value;
    }

    Vector2 Sandbox.ModAPI.Ingame.IMyTextSurface.SurfaceSize => this.SurfaceSize;

    Vector2 Sandbox.ModAPI.Ingame.IMyTextSurface.TextureSize => this.TextureSize;

    MySpriteDrawFrame Sandbox.ModAPI.Ingame.IMyTextSurface.DrawFrame() => this.DrawFrame();

    bool Sandbox.ModAPI.Ingame.IMyTextSurface.PreserveAspectRatio
    {
      get => this.PreserveAspectRatio;
      set => this.PreserveAspectRatio = value;
    }

    float Sandbox.ModAPI.Ingame.IMyTextSurface.TextPadding
    {
      get => this.TextPadding;
      set => this.TextPadding = value;
    }

    Color Sandbox.ModAPI.Ingame.IMyTextSurface.ScriptBackgroundColor
    {
      get => this.ScriptBackgroundColor;
      set => this.ScriptBackgroundColor = value;
    }

    Color Sandbox.ModAPI.Ingame.IMyTextSurface.ScriptForegroundColor
    {
      get => this.ScriptForegroundColor;
      set => this.ScriptForegroundColor = value;
    }

    string Sandbox.ModAPI.Ingame.IMyTextSurface.Name => this.Name;

    string Sandbox.ModAPI.Ingame.IMyTextSurface.DisplayName => this.DisplayName;

    public Vector2 MeasureStringInPixels(StringBuilder text, string font, float scale) => MyGuiManager.MeasureStringRaw(font, text, scale);

    [ProtoContract]
    [Serializable]
    public struct FontData
    {
      [ProtoMember(1)]
      [Nullable]
      public string Name;
      [ProtoMember(4)]
      public Color TextColor;
      [ProtoMember(7)]
      public TextAlignment Alignment;
      [ProtoMember(10)]
      public float Size;

      public FontData(string font, Color color, TextAlignment alignment, float size)
      {
        this.Name = font;
        this.TextColor = color;
        this.Alignment = alignment;
        this.Size = size;
      }

      protected class Sandbox_Game_Entities_Blocks_MyTextPanelComponent\u003C\u003EFontData\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyTextPanelComponent.FontData, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyTextPanelComponent.FontData owner, in string value) => owner.Name = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyTextPanelComponent.FontData owner, out string value) => value = owner.Name;
      }

      protected class Sandbox_Game_Entities_Blocks_MyTextPanelComponent\u003C\u003EFontData\u003C\u003ETextColor\u003C\u003EAccessor : IMemberAccessor<MyTextPanelComponent.FontData, Color>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyTextPanelComponent.FontData owner, in Color value) => owner.TextColor = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyTextPanelComponent.FontData owner, out Color value) => value = owner.TextColor;
      }

      protected class Sandbox_Game_Entities_Blocks_MyTextPanelComponent\u003C\u003EFontData\u003C\u003EAlignment\u003C\u003EAccessor : IMemberAccessor<MyTextPanelComponent.FontData, TextAlignment>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyTextPanelComponent.FontData owner, in TextAlignment value) => owner.Alignment = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyTextPanelComponent.FontData owner, out TextAlignment value) => value = owner.Alignment;
      }

      protected class Sandbox_Game_Entities_Blocks_MyTextPanelComponent\u003C\u003EFontData\u003C\u003ESize\u003C\u003EAccessor : IMemberAccessor<MyTextPanelComponent.FontData, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyTextPanelComponent.FontData owner, in float value) => owner.Size = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyTextPanelComponent.FontData owner, out float value) => value = owner.Size;
      }

      private class Sandbox_Game_Entities_Blocks_MyTextPanelComponent\u003C\u003EFontData\u003C\u003EActor : IActivator, IActivator<MyTextPanelComponent.FontData>
      {
        object IActivator.CreateInstance() => (object) new MyTextPanelComponent.FontData();

        MyTextPanelComponent.FontData IActivator<MyTextPanelComponent.FontData>.CreateInstance() => new MyTextPanelComponent.FontData();
      }
    }

    [ProtoContract]
    [Serializable]
    public struct ContentMetadata
    {
      [ProtoMember(1)]
      public ContentType ContentType;
      [ProtoMember(4)]
      public Color BackgroundColor;
      [ProtoMember(7)]
      public float ChangeInterval;
      [ProtoMember(10)]
      public bool PreserveAspectRatio;
      [ProtoMember(13)]
      public float TextPadding;
      [ProtoMember(16)]
      public byte BackgroundAlpha;

      public ContentMetadata(
        ContentType contentType,
        Color backgroundColor,
        float changeInterval,
        bool preserveAspectRatio,
        float textPadding,
        byte backgroundAlpha)
      {
        this.ContentType = contentType;
        this.BackgroundColor = backgroundColor;
        this.ChangeInterval = changeInterval;
        this.PreserveAspectRatio = preserveAspectRatio;
        this.TextPadding = textPadding;
        this.BackgroundAlpha = backgroundAlpha;
      }

      protected class Sandbox_Game_Entities_Blocks_MyTextPanelComponent\u003C\u003EContentMetadata\u003C\u003EContentType\u003C\u003EAccessor : IMemberAccessor<MyTextPanelComponent.ContentMetadata, ContentType>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyTextPanelComponent.ContentMetadata owner,
          in ContentType value)
        {
          owner.ContentType = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyTextPanelComponent.ContentMetadata owner,
          out ContentType value)
        {
          value = owner.ContentType;
        }
      }

      protected class Sandbox_Game_Entities_Blocks_MyTextPanelComponent\u003C\u003EContentMetadata\u003C\u003EBackgroundColor\u003C\u003EAccessor : IMemberAccessor<MyTextPanelComponent.ContentMetadata, Color>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyTextPanelComponent.ContentMetadata owner, in Color value) => owner.BackgroundColor = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyTextPanelComponent.ContentMetadata owner, out Color value) => value = owner.BackgroundColor;
      }

      protected class Sandbox_Game_Entities_Blocks_MyTextPanelComponent\u003C\u003EContentMetadata\u003C\u003EChangeInterval\u003C\u003EAccessor : IMemberAccessor<MyTextPanelComponent.ContentMetadata, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyTextPanelComponent.ContentMetadata owner, in float value) => owner.ChangeInterval = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyTextPanelComponent.ContentMetadata owner, out float value) => value = owner.ChangeInterval;
      }

      protected class Sandbox_Game_Entities_Blocks_MyTextPanelComponent\u003C\u003EContentMetadata\u003C\u003EPreserveAspectRatio\u003C\u003EAccessor : IMemberAccessor<MyTextPanelComponent.ContentMetadata, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyTextPanelComponent.ContentMetadata owner, in bool value) => owner.PreserveAspectRatio = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyTextPanelComponent.ContentMetadata owner, out bool value) => value = owner.PreserveAspectRatio;
      }

      protected class Sandbox_Game_Entities_Blocks_MyTextPanelComponent\u003C\u003EContentMetadata\u003C\u003ETextPadding\u003C\u003EAccessor : IMemberAccessor<MyTextPanelComponent.ContentMetadata, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyTextPanelComponent.ContentMetadata owner, in float value) => owner.TextPadding = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyTextPanelComponent.ContentMetadata owner, out float value) => value = owner.TextPadding;
      }

      protected class Sandbox_Game_Entities_Blocks_MyTextPanelComponent\u003C\u003EContentMetadata\u003C\u003EBackgroundAlpha\u003C\u003EAccessor : IMemberAccessor<MyTextPanelComponent.ContentMetadata, byte>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyTextPanelComponent.ContentMetadata owner, in byte value) => owner.BackgroundAlpha = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyTextPanelComponent.ContentMetadata owner, out byte value) => value = owner.BackgroundAlpha;
      }

      private class Sandbox_Game_Entities_Blocks_MyTextPanelComponent\u003C\u003EContentMetadata\u003C\u003EActor : IActivator, IActivator<MyTextPanelComponent.ContentMetadata>
      {
        object IActivator.CreateInstance() => (object) new MyTextPanelComponent.ContentMetadata();

        MyTextPanelComponent.ContentMetadata IActivator<MyTextPanelComponent.ContentMetadata>.CreateInstance() => new MyTextPanelComponent.ContentMetadata();
      }
    }

    [ProtoContract]
    [Serializable]
    public struct ScriptData
    {
      [ProtoMember(1)]
      public string Script;
      [ProtoMember(4)]
      public bool CustomizeScript;
      [ProtoMember(7)]
      public Color BackgroundColor;
      [ProtoMember(10)]
      public Color ForegroundColor;

      public ScriptData(
        string script,
        bool customizeScript,
        Color backgroundColor,
        Color foregroundColor)
      {
        this.Script = script;
        this.CustomizeScript = customizeScript;
        this.BackgroundColor = backgroundColor;
        this.ForegroundColor = foregroundColor;
      }

      protected class Sandbox_Game_Entities_Blocks_MyTextPanelComponent\u003C\u003EScriptData\u003C\u003EScript\u003C\u003EAccessor : IMemberAccessor<MyTextPanelComponent.ScriptData, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyTextPanelComponent.ScriptData owner, in string value) => owner.Script = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyTextPanelComponent.ScriptData owner, out string value) => value = owner.Script;
      }

      protected class Sandbox_Game_Entities_Blocks_MyTextPanelComponent\u003C\u003EScriptData\u003C\u003ECustomizeScript\u003C\u003EAccessor : IMemberAccessor<MyTextPanelComponent.ScriptData, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyTextPanelComponent.ScriptData owner, in bool value) => owner.CustomizeScript = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyTextPanelComponent.ScriptData owner, out bool value) => value = owner.CustomizeScript;
      }

      protected class Sandbox_Game_Entities_Blocks_MyTextPanelComponent\u003C\u003EScriptData\u003C\u003EBackgroundColor\u003C\u003EAccessor : IMemberAccessor<MyTextPanelComponent.ScriptData, Color>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyTextPanelComponent.ScriptData owner, in Color value) => owner.BackgroundColor = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyTextPanelComponent.ScriptData owner, out Color value) => value = owner.BackgroundColor;
      }

      protected class Sandbox_Game_Entities_Blocks_MyTextPanelComponent\u003C\u003EScriptData\u003C\u003EForegroundColor\u003C\u003EAccessor : IMemberAccessor<MyTextPanelComponent.ScriptData, Color>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyTextPanelComponent.ScriptData owner, in Color value) => owner.ForegroundColor = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyTextPanelComponent.ScriptData owner, out Color value) => value = owner.ForegroundColor;
      }

      private class Sandbox_Game_Entities_Blocks_MyTextPanelComponent\u003C\u003EScriptData\u003C\u003EActor : IActivator, IActivator<MyTextPanelComponent.ScriptData>
      {
        object IActivator.CreateInstance() => (object) new MyTextPanelComponent.ScriptData();

        MyTextPanelComponent.ScriptData IActivator<MyTextPanelComponent.ScriptData>.CreateInstance() => new MyTextPanelComponent.ScriptData();
      }
    }

    protected class m_fontData\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<MyTextPanelComponent.FontData, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<MyTextPanelComponent.FontData, SyncDirection.BothWays>(obj1, obj2));
        ((MyTextPanelComponent) obj0).m_fontData = (VRage.Sync.Sync<MyTextPanelComponent.FontData, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_contentData\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<MyTextPanelComponent.ContentMetadata, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<MyTextPanelComponent.ContentMetadata, SyncDirection.BothWays>(obj1, obj2));
        ((MyTextPanelComponent) obj0).m_contentData = (VRage.Sync.Sync<MyTextPanelComponent.ContentMetadata, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_scriptData\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<MyTextPanelComponent.ScriptData, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<MyTextPanelComponent.ScriptData, SyncDirection.BothWays>(obj1, obj2));
        ((MyTextPanelComponent) obj0).m_scriptData = (VRage.Sync.Sync<MyTextPanelComponent.ScriptData, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }
  }
}
