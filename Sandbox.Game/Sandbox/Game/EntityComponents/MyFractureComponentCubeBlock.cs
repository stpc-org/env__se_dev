// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.MyFractureComponentCubeBlock
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game.Components;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.EntityComponents
{
  [MyComponentBuilder(typeof (MyObjectBuilder_FractureComponentCubeBlock), true)]
  public class MyFractureComponentCubeBlock : MyFractureComponentBase
  {
    private readonly List<MyObjectBuilder_FractureComponentBase.FracturedShape> m_tmpShapeListInit = new List<MyObjectBuilder_FractureComponentBase.FracturedShape>();
    private MyObjectBuilder_ComponentBase m_obFracture;

    public MySlimBlock Block { get; private set; }

    public MyCubeBlockDefinition.MountPoint[] MountPoints { get; private set; }

    public override MyPhysicalModelDefinition PhysicalModelDefinition => (MyPhysicalModelDefinition) this.Block.BlockDefinition;

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      this.Block = (this.Entity as MyCubeBlock).SlimBlock;
      this.Block.FatBlock.CheckConnectionAllowed = true;
      MySlimBlock cubeBlock = this.Block.CubeGrid.GetCubeBlock(this.Block.Position);
      if (cubeBlock != null)
        cubeBlock.FatBlock.CheckConnectionAllowed = true;
      if (this.m_obFracture == null)
        return;
      this.Init(this.m_obFracture);
      this.m_obFracture = (MyObjectBuilder_ComponentBase) null;
    }

    public override void OnBeforeRemovedFromContainer()
    {
      base.OnBeforeRemovedFromContainer();
      this.Block.FatBlock.CheckConnectionAllowed = false;
      MySlimBlock cubeBlock = this.Block.CubeGrid.GetCubeBlock(this.Block.Position);
      if (cubeBlock == null || !(cubeBlock.FatBlock is MyCompoundCubeBlock))
        return;
      bool flag = false;
      foreach (MySlimBlock block in (cubeBlock.FatBlock as MyCompoundCubeBlock).GetBlocks())
        flag |= block.FatBlock.CheckConnectionAllowed;
      if (flag)
        return;
      cubeBlock.FatBlock.CheckConnectionAllowed = false;
    }

    public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
    {
      MyObjectBuilder_FractureComponentCubeBlock componentCubeBlock = base.Serialize() as MyObjectBuilder_FractureComponentCubeBlock;
      this.SerializeInternal((MyObjectBuilder_FractureComponentBase) componentCubeBlock);
      return (MyObjectBuilder_ComponentBase) componentCubeBlock;
    }

    public override void Deserialize(MyObjectBuilder_ComponentBase builder)
    {
      base.Deserialize(builder);
      if (this.Block != null)
        this.Init(builder);
      else
        this.m_obFracture = builder;
    }

    public override void SetShape(HkdBreakableShape shape, bool compound)
    {
      base.SetShape(shape, compound);
      this.CreateMountPoints();
      MySlimBlock cubeBlock = this.Block.CubeGrid.GetCubeBlock(this.Block.Position);
      cubeBlock?.CubeGrid.UpdateBlockNeighbours(cubeBlock);
      if (this.Block.CubeGrid.Physics == null)
        return;
      this.Block.CubeGrid.Physics.AddDirtyBlock(this.Block);
    }

    public override bool RemoveChildShapes(IEnumerable<string> shapeNames)
    {
      base.RemoveChildShapes(shapeNames);
      if (!this.Shape.IsValid() || this.Shape.GetChildrenCount() == 0)
      {
        this.MountPoints = Array.Empty<MyCubeBlockDefinition.MountPoint>();
        if (Sync.IsServer)
          return true;
        this.Block.FatBlock.Components.Remove<MyFractureComponentBase>();
      }
      return false;
    }

    private void Init(MyObjectBuilder_ComponentBase builder)
    {
      MyObjectBuilder_FractureComponentCubeBlock componentCubeBlock = builder as MyObjectBuilder_FractureComponentCubeBlock;
      if (componentCubeBlock.Shapes.Count == 0)
        throw new Exception("No relevant shape was found for fractured block. It was probably reexported and names changed. Block definition: " + this.Block.BlockDefinition.Id.ToString());
      this.RecreateShape(componentCubeBlock.Shapes);
    }

    public void OnCubeGridChanged()
    {
      this.m_tmpShapeList.Clear();
      this.GetCurrentFracturedShapeList(this.m_tmpShapeList);
      this.RecreateShape(this.m_tmpShapeList);
      this.m_tmpShapeList.Clear();
    }

    protected override void RecreateShape(
      List<MyObjectBuilder_FractureComponentBase.FracturedShape> shapeList)
    {
      if (this.Shape.IsValid())
      {
        this.Shape.RemoveReference();
        this.Shape = new HkdBreakableShape();
      }
      if (this.Block.FatBlock.Render is MyRenderComponentFracturedPiece render)
      {
        render.ClearModels();
        render.UpdateRenderObject(false);
      }
      if (shapeList.Count == 0)
        return;
      List<HkdShapeInstanceInfo> shapeInstanceInfoList = new List<HkdShapeInstanceInfo>();
      MyCubeBlockDefinition blockDefinition = this.Block.BlockDefinition;
      string model = blockDefinition.Model;
      if (MyModels.GetModelOnlyData(model).HavokBreakableShapes == null)
        MyDestructionData.Static.LoadModelDestruction(model, (MyPhysicalModelDefinition) blockDefinition, Vector3.One);
      HkdBreakableShape havokBreakableShape1 = MyModels.GetModelOnlyData(model).HavokBreakableShapes[0];
      HkdShapeInstanceInfo shapeInstanceInfo1 = new HkdShapeInstanceInfo(havokBreakableShape1, new Quaternion?(), new Vector3?());
      shapeInstanceInfoList.Add(shapeInstanceInfo1);
      this.m_tmpChildren.Add(shapeInstanceInfo1);
      havokBreakableShape1.GetChildren(this.m_tmpChildren);
      if (blockDefinition.BuildProgressModels != null)
      {
        foreach (MyCubeBlockDefinition.BuildProgressModel buildProgressModel in blockDefinition.BuildProgressModels)
        {
          string file = buildProgressModel.File;
          if (MyModels.GetModelOnlyData(file).HavokBreakableShapes == null)
            MyDestructionData.Static.LoadModelDestruction(file, (MyPhysicalModelDefinition) blockDefinition, Vector3.One);
          HkdBreakableShape havokBreakableShape2 = MyModels.GetModelOnlyData(file).HavokBreakableShapes[0];
          shapeInstanceInfo1 = new HkdShapeInstanceInfo(havokBreakableShape2, new Quaternion?(), new Vector3?());
          shapeInstanceInfoList.Add(shapeInstanceInfo1);
          this.m_tmpChildren.Add(shapeInstanceInfo1);
          havokBreakableShape2.GetChildren(this.m_tmpChildren);
        }
      }
      this.m_tmpShapeListInit.Clear();
      this.m_tmpShapeListInit.AddRange((IEnumerable<MyObjectBuilder_FractureComponentBase.FracturedShape>) shapeList);
      for (int index = 0; index < this.m_tmpChildren.Count; ++index)
      {
        HkdShapeInstanceInfo child = this.m_tmpChildren[index];
        IEnumerable<MyObjectBuilder_FractureComponentBase.FracturedShape> source = this.m_tmpShapeListInit.Where<MyObjectBuilder_FractureComponentBase.FracturedShape>((Func<MyObjectBuilder_FractureComponentBase.FracturedShape, bool>) (s => s.Name == child.ShapeName));
        if (source.Count<MyObjectBuilder_FractureComponentBase.FracturedShape>() > 0)
        {
          MyObjectBuilder_FractureComponentBase.FracturedShape fracturedShape = source.First<MyObjectBuilder_FractureComponentBase.FracturedShape>();
          HkdShapeInstanceInfo shapeInstanceInfo2 = new HkdShapeInstanceInfo(child.Shape.Clone(), Matrix.Identity);
          if (fracturedShape.Fixed)
            shapeInstanceInfo2.Shape.SetFlagRecursively(HkdBreakableShape.Flags.IS_FIXED);
          shapeInstanceInfoList.Add(shapeInstanceInfo2);
          this.m_tmpShapeInfos.Add(shapeInstanceInfo2);
          this.m_tmpShapeListInit.Remove(fracturedShape);
        }
        else
          child.GetChildren(this.m_tmpChildren);
      }
      this.m_tmpShapeListInit.Clear();
      if (shapeList.Count > 0 && this.m_tmpShapeInfos.Count == 0)
      {
        this.m_tmpChildren.Clear();
        throw new Exception("No relevant shape was found for fractured block. It was probably reexported and names changed. Block definition: " + this.Block.BlockDefinition.Id.ToString());
      }
      if (render != null)
      {
        foreach (HkdShapeInstanceInfo tmpShapeInfo in this.m_tmpShapeInfos)
        {
          if (!string.IsNullOrEmpty(tmpShapeInfo.Shape.Name))
            render.AddPiece(tmpShapeInfo.Shape.Name, (MatrixD) ref Matrix.Identity);
        }
        render.UpdateRenderObject(true);
      }
      this.m_tmpChildren.Clear();
      if (this.Block.CubeGrid.CreatePhysics)
      {
        HkdBreakableShape hkdBreakableShape = (HkdBreakableShape) new HkdCompoundBreakableShape((HkdBreakableShape) null, this.m_tmpShapeInfos);
        ((HkdCompoundBreakableShape) hkdBreakableShape).RecalcMassPropsFromChildren();
        HkMassProperties hkMassProperties = new HkMassProperties();
        hkdBreakableShape.BuildMassProperties(ref hkMassProperties);
        this.Shape = new HkdBreakableShape(hkdBreakableShape.GetShape(), ref hkMassProperties);
        hkdBreakableShape.RemoveReference();
        foreach (HkdShapeInstanceInfo tmpShapeInfo in this.m_tmpShapeInfos)
          this.Shape.AddShape(ref tmpShapeInfo);
        this.Shape.SetStrenght(MyDestructionConstants.STRENGTH);
        this.CreateMountPoints();
        MySlimBlock cubeBlock = this.Block.CubeGrid.GetCubeBlock(this.Block.Position);
        cubeBlock?.CubeGrid.UpdateBlockNeighbours(cubeBlock);
        if (this.Block.CubeGrid.Physics != null)
          this.Block.CubeGrid.Physics.AddDirtyBlock(this.Block);
      }
      foreach (HkdShapeInstanceInfo tmpShapeInfo in this.m_tmpShapeInfos)
        tmpShapeInfo.Shape.RemoveReference();
      this.m_tmpShapeInfos.Clear();
      foreach (HkdShapeInstanceInfo shapeInstanceInfo2 in shapeInstanceInfoList)
        shapeInstanceInfo2.RemoveReference();
    }

    private void CreateMountPoints()
    {
      if (MyFakes.FRACTURED_BLOCK_AABB_MOUNT_POINTS)
      {
        List<MyCubeBlockDefinition.MountPoint> outMountPoints = new List<MyCubeBlockDefinition.MountPoint>();
        Vector3 vector3 = new Vector3(this.Block.BlockDefinition.Size);
        BoundingBox blockBB = new BoundingBox(-vector3 / 2f, vector3 / 2f);
        Vector3 halfExtents = blockBB.HalfExtents;
        blockBB.Min += halfExtents;
        blockBB.Max += halfExtents;
        this.Shape.GetChildren(this.m_tmpChildren);
        if (this.m_tmpChildren.Count > 0)
        {
          foreach (HkdShapeInstanceInfo tmpChild in this.m_tmpChildren)
            MyFractureComponentCubeBlock.AddMountForShape(tmpChild.Shape, Matrix.Identity, ref blockBB, this.Block.CubeGrid.GridSize, outMountPoints);
        }
        else
          MyFractureComponentCubeBlock.AddMountForShape(this.Shape, Matrix.Identity, ref blockBB, this.Block.CubeGrid.GridSize, outMountPoints);
        this.MountPoints = outMountPoints.ToArray();
        this.m_tmpChildren.Clear();
      }
      else
        this.MountPoints = MyCubeBuilder.AutogenerateMountpoints(new HkShape[1]
        {
          this.Shape.GetShape()
        }, this.Block.CubeGrid.GridSize);
    }

    public static HkdBreakableShape AddMountForShape(
      HkdBreakableShape shape,
      Matrix transform,
      ref BoundingBox blockBB,
      float gridSize,
      List<MyCubeBlockDefinition.MountPoint> outMountPoints)
    {
      Vector4 min;
      Vector4 max;
      shape.GetShape().GetLocalAABB(0.01f, out min, out max);
      BoundingBox box = new BoundingBox(new Vector3(min), new Vector3(max));
      box = box.Transform(transform);
      box.Min /= gridSize;
      box.Max /= gridSize;
      box.Inflate(0.04f);
      box.Min += blockBB.HalfExtents;
      box.Max += blockBB.HalfExtents;
      if (blockBB.Contains(box) == ContainmentType.Intersects)
      {
        box.Inflate(-0.04f);
        foreach (int enumDirection in Base6Directions.EnumDirections)
        {
          Vector3 direction = Base6Directions.Directions[enumDirection];
          Vector3 vector3_1 = Vector3.Abs(direction);
          MyCubeBlockDefinition.MountPoint mountPoint = new MyCubeBlockDefinition.MountPoint();
          mountPoint.Start = box.Min;
          mountPoint.End = box.Max;
          mountPoint.Enabled = true;
          mountPoint.PressurizedWhenOpen = true;
          Vector3 vector3_2 = mountPoint.Start * vector3_1 / (blockBB.HalfExtents * 2f) - vector3_1 * 0.04f;
          Vector3 vector3_3 = mountPoint.End * vector3_1 / (blockBB.HalfExtents * 2f) + vector3_1 * 0.04f;
          bool flag1 = false;
          bool flag2 = false;
          if ((double) vector3_2.Max() < 1.0 && (double) vector3_3.Max() > 1.0 && (double) direction.Max() > 0.0)
          {
            flag1 = true;
            flag2 = true;
          }
          else if ((double) vector3_2.Min() < 0.0 && (double) vector3_3.Max() > 0.0 && (double) direction.Min() < 0.0)
            flag1 = true;
          if (flag1)
          {
            mountPoint.Start -= mountPoint.Start * vector3_1 - vector3_1 * 0.04f;
            mountPoint.End -= mountPoint.End * vector3_1 + vector3_1 * 0.04f;
            if (flag2)
            {
              mountPoint.Start += vector3_1 * blockBB.HalfExtents * 2f;
              mountPoint.End += vector3_1 * blockBB.HalfExtents * 2f;
            }
            mountPoint.Start -= blockBB.HalfExtents - Vector3.One / 2f;
            mountPoint.End -= blockBB.HalfExtents - Vector3.One / 2f;
            mountPoint.Normal = new Vector3I(direction);
            outMountPoints.Add(mountPoint);
          }
        }
      }
      return shape;
    }

    public float GetIntegrityRatioFromFracturedPieceCounts()
    {
      if (this.Shape.IsValid() && this.Block != null)
      {
        int shapeChildrenCount = this.Block.GetTotalBreakableShapeChildrenCount();
        if (shapeChildrenCount > 0)
        {
          int totalChildrenCount = this.Shape.GetTotalChildrenCount();
          if (totalChildrenCount <= shapeChildrenCount)
            return (float) totalChildrenCount / (float) shapeChildrenCount;
        }
      }
      return 0.0f;
    }

    private class MyFractureComponentBlockDebugRender : MyDebugRenderComponentBase
    {
      private MyCubeBlock m_block;

      public MyFractureComponentBlockDebugRender(MyCubeBlock b) => this.m_block = b;

      public override void DebugDraw()
      {
        if (!MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS || !this.m_block.Components.Has<MyFractureComponentBase>())
          return;
        MyFractureComponentCubeBlock fractureComponent = this.m_block.GetFractureComponent();
        if (fractureComponent == null)
          return;
        MatrixD drawMatrix = this.m_block.CubeGrid.PositionComp.WorldMatrixRef;
        drawMatrix.Translation = this.m_block.CubeGrid.GridIntegerToWorld(this.m_block.Position);
        MyCubeBuilder.DrawMountPoints(this.m_block.CubeGrid.GridSize, this.m_block.BlockDefinition, drawMatrix, ((IEnumerable<MyCubeBlockDefinition.MountPoint>) fractureComponent.MountPoints).ToArray<MyCubeBlockDefinition.MountPoint>());
      }

      public override void DebugDrawInvalidTriangles()
      {
      }
    }

    private class Sandbox_Game_EntityComponents_MyFractureComponentCubeBlock\u003C\u003EActor : IActivator, IActivator<MyFractureComponentCubeBlock>
    {
      object IActivator.CreateInstance() => (object) new MyFractureComponentCubeBlock();

      MyFractureComponentCubeBlock IActivator<MyFractureComponentCubeBlock>.CreateInstance() => new MyFractureComponentCubeBlock();
    }
  }
}
