// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.MyProjectorBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using VRage.Game.Utils;
using VRage.Generics;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Profiler;
using VRage.Serialization;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Blocks
{
  public abstract class MyProjectorBase : MyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider, IMyMultiTextPanelComponentOwner, IMyTextPanelComponentOwner, Sandbox.ModAPI.IMyProjector, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyProjector, Sandbox.ModAPI.IMyTextSurfaceProvider
  {
    private const int PROJECTION_UPDATE_TIME = 2000;
    protected const int OFFSET_LIMIT = 50;
    protected const int ROTATION_LIMIT = 2;
    protected const float SCALE_LIMIT = 0.02f;
    protected const int MAX_SCALED_DRAW_DISTANCE = 50;
    protected const int MAX_SCALED_DRAW_DISTANCE_SQUARED = 2500;
    private int m_lastUpdate;
    private readonly MyProjectorClipboard m_clipboard;
    private readonly MyProjectorClipboard m_spawnClipboard;
    protected Vector3I m_projectionOffset;
    protected Vector3I m_projectionRotation;
    protected float m_projectionScale = 1f;
    private MySlimBlock m_hiddenBlock;
    private bool m_shouldUpdateProjection;
    private bool m_forceUpdateProjection;
    private bool m_shouldUpdateTexts;
    private bool m_shouldResetBuildable;
    private List<MyObjectBuilder_CubeGrid> m_savedProjections;
    protected bool m_showOnlyBuildable;
    private int m_frameCount;
    private bool m_removeRequested;
    private Task m_updateTask;
    private List<MyObjectBuilder_CubeGrid> m_originalGridBuilders;
    protected const int MAX_NUMBER_OF_PROJECTIONS = 1000;
    protected const int MAX_NUMBER_OF_BLOCKS = 10000;
    private int m_projectionsRemaining;
    private MyMultiTextPanelComponent m_multiPanel;
    private MyGuiScreenTextPanel m_textBox;
    private bool m_tierCanProject;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_keepProjection;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_instantBuildingEnabled;
    private readonly VRage.Sync.Sync<int, SyncDirection.BothWays> m_maxNumberOfProjections;
    private readonly VRage.Sync.Sync<int, SyncDirection.BothWays> m_maxNumberOfBlocksPerProjection;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_getOwnershipFromProjector;
    public static readonly int PROJECTION_TIME_IN_FRAMES = 10800;
    private int m_projectionTimer = MyProjectorBase.PROJECTION_TIME_IN_FRAMES;
    private bool m_isTextPanelOpen;
    private HashSet<MySlimBlock> m_visibleBlocks = new HashSet<MySlimBlock>();
    private HashSet<MySlimBlock> m_buildableBlocks = new HashSet<MySlimBlock>();
    private HashSet<MySlimBlock> m_hiddenBlocks = new HashSet<MySlimBlock>();
    private int m_remainingBlocks;
    private int m_totalBlocks;
    private readonly Dictionary<MyCubeBlockDefinition, int> m_remainingBlocksPerType = new Dictionary<MyCubeBlockDefinition, int>();
    private int m_remainingArmorBlocks;
    private int m_buildableBlocksCount;
    private bool m_statsDirty;

    public MyProjectorDefinition BlockDefinition => (MyProjectorDefinition) base.BlockDefinition;

    public MyProjectorClipboard Clipboard => this.m_clipboard;

    internal MyRenderComponentScreenAreas Render
    {
      get => base.Render as MyRenderComponentScreenAreas;
      set => this.Render = (MyRenderComponentBase) value;
    }

    public Vector3I ProjectionOffset => this.m_projectionOffset;

    public Vector3I ProjectionRotation => this.m_projectionRotation;

    public Quaternion ProjectionRotationQuaternion
    {
      get
      {
        Vector3 vector3 = this.ProjectionRotation * MathHelper.ToRadians((float) this.BlockDefinition.RotationAngleStepDeg);
        return Quaternion.CreateFromYawPitchRoll(vector3.X, vector3.Y, vector3.Z);
      }
    }

    public MyProjectorBase()
    {
      this.m_clipboard = new MyProjectorClipboard(this, MyClipboardComponent.ClipboardDefinition.PastingSettings);
      this.m_spawnClipboard = new MyProjectorClipboard(this, MyClipboardComponent.ClipboardDefinition.PastingSettings);
      this.m_instantBuildingEnabled.ValueChanged += new Action<SyncBase>(this.m_instantBuildingEnabled_ValueChanged);
      this.m_maxNumberOfProjections.ValueChanged += new Action<SyncBase>(this.m_maxNumberOfProjections_ValueChanged);
      this.m_maxNumberOfBlocksPerProjection.ValueChanged += new Action<SyncBase>(this.m_maxNumberOfBlocksPerProjection_ValueChanged);
      this.m_getOwnershipFromProjector.ValueChanged += new Action<SyncBase>(this.m_getOwnershipFromProjector_ValueChanged);
      this.Render = new MyRenderComponentScreenAreas((MyEntity) this);
    }

    public MyCubeGrid ProjectedGrid => this.m_clipboard.PreviewGrids.Count != 0 ? this.m_clipboard.PreviewGrids[0] : (MyCubeGrid) null;

    protected bool InstantBuildingEnabled
    {
      get => (bool) this.m_instantBuildingEnabled;
      set => this.m_instantBuildingEnabled.Value = value;
    }

    protected int MaxNumberOfProjections
    {
      get => (int) this.m_maxNumberOfProjections;
      set => this.m_maxNumberOfProjections.Value = value;
    }

    protected int MaxNumberOfBlocksPerProjection
    {
      get => (int) this.m_maxNumberOfBlocksPerProjection;
      set => this.m_maxNumberOfBlocksPerProjection.Value = value;
    }

    protected bool GetOwnershipFromProjector
    {
      get => (bool) this.m_getOwnershipFromProjector;
      set => this.m_getOwnershipFromProjector.Value = value;
    }

    protected bool KeepProjection
    {
      get => (bool) this.m_keepProjection;
      set => this.m_keepProjection.Value = value;
    }

    public bool IsActivating { get; private set; }

    public float Scale => !this.BlockDefinition.AllowScaling ? 1f : this.m_projectionScale;

    public bool AllowScaling => this.BlockDefinition.AllowScaling;

    public bool AllowWelding => this.BlockDefinition.AllowWelding && !this.BlockDefinition.AllowScaling && !this.BlockDefinition.IgnoreSize;

    public override bool IsTieredUpdateSupported => true;

    public bool TierCanProject
    {
      get => this.m_tierCanProject || this.m_projectionTimer > 0;
      private set
      {
        if (this.m_tierCanProject == value)
          return;
        if (!value)
        {
          this.m_projectionTimer = MyProjectorBase.PROJECTION_TIME_IN_FRAMES;
          this.m_tierCanProject = value;
        }
        else
        {
          this.m_tierCanProject = value;
          this.MyProjector_IsWorkingChanged((MyCubeBlock) this);
        }
      }
    }

    protected bool IsProjecting() => this.m_clipboard.IsActive;

    protected bool CanProject()
    {
      this.UpdateIsWorking();
      this.UpdateText();
      return this.IsWorking;
    }

    protected void OnOffsetsChanged()
    {
      this.m_shouldUpdateProjection = true;
      this.m_shouldUpdateTexts = true;
      this.SendNewOffset(this.m_projectionOffset, this.m_projectionRotation, this.m_projectionScale, this.m_showOnlyBuildable);
      if (!this.AllowWelding)
        return;
      this.Remap();
    }

    public void SelectBlueprint()
    {
      MyEntity interactedEntity = (MyEntity) null;
      if (MyGuiScreenTerminal.IsOpen)
      {
        interactedEntity = MyGuiScreenTerminal.InteractedEntity;
        MyGuiScreenTerminal.Hide();
      }
      this.SendRemoveProjection();
      MyBlueprintUtils.OpenBlueprintScreen((MyGridClipboard) this.m_clipboard, true, MyBlueprintAccessType.PROJECTOR, (Action<MyGuiBlueprintScreen_Reworked>) (bp =>
      {
        if (bp == null)
          return;
        bp.Closed += (MyGuiScreenBase.ScreenHandler) ((screen, isUnloading) => this.OnBlueprintScreen_Closed(screen, interactedEntity, isUnloading));
      }));
    }

    public bool SelectPrefab(string prefabName)
    {
      MyObjectBuilder_CubeGrid[] gridPrefab = MyPrefabManager.Static.GetGridPrefab(prefabName);
      if (gridPrefab == null || gridPrefab.Length == 0)
        return false;
      this.m_clipboard.Deactivate();
      this.m_clipboard.SetGridFromBuilders(gridPrefab, Vector3.Zero, 0.0f);
      this.InitFromObjectBuilder(new List<MyObjectBuilder_CubeGrid>((IEnumerable<MyObjectBuilder_CubeGrid>) gridPrefab));
      return true;
    }

    public Vector3 GetProjectionTranslationOffset() => this.m_projectionOffset * this.m_clipboard.GridSize * this.Scale;

    private void RequestRemoveProjection()
    {
      this.m_removeRequested = true;
      this.m_frameCount = 0;
    }

    private void RemoveProjection(bool keepProjection)
    {
      this.m_hiddenBlock = (MySlimBlock) null;
      if (this.ProjectedGrid != null)
      {
        int num = 0;
        foreach (MyCubeGrid previewGrid in this.m_clipboard.PreviewGrids)
          num += previewGrid.CubeBlocks.Count;
        MyIdentity identity = MySession.Static.Players.TryGetIdentity(this.BuiltBy);
        if (identity != null)
        {
          int pcu = num;
          identity.BlockLimits.DecreaseBlocksBuilt(this.BlockDefinition.BlockPairName, pcu, this.CubeGrid, false);
          this.CubeGrid.BlocksPCU -= pcu;
        }
      }
      this.m_clipboard.Deactivate();
      if (!keepProjection)
      {
        this.m_clipboard.Clear();
        this.m_originalGridBuilders = (List<MyObjectBuilder_CubeGrid>) null;
      }
      this.UpdateSounds();
      this.SetEmissiveStateWorking();
      this.m_statsDirty = true;
      this.UpdateText();
      this.RaisePropertiesChanged();
    }

    private void ResetRotation() => this.SetRotation((MyGridClipboard) this.m_clipboard, -this.m_projectionRotation);

    private void SetRotation(MyGridClipboard clipboard, Vector3I rotation)
    {
      clipboard.RotateAroundAxis(0, Math.Sign(rotation.X), true, Math.Abs((float) rotation.X * 1.570796f));
      clipboard.RotateAroundAxis(1, Math.Sign(rotation.Y), true, Math.Abs((float) rotation.Y * 1.570796f));
      clipboard.RotateAroundAxis(2, Math.Sign(rotation.Z), true, Math.Abs((float) rotation.Z * 1.570796f));
    }

    private void OnBlueprintScreen_Closed(
      MyGuiScreenBase source,
      MyEntity interactedEntity = null,
      bool isUnloading = false)
    {
      if (isUnloading)
        return;
      this.InitFromObjectBuilder(this.m_clipboard.CopiedGrids, interactedEntity);
      this.ReopenTerminal(interactedEntity);
    }

    private void InitFromObjectBuilder(
      List<MyObjectBuilder_CubeGrid> gridsObs,
      MyEntity interactedEntity = null)
    {
      this.ResourceSink.Update();
      this.UpdateIsWorking();
      if (gridsObs.Count == 0 || !this.IsWorking)
      {
        this.RemoveProjection(false);
        if (interactedEntity == null)
          return;
        this.ReopenTerminal(interactedEntity);
      }
      else if (!this.BlockDefinition.IgnoreSize && (double) this.m_clipboard.GridSize != (double) this.CubeGrid.GridSize)
      {
        this.RemoveProjection(false);
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, messageText: MyTexts.Get(MySpaceTexts.NotificationProjectorGridSize), messageCaption: messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result =>
        {
          if (interactedEntity == null)
            return;
          this.ReopenTerminal(interactedEntity);
        }))));
      }
      else
      {
        if (gridsObs.Count > 1 && this.AllowWelding)
        {
          StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning);
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, messageText: MyTexts.Get(MySpaceTexts.NotificationProjectorMultipleGrids), messageCaption: messageCaption));
        }
        int largestGridIndex = -1;
        int num = -1;
        if (this.AllowWelding)
        {
          for (int index = 0; index < gridsObs.Count; ++index)
          {
            int count = gridsObs[index].CubeBlocks.Count;
            if (count > num)
            {
              num = count;
              largestGridIndex = index;
            }
          }
        }
        List<MyObjectBuilder_CubeGrid> gridBuilders = new List<MyObjectBuilder_CubeGrid>();
        this.m_originalGridBuilders = (List<MyObjectBuilder_CubeGrid>) null;
        Parallel.Start((Action) (() =>
        {
          if (largestGridIndex != -1)
          {
            gridBuilders.Add((MyObjectBuilder_CubeGrid) gridsObs[largestGridIndex].Clone());
          }
          else
          {
            foreach (MyObjectBuilder_Base gridsOb in gridsObs)
              gridBuilders.Add((MyObjectBuilder_CubeGrid) gridsOb.Clone());
          }
          foreach (MyObjectBuilder_CubeGrid gridsOb in gridsObs)
            this.m_clipboard.ProcessCubeGrid(gridsOb);
          foreach (MyObjectBuilder_EntityBase objectBuilder in gridBuilders)
            Sandbox.Game.Entities.MyEntities.RemapObjectBuilder(objectBuilder);
        }), (Action) (() =>
        {
          if (gridBuilders.Count <= 0 || this.m_originalGridBuilders != null)
            return;
          this.m_originalGridBuilders = gridBuilders;
          this.SendNewBlueprint(this.m_originalGridBuilders);
        }));
      }
    }

    private void ReopenTerminal(MyEntity interactedEntity = null)
    {
      if (MyGuiScreenTerminal.IsOpen)
        return;
      MyGuiScreenTerminal.Show(MyTerminalPageEnum.ControlPanel, MySession.Static.LocalCharacter, interactedEntity ?? (MyEntity) this);
    }

    protected bool ScenarioSettingsEnabled() => MySession.Static.Settings.ScenarioEditMode || MySession.Static.IsScenario;

    protected bool CanEditInstantBuildingSettings() => this.CanEnableInstantBuilding() && (bool) this.m_instantBuildingEnabled;

    protected bool CanEnableInstantBuilding() => MySession.Static.Settings.ScenarioEditMode;

    protected bool CanSpawnProjection()
    {
      if (!(bool) this.m_instantBuildingEnabled || this.ProjectedGrid == null)
        return false;
      int num = 0;
      foreach (MyCubeGrid previewGrid in this.m_clipboard.PreviewGrids)
        num += previewGrid.CubeBlocks.Count;
      return ((int) this.m_maxNumberOfBlocksPerProjection >= 10000 || (int) this.m_maxNumberOfBlocksPerProjection >= num) && (this.m_projectionsRemaining != 0 && this.ScenarioSettingsEnabled());
    }

    protected void TrySetInstantBuilding(bool v)
    {
      if (!this.CanEnableInstantBuilding())
        return;
      this.InstantBuildingEnabled = v;
    }

    protected void TrySetGetOwnership(bool v)
    {
      if (!this.CanEnableInstantBuilding())
        return;
      this.GetOwnershipFromProjector = v;
    }

    protected void TrySpawnProjection()
    {
      if (!this.CanSpawnProjection())
        return;
      this.SendSpawnProjection();
    }

    protected void TryChangeMaxNumberOfBlocksPerProjection(float v)
    {
      if (!this.CanEditInstantBuildingSettings())
        return;
      this.MaxNumberOfBlocksPerProjection = (int) Math.Round((double) v);
    }

    protected void TryChangeNumberOfProjections(float v)
    {
      if (!this.CanEditInstantBuildingSettings())
        return;
      this.MaxNumberOfProjections = (int) Math.Round((double) v);
    }

    void IMyMultiTextPanelComponentOwner.SelectPanel(
      List<MyGuiControlListbox.Item> panelItems)
    {
      this.m_multiPanel.SelectPanel((int) panelItems[0].UserData);
      this.RaisePropertiesChanged();
    }

    MyMultiTextPanelComponent IMyMultiTextPanelComponentOwner.MultiTextPanel => this.m_multiPanel;

    public MyTextPanelComponent PanelComponent => this.m_multiPanel == null ? (MyTextPanelComponent) null : this.m_multiPanel.PanelComponent;

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

    public void OpenWindow(bool isEditable, bool sync, bool isPublic)
    {
      if (sync)
      {
        this.SendChangeOpenMessage(true, isEditable, Sandbox.Game.Multiplayer.Sync.MyId, isPublic);
      }
      else
      {
        this.CreateTextBox(isEditable, new StringBuilder(this.PanelComponent.Text.ToString()), isPublic);
        MyGuiScreenGamePlay.TmpGameplayScreenHolder = MyGuiScreenGamePlay.ActiveGameplayScreen;
        MyScreenManager.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = (MyGuiScreenBase) this.m_textBox);
      }
    }

    private void SendChangeOpenMessage(bool isOpen, bool editable = false, ulong user = 0, bool isPublic = false) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProjectorBase, bool, bool, ulong, bool>(this, (Func<MyProjectorBase, Action<bool, bool, ulong, bool>>) (x => new Action<bool, bool, ulong, bool>(x.OnChangeOpenRequest)), isOpen, editable, user, isPublic);

    [Event(null, 581)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void OnChangeOpenRequest(bool isOpen, bool editable, ulong user, bool isPublic)
    {
      if (((!Sandbox.Game.Multiplayer.Sync.IsServer ? 0 : (this.IsTextPanelOpen ? 1 : 0)) & (isOpen ? 1 : 0)) != 0)
        return;
      this.OnChangeOpen(isOpen, editable, user, isPublic);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProjectorBase, bool, bool, ulong, bool>(this, (Func<MyProjectorBase, Action<bool, bool, ulong, bool>>) (x => new Action<bool, bool, ulong, bool>(x.OnChangeOpenSuccess)), isOpen, editable, user, isPublic);
    }

    [Event(null, 592)]
    [Reliable]
    [Broadcast]
    private void OnChangeOpenSuccess(bool isOpen, bool editable, ulong user, bool isPublic) => this.OnChangeOpen(isOpen, editable, user, isPublic);

    private void OnChangeOpen(bool isOpen, bool editable, ulong user, bool isPublic)
    {
      this.IsTextPanelOpen = isOpen;
      if (((Sandbox.Engine.Platform.Game.IsDedicated ? 0 : ((long) user == (long) Sandbox.Game.Multiplayer.Sync.MyId ? 1 : 0)) & (isOpen ? 1 : 0)) == 0)
        return;
      this.OpenWindow(editable, false, isPublic);
    }

    private void CreateTextBox(bool isEditable, StringBuilder description, bool isPublic)
    {
      string displayNameText = this.DisplayNameText;
      string displayName = this.PanelComponent.DisplayName;
      string description1 = description.ToString();
      bool flag = isEditable;
      Action<VRage.Game.ModAPI.ResultEnum> resultCallback = new Action<VRage.Game.ModAPI.ResultEnum>(this.OnClosedPanelTextBox);
      int num = flag ? 1 : 0;
      this.m_textBox = new MyGuiScreenTextPanel(displayNameText, "", displayName, description1, resultCallback, editable: (num != 0));
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
        this.CloseWindow(true);
    }

    public void OnClosedPanelMessageBox(MyGuiScreenMessageBox.ResultEnum result)
    {
      if (result == MyGuiScreenMessageBox.ResultEnum.YES)
      {
        this.m_textBox.Description.Text.Remove(100000, this.m_textBox.Description.Text.Length - 100000);
        this.CloseWindow(true);
      }
      else
      {
        this.CreateTextBox(true, this.m_textBox.Description.Text, true);
        MyScreenManager.AddScreen((MyGuiScreenBase) this.m_textBox);
      }
    }

    private void CloseWindow(bool isPublic)
    {
      MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiScreenGamePlay.TmpGameplayScreenHolder;
      MyGuiScreenGamePlay.TmpGameplayScreenHolder = (MyGuiScreenBase) null;
      foreach (MySlimBlock cubeBlock in this.CubeGrid.CubeBlocks)
      {
        if (cubeBlock.FatBlock != null && cubeBlock.FatBlock.EntityId == this.EntityId)
        {
          this.SendChangeDescriptionMessage(this.m_textBox.Description.Text, isPublic);
          this.SendChangeOpenMessage(false);
          break;
        }
      }
    }

    private void SendChangeDescriptionMessage(StringBuilder description, bool isPublic)
    {
      if (this.CubeGrid.IsPreview || !this.CubeGrid.SyncFlag)
      {
        this.PanelComponent.Text = description;
      }
      else
      {
        if (description.CompareTo(this.PanelComponent.Text) == 0)
          return;
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProjectorBase, string, bool>(this, (Func<MyProjectorBase, Action<string, bool>>) (x => new Action<string, bool>(x.OnChangeDescription)), description.ToString(), isPublic);
      }
    }

    [Event(null, 683)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    public void OnChangeDescription(string description, bool isPublic)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Clear().Append(description);
      this.PanelComponent.Text = stringBuilder;
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    public void ShowCube(MySlimBlock cubeBlock, bool canBuild)
    {
      if (canBuild)
        this.SetTransparency(cubeBlock, 0.25f);
      else
        this.SetTransparency(cubeBlock, MyGridConstants.PROJECTOR_TRANSPARENCY);
    }

    public void HideCube(MySlimBlock cubeBlock) => this.SetTransparency(cubeBlock, 1f);

    protected virtual void SetTransparency(MySlimBlock cubeBlock, float transparency)
    {
      transparency = -transparency;
      if ((double) cubeBlock.Dithering == (double) transparency && (double) cubeBlock.CubeGrid.Render.Transparency == (double) transparency)
        return;
      cubeBlock.CubeGrid.Render.Transparency = transparency;
      cubeBlock.CubeGrid.Render.CastShadows = false;
      cubeBlock.Dithering = transparency;
      cubeBlock.UpdateVisual(true);
      MyCubeBlock fatBlock = cubeBlock.FatBlock;
      if (fatBlock != null)
      {
        fatBlock.Render.CastShadows = false;
        this.SetTransparencyForSubparts((MyEntity) fatBlock, transparency);
      }
      if (fatBlock == null || fatBlock.UseObjectsComponent == null || fatBlock.UseObjectsComponent.DetectorPhysics == null)
        return;
      fatBlock.UseObjectsComponent.DetectorPhysics.Enabled = false;
    }

    private void SetTransparencyForSubparts(MyEntity renderEntity, float transparency)
    {
      renderEntity.Render.CastShadows = false;
      if (renderEntity.Subparts == null)
        return;
      foreach (KeyValuePair<string, MyEntitySubpart> subpart in renderEntity.Subparts)
      {
        subpart.Value.Render.Transparency = transparency;
        subpart.Value.Render.CastShadows = false;
        subpart.Value.Render.RemoveRenderObjects();
        subpart.Value.Render.AddRenderObjects();
        this.SetTransparencyForSubparts((MyEntity) subpart.Value, transparency);
      }
    }

    private void HideIntersectedBlock()
    {
      if ((bool) this.m_instantBuildingEnabled)
        return;
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      if (localCharacter == null)
        return;
      Vector3D translation = localCharacter.GetHeadMatrix(true, true, false, false, false).Translation;
      if (this.ProjectedGrid == null)
        return;
      MySlimBlock cubeBlock = this.ProjectedGrid.GetCubeBlock(this.ProjectedGrid.WorldToGridInteger(translation));
      if (cubeBlock != null)
      {
        if ((double) Math.Abs(cubeBlock.Dithering) >= 1.0 || this.m_hiddenBlock == cubeBlock)
          return;
        if (this.m_hiddenBlock != null)
          this.ShowCube(this.m_hiddenBlock, this.CanBuild(this.m_hiddenBlock));
        this.HideCube(cubeBlock);
        this.m_hiddenBlock = cubeBlock;
      }
      else
      {
        if (this.m_hiddenBlock == null)
          return;
        this.ShowCube(this.m_hiddenBlock, this.CanBuild(this.m_hiddenBlock));
        this.m_hiddenBlock = (MySlimBlock) null;
      }
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(this.BlockDefinition.ResourceSinkGroup, this.BlockDefinition.RequiredPowerInput, new Func<float>(this.CalculateRequiredPowerInput));
      this.ResourceSink = resourceSinkComponent;
      base.Init(objectBuilder, cubeGrid);
      if (!MyFakes.ENABLE_PROJECTOR_BLOCK)
        return;
      MyObjectBuilder_ProjectorBase builderProjectorBase = (MyObjectBuilder_ProjectorBase) objectBuilder;
      List<MyObjectBuilder_CubeGrid> objectBuilderCubeGridList = new List<MyObjectBuilder_CubeGrid>();
      if ((builderProjectorBase.ProjectedGrids == null || builderProjectorBase.ProjectedGrids.Count == 0) && builderProjectorBase.ProjectedGrid != null)
        objectBuilderCubeGridList.Add(builderProjectorBase.ProjectedGrid);
      else
        objectBuilderCubeGridList = builderProjectorBase.ProjectedGrids;
      if (objectBuilderCubeGridList != null && objectBuilderCubeGridList.Count > 0)
      {
        this.m_projectionOffset = Vector3I.Clamp(builderProjectorBase.ProjectionOffset, new Vector3I(-50), new Vector3I(50));
        int xyz = (int) (360.0 / (double) this.BlockDefinition.RotationAngleStepDeg);
        this.m_projectionRotation = Vector3I.Clamp(builderProjectorBase.ProjectionRotation, new Vector3I(-xyz), new Vector3I(xyz));
        this.m_projectionScale = builderProjectorBase.Scale;
        this.m_savedProjections = objectBuilderCubeGridList;
        this.m_keepProjection.SetLocalValue(builderProjectorBase.KeepProjection);
      }
      this.m_showOnlyBuildable = builderProjectorBase.ShowOnlyBuildable;
      this.m_instantBuildingEnabled.SetLocalValue(builderProjectorBase.InstantBuildingEnabled);
      this.m_maxNumberOfProjections.SetLocalValue(MathHelper.Clamp(builderProjectorBase.MaxNumberOfProjections, 0, 1000));
      this.m_maxNumberOfBlocksPerProjection.SetLocalValue(MathHelper.Clamp(builderProjectorBase.MaxNumberOfBlocks, 0, 10000));
      this.m_getOwnershipFromProjector.SetLocalValue(builderProjectorBase.GetOwnershipFromProjector);
      this.m_projectionsRemaining = MathHelper.Clamp(builderProjectorBase.ProjectionsRemaining, 0, (int) this.m_maxNumberOfProjections);
      this.IsWorkingChanged += new Action<MyCubeBlock>(this.MyProjector_IsWorkingChanged);
      resourceSinkComponent.IsPoweredChanged += new Action(this.PowerReceiver_IsPoweredChanged);
      this.ResourceSink.Update();
      this.m_statsDirty = true;
      this.UpdateText();
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      this.EnabledChanged += new Action<MyTerminalBlock>(this.OnEnabledChanged);
      if (this.BlockDefinition.ScreenAreas == null || this.BlockDefinition.ScreenAreas.Count <= 0)
        return;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
      this.m_multiPanel = new MyMultiTextPanelComponent((MyTerminalBlock) this, this.BlockDefinition.ScreenAreas, builderProjectorBase.TextPanels);
      this.m_multiPanel.Init(new Action<int, int[]>(this.SendAddImagesToSelectionRequest), new Action<int, int[]>(this.SendRemoveSelectedImageRequest), new Action<int, string>(this.ChangeTextRequest), new Action<int, MySerializableSpriteCollection>(this.UpdateSpriteCollection));
    }

    private void InitializeClipboard()
    {
      this.m_clipboard.ResetGridOrientation();
      if (this.m_clipboard.IsActive || this.IsActivating)
        return;
      int num1 = 0;
      foreach (MyObjectBuilder_CubeGrid copiedGrid in this.m_clipboard.CopiedGrids)
        num1 += copiedGrid.CubeBlocks.Count;
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(this.BuiltBy);
      if (identity != null)
      {
        int num2 = num1;
        if (!MySession.Static.CheckLimitsAndNotify(this.BuiltBy, this.BlockDefinition.BlockPairName, num2))
          return;
        identity.BlockLimits.IncreaseBlocksBuilt(this.BlockDefinition.BlockPairName, num2, this.CubeGrid, false);
        this.CubeGrid.BlocksPCU += num2;
      }
      this.IsActivating = true;
      this.m_clipboard.Activate((Action) (() =>
      {
        if (this.m_clipboard.PreviewGrids.Count != 0)
        {
          foreach (MyCubeGrid previewGrid in this.m_clipboard.PreviewGrids)
            previewGrid.Projector = this;
        }
        this.m_forceUpdateProjection = true;
        this.m_shouldUpdateTexts = true;
        this.m_shouldResetBuildable = true;
        this.m_clipboard.ActuallyTestPlacement();
        this.SetRotation((MyGridClipboard) this.m_clipboard, this.m_projectionRotation);
        this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;
        this.IsActivating = false;
      }));
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_ProjectorBase builderCubeBlock = (MyObjectBuilder_ProjectorBase) base.GetObjectBuilderCubeBlock(copy);
      if (this.m_clipboard != null && this.m_clipboard.CopiedGrids != null && (this.m_clipboard.CopiedGrids.Count > 0 && this.m_originalGridBuilders != null))
      {
        if (copy)
        {
          builderCubeBlock.ProjectedGrids = new List<MyObjectBuilder_CubeGrid>();
          foreach (MyObjectBuilder_Base originalGridBuilder in this.m_originalGridBuilders)
          {
            MyObjectBuilder_CubeGrid objectBuilderCubeGrid = (MyObjectBuilder_CubeGrid) originalGridBuilder.Clone();
            Sandbox.Game.Entities.MyEntities.RemapObjectBuilder((MyObjectBuilder_EntityBase) objectBuilderCubeGrid);
            builderCubeBlock.ProjectedGrids.Add(objectBuilderCubeGrid);
          }
        }
        else
          builderCubeBlock.ProjectedGrids = this.m_originalGridBuilders;
        builderCubeBlock.ProjectionOffset = this.m_projectionOffset;
        builderCubeBlock.ProjectionRotation = this.m_projectionRotation;
        builderCubeBlock.KeepProjection = (bool) this.m_keepProjection;
        builderCubeBlock.Scale = this.m_projectionScale;
      }
      else if ((builderCubeBlock.ProjectedGrids == null || builderCubeBlock.ProjectedGrids.Count == 0) && (this.m_savedProjections != null && this.m_savedProjections.Count > 0) && this.CubeGrid.Projector == null)
      {
        builderCubeBlock.ProjectedGrids = this.m_savedProjections;
        builderCubeBlock.ProjectionOffset = this.m_projectionOffset;
        builderCubeBlock.ProjectionRotation = this.m_projectionRotation;
        builderCubeBlock.KeepProjection = (bool) this.m_keepProjection;
      }
      else
        builderCubeBlock.ProjectedGrids = (List<MyObjectBuilder_CubeGrid>) null;
      builderCubeBlock.ShowOnlyBuildable = this.m_showOnlyBuildable;
      builderCubeBlock.InstantBuildingEnabled = (bool) this.m_instantBuildingEnabled;
      builderCubeBlock.MaxNumberOfProjections = (int) this.m_maxNumberOfProjections;
      builderCubeBlock.MaxNumberOfBlocks = (int) this.m_maxNumberOfBlocksPerProjection;
      builderCubeBlock.ProjectionsRemaining = this.m_projectionsRemaining;
      builderCubeBlock.GetOwnershipFromProjector = (bool) this.m_getOwnershipFromProjector;
      builderCubeBlock.TextPanels = this.m_multiPanel != null ? this.m_multiPanel.Serialize() : (List<MySerializedTextPanelData>) null;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    private void UpdateStats()
    {
      this.m_totalBlocks = this.ProjectedGrid.CubeBlocks.Count;
      this.m_remainingArmorBlocks = 0;
      this.m_remainingBlocksPerType.Clear();
      foreach (MySlimBlock cubeBlock1 in this.ProjectedGrid.CubeBlocks)
      {
        MySlimBlock cubeBlock2 = this.CubeGrid.GetCubeBlock(this.CubeGrid.WorldToGridInteger((Vector3D) (Vector3) this.ProjectedGrid.GridIntegerToWorld(cubeBlock1.Position)));
        if (cubeBlock2 == null || cubeBlock1.BlockDefinition.Id != cubeBlock2.BlockDefinition.Id)
        {
          if (cubeBlock1.FatBlock == null)
            ++this.m_remainingArmorBlocks;
          else if (!this.m_remainingBlocksPerType.ContainsKey(cubeBlock1.BlockDefinition))
            this.m_remainingBlocksPerType.Add(cubeBlock1.BlockDefinition, 1);
          else
            this.m_remainingBlocksPerType[cubeBlock1.BlockDefinition]++;
        }
      }
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (!this.m_tierCanProject && this.m_projectionTimer > 0)
      {
        --this.m_projectionTimer;
        if (this.m_projectionTimer == 0)
          this.MyProjector_IsWorkingChanged((MyCubeBlock) this);
      }
      this.ResourceSink.Update();
      if (this.m_removeRequested)
      {
        ++this.m_frameCount;
        if (this.m_frameCount > 10)
        {
          this.UpdateIsWorking();
          if ((!this.IsWorking || !this.TierCanProject) && this.IsProjecting())
            this.RemoveProjection(true);
          this.m_frameCount = 0;
          this.m_removeRequested = false;
        }
      }
      if (!this.m_clipboard.IsActive)
        return;
      this.m_clipboard.Update();
      if (this.m_shouldResetBuildable)
      {
        this.m_shouldResetBuildable = false;
        foreach (MySlimBlock cubeBlock in this.ProjectedGrid.CubeBlocks)
          this.HideCube(cubeBlock);
      }
      if (!this.m_forceUpdateProjection && (!this.m_shouldUpdateProjection || MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastUpdate <= 2000))
        return;
      this.UpdateProjection();
      this.m_shouldUpdateProjection = false;
      this.m_forceUpdateProjection = false;
      this.m_lastUpdate = MySandboxGame.TotalGamePlayTimeInMilliseconds;
    }

    public override void UpdateAfterSimulation100()
    {
      base.UpdateAfterSimulation100();
      if (this.m_clipboard.IsActive && (bool) this.m_instantBuildingEnabled)
        this.m_clipboard.ActuallyTestPlacement();
      if (this.m_multiPanel != null)
        this.m_multiPanel.UpdateAfterSimulation(this.IsWorking);
      if (!this.AllowScaling || this.ProjectedGrid == null)
        return;
      foreach (MyCubeGrid previewGrid in this.m_clipboard.PreviewGrids)
      {
        bool flag = this.IsInRange();
        if (previewGrid.InScene != flag)
        {
          if (flag)
            Sandbox.Game.Entities.MyEntities.Add((MyEntity) previewGrid);
          else
            Sandbox.Game.Entities.MyEntities.Remove((MyEntity) previewGrid);
        }
      }
    }

    private void UpdateProjection()
    {
      if ((bool) this.m_instantBuildingEnabled)
      {
        if (this.ProjectedGrid == null)
          return;
        foreach (MySlimBlock cubeBlock in this.ProjectedGrid.CubeBlocks)
          this.ShowCube(cubeBlock, true);
        this.m_clipboard.HasPreviewBBox = true;
      }
      else
      {
        if (!this.m_updateTask.IsComplete)
          return;
        this.m_hiddenBlock = (MySlimBlock) null;
        if (this.m_clipboard.PreviewGrids.Count == 0)
          return;
        foreach (MyCubeGrid previewGrid in this.m_clipboard.PreviewGrids)
          previewGrid.Render.Transparency = 0.0f;
        this.m_updateTask = MyProjectorBase.MyProjectorUpdateWork.Start(this);
      }
    }

    public override bool SetEmissiveStateWorking()
    {
      if (!this.IsWorking)
        return false;
      return this.IsProjecting() ? this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Alternative, this.Render.RenderObjectIDs[0]) : this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Working, this.Render.RenderObjectIDs[0]);
    }

    private void UpdateSounds()
    {
      this.UpdateIsWorking();
      if (!this.IsWorking)
        return;
      if (this.IsProjecting())
      {
        if (this.m_soundEmitter == null || !(this.m_soundEmitter.SoundId != this.BlockDefinition.PrimarySound.Arcade) || !(this.m_soundEmitter.SoundId != this.BlockDefinition.PrimarySound.Realistic))
          return;
        this.m_soundEmitter.StopSound(false);
        this.m_soundEmitter.PlaySound(this.BlockDefinition.PrimarySound);
      }
      else
      {
        if (this.m_soundEmitter == null || !(this.m_soundEmitter.SoundId != this.BlockDefinition.IdleSound.Arcade) || !(this.m_soundEmitter.SoundId != this.BlockDefinition.IdleSound.Realistic))
          return;
        this.m_soundEmitter.StopSound(false);
        this.m_soundEmitter.PlaySound(this.BlockDefinition.IdleSound);
      }
    }

    protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
    {
      base.UpdateDetailedInfo(detailedInfo);
      if ((bool) this.m_instantBuildingEnabled)
      {
        this.UpdateBaseText(detailedInfo);
        if (!this.m_clipboard.IsActive || this.ProjectedGrid == null)
          return;
        if ((int) this.m_maxNumberOfBlocksPerProjection < 10000)
        {
          detailedInfo.Append("\n");
          detailedInfo.Append("Ship blocks: " + (object) this.ProjectedGrid.BlocksCount + "/" + (object) this.m_maxNumberOfBlocksPerProjection);
        }
        if ((int) this.m_maxNumberOfProjections >= 1000)
          return;
        detailedInfo.Append("\n");
        detailedInfo.Append("Projections remaining: " + (object) this.m_projectionsRemaining + "/" + (object) this.m_maxNumberOfProjections);
      }
      else
      {
        if (this.m_statsDirty && this.m_clipboard.IsActive)
          this.UpdateStats();
        this.m_statsDirty = false;
        this.UpdateBaseText(detailedInfo);
        if (!this.m_clipboard.IsActive || !this.AllowWelding)
          return;
        detailedInfo.Append("\n");
        if (this.m_buildableBlocksCount > 0)
          detailedInfo.Append("\n");
        else
          detailedInfo.Append("WARNING! Projection out of bounds!\n");
        detailedInfo.Append("Build progress: " + (object) (this.m_totalBlocks - this.m_remainingBlocks) + "/" + (object) this.m_totalBlocks);
        if (this.m_remainingArmorBlocks > 0 || this.m_remainingBlocksPerType.Count != 0)
        {
          detailedInfo.Append("\nBlocks remaining:\n");
          detailedInfo.Append("Armor blocks: " + (object) this.m_remainingArmorBlocks);
          foreach (KeyValuePair<MyCubeBlockDefinition, int> keyValuePair in this.m_remainingBlocksPerType)
          {
            detailedInfo.Append("\n");
            detailedInfo.Append(keyValuePair.Key.DisplayNameText + ": " + (object) keyValuePair.Value);
          }
        }
        else
          detailedInfo.Append("\nComplete!");
      }
    }

    private void UpdateText()
    {
      this.SetDetailedInfoDirty();
      if ((bool) this.m_instantBuildingEnabled || !this.m_statsDirty)
        return;
      if (this.m_clipboard.IsActive)
        this.UpdateStats();
      this.RaisePropertiesChanged();
    }

    private void UpdateBaseText(StringBuilder detailedInfo)
    {
      detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
      detailedInfo.Append(this.BlockDefinition.DisplayNameText);
      detailedInfo.Append("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxRequiredInput));
      MyValueFormatter.AppendWorkInBestUnit(this.BlockDefinition.RequiredPowerInput, detailedInfo);
    }

    private void ShowNotification(MyStringId textToDisplay)
    {
      MyHudNotification myHudNotification = new MyHudNotification(textToDisplay, 5000, level: MyNotificationLevel.Important);
      MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification);
    }

    protected override void Closing()
    {
      base.Closing();
      if (this.m_multiPanel != null)
        this.m_multiPanel.SetRender((MyRenderComponentScreenAreas) null);
      if (!this.m_clipboard.IsActive)
        return;
      this.RemoveProjection(false);
    }

    private void CubeGrid_OnGridSplit(MyCubeGrid grid1, MyCubeGrid grid2)
    {
      if (this.m_originalGridBuilders == null || !Sandbox.Game.Multiplayer.Sync.IsServer || (this.MarkedForClose || this.Closed) || !this.AllowWelding)
        return;
      this.Remap();
    }

    public override void OnRegisteredToGridSystems()
    {
      if (this.m_originalGridBuilders == null || !Sandbox.Game.Multiplayer.Sync.IsServer || !this.AllowWelding)
        return;
      this.Remap();
    }

    private void Remap()
    {
      if (this.m_originalGridBuilders == null || this.m_originalGridBuilders.Count <= 0 || !Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      foreach (MyObjectBuilder_EntityBase originalGridBuilder in this.m_originalGridBuilders)
        Sandbox.Game.Entities.MyEntities.RemapObjectBuilder(originalGridBuilder);
      this.SetNewBlueprint(this.m_originalGridBuilders);
    }

    private void PowerReceiver_IsPoweredChanged()
    {
      if (!this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && this.IsProjecting())
        this.RequestRemoveProjection();
      this.UpdateIsWorking();
      this.SetEmissiveStateWorking();
      this.UpdateScreen();
    }

    private float CalculateRequiredPowerInput() => this.BlockDefinition.RequiredPowerInput;

    private void MyProjector_IsWorkingChanged(MyCubeBlock obj)
    {
      if ((!this.IsWorking || !this.TierCanProject) && this.IsProjecting())
      {
        this.RequestRemoveProjection();
      }
      else
      {
        this.SetEmissiveStateWorking();
        if (this.IsWorking && this.TierCanProject && (!this.IsProjecting() && this.m_clipboard.HasCopiedGrids()))
          this.InitializeClipboard();
      }
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    private void ComponentStack_IsFunctionalChanged() => this.ResourceSink.Update();

    public override void OnModelChange()
    {
      base.OnModelChange();
      if (this.m_multiPanel != null)
        this.m_multiPanel.Reset();
      if (this.ResourceSink != null)
        this.UpdateScreen();
      if (!this.CheckIsWorking() || this.m_multiPanel == null)
        return;
      this.Render.UpdateModelProperties();
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      if (this.CubeGrid.Physics != null && this.m_savedProjections != null && this.m_savedProjections.Count > 0)
      {
        MyObjectBuilder_CubeGrid[] grids = new MyObjectBuilder_CubeGrid[this.m_savedProjections.Count];
        for (int index = 0; index < this.m_savedProjections.Count; ++index)
        {
          MyObjectBuilder_CubeGrid gridBuilder = (MyObjectBuilder_CubeGrid) this.m_savedProjections[index].Clone();
          Sandbox.Game.Entities.MyEntities.RemapObjectBuilder((MyObjectBuilder_EntityBase) gridBuilder);
          this.m_clipboard.ProcessCubeGrid(gridBuilder);
          grids[index] = gridBuilder;
        }
        this.m_clipboard.SetGridFromBuilders(grids, Vector3.Zero, 0.0f);
        this.m_originalGridBuilders = this.m_savedProjections;
        this.m_savedProjections = (List<MyObjectBuilder_CubeGrid>) null;
        if (this.IsWorking)
          this.InitializeClipboard();
        this.RequestRemoveProjection();
      }
      this.UpdateSounds();
      this.SetEmissiveStateWorking();
      this.UpdateScreen();
    }

    private void previewGrid_OnBlockAdded(MySlimBlock obj)
    {
      this.m_shouldUpdateProjection = true;
      this.m_shouldUpdateTexts = true;
      if (this.m_originalGridBuilders == null || !this.IsProjecting())
        return;
      Vector3I gridInteger = this.ProjectedGrid.WorldToGridInteger(this.CubeGrid.GridIntegerToWorld(obj.Position));
      if (!(obj.FatBlock is MyTerminalBlock fatBlock))
        return;
      foreach (MyObjectBuilder_CubeGrid originalGridBuilder in this.m_originalGridBuilders)
      {
        foreach (MyObjectBuilder_BlockGroup blockGroup in originalGridBuilder.BlockGroups)
        {
          foreach (Vector3I block in blockGroup.Blocks)
          {
            if (gridInteger == block)
            {
              MyBlockGroup group = new MyBlockGroup()
              {
                Name = new StringBuilder(blockGroup.Name)
              };
              group.Blocks.Add(fatBlock);
              this.CubeGrid.AddGroup(group);
            }
          }
        }
      }
      fatBlock.CheckConnectionChanged += new Action<MyCubeBlock>(this.TerminalBlockOnCheckConnectionChanged);
    }

    private void TerminalBlockOnCheckConnectionChanged(MyCubeBlock myCubeBlock)
    {
      this.m_forceUpdateProjection = true;
      this.m_shouldUpdateTexts = true;
    }

    private void previewGrid_OnBlockRemoved(MySlimBlock obj)
    {
      this.m_shouldUpdateProjection = true;
      this.m_shouldUpdateTexts = true;
      if (obj == null || obj.FatBlock == null)
        return;
      obj.FatBlock.CheckConnectionChanged -= new Action<MyCubeBlock>(this.TerminalBlockOnCheckConnectionChanged);
    }

    [Event(null, 1458)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void OnSpawnProjection()
    {
      if (!this.CanSpawnProjection())
        return;
      MyObjectBuilder_CubeGrid[] grids = new MyObjectBuilder_CubeGrid[this.m_originalGridBuilders.Count];
      for (int index = 0; index < this.m_originalGridBuilders.Count; ++index)
      {
        MyObjectBuilder_CubeGrid objectBuilderCubeGrid = (MyObjectBuilder_CubeGrid) this.m_originalGridBuilders[index].Clone();
        Sandbox.Game.Entities.MyEntities.RemapObjectBuilder((MyObjectBuilder_EntityBase) objectBuilderCubeGrid);
        if ((bool) this.m_getOwnershipFromProjector)
        {
          foreach (MyObjectBuilder_CubeBlock cubeBlock in objectBuilderCubeGrid.CubeBlocks)
          {
            cubeBlock.Owner = this.OwnerId;
            cubeBlock.ShareMode = this.IDModule.ShareMode;
          }
        }
        grids[index] = objectBuilderCubeGrid;
      }
      this.m_spawnClipboard.SetGridFromBuilders(grids, Vector3.Zero, 0.0f);
      this.m_spawnClipboard.ResetGridOrientation();
      if (!this.m_spawnClipboard.IsActive)
        this.m_spawnClipboard.Activate((Action) null);
      this.SetRotation((MyGridClipboard) this.m_spawnClipboard, this.m_projectionRotation);
      this.m_spawnClipboard.Update();
      if (this.m_spawnClipboard.ActuallyTestPlacement() && this.m_spawnClipboard.PasteGrid())
        this.OnConfirmSpawnProjection();
      this.m_spawnClipboard.Deactivate();
      this.m_spawnClipboard.Clear();
    }

    [Event(null, 1500)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void OnConfirmSpawnProjection()
    {
      if ((int) this.m_maxNumberOfProjections < 1000)
        --this.m_projectionsRemaining;
      if (!(bool) this.m_keepProjection)
        this.RemoveProjection(false);
      this.UpdateText();
      this.RaisePropertiesChanged();
    }

    private void m_instantBuildingEnabled_ValueChanged(SyncBase obj)
    {
      this.m_shouldUpdateProjection = true;
      if ((bool) this.m_instantBuildingEnabled)
        this.m_projectionsRemaining = (int) this.m_maxNumberOfProjections;
      this.RaisePropertiesChanged();
    }

    private void m_maxNumberOfProjections_ValueChanged(SyncBase obj)
    {
      this.m_projectionsRemaining = (int) this.m_maxNumberOfProjections;
      this.RaisePropertiesChanged();
    }

    private void m_maxNumberOfBlocksPerProjection_ValueChanged(SyncBase obj) => this.RaisePropertiesChanged();

    private void m_getOwnershipFromProjector_ValueChanged(SyncBase obj) => this.RaisePropertiesChanged();

    private void OnEnabledChanged(MyTerminalBlock myTerminalBlock)
    {
      if (!this.Enabled || !this.TierCanProject)
        this.RemoveProjection(true);
      if (this.m_multiPanel == null || this.m_multiPanel.SurfaceCount <= 0)
        return;
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      if (this.m_multiPanel != null && this.m_multiPanel.SurfaceCount > 0)
      {
        this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
        this.m_multiPanel.AddToScene();
      }
      if (this.CubeGrid == null)
        return;
      this.CubeGrid.OnBlockAdded += new Action<MySlimBlock>(this.previewGrid_OnBlockAdded);
      this.CubeGrid.OnBlockRemoved += new Action<MySlimBlock>(this.previewGrid_OnBlockRemoved);
      this.CubeGrid.OnGridSplit += new Action<MyCubeGrid, MyCubeGrid>(this.CubeGrid_OnGridSplit);
      foreach (MyCubeBlock fatBlock in this.CubeGrid.GetFatBlocks())
      {
        if (fatBlock is Sandbox.ModAPI.IMyTerminalBlock)
          fatBlock.CheckConnectionChanged += new Action<MyCubeBlock>(this.TerminalBlockOnCheckConnectionChanged);
      }
      this.UpdateProjectionVisibility();
    }

    public override void OnRemovedFromScene(object source)
    {
      base.OnRemovedFromScene(source);
      if (this.CubeGrid == null)
        return;
      this.CubeGrid.OnBlockAdded -= new Action<MySlimBlock>(this.previewGrid_OnBlockAdded);
      this.CubeGrid.OnBlockRemoved -= new Action<MySlimBlock>(this.previewGrid_OnBlockRemoved);
      this.CubeGrid.OnGridSplit -= new Action<MyCubeGrid, MyCubeGrid>(this.CubeGrid_OnGridSplit);
      foreach (MyCubeBlock fatBlock in this.CubeGrid.GetFatBlocks())
      {
        if (fatBlock is Sandbox.ModAPI.IMyTerminalBlock)
          fatBlock.CheckConnectionChanged -= new Action<MyCubeBlock>(this.TerminalBlockOnCheckConnectionChanged);
      }
    }

    public void UpdateScreen() => this.m_multiPanel?.UpdateScreen(this.IsWorking);

    public bool IsInRange()
    {
      MyCamera mainCamera = MySector.MainCamera;
      return mainCamera != null && Vector3D.DistanceSquared(this.PositionComp.WorldVolume.Center, mainCamera.Position) < 2500.0;
    }

    private bool CanBuild(MySlimBlock cubeBlock) => this.CanBuild(cubeBlock, false) == BuildCheckResult.OK;

    public BuildCheckResult CanBuild(
      MySlimBlock projectedBlock,
      bool checkHavokIntersections)
    {
      if (!this.AllowWelding)
        return BuildCheckResult.NotWeldable;
      MyBlockOrientation orientation = projectedBlock.Orientation;
      Quaternion result1;
      orientation.GetQuaternion(out result1);
      Quaternion result2 = Quaternion.Identity;
      this.Orientation.GetQuaternion(out result2);
      result1 = Quaternion.Multiply(Quaternion.Multiply(result2, this.ProjectionRotationQuaternion), result1);
      Vector3I gridInteger1 = this.CubeGrid.WorldToGridInteger(projectedBlock.CubeGrid.GridIntegerToWorld(projectedBlock.Min));
      Vector3I gridInteger2 = this.CubeGrid.WorldToGridInteger(projectedBlock.CubeGrid.GridIntegerToWorld(projectedBlock.Max));
      Vector3I gridInteger3 = this.CubeGrid.WorldToGridInteger(projectedBlock.CubeGrid.GridIntegerToWorld(projectedBlock.Position));
      Vector3I vector3I1 = new Vector3I(Math.Min(gridInteger1.X, gridInteger2.X), Math.Min(gridInteger1.Y, gridInteger2.Y), Math.Min(gridInteger1.Z, gridInteger2.Z));
      Vector3I vector3I2 = new Vector3I(Math.Max(gridInteger1.X, gridInteger2.X), Math.Max(gridInteger1.Y, gridInteger2.Y), Math.Max(gridInteger1.Z, gridInteger2.Z));
      Vector3I min = vector3I1;
      Vector3I max = vector3I2;
      if (!this.CubeGrid.CanAddCubes(min, max))
        return BuildCheckResult.IntersectedWithGrid;
      MyGridPlacementSettings settings = new MyGridPlacementSettings();
      settings.SnapMode = SnapMode.OneFreeAxis;
      MyCubeBlockDefinition.MountPoint[] modelMountPoints = projectedBlock.BlockDefinition.GetBuildProgressModelMountPoints(1f);
      if (!MyCubeGrid.CheckConnectivity((IMyGridConnectivityTest) this.CubeGrid, projectedBlock.BlockDefinition, modelMountPoints, ref result1, ref gridInteger3))
        return BuildCheckResult.NotConnected;
      if (this.CubeGrid.GetCubeBlock(gridInteger3) != null)
        return BuildCheckResult.AlreadyBuilt;
      return checkHavokIntersections && !MyCubeGrid.TestPlacementAreaCube(this.CubeGrid, ref settings, min, max, orientation, projectedBlock.BlockDefinition, ignoredEntity: ((MyEntity) this.CubeGrid), isProjected: true) ? BuildCheckResult.IntersectedWithSomethingElse : BuildCheckResult.OK;
    }

    public void Build(
      MySlimBlock cubeBlock,
      long owner,
      long builder,
      bool requestInstant = true,
      long builtBy = 0)
    {
      ulong steamId = MySession.Static.Players.TryGetSteamId(owner);
      if (!this.AllowWelding || !MySession.Static.GetComponent<MySessionComponentDLC>().HasDefinitionDLC((MyDefinitionBase) cubeBlock.BlockDefinition, steamId))
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProjectorBase, Vector3I, long, long, bool, long>(this, (Func<MyProjectorBase, Action<Vector3I, long, long, bool, long>>) (x => new Action<Vector3I, long, long, bool, long>(x.BuildInternal)), cubeBlock.Position, owner, builder, requestInstant, builtBy);
    }

    [Event(null, 1704)]
    [Reliable]
    [Server]
    private void BuildInternal(
      Vector3I cubeBlockPosition,
      long owner,
      long builder,
      bool requestInstant = true,
      long builtBy = 0)
    {
      ulong steamId = MySession.Static.Players.TryGetSteamId(owner);
      MySlimBlock cubeBlock1 = this.ProjectedGrid.GetCubeBlock(cubeBlockPosition);
      if (cubeBlock1 == null || !this.AllowWelding || !MySession.Static.GetComponent<MySessionComponentDLC>().HasDefinitionDLC((MyDefinitionBase) cubeBlock1.BlockDefinition, steamId))
      {
        (Sandbox.Engine.Multiplayer.MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, false, (string) null, true);
      }
      else
      {
        Quaternion result1 = Quaternion.Identity;
        MyBlockOrientation orientation = cubeBlock1.Orientation;
        Quaternion result2 = Quaternion.Identity;
        this.Orientation.GetQuaternion(out result2);
        orientation.GetQuaternion(out result1);
        result1 = Quaternion.Multiply(this.ProjectionRotationQuaternion, result1);
        result1 = Quaternion.Multiply(result2, result1);
        MyCubeGrid cubeGrid1 = this.CubeGrid;
        MyCubeGrid cubeGrid2 = cubeBlock1.CubeGrid;
        Vector3I gridCoords1 = cubeBlock1.FatBlock != null ? cubeBlock1.FatBlock.Min : cubeBlock1.Position;
        Vector3I gridCoords2 = cubeBlock1.FatBlock != null ? cubeBlock1.FatBlock.Max : cubeBlock1.Position;
        Vector3I gridInteger1 = cubeGrid1.WorldToGridInteger(cubeGrid2.GridIntegerToWorld(gridCoords1));
        Vector3I gridInteger2 = cubeGrid1.WorldToGridInteger(cubeGrid2.GridIntegerToWorld(gridCoords2));
        Vector3I gridInteger3 = cubeGrid1.WorldToGridInteger(cubeGrid2.GridIntegerToWorld(cubeBlock1.Position));
        Vector3I min = new Vector3I(Math.Min(gridInteger1.X, gridInteger2.X), Math.Min(gridInteger1.Y, gridInteger2.Y), Math.Min(gridInteger1.Z, gridInteger2.Z));
        Vector3I max = new Vector3I(Math.Max(gridInteger1.X, gridInteger2.X), Math.Max(gridInteger1.Y, gridInteger2.Y), Math.Max(gridInteger1.Z, gridInteger2.Z));
        MyCubeGrid.MyBlockLocation location = new MyCubeGrid.MyBlockLocation(cubeBlock1.BlockDefinition.Id, min, max, gridInteger3, result1, 0L, owner);
        MyObjectBuilder_CubeBlock builderCubeBlock = (MyObjectBuilder_CubeBlock) null;
        MyObjectBuilder_CubeGrid objectBuilderCubeGrid = this.m_originalGridBuilders == null || this.m_originalGridBuilders.Count <= 0 ? (MyObjectBuilder_CubeGrid) null : this.m_originalGridBuilders[0];
        if (objectBuilderCubeGrid != null)
        {
          foreach (MyObjectBuilder_CubeBlock cubeBlock2 in objectBuilderCubeGrid.CubeBlocks)
          {
            if ((Vector3I) cubeBlock2.Min == gridCoords1 && cubeBlock2.GetId() == cubeBlock1.BlockDefinition.Id)
            {
              builderCubeBlock = (MyObjectBuilder_CubeBlock) cubeBlock2.Clone();
              if (MyDefinitionManagerBase.Static != null && builderCubeBlock is MyObjectBuilder_BatteryBlock)
              {
                MyBatteryBlockDefinition cubeBlockDefinition = (MyBatteryBlockDefinition) MyDefinitionManager.Static.GetCubeBlockDefinition(builderCubeBlock);
                ((MyObjectBuilder_BatteryBlock) builderCubeBlock).CurrentStoredPower = cubeBlockDefinition.InitialStoredPowerRatio * cubeBlockDefinition.MaxStoredPower;
              }
            }
          }
        }
        if (builderCubeBlock == null)
        {
          builderCubeBlock = cubeBlock1.GetObjectBuilder(false);
          location.EntityId = MyEntityIdentifier.AllocateId();
        }
        builderCubeBlock.ConstructionInventory = (MyObjectBuilder_Inventory) null;
        builderCubeBlock.BuiltBy = builtBy;
        bool instantBuild = requestInstant && MySession.Static.CreativeToolsEnabled(MyEventContext.Current.Sender.Value);
        MySessionComponentGameInventory component = MySession.Static.GetComponent<MySessionComponentGameInventory>();
        MyStringHash skinId = component != null ? component.ValidateArmor(cubeBlock1.SkinSubtypeId, steamId) : MyStringHash.NullOrEmpty;
        MyCubeGrid.MyBlockVisuals visuals = new MyCubeGrid.MyBlockVisuals(cubeBlock1.ColorMaskHSV.PackHSVToUint(), skinId);
        cubeGrid1.BuildBlockRequestInternal(visuals, location, builderCubeBlock, builder, instantBuild, owner, MyEventContext.Current.IsLocallyInvoked ? steamId : MyEventContext.Current.Sender.Value, true);
        this.HideCube(cubeBlock1);
        this.m_projectionTimer = MyProjectorBase.PROJECTION_TIME_IN_FRAMES;
      }
    }

    private void SendRemoveSelectedImageRequest(int panelIndex, int[] selection) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProjectorBase, int, int[]>(this, (Func<MyProjectorBase, Action<int, int[]>>) (x => new Action<int, int[]>(x.OnRemoveSelectedImageRequest)), panelIndex, selection);

    [Event(null, 1789)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnRemoveSelectedImageRequest(int panelIndex, int[] selection) => this.m_multiPanel?.RemoveItems(panelIndex, selection);

    private void SendAddImagesToSelectionRequest(int panelIndex, int[] selection) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProjectorBase, int, int[]>(this, (Func<MyProjectorBase, Action<int, int[]>>) (x => new Action<int, int[]>(x.OnSelectImageRequest)), panelIndex, selection);

    [Event(null, 1800)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnSelectImageRequest(int panelIndex, int[] selection) => this.m_multiPanel?.SelectItems(panelIndex, selection);

    private void ChangeTextRequest(int panelIndex, string text) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProjectorBase, int, string>(this, (Func<MyProjectorBase, Action<int, string>>) (x => new Action<int, string>(x.OnChangeTextRequest)), panelIndex, text);

    [Event(null, 1811)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnChangeTextRequest(int panelIndex, [Nullable] string text) => this.m_multiPanel?.ChangeText(panelIndex, text);

    private void UpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProjectorBase, int, MySerializableSpriteCollection>(this, (Func<MyProjectorBase, Action<int, MySerializableSpriteCollection>>) (x => new Action<int, MySerializableSpriteCollection>(x.OnUpdateSpriteCollection)), panelIndex, sprites);
    }

    [Event(null, 1825)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    [DistanceRadius(32f)]
    private void OnUpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites) => this.m_multiPanel?.UpdateSpriteCollection(panelIndex, sprites);

    internal void SetNewBlueprint(List<MyObjectBuilder_CubeGrid> gridBuilders)
    {
      this.m_originalGridBuilders = gridBuilders;
      List<MyObjectBuilder_CubeGrid> originalGridBuilders = this.m_originalGridBuilders;
      MyCubeGrid.CleanCubeGridsBeforeSetupForProjector(originalGridBuilders);
      this.m_clipboard.SetGridFromBuilders(originalGridBuilders.ToArray(), Vector3.Zero, 0.0f, false);
      BoundingBox boundingBox = BoundingBox.CreateInvalid();
      MyCubeSize gridSize = MyCubeSize.Small;
      foreach (MyObjectBuilder_CubeGrid grid in originalGridBuilders)
      {
        boundingBox = boundingBox.Include(grid.CalculateBoundingBox());
        if (grid.GridSizeEnum == MyCubeSize.Large)
          gridSize = grid.GridSizeEnum;
      }
      if ((bool) this.m_instantBuildingEnabled)
      {
        this.ResetRotation();
        this.m_projectionOffset.Y = Math.Abs((int) ((double) boundingBox.Min.Y / (double) MyDefinitionManager.Static.GetCubeSize(gridSize))) + 2;
      }
      if (!this.Enabled || !this.IsWorking)
        return;
      if (this.BlockDefinition.AllowScaling)
        this.m_projectionScale = MathHelper.Clamp(this.CubeGrid.GridSize / boundingBox.Size.Max(), 0.02f, 1f);
      this.InitializeClipboard();
    }

    internal void SetNewOffset(
      Vector3I positionOffset,
      Vector3I rotationOffset,
      bool onlyCanBuildBlock)
    {
      this.m_clipboard.ResetGridOrientation();
      this.m_projectionOffset = positionOffset;
      this.m_projectionRotation = rotationOffset;
      this.m_showOnlyBuildable = onlyCanBuildBlock;
      this.SetRotation((MyGridClipboard) this.m_clipboard, this.m_projectionRotation);
    }

    private void SendNewBlueprint(List<MyObjectBuilder_CubeGrid> projectedGrids)
    {
      this.SetNewBlueprint(projectedGrids);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProjectorBase, List<MyObjectBuilder_CubeGrid>>(this, (Func<MyProjectorBase, Action<List<MyObjectBuilder_CubeGrid>>>) (x => new Action<List<MyObjectBuilder_CubeGrid>>(x.OnNewBlueprintSuccess)), projectedGrids);
    }

    [Event(null, 1889)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [BroadcastExcept]
    private void OnNewBlueprintSuccess(List<MyObjectBuilder_CubeGrid> projectedGrids)
    {
      if (MyEventContext.Current.IsLocallyInvoked)
        return;
      if (!MySession.Static.IsUserScripter(MyEventContext.Current.Sender.Value))
      {
        bool flag = false;
        foreach (MyObjectBuilder_CubeGrid projectedGrid in projectedGrids)
          this.RemoveScriptsFromProjection(ref projectedGrid);
        if (flag)
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProjectorBase>(this, (Func<MyProjectorBase, Action>) (x => new Action(x.ShowScriptRemoveMessage)), MyEventContext.Current.Sender);
      }
      this.SetNewBlueprint(projectedGrids);
    }

    private bool RemoveScriptsFromProjection(ref MyObjectBuilder_CubeGrid grid)
    {
      bool flag = false;
      foreach (MyObjectBuilder_CubeBlock cubeBlock in grid.CubeBlocks)
      {
        if (cubeBlock is MyObjectBuilder_MyProgrammableBlock programmableBlock && programmableBlock.Program != null)
        {
          programmableBlock.Program = (string) null;
          flag = true;
        }
      }
      return flag;
    }

    [Event(null, 1928)]
    [Reliable]
    [Client]
    private void ShowScriptRemoveMessage() => MyHud.Notifications.Add((MyHudNotificationBase) new MyHudNotification(MySpaceTexts.Notification_BlueprintScriptRemoved, 5000, "Red"));

    public void SendNewOffset(
      Vector3I positionOffset,
      Vector3I rotationOffset,
      float scale,
      bool showOnlyBuildable)
    {
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProjectorBase, Vector3I, Vector3I, float, bool>(this, (Func<MyProjectorBase, Action<Vector3I, Vector3I, float, bool>>) (x => new Action<Vector3I, Vector3I, float, bool>(x.OnOffsetChangedSuccess)), positionOffset, rotationOffset, scale, showOnlyBuildable);
    }

    [Event(null, 1940)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnOffsetChangedSuccess(
      Vector3I positionOffset,
      Vector3I rotationOffset,
      float scale,
      bool showOnlyBuildable)
    {
      this.m_projectionScale = scale;
      this.SetNewOffset(positionOffset, rotationOffset, showOnlyBuildable);
      this.m_shouldUpdateProjection = true;
    }

    public void SendRemoveProjection() => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProjectorBase>(this, (Func<MyProjectorBase, Action>) (x => new Action(x.OnRemoveProjectionRequest)));

    [Event(null, 1953)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnRemoveProjectionRequest() => this.RemoveProjection(false);

    private void SendSpawnProjection() => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProjectorBase>(this, (Func<MyProjectorBase, Action>) (x => new Action(x.OnSpawnProjection)));

    private void SendConfirmSpawnProjection() => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProjectorBase>(this, (Func<MyProjectorBase, Action>) (x => new Action(x.OnConfirmSpawnProjection)));

    protected override void TiersChanged() => this.UpdateProjectionVisibility();

    private void UpdateProjectionVisibility()
    {
      switch (this.CubeGrid.PlayerPresenceTier)
      {
        case MyUpdateTiersPlayerPresence.Normal:
          this.TierCanProject = true;
          break;
        case MyUpdateTiersPlayerPresence.Tier1:
        case MyUpdateTiersPlayerPresence.Tier2:
          this.TierCanProject = false;
          break;
      }
    }

    VRage.Game.ModAPI.IMyCubeGrid Sandbox.ModAPI.IMyProjector.ProjectedGrid => (VRage.Game.ModAPI.IMyCubeGrid) this.ProjectedGrid;

    void Sandbox.ModAPI.IMyProjector.SetProjectedGrid(MyObjectBuilder_CubeGrid grid)
    {
      if (grid != null)
      {
        MyObjectBuilder_CubeGrid gridBuilder = (MyObjectBuilder_CubeGrid) null;
        this.m_originalGridBuilders = (List<MyObjectBuilder_CubeGrid>) null;
        Parallel.Start((Action) (() =>
        {
          gridBuilder = (MyObjectBuilder_CubeGrid) grid.Clone();
          this.m_clipboard.ProcessCubeGrid(gridBuilder);
          Sandbox.Game.Entities.MyEntities.RemapObjectBuilder((MyObjectBuilder_EntityBase) gridBuilder);
        }), (Action) (() =>
        {
          if (gridBuilder == null || this.m_originalGridBuilders != null)
            return;
          this.m_originalGridBuilders = new List<MyObjectBuilder_CubeGrid>(1);
          this.m_originalGridBuilders.Add(gridBuilder);
          this.SendNewBlueprint(this.m_originalGridBuilders);
        }));
      }
      else
        this.SendRemoveProjection();
    }

    BuildCheckResult Sandbox.ModAPI.IMyProjector.CanBuild(
      VRage.Game.ModAPI.IMySlimBlock projectedBlock,
      bool checkHavokIntersections)
    {
      return this.CanBuild((MySlimBlock) projectedBlock, checkHavokIntersections);
    }

    void Sandbox.ModAPI.IMyProjector.Build(
      VRage.Game.ModAPI.IMySlimBlock cubeBlock,
      long owner,
      long builder,
      bool requestInstant,
      long builtBy = 0)
    {
      this.Build((MySlimBlock) cubeBlock, owner, builder, requestInstant, builtBy);
    }

    Vector3I Sandbox.ModAPI.Ingame.IMyProjector.ProjectionOffset
    {
      get => this.m_projectionOffset;
      set => this.m_projectionOffset = value;
    }

    Vector3I Sandbox.ModAPI.Ingame.IMyProjector.ProjectionRotation
    {
      get => this.m_projectionRotation;
      set => this.m_projectionRotation = value;
    }

    void Sandbox.ModAPI.Ingame.IMyProjector.UpdateOffsetAndRotation() => this.OnOffsetsChanged();

    [Obsolete("Use ProjectionOffset vector instead.")]
    int Sandbox.ModAPI.Ingame.IMyProjector.ProjectionOffsetX => this.m_projectionOffset.X;

    [Obsolete("Use ProjectionOffset vector instead.")]
    int Sandbox.ModAPI.Ingame.IMyProjector.ProjectionOffsetY => this.m_projectionOffset.Y;

    [Obsolete("Use ProjectionOffset vector instead.")]
    int Sandbox.ModAPI.Ingame.IMyProjector.ProjectionOffsetZ => this.m_projectionOffset.Z;

    [Obsolete("Use ProjectionRotation vector instead.")]
    int Sandbox.ModAPI.Ingame.IMyProjector.ProjectionRotX => this.m_projectionRotation.X * 90;

    [Obsolete("Use ProjectionRotation vector instead.")]
    int Sandbox.ModAPI.Ingame.IMyProjector.ProjectionRotY => this.m_projectionRotation.Y * 90;

    [Obsolete("Use ProjectionRotation vector instead.")]
    int Sandbox.ModAPI.Ingame.IMyProjector.ProjectionRotZ => this.m_projectionRotation.Z * 90;

    bool Sandbox.ModAPI.Ingame.IMyProjector.IsProjecting => this.IsProjecting();

    int Sandbox.ModAPI.Ingame.IMyProjector.RemainingBlocks => this.m_remainingBlocks;

    int Sandbox.ModAPI.Ingame.IMyProjector.TotalBlocks => this.m_totalBlocks;

    int Sandbox.ModAPI.Ingame.IMyProjector.RemainingArmorBlocks => this.m_remainingArmorBlocks;

    int Sandbox.ModAPI.Ingame.IMyProjector.BuildableBlocksCount => this.m_buildableBlocksCount;

    Dictionary<MyDefinitionBase, int> Sandbox.ModAPI.Ingame.IMyProjector.RemainingBlocksPerType
    {
      get
      {
        Dictionary<MyDefinitionBase, int> dictionary = new Dictionary<MyDefinitionBase, int>();
        foreach (KeyValuePair<MyCubeBlockDefinition, int> keyValuePair in this.m_remainingBlocksPerType)
          dictionary.Add((MyDefinitionBase) keyValuePair.Key, keyValuePair.Value);
        return dictionary;
      }
    }

    bool Sandbox.ModAPI.IMyProjector.LoadRandomBlueprint(string searchPattern)
    {
      bool flag = false;
      string[] files = Directory.GetFiles(Path.Combine(MyFileSystem.ContentPath, "Data", "Blueprints"), searchPattern);
      if (files.Length != 0)
      {
        int index = MyRandom.Instance.Next() % files.Length;
        flag = this.LoadBlueprint(files[index]);
      }
      return flag;
    }

    bool Sandbox.ModAPI.IMyProjector.LoadBlueprint(string path) => this.LoadBlueprint(path);

    private bool LoadBlueprint(string path)
    {
      bool flag = false;
      MyObjectBuilder_Definitions prefab = MyBlueprintUtils.LoadPrefab(path);
      if (prefab != null)
        flag = MyGuiBlueprintScreen.CopyBlueprintPrefabToClipboard(prefab, (MyGridClipboard) this.m_clipboard);
      this.OnBlueprintScreen_Closed((MyGuiScreenBase) null);
      return flag;
    }

    bool Sandbox.ModAPI.Ingame.IMyProjector.ShowOnlyBuildable
    {
      get => this.m_showOnlyBuildable;
      set => this.m_showOnlyBuildable = value;
    }

    int Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider.SurfaceCount => this.m_multiPanel == null ? 0 : this.m_multiPanel.SurfaceCount;

    Sandbox.ModAPI.Ingame.IMyTextSurface Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider.GetSurface(
      int index)
    {
      return this.m_multiPanel == null ? (Sandbox.ModAPI.Ingame.IMyTextSurface) null : this.m_multiPanel.GetSurface(index);
    }

    private class MyProjectorUpdateWork : IWork
    {
      private static readonly MyDynamicObjectPool<MyProjectorBase.MyProjectorUpdateWork> InstancePool = new MyDynamicObjectPool<MyProjectorBase.MyProjectorUpdateWork>(8);
      private MyProjectorBase m_projector;
      private MyCubeGrid m_grid;
      private HashSet<MySlimBlock> m_visibleBlocks = new HashSet<MySlimBlock>();
      private HashSet<MySlimBlock> m_buildableBlocks = new HashSet<MySlimBlock>();
      private HashSet<MySlimBlock> m_hiddenBlocks = new HashSet<MySlimBlock>();
      private int m_remainingBlocks;
      private int m_buildableBlocksCount;

      public WorkOptions Options => Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.Block, "Projector");

      public static Task Start(MyProjectorBase projector)
      {
        MyProjectorBase.MyProjectorUpdateWork projectorUpdateWork = MyProjectorBase.MyProjectorUpdateWork.InstancePool.Allocate();
        projectorUpdateWork.m_projector = projector;
        projectorUpdateWork.m_grid = projector.ProjectedGrid;
        return Parallel.Start((IWork) projectorUpdateWork, new Action(projectorUpdateWork.OnComplete));
      }

      private void OnComplete()
      {
        if (this.m_projector.Closed || this.m_projector.CubeGrid.Closed || this.m_projector.ProjectedGrid == null)
          return;
        if (!this.m_projector.AllowWelding)
        {
          foreach (MyCubeGrid previewGrid in this.m_projector.m_clipboard.PreviewGrids)
          {
            foreach (MySlimBlock cubeBlock in previewGrid.CubeBlocks)
            {
              if (this.m_projector.Enabled)
                this.m_projector.ShowCube(cubeBlock, false);
              else
                this.m_projector.HideCube(cubeBlock);
            }
          }
        }
        else
        {
          foreach (MySlimBlock visibleBlock in this.m_visibleBlocks)
          {
            if (!this.m_projector.m_visibleBlocks.Contains(visibleBlock))
            {
              if (this.m_projector.Enabled)
                this.m_projector.ShowCube(visibleBlock, false);
              else
                this.m_projector.HideCube(visibleBlock);
            }
          }
        }
        MyUtils.Swap<HashSet<MySlimBlock>>(ref this.m_visibleBlocks, ref this.m_projector.m_visibleBlocks);
        if (this.m_projector.BlockDefinition.AllowWelding)
        {
          foreach (MySlimBlock buildableBlock in this.m_buildableBlocks)
          {
            if (!this.m_projector.m_buildableBlocks.Contains(buildableBlock))
            {
              if (this.m_projector.Enabled)
                this.m_projector.ShowCube(buildableBlock, true);
              else
                this.m_projector.HideCube(buildableBlock);
            }
          }
        }
        MyUtils.Swap<HashSet<MySlimBlock>>(ref this.m_buildableBlocks, ref this.m_projector.m_buildableBlocks);
        foreach (MySlimBlock hiddenBlock in this.m_hiddenBlocks)
        {
          if (!this.m_projector.m_hiddenBlocks.Contains(hiddenBlock))
            this.m_projector.HideCube(hiddenBlock);
        }
        MyUtils.Swap<HashSet<MySlimBlock>>(ref this.m_hiddenBlocks, ref this.m_projector.m_hiddenBlocks);
        this.m_projector.m_remainingBlocks = this.m_remainingBlocks;
        this.m_projector.m_buildableBlocksCount = this.m_buildableBlocksCount;
        if (this.m_projector.m_remainingBlocks == 0 && !(bool) this.m_projector.m_keepProjection)
        {
          this.m_projector.RemoveProjection((bool) this.m_projector.m_keepProjection);
        }
        else
        {
          this.m_projector.UpdateSounds();
          this.m_projector.SetEmissiveStateWorking();
        }
        this.m_projector.m_statsDirty = true;
        if (this.m_projector.m_shouldUpdateTexts)
        {
          this.m_projector.UpdateText();
          this.m_projector.m_shouldUpdateTexts = false;
        }
        this.m_projector.m_clipboard.HasPreviewBBox = false;
        this.m_projector = (MyProjectorBase) null;
        this.m_visibleBlocks.Clear();
        this.m_buildableBlocks.Clear();
        this.m_hiddenBlocks.Clear();
        MyProjectorBase.MyProjectorUpdateWork.InstancePool.Deallocate(this);
      }

      public void DoWork(WorkData workData = null)
      {
        this.m_remainingBlocks = this.m_grid.BlocksCount;
        this.m_buildableBlocksCount = 0;
        foreach (MySlimBlock cubeBlock1 in this.m_grid.CubeBlocks)
        {
          MySlimBlock cubeBlock2 = this.m_projector.CubeGrid.GetCubeBlock(this.m_projector.CubeGrid.WorldToGridInteger(this.m_grid.GridIntegerToWorld(cubeBlock1.Position)));
          if (cubeBlock2 != null && cubeBlock1.BlockDefinition.Id == cubeBlock2.BlockDefinition.Id)
          {
            this.m_hiddenBlocks.Add(cubeBlock1);
            --this.m_remainingBlocks;
          }
          else if (this.m_projector.CanBuild(cubeBlock1))
          {
            this.m_buildableBlocks.Add(cubeBlock1);
            ++this.m_buildableBlocksCount;
          }
          else if (this.m_projector.AllowWelding && this.m_projector.m_showOnlyBuildable)
            this.m_hiddenBlocks.Add(cubeBlock1);
          else
            this.m_visibleBlocks.Add(cubeBlock1);
        }
      }
    }

    protected sealed class OnChangeOpenRequest\u003C\u003ESystem_Boolean\u0023System_Boolean\u0023System_UInt64\u0023System_Boolean : ICallSite<MyProjectorBase, bool, bool, ulong, bool, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProjectorBase @this,
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

    protected sealed class OnChangeOpenSuccess\u003C\u003ESystem_Boolean\u0023System_Boolean\u0023System_UInt64\u0023System_Boolean : ICallSite<MyProjectorBase, bool, bool, ulong, bool, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProjectorBase @this,
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

    protected sealed class OnChangeDescription\u003C\u003ESystem_String\u0023System_Boolean : ICallSite<MyProjectorBase, string, bool, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProjectorBase @this,
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

    protected sealed class OnSpawnProjection\u003C\u003E : ICallSite<MyProjectorBase, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProjectorBase @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnSpawnProjection();
      }
    }

    protected sealed class OnConfirmSpawnProjection\u003C\u003E : ICallSite<MyProjectorBase, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProjectorBase @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnConfirmSpawnProjection();
      }
    }

    protected sealed class BuildInternal\u003C\u003EVRageMath_Vector3I\u0023System_Int64\u0023System_Int64\u0023System_Boolean\u0023System_Int64 : ICallSite<MyProjectorBase, Vector3I, long, long, bool, long, DBNull>
    {
      public virtual void Invoke(
        in MyProjectorBase @this,
        in Vector3I cubeBlockPosition,
        in long owner,
        in long builder,
        in bool requestInstant,
        in long builtBy,
        in DBNull arg6)
      {
        @this.BuildInternal(cubeBlockPosition, owner, builder, requestInstant, builtBy);
      }
    }

    protected sealed class OnRemoveSelectedImageRequest\u003C\u003ESystem_Int32\u0023System_Int32\u003C\u0023\u003E : ICallSite<MyProjectorBase, int, int[], DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProjectorBase @this,
        in int panelIndex,
        in int[] selection,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnRemoveSelectedImageRequest(panelIndex, selection);
      }
    }

    protected sealed class OnSelectImageRequest\u003C\u003ESystem_Int32\u0023System_Int32\u003C\u0023\u003E : ICallSite<MyProjectorBase, int, int[], DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProjectorBase @this,
        in int panelIndex,
        in int[] selection,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnSelectImageRequest(panelIndex, selection);
      }
    }

    protected sealed class OnChangeTextRequest\u003C\u003ESystem_Int32\u0023System_String : ICallSite<MyProjectorBase, int, string, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProjectorBase @this,
        in int panelIndex,
        in string text,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnChangeTextRequest(panelIndex, text);
      }
    }

    protected sealed class OnUpdateSpriteCollection\u003C\u003ESystem_Int32\u0023VRage_Game_GUI_TextPanel_MySerializableSpriteCollection : ICallSite<MyProjectorBase, int, MySerializableSpriteCollection, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProjectorBase @this,
        in int panelIndex,
        in MySerializableSpriteCollection sprites,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnUpdateSpriteCollection(panelIndex, sprites);
      }
    }

    protected sealed class OnNewBlueprintSuccess\u003C\u003ESystem_Collections_Generic_List`1\u003CVRage_Game_MyObjectBuilder_CubeGrid\u003E : ICallSite<MyProjectorBase, List<MyObjectBuilder_CubeGrid>, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProjectorBase @this,
        in List<MyObjectBuilder_CubeGrid> projectedGrids,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnNewBlueprintSuccess(projectedGrids);
      }
    }

    protected sealed class ShowScriptRemoveMessage\u003C\u003E : ICallSite<MyProjectorBase, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProjectorBase @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ShowScriptRemoveMessage();
      }
    }

    protected sealed class OnOffsetChangedSuccess\u003C\u003EVRageMath_Vector3I\u0023VRageMath_Vector3I\u0023System_Single\u0023System_Boolean : ICallSite<MyProjectorBase, Vector3I, Vector3I, float, bool, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProjectorBase @this,
        in Vector3I positionOffset,
        in Vector3I rotationOffset,
        in float scale,
        in bool showOnlyBuildable,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnOffsetChangedSuccess(positionOffset, rotationOffset, scale, showOnlyBuildable);
      }
    }

    protected sealed class OnRemoveProjectionRequest\u003C\u003E : ICallSite<MyProjectorBase, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProjectorBase @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnRemoveProjectionRequest();
      }
    }

    protected class m_keepProjection\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyProjectorBase) obj0).m_keepProjection = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_instantBuildingEnabled\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyProjectorBase) obj0).m_instantBuildingEnabled = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_maxNumberOfProjections\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<int, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<int, SyncDirection.BothWays>(obj1, obj2));
        ((MyProjectorBase) obj0).m_maxNumberOfProjections = (VRage.Sync.Sync<int, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_maxNumberOfBlocksPerProjection\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<int, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<int, SyncDirection.BothWays>(obj1, obj2));
        ((MyProjectorBase) obj0).m_maxNumberOfBlocksPerProjection = (VRage.Sync.Sync<int, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_getOwnershipFromProjector\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyProjectorBase) obj0).m_getOwnershipFromProjector = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }
  }
}
