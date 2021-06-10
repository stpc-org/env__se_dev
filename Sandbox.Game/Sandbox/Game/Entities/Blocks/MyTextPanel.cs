// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.MyTextPanel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Definitions;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_TextPanel))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyTextPanel), typeof (Sandbox.ModAPI.Ingame.IMyTextPanel)})]
  public class MyTextPanel : MyFunctionalBlock, IMyTextPanelComponentOwner, IMyTextPanelProvider, Sandbox.ModAPI.IMyTextPanel, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.IMyTextSurface, Sandbox.ModAPI.Ingame.IMyTextSurface, Sandbox.ModAPI.Ingame.IMyTextPanel, Sandbox.ModAPI.IMyTextSurfaceProvider, Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider
  {
    public const double MAX_DRAW_DISTANCE = 200.0;
    private readonly StringBuilder m_publicDescription = new StringBuilder();
    private readonly StringBuilder m_publicTitle = new StringBuilder();
    private readonly StringBuilder m_privateDescription = new StringBuilder();
    private readonly StringBuilder m_privateTitle = new StringBuilder();
    private bool m_isTextPanelOpen;
    private ulong m_userId;
    private MyGuiScreenTextPanel m_textBox;
    protected MySessionComponentPanels m_panelsComponent;
    private List<MyTextPanelComponent> m_panelComponents = new List<MyTextPanelComponent>();
    private int m_selectedRotationIndex;
    private int m_newSelectedRotationIndex;
    private int m_previousUpdateTime;
    private bool m_isOutofRange;
    protected MyTextPanelComponent m_activePanelComponent;
    private bool m_isEditingPublic;
    private float m_maxRenderDistanceSquared;
    private StringBuilder m_publicTitleHelper = new StringBuilder();
    private StringBuilder m_privateTitleHelper = new StringBuilder();
    private StringBuilder m_publicDescriptionHelper = new StringBuilder();
    private StringBuilder m_privateDescriptionHelper = new StringBuilder();
    private bool m_descriptionPrivateDirty;
    private bool m_descriptionPublicDirty;

    public MyTextPanel()
    {
      this.CreateTerminalControls();
      this.m_isTextPanelOpen = false;
      this.m_privateDescription = new StringBuilder();
      this.m_privateTitle = new StringBuilder();
      this.Render = (MyRenderComponentScreenAreas) new MyRenderComponentTextPanel(this);
      this.Render.NeedsDraw = false;
      this.NeedsWorldMatrix = true;
    }

    public int PanelTexturesByteCount => this.m_activePanelComponent.TextureByteCount;

    public Vector3D WorldPosition => this.PositionComp.WorldMatrixRef.Translation;

    public int RangeIndex { get; set; }

    public ContentType ContentType
    {
      get => this.PanelComponent.ContentType;
      set => this.PanelComponent.ContentType = value;
    }

    public ShowTextOnScreenFlag ShowTextFlag
    {
      get => this.PanelComponent.ShowTextFlag;
      set => this.PanelComponent.ShowTextFlag = value;
    }

    public bool ShowTextOnScreen => this.PanelComponent.ShowTextOnScreen;

    public MyTextPanelComponent PanelComponent => this.m_activePanelComponent;

    public StringBuilder PublicDescription
    {
      get => this.m_publicDescription;
      set
      {
        if (this.m_publicDescription.CompareUpdate(value))
        {
          this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
          this.m_activePanelComponent.Text = this.m_publicDescription;
        }
        if (this.m_publicDescriptionHelper == value)
          return;
        this.m_publicDescriptionHelper.Clear().Append((object) value);
      }
    }

    public StringBuilder PublicTitle
    {
      get => this.m_publicTitle;
      set
      {
        this.m_publicTitle.CompareUpdate(value);
        if (this.m_publicTitleHelper == value)
          return;
        this.m_publicTitleHelper.Clear().Append((object) value);
      }
    }

    public StringBuilder PrivateTitle
    {
      get => this.m_privateTitle;
      set
      {
        this.m_privateTitle.CompareUpdate(value);
        if (this.m_privateTitleHelper == value)
          return;
        this.m_privateTitleHelper.Clear().Append((object) value);
      }
    }

    public StringBuilder PrivateDescription
    {
      get => this.m_privateDescription;
      set
      {
        if (this.m_privateDescription.CompareUpdate(value))
          this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
        if (this.m_privateDescriptionHelper == value)
          return;
        this.m_privateDescriptionHelper.Clear().Append((object) value);
      }
    }

    public bool IsTextPanelOpen
    {
      get => this.m_isTextPanelOpen;
      set
      {
        if (this.m_isTextPanelOpen == value)
          return;
        this.m_isTextPanelOpen = value;
        this.RaisePropertiesChanged();
      }
    }

    public ulong UserId
    {
      get => this.m_userId;
      set => this.m_userId = value;
    }

    public Vector2 SurfaceSize => this.m_activePanelComponent.SurfaceSize;

    public Vector2 TextureSize => this.m_activePanelComponent.TextureSize;

    internal MyRenderComponentScreenAreas Render
    {
      get => base.Render as MyRenderComponentScreenAreas;
      set => this.Render = (MyRenderComponentBase) value;
    }

    public MyTextPanelDefinition BlockDefinition => (MyTextPanelDefinition) base.BlockDefinition;

    public int SelectedRotationIndex
    {
      get => this.m_selectedRotationIndex;
      set => this.SetSelectedRotationIndex(value);
    }

    public override void UpdateAfterSimulation10()
    {
      base.UpdateAfterSimulation10();
      if (this.IsFunctional)
        this.m_activePanelComponent.UpdateAfterSimulation(this.IsWorking, this.IsInRange());
      this.m_activePanelComponent?.UpdateModApiText();
      this.UpdateModApiText();
    }

    public override void UpdateAfterSimulation100()
    {
      base.UpdateAfterSimulation100();
      if (this.IsBeingHacked)
      {
        this.m_descriptionPrivateDirty = false;
        this.PrivateDescription.Clear();
        this.SendChangeDescriptionMessage(this.PrivateDescription, false);
      }
      this.ResourceSink.Update();
    }

    private void PowerReceiver_IsPoweredChanged()
    {
      this.SetDetailedInfoDirty();
      this.UpdateIsWorking();
      if (this.Render == null)
        return;
      this.UpdateScreen();
    }

    protected override bool CheckIsWorking() => base.CheckIsWorking() && this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId);

    private void ComponentStack_IsFunctionalChanged()
    {
      this.ResourceSink.Update();
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    protected override void OnStartWorking() => this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;

    protected override void OnStopWorking() => this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      this.ComponentStack_IsFunctionalChanged();
      this.PanelComponent.Reset();
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      MyCubeGridRenderCell orAddCell = this.CubeGrid.RenderData.GetOrAddCell(this.Position * this.CubeGrid.GridSize);
      if (orAddCell.ParentCullObject != uint.MaxValue)
        this.Render.SetParent(0, orAddCell.ParentCullObject, new Matrix?(this.PositionComp.LocalMatrixRef));
      this.PanelComponent.SetRender(this.Render);
      if (this.m_newSelectedRotationIndex != this.m_selectedRotationIndex)
        this.SetSelectedRotationIndex(this.m_newSelectedRotationIndex);
      this.HideInactivePanelComponents();
      this.UpdateScreen();
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyTextPanel>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlTextbox<MyTextPanel> terminalControlTextbox = new MyTerminalControlTextbox<MyTextPanel>("Title", MySpaceTexts.BlockPropertyTitle_TextPanelPublicTitle, MySpaceTexts.Blank);
      terminalControlTextbox.Getter = (MyTerminalControlTextbox<MyTextPanel>.GetterDelegate) (x => x.PublicTitle);
      terminalControlTextbox.Setter = (MyTerminalControlTextbox<MyTextPanel>.SetterDelegate) ((x, v) => x.SendChangeTitleMessage(v, true));
      terminalControlTextbox.SupportsMultipleBlocks = false;
      MyTerminalControlFactory.AddControl<MyTextPanel>((MyTerminalControl<MyTextPanel>) terminalControlTextbox);
      MyTerminalControlSlider<MyTextPanel> slider = new MyTerminalControlSlider<MyTextPanel>("Rotate", MyCommonTexts.ScriptingTools_Rotation, MyCommonTexts.ScriptingTools_Rotation);
      slider.SetLimits((MyTerminalValueControl<MyTextPanel, float>.GetterDelegate) (block => 0.0f), (MyTerminalValueControl<MyTextPanel, float>.GetterDelegate) (block => 270f));
      slider.DefaultValue = new float?(0.0f);
      slider.Getter = (MyTerminalValueControl<MyTextPanel, float>.GetterDelegate) (x => (float) (x.m_selectedRotationIndex * 90));
      slider.Setter = (MyTerminalValueControl<MyTextPanel, float>.SetterDelegate) ((x, v) =>
      {
        int int32 = Convert.ToInt32(v / 90f);
        if (int32 == x.m_selectedRotationIndex)
          return;
        x.SendSelectRotationIndexRequest(int32);
      });
      slider.Writer = (MyTerminalControl<MyTextPanel>.WriterDelegate) ((x, result) => result.AppendInt32(x.m_selectedRotationIndex * 90).Append("°"));
      slider.EnableActions<MyTextPanel>(0.25f);
      slider.Visible = (Func<MyTextPanel, bool>) (x => x.m_panelComponents.Count == 4);
      MyTerminalControlFactory.AddControl<MyTextPanel>((MyTerminalControl<MyTextPanel>) slider);
      MyTerminalControlFactory.AddControl<MyTextPanel>((MyTerminalControl<MyTextPanel>) new MyTerminalControlSeparator<MyTextPanel>());
      MyTextPanelComponent.CreateTerminalControls<MyTextPanel>();
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.SyncFlag = true;
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(this.BlockDefinition.ResourceSinkGroup, this.BlockDefinition.RequiredPowerInput, (Func<float>) (() => !this.Enabled || !this.IsFunctional ? 0.0f : this.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId)));
      this.ResourceSink = resourceSinkComponent;
      this.m_panelsComponent = MySession.Static.GetComponent<MySessionComponentPanels>();
      this.OnClosing += (Action<MyEntity>) (x => this.m_panelsComponent.Remove((IMyTextPanelProvider) this));
      this.RangeIndex = -1;
      this.m_maxRenderDistanceSquared = this.BlockDefinition.MaxScreenRenderDistance * this.BlockDefinition.MaxScreenRenderDistance;
      MyObjectBuilder_TextPanel ob = (MyObjectBuilder_TextPanel) objectBuilder;
      if (this.BlockDefinition.ScreenAreas != null && this.BlockDefinition.ScreenAreas.Count == 4)
      {
        for (int index = 0; index < this.BlockDefinition.ScreenAreas.Count; ++index)
        {
          MyTextPanelComponent textPanelComponent = new MyTextPanelComponent(index, (MyTerminalBlock) this, this.BlockDefinition.ScreenAreas[index].Name, this.BlockDefinition.ScreenAreas[index].DisplayName, this.BlockDefinition.ScreenAreas[index].TextureResolution, this.BlockDefinition.ScreenAreas[index].ScreenWidth, this.BlockDefinition.ScreenAreas[index].ScreenHeight);
          this.SyncType.Append((object) textPanelComponent);
          textPanelComponent.Init(ob != null ? ob.Sprites : new MySerializableSpriteCollection(), addImagesRequest: new Action<MyTextPanelComponent, int[]>(this.SendAddImagesToSelectionRequest), removeImagesRequest: new Action<MyTextPanelComponent, int[]>(this.SendRemoveSelectedImageRequest), changeTextRequest: new Action<MyTextPanelComponent, string>(this.ChangeTextRequest), spriteCollectionUpdate: new Action<MyTextPanelComponent, MySerializableSpriteCollection>(this.UpdateSpriteCollection));
          this.m_panelComponents.Add(textPanelComponent);
        }
        this.m_activePanelComponent = this.m_panelComponents[0];
        this.SyncType.Append((object) this.m_panelComponents);
        if (ob.SelectedRotationIndex.HasValue && ob.SelectedRotationIndex.Value > 0 && ob.SelectedRotationIndex.Value < this.m_panelComponents.Count)
        {
          this.m_selectedRotationIndex = this.m_newSelectedRotationIndex = ob.SelectedRotationIndex.Value;
          this.m_activePanelComponent = this.m_panelComponents[this.m_selectedRotationIndex];
        }
      }
      else
      {
        this.m_activePanelComponent = new MyTextPanelComponent(0, (MyTerminalBlock) this, this.BlockDefinition.PanelMaterialName, this.BlockDefinition.PanelMaterialName, this.BlockDefinition.TextureResolution, this.BlockDefinition.ScreenWidth, this.BlockDefinition.ScreenHeight);
        this.SyncType.Append((object) this.m_activePanelComponent);
        this.m_activePanelComponent.Init(ob != null ? ob.Sprites : new MySerializableSpriteCollection(), addImagesRequest: new Action<MyTextPanelComponent, int[]>(this.SendAddImagesToSelectionRequest), removeImagesRequest: new Action<MyTextPanelComponent, int[]>(this.SendRemoveSelectedImageRequest), changeTextRequest: new Action<MyTextPanelComponent, string>(this.ChangeTextRequest), spriteCollectionUpdate: new Action<MyTextPanelComponent, MySerializableSpriteCollection>(this.UpdateSpriteCollection));
        this.m_panelComponents.Add(this.m_activePanelComponent);
      }
      if (ob != null)
        this.InitTextPanelComponent(this.m_activePanelComponent, ob);
      base.Init(objectBuilder, cubeGrid);
      this.ResourceSink.Update();
      this.ResourceSink.IsPoweredChanged += new Action(this.PowerReceiver_IsPoweredChanged);
      this.ResourceSink.RequiredInputChanged += new MyRequiredResourceChangeDelegate(this.PowerReceiver_RequiredInputChanged);
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
    }

    protected void InitTextPanelComponent(
      MyTextPanelComponent component,
      MyObjectBuilder_TextPanel ob)
    {
      if (ob == null)
        return;
      this.PrivateTitle.Append(ob.Title);
      this.PrivateDescription.Append(ob.Description);
      this.PublicDescription.Append(MyStatControlText.SubstituteTexts(ob.PublicDescription));
      this.PublicTitle.Append(ob.PublicTitle);
      if (Sync.IsServer && Sync.Clients != null)
        Sync.Clients.ClientRemoved += new Action<ulong>(this.TextPanel_ClientRemoved);
      component.CurrentSelectedTexture = ob.CurrentShownTexture;
      MyTextPanelComponent.ContentMetadata content = new MyTextPanelComponent.ContentMetadata()
      {
        ContentType = ob.ContentType,
        BackgroundColor = ob.BackgroundColor,
        ChangeInterval = MathHelper.Clamp(ob.ChangeInterval, 0.0f, this.BlockDefinition.MaxChangingSpeed),
        PreserveAspectRatio = ob.PreserveAspectRatio,
        TextPadding = ob.TextPadding
      };
      MyTextPanelComponent.FontData font = new MyTextPanelComponent.FontData()
      {
        Alignment = (TextAlignment) ob.Alignment,
        Size = MathHelper.Clamp(ob.FontSize, this.BlockDefinition.MinFontSize, this.BlockDefinition.MaxFontSize),
        TextColor = ob.FontColor
      };
      MyTextPanelComponent.ScriptData script = new MyTextPanelComponent.ScriptData()
      {
        Script = ob.SelectedScript ?? string.Empty,
        CustomizeScript = ob.CustomizeScripts,
        BackgroundColor = ob.ScriptBackgroundColor,
        ForegroundColor = ob.ScriptForegroundColor
      };
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
      this.Render.NeedsDrawFromParent = true;
      if (!ob.Font.IsNull())
        font.Name = ob.Font.SubtypeName;
      if (ob.SelectedImages != null)
      {
        foreach (string selectedImage in ob.SelectedImages)
        {
          foreach (MyLCDTextureDefinition definition in component.Definitions)
          {
            if (definition.Id.SubtypeName == selectedImage)
            {
              component.SelectedTexturesToDraw.Add(definition);
              break;
            }
          }
        }
        component.CurrentSelectedTexture = Math.Min(component.CurrentSelectedTexture, component.SelectedTexturesToDraw.Count);
        this.RaisePropertiesChanged();
      }
      if (ob.Version == (byte) 0)
      {
        if (ob.ContentType == ContentType.NONE && (ob.SelectedImages != null && ob.SelectedImages.Count > 0 || (ob.ShowText != ShowTextOnScreenFlag.NONE || ob.PublicDescription != string.Empty)))
        {
          if (ob.ShowText != ShowTextOnScreenFlag.NONE)
            component.SelectedTexturesToDraw.Clear();
          else
            this.PublicDescription.Clear();
          content.ContentType = ContentType.TEXT_AND_IMAGE;
        }
        else
          content.ContentType = ob.ContentType != ContentType.IMAGE ? ob.ContentType : ContentType.TEXT_AND_IMAGE;
      }
      component.SetLocalValues(content, font, script);
      component.Text = this.PublicDescription;
    }

    private void PowerReceiver_RequiredInputChanged(
      MyDefinitionId resourceTypeId,
      MyResourceSinkComponent receiver,
      float oldRequirement,
      float newRequirement)
    {
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_TextPanel builderCubeBlock = (MyObjectBuilder_TextPanel) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.Description = this.m_privateDescription.ToString();
      builderCubeBlock.Title = this.m_privateTitle.ToString();
      builderCubeBlock.PublicDescription = this.m_publicDescription.ToString();
      builderCubeBlock.PublicTitle = this.m_publicTitle.ToString();
      builderCubeBlock.ChangeInterval = this.ChangeInterval;
      builderCubeBlock.Font = (SerializableDefinitionId) this.PanelComponent.Font;
      builderCubeBlock.FontSize = this.FontSize;
      builderCubeBlock.FontColor = this.FontColor;
      builderCubeBlock.BackgroundColor = this.BackgroundColor;
      builderCubeBlock.CurrentShownTexture = this.PanelComponent.CurrentSelectedTexture;
      builderCubeBlock.ShowText = ShowTextOnScreenFlag.NONE;
      builderCubeBlock.Alignment = (TextAlignmentEnum) this.PanelComponent.Alignment;
      builderCubeBlock.ContentType = this.PanelComponent.ContentType == ContentType.IMAGE ? ContentType.TEXT_AND_IMAGE : this.PanelComponent.ContentType;
      builderCubeBlock.SelectedScript = this.PanelComponent.Script;
      builderCubeBlock.CustomizeScripts = this.PanelComponent.CustomizeScripts;
      builderCubeBlock.ScriptBackgroundColor = this.PanelComponent.ScriptBackgroundColor;
      builderCubeBlock.ScriptForegroundColor = this.PanelComponent.ScriptForegroundColor;
      builderCubeBlock.TextPadding = this.PanelComponent.TextPadding;
      builderCubeBlock.PreserveAspectRatio = this.PanelComponent.PreserveAspectRatio;
      builderCubeBlock.Version = (byte) 1;
      if (this.PanelComponent.SelectedTexturesToDraw.Count > 0)
      {
        builderCubeBlock.SelectedImages = new List<string>();
        foreach (MyLCDTextureDefinition textureDefinition in this.PanelComponent.SelectedTexturesToDraw)
          builderCubeBlock.SelectedImages.Add(textureDefinition.Id.SubtypeName);
      }
      builderCubeBlock.Sprites = this.PanelComponent.ExternalSprites;
      builderCubeBlock.SelectedRotationIndex = new int?(this.m_selectedRotationIndex);
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public void Use(UseActionEnum actionEnum, VRage.ModAPI.IMyEntity entity)
    {
      if (this.m_isTextPanelOpen)
        return;
      MyCharacter user = entity as MyCharacter;
      MyRelationsBetweenPlayerAndBlock userRelationToOwner = this.GetUserRelationToOwner(user.ControllerInfo.Controller.Player.Identity.IdentityId);
      if (this.OwnerId == 0L)
      {
        this.OnOwnerUse(actionEnum, user);
      }
      else
      {
        switch (userRelationToOwner)
        {
          case MyRelationsBetweenPlayerAndBlock.NoOwnership:
          case MyRelationsBetweenPlayerAndBlock.FactionShare:
            if (this.OwnerId == 0L)
            {
              this.OnOwnerUse(actionEnum, user);
              break;
            }
            this.OnFactionUse(actionEnum, user);
            break;
          case MyRelationsBetweenPlayerAndBlock.Owner:
            this.OnOwnerUse(actionEnum, user);
            break;
          case MyRelationsBetweenPlayerAndBlock.Neutral:
          case MyRelationsBetweenPlayerAndBlock.Enemies:
          case MyRelationsBetweenPlayerAndBlock.Friends:
            if (MySession.Static.Factions.TryGetPlayerFaction(user.ControllerInfo.Controller.Player.Identity.IdentityId) == MySession.Static.Factions.TryGetPlayerFaction(this.IDModule.Owner) && actionEnum == UseActionEnum.Manipulate)
            {
              this.OnFactionUse(actionEnum, user);
              break;
            }
            this.OnEnemyUse(actionEnum, user);
            break;
        }
      }
    }

    private void OnEnemyUse(UseActionEnum actionEnum, MyCharacter user)
    {
      if (actionEnum == UseActionEnum.Manipulate)
      {
        this.OpenWindow(false, true, true);
      }
      else
      {
        if (actionEnum != UseActionEnum.OpenTerminal)
          return;
        MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
      }
    }

    private void OnFactionUse(UseActionEnum actionEnum, MyCharacter user)
    {
      bool flag = false;
      switch (actionEnum)
      {
        case UseActionEnum.Manipulate:
          if (this.GetUserRelationToOwner(user.GetPlayerIdentityId()) == MyRelationsBetweenPlayerAndBlock.FactionShare)
          {
            this.OpenWindow(true, true, true);
            break;
          }
          this.OpenWindow(false, true, true);
          break;
        case UseActionEnum.OpenTerminal:
          if (this.GetUserRelationToOwner(user.GetPlayerIdentityId()) == MyRelationsBetweenPlayerAndBlock.FactionShare)
          {
            MyGuiScreenTerminal.Show(MyTerminalPageEnum.ControlPanel, user, (MyEntity) this);
            break;
          }
          flag = true;
          break;
      }
      if (user.ControllerInfo.Controller.Player != MySession.Static.LocalHumanPlayer || !flag)
        return;
      MyHud.Notifications.Add(MyNotificationSingletons.TextPanelReadOnly);
    }

    private void OnOwnerUse(UseActionEnum actionEnum, MyCharacter user)
    {
      if (actionEnum == UseActionEnum.Manipulate)
      {
        this.OpenWindow(true, true, true);
      }
      else
      {
        if (actionEnum != UseActionEnum.OpenTerminal)
          return;
        MyGuiScreenTerminal.Show(MyTerminalPageEnum.ControlPanel, user, (MyEntity) this);
      }
    }

    public override void OnRemovedFromScene(object source)
    {
      base.OnRemovedFromScene(source);
      if (this.PanelComponent != null)
        this.PanelComponent.SetRender((MyRenderComponentScreenAreas) null);
      if (this.m_panelComponents == null)
        return;
      foreach (MyTextPanelComponent panelComponent in this.m_panelComponents)
        panelComponent?.SetRender((MyRenderComponentScreenAreas) null);
    }

    protected override void Closing()
    {
      base.Closing();
      if (!Sync.IsServer || Sync.Clients == null)
        return;
      Sync.Clients.ClientRemoved -= new Action<ulong>(this.TextPanel_ClientRemoved);
    }

    private void TextPanel_ClientRemoved(ulong playerId)
    {
      if ((long) playerId != (long) this.m_userId)
        return;
      this.SendChangeOpenMessage(false);
    }

    protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
    {
      base.UpdateDetailedInfo(detailedInfo);
      detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
      detailedInfo.Append(this.BlockDefinition.DisplayNameText);
      detailedInfo.Append("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxRequiredInput));
      MyValueFormatter.AppendWorkInBestUnit(this.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId), detailedInfo);
      detailedInfo.Append("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyProperties_CurrentInput));
      MyValueFormatter.AppendWorkInBestUnit(this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) ? this.ResourceSink.RequiredInputByType(MyResourceDistributorComponent.ElectricityId) : 0.0f, detailedInfo);
    }

    public override void OnModelChange()
    {
      base.OnModelChange();
      if (this.m_activePanelComponent != null)
        this.m_activePanelComponent.Reset();
      if (this.ResourceSink != null)
        this.UpdateScreen();
      if (this.CheckIsWorking() && this.ShowTextOnScreen)
        this.Render.UpdateModelProperties();
      this.HideInactivePanelComponents();
    }

    public float FontSize
    {
      get => this.m_activePanelComponent.FontSize;
      set => this.m_activePanelComponent.FontSize = (float) Math.Round((double) value, 3);
    }

    public Color FontColor
    {
      get => this.m_activePanelComponent.FontColor;
      set => this.m_activePanelComponent.FontColor = value;
    }

    public Color BackgroundColor
    {
      get => this.m_activePanelComponent.BackgroundColor;
      set => this.m_activePanelComponent.BackgroundColor = value;
    }

    public byte BackgroundAlpha
    {
      get => this.m_activePanelComponent.BackgroundAlpha;
      set => this.m_activePanelComponent.BackgroundAlpha = value;
    }

    public float ChangeInterval
    {
      get => this.m_activePanelComponent.ChangeInterval;
      set => this.m_activePanelComponent.ChangeInterval = (float) Math.Round((double) value, 3);
    }

    public void OpenWindow(bool isEditable, bool sync, bool isPublic)
    {
      if (sync)
      {
        this.SendChangeOpenMessage(true, isEditable, Sync.MyId, isPublic);
      }
      else
      {
        this.m_isEditingPublic = isPublic;
        this.CreateTextBox(isEditable, isPublic ? this.PublicDescription : this.PrivateDescription, isPublic);
        MyGuiScreenGamePlay.TmpGameplayScreenHolder = MyGuiScreenGamePlay.ActiveGameplayScreen;
        MyScreenManager.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = (MyGuiScreenBase) this.m_textBox);
      }
    }

    private void CreateTextBox(bool isEditable, StringBuilder description, bool isPublic)
    {
      string missionTitle = isPublic ? this.m_publicTitle.ToString() : this.m_privateTitle.ToString();
      string description1 = description.ToString();
      bool flag = isEditable;
      Action<VRage.Game.ModAPI.ResultEnum> resultCallback = new Action<VRage.Game.ModAPI.ResultEnum>(this.OnClosedPanelTextBox);
      int num = flag ? 1 : 0;
      this.m_textBox = new MyGuiScreenTextPanel(missionTitle, "", "", description1, resultCallback, editable: (num != 0));
    }

    public void OnClosedPanelTextBox(VRage.Game.ModAPI.ResultEnum result)
    {
      if (this.m_textBox == null)
        return;
      if (this.m_textBox.Description.Text.Length > 100000)
      {
        Action<MyGuiScreenMessageBox.ResultEnum> callback = new Action<MyGuiScreenMessageBox.ResultEnum>(this.OnClosedPanelMessageBox);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.MessageBoxTextTooLongText), callback: callback));
      }
      else
        this.CloseWindow(this.m_isEditingPublic);
    }

    public void OnClosedPanelMessageBox(MyGuiScreenMessageBox.ResultEnum result)
    {
      if (result == MyGuiScreenMessageBox.ResultEnum.YES)
      {
        this.m_textBox.Description.Text.Remove(100000, this.m_textBox.Description.Text.Length - 100000);
        this.CloseWindow(this.m_isEditingPublic);
      }
      else
      {
        this.CreateTextBox(true, this.m_textBox.Description.Text, this.m_isEditingPublic);
        MyScreenManager.AddScreen((MyGuiScreenBase) this.m_textBox);
      }
    }

    private void CloseWindow(bool isPublic)
    {
      MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiScreenGamePlay.TmpGameplayScreenHolder;
      MyGuiScreenGamePlay.TmpGameplayScreenHolder = (MyGuiScreenBase) null;
      MySession.Static.Gpss.ScanText(this.m_textBox.Description.Text.ToString(), this.PublicTitle);
      foreach (MySlimBlock cubeBlock in this.CubeGrid.CubeBlocks)
      {
        if (cubeBlock.FatBlock != null && cubeBlock.FatBlock.EntityId == this.EntityId)
        {
          if (isPublic)
            this.m_descriptionPublicDirty = false;
          else
            this.m_descriptionPrivateDirty = false;
          this.SendChangeDescriptionMessage(this.m_textBox.Description.Text, isPublic);
          this.SendChangeOpenMessage(false);
          break;
        }
      }
    }

    public void UpdateScreen()
    {
      if (this.m_activePanelComponent == null)
        return;
      this.m_activePanelComponent.UpdateAfterSimulation(this.CheckIsWorking(), this.IsInRange());
    }

    private bool IsInRange()
    {
      if (!this.IsContentStatic())
        return this.m_panelsComponent.IsInRange((IMyTextPanelProvider) this, this.m_maxRenderDistanceSquared);
      this.m_panelsComponent.Remove((IMyTextPanelProvider) this);
      return true;
    }

    private bool IsContentStatic() => this.m_activePanelComponent != null && this.m_activePanelComponent.IsStatic;

    [Event(null, 885)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void SetSelectedRotationIndex(int newIndex)
    {
      if (newIndex == this.m_selectedRotationIndex)
        return;
      MyTextPanelComponent.ContentMetadata content;
      MyTextPanelComponent.FontData font;
      MyTextPanelComponent.ScriptData script;
      this.m_activePanelComponent.GetLocalValues(out content, out font, out script);
      this.m_panelComponents[newIndex].SetLocalValues(content, font, script);
      this.m_panelComponents[newIndex].Text = this.PublicDescription;
      this.m_panelComponents[newIndex].SelectedTexturesToDraw.Clear();
      this.m_panelComponents[newIndex].SelectedTexturesToDraw.AddRange((IEnumerable<MyLCDTextureDefinition>) this.m_activePanelComponent.SelectedTexturesToDraw);
      this.m_activePanelComponent.ChangeRenderTexture(this.m_selectedRotationIndex, (string) null);
      this.m_activePanelComponent.ReleaseTexture(false);
      this.m_activePanelComponent = this.m_panelComponents[newIndex];
      this.m_selectedRotationIndex = newIndex;
      this.m_newSelectedRotationIndex = newIndex;
      this.RaisePropertiesChanged();
    }

    private void HideInactivePanelComponents()
    {
      if (!this.IsFunctional)
        return;
      for (int index = 0; index < this.m_panelComponents.Count; ++index)
      {
        if (this.m_panelComponents[index] != null)
        {
          this.m_panelComponents[index].SetRender(this.Render);
          if (index != this.m_selectedRotationIndex && !this.m_activePanelComponent.Name.Contains("TransparentScreenArea", StringComparison.Ordinal))
            this.m_panelComponents[index].RemoveTexture(index);
        }
      }
    }

    private void SendSelectRotationIndexRequest(int selection) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyTextPanel, int>(this, (Func<MyTextPanel, Action<int>>) (x => new Action<int>(x.SetSelectedRotationIndex)), selection);

    private void SendRemoveSelectedImageRequest(Sandbox.ModAPI.IMyTextSurface panel, int[] selection) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyTextPanel, int[]>(this, (Func<MyTextPanel, Action<int[]>>) (x => new Action<int[]>(x.OnRemoveSelectedImageRequest)), selection);

    [Event(null, 933)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnRemoveSelectedImageRequest(int[] selection) => this.PanelComponent.RemoveItems(selection);

    private void SendAddImagesToSelectionRequest(Sandbox.ModAPI.IMyTextSurface panel, int[] selection) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyTextPanel, int[]>(this, (Func<MyTextPanel, Action<int[]>>) (x => new Action<int[]>(x.OnSelectImageRequest)), selection);

    [Event(null, 944)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnSelectImageRequest(int[] selection) => this.PanelComponent.SelectItems(selection);

    private void ChangeTextRequest(MyTextPanelComponent panel, string text) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyTextPanel, string, bool>(this, (Func<MyTextPanel, Action<string, bool>>) (x => new Action<string, bool>(x.OnChangeDescription)), text, true);

    private void UpdateSpriteCollection(
      MyTextPanelComponent panel,
      MySerializableSpriteCollection sprites)
    {
      if (!Sync.IsServer)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyTextPanel, MySerializableSpriteCollection>(this, (Func<MyTextPanel, Action<MySerializableSpriteCollection>>) (x => new Action<MySerializableSpriteCollection>(x.OnUpdateSpriteCollection)), sprites);
    }

    [Event(null, 963)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    [DistanceRadius(32f)]
    private void OnUpdateSpriteCollection(MySerializableSpriteCollection sprites)
    {
      foreach (MyTextPanelComponent panelComponent in this.m_panelComponents)
        panelComponent?.UpdateSpriteCollection(sprites);
    }

    [Event(null, 972)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    public void OnChangeDescription(string description, bool isPublic)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Clear().Append(description);
      if (isPublic)
        this.PublicDescription = stringBuilder;
      else
        this.PrivateDescription = stringBuilder;
    }

    [Event(null, 987)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnChangeTitle(string title, bool isPublic)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Clear().Append(title);
      if (isPublic)
        this.PublicTitle = stringBuilder;
      else
        this.PrivateTitle = stringBuilder;
    }

    private void SendChangeOpenMessage(bool isOpen, bool editable = false, ulong user = 0, bool isPublic = false) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyTextPanel, bool, bool, ulong, bool>(this, (Func<MyTextPanel, Action<bool, bool, ulong, bool>>) (x => new Action<bool, bool, ulong, bool>(x.OnChangeOpenRequest)), isOpen, editable, user, isPublic);

    [Event(null, 1007)]
    [Reliable]
    [Server(ValidationType.Access)]
    private void OnChangeOpenRequest(bool isOpen, bool editable, ulong user, bool isPublic)
    {
      if (((!Sync.IsServer ? 0 : (this.IsTextPanelOpen ? 1 : 0)) & (isOpen ? 1 : 0)) != 0)
        return;
      this.OnChangeOpen(isOpen, editable, user, isPublic);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyTextPanel, bool, bool, ulong, bool>(this, (Func<MyTextPanel, Action<bool, bool, ulong, bool>>) (x => new Action<bool, bool, ulong, bool>(x.OnChangeOpenSuccess)), isOpen, editable, user, isPublic);
    }

    [Event(null, 1018)]
    [Reliable]
    [Broadcast]
    private void OnChangeOpenSuccess(bool isOpen, bool editable, ulong user, bool isPublic) => this.OnChangeOpen(isOpen, editable, user, isPublic);

    private void OnChangeOpen(bool isOpen, bool editable, ulong user, bool isPublic)
    {
      this.IsTextPanelOpen = isOpen;
      this.UserId = user;
      if (((Sandbox.Engine.Platform.Game.IsDedicated ? 0 : ((long) user == (long) Sync.MyId ? 1 : 0)) & (isOpen ? 1 : 0)) == 0)
        return;
      this.OpenWindow(editable, false, isPublic);
    }

    private void SendChangeDescriptionMessage(StringBuilder description, bool isPublic)
    {
      if (this.CubeGrid.IsPreview || !this.CubeGrid.SyncFlag)
      {
        if (isPublic)
          this.PublicDescription = description;
        else
          this.PrivateDescription = description;
      }
      else
      {
        if (description.CompareTo(this.PublicDescription) == 0 & isPublic || description.CompareTo(this.PrivateDescription) == 0 && !isPublic)
          return;
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyTextPanel, string, bool>(this, (Func<MyTextPanel, Action<string, bool>>) (x => new Action<string, bool>(x.OnChangeDescription)), description.ToString(), isPublic);
      }
    }

    private void SendChangeTitleMessage(StringBuilder title, bool isPublic)
    {
      if (this.CubeGrid.IsPreview || !this.CubeGrid.SyncFlag)
      {
        if (isPublic)
          this.PublicTitle = title;
        else
          this.PrivateTitle = title;
      }
      else
      {
        if (title.CompareTo(this.PublicTitle) == 0 & isPublic || title.CompareTo(this.PrivateTitle) == 0 && !isPublic)
          return;
        if (isPublic)
          this.PublicTitle = title;
        else
          this.PrivateTitle = title;
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyTextPanel, string, bool>(this, (Func<MyTextPanel, Action<string, bool>>) (x => new Action<string, bool>(x.OnChangeTitle)), title.ToString(), isPublic);
      }
    }

    void Sandbox.ModAPI.Ingame.IMyTextPanel.ShowPrivateTextOnScreen() => this.ShowTextFlag = ShowTextOnScreenFlag.PRIVATE;

    void Sandbox.ModAPI.Ingame.IMyTextPanel.ShowPublicTextOnScreen() => this.ContentType = ContentType.TEXT_AND_IMAGE;

    void Sandbox.ModAPI.Ingame.IMyTextPanel.ShowTextureOnScreen() => this.ContentType = ContentType.TEXT_AND_IMAGE;

    void Sandbox.ModAPI.Ingame.IMyTextPanel.SetShowOnScreen(ShowTextOnScreenFlag set) => this.ShowTextFlag = set;

    string Sandbox.ModAPI.Ingame.IMyTextPanel.GetPublicTitle() => this.m_publicTitleHelper.ToString();

    bool Sandbox.ModAPI.Ingame.IMyTextPanel.WritePublicTitle(string value, bool append)
    {
      if (this.m_isTextPanelOpen)
        return false;
      if (!append)
        this.m_publicTitleHelper.Clear();
      this.m_publicTitleHelper.Append(value);
      this.SendChangeTitleMessage(this.m_publicTitleHelper, true);
      return true;
    }

    bool Sandbox.ModAPI.Ingame.IMyTextPanel.WritePublicText(string value, bool append) => ((Sandbox.ModAPI.Ingame.IMyTextSurface) this).WriteText(value, append);

    string Sandbox.ModAPI.Ingame.IMyTextPanel.GetPublicText() => ((Sandbox.ModAPI.Ingame.IMyTextSurface) this).GetText();

    bool Sandbox.ModAPI.Ingame.IMyTextPanel.WritePublicText(
      StringBuilder value,
      bool append)
    {
      return ((Sandbox.ModAPI.Ingame.IMyTextSurface) this).WriteText(value, append);
    }

    void Sandbox.ModAPI.Ingame.IMyTextPanel.ReadPublicText(
      StringBuilder buffer,
      bool append)
    {
      ((Sandbox.ModAPI.Ingame.IMyTextSurface) this).ReadText(buffer, append);
    }

    bool Sandbox.ModAPI.Ingame.IMyTextPanel.WritePrivateTitle(string value, bool append)
    {
      if (this.m_isTextPanelOpen)
        return false;
      if (!append)
        this.m_privateTitleHelper.Clear();
      this.m_privateTitleHelper.Append(value);
      this.SendChangeTitleMessage(this.m_privateTitleHelper, false);
      return true;
    }

    string Sandbox.ModAPI.Ingame.IMyTextPanel.GetPrivateTitle() => this.m_privateTitle.ToString();

    private void UpdateModApiText()
    {
      if (this.m_descriptionPrivateDirty)
      {
        this.m_descriptionPrivateDirty = false;
        this.SendChangeDescriptionMessage(this.m_privateDescriptionHelper, false);
      }
      if (!this.m_descriptionPublicDirty)
        return;
      this.m_descriptionPublicDirty = false;
      this.SendChangeDescriptionMessage(this.m_publicDescriptionHelper, true);
    }

    bool Sandbox.ModAPI.Ingame.IMyTextPanel.WritePrivateText(string value, bool append)
    {
      if (this.m_isTextPanelOpen)
        return false;
      if (!append)
        this.m_privateDescriptionHelper.Clear();
      this.m_privateDescriptionHelper.Append(value);
      this.m_descriptionPrivateDirty = true;
      return true;
    }

    string Sandbox.ModAPI.Ingame.IMyTextPanel.GetPrivateText() => this.m_privateDescription.ToString();

    ShowTextOnScreenFlag Sandbox.ModAPI.Ingame.IMyTextPanel.ShowOnScreen => this.ShowTextFlag;

    bool Sandbox.ModAPI.Ingame.IMyTextPanel.ShowText => this.ShowTextOnScreen;

    bool Sandbox.ModAPI.Ingame.IMyTextSurface.WriteText(string value, bool append)
    {
      if (this.m_isTextPanelOpen)
        return false;
      if (!append)
        this.m_publicDescriptionHelper.Clear();
      if (value.Length + this.m_publicDescriptionHelper.Length > 100000)
        value = value.Remove(100000 - this.m_publicDescriptionHelper.Length);
      this.m_publicDescriptionHelper.Append(value);
      this.m_descriptionPublicDirty = true;
      return true;
    }

    string Sandbox.ModAPI.Ingame.IMyTextSurface.GetText() => this.m_publicDescription.ToString();

    bool Sandbox.ModAPI.Ingame.IMyTextSurface.WriteText(
      StringBuilder value,
      bool append)
    {
      if (this.m_isTextPanelOpen)
        return false;
      if (!append)
        this.m_publicDescriptionHelper.Clear();
      this.m_publicDescriptionHelper.Append((object) value);
      this.m_descriptionPublicDirty = true;
      return true;
    }

    void Sandbox.ModAPI.Ingame.IMyTextSurface.ReadText(
      StringBuilder buffer,
      bool append)
    {
      if (!append)
        buffer.Clear();
      buffer.AppendStringBuilder(this.m_publicDescription);
    }

    void Sandbox.ModAPI.Ingame.IMyTextSurface.AddImageToSelection(
      string id,
      bool checkExistence)
    {
      if (id == null)
        return;
      for (int index1 = 0; index1 < this.PanelComponent.Definitions.Count; ++index1)
      {
        if (this.PanelComponent.Definitions[index1].Id.SubtypeName == id)
        {
          if (checkExistence)
          {
            for (int index2 = 0; index2 < this.PanelComponent.SelectedTexturesToDraw.Count; ++index2)
            {
              if (this.PanelComponent.SelectedTexturesToDraw[index2].Id.SubtypeName == id)
                return;
            }
          }
          this.SendAddImagesToSelectionRequest((Sandbox.ModAPI.IMyTextSurface) this, new int[1]
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
        for (int index1 = 0; index1 < this.PanelComponent.Definitions.Count; ++index1)
        {
          if (this.PanelComponent.Definitions[index1].Id.SubtypeName == id)
          {
            bool flag = false;
            if (checkExistence)
            {
              for (int index2 = 0; index2 < this.PanelComponent.SelectedTexturesToDraw.Count; ++index2)
              {
                if (this.PanelComponent.SelectedTexturesToDraw[index2].Id.SubtypeName == id)
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
      if (intList.Count <= 0)
        return;
      this.SendAddImagesToSelectionRequest((Sandbox.ModAPI.IMyTextSurface) this, intList.ToArray());
    }

    void Sandbox.ModAPI.Ingame.IMyTextSurface.RemoveImageFromSelection(
      string id,
      bool removeDuplicates)
    {
      if (id == null)
        return;
      List<int> intList = new List<int>();
      for (int index1 = 0; index1 < this.PanelComponent.Definitions.Count; ++index1)
      {
        if (this.PanelComponent.Definitions[index1].Id.SubtypeName == id)
        {
          if (removeDuplicates)
          {
            for (int index2 = 0; index2 < this.PanelComponent.SelectedTexturesToDraw.Count; ++index2)
            {
              if (this.PanelComponent.SelectedTexturesToDraw[index2].Id.SubtypeName == id)
                intList.Add(index1);
            }
            break;
          }
          intList.Add(index1);
          break;
        }
      }
      if (intList.Count <= 0)
        return;
      this.SendRemoveSelectedImageRequest((Sandbox.ModAPI.IMyTextSurface) this, intList.ToArray());
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
        for (int index1 = 0; index1 < this.PanelComponent.Definitions.Count; ++index1)
        {
          if (this.PanelComponent.Definitions[index1].Id.SubtypeName == id)
          {
            if (removeDuplicates)
            {
              for (int index2 = 0; index2 < this.PanelComponent.SelectedTexturesToDraw.Count; ++index2)
              {
                if (this.PanelComponent.SelectedTexturesToDraw[index2].Id.SubtypeName == id)
                  intList.Add(index1);
              }
              break;
            }
            intList.Add(index1);
            break;
          }
        }
      }
      if (intList.Count <= 0)
        return;
      this.SendRemoveSelectedImageRequest((Sandbox.ModAPI.IMyTextSurface) this, intList.ToArray());
    }

    void Sandbox.ModAPI.Ingame.IMyTextSurface.ClearImagesFromSelection()
    {
      if (this.PanelComponent.SelectedTexturesToDraw.Count == 0)
        return;
      List<int> intList = new List<int>();
      for (int index1 = 0; index1 < this.PanelComponent.SelectedTexturesToDraw.Count; ++index1)
      {
        for (int index2 = 0; index2 < this.PanelComponent.Definitions.Count; ++index2)
        {
          if (this.PanelComponent.Definitions[index2].Id.SubtypeName == this.PanelComponent.SelectedTexturesToDraw[index1].Id.SubtypeName)
          {
            intList.Add(index2);
            break;
          }
        }
      }
      this.SendRemoveSelectedImageRequest((Sandbox.ModAPI.IMyTextSurface) this, intList.ToArray());
    }

    void Sandbox.ModAPI.Ingame.IMyTextSurface.GetSelectedImages(List<string> output)
    {
      foreach (MyLCDTextureDefinition textureDefinition in this.PanelComponent.SelectedTexturesToDraw)
        output.Add(textureDefinition.Id.SubtypeName);
    }

    string Sandbox.ModAPI.Ingame.IMyTextSurface.CurrentlyShownImage
    {
      get
      {
        if (this.PanelComponent.SelectedTexturesToDraw.Count == 0)
          return (string) null;
        return this.PanelComponent.CurrentSelectedTexture >= this.PanelComponent.SelectedTexturesToDraw.Count ? this.PanelComponent.SelectedTexturesToDraw[0].Id.SubtypeName : this.PanelComponent.SelectedTexturesToDraw[this.PanelComponent.CurrentSelectedTexture].Id.SubtypeName;
      }
    }

    string Sandbox.ModAPI.Ingame.IMyTextSurface.Font
    {
      get => this.PanelComponent.Font.SubtypeName;
      set
      {
        if (string.IsNullOrEmpty(value) || MyDefinitionManager.Static.GetDefinition<MyFontDefinition>(value) == null)
          return;
        this.PanelComponent.Font = MyDefinitionManager.Static.GetDefinition<MyFontDefinition>(value).Id;
      }
    }

    void Sandbox.ModAPI.Ingame.IMyTextSurface.GetFonts(List<string> fonts)
    {
      if (fonts == null)
        return;
      foreach (MyFontDefinition definition in MyDefinitionManager.Static.GetDefinitions<MyFontDefinition>())
        fonts.Add(definition.Id.SubtypeName);
    }

    public void GetSprites(List<string> sprites) => this.PanelComponent.GetSprites(sprites);

    TextAlignment Sandbox.ModAPI.Ingame.IMyTextSurface.Alignment
    {
      get => this.m_activePanelComponent == null ? TextAlignment.LEFT : this.m_activePanelComponent.Alignment;
      set
      {
        if (this.m_activePanelComponent == null)
          return;
        this.m_activePanelComponent.Alignment = value;
      }
    }

    void Sandbox.ModAPI.Ingame.IMyTextSurface.GetScripts(List<string> scripts)
    {
      if (this.m_activePanelComponent == null)
        return;
      this.m_activePanelComponent.GetScripts(scripts);
    }

    string Sandbox.ModAPI.Ingame.IMyTextSurface.Script
    {
      get => this.m_activePanelComponent == null ? string.Empty : this.m_activePanelComponent.Script;
      set
      {
        if (this.m_activePanelComponent == null)
          return;
        this.m_activePanelComponent.Script = value;
      }
    }

    ContentType Sandbox.ModAPI.Ingame.IMyTextSurface.ContentType
    {
      get => this.ContentType;
      set => this.ContentType = value;
    }

    Vector2 Sandbox.ModAPI.Ingame.IMyTextSurface.SurfaceSize => this.SurfaceSize;

    Vector2 Sandbox.ModAPI.Ingame.IMyTextSurface.TextureSize => this.TextureSize;

    MySpriteDrawFrame Sandbox.ModAPI.Ingame.IMyTextSurface.DrawFrame() => this.m_activePanelComponent != null ? this.m_activePanelComponent.DrawFrame() : new MySpriteDrawFrame((Action<MySpriteDrawFrame>) null);

    bool Sandbox.ModAPI.Ingame.IMyTextSurface.PreserveAspectRatio
    {
      get => this.m_activePanelComponent != null && this.m_activePanelComponent.PreserveAspectRatio;
      set
      {
        if (this.m_activePanelComponent == null)
          return;
        this.m_activePanelComponent.PreserveAspectRatio = value;
      }
    }

    float Sandbox.ModAPI.Ingame.IMyTextSurface.TextPadding
    {
      get => this.m_activePanelComponent == null ? 0.0f : this.m_activePanelComponent.TextPadding;
      set
      {
        if (this.m_activePanelComponent == null)
          return;
        this.m_activePanelComponent.TextPadding = value;
      }
    }

    Color Sandbox.ModAPI.Ingame.IMyTextSurface.ScriptBackgroundColor
    {
      get => this.m_activePanelComponent == null ? Color.White : this.m_activePanelComponent.ScriptBackgroundColor;
      set
      {
        if (this.m_activePanelComponent == null)
          return;
        this.m_activePanelComponent.ScriptBackgroundColor = value;
      }
    }

    Color Sandbox.ModAPI.Ingame.IMyTextSurface.ScriptForegroundColor
    {
      get => this.m_activePanelComponent == null ? Color.White : this.m_activePanelComponent.ScriptForegroundColor;
      set
      {
        if (this.m_activePanelComponent == null)
          return;
        this.m_activePanelComponent.ScriptForegroundColor = value;
      }
    }

    public Vector2 MeasureStringInPixels(StringBuilder text, string font, float scale) => MyGuiManager.MeasureStringRaw(font, text, scale);

    public Vector2 MeasureStringInPixels(string text, string font, float scale) => MyGuiManager.MeasureStringRaw(font, text, scale);

    string Sandbox.ModAPI.Ingame.IMyTextSurface.Name => this.m_activePanelComponent == null ? (string) null : this.m_activePanelComponent.Name;

    string Sandbox.ModAPI.Ingame.IMyTextSurface.DisplayName => this.m_activePanelComponent == null ? (string) null : this.m_activePanelComponent.DisplayName;

    int Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider.SurfaceCount => 1;

    Sandbox.ModAPI.Ingame.IMyTextSurface Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider.GetSurface(
      int index)
    {
      return index != 0 ? (Sandbox.ModAPI.Ingame.IMyTextSurface) null : (Sandbox.ModAPI.Ingame.IMyTextSurface) this.m_activePanelComponent;
    }

    protected sealed class SetSelectedRotationIndex\u003C\u003ESystem_Int32 : ICallSite<MyTextPanel, int, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyTextPanel @this,
        in int newIndex,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.SetSelectedRotationIndex(newIndex);
      }
    }

    protected sealed class OnRemoveSelectedImageRequest\u003C\u003ESystem_Int32\u003C\u0023\u003E : ICallSite<MyTextPanel, int[], DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyTextPanel @this,
        in int[] selection,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnRemoveSelectedImageRequest(selection);
      }
    }

    protected sealed class OnSelectImageRequest\u003C\u003ESystem_Int32\u003C\u0023\u003E : ICallSite<MyTextPanel, int[], DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyTextPanel @this,
        in int[] selection,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnSelectImageRequest(selection);
      }
    }

    protected sealed class OnUpdateSpriteCollection\u003C\u003EVRage_Game_GUI_TextPanel_MySerializableSpriteCollection : ICallSite<MyTextPanel, MySerializableSpriteCollection, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyTextPanel @this,
        in MySerializableSpriteCollection sprites,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnUpdateSpriteCollection(sprites);
      }
    }

    protected sealed class OnChangeDescription\u003C\u003ESystem_String\u0023System_Boolean : ICallSite<MyTextPanel, string, bool, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyTextPanel @this,
        in string description,
        in bool isPublic,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnChangeDescription(description, isPublic);
      }
    }

    protected sealed class OnChangeTitle\u003C\u003ESystem_String\u0023System_Boolean : ICallSite<MyTextPanel, string, bool, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyTextPanel @this,
        in string title,
        in bool isPublic,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnChangeTitle(title, isPublic);
      }
    }

    protected sealed class OnChangeOpenRequest\u003C\u003ESystem_Boolean\u0023System_Boolean\u0023System_UInt64\u0023System_Boolean : ICallSite<MyTextPanel, bool, bool, ulong, bool, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyTextPanel @this,
        in bool isOpen,
        in bool editable,
        in ulong user,
        in bool isPublic,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnChangeOpenRequest(isOpen, editable, user, isPublic);
      }
    }

    protected sealed class OnChangeOpenSuccess\u003C\u003ESystem_Boolean\u0023System_Boolean\u0023System_UInt64\u0023System_Boolean : ICallSite<MyTextPanel, bool, bool, ulong, bool, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyTextPanel @this,
        in bool isOpen,
        in bool editable,
        in ulong user,
        in bool isPublic,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnChangeOpenSuccess(isOpen, editable, user, isPublic);
      }
    }

    private class Sandbox_Game_Entities_Blocks_MyTextPanel\u003C\u003EActor : IActivator, IActivator<MyTextPanel>
    {
      object IActivator.CreateInstance() => (object) new MyTextPanel();

      MyTextPanel IActivator<MyTextPanel>.CreateInstance() => new MyTextPanel();
    }
  }
}
