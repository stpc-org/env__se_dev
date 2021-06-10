// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyMissileAmmoDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_MissileAmmoDefinition), null)]
  public class MyMissileAmmoDefinition : MyAmmoDefinition
  {
    public const float MINIMAL_EXPLOSION_RADIUS = 0.6f;
    public float MissileMass;
    public float MissileExplosionRadius;
    public string MissileModelName;
    public float MissileAcceleration;
    public float MissileInitialSpeed;
    public bool MissileSkipAcceleration;
    public float MissileExplosionDamage;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_MissileAmmoDefinition missileAmmoDefinition = builder as MyObjectBuilder_MissileAmmoDefinition;
      this.AmmoType = MyAmmoType.Missile;
      MyObjectBuilder_MissileAmmoDefinition.AmmoMissileProperties missileProperties = missileAmmoDefinition.MissileProperties;
      this.MissileAcceleration = missileProperties.MissileAcceleration;
      this.MissileExplosionDamage = missileProperties.MissileExplosionDamage;
      this.MissileExplosionRadius = missileProperties.MissileExplosionRadius;
      this.MissileInitialSpeed = missileProperties.MissileInitialSpeed;
      this.MissileMass = missileProperties.MissileMass;
      this.MissileModelName = missileProperties.MissileModelName;
      this.MissileSkipAcceleration = missileProperties.MissileSkipAcceleration;
    }

    public override float GetDamageForMechanicalObjects() => this.MissileExplosionDamage;

    private class Sandbox_Definitions_MyMissileAmmoDefinition\u003C\u003EActor : IActivator, IActivator<MyMissileAmmoDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyMissileAmmoDefinition();

      MyMissileAmmoDefinition IActivator<MyMissileAmmoDefinition>.CreateInstance() => new MyMissileAmmoDefinition();
    }
  }
}
