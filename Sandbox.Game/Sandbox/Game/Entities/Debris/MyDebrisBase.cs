// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Debris.MyDebrisBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Game.Components;
using Sandbox.Game.GameSystems;
using System;
using System.Text;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Debris
{
  public class MyDebrisBase : MyEntity
  {
    private const float STONE_DENSITY = 2600f;

    public MyDebrisBase.MyDebrisBaseLogic Debris => (MyDebrisBase.MyDebrisBaseLogic) this.GameLogic;

    public override void InitComponents()
    {
      if (this.GameLogic == null)
        this.GameLogic = (MyGameLogicComponent) new MyDebrisBase.MyDebrisBaseLogic();
      base.InitComponents();
    }

    public class MyDebrisPhysics : MyPhysicsBody
    {
      public MyDebrisPhysics(IMyEntity entity, RigidBodyFlag rigidBodyFlag)
        : base(entity, rigidBodyFlag)
      {
      }

      public virtual void CreatePhysicsShape(
        out HkShape shape,
        out HkMassProperties massProperties,
        float mass)
      {
        HkBoxShape hkBoxShape = new HkBoxShape((((MyEntity) this.Entity).Render.GetModel().BoundingBox.Max - ((MyEntity) this.Entity).Render.GetModel().BoundingBox.Min) / 2f * this.Entity.PositionComp.Scale.Value);
        Vector3 translation = (((MyEntity) this.Entity).Render.GetModel().BoundingBox.Max + ((MyEntity) this.Entity).Render.GetModel().BoundingBox.Min) / 2f;
        shape = (HkShape) new HkTransformShape((HkShape) hkBoxShape, ref translation, ref Quaternion.Identity);
        massProperties = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(hkBoxShape.HalfExtents, mass);
        massProperties.CenterOfMass = translation;
      }

      public virtual void ScalePhysicsShape(ref HkMassProperties massProperties)
      {
        MyModel model = this.Entity.Render.GetModel();
        HkShape shape1;
        if (model.HavokCollisionShapes != null && model.HavokCollisionShapes.Length != 0)
        {
          shape1 = model.HavokCollisionShapes[0];
          Vector4 min;
          Vector4 max;
          shape1.GetLocalAABB(0.1f, out min, out max);
          Vector3 halfExtents = new Vector3((max - min) * 0.5f);
          massProperties = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(halfExtents, halfExtents.Volume * 50f);
          massProperties.CenterOfMass = new Vector3((min + max) * 0.5f);
        }
        else
        {
          HkTransformShape shape2 = (HkTransformShape) this.RigidBody.GetShape();
          HkBoxShape childShape = (HkBoxShape) shape2.ChildShape;
          childShape.HalfExtents = (model.BoundingBox.Max - model.BoundingBox.Min) / 2f * this.Entity.PositionComp.Scale.Value;
          massProperties = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(childShape.HalfExtents, childShape.HalfExtents.Volume * 0.5f);
          massProperties.CenterOfMass = shape2.Transform.Translation;
          shape1 = (HkShape) shape2;
        }
        int num1 = (int) this.RigidBody.SetShape(shape1);
        this.RigidBody.SetMassProperties(ref massProperties);
        int num2 = (int) this.RigidBody.UpdateShape();
      }

      private class Sandbox_Game_Entities_Debris_MyDebrisBase\u003C\u003EMyDebrisPhysics\u003C\u003EActor
      {
      }
    }

    public class MyDebrisBaseLogic : MyEntityGameLogic
    {
      private MyDebrisBase m_debris;
      private Action<MyDebrisBase> m_onCloseCallback;
      private bool m_isStarted;
      private int m_createdTime;
      public int LifespanInMiliseconds;
      protected HkMassProperties m_massProperties;

      public virtual void Init(MyDebrisBaseDescription desc)
      {
        this.Init((StringBuilder) null, desc.Model, (MyEntity) null, new float?(1f));
        this.LifespanInMiliseconds = MyUtils.GetRandomInt(desc.LifespanMinInMiliseconds, desc.LifespanMaxInMiliseconds);
        MyDebrisBase.MyDebrisPhysics physics = (MyDebrisBase.MyDebrisPhysics) this.GetPhysics(RigidBodyFlag.RBF_DEBRIS);
        this.Container.Entity.Physics = (MyPhysicsComponentBase) physics;
        float mass = this.CalculateMass(((MyEntity) this.Entity).Render.GetModel().BoundingSphere.Radius);
        HkShape shape;
        physics.CreatePhysicsShape(out shape, out this.m_massProperties, mass);
        physics.CreateFromCollisionObject(shape, Vector3.Zero, MatrixD.Identity, new HkMassProperties?(this.m_massProperties), 20);
        HkMassChangerUtil.Create(physics.RigidBody, -21, 1f, 0.0f);
        new HkEasePenetrationAction(physics.RigidBody, 3f)
        {
          InitialAllowedPenetrationDepthMultiplier = 10f
        }.Dispose();
        this.Container.Entity.Physics.Enabled = false;
        shape.RemoveReference();
        this.m_entity.Save = false;
        this.Container.Entity.Physics.PlayCollisionCueEnabled = true;
        this.NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME;
        this.Container.Entity.NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME;
        this.m_onCloseCallback = desc.OnCloseAction;
      }

      protected virtual float CalculateMass(float r) => (float) (3.14159297943115 * ((double) r * (double) r) * (1.33333337306976 * (double) r) * 2600.0);

      protected virtual MyPhysicsComponentBase GetPhysics(
        RigidBodyFlag rigidBodyFlag)
      {
        return (MyPhysicsComponentBase) new MyDebrisBase.MyDebrisPhysics(this.Container.Entity, rigidBodyFlag);
      }

      public virtual void Free()
      {
        if (this.Container.Entity.Physics == null)
          return;
        this.Container.Entity.Physics.Close();
        this.Container.Entity.Physics = (MyPhysicsComponentBase) null;
      }

      public virtual void Start(Vector3D position, Vector3D initialVelocity) => this.Start(MatrixD.CreateTranslation(position), initialVelocity);

      public virtual void Start(MatrixD position, Vector3D initialVelocity, bool randomRotation = true)
      {
        this.m_createdTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
        this.Container.Entity.WorldMatrix = position;
        this.Container.Entity.Physics.Clear();
        this.Container.Entity.Physics.LinearVelocity = (Vector3) initialVelocity;
        if (randomRotation)
          this.Container.Entity.Physics.AngularVelocity = new Vector3(MyUtils.GetRandomRadian(), MyUtils.GetRandomRadian(), MyUtils.GetRandomRadian());
        MyEntities.Add(this.m_entity);
        this.Container.Entity.Physics.Enabled = true;
        this.Container.Entity.Physics.RigidBody.Gravity = MyGravityProviderSystem.CalculateNaturalGravityInPoint(position.Translation);
        this.m_isStarted = true;
      }

      public override void OnAddedToContainer()
      {
        base.OnAddedToContainer();
        this.m_debris = this.Container.Entity as MyDebrisBase;
      }

      public override void UpdateAfterSimulation()
      {
        base.UpdateAfterSimulation();
        if (!this.m_isStarted)
          return;
        int num1 = MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_createdTime;
        if (num1 > this.LifespanInMiliseconds || MyDebris.Static.TooManyDebris)
        {
          this.MarkForClose();
        }
        else
        {
          float num2 = (float) num1 / (float) this.LifespanInMiliseconds;
          float num3 = 0.75f;
          if ((double) num2 <= (double) num3)
            return;
          uint renderObjectId = this.Container.Entity.Render.GetRenderObjectID();
          if (renderObjectId == uint.MaxValue)
            return;
          MyRenderProxy.UpdateRenderEntity(renderObjectId, new Color?(), new Vector3?(), new float?((float) (((double) num2 - (double) num3) / (1.0 - (double) num3))));
        }
      }

      public override void MarkForClose()
      {
        if (this.m_onCloseCallback != null)
        {
          this.m_onCloseCallback(this.m_debris);
          this.m_onCloseCallback = (Action<MyDebrisBase>) null;
        }
        base.MarkForClose();
      }

      public override void Close() => base.Close();

      private class Sandbox_Game_Entities_Debris_MyDebrisBase\u003C\u003EMyDebrisBaseLogic\u003C\u003EActor : IActivator, IActivator<MyDebrisBase.MyDebrisBaseLogic>
      {
        object IActivator.CreateInstance() => (object) new MyDebrisBase.MyDebrisBaseLogic();

        MyDebrisBase.MyDebrisBaseLogic IActivator<MyDebrisBase.MyDebrisBaseLogic>.CreateInstance() => new MyDebrisBase.MyDebrisBaseLogic();
      }
    }

    private class Sandbox_Game_Entities_Debris_MyDebrisBase\u003C\u003EActor : IActivator, IActivator<MyDebrisBase>
    {
      object IActivator.CreateInstance() => (object) new MyDebrisBase();

      MyDebrisBase IActivator<MyDebrisBase>.CreateInstance() => new MyDebrisBase();
    }
  }
}
