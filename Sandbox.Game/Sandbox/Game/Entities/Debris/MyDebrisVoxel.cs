// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Debris.MyDebrisVoxel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Definitions;
using Sandbox.Game.Components;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Debris
{
  internal class MyDebrisVoxel : MyDebrisBase
  {
    public override void InitComponents()
    {
      this.GameLogic = (MyGameLogicComponent) new MyDebrisVoxel.MyDebrisVoxelLogic();
      this.Render = (MyRenderComponentBase) new MyRenderComponentDebrisVoxel();
      base.InitComponents();
    }

    internal class MyDebrisVoxelPhysics : MyDebrisBase.MyDebrisPhysics
    {
      private const float VoxelDensity = 260f;

      public MyDebrisVoxelPhysics(IMyEntity entity, RigidBodyFlag rigidBodyFlag)
        : base(entity, rigidBodyFlag)
      {
      }

      public override void CreatePhysicsShape(
        out HkShape shape,
        out HkMassProperties massProperties,
        float mass)
      {
        HkSphereShape hkSphereShape = new HkSphereShape(0.5f * ((MyEntity) this.Entity).Render.GetModel().BoundingSphere.Radius * this.Entity.PositionComp.Scale.Value);
        shape = (HkShape) hkSphereShape;
        massProperties = HkInertiaTensorComputer.ComputeSphereVolumeMassProperties(hkSphereShape.Radius * 0.5f, mass);
      }

      public override void ScalePhysicsShape(ref HkMassProperties massProperties)
      {
        HkSphereShape shape = (HkSphereShape) this.RigidBody.GetShape();
        shape.Radius = ((MyEntity) this.Entity).Render.GetModel().BoundingSphere.Radius * this.Entity.PositionComp.Scale.Value;
        float mass = this.SphereMass(shape.Radius, 260f);
        massProperties = HkInertiaTensorComputer.ComputeSphereVolumeMassProperties(shape.Radius, mass);
        int num1 = (int) this.RigidBody.SetShape((HkShape) shape);
        this.RigidBody.SetMassProperties(ref massProperties);
        int num2 = (int) this.RigidBody.UpdateShape();
      }

      private float SphereMass(float radius, float density) => (float) ((double) radius * (double) radius * (double) radius * 3.14159297943115 * 4.0 * 0.333000004291534) * density;

      private class Sandbox_Game_Entities_Debris_MyDebrisVoxel\u003C\u003EMyDebrisVoxelPhysics\u003C\u003EActor
      {
      }
    }

    internal class MyDebrisVoxelLogic : MyDebrisBase.MyDebrisBaseLogic
    {
      protected override MyPhysicsComponentBase GetPhysics(
        RigidBodyFlag rigidBodyFlag)
      {
        return (MyPhysicsComponentBase) new MyDebrisVoxel.MyDebrisVoxelPhysics(this.Container.Entity, rigidBodyFlag);
      }

      public override void Start(Vector3D position, Vector3D initialVelocity) => this.Start(position, initialVelocity, MyDefinitionManager.Static.GetDefaultVoxelMaterialDefinition());

      public void Start(Vector3D position, Vector3D initialVelocity, MyVoxelMaterialDefinition mat)
      {
        MyRenderComponentDebrisVoxel render = this.Container.Entity.Render as MyRenderComponentDebrisVoxel;
        render.TexCoordOffset = MyUtils.GetRandomFloat(5f, 15f);
        render.TexCoordScale = MyUtils.GetRandomFloat(8f, 12f);
        render.VoxelMaterialIndex = mat.Index;
        base.Start(position, initialVelocity);
        this.Container.Entity.Render.NeedsResolveCastShadow = true;
        this.Container.Entity.Render.FastCastShadowResolve = true;
      }

      private class Sandbox_Game_Entities_Debris_MyDebrisVoxel\u003C\u003EMyDebrisVoxelLogic\u003C\u003EActor : IActivator, IActivator<MyDebrisVoxel.MyDebrisVoxelLogic>
      {
        object IActivator.CreateInstance() => (object) new MyDebrisVoxel.MyDebrisVoxelLogic();

        MyDebrisVoxel.MyDebrisVoxelLogic IActivator<MyDebrisVoxel.MyDebrisVoxelLogic>.CreateInstance() => new MyDebrisVoxel.MyDebrisVoxelLogic();
      }
    }

    private class Sandbox_Game_Entities_Debris_MyDebrisVoxel\u003C\u003EActor : IActivator, IActivator<MyDebrisVoxel>
    {
      object IActivator.CreateInstance() => (object) new MyDebrisVoxel();

      MyDebrisVoxel IActivator<MyDebrisVoxel>.CreateInstance() => new MyDebrisVoxel();
    }
  }
}
