// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyCubeBuilder
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.AI.Pathfinding.Obsolete;
using Sandbox.Game.Audio;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Cube.CubeBuilder;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.ContextHandling;
using Sandbox.Game.GameSystems.CoordinateSystem;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Components.Session;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.ModAPI;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.Components;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using VRage.GameServices;
using VRage.Input;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Utils;

namespace Sandbox.Game.Entities
{
  [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation | MyUpdateOrder.AfterSimulation)]
  [StaticEventOwner]
  public class MyCubeBuilder : MyBlockBuilderBase, IMyFocusHolder, IMyCubeBuilder
  {
    private static float SEMI_TRANSPARENT_BOX_MODIFIER = 1.1f;
    private static readonly MyStringId ID_SQUARE = MyStringId.GetOrCompute("Square");
    private static readonly MyStringId ID_GIZMO_DRAW_LINE_RED = MyStringId.GetOrCompute("GizmoDrawLineRed");
    private static readonly MyStringId ID_GIZMO_DRAW_LINE = MyStringId.GetOrCompute("GizmoDrawLine");
    private static readonly MyStringId ID_GIZMO_DRAW_LINE_WHITE = MyStringId.GetOrCompute("GizmoDrawLineWhite");
    private const float DEBUG_SCALE = 0.5f;
    private static string[] m_mountPointSideNames = new string[6]
    {
      "Front",
      "Back",
      "Left",
      "Right",
      "Top",
      "Bottom"
    };
    private MyBlockRemovalData m_removalTemporalData;
    public static MyCubeBuilder Static;
    protected static double BLOCK_ROTATION_SPEED = 0.002;
    private static readonly MyDefinitionId DEFAULT_BLOCK = MyDefinitionId.Parse("MyObjectBuilder_CubeBlock/LargeBlockArmorBlock");
    public MyCubeBuilderToolType m_toolType;
    private static MyCubeBuilder.MyColoringArea[] m_currColoringArea = new MyCubeBuilder.MyColoringArea[8];
    private static List<Vector3I> m_cacheGridIntersections = new List<Vector3I>();
    private static int m_cycle = 0;
    public static Dictionary<MyPlayer.PlayerId, List<Vector3>> AllPlayersColors = (Dictionary<MyPlayer.PlayerId, List<Vector3>>) null;
    protected bool canBuild = true;
    private List<Vector3D> m_collisionTestPoints = new List<Vector3D>(12);
    private int m_lastInputHandleTime;
    private bool m_customRotation;
    private float m_animationSpeed = 0.1f;
    private bool m_animationLock;
    private bool m_stationPlacement;
    protected MyBlockBuilderRotationHints m_rotationHints = new MyBlockBuilderRotationHints();
    protected MyBlockBuilderRenderData m_renderData = new MyBlockBuilderRenderData();
    private int m_selectedAxis;
    private bool m_showAxis;
    private bool m_blockCreationActivated;
    private bool m_useSymmetry;
    private bool m_useTransparency = true;
    private bool m_alignToDefault = true;
    public Vector3D? MaxGridDistanceFrom;
    private bool AllowFreeSpacePlacement = true;
    private float FreeSpacePlacementDistance = 20f;
    private StringBuilder m_cubeCountStringBuilder = new StringBuilder(10);
    private const int MAX_CUBES_BUILT_AT_ONCE = 2048;
    private const int MAX_CUBES_BUILT_IN_ONE_AXIS = 255;
    private const float CONTINUE_BUILDING_VIEW_ANGLE_CHANGE_THRESHOLD = 0.998f;
    private const float CONTINUE_BUILDING_VIEW_POINT_CHANGE_THRESHOLD = 0.25f;
    protected MyCubeBuilderGizmo m_gizmo;
    private MySymmetrySettingModeEnum m_symmetrySettingMode = MySymmetrySettingModeEnum.NoPlane;
    private Vector3D m_initialIntersectionStart;
    private Vector3D m_initialIntersectionDirection;
    protected MyCubeBuilderState m_cubeBuilderState;
    protected MyCoordinateSystem.CoordSystemData m_lastLocalCoordSysData;
    private MyCubeBlockDefinition m_lastBlockDefinition;
    private MyHudNotification m_blockNotAvailableNotification;
    private MyHudNotification m_symmetryNotification;
    private MyCubeBuilder.CubePlacementModeEnum m_cubePlacementMode;
    private bool m_isBuildMode;
    private MyHudNotification m_buildModeHint;
    private MyHudNotification m_cubePlacementModeNotification;
    private MyHudNotification m_cubePlacementModeHint;
    private MyHudNotification m_cubePlacementUnable;
    private MyHudNotification m_coloringToolHints;
    protected HashSet<MyCubeGrid.MyBlockLocation> m_blocksBuildQueue = new HashSet<MyCubeGrid.MyBlockLocation>();
    protected List<Vector3I> m_tmpBlockPositionList = new List<Vector3I>();
    protected List<Tuple<Vector3I, ushort>> m_tmpCompoundBlockPositionIdList = new List<Tuple<Vector3I, ushort>>();
    protected HashSet<Vector3I> m_tmpBlockPositionsSet = new HashSet<Vector3I>();
    protected MySessionComponentGameInventory m_gameInventory;

    public static void DrawSemiTransparentBox(
      MyCubeGrid grid,
      MySlimBlock block,
      Color color,
      bool onlyWireframe = false,
      MyStringId? lineMaterial = null,
      Vector4? lineColor = null)
    {
      MyCubeBuilder.DrawSemiTransparentBox(block.Min, block.Max, grid, color, onlyWireframe, lineMaterial, lineColor);
    }

    public static void DrawSemiTransparentBox(
      Vector3I minPosition,
      Vector3I maxPosition,
      MyCubeGrid grid,
      Color color,
      bool onlyWireframe = false,
      MyStringId? lineMaterial = null,
      Vector4? lineColor = null)
    {
      float gridSize = grid.GridSize;
      BoundingBoxD localbox = new BoundingBoxD((Vector3D) (minPosition * gridSize - new Vector3(gridSize / 2f * MyCubeBuilder.SEMI_TRANSPARENT_BOX_MODIFIER)), (Vector3D) (maxPosition * gridSize + new Vector3(gridSize / 2f * MyCubeBuilder.SEMI_TRANSPARENT_BOX_MODIFIER)));
      MatrixD worldMatrix = grid.WorldMatrix;
      Color white = Color.White;
      if (lineColor.HasValue)
        white = (Color) lineColor.Value;
      MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref localbox, ref white, MySimpleObjectRasterizer.Wireframe, 1, 0.04f, lineMaterial: lineMaterial, blendType: MyBillboard.BlendTypeEnum.LDR);
      if (onlyWireframe)
        return;
      Color color1 = new Color(color * 0.2f, 0.3f);
      MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref localbox, ref color1, MySimpleObjectRasterizer.Solid, 0, 0.04f, new MyStringId?(MyCubeBuilder.ID_SQUARE), onlyFrontFaces: true, blendType: MyBillboard.BlendTypeEnum.LDR);
    }

    protected void ClearRenderData()
    {
      this.m_renderData.BeginCollectingInstanceData();
      this.m_renderData.EndCollectingInstanceData(this.CurrentGrid != null ? this.CurrentGrid.WorldMatrix : MatrixD.Identity, this.UseTransparency);
    }

    public override void Draw()
    {
      base.Draw();
      this.DebugDraw();
      if (this.BlockCreationIsActivated)
        MyHud.Crosshair.Recenter();
      if (!this.IsActivated || this.CurrentBlockDefinition == null)
        this.ClearRenderData();
      else if (!this.BuildInputValid)
      {
        this.ClearRenderData();
      }
      else
      {
        this.DrawBuildingStepsCount(this.m_gizmo.SpaceDefault.m_startBuild, this.m_gizmo.SpaceDefault.m_startRemove, this.m_gizmo.SpaceDefault.m_continueBuild, ref this.m_gizmo.SpaceDefault.m_localMatrixAdd);
        bool addPos = this.m_gizmo.SpaceDefault.m_startBuild.HasValue;
        bool removePos = false;
        float gridSize = 0.0f;
        if (this.CurrentBlockDefinition != null)
          gridSize = MyDefinitionManager.Static.GetCubeSize(this.CurrentBlockDefinition.CubeSize);
        if (this.DynamicMode)
        {
          PlaneD planeD = new PlaneD(MySector.MainCamera.Position, MySector.MainCamera.UpVector);
          Vector3D point = MyBlockBuilderBase.IntersectionStart;
          point = planeD.ProjectPoint(ref point);
          Vector3D defaultPos = point + (double) MyBlockBuilderBase.IntersectionDistance * MyBlockBuilderBase.IntersectionDirection;
          if (this.m_hitInfo.HasValue)
            defaultPos = this.m_hitInfo.Value.Position;
          addPos = this.CaluclateDynamicModePos(defaultPos, this.IsDynamicOverride());
          MyCoordinateSystem.Static.Visible = false;
        }
        else if (!this.m_gizmo.SpaceDefault.m_startBuild.HasValue && !this.m_gizmo.SpaceDefault.m_startRemove.HasValue)
        {
          if (!this.FreezeGizmo)
          {
            if (this.CurrentGrid != null)
            {
              MyCoordinateSystem.Static.Visible = false;
              addPos = this.GetAddAndRemovePositions(gridSize, this.PlacingSmallGridOnLargeStatic, out this.m_gizmo.SpaceDefault.m_addPos, out this.m_gizmo.SpaceDefault.m_addPosSmallOnLarge, out this.m_gizmo.SpaceDefault.m_addDir, out this.m_gizmo.SpaceDefault.m_removePos, out this.m_gizmo.SpaceDefault.m_removeBlock, out this.m_gizmo.SpaceDefault.m_blockIdInCompound, this.m_gizmo.SpaceDefault.m_removeBlocksInMultiBlock);
              if (addPos || this.m_gizmo.SpaceDefault.m_removeBlock != null)
              {
                this.m_gizmo.SpaceDefault.m_localMatrixAdd.Translation = !this.PlacingSmallGridOnLargeStatic ? (Vector3) this.m_gizmo.SpaceDefault.m_addPos : this.m_gizmo.SpaceDefault.m_addPosSmallOnLarge.Value;
                Vector3D translation = this.m_gizmo.SpaceDefault.m_worldMatrixAdd.Translation;
                this.m_gizmo.SpaceDefault.m_worldMatrixAdd = this.m_gizmo.SpaceDefault.m_localMatrixAdd * this.CurrentGrid.WorldMatrix;
                this.m_gizmo.SpaceDefault.m_worldMatrixAdd.Translation = translation;
                Vector3I? mountPointNormal = this.GetSingleMountPointNormal();
                if (mountPointNormal.HasValue && this.GridAndBlockValid && this.m_gizmo.SpaceDefault.m_addDir != Vector3I.Zero)
                  this.m_gizmo.SetupLocalAddMatrix(this.m_gizmo.SpaceDefault, mountPointNormal.Value);
              }
            }
            else
            {
              MyCoordinateSystem.Static.Visible = true;
              Vector3D localSnappedPos = this.m_lastLocalCoordSysData.LocalSnappedPos;
              if (!MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.StaticGridAlignToCenter)
                localSnappedPos -= new Vector3D(0.5 * (double) gridSize, 0.5 * (double) gridSize, -0.5 * (double) gridSize);
              this.m_gizmo.SpaceDefault.m_addPos = Vector3I.Round(localSnappedPos / (double) gridSize);
              this.m_gizmo.SpaceDefault.m_localMatrixAdd.Translation = (Vector3) this.m_lastLocalCoordSysData.LocalSnappedPos;
              this.m_gizmo.SpaceDefault.m_worldMatrixAdd = this.m_lastLocalCoordSysData.Origin.TransformMatrix;
              addPos = true;
            }
          }
          if (this.m_gizmo.SpaceDefault.m_removeBlock != null)
            removePos = true;
        }
        if ((MySession.Static.ControlledEntity == null || !(MySession.Static.ControlledEntity is MyCockpit) ? 0 : (!MyBlockBuilderBase.SpectatorIsBuilding ? 1 : 0)) == 0)
        {
          if (this.IsInSymmetrySettingMode)
          {
            this.m_gizmo.SpaceDefault.m_continueBuild = new Vector3I?();
            addPos = false;
            removePos = false;
            if (this.m_gizmo.SpaceDefault.m_removeBlock != null)
              MyCubeBuilder.DrawSemiTransparentBox(this.CurrentGrid, this.m_gizmo.SpaceDefault.m_removeBlock, (Color) this.DrawSymmetryPlane(this.SymmetrySettingMode, this.CurrentGrid, (this.m_gizmo.SpaceDefault.m_removeBlock.Min * this.CurrentGrid.GridSize + this.m_gizmo.SpaceDefault.m_removeBlock.Max * this.CurrentGrid.GridSize) * 0.5f).ToVector4(), lineMaterial: new MyStringId?(MyCubeBuilder.ID_GIZMO_DRAW_LINE_RED));
          }
          if (this.CurrentGrid != null && (this.UseSymmetry || this.IsInSymmetrySettingMode))
          {
            if (this.CurrentGrid.XSymmetryPlane.HasValue)
            {
              Vector3 center = this.CurrentGrid.XSymmetryPlane.Value * this.CurrentGrid.GridSize;
              this.DrawSymmetryPlane(this.CurrentGrid.XSymmetryOdd ? MySymmetrySettingModeEnum.XPlaneOdd : MySymmetrySettingModeEnum.XPlane, this.CurrentGrid, center);
            }
            if (this.CurrentGrid.YSymmetryPlane.HasValue)
            {
              Vector3 center = this.CurrentGrid.YSymmetryPlane.Value * this.CurrentGrid.GridSize;
              this.DrawSymmetryPlane(this.CurrentGrid.YSymmetryOdd ? MySymmetrySettingModeEnum.YPlaneOdd : MySymmetrySettingModeEnum.YPlane, this.CurrentGrid, center);
            }
            if (this.CurrentGrid.ZSymmetryPlane.HasValue)
            {
              Vector3 center = this.CurrentGrid.ZSymmetryPlane.Value * this.CurrentGrid.GridSize;
              this.DrawSymmetryPlane(this.CurrentGrid.ZSymmetryOdd ? MySymmetrySettingModeEnum.ZPlaneOdd : MySymmetrySettingModeEnum.ZPlane, this.CurrentGrid, center);
            }
          }
          if (this.ShowAxis)
            this.DrawRotationAxis(this.m_selectedAxis);
        }
        this.UpdateGizmos(addPos, removePos, true);
        if (this.CurrentGrid == null || this.DynamicMode && this.CurrentGrid != null)
        {
          MatrixD worldMatrixAdd = this.m_gizmo.SpaceDefault.m_worldMatrixAdd;
          Vector3D result;
          Vector3D.TransformNormal(ref this.CurrentBlockDefinition.ModelOffset, ref worldMatrixAdd, out result);
          worldMatrixAdd.Translation += result;
          this.m_renderData.EndCollectingInstanceData(worldMatrixAdd, this.UseTransparency);
        }
        else
          this.m_renderData.EndCollectingInstanceData(this.CurrentGrid.WorldMatrix, this.UseTransparency);
      }
    }

    protected virtual bool CaluclateDynamicModePos(Vector3D defaultPos, bool isDynamicOverride = false)
    {
      bool valid = true;
      if (!this.FreezeGizmo)
      {
        this.m_gizmo.SpaceDefault.m_worldMatrixAdd.Translation = defaultPos;
        if (isDynamicOverride)
        {
          defaultPos = this.GetFreeSpacePlacementPosition(out valid);
          this.m_gizmo.SpaceDefault.m_worldMatrixAdd.Translation = defaultPos;
        }
      }
      return valid;
    }

    protected void DrawBuildingStepsCount(
      Vector3I? startBuild,
      Vector3I? startRemove,
      Vector3I? continueBuild,
      ref Matrix localMatrixAdd)
    {
      Vector3I? nullable1 = startBuild;
      Vector3I? nullable2 = nullable1.HasValue ? nullable1 : startRemove;
      if (!nullable2.HasValue || !continueBuild.HasValue)
        return;
      Vector3I result;
      Vector3I.TransformNormal(ref this.CurrentBlockDefinition.Size, ref localMatrixAdd, out result);
      result = Vector3I.Abs(result);
      int stepCount;
      MyBlockBuilderBase.ComputeSteps(nullable2.Value, continueBuild.Value, startBuild.HasValue ? result : Vector3I.One, out Vector3I _, out Vector3I _, out stepCount);
      this.m_cubeCountStringBuilder.Clear();
      this.m_cubeCountStringBuilder.Append((object) MyTexts.Get(MyCommonTexts.Clipboard_TotalBlocks));
      this.m_cubeCountStringBuilder.AppendInt32(stepCount);
      MyGuiManager.DrawString("White", this.m_cubeCountStringBuilder.ToString(), new Vector2(0.51f, 0.51f), 0.7f);
    }

    private void DebugDraw()
    {
      if (MyPerGameSettings.EnableAi && MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES != MyWEMDebugDrawMode.NONE)
      {
        MyCubeBlockDefinition currentBlockDefinition = this.CurrentBlockDefinition;
        if (currentBlockDefinition != null && this.CurrentGrid != null)
        {
          Vector3 vector3 = (Vector3) Vector3.Transform(this.m_gizmo.SpaceDefault.m_addPos * 2.5f, this.CurrentGrid.PositionComp.WorldMatrixRef);
          Matrix matrix = (Matrix) ref this.m_gizmo.SpaceDefault.m_worldMatrixAdd;
          matrix.Translation = vector3;
          Matrix.Rescale(matrix, this.CurrentGrid.GridSize);
          if (currentBlockDefinition.NavigationDefinition != null)
          {
            MyGridNavigationMesh mesh = currentBlockDefinition.NavigationDefinition.Mesh;
          }
        }
      }
      if (MyFakes.ENABLE_DEBUG_DRAW_TEXTURE_NAMES)
        this.DebugDrawModelTextures();
      if (!MyDebugDrawSettings.DEBUG_DRAW_MODEL_INFO)
        return;
      this.DebugDrawModelInfo();
    }

    private void DebugDrawModelTextures()
    {
      LineD line = new LineD(MyBlockBuilderBase.IntersectionStart, MyBlockBuilderBase.IntersectionStart + MyBlockBuilderBase.IntersectionDirection * 200.0);
      MyIntersectionResultLineTriangleEx? intersectionWithLine = MyEntities.GetIntersectionWithLine(ref line, (MyEntity) MySession.Static.LocalCharacter, (MyEntity) null);
      if (!intersectionWithLine.HasValue)
        return;
      float yPos = 0.0f;
      if (!(intersectionWithLine.Value.Entity is MyCubeGrid))
        return;
      MyCubeGrid entity = intersectionWithLine.Value.Entity as MyCubeGrid;
      MyIntersectionResultLineTriangleEx? nullable = new MyIntersectionResultLineTriangleEx?();
      MySlimBlock mySlimBlock = (MySlimBlock) null;
      ref LineD local1 = ref line;
      ref MyIntersectionResultLineTriangleEx? local2 = ref nullable;
      ref MySlimBlock local3 = ref mySlimBlock;
      if (!entity.GetIntersectionWithLine(ref local1, out local2, out local3) || !nullable.HasValue || mySlimBlock == null)
        return;
      MyCubeBuilder.DebugDrawModelTextures(mySlimBlock.FatBlock, ref yPos);
    }

    private void DebugDrawModelInfo()
    {
      MyGuiScreenDebugRenderDebug.ClipboardText.Clear();
      LineD line = new LineD(MyBlockBuilderBase.IntersectionStart, MyBlockBuilderBase.IntersectionStart + MyBlockBuilderBase.IntersectionDirection * 1000.0);
      MyIntersectionResultLineTriangleEx? intersectionWithLine = MyEntities.GetIntersectionWithLine(ref line, (MyEntity) MySession.Static.LocalCharacter, (MyEntity) null, ignoreFloatingObjects: false, ignoreObjectsWithoutPhysics: false);
      IMyEntity myEntity = (IMyEntity) null;
      Vector3D vector3D1 = Vector3D.Zero;
      if (intersectionWithLine.HasValue)
      {
        myEntity = intersectionWithLine.Value.Entity;
        vector3D1 = intersectionWithLine.Value.IntersectionPointInWorldSpace;
      }
      MyPhysics.HitInfo? nullable = MyPhysics.CastRay(line.From, line.To, 30);
      Vector3D vector3D2;
      if (nullable.HasValue)
      {
        if (intersectionWithLine.HasValue)
        {
          double num1 = (nullable.Value.Position - line.From).Length();
          vector3D2 = vector3D1 - line.From;
          double num2 = vector3D2.Length();
          if (num1 >= num2)
            goto label_6;
        }
        myEntity = nullable.Value.HkHitInfo.GetHitEntity();
        vector3D1 = nullable.Value.Position;
      }
label_6:
      float y1 = 20f;
      if (myEntity != null)
      {
        vector3D2 = vector3D1 - line.From;
        double num = vector3D2.Length();
        float yPos;
        if (myEntity is MyEnvironmentSector)
        {
          MyEnvironmentSector environmentSector = myEntity as MyEnvironmentSector;
          MyRenderProxy.DebugDrawText2D(new Vector2(20f, y1), "Type: EnvironmentSector " + (object) environmentSector.SectorId, Color.Yellow, 0.5f);
          yPos = y1 + 10f;
          int itemFromShapeKey = environmentSector.GetItemFromShapeKey(nullable.Value.HkHitInfo.GetShapeKey(0));
          short modelIndex = environmentSector.GetModelIndex(itemFromShapeKey);
          MyCubeBuilder.DebugDrawModelInfo(MyModels.GetModelOnlyData(environmentSector.Owner.GetModelForId(modelIndex).Model), ref yPos);
        }
        else if (myEntity is MyVoxelBase)
        {
          MyVoxelBase self = (MyVoxelBase) myEntity;
          if (self.RootVoxel != null)
            self = self.RootVoxel;
          Vector3D worldPosition = vector3D1;
          MyVoxelMaterialDefinition materialAt = self.GetMaterialAt(ref worldPosition);
          float y2;
          if (self.RootVoxel is MyPlanet)
          {
            MyRenderProxy.DebugDrawText2D(new Vector2(20f, y1), "Type: planet/moon", Color.Yellow, 0.5f);
            float y3 = y1 + 10f;
            MyRenderProxy.DebugDrawText2D(new Vector2(20f, y3), "Terrain: " + (object) materialAt, Color.Yellow, 0.5f);
            y2 = y3 + 10f;
          }
          else
          {
            MyRenderProxy.DebugDrawText2D(new Vector2(20f, y1), "Type: asteroid", Color.Yellow, 0.5f);
            float y3 = y1 + 10f;
            MyRenderProxy.DebugDrawText2D(new Vector2(20f, y3), "Terrain: " + (object) materialAt, Color.Yellow, 0.5f);
            y2 = y3 + 10f;
          }
          MyRenderProxy.DebugDrawText2D(new Vector2(20f, y2), "Object size: " + (object) self.SizeInMetres, Color.Yellow, 0.5f);
          yPos = y2 + 10f;
        }
        else if (myEntity is MyCubeGrid)
        {
          MyCubeGrid myCubeGrid = (MyCubeGrid) myEntity;
          MyRenderProxy.DebugDrawText2D(new Vector2(20f, y1), "Detected grid object", Color.Yellow, 0.5f);
          float y2 = y1 + 10f;
          MyRenderProxy.DebugDrawText2D(new Vector2(20f, y2), string.Format("Grid name: {0}", (object) myCubeGrid.DisplayName), Color.Yellow, 0.5f);
          yPos = y2 + 10f;
          MyIntersectionResultLineTriangleEx? t;
          MySlimBlock slimBlock;
          if (myCubeGrid.GetIntersectionWithLine(ref line, out t, out slimBlock) && t.HasValue && slimBlock != null)
          {
            if (slimBlock.FatBlock != null)
              MyCubeBuilder.DebugDrawModelTextures(slimBlock.FatBlock, ref yPos);
            else
              MyCubeBuilder.DebugDrawBareBlockInfo(slimBlock, ref yPos);
          }
        }
        else if (intersectionWithLine.HasValue && intersectionWithLine.Value.Entity is MyCubeBlock)
        {
          MyCubeBlock entity = (MyCubeBlock) intersectionWithLine.Value.Entity;
          MyRenderProxy.DebugDrawText2D(new Vector2(20f, y1), "Detected block", Color.Yellow, 0.5f);
          float y2 = y1 + 10f;
          MyRenderProxy.DebugDrawText2D(new Vector2(20f, y2), string.Format("Block name: {0}", (object) entity.DisplayName), Color.Yellow, 0.5f);
          yPos = y2 + 10f;
          MyCubeBuilder.DebugDrawModelTextures(entity, ref yPos);
        }
        else
        {
          MyRenderProxy.DebugDrawText2D(new Vector2(20f, y1), "Unknown object detected", Color.Yellow, 0.5f);
          yPos = y1 + 10f;
          if (myEntity.Model is MyModel model)
            MyCubeBuilder.DebugDrawModelInfo(model, ref yPos);
        }
        MyRenderProxy.DebugDrawText2D(new Vector2(20f, yPos), "Distance " + (object) num + "m", Color.Yellow, 0.5f);
      }
      else
        MyRenderProxy.DebugDrawText2D(new Vector2(20f, 20f), "Nothing detected nearby", Color.Yellow, 0.5f);
    }

    private static void DebugDrawTexturesInfo(MyModel model, ref float yPos)
    {
      HashSet<string> source = new HashSet<string>();
      foreach (VRageRender.Models.MyMesh mesh in model.GetMeshList())
      {
        if (mesh.Material.Textures == null)
        {
          source.Add("<null material>");
        }
        else
        {
          source.Add("Material: " + mesh.Material.Name);
          foreach (string str in mesh.Material.Textures.Values)
          {
            if (!string.IsNullOrWhiteSpace(str))
              source.Add(str);
          }
        }
      }
      foreach (string text in (IEnumerable<string>) source.OrderBy<string, string>((Func<string, string>) (s => s), (IComparer<string>) StringComparer.InvariantCultureIgnoreCase))
      {
        MyGuiScreenDebugRenderDebug.ClipboardText.AppendLine(text);
        MyRenderProxy.DebugDrawText2D(new Vector2(20f, yPos), text, Color.White, 0.5f);
        yPos += 10f;
      }
    }

    private static void DebugDrawBareBlockInfo(MySlimBlock block, ref float yPos)
    {
      yPos += 20f;
      MyGuiScreenDebugRenderDebug.ClipboardText.AppendLine(string.Format("Display Name: {0}", (object) block.BlockDefinition.DisplayNameText));
      MyGuiScreenDebugRenderDebug.ClipboardText.AppendLine(string.Format("Cube type: {0}", (object) block.BlockDefinition.CubeDefinition.CubeTopology));
      MyGuiScreenDebugRenderDebug.ClipboardText.AppendLine(string.Format("Skin: {0}", (object) block.SkinSubtypeId));
      MyRenderProxy.DebugDrawText2D(new Vector2(20f, yPos), string.Format("Display Name: {0}", (object) block.BlockDefinition.DisplayNameText), Color.Yellow, 0.5f);
      yPos += 10f;
      MyRenderProxy.DebugDrawText2D(new Vector2(20f, yPos), string.Format("Cube type: {0}", (object) block.BlockDefinition.CubeDefinition.CubeTopology), Color.Yellow, 0.5f);
      yPos += 10f;
      MyRenderProxy.DebugDrawText2D(new Vector2(20f, yPos), string.Format("Skin: {0}", (object) block.SkinSubtypeId), Color.Yellow, 0.5f);
      yPos += 10f;
      foreach (string modelAsset in (IEnumerable<string>) ((IEnumerable<string>) block.BlockDefinition.CubeDefinition.Model).Distinct<string>().OrderBy<string, string>((Func<string, string>) (s => s), (IComparer<string>) StringComparer.InvariantCultureIgnoreCase))
        MyCubeBuilder.DebugDrawModelInfo(MyModels.GetModel(modelAsset), ref yPos);
    }

    private static void DebugDrawModelTextures(MyCubeBlock block, ref float yPos)
    {
      MyModel model = (MyModel) null;
      if (block != null)
        model = block.Model;
      if (model == null)
        return;
      yPos += 20f;
      MyRenderProxy.DebugDrawText2D(new Vector2(20f, yPos), "SubTypeId: " + block.BlockDefinition.Id.SubtypeName, Color.Yellow, 0.5f);
      yPos += 10f;
      MyRenderProxy.DebugDrawText2D(new Vector2(20f, yPos), "Display name: " + block.BlockDefinition.DisplayNameText, Color.Yellow, 0.5f);
      yPos += 10f;
      MyRenderProxy.DebugDrawText2D(new Vector2(20f, yPos), "Skin: " + (object) block.SlimBlock.SkinSubtypeId, Color.Yellow, 0.5f);
      yPos += 10f;
      if (block.SlimBlock.IsMultiBlockPart)
      {
        MyCubeGridMultiBlockInfo multiBlockInfo = block.CubeGrid.GetMultiBlockInfo(block.SlimBlock.MultiBlockId);
        if (multiBlockInfo != null)
        {
          MyRenderProxy.DebugDrawText2D(new Vector2(20f, yPos), "Multiblock: " + multiBlockInfo.MultiBlockDefinition.Id.SubtypeName + " (Id:" + (object) block.SlimBlock.MultiBlockId + ")", Color.Yellow, 0.5f);
          yPos += 10f;
        }
      }
      if (block.BlockDefinition.IsGeneratedBlock)
      {
        MyRenderProxy.DebugDrawText2D(new Vector2(20f, yPos), "Generated block: " + (object) block.BlockDefinition.GeneratedBlockType, Color.Yellow, 0.5f);
        yPos += 10f;
      }
      MyRenderProxy.DebugDrawText2D(new Vector2(20f, yPos), "BlockID: " + (object) block.EntityId, Color.Yellow, 0.5f);
      yPos += 10f;
      if (block.ModelCollision != null)
        MyRenderProxy.DebugDrawText2D(new Vector2(20f, yPos), "Collision: " + block.ModelCollision.AssetName, Color.Yellow, 0.5f);
      yPos += 10f;
      MyCubeBuilder.DebugDrawModelInfo(model, ref yPos);
    }

    private static void DebugDrawModelInfo(MyModel model, ref float yPos)
    {
      MyGuiScreenDebugRenderDebug.ClipboardText.AppendLine("Asset: " + model.AssetName);
      MyRenderProxy.DebugDrawText2D(new Vector2(20f, yPos), "Asset: " + model.AssetName, Color.Yellow, 0.5f);
      yPos += 10f;
      int startIndex = model.AssetName.LastIndexOf("\\") + 1;
      MyTomasInputComponent.ClipboardText = startIndex == -1 || startIndex >= model.AssetName.Length ? model.AssetName : model.AssetName.Substring(startIndex);
      MyCubeBuilder.DebugDrawTexturesInfo(model, ref yPos);
    }

    private Color DrawSymmetryPlane(
      MySymmetrySettingModeEnum plane,
      MyCubeGrid localGrid,
      Vector3 center)
    {
      BoundingBox localAabb = localGrid.PositionComp.LocalAABB;
      float num = 1f;
      float gridSize = localGrid.GridSize;
      Vector3 position1 = Vector3.Zero;
      Vector3 position2 = Vector3.Zero;
      Vector3 position3 = Vector3.Zero;
      Vector3 position4 = Vector3.Zero;
      float a = 0.1f;
      Color color = Color.Gray;
      switch (plane)
      {
        case MySymmetrySettingModeEnum.XPlane:
        case MySymmetrySettingModeEnum.XPlaneOdd:
          color = new Color(1f, 0.0f, 0.0f, a);
          center.X -= localAabb.Center.X + (plane == MySymmetrySettingModeEnum.XPlaneOdd ? localGrid.GridSize * 0.50025f : 0.0f);
          center.Y = 0.0f;
          center.Z = 0.0f;
          position1 = new Vector3(0.0f, localAabb.HalfExtents.Y * num + gridSize, localAabb.HalfExtents.Z * num + gridSize) + localAabb.Center + center;
          position2 = new Vector3(0.0f, -localAabb.HalfExtents.Y * num - gridSize, localAabb.HalfExtents.Z * num + gridSize) + localAabb.Center + center;
          position3 = new Vector3(0.0f, localAabb.HalfExtents.Y * num + gridSize, -localAabb.HalfExtents.Z * num - gridSize) + localAabb.Center + center;
          position4 = new Vector3(0.0f, -localAabb.HalfExtents.Y * num - gridSize, -localAabb.HalfExtents.Z * num - gridSize) + localAabb.Center + center;
          break;
        case MySymmetrySettingModeEnum.YPlane:
        case MySymmetrySettingModeEnum.YPlaneOdd:
          color = new Color(0.0f, 1f, 0.0f, a);
          center.X = 0.0f;
          center.Y -= localAabb.Center.Y + (plane == MySymmetrySettingModeEnum.YPlaneOdd ? localGrid.GridSize * 0.50025f : 0.0f);
          center.Z = 0.0f;
          position1 = new Vector3(localAabb.HalfExtents.X * num + gridSize, 0.0f, localAabb.HalfExtents.Z * num + gridSize) + localAabb.Center + center;
          position2 = new Vector3(-localAabb.HalfExtents.X * num - gridSize, 0.0f, localAabb.HalfExtents.Z * num + gridSize) + localAabb.Center + center;
          position3 = new Vector3(localAabb.HalfExtents.X * num + gridSize, 0.0f, -localAabb.HalfExtents.Z * num - gridSize) + localAabb.Center + center;
          position4 = new Vector3(-localAabb.HalfExtents.X * num - gridSize, 0.0f, -localAabb.HalfExtents.Z * num - gridSize) + localAabb.Center + center;
          break;
        case MySymmetrySettingModeEnum.ZPlane:
        case MySymmetrySettingModeEnum.ZPlaneOdd:
          color = new Color(0.0f, 0.0f, 1f, a);
          center.X = 0.0f;
          center.Y = 0.0f;
          center.Z -= localAabb.Center.Z - (plane == MySymmetrySettingModeEnum.ZPlaneOdd ? localGrid.GridSize * 0.50025f : 0.0f);
          position1 = new Vector3(localAabb.HalfExtents.X * num + gridSize, localAabb.HalfExtents.Y * num + gridSize, 0.0f) + localAabb.Center + center;
          position2 = new Vector3(-localAabb.HalfExtents.X * num - gridSize, localAabb.HalfExtents.Y * num + gridSize, 0.0f) + localAabb.Center + center;
          position3 = new Vector3(localAabb.HalfExtents.X * num + gridSize, -localAabb.HalfExtents.Y * num - gridSize, 0.0f) + localAabb.Center + center;
          position4 = new Vector3(-localAabb.HalfExtents.X * num - gridSize, -localAabb.HalfExtents.Y * num - gridSize, 0.0f) + localAabb.Center + center;
          break;
      }
      MatrixD worldMatrix = this.CurrentGrid.WorldMatrix;
      Vector3D result1;
      Vector3D.Transform(ref position1, ref worldMatrix, out result1);
      Vector3D result2;
      Vector3D.Transform(ref position2, ref worldMatrix, out result2);
      Vector3D result3;
      Vector3D.Transform(ref position3, ref worldMatrix, out result3);
      Vector3D result4;
      Vector3D.Transform(ref position4, ref worldMatrix, out result4);
      MyRenderProxy.DebugDrawTriangle(result1, result2, result3, color, true, true);
      MyRenderProxy.DebugDrawTriangle(result3, result2, result4, color, true, true);
      return color;
    }

    private void DrawRotationAxis(int axis)
    {
      MatrixD worldMatrixAdd = this.m_gizmo.SpaceDefault.m_worldMatrixAdd;
      Vector3D vector3D1 = Vector3D.Zero;
      Color color = Color.White;
      switch (axis)
      {
        case 0:
          vector3D1 = worldMatrixAdd.Left;
          color = Color.Red;
          break;
        case 1:
          vector3D1 = worldMatrixAdd.Up;
          color = Color.Green;
          break;
        case 2:
          vector3D1 = worldMatrixAdd.Forward;
          color = Color.Blue;
          break;
      }
      Vector3D vector3D2 = vector3D1 * (this.CurrentBlockDefinition.CubeSize == MyCubeSize.Small ? 1.5 : 3.0);
      Vector4 vector4 = color.ToVector4();
      MySimpleObjectDraw.DrawLine(worldMatrixAdd.Translation + vector3D2, worldMatrixAdd.Translation - vector3D2, new MyStringId?(MyCubeBuilder.ID_GIZMO_DRAW_LINE_WHITE), ref vector4, 0.15f, MyBillboard.BlendTypeEnum.LDR);
    }

    public static void DrawMountPoints(
      float cubeSize,
      MyCubeBlockDefinition def,
      ref MatrixD drawMatrix)
    {
      MyCubeBlockDefinition.MountPoint[] modelMountPoints = def.GetBuildProgressModelMountPoints(1f);
      if (modelMountPoints == null)
        return;
      if (!MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS_AUTOGENERATE)
        MyCubeBuilder.DrawMountPoints(cubeSize, def, drawMatrix, modelMountPoints);
      else if (def.Model != null)
      {
        int shapeIndex = 0;
        MyModel model = MyModels.GetModel(def.Model);
        foreach (HkShape havokCollisionShape in model.HavokCollisionShapes)
          MyPhysicsDebugDraw.DrawCollisionShape(havokCollisionShape, drawMatrix, 0.2f, ref shapeIndex);
        MyCubeBlockDefinition.MountPoint[] mountPoints = MyCubeBuilder.AutogenerateMountpoints(model, cubeSize);
        MyCubeBuilder.DrawMountPoints(cubeSize, def, drawMatrix, mountPoints);
      }
      if (!MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS_AXIS_HELPERS)
        return;
      MyCubeBuilder.DrawMountPointsAxisHelpers(def, ref drawMatrix, cubeSize);
    }

    public static MyCubeBlockDefinition.MountPoint[] AutogenerateMountpoints(
      MyModel model,
      float gridSize)
    {
      HkShape[] shapes = model.HavokCollisionShapes;
      if (shapes == null)
      {
        if (model.HavokBreakableShapes == null)
          return Array.Empty<MyCubeBlockDefinition.MountPoint>();
        shapes = new HkShape[1]
        {
          model.HavokBreakableShapes[0].GetShape()
        };
      }
      return MyCubeBuilder.AutogenerateMountpoints(shapes, gridSize);
    }

    public static MyCubeBlockDefinition.MountPoint[] AutogenerateMountpoints(
      HkShape[] shapes,
      float gridSize)
    {
      List<BoundingBox>[] boundingBoxListArray = new List<BoundingBox>[Base6Directions.EnumDirections.Length];
      List<MyCubeBlockDefinition.MountPoint> mountPoints = new List<MyCubeBlockDefinition.MountPoint>();
      Base6Directions.Direction[] enumDirections = Base6Directions.EnumDirections;
label_20:
      for (int index1 = 0; index1 < enumDirections.Length; ++index1)
      {
        int index2 = (int) enumDirections[index1];
        Vector3 direction = Base6Directions.Directions[index2];
        foreach (HkShape shape1 in shapes)
        {
          if (shape1.ShapeType == HkShapeType.List)
          {
            HkShapeContainerIterator iterator = ((HkListShape) shape1).GetIterator();
            while (iterator.IsValid)
            {
              HkShape currentValue = iterator.CurrentValue;
              if (currentValue.ShapeType == HkShapeType.ConvexTransform)
                MyCubeBuilder.FindMountPoint(((HkConvexTransformShape) currentValue).Base, direction, gridSize, mountPoints);
              else if (currentValue.ShapeType == HkShapeType.ConvexTranslate)
                MyCubeBuilder.FindMountPoint(((HkConvexTranslateShape) currentValue).Base, direction, gridSize, mountPoints);
              else
                MyCubeBuilder.FindMountPoint(currentValue, direction, gridSize, mountPoints);
              iterator.Next();
            }
            break;
          }
          if (shape1.ShapeType == HkShapeType.Mopp)
          {
            HkMoppBvTreeShape hkMoppBvTreeShape = (HkMoppBvTreeShape) shape1;
            int num1 = 0;
            while (true)
            {
              int num2 = num1;
              HkShapeCollection shapeCollection = hkMoppBvTreeShape.ShapeCollection;
              int shapeCount = shapeCollection.ShapeCount;
              if (num2 < shapeCount)
              {
                shapeCollection = hkMoppBvTreeShape.ShapeCollection;
                HkShape shape2 = shapeCollection.GetShape((uint) num1, (HkShapeBuffer) null);
                if (shape2.ShapeType == HkShapeType.ConvexTranslate)
                  MyCubeBuilder.FindMountPoint(((HkConvexTranslateShape) shape2).Base, direction, gridSize, mountPoints);
                ++num1;
              }
              else
                goto label_20;
            }
          }
          else
            MyCubeBuilder.FindMountPoint(shape1, direction, gridSize, mountPoints);
        }
      }
      return mountPoints.ToArray();
    }

    private static bool FindMountPoint(
      HkShape shape,
      Vector3 direction,
      float gridSize,
      List<MyCubeBlockDefinition.MountPoint> mountPoints)
    {
      float d = (float) ((double) gridSize * 0.75 / 2.0);
      Plane plane = new Plane(-direction, d);
      float num1 = 0.2f;
      Vector3 aabbMin;
      Vector3 aabbMax;
      if (!HkShapeCutterUtil.Cut(shape, new Vector4(plane.Normal.X, plane.Normal.Y, plane.Normal.Z, plane.D), out aabbMin, out aabbMax))
        return false;
      BoundingBox boundingBox = new BoundingBox(aabbMin, aabbMax);
      boundingBox.InflateToMinimum(new Vector3(num1));
      float num2 = gridSize * 0.5f;
      MyCubeBlockDefinition.MountPoint mountPoint = new MyCubeBlockDefinition.MountPoint();
      mountPoint.Normal = new Vector3I(direction);
      mountPoint.Start = (boundingBox.Min + new Vector3(num2)) / gridSize;
      mountPoint.End = (boundingBox.Max + new Vector3(num2)) / gridSize;
      mountPoint.Enabled = true;
      mountPoint.PressurizedWhenOpen = true;
      Vector3 vector3 = Vector3.Abs(direction) * mountPoint.Start;
      int num3 = (double) vector3.AbsMax() > 0.5 ? 1 : 0;
      mountPoint.Start -= vector3;
      mountPoint.Start -= direction * 0.04f;
      mountPoint.End -= Vector3.Abs(direction) * mountPoint.End;
      mountPoint.End += direction * 0.04f;
      if (num3 != 0)
      {
        mountPoint.Start += Vector3.Abs(direction);
        mountPoint.End += Vector3.Abs(direction);
      }
      mountPoints.Add(mountPoint);
      return true;
    }

    public static void DrawMountPoints(
      float cubeSize,
      MyCubeBlockDefinition def,
      MatrixD drawMatrix,
      MyCubeBlockDefinition.MountPoint[] mountPoints)
    {
      Color yellow = Color.Yellow;
      Color blue = Color.Blue;
      Vector3I center = def.Center;
      Vector3 vector3_1 = def.Size * 0.5f;
      MatrixD transform = MatrixD.CreateTranslation(((Vector3) center - vector3_1) * cubeSize) * drawMatrix;
      for (int index = 0; index < mountPoints.Length; ++index)
      {
        if ((!(mountPoints[index].Normal == Base6Directions.IntDirections[0]) || MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS_AXIS0) && (!(mountPoints[index].Normal == Base6Directions.IntDirections[1]) || MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS_AXIS1) && ((!(mountPoints[index].Normal == Base6Directions.IntDirections[2]) || MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS_AXIS2) && (!(mountPoints[index].Normal == Base6Directions.IntDirections[3]) || MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS_AXIS3)) && ((!(mountPoints[index].Normal == Base6Directions.IntDirections[4]) || MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS_AXIS4) && (!(mountPoints[index].Normal == Base6Directions.IntDirections[5]) || MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS_AXIS5)))
        {
          Vector3 vector3_2 = mountPoints[index].Start - (Vector3) center;
          Vector3 vector3_3 = mountPoints[index].End - (Vector3) center;
          MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(new BoundingBoxD((Vector3D) (Vector3.Min(vector3_2, vector3_3) * cubeSize), (Vector3D) (Vector3.Max(vector3_2, vector3_3) * cubeSize)), transform), mountPoints[index].Default ? blue : yellow, 0.2f, true, false);
        }
      }
    }

    private static void DrawMountPointsAxisHelpers(
      MyCubeBlockDefinition def,
      ref MatrixD drawMatrix,
      float cubeSize)
    {
      MatrixD matrix = MatrixD.CreateTranslation((Vector3) def.Center - def.Size * 0.5f) * MatrixD.CreateScale((double) cubeSize) * drawMatrix;
      Vector3D vector3D1;
      for (int index = 0; index < 6; ++index)
      {
        Base6Directions.Direction mountPointDirection = (Base6Directions.Direction) index;
        Vector3D zero = Vector3D.Zero;
        zero.Z = -0.200000002980232;
        Vector3D forward1 = (Vector3D) Vector3.Forward;
        Vector3D right = (Vector3D) Vector3.Right;
        Vector3D up1 = (Vector3D) Vector3.Up;
        Vector3D vector3D2 = Vector3D.Transform((Vector3D) def.MountPointLocalToBlockLocal((Vector3) zero, mountPointDirection), matrix);
        Vector3D forward2 = Vector3D.TransformNormal((Vector3D) def.MountPointLocalNormalToBlockLocal((Vector3) forward1, mountPointDirection), matrix);
        Vector3D up2 = Vector3D.TransformNormal((Vector3D) def.MountPointLocalNormalToBlockLocal((Vector3) up1, mountPointDirection), matrix);
        Vector3D up3 = Vector3D.TransformNormal((Vector3D) def.MountPointLocalNormalToBlockLocal((Vector3) right, mountPointDirection), matrix);
        MatrixD world1 = MatrixD.CreateWorld(vector3D2 + up3 * 0.25, forward2, up3);
        MatrixD world2 = MatrixD.CreateWorld(vector3D2 + up2 * 0.25, forward2, up2);
        Vector4 vector4_1 = Color.Red.ToVector4();
        Vector4 vector4_2 = Color.Green.ToVector4();
        MyRenderProxy.DebugDrawSphere(vector3D2, 0.03f * cubeSize, (Color) Color.Red.ToVector3());
        MySimpleObjectDraw.DrawTransparentCylinder(ref world1, 0.0f, 0.03f * cubeSize, 0.5f * cubeSize, ref vector4_1, false, 16, 0.01f * cubeSize);
        MySimpleObjectDraw.DrawTransparentCylinder(ref world2, 0.0f, 0.03f * cubeSize, 0.5f * cubeSize, ref vector4_2, false, 16, 0.01f * cubeSize);
        MyRenderProxy.DebugDrawLine3D(vector3D2, vector3D2 - forward2 * 0.200000002980232, Color.Red, Color.Red, true);
        float scale1 = 0.5f * cubeSize;
        float scale2 = 0.5f * cubeSize;
        float scale3 = 0.5f * cubeSize;
        if (MySector.MainCamera != null)
        {
          vector3D1 = vector3D2 + up3 * 0.550000011920929 - MySector.MainCamera.Position;
          float num1 = (float) vector3D1.Length();
          vector3D1 = vector3D2 + up2 * 0.550000011920929 - MySector.MainCamera.Position;
          float num2 = (float) vector3D1.Length();
          vector3D1 = vector3D2 + forward2 * 0.100000001490116 - MySector.MainCamera.Position;
          float num3 = (float) vector3D1.Length();
          scale1 = scale1 * 6f / num1;
          scale2 = scale2 * 6f / num2;
          scale3 = scale3 * 6f / num3;
        }
        MyRenderProxy.DebugDrawText3D(vector3D2 + up3 * 0.550000011920929, "X", Color.Red, scale1, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
        MyRenderProxy.DebugDrawText3D(vector3D2 + up2 * 0.550000011920929, "Y", Color.Green, scale2, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
        MyRenderProxy.DebugDrawText3D(vector3D2 + forward2 * 0.100000001490116, MyCubeBuilder.m_mountPointSideNames[index], Color.White, scale3, true, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
      }
      vector3D1 = matrix.Translation - MySector.MainCamera.Position;
      double num4 = vector3D1.Length();
      BoundingBoxD boundingBoxD = new BoundingBoxD((Vector3D) (-def.Size * cubeSize * 0.5f), (Vector3D) (def.Size * cubeSize * 0.5f));
      vector3D1 = boundingBoxD.Size;
      double num5 = vector3D1.Max() * 0.86599999666214;
      double num6 = num4 - num5;
      Color black = Color.Black;
      double num7 = (double) cubeSize * 3.0;
      if (num6 >= num7)
        return;
      ref MatrixD local1 = ref drawMatrix;
      ref BoundingBoxD local2 = ref boundingBoxD;
      ref Color local3 = ref black;
      ref Color local4 = ref black;
      Vector3I wireDivideRatio = def.Size * 10;
      vector3D1 = boundingBoxD.Size;
      double num8 = 0.00499999988824129 / vector3D1.Max() * (double) cubeSize;
      MyStringId? faceMaterial = new MyStringId?();
      MyStringId? lineMaterial = new MyStringId?();
      MySimpleObjectDraw.DrawTransparentBox(ref local1, ref local2, ref local3, ref local4, MySimpleObjectRasterizer.Wireframe, wireDivideRatio, (float) num8, faceMaterial, lineMaterial, true);
    }

    protected static void DrawRemovingCubes(
      Vector3I? startRemove,
      Vector3I? continueBuild,
      MySlimBlock removeBlock)
    {
      if (!startRemove.HasValue || !continueBuild.HasValue || removeBlock == null)
        return;
      Color white = Color.White;
      Vector3I counter;
      MyBlockBuilderBase.ComputeSteps(startRemove.Value, continueBuild.Value, Vector3I.One, out Vector3I _, out counter, out int _);
      MatrixD worldMatrix = removeBlock.CubeGrid.WorldMatrix;
      BoundingBoxD invalid = BoundingBoxD.CreateInvalid();
      invalid.Include((Vector3D) (startRemove.Value * removeBlock.CubeGrid.GridSize));
      invalid.Include((Vector3D) (continueBuild.Value * removeBlock.CubeGrid.GridSize));
      invalid.Min -= new Vector3((float) ((double) removeBlock.CubeGrid.GridSize / 2.0 + 0.0199999995529652));
      invalid.Max += new Vector3((float) ((double) removeBlock.CubeGrid.GridSize / 2.0 + 0.0199999995529652));
      MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref invalid, ref white, ref white, MySimpleObjectRasterizer.Wireframe, counter, 0.04f, lineMaterial: new MyStringId?(MyCubeBuilder.ID_GIZMO_DRAW_LINE_RED), onlyFrontFaces: true);
      Color color = new Color(Color.Red * 0.2f, 0.3f);
      MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref invalid, ref color, MySimpleObjectRasterizer.Solid, 0, 0.04f, new MyStringId?(MyCubeBuilder.ID_SQUARE), onlyFrontFaces: true);
    }

    public override System.Type[] Dependencies
    {
      get
      {
        System.Type[] typeArray = new System.Type[base.Dependencies.Length + 1];
        for (int index = 0; index < base.Dependencies.Length; ++index)
          typeArray[index] = base.Dependencies[index];
        typeArray[typeArray.Length - 1] = typeof (MyToolbarComponent);
        return typeArray;
      }
    }

    public event Action OnBlockSizeChanged;

    public event Action<MyCubeBlockDefinition> OnBlockAdded;

    public event Action OnActivated;

    public event Action OnDeactivated;

    public event Action OnBlockVariantChanged;

    public event Action OnSymmetrySetupModeChanged;

    public MyCubeBuilderToolType ToolType
    {
      get => this.m_toolType;
      set
      {
        if (this.m_toolType == value)
          return;
        this.m_toolType = value;
        if (!MyInput.Static.IsJoystickLastUsed)
          return;
        if (value != MyCubeBuilderToolType.ColorTool && this.SymmetrySettingMode == MySymmetrySettingModeEnum.NoPlane)
          this.ShowAxis = true;
        else
          this.ShowAxis = false;
      }
    }

    public static MyBuildComponentBase BuildComponent { get; set; }

    private bool ShowAxis
    {
      get => this.m_showAxis;
      set
      {
        if (this.m_showAxis == value)
          return;
        this.m_showAxis = value;
      }
    }

    public bool CompoundEnabled { get; protected set; }

    public int RotationAxis => this.m_selectedAxis;

    public bool BlockCreationIsActivated
    {
      get => this.m_blockCreationActivated;
      private set => this.m_blockCreationActivated = value;
    }

    public override bool IsActivated => this.BlockCreationIsActivated;

    public bool UseSymmetry
    {
      get => this.m_useSymmetry && MySession.Static != null && (MySession.Static.CreativeMode || MySession.Static.CreativeToolsEnabled(Sync.MyId)) && !(MySession.Static.ControlledEntity is MyShipController);
      set
      {
        if (this.m_useSymmetry == value)
          return;
        this.m_useSymmetry = value;
        MySandboxGame.Config.CubeBuilderUseSymmetry = value;
        MySandboxGame.Config.Save();
      }
    }

    public bool UseTransparency
    {
      get => this.m_useTransparency;
      set
      {
        if (this.m_useTransparency == value)
          return;
        this.m_useTransparency = value;
        this.m_renderData.BeginCollectingInstanceData();
        this.m_rotationHints.Clear();
        this.m_renderData.EndCollectingInstanceData(this.CurrentGrid != null ? this.CurrentGrid.WorldMatrix : MatrixD.Identity, this.UseTransparency);
      }
    }

    public bool AlignToDefault
    {
      get => this.m_alignToDefault;
      set
      {
        if (this.m_alignToDefault == value)
          return;
        this.m_alignToDefault = value;
        MySandboxGame.Config.CubeBuilderAlignToDefault = value;
        MySandboxGame.Config.Save();
      }
    }

    public bool FreezeGizmo { get; set; }

    public bool ShowRemoveGizmo { get; set; }

    private MySymmetrySettingModeEnum SymmetrySettingMode
    {
      get => this.m_symmetrySettingMode;
      set
      {
        if (this.m_symmetrySettingMode == value)
          return;
        this.m_symmetrySettingMode = value;
        if (value != MySymmetrySettingModeEnum.NoPlane || this.ToolType == MyCubeBuilderToolType.ColorTool)
        {
          this.ShowAxis = false;
        }
        else
        {
          if (!MyInput.Static.IsJoystickLastUsed)
            return;
          this.ShowAxis = true;
        }
      }
    }

    public MyCubeBuilderState CubeBuilderState => this.m_cubeBuilderState;

    protected internal override MyCubeGrid CurrentGrid
    {
      get => this.m_currentGrid;
      protected set
      {
        if (this.FreezeGizmo || this.m_currentGrid == value)
          return;
        this.BeforeCurrentGridChange(value);
        this.m_currentGrid = value;
        this.m_customRotation = false;
        if (this.IsCubeSizeModesAvailable && this.CurrentBlockDefinition != null && this.m_currentGrid != null)
        {
          MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(this.CurrentBlockDefinition.BlockPairName);
          int index = this.m_cubeBuilderState.CurrentBlockDefinitionStages.IndexOf(this.CurrentBlockDefinition);
          MyCubeSize gridSizeEnum = this.m_currentGrid.GridSizeEnum;
          if (gridSizeEnum != this.CurrentBlockDefinition.CubeSize && (gridSizeEnum == MyCubeSize.Small && definitionGroup.Small != null || gridSizeEnum == MyCubeSize.Large && definitionGroup.Large != null))
          {
            this.m_cubeBuilderState.SetCubeSize(gridSizeEnum);
            this.SetSurvivalIntersectionDist();
            if (gridSizeEnum == MyCubeSize.Small && this.CubePlacementMode == MyCubeBuilder.CubePlacementModeEnum.LocalCoordinateSystem)
              this.CycleCubePlacementMode();
            if (index != -1 && index < this.m_cubeBuilderState.CurrentBlockDefinitionStages.Count)
              this.UpdateCubeBlockStageDefinition(this.m_cubeBuilderState.CurrentBlockDefinitionStages[index]);
          }
        }
        if (this.m_currentGrid != null)
          return;
        this.RemoveSymmetryNotification();
        this.m_gizmo.Clear();
      }
    }

    protected internal override MyVoxelBase CurrentVoxelBase
    {
      get => this.m_currentVoxelBase;
      protected set
      {
        if (this.FreezeGizmo || this.m_currentVoxelBase == value)
          return;
        this.m_currentVoxelBase = value;
        if (this.m_currentVoxelBase != null)
          return;
        this.RemoveSymmetryNotification();
        this.m_gizmo.Clear();
      }
    }

    public override MyCubeBlockDefinition CurrentBlockDefinition
    {
      get => this.m_cubeBuilderState == null ? (MyCubeBlockDefinition) null : this.m_cubeBuilderState.CurrentBlockDefinition;
      protected set
      {
        if (this.m_cubeBuilderState == null)
          return;
        if (this.m_cubeBuilderState.CurrentBlockDefinition != null)
          this.m_lastBlockDefinition = this.m_cubeBuilderState.CurrentBlockDefinition;
        this.m_cubeBuilderState.CurrentBlockDefinition = value;
      }
    }

    public MyCubeBlockDefinition ToolbarBlockDefinition
    {
      get
      {
        if (this.m_cubeBuilderState == null)
          return (MyCubeBlockDefinition) null;
        return MyFakes.ENABLE_BLOCK_STAGES && this.m_cubeBuilderState.CurrentBlockDefinitionStages.Count > 0 ? this.m_cubeBuilderState.CurrentBlockDefinitionStages[0] : this.CurrentBlockDefinition;
      }
    }

    public event Action OnToolTypeChanged;

    public static MyCubeBuilder.BuildingModeEnum BuildingMode
    {
      get
      {
        int num = MySandboxGame.Config.CubeBuilderBuildingMode;
        if (!Enum.IsDefined(typeof (MyCubeBuilder.BuildingModeEnum), (object) num))
          num = 0;
        return (MyCubeBuilder.BuildingModeEnum) num;
      }
      set => MySandboxGame.Config.CubeBuilderBuildingMode = (int) value;
    }

    public virtual bool IsCubeSizeModesAvailable => true;

    public bool IsBuildMode
    {
      get => this.m_isBuildMode;
      set
      {
        this.m_isBuildMode = value;
        MyHud.IsBuildMode = value;
        if (value)
          this.ActivateBuildModeNotifications(MyInput.Static.IsJoystickConnected() && MyInput.Static.IsJoystickLastUsed && MyFakes.ENABLE_CONTROLLER_HINTS);
        else
          this.DeactivateBuildModeNotifications();
      }
    }

    public MyCubeBuilder.CubePlacementModeEnum CubePlacementMode
    {
      get => this.m_cubePlacementMode;
      set
      {
        if (this.m_cubePlacementMode == value)
          return;
        this.m_cubePlacementMode = value;
        if (this.IsBuildToolActive())
        {
          this.ShowCubePlacementNotification();
        }
        else
        {
          if (!this.IsOnlyColorToolActive())
            return;
          this.ShowColorToolNotifications();
        }
      }
    }

    public bool DynamicMode { get; protected set; }

    static MyCubeBuilder()
    {
      if (!Sync.IsServer)
        return;
      MyCubeBuilder.AllPlayersColors = new Dictionary<MyPlayer.PlayerId, List<Vector3>>();
    }

    public MyCubeBuilder()
    {
      this.m_gizmo = new MyCubeBuilderGizmo();
      this.InitializeNotifications();
    }

    public override void InitFromDefinition(MySessionComponentDefinition definition) => base.InitFromDefinition(definition);

    public override void LoadData()
    {
      base.LoadData();
      this.m_cubeBuilderState = new MyCubeBuilderState();
      MyCubeBuilder.Static = this;
      this.m_gameInventory = MySession.Static.GetComponent<MySessionComponentGameInventory>();
      this.m_useSymmetry = MySandboxGame.Config.CubeBuilderUseSymmetry;
      this.m_alignToDefault = MySandboxGame.Config.CubeBuilderAlignToDefault;
    }

    protected bool GridValid => this.BlockCreationIsActivated && this.CurrentGrid != null;

    protected bool GridAndBlockValid
    {
      get
      {
        if (!this.GridValid || this.CurrentBlockDefinition == null)
          return false;
        return this.CurrentBlockDefinition.CubeSize == this.CurrentGrid.GridSizeEnum || this.PlacingSmallGridOnLargeStatic;
      }
    }

    protected bool VoxelMapAndBlockValid => this.BlockCreationIsActivated && this.CurrentVoxelBase != null && this.CurrentBlockDefinition != null;

    public bool PlacingSmallGridOnLargeStatic => MyFakes.ENABLE_STATIC_SMALL_GRID_ON_LARGE && this.GridValid && (this.CurrentBlockDefinition != null && this.CurrentBlockDefinition.CubeSize == MyCubeSize.Small) && this.CurrentGrid.GridSizeEnum == MyCubeSize.Large && this.CurrentGrid.IsStatic;

    protected bool BuildInputValid => this.GridAndBlockValid || this.VoxelMapAndBlockValid || this.DynamicMode || this.CurrentBlockDefinition != null;

    private float CurrentBlockScale => this.CurrentBlockDefinition == null ? 1f : MyDefinitionManager.Static.GetCubeSize(this.CurrentBlockDefinition.CubeSize) / MyDefinitionManager.Static.GetCubeSizeOriginal(this.CurrentBlockDefinition.CubeSize);

    protected virtual void RotateAxis(int index, int sign, double angleDelta, bool newlyPressed)
    {
      if (this.DynamicMode)
      {
        MatrixD worldMatrixAdd = this.m_gizmo.SpaceDefault.m_worldMatrixAdd;
        MatrixD rotatedMatrix;
        if (!MyCubeBuilder.CalculateBlockRotation(index, sign, ref worldMatrixAdd, out rotatedMatrix, angleDelta))
          return;
        this.m_gizmo.SpaceDefault.m_worldMatrixAdd = rotatedMatrix;
      }
      else
      {
        if (!newlyPressed)
          return;
        angleDelta = Math.PI / 2.0;
        MatrixD currentMatrix = (MatrixD) ref this.m_gizmo.SpaceDefault.m_localMatrixAdd;
        MatrixD rotatedMatrix;
        if (!MyCubeBuilder.CalculateBlockRotation(index, sign, ref currentMatrix, out rotatedMatrix, angleDelta, this.CurrentBlockDefinition != null ? this.CurrentBlockDefinition.Direction : MyBlockDirection.Both, this.CurrentBlockDefinition != null ? this.CurrentBlockDefinition.Rotation : MyBlockRotation.Both))
          return;
        MyGuiAudio.PlaySound(MyGuiSounds.HudRotateBlock);
        this.m_gizmo.RotateAxis(ref rotatedMatrix);
      }
    }

    public static bool CalculateBlockRotation(
      int index,
      int sign,
      ref MatrixD currentMatrix,
      out MatrixD rotatedMatrix,
      double angle,
      MyBlockDirection blockDirection = MyBlockDirection.Both,
      MyBlockRotation blockRotation = MyBlockRotation.Both)
    {
      MatrixD matrixD = MatrixD.Identity;
      if (index == 2)
        sign *= -1;
      Vector3D zero = Vector3D.Zero;
      switch (index)
      {
        case 0:
          zero.X += (double) sign * angle;
          matrixD = MatrixD.CreateFromAxisAngle(currentMatrix.Right, (double) sign * angle);
          break;
        case 1:
          zero.Y += (double) sign * angle;
          matrixD = MatrixD.CreateFromAxisAngle(currentMatrix.Up, (double) sign * angle);
          break;
        case 2:
          zero.Z += (double) sign * angle;
          matrixD = MatrixD.CreateFromAxisAngle(currentMatrix.Forward, (double) sign * angle);
          break;
      }
      rotatedMatrix = currentMatrix;
      rotatedMatrix *= matrixD;
      rotatedMatrix = MatrixD.Orthogonalize(rotatedMatrix);
      bool flag = MyCubeBuilder.CheckValidBlockRotation((Matrix) ref rotatedMatrix, blockDirection, blockRotation);
      if (flag && !MyCubeBuilder.Static.DynamicMode)
      {
        if (!MyCubeBuilder.Static.m_animationLock)
          MyCubeBuilder.Static.m_animationLock = true;
        else
          flag = !flag;
      }
      return flag;
    }

    private void ActivateBlockCreation(MyDefinitionId? blockDefinitionId = null)
    {
      if (MySession.Static.CameraController != null)
      {
        int num = MySession.Static.CameraController.AllowCubeBuilding ? 1 : 0;
      }
      if (MySession.Static.ControlledEntity is MyShipController && !(MySession.Static.ControlledEntity as MyShipController).BuildingMode || MySession.Static.ControlledEntity is MyCharacter && (MySession.Static.ControlledEntity as MyCharacter).IsDead)
        return;
      if (!blockDefinitionId.HasValue)
        blockDefinitionId = this.m_lastBlockDefinition == null ? new MyDefinitionId?(MyCubeBuilder.DEFAULT_BLOCK) : new MyDefinitionId?(this.m_lastBlockDefinition.Id);
      bool updateNotAvailableNotification = false;
      if (this.IsCubeSizeModesAvailable && blockDefinitionId.HasValue && this.CurrentBlockDefinition != null)
      {
        bool flag = true;
        if (CheckBlock(this.CurrentBlockDefinition))
        {
          flag = false;
        }
        else
        {
          foreach (MyCubeBlockDefinition blockDefinitionStage in this.m_cubeBuilderState.CurrentBlockDefinitionStages)
          {
            if (CheckBlock(blockDefinitionStage))
            {
              flag = false;
              break;
            }
          }
        }
        MyCubeBlockDefinition cubeBlockDefinition = this.CurrentBlockDefinition;
        if (flag)
          cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(blockDefinitionId.Value);
        MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(cubeBlockDefinition.BlockPairName);
        this.OnBlockSizeChanged.InvokeIfNotNull();
        MyCubeSize myCubeSize = this.m_cubeBuilderState.CubeSizeMode == MyCubeSize.Large ? MyCubeSize.Small : MyCubeSize.Large;
        if (flag || definitionGroup[myCubeSize] != null)
        {
          if (flag)
          {
            this.m_cubeBuilderState.CurrentBlockDefinition = cubeBlockDefinition;
            this.m_cubeBuilderState.SetCubeSize(cubeBlockDefinition.CubeSize);
          }
          else
            this.m_cubeBuilderState.SetCubeSize(myCubeSize);
          this.SetSurvivalIntersectionDist();
          if (myCubeSize == MyCubeSize.Small && this.CubePlacementMode == MyCubeBuilder.CubePlacementModeEnum.LocalCoordinateSystem)
            this.CycleCubePlacementMode();
          int index = this.m_cubeBuilderState.CurrentBlockDefinitionStages.IndexOf(this.CurrentBlockDefinition);
          if (index != -1 && this.m_cubeBuilderState.CurrentBlockDefinitionStages.Count > 0)
            this.UpdateCubeBlockStageDefinition(this.m_cubeBuilderState.CurrentBlockDefinitionStages[index]);
        }
        else
          updateNotAvailableNotification = true;
      }
      else if (this.CurrentBlockDefinition == null && blockDefinitionId.HasValue)
      {
        MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(blockDefinitionId.Value);
        MyCubeSize newCubeSize = this.m_cubeBuilderState.CubeSizeMode;
        if (cubeBlockDefinition.CubeSize != newCubeSize && (cubeBlockDefinition.CubeSize == MyCubeSize.Large ? MyDefinitionManager.Static.GetDefinitionGroup(cubeBlockDefinition.BlockPairName).Small : MyDefinitionManager.Static.GetDefinitionGroup(cubeBlockDefinition.BlockPairName).Large) == null)
          newCubeSize = cubeBlockDefinition.CubeSize;
        this.m_cubeBuilderState.SetCubeSize(newCubeSize);
        this.CubePlacementMode = MyCubeBuilder.CubePlacementModeEnum.FreePlacement;
        if (this.IsBuildToolActive())
          this.ShowCubePlacementNotification();
        else if (this.IsOnlyColorToolActive())
          this.ShowColorToolNotifications();
      }
      this.UpdateNotificationBlockNotAvailable(updateNotAvailableNotification);
      this.UpdateCubeBlockDefinition(blockDefinitionId);
      this.SetSurvivalIntersectionDist();
      if (MySession.Static.CreativeMode)
      {
        this.AllowFreeSpacePlacement = false;
        this.MaxGridDistanceFrom = new Vector3D?();
        this.ShowRemoveGizmo = MyFakes.SHOW_REMOVE_GIZMO;
      }
      else
      {
        this.AllowFreeSpacePlacement = false;
        this.ShowRemoveGizmo = true;
      }
      this.ActivateNotifications();
      if (!(MySession.Static.ControlledEntity is MyShipController) || !(MySession.Static.ControlledEntity as MyShipController).BuildingMode)
        MyHud.Crosshair.ResetToDefault();
      this.BlockCreationIsActivated = true;
      this.AlignToGravity();

      bool CheckBlock(MyCubeBlockDefinition b)
      {
        MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(b.BlockPairName);
        foreach (MyCubeSize size in MyEnum<MyCubeSize>.Values)
        {
          MyDefinitionId? id = definitionGroup[size]?.Id;
          MyDefinitionId myDefinitionId = blockDefinitionId.Value;
          if ((id.HasValue ? (id.HasValue ? (id.GetValueOrDefault() == myDefinitionId ? 1 : 0) : 1) : 0) != 0)
            return true;
        }
        return false;
      }
    }

    public void DeactivateBlockCreation()
    {
      if (this.m_cubeBuilderState != null && this.m_cubeBuilderState.CurrentBlockDefinition != null)
        this.m_cubeBuilderState.UpdateCubeBlockDefinition(new MyDefinitionId?(this.m_cubeBuilderState.CurrentBlockDefinition.Id), (MatrixD) ref this.m_gizmo.SpaceDefault.m_localMatrixAdd);
      this.BlockCreationIsActivated = false;
      this.DeactivateNotifications();
    }

    private void ActivateNotifications()
    {
      if (this.m_cubePlacementModeHint == null)
        return;
      this.m_cubePlacementModeHint.Level = MyNotificationLevel.Control;
      MyHud.Notifications.Add((MyHudNotificationBase) this.m_cubePlacementModeHint);
    }

    private void DeactivateNotifications()
    {
    }

    protected virtual bool IsDynamicOverride() => this.m_cubeBuilderState != null && this.m_cubeBuilderState.CurrentBlockDefinition != null && this.CurrentGrid != null && this.m_cubeBuilderState.CurrentBlockDefinition.CubeSize != this.CurrentGrid.GridSizeEnum;

    private void ActivateBuildModeNotifications(bool joystick)
    {
    }

    private void DeactivateBuildModeNotifications()
    {
    }

    private void InitializeNotifications()
    {
      this.m_cubePlacementModeNotification = new MyHudNotification(MyCommonTexts.NotificationCubePlacementModeChanged);
      this.UpdatePlacementNotificationState();
      this.m_cubePlacementModeHint = new MyHudNotification(MyCommonTexts.ControlHintCubePlacementMode, MyHudNotificationBase.INFINITE);
      this.m_cubePlacementModeHint.Level = MyNotificationLevel.Control;
      this.m_cubePlacementUnable = new MyHudNotification(MyCommonTexts.NotificationCubePlacementUnable, font: "Red");
      this.m_coloringToolHints = new MyHudNotification(MyCommonTexts.ControlHintColoringTool, MyHudNotificationBase.INFINITE);
      this.m_coloringToolHints.Level = MyNotificationLevel.Control;
    }

    private void UpdatePlacementNotificationState() => this.m_cubePlacementModeNotification.m_lifespanMs = MySandboxGame.Config.ControlsHints ? 0 : 2500;

    public override void Deactivate()
    {
      if (!this.Loaded)
        return;
      if (this.BlockCreationIsActivated)
        this.DeactivateBlockCreation();
      if (this.m_cubeBuilderState != null)
        this.CurrentBlockDefinition = (MyCubeBlockDefinition) null;
      this.m_stationPlacement = false;
      this.CurrentGrid = (MyCubeGrid) null;
      this.CurrentVoxelBase = (MyVoxelBase) null;
      this.IsBuildMode = false;
      MyBlockBuilderBase.PlacementProvider = (IMyPlacementProvider) null;
      if (!Sandbox.Engine.Platform.Game.IsDedicated)
        this.m_rotationHints.ReleaseRenderData();
      if (MyCoordinateSystem.Static != null)
        MyCoordinateSystem.Static.Visible = false;
      MyHud.Notifications.Remove((MyHudNotificationBase) this.m_cubePlacementModeNotification);
      MyHud.Notifications.Remove((MyHudNotificationBase) this.m_cubePlacementModeHint);
      MyHud.Notifications.Remove((MyHudNotificationBase) this.m_coloringToolHints);
      this.RemoveSymmetryNotification();
      Action onDeactivated = this.OnDeactivated;
      if (onDeactivated == null)
        return;
      onDeactivated();
    }

    public void OnLostFocus() => this.Deactivate();

    public override void Activate(MyDefinitionId? blockDefinitionId = null)
    {
      if (blockDefinitionId.HasValue && !MySession.Static.CheckResearchAndNotify(MySession.Static.LocalPlayerId, blockDefinitionId.Value))
        return;
      this.ToolType = MyCubeBuilderToolType.Combined;
      if (MySession.Static.CameraController != null)
        MySession.Static.GameFocusManager.Register((IMyFocusHolder) this);
      this.ActivateBlockCreation(blockDefinitionId);
      Action onActivated = this.OnActivated;
      if (onActivated == null)
        return;
      onActivated();
    }

    public void ActivateFromRadialMenu(MyDefinitionId? blockDefinitionId = null)
    {
      if (blockDefinitionId.HasValue)
        this.CurrentBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(blockDefinitionId.Value);
      this.Activate(blockDefinitionId);
    }

    public void SetToolType(MyCubeBuilderToolType type)
    {
      this.ToolType = type;
      if (this.IsBuildToolActive())
      {
        this.ShowCubePlacementNotification();
        this.ShowAxis = MyInput.Static.IsJoystickLastUsed;
      }
      else if (this.IsOnlyColorToolActive())
      {
        this.ShowColorToolNotifications();
        this.ShowAxis = false;
      }
      this.OnToolTypeChanged.InvokeIfNotNull();
    }

    protected virtual void UpdateCubeBlockStageDefinition(
      MyCubeBlockDefinition stageCubeBlockDefinition)
    {
      if (this.CurrentBlockDefinition != null && stageCubeBlockDefinition != null)
        this.m_cubeBuilderState.RotationsByDefinitionHash[this.CurrentBlockDefinition.Id] = Quaternion.CreateFromRotationMatrix(this.m_gizmo.SpaceDefault.m_localMatrixAdd);
      this.CurrentBlockDefinition = stageCubeBlockDefinition;
      this.m_gizmo.RotationOptions = MyCubeGridDefinitions.GetCubeRotationOptions(this.CurrentBlockDefinition);
      Quaternion quaternion;
      if (this.m_cubeBuilderState.RotationsByDefinitionHash.TryGetValue(stageCubeBlockDefinition.Id, out quaternion))
        this.m_gizmo.SpaceDefault.m_localMatrixAdd = Matrix.CreateFromQuaternion(quaternion);
      else
        this.m_gizmo.SpaceDefault.m_localMatrixAdd = Matrix.Identity;
    }

    protected virtual void UpdateCubeBlockDefinition(MyDefinitionId? id)
    {
      this.m_cubeBuilderState.UpdateCubeBlockDefinition(id, (MatrixD) ref this.m_gizmo.SpaceDefault.m_localMatrixAdd);
      if (this.CurrentBlockDefinition != null && this.IsCubeSizeModesAvailable)
        this.m_cubeBuilderState.UpdateComplementBlock();
      this.m_cubeBuilderState.UpdateCurrentBlockToLastSelectedVariant();
      if (this.m_cubeBuilderState.CurrentBlockDefinition == null)
        return;
      this.m_gizmo.RotationOptions = MyCubeGridDefinitions.GetCubeRotationOptions(this.CurrentBlockDefinition);
      Quaternion quaternion;
      if (this.m_cubeBuilderState.RotationsByDefinitionHash.TryGetValue(id.HasValue ? id.Value : new MyDefinitionId(), out quaternion))
        this.m_gizmo.SpaceDefault.m_localMatrixAdd = Matrix.CreateFromQuaternion(quaternion);
      else
        this.m_gizmo.SpaceDefault.m_localMatrixAdd = Matrix.Identity;
    }

    public void AddFastBuildModels(
      MatrixD baseMatrix,
      ref Matrix localMatrixAdd,
      List<MatrixD> matrices,
      List<string> models,
      MyCubeBlockDefinition definition,
      Vector3I? startBuild,
      Vector3I? continueBuild)
    {
      MyBlockBuilderBase.AddFastBuildModelWithSubparts(ref baseMatrix, matrices, models, definition, this.CurrentBlockScale);
      if (this.CurrentBlockDefinition == null || !startBuild.HasValue || !continueBuild.HasValue)
        return;
      Vector3I result;
      Vector3I.TransformNormal(ref this.CurrentBlockDefinition.Size, ref localMatrixAdd, out result);
      Vector3I rotatedSize = Vector3I.Abs(result);
      Vector3I stepDelta;
      Vector3I counter;
      MyBlockBuilderBase.ComputeSteps(startBuild.Value, continueBuild.Value, rotatedSize, out stepDelta, out counter, out int _);
      Vector3I zero = Vector3I.Zero;
      for (int index1 = 0; index1 < counter.X; zero.X += stepDelta.X)
      {
        zero.Y = 0;
        for (int index2 = 0; index2 < counter.Y; zero.Y += stepDelta.Y)
        {
          zero.Z = 0;
          for (int index3 = 0; index3 < counter.Z; zero.Z += stepDelta.Z)
          {
            Vector3I vector3I = zero;
            Vector3 vector3;
            if (this.CurrentGrid != null)
            {
              vector3 = (Vector3) Vector3.Transform(vector3I * this.CurrentGrid.GridSize, this.CurrentGrid.WorldMatrix.GetOrientation());
            }
            else
            {
              float cubeSize = MyDefinitionManager.Static.GetCubeSize(this.CurrentBlockDefinition.CubeSize);
              vector3 = vector3I * cubeSize;
            }
            MatrixD matrix = baseMatrix;
            matrix.Translation += vector3;
            MyBlockBuilderBase.AddFastBuildModelWithSubparts(ref matrix, matrices, models, definition, this.CurrentBlockScale);
            ++index3;
          }
          ++index2;
        }
        ++index1;
      }
    }

    private void AddFastBuildModels(
      MyCubeBuilderGizmo.MyGizmoSpaceProperties gizmoSpace,
      MatrixD baseMatrix,
      List<MatrixD> matrices,
      List<string> models,
      MyCubeBlockDefinition definition)
    {
      this.AddFastBuildModels(baseMatrix, ref gizmoSpace.m_localMatrixAdd, matrices, models, definition, gizmoSpace.m_startBuild, gizmoSpace.m_continueBuild);
    }

    public void AlignToGravity(bool alignToCamera = true)
    {
      Vector3 totalGravityInPoint = MyGravityProviderSystem.CalculateTotalGravityInPoint(MyBlockBuilderBase.IntersectionStart);
      if ((double) totalGravityInPoint.LengthSquared() <= 0.0)
        return;
      Matrix matrix = (Matrix) ref this.m_gizmo.SpaceDefault.m_worldMatrixAdd;
      double num = (double) totalGravityInPoint.Normalize();
      Vector3D vector3D = !(MySector.MainCamera != null & alignToCamera) ? Vector3D.Reject(this.m_gizmo.SpaceDefault.m_worldMatrixAdd.Forward, (Vector3D) totalGravityInPoint) : Vector3D.Reject((Vector3D) MySector.MainCamera.ForwardVector, (Vector3D) totalGravityInPoint);
      if (!vector3D.IsValid() || vector3D.LengthSquared() <= double.Epsilon)
        vector3D = Vector3D.CalculatePerpendicularVector((Vector3D) totalGravityInPoint);
      vector3D.Normalize();
      MyCubeBuilderGizmo.MyGizmoSpaceProperties spaceDefault = this.m_gizmo.SpaceDefault;
      Matrix world = Matrix.CreateWorld(matrix.Translation, (Vector3) vector3D, -totalGravityInPoint);
      MatrixD matrixD = (MatrixD) ref world;
      spaceDefault.m_worldMatrixAdd = matrixD;
    }

    public virtual bool HandleGameInput()
    {
      if (this.HandleExportInput())
        return true;
      if (MyGuiScreenGamePlay.DisableInput)
        return false;
      IMyControllableEntity controlledEntity1 = MySession.Static.ControlledEntity;
      if (MyControllerHelper.IsControl(controlledEntity1 != null ? controlledEntity1.ControlContext : MyStringId.NullOrEmpty, MyControlsSpace.COLOR_PICKER) && MySession.Static.ControlledEntity == MySession.Static.LocalCharacter && (MySession.Static.LocalHumanPlayer != null && MySession.Static.LocalHumanPlayer.Identity.Character == MySession.Static.ControlledEntity) && (!MyInput.Static.IsAnyShiftKeyPressed() && MyGuiScreenGamePlay.ActiveGameplayScreen == null))
      {
        MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
        MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = (MyGuiScreenBase) new MyGuiScreenColorPicker());
      }
      IMyControllableEntity controlledEntity2 = MySession.Static.ControlledEntity;
      MyStringId context = controlledEntity2 != null ? controlledEntity2.AuxiliaryContext : MyStringId.NullOrEmpty;
      if (MyControllerHelper.IsControl(context, MyControlsSpace.SLOT0))
      {
        this.Deactivate();
        return true;
      }
      if (MyControllerHelper.IsControl(context, MyControlsSpace.CUBE_DEFAULT_MOUNTPOINT))
      {
        this.AlignToDefault = !this.AlignToDefault;
        MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
      }
      if (!this.IsActivated)
        return false;
      int frameDt = MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastInputHandleTime;
      this.m_lastInputHandleTime += frameDt;
      bool flag1 = MySession.Static.ControlledEntity is MyCockpit && !MyBlockBuilderBase.SpectatorIsBuilding;
      if (flag1 && MySession.Static.ControlledEntity is MyCockpit && (MySession.Static.ControlledEntity as MyCockpit).BuildingMode)
        flag1 = false;
      if (MySandboxGame.IsPaused | flag1)
        return false;
      if (MySession.Static.LocalCharacter != null && MyControllerHelper.IsControl(context, MyControlsSpace.PRIMARY_TOOL_ACTION) && (MySession.Static.ControlledEntity is MyCockpit && (MySession.Static.ControlledEntity as MyCockpit).BuildingMode) && MySession.Static.SurvivalMode)
        MySession.Static.LocalCharacter.BeginShoot(MyShootActionEnum.PrimaryAction);
      if (this.IsActivated && MyControllerHelper.IsControl(context, MyControlsSpace.BUILD_MODE))
        this.IsBuildMode = !this.IsBuildMode;
      if (MyControllerHelper.IsControl(context, MyControlsSpace.FREE_ROTATION))
      {
        MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
        this.CycleCubePlacementMode();
      }
      this.GamepadControls(context);
      if (this.HandleAdminAndCreativeInput(context))
        return true;
      this.HandleSurvivalInput(context);
      if (this.CurrentGrid != null && MyInput.Static.IsNewGameControlPressed(MyControlsSpace.COLOR_PICKER) && MyInput.Static.IsAnyShiftKeyPressed())
        this.PickColor();
      bool flag2 = false;
      if (MySession.Static.LocalCharacter != null)
      {
        MyCharacterDetectorComponent detectorComponent = MySession.Static.LocalCharacter.Components.Get<MyCharacterDetectorComponent>();
        if (detectorComponent != null && detectorComponent.UseObject != null && detectorComponent.UseObject.SupportedActions.HasFlag((Enum) UseActionEnum.BuildPlanner))
          flag2 = true;
        if (!MyControllerHelper.IsControl(context, MyControlsSpace.BUILD_PLANNER, MyControlStateType.PRESSED))
          flag2 = false;
      }
      if (this.CurrentGrid != null && MyControllerHelper.IsControl(context, MyControlsSpace.CUBE_COLOR_CHANGE, MyControlStateType.PRESSED) && !flag2)
        this.RecolorControlsKeyboard();
      if (this.CurrentGrid != null && this.IsOnlyColorToolActive())
        this.RecolorControlsGamepad();
      if (this.HandleRotationInput(context, frameDt))
        return true;
      if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.SWITCH_LEFT))
      {
        if (MyInput.Static.IsAnyCtrlKeyPressed())
          this.SwitchToPreviousSkin();
        else
          this.SwitchToPreviousColor();
      }
      if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.SWITCH_RIGHT))
      {
        if (MyInput.Static.IsAnyCtrlKeyPressed())
          this.SwitchToNextSkin();
        else
          this.SwitchToNextColor();
      }
      return this.HandleBlockVariantsInput(context);
    }

    private void PickColor()
    {
      foreach (MyCubeBuilderGizmo.MyGizmoSpaceProperties space in this.m_gizmo.Spaces)
      {
        if (space.m_removeBlock != null && MySession.Static.LocalHumanPlayer != null)
        {
          MySession.Static.LocalHumanPlayer.ChangeOrSwitchToColor(space.m_removeBlock.ColorMaskHSV);
          MyStringHash skinSubtypeId = space.m_removeBlock.SkinSubtypeId;
          MySession.Static.LocalHumanPlayer.BuildArmorSkin = skinSubtypeId != MyStringHash.NullOrEmpty ? skinSubtypeId.ToString() : string.Empty;
        }
      }
    }

    private void RecolorControlsKeyboard()
    {
      int expand = 0;
      if (MyInput.Static.IsAnyCtrlKeyPressed() && MyInput.Static.IsAnyShiftKeyPressed())
        expand = -1;
      else if (MyInput.Static.IsAnyCtrlKeyPressed())
        expand = 1;
      else if (MyInput.Static.IsAnyShiftKeyPressed())
        expand = 3;
      this.Change(expand);
    }

    private void RecolorControlsGamepad()
    {
      int expand = 0;
      bool flag = false;
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      MyStringId context = controlledEntity != null ? controlledEntity.AuxiliaryContext : MyStringId.NullOrEmpty;
      if (MyControllerHelper.IsControl(context, MyControlsSpace.RECOLOR, MyControlStateType.PRESSED))
      {
        expand = 0;
        flag = true;
      }
      if (MyControllerHelper.IsControl(context, MyControlsSpace.MEDIUM_COLOR_BRUSH, MyControlStateType.PRESSED))
      {
        expand = 1;
        flag = true;
      }
      if (MyControllerHelper.IsControl(context, MyControlsSpace.LARGE_COLOR_BRUSH, MyControlStateType.PRESSED))
      {
        expand = 3;
        flag = true;
      }
      if (MyControllerHelper.IsControl(context, MyControlsSpace.RECOLOR_WHOLE_GRID, MyControlStateType.PRESSED))
      {
        expand = -1;
        flag = true;
      }
      if (!flag)
        return;
      this.Change(expand);
    }

    private bool HandleBlockVariantsInput(MyStringId context)
    {
      if (MyFakes.ENABLE_BLOCK_STAGES && this.CurrentBlockDefinition != null && (this.m_cubeBuilderState.CurrentBlockDefinitionStages.Count > 0 && !this.FreezeGizmo))
      {
        bool? nullable = new bool?();
        int num1 = MyInput.Static.MouseScrollWheelValue();
        if (!MyControllerHelper.IsControl(MyControllerHelper.CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED) && !MyInput.Static.IsAnyCtrlKeyPressed() && !MyInput.Static.IsAnyShiftKeyPressed())
        {
          if (num1 != 0 && MyInput.Static.PreviousMouseScrollWheelValue() < MyInput.Static.MouseScrollWheelValue() || MyControllerHelper.IsControl(context, MyControlsSpace.PREV_BLOCK_STAGE))
            nullable = new bool?(false);
          else if (num1 != 0 && MyInput.Static.PreviousMouseScrollWheelValue() > MyInput.Static.MouseScrollWheelValue() || MyControllerHelper.IsControl(context, MyControlsSpace.NEXT_BLOCK_STAGE))
            nullable = new bool?(true);
        }
        if (nullable.HasValue)
        {
          if (MyInput.Static.GetMouseScrollBlockSelectionInversion())
            nullable = new bool?(!nullable.Value);
          int num2 = this.m_cubeBuilderState.CurrentBlockDefinitionStages.IndexOf(this.CurrentBlockDefinition);
          int num3 = nullable.Value ? 1 : -1;
          int index = num2;
          int num4 = (this.m_cubeBuilderState.CurrentBlockDefinitionStages.Count + 1) * 2;
          while ((index += num3) != num2 && num4-- != 0)
          {
            if (index >= this.m_cubeBuilderState.CurrentBlockDefinitionStages.Count)
              index = 0;
            else if (index < 0)
              index = this.m_cubeBuilderState.CurrentBlockDefinitionStages.Count - 1;
            MyCubeBlockDefinition blockDefinitionStage = this.m_cubeBuilderState.CurrentBlockDefinitionStages[index];
            if ((!MySession.Static.SurvivalMode || blockDefinitionStage.AvailableInSurvival && (MyFakes.ENABLE_MULTIBLOCK_CONSTRUCTION || blockDefinitionStage.MultiBlock == null)) && (MySession.Static.GetComponent<MySessionComponentDLC>().HasDefinitionDLC((MyDefinitionBase) blockDefinitionStage, Sync.MyId) && (MySessionComponentResearch.Static.CanUse(MySession.Static.LocalPlayerId, blockDefinitionStage.Id) || MySession.Static.CreativeToolsEnabled(Sync.MyId))))
            {
              this.UpdateCubeBlockStageDefinition(this.m_cubeBuilderState.CurrentBlockDefinitionStages[index]);
              this.CubeBuilderState.SetCurrentBlockForBlockVariantGroup(MyDefinitionManager.Static.GetDefinitionGroup(this.CurrentBlockDefinition.BlockPairName));
              Action blockVariantChanged = this.OnBlockVariantChanged;
              if (blockVariantChanged != null)
              {
                blockVariantChanged();
                break;
              }
              break;
            }
          }
        }
      }
      if ((!this.IsCubeSizeModesAvailable ? 0 : (this.CurrentBlockDefinition != null ? 1 : 0)) == 0 || !MyControllerHelper.IsControl(context, MyControlsSpace.CUBE_BUILDER_CUBESIZE_MODE))
        return false;
      MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(this.CurrentBlockDefinition.BlockPairName);
      Action blockSizeChanged = this.OnBlockSizeChanged;
      if (blockSizeChanged != null)
        blockSizeChanged();
      if (this.CurrentBlockDefinition.CubeSize == MyCubeSize.Large && definitionGroup.Small != null || this.CurrentBlockDefinition.CubeSize == MyCubeSize.Small && definitionGroup.Large != null)
      {
        MyCubeSize newCubeSize = this.m_cubeBuilderState.CubeSizeMode == MyCubeSize.Large ? MyCubeSize.Small : MyCubeSize.Large;
        this.m_cubeBuilderState.SetCubeSize(newCubeSize);
        this.SetSurvivalIntersectionDist();
        if (newCubeSize == MyCubeSize.Small && this.CubePlacementMode == MyCubeBuilder.CubePlacementModeEnum.LocalCoordinateSystem)
          this.CycleCubePlacementMode();
        int index = this.m_cubeBuilderState.CurrentBlockDefinitionStages.IndexOf(this.CurrentBlockDefinition);
        if (index != -1 && this.m_cubeBuilderState.CurrentBlockDefinitionStages.Count > 0)
          this.UpdateCubeBlockStageDefinition(this.m_cubeBuilderState.CurrentBlockDefinitionStages[index]);
        this.UpdateNotificationBlockNotAvailable(false);
      }
      else
        this.UpdateNotificationBlockNotAvailable(true);
      return true;
    }

    public MyCubeBlockDefinition GetNextBlockVariantDefinition()
    {
      if (this.m_cubeBuilderState.CurrentBlockDefinitionStages.Count <= 0)
        return (MyCubeBlockDefinition) null;
      int num1 = this.m_cubeBuilderState.CurrentBlockDefinitionStages.IndexOf(this.CurrentBlockDefinition);
      int num2 = 0;
      MyCubeBlockDefinition blockDefinitionStage;
      do
      {
        blockDefinitionStage = this.m_cubeBuilderState.CurrentBlockDefinitionStages[++num1 % this.m_cubeBuilderState.CurrentBlockDefinitionStages.Count];
        ++num2;
        if (num2 > this.m_cubeBuilderState.CurrentBlockDefinitionStages.Count)
          return (MyCubeBlockDefinition) null;
      }
      while (!MySession.Static.GetComponent<MySessionComponentDLC>().HasDefinitionDLC((MyDefinitionBase) blockDefinitionStage, Sync.MyId) || !MySessionComponentResearch.Static.CanUse(MySession.Static.LocalPlayerId, blockDefinitionStage.Id) && !MySession.Static.CreativeToolsEnabled(Sync.MyId));
      return blockDefinitionStage;
    }

    private void SetSurvivalIntersectionDist()
    {
      if (this.CurrentBlockDefinition == null || !MySession.Static.SurvivalMode || (MyBlockBuilderBase.SpectatorIsBuilding || MySession.Static.CreativeToolsEnabled(Sync.MyId)))
        return;
      if (this.CurrentBlockDefinition.CubeSize == MyCubeSize.Large)
        MyBlockBuilderBase.IntersectionDistance = (float) MyBlockBuilderBase.CubeBuilderDefinition.BuildingDistLargeSurvivalCharacter;
      else
        MyBlockBuilderBase.IntersectionDistance = (float) MyBlockBuilderBase.CubeBuilderDefinition.BuildingDistSmallSurvivalCharacter;
    }

    private bool HandleRotationInput(MyStringId context, int frameDt)
    {
      if (this.IsActivated)
      {
        for (int index1 = 0; index1 < 6; ++index1)
        {
          if (MyControllerHelper.IsControl(context, MyBlockBuilderBase.m_rotationControls[index1], MyControlStateType.PRESSED))
          {
            if (this.AlignToDefault)
              this.m_customRotation = true;
            bool newlyPressed = MyControllerHelper.IsControl(context, MyBlockBuilderBase.m_rotationControls[index1]);
            int index2 = -1;
            int rotationDirection = MyBlockBuilderBase.m_rotationDirections[index1];
            if (MyFakes.ENABLE_STANDARD_AXES_ROTATION)
            {
              index2 = this.GetStandardRotationAxisAndDirection(index1, ref rotationDirection);
            }
            else
            {
              if (index1 < 2)
              {
                index2 = this.m_rotationHints.RotationUpAxis;
                rotationDirection *= this.m_rotationHints.RotationUpDirection;
              }
              if (index1 >= 2 && index1 < 4)
              {
                index2 = this.m_rotationHints.RotationRightAxis;
                rotationDirection *= this.m_rotationHints.RotationRightDirection;
              }
              if (index1 >= 4)
              {
                index2 = this.m_rotationHints.RotationForwardAxis;
                rotationDirection *= this.m_rotationHints.RotationForwardDirection;
              }
            }
            if (index2 != -1)
            {
              if (this.CurrentBlockDefinition != null && this.CurrentBlockDefinition.Rotation == MyBlockRotation.None)
                return false;
              double angleDelta = (double) frameDt * MyCubeBuilder.BLOCK_ROTATION_SPEED;
              if (MyInput.Static.IsAnyCtrlKeyPressed() || this.m_cubePlacementMode == MyCubeBuilder.CubePlacementModeEnum.GravityAligned)
              {
                if (!newlyPressed)
                  return false;
                angleDelta = Math.PI / 2.0;
              }
              if (MyInput.Static.IsAnyAltKeyPressed())
              {
                if (!newlyPressed)
                  return false;
                angleDelta = MathHelperD.ToRadians(1.0);
              }
              this.RotateAxis(index2, rotationDirection, angleDelta, newlyPressed);
              this.ShowAxis = false;
            }
          }
        }
        if (MyControllerHelper.IsControl(context, MyControlsSpace.CHANGE_ROTATION_AXIS) && this.ToolType != MyCubeBuilderToolType.ColorTool && !this.IsSymmetrySetupMode())
        {
          this.m_selectedAxis = (this.m_selectedAxis + 1) % 3;
          this.ShowAxis = true;
        }
        if (MyControllerHelper.IsControl(context, MyControlsSpace.ROTATE_AXIS_LEFT, MyControlStateType.PRESSED))
        {
          if (this.AlignToDefault)
            this.m_customRotation = true;
          bool newlyPressed = MyControllerHelper.IsControl(context, MyControlsSpace.ROTATE_AXIS_LEFT);
          this.RotateAxis(this.m_selectedAxis, 1, (double) frameDt * MyCubeBuilder.BLOCK_ROTATION_SPEED, newlyPressed);
          this.ShowAxis = true;
        }
        if (MyControllerHelper.IsControl(context, MyControlsSpace.ROTATE_AXIS_RIGHT, MyControlStateType.PRESSED))
        {
          if (this.AlignToDefault)
            this.m_customRotation = true;
          bool newlyPressed = MyControllerHelper.IsControl(context, MyControlsSpace.ROTATE_AXIS_RIGHT);
          this.RotateAxis(this.m_selectedAxis, -1, (double) frameDt * MyCubeBuilder.BLOCK_ROTATION_SPEED, newlyPressed);
          this.ShowAxis = true;
        }
      }
      return false;
    }

    private bool HandleAdminAndCreativeInput(MyStringId context)
    {
      bool flag = MySession.Static.CreativeToolsEnabled(Sync.MyId) && MySession.Static.HasCreativeRights || MySession.Static.CreativeMode;
      if (!flag)
      {
        if (!MyBlockBuilderBase.SpectatorIsBuilding)
          ;
      }
      else
      {
        if (!(MySession.Static.ControlledEntity is MyShipController) && this.HandleBlockCreationMovement(context))
          return true;
        if (this.DynamicMode)
        {
          if (this.IsBuildToolActive() & flag && MyControllerHelper.IsControl(context, MyControlsSpace.PRIMARY_TOOL_ACTION))
            this.Add();
        }
        else if (this.CurrentGrid != null)
          this.HandleCurrentGridInput(context);
        else if (MyControllerHelper.IsControl(context, MyControlsSpace.PRIMARY_TOOL_ACTION))
          this.Add();
      }
      return false;
    }

    private void HandleSurvivalInput(MyStringId context)
    {
      if ((!MySession.Static.CreativeToolsEnabled(Sync.MyId) || !MySession.Static.HasCreativeRights ? (MySession.Static.CreativeMode ? 1 : 0) : 1) != 0 || !this.IsBuildToolActive() || (!MyInput.Static.IsJoystickLastUsed || !MyControllerHelper.IsControl(context, MyControlsSpace.SECONDARY_TOOL_ACTION, MyControlStateType.NEW_RELEASED)))
        return;
      MySession.Static.GetComponent<MyToolSwitcher>().SwitchToGrinder();
    }

    public void ActivateColorTool()
    {
      MySession.Static.LocalCharacter?.SwitchToWeapon(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_CubePlacer)));
      MyCubeBuilder.Static.CubeBuilderState.SetCubeSize(MyCubeSize.Large);
      MyDefinitionId myDefinitionId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_CubeBlock), "LargeBlockArmorBlock");
      MyCubeBlockDefinition blockDefinition;
      MyDefinitionManager.Static.TryGetCubeBlockDefinition(myDefinitionId, out blockDefinition);
      if (!MySession.Static.GetComponent<MySessionComponentDLC>().HasDefinitionDLC((MyDefinitionBase) blockDefinition, Sync.MyId) || !MySessionComponentResearch.Static.CanUse(MySession.Static.LocalPlayerId, myDefinitionId) && !MySession.Static.CreativeToolsEnabled(Sync.MyId))
        return;
      MyCubeBuilder.Static.Activate(new MyDefinitionId?(myDefinitionId));
      MyCubeBuilder.Static.SetToolType(MyCubeBuilderToolType.ColorTool);
      this.ShowAxis = false;
    }

    public void ColorPickerOk()
    {
      if (!MyInput.Static.IsJoystickLastUsed)
        return;
      this.ActivateColorTool();
    }

    public void ColorPickerCancel()
    {
      if (!MyInput.Static.IsJoystickLastUsed || !this.IsOnlyColorToolActive() || (MySession.Static.LocalCharacter == null || !(MySession.Static.ControlledEntity is MyCharacter controlledEntity)))
        return;
      // ISSUE: explicit non-virtual call
      __nonvirtual (controlledEntity.SwitchToWeapon((MyToolbarItemWeapon) null));
    }

    public bool IsSymmetrySetupMode() => this.SymmetrySettingMode != MySymmetrySettingModeEnum.NoPlane && (uint) this.SymmetrySettingMode > 0U;

    public void ToggleSymmetrySetup()
    {
      this.SymmetrySettingMode = this.SymmetrySettingMode != MySymmetrySettingModeEnum.NoPlane ? MySymmetrySettingModeEnum.NoPlane : MySymmetrySettingModeEnum.XPlane;
      this.UseSymmetry = true;
      this.UpdateSymmetryNotification(MyCommonTexts.SettingSymmetryX, MyCommonTexts.SettingSymmetryXGamepad);
      Action setupModeChanged = this.OnSymmetrySetupModeChanged;
      if (setupModeChanged != null)
        setupModeChanged();
      this.ShowCubePlacementNotification();
    }

    private void GamepadControls(MyStringId context)
    {
      if (!MySession.Static.CreativeToolsEnabled(Sync.MyId) && !MySession.Static.CreativeMode)
      {
        if (this.SymmetrySettingMode != MySymmetrySettingModeEnum.NoPlane && this.SymmetrySettingMode != MySymmetrySettingModeEnum.Disabled)
        {
          this.SymmetrySettingMode = MySymmetrySettingModeEnum.NoPlane;
          this.RemoveSymmetryNotification();
          Action setupModeChanged = this.OnSymmetrySetupModeChanged;
          if (setupModeChanged != null)
            setupModeChanged();
        }
      }
      else if ((MyControllerHelper.IsControl(context, MyControlsSpace.SYMMETRY_SWITCH) || MyControllerHelper.IsControl(context, MyControlsSpace.CHANGE_ROTATION_AXIS) && this.IsSymmetrySetupMode()) && !(MySession.Static.ControlledEntity is MyShipController))
      {
        if (this.BlockCreationIsActivated)
          MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
        switch (this.SymmetrySettingMode)
        {
          case MySymmetrySettingModeEnum.NoPlane:
            this.SymmetrySettingMode = MySymmetrySettingModeEnum.XPlane;
            this.UpdateSymmetryNotification(MyCommonTexts.SettingSymmetryX, MyCommonTexts.SettingSymmetryXGamepad);
            Action setupModeChanged1 = this.OnSymmetrySetupModeChanged;
            if (setupModeChanged1 != null)
            {
              setupModeChanged1();
              break;
            }
            break;
          case MySymmetrySettingModeEnum.XPlane:
            this.SymmetrySettingMode = MySymmetrySettingModeEnum.XPlaneOdd;
            this.UpdateSymmetryNotification(MyCommonTexts.SettingSymmetryXOffset, MyCommonTexts.SettingSymmetryXOffsetGamepad);
            Action setupModeChanged2 = this.OnSymmetrySetupModeChanged;
            if (setupModeChanged2 != null)
            {
              setupModeChanged2();
              break;
            }
            break;
          case MySymmetrySettingModeEnum.XPlaneOdd:
            this.SymmetrySettingMode = MySymmetrySettingModeEnum.YPlane;
            this.UpdateSymmetryNotification(MyCommonTexts.SettingSymmetryY, MyCommonTexts.SettingSymmetryYGamepad);
            Action setupModeChanged3 = this.OnSymmetrySetupModeChanged;
            if (setupModeChanged3 != null)
            {
              setupModeChanged3();
              break;
            }
            break;
          case MySymmetrySettingModeEnum.YPlane:
            this.SymmetrySettingMode = MySymmetrySettingModeEnum.YPlaneOdd;
            this.UpdateSymmetryNotification(MyCommonTexts.SettingSymmetryYOffset, MyCommonTexts.SettingSymmetryYOffsetGamepad);
            Action setupModeChanged4 = this.OnSymmetrySetupModeChanged;
            if (setupModeChanged4 != null)
            {
              setupModeChanged4();
              break;
            }
            break;
          case MySymmetrySettingModeEnum.YPlaneOdd:
            this.SymmetrySettingMode = MySymmetrySettingModeEnum.ZPlane;
            this.UpdateSymmetryNotification(MyCommonTexts.SettingSymmetryZ, MyCommonTexts.SettingSymmetryZGamepad);
            Action setupModeChanged5 = this.OnSymmetrySetupModeChanged;
            if (setupModeChanged5 != null)
            {
              setupModeChanged5();
              break;
            }
            break;
          case MySymmetrySettingModeEnum.ZPlane:
            this.SymmetrySettingMode = MySymmetrySettingModeEnum.ZPlaneOdd;
            this.UpdateSymmetryNotification(MyCommonTexts.SettingSymmetryZOffset, MyCommonTexts.SettingSymmetryZOffsetGamepad);
            Action setupModeChanged6 = this.OnSymmetrySetupModeChanged;
            if (setupModeChanged6 != null)
            {
              setupModeChanged6();
              break;
            }
            break;
          case MySymmetrySettingModeEnum.ZPlaneOdd:
            this.SymmetrySettingMode = MySymmetrySettingModeEnum.NoPlane;
            this.RemoveSymmetryNotification();
            Action setupModeChanged7 = this.OnSymmetrySetupModeChanged;
            if (setupModeChanged7 != null)
            {
              setupModeChanged7();
              break;
            }
            break;
        }
        this.UseSymmetry = true;
        this.ShowCubePlacementNotification();
      }
      if (!this.IsOnlyColorToolActive())
        return;
      if (MyControllerHelper.IsControl(context, MyControlsSpace.CYCLE_SKIN_RIGHT))
        this.SwitchToNextSkin();
      if (MyControllerHelper.IsControl(context, MyControlsSpace.CYCLE_SKIN_LEFT))
        this.SwitchToPreviousSkin();
      if (MyControllerHelper.IsControl(context, MyControlsSpace.CYCLE_COLOR_RIGHT))
        this.SwitchToNextColor();
      if (MyControllerHelper.IsControl(context, MyControlsSpace.CYCLE_COLOR_LEFT))
        this.SwitchToPreviousColor();
      if (MyControllerHelper.IsControl(context, MyControlsSpace.SATURATION_INCREASE))
        this.IncreaseSaturation();
      if (MyControllerHelper.IsControl(context, MyControlsSpace.SATURATION_DECREASE))
        this.DecreaseSaturation();
      if (MyControllerHelper.IsControl(context, MyControlsSpace.VALUE_INCREASE))
        this.IncreaseValue();
      if (MyControllerHelper.IsControl(context, MyControlsSpace.VALUE_DECREASE))
        this.DecreaseValue();
      if (!MyControllerHelper.IsControl(context, MyControlsSpace.COPY_COLOR))
        return;
      this.PickColor();
    }

    private bool HandleCurrentGridInput(MyStringId context)
    {
      if (MyControllerHelper.IsControl(context, MyControlsSpace.USE_SYMMETRY) && !MyControllerHelper.IsControl(context, MyControlsSpace.SYMMETRY_SWITCH_MODE, MyControlStateType.PRESSED) && !(MySession.Static.ControlledEntity is MyShipController))
      {
        MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
        if (this.ToggleSymmetry())
          return true;
      }
      if (this.CurrentBlockDefinition == null || !this.BlockCreationIsActivated)
      {
        this.SymmetrySettingMode = MySymmetrySettingModeEnum.NoPlane;
        this.RemoveSymmetryNotification();
      }
      if (this.IsInSymmetrySettingMode && !(MySession.Static.ControlledEntity is MyShipController))
      {
        if (MyControllerHelper.IsControl(context, MyControlsSpace.PRIMARY_TOOL_ACTION) || MyControllerHelper.IsControl(context, MyControlsSpace.SYMMETRY_SETUP_ADD) && this.IsSymmetrySetupMode())
        {
          if (this.m_gizmo.SpaceDefault.m_removeBlock != null)
          {
            Vector3I vector3I = (this.m_gizmo.SpaceDefault.m_removeBlock.Min + this.m_gizmo.SpaceDefault.m_removeBlock.Max) / 2;
            MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
            switch (this.SymmetrySettingMode)
            {
              case MySymmetrySettingModeEnum.XPlane:
                this.CurrentGrid.XSymmetryPlane = new Vector3I?(vector3I);
                this.CurrentGrid.XSymmetryOdd = false;
                break;
              case MySymmetrySettingModeEnum.XPlaneOdd:
                this.CurrentGrid.XSymmetryPlane = new Vector3I?(vector3I);
                this.CurrentGrid.XSymmetryOdd = true;
                break;
              case MySymmetrySettingModeEnum.YPlane:
                this.CurrentGrid.YSymmetryPlane = new Vector3I?(vector3I);
                this.CurrentGrid.YSymmetryOdd = false;
                break;
              case MySymmetrySettingModeEnum.YPlaneOdd:
                this.CurrentGrid.YSymmetryPlane = new Vector3I?(vector3I);
                this.CurrentGrid.YSymmetryOdd = true;
                break;
              case MySymmetrySettingModeEnum.ZPlane:
                this.CurrentGrid.ZSymmetryPlane = new Vector3I?(vector3I);
                this.CurrentGrid.ZSymmetryOdd = false;
                break;
              case MySymmetrySettingModeEnum.ZPlaneOdd:
                this.CurrentGrid.ZSymmetryPlane = new Vector3I?(vector3I);
                this.CurrentGrid.ZSymmetryOdd = true;
                break;
            }
          }
          return true;
        }
        if (MyControllerHelper.IsControl(context, MyControlsSpace.SECONDARY_TOOL_ACTION) || MyControllerHelper.IsControl(context, MyControlsSpace.SYMMETRY_SETUP_REMOVE) && this.IsSymmetrySetupMode())
        {
          MyGuiAudio.PlaySound(MyGuiSounds.HudDeleteBlock);
          switch (this.SymmetrySettingMode)
          {
            case MySymmetrySettingModeEnum.XPlane:
            case MySymmetrySettingModeEnum.XPlaneOdd:
              this.CurrentGrid.XSymmetryPlane = new Vector3I?();
              this.CurrentGrid.XSymmetryOdd = false;
              break;
            case MySymmetrySettingModeEnum.YPlane:
            case MySymmetrySettingModeEnum.YPlaneOdd:
              this.CurrentGrid.YSymmetryPlane = new Vector3I?();
              this.CurrentGrid.YSymmetryOdd = false;
              break;
            case MySymmetrySettingModeEnum.ZPlane:
            case MySymmetrySettingModeEnum.ZPlaneOdd:
              this.CurrentGrid.ZSymmetryPlane = new Vector3I?();
              this.CurrentGrid.ZSymmetryOdd = false;
              break;
          }
          MyControllerHelper.IsControl(context, MyControlsSpace.SECONDARY_TOOL_ACTION);
          return false;
        }
      }
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.SYMMETRY_SETUP_CANCEL) && this.SymmetrySettingMode != MySymmetrySettingModeEnum.NoPlane)
      {
        this.SymmetrySettingMode = MySymmetrySettingModeEnum.NoPlane;
        this.RemoveSymmetryNotification();
        return true;
      }
      if (MyInput.Static.IsNewKeyPressed(MyKeys.Escape))
      {
        if (this.SymmetrySettingMode != MySymmetrySettingModeEnum.NoPlane)
        {
          this.SymmetrySettingMode = MySymmetrySettingModeEnum.NoPlane;
          this.RemoveSymmetryNotification();
          return true;
        }
        if (this.CancelBuilding())
          return true;
      }
      if (this.IsBuildToolActive())
      {
        if (MyControllerHelper.IsControl(context, MyControlsSpace.PRIMARY_TOOL_ACTION))
        {
          if (!this.PlacingSmallGridOnLargeStatic && (MyInput.Static.IsAnyCtrlKeyPressed() || MyCubeBuilder.BuildingMode != MyCubeBuilder.BuildingModeEnum.SingleBlock))
            this.StartBuilding();
          else
            this.Add();
        }
      }
      else if (this.IsOnlyColorToolActive() && MyControllerHelper.IsControl(context, MyControlsSpace.PRIMARY_TOOL_ACTION, MyControlStateType.PRESSED))
        this.RecolorControlsKeyboard();
      if (MyControllerHelper.IsControl(context, MyControlsSpace.SECONDARY_TOOL_ACTION))
      {
        if (this.IsBuildToolActive())
        {
          if (MyInput.Static.IsAnyCtrlKeyPressed() || MyCubeBuilder.BuildingMode != MyCubeBuilder.BuildingModeEnum.SingleBlock)
          {
            this.StartRemoving();
          }
          else
          {
            if (MyFakes.ENABLE_COMPOUND_BLOCKS && !this.CompoundEnabled)
            {
              foreach (MyCubeBuilderGizmo.MyGizmoSpaceProperties space in this.m_gizmo.Spaces)
              {
                if (space.Enabled)
                  space.m_blockIdInCompound = new ushort?();
              }
            }
            this.PrepareBlocksToRemove();
            this.Remove();
          }
        }
        else
          this.PickColor();
      }
      if (this.IsBuildToolActive())
      {
        if (MyInput.Static.IsLeftMousePressed() || MyInput.Static.IsRightMousePressed() || (MyControllerHelper.IsControl(context, MyControlsSpace.PRIMARY_TOOL_ACTION, MyControlStateType.PRESSED) || MyControllerHelper.IsControl(context, MyControlsSpace.SECONDARY_TOOL_ACTION, MyControlStateType.PRESSED)))
          this.ContinueBuilding(MyInput.Static.IsAnyShiftKeyPressed() || MyCubeBuilder.BuildingMode == MyCubeBuilder.BuildingModeEnum.Plane);
        if (MyInput.Static.IsNewLeftMouseReleased() || MyInput.Static.IsNewRightMouseReleased() || (MyControllerHelper.IsControl(context, MyControlsSpace.PRIMARY_TOOL_ACTION, MyControlStateType.NEW_RELEASED) || MyControllerHelper.IsControl(context, MyControlsSpace.SECONDARY_TOOL_ACTION, MyControlStateType.NEW_RELEASED)))
          this.StopBuilding();
      }
      return false;
    }

    private void DecreaseValue()
    {
    }

    private void IncreaseValue()
    {
    }

    private void DecreaseSaturation()
    {
    }

    private void IncreaseSaturation()
    {
    }

    private void SwitchToPreviousColor()
    {
      MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
      if (!this.IsActivated || this.CurrentBlockDefinition != null && !MyFakes.ENABLE_BLOCK_COLORING || localHumanPlayer == null)
        return;
      MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
      localHumanPlayer.SelectedBuildColorSlot = (localHumanPlayer.SelectedBuildColorSlot + localHumanPlayer.BuildColorSlots.Count - 1) % localHumanPlayer.BuildColorSlots.Count;
    }

    private void SwitchToNextColor()
    {
      MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
      if (!this.IsActivated || this.CurrentBlockDefinition != null && !MyFakes.ENABLE_BLOCK_COLORING || localHumanPlayer == null)
        return;
      MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
      localHumanPlayer.SelectedBuildColorSlot = (localHumanPlayer.SelectedBuildColorSlot + 1) % localHumanPlayer.BuildColorSlots.Count;
    }

    private void SwitchToPreviousSkin() => this.SwitchToSkin(false);

    private void SwitchToNextSkin() => this.SwitchToSkin();

    private void SwitchToSkin(bool next = true)
    {
      List<string> availableSkins = this.GetAvailableSkins();
      string buildArmorSkin = MySession.Static.LocalHumanPlayer.BuildArmorSkin;
      string empty = string.Empty;
      int num1 = -1;
      for (int index = 0; index < availableSkins.Count; ++index)
      {
        if (availableSkins[index] == buildArmorSkin)
        {
          num1 = index;
          break;
        }
      }
      int num2 = availableSkins.Count + 1;
      int num3 = next ? 1 : num2 - 1;
      int index1 = (num1 + 1 + num3) % num2 - 1;
      if (index1 == -1)
        MySession.Static.LocalHumanPlayer.BuildArmorSkin = (string) null;
      else
        MySession.Static.LocalHumanPlayer.BuildArmorSkin = availableSkins[index1];
    }

    private List<string> GetAvailableSkins()
    {
      List<string> stringList = new List<string>();
      HashSet<string> stringSet = new HashSet<string>();
      foreach (MyGameInventoryItem inventoryItem in (IEnumerable<MyGameInventoryItem>) MyGameService.InventoryItems)
      {
        if (inventoryItem.ItemDefinition.ItemSlot == MyGameInventoryItemSlot.Armor)
        {
          MyAssetModifierDefinition modifierDefinition = MyDefinitionManager.Static.GetAssetModifierDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_AssetModifierDefinition), inventoryItem.ItemDefinition.AssetModifierId));
          if (modifierDefinition != null && !stringSet.Contains(inventoryItem.ItemDefinition.AssetModifierId) && this.CheckArmorSkin(inventoryItem.ItemDefinition, modifierDefinition))
          {
            stringList.Add(inventoryItem.ItemDefinition.AssetModifierId);
            stringSet.Add(inventoryItem.ItemDefinition.AssetModifierId);
          }
        }
      }
      return stringList;
    }

    private bool CheckArmorSkin(
      MyGameInventoryItemDefinition item,
      MyAssetModifierDefinition definition)
    {
      return true;
    }

    public bool ToggleSymmetry()
    {
      if (this.SymmetrySettingMode != MySymmetrySettingModeEnum.NoPlane)
      {
        this.UseSymmetry = false;
        this.SymmetrySettingMode = MySymmetrySettingModeEnum.NoPlane;
        this.RemoveSymmetryNotification();
        return true;
      }
      this.UseSymmetry = !this.UseSymmetry;
      return false;
    }

    private bool HandleExportInput()
    {
      if (!MyInput.Static.IsNewKeyPressed(MyKeys.E) || !MyInput.Static.IsAnyAltKeyPressed() || (!MyInput.Static.IsAnyCtrlKeyPressed() || MyInput.Static.IsAnyShiftKeyPressed()) || (MyInput.Static.IsAnyMousePressed() || !MyPerGameSettings.EnableObjectExport))
        return false;
      MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
      MyCubeGrid targetGrid = MyCubeGrid.GetTargetGrid();
      if (targetGrid != null)
        MyCubeGrid.ExportObject(targetGrid, false, true);
      return true;
    }

    private bool HandleBlockCreationMovement(MyStringId context)
    {
      bool flag = MyInput.Static.IsAnyCtrlKeyPressed();
      if (flag && MyInput.Static.PreviousMouseScrollWheelValue() < MyInput.Static.MouseScrollWheelValue() || MyControllerHelper.IsControl(context, MyControlsSpace.MOVE_FURTHER, MyControlStateType.PRESSED))
      {
        float intersectionDistance = MyBlockBuilderBase.IntersectionDistance;
        MyBlockBuilderBase.IntersectionDistance *= 1.1f;
        if ((double) MyBlockBuilderBase.IntersectionDistance > (double) MyBlockBuilderBase.CubeBuilderDefinition.MaxBlockBuildingDistance)
          MyBlockBuilderBase.IntersectionDistance = MyBlockBuilderBase.CubeBuilderDefinition.MaxBlockBuildingDistance;
        if (MySession.Static.SurvivalMode && !MyBlockBuilderBase.SpectatorIsBuilding && this.CurrentBlockDefinition != null)
        {
          float cubeSize = MyDefinitionManager.Static.GetCubeSize(this.CurrentBlockDefinition.CubeSize);
          BoundingBoxD gizmoBox = new BoundingBoxD((Vector3D) (-this.CurrentBlockDefinition.Size * cubeSize * 0.5f), (Vector3D) (this.CurrentBlockDefinition.Size * cubeSize * 0.5f));
          MatrixD worldMatrixAdd = this.m_gizmo.SpaceDefault.m_worldMatrixAdd;
          worldMatrixAdd.Translation = this.FreePlacementTarget;
          MatrixD invGridWorldMatrix = MatrixD.Invert(worldMatrixAdd);
          if (!MyCubeBuilderGizmo.DefaultGizmoCloseEnough(ref invGridWorldMatrix, gizmoBox, cubeSize, MyBlockBuilderBase.IntersectionDistance))
            MyBlockBuilderBase.IntersectionDistance = intersectionDistance;
        }
        return true;
      }
      if ((!flag || MyInput.Static.PreviousMouseScrollWheelValue() <= MyInput.Static.MouseScrollWheelValue()) && !MyControllerHelper.IsControl(context, MyControlsSpace.MOVE_CLOSER, MyControlStateType.PRESSED))
        return false;
      MyBlockBuilderBase.IntersectionDistance /= 1.1f;
      if ((double) MyBlockBuilderBase.IntersectionDistance < (double) MyBlockBuilderBase.CubeBuilderDefinition.MinBlockBuildingDistance)
        MyBlockBuilderBase.IntersectionDistance = MyBlockBuilderBase.CubeBuilderDefinition.MinBlockBuildingDistance;
      return true;
    }

    private int GetStandardRotationAxisAndDirection(int index, ref int direction)
    {
      int num1 = -1;
      MatrixD matrix = MatrixD.Transpose((MatrixD) ref this.m_gizmo.SpaceDefault.m_localMatrixAdd);
      Vector3I vector1_1 = Vector3I.Round(Vector3D.TransformNormal(Vector3D.Up, matrix));
      if (MyInput.Static.IsAnyShiftKeyPressed())
        direction *= -1;
      if (this.CubePlacementMode == MyCubeBuilder.CubePlacementModeEnum.FreePlacement)
        return new int[6]{ 1, 1, 0, 0, 2, 2 }[index];
      Vector3I? mountPointNormal = this.GetSingleMountPointNormal();
      if (mountPointNormal.HasValue)
      {
        Vector3I vector1_2 = mountPointNormal.Value;
        int num2 = Vector3I.Dot(ref vector1_2, ref Vector3I.Up);
        int num3 = Vector3I.Dot(ref vector1_2, ref Vector3I.Right);
        int num4 = Vector3I.Dot(ref vector1_2, ref Vector3I.Forward);
        if (num2 == 1 || num2 == -1)
        {
          num1 = 1;
          direction *= num2;
        }
        else if (num3 == 1 || num3 == -1)
        {
          num1 = 0;
          direction *= num3;
        }
        else if (num4 == 1 || num4 == -1)
        {
          num1 = 2;
          direction *= num4;
        }
      }
      else if (index < 2)
      {
        int num2 = Vector3I.Dot(ref vector1_1, ref Vector3I.Up);
        int num3 = Vector3I.Dot(ref vector1_1, ref Vector3I.Right);
        int num4 = Vector3I.Dot(ref vector1_1, ref Vector3I.Forward);
        if (num2 == 1 || num2 == -1)
        {
          num1 = 1;
          direction *= num2;
        }
        else if (num3 == 1 || num3 == -1)
        {
          num1 = 0;
          direction *= num3;
        }
        else if (num4 == 1 || num4 == -1)
        {
          num1 = 2;
          direction *= num4;
        }
      }
      else if (index >= 2 && index < 4)
      {
        Vector3I vector1_2 = Vector3I.Round(this.m_gizmo.SpaceDefault.m_localMatrixAdd.Forward);
        if (Vector3I.Dot(ref vector1_2, ref Vector3I.Up) == 0)
        {
          Vector3I result;
          Vector3I.Cross(ref vector1_2, ref Vector3I.Up, out result);
          Vector3I vector1_3 = Vector3I.Round(Vector3D.TransformNormal((Vector3) result, matrix));
          int num2 = Vector3I.Dot(ref vector1_3, ref Vector3I.Up);
          int num3 = Vector3I.Dot(ref vector1_3, ref Vector3I.Right);
          int num4 = Vector3I.Dot(ref vector1_3, ref Vector3I.Forward);
          if (num2 == 1 || num2 == -1)
          {
            num1 = 1;
            direction *= num2;
          }
          else if (num3 == 1 || num3 == -1)
            num1 = 0;
          else if (num4 == 1 || num4 == -1)
          {
            num1 = 2;
            direction *= num4;
          }
        }
        else
          num1 = 0;
      }
      else if (index >= 4)
        num1 = 2;
      return num1;
    }

    public void InputLost() => this.m_gizmo.Clear();

    private void UpdateSymmetryNotification(
      MyStringId myTextsWrapperEnum,
      MyStringId myTextsWrapperEnumGamepad)
    {
      this.RemoveSymmetryNotification();
      if (!MyInput.Static.IsJoystickConnected() || !MyInput.Static.IsJoystickLastUsed)
      {
        this.m_symmetryNotification = new MyHudNotification(myTextsWrapperEnum, 0, level: MyNotificationLevel.Control);
        this.m_symmetryNotification.SetTextFormatArguments((object) MyInput.Static.GetGameControl(MyControlsSpace.PRIMARY_TOOL_ACTION), (object) MyInput.Static.GetGameControl(MyControlsSpace.SECONDARY_TOOL_ACTION));
      }
      else
      {
        IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
        MyStringId context = controlledEntity != null ? controlledEntity.AuxiliaryContext : MyStringId.NullOrEmpty;
        this.m_symmetryNotification = new MyHudNotification(myTextsWrapperEnumGamepad, 0, level: MyNotificationLevel.Control);
        this.m_symmetryNotification.SetTextFormatArguments((object) MyControllerHelper.GetCodeForControl(context, MyControlsSpace.PRIMARY_TOOL_ACTION), (object) MyControllerHelper.GetCodeForControl(context, MyControlsSpace.SECONDARY_TOOL_ACTION), (object) MyControllerHelper.GetCodeForControl(context, MyControlsSpace.SYMMETRY_SWITCH));
      }
      MyHud.Notifications.Add((MyHudNotificationBase) this.m_symmetryNotification);
    }

    private void RemoveSymmetryNotification()
    {
      if (this.m_symmetryNotification == null)
        return;
      MyHud.Notifications.Remove((MyHudNotificationBase) this.m_symmetryNotification);
      this.m_symmetryNotification = (MyHudNotification) null;
    }

    public static void PrepareCharacterCollisionPoints(List<Vector3D> outList)
    {
      if (!(MySession.Static.ControlledEntity is MyCharacter controlledEntity))
        return;
      float num1 = controlledEntity.Definition.CharacterCollisionHeight * 0.7f;
      float num2 = controlledEntity.Definition.CharacterCollisionWidth * 0.2f;
      if (controlledEntity == null)
        return;
      if (controlledEntity.IsCrouching)
        num1 = controlledEntity.Definition.CharacterCollisionCrouchHeight;
      Matrix matrix = controlledEntity.PositionComp.LocalMatrixRef;
      Vector3 vector3_1 = matrix.Up * num1;
      matrix = controlledEntity.PositionComp.LocalMatrixRef;
      Vector3 vector3_2 = matrix.Forward * num2;
      matrix = controlledEntity.PositionComp.LocalMatrixRef;
      Vector3 vector3_3 = matrix.Right * num2;
      Vector3D position = controlledEntity.Entity.PositionComp.GetPosition();
      matrix = controlledEntity.PositionComp.LocalMatrixRef;
      Vector3 vector3_4 = matrix.Up * 0.2f;
      Vector3D vector3D1 = position + vector3_4;
      float num3 = 0.0f;
      for (int index = 0; index < 6; ++index)
      {
        float num4 = (float) Math.Sin((double) num3);
        float num5 = (float) Math.Cos((double) num3);
        Vector3D vector3D2 = vector3D1 + num4 * vector3_3 + num5 * vector3_2;
        outList.Add(vector3D2);
        outList.Add(vector3D2 + vector3_1);
        num3 += 1.047198f;
      }
    }

    protected virtual void UpdateGizmo(
      MyCubeBuilderGizmo.MyGizmoSpaceProperties gizmoSpace,
      bool add,
      bool remove,
      bool draw)
    {
      if (!gizmoSpace.Enabled)
        return;
      if (!MyCubeBuilder.Static.canBuild)
      {
        gizmoSpace.m_showGizmoCube = false;
        gizmoSpace.m_buildAllowed = false;
      }
      if (this.DynamicMode)
        this.UpdateGizmo_DynamicMode(gizmoSpace);
      else if (this.CurrentGrid != null)
        this.UpdateGizmo_Grid(gizmoSpace, add, remove, draw);
      else
        this.UpdateGizmo_VoxelMap(gizmoSpace, add, remove, draw);
    }

    private void UpdateGizmo_DynamicMode(
      MyCubeBuilderGizmo.MyGizmoSpaceProperties gizmoSpace)
    {
      gizmoSpace.m_animationProgress = 1f;
      float cubeSize = MyDefinitionManager.Static.GetCubeSize(this.CurrentBlockDefinition.CubeSize);
      BoundingBoxD localbox = new BoundingBoxD((Vector3D) (-this.CurrentBlockDefinition.Size * cubeSize * 0.5f), (Vector3D) (this.CurrentBlockDefinition.Size * cubeSize * 0.5f));
      MyGridPlacementSettings settings = this.CurrentBlockDefinition.CubeSize == MyCubeSize.Large ? MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.LargeGrid : MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.SmallGrid;
      MatrixD worldMatrixAdd = gizmoSpace.m_worldMatrixAdd;
      MyCubeGrid.GetCubeParts(this.CurrentBlockDefinition, Vector3I.Zero, Matrix.Identity, cubeSize, gizmoSpace.m_cubeModelsTemp, gizmoSpace.m_cubeMatricesTemp, gizmoSpace.m_cubeNormals, gizmoSpace.m_patternOffsets, true);
      if (gizmoSpace.m_showGizmoCube)
      {
        this.m_gizmo.AddFastBuildParts(gizmoSpace, this.CurrentBlockDefinition, (MyCubeGrid) null);
        this.m_gizmo.UpdateGizmoCubeParts(gizmoSpace, this.m_renderData, ref MatrixD.Identity, this.CurrentBlockDefinition);
      }
      MyCubeBuilder.BuildComponent.GetGridSpawnMaterials(this.CurrentBlockDefinition, worldMatrixAdd, false);
      if (!MySession.Static.CreativeToolsEnabled(Sync.MyId))
        gizmoSpace.m_buildAllowed &= MyCubeBuilder.BuildComponent.HasBuildingMaterials((MyEntity) MySession.Static.LocalCharacter);
      MatrixD matrixD = MatrixD.Invert(worldMatrixAdd);
      if (MySession.Static.SurvivalMode && !MyBlockBuilderBase.SpectatorIsBuilding && !MySession.Static.CreativeToolsEnabled(Sync.MyId))
      {
        if (!MyCubeBuilderGizmo.DefaultGizmoCloseEnough(ref matrixD, localbox, cubeSize, MyBlockBuilderBase.IntersectionDistance) || MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Spectator)
        {
          gizmoSpace.m_buildAllowed = false;
          gizmoSpace.m_removeBlock = (MySlimBlock) null;
        }
        if (MyBlockBuilderBase.CameraControllerSpectator)
        {
          gizmoSpace.m_showGizmoCube = false;
          gizmoSpace.m_buildAllowed = false;
          return;
        }
      }
      if (!gizmoSpace.m_dynamicBuildAllowed)
      {
        bool flag = MyCubeGrid.TestBlockPlacementArea(this.CurrentBlockDefinition, new MyBlockOrientation?(), worldMatrixAdd, ref settings, localbox, this.DynamicMode);
        gizmoSpace.m_buildAllowed &= flag;
      }
      gizmoSpace.m_showGizmoCube = this.IsBuildToolActive();
      gizmoSpace.m_cubeMatricesTemp.Clear();
      gizmoSpace.m_cubeModelsTemp.Clear();
      bool draw = (double) MyHud.Stats.GetStat(MyStringHash.GetOrCompute("hud_mode")).CurrentValue == 1.0 && !MyHud.CutsceneHud && (MySandboxGame.Config.RotationHints && MyFakes.ENABLE_ROTATION_HINTS) && !MyInput.Static.IsJoystickLastUsed;
      this.m_rotationHints.CalculateRotationHints(worldMatrixAdd, draw);
      if (!this.CurrentBlockDefinition.IsStandAlone)
        gizmoSpace.m_buildAllowed = false;
      gizmoSpace.m_buildAllowed &= !this.IntersectsCharacterOrCamera(gizmoSpace, cubeSize, ref matrixD);
      if (!MySessionComponentSafeZones.IsActionAllowed(localbox.TransformFast(ref worldMatrixAdd), MySafeZoneAction.Building, user: Sync.MyId))
      {
        gizmoSpace.m_buildAllowed = false;
        gizmoSpace.m_removeBlock = (MySlimBlock) null;
      }
      if (!gizmoSpace.m_showGizmoCube)
        return;
      Color white = Color.White;
      MyStringId myStringId = gizmoSpace.m_buildAllowed ? MyCubeBuilder.ID_GIZMO_DRAW_LINE : MyCubeBuilder.ID_GIZMO_DRAW_LINE_RED;
      if (gizmoSpace.SymmetryPlane == MySymmetrySettingModeEnum.Disabled)
        MySimpleObjectDraw.DrawTransparentBox(ref worldMatrixAdd, ref localbox, ref white, MySimpleObjectRasterizer.Wireframe, 1, 0.04f, lineMaterial: new MyStringId?(myStringId), blendType: MyBillboard.BlendTypeEnum.LDR);
      this.AddFastBuildModels(gizmoSpace, MatrixD.Identity, gizmoSpace.m_cubeMatricesTemp, gizmoSpace.m_cubeModelsTemp, gizmoSpace.m_blockDefinition);
      for (int index = 0; index < gizmoSpace.m_cubeMatricesTemp.Count; ++index)
      {
        string assetName = gizmoSpace.m_cubeModelsTemp[index];
        if (!string.IsNullOrEmpty(assetName))
          this.m_renderData.AddInstance(MyModel.GetId(assetName), gizmoSpace.m_cubeMatricesTemp[index], ref MatrixD.Identity, MyPlayer.SelectedColor, new MyStringHash?(MyStringHash.GetOrCompute(MyPlayer.SelectedArmorSkin)));
      }
    }

    public bool IsBuildToolActive() => this.ToolType == MyCubeBuilderToolType.BuildTool || this.ToolType == MyCubeBuilderToolType.Combined;

    public bool IsOnlyColorToolActive() => this.ToolType == MyCubeBuilderToolType.ColorTool;

    private void UpdateGizmo_VoxelMap(
      MyCubeBuilderGizmo.MyGizmoSpaceProperties gizmoSpace,
      bool add,
      bool remove,
      bool draw)
    {
      if (!this.m_animationLock)
        gizmoSpace.m_animationLastMatrix = (MatrixD) ref gizmoSpace.m_localMatrixAdd;
      MatrixD matrixD1 = (MatrixD) ref gizmoSpace.m_localMatrixAdd;
      if ((double) gizmoSpace.m_animationProgress < 1.0)
        matrixD1 = MatrixD.Slerp(gizmoSpace.m_animationLastMatrix, (MatrixD) ref gizmoSpace.m_localMatrixAdd, gizmoSpace.m_animationProgress);
      else if ((double) gizmoSpace.m_animationProgress >= 1.0)
      {
        this.m_animationLock = false;
        gizmoSpace.m_animationLastMatrix = (MatrixD) ref gizmoSpace.m_localMatrixAdd;
      }
      Color color = new Color(Color.Green * 0.6f, 1f);
      float cubeSize = MyDefinitionManager.Static.GetCubeSize(this.CurrentBlockDefinition.CubeSize);
      Vector3D zero = Vector3D.Zero;
      MatrixD worldMatrixAdd = gizmoSpace.m_worldMatrixAdd;
      MatrixD orientation = matrixD1.GetOrientation();
      gizmoSpace.m_showGizmoCube = !this.IntersectsCharacterOrCamera(gizmoSpace, cubeSize, ref MatrixD.Identity);
      int num = 0;
      Vector3 vector3;
      for (vector3.X = 0.0f; (double) vector3.X < (double) this.CurrentBlockDefinition.Size.X; ++vector3.X)
      {
        for (vector3.Y = 0.0f; (double) vector3.Y < (double) this.CurrentBlockDefinition.Size.Y; ++vector3.Y)
        {
          for (vector3.Z = 0.0f; (double) vector3.Z < (double) this.CurrentBlockDefinition.Size.Z; ++vector3.Z)
          {
            Vector3I position1 = gizmoSpace.m_positions[num++];
            Vector3D position2 = (Vector3D) (position1 * cubeSize);
            if (!MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.StaticGridAlignToCenter)
              position2 += new Vector3D(0.5 * (double) cubeSize, 0.5 * (double) cubeSize, -0.5 * (double) cubeSize);
            Vector3D vector3D = Vector3D.Transform(position2, gizmoSpace.m_worldMatrixAdd);
            zero += position2;
            MyCubeGrid.GetCubeParts(this.CurrentBlockDefinition, position1, (Matrix) ref orientation, cubeSize, gizmoSpace.m_cubeModelsTemp, gizmoSpace.m_cubeMatricesTemp, gizmoSpace.m_cubeNormals, gizmoSpace.m_patternOffsets, false);
            if (gizmoSpace.m_showGizmoCube)
            {
              for (int index = 0; index < gizmoSpace.m_cubeMatricesTemp.Count; ++index)
              {
                MatrixD matrixD2 = gizmoSpace.m_cubeMatricesTemp[index] * gizmoSpace.m_worldMatrixAdd;
                matrixD2.Translation = vector3D;
                gizmoSpace.m_cubeMatricesTemp[index] = matrixD2;
              }
              worldMatrixAdd.Translation = vector3D;
              MatrixD invGridWorldMatrix = MatrixD.Invert(orientation * worldMatrixAdd);
              this.m_gizmo.AddFastBuildParts(gizmoSpace, this.CurrentBlockDefinition, (MyCubeGrid) null);
              this.m_gizmo.UpdateGizmoCubeParts(gizmoSpace, this.m_renderData, ref invGridWorldMatrix, this.CurrentBlockDefinition);
            }
          }
        }
      }
      Vector3D position = zero / (double) this.CurrentBlockDefinition.Size.Size;
      if (!this.m_animationLock)
      {
        gizmoSpace.m_animationProgress = 0.0f;
        gizmoSpace.m_animationLastPosition = position;
      }
      else if ((double) gizmoSpace.m_animationProgress < 1.0)
        position = Vector3D.Lerp(gizmoSpace.m_animationLastPosition, position, (double) gizmoSpace.m_animationProgress);
      Vector3D vector3D1 = Vector3D.Transform(position, gizmoSpace.m_worldMatrixAdd);
      worldMatrixAdd.Translation = vector3D1;
      MatrixD matrixD3 = orientation * worldMatrixAdd;
      BoundingBoxD localbox = new BoundingBoxD((Vector3D) (-this.CurrentBlockDefinition.Size * cubeSize * 0.5f), (Vector3D) (this.CurrentBlockDefinition.Size * cubeSize * 0.5f));
      MyGridPlacementSettings settings = this.CurrentBlockDefinition.CubeSize == MyCubeSize.Large ? MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.LargeStaticGrid : MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.SmallStaticGrid;
      MyBlockOrientation blockOrientation = new MyBlockOrientation(ref Quaternion.Identity);
      bool flag = MyCubeBuilder.CheckValidBlockRotation(gizmoSpace.m_localMatrixAdd, this.CurrentBlockDefinition.Direction, this.CurrentBlockDefinition.Rotation) && MyCubeGrid.TestBlockPlacementArea(this.CurrentBlockDefinition, new MyBlockOrientation?(blockOrientation), matrixD3, ref settings, localbox, false);
      gizmoSpace.m_buildAllowed &= flag;
      gizmoSpace.m_buildAllowed &= gizmoSpace.m_showGizmoCube;
      gizmoSpace.m_worldMatrixAdd = matrixD3;
      MyCubeBuilder.BuildComponent.GetGridSpawnMaterials(this.CurrentBlockDefinition, matrixD3, true);
      if (!MySession.Static.CreativeToolsEnabled(Sync.MyId))
        gizmoSpace.m_buildAllowed &= MyCubeBuilder.BuildComponent.HasBuildingMaterials((MyEntity) MySession.Static.LocalCharacter);
      if (MySession.Static.SurvivalMode && !MyBlockBuilderBase.SpectatorIsBuilding && !MySession.Static.CreativeToolsEnabled(Sync.MyId))
      {
        BoundingBoxD gizmoBox = localbox.TransformFast(ref matrixD3);
        if (!MyCubeBuilderGizmo.DefaultGizmoCloseEnough(ref MatrixD.Identity, gizmoBox, cubeSize, MyBlockBuilderBase.IntersectionDistance) || MyBlockBuilderBase.CameraControllerSpectator)
        {
          gizmoSpace.m_buildAllowed = false;
          gizmoSpace.m_showGizmoCube = false;
          gizmoSpace.m_removeBlock = (MySlimBlock) null;
          return;
        }
      }
      Color white = Color.White;
      MyStringId myStringId = gizmoSpace.m_buildAllowed ? MyCubeBuilder.ID_GIZMO_DRAW_LINE : MyCubeBuilder.ID_GIZMO_DRAW_LINE_RED;
      if (gizmoSpace.SymmetryPlane == MySymmetrySettingModeEnum.Disabled)
      {
        MySimpleObjectDraw.DrawTransparentBox(ref matrixD3, ref localbox, ref white, MySimpleObjectRasterizer.Wireframe, 1, 0.04f, lineMaterial: new MyStringId?(myStringId), blendType: MyBillboard.BlendTypeEnum.LDR);
        bool draw1 = ((MyHud.MinimalHud || MyHud.CutsceneHud ? 0 : (MySandboxGame.Config.RotationHints ? 1 : 0)) & (draw ? 1 : 0)) != 0 && MyFakes.ENABLE_ROTATION_HINTS && !MyInput.Static.IsJoystickLastUsed;
        this.m_rotationHints.CalculateRotationHints(matrixD3, draw1);
      }
      gizmoSpace.m_cubeMatricesTemp.Clear();
      gizmoSpace.m_cubeModelsTemp.Clear();
      if (gizmoSpace.m_showGizmoCube)
      {
        if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS)
          MyCubeBuilder.DrawMountPoints(cubeSize, this.CurrentBlockDefinition, ref matrixD3);
        this.AddFastBuildModels(gizmoSpace, MatrixD.Identity, gizmoSpace.m_cubeMatricesTemp, gizmoSpace.m_cubeModelsTemp, gizmoSpace.m_blockDefinition);
        for (int index = 0; index < gizmoSpace.m_cubeMatricesTemp.Count; ++index)
        {
          string assetName = gizmoSpace.m_cubeModelsTemp[index];
          if (!string.IsNullOrEmpty(assetName))
            this.m_renderData.AddInstance(MyModel.GetId(assetName), gizmoSpace.m_cubeMatricesTemp[index], ref MatrixD.Identity, MyPlayer.SelectedColor, new MyStringHash?(MyStringHash.GetOrCompute(MyPlayer.SelectedArmorSkin)));
        }
      }
      gizmoSpace.m_animationProgress += this.m_animationSpeed;
    }

    private void UpdateGizmo_Grid(
      MyCubeBuilderGizmo.MyGizmoSpaceProperties gizmoSpace,
      bool add,
      bool remove,
      bool draw)
    {
      Color color1 = new Color(Color.Green * 0.6f, 1f);
      Color color2 = new Color(Color.Red * 0.8f, 1f);
      Color yellow = Color.Yellow;
      Color black = Color.Black;
      Color gray = Color.Gray;
      Color white1 = Color.White;
      Color cornflowerBlue = Color.CornflowerBlue;
      if (add)
      {
        if (!this.m_animationLock)
          gizmoSpace.m_animationLastMatrix = (MatrixD) ref gizmoSpace.m_localMatrixAdd;
        MatrixD matrixD1 = (MatrixD) ref gizmoSpace.m_localMatrixAdd;
        if ((double) gizmoSpace.m_animationProgress < 1.0)
          matrixD1 = MatrixD.Slerp(gizmoSpace.m_animationLastMatrix, (MatrixD) ref gizmoSpace.m_localMatrixAdd, gizmoSpace.m_animationProgress);
        else if ((double) gizmoSpace.m_animationProgress >= 1.0)
        {
          this.m_animationLock = false;
          gizmoSpace.m_animationLastMatrix = (MatrixD) ref gizmoSpace.m_localMatrixAdd;
        }
        MatrixD worldMatrix = matrixD1 * this.CurrentGrid.WorldMatrix;
        if (gizmoSpace.m_startBuild.HasValue && gizmoSpace.m_continueBuild.HasValue)
          gizmoSpace.m_buildAllowed = true;
        if (this.PlacingSmallGridOnLargeStatic && gizmoSpace.m_positionsSmallOnLarge.Count == 0)
          return;
        if (this.CurrentBlockDefinition != null)
        {
          Matrix orientation1 = gizmoSpace.m_localMatrixAdd.GetOrientation();
          MyBlockOrientation orientation2 = new MyBlockOrientation(ref orientation1);
          if (!this.PlacingSmallGridOnLargeStatic)
          {
            bool flag1 = MyCubeBuilder.CheckValidBlockRotation(gizmoSpace.m_localMatrixAdd, this.CurrentBlockDefinition.Direction, this.CurrentBlockDefinition.Rotation);
            bool flag2 = this.CurrentGrid.CanPlaceBlock(gizmoSpace.m_min, gizmoSpace.m_max, orientation2, gizmoSpace.m_blockDefinition, Sync.MyId);
            gizmoSpace.m_buildAllowed &= flag1 & flag2;
          }
          MyCubeBuilder.BuildComponent.GetBlockPlacementMaterials(gizmoSpace.m_blockDefinition, gizmoSpace.m_addPos, orientation2, this.CurrentGrid);
          if (!MySession.Static.CreativeToolsEnabled(Sync.MyId))
            gizmoSpace.m_buildAllowed &= MyCubeBuilder.BuildComponent.HasBuildingMaterials((MyEntity) MySession.Static.LocalCharacter);
          if (!this.PlacingSmallGridOnLargeStatic && MySession.Static.SurvivalMode && (!MySession.Static.CreativeToolsEnabled(Sync.MyId) && !MyBlockBuilderBase.SpectatorIsBuilding) && (!MyCubeBuilderGizmo.DefaultGizmoCloseEnough(ref this.m_invGridWorldMatrix, new BoundingBoxD((Vector3D) (((Vector3) this.m_gizmo.SpaceDefault.m_min - new Vector3(0.5f)) * this.CurrentGrid.GridSize), (Vector3D) (((Vector3) this.m_gizmo.SpaceDefault.m_max + new Vector3(0.5f)) * this.CurrentGrid.GridSize)), this.CurrentGrid.GridSize, MyBlockBuilderBase.IntersectionDistance) || MyBlockBuilderBase.CameraControllerSpectator))
          {
            gizmoSpace.m_buildAllowed = false;
            gizmoSpace.m_removeBlock = (MySlimBlock) null;
            return;
          }
          if (gizmoSpace.m_buildAllowed)
          {
            Quaternion.CreateFromRotationMatrix(ref gizmoSpace.m_localMatrixAdd, out gizmoSpace.m_rotation);
            if (gizmoSpace.SymmetryPlane == MySymmetrySettingModeEnum.Disabled && !this.PlacingSmallGridOnLargeStatic)
            {
              MyCubeBlockDefinition.MountPoint[] modelMountPoints = this.CurrentBlockDefinition.GetBuildProgressModelMountPoints(MyComponentStack.NewBlockIntegrity);
              gizmoSpace.m_buildAllowed = MyCubeGrid.CheckConnectivity((IMyGridConnectivityTest) this.CurrentGrid, this.CurrentBlockDefinition, modelMountPoints, ref gizmoSpace.m_rotation, ref gizmoSpace.m_centerPos);
            }
          }
          if (this.PlacingSmallGridOnLargeStatic)
          {
            MatrixD inverseBlockInGridWorldMatrix = MatrixD.Invert(gizmoSpace.m_worldMatrixAdd);
            gizmoSpace.m_showGizmoCube = this.IsBuildToolActive() && !this.IntersectsCharacterOrCamera(gizmoSpace, this.CurrentGrid.GridSize, ref inverseBlockInGridWorldMatrix);
          }
          else
            gizmoSpace.m_showGizmoCube = this.IsBuildToolActive() && !this.IntersectsCharacterOrCamera(gizmoSpace, this.CurrentGrid.GridSize, ref this.m_invGridWorldMatrix);
          gizmoSpace.m_buildAllowed &= gizmoSpace.m_showGizmoCube;
          Vector3D zero = Vector3D.Zero;
          Vector3D translation = gizmoSpace.m_worldMatrixAdd.Translation;
          MatrixD worldMatrixAdd = gizmoSpace.m_worldMatrixAdd;
          int num1 = 0;
          Vector3 vector3_1;
          for (vector3_1.X = 0.0f; (double) vector3_1.X < (double) this.CurrentBlockDefinition.Size.X; ++vector3_1.X)
          {
            for (vector3_1.Y = 0.0f; (double) vector3_1.Y < (double) this.CurrentBlockDefinition.Size.Y; ++vector3_1.Y)
            {
              for (vector3_1.Z = 0.0f; (double) vector3_1.Z < (double) this.CurrentBlockDefinition.Size.Z; ++vector3_1.Z)
              {
                if (this.PlacingSmallGridOnLargeStatic)
                {
                  float num2 = MyDefinitionManager.Static.GetCubeSize(this.CurrentBlockDefinition.CubeSize) / this.CurrentGrid.GridSize;
                  Vector3D vector3D1 = (Vector3D) gizmoSpace.m_positionsSmallOnLarge[num1++];
                  Vector3I inputPosition = Vector3I.Round(vector3D1 / (double) num2);
                  Vector3D vector3D2 = Vector3D.Transform(vector3D1 * (double) this.CurrentGrid.GridSize, this.CurrentGrid.WorldMatrix);
                  zero += vector3D2;
                  worldMatrixAdd.Translation = vector3D2;
                  MyCubeGrid.GetCubeParts(this.CurrentBlockDefinition, inputPosition, gizmoSpace.m_localMatrixAdd.GetOrientation(), this.CurrentGrid.GridSize, gizmoSpace.m_cubeModelsTemp, gizmoSpace.m_cubeMatricesTemp, gizmoSpace.m_cubeNormals, gizmoSpace.m_patternOffsets, true);
                  if (gizmoSpace.m_showGizmoCube)
                  {
                    for (int index = 0; index < gizmoSpace.m_cubeMatricesTemp.Count; ++index)
                    {
                      MatrixD matrixD2 = gizmoSpace.m_cubeMatricesTemp[index];
                      matrixD2.Translation *= (double) num2;
                      matrixD2 *= this.CurrentGrid.WorldMatrix;
                      matrixD2.Translation = vector3D2;
                      gizmoSpace.m_cubeMatricesTemp[index] = matrixD2;
                    }
                    this.m_gizmo.AddFastBuildParts(gizmoSpace, this.CurrentBlockDefinition, this.CurrentGrid);
                    this.m_gizmo.UpdateGizmoCubeParts(gizmoSpace, this.m_renderData, ref this.m_invGridWorldMatrix);
                  }
                }
                else
                {
                  Vector3I position = gizmoSpace.m_positions[num1++];
                  Vector3D vector3D = Vector3D.Transform(position * this.CurrentGrid.GridSize, this.CurrentGrid.WorldMatrix);
                  zero += position * this.CurrentGrid.GridSize;
                  MyCubeBlockDefinition currentBlockDefinition = this.CurrentBlockDefinition;
                  Vector3I inputPosition = position;
                  MatrixD orientation3 = matrixD1.GetOrientation();
                  Matrix rotation = (Matrix) ref orientation3;
                  double gridSize = (double) this.CurrentGrid.GridSize;
                  List<string> cubeModelsTemp = gizmoSpace.m_cubeModelsTemp;
                  List<MatrixD> cubeMatricesTemp = gizmoSpace.m_cubeMatricesTemp;
                  List<Vector3> cubeNormals = gizmoSpace.m_cubeNormals;
                  List<Vector4UByte> patternOffsets = gizmoSpace.m_patternOffsets;
                  MyCubeGrid.GetCubeParts(currentBlockDefinition, inputPosition, rotation, (float) gridSize, cubeModelsTemp, cubeMatricesTemp, cubeNormals, patternOffsets, false);
                  if (gizmoSpace.m_showGizmoCube)
                  {
                    for (int index = 0; index < gizmoSpace.m_cubeMatricesTemp.Count; ++index)
                    {
                      MatrixD matrixD2 = gizmoSpace.m_cubeMatricesTemp[index] * this.CurrentGrid.WorldMatrix;
                      matrixD2.Translation = vector3D;
                      gizmoSpace.m_cubeMatricesTemp[index] = matrixD2;
                    }
                    this.m_gizmo.AddFastBuildParts(gizmoSpace, this.CurrentBlockDefinition, this.CurrentGrid);
                    this.m_gizmo.UpdateGizmoCubeParts(gizmoSpace, this.m_renderData, ref this.m_invGridWorldMatrix, this.CurrentBlockDefinition);
                  }
                }
              }
            }
          }
          Vector3D position1 = zero / (double) this.CurrentBlockDefinition.Size.Size;
          if (!this.m_animationLock)
          {
            gizmoSpace.m_animationProgress = 0.0f;
            gizmoSpace.m_animationLastPosition = position1;
          }
          else if ((double) gizmoSpace.m_animationProgress < 1.0)
            position1 = Vector3D.Lerp(gizmoSpace.m_animationLastPosition, position1, (double) gizmoSpace.m_animationProgress);
          Vector3D vector3D3 = Vector3D.Transform(position1, this.CurrentGrid.WorldMatrix);
          worldMatrixAdd.Translation = vector3D3;
          gizmoSpace.m_worldMatrixAdd = worldMatrixAdd;
          float gridSize1 = this.PlacingSmallGridOnLargeStatic ? MyDefinitionManager.Static.GetCubeSize(this.CurrentBlockDefinition.CubeSize) : this.CurrentGrid.GridSize;
          BoundingBoxD localbox1 = new BoundingBoxD((Vector3D) (-this.CurrentBlockDefinition.Size * gridSize1 * 0.5f), (Vector3D) (this.CurrentBlockDefinition.Size * gridSize1 * 0.5f));
          MyGridPlacementSettings placementSettings1 = MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.GetGridPlacementSettings(this.CurrentBlockDefinition.CubeSize, this.CurrentGrid.IsStatic);
          MyBlockOrientation blockOrientation = new MyBlockOrientation(ref Quaternion.Identity);
          bool flag3 = MyCubeGrid.TestVoxelPlacement(this.CurrentBlockDefinition, placementSettings1, false, worldMatrixAdd, localbox1);
          gizmoSpace.m_buildAllowed &= flag3;
          if (this.PlacingSmallGridOnLargeStatic)
          {
            if (MySession.Static.SurvivalMode && !MyBlockBuilderBase.SpectatorIsBuilding && !MySession.Static.CreativeToolsEnabled(Sync.MyId))
            {
              Matrix matrix = Matrix.Invert((Matrix) ref worldMatrixAdd);
              MatrixD invGridWorldMatrix = (MatrixD) ref matrix;
              MyCubeBuilder.BuildComponent.GetBlockPlacementMaterials(this.CurrentBlockDefinition, gizmoSpace.m_addPos, orientation2, this.CurrentGrid);
              gizmoSpace.m_buildAllowed &= MyCubeBuilder.BuildComponent.HasBuildingMaterials((MyEntity) MySession.Static.LocalCharacter);
              if (!MyCubeBuilderGizmo.DefaultGizmoCloseEnough(ref invGridWorldMatrix, localbox1, gridSize1, MyBlockBuilderBase.IntersectionDistance) || MyBlockBuilderBase.CameraControllerSpectator)
              {
                gizmoSpace.m_buildAllowed = false;
                gizmoSpace.m_removeBlock = (MySlimBlock) null;
                return;
              }
            }
            MyGridPlacementSettings placementSettings2 = MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.GetGridPlacementSettings(this.CurrentGrid.GridSizeEnum, this.CurrentGrid.IsStatic);
            bool flag1 = MyCubeBuilder.CheckValidBlockRotation(gizmoSpace.m_localMatrixAdd, this.CurrentBlockDefinition.Direction, this.CurrentBlockDefinition.Rotation) && MyCubeGrid.TestBlockPlacementArea(this.CurrentBlockDefinition, new MyBlockOrientation?(blockOrientation), worldMatrixAdd, ref placementSettings2, localbox1, !this.CurrentGrid.IsStatic, testVoxel: false);
            gizmoSpace.m_buildAllowed &= flag1;
            if (gizmoSpace.m_buildAllowed && gizmoSpace.SymmetryPlane == MySymmetrySettingModeEnum.Disabled)
              gizmoSpace.m_buildAllowed &= MyCubeGrid.CheckConnectivitySmallBlockToLargeGrid(this.CurrentGrid, this.CurrentBlockDefinition, ref gizmoSpace.m_rotation, ref gizmoSpace.m_addDir);
            gizmoSpace.m_worldMatrixAdd = worldMatrixAdd;
          }
          if (!MySessionComponentSafeZones.IsActionAllowed(localbox1.TransformFast(ref worldMatrixAdd), MySafeZoneAction.Building, user: Sync.MyId))
          {
            gizmoSpace.m_buildAllowed = false;
            gizmoSpace.m_removeBlock = (MySlimBlock) null;
          }
          Color white2 = Color.White;
          MyStringId myStringId = gizmoSpace.m_buildAllowed ? MyCubeBuilder.ID_GIZMO_DRAW_LINE : MyCubeBuilder.ID_GIZMO_DRAW_LINE_RED;
          if (this.IsBuildToolActive() && gizmoSpace.SymmetryPlane == MySymmetrySettingModeEnum.Disabled)
          {
            if (MyFakes.ENABLE_VR_BUILDING)
            {
              Vector3 vector3_2 = -0.5f * gizmoSpace.m_addDir;
              if (gizmoSpace.m_addPosSmallOnLarge.HasValue)
                vector3_2 = -0.5f * (MyDefinitionManager.Static.GetCubeSize(this.CurrentBlockDefinition.CubeSize) / this.CurrentGrid.GridSize) * gizmoSpace.m_addDir;
              Vector3 vector3_3 = vector3_2 * this.CurrentGrid.GridSize;
              Vector3I vector3I1 = Vector3I.Round(Vector3.Abs(Vector3.TransformNormal((Vector3) this.CurrentBlockDefinition.Size, gizmoSpace.m_localMatrixAdd)));
              Vector3I vector3I2 = Vector3I.One - Vector3I.Abs(gizmoSpace.m_addDir);
              Vector3 vector3_4 = gridSize1 * 0.5f * (vector3I1 * vector3I2) + 0.02f * Vector3I.Abs(gizmoSpace.m_addDir);
              BoundingBoxD localbox2 = new BoundingBoxD((Vector3D) (-vector3_4 + vector3_3), (Vector3D) (vector3_4 + vector3_3));
              MySimpleObjectDraw.DrawTransparentBox(ref worldMatrixAdd, ref localbox2, ref white2, MySimpleObjectRasterizer.Wireframe, 1, gizmoSpace.m_addPosSmallOnLarge.HasValue ? 0.04f : 0.06f, lineMaterial: new MyStringId?(myStringId), blendType: MyBillboard.BlendTypeEnum.LDR);
            }
            else
            {
              worldMatrix.Translation = worldMatrixAdd.Translation;
              MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref localbox1, ref white2, MySimpleObjectRasterizer.Wireframe, 1, 0.04f, lineMaterial: new MyStringId?(myStringId), blendType: MyBillboard.BlendTypeEnum.LDR);
            }
          }
          gizmoSpace.m_cubeMatricesTemp.Clear();
          gizmoSpace.m_cubeModelsTemp.Clear();
          if (gizmoSpace.m_showGizmoCube)
          {
            if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS)
            {
              float cubeSize = MyDefinitionManager.Static.GetCubeSize(this.CurrentBlockDefinition.CubeSize);
              if (!this.PlacingSmallGridOnLargeStatic)
                cubeSize = this.CurrentGrid.GridSize;
              MyCubeBuilder.DrawMountPoints(cubeSize, this.CurrentBlockDefinition, ref worldMatrixAdd);
            }
            Vector3D result;
            Vector3D.TransformNormal(ref this.CurrentBlockDefinition.ModelOffset, ref gizmoSpace.m_worldMatrixAdd, out result);
            worldMatrix.Translation = vector3D3 + (double) this.CurrentGrid.GridScale * result;
            this.AddFastBuildModels(gizmoSpace, worldMatrix, gizmoSpace.m_cubeMatricesTemp, gizmoSpace.m_cubeModelsTemp, gizmoSpace.m_blockDefinition);
            for (int index = 0; index < gizmoSpace.m_cubeMatricesTemp.Count; ++index)
            {
              string assetName = gizmoSpace.m_cubeModelsTemp[index];
              if (!string.IsNullOrEmpty(assetName))
                this.m_renderData.AddInstance(MyModel.GetId(assetName), gizmoSpace.m_cubeMatricesTemp[index], ref this.m_invGridWorldMatrix, MyPlayer.SelectedColor, new MyStringHash?(MyStringHash.GetOrCompute(MyPlayer.SelectedArmorSkin)));
            }
          }
          if (gizmoSpace.SymmetryPlane == MySymmetrySettingModeEnum.Disabled)
          {
            IMyHudStat stat = MyHud.Stats.GetStat(MyStringHash.GetOrCompute("hud_mode"));
            bool draw1 = ((MyHud.MinimalHud || MyHud.CutsceneHud ? 0 : (MySandboxGame.Config.RotationHints ? 1 : 0)) & (draw ? 1 : 0)) != 0 && MyFakes.ENABLE_ROTATION_HINTS && (double) stat.CurrentValue == 1.0 && !MyInput.Static.IsJoystickLastUsed;
            this.m_rotationHints.CalculateRotationHints(worldMatrix, draw1);
          }
        }
      }
      if (gizmoSpace.m_startRemove.HasValue && gizmoSpace.m_continueBuild.HasValue)
      {
        gizmoSpace.m_buildAllowed = this.IsBuildToolActive();
        MyCubeBuilder.DrawRemovingCubes(gizmoSpace.m_startRemove, gizmoSpace.m_continueBuild, gizmoSpace.m_removeBlock);
      }
      else if (remove && this.ShowRemoveGizmo)
      {
        if (gizmoSpace.m_removeBlocksInMultiBlock.Count > 0)
        {
          this.m_tmpBlockPositionsSet.Clear();
          MyCubeBuilder.GetAllBlocksPositions(gizmoSpace.m_removeBlocksInMultiBlock, this.m_tmpBlockPositionsSet);
          foreach (Vector3I tmpBlockPositions in this.m_tmpBlockPositionsSet)
            MyCubeBuilder.DrawSemiTransparentBox(tmpBlockPositions, tmpBlockPositions, this.CurrentGrid, color2, lineMaterial: new MyStringId?(MyCubeBuilder.ID_GIZMO_DRAW_LINE_RED));
          this.m_tmpBlockPositionsSet.Clear();
        }
        else if (gizmoSpace.m_removeBlock != null && !MyFakes.ENABLE_VR_BUILDING)
        {
          Color white2 = Color.White;
          Color white3 = Color.White;
          MyStringId nullOrEmpty = MyStringId.NullOrEmpty;
          Color color3;
          MyStringId gizmoDrawLineWhite;
          Color color4;
          if (this.IsBuildToolActive())
          {
            color3 = color2;
            gizmoDrawLineWhite = MyCubeBuilder.ID_GIZMO_DRAW_LINE_WHITE;
            color4 = color2;
          }
          else if (this.CurrentGrid.ColorGridOrBlockRequestValidation(MySession.Static.LocalPlayerId))
          {
            color3 = Color.Lime;
            gizmoDrawLineWhite = MyCubeBuilder.ID_GIZMO_DRAW_LINE_WHITE;
            color4 = Color.Lime;
          }
          else
          {
            color3 = color2;
            gizmoDrawLineWhite = MyCubeBuilder.ID_GIZMO_DRAW_LINE_WHITE;
            color4 = color2;
          }
          MyCubeBuilder.DrawSemiTransparentBox(this.CurrentGrid, gizmoSpace.m_removeBlock, color3, true, new MyStringId?(gizmoDrawLineWhite), new Vector4?((Vector4) color4));
        }
        if (gizmoSpace.m_removeBlock != null && MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_REMOVE_CUBE_COORDS)
        {
          MySlimBlock removeBlock = gizmoSpace.m_removeBlock;
          MyCubeGrid cubeGrid = removeBlock.CubeGrid;
          MatrixD worldMatrix = cubeGrid.WorldMatrix;
          Matrix matrix = (Matrix) ref worldMatrix;
          MyRenderProxy.DebugDrawText3D((Vector3D) Vector3.Transform(removeBlock.Position * cubeGrid.GridSize, matrix), removeBlock.Position.ToString(), Color.White, 1f, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
        }
      }
      else if (MySession.Static.SurvivalMode && !MySession.Static.CreativeToolsEnabled(Sync.MyId))
      {
        int num = MyBlockBuilderBase.CameraControllerSpectator ? 1 : 0;
        if (!MyCubeBuilderGizmo.DefaultGizmoCloseEnough(ref this.m_invGridWorldMatrix, new BoundingBoxD((Vector3D) (((Vector3) this.m_gizmo.SpaceDefault.m_min - new Vector3(0.5f)) * this.CurrentGrid.GridSize), (Vector3D) (((Vector3) this.m_gizmo.SpaceDefault.m_max + new Vector3(0.5f)) * this.CurrentGrid.GridSize)), this.CurrentGrid.GridSize, MyBlockBuilderBase.IntersectionDistance))
          gizmoSpace.m_removeBlock = (MySlimBlock) null;
      }
      gizmoSpace.m_animationProgress += this.m_animationSpeed;
    }

    private bool IntersectsCharacterOrCamera(
      MyCubeBuilderGizmo.MyGizmoSpaceProperties gizmoSpace,
      float gridSize,
      ref MatrixD inverseBlockInGridWorldMatrix)
    {
      if (this.CurrentBlockDefinition == null)
        return false;
      bool flag = false;
      if (MySector.MainCamera != null)
        flag = this.m_gizmo.PointInsideGizmo(MySector.MainCamera.Position, gizmoSpace.SourceSpace, ref inverseBlockInGridWorldMatrix, gridSize, 0.05f, this.CurrentVoxelBase != null, this.DynamicMode);
      if (flag)
        return true;
      if (MySession.Static.ControlledEntity != null && MySession.Static.ControlledEntity is MyCharacter)
      {
        this.m_collisionTestPoints.Clear();
        MyCubeBuilder.PrepareCharacterCollisionPoints(this.m_collisionTestPoints);
        flag = this.m_gizmo.PointsAABBIntersectsGizmo(this.m_collisionTestPoints, gizmoSpace.SourceSpace, ref inverseBlockInGridWorldMatrix, gridSize, 0.05f, this.CurrentVoxelBase != null, this.DynamicMode);
      }
      return flag;
    }

    public static bool CheckValidBlockRotation(
      Matrix localMatrix,
      MyBlockDirection blockDirection,
      MyBlockRotation blockRotation)
    {
      Vector3I vector3I1 = Vector3I.Round(localMatrix.Forward);
      Vector3I vector3I2 = Vector3I.Round(localMatrix.Up);
      int num1 = Vector3I.Dot(ref vector3I1, ref vector3I1);
      int num2 = Vector3I.Dot(ref vector3I2, ref vector3I2);
      return num1 > 1 || num2 > 1 ? blockDirection == MyBlockDirection.Both : blockDirection != MyBlockDirection.Horizontal || !(vector3I1 == Vector3I.Up) && !(vector3I1 == -Vector3I.Up) && (blockRotation != MyBlockRotation.Vertical || !(vector3I2 != Vector3I.Up));
    }

    public static bool CheckValidBlocksRotation(Matrix gridLocalMatrix, MyCubeGrid grid)
    {
      bool flag = true;
      foreach (MySlimBlock block1 in grid.GetBlocks())
      {
        Matrix result;
        if (block1.FatBlock is MyCompoundCubeBlock fatBlock)
        {
          foreach (MySlimBlock block2 in fatBlock.GetBlocks())
          {
            block2.Orientation.GetMatrix(out result);
            result *= gridLocalMatrix;
            flag = flag && MyCubeBuilder.CheckValidBlockRotation(result, block2.BlockDefinition.Direction, block2.BlockDefinition.Rotation);
            if (!flag)
              break;
          }
        }
        else
        {
          block1.Orientation.GetMatrix(out result);
          result *= gridLocalMatrix;
          flag = flag && MyCubeBuilder.CheckValidBlockRotation(result, block1.BlockDefinition.Direction, block1.BlockDefinition.Rotation);
        }
        if (!flag)
          break;
      }
      return flag;
    }

    public virtual void Add()
    {
      if (this.CurrentBlockDefinition == null)
        return;
      this.m_blocksBuildQueue.Clear();
      bool flag = true;
      foreach (MyCubeBuilderGizmo.MyGizmoSpaceProperties space in this.m_gizmo.Spaces)
      {
        if (this.BuildInputValid && space.Enabled && (space.m_buildAllowed && MyCubeBuilder.Static.canBuild))
        {
          flag = false;
          this.AddBlocksToBuildQueueOrSpawn(space);
        }
      }
      if (flag)
        this.NotifyPlacementUnable();
      if (this.m_blocksBuildQueue.Count <= 0)
        return;
      if (MyMusicController.Static != null)
        MyMusicController.Static.Building(2000);
      this.CurrentGrid.BuildBlocks(MyPlayer.SelectedColor, MyStringHash.GetOrCompute(MyPlayer.SelectedArmorSkin), this.m_blocksBuildQueue, MySession.Static.LocalCharacterEntityId, MySession.Static.LocalPlayerId);
    }

    public void NotifyPlacementUnable()
    {
      if (this.CurrentBlockDefinition == null)
        return;
      MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
      this.m_cubePlacementUnable.SetTextFormatArguments((object) this.CurrentBlockDefinition.DisplayNameText);
      MyHud.Notifications.Add((MyHudNotificationBase) this.m_cubePlacementUnable);
    }

    public bool AddBlocksToBuildQueueOrSpawn(
      MyCubeBlockDefinition blockDefinition,
      ref MatrixD worldMatrixAdd,
      Vector3I min,
      Vector3I max,
      Vector3I center,
      Quaternion localOrientation)
    {
      return this.AddBlocksToBuildQueueOrSpawn(blockDefinition, ref worldMatrixAdd, min, max, center, localOrientation, new MyCubeGrid.MyBlockVisuals(MyPlayer.SelectedColor.PackHSVToUint(), MyStringHash.GetOrCompute(MyPlayer.SelectedArmorSkin)));
    }

    private bool AddBlocksToBuildQueueOrSpawn(
      MyCubeBlockDefinition blockDefinition,
      ref MatrixD worldMatrixAdd,
      Vector3I min,
      Vector3I max,
      Vector3I center,
      Quaternion localOrientation,
      MyCubeGrid.MyBlockVisuals visuals)
    {
      MyPlayer.PlayerId result;
      if (!MySession.Static.Players.TryGetPlayerId(MySession.Static.LocalPlayerId, out result) || !MySession.Static.Players.TryGetPlayerById(result, out MyPlayer _))
        return false;
      bool flag1 = MySession.Static.CreativeToolsEnabled(result.SteamId) || MySession.Static.CreativeMode;
      if (!MySession.Static.CheckLimitsAndNotify(MySession.Static.LocalPlayerId, blockDefinition.BlockPairName, flag1 ? blockDefinition.PCU : MyCubeBlockDefinition.PCU_CONSTRUCTION_STAGE_COST, 1))
        return false;
      MyCubeBuilder.BuildData buildData = new MyCubeBuilder.BuildData();
      bool flag2;
      if (this.GridAndBlockValid)
      {
        if (this.PlacingSmallGridOnLargeStatic)
        {
          MatrixD matrixD = worldMatrixAdd;
          buildData.Position = matrixD.Translation;
          if (MySession.Static.ControlledEntity != null)
            buildData.Position -= MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition();
          else
            buildData.AbsolutePosition = true;
          buildData.Forward = (Vector3) matrixD.Forward;
          buildData.Up = (Vector3) matrixD.Up;
          MyMultiplayer.RaiseStaticEvent<MyCubeBuilder.Author, DefinitionIdBlit, MyCubeBuilder.BuildData, bool, bool, MyCubeGrid.MyBlockVisuals>((Func<IMyEventOwner, Action<MyCubeBuilder.Author, DefinitionIdBlit, MyCubeBuilder.BuildData, bool, bool, MyCubeGrid.MyBlockVisuals>>) (s => new Action<MyCubeBuilder.Author, DefinitionIdBlit, MyCubeBuilder.BuildData, bool, bool, MyCubeGrid.MyBlockVisuals>(MyCubeBuilder.RequestGridSpawn)), new MyCubeBuilder.Author(MySession.Static.LocalCharacterEntityId, MySession.Static.LocalPlayerId), (DefinitionIdBlit) blockDefinition.Id, buildData, MySession.Static.CreativeToolsEnabled(Sync.MyId), true, visuals);
        }
        else
          this.m_blocksBuildQueue.Add(new MyCubeGrid.MyBlockLocation(blockDefinition.Id, min, max, center, localOrientation, MyEntityIdentifier.AllocateId(), MySession.Static.LocalPlayerId));
        flag2 = true;
      }
      else
      {
        buildData.Position = worldMatrixAdd.Translation;
        if (MySession.Static.ControlledEntity != null)
          buildData.Position -= MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition();
        else
          buildData.AbsolutePosition = true;
        buildData.Forward = (Vector3) worldMatrixAdd.Forward;
        buildData.Up = (Vector3) worldMatrixAdd.Up;
        MyMultiplayer.RaiseStaticEvent<MyCubeBuilder.Author, DefinitionIdBlit, MyCubeBuilder.BuildData, bool, bool, MyCubeGrid.MyBlockVisuals>((Func<IMyEventOwner, Action<MyCubeBuilder.Author, DefinitionIdBlit, MyCubeBuilder.BuildData, bool, bool, MyCubeGrid.MyBlockVisuals>>) (s => new Action<MyCubeBuilder.Author, DefinitionIdBlit, MyCubeBuilder.BuildData, bool, bool, MyCubeGrid.MyBlockVisuals>(MyCubeBuilder.RequestGridSpawn)), new MyCubeBuilder.Author(MySession.Static.LocalCharacterEntityId, MySession.Static.LocalPlayerId), (DefinitionIdBlit) blockDefinition.Id, buildData, MySession.Static.CreativeToolsEnabled(Sync.MyId), false, visuals);
        flag2 = true;
        ++MySession.Static.TotalBlocksCreated;
        if (MySession.Static.ControlledEntity is MyCockpit)
          ++MySession.Static.TotalBlocksCreatedFromShips;
      }
      if (this.OnBlockAdded != null)
        this.OnBlockAdded(blockDefinition);
      return flag2;
    }

    private bool AddBlocksToBuildQueueOrSpawn(
      MyCubeBuilderGizmo.MyGizmoSpaceProperties gizmoSpace)
    {
      return this.AddBlocksToBuildQueueOrSpawn(gizmoSpace.m_blockDefinition, ref gizmoSpace.m_worldMatrixAdd, gizmoSpace.m_min, gizmoSpace.m_max, gizmoSpace.m_centerPos, gizmoSpace.LocalOrientation);
    }

    private void UpdateGizmos(bool addPos, bool removePos, bool draw)
    {
      if (this.CurrentBlockDefinition == null || this.CurrentGrid != null && this.CurrentGrid.Physics != null && this.CurrentGrid.Physics.RigidBody.HasProperty(254))
        return;
      this.m_gizmo.SpaceDefault.m_blockDefinition = this.CurrentBlockDefinition;
      this.m_gizmo.EnableGizmoSpaces(this.CurrentBlockDefinition, this.CurrentGrid, this.UseSymmetry);
      this.m_renderData.BeginCollectingInstanceData();
      this.m_rotationHints.Clear();
      int length = this.m_gizmo.Spaces.Length;
      if (this.CurrentGrid != null)
        this.m_invGridWorldMatrix = this.CurrentGrid.PositionComp.WorldMatrixInvScaled;
      for (int index = 0; index < length; ++index)
      {
        MyCubeBuilderGizmo.MyGizmoSpaceProperties space = this.m_gizmo.Spaces[index];
        bool flag = addPos && this.BuildInputValid;
        if (space.SymmetryPlane == MySymmetrySettingModeEnum.Disabled)
        {
          Quaternion localOrientation = space.LocalOrientation;
          if (!this.PlacingSmallGridOnLargeStatic && this.CurrentGrid != null && !MySession.Static.CreativeToolsEnabled(Sync.MyId))
            flag &= this.CurrentGrid.CanAddCube(space.m_addPos, new MyBlockOrientation?(new MyBlockOrientation(ref localOrientation)), this.CurrentBlockDefinition);
        }
        else
        {
          flag &= this.UseSymmetry;
          removePos &= this.UseSymmetry;
        }
        this.UpdateGizmo(space, flag || this.FreezeGizmo, removePos || this.FreezeGizmo, draw);
      }
    }

    public MyOrientedBoundingBoxD GetBuildBoundingBox(float inflate = 0.0f)
    {
      if (this.m_gizmo.SpaceDefault.m_blockDefinition == null)
        return new MyOrientedBoundingBoxD();
      float cubeSize = MyDefinitionManager.Static.GetCubeSize(this.m_gizmo.SpaceDefault.m_blockDefinition.CubeSize);
      Vector3 vector3 = this.m_gizmo.SpaceDefault.m_blockDefinition.Size * cubeSize * 0.5f + inflate;
      MatrixD worldMatrixAdd = this.m_gizmo.SpaceDefault.m_worldMatrixAdd;
      if (this.m_gizmo.SpaceDefault.m_removeBlock != null && !this.m_gizmo.SpaceDefault.m_addPosSmallOnLarge.HasValue)
      {
        MySlimBlock removeBlock = this.m_gizmo.SpaceDefault.m_removeBlock;
        Vector3D vector3D = Vector3D.Transform(this.m_gizmo.SpaceDefault.m_addPos * cubeSize, removeBlock.CubeGrid.PositionComp.WorldMatrixRef);
        worldMatrixAdd.Translation = vector3D;
      }
      return new MyOrientedBoundingBoxD(new BoundingBoxD(Vector3D.Zero - vector3, Vector3D.Zero + vector3), worldMatrixAdd);
    }

    public virtual bool CanStartConstruction(MyEntity buildingEntity)
    {
      MyCubeBuilder.BuildComponent.GetGridSpawnMaterials(this.CurrentBlockDefinition, this.m_gizmo.SpaceDefault.m_worldMatrixAdd, false);
      return MyCubeBuilder.BuildComponent.HasBuildingMaterials(buildingEntity);
    }

    public virtual bool AddConstruction(MyEntity builder)
    {
      MyPlayer controllingPlayer = Sync.Players.GetControllingPlayer(builder);
      if (!this.canBuild || controllingPlayer != null && !controllingPlayer.IsLocalPlayer)
        return false;
      if (controllingPlayer == null || controllingPlayer.IsRemotePlayer)
      {
        MyEntity isUsing = (builder as MyCharacter).IsUsing;
        if (isUsing == null)
          return false;
        controllingPlayer = Sync.Players.GetControllingPlayer(isUsing);
        if (controllingPlayer == null || controllingPlayer.IsRemotePlayer)
          return false;
      }
      MyCubeBuilderGizmo.MyGizmoSpaceProperties spaceDefault = this.m_gizmo.SpaceDefault;
      if (spaceDefault.Enabled && this.BuildInputValid && (spaceDefault.m_buildAllowed && this.canBuild))
      {
        this.m_blocksBuildQueue.Clear();
        int num = this.AddBlocksToBuildQueueOrSpawn(spaceDefault) ? 1 : 0;
        if (num == 0)
          return num != 0;
        if (this.CurrentGrid == null)
          return num != 0;
        if (this.m_blocksBuildQueue.Count <= 0)
          return num != 0;
        if (MySession.Static != null && builder == MySession.Static.LocalCharacter && MyMusicController.Static != null)
          MyMusicController.Static.Building(2000);
        if (Sync.IsServer)
          MyGuiAudio.PlaySound(MyGuiSounds.HudPlaceBlock);
        if (builder == MySession.Static.LocalCharacter)
        {
          ++MySession.Static.TotalBlocksCreated;
          if (MySession.Static.ControlledEntity is MyCockpit)
            ++MySession.Static.TotalBlocksCreatedFromShips;
        }
        this.CurrentGrid.BuildBlocks(MyPlayer.SelectedColor, MyStringHash.GetOrCompute(MyPlayer.SelectedArmorSkin), this.m_blocksBuildQueue, builder.EntityId, controllingPlayer.Identity.IdentityId);
        return num != 0;
      }
      this.NotifyPlacementUnable();
      return false;
    }

    private Vector3I MakeCubePosition(Vector3D position)
    {
      position -= this.CurrentGrid.WorldMatrix.Translation;
      Vector3D vector3D1 = new Vector3D((double) this.CurrentGrid.GridSize);
      Vector3D vector3D2 = position / vector3D1;
      Vector3I vector3I;
      vector3I.X = (int) Math.Round(vector3D2.X);
      vector3I.Y = (int) Math.Round(vector3D2.Y);
      vector3I.Z = (int) Math.Round(vector3D2.Z);
      return vector3I;
    }

    public void GetAddPosition(out Vector3D position) => position = this.m_gizmo.SpaceDefault.m_worldMatrixAdd.Translation;

    public virtual bool GetAddAndRemovePositions(
      float gridSize,
      bool placingSmallGridOnLargeStatic,
      out Vector3I addPos,
      out Vector3? addPosSmallOnLarge,
      out Vector3I addDir,
      out Vector3I removePos,
      out MySlimBlock removeBlock,
      out ushort? compoundBlockId,
      HashSet<Tuple<MySlimBlock, ushort?>> removeBlocksInMultiBlock)
    {
      addPosSmallOnLarge = new Vector3?();
      removePos = new Vector3I();
      removeBlock = (MySlimBlock) null;
      MySlimBlock intersectedBlock;
      Vector3D intersectedBlockPos;
      Vector3D intersectExactPos;
      bool flag = this.GetBlockAddPosition(gridSize, placingSmallGridOnLargeStatic, out intersectedBlock, out intersectedBlockPos, out intersectExactPos, out addPos, out addDir, out compoundBlockId);
      float num1 = placingSmallGridOnLargeStatic ? this.CurrentGrid.GridSize : gridSize;
      if (!this.MaxGridDistanceFrom.HasValue || Vector3D.DistanceSquared(intersectExactPos * (double) num1, Vector3D.Transform(this.MaxGridDistanceFrom.Value, this.m_invGridWorldMatrix)) < (double) MyBlockBuilderBase.CubeBuilderDefinition.MaxBlockBuildingDistance * (double) MyBlockBuilderBase.CubeBuilderDefinition.MaxBlockBuildingDistance)
      {
        removePos = Vector3I.Round(intersectedBlockPos);
        removeBlock = intersectedBlock;
        if (removeBlock != null && removeBlock.FatBlock != null && MySession.Static.ControlledEntity as MyShipController == removeBlock.FatBlock)
          removeBlock = (MySlimBlock) null;
      }
      else if (this.AllowFreeSpacePlacement && this.CurrentGrid != null)
      {
        Vector3D position = MyBlockBuilderBase.IntersectionStart + MyBlockBuilderBase.IntersectionDirection * (double) Math.Min(this.FreeSpacePlacementDistance, MyBlockBuilderBase.IntersectionDistance);
        addPos = this.MakeCubePosition(position);
        addDir = new Vector3I(0, 0, 1);
        removePos = addPos - addDir;
        removeBlock = this.CurrentGrid.GetCubeBlock(removePos);
        if (removeBlock != null && removeBlock.FatBlock != null && MySession.Static.ControlledEntity as MyShipController == removeBlock.FatBlock)
          removeBlock = (MySlimBlock) null;
        flag = true;
      }
      else
        flag = false;
      if (!MyCubeBuilder.Static.canBuild)
        return false;
      if (flag & placingSmallGridOnLargeStatic)
      {
        MatrixD matrix = (MatrixD) ref Matrix.Identity;
        if (intersectedBlock != null)
        {
          matrix = intersectedBlock.CubeGrid.WorldMatrix.GetOrientation();
          if (intersectedBlock.FatBlock != null)
          {
            if (compoundBlockId.HasValue)
            {
              if (intersectedBlock.FatBlock is MyCompoundCubeBlock fatBlock)
              {
                MySlimBlock block = fatBlock.GetBlock(compoundBlockId.Value);
                if (block != null && block.FatBlock.Components.Has<MyFractureComponentBase>())
                  return false;
              }
            }
            else if (intersectedBlock.FatBlock.Components.Has<MyFractureComponentBase>())
              return false;
          }
        }
        MatrixD.Invert(matrix);
        if (this.m_hitInfo.HasValue)
        {
          Vector3 vector3 = Vector3.TransformNormal(this.m_hitInfo.Value.HkHitInfo.Normal, this.m_invGridWorldMatrix);
          addDir = Vector3I.Sign(Vector3.DominantAxisProjection(vector3));
        }
        Vector3 vector3_1 = (Vector3) removePos + 0.5f * addDir;
        Vector3D vector3D1 = intersectExactPos - vector3_1;
        Vector3I vector3I1 = Vector3I.Abs(addDir);
        Vector3D vector3D2 = (Vector3I.One - vector3I1) * Vector3.Clamp((Vector3) vector3D1, new Vector3(-0.495f), new Vector3(0.495f)) + vector3I1 * vector3D1;
        Vector3D vector3D3 = vector3_1 + vector3D2;
        float num2 = gridSize / this.CurrentGrid.GridSize;
        Vector3 vector3_2 = (MyFakes.ENABLE_VR_BUILDING ? 0.25f : 0.1f) * num2 * addDir;
        Vector3I vector3I2 = Vector3I.Round((vector3D3 + vector3_2 - num2 * Vector3.Half) / (double) num2);
        addPosSmallOnLarge = new Vector3?(num2 * vector3I2 + num2 * Vector3.Half);
      }
      return flag;
    }

    protected virtual void PrepareBlocksToRemove()
    {
      this.m_tmpBlockPositionList.Clear();
      this.m_tmpCompoundBlockPositionIdList.Clear();
      foreach (MyCubeBuilderGizmo.MyGizmoSpaceProperties space in this.m_gizmo.Spaces)
      {
        if (space.Enabled && this.GridAndBlockValid && space.m_removeBlock != null && ((space.m_removeBlock.FatBlock == null || !space.m_removeBlock.FatBlock.IsSubBlock) && this.CurrentGrid == space.m_removeBlock.CubeGrid))
        {
          if (space.m_removeBlocksInMultiBlock.Count > 0)
          {
            foreach (Tuple<MySlimBlock, ushort?> tuple in space.m_removeBlocksInMultiBlock)
              this.RemoveBlock(tuple.Item1, tuple.Item2);
          }
          else
            this.RemoveBlock(space.m_removeBlock, space.m_blockIdInCompound, true);
          space.m_removeBlock = (MySlimBlock) null;
          space.m_removeBlocksInMultiBlock.Clear();
        }
      }
    }

    protected void Remove()
    {
      if (this.m_tmpBlockPositionList.Count <= 0 && this.m_tmpCompoundBlockPositionIdList.Count <= 0)
        return;
      MyGuiAudio.PlaySound(MyGuiSounds.HudDeleteBlock);
      if (this.m_tmpBlockPositionList.Count > 0)
      {
        this.CurrentGrid.RazeBlocks(this.m_tmpBlockPositionList, user: Sync.MyId);
        this.m_tmpBlockPositionList.Clear();
      }
      if (this.m_tmpCompoundBlockPositionIdList.Count <= 0)
        return;
      this.CurrentGrid.RazeBlockInCompoundBlock(this.m_tmpCompoundBlockPositionIdList);
    }

    protected void RemoveBlock(MySlimBlock block, ushort? blockIdInCompound, bool checkExisting = false)
    {
      if (block == null || block.FatBlock != null && block.FatBlock.IsSubBlock)
        return;
      if (block.FatBlock is MyCockpit fatBlock && fatBlock.Pilot != null)
      {
        this.m_removalTemporalData = new MyBlockRemovalData(block, blockIdInCompound, checkExisting);
        if (!MySession.Static.CreativeMode && MySession.Static.IsUserAdmin(Sync.MyId))
        {
          Action<MyGuiScreenMessageBox.ResultEnum> callback = new Action<MyGuiScreenMessageBox.ResultEnum>(this.OnClosedMessageBox);
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.RemovePilotToo), callback: callback));
        }
        else
          this.OnClosedMessageBox(MyGuiScreenMessageBox.ResultEnum.NO);
      }
      else
        this.RemoveBlockInternal(block, blockIdInCompound, checkExisting);
    }

    protected void RemoveBlockInternal(
      MySlimBlock block,
      ushort? blockIdInCompound,
      bool checkExisting = false)
    {
      if (block.FatBlock is MyCompoundCubeBlock)
      {
        if (blockIdInCompound.HasValue)
        {
          if (checkExisting && this.m_tmpCompoundBlockPositionIdList.Exists((Predicate<Tuple<Vector3I, ushort>>) (t => t.Item1 == block.Min && (int) t.Item2 == (int) blockIdInCompound.Value)))
            return;
          this.m_tmpCompoundBlockPositionIdList.Add(new Tuple<Vector3I, ushort>(block.Min, blockIdInCompound.Value));
        }
        else
        {
          if (checkExisting && this.m_tmpBlockPositionList.Contains(block.Min))
            return;
          this.m_tmpBlockPositionList.Add(block.Min);
        }
      }
      else
      {
        if (checkExisting && this.m_tmpBlockPositionList.Contains(block.Min))
          return;
        this.m_tmpBlockPositionList.Add(block.Min);
      }
    }

    public void OnClosedMessageBox(MyGuiScreenMessageBox.ResultEnum result)
    {
      if (this.m_removalTemporalData != null && this.m_removalTemporalData.Block != null && this.CurrentGrid != null && ((this.m_removalTemporalData.Block.FatBlock == null || !this.m_removalTemporalData.Block.FatBlock.IsSubBlock) && (this.m_removalTemporalData.Block.CubeGrid != null && !this.m_removalTemporalData.Block.CubeGrid.Closed)))
      {
        MyCockpit fatBlock = this.m_removalTemporalData.Block.FatBlock as MyCockpit;
        if (result == MyGuiScreenMessageBox.ResultEnum.NO && fatBlock != null && (!fatBlock.Closed && fatBlock.Pilot != null))
          fatBlock.RequestRemovePilot();
        this.RemoveBlockInternal(this.m_removalTemporalData.Block, this.m_removalTemporalData.BlockIdInCompound, this.m_removalTemporalData.CheckExisting);
        this.Remove();
      }
      this.m_removalTemporalData = (MyBlockRemovalData) null;
    }

    private void Change(int expand = 0)
    {
      this.m_tmpBlockPositionList.Clear();
      if (expand == -1)
      {
        this.CurrentGrid.SkinGrid(MyPlayer.SelectedColor, MyStringHash.GetOrCompute(MyPlayer.SelectedArmorSkin), true, true, MyGuiScreenColorPicker.ApplyColor, MyGuiScreenColorPicker.ApplySkin);
      }
      else
      {
        int index = -1;
        foreach (MyCubeBuilderGizmo.MyGizmoSpaceProperties space in this.m_gizmo.Spaces)
        {
          ++index;
          if (space.Enabled && space.m_removeBlock != null)
          {
            bool playSound = false;
            Vector3I min = space.m_removeBlock.Position - Vector3I.One * expand;
            Vector3I max = space.m_removeBlock.Position + Vector3I.One * expand;
            if (MyCubeBuilder.m_currColoringArea[index].Start != min || MyCubeBuilder.m_currColoringArea[index].End != max)
            {
              MyCubeBuilder.m_currColoringArea[index].Start = min;
              MyCubeBuilder.m_currColoringArea[index].End = max;
              playSound = true;
            }
            this.CurrentGrid.SkinBlocks(min, max, MyGuiScreenColorPicker.ApplyColor ? new Vector3?(MyPlayer.SelectedColor) : new Vector3?(), MyGuiScreenColorPicker.ApplySkin ? new MyStringHash?(MyStringHash.GetOrCompute(MyPlayer.SelectedArmorSkin)) : new MyStringHash?(), playSound, true);
          }
        }
      }
    }

    private bool IsInSymmetrySettingMode => this.SymmetrySettingMode != MySymmetrySettingModeEnum.NoPlane;

    private Vector3I? GetSingleMountPointNormal()
    {
      if (this.CurrentBlockDefinition == null)
        return new Vector3I?();
      MyCubeBlockDefinition.MountPoint[] modelMountPoints = this.CurrentBlockDefinition.GetBuildProgressModelMountPoints(1f);
      if (modelMountPoints == null || modelMountPoints.Length == 0)
        return new Vector3I?();
      Vector3I normal1 = modelMountPoints[0].Normal;
      if (this.AlignToDefault && !this.m_customRotation)
      {
        for (int index = 0; index < modelMountPoints.Length; ++index)
        {
          if (modelMountPoints[index].Default)
            return new Vector3I?(modelMountPoints[index].Normal);
        }
        for (int index = 0; index < modelMountPoints.Length; ++index)
        {
          if (MyCubeBlockDefinition.NormalToBlockSide(modelMountPoints[index].Normal) == BlockSideEnum.Bottom)
            return new Vector3I?(modelMountPoints[index].Normal);
        }
      }
      Vector3I vector3I = -normal1;
      switch (this.CurrentBlockDefinition.AutorotateMode)
      {
        case MyAutorotateMode.OneDirection:
          for (int index = 1; index < modelMountPoints.Length; ++index)
          {
            if (modelMountPoints[index].Normal != normal1)
              return new Vector3I?();
          }
          goto case MyAutorotateMode.FirstDirection;
        case MyAutorotateMode.OppositeDirections:
          for (int index = 1; index < modelMountPoints.Length; ++index)
          {
            Vector3I normal2 = modelMountPoints[index].Normal;
            if (normal2 != normal1 && normal2 != vector3I)
              return new Vector3I?();
          }
          goto case MyAutorotateMode.FirstDirection;
        case MyAutorotateMode.FirstDirection:
          return new Vector3I?(normal1);
        default:
          return new Vector3I?();
      }
    }

    public void CycleCubePlacementMode()
    {
      switch (this.CubePlacementMode)
      {
        case MyCubeBuilder.CubePlacementModeEnum.LocalCoordinateSystem:
          this.CubePlacementMode = MyCubeBuilder.CubePlacementModeEnum.FreePlacement;
          break;
        case MyCubeBuilder.CubePlacementModeEnum.FreePlacement:
          this.CubePlacementMode = MyCubeBuilder.CubePlacementModeEnum.GravityAligned;
          break;
        case MyCubeBuilder.CubePlacementModeEnum.GravityAligned:
          this.CubePlacementMode = this.CurrentBlockDefinition == null || this.CurrentBlockDefinition.CubeSize != MyCubeSize.Large ? MyCubeBuilder.CubePlacementModeEnum.FreePlacement : MyCubeBuilder.CubePlacementModeEnum.LocalCoordinateSystem;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private void ShowCubePlacementNotification()
    {
      MyHud.Notifications.Remove((MyHudNotificationBase) this.m_coloringToolHints);
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      MyStringId context = controlledEntity != null ? controlledEntity.AuxiliaryContext : MyStringId.NullOrEmpty;
      string str = MyInput.Static.IsJoystickLastUsed ? MyControllerHelper.GetCodeForControl(context, MyControlsSpace.FREE_ROTATION) : "[" + (object) MyInput.Static.GetGameControl(MyControlsSpace.FREE_ROTATION) + "]";
      switch (this.CubePlacementMode)
      {
        case MyCubeBuilder.CubePlacementModeEnum.LocalCoordinateSystem:
          this.m_cubePlacementModeNotification.SetTextFormatArguments((object) MyTexts.GetString(MyCommonTexts.NotificationCubePlacementMode_LocalCoordSystem));
          this.m_cubePlacementModeHint.SetTextFormatArguments((object) str, (object) MyTexts.GetString(MyCommonTexts.ControlHintCubePlacementMode_LocalCoordSystem));
          break;
        case MyCubeBuilder.CubePlacementModeEnum.FreePlacement:
          this.m_cubePlacementModeNotification.SetTextFormatArguments((object) MyTexts.GetString(MyCommonTexts.NotificationCubePlacementMode_FreePlacement));
          this.m_cubePlacementModeHint.SetTextFormatArguments((object) str, (object) MyTexts.GetString(MyCommonTexts.ControlHintCubePlacementMode_FreePlacement));
          break;
        case MyCubeBuilder.CubePlacementModeEnum.GravityAligned:
          this.m_cubePlacementModeNotification.SetTextFormatArguments((object) MyTexts.GetString(MyCommonTexts.NotificationCubePlacementMode_GravityAligned));
          this.m_cubePlacementModeHint.SetTextFormatArguments((object) str, (object) MyTexts.GetString(MyCommonTexts.ControlHintCubePlacementMode_GravityAligned));
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      this.UpdatePlacementNotificationState();
      MyHud.Notifications.Remove((MyHudNotificationBase) this.m_cubePlacementModeNotification);
      MyHud.Notifications.Add((MyHudNotificationBase) this.m_cubePlacementModeNotification);
      MyHud.Notifications.Remove((MyHudNotificationBase) this.m_cubePlacementModeHint);
      MyHud.Notifications.Add((MyHudNotificationBase) this.m_cubePlacementModeHint);
    }

    private void ShowColorToolNotifications()
    {
      MyHud.Notifications.Remove((MyHudNotificationBase) this.m_cubePlacementModeNotification);
      MyHud.Notifications.Remove((MyHudNotificationBase) this.m_cubePlacementModeHint);
      string str1;
      string str2;
      if (MyInput.Static.IsJoystickLastUsed)
      {
        IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
        if (controlledEntity == null)
        {
          MyStringId nullOrEmpty = MyStringId.NullOrEmpty;
        }
        else
        {
          MyStringId auxiliaryContext = controlledEntity.AuxiliaryContext;
        }
        str1 = '\xE006'.ToString();
        str2 = '\xE005'.ToString();
      }
      else
      {
        str1 = "[Ctrl]";
        str2 = "[Shift]";
      }
      this.m_coloringToolHints.SetTextFormatArguments((object) str1, (object) str2);
      MyHud.Notifications.Remove((MyHudNotificationBase) this.m_coloringToolHints);
      MyHud.Notifications.Add((MyHudNotificationBase) this.m_coloringToolHints);
    }

    private void CalculateCubePlacement()
    {
      if (!this.IsActivated || this.CurrentBlockDefinition == null)
        return;
      this.ChooseHitObject();
      Vector3D worldPos = this.m_hitInfo.HasValue ? this.m_hitInfo.Value.Position : MyBlockBuilderBase.IntersectionStart + (double) MyBlockBuilderBase.IntersectionDistance * MyBlockBuilderBase.IntersectionDirection;
      if (this.CurrentBlockDefinition == null)
        return;
      float cubeSize = MyDefinitionManager.Static.GetCubeSize(this.CurrentBlockDefinition.CubeSize);
      if (MyCoordinateSystem.Static != null)
      {
        double num1 = (worldPos - this.m_lastLocalCoordSysData.Origin.Position).LengthSquared();
        long num2 = this.m_currentGrid == null || this.m_currentGrid.LocalCoordSystem == this.m_lastLocalCoordSysData.Id ? (num1 > (double) MyCoordinateSystem.Static.CoordSystemSizeSquared ? 0L : this.m_lastLocalCoordSysData.Id) : this.m_currentGrid.LocalCoordSystem;
        this.m_lastLocalCoordSysData = MyCoordinateSystem.Static.SnapWorldPosToClosestGrid(ref worldPos, (double) cubeSize, MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.StaticGridAlignToCenter, new long?(num2));
      }
      switch (this.CubePlacementMode)
      {
        case MyCubeBuilder.CubePlacementModeEnum.LocalCoordinateSystem:
          this.CalculateLocalCoordinateSystemMode(worldPos);
          break;
        case MyCubeBuilder.CubePlacementModeEnum.FreePlacement:
          this.CalculateFreePlacementMode(worldPos);
          break;
        case MyCubeBuilder.CubePlacementModeEnum.GravityAligned:
          this.CalculateGravityAlignedMode(worldPos);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    protected void CalculateLocalCoordinateSystemMode(Vector3D position) => this.DynamicMode = this.m_currentGrid == null && this.m_currentVoxelBase == null;

    protected void CalculateFreePlacementMode(Vector3D position) => this.DynamicMode = this.m_currentGrid == null || this.IsDynamicOverride();

    protected void CalculateGravityAlignedMode(Vector3D position)
    {
      this.DynamicMode = this.m_currentGrid == null || this.IsDynamicOverride();
      if (this.m_animationLock)
        return;
      this.AlignToGravity(false);
    }

    public override void UpdateBeforeSimulation()
    {
      this.UpdateNotificationBlockLimit();
      MyShipController controlledEntity = MySession.Static.ControlledEntity as MyShipController;
      if (MyCubeBuilder.Static.IsActivated && controlledEntity != null)
      {
        if (controlledEntity.hasPower && controlledEntity.BuildingMode && MyEntities.IsInsideWorld(controlledEntity.PositionComp.GetPosition()))
          MyCubeBuilder.Static.canBuild = true;
        else
          MyCubeBuilder.Static.canBuild = false;
      }
      else
        MyCubeBuilder.Static.canBuild = true;
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      this.CalculateCubePlacement();
      MyCubeBuilder.UpdateBlockInfoHud();
    }

    protected override void UnloadData()
    {
      this.Deactivate();
      base.UnloadData();
      this.RemoveSymmetryNotification();
      this.m_gizmo.Clear();
      this.CurrentGrid = (MyCubeGrid) null;
      this.UnloadRenderObjects();
      this.m_cubeBuilderState = (MyCubeBuilderState) null;
      MyCubeBuilder.Static = (MyCubeBuilder) null;
    }

    private void UnloadRenderObjects()
    {
      this.m_gizmo.RemoveGizmoCubeParts();
      this.m_renderData.UnloadRenderObjects();
    }

    private void UpdateNotificationBlockLimit()
    {
    }

    public void UpdateNotificationBlockNotAvailable(bool updateNotAvailableNotification)
    {
      if (!MyFakes.ENABLE_NOTIFICATION_BLOCK_NOT_AVAILABLE)
        return;
      if (!updateNotAvailableNotification)
      {
        this.HideNotificationBlockNotAvailable();
      }
      else
      {
        bool flag1 = MySession.Static.GetCameraControllerEnum() != MyCameraControllerEnum.Spectator && false;
        bool flag2 = MySession.Static.ControlledEntity != null && MySession.Static.ControlledEntity is MyCockpit && !flag1;
        if (!this.BlockCreationIsActivated || this.CurrentBlockDefinition == null)
          return;
        if (this.CurrentGrid != null && this.CurrentBlockDefinition.CubeSize != this.CurrentGrid.GridSizeEnum && (!flag2 && !this.PlacingSmallGridOnLargeStatic))
        {
          this.ShowNotificationBlockNotAvailable(this.CurrentGrid.GridSizeEnum == MyCubeSize.Small ? MySpaceTexts.NotificationArgLargeShip : MySpaceTexts.NotificationArgSmallShip, this.CurrentBlockDefinition.DisplayNameText, this.CurrentGrid.GridSizeEnum == MyCubeSize.Small ? MySpaceTexts.NotificationArgSmallShip : (this.CurrentGrid.IsStatic ? MySpaceTexts.NotificationArgStation : MySpaceTexts.NotificationArgLargeShip));
        }
        else
        {
          if (!this.BlockCreationIsActivated || this.CurrentBlockDefinition == null || this.CurrentGrid != null)
            return;
          this.ShowNotificationBlockNotAvailable(this.CurrentBlockDefinition.CubeSize == MyCubeSize.Small ? MySpaceTexts.NotificationArgSmallShip : MySpaceTexts.NotificationArgLargeShip, this.CurrentBlockDefinition.DisplayNameText, this.CurrentBlockDefinition.CubeSize == MyCubeSize.Small ? MySpaceTexts.NotificationArgLargeShip : MySpaceTexts.NotificationArgSmallShip);
        }
      }
    }

    private void ShowNotificationBlockNotAvailable(
      MyStringId grid1Text,
      string blockDisplayName,
      MyStringId grid2Text)
    {
      if (!MyFakes.ENABLE_NOTIFICATION_BLOCK_NOT_AVAILABLE)
        return;
      if (this.m_blockNotAvailableNotification == null)
        this.m_blockNotAvailableNotification = new MyHudNotification(MySpaceTexts.NotificationBlockNotAvailableFor, font: "Red", priority: 1);
      this.m_blockNotAvailableNotification.SetTextFormatArguments((object) MyTexts.Get(grid1Text).ToLower().FirstLetterUpperCase(), (object) blockDisplayName.ToLower(), (object) MyTexts.Get(grid2Text).ToLower());
      MyHud.Notifications.Add((MyHudNotificationBase) this.m_blockNotAvailableNotification);
    }

    private void HideNotificationBlockNotAvailable()
    {
      if (this.m_blockNotAvailableNotification == null || !this.m_blockNotAvailableNotification.Alive)
        return;
      MyHud.Notifications.Remove((MyHudNotificationBase) this.m_blockNotAvailableNotification);
    }

    public virtual void StartBuilding() => this.StartBuilding(ref this.m_gizmo.SpaceDefault.m_startBuild, this.m_gizmo.SpaceDefault.m_startRemove);

    protected void StartBuilding(ref Vector3I? startBuild, Vector3I? startRemove)
    {
      if (!this.GridAndBlockValid && !this.VoxelMapAndBlockValid || this.PlacingSmallGridOnLargeStatic)
        return;
      this.m_initialIntersectionStart = MyBlockBuilderBase.IntersectionStart;
      this.m_initialIntersectionDirection = MyBlockBuilderBase.IntersectionDirection;
      float cubeSize = MyDefinitionManager.Static.GetCubeSize(this.CurrentBlockDefinition.CubeSize);
      Vector3I addPos;
      if (!startRemove.HasValue && this.GetAddAndRemovePositions(cubeSize, this.PlacingSmallGridOnLargeStatic, out addPos, out Vector3? _, out Vector3I _, out Vector3I _, out MySlimBlock _, out ushort? _, (HashSet<Tuple<MySlimBlock, ushort?>>) null))
        startBuild = new Vector3I?(addPos);
      else
        startBuild = new Vector3I?();
    }

    protected virtual void StartRemoving() => this.StartRemoving(this.m_gizmo.SpaceDefault.m_startBuild, ref this.m_gizmo.SpaceDefault.m_startRemove);

    protected void StartRemoving(Vector3I? startBuild, ref Vector3I? startRemove)
    {
      if (this.PlacingSmallGridOnLargeStatic)
        return;
      this.m_initialIntersectionStart = MyBlockBuilderBase.IntersectionStart;
      this.m_initialIntersectionDirection = MyBlockBuilderBase.IntersectionDirection;
      if (this.CurrentGrid == null || startBuild.HasValue)
        return;
      startRemove = this.IntersectCubes(this.CurrentGrid, out double _);
    }

    public virtual void ContinueBuilding(bool planeBuild)
    {
      MyCubeBuilderGizmo.MyGizmoSpaceProperties spaceDefault = this.m_gizmo.SpaceDefault;
      this.ContinueBuilding(planeBuild, spaceDefault.m_startBuild, spaceDefault.m_startRemove, ref spaceDefault.m_continueBuild, spaceDefault.m_min, spaceDefault.m_max);
    }

    protected void ContinueBuilding(
      bool planeBuild,
      Vector3I? startBuild,
      Vector3I? startRemove,
      ref Vector3I? continueBuild,
      Vector3I blockMinPosision,
      Vector3I blockMaxPosition)
    {
      if (!startBuild.HasValue && !startRemove.HasValue || !this.GridAndBlockValid && !this.VoxelMapAndBlockValid)
        return;
      continueBuild = new Vector3I?();
      if (this.CheckSmallViewChange())
        return;
      this.IntersectInflated(MyCubeBuilder.m_cacheGridIntersections, this.CurrentGrid);
      Vector3I vector3I1 = startBuild.HasValue ? blockMinPosision : startRemove.Value;
      Vector3I vector3I2 = startBuild.HasValue ? blockMaxPosition : startRemove.Value;
      Vector3I vector3I3;
      for (vector3I3.X = vector3I1.X; vector3I3.X <= vector3I2.X; ++vector3I3.X)
      {
        for (vector3I3.Y = vector3I1.Y; vector3I3.Y <= vector3I2.Y; ++vector3I3.Y)
        {
          for (vector3I3.Z = vector3I1.Z; vector3I3.Z <= vector3I2.Z; ++vector3I3.Z)
          {
            if (planeBuild)
            {
              foreach (Vector3I gridIntersection in MyCubeBuilder.m_cacheGridIntersections)
              {
                if (gridIntersection.X == vector3I3.X || gridIntersection.Y == vector3I3.Y || gridIntersection.Z == vector3I3.Z)
                {
                  Vector3 zero1 = Vector3.Zero;
                  Vector3 zero2 = Vector3.Zero;
                  if (gridIntersection.X == vector3I3.X)
                  {
                    if (this.CurrentGrid != null)
                    {
                      Vector3 up = (Vector3) this.CurrentGrid.WorldMatrix.Up;
                      Vector3 forward = (Vector3) this.CurrentGrid.WorldMatrix.Forward;
                    }
                    else
                    {
                      Vector3 up = Vector3.Up;
                      Vector3 forward = Vector3.Forward;
                    }
                  }
                  else if (gridIntersection.Y == vector3I3.Y)
                  {
                    if (this.CurrentGrid != null)
                    {
                      Vector3 right = (Vector3) this.CurrentGrid.WorldMatrix.Right;
                      Vector3 forward = (Vector3) this.CurrentGrid.WorldMatrix.Forward;
                    }
                    else
                    {
                      Vector3 right = Vector3.Right;
                      Vector3 forward = Vector3.Forward;
                    }
                  }
                  else if (gridIntersection.Z == vector3I3.Z)
                  {
                    if (this.CurrentGrid != null)
                    {
                      Vector3 up = (Vector3) this.CurrentGrid.WorldMatrix.Up;
                      Vector3 right = (Vector3) this.CurrentGrid.WorldMatrix.Right;
                    }
                    else
                    {
                      Vector3 up = Vector3.Up;
                      Vector3 right = Vector3.Right;
                    }
                  }
                  Vector3I vector3I4 = Vector3I.Abs(gridIntersection - vector3I3) + Vector3I.One;
                  if (vector3I4.Size < 2048 && vector3I4.AbsMax() <= (int) byte.MaxValue)
                  {
                    continueBuild = new Vector3I?(gridIntersection);
                    break;
                  }
                }
              }
            }
            else
            {
              foreach (Vector3I gridIntersection in MyCubeBuilder.m_cacheGridIntersections)
              {
                if ((gridIntersection.X == vector3I3.X && gridIntersection.Y == vector3I3.Y || gridIntersection.Y == vector3I3.Y && gridIntersection.Z == vector3I3.Z || gridIntersection.X == vector3I3.X && gridIntersection.Z == vector3I3.Z) && (gridIntersection - vector3I3 + Vector3I.One).AbsMax() <= (int) byte.MaxValue)
                {
                  continueBuild = new Vector3I?(gridIntersection);
                  break;
                }
              }
            }
          }
        }
      }
    }

    public virtual void StopBuilding()
    {
      if (!this.GridAndBlockValid && !this.VoxelMapAndBlockValid)
      {
        foreach (MyCubeBuilderGizmo.MyGizmoSpaceProperties space in this.m_gizmo.Spaces)
        {
          space.m_startBuild = new Vector3I?();
          space.m_continueBuild = new Vector3I?();
          space.m_startRemove = new Vector3I?();
        }
      }
      else
      {
        bool smallViewChange = this.CheckSmallViewChange();
        this.m_blocksBuildQueue.Clear();
        this.m_tmpBlockPositionList.Clear();
        this.UpdateGizmos(true, true, false);
        int num = 0;
        foreach (MyCubeBuilderGizmo.MyGizmoSpaceProperties space in this.m_gizmo.Spaces)
        {
          if (space.Enabled)
            ++num;
        }
        foreach (MyCubeBuilderGizmo.MyGizmoSpaceProperties space in this.m_gizmo.Spaces)
        {
          if (space.Enabled)
            this.StopBuilding(smallViewChange, ref space.m_startBuild, ref space.m_startRemove, ref space.m_continueBuild, space.m_min, space.m_max, space.m_centerPos, ref space.m_localMatrixAdd, space.m_blockDefinition);
        }
        if (this.m_blocksBuildQueue.Count > 0)
        {
          this.CurrentGrid.BuildBlocks(MyPlayer.SelectedColor, MyStringHash.GetOrCompute(MyPlayer.SelectedArmorSkin), this.m_blocksBuildQueue, MySession.Static.LocalCharacterEntityId, MySession.Static.LocalPlayerId);
          this.m_blocksBuildQueue.Clear();
        }
        if (this.m_tmpBlockPositionList.Count <= 0)
          return;
        this.CurrentGrid.RazeBlocks(this.m_tmpBlockPositionList, MySession.Static.LocalCharacterEntityId);
        this.m_tmpBlockPositionList.Clear();
      }
    }

    protected void StopBuilding(
      bool smallViewChange,
      ref Vector3I? startBuild,
      ref Vector3I? startRemove,
      ref Vector3I? continueBuild,
      Vector3I blockMinPosition,
      Vector3I blockMaxPosition,
      Vector3I blockCenterPosition,
      ref Matrix localMatrixAdd,
      MyCubeBlockDefinition blockDefinition)
    {
      if (startBuild.HasValue && continueBuild.HasValue | smallViewChange)
      {
        Vector3I vec1 = blockMinPosition - blockCenterPosition;
        Vector3I vec2 = blockMaxPosition - blockCenterPosition;
        Vector3I result;
        Vector3I.TransformNormal(ref this.CurrentBlockDefinition.Size, ref localMatrixAdd, out result);
        Vector3I rotatedSize = Vector3I.Abs(result);
        if (smallViewChange)
          continueBuild = startBuild;
        Vector3I stepDelta;
        Vector3I counter;
        MyBlockBuilderBase.ComputeSteps(startBuild.Value, continueBuild.Value, rotatedSize, out stepDelta, out counter, out int _);
        Vector3I vector3I1 = blockCenterPosition;
        Quaternion fromRotationMatrix = Quaternion.CreateFromRotationMatrix(localMatrixAdd);
        MyDefinitionId id = blockDefinition.Id;
        if ((!blockDefinition.RandomRotation || blockDefinition.Size.X != blockDefinition.Size.Y || blockDefinition.Size.X != blockDefinition.Size.Z ? 0 : (blockDefinition.Rotation == MyBlockRotation.Both ? 1 : (blockDefinition.Rotation == MyBlockRotation.Vertical ? 1 : 0))) != 0)
        {
          this.m_blocksBuildQueue.Clear();
          Vector3I vector3I2;
          for (vector3I2.X = 0; vector3I2.X < counter.X; ++vector3I2.X)
          {
            for (vector3I2.Y = 0; vector3I2.Y < counter.Y; ++vector3I2.Y)
            {
              for (vector3I2.Z = 0; vector3I2.Z < counter.Z; ++vector3I2.Z)
              {
                Vector3I center = blockCenterPosition + vector3I2 * stepDelta;
                Vector3I min = blockMinPosition + vector3I2 * stepDelta;
                Vector3I max = blockMaxPosition + vector3I2 * stepDelta;
                Quaternion fromForwardUp;
                if (blockDefinition.Rotation == MyBlockRotation.Both)
                {
                  Base6Directions.Direction dir1 = (Base6Directions.Direction) (Math.Abs(MyRandom.Instance.Next()) % 6);
                  Base6Directions.Direction dir2 = dir1;
                  while (Vector3I.Dot(Base6Directions.GetIntVector(dir1), Base6Directions.GetIntVector(dir2)) != 0)
                    dir2 = (Base6Directions.Direction) (Math.Abs(MyRandom.Instance.Next()) % 6);
                  fromForwardUp = Quaternion.CreateFromForwardUp((Vector3) Base6Directions.GetIntVector(dir1), (Vector3) Base6Directions.GetIntVector(dir2));
                }
                else
                {
                  Base6Directions.Direction dir1 = Base6Directions.Direction.Up;
                  Base6Directions.Direction dir2 = dir1;
                  while (Vector3I.Dot(Base6Directions.GetIntVector(dir2), Base6Directions.GetIntVector(dir1)) != 0)
                    dir2 = (Base6Directions.Direction) (Math.Abs(MyRandom.Instance.Next()) % 6);
                  fromForwardUp = Quaternion.CreateFromForwardUp((Vector3) Base6Directions.GetIntVector(dir2), (Vector3) Base6Directions.GetIntVector(dir1));
                }
                this.m_blocksBuildQueue.Add(new MyCubeGrid.MyBlockLocation(blockDefinition.Id, min, max, center, fromForwardUp, MyEntityIdentifier.AllocateId(), MySession.Static.LocalPlayerId));
              }
            }
          }
          if (this.m_blocksBuildQueue.Count > 0)
            MyGuiAudio.PlaySound(MyGuiSounds.HudPlaceBlock);
        }
        else
        {
          this.CurrentGrid.BuildBlocks(ref new MyCubeGrid.MyBlockBuildArea()
          {
            PosInGrid = vector3I1,
            BlockMin = new Vector3B(vec1),
            BlockMax = new Vector3B(vec2),
            BuildAreaSize = new Vector3UByte(counter),
            StepDelta = new Vector3B(stepDelta),
            OrientationForward = Base6Directions.GetForward(ref fromRotationMatrix),
            OrientationUp = Base6Directions.GetUp(ref fromRotationMatrix),
            DefinitionId = (DefinitionIdBlit) id,
            ColorMaskHSV = MyPlayer.SelectedColor.PackHSVToUint(),
            SkinId = MyStringHash.GetOrCompute(MyPlayer.SelectedArmorSkin)
          }, MySession.Static.LocalCharacterEntityId, MySession.Static.LocalPlayerId);
          if (this.OnBlockAdded != null)
            this.OnBlockAdded(blockDefinition);
        }
      }
      else if (startRemove.HasValue && continueBuild.HasValue | smallViewChange)
      {
        MyGuiAudio.PlaySound(MyGuiSounds.HudDeleteBlock);
        Vector3I vector3I1 = startRemove.Value;
        Vector3I vector3I2 = startRemove.Value;
        if (smallViewChange)
          continueBuild = startRemove;
        MyBlockBuilderBase.ComputeSteps(startRemove.Value, continueBuild.Value, Vector3I.One, out Vector3I _, out Vector3I _, out int _);
        Vector3I pos = Vector3I.Min(startRemove.Value, continueBuild.Value);
        Vector3UByte size = new Vector3UByte(Vector3I.Max(startRemove.Value, continueBuild.Value) - pos);
        MyStringId context = !this.IsActivated || !(MySession.Static.ControlledEntity is MyCharacter) ? MyStringId.NullOrEmpty : MySession.Static.ControlledEntity.ControlContext;
        MyCharacterDetectorComponent detectorComponent = MySession.Static.LocalCharacter.Components.Get<MyCharacterDetectorComponent>();
        if (detectorComponent != null && detectorComponent.UseObject != null)
          detectorComponent.UseObject.SupportedActions.HasFlag((Enum) UseActionEnum.BuildPlanner);
        MyStringId buildPlanner = MyControlsSpace.BUILD_PLANNER;
        MyControllerHelper.IsControl(context, buildPlanner, MyControlStateType.NEW_RELEASED);
        if (true)
          this.CurrentGrid.RazeBlocksDelayed(ref pos, ref size, MySession.Static.LocalCharacterEntityId);
      }
      startBuild = new Vector3I?();
      continueBuild = new Vector3I?();
      startRemove = new Vector3I?();
    }

    protected virtual bool CancelBuilding()
    {
      if (!this.m_gizmo.SpaceDefault.m_continueBuild.HasValue)
        return false;
      this.m_gizmo.SpaceDefault.m_startBuild = new Vector3I?();
      this.m_gizmo.SpaceDefault.m_startRemove = new Vector3I?();
      this.m_gizmo.SpaceDefault.m_continueBuild = new Vector3I?();
      return true;
    }

    protected virtual bool IsBuilding() => this.m_gizmo.SpaceDefault.m_startBuild.HasValue || this.m_gizmo.SpaceDefault.m_startRemove.HasValue;

    protected bool CheckSmallViewChange()
    {
      double num1 = Vector3D.Dot(this.m_initialIntersectionDirection, MyBlockBuilderBase.IntersectionDirection);
      double num2 = (this.m_initialIntersectionStart - MyBlockBuilderBase.IntersectionStart).Length();
      return num1 > 0.998000025749207 && num2 < 0.25;
    }

    protected internal override void ChooseHitObject()
    {
      if (this.IsBuilding())
        return;
      base.ChooseHitObject();
      this.m_gizmo.Clear();
    }

    private Vector3D GetFreeSpacePlacementPosition(out bool valid)
    {
      valid = false;
      Vector3 halfExtents = this.CurrentBlockDefinition.Size * MyDefinitionManager.Static.GetCubeSize(this.CurrentBlockDefinition.CubeSize) * 0.5f;
      MatrixD worldMatrixAdd = this.m_gizmo.SpaceDefault.m_worldMatrixAdd;
      Vector3D intersectionStart = MyBlockBuilderBase.IntersectionStart;
      Vector3D freePlacementTarget = this.FreePlacementTarget;
      worldMatrixAdd.Translation = intersectionStart;
      HkShape shape = (HkShape) new HkBoxShape(halfExtents);
      double num1 = double.MaxValue;
      try
      {
        float? nullable = MyPhysics.CastShape(freePlacementTarget, shape, ref worldMatrixAdd, 30);
        if (nullable.HasValue)
        {
          if ((double) nullable.Value != 0.0)
          {
            num1 = (intersectionStart + (double) nullable.Value * (freePlacementTarget - intersectionStart) - MyBlockBuilderBase.IntersectionStart).Length() * 0.98;
            valid = true;
          }
        }
      }
      finally
      {
        shape.RemoveReference();
      }
      float num2 = this.LowLimitDistanceForDynamicMode();
      if (num1 < (double) num2)
      {
        num1 = (double) MyBlockBuilderBase.IntersectionDistance;
        valid = false;
      }
      if (num1 > (double) MyBlockBuilderBase.IntersectionDistance)
      {
        num1 = (double) MyBlockBuilderBase.IntersectionDistance;
        valid = false;
      }
      if (!MyEntities.IsInsideWorld(MyBlockBuilderBase.IntersectionStart + num1 * MyBlockBuilderBase.IntersectionDirection))
        valid = false;
      return MyBlockBuilderBase.IntersectionStart + num1 * MyBlockBuilderBase.IntersectionDirection;
    }

    private float LowLimitDistanceForDynamicMode() => this.CurrentBlockDefinition != null ? MyDefinitionManager.Static.GetCubeSize(this.CurrentBlockDefinition.CubeSize) + 0.1f : 2.6f;

    protected static void UpdateBlockInfoHud()
    {
      MyHud.BlockInfo.RemoveDisplayer(MyHudBlockInfo.WhoWantsInfoDisplayed.CubeBuilder);
      MyCubeBlockDefinition currentBlockDefinition = MyCubeBuilder.Static.CurrentBlockDefinition;
      if (currentBlockDefinition == null || !MyCubeBuilder.Static.IsActivated || !MyFakes.ENABLE_SMALL_GRID_BLOCK_INFO && currentBlockDefinition != null && currentBlockDefinition.CubeSize == MyCubeSize.Small)
        return;
      if (MyHud.BlockInfo.DefinitionId != currentBlockDefinition.Id)
        MySlimBlock.SetBlockComponents(MyHud.BlockInfo, currentBlockDefinition, MyCubeBuilder.BuildComponent.GetBuilderInventory((MyEntity) MySession.Static.LocalCharacter));
      MyHud.BlockInfo.ChangeDisplayer(MyHudBlockInfo.WhoWantsInfoDisplayed.CubeBuilder, MyCubeBuilder.Static.IsBuildToolActive());
    }

    public void StartStaticGridPlacement(MyCubeSize cubeSize, bool isStatic)
    {
      MySession.Static.LocalCharacter?.SwitchToWeapon((MyToolbarItemWeapon) null);
      MyCubeBlockDefinition blockDefinition;
      if (!MyDefinitionManager.Static.TryGetCubeBlockDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_CubeBlock), "LargeBlockArmorBlock"), out blockDefinition))
        return;
      this.Activate(new MyDefinitionId?(blockDefinition.Id));
      this.m_stationPlacement = true;
    }

    protected static MyObjectBuilder_CubeGrid CreateMultiBlockGridBuilder(
      MyMultiBlockDefinition multiCubeBlockDefinition,
      Matrix rotationMatrix,
      Vector3D position = default (Vector3D))
    {
      MyObjectBuilder_CubeGrid newObject1 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_CubeGrid>();
      newObject1.PositionAndOrientation = new MyPositionAndOrientation?(new MyPositionAndOrientation(position, rotationMatrix.Forward, rotationMatrix.Up));
      newObject1.IsStatic = false;
      newObject1.PersistentFlags |= MyPersistentEntityFlags2.Enabled | MyPersistentEntityFlags2.InScene;
      if (multiCubeBlockDefinition.BlockDefinitions == null)
        return (MyObjectBuilder_CubeGrid) null;
      MyCubeSize? nullable = new MyCubeSize?();
      Vector3I vector3I1 = Vector3I.MaxValue;
      Vector3I vector3I2 = Vector3I.MinValue;
      int num = MyRandom.Instance.Next();
      while (num == 0)
        num = MyRandom.Instance.Next();
      for (int index = 0; index < multiCubeBlockDefinition.BlockDefinitions.Length; ++index)
      {
        MyMultiBlockDefinition.MyMultiBlockPartDefinition blockDefinition1 = multiCubeBlockDefinition.BlockDefinitions[index];
        MyCubeBlockDefinition blockDefinition2;
        MyDefinitionManager.Static.TryGetCubeBlockDefinition(blockDefinition1.Id, out blockDefinition2);
        if (blockDefinition2 != null)
        {
          if (!nullable.HasValue)
            nullable = new MyCubeSize?(blockDefinition2.CubeSize);
          else if (nullable.Value != blockDefinition2.CubeSize)
            continue;
          MyObjectBuilder_CubeBlock newObject2 = MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) blockDefinition2.Id) as MyObjectBuilder_CubeBlock;
          newObject2.Orientation = (SerializableQuaternion) Base6Directions.GetOrientation(blockDefinition1.Forward, blockDefinition1.Up);
          newObject2.Min = (SerializableVector3I) blockDefinition1.Min;
          newObject2.ColorMaskHSV = (SerializableVector3) MyPlayer.SelectedColor;
          newObject2.SkinSubtypeId = MyPlayer.SelectedArmorSkin;
          newObject2.MultiBlockId = num;
          newObject2.MultiBlockIndex = index;
          newObject2.MultiBlockDefinition = new SerializableDefinitionId?((SerializableDefinitionId) multiCubeBlockDefinition.Id);
          newObject2.EntityId = MyEntityIdentifier.AllocateId();
          bool flag1 = false;
          bool flag2 = true;
          bool flag3 = MyCompoundCubeBlock.IsCompoundEnabled(blockDefinition2);
          foreach (MyObjectBuilder_CubeBlock cubeBlock in newObject1.CubeBlocks)
          {
            if (cubeBlock.Min == newObject2.Min)
            {
              if (MyFakes.ENABLE_COMPOUND_BLOCKS && cubeBlock is MyObjectBuilder_CompoundCubeBlock)
              {
                if (flag3)
                {
                  MyObjectBuilder_CompoundCubeBlock compoundCubeBlock = cubeBlock as MyObjectBuilder_CompoundCubeBlock;
                  MyObjectBuilder_CubeBlock[] builderCubeBlockArray = new MyObjectBuilder_CubeBlock[compoundCubeBlock.Blocks.Length + 1];
                  Array.Copy((Array) compoundCubeBlock.Blocks, (Array) builderCubeBlockArray, compoundCubeBlock.Blocks.Length);
                  builderCubeBlockArray[builderCubeBlockArray.Length - 1] = newObject2;
                  compoundCubeBlock.Blocks = builderCubeBlockArray;
                  flag1 = true;
                  break;
                }
                flag2 = false;
                break;
              }
              flag2 = false;
              break;
            }
          }
          if (flag2)
          {
            if (!flag1)
            {
              if (MyFakes.ENABLE_COMPOUND_BLOCKS && MyCompoundCubeBlock.IsCompoundEnabled(blockDefinition2))
              {
                MyObjectBuilder_CompoundCubeBlock builder = MyCompoundCubeBlock.CreateBuilder(newObject2);
                newObject1.CubeBlocks.Add((MyObjectBuilder_CubeBlock) builder);
              }
              else
                newObject1.CubeBlocks.Add(newObject2);
            }
            vector3I1 = Vector3I.Min(vector3I1, blockDefinition1.Min);
            vector3I2 = Vector3I.Max(vector3I2, blockDefinition1.Min);
          }
        }
      }
      if (newObject1.CubeBlocks.Count == 0)
        return (MyObjectBuilder_CubeGrid) null;
      newObject1.GridSizeEnum = nullable.Value;
      return newObject1;
    }

    protected static void AfterGridBuild(
      MyEntity builder,
      MyCubeGrid grid,
      bool instantBuild,
      ulong senderId)
    {
      if (grid == null || grid.Closed)
      {
        MyCubeBuilder.SpawnGridReply(false, senderId);
      }
      else
      {
        MySlimBlock cubeBlock = grid.GetCubeBlock(Vector3I.Zero);
        if (cubeBlock == null)
          return;
        if (grid.IsStatic)
        {
          MySlimBlock mySlimBlock = !(cubeBlock.FatBlock is MyCompoundCubeBlock fatBlock) || fatBlock.GetBlocksCount() <= 0 ? (MySlimBlock) null : fatBlock.GetBlocks()[0];
          MyCubeGrid myCubeGrid = grid.DetectMerge(cubeBlock, newGrid: true) ?? grid;
          if (mySlimBlock != null)
            myCubeGrid.GetCubeBlock(mySlimBlock.Position);
          if (MyCubeGridSmallToLargeConnection.Static != null && Sync.IsServer && (!MyCubeGridSmallToLargeConnection.Static.AddBlockSmallToLargeConnection(cubeBlock) && grid.GridSizeEnum == MyCubeSize.Small))
            cubeBlock.CubeGrid.TestDynamic = MyCubeGrid.MyTestDynamicReason.GridCopied;
        }
        if (Sync.IsServer)
          MyCubeBuilder.BuildComponent.AfterSuccessfulBuild(builder, instantBuild);
        if (cubeBlock.FatBlock != null)
          cubeBlock.FatBlock.OnBuildSuccess(cubeBlock.BuiltBy, instantBuild);
        if (grid.IsStatic && grid.GridSizeEnum != MyCubeSize.Small)
        {
          MatrixD worldMatrix = grid.WorldMatrix;
          if (MyCoordinateSystem.Static.IsLocalCoordSysExist(ref worldMatrix, (double) grid.GridSize))
            MyCoordinateSystem.Static.RegisterCubeGrid(grid);
          else
            MyCoordinateSystem.Static.CreateCoordSys(grid, MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.StaticGridAlignToCenter, true);
        }
        MyCubeGrids.NotifyBlockBuilt(grid, cubeBlock);
        MyCubeBuilder.SpawnGridReply(true, senderId);
      }
    }

    public static MyCubeGrid SpawnStaticGrid(
      MyCubeBlockDefinition blockDefinition,
      MyEntity builder,
      MatrixD worldMatrix,
      Vector3 color,
      MyStringHash skinId,
      MyCubeBuilder.SpawnFlags spawnFlags = MyCubeBuilder.SpawnFlags.Default,
      long builtBy = 0,
      Action<MyEntity> completionCallback = null)
    {
      MyObjectBuilder_CubeGrid newObject1 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_CubeGrid>();
      Vector3 vector3 = Vector3.TransformNormal(MyCubeBlock.GetBlockGridOffset(blockDefinition), worldMatrix);
      newObject1.PositionAndOrientation = new MyPositionAndOrientation?(new MyPositionAndOrientation(worldMatrix.Translation - vector3, (Vector3) worldMatrix.Forward, (Vector3) worldMatrix.Up));
      newObject1.GridSizeEnum = blockDefinition.CubeSize;
      newObject1.IsStatic = true;
      newObject1.CreatePhysics = (uint) (spawnFlags & MyCubeBuilder.SpawnFlags.CreatePhysics) > 0U;
      newObject1.EnableSmallToLargeConnections = (uint) (spawnFlags & MyCubeBuilder.SpawnFlags.EnableSmallTolargeConnections) > 0U;
      newObject1.PersistentFlags |= MyPersistentEntityFlags2.Enabled | MyPersistentEntityFlags2.InScene;
      if ((spawnFlags & MyCubeBuilder.SpawnFlags.AddToScene) != MyCubeBuilder.SpawnFlags.None)
        newObject1.EntityId = MyEntityIdentifier.AllocateId();
      MyObjectBuilder_CubeBlock newObject2 = MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) blockDefinition.Id) as MyObjectBuilder_CubeBlock;
      newObject2.Orientation = (SerializableQuaternion) Quaternion.CreateFromForwardUp((Vector3) Vector3I.Forward, (Vector3) Vector3I.Up);
      newObject2.Min = (SerializableVector3I) (blockDefinition.Size / 2 - blockDefinition.Size + Vector3I.One);
      if ((spawnFlags & MyCubeBuilder.SpawnFlags.AddToScene) != MyCubeBuilder.SpawnFlags.None)
        newObject2.EntityId = MyEntityIdentifier.AllocateId();
      newObject2.ColorMaskHSV = (SerializableVector3) color;
      newObject2.SkinSubtypeId = skinId.String;
      newObject2.BuiltBy = builtBy;
      newObject2.Owner = builtBy;
      MyCubeBuilder.BuildComponent.BeforeCreateBlock(blockDefinition, builder, newObject2, (uint) (spawnFlags & MyCubeBuilder.SpawnFlags.SpawnAsMaster) > 0U);
      newObject1.CubeBlocks.Add(newObject2);
      return (spawnFlags & MyCubeBuilder.SpawnFlags.AddToScene) == MyCubeBuilder.SpawnFlags.None ? MyEntities.CreateFromObjectBuilderParallel((MyObjectBuilder_EntityBase) newObject1, completionCallback: completionCallback, checkPosition: true) as MyCubeGrid : MyEntities.CreateFromObjectBuilderParallel((MyObjectBuilder_EntityBase) newObject1, true, completionCallback, checkPosition: true) as MyCubeGrid;
    }

    public static MySlimBlock SpawnStaticGrid_nonParalel(
      MyCubeBlockDefinition blockDefinition,
      MyEntity builder,
      MatrixD worldMatrix,
      Vector3 color,
      MyStringHash skinId,
      MyCubeBuilder.SpawnFlags spawnFlags = MyCubeBuilder.SpawnFlags.Default,
      long builtBy = 0)
    {
      MyObjectBuilder_CubeGrid newObject1 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_CubeGrid>();
      Vector3 vector3 = Vector3.TransformNormal(MyCubeBlock.GetBlockGridOffset(blockDefinition), worldMatrix);
      newObject1.PositionAndOrientation = new MyPositionAndOrientation?(new MyPositionAndOrientation(worldMatrix.Translation - vector3, (Vector3) worldMatrix.Forward, (Vector3) worldMatrix.Up));
      newObject1.GridSizeEnum = blockDefinition.CubeSize;
      newObject1.IsStatic = true;
      newObject1.CreatePhysics = (uint) (spawnFlags & MyCubeBuilder.SpawnFlags.CreatePhysics) > 0U;
      newObject1.EnableSmallToLargeConnections = (uint) (spawnFlags & MyCubeBuilder.SpawnFlags.EnableSmallTolargeConnections) > 0U;
      newObject1.PersistentFlags |= MyPersistentEntityFlags2.Enabled | MyPersistentEntityFlags2.InScene;
      if ((spawnFlags & MyCubeBuilder.SpawnFlags.AddToScene) != MyCubeBuilder.SpawnFlags.None)
        newObject1.EntityId = MyEntityIdentifier.AllocateId();
      MyObjectBuilder_CubeBlock newObject2 = MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) blockDefinition.Id) as MyObjectBuilder_CubeBlock;
      newObject2.Orientation = (SerializableQuaternion) Quaternion.CreateFromForwardUp((Vector3) Vector3I.Forward, (Vector3) Vector3I.Up);
      newObject2.Min = (SerializableVector3I) (blockDefinition.Size / 2 - blockDefinition.Size + Vector3I.One);
      if ((spawnFlags & MyCubeBuilder.SpawnFlags.AddToScene) != MyCubeBuilder.SpawnFlags.None)
        newObject2.EntityId = MyEntityIdentifier.AllocateId();
      newObject2.ColorMaskHSV = (SerializableVector3) color;
      newObject2.SkinSubtypeId = skinId.String;
      newObject2.BuiltBy = builtBy;
      newObject2.Owner = builtBy;
      MyCubeBuilder.BuildComponent.BeforeCreateBlock(blockDefinition, builder, newObject2, (uint) (spawnFlags & MyCubeBuilder.SpawnFlags.SpawnAsMaster) > 0U);
      newObject1.CubeBlocks.Add(newObject2);
      return (MyEntities.CreateFromObjectBuilderAndAdd((MyObjectBuilder_EntityBase) newObject1, false) as MyCubeGrid).GetBlocks().First<MySlimBlock>();
    }

    public static MyCubeGrid SpawnDynamicGrid(
      MyCubeBlockDefinition blockDefinition,
      MyEntity builder,
      MatrixD worldMatrix,
      Vector3 color,
      MyStringHash skinId,
      long entityId = 0,
      MyCubeBuilder.SpawnFlags spawnFlags = MyCubeBuilder.SpawnFlags.Default,
      long builtBy = 0,
      Action<MyEntity> completionCallback = null)
    {
      MyObjectBuilder_CubeGrid newObject1 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_CubeGrid>();
      Vector3 vector3 = Vector3.TransformNormal(MyCubeBlock.GetBlockGridOffset(blockDefinition), worldMatrix);
      Vector3D? relativeOffset = new Vector3D?(worldMatrix.Translation - vector3 - builder.WorldMatrix.Translation);
      newObject1.PositionAndOrientation = new MyPositionAndOrientation?(new MyPositionAndOrientation(worldMatrix.Translation - vector3, (Vector3) worldMatrix.Forward, (Vector3) worldMatrix.Up));
      newObject1.GridSizeEnum = blockDefinition.CubeSize;
      newObject1.IsStatic = false;
      newObject1.PersistentFlags |= MyPersistentEntityFlags2.Enabled | MyPersistentEntityFlags2.InScene;
      MyObjectBuilder_CubeBlock newObject2 = MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) blockDefinition.Id) as MyObjectBuilder_CubeBlock;
      newObject2.Orientation = (SerializableQuaternion) Quaternion.CreateFromForwardUp((Vector3) Vector3I.Forward, (Vector3) Vector3I.Up);
      newObject2.Min = (SerializableVector3I) (blockDefinition.Size / 2 - blockDefinition.Size + Vector3I.One);
      newObject2.ColorMaskHSV = (SerializableVector3) color;
      newObject2.SkinSubtypeId = skinId.String;
      newObject2.BuiltBy = builtBy;
      newObject2.Owner = builtBy;
      MyCubeBuilder.BuildComponent.BeforeCreateBlock(blockDefinition, builder, newObject2, (uint) (spawnFlags & MyCubeBuilder.SpawnFlags.SpawnAsMaster) > 0U);
      newObject1.CubeBlocks.Add(newObject2);
      MyCubeGrid myCubeGrid = (MyCubeGrid) null;
      if (builder != null)
      {
        MyEntity myEntity = builder.Parent == null ? builder : (builder.Parent is MyCubeBlock ? (MyEntity) ((MyCubeBlock) builder.Parent).CubeGrid : builder.Parent);
        if (myEntity.Physics != null && (double) myEntity.Physics.LinearVelocity.LengthSquared() >= 225.0)
          newObject1.LinearVelocity = (SerializableVector3) myEntity.Physics.LinearVelocity;
      }
      if (entityId != 0L)
      {
        newObject1.EntityId = entityId;
        newObject2.EntityId = entityId + 1L;
        myCubeGrid = MyEntities.CreateFromObjectBuilderParallel((MyObjectBuilder_EntityBase) newObject1, true, completionCallback, checkPosition: true) as MyCubeGrid;
      }
      else if (Sync.IsServer)
      {
        newObject1.EntityId = MyEntityIdentifier.AllocateId();
        newObject2.EntityId = newObject1.EntityId + 1L;
        myCubeGrid = MyEntities.CreateFromObjectBuilderParallel((MyObjectBuilder_EntityBase) newObject1, true, completionCallback, relativeSpawner: builder, relativeOffset: relativeOffset, checkPosition: true) as MyCubeGrid;
      }
      return myCubeGrid;
    }

    public static void SelectBlockToToolbar(MySlimBlock block, bool selectToNextSlot = true)
    {
      MyDefinitionId myDefinitionId = block.BlockDefinition.Id;
      if (block.FatBlock is MyCompoundCubeBlock)
      {
        MyCompoundCubeBlock fatBlock = block.FatBlock as MyCompoundCubeBlock;
        MyCubeBuilder.m_cycle %= fatBlock.GetBlocksCount();
        myDefinitionId = fatBlock.GetBlocks()[MyCubeBuilder.m_cycle].BlockDefinition.Id;
        ++MyCubeBuilder.m_cycle;
      }
      if (block.FatBlock is MyFracturedBlock)
      {
        MyFracturedBlock fatBlock = block.FatBlock as MyFracturedBlock;
        MyCubeBuilder.m_cycle %= fatBlock.OriginalBlocks.Count;
        myDefinitionId = fatBlock.OriginalBlocks[MyCubeBuilder.m_cycle];
        ++MyCubeBuilder.m_cycle;
      }
      int? selectedSlot = MyToolbarComponent.CurrentToolbar.SelectedSlot;
      if (selectedSlot.HasValue)
      {
        selectedSlot = MyToolbarComponent.CurrentToolbar.SelectedSlot;
        int slot = selectedSlot.Value;
        if (selectToNextSlot)
          ++slot;
        if (!MyToolbarComponent.CurrentToolbar.IsValidSlot(slot))
          slot = 0;
        MyObjectBuilder_ToolbarItemCubeBlock toolbarItemCubeBlock = new MyObjectBuilder_ToolbarItemCubeBlock();
        toolbarItemCubeBlock.DefinitionId = (SerializableDefinitionId) myDefinitionId;
        MyToolbarItem toolbarItem = MyToolbarItemFactory.CreateToolbarItem((MyObjectBuilder_ToolbarItem) toolbarItemCubeBlock);
        MyToolbarComponent.CurrentToolbar.SetItemAtSlot(slot, toolbarItem);
      }
      else
      {
        int slot = 0;
        while (MyToolbarComponent.CurrentToolbar.GetSlotItem(slot) != null)
          ++slot;
        if (!MyToolbarComponent.CurrentToolbar.IsValidSlot(slot))
          slot = 0;
        MyObjectBuilder_ToolbarItemCubeBlock toolbarItemCubeBlock = new MyObjectBuilder_ToolbarItemCubeBlock();
        toolbarItemCubeBlock.DefinitionId = (SerializableDefinitionId) myDefinitionId;
        MyToolbarItem toolbarItem = MyToolbarItemFactory.CreateToolbarItem((MyObjectBuilder_ToolbarItem) toolbarItemCubeBlock);
        MyToolbarComponent.CurrentToolbar.SetItemAtSlot(slot, toolbarItem);
      }
    }

    private void BeforeCurrentGridChange(MyCubeGrid newCurrentGrid) => this.TriggerRespawnShipNotification(newCurrentGrid);

    private void TriggerRespawnShipNotification(MyCubeGrid newCurrentGrid)
    {
      MyNotificationSingletons singleNotification = MySession.Static.Settings.RespawnShipDelete ? MyNotificationSingletons.RespawnShipWarning : MyNotificationSingletons.BuildingOnRespawnShipWarning;
      if (newCurrentGrid != null && newCurrentGrid.IsRespawnGrid)
        MyHud.Notifications.Add(singleNotification);
      else
        MyHud.Notifications.Remove(singleNotification);
    }

    public static double? GetCurrentRayIntersection()
    {
      MyPhysics.HitInfo? nullable = MyPhysics.CastRay(MyBlockBuilderBase.IntersectionStart, MyBlockBuilderBase.IntersectionStart + 2000.0 * MyBlockBuilderBase.IntersectionDirection, 30);
      return nullable.HasValue ? new double?((nullable.Value.Position - MyBlockBuilderBase.IntersectionStart).Length()) : new double?();
    }

    public static Vector3 TransformLargeGridHitCoordToSmallGrid(
      Vector3D coords,
      MatrixD worldMatrixNormalizedInv,
      float gridSize)
    {
      Vector3D vector3D1 = Vector3D.Transform(coords, worldMatrixNormalizedInv) / (double) gridSize * 10.0;
      Vector3I vector3I = Vector3I.Sign((Vector3) vector3D1);
      Vector3D vector3D2 = vector3D1 - 0.5 * vector3I;
      return (Vector3) (((Vector3D) (vector3I * Vector3I.Round(Vector3D.Abs(vector3D2))) + 0.5 * vector3I) / 10.0);
    }

    public static MyObjectBuilder_CubeGrid ConvertGridBuilderToStatic(
      MyObjectBuilder_CubeGrid originalGrid,
      MatrixD worldMatrix)
    {
      MyObjectBuilder_CubeGrid newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_CubeGrid>();
      newObject.EntityId = originalGrid.EntityId;
      newObject.PositionAndOrientation = new MyPositionAndOrientation?(new MyPositionAndOrientation(worldMatrix.Translation, (Vector3) worldMatrix.Forward, (Vector3) worldMatrix.Up));
      newObject.GridSizeEnum = originalGrid.GridSizeEnum;
      newObject.IsStatic = true;
      newObject.PersistentFlags |= MyPersistentEntityFlags2.Enabled | MyPersistentEntityFlags2.InScene;
      foreach (MyObjectBuilder_CubeBlock cubeBlock in originalGrid.CubeBlocks)
      {
        if (cubeBlock is MyObjectBuilder_CompoundCubeBlock)
        {
          MyObjectBuilder_CompoundCubeBlock compoundCubeBlock1 = cubeBlock as MyObjectBuilder_CompoundCubeBlock;
          if (MyCubeBuilder.ConvertDynamicGridBlockToStatic(ref worldMatrix, cubeBlock) is MyObjectBuilder_CompoundCubeBlock compoundCubeBlock)
          {
            compoundCubeBlock.Blocks = new MyObjectBuilder_CubeBlock[compoundCubeBlock1.Blocks.Length];
            for (int index = 0; index < compoundCubeBlock1.Blocks.Length; ++index)
            {
              MyObjectBuilder_CubeBlock block = compoundCubeBlock1.Blocks[index];
              MyObjectBuilder_CubeBlock builderCubeBlock = MyCubeBuilder.ConvertDynamicGridBlockToStatic(ref worldMatrix, block);
              if (builderCubeBlock != null)
                compoundCubeBlock.Blocks[index] = builderCubeBlock;
            }
            newObject.CubeBlocks.Add((MyObjectBuilder_CubeBlock) compoundCubeBlock);
          }
        }
        else
        {
          MyObjectBuilder_CubeBlock builderCubeBlock = MyCubeBuilder.ConvertDynamicGridBlockToStatic(ref worldMatrix, cubeBlock);
          if (builderCubeBlock != null)
            newObject.CubeBlocks.Add(builderCubeBlock);
        }
      }
      return newObject;
    }

    public static MyObjectBuilder_CubeBlock ConvertDynamicGridBlockToStatic(
      ref MatrixD worldMatrix,
      MyObjectBuilder_CubeBlock origBlock)
    {
      MyDefinitionId defId = new MyDefinitionId(origBlock.TypeId, origBlock.SubtypeName);
      MyCubeBlockDefinition blockDefinition;
      MyDefinitionManager.Static.TryGetCubeBlockDefinition(defId, out blockDefinition);
      if (blockDefinition == null)
        return (MyObjectBuilder_CubeBlock) null;
      MyObjectBuilder_CubeBlock newObject = MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) defId) as MyObjectBuilder_CubeBlock;
      newObject.EntityId = origBlock.EntityId;
      Quaternion result;
      ((MyBlockOrientation) origBlock.BlockOrientation).GetQuaternion(out result);
      Matrix fromQuaternion = Matrix.CreateFromQuaternion(result);
      MatrixD matrixD = fromQuaternion * worldMatrix;
      Matrix matrix = (Matrix) ref matrixD;
      newObject.Orientation = (SerializableQuaternion) Quaternion.CreateFromRotationMatrix(fromQuaternion);
      Vector3I vector3I1 = Vector3I.Abs(Vector3I.Round(Vector3.TransformNormal((Vector3) blockDefinition.Size, fromQuaternion)));
      Vector3I min = (Vector3I) origBlock.Min;
      Vector3I vector3I2 = (Vector3I) origBlock.Min + vector3I1 - Vector3I.One;
      Vector3I.Round(Vector3.TransformNormal((Vector3) min, worldMatrix));
      Vector3I.Round(Vector3.TransformNormal((Vector3) vector3I2, worldMatrix));
      newObject.Min = (SerializableVector3I) Vector3I.Min(min, vector3I2);
      newObject.MultiBlockId = origBlock.MultiBlockId;
      newObject.MultiBlockDefinition = origBlock.MultiBlockDefinition;
      newObject.MultiBlockIndex = origBlock.MultiBlockIndex;
      newObject.BuildPercent = origBlock.BuildPercent;
      newObject.IntegrityPercent = origBlock.BuildPercent;
      return newObject;
    }

    public static void GetAllBlocksPositions(
      HashSet<Tuple<MySlimBlock, ushort?>> blockInCompoundIDs,
      HashSet<Vector3I> outPositions)
    {
      foreach (Tuple<MySlimBlock, ushort?> blockInCompoundId in blockInCompoundIDs)
      {
        Vector3I next = blockInCompoundId.Item1.Min;
        Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref blockInCompoundId.Item1.Min, ref blockInCompoundId.Item1.Max);
        while (vector3IRangeIterator.IsValid())
        {
          outPositions.Add(next);
          vector3IRangeIterator.GetNext(out next);
        }
      }
    }

    [Event(null, 5280)]
    [Reliable]
    [Server]
    private static void RequestGridSpawn(
      MyCubeBuilder.Author author,
      DefinitionIdBlit definition,
      MyCubeBuilder.BuildData position,
      bool instantBuild,
      bool forceStatic,
      MyCubeGrid.MyBlockVisuals visuals)
    {
      MyEntity builder;
      MyEntities.TryGetEntityById(author.EntityId, out builder);
      bool flag = MyEventContext.Current.IsLocallyInvoked || MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value) || MySession.Static.CreativeToolsEnabled(Sync.MyId);
      if (builder == null || instantBuild && !flag || !MySession.Static.GetComponent<MySessionComponentDLC>().HasDefinitionDLC((MyDefinitionId) definition, MyEventContext.Current.Sender.Value) || MySession.Static.ResearchEnabled && !flag && !MySessionComponentResearch.Static.CanUse(author.IdentityId, (MyDefinitionId) definition))
      {
        if (!(MyMultiplayer.Static is MyMultiplayerServerBase multiplayerServerBase))
          return;
        // ISSUE: explicit non-virtual call
        __nonvirtual (multiplayerServerBase.ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true));
      }
      else
      {
        MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition((MyDefinitionId) definition);
        Vector3D position1 = position.Position;
        if (!position.AbsolutePosition)
          position1 += builder.PositionComp.GetPosition();
        MatrixD world = MatrixD.CreateWorld(position1, position.Forward, position.Up);
        if (!MyEntities.IsInsideWorld(world.Translation))
          return;
        float cubeSize = MyDefinitionManager.Static.GetCubeSize(cubeBlockDefinition.CubeSize);
        BoundingBoxD localAabb = new BoundingBoxD((Vector3D) (-cubeBlockDefinition.Size * cubeSize * 0.5f), (Vector3D) (cubeBlockDefinition.Size * cubeSize * 0.5f));
        if (!MySessionComponentSafeZones.IsActionAllowed(localAabb.TransformFast(ref world), MySafeZoneAction.Building, builder.EntityId, MyEventContext.Current.Sender.Value))
          return;
        MyGridPlacementSettings placementSettings1 = MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.GetGridPlacementSettings(cubeBlockDefinition.CubeSize);
        VoxelPlacementSettings placementSettings2 = new VoxelPlacementSettings()
        {
          PlacementMode = VoxelPlacementMode.OutsideVoxel
        };
        placementSettings1.VoxelPlacement = new VoxelPlacementSettings?(placementSettings2);
        bool isStatic = forceStatic || MyCubeGrid.IsAabbInsideVoxel(world, localAabb, placementSettings1) || MyCubeBuilder.Static.m_stationPlacement;
        MyCubeBuilder.BuildComponent.GetGridSpawnMaterials(cubeBlockDefinition, world, isStatic);
        bool canSpawn = flag & instantBuild || MyCubeBuilder.BuildComponent.HasBuildingMaterials(builder);
        ulong senderId = MyEventContext.Current.Sender.Value;
        if (!canSpawn)
        {
          MyCubeBuilder.SpawnGridReply(canSpawn, senderId);
        }
        else
        {
          MyCubeBuilder.SpawnFlags spawnFlags = MyCubeBuilder.SpawnFlags.Default;
          if (flag & instantBuild)
            spawnFlags |= MyCubeBuilder.SpawnFlags.SpawnAsMaster;
          Vector3 color = ColorExtensions.UnpackHSVFromUint(visuals.ColorMaskHSV);
          MySessionComponentGameInventory component = MySession.Static.GetComponent<MySessionComponentGameInventory>();
          MyStringHash skinId = component != null ? component.ValidateArmor(visuals.SkinId, senderId) : MyStringHash.NullOrEmpty;
          if (isStatic)
            MyCubeBuilder.SpawnStaticGrid(cubeBlockDefinition, builder, world, color, skinId, spawnFlags, author.IdentityId, (Action<MyEntity>) (grid => MyCubeBuilder.AfterGridBuild(builder, grid as MyCubeGrid, instantBuild, senderId)));
          else
            MyCubeBuilder.SpawnDynamicGrid(cubeBlockDefinition, builder, world, color, skinId, spawnFlags: spawnFlags, builtBy: author.IdentityId, completionCallback: ((Action<MyEntity>) (grid => MyCubeBuilder.AfterGridBuild(builder, grid as MyCubeGrid, instantBuild, senderId))));
        }
      }
    }

    private static void SpawnGridReply(bool canSpawn, ulong senderId)
    {
      if (senderId == 0UL)
        MyCubeBuilder.SpawnGridReply(canSpawn);
      else
        MyMultiplayer.RaiseStaticEvent<bool>((Func<IMyEventOwner, Action<bool>>) (s => new Action<bool>(MyCubeBuilder.SpawnGridReply)), canSpawn, new EndpointId(senderId));
    }

    [Event(null, 5371)]
    [Reliable]
    [Client]
    private static void SpawnGridReply(bool success)
    {
      if (success)
        MyGuiAudio.PlaySound(MyGuiSounds.HudPlaceBlock);
      else
        MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
    }

    public static void RemovePlayerColors(MyPlayer.PlayerId playerId)
    {
      if (!Sync.IsServer)
        return;
      MyCubeBuilder.RemovePlayerColors_Internal(playerId);
    }

    public static void RemovePlayerColors_Internal(MyPlayer.PlayerId playerId)
    {
      if (!MyCubeBuilder.AllPlayersColors.ContainsKey(playerId))
        return;
      MyCubeBuilder.AllPlayersColors.Remove(playerId);
    }

    bool IMyCubeBuilder.AddConstruction(IMyEntity buildingEntity) => false;

    IMyCubeGrid IMyCubeBuilder.FindClosestGrid() => (IMyCubeGrid) this.FindClosestGrid();

    void IMyCubeBuilder.Activate(MyDefinitionId? blockDefinitionId = null) => this.Activate(blockDefinitionId);

    bool IMyCubeBuilder.BlockCreationIsActivated => this.BlockCreationIsActivated;

    void IMyCubeBuilder.Deactivate() => this.Deactivate();

    void IMyCubeBuilder.DeactivateBlockCreation() => this.DeactivateBlockCreation();

    bool IMyCubeBuilder.FreezeGizmo
    {
      get => this.FreezeGizmo;
      set => this.FreezeGizmo = value;
    }

    bool IMyCubeBuilder.ShowRemoveGizmo
    {
      get => this.ShowRemoveGizmo;
      set => this.ShowRemoveGizmo = value;
    }

    void IMyCubeBuilder.StartNewGridPlacement(MyCubeSize cubeSize, bool isStatic) => this.StartStaticGridPlacement(cubeSize, isStatic);

    bool IMyCubeBuilder.UseSymmetry
    {
      get => this.UseSymmetry;
      set => this.UseSymmetry = value;
    }

    bool IMyCubeBuilder.UseTransparency
    {
      get => this.UseTransparency;
      set => this.UseTransparency = value;
    }

    bool IMyCubeBuilder.IsActivated => this.IsActivated;

    [Serializable]
    private struct BuildData
    {
      public Vector3D Position;
      public Vector3 Forward;
      public Vector3 Up;
      public bool AbsolutePosition;

      protected class Sandbox_Game_Entities_MyCubeBuilder\u003C\u003EBuildData\u003C\u003EPosition\u003C\u003EAccessor : IMemberAccessor<MyCubeBuilder.BuildData, Vector3D>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeBuilder.BuildData owner, in Vector3D value) => owner.Position = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeBuilder.BuildData owner, out Vector3D value) => value = owner.Position;
      }

      protected class Sandbox_Game_Entities_MyCubeBuilder\u003C\u003EBuildData\u003C\u003EForward\u003C\u003EAccessor : IMemberAccessor<MyCubeBuilder.BuildData, Vector3>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeBuilder.BuildData owner, in Vector3 value) => owner.Forward = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeBuilder.BuildData owner, out Vector3 value) => value = owner.Forward;
      }

      protected class Sandbox_Game_Entities_MyCubeBuilder\u003C\u003EBuildData\u003C\u003EUp\u003C\u003EAccessor : IMemberAccessor<MyCubeBuilder.BuildData, Vector3>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeBuilder.BuildData owner, in Vector3 value) => owner.Up = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeBuilder.BuildData owner, out Vector3 value) => value = owner.Up;
      }

      protected class Sandbox_Game_Entities_MyCubeBuilder\u003C\u003EBuildData\u003C\u003EAbsolutePosition\u003C\u003EAccessor : IMemberAccessor<MyCubeBuilder.BuildData, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeBuilder.BuildData owner, in bool value) => owner.AbsolutePosition = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeBuilder.BuildData owner, out bool value) => value = owner.AbsolutePosition;
      }
    }

    [Flags]
    public enum SpawnFlags : ushort
    {
      None = 0,
      AddToScene = 1,
      CreatePhysics = 2,
      EnableSmallTolargeConnections = 4,
      SpawnAsMaster = 8,
      Default = EnableSmallTolargeConnections | CreatePhysics | AddToScene, // 0x0007
    }

    [Serializable]
    private struct Author
    {
      public long EntityId;
      public long IdentityId;

      public Author(long entityId, long identityId)
      {
        this.EntityId = entityId;
        this.IdentityId = identityId;
      }

      protected class Sandbox_Game_Entities_MyCubeBuilder\u003C\u003EAuthor\u003C\u003EEntityId\u003C\u003EAccessor : IMemberAccessor<MyCubeBuilder.Author, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeBuilder.Author owner, in long value) => owner.EntityId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeBuilder.Author owner, out long value) => value = owner.EntityId;
      }

      protected class Sandbox_Game_Entities_MyCubeBuilder\u003C\u003EAuthor\u003C\u003EIdentityId\u003C\u003EAccessor : IMemberAccessor<MyCubeBuilder.Author, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCubeBuilder.Author owner, in long value) => owner.IdentityId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCubeBuilder.Author owner, out long value) => value = owner.IdentityId;
      }
    }

    public enum BuildingModeEnum
    {
      SingleBlock,
      Line,
      Plane,
    }

    public enum CubePlacementModeEnum
    {
      LocalCoordinateSystem,
      FreePlacement,
      GravityAligned,
    }

    private struct MyColoringArea
    {
      public Vector3I Start;
      public Vector3I End;
    }

    protected sealed class RequestGridSpawn\u003C\u003ESandbox_Game_Entities_MyCubeBuilder\u003C\u003EAuthor\u0023VRage_Game_DefinitionIdBlit\u0023Sandbox_Game_Entities_MyCubeBuilder\u003C\u003EBuildData\u0023System_Boolean\u0023System_Boolean\u0023Sandbox_Game_Entities_MyCubeGrid\u003C\u003EMyBlockVisuals : ICallSite<IMyEventOwner, MyCubeBuilder.Author, DefinitionIdBlit, MyCubeBuilder.BuildData, bool, bool, MyCubeGrid.MyBlockVisuals>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyCubeBuilder.Author author,
        in DefinitionIdBlit definition,
        in MyCubeBuilder.BuildData position,
        in bool instantBuild,
        in bool forceStatic,
        in MyCubeGrid.MyBlockVisuals visuals)
      {
        MyCubeBuilder.RequestGridSpawn(author, definition, position, instantBuild, forceStatic, visuals);
      }
    }

    protected sealed class SpawnGridReply\u003C\u003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in bool success,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyCubeBuilder.SpawnGridReply(success);
      }
    }
  }
}
