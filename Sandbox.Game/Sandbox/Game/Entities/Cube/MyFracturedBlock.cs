// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyFracturedBlock
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.EntityComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
  [MyCubeBlockType(typeof (MyObjectBuilder_FracturedBlock))]
  public class MyFracturedBlock : MyCubeBlock
  {
    private static List<HkdShapeInstanceInfo> m_children = new List<HkdShapeInstanceInfo>();
    private static List<HkdShapeInstanceInfo> m_shapeInfos = new List<HkdShapeInstanceInfo>();
    private static HashSet<Tuple<string, float>> m_tmpNamesAndBuildProgress = new HashSet<Tuple<string, float>>();
    public HkdBreakableShape Shape;
    public List<MyDefinitionId> OriginalBlocks;
    public List<MyBlockOrientation> Orientations;
    public List<MyFracturedBlock.MultiBlockPartInfo> MultiBlocks;
    private List<MyObjectBuilder_FracturedBlock.ShapeB> m_shapes = new List<MyObjectBuilder_FracturedBlock.ShapeB>();
    private List<MyCubeBlockDefinition.MountPoint> m_mpCache = new List<MyCubeBlockDefinition.MountPoint>();

    private MyRenderComponentFracturedPiece Render
    {
      get => (MyRenderComponentFracturedPiece) base.Render;
      set => this.Render = (MyRenderComponentBase) value;
    }

    public MyFracturedBlock()
    {
      this.EntityId = MyEntityIdentifier.AllocateId();
      this.Render = new MyRenderComponentFracturedPiece();
      this.Render.NeedsDraw = true;
      this.Render.PersistentFlags = MyPersistentEntityFlags2.Enabled;
      this.CheckConnectionAllowed = true;
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyFracturedBlock.MyFBDebugRender(this));
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_FracturedBlock builderCubeBlock = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_FracturedBlock;
      if (string.IsNullOrEmpty(this.Shape.Name) || this.Shape.IsCompound())
      {
        this.Shape.GetChildren(MyFracturedBlock.m_children);
        foreach (HkdShapeInstanceInfo child in MyFracturedBlock.m_children)
        {
          MyObjectBuilder_FracturedBlock.ShapeB shapeB = new MyObjectBuilder_FracturedBlock.ShapeB()
          {
            Name = child.ShapeName,
            Orientation = (SerializableQuaternion) Quaternion.CreateFromRotationMatrix(child.GetTransform().GetOrientation()),
            Fixed = MyDestructionHelper.IsFixed(child.Shape)
          };
          builderCubeBlock.Shapes.Add(shapeB);
        }
        MyFracturedBlock.m_children.Clear();
      }
      else
        builderCubeBlock.Shapes.Add(new MyObjectBuilder_FracturedBlock.ShapeB()
        {
          Name = this.Shape.Name
        });
      foreach (MyDefinitionId originalBlock in this.OriginalBlocks)
        builderCubeBlock.BlockDefinitions.Add((SerializableDefinitionId) originalBlock);
      foreach (MyBlockOrientation orientation in this.Orientations)
        builderCubeBlock.BlockOrientations.Add((SerializableBlockOrientation) orientation);
      if (this.MultiBlocks != null)
      {
        foreach (MyFracturedBlock.MultiBlockPartInfo multiBlock in this.MultiBlocks)
        {
          if (multiBlock != null)
            builderCubeBlock.MultiBlocks.Add(new MyObjectBuilder_FracturedBlock.MyMultiBlockPart()
            {
              MultiBlockDefinition = (SerializableDefinitionId) multiBlock.MultiBlockDefinition,
              MultiBlockId = multiBlock.MultiBlockId
            });
          else
            builderCubeBlock.MultiBlocks.Add((MyObjectBuilder_FracturedBlock.MyMultiBlockPart) null);
        }
      }
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public List<MyCubeBlockDefinition.MountPoint> MountPoints { get; private set; }

    public override void Init(MyObjectBuilder_CubeBlock builder, MyCubeGrid cubeGrid)
    {
      base.Init(builder, cubeGrid);
      this.CheckConnectionAllowed = true;
      MyObjectBuilder_FracturedBlock builderFracturedBlock = builder as MyObjectBuilder_FracturedBlock;
      if (builderFracturedBlock.Shapes.Count == 0)
      {
        if (!builderFracturedBlock.CreatingFracturedBlock)
          throw new Exception("No relevant shape was found for fractured block. It was probably reexported and names changed.");
      }
      else
      {
        this.OriginalBlocks = new List<MyDefinitionId>();
        this.Orientations = new List<MyBlockOrientation>();
        List<HkdShapeInstanceInfo> shapeInstanceInfoList = new List<HkdShapeInstanceInfo>();
        foreach (SerializableDefinitionId blockDefinition in builderFracturedBlock.BlockDefinitions)
        {
          MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition((MyDefinitionId) blockDefinition);
          string model = cubeBlockDefinition.Model;
          if (MyModels.GetModelOnlyData(model).HavokBreakableShapes == null)
            MyDestructionData.Static.LoadModelDestruction(model, (MyPhysicalModelDefinition) cubeBlockDefinition, Vector3.One);
          HkdBreakableShape havokBreakableShape1 = MyModels.GetModelOnlyData(model).HavokBreakableShapes[0];
          HkdShapeInstanceInfo shapeInstanceInfo = new HkdShapeInstanceInfo(havokBreakableShape1, new Quaternion?(), new Vector3?());
          shapeInstanceInfoList.Add(shapeInstanceInfo);
          MyFracturedBlock.m_children.Add(shapeInstanceInfo);
          havokBreakableShape1.GetChildren(MyFracturedBlock.m_children);
          if (cubeBlockDefinition.BuildProgressModels != null)
          {
            foreach (MyCubeBlockDefinition.BuildProgressModel buildProgressModel in cubeBlockDefinition.BuildProgressModels)
            {
              string file = buildProgressModel.File;
              if (MyModels.GetModelOnlyData(file).HavokBreakableShapes == null)
                MyDestructionData.Static.LoadModelDestruction(file, (MyPhysicalModelDefinition) cubeBlockDefinition, Vector3.One);
              HkdBreakableShape havokBreakableShape2 = MyModels.GetModelOnlyData(file).HavokBreakableShapes[0];
              shapeInstanceInfo = new HkdShapeInstanceInfo(havokBreakableShape2, new Quaternion?(), new Vector3?());
              shapeInstanceInfoList.Add(shapeInstanceInfo);
              MyFracturedBlock.m_children.Add(shapeInstanceInfo);
              havokBreakableShape2.GetChildren(MyFracturedBlock.m_children);
            }
          }
          this.OriginalBlocks.Add((MyDefinitionId) blockDefinition);
        }
        foreach (SerializableBlockOrientation blockOrientation in builderFracturedBlock.BlockOrientations)
          this.Orientations.Add((MyBlockOrientation) blockOrientation);
        if (builderFracturedBlock.MultiBlocks.Count > 0)
        {
          this.MultiBlocks = new List<MyFracturedBlock.MultiBlockPartInfo>();
          foreach (MyObjectBuilder_FracturedBlock.MyMultiBlockPart multiBlock in builderFracturedBlock.MultiBlocks)
          {
            if (multiBlock != null)
              this.MultiBlocks.Add(new MyFracturedBlock.MultiBlockPartInfo()
              {
                MultiBlockDefinition = (MyDefinitionId) multiBlock.MultiBlockDefinition,
                MultiBlockId = multiBlock.MultiBlockId
              });
            else
              this.MultiBlocks.Add((MyFracturedBlock.MultiBlockPartInfo) null);
          }
        }
        this.m_shapes.AddRange((IEnumerable<MyObjectBuilder_FracturedBlock.ShapeB>) builderFracturedBlock.Shapes);
        Matrix matrix;
        for (int index = 0; index < MyFracturedBlock.m_children.Count; ++index)
        {
          HkdShapeInstanceInfo child = MyFracturedBlock.m_children[index];
          IEnumerable<MyObjectBuilder_FracturedBlock.ShapeB> source = this.m_shapes.Where<MyObjectBuilder_FracturedBlock.ShapeB>((Func<MyObjectBuilder_FracturedBlock.ShapeB, bool>) (s => s.Name == child.ShapeName));
          if (source.Count<MyObjectBuilder_FracturedBlock.ShapeB>() > 0)
          {
            MyObjectBuilder_FracturedBlock.ShapeB shapeB = source.First<MyObjectBuilder_FracturedBlock.ShapeB>();
            Matrix fromQuaternion = Matrix.CreateFromQuaternion((Quaternion) shapeB.Orientation);
            ref Matrix local = ref fromQuaternion;
            matrix = child.GetTransform();
            Vector3 translation = matrix.Translation;
            local.Translation = translation;
            HkdShapeInstanceInfo shapeInstanceInfo = new HkdShapeInstanceInfo(child.Shape.Clone(), fromQuaternion);
            if (shapeB.Fixed)
              shapeInstanceInfo.Shape.SetFlagRecursively(HkdBreakableShape.Flags.IS_FIXED);
            shapeInstanceInfoList.Add(shapeInstanceInfo);
            MyFracturedBlock.m_shapeInfos.Add(shapeInstanceInfo);
            this.m_shapes.Remove(shapeB);
          }
          else
            child.GetChildren(MyFracturedBlock.m_children);
        }
        if (MyFracturedBlock.m_shapeInfos.Count == 0)
        {
          MyFracturedBlock.m_children.Clear();
          throw new Exception("No relevant shape was found for fractured block. It was probably reexported and names changed.");
        }
        foreach (HkdShapeInstanceInfo shapeInfo in MyFracturedBlock.m_shapeInfos)
        {
          if (!string.IsNullOrEmpty(shapeInfo.Shape.Name))
          {
            MyRenderComponentFracturedPiece render = this.Render;
            string name = shapeInfo.Shape.Name;
            matrix = shapeInfo.GetTransform();
            matrix = Matrix.CreateFromQuaternion(Quaternion.CreateFromRotationMatrix(matrix.GetOrientation()));
            MatrixD localTransform = (MatrixD) ref matrix;
            render.AddPiece(name, localTransform);
          }
        }
        if (this.CubeGrid.CreatePhysics)
        {
          HkdBreakableShape hkdBreakableShape = (HkdBreakableShape) new HkdCompoundBreakableShape((HkdBreakableShape) null, MyFracturedBlock.m_shapeInfos);
          ((HkdCompoundBreakableShape) hkdBreakableShape).RecalcMassPropsFromChildren();
          this.Shape = hkdBreakableShape;
          HkMassProperties hkMassProperties = new HkMassProperties();
          hkdBreakableShape.BuildMassProperties(ref hkMassProperties);
          this.Shape = new HkdBreakableShape(hkdBreakableShape.GetShape(), ref hkMassProperties);
          hkdBreakableShape.RemoveReference();
          foreach (HkdShapeInstanceInfo shapeInfo in MyFracturedBlock.m_shapeInfos)
            this.Shape.AddShape(ref shapeInfo);
          this.Shape.SetStrenght(MyDestructionConstants.STRENGTH);
          this.CreateMountPoints();
        }
        MyFracturedBlock.m_children.Clear();
        foreach (HkdShapeInstanceInfo shapeInfo in MyFracturedBlock.m_shapeInfos)
          shapeInfo.Shape.RemoveReference();
        foreach (HkdShapeInstanceInfo shapeInstanceInfo in shapeInstanceInfoList)
          shapeInstanceInfo.RemoveReference();
        MyFracturedBlock.m_shapeInfos.Clear();
      }
    }

    public void SetDataFromCompound(HkdBreakableShape compound)
    {
      MyRenderComponentFracturedPiece render = this.Render;
      if (render == null)
        return;
      compound.GetChildren(MyFracturedBlock.m_shapeInfos);
      foreach (HkdShapeInstanceInfo shapeInfo in MyFracturedBlock.m_shapeInfos)
      {
        if (shapeInfo.IsValid())
        {
          MyRenderComponentFracturedPiece componentFracturedPiece = render;
          string shapeName = shapeInfo.ShapeName;
          Matrix transform = shapeInfo.GetTransform();
          MatrixD localTransform = (MatrixD) ref transform;
          componentFracturedPiece.AddPiece(shapeName, localTransform);
        }
      }
      MyFracturedBlock.m_shapeInfos.Clear();
    }

    private void AddMeshBuilderRecursively(List<HkdShapeInstanceInfo> children)
    {
      MyRenderComponentFracturedPiece render = this.Render;
      foreach (HkdShapeInstanceInfo child in children)
        render.AddPiece(child.ShapeName, (MatrixD) ref Matrix.Identity);
    }

    internal void SetDataFromHavok(HkdBreakableShape shape, bool compound)
    {
      this.Shape = shape;
      if (compound)
        this.SetDataFromCompound(shape);
      else
        this.Render.AddPiece(shape.Name, (MatrixD) ref Matrix.Identity);
      this.CreateMountPoints();
    }

    private void CreateMountPoints()
    {
      if (MyFakes.FRACTURED_BLOCK_AABB_MOUNT_POINTS)
      {
        this.MountPoints = new List<MyCubeBlockDefinition.MountPoint>();
        BoundingBox blockBB = BoundingBox.CreateInvalid();
        for (int index = 0; index < this.OriginalBlocks.Count; ++index)
        {
          MyDefinitionId originalBlock = this.OriginalBlocks[index];
          Matrix result;
          this.Orientations[index].GetMatrix(out result);
          Vector3 vector3 = new Vector3(MyDefinitionManager.Static.GetCubeBlockDefinition(originalBlock).Size);
          BoundingBox boundingBox = new BoundingBox(-vector3 / 2f, vector3 / 2f);
          blockBB = blockBB.Include(boundingBox.Transform(result));
        }
        Vector3 halfExtents = blockBB.HalfExtents;
        blockBB.Min += halfExtents;
        blockBB.Max += halfExtents;
        this.Shape.GetChildren(MyFracturedBlock.m_children);
        foreach (HkdShapeInstanceInfo child in MyFracturedBlock.m_children)
          MyFractureComponentCubeBlock.AddMountForShape(child.Shape, child.GetTransform(), ref blockBB, this.CubeGrid.GridSize, this.MountPoints);
        if (MyFracturedBlock.m_children.Count == 0)
          MyFractureComponentCubeBlock.AddMountForShape(this.Shape, Matrix.Identity, ref blockBB, this.CubeGrid.GridSize, this.MountPoints);
        MyFracturedBlock.m_children.Clear();
      }
      else
        this.MountPoints = new List<MyCubeBlockDefinition.MountPoint>((IEnumerable<MyCubeBlockDefinition.MountPoint>) MyCubeBuilder.AutogenerateMountpoints(new HkShape[1]
        {
          this.Shape.GetShape()
        }, this.CubeGrid.GridSize));
    }

    protected override void Closing()
    {
      if (this.Shape.IsValid())
        this.Shape.RemoveReference();
      base.Closing();
    }

    public override bool ConnectionAllowed(
      ref Vector3I otherBlockPos,
      ref Vector3I faceNormal,
      MyCubeBlockDefinition def)
    {
      if (this.MountPoints == null)
        return true;
      Vector3I positionB = this.Position + faceNormal;
      MySlimBlock cubeBlock = this.CubeGrid.GetCubeBlock(positionB);
      MyBlockOrientation blockOrientation = cubeBlock == null ? MyBlockOrientation.Identity : cubeBlock.Orientation;
      Vector3I position = this.Position;
      this.m_mpCache.Clear();
      if (cubeBlock != null && cubeBlock.FatBlock is MyFracturedBlock)
        this.m_mpCache.AddRange((IEnumerable<MyCubeBlockDefinition.MountPoint>) (cubeBlock.FatBlock as MyFracturedBlock).MountPoints);
      else if (cubeBlock != null && cubeBlock.FatBlock is MyCompoundCubeBlock)
      {
        List<MyCubeBlockDefinition.MountPoint> outMountPoints = new List<MyCubeBlockDefinition.MountPoint>();
        foreach (MySlimBlock block in (cubeBlock.FatBlock as MyCompoundCubeBlock).GetBlocks())
        {
          MyCubeBlockDefinition.MountPoint[] modelMountPoints = block.BlockDefinition.GetBuildProgressModelMountPoints(block.BuildLevelRatio);
          MyCubeGrid.TransformMountPoints(outMountPoints, block.BlockDefinition, modelMountPoints, ref block.Orientation);
          this.m_mpCache.AddRange((IEnumerable<MyCubeBlockDefinition.MountPoint>) outMountPoints);
        }
      }
      else if (cubeBlock != null)
      {
        MyCubeBlockDefinition.MountPoint[] modelMountPoints = def.GetBuildProgressModelMountPoints(cubeBlock.BuildLevelRatio);
        MyCubeGrid.TransformMountPoints(this.m_mpCache, def, modelMountPoints, ref blockOrientation);
      }
      return MyCubeGrid.CheckMountPointsForSide(this.MountPoints, ref this.SlimBlock.Orientation, ref position, this.BlockDefinition.Id, ref faceNormal, this.m_mpCache, ref blockOrientation, ref positionB, def.Id);
    }

    public bool IsMultiBlockPart(MyDefinitionId multiBlockDefinition, int multiblockId)
    {
      if (this.MultiBlocks == null)
        return false;
      foreach (MyFracturedBlock.MultiBlockPartInfo multiBlock in this.MultiBlocks)
      {
        if (multiBlock != null && multiBlock.MultiBlockDefinition == multiBlockDefinition && multiBlock.MultiBlockId == multiblockId)
          return true;
      }
      return false;
    }

    public MyObjectBuilder_CubeBlock ConvertToOriginalBlocksWithFractureComponent()
    {
      List<MyObjectBuilder_CubeBlock> cubeBlockBuilders = new List<MyObjectBuilder_CubeBlock>();
      for (int index = 0; index < this.OriginalBlocks.Count; ++index)
      {
        MyDefinitionId originalBlock = this.OriginalBlocks[index];
        MyCubeBlockDefinition blockDefinition;
        MyDefinitionManager.Static.TryGetCubeBlockDefinition(originalBlock, out blockDefinition);
        if (blockDefinition != null)
        {
          MyBlockOrientation orientation = this.Orientations[index];
          MyFracturedBlock.MultiBlockPartInfo multiBlockPartInfo = this.MultiBlocks == null || this.MultiBlocks.Count <= index ? (MyFracturedBlock.MultiBlockPartInfo) null : this.MultiBlocks[index];
          MyObjectBuilder_CubeBlock newObject = MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) originalBlock) as MyObjectBuilder_CubeBlock;
          Quaternion result;
          orientation.GetQuaternion(out result);
          newObject.Orientation = (SerializableQuaternion) result;
          newObject.Min = (SerializableVector3I) this.Position;
          newObject.MultiBlockId = multiBlockPartInfo != null ? multiBlockPartInfo.MultiBlockId : 0;
          newObject.MultiBlockDefinition = new SerializableDefinitionId?();
          if (multiBlockPartInfo != null)
            newObject.MultiBlockDefinition = new SerializableDefinitionId?((SerializableDefinitionId) multiBlockPartInfo.MultiBlockDefinition);
          newObject.ComponentContainer = new MyObjectBuilder_ComponentContainer();
          MyObjectBuilder_FractureComponentCubeBlock fractureComponentBuilder = new MyObjectBuilder_FractureComponentCubeBlock();
          MyFracturedBlock.m_tmpNamesAndBuildProgress.Clear();
          MyFracturedBlock.GetAllBlockBreakableShapeNames(blockDefinition, MyFracturedBlock.m_tmpNamesAndBuildProgress);
          float buildProgress;
          MyFracturedBlock.ConvertAllShapesToFractureComponentShapeBuilder(this.Shape, ref Matrix.Identity, orientation, MyFracturedBlock.m_tmpNamesAndBuildProgress, fractureComponentBuilder, out buildProgress);
          MyFracturedBlock.m_tmpNamesAndBuildProgress.Clear();
          if (fractureComponentBuilder.Shapes.Count != 0)
          {
            if (blockDefinition.BuildProgressModels != null)
            {
              foreach (MyCubeBlockDefinition.BuildProgressModel buildProgressModel in blockDefinition.BuildProgressModels)
              {
                if ((double) buildProgressModel.BuildRatioUpperBound < (double) buildProgress)
                {
                  double buildRatioUpperBound = (double) buildProgressModel.BuildRatioUpperBound;
                }
                else
                  break;
              }
            }
            newObject.ComponentContainer.Components.Add(new MyObjectBuilder_ComponentContainer.ComponentData()
            {
              TypeId = typeof (MyFractureComponentBase).Name,
              Component = (MyObjectBuilder_ComponentBase) fractureComponentBuilder
            });
            newObject.BuildPercent = buildProgress;
            newObject.IntegrityPercent = MyDefinitionManager.Static.DestructionDefinition.ConvertedFractureIntegrityRatio * buildProgress;
            if (index == 0 && this.CubeGrid.GridSizeEnum == MyCubeSize.Small)
              return newObject;
            cubeBlockBuilders.Add(newObject);
          }
        }
      }
      return cubeBlockBuilders.Count > 0 ? (MyObjectBuilder_CubeBlock) MyCompoundCubeBlock.CreateBuilder(cubeBlockBuilders) : (MyObjectBuilder_CubeBlock) null;
    }

    public static MyObjectBuilder_CubeGrid ConvertFracturedBlocksToComponents(
      MyObjectBuilder_CubeGrid gridBuilder)
    {
      bool flag = false;
      foreach (MyObjectBuilder_CubeBlock cubeBlock in gridBuilder.CubeBlocks)
      {
        if (cubeBlock is MyObjectBuilder_FracturedBlock)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        return gridBuilder;
      bool largeConnections = gridBuilder.EnableSmallToLargeConnections;
      gridBuilder.EnableSmallToLargeConnections = false;
      bool createPhysics = gridBuilder.CreatePhysics;
      gridBuilder.CreatePhysics = true;
      if (!(Sandbox.Game.Entities.MyEntities.CreateFromObjectBuilder((MyObjectBuilder_EntityBase) gridBuilder, false) is MyCubeGrid fromObjectBuilder))
        return gridBuilder;
      fromObjectBuilder.ConvertFracturedBlocksToComponents();
      MyObjectBuilder_CubeGrid objectBuilder = (MyObjectBuilder_CubeGrid) fromObjectBuilder.GetObjectBuilder(false);
      gridBuilder.EnableSmallToLargeConnections = largeConnections;
      objectBuilder.EnableSmallToLargeConnections = largeConnections;
      gridBuilder.CreatePhysics = createPhysics;
      objectBuilder.CreatePhysics = createPhysics;
      fromObjectBuilder.Close();
      Sandbox.Game.Entities.MyEntities.RemapObjectBuilder((MyObjectBuilder_EntityBase) objectBuilder);
      return objectBuilder;
    }

    public static void GetAllBlockBreakableShapeNames(
      MyCubeBlockDefinition blockDef,
      HashSet<Tuple<string, float>> outNamesAndBuildProgress)
    {
      string model = blockDef.Model;
      if (MyModels.GetModelOnlyData(model).HavokBreakableShapes == null)
        MyDestructionData.Static.LoadModelDestruction(model, (MyPhysicalModelDefinition) blockDef, Vector3.One);
      MyFracturedBlock.GetAllBlockBreakableShapeNames(MyModels.GetModelOnlyData(model).HavokBreakableShapes[0], outNamesAndBuildProgress, 1f);
      if (blockDef.BuildProgressModels == null)
        return;
      float num1 = 0.0f;
      foreach (MyCubeBlockDefinition.BuildProgressModel buildProgressModel in blockDef.BuildProgressModels)
      {
        string file = buildProgressModel.File;
        if (MyModels.GetModelOnlyData(file).HavokBreakableShapes == null)
          MyDestructionData.Static.LoadModelDestruction(file, (MyPhysicalModelDefinition) blockDef, Vector3.One);
        HkdBreakableShape havokBreakableShape = MyModels.GetModelOnlyData(file).HavokBreakableShapes[0];
        float num2 = (float) (0.5 * ((double) buildProgressModel.BuildRatioUpperBound + (double) num1));
        HashSet<Tuple<string, float>> outNamesAndBuildProgress1 = outNamesAndBuildProgress;
        double num3 = (double) num2;
        MyFracturedBlock.GetAllBlockBreakableShapeNames(havokBreakableShape, outNamesAndBuildProgress1, (float) num3);
        num1 = buildProgressModel.BuildRatioUpperBound;
      }
    }

    public static void GetAllBlockBreakableShapeNames(
      HkdBreakableShape shape,
      HashSet<Tuple<string, float>> outNamesAndBuildProgress,
      float buildProgress)
    {
      string name = shape.Name;
      if (!string.IsNullOrEmpty(name))
        outNamesAndBuildProgress.Add(new Tuple<string, float>(name, buildProgress));
      if (shape.GetChildrenCount() <= 0)
        return;
      List<HkdShapeInstanceInfo> list = new List<HkdShapeInstanceInfo>();
      shape.GetChildren(list);
      foreach (HkdShapeInstanceInfo shapeInstanceInfo in list)
        MyFracturedBlock.GetAllBlockBreakableShapeNames(shapeInstanceInfo.Shape, outNamesAndBuildProgress, buildProgress);
    }

    private static void ConvertAllShapesToFractureComponentShapeBuilder(
      HkdBreakableShape shape,
      ref Matrix shapeRotation,
      MyBlockOrientation blockOrientation,
      HashSet<Tuple<string, float>> namesAndBuildProgress,
      MyObjectBuilder_FractureComponentCubeBlock fractureComponentBuilder,
      out float buildProgress)
    {
      buildProgress = 1f;
      string name = shape.Name;
      Tuple<string, float> tuple1 = (Tuple<string, float>) null;
      foreach (Tuple<string, float> tuple2 in namesAndBuildProgress)
      {
        if (tuple2.Item1 == name)
        {
          tuple1 = tuple2;
          break;
        }
      }
      if (tuple1 != null && new MyBlockOrientation(ref shapeRotation) == blockOrientation)
      {
        fractureComponentBuilder.Shapes.Add(new MyObjectBuilder_FractureComponentBase.FracturedShape()
        {
          Name = name,
          Fixed = MyDestructionHelper.IsFixed(shape)
        });
        buildProgress = tuple1.Item2;
      }
      if (shape.GetChildrenCount() <= 0)
        return;
      List<HkdShapeInstanceInfo> list = new List<HkdShapeInstanceInfo>();
      shape.GetChildren(list);
      foreach (HkdShapeInstanceInfo shapeInstanceInfo in list)
      {
        Matrix transform = shapeInstanceInfo.GetTransform();
        float buildProgress1;
        MyFracturedBlock.ConvertAllShapesToFractureComponentShapeBuilder(shapeInstanceInfo.Shape, ref transform, blockOrientation, namesAndBuildProgress, fractureComponentBuilder, out buildProgress1);
        if (tuple1 == null)
          buildProgress = buildProgress1;
      }
    }

    public class MultiBlockPartInfo
    {
      public MyDefinitionId MultiBlockDefinition;
      public int MultiBlockId;
    }

    public struct Info
    {
      public HkdBreakableShape Shape;
      public Vector3I Position;
      public bool Compound;
      public List<MyDefinitionId> OriginalBlocks;
      public List<MyBlockOrientation> Orientations;
      public List<MyFracturedBlock.MultiBlockPartInfo> MultiBlocks;
    }

    private class MyFBDebugRender : MyDebugRenderComponentBase
    {
      private MyFracturedBlock m_block;

      public MyFBDebugRender(MyFracturedBlock b) => this.m_block = b;

      public override void DebugDraw()
      {
        if (!MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS || this.m_block.MountPoints == null)
          return;
        MatrixD drawMatrix = this.m_block.CubeGrid.PositionComp.WorldMatrixRef;
        drawMatrix.Translation = this.m_block.CubeGrid.GridIntegerToWorld(this.m_block.Position);
        MyCubeBuilder.DrawMountPoints(this.m_block.CubeGrid.GridSize, this.m_block.BlockDefinition, drawMatrix, this.m_block.MountPoints.ToArray());
      }

      public override void DebugDrawInvalidTriangles()
      {
      }
    }

    private class Sandbox_Game_Entities_Cube_MyFracturedBlock\u003C\u003EActor : IActivator, IActivator<MyFracturedBlock>
    {
      object IActivator.CreateInstance() => (object) new MyFracturedBlock();

      MyFracturedBlock IActivator<MyFracturedBlock>.CreateInstance() => new MyFracturedBlock();
    }
  }
}
