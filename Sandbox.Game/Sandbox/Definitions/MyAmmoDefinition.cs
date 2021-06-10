// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyAmmoDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_AmmoDefinition), null)]
  public abstract class MyAmmoDefinition : MyDefinitionBase
  {
    public MyAmmoType AmmoType;
    public float DesiredSpeed;
    public float SpeedVar;
    public float MaxTrajectory;
    public bool IsExplosive;
    public float BackkickForce;
    public MyStringHash PhysicalMaterial;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_AmmoDefinition builderAmmoDefinition = builder as MyObjectBuilder_AmmoDefinition;
      this.DesiredSpeed = builderAmmoDefinition.BasicProperties.DesiredSpeed;
      this.SpeedVar = MathHelper.Clamp(builderAmmoDefinition.BasicProperties.SpeedVariance, 0.0f, 1f);
      this.MaxTrajectory = builderAmmoDefinition.BasicProperties.MaxTrajectory;
      this.IsExplosive = builderAmmoDefinition.BasicProperties.IsExplosive;
      this.BackkickForce = builderAmmoDefinition.BasicProperties.BackkickForce;
      this.PhysicalMaterial = MyStringHash.GetOrCompute(builderAmmoDefinition.BasicProperties.PhysicalMaterial);
    }

    public abstract float GetDamageForMechanicalObjects();
  }
}
