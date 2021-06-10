// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyCubeBuilderGizmo
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Screens.DebugScreens;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Models;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
  public class MyCubeBuilderGizmo
  {
    private MyCubeBuilderGizmo.MyGizmoSpaceProperties[] m_spaces = new MyCubeBuilderGizmo.MyGizmoSpaceProperties[8];
    public MyRotationOptionsEnum RotationOptions;
    public static MySymmetryAxisEnum CurrentBlockMirrorAxis;
    public static MySymmetryAxisEnum CurrentBlockMirrorOption;

    public MyCubeBuilderGizmo.MyGizmoSpaceProperties SpaceDefault => this.m_spaces[0];

    public MyCubeBuilderGizmo.MyGizmoSpaceProperties[] Spaces => this.m_spaces;

    public MyCubeBuilderGizmo()
    {
      for (int index = 0; index < 8; ++index)
        this.m_spaces[index] = new MyCubeBuilderGizmo.MyGizmoSpaceProperties();
      this.m_spaces[0].Enabled = true;
      this.m_spaces[1].SourceSpace = MyGizmoSpaceEnum.Default;
      this.m_spaces[1].SymmetryPlane = MySymmetrySettingModeEnum.NoPlane;
      this.m_spaces[1].SourceSpace = MyGizmoSpaceEnum.Default;
      this.m_spaces[1].SymmetryPlane = MySymmetrySettingModeEnum.XPlane;
      this.m_spaces[2].SourceSpace = MyGizmoSpaceEnum.Default;
      this.m_spaces[2].SymmetryPlane = MySymmetrySettingModeEnum.YPlane;
      this.m_spaces[3].SourceSpace = MyGizmoSpaceEnum.Default;
      this.m_spaces[3].SymmetryPlane = MySymmetrySettingModeEnum.ZPlane;
      this.m_spaces[4].SourceSpace = MyGizmoSpaceEnum.SymmetryX;
      this.m_spaces[4].SymmetryPlane = MySymmetrySettingModeEnum.YPlane;
      this.m_spaces[5].SourceSpace = MyGizmoSpaceEnum.SymmetryY;
      this.m_spaces[5].SymmetryPlane = MySymmetrySettingModeEnum.ZPlane;
      this.m_spaces[6].SourceSpace = MyGizmoSpaceEnum.SymmetryX;
      this.m_spaces[6].SymmetryPlane = MySymmetrySettingModeEnum.ZPlane;
      this.m_spaces[7].SourceSpace = MyGizmoSpaceEnum.SymmetryXZ;
      this.m_spaces[7].SymmetryPlane = MySymmetrySettingModeEnum.YPlane;
    }

    public void Clear()
    {
      foreach (MyCubeBuilderGizmo.MyGizmoSpaceProperties space in this.m_spaces)
        space.Clear();
    }

    public void RotateAxis(ref MatrixD rotatedMatrix)
    {
      this.SpaceDefault.m_localMatrixAdd = (Matrix) ref rotatedMatrix;
      this.SpaceDefault.m_localMatrixAdd.Forward = (Vector3) Vector3I.Round(this.SpaceDefault.m_localMatrixAdd.Forward);
      this.SpaceDefault.m_localMatrixAdd.Up = (Vector3) Vector3I.Round(this.SpaceDefault.m_localMatrixAdd.Up);
      this.SpaceDefault.m_localMatrixAdd.Right = (Vector3) Vector3I.Round(this.SpaceDefault.m_localMatrixAdd.Right);
    }

    public void SetupLocalAddMatrix(
      MyCubeBuilderGizmo.MyGizmoSpaceProperties gizmoSpace,
      Vector3I normal)
    {
      Vector3I vector3I1 = -normal;
      Matrix world1 = Matrix.CreateWorld(Vector3.Zero, (Vector3) vector3I1, (Vector3) Vector3I.Shift(vector3I1));
      Matrix matrix = Matrix.Invert(world1);
      Vector3I vector3I2 = Vector3I.Round((world1 * gizmoSpace.m_localMatrixAdd).Up);
      if (vector3I2 == gizmoSpace.m_addDir || vector3I2 == -gizmoSpace.m_addDir)
        vector3I2 = Vector3I.Shift(vector3I2);
      Matrix world2 = Matrix.CreateWorld(Vector3.Zero, (Vector3) gizmoSpace.m_addDir, (Vector3) vector3I2);
      gizmoSpace.m_localMatrixAdd = matrix * world2;
    }

    public void UpdateGizmoCubeParts(
      MyCubeBuilderGizmo.MyGizmoSpaceProperties gizmoSpace,
      MyBlockBuilderRenderData renderData,
      ref MatrixD invGridWorldMatrix,
      MyCubeBlockDefinition definition = null)
    {
      this.RemoveGizmoCubeParts(gizmoSpace);
      this.AddGizmoCubeParts(gizmoSpace, renderData, ref invGridWorldMatrix, definition);
    }

    private void AddGizmoCubeParts(
      MyCubeBuilderGizmo.MyGizmoSpaceProperties gizmoSpace,
      MyBlockBuilderRenderData renderData,
      ref MatrixD invGridWorldMatrix,
      MyCubeBlockDefinition definition)
    {
      Vector3UByte[] vector3UbyteArray1 = (Vector3UByte[]) null;
      MyTileDefinition[] myTileDefinitionArray = (MyTileDefinition[]) null;
      MatrixD orientation = invGridWorldMatrix.GetOrientation();
      float range = 1f;
      if (definition != null && definition.Skeleton != null)
      {
        myTileDefinitionArray = MyCubeGridDefinitions.GetCubeTiles(definition);
        range = MyDefinitionManager.Static.GetCubeSize(definition.CubeSize);
      }
      for (int index1 = 0; index1 < gizmoSpace.m_cubeModelsTemp.Count; ++index1)
      {
        string str = gizmoSpace.m_cubeModelsTemp[index1];
        gizmoSpace.m_cubeModels.Add(str);
        gizmoSpace.m_cubeMatrices.Add(gizmoSpace.m_cubeMatricesTemp[index1]);
        if (myTileDefinitionArray != null)
        {
          int index2 = index1 % myTileDefinitionArray.Length;
          MatrixD matrix = Matrix.Transpose(myTileDefinitionArray[index2].LocalMatrix) * gizmoSpace.m_cubeMatricesTemp[index1].GetOrientation() * orientation;
          vector3UbyteArray1 = new Vector3UByte[9];
          for (int index3 = 0; index3 < 9; ++index3)
            vector3UbyteArray1[index3] = new Vector3UByte((byte) 128, (byte) 128, (byte) 128);
          MyModel model = MyModels.GetModel(str);
          if (model.BoneMapping != null)
          {
            for (int index3 = 0; index3 < Math.Min(model.BoneMapping.Length, 9); ++index3)
            {
              Vector3I vector3I = Vector3I.Round(Vector3.Transform((Vector3) model.BoneMapping[index3] - Vector3.One, myTileDefinitionArray[index2].LocalMatrix) + Vector3.One);
              for (int index4 = 0; index4 < definition.Skeleton.Count; ++index4)
              {
                VRage.Game.BoneInfo boneInfo = definition.Skeleton[index4];
                if (boneInfo.BonePosition == (SerializableVector3I) vector3I)
                {
                  Vector3 vec = (Vector3) Vector3.Transform(Vector3UByte.Denormalize((Vector3UByte) boneInfo.BoneOffset, range), matrix);
                  vector3UbyteArray1[index3] = Vector3UByte.Normalize(vec, range);
                  break;
                }
              }
            }
          }
        }
        MyBlockBuilderRenderData builderRenderData = renderData;
        int id = MyModel.GetId(str);
        MatrixD matrix1 = gizmoSpace.m_cubeMatricesTemp[index1];
        ref MatrixD local = ref invGridWorldMatrix;
        Vector3UByte[] vector3UbyteArray2 = vector3UbyteArray1;
        float num1 = range;
        Vector3 selectedColor = MyPlayer.SelectedColor;
        MyStringHash? skinId = new MyStringHash?(MyStringHash.GetOrCompute(MyPlayer.SelectedArmorSkin));
        Vector3UByte[] bones = vector3UbyteArray2;
        double num2 = (double) num1;
        builderRenderData.AddInstance(id, matrix1, ref local, selectedColor, skinId, bones, (float) num2);
      }
    }

    public void RemoveGizmoCubeParts()
    {
      foreach (MyCubeBuilderGizmo.MyGizmoSpaceProperties space in this.m_spaces)
        this.RemoveGizmoCubeParts(space);
    }

    private void RemoveGizmoCubeParts(
      MyCubeBuilderGizmo.MyGizmoSpaceProperties gizmoSpace)
    {
      gizmoSpace.m_cubeMatrices.Clear();
      gizmoSpace.m_cubeModels.Clear();
    }

    public void AddFastBuildParts(
      MyCubeBuilderGizmo.MyGizmoSpaceProperties gizmoSpace,
      MyCubeBlockDefinition cubeBlockDefinition,
      MyCubeGrid grid)
    {
      if (cubeBlockDefinition == null || !gizmoSpace.m_startBuild.HasValue || !gizmoSpace.m_continueBuild.HasValue)
        return;
      Vector3I vector3I1 = Vector3I.Min(gizmoSpace.m_startBuild.Value, gizmoSpace.m_continueBuild.Value);
      Vector3I vector3I2 = Vector3I.Max(gizmoSpace.m_startBuild.Value, gizmoSpace.m_continueBuild.Value);
      Vector3I vector3I3 = new Vector3I();
      int count = gizmoSpace.m_cubeMatricesTemp.Count;
      for (vector3I3.X = vector3I1.X; vector3I3.X <= vector3I2.X; vector3I3.X += cubeBlockDefinition.Size.X)
      {
        for (vector3I3.Y = vector3I1.Y; vector3I3.Y <= vector3I2.Y; vector3I3.Y += cubeBlockDefinition.Size.Y)
        {
          for (vector3I3.Z = vector3I1.Z; vector3I3.Z <= vector3I2.Z; vector3I3.Z += cubeBlockDefinition.Size.Z)
          {
            if (!((Vector3) (vector3I3 - gizmoSpace.m_startBuild.Value) == Vector3.Zero))
            {
              Vector3D vector3D = grid != null ? Vector3D.Transform(vector3I3 * grid.GridSize, grid.WorldMatrix) : (Vector3D) vector3I3 * (double) MyDefinitionManager.Static.GetCubeSize(cubeBlockDefinition.CubeSize);
              for (int index = 0; index < count; ++index)
              {
                gizmoSpace.m_cubeModelsTemp.Add(gizmoSpace.m_cubeModelsTemp[index]);
                MatrixD matrixD = gizmoSpace.m_cubeMatricesTemp[index];
                matrixD.Translation = vector3D;
                gizmoSpace.m_cubeMatricesTemp.Add(matrixD);
              }
            }
          }
        }
      }
    }

    public static bool DefaultGizmoCloseEnough(
      ref MatrixD invGridWorldMatrix,
      BoundingBoxD gizmoBox,
      float gridSize,
      float intersectionDistance)
    {
      MatrixD matrix = invGridWorldMatrix;
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      if (localCharacter == null)
        return false;
      Vector3D translation = localCharacter.GetHeadMatrix(true, true, false, false, false).Translation;
      Vector3D position = MySector.MainCamera.Position;
      Vector3 forwardVector = MySector.MainCamera.ForwardVector;
      double num1 = (translation - MySector.MainCamera.Position).Length();
      Vector3 vector3 = (Vector3) Vector3D.Transform(translation, matrix);
      LineD line = new LineD((Vector3D) (Vector3) Vector3D.Transform(position, matrix), (Vector3D) (Vector3) Vector3D.Transform(position + forwardVector * (intersectionDistance + (float) num1), matrix));
      float num2 = 0.025f * gridSize;
      gizmoBox.Inflate((double) num2);
      double distance = double.MaxValue;
      if (!gizmoBox.Intersects(ref line, out distance))
        return false;
      double num3 = gizmoBox.Distance((Vector3D) vector3);
      return MySession.Static.ControlledEntity is MyShipController ? (MyCubeBuilder.Static.CubeBuilderState.CurrentBlockDefinition.CubeSize == MyCubeSize.Large ? num3 <= MyBlockBuilderBase.CubeBuilderDefinition.BuildingDistLargeSurvivalShip : num3 <= MyBlockBuilderBase.CubeBuilderDefinition.BuildingDistSmallSurvivalShip) : (MyCubeBuilder.Static.CubeBuilderState.CurrentBlockDefinition.CubeSize == MyCubeSize.Large ? num3 <= MyBlockBuilderBase.CubeBuilderDefinition.BuildingDistLargeSurvivalCharacter : num3 <= MyBlockBuilderBase.CubeBuilderDefinition.BuildingDistSmallSurvivalCharacter);
    }

    private void GetGizmoPointTestVariables(
      ref MatrixD invGridWorldMatrix,
      float gridSize,
      out BoundingBoxD bb,
      out MatrixD m,
      MyGizmoSpaceEnum gizmo,
      float inflate = 0.0f,
      bool onVoxel = false,
      bool dynamicMode = false)
    {
      m = invGridWorldMatrix * MatrixD.CreateScale(1.0 / (double) gridSize);
      MyCubeBuilderGizmo.MyGizmoSpaceProperties space = this.m_spaces[(int) gizmo];
      if (dynamicMode)
      {
        m = invGridWorldMatrix;
        bb = new BoundingBoxD((Vector3D) (-space.m_blockDefinition.Size * gridSize * 0.5f), (Vector3D) (space.m_blockDefinition.Size * gridSize * 0.5f));
      }
      else if (onVoxel)
      {
        m = invGridWorldMatrix;
        Vector3D vector3D1 = MyCubeGrid.StaticGlobalGrid_UGToWorld((Vector3D) space.m_min, gridSize, MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.StaticGridAlignToCenter) - Vector3D.Half * (double) gridSize;
        Vector3D vector3D2 = MyCubeGrid.StaticGlobalGrid_UGToWorld((Vector3D) space.m_max, gridSize, MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.StaticGridAlignToCenter) + Vector3D.Half * (double) gridSize;
        bb = new BoundingBoxD(vector3D1 - new Vector3D((double) inflate * (double) gridSize), vector3D2 + new Vector3D((double) inflate * (double) gridSize));
      }
      else if (MyFakes.ENABLE_STATIC_SMALL_GRID_ON_LARGE && space.m_addPosSmallOnLarge.HasValue)
      {
        float num = MyDefinitionManager.Static.GetCubeSize(space.m_blockDefinition.CubeSize) / gridSize;
        Vector3 vector3_1 = space.m_minSmallOnLarge - new Vector3(0.5f * num);
        Vector3 vector3_2 = space.m_maxSmallOnLarge + new Vector3(0.5f * num);
        bb = new BoundingBoxD((Vector3D) (vector3_1 - new Vector3(inflate)), (Vector3D) (vector3_2 + new Vector3(inflate)));
      }
      else
      {
        Vector3 vector3_1 = (Vector3) space.m_min - new Vector3(0.5f);
        Vector3 vector3_2 = (Vector3) space.m_max + new Vector3(0.5f);
        bb = new BoundingBoxD((Vector3D) (vector3_1 - new Vector3(inflate)), (Vector3D) (vector3_2 + new Vector3(inflate)));
      }
    }

    public bool PointsAABBIntersectsGizmo(
      List<Vector3D> points,
      MyGizmoSpaceEnum gizmo,
      ref MatrixD invGridWorldMatrix,
      float gridSize,
      float inflate = 0.0f,
      bool onVoxel = false,
      bool dynamicMode = false)
    {
      MatrixD m = new MatrixD();
      BoundingBoxD bb = new BoundingBoxD();
      this.GetGizmoPointTestVariables(ref invGridWorldMatrix, gridSize, out bb, out m, gizmo, inflate, onVoxel, dynamicMode);
      BoundingBoxD invalid = BoundingBoxD.CreateInvalid();
      foreach (Vector3D point1 in points)
      {
        Vector3D point2 = Vector3D.Transform(point1, m);
        if (bb.Contains(point2) == ContainmentType.Contains)
          return true;
        invalid.Include(point2);
      }
      return invalid.Intersects(ref bb);
    }

    public bool PointInsideGizmo(
      Vector3D point,
      MyGizmoSpaceEnum gizmo,
      ref MatrixD invGridWorldMatrix,
      float gridSize,
      float inflate = 0.0f,
      bool onVoxel = false,
      bool dynamicMode = false)
    {
      MatrixD m = new MatrixD();
      BoundingBoxD bb = new BoundingBoxD();
      this.GetGizmoPointTestVariables(ref invGridWorldMatrix, gridSize, out bb, out m, gizmo, inflate, onVoxel, dynamicMode);
      Vector3D point1 = Vector3D.Transform(point, m);
      return bb.Contains(point1) == ContainmentType.Contains;
    }

    private void EnableGizmoSpace(
      MyGizmoSpaceEnum gizmoSpaceEnum,
      bool enable,
      Vector3I? planePos,
      bool isOdd,
      MyCubeBlockDefinition cubeBlockDefinition,
      MyCubeGrid cubeGrid)
    {
      MyCubeBuilderGizmo.MyGizmoSpaceProperties space = this.m_spaces[(int) gizmoSpaceEnum];
      space.Enabled = enable;
      if (!enable)
        return;
      if (planePos.HasValue)
        space.SymmetryPlanePos = planePos.Value;
      space.SymmetryIsOdd = isOdd;
      space.m_buildAllowed = false;
      if (cubeBlockDefinition != null)
      {
        Quaternion localOrientation = space.LocalOrientation;
        MyBlockOrientation blockOrientation = new MyBlockOrientation(ref localOrientation);
        Vector3I size;
        MyCubeGridDefinitions.GetRotatedBlockSize(cubeBlockDefinition, ref space.m_localMatrixAdd, out size);
        Vector3I center = cubeBlockDefinition.Center;
        Vector3I result1;
        Vector3I.TransformNormal(ref center, ref space.m_localMatrixAdd, out result1);
        Vector3I vector3I1 = new Vector3I(Math.Sign(size.X) == Math.Sign(space.m_addDir.X) ? result1.X : Math.Sign(space.m_addDir.X) * (Math.Abs(size.X) - Math.Abs(result1.X) - 1), Math.Sign(size.Y) == Math.Sign(space.m_addDir.Y) ? result1.Y : Math.Sign(space.m_addDir.Y) * (Math.Abs(size.Y) - Math.Abs(result1.Y) - 1), Math.Sign(size.Z) == Math.Sign(space.m_addDir.Z) ? result1.Z : Math.Sign(space.m_addDir.Z) * (Math.Abs(size.Z) - Math.Abs(result1.Z) - 1));
        space.m_positions.Clear();
        space.m_positionsSmallOnLarge.Clear();
        if (MyFakes.ENABLE_STATIC_SMALL_GRID_ON_LARGE && space.m_addPosSmallOnLarge.HasValue)
        {
          float num = MyDefinitionManager.Static.GetCubeSize(cubeBlockDefinition.CubeSize) / cubeGrid.GridSize;
          space.m_minSmallOnLarge = Vector3.MaxValue;
          space.m_maxSmallOnLarge = Vector3.MinValue;
          space.m_centerPosSmallOnLarge = space.m_addPosSmallOnLarge.Value + num * vector3I1;
          space.m_buildAllowed = true;
          Vector3I vector3I2 = new Vector3I();
          for (vector3I2.X = 0; vector3I2.X < cubeBlockDefinition.Size.X; ++vector3I2.X)
          {
            for (vector3I2.Y = 0; vector3I2.Y < cubeBlockDefinition.Size.Y; ++vector3I2.Y)
            {
              for (vector3I2.Z = 0; vector3I2.Z < cubeBlockDefinition.Size.Z; ++vector3I2.Z)
              {
                Vector3I normal = vector3I2 - center;
                Vector3I result2;
                Vector3I.TransformNormal(ref normal, ref space.m_localMatrixAdd, out result2);
                Vector3 vector3 = space.m_addPosSmallOnLarge.Value + num * (result2 + vector3I1);
                space.m_minSmallOnLarge = Vector3.Min(vector3, space.m_minSmallOnLarge);
                space.m_maxSmallOnLarge = Vector3.Max(vector3, space.m_maxSmallOnLarge);
                space.m_positionsSmallOnLarge.Add(vector3);
              }
            }
          }
        }
        else
        {
          space.m_min = Vector3I.MaxValue;
          space.m_max = Vector3I.MinValue;
          space.m_centerPos = space.m_addPos + vector3I1;
          space.m_buildAllowed = true;
          Vector3I vector3I2 = new Vector3I();
          for (vector3I2.X = 0; vector3I2.X < cubeBlockDefinition.Size.X; ++vector3I2.X)
          {
            for (vector3I2.Y = 0; vector3I2.Y < cubeBlockDefinition.Size.Y; ++vector3I2.Y)
            {
              for (vector3I2.Z = 0; vector3I2.Z < cubeBlockDefinition.Size.Z; ++vector3I2.Z)
              {
                Vector3I normal = vector3I2 - center;
                Vector3I result2;
                Vector3I.TransformNormal(ref normal, ref space.m_localMatrixAdd, out result2);
                Vector3I pos = space.m_addPos + result2 + vector3I1;
                space.m_min = Vector3I.Min(pos, space.m_min);
                space.m_max = Vector3I.Max(pos, space.m_max);
                if (cubeGrid != null && cubeBlockDefinition.CubeSize == cubeGrid.GridSizeEnum && !cubeGrid.CanAddCube(pos, new MyBlockOrientation?(blockOrientation), cubeBlockDefinition))
                  space.m_buildAllowed = false;
                space.m_positions.Add(pos);
              }
            }
          }
        }
      }
      if (space.SymmetryPlane == MySymmetrySettingModeEnum.Disabled)
        return;
      this.MirrorGizmoSpace(space, this.m_spaces[(int) space.SourceSpace], space.SymmetryPlane, planePos.Value, isOdd, cubeBlockDefinition, cubeGrid);
    }

    public void EnableGizmoSpaces(
      MyCubeBlockDefinition cubeBlockDefinition,
      MyCubeGrid cubeGrid,
      bool useSymmetry)
    {
      this.EnableGizmoSpace(MyGizmoSpaceEnum.Default, true, new Vector3I?(), false, cubeBlockDefinition, cubeGrid);
      if (cubeGrid != null)
      {
        this.EnableGizmoSpace(MyGizmoSpaceEnum.SymmetryX, useSymmetry && cubeGrid.XSymmetryPlane.HasValue, cubeGrid.XSymmetryPlane, cubeGrid.XSymmetryOdd, cubeBlockDefinition, cubeGrid);
        this.EnableGizmoSpace(MyGizmoSpaceEnum.SymmetryY, useSymmetry && cubeGrid.YSymmetryPlane.HasValue, cubeGrid.YSymmetryPlane, cubeGrid.YSymmetryOdd, cubeBlockDefinition, cubeGrid);
        this.EnableGizmoSpace(MyGizmoSpaceEnum.SymmetryZ, useSymmetry && cubeGrid.ZSymmetryPlane.HasValue, cubeGrid.ZSymmetryPlane, cubeGrid.ZSymmetryOdd, cubeBlockDefinition, cubeGrid);
        this.EnableGizmoSpace(MyGizmoSpaceEnum.SymmetryXY, useSymmetry && cubeGrid.XSymmetryPlane.HasValue && cubeGrid.YSymmetryPlane.HasValue, cubeGrid.YSymmetryPlane, cubeGrid.YSymmetryOdd, cubeBlockDefinition, cubeGrid);
        this.EnableGizmoSpace(MyGizmoSpaceEnum.SymmetryYZ, useSymmetry && cubeGrid.YSymmetryPlane.HasValue && cubeGrid.ZSymmetryPlane.HasValue, cubeGrid.ZSymmetryPlane, cubeGrid.ZSymmetryOdd, cubeBlockDefinition, cubeGrid);
        this.EnableGizmoSpace(MyGizmoSpaceEnum.SymmetryXZ, useSymmetry && cubeGrid.XSymmetryPlane.HasValue && cubeGrid.ZSymmetryPlane.HasValue, cubeGrid.ZSymmetryPlane, cubeGrid.ZSymmetryOdd, cubeBlockDefinition, cubeGrid);
        this.EnableGizmoSpace(MyGizmoSpaceEnum.SymmetryXYZ, useSymmetry && cubeGrid.XSymmetryPlane.HasValue && cubeGrid.YSymmetryPlane.HasValue && cubeGrid.ZSymmetryPlane.HasValue, cubeGrid.YSymmetryPlane, cubeGrid.YSymmetryOdd, cubeBlockDefinition, cubeGrid);
      }
      else
      {
        this.EnableGizmoSpace(MyGizmoSpaceEnum.SymmetryX, false, new Vector3I?(), false, cubeBlockDefinition, (MyCubeGrid) null);
        this.EnableGizmoSpace(MyGizmoSpaceEnum.SymmetryY, false, new Vector3I?(), false, cubeBlockDefinition, (MyCubeGrid) null);
        this.EnableGizmoSpace(MyGizmoSpaceEnum.SymmetryZ, false, new Vector3I?(), false, cubeBlockDefinition, (MyCubeGrid) null);
        this.EnableGizmoSpace(MyGizmoSpaceEnum.SymmetryXY, false, new Vector3I?(), false, cubeBlockDefinition, (MyCubeGrid) null);
        this.EnableGizmoSpace(MyGizmoSpaceEnum.SymmetryYZ, false, new Vector3I?(), false, cubeBlockDefinition, (MyCubeGrid) null);
        this.EnableGizmoSpace(MyGizmoSpaceEnum.SymmetryXZ, false, new Vector3I?(), false, cubeBlockDefinition, (MyCubeGrid) null);
        this.EnableGizmoSpace(MyGizmoSpaceEnum.SymmetryXYZ, false, new Vector3I?(), false, cubeBlockDefinition, (MyCubeGrid) null);
      }
    }

    private static Vector3I MirrorBlockByPlane(
      MySymmetrySettingModeEnum mirror,
      Vector3I mirrorPosition,
      bool isOdd,
      Vector3I sourcePosition)
    {
      Vector3I vector3I = sourcePosition;
      if (mirror == MySymmetrySettingModeEnum.XPlane)
      {
        vector3I = new Vector3I(mirrorPosition.X - (sourcePosition.X - mirrorPosition.X), sourcePosition.Y, sourcePosition.Z);
        if (isOdd)
          --vector3I.X;
      }
      if (mirror == MySymmetrySettingModeEnum.YPlane)
      {
        vector3I = new Vector3I(sourcePosition.X, mirrorPosition.Y - (sourcePosition.Y - mirrorPosition.Y), sourcePosition.Z);
        if (isOdd)
          --vector3I.Y;
      }
      if (mirror == MySymmetrySettingModeEnum.ZPlane)
      {
        vector3I = new Vector3I(sourcePosition.X, sourcePosition.Y, mirrorPosition.Z - (sourcePosition.Z - mirrorPosition.Z));
        if (isOdd)
          ++vector3I.Z;
      }
      return vector3I;
    }

    private static Vector3I MirrorDirByPlane(
      MySymmetrySettingModeEnum mirror,
      Vector3I mirrorDir,
      bool isOdd,
      Vector3I sourceDir)
    {
      Vector3I vector3I = sourceDir;
      if (mirror == MySymmetrySettingModeEnum.XPlane)
        vector3I = new Vector3I(-sourceDir.X, sourceDir.Y, sourceDir.Z);
      if (mirror == MySymmetrySettingModeEnum.YPlane)
        vector3I = new Vector3I(sourceDir.X, -sourceDir.Y, sourceDir.Z);
      if (mirror == MySymmetrySettingModeEnum.ZPlane)
        vector3I = new Vector3I(sourceDir.X, sourceDir.Y, -sourceDir.Z);
      return vector3I;
    }

    private void MirrorGizmoSpace(
      MyCubeBuilderGizmo.MyGizmoSpaceProperties targetSpace,
      MyCubeBuilderGizmo.MyGizmoSpaceProperties sourceSpace,
      MySymmetrySettingModeEnum mirrorPlane,
      Vector3I mirrorPosition,
      bool isOdd,
      MyCubeBlockDefinition cubeBlockDefinition,
      MyCubeGrid cubeGrid)
    {
      targetSpace.m_addPos = MyCubeBuilderGizmo.MirrorBlockByPlane(mirrorPlane, mirrorPosition, isOdd, sourceSpace.m_addPos);
      targetSpace.m_localMatrixAdd.Translation = (Vector3) targetSpace.m_addPos;
      targetSpace.m_addDir = MyCubeBuilderGizmo.MirrorDirByPlane(mirrorPlane, mirrorPosition, isOdd, sourceSpace.m_addDir);
      targetSpace.m_removePos = MyCubeBuilderGizmo.MirrorBlockByPlane(mirrorPlane, mirrorPosition, isOdd, sourceSpace.m_removePos);
      targetSpace.m_removeBlock = cubeGrid.GetCubeBlock(targetSpace.m_removePos);
      targetSpace.m_startBuild = !sourceSpace.m_startBuild.HasValue ? new Vector3I?() : new Vector3I?(MyCubeBuilderGizmo.MirrorBlockByPlane(mirrorPlane, mirrorPosition, isOdd, sourceSpace.m_startBuild.Value));
      targetSpace.m_continueBuild = !sourceSpace.m_continueBuild.HasValue ? new Vector3I?() : new Vector3I?(MyCubeBuilderGizmo.MirrorBlockByPlane(mirrorPlane, mirrorPosition, isOdd, sourceSpace.m_continueBuild.Value));
      targetSpace.m_startRemove = !sourceSpace.m_startRemove.HasValue ? new Vector3I?() : new Vector3I?(MyCubeBuilderGizmo.MirrorBlockByPlane(mirrorPlane, mirrorPosition, isOdd, sourceSpace.m_startRemove.Value));
      Vector3 vector2_1 = Vector3.Zero;
      switch (mirrorPlane)
      {
        case MySymmetrySettingModeEnum.XPlane:
          vector2_1 = Vector3.Right;
          break;
        case MySymmetrySettingModeEnum.YPlane:
          vector2_1 = Vector3.Up;
          break;
        case MySymmetrySettingModeEnum.ZPlane:
          vector2_1 = Vector3.Forward;
          break;
      }
      MyCubeBuilderGizmo.CurrentBlockMirrorAxis = MySymmetryAxisEnum.None;
      if (MyUtils.IsZero(Math.Abs(Vector3.Dot(sourceSpace.m_localMatrixAdd.Right, vector2_1)) - 1f, 1E-05f))
        MyCubeBuilderGizmo.CurrentBlockMirrorAxis = MySymmetryAxisEnum.X;
      else if (MyUtils.IsZero(Math.Abs(Vector3.Dot(sourceSpace.m_localMatrixAdd.Up, vector2_1)) - 1f, 1E-05f))
        MyCubeBuilderGizmo.CurrentBlockMirrorAxis = MySymmetryAxisEnum.Y;
      else if (MyUtils.IsZero(Math.Abs(Vector3.Dot(sourceSpace.m_localMatrixAdd.Forward, vector2_1)) - 1f, 1E-05f))
        MyCubeBuilderGizmo.CurrentBlockMirrorAxis = MySymmetryAxisEnum.Z;
      MyCubeBuilderGizmo.CurrentBlockMirrorOption = MySymmetryAxisEnum.None;
      MySymmetryAxisEnum symmetryAxisEnum1 = MyGuiScreenDebugCubeBlocks.DebugXMirroringAxis.HasValue ? MyGuiScreenDebugCubeBlocks.DebugXMirroringAxis.Value : cubeBlockDefinition.SymmetryX;
      MySymmetryAxisEnum symmetryAxisEnum2 = MyGuiScreenDebugCubeBlocks.DebugYMirroringAxis.HasValue ? MyGuiScreenDebugCubeBlocks.DebugYMirroringAxis.Value : cubeBlockDefinition.SymmetryY;
      MySymmetryAxisEnum symmetryAxisEnum3 = MyGuiScreenDebugCubeBlocks.DebugZMirroringAxis.HasValue ? MyGuiScreenDebugCubeBlocks.DebugZMirroringAxis.Value : cubeBlockDefinition.SymmetryZ;
      switch (MyCubeBuilderGizmo.CurrentBlockMirrorAxis)
      {
        case MySymmetryAxisEnum.X:
          MyCubeBuilderGizmo.CurrentBlockMirrorOption = symmetryAxisEnum1;
          break;
        case MySymmetryAxisEnum.Y:
          MyCubeBuilderGizmo.CurrentBlockMirrorOption = symmetryAxisEnum2;
          break;
        case MySymmetryAxisEnum.Z:
          MyCubeBuilderGizmo.CurrentBlockMirrorOption = symmetryAxisEnum3;
          break;
      }
      switch (MyCubeBuilderGizmo.CurrentBlockMirrorOption)
      {
        case MySymmetryAxisEnum.X:
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationX(3.141593f) * sourceSpace.m_localMatrixAdd;
          break;
        case MySymmetryAxisEnum.Y:
        case MySymmetryAxisEnum.YThenOffsetX:
        case MySymmetryAxisEnum.YThenOffsetXOdd:
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationY(3.141593f) * sourceSpace.m_localMatrixAdd;
          break;
        case MySymmetryAxisEnum.Z:
        case MySymmetryAxisEnum.ZThenOffsetX:
        case MySymmetryAxisEnum.ZThenOffsetXOdd:
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationZ(3.141593f) * sourceSpace.m_localMatrixAdd;
          break;
        case MySymmetryAxisEnum.XHalfY:
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationX(3.141593f) * sourceSpace.m_localMatrixAdd;
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationY(1.570796f) * targetSpace.m_localMatrixAdd;
          break;
        case MySymmetryAxisEnum.YHalfY:
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationY(3.141593f) * sourceSpace.m_localMatrixAdd;
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationY(1.570796f) * targetSpace.m_localMatrixAdd;
          break;
        case MySymmetryAxisEnum.ZHalfY:
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationZ(3.141593f) * sourceSpace.m_localMatrixAdd;
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationY(1.570796f) * targetSpace.m_localMatrixAdd;
          break;
        case MySymmetryAxisEnum.XHalfX:
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationX(3.141593f) * sourceSpace.m_localMatrixAdd;
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationX(-1.570796f) * targetSpace.m_localMatrixAdd;
          break;
        case MySymmetryAxisEnum.YHalfX:
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationY(3.141593f) * sourceSpace.m_localMatrixAdd;
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationX(-1.570796f) * targetSpace.m_localMatrixAdd;
          break;
        case MySymmetryAxisEnum.ZHalfX:
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationZ(3.141593f) * sourceSpace.m_localMatrixAdd;
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationX(-1.570796f) * targetSpace.m_localMatrixAdd;
          break;
        case MySymmetryAxisEnum.XHalfZ:
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationX(3.141593f) * sourceSpace.m_localMatrixAdd;
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationZ(-1.570796f) * targetSpace.m_localMatrixAdd;
          break;
        case MySymmetryAxisEnum.YHalfZ:
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationY(3.141593f) * sourceSpace.m_localMatrixAdd;
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationZ(-1.570796f) * targetSpace.m_localMatrixAdd;
          break;
        case MySymmetryAxisEnum.ZHalfZ:
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationZ(3.141593f) * sourceSpace.m_localMatrixAdd;
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationZ(-1.570796f) * targetSpace.m_localMatrixAdd;
          break;
        case MySymmetryAxisEnum.MinusHalfX:
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationX(1.570796f) * sourceSpace.m_localMatrixAdd;
          break;
        case MySymmetryAxisEnum.MinusHalfY:
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationY(1.570796f) * sourceSpace.m_localMatrixAdd;
          break;
        case MySymmetryAxisEnum.MinusHalfZ:
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationZ(1.570796f) * sourceSpace.m_localMatrixAdd;
          break;
        case MySymmetryAxisEnum.HalfX:
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationX(-1.570796f) * sourceSpace.m_localMatrixAdd;
          break;
        case MySymmetryAxisEnum.HalfY:
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationY(-1.570796f) * sourceSpace.m_localMatrixAdd;
          break;
        case MySymmetryAxisEnum.HalfZ:
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationZ(-1.570796f) * sourceSpace.m_localMatrixAdd;
          break;
        case MySymmetryAxisEnum.XMinusHalfZ:
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationX(3.141593f) * sourceSpace.m_localMatrixAdd;
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationZ(1.570796f) * targetSpace.m_localMatrixAdd;
          break;
        case MySymmetryAxisEnum.YMinusHalfZ:
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationY(3.141593f) * sourceSpace.m_localMatrixAdd;
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationZ(1.570796f) * targetSpace.m_localMatrixAdd;
          break;
        case MySymmetryAxisEnum.ZMinusHalfZ:
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationZ(3.141593f) * sourceSpace.m_localMatrixAdd;
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationZ(1.570796f) * targetSpace.m_localMatrixAdd;
          break;
        case MySymmetryAxisEnum.XMinusHalfX:
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationX(3.141593f) * sourceSpace.m_localMatrixAdd;
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationX(1.570796f) * targetSpace.m_localMatrixAdd;
          break;
        case MySymmetryAxisEnum.YMinusHalfX:
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationY(3.141593f) * sourceSpace.m_localMatrixAdd;
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationX(1.570796f) * targetSpace.m_localMatrixAdd;
          break;
        case MySymmetryAxisEnum.ZMinusHalfX:
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationZ(3.141593f) * sourceSpace.m_localMatrixAdd;
          targetSpace.m_localMatrixAdd = Matrix.CreateRotationX(1.570796f) * targetSpace.m_localMatrixAdd;
          break;
        case MySymmetryAxisEnum.OffsetX:
        case MySymmetryAxisEnum.OffsetXOddTest:
          targetSpace.m_localMatrixAdd = sourceSpace.m_localMatrixAdd;
          break;
        default:
          targetSpace.m_localMatrixAdd = sourceSpace.m_localMatrixAdd;
          break;
      }
      targetSpace.m_blockDefinition = string.IsNullOrEmpty(sourceSpace.m_blockDefinition.MirroringBlock) ? sourceSpace.m_blockDefinition : MyDefinitionManager.Static.GetCubeBlockDefinition(new MyDefinitionId(sourceSpace.m_blockDefinition.Id.TypeId, sourceSpace.m_blockDefinition.MirroringBlock));
      if (symmetryAxisEnum1 == MySymmetryAxisEnum.None && symmetryAxisEnum2 == MySymmetryAxisEnum.None && symmetryAxisEnum3 == MySymmetryAxisEnum.None)
      {
        BoundingBox boundingBox = new BoundingBox(sourceSpace.m_min * cubeGrid.GridSize - new Vector3(cubeGrid.GridSize / 2f), sourceSpace.m_max * cubeGrid.GridSize + new Vector3(cubeGrid.GridSize / 2f));
        if ((double) boundingBox.Size.X > 1.0 * (double) cubeGrid.GridSize || (double) boundingBox.Size.Y > 1.0 * (double) cubeGrid.GridSize || (double) boundingBox.Size.Z > 1.0 * (double) cubeGrid.GridSize)
        {
          Vector3 position = sourceSpace.m_addPos * cubeGrid.GridSize;
          Vector3D vector3D1 = Vector3D.Transform(position, cubeGrid.WorldMatrix);
          Vector3 vector3_1 = (mirrorPosition - sourceSpace.m_addPos) * cubeGrid.GridSize;
          if (isOdd)
          {
            vector3_1.X -= cubeGrid.GridSize / 2f;
            vector3_1.Y -= cubeGrid.GridSize / 2f;
            vector3_1.Z += cubeGrid.GridSize / 2f;
          }
          Vector3 normal1 = vector3_1;
          Vector3 normal2 = Vector3.Clamp(position + vector3_1, boundingBox.Min, boundingBox.Max) - position;
          Vector3 vector3_2 = Vector3.Clamp(position + vector3_1 * 100f, boundingBox.Min, boundingBox.Max) - position;
          Vector3 normal3 = Vector3.Clamp(position - vector3_1 * 100f, boundingBox.Min, boundingBox.Max) - position;
          switch (mirrorPlane)
          {
            case MySymmetrySettingModeEnum.XPlane:
            case MySymmetrySettingModeEnum.XPlaneOdd:
              normal3.Y = 0.0f;
              normal3.Z = 0.0f;
              normal2.Y = 0.0f;
              normal2.Z = 0.0f;
              normal1.Y = 0.0f;
              normal1.Z = 0.0f;
              vector3_2.Y = 0.0f;
              vector3_2.Z = 0.0f;
              break;
            case MySymmetrySettingModeEnum.YPlane:
            case MySymmetrySettingModeEnum.YPlaneOdd:
              normal3.X = 0.0f;
              normal3.Z = 0.0f;
              normal2.X = 0.0f;
              normal2.Z = 0.0f;
              normal1.X = 0.0f;
              normal1.Z = 0.0f;
              vector3_2.X = 0.0f;
              vector3_2.Z = 0.0f;
              break;
            case MySymmetrySettingModeEnum.ZPlane:
            case MySymmetrySettingModeEnum.ZPlaneOdd:
              normal3.Y = 0.0f;
              normal3.X = 0.0f;
              normal2.Y = 0.0f;
              normal2.X = 0.0f;
              normal1.Y = 0.0f;
              normal1.X = 0.0f;
              vector3_2.Y = 0.0f;
              vector3_2.X = 0.0f;
              break;
          }
          Vector3 normal4 = normal1 - normal2;
          Vector3D.TransformNormal(normal2, cubeGrid.WorldMatrix);
          Vector3D vector3D2 = Vector3D.TransformNormal(normal1, cubeGrid.WorldMatrix);
          Vector3D.TransformNormal(normal3, cubeGrid.WorldMatrix);
          bool flag = false;
          if ((double) normal1.LengthSquared() < (double) vector3_2.LengthSquared())
            flag = true;
          Vector3 normal5 = -normal3;
          Vector3D vector3D3 = (Vector3D) Vector3.TransformNormal(normal4, cubeGrid.WorldMatrix);
          Vector3D vector3D4 = (Vector3D) Vector3.TransformNormal(normal5, cubeGrid.WorldMatrix);
          Vector3D vector3D5 = vector3D1 + vector3D2;
          Vector3 normal6 = normal4 + normal5;
          Vector3D.TransformNormal(normal6, cubeGrid.WorldMatrix);
          Vector3 normal7 = (Vector3) sourceSpace.m_addPos + (normal1 + normal6) / cubeGrid.GridSize;
          if (!flag)
          {
            Vector3D.TransformNormal(normal7, cubeGrid.WorldMatrix);
            Vector3 xyz = normal7;
            targetSpace.m_mirroringOffset = new Vector3I(xyz) - targetSpace.m_addPos;
            targetSpace.m_addPos += targetSpace.m_mirroringOffset;
            targetSpace.m_addDir = sourceSpace.m_addDir;
            targetSpace.m_localMatrixAdd.Translation = (Vector3) targetSpace.m_addPos;
            if (targetSpace.m_startBuild.HasValue)
            {
              MyCubeBuilderGizmo.MyGizmoSpaceProperties gizmoSpaceProperties = targetSpace;
              Vector3I? startBuild = targetSpace.m_startBuild;
              Vector3I mirroringOffset = targetSpace.m_mirroringOffset;
              Vector3I? nullable = startBuild.HasValue ? new Vector3I?(startBuild.GetValueOrDefault() + mirroringOffset) : new Vector3I?();
              gizmoSpaceProperties.m_startBuild = nullable;
            }
          }
          else
          {
            targetSpace.m_mirroringOffset = Vector3I.Zero;
            targetSpace.m_addPos = sourceSpace.m_addPos;
            targetSpace.m_removePos = sourceSpace.m_removePos;
            targetSpace.m_removeBlock = cubeGrid.GetCubeBlock(sourceSpace.m_removePos);
          }
        }
      }
      Vector3I vector3I = Vector3I.Zero;
      switch (MyCubeBuilderGizmo.CurrentBlockMirrorOption)
      {
        case MySymmetryAxisEnum.ZThenOffsetX:
        case MySymmetryAxisEnum.YThenOffsetX:
        case MySymmetryAxisEnum.OffsetX:
          vector3I = new Vector3I(targetSpace.m_localMatrixAdd.Left);
          break;
        case MySymmetryAxisEnum.ZThenOffsetXOdd:
          Vector3 vector2_2 = Vector3.Left;
          switch (mirrorPlane)
          {
            case MySymmetrySettingModeEnum.XPlane:
              vector2_2 = Vector3.Up;
              break;
            case MySymmetrySettingModeEnum.YPlane:
              vector2_2 = Vector3.Forward;
              break;
            case MySymmetrySettingModeEnum.ZPlane:
              vector2_2 = Vector3.Left;
              break;
          }
          if ((double) Math.Abs(Vector3.Dot(targetSpace.m_localMatrixAdd.Left, vector2_2)) > 0.899999976158142)
          {
            vector3I = new Vector3I(targetSpace.m_localMatrixAdd.Left);
            break;
          }
          break;
        case MySymmetryAxisEnum.YThenOffsetXOdd:
          Vector3 vector2_3 = Vector3.Left;
          switch (mirrorPlane)
          {
            case MySymmetrySettingModeEnum.XPlane:
              vector2_3 = Vector3.Up;
              break;
            case MySymmetrySettingModeEnum.YPlane:
              vector2_3 = Vector3.Forward;
              break;
            case MySymmetrySettingModeEnum.ZPlane:
              vector2_3 = Vector3.Left;
              break;
          }
          if ((double) Math.Abs(Vector3.Dot(targetSpace.m_localMatrixAdd.Left, vector2_3)) > 0.899999976158142)
          {
            vector3I = Vector3I.Round(targetSpace.m_localMatrixAdd.Left);
            break;
          }
          break;
        case MySymmetryAxisEnum.OffsetXOddTest:
          Vector3 vector2_4 = Vector3.Left;
          switch (MyCubeBuilderGizmo.CurrentBlockMirrorAxis)
          {
            case MySymmetryAxisEnum.X:
              vector2_4 = Vector3.Forward;
              break;
            case MySymmetryAxisEnum.Y:
              vector2_4 = Vector3.Up;
              break;
            case MySymmetryAxisEnum.Z:
              vector2_4 = Vector3.Left;
              break;
          }
          if ((double) Math.Abs(Vector3.Dot(targetSpace.m_localMatrixAdd.Left, vector2_4)) > 0.899999976158142)
          {
            vector3I = Vector3I.Round(targetSpace.m_localMatrixAdd.Left);
            break;
          }
          break;
      }
      switch (MyCubeBuilderGizmo.CurrentBlockMirrorOption)
      {
        case MySymmetryAxisEnum.None:
        case MySymmetryAxisEnum.ZThenOffsetX:
        case MySymmetryAxisEnum.YThenOffsetX:
        case MySymmetryAxisEnum.OffsetX:
        case MySymmetryAxisEnum.ZThenOffsetXOdd:
        case MySymmetryAxisEnum.YThenOffsetXOdd:
        case MySymmetryAxisEnum.OffsetXOddTest:
          targetSpace.m_mirroringOffset = vector3I;
          targetSpace.m_addPos += targetSpace.m_mirroringOffset;
          targetSpace.m_removePos += targetSpace.m_mirroringOffset;
          targetSpace.m_removeBlock = cubeGrid.GetCubeBlock(targetSpace.m_removePos);
          targetSpace.m_localMatrixAdd.Translation += (Vector3) vector3I;
          break;
      }
      targetSpace.m_worldMatrixAdd = targetSpace.m_localMatrixAdd * cubeGrid.WorldMatrix;
    }

    public class MyGizmoSpaceProperties
    {
      public bool Enabled;
      public MyGizmoSpaceEnum SourceSpace;
      public MySymmetrySettingModeEnum SymmetryPlane;
      public Vector3I SymmetryPlanePos;
      public bool SymmetryIsOdd;
      public MatrixD m_worldMatrixAdd = (MatrixD) ref Matrix.Identity;
      public Matrix m_localMatrixAdd = Matrix.Identity;
      public Vector3I m_addDir = Vector3I.Up;
      public Vector3I m_addPos;
      public Vector3I m_min;
      public Vector3I m_max;
      public Vector3I m_centerPos;
      public Vector3I m_removePos;
      public MySlimBlock m_removeBlock;
      public ushort? m_blockIdInCompound;
      public Vector3I? m_startBuild;
      public Vector3I? m_continueBuild;
      public Vector3I? m_startRemove;
      public List<Vector3I> m_positions = new List<Vector3I>();
      public List<Vector3> m_cubeNormals = new List<Vector3>();
      public List<Vector4UByte> m_patternOffsets = new List<Vector4UByte>();
      public Vector3? m_addPosSmallOnLarge;
      public Vector3 m_minSmallOnLarge;
      public Vector3 m_maxSmallOnLarge;
      public Vector3 m_centerPosSmallOnLarge;
      public List<Vector3> m_positionsSmallOnLarge = new List<Vector3>();
      public List<string> m_cubeModels = new List<string>();
      public List<MatrixD> m_cubeMatrices = new List<MatrixD>();
      public List<string> m_cubeModelsTemp = new List<string>();
      public List<MatrixD> m_cubeMatricesTemp = new List<MatrixD>();
      public bool m_buildAllowed;
      public bool m_showGizmoCube;
      public Quaternion m_rotation;
      public Vector3I m_mirroringOffset;
      public MyCubeBlockDefinition m_blockDefinition;
      public bool m_dynamicBuildAllowed;
      public HashSet<Tuple<MySlimBlock, ushort?>> m_removeBlocksInMultiBlock = new HashSet<Tuple<MySlimBlock, ushort?>>();
      public MatrixD m_animationLastMatrix = MatrixD.Identity;
      public Vector3D m_animationLastPosition = Vector3D.Zero;
      public float m_animationProgress = 1f;

      public Quaternion LocalOrientation => Quaternion.CreateFromRotationMatrix(this.m_localMatrixAdd);

      public void Clear()
      {
        this.m_startBuild = new Vector3I?();
        this.m_startRemove = new Vector3I?();
        this.m_removeBlock = (MySlimBlock) null;
        this.m_blockIdInCompound = new ushort?();
        this.m_positions.Clear();
        this.m_cubeNormals.Clear();
        this.m_patternOffsets.Clear();
        this.m_cubeModels.Clear();
        this.m_cubeMatrices.Clear();
        this.m_cubeModelsTemp.Clear();
        this.m_cubeMatricesTemp.Clear();
        this.m_mirroringOffset = Vector3I.Zero;
        this.m_addPosSmallOnLarge = new Vector3?();
        this.m_positionsSmallOnLarge.Clear();
        this.m_dynamicBuildAllowed = false;
        this.m_removeBlocksInMultiBlock.Clear();
      }
    }
  }
}
