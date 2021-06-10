// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.Renders.MyRenderComponentWheel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using System;
using VRage.Game;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.EntityComponents.Renders
{
  public class MyRenderComponentWheel : MyRenderComponentCubeBlock
  {
    private MyParticleEffect m_dustParticleEffect;
    private string m_dustParticleName = string.Empty;
    private Vector3 m_relativePosition = Vector3.Zero;
    private int m_timer;
    private MyWheel m_wheel;
    private const int PARTICLE_GENERATION_TIMEOUT = 20;
    private const float PARTICLE_SCALE_MIN = 0.3f;
    private const float PARTICLE_SCALE_MAX = 0.7f;
    private const float PARTICLE_EMITTER_SCALE = 2f;

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      this.m_wheel = this.Entity as MyWheel;
    }

    public override void OnBeforeRemovedFromContainer()
    {
      base.OnBeforeRemovedFromContainer();
      if (this.m_dustParticleEffect != null)
        this.m_dustParticleEffect.Stop(false);
      this.m_wheel = (MyWheel) null;
    }

    public override void OnRemovedFromScene()
    {
      base.OnRemovedFromScene();
      if (this.m_dustParticleEffect == null)
        return;
      this.m_dustParticleEffect.Stop(false);
    }

    public bool TrySpawnParticle(string particleName, ref Vector3D position, ref Vector3 normal)
    {
      if (!MyFakes.ENABLE_DRIVING_PARTICLES)
        return false;
      if (this.m_dustParticleEffect == null || !particleName.Equals(this.m_dustParticleName))
      {
        if (this.m_dustParticleEffect != null)
        {
          this.m_dustParticleEffect.Stop(false);
          this.m_dustParticleEffect = (MyParticleEffect) null;
        }
        if ((double) this.m_wheel.GetTopMostParent((Type) null).Physics.LinearVelocity.LengthSquared() > 0.100000001490116)
        {
          this.m_dustParticleName = particleName;
          MyParticlesManager.TryCreateParticleEffect(this.m_dustParticleName, this.GetParticleMatrix(ref position, ref normal), out this.m_dustParticleEffect);
          if (this.m_dustParticleEffect != null)
            this.m_dustParticleEffect.UserScale = 2f;
          this.m_timer = 20;
        }
      }
      return true;
    }

    private MatrixD GetParticleMatrix(ref Vector3D position, ref Vector3 normal)
    {
      Vector3 vector2 = Vector3.Cross(normal, this.m_wheel.GetTopMostParent((Type) null).Physics.LinearVelocity);
      Vector3 up = Vector3.Cross(normal, vector2);
      return MatrixD.CreateWorld(position, normal, up);
    }

    public void UpdateParticle(ref Vector3D position, ref Vector3 normal)
    {
      if (this.m_dustParticleEffect == null)
        return;
      float num = this.m_wheel.GetTopMostParent((Type) null).Physics.LinearVelocity.LengthSquared();
      if ((double) num < 0.100000001490116)
      {
        this.m_dustParticleEffect.Stop(false);
        this.m_dustParticleEffect = (MyParticleEffect) null;
      }
      else
      {
        float amount = Math.Min((float) (5.0 * (double) num / ((double) MyGridPhysics.ShipMaxLinearVelocity() * (double) MyGridPhysics.ShipMaxLinearVelocity())), 1f);
        this.m_relativePosition = (Vector3) (position - this.m_wheel.WorldMatrix.Translation);
        this.m_dustParticleEffect.WorldMatrix = this.GetParticleMatrix(ref position, ref normal);
        this.m_dustParticleEffect.UserScale = 2f;
        this.m_dustParticleEffect.UserRadiusMultiplier = MathHelper.Lerp(0.3f, 0.7f, amount);
        this.m_timer = 20;
      }
    }

    public void UpdatePosition()
    {
      if (this.m_dustParticleEffect == null)
        return;
      --this.m_timer;
      if (this.m_timer <= 0)
      {
        this.m_dustParticleEffect.Stop(false);
        this.m_dustParticleEffect = (MyParticleEffect) null;
      }
      else
      {
        Vector3D trans = this.m_wheel.WorldMatrix.Translation + this.m_relativePosition;
        this.m_dustParticleEffect.SetTranslation(ref trans);
      }
    }

    public bool UpdateNeeded => this.m_timer > 0;

    private class Sandbox_Game_EntityComponents_Renders_MyRenderComponentWheel\u003C\u003EActor : IActivator, IActivator<MyRenderComponentWheel>
    {
      object IActivator.CreateInstance() => (object) new MyRenderComponentWheel();

      MyRenderComponentWheel IActivator<MyRenderComponentWheel>.CreateInstance() => new MyRenderComponentWheel();
    }
  }
}
