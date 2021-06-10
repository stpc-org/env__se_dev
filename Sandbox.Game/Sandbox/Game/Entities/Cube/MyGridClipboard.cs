// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyGridClipboard
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using ParallelTasks;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.CoordinateSystem;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using VRage.Input;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Cube
{
  public class MyGridClipboard
  {
    protected static readonly MyStringId ID_GIZMO_DRAW_LINE = MyStringId.GetOrCompute("GizmoDrawLine");
    protected static readonly MyStringId ID_GIZMO_DRAW_LINE_RED = MyStringId.GetOrCompute("GizmoDrawLineRed");
    private static bool BUG_SE_19306_FIRED = false;
    [ThreadStatic]
    private static HashSet<IMyEntity> m_cacheEntitySet = new HashSet<IMyEntity>();
    private MyGridClipboard.CopiedGridInfo m_copiedGridsInfo;
    protected readonly List<MyObjectBuilder_CubeGrid> m_copiedGrids = new List<MyObjectBuilder_CubeGrid>();
    protected readonly List<Vector3> m_copiedGridOffsets = new List<Vector3>();
    protected List<MyCubeGrid> m_previewGrids = new List<MyCubeGrid>();
    private List<MyCubeGrid> m_previewGridsParallel = new List<MyCubeGrid>();
    private List<MyObjectBuilder_CubeGrid> m_copiedGridsParallel = new List<MyObjectBuilder_CubeGrid>();
    private readonly MyComponentList m_buildComponents = new MyComponentList();
    protected Vector3D m_pastePosition;
    protected Vector3D m_pastePositionPrevious;
    protected bool m_calculateVelocity = true;
    protected Vector3 m_objectVelocity = Vector3.Zero;
    protected float m_pasteOrientationAngle;
    protected Vector3 m_pasteDirUp = new Vector3(1f, 0.0f, 0.0f);
    protected Vector3 m_pasteDirForward = new Vector3(0.0f, 1f, 0.0f);
    protected float m_dragDistance;
    protected const float m_maxDragDistance = 20000f;
    protected Vector3 m_dragPointToPositionLocal;
    protected bool m_canBePlaced;
    private bool m_canBePlacedNeedsRefresh = true;
    protected bool m_characterHasEnoughMaterials;
    protected MyPlacementSettings m_settings;
    private long? m_spawnerId;
    private Vector3D m_originalSpawnerPosition = Vector3D.Zero;
    private readonly List<MyPhysics.HitInfo> m_raycastCollisionResults = new List<MyPhysics.HitInfo>();
    protected float m_closestHitDistSq = float.MaxValue;
    protected Vector3D m_hitPos = (Vector3D) new Vector3(0.0f, 0.0f, 0.0f);
    protected Vector3 m_hitNormal = new Vector3(1f, 0.0f, 0.0f);
    protected IMyEntity m_hitEntity;
    protected bool m_visible = true;
    private bool m_allowSwitchCameraMode = true;
    protected bool m_useDynamicPreviews;
    protected Dictionary<string, int> m_blocksPerType = new Dictionary<string, int>();
    protected List<MyCubeGrid> m_touchingGrids = new List<MyCubeGrid>();
    private Task ActivationTask;
    private readonly List<IMyEntity> m_resultIDs = new List<IMyEntity>();
    private bool m_isBeingAdded;
    protected bool m_enableUpdateHitEntity = true;
    private bool m_enableStationRotation;
    public bool ShowModdedBlocksWarning = true;
    private bool m_isAligning;
    private int m_lastFrameAligned;

    public Vector3D PastePosition => this.m_pastePosition - Vector3.TransformNormal(this.m_dragPointToPositionLocal, this.GetFirstGridOrientationMatrix());

    public float GetGridHalfExtent(int axis)
    {
      if (this.m_previewGrids.Count <= 0)
        return 0.0f;
      switch (axis)
      {
        case 0:
          return this.m_previewGrids[0].PositionComp.LocalAABB.Extents.X;
        case 1:
          return this.m_previewGrids[0].PositionComp.LocalAABB.Extents.Y;
        case 2:
          return this.m_previewGrids[0].PositionComp.LocalAABB.Extents.Z;
        default:
          return 0.0f;
      }
    }

    protected virtual bool CanBePlaced
    {
      get
      {
        if (this.m_canBePlacedNeedsRefresh)
          this.m_canBePlaced = this.TestPlacement();
        return this.m_canBePlaced;
      }
    }

    public bool CharacterHasEnoughMaterials => this.m_characterHasEnoughMaterials;

    public event Action<MyGridClipboard, bool> Deactivated;

    public virtual bool HasPreviewBBox
    {
      get => true;
      set
      {
      }
    }

    public bool IsActive { get; protected set; }

    public bool AllowSwitchCameraMode
    {
      get => this.m_allowSwitchCameraMode;
      private set => this.m_allowSwitchCameraMode = value;
    }

    public bool IsSnapped { get; protected set; }

    public List<MyObjectBuilder_CubeGrid> CopiedGrids => this.m_copiedGrids;

    public SnapMode SnapMode => this.m_previewGrids.Count == 0 ? SnapMode.Base6Directions : this.m_settings.GetGridPlacementSettings(this.m_previewGrids[0].GridSizeEnum).SnapMode;

    public bool EnablePreciseRotationWhenSnapped => this.m_previewGrids.Count != 0 && this.m_settings.GetGridPlacementSettings(this.m_previewGrids[0].GridSizeEnum).EnablePreciseRotationWhenSnapped && this.EnableStationRotation;

    public bool OneAxisRotationMode => this.IsSnapped && this.SnapMode == SnapMode.OneFreeAxis;

    public List<MyCubeGrid> PreviewGrids => this.m_previewGrids;

    protected virtual bool AnyCopiedGridIsStatic
    {
      get
      {
        foreach (MyCubeGrid previewGrid in this.m_previewGrids)
        {
          if (previewGrid.IsStatic)
            return true;
        }
        return false;
      }
    }

    public bool EnableStationRotation
    {
      get => this.m_enableStationRotation && MyFakes.ENABLE_STATION_ROTATION;
      set
      {
        if (this.m_enableStationRotation == value)
          return;
        this.m_enableStationRotation = value;
        if (this.IsActive && this.m_enableStationRotation)
        {
          this.AlignClipboardToGravity();
          MyCoordinateSystem.Static.Visible = false;
        }
        else
        {
          if (!this.IsActive || this.m_enableStationRotation)
            return;
          this.AlignRotationToCoordSys();
          MyCoordinateSystem.Static.Visible = true;
        }
      }
    }

    public bool CreationMode { get; set; }

    public MyCubeSize CubeSize { get; set; }

    public bool IsStatic { get; set; }

    public bool IsBeingAdded
    {
      get => this.m_isBeingAdded;
      set => this.m_isBeingAdded = value;
    }

    public int MaxVisiblePCU { get; set; } = int.MaxValue;

    public MyGridClipboard(MyPlacementSettings settings, bool calculateVelocity = true)
    {
      this.m_calculateVelocity = calculateVelocity;
      this.m_settings = settings;
    }

    public MyCubeBlockDefinition GetFirstBlockDefinition(
      MyObjectBuilder_CubeGrid grid = null)
    {
      if (grid == null)
      {
        if (this.m_copiedGrids.Count <= 0)
          return (MyCubeBlockDefinition) null;
        grid = this.m_copiedGrids[0];
      }
      return grid.CubeBlocks.Count > 0 ? MyDefinitionManager.Static.GetCubeBlockDefinition(grid.CubeBlocks[0].GetId()) : (MyCubeBlockDefinition) null;
    }

    public virtual void ActivateNoAlign(Action callback = null)
    {
      if (!this.ActivationTask.IsComplete || this.m_isBeingAdded)
        return;
      this.m_isBeingAdded = true;
      if (MySandboxGame.Config.SyncRendering)
      {
        MyEntityIdentifier.PrepareSwapData();
        MyEntityIdentifier.SwapPerThreadData();
      }
      this.m_copiedGridsParallel.Clear();
      this.m_copiedGridsParallel.AddRange((IEnumerable<MyObjectBuilder_CubeGrid>) this.m_copiedGrids);
      this.ActivationTask = Parallel.Start((Action) (() => this.ChangeClipboardPreview(true, this.m_previewGridsParallel, this.m_copiedGridsParallel)), (Action) (() =>
      {
        if (this.m_visible)
        {
          foreach (MyCubeGrid myCubeGrid in this.m_previewGridsParallel)
          {
            Sandbox.Game.Entities.MyEntities.Add((MyEntity) myCubeGrid);
            this.DisablePhysicsRecursively((MyEntity) myCubeGrid);
          }
        }
        List<MyCubeGrid> previewGridsParallel = this.m_previewGridsParallel;
        this.m_previewGridsParallel = this.m_previewGrids;
        this.m_previewGrids = previewGridsParallel;
        if (this.m_visible)
        {
          if (callback != null)
            callback();
          this.IsActive = true;
        }
        this.m_isBeingAdded = false;
      }));
      if (!MySandboxGame.Config.SyncRendering)
        return;
      MyEntityIdentifier.ClearSwapDataAndRestore();
    }

    public virtual void Activate(Action callback = null)
    {
      if (!this.ActivationTask.IsComplete || this.m_isBeingAdded)
        return;
      MyHud.PushRotatingWheelVisible();
      this.m_isBeingAdded = true;
      if (MySandboxGame.Config.SyncRendering)
      {
        MyEntityIdentifier.PrepareSwapData();
        MyEntityIdentifier.SwapPerThreadData();
      }
      this.m_copiedGridsParallel.Clear();
      this.m_copiedGridsParallel.AddRange((IEnumerable<MyObjectBuilder_CubeGrid>) this.m_copiedGrids);
      this.ActivationTask = Parallel.Start(new Action(this.ActivateInternal), (Action) (() =>
      {
        if (this.m_visible)
        {
          foreach (IMyEntity resultId in this.m_resultIDs)
          {
            IMyEntity entity;
            MyEntityIdentifier.TryGetEntity(resultId.EntityId, out entity);
            if (entity == null)
              MyEntityIdentifier.AddEntityWithId(resultId);
          }
          this.m_resultIDs.Clear();
          foreach (MyCubeGrid myCubeGrid in this.m_previewGridsParallel)
          {
            Sandbox.Game.Entities.MyEntities.Add((MyEntity) myCubeGrid);
            this.DisablePhysicsRecursively((MyEntity) myCubeGrid);
          }
          if (callback != null)
            callback();
          this.IsActive = true;
        }
        List<MyCubeGrid> previewGridsParallel = this.m_previewGridsParallel;
        this.m_previewGridsParallel = this.m_previewGrids;
        this.m_previewGrids = previewGridsParallel;
        this.m_isBeingAdded = false;
        MyHud.PopRotatingWheelVisible();
      }));
      if (!MySandboxGame.Config.SyncRendering)
        return;
      MyEntityIdentifier.ClearSwapDataAndRestore();
    }

    private void ActivateInternal()
    {
      this.ChangeClipboardPreview(true, this.m_previewGridsParallel, this.m_copiedGridsParallel);
      if (this.EnableStationRotation)
        this.AlignClipboardToGravity();
      if (MyClipboardComponent.Static.Clipboard != this)
        return;
      if (!this.EnableStationRotation)
      {
        MyCoordinateSystem.Static.Visible = true;
        this.AlignRotationToCoordSys();
      }
      MyCoordinateSystem.OnCoordinateChange += new Action(this.OnCoordinateChange);
    }

    private void OnCoordinateChange()
    {
      if (this.EnableStationRotation)
        return;
      if (MyCoordinateSystem.Static.LocalCoordExist && this.AnyCopiedGridIsStatic)
      {
        this.EnableStationRotation = false;
        MyCoordinateSystem.Static.Visible = true;
      }
      if (!MyCoordinateSystem.Static.LocalCoordExist)
      {
        this.EnableStationRotation = true;
        MyCoordinateSystem.Static.Visible = false;
      }
      else
      {
        this.EnableStationRotation = false;
        MyCoordinateSystem.Static.Visible = true;
      }
      if (this.m_enableStationRotation || !this.IsActive || this.m_isAligning)
        return;
      this.m_isAligning = true;
      int sessionTotalFrames = MyFpsManager.GetSessionTotalFrames();
      if (sessionTotalFrames - this.m_lastFrameAligned >= 12)
      {
        this.AlignRotationToCoordSys();
        this.m_lastFrameAligned = sessionTotalFrames;
      }
      this.m_isAligning = false;
    }

    public virtual void Deactivate(bool afterPaste = false)
    {
      this.CreationMode = false;
      int num = this.IsActive ? 1 : 0;
      this.ChangeClipboardPreview(false, this.m_previewGrids, this.m_copiedGrids);
      this.IsActive = false;
      Action<MyGridClipboard, bool> deactivated = this.Deactivated;
      if (num != 0 && deactivated != null)
        deactivated(this, afterPaste);
      if (MyClipboardComponent.Static.Clipboard != this)
        return;
      if (MyCoordinateSystem.Static != null)
      {
        MyCoordinateSystem.Static.Visible = false;
        MyCoordinateSystem.Static.ResetSelection();
      }
      MyCoordinateSystem.OnCoordinateChange -= new Action(this.OnCoordinateChange);
    }

    public void Hide()
    {
      if (MyFakes.ENABLE_VR_BUILDING)
        this.ShowPreview(false);
      else
        this.ChangeClipboardPreview(false, this.m_previewGrids, this.m_copiedGrids);
    }

    public void Show()
    {
      if (!this.IsActive || this.m_isBeingAdded)
        return;
      if (this.m_previewGrids.Count == 0)
        this.ChangeClipboardPreview(true, this.m_previewGrids, this.m_copiedGrids);
      if (!MyFakes.ENABLE_VR_BUILDING)
        return;
      this.ShowPreview(true);
    }

    protected void ShowPreview(bool show)
    {
      if (this.PreviewGrids.Count == 0 || this.PreviewGrids[0].Render.Visible == show)
        return;
      foreach (MyCubeGrid previewGrid in this.PreviewGrids)
      {
        previewGrid.Render.Visible = show;
        foreach (MySlimBlock block1 in previewGrid.GetBlocks())
        {
          if (block1.FatBlock is MyCompoundCubeBlock fatBlock)
          {
            fatBlock.Render.UpdateRenderObject(show);
            foreach (MySlimBlock block2 in fatBlock.GetBlocks())
            {
              if (block2.FatBlock != null)
                block2.FatBlock.Render.UpdateRenderObject(show);
            }
          }
          else if (block1.FatBlock != null)
            block1.FatBlock.Render.UpdateRenderObject(show);
        }
      }
    }

    public void ClearClipboard()
    {
      if (this.IsActive)
        this.Deactivate();
      this.m_copiedGrids.Clear();
      this.m_copiedGridOffsets.Clear();
    }

    public void CopyGroup(MyCubeGrid gridInGroup, GridLinkTypeEnum groupType)
    {
      if (gridInGroup == null)
        return;
      this.m_copiedGrids.Clear();
      this.m_copiedGridOffsets.Clear();
      if (MyFakes.ENABLE_COPY_GROUP && MyFakes.ENABLE_LARGE_STATIC_GROUP_COPY_FIRST)
      {
        List<MyCubeGrid> groupNodes1 = MyCubeGridGroups.Static.GetGroups(groupType).GetGroupNodes(gridInGroup);
        MyCubeGrid myCubeGrid1 = (MyCubeGrid) null;
        MyCubeGrid myCubeGrid2 = (MyCubeGrid) null;
        MyCubeGrid myCubeGrid3 = (MyCubeGrid) null;
        if (gridInGroup.GridSizeEnum == MyCubeSize.Large)
        {
          myCubeGrid2 = gridInGroup;
          if (gridInGroup.IsStatic)
            myCubeGrid1 = gridInGroup;
        }
        else if (gridInGroup.GridSizeEnum == MyCubeSize.Small && gridInGroup.IsStatic)
          myCubeGrid3 = gridInGroup;
        foreach (MyCubeGrid myCubeGrid4 in groupNodes1)
        {
          if (myCubeGrid2 == null && myCubeGrid4.GridSizeEnum == MyCubeSize.Large)
            myCubeGrid2 = myCubeGrid4;
          if (myCubeGrid1 == null && myCubeGrid4.GridSizeEnum == MyCubeSize.Large && myCubeGrid4.IsStatic)
            myCubeGrid1 = myCubeGrid4;
          if (myCubeGrid3 == null && myCubeGrid4.GridSizeEnum == MyCubeSize.Small && myCubeGrid4.IsStatic)
            myCubeGrid3 = myCubeGrid4;
        }
        MyCubeGrid myCubeGrid5 = (((myCubeGrid1 ?? (MyCubeGrid) null) ?? myCubeGrid2 ?? (MyCubeGrid) null) ?? myCubeGrid3 ?? (MyCubeGrid) null) ?? gridInGroup;
        List<MyCubeGrid> groupNodes2 = MyCubeGridGroups.Static.GetGroups(groupType).GetGroupNodes(myCubeGrid5);
        this.CopyGridInternal(myCubeGrid5);
        foreach (MyCubeGrid toCopy in groupNodes2)
        {
          if (toCopy != myCubeGrid5)
            this.CopyGridInternal(toCopy);
        }
      }
      else
      {
        this.CopyGridInternal(gridInGroup);
        if (!MyFakes.ENABLE_COPY_GROUP)
          return;
        foreach (MyCubeGrid groupNode in MyCubeGridGroups.Static.GetGroups(groupType).GetGroupNodes(gridInGroup))
        {
          if (groupNode != gridInGroup)
            this.CopyGridInternal(groupNode);
        }
      }
    }

    public void CutGrid(MyCubeGrid grid)
    {
      if (grid == null)
        return;
      this.CopyGrid(grid);
      this.DeleteGrid(grid);
    }

    public void DeleteGrid(MyCubeGrid grid) => grid?.SendGridCloseRequest();

    public void CopyGrid(MyCubeGrid grid)
    {
      if (grid == null)
        return;
      this.m_copiedGrids.Clear();
      this.m_copiedGridOffsets.Clear();
      this.CopyGridInternal(grid);
    }

    public void CutGroup(MyCubeGrid grid, GridLinkTypeEnum groupType)
    {
      if (grid == null)
        return;
      this.CopyGroup(grid, groupType);
      this.DeleteGroup(grid, groupType);
    }

    public void DeleteGroup(MyCubeGrid grid, GridLinkTypeEnum groupType)
    {
      if (grid == null)
        return;
      if (MyFakes.ENABLE_COPY_GROUP)
      {
        foreach (MyCubeGrid groupNode in MyCubeGridGroups.Static.GetGroups(groupType).GetGroupNodes(grid))
        {
          foreach (MySlimBlock block in groupNode.GetBlocks())
          {
            if (block.FatBlock is MyCockpit fatBlock && fatBlock.Pilot != null)
              fatBlock.RequestRemovePilot();
          }
          groupNode.SendGridCloseRequest();
        }
      }
      else
        grid.SendGridCloseRequest();
    }

    private void CopyGridInternal(MyCubeGrid toCopy)
    {
      if (MySession.Static.CameraController.Equals((object) toCopy))
        MySession.Static.SetCameraController(MyCameraControllerEnum.Spectator, (IMyEntity) null, new Vector3D?(toCopy.PositionComp.GetPosition()));
      MyObjectBuilder_CubeGrid objectBuilder = (MyObjectBuilder_CubeGrid) toCopy.GetObjectBuilder(true);
      objectBuilder.GridGeneralDamageModifier = 1f;
      objectBuilder.Immune = false;
      this.m_copiedGrids.Add(objectBuilder);
      this.RemovePilots(objectBuilder);
      if (this.m_copiedGrids.Count == 1)
      {
        MatrixD pasteMatrix = MyGridClipboard.GetPasteMatrix();
        Vector3I? nullable = toCopy.RayCastBlocks(pasteMatrix.Translation, pasteMatrix.Translation + pasteMatrix.Forward * 1000.0);
        Vector3D vector3D = nullable.HasValue ? toCopy.GridIntegerToWorld(nullable.Value) : toCopy.WorldMatrix.Translation;
        this.m_dragPointToPositionLocal = (Vector3) Vector3D.TransformNormal(toCopy.PositionComp.GetPosition() - vector3D, toCopy.PositionComp.WorldMatrixNormalizedInv);
        this.m_dragDistance = (float) (vector3D - pasteMatrix.Translation).Length();
        this.m_pasteDirUp = (Vector3) toCopy.WorldMatrix.Up;
        this.m_pasteDirForward = (Vector3) toCopy.WorldMatrix.Forward;
        this.m_pasteOrientationAngle = 0.0f;
      }
      this.m_copiedGridOffsets.Add((Vector3) (toCopy.WorldMatrix.Translation - (Vector3D) this.m_copiedGrids[0].PositionAndOrientation.Value.Position));
    }

    public virtual bool PasteGrid(bool deactivate = true, bool showWarning = true)
    {
      try
      {
        this.UpdateTouchingGrids(Sync.MyId);
        return this.PasteGridInternal(deactivate, touchingGrids: this.m_touchingGrids, showWarning: showWarning);
      }
      catch
      {
        MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
        MyHud.Notifications.Add(MyNotificationSingletons.PasteFailed);
        return false;
      }
    }

    protected bool PasteGridInternal(
      bool deactivate,
      List<MyObjectBuilder_CubeGrid> pastedBuilders = null,
      List<MyCubeGrid> touchingGrids = null,
      MyGridClipboard.UpdateAfterPasteCallback updateAfterPasteCallback = null,
      bool multiBlock = false,
      bool showWarning = true)
    {
      if (this.m_copiedGrids.Count == 0 & showWarning)
      {
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.Blueprints_EmptyClipboardMessageHeader);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, messageText: MyTexts.Get(MyCommonTexts.Blueprints_EmptyClipboardMessage), messageCaption: messageCaption));
        return false;
      }
      if (this.m_copiedGrids.Count > 0 && !this.IsActive)
      {
        this.Activate();
        return true;
      }
      if (!this.CanBePlaced)
      {
        MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
        return false;
      }
      if (!this.CheckLimitsAndNotify() || this.m_previewGrids.Count == 0)
        return false;
      foreach (MyEntity previewGrid in this.m_previewGrids)
      {
        if (!MySessionComponentSafeZones.IsActionAllowed(previewGrid.PositionComp.WorldAABB, MySafeZoneAction.Building, MySession.Static.LocalCharacterEntityId))
          return false;
      }
      if (!MySession.Static.IsRunningExperimental)
      {
        bool flag = false;
        foreach (MyCubeGrid previewGrid in this.m_previewGrids)
        {
          if (previewGrid.UnsafeBlocks.Count > 0)
          {
            flag = true;
            break;
          }
        }
        if (flag)
        {
          StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.Blueprints_UnsafeClipboardMessageHeader);
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, messageText: MyTexts.Get(MyCommonTexts.Blueprints_UnsafeClipboardMessage), messageCaption: messageCaption));
          return false;
        }
      }
      if (!MySession.Static.IsUserAdmin(Sync.MyId) || !MySession.Static.AdminSettings.HasFlag((Enum) AdminSettingsEnum.KeepOriginalOwnershipOnPaste))
      {
        foreach (MyObjectBuilder_CubeGrid copiedGrid in this.m_copiedGrids)
        {
          foreach (MyObjectBuilder_CubeBlock cubeBlock in copiedGrid.CubeBlocks)
          {
            cubeBlock.BuiltBy = MySession.Static.LocalPlayerId;
            if (cubeBlock.Owner != 0L && Sync.Players.IdentityIsNpc(cubeBlock.Owner))
              cubeBlock.Owner = MySession.Static.LocalPlayerId;
          }
        }
      }
      bool missingBlockDefinitions = false;
      if (this.ShowModdedBlocksWarning)
        missingBlockDefinitions = !this.CheckPastedBlocks();
      if (missingBlockDefinitions)
      {
        this.AllowSwitchCameraMode = false;
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.MessageBoxTextDoYouWantToPasteGridWithMissingBlocks), MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result =>
        {
          if (result == MyGuiScreenMessageBox.ResultEnum.YES)
            this.PasteInternal(missingBlockDefinitions, deactivate, pastedBuilders, updateAfterPasteCallback: updateAfterPasteCallback, multiBlock: multiBlock);
          this.AllowSwitchCameraMode = true;
        }))));
        return false;
      }
      MyDLCs.MyDLC missingDLC = this.CheckPastedDLCBlocks();
      if (missingDLC != null)
      {
        this.AllowSwitchCameraMode = false;
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO_CANCEL, MyTexts.Get(MyCommonTexts.MessageBoxTextMissingDLCWhenPasting), MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning), yesButtonText: new MyStringId?(MyCommonTexts.VisitStore), noButtonText: new MyStringId?(MyCommonTexts.PasteAnyway), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result =>
        {
          switch (result)
          {
            case MyGuiScreenMessageBox.ResultEnum.YES:
              MyGameService.OpenDlcInShop(missingDLC.AppId);
              break;
            case MyGuiScreenMessageBox.ResultEnum.NO:
              this.PasteInternal(missingBlockDefinitions, deactivate, pastedBuilders, updateAfterPasteCallback: updateAfterPasteCallback, multiBlock: multiBlock);
              break;
          }
          this.AllowSwitchCameraMode = true;
        }))));
        return false;
      }
      if (!MySession.Static.IsUserScripter(Sync.MyId) && !this.CheckPastedScripts())
        MyHud.Notifications.Add(MyNotificationSingletons.BlueprintScriptsRemoved);
      return this.PasteInternal(missingBlockDefinitions, deactivate, pastedBuilders, touchingGrids, updateAfterPasteCallback, multiBlock, true);
    }

    private bool PasteInternal(
      bool missingDefinitions,
      bool deactivate,
      List<MyObjectBuilder_CubeGrid> pastedBuilders = null,
      List<MyCubeGrid> touchingGrids = null,
      MyGridClipboard.UpdateAfterPasteCallback updateAfterPasteCallback = null,
      bool multiBlock = false,
      bool keepRelativeOffset = false)
    {
      List<MyObjectBuilder_CubeGrid> objectBuilderCubeGridList = new List<MyObjectBuilder_CubeGrid>();
      foreach (MyObjectBuilder_CubeGrid copiedGrid in this.m_copiedGrids)
        objectBuilderCubeGridList.Add((MyObjectBuilder_CubeGrid) copiedGrid.Clone());
      MyObjectBuilder_CubeGrid objectBuilderCubeGrid1 = objectBuilderCubeGridList[0];
      int num = !this.IsSnapped || this.SnapMode != SnapMode.Base6Directions || (!(this.m_hitEntity is MyCubeGrid) || objectBuilderCubeGrid1 == null) ? 0 : (((MyCubeGrid) this.m_hitEntity).GridSizeEnum == objectBuilderCubeGrid1.GridSizeEnum ? 1 : 0);
      MyCubeGrid myCubeGrid = (MyCubeGrid) null;
      if (num != 0)
        myCubeGrid = this.m_hitEntity as MyCubeGrid;
      bool flag1 = num != 0 || touchingGrids != null && touchingGrids.Count > 0;
      if (myCubeGrid == null && touchingGrids != null && touchingGrids.Count > 0)
        myCubeGrid = touchingGrids[0];
      if (num != 0 && !this.CheckLimitsAndNotify(myCubeGrid != null ? myCubeGrid.BlocksCount : objectBuilderCubeGridList[0].CubeBlocks.Count))
        return false;
      MyGuiAudio.PlaySound(MyGuiSounds.HudPlaceBlock);
      int index = 0;
      foreach (MyObjectBuilder_CubeGrid objectBuilderCubeGrid2 in objectBuilderCubeGridList)
      {
        objectBuilderCubeGrid2.CreatePhysics = true;
        objectBuilderCubeGrid2.EnableSmallToLargeConnections = true;
        objectBuilderCubeGrid2.PositionAndOrientation = new MyPositionAndOrientation?(new MyPositionAndOrientation(this.m_previewGrids[index].WorldMatrix));
        objectBuilderCubeGrid2.PositionAndOrientation.Value.Orientation.Normalize();
        ++index;
      }
      long inventoryEntityId = 0;
      bool instantBuild = MySession.Static.CreativeToolsEnabled(Sync.MyId);
      if (flag1 && myCubeGrid != null)
        myCubeGrid.PasteBlocksToGrid(objectBuilderCubeGridList, inventoryEntityId, multiBlock, instantBuild);
      else if (this.CreationMode)
      {
        MyMultiplayer.RaiseStaticEvent<MyCubeSize, bool, MyPositionAndOrientation, long, bool>((Func<IMyEventOwner, Action<MyCubeSize, bool, MyPositionAndOrientation, long, bool>>) (s => new Action<MyCubeSize, bool, MyPositionAndOrientation, long, bool>(MyCubeGrid.TryCreateGrid_Implementation)), this.CubeSize, this.IsStatic, objectBuilderCubeGridList[0].PositionAndOrientation.Value, inventoryEntityId, instantBuild);
        this.CreationMode = false;
      }
      else if (MySession.Static.CreativeMode || MySession.Static.HasCreativeRights)
      {
        bool flag2 = false;
        bool flag3 = false;
        foreach (MyCubeGrid previewGrid in this.m_previewGrids)
        {
          flag3 |= previewGrid.GridSizeEnum == MyCubeSize.Small;
          MyGridPlacementSettings placementSettings = this.m_settings.GetGridPlacementSettings(previewGrid.GridSizeEnum, previewGrid.IsStatic);
          flag2 |= MyCubeGrid.IsAabbInsideVoxel(previewGrid.PositionComp.WorldMatrixRef, (BoundingBoxD) previewGrid.PositionComp.LocalAABB, placementSettings);
        }
        bool flag4 = false;
        foreach (MyObjectBuilder_CubeGrid objectBuilderCubeGrid2 in objectBuilderCubeGridList)
          objectBuilderCubeGrid2.IsStatic = flag4 | flag2 || MySession.Static.EnableConvertToStation && objectBuilderCubeGrid2.IsStatic;
        if (!Sandbox.Engine.Platform.Game.IsDedicated)
          MyHud.PushRotatingWheelVisible();
        MyCubeGrid.RelativeOffset offset = new MyCubeGrid.RelativeOffset();
        if (keepRelativeOffset && !Sandbox.Engine.Platform.Game.IsDedicated)
        {
          offset.Use = true;
          if (this.m_spawnerId.HasValue)
          {
            offset.RelativeToEntity = true;
            offset.SpawnerId = this.m_spawnerId.Value;
          }
          else
          {
            offset.RelativeToEntity = false;
            offset.SpawnerId = 0L;
          }
          offset.OriginalSpawnPoint = this.m_originalSpawnerPosition;
        }
        else
          offset.Use = false;
        MyMultiplayer.RaiseStaticEvent<MyCubeGrid.MyPasteGridParameters>((Func<IMyEventOwner, Action<MyCubeGrid.MyPasteGridParameters>>) (s => new Action<MyCubeGrid.MyPasteGridParameters>(MyCubeGrid.TryPasteGrid_Implementation)), new MyCubeGrid.MyPasteGridParameters(objectBuilderCubeGridList, missingDefinitions, multiBlock, this.m_objectVelocity, instantBuild, offset, MySession.Static.GetComponent<MySessionComponentDLC>().GetAvailableClientDLCsIds()));
      }
      if (deactivate)
        this.Deactivate(true);
      if (updateAfterPasteCallback != null)
        updateAfterPasteCallback(pastedBuilders);
      return true;
    }

    private bool CheckLimitsAndNotify(int mergedBlocksCount = 0)
    {
      int pcUs = this.m_copiedGridsInfo.PCUs;
      int totalBlocks = this.m_copiedGridsInfo.TotalBlocks;
      bool flag = !this.m_copiedGridsInfo.HasGridOverLimits;
      return (mergedBlocksCount <= 0 || MySession.Static.CheckLimitsAndNotify(MySession.Static.LocalPlayerId, (string) null, 0, blocksCount: mergedBlocksCount)) && MySession.Static.CheckLimitsAndNotify(MySession.Static.LocalPlayerId, (string) null, pcUs, totalBlocks, flag ? 0 : MySession.Static.MaxGridSize + 1, this.m_blocksPerType);
    }

    protected bool CheckPastedBlocks() => MyGridClipboard.CheckPastedBlocks((IEnumerable<MyObjectBuilder_CubeGrid>) this.m_copiedGrids);

    public static bool CheckPastedBlocks(IEnumerable<MyObjectBuilder_CubeGrid> pastedGrids)
    {
      foreach (MyObjectBuilder_CubeGrid pastedGrid in pastedGrids)
      {
        bool flag = !MySession.Static.Settings.EnableSupergridding;
        foreach (MyObjectBuilder_CubeBlock cubeBlock in pastedGrid.CubeBlocks)
        {
          MyCubeBlockDefinition blockDefinition;
          if (!MyDefinitionManager.Static.TryGetCubeBlockDefinition(new MyDefinitionId(cubeBlock.TypeId, cubeBlock.SubtypeId), out blockDefinition) || flag && blockDefinition.CubeSize != pastedGrid.GridSizeEnum)
            return false;
        }
      }
      return true;
    }

    protected bool CheckPastedScripts()
    {
      if (MySession.Static.IsUserScripter(Sync.MyId))
        return true;
      foreach (MyObjectBuilder_CubeGrid copiedGrid in this.m_copiedGrids)
      {
        foreach (MyObjectBuilder_CubeBlock cubeBlock in copiedGrid.CubeBlocks)
        {
          if (cubeBlock is MyObjectBuilder_MyProgrammableBlock programmableBlock && programmableBlock.Program != null)
            return false;
        }
      }
      return true;
    }

    protected MyDLCs.MyDLC CheckPastedDLCBlocks()
    {
      MySessionComponentDLC component = MySession.Static.GetComponent<MySessionComponentDLC>();
      foreach (MyObjectBuilder_CubeGrid copiedGrid in this.m_copiedGrids)
      {
        foreach (MyObjectBuilder_CubeBlock cubeBlock in copiedGrid.CubeBlocks)
        {
          MyDefinitionBase definition = MyDefinitionManager.Static.GetDefinition(new MyDefinitionId(cubeBlock.TypeId, cubeBlock.SubtypeId));
          MyDLCs.MyDLC missingDefinitionDlc = component.GetFirstMissingDefinitionDLC(definition, Sync.MyId);
          if (missingDefinitionDlc != null)
            return missingDefinitionDlc;
        }
      }
      return (MyDLCs.MyDLC) null;
    }

    public void SetGridFromBuilder(
      MyObjectBuilder_CubeGrid grid,
      Vector3 dragPointDelta,
      float dragVectorLength)
    {
      this.m_copiedGrids.Clear();
      this.m_copiedGridOffsets.Clear();
      this.m_dragPointToPositionLocal = dragPointDelta;
      this.m_dragDistance = dragVectorLength;
      MyPositionAndOrientation positionAndOrientation = grid.PositionAndOrientation ?? MyPositionAndOrientation.Default;
      this.m_pasteDirUp = (Vector3) positionAndOrientation.Up;
      this.m_pasteDirForward = (Vector3) positionAndOrientation.Forward;
      this.SetGridFromBuilderInternal(grid, Vector3.Zero);
    }

    public void SetGridFromBuilders(
      MyObjectBuilder_CubeGrid[] grids,
      Vector3 dragPointDelta,
      float dragVectorLength,
      bool deactivate = true)
    {
      this.ShowModdedBlocksWarning = true;
      if (this.IsActive & deactivate)
        this.Deactivate();
      this.m_copiedGrids.Clear();
      this.m_copiedGridOffsets.Clear();
      if (grids.Length == 0)
        return;
      this.m_dragPointToPositionLocal = dragPointDelta;
      this.m_dragDistance = dragVectorLength;
      MyPositionAndOrientation positionAndOrientation = grids[0].PositionAndOrientation ?? MyPositionAndOrientation.Default;
      this.m_pasteDirUp = (Vector3) positionAndOrientation.Up;
      this.m_pasteDirForward = (Vector3) positionAndOrientation.Forward;
      this.SetGridFromBuilderInternal(grids[0], Vector3.Zero);
      MatrixD matrix = MatrixD.Invert(grids[0].PositionAndOrientation.HasValue ? grids[0].PositionAndOrientation.Value.GetMatrix() : MatrixD.Identity);
      for (int index = 1; index < grids.Length; ++index)
      {
        Vector3D vector3D = Vector3D.Transform(grids[index].PositionAndOrientation.HasValue ? (Vector3D) grids[index].PositionAndOrientation.Value.Position : Vector3D.Zero, matrix);
        this.SetGridFromBuilderInternal(grids[index], (Vector3) vector3D);
      }
    }

    private void SetGridFromBuilderInternal(MyObjectBuilder_CubeGrid grid, Vector3 offset)
    {
      this.BeforeCreateGrid(grid);
      this.m_copiedGrids.Add(grid);
      this.m_copiedGridOffsets.Add(offset);
      this.RemovePilots(grid);
    }

    protected void BeforeCreateGrid(MyObjectBuilder_CubeGrid grid)
    {
      foreach (MyObjectBuilder_CubeBlock cubeBlock in grid.CubeBlocks)
      {
        MyDefinitionId id = cubeBlock.GetId();
        MyCubeBlockDefinition blockDefinition = (MyCubeBlockDefinition) null;
        MyDefinitionManager.Static.TryGetCubeBlockDefinition(id, out blockDefinition);
        if (blockDefinition != null)
          MyCubeBuilder.BuildComponent.BeforeCreateBlock(blockDefinition, this.GetClipboardBuilder(), cubeBlock, MySession.Static.CreativeToolsEnabled(Sync.MyId));
      }
    }

    protected virtual void ChangeClipboardPreview(
      bool visible,
      List<MyCubeGrid> previewGrids,
      List<MyObjectBuilder_CubeGrid> copiedGrids)
    {
      foreach (MyCubeGrid previewGrid in previewGrids)
      {
        if (!Sandbox.Engine.Platform.Game.IsDedicated)
          Sandbox.Game.Entities.MyEntities.EnableEntityBoundingBoxDraw((MyEntity) previewGrid, false);
        previewGrid.SetFadeOut(false);
        previewGrid.Close();
      }
      this.m_visible = false;
      previewGrids.Clear();
      this.m_buildComponents.Clear();
      if (copiedGrids.Count == 0 || !visible)
        return;
      MyGridClipboard.CalculateItemRequirements(copiedGrids, this.m_buildComponents);
      Sandbox.Game.Entities.MyEntities.RemapObjectBuilderCollection((IEnumerable<MyObjectBuilder_EntityBase>) copiedGrids);
      MyCubeGrid.CleanCubeGridsBeforePaste(copiedGrids);
      this.m_copiedGridsInfo = new MyGridClipboard.CopiedGridInfo();
      int maxGridSize = MySession.Static.MaxGridSize;
      foreach (MyObjectBuilder_CubeGrid copiedGrid in this.m_copiedGrids)
      {
        this.m_copiedGridsInfo.PCUs += copiedGrid.CalculatePCUs();
        this.m_copiedGridsInfo.TotalBlocks += copiedGrid.CubeBlocks.Count;
        if (maxGridSize != 0 && copiedGrid.CubeBlocks.Count > maxGridSize)
          this.m_copiedGridsInfo.HasGridOverLimits = true;
      }
      Vector3D vector3D = Vector3D.Zero;
      this.m_blocksPerType.Clear();
      MyEntityIdentifier.InEntityCreationBlock = true;
      MyEntityIdentifier.LazyInitPerThreadStorage(2048);
      bool flag1 = this.m_copiedGridsInfo.PCUs > this.MaxVisiblePCU;
      bool flag2 = true;
      foreach (MyObjectBuilder_CubeGrid copiedGrid in copiedGrids)
      {
        List<MyObjectBuilder_CubeBlock> cubeBlocks = copiedGrid.CubeBlocks;
        bool isStatic = copiedGrid.IsStatic;
        if (this.m_useDynamicPreviews)
          copiedGrid.IsStatic = false;
        copiedGrid.CreatePhysics = false;
        copiedGrid.EnableSmallToLargeConnections = false;
        foreach (MyObjectBuilder_CubeBlock cubeBlock in copiedGrid.CubeBlocks)
        {
          cubeBlock.BuiltBy = 0L;
          MyCubeBlockDefinition blockDefinition;
          if (MyDefinitionManager.Static.TryGetCubeBlockDefinition(new MyDefinitionId(cubeBlock.TypeId, cubeBlock.SubtypeId), out blockDefinition))
          {
            string blockPairName = blockDefinition.BlockPairName;
            if (this.m_blocksPerType.ContainsKey(blockPairName))
              this.m_blocksPerType[blockPairName]++;
            else
              this.m_blocksPerType.Add(blockPairName, 1);
          }
        }
        if (copiedGrid.PositionAndOrientation.HasValue)
        {
          MyPositionAndOrientation positionAndOrientation = copiedGrid.PositionAndOrientation.Value;
          if (flag2)
          {
            flag2 = false;
            vector3D = (Vector3D) positionAndOrientation.Position;
          }
          ref SerializableVector3D local = ref positionAndOrientation.Position;
          local = (SerializableVector3D) ((Vector3D) local - vector3D);
          copiedGrid.PositionAndOrientation = new MyPositionAndOrientation?(positionAndOrientation);
        }
        if (flag1)
          copiedGrid.CubeBlocks = new List<MyObjectBuilder_CubeBlock>();
        MyCubeGrid fromObjectBuilder = Sandbox.Game.Entities.MyEntities.CreateFromObjectBuilder((MyObjectBuilder_EntityBase) copiedGrid, false) as MyCubeGrid;
        copiedGrid.IsStatic = isStatic;
        copiedGrid.CubeBlocks = cubeBlocks;
        if (fromObjectBuilder == null)
        {
          this.ChangeClipboardPreview(false, previewGrids, copiedGrids);
          return;
        }
        this.MakeTransparent(fromObjectBuilder);
        if (fromObjectBuilder.CubeBlocks.Count == 0 && !flag1)
        {
          copiedGrids.Remove(copiedGrid);
          this.ChangeClipboardPreview(false, previewGrids, copiedGrids);
          return;
        }
        fromObjectBuilder.IsPreview = true;
        fromObjectBuilder.Save = false;
        previewGrids.Add(fromObjectBuilder);
        fromObjectBuilder.OnClose += new Action<MyEntity>(this.previewGrid_OnClose);
        fromObjectBuilder.PositionComp.LocalAABB = copiedGrid.CalculateBoundingBox();
      }
      if (flag1)
      {
        MyHudNotification myHudNotification = new MyHudNotification(MySpaceTexts.ClipboardBlueprintIsTooBig, 5000, priority: 1, level: MyNotificationLevel.Control);
        MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification);
      }
      this.m_resultIDs.Clear();
      MyEntityIdentifier.GetPerThreadEntities(this.m_resultIDs);
      MyEntityIdentifier.ClearPerThreadEntities();
      MyEntityIdentifier.InEntityCreationBlock = false;
      this.m_visible = visible;
    }

    private void previewGrid_OnClose(MyEntity obj)
    {
      this.m_previewGrids.Remove(obj as MyCubeGrid);
      int count = this.m_previewGrids.Count;
    }

    public static void CalculateItemRequirements(
      List<MyObjectBuilder_CubeGrid> blocksToBuild,
      MyComponentList buildComponents)
    {
      buildComponents.Clear();
      foreach (MyObjectBuilder_CubeGrid objectBuilderCubeGrid in blocksToBuild)
      {
        foreach (MyObjectBuilder_CubeBlock cubeBlock in objectBuilderCubeGrid.CubeBlocks)
        {
          if (cubeBlock is MyObjectBuilder_CompoundCubeBlock compoundCubeBlock)
          {
            foreach (MyObjectBuilder_CubeBlock block in compoundCubeBlock.Blocks)
              MyGridClipboard.AddSingleBlockRequirements(block, buildComponents);
          }
          else
            MyGridClipboard.AddSingleBlockRequirements(cubeBlock, buildComponents);
        }
      }
    }

    public static void CalculateItemRequirements(
      MyObjectBuilder_CubeGrid[] blocksToBuild,
      MyComponentList buildComponents)
    {
      buildComponents.Clear();
      foreach (MyObjectBuilder_CubeGrid objectBuilderCubeGrid in blocksToBuild)
      {
        foreach (MyObjectBuilder_CubeBlock cubeBlock in objectBuilderCubeGrid.CubeBlocks)
        {
          if (cubeBlock is MyObjectBuilder_CompoundCubeBlock compoundCubeBlock)
          {
            foreach (MyObjectBuilder_CubeBlock block in compoundCubeBlock.Blocks)
              MyGridClipboard.AddSingleBlockRequirements(block, buildComponents);
          }
          else
            MyGridClipboard.AddSingleBlockRequirements(cubeBlock, buildComponents);
        }
      }
    }

    private static void AddSingleBlockRequirements(
      MyObjectBuilder_CubeBlock block,
      MyComponentList buildComponents)
    {
      MyComponentStack.GetMountedComponents(buildComponents, block);
      if (block.ConstructionStockpile == null)
        return;
      foreach (MyObjectBuilder_StockpileItem builderStockpileItem in block.ConstructionStockpile.Items)
      {
        if (builderStockpileItem.PhysicalContent != null)
          buildComponents.AddMaterial(builderStockpileItem.PhysicalContent.GetId(), builderStockpileItem.Amount, addToDisplayList: false);
      }
    }

    protected virtual float Transparency => 0.25f;

    private void MakeTransparent(MyCubeGrid grid)
    {
      grid.Render.Transparency = this.Transparency;
      grid.Render.CastShadows = false;
      if (MyGridClipboard.m_cacheEntitySet == null)
        MyGridClipboard.m_cacheEntitySet = new HashSet<IMyEntity>();
      grid.Hierarchy.GetChildrenRecursive(MyGridClipboard.m_cacheEntitySet);
      foreach (IMyEntity cacheEntity in MyGridClipboard.m_cacheEntitySet)
      {
        if (cacheEntity is MyCubeBlock myCubeBlock)
          myCubeBlock.SlimBlock.Dithering = this.Transparency;
        cacheEntity.Render.Transparency = this.Transparency;
        cacheEntity.Render.CastShadows = false;
      }
      MyGridClipboard.m_cacheEntitySet.Clear();
    }

    private void DisablePhysicsRecursively(MyEntity entity)
    {
      MyPhysicsComponentBase physics = entity.Physics;
      if ((physics != null ? (physics.Enabled ? 1 : 0) : 0) != 0)
        entity.Physics.Enabled = false;
      if (entity is MyCubeBlock myCubeBlock)
      {
        MyPhysicsComponentBase detectorPhysics = myCubeBlock.UseObjectsComponent.DetectorPhysics;
        if ((detectorPhysics != null ? (detectorPhysics.Enabled ? 1 : 0) : 0) != 0)
          myCubeBlock.UseObjectsComponent.DetectorPhysics.Enabled = false;
        myCubeBlock.DisableUpdates();
      }
      foreach (MyEntityComponentBase child in entity.Hierarchy.Children)
        this.DisablePhysicsRecursively(child.Container.Entity as MyEntity);
    }

    public virtual void Update()
    {
      if (!this.IsActive || !this.m_visible)
        return;
      if (this.m_enableUpdateHitEntity)
        this.UpdateHitEntity();
      if (this.m_previewGrids.Count > 0)
      {
        this.UpdatePastePosition();
        this.UpdateGridTransformations();
      }
      if (this.IsSnapped && this.SnapMode == SnapMode.Base6Directions)
        this.FixSnapTransformationBase6();
      if (this.m_calculateVelocity)
        this.m_objectVelocity = (Vector3) ((this.m_pastePosition - this.m_pastePositionPrevious) / 0.0166666675359011);
      if (MyFpsManager.GetSessionTotalFrames() % 11 == 0)
        this.m_canBePlaced = this.TestPlacement();
      else
        this.m_canBePlacedNeedsRefresh = true;
      this.m_characterHasEnoughMaterials = true;
      this.UpdatePreviewBBox();
      if (!MyDebugDrawSettings.DEBUG_DRAW_COPY_PASTE)
        return;
      MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, 0.0f), "FW: " + this.m_pasteDirForward.ToString(), Color.Red, 1f);
      MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, 20f), "UP: " + this.m_pasteDirUp.ToString(), Color.Red, 1f);
      MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, 40f), "AN: " + this.m_pasteOrientationAngle.ToString(), Color.Red, 1f);
    }

    protected bool UpdateHitEntity(bool canPasteLargeOnSmall = true)
    {
      this.m_closestHitDistSq = float.MaxValue;
      this.m_hitPos = (Vector3D) new Vector3(0.0f, 0.0f, 0.0f);
      this.m_hitNormal = new Vector3(1f, 0.0f, 0.0f);
      this.m_hitEntity = (IMyEntity) null;
      MatrixD pasteMatrix = MyGridClipboard.GetPasteMatrix();
      if (MyFakes.ENABLE_VR_BUILDING && MyBlockBuilderBase.PlacementProvider != null)
      {
        MyPhysics.HitInfo? hitInfo = MyBlockBuilderBase.PlacementProvider.HitInfo;
        if (!hitInfo.HasValue)
          return false;
        this.m_hitEntity = (IMyEntity) ((MyEntity) MyBlockBuilderBase.PlacementProvider.ClosestGrid ?? (MyEntity) MyBlockBuilderBase.PlacementProvider.ClosestVoxelMap);
        hitInfo = MyBlockBuilderBase.PlacementProvider.HitInfo;
        this.m_hitPos = hitInfo.Value.Position;
        hitInfo = MyBlockBuilderBase.PlacementProvider.HitInfo;
        this.m_hitNormal = hitInfo.Value.HkHitInfo.Normal;
        this.m_hitNormal = (Vector3) Base6Directions.GetIntVector(Base6Directions.GetClosestDirection(Vector3.TransformNormal(this.m_hitNormal, this.m_hitEntity.PositionComp.WorldMatrixNormalizedInv)));
        this.m_hitNormal = Vector3.TransformNormal(this.m_hitNormal, this.m_hitEntity.PositionComp.WorldMatrixRef);
        this.m_closestHitDistSq = (float) (this.m_hitPos - pasteMatrix.Translation).LengthSquared();
        return true;
      }
      MyPhysics.CastRay(pasteMatrix.Translation, pasteMatrix.Translation + pasteMatrix.Forward * (double) this.m_dragDistance, this.m_raycastCollisionResults, 15);
      foreach (MyPhysics.HitInfo raycastCollisionResult in this.m_raycastCollisionResults)
      {
        if (!((HkReferenceObject) raycastCollisionResult.HkHitInfo.Body == (HkReferenceObject) null))
        {
          IMyEntity hitEntity = raycastCollisionResult.HkHitInfo.GetHitEntity();
          if (hitEntity != null)
          {
            MyCubeGrid myCubeGrid = hitEntity as MyCubeGrid;
            if ((canPasteLargeOnSmall || this.m_previewGrids.Count == 0 || (this.m_previewGrids[0].GridSizeEnum != MyCubeSize.Large || myCubeGrid == null) || myCubeGrid.GridSizeEnum != MyCubeSize.Small) && (hitEntity is MyVoxelBase || myCubeGrid != null && this.m_previewGrids.Count != 0 && myCubeGrid.EntityId != this.m_previewGrids[0].EntityId))
            {
              float num = (float) (raycastCollisionResult.Position - pasteMatrix.Translation).LengthSquared();
              if ((double) num < (double) this.m_closestHitDistSq)
              {
                this.m_closestHitDistSq = num;
                this.m_hitPos = raycastCollisionResult.Position;
                this.m_hitNormal = raycastCollisionResult.HkHitInfo.Normal;
                this.m_hitEntity = hitEntity;
              }
            }
          }
        }
      }
      this.m_raycastCollisionResults.Clear();
      return true;
    }

    protected virtual void TestBuildingMaterials() => this.m_characterHasEnoughMaterials = this.EntityCanPaste(this.GetClipboardBuilder());

    protected virtual MyEntity GetClipboardBuilder() => (MyEntity) MySession.Static.LocalCharacter;

    public virtual bool EntityCanPaste(MyEntity pastingEntity)
    {
      if (this.m_copiedGrids.Count < 1)
        return false;
      if (MySession.Static.CreativeToolsEnabled(Sync.MyId))
        return true;
      MyCubeBuilder.BuildComponent.GetGridSpawnMaterials(this.m_copiedGrids[0]);
      return MyCubeBuilder.BuildComponent.HasBuildingMaterials(pastingEntity);
    }

    protected void UpdateTouchingGrids(ulong pastingPlayer = 0)
    {
      for (int index = 0; index < this.m_previewGrids.Count; ++index)
      {
        MyCubeGrid previewGrid = this.m_previewGrids[index];
        MyGridPlacementSettings placementSettings = this.m_settings.GetGridPlacementSettings(previewGrid.GridSizeEnum, previewGrid.IsStatic);
        this.GetTouchingGrids(previewGrid, placementSettings, pastingPlayer);
      }
    }

    protected virtual bool TestPlacement()
    {
      this.m_canBePlacedNeedsRefresh = false;
      if (MyFakes.DISABLE_CLIPBOARD_PLACEMENT_TEST)
        return true;
      if (!Sandbox.Game.Entities.MyEntities.IsInsideWorld(this.m_pastePosition))
        return false;
      bool flag1 = true;
      for (int index = 0; index < this.m_previewGrids.Count; ++index)
      {
        MyCubeGrid previewGrid = this.m_previewGrids[index];
        if (!MySessionComponentSafeZones.IsActionAllowed(previewGrid.PositionComp.WorldAABB, MySafeZoneAction.Building, user: Sync.MyId))
          flag1 = false;
        if (flag1)
        {
          if (index == 0 && this.m_hitEntity is MyCubeGrid && (this.IsSnapped && this.SnapMode == SnapMode.Base6Directions))
          {
            MyCubeGrid hitEntity = this.m_hitEntity as MyCubeGrid;
            bool flag2 = hitEntity.GridSizeEnum == MyCubeSize.Large && previewGrid.GridSizeEnum == MyCubeSize.Small;
            MyGridPlacementSettings placementSettings = this.m_settings.GetGridPlacementSettings(previewGrid.GridSizeEnum, previewGrid.IsStatic);
            if (((!MyFakes.ENABLE_STATIC_SMALL_GRID_ON_LARGE ? 0 : (previewGrid.IsStatic ? 1 : 0)) & (flag2 ? 1 : 0)) != 0)
            {
              flag1 &= MyCubeGrid.TestPlacementArea(previewGrid, previewGrid.IsStatic, ref placementSettings, (BoundingBoxD) previewGrid.PositionComp.LocalAABB, false, (MyEntity) hitEntity);
            }
            else
            {
              Vector3I gridInteger = hitEntity.WorldToGridInteger(this.m_pastePosition);
              if (MyDebugDrawSettings.DEBUG_DRAW_COPY_PASTE)
                MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, 60f), "First grid offset: " + gridInteger.ToString(), Color.Red, 1f);
              flag1 = ((flag1 ? 1 : 0) & (hitEntity.GridSizeEnum != previewGrid.GridSizeEnum ? 0 : (hitEntity.CanMergeCubes(previewGrid, gridInteger) ? 1 : 0))) != 0 & MyCubeGrid.CheckMergeConnectivity(hitEntity, previewGrid, gridInteger) & MyCubeGrid.TestPlacementArea(previewGrid, previewGrid.IsStatic, ref placementSettings, (BoundingBoxD) previewGrid.PositionComp.LocalAABB, false, (MyEntity) hitEntity);
            }
          }
          else
          {
            MyGridPlacementSettings placementSettings = this.m_settings.GetGridPlacementSettings(previewGrid.GridSizeEnum, previewGrid.IsStatic);
            flag1 &= MyCubeGrid.TestPlacementArea(previewGrid, previewGrid.IsStatic, ref placementSettings, (BoundingBoxD) previewGrid.PositionComp.LocalAABB, false);
          }
        }
        if (!flag1)
          return false;
      }
      return flag1;
    }

    private void GetTouchingGrids(
      MyCubeGrid grid,
      MyGridPlacementSettings settings,
      ulong pastingPlayer = 0)
    {
      this.m_touchingGrids.Clear();
      foreach (MySlimBlock cubeBlock in grid.CubeBlocks)
      {
        if (cubeBlock.FatBlock is MyCompoundCubeBlock)
        {
          bool flag = false;
          foreach (MySlimBlock block in (cubeBlock.FatBlock as MyCompoundCubeBlock).GetBlocks())
          {
            MyCubeGrid touchingGrid = (MyCubeGrid) null;
            MyCubeGrid.TestPlacementAreaCubeNoAABBInflate(block.CubeGrid, ref settings, block.Min, block.Max, block.Orientation, block.BlockDefinition, out touchingGrid, ignoredEntity: ((MyEntity) block.CubeGrid));
            if (touchingGrid != null)
            {
              this.m_touchingGrids.Add(touchingGrid);
              flag = true;
              break;
            }
          }
          if (flag)
            break;
        }
        else
        {
          MyCubeGrid touchingGrid = (MyCubeGrid) null;
          MyCubeGrid.TestPlacementAreaCubeNoAABBInflate(cubeBlock.CubeGrid, ref settings, cubeBlock.Min, cubeBlock.Max, cubeBlock.Orientation, cubeBlock.BlockDefinition, out touchingGrid, pastingPlayer, (MyEntity) cubeBlock.CubeGrid);
          if (touchingGrid != null)
          {
            this.m_touchingGrids.Add(touchingGrid);
            break;
          }
        }
      }
    }

    protected virtual void UpdateGridTransformations()
    {
      if (this.m_copiedGrids.Count == 0)
        return;
      Matrix orientationMatrix = this.GetFirstGridOrientationMatrix();
      MatrixD matrix1 = this.m_copiedGrids[0].PositionAndOrientation.Value.GetMatrix();
      Matrix matrix2 = Matrix.Invert((Matrix) ref matrix1).GetOrientation() * orientationMatrix;
      for (int index = 0; index < this.m_previewGrids.Count && index <= this.m_copiedGrids.Count - 1; ++index)
      {
        if (this.m_copiedGrids[index].PositionAndOrientation.HasValue)
        {
          MatrixD matrix3 = this.m_copiedGrids[index].PositionAndOrientation.Value.GetMatrix();
          Vector3D normal = matrix3.Translation - (Vector3D) this.m_copiedGrids[0].PositionAndOrientation.Value.Position;
          this.m_copiedGridOffsets[index] = Vector3.TransformNormal(normal, matrix2);
          MatrixD rotationMatrix = matrix3 * matrix2;
          Vector3D vector3D = this.m_pastePosition + this.m_copiedGridOffsets[index];
          rotationMatrix.Translation = (Vector3D) Vector3.Zero;
          MatrixD worldMatrix = MatrixD.Orthogonalize(rotationMatrix);
          worldMatrix.Translation = vector3D;
          this.m_previewGrids[index].PositionComp.SetWorldMatrix(ref worldMatrix, skipTeleportCheck: true);
        }
      }
    }

    protected virtual void UpdatePastePosition()
    {
      if (this.m_previewGrids.Count == 0)
        return;
      this.m_pastePositionPrevious = this.m_pastePosition;
      MatrixD pasteMatrix = MyGridClipboard.GetPasteMatrix();
      this.m_spawnerId = MyGridClipboard.GetPasteSpawnerId();
      this.m_originalSpawnerPosition = MyGridClipboard.GetPasteSpawnerPosition();
      Vector3 vector3 = (Vector3) (pasteMatrix.Forward * (double) this.m_dragDistance);
      if (!this.TrySnapToSurface(this.m_settings.GetGridPlacementSettings(this.m_previewGrids[0].GridSizeEnum).SnapMode))
      {
        this.m_pastePosition = pasteMatrix.Translation + vector3;
        this.m_pastePosition += Vector3.TransformNormal(this.m_dragPointToPositionLocal, this.GetFirstGridOrientationMatrix());
      }
      double gridSize = (double) this.m_previewGrids[0].GridSize;
      MyCoordinateSystem.CoordSystemData closestGrid = MyCoordinateSystem.Static.SnapWorldPosToClosestGrid(ref this.m_pastePosition, gridSize, this.m_settings.StaticGridAlignToCenter);
      if (MyCoordinateSystem.Static.LocalCoordExist && !this.EnableStationRotation)
        this.m_pastePosition = closestGrid.SnappedTransform.Position;
      if (!MyDebugDrawSettings.DEBUG_DRAW_COPY_PASTE)
        return;
      MyRenderProxy.DebugDrawSphere(pasteMatrix.Translation + vector3, 0.15f, (Color) Color.Pink.ToVector3(), depthRead: false);
      MyRenderProxy.DebugDrawSphere(this.m_pastePosition, 0.15f, (Color) Color.Pink.ToVector3(), depthRead: false);
    }

    protected static MatrixD GetPasteMatrix() => MySession.Static.ControlledEntity != null && (MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Entity || MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.ThirdPersonSpectator) ? MySession.Static.ControlledEntity.GetHeadMatrix(true) : MySector.MainCamera.WorldMatrix;

    protected static long? GetPasteSpawnerId() => MySession.Static.ControlledEntity != null && (MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Entity || MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.ThirdPersonSpectator) ? new long?(MySession.Static.ControlledEntity.Entity.EntityId) : new long?();

    public static Vector3D GetPasteSpawnerPosition() => MySession.Static.ControlledEntity != null && (MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Entity || MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.ThirdPersonSpectator) ? MySession.Static.ControlledEntity.Entity.WorldMatrix.Translation : MySector.MainCamera.WorldMatrix.Translation;

    public virtual Matrix GetFirstGridOrientationMatrix() => Matrix.CreateWorld(Vector3.Zero, this.m_pasteDirForward, this.m_pasteDirUp);

    public void AlignClipboardToGravity()
    {
      if (this.PreviewGrids.Count <= 0)
        return;
      this.AlignClipboardToGravity(MyGravityProviderSystem.CalculateNaturalGravityInPoint(this.PreviewGrids[0].WorldMatrix.Translation));
    }

    public void AlignClipboardToGravity(Vector3 gravity)
    {
      if (this.PreviewGrids.Count <= 0 || (double) gravity.LengthSquared() <= 9.99999974737875E-05)
        return;
      double num = (double) gravity.Normalize();
      this.m_pasteDirForward = (Vector3) Vector3D.Reject((Vector3D) this.m_pasteDirForward, (Vector3D) gravity);
      this.m_pasteDirUp = -gravity;
    }

    protected void AlignRotationToCoordSys()
    {
      if (this.m_previewGrids.Count <= 0)
        return;
      double gridSize = (double) this.m_previewGrids[0].GridSize;
      MyCoordinateSystem.CoordSystemData closestGrid = MyCoordinateSystem.Static.SnapWorldPosToClosestGrid(ref this.m_pastePosition, gridSize, this.m_settings.StaticGridAlignToCenter);
      this.m_pastePosition = closestGrid.SnappedTransform.Position;
      this.m_pasteDirForward = closestGrid.SnappedTransform.Rotation.Forward;
      this.m_pasteDirUp = closestGrid.SnappedTransform.Rotation.Up;
      this.m_pasteOrientationAngle = 0.0f;
    }

    protected bool TrySnapToSurface(SnapMode snapMode)
    {
      if ((double) this.m_closestHitDistSq < 3.40282346638529E+38)
      {
        Vector3D hitPos = this.m_hitPos;
        if ((double) this.m_hitNormal.Length() > 0.5 && this.m_hitEntity is MyCubeGrid hitEntity)
        {
          MatrixD matrixD = hitEntity.WorldMatrix;
          matrixD = matrixD.GetOrientation();
          Matrix axisDefinitionMatrix = (Matrix) ref matrixD;
          Matrix orientationMatrix = this.GetFirstGridOrientationMatrix();
          Matrix axes = Matrix.AlignRotationToAxes(ref orientationMatrix, ref axisDefinitionMatrix);
          Matrix matrix = Matrix.Invert(orientationMatrix) * axes;
          this.m_pasteDirForward = axes.Forward;
          this.m_pasteDirUp = axes.Up;
          this.m_pasteOrientationAngle = 0.0f;
        }
        Vector3 vector3 = Vector3.TransformNormal(this.m_dragPointToPositionLocal, this.GetFirstGridOrientationMatrix());
        this.m_pastePosition = hitPos + vector3;
        if (MyDebugDrawSettings.DEBUG_DRAW_COPY_PASTE)
        {
          MyRenderProxy.DebugDrawSphere(hitPos, 0.08f, (Color) Color.Red.ToVector3(), depthRead: false);
          MyRenderProxy.DebugDrawSphere(this.m_pastePosition, 0.08f, (Color) Color.Red.ToVector3(), depthRead: false);
        }
        this.IsSnapped = true;
        return true;
      }
      this.IsSnapped = false;
      return false;
    }

    private void UpdatePreviewBBox()
    {
      if (this.m_previewGrids == null || this.m_isBeingAdded || Sandbox.Engine.Platform.Game.IsDedicated)
        return;
      if (!this.m_visible || !this.HasPreviewBBox)
      {
        foreach (MyEntity previewGrid in this.m_previewGrids)
          Sandbox.Game.Entities.MyEntities.EnableEntityBoundingBoxDraw(previewGrid, false);
      }
      else
      {
        Vector4 vector4 = new Vector4(Color.White.ToVector3(), 1f);
        MyStringId myStringId = MyGridClipboard.ID_GIZMO_DRAW_LINE_RED;
        bool flag = false;
        if (!MySession.Static.IsRunningExperimental)
        {
          foreach (MyCubeGrid previewGrid in this.m_previewGrids)
          {
            if (previewGrid.UnsafeBlocks.Count > 0)
            {
              flag = true;
              break;
            }
          }
        }
        if (this.m_canBePlaced && !flag)
        {
          if (this.m_characterHasEnoughMaterials)
            myStringId = MyGridClipboard.ID_GIZMO_DRAW_LINE;
          else
            vector4 = Color.Gray.ToVector4();
        }
        Vector3 vector3 = new Vector3(0.1f);
        foreach (MyCubeGrid previewGrid in this.m_previewGrids)
          Sandbox.Game.Entities.MyEntities.EnableEntityBoundingBoxDraw((MyEntity) previewGrid, true, new Vector4?(vector4), 0.04f, new Vector3?(vector3), new MyStringId?(myStringId), previewGrid.BlocksCount == 0);
        if (Sync.IsDedicated)
          return;
        StringBuilder stringBuilder1 = MyTexts.Get(MyCommonTexts.Clipboard_TotalPCU);
        StringBuilder stringBuilder2 = MyTexts.Get(MyCommonTexts.Clipboard_TotalBlocks);
        MyGuiManager.DrawString("White", stringBuilder1.ToString() + (object) this.m_copiedGridsInfo.PCUs + "\n" + stringBuilder2.ToString() + (object) this.m_copiedGridsInfo.TotalBlocks, new Vector2(0.51f, 0.51f), 0.7f);
      }
    }

    protected void FixSnapTransformationBase6()
    {
      if (this.m_copiedGrids.Count == 0 || this.m_previewGrids.Count == 0)
        return;
      MyGridClipboard.GetPasteMatrix();
      if (!(this.m_hitEntity is MyCubeGrid hitEntity))
        return;
      if (this.m_copiedGridOffsets.Count < this.m_previewGrids.Count)
      {
        MyLog.Default.Critical(string.Format("SE-19306 : FixSnapTransformationBase6 m_previewGrids={0} m_copiedGrids={1} m_copiedGridOffsets={2}", (object) 0, (object) 1, (object) 2), (object) this.m_previewGrids.Count, (object) this.m_copiedGrids.Count, (object) this.m_copiedGridOffsets.Count);
      }
      else
      {
        MatrixD matrixD = hitEntity.WorldMatrix;
        matrixD = matrixD.GetOrientation();
        Matrix matrix1 = (Matrix) ref matrixD;
        matrixD = this.m_previewGrids[0].WorldMatrix;
        matrixD = matrixD.GetOrientation();
        Matrix matrix2 = (Matrix) ref matrixD;
        Matrix axisDefinitionMatrix = Matrix.Normalize(matrix1);
        Matrix toAlign = Matrix.Normalize(matrix2);
        Matrix axes = Matrix.AlignRotationToAxes(ref toAlign, ref axisDefinitionMatrix);
        Matrix matrix3 = Matrix.Invert(toAlign) * axes;
        foreach (MyCubeGrid previewGrid in this.m_previewGrids)
        {
          matrixD = previewGrid.WorldMatrix;
          matrixD = matrixD.GetOrientation();
          Matrix matrix4 = (Matrix) ref matrixD * matrix3;
          Matrix.Invert(matrix4);
          MatrixD world = MatrixD.CreateWorld(this.m_pastePosition, matrix4.Forward, matrix4.Up);
          previewGrid.PositionComp.SetWorldMatrix(ref world);
        }
        if ((hitEntity.GridSizeEnum != MyCubeSize.Large ? 0 : (this.m_previewGrids[0].GridSizeEnum == MyCubeSize.Small ? 1 : 0)) != 0)
        {
          Vector3 smallGrid = MyCubeBuilder.TransformLargeGridHitCoordToSmallGrid(this.m_pastePosition, hitEntity.PositionComp.WorldMatrixNormalizedInv, hitEntity.GridSize);
          this.m_pastePosition = hitEntity.GridIntegerToWorld((Vector3D) smallGrid);
          if (MyFakes.ENABLE_VR_BUILDING)
          {
            Vector3 normal = (Vector3) Vector3I.Round(Vector3.TransformNormal(this.m_hitNormal, hitEntity.PositionComp.WorldMatrixNormalizedInv));
            Vector3 vector3_1 = normal * (this.m_previewGrids[0].GridSize / hitEntity.GridSize);
            Vector3 vector3_2 = (Vector3) Vector3I.Round(Vector3D.TransformNormal(Vector3D.TransformNormal(normal, hitEntity.WorldMatrix), this.m_previewGrids[0].PositionComp.WorldMatrixNormalizedInv));
            BoundingBox localAabb = this.m_previewGrids[0].PositionComp.LocalAABB;
            localAabb.Min /= this.m_previewGrids[0].GridSize;
            localAabb.Max /= this.m_previewGrids[0].GridSize;
            Vector3 vector3_3 = this.m_dragPointToPositionLocal / this.m_previewGrids[0].GridSize;
            Vector3 zero1 = Vector3.Zero;
            Vector3 zero2 = Vector3.Zero;
            BoundingBox box = new BoundingBox(-Vector3.Half, Vector3.Half);
            box.Inflate(-0.05f);
            box.Translate(-vector3_3 + zero1 - vector3_2);
            while (localAabb.Contains(box) != ContainmentType.Disjoint)
            {
              zero1 -= vector3_2;
              zero2 -= vector3_1;
              box.Translate(-vector3_2);
            }
            this.m_pastePosition = hitEntity.GridIntegerToWorld((Vector3D) (smallGrid - zero2));
          }
        }
        else
        {
          Vector3I vector3I = Vector3I.Round(Vector3.TransformNormal(this.m_hitNormal, hitEntity.PositionComp.WorldMatrixNormalizedInv));
          Vector3I gridInteger = hitEntity.WorldToGridInteger(this.m_pastePosition);
          int num;
          for (num = 0; num < 100 && !hitEntity.CanMergeCubes(this.m_previewGrids[0], gridInteger); ++num)
            gridInteger += vector3I;
          if (num == 0)
          {
            for (num = 0; num < 100; ++num)
            {
              gridInteger -= vector3I;
              if (!hitEntity.CanMergeCubes(this.m_previewGrids[0], gridInteger))
                break;
            }
            gridInteger += vector3I;
          }
          if (num == 100)
            gridInteger = hitEntity.WorldToGridInteger(this.m_pastePosition);
          this.m_pastePosition = hitEntity.GridIntegerToWorld(gridInteger);
        }
        if (MyDebugDrawSettings.DEBUG_DRAW_COPY_PASTE)
          MyRenderProxy.DebugDrawLine3D(this.m_hitPos, this.m_hitPos + this.m_hitNormal, Color.Red, Color.Green, false);
        int num1 = 0;
        foreach (MyCubeGrid previewGrid in this.m_previewGrids)
        {
          MatrixD worldMatrix = previewGrid.WorldMatrix;
          worldMatrix.Translation = this.m_pastePosition + Vector3.Transform(this.m_copiedGridOffsets[num1++], matrix3);
          previewGrid.PositionComp.SetWorldMatrix(ref worldMatrix);
        }
      }
    }

    public void DrawHud()
    {
      if (this.m_previewGrids == null)
        return;
      MyCubeBlockDefinition firstBlockDefinition = this.GetFirstBlockDefinition(this.m_copiedGrids[0]);
      MyHud.BlockInfo.LoadDefinition(firstBlockDefinition);
    }

    public void CalculateRotationHints(MyBlockBuilderRotationHints hints, bool isRotating)
    {
      MyCubeGrid myCubeGrid = this.PreviewGrids.Count > 0 ? this.PreviewGrids[0] : (MyCubeGrid) null;
      if (myCubeGrid == null)
        return;
      MatrixD worldMatrix = myCubeGrid.WorldMatrix;
      Vector3D vector3D = Vector3D.TransformNormal(-this.m_dragPointToPositionLocal, worldMatrix);
      worldMatrix.Translation += vector3D;
      hints.CalculateRotationHints(worldMatrix, !MyHud.MinimalHud && !MyHud.CutsceneHud && MySandboxGame.Config.RotationHints && !MyInput.Static.IsJoystickLastUsed, isRotating, this.OneAxisRotationMode);
    }

    private void RemovePilots(MyObjectBuilder_CubeGrid grid)
    {
      foreach (MyObjectBuilder_CubeBlock cubeBlock in grid.CubeBlocks)
      {
        if (cubeBlock is MyObjectBuilder_Cockpit objectBuilderCockpit)
        {
          objectBuilderCockpit.ClearPilotAndAutopilot();
          if (objectBuilderCockpit.ComponentContainer != null && objectBuilderCockpit.ComponentContainer.Components != null)
          {
            foreach (MyObjectBuilder_ComponentContainer.ComponentData component in objectBuilderCockpit.ComponentContainer.Components)
            {
              if (component.TypeId == typeof (MyHierarchyComponentBase).Name)
              {
                ((MyObjectBuilder_HierarchyComponentBase) component.Component).Children.RemoveAll((Predicate<MyObjectBuilder_EntityBase>) (x => x is MyObjectBuilder_Character));
                break;
              }
            }
          }
          if (objectBuilderCockpit is MyObjectBuilder_CryoChamber builderCryoChamber)
            builderCryoChamber.Clear();
        }
        else if (cubeBlock is MyObjectBuilder_LandingGear builderLandingGear)
        {
          builderLandingGear.IsLocked = false;
          builderLandingGear.MasterToSlave = new MyDeltaTransform?();
          builderLandingGear.AttachedEntityId = new long?();
          builderLandingGear.LockMode = LandingGearMode.Unlocked;
        }
      }
    }

    public bool HasCopiedGrids() => this.m_copiedGrids.Count > 0;

    public string CopiedGridsName => this.HasCopiedGrids() ? this.m_copiedGrids[0].DisplayName : (string) null;

    public void SaveClipboardAsPrefab(string name = null, string path = null)
    {
      if (this.m_copiedGrids.Count == 0)
        return;
      name = name ?? MyWorldGenerator.GetPrefabTypeName((MyObjectBuilder_EntityBase) this.m_copiedGrids[0]) + "_" + (object) MyUtils.GetRandomInt(1000000, 9999999);
      if (path == null)
        MyPrefabManager.SavePrefab(name, this.m_copiedGrids);
      else
        MyPrefabManager.SavePrefabToPath(name, path, this.m_copiedGrids);
      MyHud.Notifications.Add((MyHudNotificationBase) new MyHudNotificationDebug("Prefab saved: " + path, 10000));
    }

    public void HideGridWhenColliding(List<Vector3D> collisionTestPoints)
    {
      if (this.m_previewGrids.Count == 0)
        return;
      bool flag = true;
      foreach (Vector3D collisionTestPoint in collisionTestPoints)
      {
        foreach (MyCubeGrid previewGrid in this.m_previewGrids)
        {
          Vector3D point = Vector3.Transform((Vector3) collisionTestPoint, previewGrid.PositionComp.WorldMatrixNormalizedInv);
          if (previewGrid.PositionComp.LocalAABB.Contains(point) == ContainmentType.Contains)
          {
            flag = false;
            break;
          }
        }
        if (!flag)
          break;
      }
      foreach (MyCubeGrid previewGrid in this.m_previewGrids)
        previewGrid.Render.Visible = flag;
    }

    public void RotateAroundAxis(int axisIndex, int sign, bool newlyPressed, float angleDelta)
    {
      if ((!this.EnableStationRotation ? 1 : (this.IsSnapped ? 1 : 0)) != 0 && !this.EnablePreciseRotationWhenSnapped)
      {
        if (!newlyPressed)
          return;
        angleDelta = 1.570796f;
      }
      switch (axisIndex)
      {
        case 0:
          if (sign < 0)
          {
            this.UpMinus(angleDelta);
            break;
          }
          this.UpPlus(angleDelta);
          break;
        case 1:
          if (sign < 0)
          {
            this.AngleMinus(angleDelta);
            break;
          }
          this.AnglePlus(angleDelta);
          break;
        case 2:
          if (sign < 0)
          {
            this.RightPlus(angleDelta);
            break;
          }
          this.RightMinus(angleDelta);
          break;
      }
      this.ApplyOrientationAngle();
    }

    private void AnglePlus(float angle)
    {
      this.m_pasteOrientationAngle += angle;
      if ((double) this.m_pasteOrientationAngle < 6.28318548202515)
        return;
      this.m_pasteOrientationAngle -= 6.283185f;
    }

    private void AngleMinus(float angle)
    {
      this.m_pasteOrientationAngle -= angle;
      if ((double) this.m_pasteOrientationAngle >= 0.0)
        return;
      this.m_pasteOrientationAngle += 6.283185f;
    }

    private void UpPlus(float angle)
    {
      if (this.OneAxisRotationMode)
        return;
      this.ApplyOrientationAngle();
      Vector3.Cross(this.m_pasteDirForward, this.m_pasteDirUp);
      float num1 = (float) Math.Cos((double) angle);
      float num2 = (float) Math.Sin((double) angle);
      Vector3 vector3 = this.m_pasteDirUp * num1 - this.m_pasteDirForward * num2;
      this.m_pasteDirForward = this.m_pasteDirUp * num2 + this.m_pasteDirForward * num1;
      this.m_pasteDirUp = vector3;
    }

    private void UpMinus(float angle) => this.UpPlus(-angle);

    private void RightPlus(float angle)
    {
      if (this.OneAxisRotationMode)
        return;
      this.ApplyOrientationAngle();
      Vector3 vector3 = Vector3.Cross(this.m_pasteDirForward, this.m_pasteDirUp);
      float num1 = (float) Math.Cos((double) angle);
      float num2 = (float) Math.Sin((double) angle);
      this.m_pasteDirUp = this.m_pasteDirUp * num1 + vector3 * num2;
    }

    private void RightMinus(float angle) => this.RightPlus(-angle);

    public virtual void MoveEntityFurther() => this.m_dragDistance = MathHelper.Clamp(this.m_dragDistance * 1.1f, this.m_dragDistance, 20000f);

    public virtual void MoveEntityCloser() => this.m_dragDistance /= 1.1f;

    private void ApplyOrientationAngle()
    {
      this.m_pasteDirForward = Vector3.Normalize(this.m_pasteDirForward);
      this.m_pasteDirUp = Vector3.Normalize(this.m_pasteDirUp);
      Vector3 vector3 = Vector3.Cross(this.m_pasteDirForward, this.m_pasteDirUp);
      float num1 = (float) Math.Cos((double) this.m_pasteOrientationAngle);
      float num2 = (float) Math.Sin((double) this.m_pasteOrientationAngle);
      this.m_pasteDirForward = this.m_pasteDirForward * num1 - vector3 * num2;
      this.m_pasteOrientationAngle = 0.0f;
    }

    private struct GridCopy
    {
      private MyObjectBuilder_CubeGrid Grid;
      private Vector3 Offset;
      private Quaternion Rotation;
    }

    private struct CopiedGridInfo
    {
      public int PCUs;
      public int TotalBlocks;
      public bool HasGridOverLimits;
    }

    protected delegate void UpdateAfterPasteCallback(List<MyObjectBuilder_CubeGrid> pastedBuilders);
  }
}
