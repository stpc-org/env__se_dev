// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyHumanoidBotDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_HumanoidBotDefinition), null)]
  public class MyHumanoidBotDefinition : MyAgentDefinition
  {
    public MyDefinitionId StartingWeaponDefinitionId;
    public List<MyDefinitionId> InventoryItems;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_HumanoidBotDefinition humanoidBotDefinition = builder as MyObjectBuilder_HumanoidBotDefinition;
      if (humanoidBotDefinition.StartingItem != null && !string.IsNullOrWhiteSpace(humanoidBotDefinition.StartingItem.Subtype))
        this.StartingWeaponDefinitionId = new MyDefinitionId(humanoidBotDefinition.StartingItem.Type, humanoidBotDefinition.StartingItem.Subtype);
      this.InventoryItems = new List<MyDefinitionId>();
      if (humanoidBotDefinition.InventoryItems == null)
        return;
      foreach (MyObjectBuilder_HumanoidBotDefinition.Item inventoryItem in humanoidBotDefinition.InventoryItems)
        this.InventoryItems.Add(new MyDefinitionId(inventoryItem.Type, inventoryItem.Subtype));
    }

    public override void AddItems(MyCharacter character)
    {
      base.AddItems(character);
      MyObjectBuilder_PhysicalGunObject newObject1 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_PhysicalGunObject>(this.StartingWeaponDefinitionId.SubtypeName);
      if (character.WeaponTakesBuilderFromInventory(new MyDefinitionId?(this.StartingWeaponDefinitionId)))
        MyEntityExtensions.GetInventory(character).AddItems((MyFixedPoint) 1, (MyObjectBuilder_Base) newObject1);
      foreach (MyDefinitionId inventoryItem in this.InventoryItems)
      {
        MyObjectBuilder_PhysicalGunObject newObject2 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_PhysicalGunObject>(inventoryItem.SubtypeName);
        MyEntityExtensions.GetInventory(character).AddItems((MyFixedPoint) 1, (MyObjectBuilder_Base) newObject2);
      }
      character.SwitchToWeapon(this.StartingWeaponDefinitionId);
      MyWeaponDefinition definition;
      if (!MyDefinitionManager.Static.TryGetWeaponDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_WeaponDefinition), this.StartingWeaponDefinitionId.SubtypeName), out definition) || !definition.HasAmmoMagazines())
        return;
      MyObjectBuilder_AmmoMagazine newObject3 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_AmmoMagazine>(definition.AmmoMagazinesId[0].SubtypeName);
      MyEntityExtensions.GetInventory(character).AddItems((MyFixedPoint) 3, (MyObjectBuilder_Base) newObject3);
    }

    private class Sandbox_Definitions_MyHumanoidBotDefinition\u003C\u003EActor : IActivator, IActivator<MyHumanoidBotDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyHumanoidBotDefinition();

      MyHumanoidBotDefinition IActivator<MyHumanoidBotDefinition>.CreateInstance() => new MyHumanoidBotDefinition();
    }
  }
}
