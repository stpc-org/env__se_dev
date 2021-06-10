// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyAmmoMagazineDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_AmmoMagazineDefinition), null)]
  public class MyAmmoMagazineDefinition : MyPhysicalItemDefinition
  {
    public int Capacity;
    public MyAmmoCategoryEnum Category;
    public MyDefinitionId AmmoDefinitionId;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_AmmoMagazineDefinition magazineDefinition = builder as MyObjectBuilder_AmmoMagazineDefinition;
      this.Capacity = magazineDefinition.Capacity;
      this.Category = magazineDefinition.Category;
      if (magazineDefinition.AmmoDefinitionId != null)
        this.AmmoDefinitionId = new MyDefinitionId(magazineDefinition.AmmoDefinitionId.Type, magazineDefinition.AmmoDefinitionId.Subtype);
      else
        this.AmmoDefinitionId = this.GetAmmoDefinitionIdFromCategory(this.Category);
    }

    private MyDefinitionId GetAmmoDefinitionIdFromCategory(MyAmmoCategoryEnum category)
    {
      switch (category)
      {
        case MyAmmoCategoryEnum.SmallCaliber:
          return new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_AmmoDefinition), "SmallCaliber");
        case MyAmmoCategoryEnum.LargeCaliber:
          return new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_AmmoDefinition), "LargeCaliber");
        case MyAmmoCategoryEnum.Missile:
          return new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_AmmoDefinition), "Missile");
        default:
          throw new NotImplementedException();
      }
    }

    private class Sandbox_Definitions_MyAmmoMagazineDefinition\u003C\u003EActor : IActivator, IActivator<MyAmmoMagazineDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyAmmoMagazineDefinition();

      MyAmmoMagazineDefinition IActivator<MyAmmoMagazineDefinition>.CreateInstance() => new MyAmmoMagazineDefinition();
    }
  }
}
