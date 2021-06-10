// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Debris.MyDebrisTree
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using VRage.Game.Components;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Entities.Debris
{
  internal class MyDebrisTree : MyDebrisBase
  {
    public MyDebrisTree()
    {
      this.GameLogic = (MyGameLogicComponent) new MyDebrisTree.MyDebrisTreeLogic();
      if (!MyDebugDrawSettings.DEBUG_DRAW_TREE_COLLISION_SHAPES)
        return;
      this.NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME;
    }

    public override void UpdateBeforeSimulation()
    {
      if (!MyDebugDrawSettings.DEBUG_DRAW_TREE_COLLISION_SHAPES)
        return;
      MyPhysicsComponentBase physics = this.Physics;
      if (physics == null)
        return;
      HkRigidBody rigidBody = physics.RigidBody;
      if ((HkReferenceObject) rigidBody == (HkReferenceObject) null)
        return;
      int shapeIndex = 0;
      MyPhysicsDebugDraw.DrawCollisionShape(rigidBody.GetShape(), physics.GetWorldMatrix(), 1f, ref shapeIndex, physics.IsActive ? "A" : "I");
    }

    protected class MyDebrisTreeLogic : MyDebrisBase.MyDebrisBaseLogic
    {
      private const float AVERAGE_WOOD_DENSITY = 800f;

      protected override MyPhysicsComponentBase GetPhysics(
        RigidBodyFlag rigidBodyFlag)
      {
        return (MyPhysicsComponentBase) new MyDebrisTree.MyDebrisTreePhysics(this.Entity, rigidBodyFlag);
      }

      protected override float CalculateMass(float radius)
      {
        float height;
        float radius1;
        MyDebrisTree.MyDebrisTreePhysics.ComputeShapeDimensions(this.Entity.Model.BoundingBox, out height, out radius1);
        return (float) (3.14159297943115 * ((double) radius1 * (double) radius1) * (1.33333337306976 * (double) radius1 + (double) height) * 800.0);
      }

      public override void Init(MyDebrisBaseDescription desc)
      {
        base.Init(desc);
        HkRigidBody rigidBody = this.Entity.Physics.RigidBody;
        rigidBody.EnableDeactivation = false;
        rigidBody.MaxAngularVelocity = 2f;
      }

      private class Sandbox_Game_Entities_Debris_MyDebrisTree\u003C\u003EMyDebrisTreeLogic\u003C\u003EActor : IActivator, IActivator<MyDebrisTree.MyDebrisTreeLogic>
      {
        object IActivator.CreateInstance() => (object) new MyDebrisTree.MyDebrisTreeLogic();

        MyDebrisTree.MyDebrisTreeLogic IActivator<MyDebrisTree.MyDebrisTreeLogic>.CreateInstance() => new MyDebrisTree.MyDebrisTreeLogic();
      }
    }

    protected class MyDebrisTreePhysics : MyDebrisBase.MyDebrisPhysics
    {
      public MyDebrisTreePhysics(IMyEntity entity, RigidBodyFlag rigidBodyFlags)
        : base(entity, rigidBodyFlags)
      {
      }

      public override void CreatePhysicsShape(
        out HkShape shape,
        out HkMassProperties massProperties,
        float mass)
      {
        shape = HkShape.Empty;
        MyModel model = this.Entity.Render.GetModel();
        BoundingBox boundingBox = model.BoundingBox;
        float height;
        float radius;
        MyDebrisTree.MyDebrisTreePhysics.ComputeShapeDimensions(boundingBox, out height, out radius);
        Vector3 vector3_1 = Vector3.Up * height;
        Vector3 vector3_2 = (boundingBox.Min + boundingBox.Max) * 0.5f;
        Vector3 vector3_3 = vector3_2 + vector3_1 * 0.2f;
        Vector3 vector3_4 = vector3_2 - vector3_1 * 0.45f;
        bool flag = true;
        if (MyFakes.TREE_MESH_FROM_MODEL)
        {
          HkShape[] havokCollisionShapes = model.HavokCollisionShapes;
          if (havokCollisionShapes != null && havokCollisionShapes.Length != 0)
          {
            if (havokCollisionShapes.Length == 1)
            {
              shape = havokCollisionShapes[0];
              shape.AddReference();
            }
            else
              shape = (HkShape) new HkListShape(havokCollisionShapes, HkReferencePolicy.None);
            flag = false;
          }
        }
        if (flag)
          shape = (HkShape) new HkCapsuleShape(vector3_3, vector3_4, radius);
        massProperties = HkInertiaTensorComputer.ComputeCapsuleVolumeMassProperties(vector3_3, vector3_4, radius, mass);
      }

      public static void ComputeShapeDimensions(
        BoundingBox bBox,
        out float height,
        out float radius)
      {
        height = bBox.Height;
        radius = height / 20f;
        height -= 2f * radius;
      }

      private class Sandbox_Game_Entities_Debris_MyDebrisTree\u003C\u003EMyDebrisTreePhysics\u003C\u003EActor
      {
      }
    }

    private class Sandbox_Game_Entities_Debris_MyDebrisTree\u003C\u003EActor : IActivator, IActivator<MyDebrisTree>
    {
      object IActivator.CreateInstance() => (object) new MyDebrisTree();

      MyDebrisTree IActivator<MyDebrisTree>.CreateInstance() => new MyDebrisTree();
    }
  }
}
