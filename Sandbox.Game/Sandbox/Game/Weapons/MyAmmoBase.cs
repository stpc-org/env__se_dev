// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.MyAmmoBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Weapons
{
  public class MyAmmoBase : MyEntity
  {
    protected Vector3D m_origin;
    protected MyWeaponDefinition m_weaponDefinition;
    protected long m_originEntity;
    private float m_ammoOffsetSize;
    protected bool m_shouldExplode;
    protected bool m_markedToDestroy;
    protected bool m_physicsEnabled;

    public MyPhysicsBody Physics => base.Physics as MyPhysicsBody;

    protected MyAmmoBase() => this.Save = false;

    protected void Init(
      MyWeaponPropertiesWrapper weaponProperties,
      string modelName,
      bool spherePhysics = true,
      bool capsulePhysics = false,
      bool bulletType = false,
      bool physics = true)
    {
      int num = MyEntityIdentifier.AllocationSuspended ? 1 : 0;
      MyEntityIdentifier.AllocationSuspended = true;
      this.Init((StringBuilder) null, modelName, (MyEntity) null, new float?());
      this.m_weaponDefinition = weaponProperties.WeaponDefinition;
      this.m_physicsEnabled = physics;
      if (physics)
      {
        if (spherePhysics)
          this.InitSpherePhysics(MyMaterialType.AMMO, this.Model, 100f, MyPerGameSettings.DefaultLinearDamping, MyPerGameSettings.DefaultAngularDamping, (ushort) 27, bulletType ? RigidBodyFlag.RBF_BULLET : RigidBodyFlag.RBF_DEFAULT);
        else if (capsulePhysics)
        {
          this.InitCapsulePhysics(MyMaterialType.AMMO, new Vector3(0.0f, 0.0f, (float) (-(double) this.Model.BoundingBox.HalfExtents.Z * 0.800000011920929)), new Vector3(0.0f, 0.0f, this.Model.BoundingBox.HalfExtents.Z * 0.8f), 0.1f, 10f, 0.0f, 0.0f, (ushort) 27, bulletType ? RigidBodyFlag.RBF_BULLET : RigidBodyFlag.RBF_DEFAULT);
          this.m_ammoOffsetSize = (float) ((double) this.Model.BoundingBox.HalfExtents.Z * 0.800000011920929 + 0.100000001490116);
        }
        else
          this.InitBoxPhysics(MyMaterialType.AMMO, this.Model, 1f, MyPerGameSettings.DefaultAngularDamping, (ushort) 27, bulletType ? RigidBodyFlag.RBF_BULLET : RigidBodyFlag.RBF_DEFAULT);
        this.Physics.RigidBody.ContactPointCallbackEnabled = true;
        this.Physics.ContactPointCallback += new MyPhysicsBody.PhysicsContactHandler(this.OnContactPointCallback);
      }
      this.NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME;
      this.Render.CastShadows = false;
      MyEntityIdentifier.AllocationSuspended = num != 0;
    }

    protected void Start(Vector3D position, Vector3D initialVelocity, Vector3D direction)
    {
      this.m_shouldExplode = false;
      this.m_origin = position + direction * (double) this.m_ammoOffsetSize;
      this.m_markedToDestroy = false;
      MatrixD world = MatrixD.CreateWorld(this.m_origin, direction, Vector3D.CalculatePerpendicularVector(direction));
      this.PositionComp.SetWorldMatrix(ref world);
      if (this.m_physicsEnabled)
      {
        this.Physics.Clear();
        this.Physics.Enabled = true;
        this.Physics.LinearVelocity = (Vector3) initialVelocity;
      }
      this.NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME;
    }

    public virtual void MarkForDestroy()
    {
      this.m_markedToDestroy = true;
      this.Close();
    }

    protected override void Closing()
    {
      base.Closing();
      if (!this.m_physicsEnabled)
        return;
      this.Physics.ContactPointCallback -= new MyPhysicsBody.PhysicsContactHandler(this.OnContactPointCallback);
    }

    private void OnContactPointCallback(ref MyPhysics.MyContactPointEvent value)
    {
      if (value.ContactPointEvent.EventType == HkContactPointEvent.Type.ManifoldAtEndOfStep)
        return;
      this.OnContactStart(ref value);
    }

    protected virtual void OnContactStart(ref MyPhysics.MyContactPointEvent value)
    {
    }

    private class Sandbox_Game_Weapons_MyAmmoBase\u003C\u003EActor : IActivator, IActivator<MyAmmoBase>
    {
      object IActivator.CreateInstance() => (object) new MyAmmoBase();

      MyAmmoBase IActivator<MyAmmoBase>.CreateInstance() => new MyAmmoBase();
    }
  }
}
