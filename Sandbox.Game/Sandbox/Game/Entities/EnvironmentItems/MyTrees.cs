// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.EnvironmentItems.MyTrees
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders.Definitions;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.EnvironmentItems
{
  [MyEntityType(typeof (MyObjectBuilder_TreesMedium), false)]
  [MyEntityType(typeof (MyObjectBuilder_Trees), true)]
  [StaticEventOwner]
  public class MyTrees : MyEnvironmentItems, IMyDecalProxy
  {
    private List<MyTrees.MyCutTreeInfo> m_cutTreeInfos = new List<MyTrees.MyCutTreeInfo>();
    private const float MAX_TREE_CUT_DURATION = 60f;
    private const int BrokenTreeLifeSpan = 20000;

    public override void DoDamage(
      float damage,
      int itemInstanceId,
      Vector3D position,
      Vector3 normal,
      MyStringHash type)
    {
      MyTreeDefinition environmentItemDefinition = (MyTreeDefinition) MyDefinitionManager.Static.GetEnvironmentItemDefinition(new MyDefinitionId(this.Definition.ItemDefinitionType, this.m_itemsData[itemInstanceId].SubtypeId));
      MyParticlesManager.TryCreateParticleEffect(environmentItemDefinition.CutEffect, MatrixD.CreateWorld(position, Vector3.CalculatePerpendicularVector(normal), normal), out MyParticleEffect _);
      if (!Sync.IsServer)
        return;
      MyTrees.MyCutTreeInfo myCutTreeInfo = new MyTrees.MyCutTreeInfo();
      int index1 = -1;
      for (int index2 = 0; index2 < this.m_cutTreeInfos.Count; ++index2)
      {
        myCutTreeInfo = this.m_cutTreeInfos[index2];
        if (itemInstanceId == myCutTreeInfo.ItemInstanceId)
        {
          index1 = index2;
          break;
        }
      }
      if (index1 == -1)
      {
        myCutTreeInfo = new MyTrees.MyCutTreeInfo()
        {
          ItemInstanceId = itemInstanceId
        };
        myCutTreeInfo.MaxPoints = myCutTreeInfo.HitPoints = environmentItemDefinition.HitPoints;
        index1 = this.m_cutTreeInfos.Count;
        this.m_cutTreeInfos.Add(myCutTreeInfo);
      }
      myCutTreeInfo.LastHit = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      myCutTreeInfo.HitPoints -= damage;
      if ((double) myCutTreeInfo.Progress >= 1.0)
      {
        this.CutTree(itemInstanceId, position, normal, type == MyDamageType.Drill ? 1f : 4f);
        this.m_cutTreeInfos.RemoveAtFast<MyTrees.MyCutTreeInfo>(index1);
      }
      else
        this.m_cutTreeInfos[index1] = myCutTreeInfo;
    }

    public static bool IsEntityFracturedTree(IMyEntity entity) => entity is MyFracturedPiece && ((MyFracturedPiece) entity).OriginalBlocks != null && ((MyFracturedPiece) entity).OriginalBlocks.Count > 0 && (((MyFracturedPiece) entity).OriginalBlocks[0].TypeId == typeof (MyObjectBuilder_Tree) || ((MyFracturedPiece) entity).OriginalBlocks[0].TypeId == typeof (MyObjectBuilder_DestroyableItem) || ((MyFracturedPiece) entity).OriginalBlocks[0].TypeId == typeof (MyObjectBuilder_TreeDefinition)) && ((MyFracturedPiece) entity).Physics != null;

    protected override void OnRemoveItem(
      int instanceId,
      ref Matrix matrix,
      MyStringHash myStringId,
      int userData)
    {
      base.OnRemoveItem(instanceId, ref matrix, myStringId, userData);
    }

    private void CutTree(
      int itemInstanceId,
      Vector3D hitWorldPosition,
      Vector3 hitNormal,
      float forceMultiplier = 1f)
    {
      HkStaticCompoundShape shape = (HkStaticCompoundShape) this.Physics.RigidBody.GetShape();
      int physicsInstanceId;
      if (!this.m_localIdToPhysicsShapeInstanceId.TryGetValue(itemInstanceId, out physicsInstanceId))
        return;
      MyEnvironmentItems.MyEnvironmentItemData itemData = this.m_itemsData[itemInstanceId];
      MyTreeDefinition environmentItemDefinition = (MyTreeDefinition) MyDefinitionManager.Static.GetEnvironmentItemDefinition(new MyDefinitionId(this.Definition.ItemDefinitionType, itemData.SubtypeId));
      if (this.RemoveItem(itemInstanceId, physicsInstanceId, true, true) && environmentItemDefinition != null && (environmentItemDefinition.BreakSound != null && environmentItemDefinition.BreakSound.Length > 0))
        MyMultiplayer.RaiseStaticEvent<Vector3D, string>((Func<IMyEventOwner, Action<Vector3D, string>>) (s => new Action<Vector3D, string>(MyTrees.PlaySound)), hitWorldPosition, environmentItemDefinition.BreakSound, position: new Vector3D?(hitWorldPosition));
      if (!MyPerGameSettings.Destruction || MyModels.GetModelOnlyData(environmentItemDefinition.Model).HavokBreakableShapes == null)
        return;
      if (environmentItemDefinition.FallSound != null && environmentItemDefinition.FallSound.Length > 0)
        this.CreateBreakableShape((MyEnvironmentItemDefinition) environmentItemDefinition, ref itemData, ref hitWorldPosition, hitNormal, forceMultiplier, environmentItemDefinition.FallSound);
      else
        this.CreateBreakableShape((MyEnvironmentItemDefinition) environmentItemDefinition, ref itemData, ref hitWorldPosition, hitNormal, forceMultiplier);
    }

    [Event(null, 157)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void PlaySound(Vector3D position, string cueName)
    {
      MySoundPair soundId = new MySoundPair(cueName);
      if (soundId == MySoundPair.Empty)
        return;
      MyEntity3DSoundEmitter soundEmitter = MyAudioComponent.TryGetSoundEmitter();
      if (soundEmitter == null)
        return;
      soundEmitter.SetPosition(new Vector3D?(position));
      soundEmitter.PlaySound(soundId);
    }

    protected override MyEntity DestroyItem(int itemInstanceId)
    {
      int physicsInstanceId;
      if (!this.m_localIdToPhysicsShapeInstanceId.TryGetValue(itemInstanceId, out physicsInstanceId))
        physicsInstanceId = -1;
      MyEnvironmentItems.MyEnvironmentItemData environmentItemData = this.m_itemsData[itemInstanceId];
      this.RemoveItem(itemInstanceId, physicsInstanceId, false, true);
      Vector3D position = environmentItemData.Transform.Position;
      string str = environmentItemData.Model.AssetName.Insert(environmentItemData.Model.AssetName.Length - 4, "_broken");
      bool flag = false;
      MyEntity debris;
      if (MyModels.GetModelOnlyData(str) != null)
      {
        flag = true;
        debris = MyDebris.Static.CreateDebris(str);
      }
      else
        debris = MyDebris.Static.CreateDebris(environmentItemData.Model.AssetName);
      MyDebrisBase.MyDebrisBaseLogic gameLogic = debris.GameLogic as MyDebrisBase.MyDebrisBaseLogic;
      gameLogic.LifespanInMiliseconds = 20000;
      MatrixD fromQuaternion = MatrixD.CreateFromQuaternion(environmentItemData.Transform.Rotation);
      fromQuaternion.Translation = position + fromQuaternion.Up * (flag ? 0.0 : 5.0);
      gameLogic.Start(fromQuaternion, (Vector3D) Vector3.Zero, false);
      return debris;
    }

    private void CreateBreakableShape(
      MyEnvironmentItemDefinition itemDefinition,
      ref MyEnvironmentItems.MyEnvironmentItemData itemData,
      ref Vector3D hitWorldPosition,
      Vector3 hitNormal,
      float forceMultiplier,
      string fallSound = "")
    {
      HkdBreakableShape oldBreakableShape = MyModels.GetModelOnlyData(itemDefinition.Model).HavokBreakableShapes[0].Clone();
      MatrixD transformMatrix = itemData.Transform.TransformMatrix;
      oldBreakableShape.SetMassRecursively(500f);
      oldBreakableShape.SetStrenghtRecursively(5000f, 0.7f);
      oldBreakableShape.GetChildren(this.m_childrenTmp);
      HkdBreakableShape[] havokBreakableShapes = MyModels.GetModelOnlyData(itemDefinition.Model).HavokBreakableShapes;
      Vector3 vector3 = (Vector3) Vector3D.Transform(hitWorldPosition, MatrixD.Normalize(MatrixD.Invert(transformMatrix)));
      float num1 = (float) (hitWorldPosition.Y - itemData.Transform.Position.Y);
      List<HkdShapeInstanceInfo> shapeList1 = new List<HkdShapeInstanceInfo>();
      List<HkdShapeInstanceInfo> shapeList2 = new List<HkdShapeInstanceInfo>();
      HkdShapeInstanceInfo? nullable = new HkdShapeInstanceInfo?();
      foreach (HkdShapeInstanceInfo shapeInstanceInfo in this.m_childrenTmp)
      {
        if (!nullable.HasValue || (double) shapeInstanceInfo.CoM.Y < (double) nullable.Value.CoM.Y)
          nullable = new HkdShapeInstanceInfo?(shapeInstanceInfo);
        if ((double) shapeInstanceInfo.CoM.Y > (double) num1)
          shapeList2.Add(shapeInstanceInfo);
        else
          shapeList1.Add(shapeInstanceInfo);
      }
      if (shapeList1.Count == 2)
      {
        double y1 = (double) shapeList1[0].CoM.Y;
        HkdShapeInstanceInfo shapeInstanceInfo = shapeList1[1];
        double y2 = (double) shapeInstanceInfo.CoM.Y;
        if (y1 < y2)
        {
          double num2 = (double) num1;
          shapeInstanceInfo = shapeList1[1];
          double num3 = (double) shapeInstanceInfo.CoM.Y + 1.25;
          if (num2 < num3)
          {
            shapeList2.Insert(0, shapeList1[1]);
            shapeList1.RemoveAt(1);
            goto label_18;
          }
        }
        shapeInstanceInfo = shapeList1[0];
        double y3 = (double) shapeInstanceInfo.CoM.Y;
        shapeInstanceInfo = shapeList1[1];
        double y4 = (double) shapeInstanceInfo.CoM.Y;
        if (y3 > y4)
        {
          double num2 = (double) num1;
          shapeInstanceInfo = shapeList1[0];
          double num3 = (double) shapeInstanceInfo.CoM.Y + 1.25;
          if (num2 < num3)
          {
            shapeList2.Insert(0, shapeList1[0]);
            shapeList1.RemoveAt(0);
          }
        }
      }
      else if (shapeList1.Count == 0 && shapeList2.Remove(nullable.Value))
        shapeList1.Add(nullable.Value);
label_18:
      if (shapeList1.Count > 0)
        MyTrees.CreateFracturePiece(itemDefinition, oldBreakableShape, transformMatrix, hitNormal, shapeList1, forceMultiplier, true);
      if (shapeList2.Count > 0)
        MyTrees.CreateFracturePiece(itemDefinition, oldBreakableShape, transformMatrix, hitNormal, shapeList2, forceMultiplier, false, fallSound);
      this.m_childrenTmp.Clear();
    }

    public static void CreateFracturePiece(
      MyEnvironmentItemDefinition itemDefinition,
      HkdBreakableShape oldBreakableShape,
      MatrixD worldMatrix,
      Vector3 hitNormal,
      List<HkdShapeInstanceInfo> shapeList,
      float forceMultiplier,
      bool canContainFixedChildren,
      string fallSound = "")
    {
      bool isStatic = false;
      if (canContainFixedChildren)
      {
        foreach (HkdShapeInstanceInfo shape in shapeList)
        {
          shape.Shape.SetMotionQualityRecursively(HkdBreakableShape.BodyQualityType.QUALITY_DEBRIS);
          Vector3D translation = worldMatrix.Translation + worldMatrix.Up * 1.5;
          MatrixD matrix = worldMatrix.GetOrientation();
          Quaternion fromRotationMatrix = Quaternion.CreateFromRotationMatrix(in matrix);
          MyPhysics.GetPenetrationsShape(shape.Shape.GetShape(), ref translation, ref fromRotationMatrix, MyEnvironmentItems.m_tmpResults, 15);
          bool flag = false;
          foreach (HkBodyCollision tmpResult in MyEnvironmentItems.m_tmpResults)
          {
            if (tmpResult.GetCollisionEntity() is MyVoxelMap)
            {
              shape.Shape.SetFlagRecursively(HkdBreakableShape.Flags.IS_FIXED);
              isStatic = true;
              break;
            }
            if (flag)
              break;
          }
          MyEnvironmentItems.m_tmpResults.Clear();
        }
      }
      HkdBreakableShape compound = (HkdBreakableShape) new HkdCompoundBreakableShape(oldBreakableShape, shapeList);
      ((HkdCompoundBreakableShape) compound).RecalcMassPropsFromChildren();
      MyFracturedPiece fracturePiece = MyDestructionHelper.CreateFracturePiece(compound, ref worldMatrix, isStatic, new MyDefinitionId?(itemDefinition.Id), true);
      if (fracturePiece == null || canContainFixedChildren)
        return;
      MyTrees.ApplyImpulseToTreeFracture(ref worldMatrix, ref hitNormal, shapeList, ref compound, fracturePiece, forceMultiplier);
      fracturePiece.Physics.ForceActivate();
      if (fallSound.Length <= 0)
        return;
      fracturePiece.StartFallSound(fallSound);
    }

    public static void ApplyImpulseToTreeFracture(
      ref MatrixD worldMatrix,
      ref Vector3 hitNormal,
      List<HkdShapeInstanceInfo> shapeList,
      ref HkdBreakableShape compound,
      MyFracturedPiece fp,
      float forceMultiplier = 1f)
    {
      float mass = compound.GetMass();
      Vector3 coMMaxY = Vector3.MinValue;
      shapeList.ForEach((Action<HkdShapeInstanceInfo>) (s => coMMaxY = (double) s.CoM.Y > (double) coMMaxY.Y ? s.CoM : coMMaxY));
      Vector3 vector3 = hitNormal;
      vector3.Y = 0.0f;
      double num = (double) vector3.Normalize();
      Vector3 impulse = 0.3f * forceMultiplier * mass * vector3;
      fp.Physics.Enabled = true;
      Vector3 cluster = (Vector3) fp.Physics.WorldToCluster(Vector3D.Transform(coMMaxY, worldMatrix));
      fp.Physics.RigidBody.AngularDamping = MyPerGameSettings.DefaultAngularDamping;
      fp.Physics.RigidBody.LinearDamping = MyPerGameSettings.DefaultLinearDamping;
      fp.Physics.RigidBody.ApplyPointImpulse(impulse, cluster);
    }

    public override void UpdateAfterSimulation100()
    {
      base.UpdateAfterSimulation100();
      this.UpdateTreeInfos();
    }

    private void UpdateTreeInfos()
    {
      int timeInMilliseconds = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      int num = 60000;
      for (int index = this.m_cutTreeInfos.Count - 1; index >= 0; --index)
      {
        MyTrees.MyCutTreeInfo cutTreeInfo = this.m_cutTreeInfos[index];
        if (timeInMilliseconds - cutTreeInfo.LastHit > num)
          this.m_cutTreeInfos.RemoveAtFast<MyTrees.MyCutTreeInfo>(index);
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
      MyDecalRenderInfo renderInfo = new MyDecalRenderInfo()
      {
        Position = hitInfo.Position,
        Normal = hitInfo.Normal,
        RenderObjectIds = (uint[]) null,
        Flags = MyDecalFlags.World,
        Source = source,
        Forward = forwardDirection,
        VoxelMaterial = this.Physics.MaterialType,
        IsTrail = isTrail
      };
      renderInfo.PhysicalMaterial = physicalMaterial.GetHashCode() != 0 ? physicalMaterial : this.Physics.MaterialType;
      decalHandler.AddDecal(ref renderInfo);
    }

    private struct MyCutTreeInfo
    {
      public int ItemInstanceId;
      public int LastHit;
      public float HitPoints;
      public float MaxPoints;

      public float Progress => MathHelper.Clamp((this.MaxPoints - this.HitPoints) / this.MaxPoints, 0.0f, 1f);
    }

    protected sealed class PlaySound\u003C\u003EVRageMath_Vector3D\u0023System_String : ICallSite<IMyEventOwner, Vector3D, string, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in Vector3D position,
        in string cueName,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyTrees.PlaySound(position, cueName);
      }
    }

    private class Sandbox_Game_Entities_EnvironmentItems_MyTrees\u003C\u003EActor : IActivator, IActivator<MyTrees>
    {
      object IActivator.CreateInstance() => (object) new MyTrees();

      MyTrees IActivator<MyTrees>.CreateInstance() => new MyTrees();
    }
  }
}
