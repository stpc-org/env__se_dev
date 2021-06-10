// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.MyAmmoMagazine
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Entity;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Weapons
{
  [MyEntityType(typeof (MyObjectBuilder_AmmoMagazine), true)]
  public class MyAmmoMagazine : MyBaseInventoryItemEntity
  {
    public override void Init(MyObjectBuilder_EntityBase objectBuilder) => base.Init(objectBuilder);

    private class Sandbox_Game_Weapons_MyAmmoMagazine\u003C\u003EActor : IActivator, IActivator<MyAmmoMagazine>
    {
      object IActivator.CreateInstance() => (object) new MyAmmoMagazine();

      MyAmmoMagazine IActivator<MyAmmoMagazine>.CreateInstance() => new MyAmmoMagazine();
    }
  }
}
