// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyCompoundCubeBlock
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.Entities
{
  [MyCubeBlockType(typeof (MyObjectBuilder_CompoundCubeBlock))]
  public class MyCompoundCubeBlock : MyCubeBlock, IMyDecalProxy
  {
    private static List<VertexArealBoneIndexWeight> m_boneIndexWeightTmp;
    private static readonly string COMPOUND_DUMMY = "compound_";
    private static readonly ushort BLOCK_IN_COMPOUND_LOCAL_ID = 32768;
    private static readonly ushort BLOCK_IN_COMPOUND_LOCAL_MAX_VALUE = (ushort) short.MaxValue;
    private static readonly MyStringId BUILD_TYPE_ANY = MyStringId.GetOrCompute("any");
    private static readonly string COMPOUND_BLOCK_SUBTYPE_NAME = "CompoundBlock";
    private static readonly HashSet<string> m_tmpTemplates = new HashSet<string>();
    private static readonly List<MyModelDummy> m_tmpDummies = new List<MyModelDummy>();
    private static readonly List<MyModelDummy> m_tmpOtherDummies = new List<MyModelDummy>();
    private readonly Dictionary<ushort, MySlimBlock> m_mapIdToBlock = new Dictionary<ushort, MySlimBlock>();
    private readonly List<MySlimBlock> m_blocks = new List<MySlimBlock>();
    private ushort m_nextId;
    private ushort m_localNextId;
    private readonly HashSet<string> m_templates = new HashSet<string>();
    private static readonly List<uint> m_tmpIds = new List<uint>();

    public MyCompoundCubeBlock()
    {
      this.PositionComp = (MyPositionComponentBase) new MyCompoundCubeBlock.MyCompoundBlockPosComponent();
      this.Render = (MyRenderComponentBase) new MyRenderComponentCompoundCubeBlock();
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      base.Init(objectBuilder, cubeGrid);
      MyObjectBuilder_CompoundCubeBlock compoundCubeBlock = objectBuilder as MyObjectBuilder_CompoundCubeBlock;
      if (compoundCubeBlock.Blocks != null)
      {
        if (compoundCubeBlock.BlockIds != null)
        {
          for (int index = 0; index < compoundCubeBlock.Blocks.Length; ++index)
          {
            ushort blockId = compoundCubeBlock.BlockIds[index];
            if (!this.m_mapIdToBlock.ContainsKey(blockId))
            {
              MyObjectBuilder_CubeBlock block = compoundCubeBlock.Blocks[index];
              object cubeBlock = MyCubeBlockFactory.CreateCubeBlock(block);
              if (!(cubeBlock is MySlimBlock mySlimBlock))
                mySlimBlock = new MySlimBlock();
              mySlimBlock.Init(block, cubeGrid, cubeBlock as MyCubeBlock);
              if (mySlimBlock.FatBlock != null)
              {
                mySlimBlock.FatBlock.HookMultiplayer();
                mySlimBlock.FatBlock.Hierarchy.Parent = (MyHierarchyComponentBase) this.Hierarchy;
                this.m_mapIdToBlock.Add(blockId, mySlimBlock);
                this.m_blocks.Add(mySlimBlock);
              }
            }
          }
          this.RefreshNextId();
        }
        else
        {
          for (int index = 0; index < compoundCubeBlock.Blocks.Length; ++index)
          {
            MyObjectBuilder_CubeBlock block1 = compoundCubeBlock.Blocks[index];
            object cubeBlock = MyCubeBlockFactory.CreateCubeBlock(block1);
            if (!(cubeBlock is MySlimBlock block))
              block = new MySlimBlock();
            block.Init(block1, cubeGrid, cubeBlock as MyCubeBlock);
            block.FatBlock.HookMultiplayer();
            block.FatBlock.Hierarchy.Parent = (MyHierarchyComponentBase) this.Hierarchy;
            this.m_mapIdToBlock.Add(this.CreateId(block), block);
            this.m_blocks.Add(block);
          }
        }
      }
      this.RefreshTemplates();
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyCompoundCubeBlock.MyDebugRenderComponentCompoundBlock(this));
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_CompoundCubeBlock builderCubeBlock = (MyObjectBuilder_CompoundCubeBlock) base.GetObjectBuilderCubeBlock(copy);
      if (this.m_mapIdToBlock.Count > 0)
      {
        builderCubeBlock.Blocks = new MyObjectBuilder_CubeBlock[this.m_mapIdToBlock.Count];
        builderCubeBlock.BlockIds = new ushort[this.m_mapIdToBlock.Count];
        int index = 0;
        foreach (KeyValuePair<ushort, MySlimBlock> keyValuePair in this.m_mapIdToBlock)
        {
          builderCubeBlock.BlockIds[index] = keyValuePair.Key;
          builderCubeBlock.Blocks[index] = copy ? keyValuePair.Value.GetCopyObjectBuilder() : keyValuePair.Value.GetObjectBuilder(false);
          ++index;
        }
      }
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public override void OnAddedToScene(object source)
    {
      foreach (KeyValuePair<ushort, MySlimBlock> keyValuePair in this.m_mapIdToBlock)
      {
        if (keyValuePair.Value.FatBlock != null)
          keyValuePair.Value.FatBlock.OnAddedToScene(source);
      }
      base.OnAddedToScene(source);
    }

    public override void OnRemovedFromScene(object source)
    {
      foreach (KeyValuePair<ushort, MySlimBlock> keyValuePair in this.m_mapIdToBlock)
      {
        if (keyValuePair.Value.FatBlock != null)
          keyValuePair.Value.FatBlock.OnRemovedFromScene(source);
      }
      base.OnRemovedFromScene(source);
    }

    public override void UpdateVisual()
    {
      foreach (KeyValuePair<ushort, MySlimBlock> keyValuePair in this.m_mapIdToBlock)
      {
        if (keyValuePair.Value.FatBlock != null)
          keyValuePair.Value.FatBlock.UpdateVisual();
      }
      base.UpdateVisual();
    }

    public override float GetMass()
    {
      float num = 0.0f;
      foreach (KeyValuePair<ushort, MySlimBlock> keyValuePair in this.m_mapIdToBlock)
        num += keyValuePair.Value.GetMass();
      return num;
    }

    private void UpdateBlocksWorldMatrix(ref MatrixD parentWorldMatrix, object source = null)
    {
      MatrixD identity = MatrixD.Identity;
      foreach (KeyValuePair<ushort, MySlimBlock> keyValuePair in this.m_mapIdToBlock)
      {
        if (keyValuePair.Value.FatBlock != null)
        {
          MyCompoundCubeBlock.GetBlockLocalMatrixFromGridPositionAndOrientation(keyValuePair.Value, ref identity);
          MatrixD worldMatrix = identity * parentWorldMatrix;
          keyValuePair.Value.FatBlock.PositionComp.SetWorldMatrix(ref worldMatrix, (object) this, true);
        }
      }
    }

    protected override void Closing()
    {
      foreach (KeyValuePair<ushort, MySlimBlock> keyValuePair in this.m_mapIdToBlock)
      {
        if (keyValuePair.Value.FatBlock != null)
          keyValuePair.Value.FatBlock.Close();
      }
      base.Closing();
    }

    public override void OnCubeGridChanged(MyCubeGrid oldGrid)
    {
      base.OnCubeGridChanged(oldGrid);
      foreach (KeyValuePair<ushort, MySlimBlock> keyValuePair in this.m_mapIdToBlock)
        keyValuePair.Value.CubeGrid = this.CubeGrid;
    }

    internal override void OnTransformed(ref MatrixI transform)
    {
      foreach (KeyValuePair<ushort, MySlimBlock> keyValuePair in this.m_mapIdToBlock)
        keyValuePair.Value.Transform(ref transform);
    }

    internal override void UpdateWorldMatrix()
    {
      base.UpdateWorldMatrix();
      foreach (KeyValuePair<ushort, MySlimBlock> keyValuePair in this.m_mapIdToBlock)
      {
        if (keyValuePair.Value.FatBlock != null)
          keyValuePair.Value.FatBlock.UpdateWorldMatrix();
      }
    }

    public override bool ConnectionAllowed(
      ref Vector3I otherBlockPos,
      ref Vector3I faceNormal,
      MyCubeBlockDefinition def)
    {
      foreach (KeyValuePair<ushort, MySlimBlock> keyValuePair in this.m_mapIdToBlock)
      {
        if (keyValuePair.Value.FatBlock != null && keyValuePair.Value.FatBlock.ConnectionAllowed(ref otherBlockPos, ref faceNormal, def))
          return true;
      }
      return false;
    }

    public override bool ConnectionAllowed(
      ref Vector3I otherBlockMinPos,
      ref Vector3I otherBlockMaxPos,
      ref Vector3I faceNormal,
      MyCubeBlockDefinition def)
    {
      foreach (KeyValuePair<ushort, MySlimBlock> keyValuePair in this.m_mapIdToBlock)
      {
        if (keyValuePair.Value.FatBlock != null && keyValuePair.Value.FatBlock.ConnectionAllowed(ref otherBlockMinPos, ref otherBlockMaxPos, ref faceNormal, def))
          return true;
      }
      return false;
    }

    public bool Add(MySlimBlock block, out ushort id)
    {
      id = this.CreateId(block);
      return this.Add(id, block);
    }

    public bool Add(ushort id, MySlimBlock block)
    {
      if (!this.CanAddBlock(block) || this.m_mapIdToBlock.ContainsKey(id))
        return false;
      this.m_mapIdToBlock.Add(id, block);
      this.m_blocks.Add(block);
      MatrixD worldMatrix1 = this.Parent.WorldMatrix;
      MatrixD identity = MatrixD.Identity;
      MyCompoundCubeBlock.GetBlockLocalMatrixFromGridPositionAndOrientation(block, ref identity);
      MatrixD worldMatrix2 = identity * worldMatrix1;
      block.FatBlock.PositionComp.SetWorldMatrix(ref worldMatrix2, (object) this, true);
      block.FatBlock.Hierarchy.Parent = (MyHierarchyComponentBase) this.Hierarchy;
      block.FatBlock.OnAddedToScene((object) this);
      this.CubeGrid.UpdateBlockNeighbours(this.SlimBlock);
      this.RefreshTemplates();
      if (block.IsMultiBlockPart)
        this.CubeGrid.AddMultiBlockInfo(block);
      return true;
    }

    public bool Remove(MySlimBlock block, bool merged = false)
    {
      KeyValuePair<ushort, MySlimBlock> keyValuePair = this.m_mapIdToBlock.FirstOrDefault<KeyValuePair<ushort, MySlimBlock>>((Func<KeyValuePair<ushort, MySlimBlock>, bool>) (p => p.Value == block));
      return keyValuePair.Value == block && this.Remove(keyValuePair.Key, merged);
    }

    public bool Remove(ushort blockId, bool merged = false)
    {
      MySlimBlock block;
      if (!this.m_mapIdToBlock.TryGetValue(blockId, out block))
        return false;
      this.m_mapIdToBlock.Remove(blockId);
      this.m_blocks.Remove(block);
      if (!merged)
      {
        if (block.IsMultiBlockPart)
          this.CubeGrid.RemoveMultiBlockInfo(block);
        block.FatBlock.OnRemovedFromScene((object) this);
        block.FatBlock.Close();
      }
      if (block.FatBlock.Hierarchy.Parent == this.Hierarchy)
        block.FatBlock.Hierarchy.Parent = (MyHierarchyComponentBase) null;
      if (!merged)
        this.CubeGrid.UpdateBlockNeighbours(this.SlimBlock);
      this.RefreshTemplates();
      return true;
    }

    public bool CanAddBlock(MySlimBlock block)
    {
      if (block == null || block.FatBlock == null || this.m_mapIdToBlock.ContainsValue(block))
        return false;
      if (!(block.FatBlock is MyCompoundCubeBlock))
        return this.CanAddBlock(block.BlockDefinition, new MyBlockOrientation?(block.Orientation), block.MultiBlockId);
      foreach (MySlimBlock block1 in (block.FatBlock as MyCompoundCubeBlock).GetBlocks())
      {
        if (!this.CanAddBlock(block1.BlockDefinition, new MyBlockOrientation?(block1.Orientation), block1.MultiBlockId))
          return false;
      }
      return true;
    }

    public bool CanAddBlock(
      MyCubeBlockDefinition definition,
      MyBlockOrientation? orientation,
      int multiBlockId = 0,
      bool ignoreSame = false)
    {
      if (!MyCompoundCubeBlock.IsCompoundEnabled(definition))
        return false;
      if (MyFakes.ENABLE_COMPOUND_BLOCK_COLLISION_DUMMIES)
      {
        if (!orientation.HasValue)
          return false;
        if (this.m_blocks.Count == 0)
          return true;
        Matrix result1;
        orientation.Value.GetMatrix(out result1);
        MyCompoundCubeBlock.m_tmpOtherDummies.Clear();
        MyCompoundCubeBlock.GetCompoundCollisionDummies(definition, MyCompoundCubeBlock.m_tmpOtherDummies);
        foreach (MySlimBlock block in this.m_blocks)
        {
          if (block.BlockDefinition.Id.SubtypeId == definition.Id.SubtypeId && block.Orientation == orientation.Value)
          {
            if (!ignoreSame)
              return false;
          }
          else if ((multiBlockId == 0 || block.MultiBlockId != multiBlockId) && !block.BlockDefinition.IsGeneratedBlock)
          {
            MyCompoundCubeBlock.m_tmpDummies.Clear();
            MyCompoundCubeBlock.GetCompoundCollisionDummies(block.BlockDefinition, MyCompoundCubeBlock.m_tmpDummies);
            Matrix result2;
            block.Orientation.GetMatrix(out result2);
            if (MyCompoundCubeBlock.CompoundDummiesIntersect(ref result2, ref result1, MyCompoundCubeBlock.m_tmpDummies, MyCompoundCubeBlock.m_tmpOtherDummies))
            {
              MyCompoundCubeBlock.m_tmpDummies.Clear();
              MyCompoundCubeBlock.m_tmpOtherDummies.Clear();
              return false;
            }
          }
        }
        MyCompoundCubeBlock.m_tmpDummies.Clear();
        MyCompoundCubeBlock.m_tmpOtherDummies.Clear();
        return true;
      }
      if (orientation.HasValue)
      {
        foreach (KeyValuePair<ushort, MySlimBlock> keyValuePair in this.m_mapIdToBlock)
        {
          MyBlockOrientation? nullable;
          if (keyValuePair.Value.BlockDefinition.Id.SubtypeId == definition.Id.SubtypeId)
          {
            MyBlockOrientation orientation1 = keyValuePair.Value.Orientation;
            nullable = orientation;
            if ((nullable.HasValue ? (orientation1 == nullable.GetValueOrDefault() ? 1 : 0) : 0) != 0)
              return false;
          }
          MyStringId buildType = definition.BuildType;
          if (keyValuePair.Value.BlockDefinition.BuildType == definition.BuildType)
          {
            MyBlockOrientation orientation1 = keyValuePair.Value.Orientation;
            nullable = orientation;
            if ((nullable.HasValue ? (orientation1 == nullable.GetValueOrDefault() ? 1 : 0) : 0) != 0)
              return false;
          }
        }
      }
      foreach (string compoundTemplate in definition.CompoundTemplates)
      {
        if (this.m_templates.Contains(compoundTemplate))
        {
          MyCompoundBlockTemplateDefinition templateDefinition = MyCompoundCubeBlock.GetTemplateDefinition(compoundTemplate);
          if (templateDefinition != null && templateDefinition.Bindings != null)
          {
            MyCompoundBlockTemplateDefinition.MyCompoundBlockBinding definitionBinding1 = MyCompoundCubeBlock.GetTemplateDefinitionBinding(templateDefinition, definition);
            if (definitionBinding1 != null)
            {
              if (definitionBinding1.BuildType == MyCompoundCubeBlock.BUILD_TYPE_ANY)
                return true;
              if (!definitionBinding1.Multiple)
              {
                bool flag = false;
                foreach (KeyValuePair<ushort, MySlimBlock> keyValuePair in this.m_mapIdToBlock)
                {
                  if (keyValuePair.Value.BlockDefinition.BuildType == definition.BuildType)
                  {
                    flag = true;
                    break;
                  }
                }
                if (flag)
                  continue;
              }
              if (orientation.HasValue)
              {
                bool flag = false;
                foreach (KeyValuePair<ushort, MySlimBlock> keyValuePair in this.m_mapIdToBlock)
                {
                  MyCompoundBlockTemplateDefinition.MyCompoundBlockBinding definitionBinding2 = MyCompoundCubeBlock.GetTemplateDefinitionBinding(templateDefinition, keyValuePair.Value.BlockDefinition);
                  if (definitionBinding2 != null && !(definitionBinding2.BuildType == MyCompoundCubeBlock.BUILD_TYPE_ANY))
                  {
                    MyCompoundBlockTemplateDefinition.MyCompoundBlockRotationBinding rotationBinding = MyCompoundCubeBlock.GetRotationBinding(templateDefinition, definition, keyValuePair.Value.BlockDefinition);
                    if (rotationBinding != null)
                    {
                      if (rotationBinding.BuildTypeReference == definition.BuildType)
                      {
                        if (this.IsRotationValid(orientation.Value, keyValuePair.Value.Orientation, rotationBinding.Rotations) || rotationBinding.BuildTypeReference == keyValuePair.Value.BlockDefinition.BuildType && this.IsRotationValid(keyValuePair.Value.Orientation, orientation.Value, rotationBinding.Rotations))
                          continue;
                      }
                      else if (this.IsRotationValid(keyValuePair.Value.Orientation, orientation.Value, rotationBinding.Rotations))
                        continue;
                      flag = true;
                      break;
                    }
                  }
                }
                if (flag)
                  continue;
              }
              return true;
            }
          }
        }
      }
      return false;
    }

    public static bool CanAddBlocks(
      MyCubeBlockDefinition definition,
      MyBlockOrientation orientation,
      MyCubeBlockDefinition otherDefinition,
      MyBlockOrientation otherOrientation)
    {
      if (!MyCompoundCubeBlock.IsCompoundEnabled(definition) || !MyCompoundCubeBlock.IsCompoundEnabled(otherDefinition))
        return false;
      if (!MyFakes.ENABLE_COMPOUND_BLOCK_COLLISION_DUMMIES)
        return true;
      Matrix result1;
      orientation.GetMatrix(out result1);
      MyCompoundCubeBlock.m_tmpDummies.Clear();
      MyCompoundCubeBlock.GetCompoundCollisionDummies(definition, MyCompoundCubeBlock.m_tmpDummies);
      Matrix result2;
      otherOrientation.GetMatrix(out result2);
      MyCompoundCubeBlock.m_tmpOtherDummies.Clear();
      MyCompoundCubeBlock.GetCompoundCollisionDummies(otherDefinition, MyCompoundCubeBlock.m_tmpOtherDummies);
      int num = MyCompoundCubeBlock.CompoundDummiesIntersect(ref result1, ref result2, MyCompoundCubeBlock.m_tmpDummies, MyCompoundCubeBlock.m_tmpOtherDummies) ? 1 : 0;
      MyCompoundCubeBlock.m_tmpDummies.Clear();
      MyCompoundCubeBlock.m_tmpOtherDummies.Clear();
      return num == 0;
    }

    private static bool CompoundDummiesIntersect(
      ref Matrix thisRotation,
      ref Matrix otherRotation,
      List<MyModelDummy> thisDummies,
      List<MyModelDummy> otherDummies)
    {
      foreach (MyModelDummy thisDummy in thisDummies)
      {
        Vector3 max1 = new Vector3(thisDummy.Matrix.Right.Length(), thisDummy.Matrix.Up.Length(), thisDummy.Matrix.Forward.Length()) * 0.5f;
        BoundingBox box = new BoundingBox(-max1, max1);
        Matrix result1 = Matrix.Normalize(thisDummy.Matrix);
        Matrix result2;
        Matrix.Multiply(ref result1, ref thisRotation, out result2);
        Matrix.Invert(ref result2, out result1);
        foreach (MyModelDummy otherDummy in otherDummies)
        {
          Vector3 max2 = new Vector3(otherDummy.Matrix.Right.Length(), otherDummy.Matrix.Up.Length(), otherDummy.Matrix.Forward.Length()) * 0.5f;
          BoundingBox boundingBox = new BoundingBox(-max2, max2);
          Matrix result3 = Matrix.Normalize(otherDummy.Matrix);
          Matrix result4;
          Matrix.Multiply(ref result3, ref otherRotation, out result4);
          Matrix.Multiply(ref result4, ref result1, out result3);
          Matrix matrix = result3;
          if (MyOrientedBoundingBox.Create(boundingBox, matrix).Intersects(ref box))
            return true;
        }
      }
      return false;
    }

    private void DebugDrawAABB(BoundingBox aabb, Matrix localMatrix)
    {
      MatrixD matrix = Matrix.CreateScale(2f * aabb.HalfExtents) * (MatrixD) ref localMatrix * this.PositionComp.WorldMatrixRef;
      MyRenderProxy.DebugDrawAxis(MatrixD.Normalize(matrix), 0.1f, false);
      MyRenderProxy.DebugDrawOBB(matrix, Color.Green, 0.1f, false, false);
    }

    private void DebugDrawOBB(MyOrientedBoundingBox obb, Matrix localMatrix)
    {
      MatrixD matrix = Matrix.CreateFromTransformScale(obb.Orientation, obb.Center, 2f * obb.HalfExtent) * (MatrixD) ref localMatrix * this.PositionComp.WorldMatrixRef;
      MyRenderProxy.DebugDrawAxis(MatrixD.Normalize(matrix), 0.1f, false);
      MyRenderProxy.DebugDrawOBB(matrix, (Color) Vector3.One, 0.1f, false, false);
    }

    private bool IsRotationValid(
      MyBlockOrientation refOrientation,
      MyBlockOrientation orientation,
      MyBlockOrientation[] validRotations)
    {
      MatrixI matrix = new MatrixI(Vector3I.Zero, refOrientation.Forward, refOrientation.Up);
      MatrixI result;
      MatrixI.Invert(ref matrix, out result);
      Matrix floatMatrix = result.GetFloatMatrix();
      Base6Directions.Direction closestDirection1 = Base6Directions.GetClosestDirection(Vector3.TransformNormal((Vector3) Base6Directions.GetIntVector(orientation.Forward), floatMatrix));
      Base6Directions.Direction closestDirection2 = Base6Directions.GetClosestDirection(Vector3.TransformNormal((Vector3) Base6Directions.GetIntVector(orientation.Up), floatMatrix));
      foreach (MyBlockOrientation validRotation in validRotations)
      {
        if (validRotation.Forward == closestDirection1 && validRotation.Up == closestDirection2)
          return true;
      }
      return false;
    }

    public MySlimBlock GetBlock(ushort id)
    {
      MySlimBlock mySlimBlock;
      return this.m_mapIdToBlock.TryGetValue(id, out mySlimBlock) ? mySlimBlock : (MySlimBlock) null;
    }

    public ushort? GetBlockId(MySlimBlock block)
    {
      KeyValuePair<ushort, MySlimBlock> keyValuePair = this.m_mapIdToBlock.FirstOrDefault<KeyValuePair<ushort, MySlimBlock>>((Func<KeyValuePair<ushort, MySlimBlock>, bool>) (p => p.Value == block));
      return keyValuePair.Value == block ? new ushort?(keyValuePair.Key) : new ushort?();
    }

    public ListReader<MySlimBlock> GetBlocks() => (ListReader<MySlimBlock>) this.m_blocks;

    public int GetBlocksCount() => this.m_blocks.Count;

    public static MyObjectBuilder_CompoundCubeBlock CreateBuilder(
      MyObjectBuilder_CubeBlock cubeBlockBuilder)
    {
      MyObjectBuilder_CompoundCubeBlock newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_CompoundCubeBlock>(MyCompoundCubeBlock.COMPOUND_BLOCK_SUBTYPE_NAME);
      newObject.EntityId = MyEntityIdentifier.AllocateId();
      newObject.Min = cubeBlockBuilder.Min;
      newObject.BlockOrientation = (SerializableBlockOrientation) new MyBlockOrientation(ref Quaternion.Identity);
      newObject.ColorMaskHSV = cubeBlockBuilder.ColorMaskHSV;
      newObject.Blocks = new MyObjectBuilder_CubeBlock[1];
      newObject.Blocks[0] = cubeBlockBuilder;
      return newObject;
    }

    public static MyObjectBuilder_CompoundCubeBlock CreateBuilder(
      List<MyObjectBuilder_CubeBlock> cubeBlockBuilders)
    {
      MyObjectBuilder_CompoundCubeBlock newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_CompoundCubeBlock>(MyCompoundCubeBlock.COMPOUND_BLOCK_SUBTYPE_NAME);
      newObject.EntityId = MyEntityIdentifier.AllocateId();
      newObject.Min = cubeBlockBuilders[0].Min;
      newObject.BlockOrientation = (SerializableBlockOrientation) new MyBlockOrientation(ref Quaternion.Identity);
      newObject.ColorMaskHSV = cubeBlockBuilders[0].ColorMaskHSV;
      newObject.Blocks = cubeBlockBuilders.ToArray();
      return newObject;
    }

    public static MyCubeBlockDefinition GetCompoundCubeBlockDefinition() => MyDefinitionManager.Static.GetCubeBlockDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_CompoundCubeBlock), MyCompoundCubeBlock.COMPOUND_BLOCK_SUBTYPE_NAME));

    private static MyCompoundBlockTemplateDefinition GetTemplateDefinition(
      string template)
    {
      return MyDefinitionManager.Static.GetCompoundBlockTemplateDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_CompoundBlockTemplateDefinition), template));
    }

    private static MyCompoundBlockTemplateDefinition.MyCompoundBlockBinding GetTemplateDefinitionBinding(
      MyCompoundBlockTemplateDefinition templateDefinition,
      MyCubeBlockDefinition blockDefinition)
    {
      foreach (MyCompoundBlockTemplateDefinition.MyCompoundBlockBinding binding in templateDefinition.Bindings)
      {
        if (binding.BuildType == MyCompoundCubeBlock.BUILD_TYPE_ANY)
          return binding;
      }
      foreach (MyCompoundBlockTemplateDefinition.MyCompoundBlockBinding binding in templateDefinition.Bindings)
      {
        if (binding.BuildType == blockDefinition.BuildType && blockDefinition.BuildType != MyStringId.NullOrEmpty)
          return binding;
      }
      return (MyCompoundBlockTemplateDefinition.MyCompoundBlockBinding) null;
    }

    private static MyCompoundBlockTemplateDefinition.MyCompoundBlockRotationBinding GetRotationBinding(
      MyCompoundBlockTemplateDefinition templateDefinition,
      MyCubeBlockDefinition blockDefinition1,
      MyCubeBlockDefinition blockDefinition2)
    {
      MyCompoundBlockTemplateDefinition.MyCompoundBlockBinding definitionBinding1 = MyCompoundCubeBlock.GetTemplateDefinitionBinding(templateDefinition, blockDefinition1);
      if (definitionBinding1 == null)
        return (MyCompoundBlockTemplateDefinition.MyCompoundBlockRotationBinding) null;
      MyCompoundBlockTemplateDefinition.MyCompoundBlockRotationBinding rotationBinding = MyCompoundCubeBlock.GetRotationBinding(definitionBinding1, blockDefinition2);
      if (rotationBinding != null)
        return rotationBinding;
      MyCompoundBlockTemplateDefinition.MyCompoundBlockBinding definitionBinding2 = MyCompoundCubeBlock.GetTemplateDefinitionBinding(templateDefinition, blockDefinition2);
      return definitionBinding2 == null ? (MyCompoundBlockTemplateDefinition.MyCompoundBlockRotationBinding) null : MyCompoundCubeBlock.GetRotationBinding(definitionBinding2, blockDefinition1);
    }

    private static MyCompoundBlockTemplateDefinition.MyCompoundBlockRotationBinding GetRotationBinding(
      MyCompoundBlockTemplateDefinition.MyCompoundBlockBinding binding,
      MyCubeBlockDefinition blockDefinition)
    {
      if (binding.RotationBinds != null)
      {
        foreach (MyCompoundBlockTemplateDefinition.MyCompoundBlockRotationBinding rotationBind in binding.RotationBinds)
        {
          if (rotationBind.BuildTypeReference == blockDefinition.BuildType)
            return rotationBind;
        }
      }
      return (MyCompoundBlockTemplateDefinition.MyCompoundBlockRotationBinding) null;
    }

    private void RefreshTemplates()
    {
      this.m_templates.Clear();
      if (MyFakes.ENABLE_COMPOUND_BLOCK_COLLISION_DUMMIES)
        return;
      foreach (KeyValuePair<ushort, MySlimBlock> keyValuePair in this.m_mapIdToBlock)
      {
        if (keyValuePair.Value.BlockDefinition.CompoundTemplates != null)
        {
          if (this.m_templates.Count == 0)
          {
            foreach (string compoundTemplate in keyValuePair.Value.BlockDefinition.CompoundTemplates)
              this.m_templates.Add(compoundTemplate);
          }
          else
          {
            MyCompoundCubeBlock.m_tmpTemplates.Clear();
            foreach (string compoundTemplate in keyValuePair.Value.BlockDefinition.CompoundTemplates)
              MyCompoundCubeBlock.m_tmpTemplates.Add(compoundTemplate);
            this.m_templates.IntersectWith((IEnumerable<string>) MyCompoundCubeBlock.m_tmpTemplates);
          }
        }
      }
    }

    public override BoundingBox GetGeometryLocalBox()
    {
      BoundingBox invalid = BoundingBox.CreateInvalid();
      foreach (MySlimBlock block in this.GetBlocks())
      {
        if (block.FatBlock != null)
        {
          Matrix result;
          block.Orientation.GetMatrix(out result);
          invalid.Include(block.FatBlock.Model.BoundingBox.Transform(result));
        }
      }
      return invalid;
    }

    private void RefreshNextId()
    {
      foreach (KeyValuePair<ushort, MySlimBlock> keyValuePair in this.m_mapIdToBlock)
      {
        if (((int) keyValuePair.Key & (int) MyCompoundCubeBlock.BLOCK_IN_COMPOUND_LOCAL_ID) == (int) MyCompoundCubeBlock.BLOCK_IN_COMPOUND_LOCAL_ID)
          this.m_localNextId = Math.Max(this.m_localNextId, (ushort) ((uint) keyValuePair.Key & (uint) ~MyCompoundCubeBlock.BLOCK_IN_COMPOUND_LOCAL_ID));
        else
          this.m_nextId = Math.Max(this.m_nextId, keyValuePair.Key);
      }
      if ((int) this.m_nextId == (int) MyCompoundCubeBlock.BLOCK_IN_COMPOUND_LOCAL_MAX_VALUE)
        this.m_nextId = (ushort) 0;
      else
        ++this.m_nextId;
      if ((int) this.m_localNextId == (int) MyCompoundCubeBlock.BLOCK_IN_COMPOUND_LOCAL_MAX_VALUE)
        this.m_localNextId = (ushort) 0;
      else
        ++this.m_localNextId;
    }

    private ushort CreateId(MySlimBlock block)
    {
      ushort key;
      if ((block.BlockDefinition.IsGeneratedBlock ? 1 : 0) != 0)
      {
        for (key = (ushort) ((uint) this.m_localNextId | (uint) MyCompoundCubeBlock.BLOCK_IN_COMPOUND_LOCAL_ID); this.m_mapIdToBlock.ContainsKey(key); key = (ushort) ((uint) this.m_localNextId | (uint) MyCompoundCubeBlock.BLOCK_IN_COMPOUND_LOCAL_ID))
        {
          if ((int) this.m_localNextId == (int) MyCompoundCubeBlock.BLOCK_IN_COMPOUND_LOCAL_MAX_VALUE)
            this.m_localNextId = (ushort) 0;
          else
            ++this.m_localNextId;
        }
        ++this.m_localNextId;
      }
      else
      {
        for (key = this.m_nextId; this.m_mapIdToBlock.ContainsKey(key); key = this.m_nextId)
        {
          if ((int) this.m_nextId == (int) MyCompoundCubeBlock.BLOCK_IN_COMPOUND_LOCAL_MAX_VALUE)
            this.m_nextId = (ushort) 0;
          else
            ++this.m_nextId;
        }
        ++this.m_nextId;
      }
      return key;
    }

    internal void DoDamage(
      float damage,
      MyStringHash damageType,
      MyHitInfo? hitInfo,
      long attackerId)
    {
      float num = 0.0f;
      foreach (KeyValuePair<ushort, MySlimBlock> keyValuePair in this.m_mapIdToBlock)
        num += keyValuePair.Value.MaxIntegrity;
      for (int index = this.m_blocks.Count - 1; index >= 0; --index)
      {
        MySlimBlock block = this.m_blocks[index];
        block.DoDamage(damage * (block.MaxIntegrity / num), damageType, hitInfo, attackerId: attackerId);
      }
    }

    void IMyDecalProxy.AddDecals(
      ref MyHitInfo hitInfo,
      MyStringHash source,
      Vector3 forwardDirection,
      object customdata,
      IMyDecalHandler decalHandler,
      MyStringHash physicalMaterial,
      MyStringHash voxelMaterial,
      bool isTrail)
    {
      if (!(customdata is MyCubeGrid.MyCubeGridHitInfo gridHitInfo))
        return;
      MyPhysicalMaterialDefinition physicalMaterial1 = this.m_mapIdToBlock.First<KeyValuePair<ushort, MySlimBlock>>().Value.BlockDefinition.PhysicalMaterial;
      MyDecalRenderInfo renderInfo = new MyDecalRenderInfo()
      {
        Position = Vector3D.Transform(hitInfo.Position, this.CubeGrid.PositionComp.WorldMatrixInvScaled),
        Normal = (Vector3) Vector3D.TransformNormal(hitInfo.Normal, this.CubeGrid.PositionComp.WorldMatrixInvScaled),
        RenderObjectIds = this.CubeGrid.Render.RenderObjectIDs,
        Source = source,
        IsTrail = isTrail
      };
      VertexBoneIndicesWeights? boneIndicesWeights = gridHitInfo.Triangle.GetAffectingBoneIndicesWeights(ref MyCompoundCubeBlock.m_boneIndexWeightTmp);
      if (boneIndicesWeights.HasValue)
      {
        renderInfo.BoneIndices = boneIndicesWeights.Value.Indices;
        renderInfo.BoneWeights = boneIndicesWeights.Value.Weights;
      }
      renderInfo.PhysicalMaterial = physicalMaterial.GetHashCode() != 0 ? physicalMaterial : MyStringHash.GetOrCompute(physicalMaterial1.Id.SubtypeName);
      renderInfo.VoxelMaterial = voxelMaterial;
      MyCompoundCubeBlock.m_tmpIds.Clear();
      decalHandler.AddDecal(ref renderInfo, MyCompoundCubeBlock.m_tmpIds);
      foreach (uint tmpId in MyCompoundCubeBlock.m_tmpIds)
        this.CubeGrid.RenderData.AddDecal(this.Position, gridHitInfo, tmpId);
    }

    public bool GetIntersectionWithLine(
      ref LineD line,
      out MyIntersectionResultLineTriangleEx? t,
      out ushort blockId,
      IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES,
      bool checkZFight = false,
      bool ignoreGenerated = false)
    {
      t = new MyIntersectionResultLineTriangleEx?();
      blockId = (ushort) 0;
      double num1 = double.MaxValue;
      bool flag = false;
      foreach (KeyValuePair<ushort, MySlimBlock> keyValuePair in this.m_mapIdToBlock)
      {
        MySlimBlock mySlimBlock = keyValuePair.Value;
        MyIntersectionResultLineTriangleEx? t1;
        if ((!ignoreGenerated || !mySlimBlock.BlockDefinition.IsGeneratedBlock) && (mySlimBlock.FatBlock.GetIntersectionWithLine(ref line, out t1, IntersectionFlags.ALL_TRIANGLES) && t1.HasValue))
        {
          double num2 = (t1.Value.IntersectionPointInWorldSpace - line.From).LengthSquared();
          if (num2 < num1 && (!checkZFight || num1 >= num2 + 1.0 / 1000.0))
          {
            num1 = num2;
            t = t1;
            blockId = keyValuePair.Key;
            flag = true;
          }
        }
      }
      return flag;
    }

    public bool GetIntersectionWithLine_FullyBuiltProgressModels(
      ref LineD line,
      out MyIntersectionResultLineTriangleEx? t,
      out ushort blockId,
      IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES,
      bool checkZFight = false,
      bool ignoreGenerated = false)
    {
      t = new MyIntersectionResultLineTriangleEx?();
      blockId = (ushort) 0;
      double num1 = double.MaxValue;
      bool flag = false;
      foreach (KeyValuePair<ushort, MySlimBlock> keyValuePair in this.m_mapIdToBlock)
      {
        MySlimBlock mySlimBlock = keyValuePair.Value;
        if (!ignoreGenerated || !mySlimBlock.BlockDefinition.IsGeneratedBlock)
        {
          MyModel modelOnlyData = MyModels.GetModelOnlyData(mySlimBlock.BlockDefinition.Model);
          if (modelOnlyData != null)
          {
            MyIntersectionResultLineTriangleEx? intersectionWithLine = modelOnlyData.GetTrianglePruningStructure().GetIntersectionWithLine((IMyEntity) mySlimBlock.FatBlock, ref line, flags);
            if (intersectionWithLine.HasValue)
            {
              double num2 = (intersectionWithLine.Value.IntersectionPointInWorldSpace - line.From).LengthSquared();
              if (num2 < num1 && (!checkZFight || num1 >= num2 + 1.0 / 1000.0))
              {
                num1 = num2;
                t = intersectionWithLine;
                blockId = keyValuePair.Key;
                flag = true;
              }
            }
          }
        }
      }
      return flag;
    }

    private static void GetBlockLocalMatrixFromGridPositionAndOrientation(
      MySlimBlock block,
      ref MatrixD localMatrix)
    {
      Matrix result;
      block.Orientation.GetMatrix(out result);
      localMatrix = (MatrixD) ref result;
      localMatrix.Translation = (Vector3D) (block.CubeGrid.GridSize * block.Position);
    }

    private static void GetCompoundCollisionDummies(
      MyCubeBlockDefinition definition,
      List<MyModelDummy> outDummies)
    {
      MyModel modelOnlyDummies = MyModels.GetModelOnlyDummies(definition.Model);
      if (modelOnlyDummies == null)
        return;
      foreach (KeyValuePair<string, MyModelDummy> dummy in modelOnlyDummies.Dummies)
      {
        if (dummy.Key.ToLower().StartsWith(MyCompoundCubeBlock.COMPOUND_DUMMY))
          outDummies.Add(dummy.Value);
      }
    }

    public static bool IsCompoundEnabled(MyCubeBlockDefinition blockDefinition)
    {
      if (!MyFakes.ENABLE_COMPOUND_BLOCKS || blockDefinition == null || (blockDefinition.CubeSize != MyCubeSize.Large || blockDefinition.Size != Vector3I.One))
        return false;
      if (MyFakes.ENABLE_COMPOUND_BLOCK_COLLISION_DUMMIES)
        return blockDefinition.CompoundEnabled;
      return blockDefinition.CompoundTemplates != null && (uint) blockDefinition.CompoundTemplates.Length > 0U;
    }

    private class MyCompoundBlockPosComponent : MyCubeBlock.MyBlockPosComponent
    {
      private MyCompoundCubeBlock m_block;

      public override void OnAddedToContainer()
      {
        base.OnAddedToContainer();
        this.m_block = this.Container.Entity as MyCompoundCubeBlock;
      }

      public override void UpdateWorldMatrix(
        ref MatrixD parentWorldMatrix,
        object source = null,
        bool updateChildren = true,
        bool forceUpdateAllChildren = false)
      {
        this.m_block.UpdateBlocksWorldMatrix(ref parentWorldMatrix, source);
        base.UpdateWorldMatrix(ref parentWorldMatrix, source, updateChildren, forceUpdateAllChildren);
      }

      private class Sandbox_Game_Entities_MyCompoundCubeBlock\u003C\u003EMyCompoundBlockPosComponent\u003C\u003EActor : IActivator, IActivator<MyCompoundCubeBlock.MyCompoundBlockPosComponent>
      {
        object IActivator.CreateInstance() => (object) new MyCompoundCubeBlock.MyCompoundBlockPosComponent();

        MyCompoundCubeBlock.MyCompoundBlockPosComponent IActivator<MyCompoundCubeBlock.MyCompoundBlockPosComponent>.CreateInstance() => new MyCompoundCubeBlock.MyCompoundBlockPosComponent();
      }
    }

    private class MyDebugRenderComponentCompoundBlock : MyDebugRenderComponent
    {
      private readonly MyCompoundCubeBlock m_compoundBlock;

      public MyDebugRenderComponentCompoundBlock(MyCompoundCubeBlock compoundBlock)
        : base((IMyEntity) compoundBlock)
        => this.m_compoundBlock = compoundBlock;

      public override void DebugDraw()
      {
        foreach (MySlimBlock block in this.m_compoundBlock.GetBlocks())
        {
          if (block.FatBlock != null)
            block.FatBlock.DebugDraw();
        }
      }
    }

    private class Sandbox_Game_Entities_MyCompoundCubeBlock\u003C\u003EActor : IActivator, IActivator<MyCompoundCubeBlock>
    {
      object IActivator.CreateInstance() => (object) new MyCompoundCubeBlock();

      MyCompoundCubeBlock IActivator<MyCompoundCubeBlock>.CreateInstance() => new MyCompoundCubeBlock();
    }
  }
}
