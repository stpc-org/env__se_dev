// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyLargeTurretBaseDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_LargeTurretBaseDefinition), null)]
  public class MyLargeTurretBaseDefinition : MyWeaponBlockDefinition
  {
    public string OverlayTexture;
    public bool AiEnabled;
    public int MinElevationDegrees;
    public int MaxElevationDegrees;
    public int MinAzimuthDegrees;
    public int MaxAzimuthDegrees;
    public bool IdleRotation;
    public float MaxRangeMeters;
    public float RotationSpeed;
    public float ElevationSpeed;
    public float MinFov;
    public float MaxFov;
    public int AmmoPullAmount;
    public new float InventoryFillFactorMin;
    public float InventoryFillFactorMax;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_LargeTurretBaseDefinition turretBaseDefinition = builder as MyObjectBuilder_LargeTurretBaseDefinition;
      this.OverlayTexture = turretBaseDefinition.OverlayTexture;
      this.AiEnabled = turretBaseDefinition.AiEnabled;
      this.MinElevationDegrees = turretBaseDefinition.MinElevationDegrees;
      this.MaxElevationDegrees = turretBaseDefinition.MaxElevationDegrees;
      this.MinAzimuthDegrees = turretBaseDefinition.MinAzimuthDegrees;
      this.MaxAzimuthDegrees = turretBaseDefinition.MaxAzimuthDegrees;
      this.IdleRotation = turretBaseDefinition.IdleRotation;
      this.MaxRangeMeters = turretBaseDefinition.MaxRangeMeters;
      this.RotationSpeed = turretBaseDefinition.RotationSpeed;
      this.ElevationSpeed = turretBaseDefinition.ElevationSpeed;
      this.MinFov = turretBaseDefinition.MinFov;
      this.MaxFov = turretBaseDefinition.MaxFov;
      this.AmmoPullAmount = turretBaseDefinition.AmmoPullAmountPerTick;
      this.InventoryFillFactorMin = turretBaseDefinition.InventoryFillFactorMin;
      this.InventoryFillFactorMax = turretBaseDefinition.InventoryFillFactorMax;
    }

    private class Sandbox_Definitions_MyLargeTurretBaseDefinition\u003C\u003EActor : IActivator, IActivator<MyLargeTurretBaseDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyLargeTurretBaseDefinition();

      MyLargeTurretBaseDefinition IActivator<MyLargeTurretBaseDefinition>.CreateInstance() => new MyLargeTurretBaseDefinition();
    }
  }
}
