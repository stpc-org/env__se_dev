// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.MyExplosionInfo
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game
{
  public struct MyExplosionInfo
  {
    public float PlayerDamage;
    public float Damage;
    public BoundingSphereD ExplosionSphere;
    public float StrengthImpulse;
    public MyEntity ExcludedEntity;
    public MyEntity OwnerEntity;
    public MyEntity HitEntity;
    public MyExplosionFlags ExplosionFlags;
    public MyExplosionTypeEnum ExplosionType;
    public int LifespanMiliseconds;
    public int ObjectsRemoveDelayInMiliseconds;
    public float ParticleScale;
    public float VoxelCutoutScale;
    public Vector3? Direction;
    public Vector3D VoxelExplosionCenter;
    public bool PlaySound;
    public bool CheckIntersections;
    public Vector3 Velocity;
    public long OriginEntity;
    public string CustomEffect;
    public MySoundPair CustomSound;
    public bool KeepAffectedBlocks;
    public bool IgnoreFriendlyFireSetting;

    public MyExplosionInfo(
      float playerDamage,
      float damage,
      BoundingSphereD explosionSphere,
      MyExplosionTypeEnum type,
      bool playSound,
      bool checkIntersection = true)
    {
      this.PlayerDamage = playerDamage;
      this.Damage = damage;
      this.ExplosionSphere = explosionSphere;
      this.StrengthImpulse = 0.0f;
      this.ExcludedEntity = this.OwnerEntity = this.HitEntity = (MyEntity) null;
      this.ExplosionFlags = MyExplosionFlags.CREATE_DEBRIS | MyExplosionFlags.AFFECT_VOXELS | MyExplosionFlags.APPLY_FORCE_AND_DAMAGE | MyExplosionFlags.CREATE_DECALS | MyExplosionFlags.CREATE_PARTICLE_EFFECT | MyExplosionFlags.APPLY_DEFORMATION;
      this.ExplosionType = type;
      this.LifespanMiliseconds = 700;
      this.ObjectsRemoveDelayInMiliseconds = 0;
      this.ParticleScale = 1f;
      this.VoxelCutoutScale = 1f;
      this.Direction = new Vector3?();
      this.VoxelExplosionCenter = explosionSphere.Center;
      this.PlaySound = playSound;
      this.CheckIntersections = checkIntersection;
      this.Velocity = Vector3.Zero;
      this.OriginEntity = 0L;
      this.CustomEffect = "";
      this.CustomSound = (MySoundPair) null;
      this.KeepAffectedBlocks = false;
      this.IgnoreFriendlyFireSetting = true;
    }

    private void SetFlag(MyExplosionFlags flag, bool value)
    {
      if (value)
        this.ExplosionFlags |= flag;
      else
        this.ExplosionFlags &= ~flag;
    }

    private bool HasFlag(MyExplosionFlags flag) => (this.ExplosionFlags & flag) == flag;

    public bool AffectVoxels
    {
      get => this.HasFlag(MyExplosionFlags.AFFECT_VOXELS);
      set => this.SetFlag(MyExplosionFlags.AFFECT_VOXELS, value);
    }

    public bool CreateDebris
    {
      get => this.HasFlag(MyExplosionFlags.CREATE_DEBRIS);
      set => this.SetFlag(MyExplosionFlags.CREATE_DEBRIS, value);
    }

    public bool CreateParticleDebris
    {
      get => this.HasFlag(MyExplosionFlags.CREATE_PARTICLE_DEBRIS);
      set => this.SetFlag(MyExplosionFlags.CREATE_PARTICLE_DEBRIS, value);
    }

    public bool ApplyForceAndDamage
    {
      get => this.HasFlag(MyExplosionFlags.APPLY_FORCE_AND_DAMAGE);
      set => this.SetFlag(MyExplosionFlags.APPLY_FORCE_AND_DAMAGE, value);
    }

    public bool CreateDecals
    {
      get => this.HasFlag(MyExplosionFlags.CREATE_DECALS);
      set => this.SetFlag(MyExplosionFlags.CREATE_DECALS, value);
    }

    public bool ForceDebris
    {
      get => this.HasFlag(MyExplosionFlags.FORCE_DEBRIS);
      set => this.SetFlag(MyExplosionFlags.FORCE_DEBRIS, value);
    }

    public bool CreateParticleEffect
    {
      get => this.HasFlag(MyExplosionFlags.CREATE_PARTICLE_EFFECT);
      set => this.SetFlag(MyExplosionFlags.CREATE_PARTICLE_EFFECT, value);
    }

    public bool CreateShrapnels
    {
      get => this.HasFlag(MyExplosionFlags.CREATE_SHRAPNELS);
      set => this.SetFlag(MyExplosionFlags.CREATE_SHRAPNELS, value);
    }
  }
}
