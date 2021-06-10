// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Character.Components.MyInventorySpawnComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders;
using VRage.Network;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Entities.Character.Components
{
  public class MyInventorySpawnComponent : MyCharacterComponent
  {
    private MyInventory m_spawnInventory;
    private const string INVENTORY_USE_DUMMY_NAME = "inventory";

    public override string ComponentTypeDebugString => "Inventory Spawn Component";

    public override void OnCharacterDead()
    {
      if (!this.Character.IsDead || !this.Character.Definition.EnableSpawnInventoryAsContainer || !this.Character.Definition.InventorySpawnContainerId.HasValue)
        return;
      if (this.Character.Components.Has<MyInventoryBase>())
      {
        MyInventoryBase myInventoryBase = this.Character.Components.Get<MyInventoryBase>();
        switch (myInventoryBase)
        {
          case MyInventoryAggregate _:
            MyInventoryAggregate aggregate = myInventoryBase as MyInventoryAggregate;
            List<MyComponentBase> output = new List<MyComponentBase>();
            aggregate.GetComponentsFlattened(output);
            using (List<MyComponentBase>.Enumerator enumerator = output.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                MyComponentBase current = enumerator.Current;
                if (current is MyInventory myInventory && myInventory.GetItemsCount() > 0)
                {
                  if (MyDefinitionManager.Static.TryGetContainerDefinition(this.Character.Definition.InventorySpawnContainerId.Value, out MyContainerDefinition _))
                  {
                    aggregate.RemoveComponent((MyComponentBase) myInventory);
                    if (Sync.IsServer)
                    {
                      MyInventory inventory = new MyInventory();
                      inventory.Init(myInventory.GetObjectBuilder());
                      this.SpawnInventoryContainer(this.Character.Definition.InventorySpawnContainerId.Value, inventory);
                    }
                  }
                }
                else
                  aggregate.RemoveComponent(current);
              }
              break;
            }
          case MyInventory _:
            if (this.Character.Definition.SpawnInventoryOnBodyRemoval)
            {
              this.m_spawnInventory = myInventoryBase as MyInventory;
              this.SpawnBackpack((MyEntity) this.Character);
              break;
            }
            break;
        }
      }
      this.CloseComponent();
    }

    private void SpawnBackpack(MyEntity obj)
    {
      MyInventory myInventory = new MyInventory();
      myInventory.Init(this.m_spawnInventory.GetObjectBuilder());
      this.m_spawnInventory = myInventory;
      if (this.m_spawnInventory == null)
        return;
      MyContainerDefinition definition;
      if (!MyComponentContainerExtension.TryGetContainerDefinition(this.Character.Definition.InventorySpawnContainerId.Value.TypeId, this.Character.Definition.InventorySpawnContainerId.Value.SubtypeId, out definition))
      {
        MyDefinitionId myDefinitionId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_InventoryBagEntity), this.Character.Definition.InventorySpawnContainerId.Value.SubtypeId);
        MyComponentContainerExtension.TryGetContainerDefinition(myDefinitionId.TypeId, myDefinitionId.SubtypeId, out definition);
      }
      if (definition == null || !Sync.IsServer || MyFakes.USE_GPS_AS_FRIENDLY_SPAWN_LOCATIONS)
        return;
      long entityId = this.SpawnInventoryContainer(this.Character.Definition.InventorySpawnContainerId.Value, this.m_spawnInventory, false, this.Character.DeadPlayerIdentityId);
      MyGps gps = new MyGps()
      {
        ShowOnHud = true,
        Name = new StringBuilder().AppendStringBuilder(MyTexts.Get(MySpaceTexts.GPS_Body_Location_Name)).Append(" - ").AppendFormatedDateTime(DateTime.Now).ToString(),
        DisplayName = MyTexts.GetString(MySpaceTexts.GPS_Body_Location_Name),
        DiscardAt = new TimeSpan?(),
        Coords = this.Character.PositionComp.GetPosition(),
        Description = "",
        AlwaysVisible = true,
        GPSColor = new Color(117, 201, 241),
        IsContainerGPS = true
      };
      MySession.Static.Gpss.SendAddGps(this.Character.DeadPlayerIdentityId, ref gps, entityId, false);
    }

    private long SpawnInventoryContainer(
      MyDefinitionId bagDefinition,
      MyInventory inventory,
      bool spawnAboveCharacter = true,
      long ownerIdentityId = 0)
    {
      if (MySession.Static == null || !MySession.Static.Ready)
        return 0;
      MyEntity character = (MyEntity) this.Character;
      MatrixD worldMatrix = this.Character.WorldMatrix;
      if (spawnAboveCharacter)
        worldMatrix.Translation += worldMatrix.Up + worldMatrix.Forward;
      else
        worldMatrix.Translation = this.Character.PositionComp.WorldAABB.Center + worldMatrix.Backward * 0.400000005960464;
      MyContainerDefinition definition;
      if (!MyComponentContainerExtension.TryGetContainerDefinition(bagDefinition.TypeId, bagDefinition.SubtypeId, out definition))
        return 0;
      MyEntity definitionAndAdd = MyEntities.CreateFromComponentContainerDefinitionAndAdd(definition.Id, false);
      if (definitionAndAdd == null)
        return 0;
      if (definitionAndAdd is MyInventoryBagEntity inventoryBagEntity)
      {
        inventoryBagEntity.OwnerIdentityId = ownerIdentityId;
        MyTimerComponent component;
        if (inventoryBagEntity.Components.TryGet<MyTimerComponent>(out component))
          component.ChangeTimerTick((uint) ((double) MySession.Static.Settings.BackpackDespawnTimer * 3600.0));
      }
      definitionAndAdd.PositionComp.SetWorldMatrix(ref worldMatrix);
      definitionAndAdd.Physics.LinearVelocity = character.Physics.LinearVelocity;
      definitionAndAdd.Physics.AngularVelocity = character.Physics.AngularVelocity;
      definitionAndAdd.Render.EnableColorMaskHsv = true;
      definitionAndAdd.Render.ColorMaskHsv = this.Character.Render.ColorMaskHsv;
      inventory.RemoveEntityOnEmpty = true;
      definitionAndAdd.Components.Add<MyInventoryBase>((MyInventoryBase) inventory);
      return definitionAndAdd.EntityId;
    }

    public override void OnAddedToContainer() => base.OnAddedToContainer();

    private void CloseComponent()
    {
    }

    public override bool IsSerialized() => false;

    private class Sandbox_Game_Entities_Character_Components_MyInventorySpawnComponent\u003C\u003EActor : IActivator, IActivator<MyInventorySpawnComponent>
    {
      object IActivator.CreateInstance() => (object) new MyInventorySpawnComponent();

      MyInventorySpawnComponent IActivator<MyInventorySpawnComponent>.CreateInstance() => new MyInventorySpawnComponent();
    }
  }
}
