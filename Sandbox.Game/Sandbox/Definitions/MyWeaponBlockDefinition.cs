// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyWeaponBlockDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_WeaponBlockDefinition), null)]
  public class MyWeaponBlockDefinition : MyCubeBlockDefinition
  {
    public MyDefinitionId WeaponDefinitionId;
    public MyStringHash ResourceSinkGroup;
    public float InventoryMaxVolume;
    public float InventoryFillFactorMin;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_WeaponBlockDefinition weaponBlockDefinition = builder as MyObjectBuilder_WeaponBlockDefinition;
      this.WeaponDefinitionId = new MyDefinitionId(weaponBlockDefinition.WeaponDefinitionId.Type, weaponBlockDefinition.WeaponDefinitionId.Subtype);
      this.ResourceSinkGroup = MyStringHash.GetOrCompute(weaponBlockDefinition.ResourceSinkGroup);
      this.InventoryMaxVolume = weaponBlockDefinition.InventoryMaxVolume;
      this.InventoryFillFactorMin = weaponBlockDefinition.InventoryFillFactorMin;
    }

    private class Sandbox_Definitions_MyWeaponBlockDefinition\u003C\u003EActor : IActivator, IActivator<MyWeaponBlockDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyWeaponBlockDefinition();

      MyWeaponBlockDefinition IActivator<MyWeaponBlockDefinition>.CreateInstance() => new MyWeaponBlockDefinition();
    }
  }
}
