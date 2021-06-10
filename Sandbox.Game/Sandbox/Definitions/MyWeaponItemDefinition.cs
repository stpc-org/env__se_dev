// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyWeaponItemDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_WeaponItemDefinition), null)]
  public class MyWeaponItemDefinition : MyPhysicalItemDefinition
  {
    public MyDefinitionId WeaponDefinitionId;
    public bool ShowAmmoCount;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_WeaponItemDefinition weaponItemDefinition = builder as MyObjectBuilder_WeaponItemDefinition;
      this.WeaponDefinitionId = new MyDefinitionId(weaponItemDefinition.WeaponDefinitionId.Type, weaponItemDefinition.WeaponDefinitionId.Subtype);
      this.ShowAmmoCount = weaponItemDefinition.ShowAmmoCount;
    }

    private class Sandbox_Definitions_MyWeaponItemDefinition\u003C\u003EActor : IActivator, IActivator<MyWeaponItemDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyWeaponItemDefinition();

      MyWeaponItemDefinition IActivator<MyWeaponItemDefinition>.CreateInstance() => new MyWeaponItemDefinition();
    }
  }
}
