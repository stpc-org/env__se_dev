// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyProjectileAmmoDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ProjectileAmmoDefinition), null)]
  public class MyProjectileAmmoDefinition : MyAmmoDefinition
  {
    public float ProjectileHitImpulse;
    public float ProjectileTrailScale;
    public Vector3 ProjectileTrailColor;
    public string ProjectileTrailMaterial;
    public float ProjectileTrailProbability;
    public string ProjectileOnHitEffectName;
    public float ProjectileMassDamage;
    public float ProjectileHealthDamage;
    public bool HeadShot;
    public float ProjectileHeadShotDamage;
    public int ProjectileCount;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_ProjectileAmmoDefinition projectileAmmoDefinition = builder as MyObjectBuilder_ProjectileAmmoDefinition;
      this.AmmoType = MyAmmoType.HighSpeed;
      MyObjectBuilder_ProjectileAmmoDefinition.AmmoProjectileProperties projectileProperties = projectileAmmoDefinition.ProjectileProperties;
      this.ProjectileHealthDamage = projectileProperties.ProjectileHealthDamage;
      this.ProjectileHitImpulse = projectileProperties.ProjectileHitImpulse;
      this.ProjectileMassDamage = projectileProperties.ProjectileMassDamage;
      this.ProjectileOnHitEffectName = projectileProperties.ProjectileOnHitEffectName;
      this.ProjectileTrailColor = (Vector3) projectileProperties.ProjectileTrailColor;
      this.ProjectileTrailMaterial = projectileProperties.ProjectileTrailMaterial;
      this.ProjectileTrailProbability = projectileProperties.ProjectileTrailProbability;
      this.ProjectileTrailScale = projectileProperties.ProjectileTrailScale;
      this.HeadShot = projectileProperties.HeadShot;
      this.ProjectileHeadShotDamage = projectileProperties.ProjectileHeadShotDamage;
      this.ProjectileCount = projectileProperties.ProjectileCount;
    }

    public override float GetDamageForMechanicalObjects() => this.ProjectileMassDamage;

    private class Sandbox_Definitions_MyProjectileAmmoDefinition\u003C\u003EActor : IActivator, IActivator<MyProjectileAmmoDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyProjectileAmmoDefinition();

      MyProjectileAmmoDefinition IActivator<MyProjectileAmmoDefinition>.CreateInstance() => new MyProjectileAmmoDefinition();
    }
  }
}
