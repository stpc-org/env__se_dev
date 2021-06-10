// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MySessionComponentVoxelHand
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Components;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game.Utils;
using VRage.Input;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.SessionComponents
{
  [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
  public class MySessionComponentVoxelHand : MySessionComponentBase
  {
    private IMyVoxelBrush[] m_brushes;
    public static MySessionComponentVoxelHand Static;
    internal const float VOXEL_SIZE = 1f;
    internal const float VOXEL_HALF = 0.5f;
    internal static float GRID_SIZE = MyDefinitionManager.Static.GetCubeSize(MyCubeSize.Large);
    internal static float SCALE_MAX = MyDefinitionManager.Static.GetCubeSize(MyCubeSize.Large) * 10f;
    internal static float MIN_BRUSH_ZOOM = MySessionComponentVoxelHand.GRID_SIZE;
    internal static float MAX_BRUSH_ZOOM = MySessionComponentVoxelHand.GRID_SIZE * 20f;
    private static float DEG_IN_RADIANS = MathHelper.ToRadians(1f);
    private static readonly float GAMEPAD_ZOOM_SPEED = 1f;
    private byte m_selectedMaterial;
    private int m_materialCount;
    private float m_position;
    public MatrixD m_rotation;
    private MyVoxelBase m_currentVoxelMap;
    private readonly List<MyVoxelBase> m_previousVoxelMaps = new List<MyVoxelBase>();
    private readonly List<MyVoxelBase> m_currentVoxelMaps = new List<MyVoxelBase>();
    private MyGuiCompositeTexture m_texture;
    public Color ShapeColor;
    private MyHudNotification m_safezoneNotification;
    private MyHudNotification m_noVoxelMapNotification;
    private int m_selectedAxis;
    private bool m_showAxis;
    private bool m_buildMode;
    private bool m_enabled;
    private bool m_editing;
    private int m_currentBrushIndex = -1;
    private MyHudNotification m_voxelMaterialHint;
    private MyHudNotification m_voxelSettingsHint;
    private bool m_placeRepressed;
    private static List<MyEntity> m_foundElements = new List<MyEntity>();

    public override Type[] Dependencies => new Type[2]
    {
      typeof (MyToolbarComponent),
      typeof (MyCubeBuilder)
    };

    public bool BuildMode
    {
      private set
      {
        this.m_buildMode = value;
        MyHud.IsBuildMode = value;
        if (value)
          this.ActivateHudBuildModeNotifications();
        else
          this.DeactivateHudBuildModeNotifications();
      }
      get => this.m_buildMode;
    }

    public bool Enabled
    {
      get => this.m_enabled;
      set
      {
        if (this.m_enabled == value)
          return;
        if (value)
          this.Activate();
        else
          this.Deactivate();
        this.m_enabled = value;
        if (this.OnEnabledChanged == null)
          return;
        this.OnEnabledChanged();
      }
    }

    public bool SnapToVoxel { get; set; }

    public bool ProjectToVoxel { get; set; }

    public bool ShowGizmos { get; set; }

    public bool FreezePhysics { get; set; }

    public IMyVoxelBrush CurrentShape { get; set; }

    public MyVoxelHandDefinition CurrentDefinition { get; set; }

    public event Action OnEnabledChanged;

    public event Action OnBrushChanged;

    public MySessionComponentVoxelHand()
    {
      MySessionComponentVoxelHand.Static = this;
      this.SnapToVoxel = true;
      this.ShowGizmos = true;
      this.ShapeColor = (Color) new Vector4(0.6f, 0.6f, 0.6f, 0.25f);
      this.m_selectedMaterial = (byte) 1;
      this.CurrentMaterialTextureName = "Textures\\GUI\\Icons\\RadialMenu_Voxel\\Stone2.png";
      this.m_materialCount = MyDefinitionManager.Static.VoxelMaterialCount;
      this.m_position = MySessionComponentVoxelHand.MIN_BRUSH_ZOOM * 2f;
      this.m_rotation = MatrixD.Identity;
      this.m_texture = new MyGuiCompositeTexture();
      MyVoxelMaterialDefinition materialDefinition1 = MyDefinitionManager.Static.GetVoxelMaterialDefinition(this.m_selectedMaterial);
      if (materialDefinition1 is MyDx11VoxelMaterialDefinition materialDefinition2)
        this.m_texture.Center = new MyGuiSizedTexture()
        {
          Texture = materialDefinition2.VoxelHandPreview
        };
      else
        this.m_texture.Center = new MyGuiSizedTexture()
        {
          Texture = materialDefinition1.VoxelHandPreview
        };
    }

    public override void LoadData()
    {
      base.LoadData();
      MyToolbarComponent.CurrentToolbar.SelectedSlotChanged += new Action<MyToolbar, MyToolbar.SlotArgs>(this.CurrentToolbar_SelectedSlotChanged);
      MyToolbarComponent.CurrentToolbar.SlotActivated += new Action<MyToolbar, MyToolbar.SlotArgs, bool>(this.CurrentToolbar_SlotActivated);
      MyToolbarComponent.CurrentToolbar.Unselected += new Action<MyToolbar>(this.CurrentToolbar_Unselected);
      MyCubeBuilder.Static.OnActivated += new Action(this.OnCubeBuilderActivated);
      this.InitializeHints();
    }

    protected override void UnloadData()
    {
      MyToolbarComponent.CurrentToolbar.Unselected -= new Action<MyToolbar>(this.CurrentToolbar_Unselected);
      MyToolbarComponent.CurrentToolbar.SlotActivated -= new Action<MyToolbar, MyToolbar.SlotArgs, bool>(this.CurrentToolbar_SlotActivated);
      MyToolbarComponent.CurrentToolbar.SelectedSlotChanged -= new Action<MyToolbar, MyToolbar.SlotArgs>(this.CurrentToolbar_SelectedSlotChanged);
      MyCubeBuilder.Static.OnActivated -= new Action(this.OnCubeBuilderActivated);
      base.UnloadData();
      MySessionComponentVoxelHand.Static = (MySessionComponentVoxelHand) null;
    }

    private void InitializeHints()
    {
      string str1 = "[" + (object) MyInput.Static.GetGameControl(MyControlsSpace.SWITCH_LEFT) + "]";
      string str2 = "[" + (object) MyInput.Static.GetGameControl(MyControlsSpace.SWITCH_RIGHT) + "]";
      this.m_voxelMaterialHint = MyHudNotifications.CreateControlNotification(MyCommonTexts.NotificationVoxelMaterialFormat, (object) str1, (object) str2);
      this.m_voxelSettingsHint = MyHudNotifications.CreateControlNotification(MyCommonTexts.NotificationVoxelHandHintFormat, (object) ("[" + (object) MyInput.Static.GetGameControl(MyControlsSpace.VOXEL_HAND_SETTINGS) + "]"));
    }

    private void CurrentToolbar_SelectedSlotChanged(MyToolbar toolbar, MyToolbar.SlotArgs args)
    {
      if (toolbar.SelectedItem is MyToolbarItemVoxelHand || !this.Enabled)
        return;
      this.Enabled = false;
    }

    private void CurrentToolbar_SlotActivated(
      MyToolbar toolbar,
      MyToolbar.SlotArgs args,
      bool userActivated)
    {
      if (toolbar.GetItemAtIndex(toolbar.SlotToIndex(args.SlotNumber.Value)) is MyToolbarItemVoxelHand || !this.Enabled)
        return;
      this.Enabled = false;
    }

    private void CurrentToolbar_Unselected(MyToolbar toolbar)
    {
      if (!this.Enabled)
        return;
      this.Enabled = false;
    }

    private void OnCubeBuilderActivated()
    {
      if (!this.Enabled)
        return;
      this.Enabled = false;
    }

    public override void HandleInput()
    {
      if (!this.Enabled || !(MyScreenManager.GetScreenWithFocus() is MyGuiScreenGamePlay))
        return;
      if (!MySession.Static.CreativeMode && !MySession.Static.IsUserAdmin(Sync.MyId))
      {
        this.Enabled = false;
      }
      else
      {
        base.HandleInput();
        if (MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.IsDead)
        {
          this.Enabled = false;
        }
        else
        {
          MyStringId axVoxel = MySpaceBindingCreator.AX_VOXEL;
          if (MyControllerHelper.IsControl(axVoxel, MyControlsSpace.SLOT0))
          {
            this.Enabled = false;
          }
          else
          {
            if (MyControllerHelper.IsControl(axVoxel, MyControlsSpace.VOXEL_HAND_SETTINGS))
              MyScreenManager.AddScreen((MyGuiScreenBase) new MyGuiScreenVoxelHandSetting());
            if (MyControllerHelper.IsControl(axVoxel, MyControlsSpace.CHANGE_ROTATION_AXIS))
            {
              this.m_selectedAxis = (this.m_selectedAxis + 1) % 3;
              this.m_showAxis = true;
            }
            if (MyControllerHelper.IsControl(axVoxel, MyControlsSpace.ROTATE_AXIS_LEFT, MyControlStateType.PRESSED))
            {
              MatrixD result;
              this.CreateRotationMatrix(ref this.m_rotation, this.m_selectedAxis, (double) MySessionComponentVoxelHand.DEG_IN_RADIANS, out result);
              this.m_rotation *= result;
              this.m_showAxis = true;
            }
            if (MyControllerHelper.IsControl(axVoxel, MyControlsSpace.ROTATE_AXIS_RIGHT, MyControlStateType.PRESSED))
            {
              MatrixD result;
              this.CreateRotationMatrix(ref this.m_rotation, this.m_selectedAxis, -(double) MySessionComponentVoxelHand.DEG_IN_RADIANS, out result);
              this.m_rotation *= result;
              this.m_showAxis = true;
            }
            MyControllerHelper.IsControl(axVoxel, MyControlsSpace.NEXT_BLOCK_STAGE);
            if (MyControllerHelper.IsControl(axVoxel, MyControlsSpace.CUBE_ROTATE_HORISONTAL_POSITIVE, MyControlStateType.PRESSED))
              this.m_rotation *= MatrixD.CreateFromAxisAngle(this.m_rotation.Forward, (double) MySessionComponentVoxelHand.DEG_IN_RADIANS);
            else if (MyControllerHelper.IsControl(axVoxel, MyControlsSpace.CUBE_ROTATE_HORISONTAL_NEGATIVE, MyControlStateType.PRESSED))
              this.m_rotation *= MatrixD.CreateFromAxisAngle(this.m_rotation.Forward, -(double) MySessionComponentVoxelHand.DEG_IN_RADIANS);
            else if (MyControllerHelper.IsControl(axVoxel, MyControlsSpace.CUBE_ROTATE_VERTICAL_NEGATIVE, MyControlStateType.PRESSED))
              this.m_rotation *= MatrixD.CreateFromAxisAngle(this.m_rotation.Up, -(double) MySessionComponentVoxelHand.DEG_IN_RADIANS);
            else if (MyControllerHelper.IsControl(axVoxel, MyControlsSpace.CUBE_ROTATE_VERTICAL_POSITIVE, MyControlStateType.PRESSED))
              this.m_rotation *= MatrixD.CreateFromAxisAngle(this.m_rotation.Up, (double) MySessionComponentVoxelHand.DEG_IN_RADIANS);
            else if (MyControllerHelper.IsControl(axVoxel, MyControlsSpace.CUBE_ROTATE_ROLL_NEGATIVE, MyControlStateType.PRESSED))
              this.m_rotation *= MatrixD.CreateFromAxisAngle(this.m_rotation.Right, -(double) MySessionComponentVoxelHand.DEG_IN_RADIANS);
            else if (MyControllerHelper.IsControl(axVoxel, MyControlsSpace.CUBE_ROTATE_ROLL_POSITIVE, MyControlStateType.PRESSED))
              this.m_rotation *= MatrixD.CreateFromAxisAngle(this.m_rotation.Right, (double) MySessionComponentVoxelHand.DEG_IN_RADIANS);
            if (this.CurrentShape != null)
              this.CurrentShape.SetRotation(ref this.m_rotation);
            if (MyControllerHelper.IsControl(axVoxel, MyControlsSpace.SWITCH_LEFT))
              this.SetMaterial(this.m_selectedMaterial, false);
            else if (MyControllerHelper.IsControl(axVoxel, MyControlsSpace.SWITCH_RIGHT))
              this.SetMaterial(this.m_selectedMaterial);
            if (this.CurrentShape is MyBrushAutoLevel currentShape)
            {
              if (MyControllerHelper.IsControl(axVoxel, MyControlsSpace.PRIMARY_TOOL_ACTION) || MyControllerHelper.IsControl(axVoxel, MyControlsSpace.SECONDARY_TOOL_ACTION))
                currentShape.FixAxis();
              else if (MyControllerHelper.IsControl(axVoxel, MyControlsSpace.PRIMARY_TOOL_ACTION, MyControlStateType.NEW_RELEASED) || MyControllerHelper.IsControl(axVoxel, MyControlsSpace.SECONDARY_TOOL_ACTION, MyControlStateType.NEW_RELEASED))
                currentShape.UnFix();
            }
            if (MyControllerHelper.IsControl(axVoxel, MyControlsSpace.PRIMARY_TOOL_ACTION))
              this.m_placeRepressed = true;
            if (MyControllerHelper.IsControl(axVoxel, MyControlsSpace.VOXEL_PLACE_DUMMY_RELEASE, MyControlStateType.NEW_RELEASED))
              this.m_placeRepressed = false;
            bool flag = false;
            if (MyControllerHelper.IsControl(axVoxel, MyControlsSpace.PRIMARY_TOOL_ACTION, MyControlStateType.PRESSED) && this.m_placeRepressed)
            {
              if (this.m_currentVoxelMap == null)
              {
                this.ShowNoVoxelMapNotification();
                return;
              }
              flag = this.DoPrimaryToolAction();
            }
            else if (MyInput.Static.IsMiddleMousePressed() || MyControllerHelper.IsControl(axVoxel, MyControlsSpace.VOXEL_PAINT, MyControlStateType.PRESSED))
            {
              if (this.m_currentVoxelMap == null)
              {
                this.ShowNoVoxelMapNotification();
                return;
              }
              this.DoVoxelPaintAction();
            }
            else if (MyControllerHelper.IsControl(axVoxel, MyControlsSpace.SECONDARY_TOOL_ACTION, MyControlStateType.PRESSED))
            {
              if (this.m_currentVoxelMap == null)
              {
                this.ShowNoVoxelMapNotification();
                return;
              }
              flag = this.DoSecondaryToolAction();
            }
            else if (MyInput.Static.IsRightMousePressed() && MyInput.Static.IsAnyCtrlKeyPressed() || MyControllerHelper.IsControl(axVoxel, MyControlsSpace.VOXEL_REVERT, MyControlStateType.PRESSED))
              flag = this.DoVoxelRevertAction();
            if (this.CurrentShape != null)
            {
              if (MyControllerHelper.IsControl(axVoxel, MyControlsSpace.VOXEL_SCALE_UP))
                this.CurrentShape.ScaleShapeUp();
              else if (MyControllerHelper.IsControl(axVoxel, MyControlsSpace.VOXEL_SCALE_DOWN))
                this.CurrentShape.ScaleShapeDown();
              int num1 = Math.Sign(MyInput.Static.DeltaMouseScrollWheelValue());
              if (num1 != 0 && MyInput.Static.IsAnyCtrlKeyPressed())
              {
                float num2 = (float) this.CurrentShape.GetBoundaries().HalfExtents.Length() * 0.5f;
                this.SetBrushZoom(this.m_position + (float) num1 * num2);
              }
              if (MyControllerHelper.IsControl(axVoxel, MyControlsSpace.VOXEL_FURTHER, MyControlStateType.PRESSED))
                this.SetBrushZoom(this.m_position + (float) this.CurrentShape.GetBoundaries().HalfExtents.Length() * 0.5f * MySessionComponentVoxelHand.GAMEPAD_ZOOM_SPEED);
              else if (MyControllerHelper.IsControl(axVoxel, MyControlsSpace.VOXEL_CLOSER, MyControlStateType.PRESSED))
                this.SetBrushZoom(this.m_position + (float) this.CurrentShape.GetBoundaries().HalfExtents.Length() * 0.5f * -MySessionComponentVoxelHand.GAMEPAD_ZOOM_SPEED);
            }
            if (this.m_editing == flag)
              return;
            for (int index = 0; index < this.m_currentVoxelMaps.Count; ++index)
            {
              MyVoxelPhysicsBody physics = (MyVoxelPhysicsBody) this.m_currentVoxelMaps[index].Physics;
              if (physics != null)
                physics.QueueInvalidate = flag;
            }
            this.m_editing = flag;
          }
        }
      }
    }

    private bool DoVoxelRevertAction()
    {
      if (this.CurrentShape == null || this.m_currentVoxelMap == null)
        return false;
      bool flag = false;
      for (int index = 0; index < this.m_currentVoxelMaps.Count; ++index)
      {
        MyVoxelPhysicsBody physics = (MyVoxelPhysicsBody) this.m_currentVoxelMaps[index].Physics;
        if (physics != null)
          physics.QueueInvalidate = flag = this.FreezePhysics;
      }
      if (MySessionComponentSafeZones.IsActionAllowed(this.CurrentShape.GetWorldBoundaries(), MySafeZoneAction.VoxelHand))
      {
        if (this.m_currentVoxelMap.Storage.DeleteSupported)
          this.CurrentShape.Revert(this.m_currentVoxelMap);
      }
      else
        this.ShowSafeZoneNotification();
      return flag;
    }

    private bool DoSecondaryToolAction()
    {
      if (this.CurrentShape == null)
        return false;
      bool flag = false;
      for (int index = 0; index < this.m_currentVoxelMaps.Count; ++index)
      {
        MyVoxelPhysicsBody physics = (MyVoxelPhysicsBody) this.m_currentVoxelMaps[index].Physics;
        if (physics != null)
          physics.QueueInvalidate = flag = this.FreezePhysics;
      }
      if (MySessionComponentSafeZones.IsActionAllowed(this.CurrentShape.GetWorldBoundaries(), MySafeZoneAction.VoxelHand, user: Sync.MyId))
      {
        if (MyInput.Static.IsAnyCtrlKeyPressed())
        {
          for (int index = 0; index < this.m_currentVoxelMaps.Count; ++index)
          {
            if (this.m_currentVoxelMaps[index].Storage.DeleteSupported)
              this.CurrentShape.Revert(this.m_currentVoxelMaps[index]);
          }
        }
        else
        {
          for (int index = 0; index < this.m_currentVoxelMaps.Count; ++index)
            this.CurrentShape.CutOut(this.m_currentVoxelMaps[index]);
        }
      }
      else
        this.ShowSafeZoneNotification();
      return flag;
    }

    private void DoVoxelPaintAction()
    {
      if (this.CurrentShape == null)
        return;
      if (MySessionComponentSafeZones.IsActionAllowed(this.CurrentShape.GetWorldBoundaries(), MySafeZoneAction.VoxelHand, user: Sync.MyId))
      {
        for (int index = 0; index < this.m_currentVoxelMaps.Count; ++index)
          this.CurrentShape.Paint(this.m_currentVoxelMaps[index], this.m_selectedMaterial);
      }
      else
        this.ShowSafeZoneNotification();
    }

    private bool DoPrimaryToolAction()
    {
      bool flag = false;
      if (this.CurrentShape == null)
        return false;
      for (int index = 0; index < this.m_currentVoxelMaps.Count; ++index)
      {
        MyVoxelPhysicsBody physics = (MyVoxelPhysicsBody) this.m_currentVoxelMaps[index].Physics;
        if (physics != null)
          physics.QueueInvalidate = flag = this.FreezePhysics;
      }
      if (MySessionComponentSafeZones.IsActionAllowed(this.CurrentShape.GetWorldBoundaries(), MySafeZoneAction.VoxelHand, user: Sync.MyId))
      {
        for (int index = 0; index < this.m_currentVoxelMaps.Count; ++index)
          this.CurrentShape.Fill(this.m_currentVoxelMaps[index], this.m_selectedMaterial);
      }
      else
        this.ShowSafeZoneNotification();
      return flag;
    }

    internal bool IsBrushRotationEnabled() => this.CurrentShape != null && this.CurrentShape.ShowRotationGizmo();

    protected void CreateRotationMatrix(
      ref MatrixD source,
      int index,
      double angleDelta,
      out MatrixD result)
    {
      Vector3D axis;
      switch (index)
      {
        case 0:
          axis = source.Right;
          break;
        case 1:
          axis = source.Up;
          break;
        case 2:
          axis = source.Forward;
          break;
        default:
          axis = source.Right;
          break;
      }
      result = MatrixD.CreateFromAxisAngle(axis, angleDelta);
    }

    public string CurrentMaterialTextureName { get; set; }

    public float GetBrushZoom() => this.m_position;

    public void SetBrushZoom(float value) => this.m_position = MathHelper.Clamp(value, MySessionComponentVoxelHand.MIN_BRUSH_ZOOM, MySessionComponentVoxelHand.MAX_BRUSH_ZOOM);

    public void SetMaterial(SerializableDefinitionId id)
    {
      MyVoxelMaterialDefinition materialDefinition1 = MyDefinitionManager.Static.GetVoxelMaterialDefinition(id.SubtypeId);
      if (materialDefinition1 == null)
        return;
      this.m_selectedMaterial = MyDefinitionManager.Static.GetVoxelMaterialDefinitionIndex(id.SubtypeId);
      if (materialDefinition1 is MyDx11VoxelMaterialDefinition materialDefinition2)
        this.m_texture.Center = new MyGuiSizedTexture()
        {
          Texture = materialDefinition2.VoxelHandPreview
        };
      else
        this.m_texture.Center = new MyGuiSizedTexture()
        {
          Texture = materialDefinition1.VoxelHandPreview
        };
    }

    private void SetMaterial(byte idx, bool next = true)
    {
      int num;
      if (!next)
        num = (int) --idx;
      else
        idx = (byte) (num = (int) (byte) ((uint) idx + 1U));
      idx = (byte) num;
      if (idx == byte.MaxValue)
        idx = (byte) (this.m_materialCount - 1);
      this.m_selectedMaterial = (byte) ((uint) idx % (uint) this.m_materialCount);
      MyVoxelMaterialDefinition materialDefinition1 = MyDefinitionManager.Static.GetVoxelMaterialDefinition(this.m_selectedMaterial);
      if (materialDefinition1.Id.SubtypeName == "BrownMaterial" || materialDefinition1.Id.SubtypeName == "DebugMaterial")
        this.SetMaterial(idx, next);
      else if (materialDefinition1 is MyDx11VoxelMaterialDefinition materialDefinition2)
        this.m_texture.Center = new MyGuiSizedTexture()
        {
          Texture = materialDefinition2.VoxelHandPreview
        };
      else
        this.m_texture.Center = new MyGuiSizedTexture()
        {
          Texture = materialDefinition1.VoxelHandPreview
        };
    }

    public override void UpdateBeforeSimulation()
    {
      if (!this.Enabled)
        return;
      base.UpdateBeforeSimulation();
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      if (localCharacter == null)
        return;
      if (localCharacter.ControllerInfo.Controller == null)
      {
        this.Enabled = false;
      }
      else
      {
        MyCamera mainCamera = MySector.MainCamera;
        if (mainCamera == null)
          return;
        Vector3D from = MySession.Static.IsCameraUserControlledSpectator() ? mainCamera.Position : localCharacter.GetHeadMatrix(true, true, false, false, false).Translation;
        Vector3D vector3D1 = from;
        Vector3D forwardVector = (Vector3D) mainCamera.ForwardVector;
        BoundingBoxD boundingBoxD = this.CurrentShape.GetBoundaries();
        boundingBoxD = boundingBoxD.TransformFast(mainCamera.ViewMatrix);
        double num = Math.Max(2.0 * boundingBoxD.HalfExtents.Z, (double) this.m_position);
        Vector3D vector3D2 = forwardVector * num;
        Vector3D targetPosition = vector3D1 + vector3D2;
        MyVoxelBase currentVoxelMap = this.m_currentVoxelMap;
        BoundingBoxD boundingBox = this.CurrentShape.PeekWorldBoundingBox(ref targetPosition);
        this.m_currentVoxelMap = MySession.Static.VoxelMaps.GetVoxelMapWhoseBoundingBoxIntersectsBox(ref boundingBox, (MyVoxelBase) null);
        this.m_previousVoxelMaps.Clear();
        this.m_previousVoxelMaps.AddRange((IEnumerable<MyVoxelBase>) this.m_currentVoxelMaps);
        this.m_currentVoxelMaps.Clear();
        MySession.Static.VoxelMaps.GetVoxelMapsWhoseBoundingBoxesIntersectBox(ref boundingBox, (MyVoxelBase) null, this.m_currentVoxelMaps);
        if (this.ProjectToVoxel && this.m_currentVoxelMap != null)
        {
          List<MyPhysics.HitInfo> toList = new List<MyPhysics.HitInfo>();
          MyPhysics.CastRay(from, from + mainCamera.ForwardVector * this.m_currentVoxelMap.SizeInMetres, toList, 11);
          bool flag = false;
          foreach (MyPhysics.HitInfo hitInfo in toList)
          {
            IMyEntity hitEntity = hitInfo.HkHitInfo.GetHitEntity();
            if (hitEntity is MyVoxelBase && ((MyVoxelBase) hitEntity).RootVoxel == this.m_currentVoxelMap.RootVoxel)
            {
              targetPosition = hitInfo.Position;
              this.m_currentVoxelMap = (MyVoxelBase) hitEntity;
              flag = true;
              break;
            }
          }
          if (!flag)
            this.m_currentVoxelMap = (MyVoxelBase) null;
        }
        if (currentVoxelMap != this.m_currentVoxelMap && currentVoxelMap != null && currentVoxelMap.Physics != null)
          ((MyVoxelPhysicsBody) currentVoxelMap.Physics).QueueInvalidate = false;
        foreach (MyVoxelBase previousVoxelMap in this.m_previousVoxelMaps)
        {
          if (!this.m_currentVoxelMaps.Contains(previousVoxelMap) && previousVoxelMap.Physics != null)
            ((MyVoxelPhysicsBody) previousVoxelMap.Physics).QueueInvalidate = false;
        }
        if (this.m_currentVoxelMap == null)
          return;
        this.m_currentVoxelMap = this.m_currentVoxelMap.RootVoxel;
        int count = this.m_currentVoxelMaps.Count;
        if (count > 0)
        {
          for (int index = 0; index < count; ++index)
            this.m_currentVoxelMaps.Add(this.m_currentVoxelMaps[index].RootVoxel);
          this.m_currentVoxelMaps.RemoveRange(0, count);
        }
        if (this.SnapToVoxel)
        {
          Vector3D worldPosition = targetPosition + 0.5f;
          Vector3I voxelCoord;
          MyVoxelCoordSystems.WorldPositionToVoxelCoord(this.m_currentVoxelMap.PositionLeftBottomCorner, ref worldPosition, out voxelCoord);
          MyVoxelCoordSystems.VoxelCoordToWorldPosition(this.m_currentVoxelMap.PositionLeftBottomCorner, ref voxelCoord, out worldPosition);
          this.CurrentShape.SetPosition(ref worldPosition);
        }
        else
          this.CurrentShape.SetPosition(ref targetPosition);
      }
    }

    public override void Draw()
    {
      if (!this.Enabled || this.m_currentVoxelMap == null)
        return;
      base.Draw();
      MySessionComponentVoxelHand.m_foundElements.Clear();
      BoundingBoxD worldAabb = this.m_currentVoxelMap.PositionComp.WorldAABB;
      Color color1 = new Color(0.2f, 0.0f, 0.0f, 0.2f);
      if (this.ShowGizmos)
      {
        if (MyFakes.SHOW_FORBIDDEN_ENITIES_VOXEL_HAND)
        {
          MyEntities.GetElementsInBox(ref worldAabb, MySessionComponentVoxelHand.m_foundElements);
          foreach (MyEntity foundElement in MySessionComponentVoxelHand.m_foundElements)
          {
            if (!(foundElement is MyCharacter) && MyVoxelBase.IsForbiddenEntity(foundElement))
            {
              MatrixD worldMatrix = foundElement.PositionComp.WorldMatrixRef;
              BoundingBoxD localAabb = (BoundingBoxD) foundElement.PositionComp.LocalAABB;
              MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref localAabb, ref color1, MySimpleObjectRasterizer.SolidAndWireframe, 0, blendType: MyBillboard.BlendTypeEnum.LDR);
            }
          }
        }
        if (MyFakes.SHOW_CURRENT_VOXEL_MAP_AABB_IN_VOXEL_HAND)
        {
          BoundingBoxD localAabb = (BoundingBoxD) this.m_currentVoxelMap.PositionComp.LocalAABB;
          Color color2 = (Color) new Vector4(0.0f, 0.2f, 0.0f, 0.1f);
          MatrixD worldMatrix = this.m_currentVoxelMap.PositionComp.WorldMatrixRef;
          MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref localAabb, ref color2, MySimpleObjectRasterizer.Solid, 0, blendType: MyBillboard.BlendTypeEnum.LDR);
        }
      }
      this.CurrentShape.Draw(ref this.ShapeColor);
      ConditionBase visibleCondition = MyHud.HudDefinition.Toolbar.VisibleCondition;
      if (visibleCondition != null && visibleCondition.Eval() && (MyGuiScreenHudSpace.Static != null && MyGuiScreenHudSpace.Static.Visible))
        this.DrawMaterial();
      if (!this.m_showAxis || this.CurrentShape == null || !this.CurrentShape.ShowRotationGizmo())
        return;
      this.DrawRotationAxis(this.m_selectedAxis);
    }

    private void DrawRotationAxis(int axis)
    {
      Vector3D vector3D = Vector3D.Zero;
      Color color = Color.White;
      switch (axis)
      {
        case 0:
          vector3D = this.m_rotation.Left;
          color = Color.Red;
          break;
        case 1:
          vector3D = this.m_rotation.Up;
          color = Color.Green;
          break;
        case 2:
          vector3D = this.m_rotation.Forward;
          color = Color.Blue;
          break;
      }
      Vector3D position = this.CurrentShape.GetPosition();
      MyRenderProxy.DebugDrawLine3D(position + vector3D, position - vector3D, color, color, false);
    }

    public void DrawMaterial()
    {
      MyObjectBuilder_ToolbarControlVisualStyle toolbar = MyHud.HudDefinition.Toolbar;
      Vector2 voxelHandPosition = toolbar.ColorPanelStyle.VoxelHandPosition;
      Vector2 size1 = toolbar.ColorPanelStyle.Size;
      Vector2 size2 = new Vector2(size1.X, size1.Y);
      this.m_texture.Draw(voxelHandPosition, size2, Color.White);
      Vector2 vector2 = new Vector2(size1.X + 0.005f, -0.0018f);
      MyGuiManager.DrawString("White", string.Format("{1}", (object) MyTexts.GetString(MyCommonTexts.VoxelHandSettingScreen_HandMaterial), (object) MyDefinitionManager.Static.GetVoxelMaterialDefinition(this.m_selectedMaterial).Id.SubtypeName), voxelHandPosition + vector2, 0.5f);
    }

    private void ActivateHudNotifications()
    {
      if (!MySession.Static.CreativeMode || MyInput.Static.IsJoystickConnected() && MyInput.Static.IsJoystickLastUsed)
        return;
      MyHud.Notifications.Add((MyHudNotificationBase) this.m_voxelMaterialHint);
      MyHud.Notifications.Add((MyHudNotificationBase) this.m_voxelSettingsHint);
    }

    private void DeactivateHudNotifications()
    {
      if (!MySession.Static.CreativeMode)
        return;
      MyHud.Notifications.Remove((MyHudNotificationBase) this.m_voxelMaterialHint);
      MyHud.Notifications.Remove((MyHudNotificationBase) this.m_voxelSettingsHint);
    }

    private void ActivateHudBuildModeNotifications()
    {
      if (!MySession.Static.CreativeMode || !MyInput.Static.IsJoystickConnected())
        return;
      int num = MyInput.Static.IsJoystickLastUsed ? 1 : 0;
    }

    private void DeactivateHudBuildModeNotifications()
    {
      int num = MySession.Static.CreativeMode ? 1 : 0;
    }

    private void Activate()
    {
      this.AlignToGravity();
      this.ActivateHudNotifications();
    }

    private void Deactivate()
    {
      this.DeactivateHudNotifications();
      this.CurrentShape = (IMyVoxelBrush) null;
      this.m_currentBrushIndex = -1;
      this.BuildMode = false;
    }

    private void AlignToGravity()
    {
      if (!this.CurrentShape.AutoRotate)
        return;
      Vector3D naturalGravityInPoint = (Vector3D) MyGravityProviderSystem.CalculateNaturalGravityInPoint(MySector.MainCamera.Position);
      if (naturalGravityInPoint.Equals((Vector3D) Vector3.Zero))
        return;
      naturalGravityInPoint.Normalize();
      Vector3D result = naturalGravityInPoint;
      naturalGravityInPoint.CalculatePerpendicularVector(out result);
      MatrixD fromDir = MatrixD.CreateFromDir(result, -naturalGravityInPoint);
      this.CurrentShape.SetRotation(ref fromDir);
      this.m_rotation = fromDir;
    }

    public bool TrySetBrush(string brushSubtypeName)
    {
      if (this.m_brushes == null)
        this.InitializeBrushes();
      int num = 0;
      foreach (IMyVoxelBrush brush in this.m_brushes)
      {
        if (brushSubtypeName == brush.SubtypeName)
        {
          this.CurrentShape = brush;
          this.m_currentBrushIndex = num;
          if (this.OnBrushChanged != null)
            this.OnBrushChanged();
          return true;
        }
        ++num;
      }
      return false;
    }

    public void SwitchToNextBrush()
    {
      if (this.m_brushes == null)
        this.InitializeBrushes();
      int index = (this.m_currentBrushIndex + 1) % this.m_brushes.Length;
      this.CurrentShape = this.m_brushes[index];
      this.m_currentBrushIndex = index;
      if (this.OnBrushChanged == null)
        return;
      this.OnBrushChanged();
    }

    public void EquipVoxelHand()
    {
      if (this.m_brushes == null)
        this.InitializeBrushes();
      if (this.m_currentBrushIndex == -1)
        this.m_currentBrushIndex = 0;
      this.CurrentShape = this.m_brushes[this.m_currentBrushIndex];
      this.Enabled = true;
    }

    public void EquipVoxelHand(string brushName)
    {
      if (this.m_brushes == null)
        this.InitializeBrushes();
      if (this.m_currentBrushIndex == -1)
        this.m_currentBrushIndex = 0;
      this.TrySetBrush(brushName);
      this.Enabled = true;
    }

    private void InitializeBrushes()
    {
      this.m_brushes = new IMyVoxelBrush[6]
      {
        (IMyVoxelBrush) MyBrushBox.Static,
        (IMyVoxelBrush) MyBrushCapsule.Static,
        (IMyVoxelBrush) MyBrushRamp.Static,
        (IMyVoxelBrush) MyBrushSphere.Static,
        (IMyVoxelBrush) MyBrushAutoLevel.Static,
        (IMyVoxelBrush) MyBrushEllipsoid.Static
      };
      this.m_currentBrushIndex = 0;
      this.CurrentShape = this.m_brushes[this.m_currentBrushIndex];
    }

    private void ShowSafeZoneNotification()
    {
      MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
      if (this.m_safezoneNotification == null)
        this.m_safezoneNotification = new MyHudNotification(MyCommonTexts.SafeZone_VoxelhandDisabled, 2000, "Red");
      MyHud.Notifications.Add((MyHudNotificationBase) this.m_safezoneNotification);
    }

    private void ShowNoVoxelMapNotification()
    {
      MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
      if (this.m_noVoxelMapNotification == null)
        this.m_noVoxelMapNotification = new MyHudNotification(MyCommonTexts.VoxelHand_VoxelMapNeeded, 2000, "Red");
      MyHud.Notifications.Add((MyHudNotificationBase) this.m_noVoxelMapNotification);
    }
  }
}
