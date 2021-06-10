// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugSpawnMenu
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.World;
using Sandbox.Game.World.Generator;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Voxels;
using VRage.Input;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Gui
{
  [StaticEventOwner]
  internal class MyGuiScreenDebugSpawnMenu : MyGuiScreenDebugBase
  {
    private static readonly Vector2 SCREEN_SIZE = new Vector2(0.4f, 1.2f);
    private static readonly float HIDDEN_PART_RIGHT = 0.04f;
    private readonly Vector2 m_controlPadding = new Vector2(0.02f, 0.02f);
    private MyGuiControlListbox m_asteroidListBox;
    private MyGuiControlListbox m_materialTypeListbox;
    private MyGuiControlListbox m_physicalObjectListbox;
    private static MyPhysicalItemDefinition m_lastSelectedPhysicalItemDefinition;
    private MyGuiControlListbox m_asteroidTypeListbox;
    private MyGuiControlListbox m_planetListbox;
    private static string m_lastSelectedAsteroidName = (string) null;
    private MyGuiControlTextbox m_amountTextbox;
    private MyGuiControlLabel m_errorLabel;
    private static long m_amount = 1;
    private static int m_asteroidCounter = 0;
    private static float m_procAsteroidSizeValue = 64f;
    private static string m_procAsteroidSeedValue = "12345";
    private MyGuiControlSlider m_procAsteroidSize;
    private MyGuiControlTextbox m_procAsteroidSeed;
    private static string m_selectedPlanetName;
    private static int m_selectedScreen;
    private static string m_selectedVoxelmapMaterial;
    private MyGuiScreenDebugSpawnMenu.Screen[] Screens;
    public static MyGuiScreenDebugSpawnMenu.SpawnAsteroidInfo m_lastAsteroidInfo;
    private MyGuiControlSlider m_planetSizeSlider;
    private MyGuiControlTextbox m_procPlanetSeedValue;

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugSpawnMenu);

    public MyGuiScreenDebugSpawnMenu()
      : base(new Vector2(MyGuiManager.GetMaxMouseCoord().X - MyGuiScreenDebugSpawnMenu.SCREEN_SIZE.X * 0.5f + MyGuiScreenDebugSpawnMenu.HIDDEN_PART_RIGHT, 0.5f), new Vector2?(MyGuiScreenDebugSpawnMenu.SCREEN_SIZE), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), false)
    {
      this.m_backgroundTransition = MySandboxGame.Config.UIBkOpacity;
      this.m_guiTransition = MySandboxGame.Config.UIOpacity;
      this.CanBeHidden = true;
      this.CanHideOthers = false;
      this.m_canCloseInCloseAllScreenCalls = true;
      this.m_canShareInput = true;
      this.m_isTopScreen = false;
      this.m_isTopMostScreen = false;
      this.Screens = new MyGuiScreenDebugSpawnMenu.Screen[5]
      {
        new MyGuiScreenDebugSpawnMenu.Screen()
        {
          Name = MyTexts.GetString(MySpaceTexts.ScreenDebugSpawnMenu_Items),
          Creator = new MyGuiScreenDebugSpawnMenu.CreateScreen(this.CreateObjectsSpawnMenu)
        },
        new MyGuiScreenDebugSpawnMenu.Screen()
        {
          Name = MyTexts.GetString(MySpaceTexts.ScreenDebugSpawnMenu_Asteroids),
          Creator = new MyGuiScreenDebugSpawnMenu.CreateScreen(this.CreateAsteroidsSpawnMenu)
        },
        new MyGuiScreenDebugSpawnMenu.Screen()
        {
          Name = MyTexts.GetString(MySpaceTexts.ScreenDebugSpawnMenu_ProceduralAsteroids),
          Creator = new MyGuiScreenDebugSpawnMenu.CreateScreen(this.CreateProceduralAsteroidsSpawnMenu)
        },
        new MyGuiScreenDebugSpawnMenu.Screen()
        {
          Name = MyTexts.GetString(MySpaceTexts.ScreenDebugSpawnMenu_Planets),
          Creator = new MyGuiScreenDebugSpawnMenu.CreateScreen(this.CreatePlanetsSpawnMenu)
        },
        new MyGuiScreenDebugSpawnMenu.Screen()
        {
          Name = MyTexts.GetString(MySpaceTexts.ScreenDebugSpawnMenu_EmptyVoxelMap),
          Creator = new MyGuiScreenDebugSpawnMenu.CreateScreen(this.CreateEmptyVoxelMapSpawnMenu)
        }
      };
      this.RecreateControls(true);
    }

    private void CreateScreenSelector()
    {
      this.m_currentPosition.X += 0.018f;
      this.m_currentPosition.Y += (float) ((double) MyGuiConstants.SCREEN_CAPTION_DELTA_Y + (double) this.m_controlPadding.Y - 0.0710000023245811);
      MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
      int num = localHumanPlayer == null ? 0 : (MySession.Static.IsUserSpaceMaster(localHumanPlayer.Id.SteamId) ? 1 : 0);
      MyGuiControlCombobox combo = this.AddCombo();
      combo.AddItem(0L, MySpaceTexts.ScreenDebugSpawnMenu_Items);
      if (num != 0)
      {
        if (MySession.Static.Settings.PredefinedAsteroids)
          combo.AddItem(1L, MySpaceTexts.ScreenDebugSpawnMenu_PredefinedAsteroids);
        combo.AddItem(2L, MySpaceTexts.ScreenDebugSpawnMenu_ProceduralAsteroids);
        if (MyFakes.ENABLE_PLANETS)
          combo.AddItem(3L, MySpaceTexts.ScreenDebugSpawnMenu_Planets);
        combo.AddItem(4L, MySpaceTexts.ScreenDebugSpawnMenu_EmptyVoxelMap);
      }
      combo.SelectItemByKey((long) MyGuiScreenDebugSpawnMenu.m_selectedScreen);
      combo.ItemSelected += (MyGuiControlCombobox.ItemSelectedDelegate) (() =>
      {
        MyGuiScreenDebugSpawnMenu.m_selectedScreen = (int) combo.GetSelectedKey();
        this.RecreateControls(false);
      });
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      Vector2 vector2_1 = new Vector2(-0.05f, 0.0f);
      Vector2 vector2_2 = new Vector2(0.02f, 0.02f);
      float num1 = 0.8f;
      float separatorSize = 0.01f;
      float usableWidth = (float) ((double) MyGuiScreenDebugSpawnMenu.SCREEN_SIZE.X - (double) MyGuiScreenDebugSpawnMenu.HIDDEN_PART_RIGHT - (double) vector2_2.X * 2.0);
      float num2 = (float) (((double) MyGuiScreenDebugSpawnMenu.SCREEN_SIZE.Y - 1.0) / 2.0);
      this.m_currentPosition = -this.m_size.Value / 2f;
      this.m_currentPosition = this.m_currentPosition + vector2_2;
      this.m_currentPosition.Y += num2;
      this.m_scale = num1;
      this.AddCaption(MyTexts.Get(MySpaceTexts.ScreenDebugSpawnMenu_Caption).ToString(), new Vector4?(Color.White.ToVector4()), new Vector2?(vector2_2 + new Vector2(-MyGuiScreenDebugSpawnMenu.HIDDEN_PART_RIGHT, num2 - 0.03f)));
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.829999983310699 / 2.0), 0.44f), this.m_size.Value.X * 0.73f);
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.829999983310699 / 2.0), 0.365f), this.m_size.Value.X * 0.73f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList);
      this.m_currentPosition.Y += MyGuiConstants.SCREEN_CAPTION_DELTA_Y + separatorSize;
      this.CreateMenu(separatorSize, usableWidth);
    }

    private void CreateMenu(float separatorSize, float usableWidth)
    {
      this.CreateScreenSelector();
      this.Screens[MyGuiScreenDebugSpawnMenu.m_selectedScreen].Creator(separatorSize, usableWidth);
    }

    private void CreateAsteroidsSpawnMenu(float separatorSize, float usableWidth)
    {
      this.m_currentPosition.Y += 0.025f;
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
      myGuiControlLabel1.Position = new Vector2(this.m_currentPosition.X + 1f / 1000f, this.m_currentPosition.Y);
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel1.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugSpawnMenu_Asteroid);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel1);
      this.m_currentPosition.Y += 0.03f;
      this.m_asteroidTypeListbox = this.AddListBox(0.4f);
      this.m_asteroidTypeListbox.MultiSelect = false;
      this.m_asteroidTypeListbox.VisibleRowsCount = 12;
      foreach (MyDefinitionBase myDefinitionBase in (IEnumerable<MyVoxelMapStorageDefinition>) MyDefinitionManager.Static.GetVoxelMapStorageDefinitions().OrderBy<MyVoxelMapStorageDefinition, string>((Func<MyVoxelMapStorageDefinition, string>) (e => e.Id.SubtypeId.ToString())))
      {
        string toolTip = myDefinitionBase.Id.SubtypeId.ToString();
        this.m_asteroidTypeListbox.Add(new MyGuiControlListbox.Item(new StringBuilder(toolTip), toolTip, userData: ((object) toolTip)));
      }
      this.m_asteroidTypeListbox.ItemsSelected += new Action<MyGuiControlListbox>(this.OnAsteroidTypeListbox_ItemSelected);
      this.m_asteroidTypeListbox.ItemDoubleClicked += new Action<MyGuiControlListbox>(this.OnLoadAsteroid);
      if (this.m_asteroidTypeListbox.Items.Count > 0)
      {
        MyGuiControlListbox.Item obj = this.m_asteroidTypeListbox.Items.Where<MyGuiControlListbox.Item>((Func<MyGuiControlListbox.Item, bool>) (x => x.UserData as string == MyGuiScreenDebugSpawnMenu.m_lastSelectedAsteroidName)).FirstOrDefault<MyGuiControlListbox.Item>();
        if (obj != null)
          this.m_asteroidTypeListbox.SelectedItems.Add(obj);
      }
      this.m_asteroidTypeListbox.ScrollToFirstSelection();
      this.OnAsteroidTypeListbox_ItemSelected(this.m_asteroidTypeListbox);
      this.m_currentPosition.Y += 0.03f;
      MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel();
      myGuiControlLabel2.Position = new Vector2(this.m_currentPosition.X + 1f / 1000f, this.m_currentPosition.Y);
      myGuiControlLabel2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel2.Text = "Material:";
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      this.m_currentPosition.Y += 0.03f;
      this.m_materialTypeListbox = this.AddListBox(0.2f);
      this.m_materialTypeListbox.MultiSelect = false;
      this.m_materialTypeListbox.VisibleRowsCount = 7;
      this.m_materialTypeListbox.ItemsSelected += new Action<MyGuiControlListbox>(this.OnMaterialTypeListbox_ItemSelected);
      this.m_materialTypeListbox.Add(new MyGuiControlListbox.Item(new StringBuilder("<keep original>"), "Keep original asteroid material"));
      foreach (MyDefinitionBase myDefinitionBase in (IEnumerable<MyVoxelMaterialDefinition>) MyDefinitionManager.Static.GetVoxelMaterialDefinitions().OrderBy<MyVoxelMaterialDefinition, string>((Func<MyVoxelMaterialDefinition, string>) (e => e.Id.SubtypeId.ToString())))
      {
        string toolTip = myDefinitionBase.Id.SubtypeId.ToString();
        this.m_materialTypeListbox.Add(new MyGuiControlListbox.Item(new StringBuilder(toolTip), toolTip, userData: ((object) toolTip)));
      }
      if (this.m_materialTypeListbox.Items.Count > 0)
      {
        MyGuiControlListbox.Item obj = this.m_materialTypeListbox.Items.Where<MyGuiControlListbox.Item>((Func<MyGuiControlListbox.Item, bool>) (x => x.UserData as string == MyGuiScreenDebugSpawnMenu.m_selectedVoxelmapMaterial)).FirstOrDefault<MyGuiControlListbox.Item>();
        if (obj != null)
          this.m_materialTypeListbox.SelectedItems.Add(obj);
      }
      this.m_materialTypeListbox.ScrollToFirstSelection();
      this.m_currentPosition.Y += 0.04f;
      this.CreateDebugButton(0.284f, MySpaceTexts.ScreenDebugSpawnMenu_SpawnAsteroid, new Action<MyGuiControlButton>(this.OnLoadAsteroid)).PositionX += 1f / 500f;
    }

    private void CreateProceduralAsteroidsSpawnMenu(float separatorSize, float usableWidth)
    {
      this.m_currentPosition.Y += 0.025f;
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
      myGuiControlLabel1.Position = new Vector2(this.m_currentPosition.X + 1f / 1000f, this.m_currentPosition.Y);
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel1.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugSpawnMenu_ProceduralSize);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel1);
      this.m_currentPosition.Y += 0.03f;
      this.m_procAsteroidSize = new MyGuiControlSlider(new Vector2?(this.m_currentPosition), 5f, 2000f, 0.285f, labelText: string.Empty, labelDecimalPlaces: 2, labelScale: (0.75f * this.m_scale), labelFont: "Debug", originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      this.m_procAsteroidSize.DebugScale = this.m_sliderDebugScale;
      this.m_procAsteroidSize.ColorMask = Color.White.ToVector4();
      this.Controls.Add((MyGuiControlBase) this.m_procAsteroidSize);
      MyGuiControlLabel label = new MyGuiControlLabel(new Vector2?(this.m_currentPosition + new Vector2(this.m_procAsteroidSize.Size.X - 3f / 1000f, this.m_procAsteroidSize.Size.Y - 0.065f)), text: string.Empty, colorMask: new Vector4?(Color.White.ToVector4()), font: "Debug");
      label.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      this.Controls.Add((MyGuiControlBase) label);
      this.m_procAsteroidSize.ValueChanged += (Action<MyGuiControlSlider>) (s =>
      {
        label.Text = MyValueFormatter.GetFormatedFloat(s.Value, 2) + "m";
        MyGuiScreenDebugSpawnMenu.m_procAsteroidSizeValue = s.Value;
      });
      this.m_procAsteroidSize.Value = MyGuiScreenDebugSpawnMenu.m_procAsteroidSizeValue;
      this.m_currentPosition.Y += this.m_procAsteroidSize.Size.Y;
      this.m_currentPosition.Y += separatorSize;
      MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel();
      myGuiControlLabel2.Position = new Vector2(this.m_currentPosition.X + 1f / 1000f, this.m_currentPosition.Y);
      myGuiControlLabel2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel2.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugSpawnMenu_ProceduralSeed);
      myGuiControlLabel2.IsAutoEllipsisEnabled = true;
      myGuiControlLabel2.IsAutoScaleEnabled = true;
      MyGuiControlLabel myGuiControlLabel3 = myGuiControlLabel2;
      myGuiControlLabel3.SetMaxWidth(this.m_procAsteroidSize.Size.X);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel3);
      this.m_currentPosition.Y += 0.03f;
      this.m_procAsteroidSeed = new MyGuiControlTextbox(new Vector2?(this.m_currentPosition), MyGuiScreenDebugSpawnMenu.m_procAsteroidSeedValue, 20, new Vector4?(Color.White.ToVector4()), this.m_scale);
      this.m_procAsteroidSeed.TextChanged += (Action<MyGuiControlTextbox>) (t => MyGuiScreenDebugSpawnMenu.m_procAsteroidSeedValue = t.Text);
      this.m_procAsteroidSeed.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_procAsteroidSeed.Size = new Vector2(0.285f, 1f);
      this.Controls.Add((MyGuiControlBase) this.m_procAsteroidSeed);
      this.m_currentPosition.Y += this.m_procAsteroidSize.Size.Y + separatorSize;
      this.m_currentPosition.Y -= 0.015f;
      MyGuiControlButton debugButton1 = this.CreateDebugButton(0.284f, MySpaceTexts.ScreenDebugSpawnMenu_GenerateSeed, new Action<MyGuiControlButton>(this.generateSeedButton_OnButtonClick));
      debugButton1.PositionX += 1f / 500f;
      this.m_currentPosition.Y -= 0.01f;
      MyGuiControlButton debugButton2 = this.CreateDebugButton(0.284f, MySpaceTexts.ScreenDebugSpawnMenu_SpawnAsteroid, new Action<MyGuiControlButton>(this.OnSpawnProceduralAsteroid));
      debugButton2.PositionX += 1f / 500f;
      this.m_currentPosition.Y += 0.01f;
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(this.m_currentPosition + new Vector2(1f / 500f, 0.0f)), text: MyTexts.GetString(MySpaceTexts.ScreenDebugSpawnMenu_AsteroidGenerationCanTakeLong), colorMask: new Vector4?(Color.Red.ToVector4()), textScale: (0.8f * this.m_scale), font: "Debug"));
      this.m_currentPosition.Y += 0.03f;
      MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
      bool flag = localHumanPlayer != null && MySession.Static.IsUserSpaceMaster(localHumanPlayer.Id.SteamId);
      if (!flag)
        this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(this.m_currentPosition + new Vector2(1f / 500f, 0.0f)), text: MyTexts.GetString(MyCommonTexts.Warning_SpacemasterOrHigherRequired), colorMask: new Vector4?(Color.Red.ToVector4()), textScale: (0.8f * this.m_scale), font: "Debug"));
      this.m_procAsteroidSize.Enabled = flag;
      this.m_procAsteroidSeed.Enabled = flag;
      debugButton1.Enabled = flag;
      debugButton2.Enabled = flag;
    }

    private void CreateObjectsSpawnMenu(float separatorSize, float usableWidth)
    {
      this.m_currentPosition.Y += 0.025f;
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
      myGuiControlLabel1.Position = new Vector2(this.m_currentPosition.X + 1f / 1000f, this.m_currentPosition.Y);
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel1.Text = "Select Item :";
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel1);
      this.m_currentPosition.Y += 0.03f;
      this.m_physicalObjectListbox = this.AddListBox(0.585f);
      this.m_physicalObjectListbox.IsAutoscaleEnabled = true;
      this.m_physicalObjectListbox.MultiSelect = false;
      this.m_physicalObjectListbox.VisibleRowsCount = 17;
      foreach (MyPhysicalItemDefinition physicalItemDefinition in (IEnumerable<MyPhysicalItemDefinition>) MyDefinitionManager.Static.GetAllDefinitions().Where<MyDefinitionBase>((Func<MyDefinitionBase, bool>) (e => e is MyPhysicalItemDefinition && e.Public)).Cast<MyPhysicalItemDefinition>().OrderBy<MyPhysicalItemDefinition, string>((Func<MyPhysicalItemDefinition, string>) (e => e.DisplayNameText)))
      {
        if (physicalItemDefinition.CanSpawnFromScreen)
        {
          string icon = physicalItemDefinition.Icons == null || physicalItemDefinition.Icons.Length == 0 ? MyGuiConstants.TEXTURE_ICON_FAKE.Texture : physicalItemDefinition.Icons[0];
          MyGuiControlListbox.Item obj = new MyGuiControlListbox.Item(new StringBuilder(physicalItemDefinition.DisplayNameText), physicalItemDefinition.DisplayNameText, icon, (object) physicalItemDefinition);
          this.m_physicalObjectListbox.Items.Add(obj);
          if (physicalItemDefinition == MyGuiScreenDebugSpawnMenu.m_lastSelectedPhysicalItemDefinition)
            this.m_physicalObjectListbox.SelectedItems.Add(obj);
        }
      }
      this.m_physicalObjectListbox.ItemsSelected += new Action<MyGuiControlListbox>(this.OnPhysicalObjectListbox_ItemSelected);
      this.m_physicalObjectListbox.ItemDoubleClicked += new Action<MyGuiControlListbox>(this.OnSpawnPhysicalObject);
      this.OnPhysicalObjectListbox_ItemSelected(this.m_physicalObjectListbox);
      MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel();
      myGuiControlLabel2.Position = new Vector2(this.m_currentPosition.X + 1f / 1000f, this.m_currentPosition.Y);
      myGuiControlLabel2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel2.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugSpawnMenu_ItemAmount);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      this.m_currentPosition.Y += 0.03f;
      this.m_amountTextbox = new MyGuiControlTextbox(new Vector2?(this.m_currentPosition), MyGuiScreenDebugSpawnMenu.m_amount.ToString(), 6, textScale: this.m_scale, type: MyGuiControlTextboxType.DigitsOnly);
      this.m_amountTextbox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_amountTextbox.TextChanged += new Action<MyGuiControlTextbox>(this.OnAmountTextChanged);
      this.Controls.Add((MyGuiControlBase) this.m_amountTextbox);
      this.m_amountTextbox.Size = new Vector2(0.285f, 1f);
      this.m_currentPosition.Y -= 0.02f;
      this.m_currentPosition.Y += separatorSize + this.m_amountTextbox.Size.Y;
      this.m_errorLabel = this.AddLabel(MyTexts.GetString(MySpaceTexts.ScreenDebugSpawnMenu_InvalidAmount), Color.Red.ToVector4(), 0.8f);
      this.m_errorLabel.PositionX += 0.282f;
      this.m_errorLabel.PositionY -= 0.04f;
      this.m_errorLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      this.m_errorLabel.Visible = false;
      this.m_currentPosition.Y += 0.01f;
      this.CreateDebugButton(0.284f, MySpaceTexts.ScreenDebugSpawnMenu_SpawnObject, new Action<MyGuiControlButton>(this.OnSpawnPhysicalObject)).PositionX += 1f / 500f;
      MyEntity myEntity = (MyEntity) null;
      bool enabled = false;
      MyCharacterDetectorComponent component;
      if (MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.Components.TryGet<MyCharacterDetectorComponent>(out component) && component.UseObject != null)
        myEntity = component.DetectedEntity as MyEntity;
      string str = "-";
      if (myEntity != null && myEntity.HasInventory)
      {
        str = myEntity.DisplayNameText ?? myEntity.Name;
        enabled = true;
      }
      this.m_currentPosition.Y -= 0.015f;
      this.CreateDebugButton(0.284f, MySpaceTexts.ScreenDebugSpawnMenu_SpawnTargeted, new Action<MyGuiControlButton>(this.OnSpawnIntoContainer), enabled).PositionX += 1f / 500f;
      this.AddLabel(MyTexts.GetString(MySpaceTexts.ScreenDebugSpawnMenu_CurrentTarget) + str, Color.White.ToVector4(), this.m_scale);
    }

    private static float NormalizeLog(float f, float min, float max) => MathHelper.Clamp(MathHelper.InterpLogInv(f, min, max), 0.0f, 1f);

    private static float DenormalizeLog(float f, float min, float max) => MathHelper.Clamp(MathHelper.InterpLog(f, min, max), min, max);

    private void UpdateLayerSlider(MyGuiControlSlider slider, float minValue, float maxValue)
    {
      slider.Value = MathHelper.Max(minValue, MathHelper.Min(slider.Value, maxValue));
      slider.MaxValue = maxValue;
      slider.MinValue = minValue;
    }

    private void OnAmountTextChanged(MyGuiControlTextbox textbox) => this.m_errorLabel.Visible = false;

    private bool IsValidAmount() => long.TryParse(this.m_amountTextbox.Text, out MyGuiScreenDebugSpawnMenu.m_amount) && MyGuiScreenDebugSpawnMenu.m_amount >= 1L;

    private void OnAsteroidTypeListbox_ItemSelected(MyGuiControlListbox listbox)
    {
      if (listbox.SelectedItems.Count == 0)
        return;
      MyGuiScreenDebugSpawnMenu.m_lastSelectedAsteroidName = listbox.SelectedItems[0].UserData as string;
    }

    private void OnMaterialTypeListbox_ItemSelected(MyGuiControlListbox listbox)
    {
      if (listbox.SelectedItems.Count == 0)
        return;
      MyGuiScreenDebugSpawnMenu.m_selectedVoxelmapMaterial = listbox.SelectedItems[0].UserData as string;
    }

    private void OnPlanetListbox_ItemSelected(MyGuiControlListbox listbox)
    {
      if (listbox.SelectedItems.Count == 0)
        return;
      MyGuiScreenDebugSpawnMenu.m_selectedPlanetName = listbox.SelectedItems[0].UserData as string;
    }

    private void OnPhysicalObjectListbox_ItemSelected(MyGuiControlListbox listbox)
    {
      if (listbox.SelectedItems.Count == 0)
        return;
      MyGuiScreenDebugSpawnMenu.m_lastSelectedPhysicalItemDefinition = listbox.SelectedItems[0].UserData as MyPhysicalItemDefinition;
    }

    private void ScreenAsteroids(object _)
    {
      MyGuiControlListbox.Item[] selectedItems = this.m_asteroidListBox.SelectedItems.ToArray();
      if (selectedItems.Length == 0)
      {
        MyScreenManager.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder("No asteroids selected"), messageCaption: new StringBuilder("Error"), canHideOthers: false));
      }
      else
      {
        string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), MyPerGameSettings.GameNameSafe + "_AsteroidScreens");
        int state = 0;
        int pauseTimeout = 0;
        int asteroidIndex = 0;
        Action stateMachine = (Action) null;
        stateMachine = (Action) (() =>
        {
          if (pauseTimeout > 0)
          {
            --pauseTimeout;
          }
          else
          {
            MyVoxelMapStorageDefinition userData = (MyVoxelMapStorageDefinition) selectedItems[asteroidIndex].UserData;
            switch (state)
            {
              case 0:
                this.SpawnVoxelPreview(userData.Id.SubtypeName);
                pauseTimeout = 100;
                break;
              case 1:
                if (!MyClipboardComponent.Static.IsActive)
                {
                  MyScreenManager.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder("Screening interrupted"), messageCaption: new StringBuilder("Done"), canHideOthers: false));
                  return;
                }
                string pathToSave = Path.Combine(folder, userData.Id.SubtypeName + ".png");
                MyRenderProxy.TakeScreenshot(Vector2.One, pathToSave, false, true, false);
                pauseTimeout = 10;
                ++asteroidIndex;
                if (asteroidIndex == selectedItems.Length)
                {
                  StringBuilder messageCaption = new StringBuilder("Done");
                  MyScreenManager.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder("All screens saved to\n" + folder), messageCaption: messageCaption, canHideOthers: false));
                  return;
                }
                break;
            }
            ++state;
            if (state > 1)
              state = 0;
          }
          MySandboxGame.Static.Invoke(stateMachine, "Asteroid screening");
        });
        MySandboxGame.Static.Invoke(stateMachine, "Asteroid screening");
        this.CloseScreenNow();
      }
    }

    private void SpawnFloatingObjectPreview()
    {
      if (MyGuiScreenDebugSpawnMenu.m_lastSelectedPhysicalItemDefinition == null)
        return;
      MyDefinitionId id = MyGuiScreenDebugSpawnMenu.m_lastSelectedPhysicalItemDefinition.Id;
      MyFixedPoint myFixedPoint = (MyFixedPoint) (Decimal) MyGuiScreenDebugSpawnMenu.m_amount;
      MyObjectBuilder_PhysicalObject newObject1 = (MyObjectBuilder_PhysicalObject) MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) id);
      switch (newObject1)
      {
        case MyObjectBuilder_PhysicalGunObject _:
        case MyObjectBuilder_OxygenContainerObject _:
        case MyObjectBuilder_GasContainerObject _:
          myFixedPoint = (MyFixedPoint) 1;
          break;
      }
      MyObjectBuilder_FloatingObject newObject2 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_FloatingObject>();
      newObject2.PositionAndOrientation = new MyPositionAndOrientation?(MyPositionAndOrientation.Default);
      newObject2.Item = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_InventoryItem>();
      newObject2.Item.Amount = myFixedPoint;
      newObject2.Item.PhysicalContent = newObject1;
      MyClipboardComponent.Static.ActivateFloatingObjectClipboard(newObject2, Vector3D.Zero, 1f);
    }

    private MyGuiControlButton CreateDebugButton(
      float usableWidth,
      MyStringId text,
      Action<MyGuiControlButton> onClick,
      bool enabled = true,
      MyStringId? tooltip = null)
    {
      this.m_currentPosition.Y += 0.01f;
      MyGuiControlButton guiControlButton = this.AddButton(MyTexts.Get(text), onClick);
      guiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.Rectangular;
      guiControlButton.TextScale = this.m_scale;
      guiControlButton.Size = new Vector2(usableWidth, guiControlButton.Size.Y);
      guiControlButton.Position = guiControlButton.Position + new Vector2((float) (-(double) MyGuiScreenDebugSpawnMenu.HIDDEN_PART_RIGHT / 2.0), 0.0f);
      guiControlButton.Enabled = enabled;
      if (tooltip.HasValue)
        guiControlButton.SetToolTip(tooltip.Value);
      return guiControlButton;
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      base.HandleInput(receivedFocusInThisUpdate);
      if (!MyInput.Static.IsNewKeyPressed(MyKeys.F12) && !MyInput.Static.IsNewKeyPressed(MyKeys.F11) && !MyInput.Static.IsNewKeyPressed(MyKeys.F10))
        return;
      this.CloseScreen();
    }

    private static Matrix GetPasteMatrix()
    {
      if (MySession.Static.ControlledEntity == null || MySession.Static.GetCameraControllerEnum() != MyCameraControllerEnum.Entity && MySession.Static.GetCameraControllerEnum() != MyCameraControllerEnum.ThirdPersonSpectator)
        return (Matrix) ref MySector.MainCamera.WorldMatrix;
      MatrixD headMatrix = MySession.Static.ControlledEntity.GetHeadMatrix(true);
      return (Matrix) ref headMatrix;
    }

    private void OnSpawnPhysicalObject(object obj)
    {
      if (!this.IsValidAmount())
      {
        this.m_errorLabel.Visible = true;
      }
      else
      {
        this.SpawnFloatingObjectPreview();
        this.CloseScreenNow();
      }
    }

    private void OnSpawnIntoContainer(MyGuiControlButton myGuiControlButton)
    {
      if (!this.IsValidAmount() || MyGuiScreenDebugSpawnMenu.m_lastSelectedPhysicalItemDefinition == null)
      {
        this.m_errorLabel.Visible = true;
      }
      else
      {
        MyCharacterDetectorComponent component;
        if (MySession.Static.LocalCharacter == null || !MySession.Static.LocalCharacter.Components.TryGet<MyCharacterDetectorComponent>(out component) || (!(component.DetectedEntity is MyEntity detectedEntity) || !detectedEntity.HasInventory))
          return;
        SerializableDefinitionId id = (SerializableDefinitionId) MyGuiScreenDebugSpawnMenu.m_lastSelectedPhysicalItemDefinition.Id;
        MyMultiplayer.RaiseStaticEvent<long, SerializableDefinitionId, long, long>((Func<IMyEventOwner, Action<long, SerializableDefinitionId, long, long>>) (x => new Action<long, SerializableDefinitionId, long, long>(MyGuiScreenDebugSpawnMenu.SpawnIntoContainer_Implementation)), MyGuiScreenDebugSpawnMenu.m_amount, id, detectedEntity.EntityId, MySession.Static.LocalPlayerId);
      }
    }

    [Event(null, 851)]
    [Reliable]
    [Server]
    private static void SpawnIntoContainer_Implementation(
      long amount,
      SerializableDefinitionId item,
      long entityId,
      long playerId)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value) && !MySession.Static.CreativeToolsEnabled(MyEventContext.Current.Sender.Value))
      {
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        MyEntity entity;
        if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById(entityId, out entity) || !entity.HasInventory || entity is MyTerminalBlock && !((MyTerminalBlock) entity).HasPlayerAccess(playerId))
          return;
        MyInventory inventory = MyEntityExtensions.GetInventory(entity);
        if (!inventory.CheckConstraint((MyDefinitionId) item))
          return;
        MyFixedPoint amount1 = (MyFixedPoint) Math.Min((Decimal) amount, (Decimal) inventory.ComputeAmountThatFits((MyDefinitionId) item, 0.0f, 0.0f));
        inventory.AddItems(amount1, MyObjectBuilderSerializer.CreateNewObject(item));
      }
    }

    private void OnLoadAsteroid(object obj)
    {
      this.SpawnVoxelPreview();
      this.CloseScreenNow();
    }

    private void OnSpawnProceduralAsteroid(MyGuiControlButton obj)
    {
      this.SpawnProceduralAsteroid(this.GetProceduralAsteroidSeed(this.m_procAsteroidSeed), this.m_procAsteroidSize.Value);
      this.CloseScreenNow();
    }

    private void generateSeedButton_OnButtonClick(MyGuiControlButton sender) => this.m_procAsteroidSeed.Text = MyRandom.Instance.Next().ToString();

    private int GetProceduralAsteroidSeed(MyGuiControlTextbox textbox)
    {
      int result = 12345;
      if (!int.TryParse(textbox.Text, out result))
      {
        string text = textbox.Text;
        byte[] hash = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(text));
        int num = 0;
        for (int index = 0; index < 4 && index < hash.Length; ++index)
        {
          result |= (int) hash[index] << num;
          num += 8;
        }
      }
      return result;
    }

    public static MyStorageBase CreateAsteroidStorage(string asteroid)
    {
      if (MySandboxGame.Static.MemoryState < MySandboxGame.MemState.Low)
        return MyStorageBase.CreateAsteroidStorage(asteroid);
      MyHud.Notifications.Add((MyHudNotificationBase) new MyHudNotification(MyCommonTexts.Performance_LowOnMemory));
      return (MyStorageBase) null;
    }

    public static MyObjectBuilder_VoxelMap CreateAsteroidObjectBuilder(
      string storageName)
    {
      MyObjectBuilder_VoxelMap newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_VoxelMap>();
      newObject.StorageName = storageName;
      newObject.PersistentFlags = MyPersistentEntityFlags2.Enabled | MyPersistentEntityFlags2.InScene;
      newObject.PositionAndOrientation = new MyPositionAndOrientation?(MyPositionAndOrientation.Default);
      newObject.MutableStorage = false;
      return newObject;
    }

    private void SpawnVoxelPreview()
    {
      if (string.IsNullOrEmpty(MyGuiScreenDebugSpawnMenu.m_lastSelectedAsteroidName))
        return;
      this.SpawnVoxelPreview(MyGuiScreenDebugSpawnMenu.m_lastSelectedAsteroidName);
    }

    private void SpawnVoxelPreview(string storageNameBase)
    {
      string storageName = MyGuiScreenDebugSpawnMenu.MakeStorageName(storageNameBase);
      string userData = this.m_materialTypeListbox.SelectedItems[0].UserData as string;
      MyStorageBase predefinedDataStorage = MyGuiScreenDebugSpawnMenu.CreatePredefinedDataStorage(storageNameBase, userData);
      if (predefinedDataStorage == null)
        return;
      MyObjectBuilder_VoxelMap asteroidObjectBuilder = MyGuiScreenDebugSpawnMenu.CreateAsteroidObjectBuilder(storageName);
      MyGuiScreenDebugSpawnMenu.m_lastAsteroidInfo = new MyGuiScreenDebugSpawnMenu.SpawnAsteroidInfo()
      {
        Asteroid = storageNameBase,
        WorldMatrix = MatrixD.Identity,
        IsProcedural = false,
        VoxelMaterial = userData
      };
      if (predefinedDataStorage == null)
        return;
      MyClipboardComponent.Static.ActivateVoxelClipboard((MyObjectBuilder_EntityBase) asteroidObjectBuilder, (IMyStorage) predefinedDataStorage, MySector.MainCamera.ForwardVector, (predefinedDataStorage.Size * 0.5f).Length());
    }

    public static MyStorageBase CreatePredefinedDataStorage(
      string storageName,
      string voxelMaterial)
    {
      MyPredefinedDataProvider predefinedShape = MyPredefinedDataProvider.CreatePredefinedShape(storageName, voxelMaterial);
      return predefinedShape != null ? (MyStorageBase) new MyOctreeStorage((IMyStorageDataProvider) predefinedShape, predefinedShape.Storage.Size) : (MyStorageBase) null;
    }

    public static MyStorageBase CreateProceduralAsteroidStorage(int seed, float radius) => (MyStorageBase) new MyOctreeStorage((IMyStorageDataProvider) MyCompositeShapeProvider.CreateAsteroidShape(seed, radius), MyVoxelCoordSystems.FindBestOctreeSize(radius));

    private void SpawnProceduralAsteroid(int seed, float radius)
    {
      string storageName = MyGuiScreenDebugSpawnMenu.MakeStorageName("ProcAsteroid-" + (object) seed + "r" + (object) radius);
      MyStorageBase proceduralAsteroidStorage = MyGuiScreenDebugSpawnMenu.CreateProceduralAsteroidStorage(seed, radius);
      MyObjectBuilder_VoxelMap asteroidObjectBuilder = MyGuiScreenDebugSpawnMenu.CreateAsteroidObjectBuilder(storageName);
      MyGuiScreenDebugSpawnMenu.m_lastAsteroidInfo = new MyGuiScreenDebugSpawnMenu.SpawnAsteroidInfo()
      {
        Asteroid = (string) null,
        RandomSeed = seed,
        WorldMatrix = MatrixD.Identity,
        IsProcedural = true,
        ProceduralRadius = radius
      };
      MyClipboardComponent.Static.ActivateVoxelClipboard((MyObjectBuilder_EntityBase) asteroidObjectBuilder, (IMyStorage) proceduralAsteroidStorage, MySector.MainCamera.ForwardVector, (proceduralAsteroidStorage.Size * 0.5f).Length());
    }

    public static void RecreateAsteroidBeforePaste(float dragVectorLength)
    {
      int randomSeed = MyGuiScreenDebugSpawnMenu.m_lastAsteroidInfo.RandomSeed;
      float proceduralRadius = MyGuiScreenDebugSpawnMenu.m_lastAsteroidInfo.ProceduralRadius;
      string storageName = MyGuiScreenDebugSpawnMenu.MakeStorageName("ProcAsteroid-" + (object) randomSeed + "r" + (object) proceduralRadius);
      MyStorageBase myStorageBase;
      if (MyGuiScreenDebugSpawnMenu.m_lastAsteroidInfo.IsProcedural)
      {
        myStorageBase = MyGuiScreenDebugSpawnMenu.CreateProceduralAsteroidStorage(randomSeed, proceduralRadius);
      }
      else
      {
        bool useStorageCache = MyStorageBase.UseStorageCache;
        MyStorageBase.UseStorageCache = false;
        try
        {
          if (MyGuiScreenDebugSpawnMenu.m_lastAsteroidInfo.Asteroid != null)
          {
            myStorageBase = MyGuiScreenDebugSpawnMenu.CreatePredefinedDataStorage(MyGuiScreenDebugSpawnMenu.m_lastAsteroidInfo.Asteroid, MyGuiScreenDebugSpawnMenu.m_lastAsteroidInfo.VoxelMaterial);
            if (myStorageBase == null)
              return;
          }
          else
            myStorageBase = (MyStorageBase) new MyOctreeStorage((IMyStorageDataProvider) null, new Vector3I((int) MyGuiScreenDebugSpawnMenu.m_procAsteroidSizeValue));
        }
        finally
        {
          MyStorageBase.UseStorageCache = useStorageCache;
        }
      }
      MyObjectBuilder_VoxelMap asteroidObjectBuilder = MyGuiScreenDebugSpawnMenu.CreateAsteroidObjectBuilder(storageName);
      MyClipboardComponent.Static.ActivateVoxelClipboard((MyObjectBuilder_EntityBase) asteroidObjectBuilder, (IMyStorage) myStorageBase, MySector.MainCamera.ForwardVector, dragVectorLength);
    }

    public static string MakeStorageName(string storageNameBase)
    {
      string str = storageNameBase;
      int num = 0;
      bool flag;
      do
      {
        flag = false;
        foreach (MyVoxelBase instance in MySession.Static.VoxelMaps.Instances)
        {
          if (instance.StorageName == str)
          {
            flag = true;
            break;
          }
        }
        if (flag)
          str = storageNameBase + "-" + (object) num++;
      }
      while (flag);
      return str;
    }

    private static void AddSeparator(MyGuiControlList list)
    {
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.Size = new Vector2(1f, 0.01f);
      controlSeparatorList.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      controlSeparatorList.AddHorizontal(Vector2.Zero, 1f);
      list.Controls.Add((MyGuiControlBase) controlSeparatorList);
    }

    private MyGuiControlLabel CreateSliderWithDescription(
      float usableWidth,
      float min,
      float max,
      string description,
      ref MyGuiControlSlider slider)
    {
      this.AddLabel(description, Vector4.One, this.m_scale);
      this.CreateSlider(usableWidth, min, max, ref slider);
      return this.AddLabel("", Vector4.One, this.m_scale);
    }

    private void CreateSlider(
      float usableWidth,
      float min,
      float max,
      ref MyGuiControlSlider slider)
    {
      slider = this.AddSlider(string.Empty, 5f, min, max);
      slider.Size = new Vector2(400f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, slider.Size.Y);
      slider.LabelDecimalPlaces = 4;
      slider.DebugScale = this.m_sliderDebugScale;
      slider.ColorMask = Color.White.ToVector4();
    }

    public static string GetAsteroidName() => MyGuiScreenDebugSpawnMenu.m_lastAsteroidInfo.Asteroid;

    public static void SpawnAsteroid(MatrixD worldMatrix)
    {
      MyGuiScreenDebugSpawnMenu.m_lastAsteroidInfo.WorldMatrix = worldMatrix;
      if (!MySession.Static.HasCreativeRights && !MySession.Static.CreativeMode)
        return;
      MyMultiplayer.RaiseStaticEvent<MyGuiScreenDebugSpawnMenu.SpawnAsteroidInfo>((Func<IMyEventOwner, Action<MyGuiScreenDebugSpawnMenu.SpawnAsteroidInfo>>) (s => new Action<MyGuiScreenDebugSpawnMenu.SpawnAsteroidInfo>(MyGuiScreenDebugSpawnMenu.SpawnAsteroid)), MyGuiScreenDebugSpawnMenu.m_lastAsteroidInfo);
    }

    [Event(null, 1127)]
    [Reliable]
    [Server]
    private static void SpawnAsteroid(
      MyGuiScreenDebugSpawnMenu.SpawnAsteroidInfo asteroidInfo)
    {
      if (MyEventContext.Current.IsLocallyInvoked || MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
      {
        MyStorageBase myStorageBase;
        string storageName;
        if (!asteroidInfo.IsProcedural)
        {
          if (asteroidInfo.Asteroid == null)
          {
            myStorageBase = (MyStorageBase) new MyOctreeStorage((IMyStorageDataProvider) null, new Vector3I((int) asteroidInfo.ProceduralRadius));
          }
          else
          {
            myStorageBase = MyGuiScreenDebugSpawnMenu.CreateAsteroidStorage(asteroidInfo.Asteroid);
            if (myStorageBase == null)
              return;
          }
          storageName = MyGuiScreenDebugSpawnMenu.MakeStorageName(asteroidInfo.Asteroid + "-" + (object) asteroidInfo.RandomSeed);
        }
        else
        {
          using (MyRandom.Instance.PushSeed(asteroidInfo.RandomSeed))
          {
            storageName = MyGuiScreenDebugSpawnMenu.MakeStorageName("ProcAsteroid-" + (object) asteroidInfo.RandomSeed + "r" + (object) asteroidInfo.ProceduralRadius);
            myStorageBase = MyGuiScreenDebugSpawnMenu.CreateProceduralAsteroidStorage(asteroidInfo.RandomSeed, asteroidInfo.ProceduralRadius);
          }
        }
        MyVoxelMap myVoxelMap = new MyVoxelMap();
        myVoxelMap.CreatedByUser = true;
        myVoxelMap.Save = true;
        myVoxelMap.AsteroidName = asteroidInfo.Asteroid;
        myVoxelMap.Init(storageName, (IMyStorage) myStorageBase, asteroidInfo.WorldMatrix.Translation - myStorageBase.Size * 0.5f);
        myVoxelMap.WorldMatrix = asteroidInfo.WorldMatrix;
        Sandbox.Game.Entities.MyEntities.Add((MyEntity) myVoxelMap);
        Sandbox.Game.Entities.MyEntities.RaiseEntityCreated((MyEntity) myVoxelMap);
      }
      else
        ((MyMultiplayerServerBase) MyMultiplayer.Static).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
    }

    private void CreatePlanetsSpawnMenu(float separatorSize, float usableWidth)
    {
      this.m_currentPosition.Y += 0.025f;
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
      myGuiControlLabel1.Position = new Vector2(this.m_currentPosition.X + 1f / 1000f, this.m_currentPosition.Y);
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel1.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugSpawnMenu_Asteroid);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel1);
      this.m_currentPosition.Y += 0.03f;
      this.m_planetListbox = this.AddListBox(0.5f);
      this.m_planetListbox.MultiSelect = false;
      this.m_planetListbox.VisibleRowsCount = 14;
      foreach (MyPlanetGeneratorDefinition planetType in (IEnumerable<MyPlanetGeneratorDefinition>) MyDefinitionManager.Static.GetPlanetsGeneratorsDefinitions().OrderBy<MyPlanetGeneratorDefinition, string>((Func<MyPlanetGeneratorDefinition, string>) (e => e.Id.SubtypeId.ToString())))
      {
        string str = planetType.Id.SubtypeId.ToString();
        Vector4? nullable = new Vector4?();
        string toolTip = str;
        string errorMessage;
        if (!MyPlanets.Static.CanSpawnPlanet(planetType, false, out errorMessage))
        {
          toolTip = errorMessage;
          nullable = new Vector4?(MyGuiConstants.DISABLED_CONTROL_COLOR_MASK_MULTIPLIER);
        }
        MyGuiControlListbox planetListbox = this.m_planetListbox;
        MyGuiControlListbox.Item obj = new MyGuiControlListbox.Item(new StringBuilder(str), toolTip, userData: ((object) str));
        obj.ColorMask = nullable;
        int? position = new int?();
        planetListbox.Add(obj, position);
      }
      this.m_planetListbox.ItemsSelected += new Action<MyGuiControlListbox>(this.OnPlanetListbox_ItemSelected);
      this.m_planetListbox.ItemDoubleClicked += new Action<MyGuiControlListbox>(this.OnCreatePlanetClicked);
      if (this.m_planetListbox.Items.Count > 0)
        this.m_planetListbox.SelectedItems.Add(this.m_planetListbox.Items[0]);
      this.OnPlanetListbox_ItemSelected(this.m_planetListbox);
      MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel();
      myGuiControlLabel2.Position = new Vector2(this.m_currentPosition.X + 1f / 1000f, this.m_currentPosition.Y);
      myGuiControlLabel2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel2.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugSpawnMenu_ProceduralSize);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      this.m_currentPosition.Y += 0.03f;
      this.m_planetSizeSlider = new MyGuiControlSlider(new Vector2?(this.m_currentPosition), MyFakes.ENABLE_EXTENDED_PLANET_OPTIONS ? 100f : 19000f, 120000f, 0.285f, labelText: string.Empty, labelFont: "Debug", originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, intValue: true);
      this.m_planetSizeSlider.DebugScale = this.m_sliderDebugScale;
      this.m_planetSizeSlider.ColorMask = Color.White.ToVector4();
      this.Controls.Add((MyGuiControlBase) this.m_planetSizeSlider);
      MyGuiControlLabel label = new MyGuiControlLabel(new Vector2?(this.m_currentPosition + new Vector2(this.m_planetSizeSlider.Size.X - 3f / 1000f, this.m_planetSizeSlider.Size.Y - 0.065f)), text: string.Empty, colorMask: new Vector4?(Color.White.ToVector4()), font: "Debug");
      label.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      this.Controls.Add((MyGuiControlBase) label);
      this.m_currentPosition.Y += this.m_planetSizeSlider.Size.Y;
      this.m_currentPosition.Y += separatorSize;
      this.m_planetSizeSlider.ValueChanged += (Action<MyGuiControlSlider>) (s =>
      {
        StringBuilder output = new StringBuilder();
        MyValueFormatter.AppendDistanceInBestUnit(s.Value, output);
        label.Text = output.ToString();
        MyGuiScreenDebugSpawnMenu.m_procAsteroidSizeValue = s.Value;
      });
      this.m_planetSizeSlider.Value = 8000f;
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel();
      myGuiControlLabel3.Position = new Vector2(this.m_currentPosition.X + 1f / 1000f, this.m_currentPosition.Y);
      myGuiControlLabel3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel3.Text = MyTexts.GetString(MySpaceTexts.ScreenDebugSpawnMenu_ProceduralSeed);
      myGuiControlLabel3.IsAutoEllipsisEnabled = true;
      myGuiControlLabel3.IsAutoScaleEnabled = true;
      MyGuiControlLabel myGuiControlLabel4 = myGuiControlLabel3;
      myGuiControlLabel4.SetMaxWidth(this.m_planetListbox.Size.X);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel4);
      this.m_currentPosition.Y += 0.03f;
      Vector2? position1 = new Vector2?(this.m_currentPosition);
      string asteroidSeedValue = MyGuiScreenDebugSpawnMenu.m_procAsteroidSeedValue;
      Color color = Color.White;
      Vector4? textColor = new Vector4?(color.ToVector4());
      double scale = (double) this.m_scale;
      this.m_procPlanetSeedValue = new MyGuiControlTextbox(position1, asteroidSeedValue, 20, textColor, (float) scale);
      this.m_procPlanetSeedValue.TextChanged += (Action<MyGuiControlTextbox>) (t => MyGuiScreenDebugSpawnMenu.m_procAsteroidSeedValue = t.Text);
      this.m_procPlanetSeedValue.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_procPlanetSeedValue.Size = new Vector2(0.285f, 1f);
      this.Controls.Add((MyGuiControlBase) this.m_procPlanetSeedValue);
      this.m_currentPosition.Y += 0.043f;
      MyGuiControlButton debugButton1 = this.CreateDebugButton(0.285f, MySpaceTexts.ScreenDebugSpawnMenu_GenerateSeed, (Action<MyGuiControlButton>) (buttonClicked => this.m_procPlanetSeedValue.Text = MyRandom.Instance.Next().ToString()));
      debugButton1.PositionX += 1f / 500f;
      MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
      bool flag = localHumanPlayer != null && MySession.Static.IsUserSpaceMaster(localHumanPlayer.Id.SteamId);
      if (!flag)
      {
        Vector2? position2 = new Vector2?(this.m_currentPosition + new Vector2(1f / 500f, 0.05f));
        Vector2? size = new Vector2?();
        string text = MyTexts.GetString(MyCommonTexts.Warning_SpacemasterOrHigherRequired);
        color = Color.Red;
        Vector4? colorMask = new Vector4?(color.ToVector4());
        double num = 0.800000011920929 * (double) this.m_scale;
        this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(position2, size, text, colorMask, (float) num, "Debug"));
      }
      this.m_currentPosition.Y -= 0.01f;
      MyGuiControlButton debugButton2 = this.CreateDebugButton(0.285f, MySpaceTexts.ScreenDebugSpawnMenu_SpawnAsteroid, new Action<MyGuiControlButton>(this.OnCreatePlanetClicked));
      debugButton2.PositionX += 1f / 500f;
      this.m_planetSizeSlider.Enabled = flag;
      this.m_procPlanetSeedValue.Enabled = flag;
      debugButton1.Enabled = flag;
      this.m_planetListbox.Enabled = flag;
      debugButton2.Enabled = flag;
    }

    private void OnCreatePlanetClicked(object _)
    {
      this.CreatePlanet(this.GetProceduralAsteroidSeed(this.m_procPlanetSeedValue), this.m_planetSizeSlider.Value);
      this.CloseScreenNow();
    }

    private void CreatePlanet(int seed, float size)
    {
      Vector3D vector3D = MySector.MainCamera.Position + MySector.MainCamera.ForwardVector * size * 3f - new Vector3D((double) size);
      MyPlanetGeneratorDefinition definition = MyDefinitionManager.Static.GetDefinition<MyPlanetGeneratorDefinition>(MyStringHash.GetOrCompute(MyGuiScreenDebugSpawnMenu.m_selectedPlanetName));
      string errorMessage;
      if (!MyPlanets.Static.CanSpawnPlanet(definition, false, out errorMessage))
      {
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
        MyScreenManager.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder(errorMessage), messageCaption: messageCaption, canHideOthers: false));
      }
      else
      {
        MyPlanetStorageProvider planetStorageProvider = new MyPlanetStorageProvider();
        planetStorageProvider.Init((long) seed, definition, (double) size / 2.0, false);
        IMyStorage storage = (IMyStorage) new MyOctreeStorage((IMyStorageDataProvider) planetStorageProvider, planetStorageProvider.StorageSize);
        float num1 = planetStorageProvider.Radius * definition.HillParams.Min;
        float num2 = planetStorageProvider.Radius * definition.HillParams.Max;
        double radius = (double) planetStorageProvider.Radius;
        float num3 = (float) radius + num2;
        float num4 = (float) radius + num1;
        float num5 = (!definition.AtmosphereSettings.HasValue || (double) definition.AtmosphereSettings.Value.Scale <= 1.0 ? 1.75f : 1f + definition.AtmosphereSettings.Value.Scale) * planetStorageProvider.Radius;
        MyPlanet myPlanet = new MyPlanet();
        myPlanet.EntityId = MyRandom.Instance.NextLong();
        MyPlanetInitArguments arguments;
        arguments.StorageName = "test";
        arguments.Seed = seed;
        arguments.Storage = storage;
        arguments.PositionMinCorner = vector3D;
        arguments.Radius = planetStorageProvider.Radius;
        arguments.AtmosphereRadius = num5;
        arguments.MaxRadius = num3;
        arguments.MinRadius = num4;
        arguments.HasAtmosphere = definition.HasAtmosphere;
        arguments.AtmosphereWavelengths = Vector3.Zero;
        arguments.GravityFalloff = definition.GravityFalloffPower;
        arguments.MarkAreaEmpty = true;
        arguments.AtmosphereSettings = definition.AtmosphereSettings ?? MyAtmosphereSettings.Defaults();
        arguments.SurfaceGravity = definition.SurfaceGravity;
        arguments.AddGps = false;
        arguments.SpherizeWithDistance = true;
        arguments.Generator = definition;
        arguments.UserCreated = true;
        arguments.InitializeComponents = true;
        arguments.FadeIn = false;
        myPlanet.Init(arguments);
        MyGuiScreenDebugSpawnMenu.m_lastAsteroidInfo = new MyGuiScreenDebugSpawnMenu.SpawnAsteroidInfo()
        {
          Asteroid = (string) null,
          RandomSeed = seed,
          WorldMatrix = MatrixD.Identity,
          IsProcedural = true,
          ProceduralRadius = size
        };
        MyClipboardComponent.Static.ActivateVoxelClipboard(myPlanet.GetObjectBuilder(false), storage, MySector.MainCamera.ForwardVector, (storage.Size * 0.5f).Length());
        myPlanet.Close();
      }
    }

    public static void SpawnPlanet(Vector3D pos) => MyMultiplayer.RaiseStaticEvent<string, float, int, Vector3D>((Func<IMyEventOwner, Action<string, float, int, Vector3D>>) (s => new Action<string, float, int, Vector3D>(MyGuiScreenDebugSpawnMenu.SpawnPlanet_Server)), MyGuiScreenDebugSpawnMenu.m_selectedPlanetName, MyGuiScreenDebugSpawnMenu.m_lastAsteroidInfo.ProceduralRadius, MyGuiScreenDebugSpawnMenu.m_lastAsteroidInfo.RandomSeed, pos);

    [Event(null, 1425)]
    [Reliable]
    [Server]
    private static void SpawnPlanet_Server(string planetName, float size, int seed, Vector3D pos)
    {
      if (MyEventContext.Current.IsLocallyInvoked || MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value) && (MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value) || MySession.Static.CreativeMode))
      {
        string str = planetName + "-" + (object) seed + "d" + (object) size;
        MyGuiScreenDebugSpawnMenu.MakeStorageName(str);
        long entityId = MyRandom.Instance.NextLong();
        if (MyWorldGenerator.AddPlanet(str, planetName, planetName, pos, seed, size, true, entityId, userCreated: true) == null)
          return;
        if (MySession.Static.RequiresDX < 11)
          MySession.Static.RequiresDX = 11;
        MyMultiplayer.RaiseStaticEvent<string, string, float, int, Vector3D, long>((Func<IMyEventOwner, Action<string, string, float, int, Vector3D, long>>) (s => new Action<string, string, float, int, Vector3D, long>(MyGuiScreenDebugSpawnMenu.SpawnPlanet_Client)), planetName, str, size, seed, pos, entityId);
      }
      else
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
    }

    [Event(null, 1453)]
    [Reliable]
    [Broadcast]
    private static void SpawnPlanet_Client(
      string planetName,
      string storageNameBase,
      float size,
      int seed,
      Vector3D pos,
      long entityId)
    {
      MyWorldGenerator.AddPlanet(storageNameBase, planetName, planetName, pos, seed, size, true, entityId, userCreated: true);
      if (MySession.Static.RequiresDX >= 11)
        return;
      MySession.Static.RequiresDX = 11;
    }

    private void CreateEmptyVoxelMapSpawnMenu(float separatorSize, float usableWidth)
    {
      this.m_currentPosition.Y += 0.025f;
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel();
      myGuiControlLabel.Position = new Vector2(this.m_currentPosition.X + 1f / 1000f, this.m_currentPosition.Y);
      myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel.Text = "Voxel Size: ";
      MyGuiControlLabel label = myGuiControlLabel;
      this.Controls.Add((MyGuiControlBase) label);
      this.m_currentPosition.Y += 0.03f;
      MyGuiControlSlider guiControlSlider = new MyGuiControlSlider(new Vector2?(this.m_currentPosition), 2f, 10f, 0.285f, labelText: string.Empty, labelFont: "Debug", originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, intValue: true);
      guiControlSlider.DebugScale = this.m_sliderDebugScale;
      guiControlSlider.ColorMask = Color.White.ToVector4();
      this.Controls.Add((MyGuiControlBase) guiControlSlider);
      label = new MyGuiControlLabel(new Vector2?(this.m_currentPosition + new Vector2(guiControlSlider.Size.X - 3f / 1000f, guiControlSlider.Size.Y - 0.065f)), text: string.Empty, colorMask: new Vector4?(Color.White.ToVector4()), font: "Debug");
      label.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      this.Controls.Add((MyGuiControlBase) label);
      guiControlSlider.ValueChanged += (Action<MyGuiControlSlider>) (s =>
      {
        int num = 1 << (int) s.Value;
        label.Text = num.ToString() + "m";
        MyGuiScreenDebugSpawnMenu.m_procAsteroidSizeValue = (float) num;
      });
      guiControlSlider.Value = 5f;
      this.m_currentPosition.Y += guiControlSlider.Size.Y;
      this.m_currentPosition.Y += separatorSize;
      this.m_currentPosition.Y -= 0.01f;
      this.CreateDebugButton(0.284f, MySpaceTexts.ScreenDebugSpawnMenu_SpawnAsteroid, (Action<MyGuiControlButton>) (x =>
      {
        int asteroidSizeValue = (int) MyGuiScreenDebugSpawnMenu.m_procAsteroidSizeValue;
        MyStorageBase myStorageBase = (MyStorageBase) new MyOctreeStorage((IMyStorageDataProvider) null, new Vector3I(asteroidSizeValue));
        MyObjectBuilder_VoxelMap asteroidObjectBuilder = MyGuiScreenDebugSpawnMenu.CreateAsteroidObjectBuilder(MyGuiScreenDebugSpawnMenu.MakeStorageName("MyEmptyVoxelMap"));
        MyGuiScreenDebugSpawnMenu.m_lastAsteroidInfo.Asteroid = (string) null;
        MyGuiScreenDebugSpawnMenu.m_lastAsteroidInfo.ProceduralRadius = (float) asteroidSizeValue;
        MyClipboardComponent.Static.ActivateVoxelClipboard((MyObjectBuilder_EntityBase) asteroidObjectBuilder, (IMyStorage) myStorageBase, MySector.MainCamera.ForwardVector, (myStorageBase.Size * 0.5f).Length());
        this.CloseScreenNow();
      }));
    }

    [Serializable]
    public struct SpawnAsteroidInfo
    {
      [Serialize(MyObjectFlags.DefaultZero)]
      public string Asteroid;
      public int RandomSeed;
      public MatrixD WorldMatrix;
      public bool IsProcedural;
      public float ProceduralRadius;
      [Nullable]
      public string VoxelMaterial;

      protected class Sandbox_Game_Gui_MyGuiScreenDebugSpawnMenu\u003C\u003ESpawnAsteroidInfo\u003C\u003EAsteroid\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenDebugSpawnMenu.SpawnAsteroidInfo, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyGuiScreenDebugSpawnMenu.SpawnAsteroidInfo owner,
          in string value)
        {
          owner.Asteroid = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyGuiScreenDebugSpawnMenu.SpawnAsteroidInfo owner,
          out string value)
        {
          value = owner.Asteroid;
        }
      }

      protected class Sandbox_Game_Gui_MyGuiScreenDebugSpawnMenu\u003C\u003ESpawnAsteroidInfo\u003C\u003ERandomSeed\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenDebugSpawnMenu.SpawnAsteroidInfo, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyGuiScreenDebugSpawnMenu.SpawnAsteroidInfo owner,
          in int value)
        {
          owner.RandomSeed = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyGuiScreenDebugSpawnMenu.SpawnAsteroidInfo owner,
          out int value)
        {
          value = owner.RandomSeed;
        }
      }

      protected class Sandbox_Game_Gui_MyGuiScreenDebugSpawnMenu\u003C\u003ESpawnAsteroidInfo\u003C\u003EWorldMatrix\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenDebugSpawnMenu.SpawnAsteroidInfo, MatrixD>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyGuiScreenDebugSpawnMenu.SpawnAsteroidInfo owner,
          in MatrixD value)
        {
          owner.WorldMatrix = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyGuiScreenDebugSpawnMenu.SpawnAsteroidInfo owner,
          out MatrixD value)
        {
          value = owner.WorldMatrix;
        }
      }

      protected class Sandbox_Game_Gui_MyGuiScreenDebugSpawnMenu\u003C\u003ESpawnAsteroidInfo\u003C\u003EIsProcedural\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenDebugSpawnMenu.SpawnAsteroidInfo, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyGuiScreenDebugSpawnMenu.SpawnAsteroidInfo owner,
          in bool value)
        {
          owner.IsProcedural = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyGuiScreenDebugSpawnMenu.SpawnAsteroidInfo owner,
          out bool value)
        {
          value = owner.IsProcedural;
        }
      }

      protected class Sandbox_Game_Gui_MyGuiScreenDebugSpawnMenu\u003C\u003ESpawnAsteroidInfo\u003C\u003EProceduralRadius\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenDebugSpawnMenu.SpawnAsteroidInfo, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyGuiScreenDebugSpawnMenu.SpawnAsteroidInfo owner,
          in float value)
        {
          owner.ProceduralRadius = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyGuiScreenDebugSpawnMenu.SpawnAsteroidInfo owner,
          out float value)
        {
          value = owner.ProceduralRadius;
        }
      }

      protected class Sandbox_Game_Gui_MyGuiScreenDebugSpawnMenu\u003C\u003ESpawnAsteroidInfo\u003C\u003EVoxelMaterial\u003C\u003EAccessor : IMemberAccessor<MyGuiScreenDebugSpawnMenu.SpawnAsteroidInfo, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyGuiScreenDebugSpawnMenu.SpawnAsteroidInfo owner,
          in string value)
        {
          owner.VoxelMaterial = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyGuiScreenDebugSpawnMenu.SpawnAsteroidInfo owner,
          out string value)
        {
          value = owner.VoxelMaterial;
        }
      }
    }

    private delegate void CreateScreen(float space, float width);

    private struct Screen
    {
      public string Name;
      public MyGuiScreenDebugSpawnMenu.CreateScreen Creator;
    }

    protected sealed class SpawnIntoContainer_Implementation\u003C\u003ESystem_Int64\u0023VRage_ObjectBuilders_SerializableDefinitionId\u0023System_Int64\u0023System_Int64 : ICallSite<IMyEventOwner, long, SerializableDefinitionId, long, long, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long amount,
        in SerializableDefinitionId item,
        in long entityId,
        in long playerId,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenDebugSpawnMenu.SpawnIntoContainer_Implementation(amount, item, entityId, playerId);
      }
    }

    protected sealed class SpawnAsteroid\u003C\u003ESandbox_Game_Gui_MyGuiScreenDebugSpawnMenu\u003C\u003ESpawnAsteroidInfo : ICallSite<IMyEventOwner, MyGuiScreenDebugSpawnMenu.SpawnAsteroidInfo, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyGuiScreenDebugSpawnMenu.SpawnAsteroidInfo asteroidInfo,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenDebugSpawnMenu.SpawnAsteroid(asteroidInfo);
      }
    }

    protected sealed class SpawnPlanet_Server\u003C\u003ESystem_String\u0023System_Single\u0023System_Int32\u0023VRageMath_Vector3D : ICallSite<IMyEventOwner, string, float, int, Vector3D, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string planetName,
        in float size,
        in int seed,
        in Vector3D pos,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenDebugSpawnMenu.SpawnPlanet_Server(planetName, size, seed, pos);
      }
    }

    protected sealed class SpawnPlanet_Client\u003C\u003ESystem_String\u0023System_String\u0023System_Single\u0023System_Int32\u0023VRageMath_Vector3D\u0023System_Int64 : ICallSite<IMyEventOwner, string, string, float, int, Vector3D, long>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string planetName,
        in string storageNameBase,
        in float size,
        in int seed,
        in Vector3D pos,
        in long entityId)
      {
        MyGuiScreenDebugSpawnMenu.SpawnPlanet_Client(planetName, storageNameBase, size, seed, pos, entityId);
      }
    }
  }
}
