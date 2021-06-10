// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Physics.MyDestructionHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Replication;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Engine.Physics
{
  public static class MyDestructionHelper
  {
    public static readonly float MASS_REDUCTION_COEF = 0.04f;
    private static List<HkdShapeInstanceInfo> m_tmpInfos = new List<HkdShapeInstanceInfo>();
    private static List<HkdShapeInstanceInfo> m_tmpInfos2 = new List<HkdShapeInstanceInfo>();

    private static bool DontCreateFracture(HkdBreakableShape breakableShape) => breakableShape.IsValid() && (breakableShape.UserObject & 2U) > 0U;

    public static bool IsFixed(HkdBreakableBodyInfo breakableBodyInfo)
    {
      new HkdBreakableBodyHelper(breakableBodyInfo).GetChildren(MyDestructionHelper.m_tmpInfos2);
      foreach (HkdShapeInstanceInfo shapeInstanceInfo in MyDestructionHelper.m_tmpInfos2)
      {
        if (MyDestructionHelper.IsFixed(shapeInstanceInfo.Shape))
        {
          MyDestructionHelper.m_tmpInfos2.Clear();
          return true;
        }
      }
      MyDestructionHelper.m_tmpInfos2.Clear();
      return false;
    }

    public static bool IsFixed(HkdBreakableShape breakableShape)
    {
      if (!breakableShape.IsValid())
        return false;
      if (((int) breakableShape.UserObject & 4) != 0)
        return true;
      breakableShape.GetChildren(MyDestructionHelper.m_tmpInfos);
      foreach (HkdShapeInstanceInfo tmpInfo in MyDestructionHelper.m_tmpInfos)
      {
        if (((int) tmpInfo.Shape.UserObject & 4) != 0)
        {
          MyDestructionHelper.m_tmpInfos.Clear();
          return true;
        }
      }
      MyDestructionHelper.m_tmpInfos.Clear();
      return false;
    }

    private static bool IsBodyWithoutGeneratedFracturedPieces(HkdBreakableBody b, MyCubeBlock block)
    {
      if (MyFakes.REMOVE_GENERATED_BLOCK_FRACTURES && (block == null || MyDestructionHelper.ContainsBlockWithoutGeneratedFracturedPieces(block)))
      {
        if (b.BreakableShape.IsCompound())
        {
          b.BreakableShape.GetChildren(MyDestructionHelper.m_tmpInfos);
          for (int index = MyDestructionHelper.m_tmpInfos.Count - 1; index >= 0 && MyDestructionHelper.DontCreateFracture(MyDestructionHelper.m_tmpInfos[index].Shape); --index)
            MyDestructionHelper.m_tmpInfos.RemoveAt(index);
          if (MyDestructionHelper.m_tmpInfos.Count == 0)
            return true;
          MyDestructionHelper.m_tmpInfos.Clear();
        }
        else if (MyDestructionHelper.DontCreateFracture(b.BreakableShape))
          return true;
      }
      return false;
    }

    public static MyFracturedPiece CreateFracturePiece(
      HkdBreakableBody b,
      ref MatrixD worldMatrix,
      List<MyDefinitionId> originalBlocks,
      MyCubeBlock block = null,
      bool sync = true)
    {
      if (MyDestructionHelper.IsBodyWithoutGeneratedFracturedPieces(b, block))
        return (MyFracturedPiece) null;
      MyFracturedPiece pieceFromPool = MyFracturedPiecesManager.Static.GetPieceFromPool(0L);
      pieceFromPool.InitFromBreakableBody(b, worldMatrix, block);
      pieceFromPool.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      if (originalBlocks != null && originalBlocks.Count != 0)
      {
        pieceFromPool.OriginalBlocks.Clear();
        pieceFromPool.OriginalBlocks.AddRange((IEnumerable<MyDefinitionId>) originalBlocks);
        MyPhysicalModelDefinition definition;
        if (MyDefinitionManager.Static.TryGetDefinition<MyPhysicalModelDefinition>(originalBlocks[0], out definition))
          pieceFromPool.Physics.MaterialType = definition.PhysicalMaterial.Id.SubtypeId;
      }
      if (MyFakes.ENABLE_FRACTURE_PIECE_SHAPE_CHECK)
        pieceFromPool.DebugCheckValidShapes();
      if (MyExternalReplicable.FindByObject((object) pieceFromPool) == null)
        Sandbox.Game.Entities.MyEntities.RaiseEntityCreated((MyEntity) pieceFromPool);
      Sandbox.Game.Entities.MyEntities.Add((MyEntity) pieceFromPool);
      return pieceFromPool;
    }

    public static void FixPosition(MyFracturedPiece fp)
    {
      HkdBreakableShape breakableShape = fp.Physics.BreakableBody.BreakableShape;
      if (breakableShape.GetChildrenCount() == 0)
        return;
      breakableShape.GetChildren(MyDestructionHelper.m_tmpInfos);
      Vector3 translation = MyDestructionHelper.m_tmpInfos[0].GetTransform().Translation;
      if ((double) translation.LengthSquared() < 1.0)
      {
        MyDestructionHelper.m_tmpInfos.Clear();
      }
      else
      {
        List<HkdConnection> resultList = new List<HkdConnection>();
        HashSet<HkdBreakableShape> hkdBreakableShapeSet1 = new HashSet<HkdBreakableShape>();
        HashSet<HkdBreakableShape> hkdBreakableShapeSet2 = new HashSet<HkdBreakableShape>();
        hkdBreakableShapeSet1.Add(breakableShape);
        breakableShape.GetConnectionList(resultList);
        fp.PositionComp.SetPosition(Vector3D.Transform(translation, fp.PositionComp.WorldMatrixRef));
        foreach (HkdShapeInstanceInfo tmpInfo in MyDestructionHelper.m_tmpInfos)
        {
          Matrix transform = tmpInfo.GetTransform();
          transform.Translation -= translation;
          tmpInfo.SetTransform(ref transform);
          MyDestructionHelper.m_tmpInfos2.Add(tmpInfo);
          HkdBreakableShape hkdBreakableShape = tmpInfo.Shape;
          hkdBreakableShape.GetConnectionList(resultList);
          while (hkdBreakableShape.HasParent)
          {
            hkdBreakableShape = hkdBreakableShape.GetParent();
            if (hkdBreakableShapeSet1.Add(hkdBreakableShape))
              hkdBreakableShape.GetConnectionList(resultList);
          }
          hkdBreakableShapeSet2.Add(tmpInfo.Shape);
        }
        MyDestructionHelper.m_tmpInfos.Clear();
        HkdBreakableShape parent = (HkdBreakableShape) new HkdCompoundBreakableShape(breakableShape, MyDestructionHelper.m_tmpInfos2);
        ((HkdCompoundBreakableShape) parent).RecalcMassPropsFromChildren();
        parent.SetChildrenParent(parent);
        foreach (HkdConnection hkdConnection in resultList)
        {
          HkBaseSystem.EnableAssert(390435339, true);
          if (hkdBreakableShapeSet2.Contains(hkdConnection.ShapeA) && hkdBreakableShapeSet2.Contains(hkdConnection.ShapeB))
          {
            HkdConnection connection = hkdConnection;
            parent.AddConnection(ref connection);
          }
        }
        fp.Physics.BreakableBody.BreakableShape = parent;
        MyDestructionHelper.m_tmpInfos2.Clear();
        ((HkdCompoundBreakableShape) parent).RecalcMassPropsFromChildren();
      }
    }

    private static bool ContainsBlockWithoutGeneratedFracturedPieces(MyCubeBlock block)
    {
      if (!block.BlockDefinition.CreateFracturedPieces)
        return true;
      if (block is MyCompoundCubeBlock)
      {
        foreach (MySlimBlock block1 in (block as MyCompoundCubeBlock).GetBlocks())
        {
          if (!block1.BlockDefinition.CreateFracturedPieces)
            return true;
        }
      }
      if (block is MyFracturedBlock)
      {
        foreach (MyDefinitionId originalBlock in (block as MyFracturedBlock).OriginalBlocks)
        {
          if (!MyDefinitionManager.Static.GetCubeBlockDefinition(originalBlock).CreateFracturedPieces)
            return true;
        }
      }
      return false;
    }

    public static MyFracturedPiece CreateFracturePiece(
      HkdBreakableShape shape,
      ref MatrixD worldMatrix,
      bool isStatic,
      MyDefinitionId? definition,
      bool sync)
    {
      MyFracturedPiece fracturePiece = MyDestructionHelper.CreateFracturePiece(ref shape, ref worldMatrix, isStatic);
      if (definition.HasValue)
      {
        fracturePiece.OriginalBlocks.Clear();
        fracturePiece.OriginalBlocks.Add(definition.Value);
        MyPhysicalModelDefinition definition1;
        if (MyDefinitionManager.Static.TryGetDefinition<MyPhysicalModelDefinition>(definition.Value, out definition1))
          fracturePiece.Physics.MaterialType = definition1.PhysicalMaterial.Id.SubtypeId;
      }
      else
        fracturePiece.Save = false;
      if (fracturePiece.Save && MyFakes.ENABLE_FRACTURE_PIECE_SHAPE_CHECK)
        fracturePiece.DebugCheckValidShapes();
      if (MyExternalReplicable.FindByObject((object) fracturePiece) == null)
        Sandbox.Game.Entities.MyEntities.RaiseEntityCreated((MyEntity) fracturePiece);
      Sandbox.Game.Entities.MyEntities.Add((MyEntity) fracturePiece);
      return fracturePiece;
    }

    public static MyFracturedPiece CreateFracturePiece(
      MyFracturedBlock fracturedBlock,
      bool sync)
    {
      MatrixD worldMatrix = fracturedBlock.CubeGrid.PositionComp.WorldMatrixRef;
      worldMatrix.Translation = fracturedBlock.CubeGrid.GridIntegerToWorld(fracturedBlock.Position);
      MyFracturedPiece fracturePiece = MyDestructionHelper.CreateFracturePiece(ref fracturedBlock.Shape, ref worldMatrix, false);
      fracturePiece.OriginalBlocks = fracturedBlock.OriginalBlocks;
      MyPhysicalModelDefinition definition;
      if (MyDefinitionManager.Static.TryGetDefinition<MyPhysicalModelDefinition>(fracturePiece.OriginalBlocks[0], out definition))
        fracturePiece.Physics.MaterialType = definition.PhysicalMaterial.Id.SubtypeId;
      if (MyFakes.ENABLE_FRACTURE_PIECE_SHAPE_CHECK)
        fracturePiece.DebugCheckValidShapes();
      if (MyExternalReplicable.FindByObject((object) fracturePiece) == null)
        Sandbox.Game.Entities.MyEntities.RaiseEntityCreated((MyEntity) fracturePiece);
      Sandbox.Game.Entities.MyEntities.Add((MyEntity) fracturePiece);
      return fracturePiece;
    }

    public static MyFracturedPiece CreateFracturePiece(
      MyFractureComponentCubeBlock fractureBlockComponent,
      bool sync)
    {
      if (!fractureBlockComponent.Block.BlockDefinition.CreateFracturedPieces)
        return (MyFracturedPiece) null;
      if (!fractureBlockComponent.Shape.IsValid())
      {
        MyLog.Default.WriteLine("Invalid shape in fracture component, Id: " + fractureBlockComponent.Block.BlockDefinition.Id.ToString() + ", closed: " + fractureBlockComponent.Block.FatBlock.Closed.ToString());
        return (MyFracturedPiece) null;
      }
      MatrixD worldMatrix = fractureBlockComponent.Block.FatBlock.WorldMatrix;
      MyFracturedPiece fracturePiece = MyDestructionHelper.CreateFracturePiece(ref fractureBlockComponent.Shape, ref worldMatrix, false);
      fracturePiece.OriginalBlocks.Add(fractureBlockComponent.Block.BlockDefinition.Id);
      if (MyFakes.ENABLE_FRACTURE_PIECE_SHAPE_CHECK)
        fracturePiece.DebugCheckValidShapes();
      MyPhysicalModelDefinition definition;
      if (MyDefinitionManager.Static.TryGetDefinition<MyPhysicalModelDefinition>(fracturePiece.OriginalBlocks[0], out definition))
        fracturePiece.Physics.MaterialType = definition.PhysicalMaterial.Id.SubtypeId;
      if (MyExternalReplicable.FindByObject((object) fracturePiece) == null)
        Sandbox.Game.Entities.MyEntities.RaiseEntityCreated((MyEntity) fracturePiece);
      Sandbox.Game.Entities.MyEntities.Add((MyEntity) fracturePiece);
      return fracturePiece;
    }

    private static MyFracturedPiece CreateFracturePiece(
      ref HkdBreakableShape shape,
      ref MatrixD worldMatrix,
      bool isStatic)
    {
      MyFracturedPiece pieceFromPool = MyFracturedPiecesManager.Static.GetPieceFromPool(0L);
      pieceFromPool.PositionComp.SetWorldMatrix(ref worldMatrix);
      pieceFromPool.Physics.Flags = isStatic ? RigidBodyFlag.RBF_STATIC : RigidBodyFlag.RBF_DEBRIS;
      MyPhysicsBody physics = pieceFromPool.Physics;
      HkMassProperties massProperties = new HkMassProperties();
      shape.BuildMassProperties(ref massProperties);
      physics.InitialSolverDeactivation = HkSolverDeactivation.High;
      physics.CreateFromCollisionObject(shape.GetShape(), Vector3.Zero, worldMatrix, new HkMassProperties?(massProperties));
      physics.LinearDamping = MyPerGameSettings.DefaultLinearDamping;
      physics.AngularDamping = MyPerGameSettings.DefaultAngularDamping;
      physics.BreakableBody = new HkdBreakableBody(shape, physics.RigidBody, (HkdWorld) null, (Matrix) ref worldMatrix);
      physics.BreakableBody.AfterReplaceBody += new BreakableBodyReplaced(physics.FracturedBody_AfterReplaceBody);
      if (pieceFromPool.SyncFlag)
        pieceFromPool.CreateSync();
      pieceFromPool.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      pieceFromPool.SetDataFromHavok(shape);
      pieceFromPool.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      shape.RemoveReference();
      return pieceFromPool;
    }

    public static void TriggerDestruction(
      HkWorld world,
      HkRigidBody body,
      Vector3 havokPosition,
      float radius = 0.0005f)
    {
      HkdFractureImpactDetails fractureImpactDetails = HkdFractureImpactDetails.Create();
      fractureImpactDetails.SetBreakingBody(body);
      fractureImpactDetails.SetContactPoint(havokPosition);
      fractureImpactDetails.SetDestructionRadius(radius);
      fractureImpactDetails.SetBreakingImpulse(MyDestructionConstants.STRENGTH * 10f);
      fractureImpactDetails.Flag |= HkdFractureImpactDetails.Flags.FLAG_DONT_RECURSE;
      MyPhysics.EnqueueDestruction(new MyPhysics.FractureImpactDetails()
      {
        Details = fractureImpactDetails,
        World = world
      });
    }

    public static void TriggerDestruction(
      float destructionImpact,
      MyPhysicsBody body,
      Vector3D position,
      Vector3 normal,
      float maxDestructionRadius)
    {
      if (!((HkReferenceObject) body.BreakableBody != (HkReferenceObject) null))
        return;
      double mass = (double) body.Mass;
      float v1 = Math.Min(destructionImpact / 8000f, maxDestructionRadius);
      float v2 = MyDestructionConstants.STRENGTH + destructionImpact / 10000f;
      float v3 = Math.Min(destructionImpact / 10000f, 3f);
      HkdFractureImpactDetails fractureImpactDetails = HkdFractureImpactDetails.Create();
      fractureImpactDetails.SetBreakingBody(body.RigidBody);
      fractureImpactDetails.SetContactPoint((Vector3) body.WorldToCluster(position));
      fractureImpactDetails.SetDestructionRadius(v1);
      fractureImpactDetails.SetBreakingImpulse(v2);
      fractureImpactDetails.SetParticleExpandVelocity(v3);
      fractureImpactDetails.SetParticlePosition((Vector3) body.WorldToCluster(position - normal * 0.25f));
      fractureImpactDetails.SetParticleMass(1E+07f);
      fractureImpactDetails.ZeroCollidingParticleVelocity();
      fractureImpactDetails.Flag = fractureImpactDetails.Flag | HkdFractureImpactDetails.Flags.FLAG_DONT_RECURSE | HkdFractureImpactDetails.Flags.FLAG_TRIGGERED_DESTRUCTION;
      MyPhysics.EnqueueDestruction(new MyPhysics.FractureImpactDetails()
      {
        Details = fractureImpactDetails,
        World = body.HavokWorld,
        ContactInWorld = position,
        Entity = (MyEntity) body.Entity
      });
    }

    public static float MassToHavok(float m) => MyPerGameSettings.Destruction ? m * MyDestructionHelper.MASS_REDUCTION_COEF : m;

    public static float MassFromHavok(float m) => MyPerGameSettings.Destruction ? m / MyDestructionHelper.MASS_REDUCTION_COEF : m;
  }
}
