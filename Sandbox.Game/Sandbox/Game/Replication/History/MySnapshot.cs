// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.History.MySnapshot
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using System;
using VRage.Game.Entity;
using VRage.Library.Collections;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Replication.History
{
  public struct MySnapshot
  {
    public long ParentId;
    public bool SkippedParent;
    public bool Active;
    public bool InheritRotation;
    public Vector3D Position;
    public Quaternion Rotation;
    public Vector3 LinearVelocity;
    public Vector3 AngularVelocity;
    public Vector3 Pivot;
    public static bool ApplyReset = true;

    public MySnapshot(BitStream stream)
      : this()
      => this.Read(stream);

    public MySnapshot(
      MyEntity entity,
      MyClientInfo forClient,
      bool localPhysics = false,
      bool inheritRotation = true)
    {
      MyEntity myEntity = MySnapshot.GetParent(entity, out this.SkippedParent);
      if (myEntity != null && forClient.IsValid)
      {
        MyExternalReplicable byObject = MyExternalReplicable.FindByObject((object) myEntity);
        if (byObject != null && !forClient.IsReplicableReady((IMyReplicable) byObject))
          myEntity = (MyEntity) null;
      }
      this.Active = entity.Physics == null || (HkReferenceObject) entity.Physics.RigidBody == (HkReferenceObject) null || entity.Physics.RigidBody.IsActive;
      this.InheritRotation = inheritRotation;
      this.LinearVelocity = Vector3.Zero;
      this.AngularVelocity = Vector3.Zero;
      this.Pivot = Vector3.Zero;
      MatrixD result = entity.WorldMatrix;
      MyCubeGrid grid = entity as MyCubeGrid;
      if (myEntity != null)
      {
        this.ParentId = myEntity.EntityId;
        MatrixD matrixD = new MatrixD();
        if (this.InheritRotation)
        {
          MatrixD matrix2 = myEntity.PositionComp.WorldMatrixInvScaled;
          MatrixD.Multiply(ref result, ref matrix2, out result);
        }
        else
          result.Translation -= myEntity.PositionComp.GetPosition();
        this.Position = result.Translation;
        Quaternion.CreateFromRotationMatrix(ref result, out this.Rotation);
        this.Rotation.Normalize();
        if (grid != null && MyFakes.SNAPSHOTS_MECHANICAL_PIVOTS)
        {
          if (MyGridPhysicalHierarchy.Static.GetEntityConnectingToParent(grid) is MyMechanicalConnectionBlockBase connectingToParent)
          {
            Vector3? constraintPosition = connectingToParent.GetConstraintPosition(grid);
            if (constraintPosition.HasValue)
            {
              this.Pivot = constraintPosition.Value;
              Vector3D.Transform(ref this.Pivot, ref result, out this.Position);
            }
          }
        }
        else
        {
          this.Pivot = entity.Physics == null || !((HkReferenceObject) entity.Physics.RigidBody != (HkReferenceObject) null) ? entity.PositionComp.LocalAABB.Center : entity.Physics.CenterOfMassLocal;
          Vector3D.Transform(ref this.Pivot, ref result, out this.Position);
        }
        if (entity.Physics == null || myEntity.Physics == null)
          return;
        this.LinearVelocity = entity.Physics.LinearVelocityLocal;
        this.AngularVelocity = entity.Physics.AngularVelocity;
        if (localPhysics)
          return;
        Vector3 linearVelocity;
        if (this.InheritRotation)
        {
          Vector3D position = entity.PositionComp.GetPosition();
          myEntity.Physics.GetVelocityAtPointLocal(ref position, out linearVelocity);
        }
        else
          linearVelocity = myEntity.Physics.LinearVelocity;
        this.LinearVelocity -= linearVelocity;
      }
      else
      {
        this.ParentId = 0L;
        this.Pivot = entity.Physics == null || !((HkReferenceObject) entity.Physics.RigidBody != (HkReferenceObject) null) ? entity.PositionComp.LocalAABB.Center : entity.Physics.CenterOfMassLocal;
        Vector3D.Transform(ref this.Pivot, ref result, out this.Position);
        Quaternion.CreateFromRotationMatrix(ref result, out this.Rotation);
        this.Rotation.Normalize();
        if (entity.Physics == null)
          return;
        this.LinearVelocity = entity.Physics.LinearVelocity;
        this.AngularVelocity = entity.Physics.AngularVelocity;
      }
    }

    public static MyEntity GetParent(MyEntity entity, out bool skipped)
    {
      skipped = false;
      if (MyFakes.WORLD_SNAPSHOTS)
      {
        skipped = true;
        return (MyEntity) null;
      }
      MyEntity myEntity = (MyEntity) null;
      if (entity is MyCubeGrid grid)
      {
        if (grid.ClosestParentId != 0L)
          myEntity = MyEntities.GetEntityById(grid.ClosestParentId);
        else if (MyGridPhysicalHierarchy.Static.GetNodeChainLength(grid) < 4)
          myEntity = (MyEntity) MyGridPhysicalHierarchy.Static.GetParent(grid);
        else
          skipped = true;
      }
      else if (entity is MyCharacter myCharacter)
        myEntity = MyEntities.GetEntityById(myCharacter.ClosestParentId);
      return myEntity != null && (myEntity.MarkedForClose || myEntity.Closed) ? (MyEntity) null : myEntity;
    }

    public void Diff(ref MySnapshot value, out MySnapshot ss)
    {
      if (this.ParentId == value.ParentId)
      {
        Quaternion quaternion1 = Quaternion.Inverse(this.Rotation);
        ss.Active = this.Active;
        ss.ParentId = this.ParentId;
        ss.SkippedParent = this.SkippedParent;
        ss.InheritRotation = this.InheritRotation;
        Vector3D.Subtract(ref this.Position, ref value.Position, out ss.Position);
        Quaternion.Multiply(ref quaternion1, ref value.Rotation, out ss.Rotation);
        Vector3.Subtract(ref this.LinearVelocity, ref value.LinearVelocity, out ss.LinearVelocity);
        Vector3.Subtract(ref this.AngularVelocity, ref value.AngularVelocity, out ss.AngularVelocity);
        Vector3.Subtract(ref this.Pivot, ref value.Pivot, out ss.Pivot);
      }
      else
        ss = new MySnapshot();
    }

    public void Scale(float factor)
    {
      this.ScaleTransform(factor);
      this.LinearVelocity *= factor;
      this.AngularVelocity *= factor;
    }

    private void ScaleTransform(float factor)
    {
      Vector3 axis;
      float angle1;
      this.Rotation.GetAxisAngle(out axis, out angle1);
      float angle2 = angle1 * factor;
      Quaternion.CreateFromAxisAngle(ref axis, angle2, out this.Rotation);
      this.Position *= (double) factor;
      this.Pivot *= factor;
    }

    public bool CheckThresholds(float posSq, float rotSq, float linearSq, float angularSq) => this.Position.LengthSquared() > (double) posSq || (double) Math.Abs(this.Rotation.W - 1f) > (double) rotSq || (double) this.LinearVelocity.LengthSquared() > (double) linearSq || (double) this.AngularVelocity.LengthSquared() > (double) angularSq;

    public void Add(ref MySnapshot value)
    {
      if (this.ParentId != value.ParentId)
        return;
      this.Active = value.Active;
      this.InheritRotation = value.InheritRotation;
      this.Position += value.Position;
      this.Pivot += value.Pivot;
      this.Rotation *= Quaternion.Inverse(value.Rotation);
      this.Rotation.Normalize();
      this.LinearVelocity += value.LinearVelocity;
      this.AngularVelocity += value.AngularVelocity;
    }

    public void GetMatrix(
      MyEntity entity,
      out MatrixD mat,
      bool applyPosition = true,
      bool applyRotation = true)
    {
      MatrixD worldMatrix = entity.WorldMatrix;
      this.GetMatrix(out mat, ref worldMatrix, applyPosition, applyRotation);
    }

    public void GetMatrix(
      out MatrixD mat,
      ref MatrixD originalWorldMat,
      bool applyPosition = true,
      bool applyRotation = true)
    {
      MyEntity myEntity = (MyEntity) null;
      if (this.ParentId != 0L)
        myEntity = MyEntities.GetEntityById(this.ParentId);
      if (myEntity == null)
      {
        if (applyRotation)
          MatrixD.CreateFromQuaternion(ref this.Rotation, out mat);
        else
          mat = originalWorldMat;
        if (applyPosition)
        {
          mat.Translation = this.Position;
          mat.Translation = Vector3D.Transform((Vector3D) -this.Pivot, ref mat);
        }
        else
          mat.Translation = originalWorldMat.Translation;
      }
      else
      {
        MatrixD worldMatrix = myEntity.WorldMatrix;
        if (applyPosition & applyRotation)
        {
          MatrixD.CreateFromQuaternion(ref this.Rotation, out mat);
          mat.Translation = this.Position;
          if (this.InheritRotation)
            MatrixD.Multiply(ref mat, ref worldMatrix, out mat);
          else
            mat.Translation += worldMatrix.Translation;
        }
        else if (applyPosition)
        {
          mat = originalWorldMat;
          if (this.InheritRotation)
          {
            Vector3D result;
            Vector3D.Transform(ref this.Position, ref worldMatrix, out result);
            mat.Translation = result;
          }
          else
            mat.Translation = worldMatrix.Translation + this.Position;
        }
        else
        {
          MatrixD.CreateFromQuaternion(ref this.Rotation, out mat);
          if (this.InheritRotation)
            MatrixD.Multiply(ref mat, ref worldMatrix, out mat);
          mat.Translation = originalWorldMat.Translation;
        }
        mat.Translation = Vector3D.Transform((Vector3D) -this.Pivot, ref mat);
      }
    }

    public Vector3 GetLinearVelocity(bool local)
    {
      MyEntity myEntity = (MyEntity) null;
      if (this.ParentId != 0L)
        myEntity = MyEntities.GetEntityById(this.ParentId);
      if (myEntity == null | local)
        return this.LinearVelocity;
      if (!this.InheritRotation)
        return myEntity.Physics.LinearVelocity + this.LinearVelocity;
      Vector3D position = myEntity.PositionComp.GetPosition();
      Vector3 linearVelocity;
      myEntity.Physics.GetVelocityAtPointLocal(ref position, out linearVelocity);
      return linearVelocity + this.LinearVelocity;
    }

    public Vector3 GetAngularVelocity(bool local) => this.AngularVelocity;

    public void ApplyPhysics(MyEntity entity, bool angular = true, bool linear = true, bool local = false)
    {
      if (entity.Physics == null)
        return;
      if (this.Active)
      {
        if (linear)
          entity.Physics.LinearVelocity = this.GetLinearVelocity(local);
        if (angular)
          entity.Physics.AngularVelocity = this.GetAngularVelocity(local);
      }
      else
      {
        entity.Physics.LinearVelocity = Vector3.Zero;
        entity.Physics.AngularVelocity = Vector3.Zero;
      }
      HkRigidBody rigidBody = entity.Physics.RigidBody;
      if (!((HkReferenceObject) rigidBody != (HkReferenceObject) null) || rigidBody.IsFixed || rigidBody.IsActive == this.Active)
        return;
      if (this.Active)
        rigidBody.Activate();
      else
        rigidBody.Deactivate();
    }

    public void Lerp(ref MySnapshot value, float factor, out MySnapshot ss)
    {
      ss.Active = (double) factor > 1.0 ? value.Active : ((double) factor < 0.0 ? this.Active : this.Active || value.Active);
      ss.ParentId = this.ParentId == value.ParentId ? this.ParentId : -1L;
      ss.SkippedParent = this.SkippedParent;
      ss.InheritRotation = this.InheritRotation;
      Vector3D.Lerp(ref this.Position, ref value.Position, (double) factor, out ss.Position);
      Vector3.Lerp(ref this.Pivot, ref value.Pivot, factor, out ss.Pivot);
      Quaternion.Slerp(ref this.Rotation, ref value.Rotation, factor, out ss.Rotation);
      Vector3.Lerp(ref this.LinearVelocity, ref value.LinearVelocity, factor, out ss.LinearVelocity);
      Vector3.Lerp(ref this.AngularVelocity, ref value.AngularVelocity, factor, out ss.AngularVelocity);
      ss.Rotation.Normalize();
    }

    public void Write(BitStream stream)
    {
      stream.WriteVariantSigned(this.ParentId);
      stream.WriteBool(this.Active);
      stream.WriteBool(this.InheritRotation);
      stream.Write(this.Position);
      stream.Write(this.Pivot);
      stream.WriteQuaternion(this.Rotation);
      if (!this.Active)
        return;
      stream.Write(this.LinearVelocity);
      stream.Write(this.AngularVelocity);
    }

    private void Read(BitStream stream)
    {
      this.ParentId = stream.ReadInt64Variant();
      this.Active = stream.ReadBool();
      this.InheritRotation = stream.ReadBool();
      this.Position = stream.ReadVector3D();
      this.Pivot = stream.ReadVector3();
      this.Rotation = stream.ReadQuaternion();
      if (this.Active)
      {
        this.LinearVelocity = stream.ReadVector3();
        this.AngularVelocity = stream.ReadVector3();
      }
      else
      {
        this.LinearVelocity = Vector3.Zero;
        this.AngularVelocity = Vector3.Zero;
      }
    }

    public override string ToString() => " pos " + this.Position.ToString("N3") + " linVel " + this.LinearVelocity.ToString("N3");

    public bool SanityCheck() => this.Position.LengthSquared() < 250000.0 && (double) this.AngularVelocity.LengthSquared() < 250000.0 && (double) this.LinearVelocity.LengthSquared() < 160000.0;
  }
}
