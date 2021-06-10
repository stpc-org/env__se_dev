// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MySessionCompatHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.World
{
  public class MySessionCompatHelper
  {
    public virtual void FixSessionComponentObjectBuilders(
      MyObjectBuilder_Checkpoint checkpoint,
      MyObjectBuilder_Sector sector)
    {
    }

    public virtual void FixSessionObjectBuilders(
      MyObjectBuilder_Checkpoint checkpoint,
      MyObjectBuilder_Sector sector)
    {
    }

    public virtual void AfterEntitiesLoad(int saveVersion)
    {
    }

    public virtual void CheckAndFixPrefab(MyObjectBuilder_Definitions prefab)
    {
    }

    protected MyObjectBuilder_EntityBase ConvertBuilderToEntityBase(
      MyObjectBuilder_EntityBase origEntity,
      string subTypeNamePrefix)
    {
      string str = !string.IsNullOrEmpty(origEntity.SubtypeName) ? origEntity.SubtypeName : (origEntity.EntityDefinitionId.HasValue ? origEntity.EntityDefinitionId.Value.SubtypeName : (string) null);
      if (str == null)
        return (MyObjectBuilder_EntityBase) null;
      string subtypeName = subTypeNamePrefix != null ? subTypeNamePrefix : str ?? "";
      MyObjectBuilder_EntityBase newObject = MyObjectBuilderSerializer.CreateNewObject((MyObjectBuilderType) typeof (MyObjectBuilder_EntityBase), subtypeName) as MyObjectBuilder_EntityBase;
      newObject.EntityId = origEntity.EntityId;
      newObject.PersistentFlags = origEntity.PersistentFlags;
      newObject.Name = origEntity.Name;
      newObject.PositionAndOrientation = origEntity.PositionAndOrientation;
      newObject.ComponentContainer = origEntity.ComponentContainer;
      if (newObject.ComponentContainer != null && newObject.ComponentContainer.Components.Count > 0)
      {
        foreach (MyObjectBuilder_ComponentContainer.ComponentData component in newObject.ComponentContainer.Components)
        {
          if (!string.IsNullOrEmpty(component.Component.SubtypeName) && component.Component.SubtypeName == str)
            component.Component.SubtypeName = subtypeName;
        }
      }
      return newObject;
    }

    private MyObjectBuilder_EntityBase ConvertInventoryBagToEntityBase(
      MyObjectBuilder_EntityBase oldBagBuilder,
      Vector3 linearVelocity,
      Vector3 angularVelocity)
    {
      MyObjectBuilder_EntityBase entityBase = this.ConvertBuilderToEntityBase(oldBagBuilder, (string) null);
      if (entityBase == null)
        return (MyObjectBuilder_EntityBase) null;
      if (entityBase.ComponentContainer == null)
        entityBase.ComponentContainer = MyObjectBuilderSerializer.CreateNewObject((MyObjectBuilderType) typeof (MyObjectBuilder_ComponentContainer), entityBase.SubtypeName) as MyObjectBuilder_ComponentContainer;
      foreach (MyObjectBuilder_ComponentContainer.ComponentData component in entityBase.ComponentContainer.Components)
      {
        if (component.Component is MyObjectBuilder_PhysicsComponentBase)
          return entityBase;
      }
      MyObjectBuilder_PhysicsComponentBase newObject = MyObjectBuilderSerializer.CreateNewObject((MyObjectBuilderType) typeof (MyObjectBuilder_PhysicsBodyComponent), entityBase.SubtypeName) as MyObjectBuilder_PhysicsComponentBase;
      entityBase.ComponentContainer.Components.Add(new MyObjectBuilder_ComponentContainer.ComponentData()
      {
        Component = (MyObjectBuilder_ComponentBase) newObject,
        TypeId = typeof (MyPhysicsComponentBase).Name
      });
      newObject.LinearVelocity = (SerializableVector3) linearVelocity;
      newObject.AngularVelocity = (SerializableVector3) angularVelocity;
      return entityBase;
    }

    protected MyObjectBuilder_EntityBase ConvertInventoryBagToEntityBase(
      MyObjectBuilder_EntityBase oldBuilder)
    {
      switch (oldBuilder)
      {
        case MyObjectBuilder_ReplicableEntity replicableEntity:
          return this.ConvertInventoryBagToEntityBase((MyObjectBuilder_EntityBase) replicableEntity, (Vector3) replicableEntity.LinearVelocity, (Vector3) replicableEntity.AngularVelocity);
        case MyObjectBuilder_InventoryBagEntity inventoryBagEntity:
          return this.ConvertInventoryBagToEntityBase((MyObjectBuilder_EntityBase) inventoryBagEntity, (Vector3) inventoryBagEntity.LinearVelocity, (Vector3) inventoryBagEntity.AngularVelocity);
        default:
          return (MyObjectBuilder_EntityBase) null;
      }
    }
  }
}
