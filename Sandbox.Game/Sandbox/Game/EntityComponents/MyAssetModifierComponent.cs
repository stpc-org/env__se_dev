// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.MyAssetModifierComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ProtoBuf;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Game.Entities;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.GameServices;
using VRage.Network;
using VRage.ObjectBuilders;
using VRageRender;

namespace Sandbox.Game.EntityComponents
{
  [StaticEventOwner]
  [MyComponentBuilder(typeof (MyObjectBuilder_AssetModifierComponent), true)]
  public class MyAssetModifierComponent : MyEntityComponentBase
  {
    private List<MyDefinitionId> m_assetModifiers;
    private MySessionComponentAssetModifiers m_sessionComponent;

    [ProtoMember(1, IsRequired = false)]
    public List<MyDefinitionId> AssetModifiers => this.m_assetModifiers;

    public MyAssetModifierComponent() => this.InitSessionComponent();

    private void InitSessionComponent()
    {
      if (this.m_sessionComponent != null || MySession.Static == null)
        return;
      this.m_sessionComponent = MySession.Static.GetComponent<MySessionComponentAssetModifiers>();
    }

    public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
    {
      MyObjectBuilder_AssetModifierComponent modifierComponent = base.Serialize(copy) as MyObjectBuilder_AssetModifierComponent;
      if (this.m_assetModifiers != null && this.m_assetModifiers.Count > 0)
      {
        modifierComponent.AssetModifiers = new List<SerializableDefinitionId>();
        foreach (MyDefinitionId assetModifier in this.m_assetModifiers)
          modifierComponent.AssetModifiers.Add((SerializableDefinitionId) assetModifier);
      }
      return (MyObjectBuilder_ComponentBase) modifierComponent;
    }

    public override void Deserialize(MyObjectBuilder_ComponentBase builder)
    {
      base.Deserialize(builder);
      MyObjectBuilder_AssetModifierComponent modifierComponent = builder as MyObjectBuilder_AssetModifierComponent;
      if (modifierComponent.AssetModifiers == null || modifierComponent.AssetModifiers.Count <= 0 || this.m_assetModifiers != null && this.m_assetModifiers.Count != 0)
        return;
      this.m_assetModifiers = new List<MyDefinitionId>();
      foreach (SerializableDefinitionId assetModifier in modifierComponent.AssetModifiers)
        this.m_assetModifiers.Add((MyDefinitionId) assetModifier);
      this.InitSessionComponent();
      this.m_sessionComponent.RegisterComponentForLazyUpdate(this);
    }

    public bool LazyUpdate()
    {
      if (this.m_assetModifiers != null && this.Entity != null)
      {
        MyEntity entityById = MyEntities.GetEntityById(this.Entity.EntityId);
        if (entityById == null || !entityById.InScene)
          return false;
        foreach (MyDefinitionId assetModifier in this.m_assetModifiers)
        {
          if (MyGameService.IsActive && MyGameService.GetInventoryItemDefinition(assetModifier.SubtypeName) == null)
            return false;
        }
      }
      return true;
    }

    public void ResetSlot(MyGameInventoryItemSlot slot) => MyMultiplayer.RaiseStaticEvent<long, MyGameInventoryItemSlot>((Func<IMyEventOwner, Action<long, MyGameInventoryItemSlot>>) (x => new Action<long, MyGameInventoryItemSlot>(MyAssetModifierComponent.ResetAssetModifierSync)), this.Entity.EntityId, slot);

    [Event(null, 120)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void ResetAssetModifierSync(long entityId, MyGameInventoryItemSlot slot)
    {
      MyEntity entityById = MyEntities.GetEntityById(entityId);
      MyAssetModifierComponent component;
      if (entityById == null || !entityById.Components.TryGet<MyAssetModifierComponent>(out component))
        return;
      component.RemoveModifiers(entityById, slot);
    }

    private void RemoveModifiers(MyEntity entity, MyGameInventoryItemSlot slot)
    {
      if (this.m_assetModifiers == null)
        return;
      for (int index = 0; index < this.m_assetModifiers.Count; ++index)
      {
        MyDefinitionId assetModifier = this.m_assetModifiers[index];
        MyGameInventoryItemDefinition inventoryItemDefinition = MyGameService.GetInventoryItemDefinition(assetModifier.SubtypeName);
        if (inventoryItemDefinition == null || inventoryItemDefinition.ItemSlot == slot)
        {
          this.m_assetModifiers.Remove(assetModifier);
          int num = index - 1;
          if (entity.Render == null)
            break;
          switch (slot)
          {
            case MyGameInventoryItemSlot.Face:
              MyAssetModifierComponent.SetDefaultTextures(entity, "Astronaut_head");
              return;
            case MyGameInventoryItemSlot.Helmet:
              MyAssetModifierComponent.SetDefaultTextures(entity, "Head");
              MyAssetModifierComponent.SetDefaultTextures(entity, "Astronaut_head");
              MyAssetModifierComponent.SetDefaultTextures(entity, "Spacesuit_hood");
              return;
            case MyGameInventoryItemSlot.Gloves:
              MyAssetModifierComponent.SetDefaultTextures(entity, "LeftGlove");
              MyAssetModifierComponent.SetDefaultTextures(entity, "RightGlove");
              return;
            case MyGameInventoryItemSlot.Boots:
              MyAssetModifierComponent.SetDefaultTextures(entity, "Boots");
              return;
            case MyGameInventoryItemSlot.Suit:
              MyAssetModifierComponent.SetDefaultTextures(entity, "Arms");
              MyAssetModifierComponent.SetDefaultTextures(entity, "RightArm");
              MyAssetModifierComponent.SetDefaultTextures(entity, "Gear");
              MyAssetModifierComponent.SetDefaultTextures(entity, "Cloth");
              MyAssetModifierComponent.SetDefaultTextures(entity, "Emissive");
              MyAssetModifierComponent.SetDefaultTextures(entity, "Backpack");
              return;
            case MyGameInventoryItemSlot.Rifle:
              MyAssetModifierComponent.ResetRifle(entity);
              return;
            case MyGameInventoryItemSlot.Welder:
              MyAssetModifierComponent.ResetWelder(entity);
              return;
            case MyGameInventoryItemSlot.Grinder:
              MyAssetModifierComponent.ResetGrinder(entity);
              return;
            case MyGameInventoryItemSlot.Drill:
              MyAssetModifierComponent.ResetDrill(entity);
              return;
            default:
              return;
          }
        }
      }
    }

    public static void ResetDrill(MyEntity entity) => MyRenderProxy.ChangeMaterialTexture(entity.Render.RenderObjectIDs[0], "HandDrill");

    public static void ResetGrinder(MyEntity entity) => MyRenderProxy.ChangeMaterialTexture(entity.Render.RenderObjectIDs[0], "AngleGrinder");

    public static void ResetWelder(MyEntity entity) => MyRenderProxy.ChangeMaterialTexture(entity.Render.RenderObjectIDs[0], "Welder");

    public static void ResetRifle(MyEntity entity) => MyRenderProxy.ChangeMaterialTexture(entity.Render.RenderObjectIDs[0], "AutomaticRifle");

    public static void SetDefaultTextures(MyEntity entity, string materialName) => MyRenderProxy.ChangeMaterialTexture(entity.Render.RenderObjectIDs[0], materialName);

    public bool TryAddAssetModifier(byte[] checkData)
    {
      if (this.Entity == null || this.Entity.Closed || !this.Entity.InScene)
        return false;
      MyMultiplayer.RaiseStaticEvent<long, byte[], bool>((Func<IMyEventOwner, Action<long, byte[], bool>>) (x => new Action<long, byte[], bool>(MyAssetModifierComponent.ApplyAssetModifierSync)), this.Entity.EntityId, checkData, true);
      return true;
    }

    public bool TryAddAssetModifier(string assetModifierId)
    {
      if (this.Entity == null || this.Entity.Closed || !this.Entity.InScene)
        return false;
      MyMultiplayer.RaiseStaticEvent<long, string, bool>((Func<IMyEventOwner, Action<long, string, bool>>) (x => new Action<long, string, bool>(MyAssetModifierComponent.ApplyAssetModifierSync)), this.Entity.EntityId, assetModifierId, true);
      return true;
    }

    [Event(null, 245)]
    [Reliable]
    [Server]
    [Broadcast]
    public static void ApplyAssetModifierSync(long entityId, byte[] checkData, bool addToList)
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated || !MyGameService.IsActive || (checkData == null || checkData == MySessionComponentAssetModifiers.INVALID_CHECK_DATA))
        return;
      bool checkResult = false;
      List<MyGameInventoryItem> items = MyGameService.CheckItemData(checkData, out checkResult);
      if (!checkResult)
        return;
      MyAssetModifierComponent.ApplyAssetModifier(entityId, items, addToList);
    }

    [Event(null, 263)]
    [Reliable]
    [Server]
    [Broadcast]
    public static void ApplyAssetModifierSync(
      long entityId,
      string assetModifierId,
      bool addToList)
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated || !MyGameService.IsActive)
        return;
      MyAssetModifierComponent.ApplyAssetModifier(entityId, new List<MyGameInventoryItem>()
      {
        MyGameService.InventoryItems.First<MyGameInventoryItem>((Func<MyGameInventoryItem, bool>) (x => x.ItemDefinition.AssetModifierId == assetModifierId))
      }, addToList);
    }

    private static void ApplyAssetModifier(
      long entityId,
      List<MyGameInventoryItem> items,
      bool addToList)
    {
      foreach (MyGameInventoryItem gameInventoryItem in items)
      {
        if (MyGameService.GetInventoryItemDefinition(gameInventoryItem.ItemDefinition.AssetModifierId) == null)
          break;
        MyEntity entityById = MyEntities.GetEntityById(entityId);
        MyDefinitionManager.MyAssetModifiers definitionForRender = MyDefinitionManager.Static.GetAssetModifierDefinitionForRender(gameInventoryItem.ItemDefinition.AssetModifierId);
        if (entityById != null && definitionForRender.SkinTextureChanges != null)
        {
          MyAssetModifierComponent component;
          if (addToList && entityById.Components.TryGet<MyAssetModifierComponent>(out component))
          {
            MyDefinitionId id = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_AssetModifierDefinition), gameInventoryItem.ItemDefinition.AssetModifierId);
            component.AddAssetModifier(id, gameInventoryItem.ItemDefinition.ItemSlot);
          }
          if (entityById.Render != null && entityById.Render.RenderObjectIDs[0] != uint.MaxValue)
            MyRenderProxy.ChangeMaterialTexture(entityById.Render.RenderObjectIDs[0], definitionForRender.SkinTextureChanges);
        }
      }
    }

    private void AddAssetModifier(MyDefinitionId id, MyGameInventoryItemSlot itemSlot)
    {
      if (this.m_assetModifiers == null)
        this.m_assetModifiers = new List<MyDefinitionId>();
      if (!Sandbox.Engine.Platform.Game.IsDedicated)
      {
        for (int index = 0; index < this.m_assetModifiers.Count; ++index)
        {
          MyDefinitionId assetModifier = this.m_assetModifiers[index];
          MyGameInventoryItemDefinition inventoryItemDefinition = MyGameService.GetInventoryItemDefinition(assetModifier.SubtypeName);
          if (inventoryItemDefinition == null)
          {
            this.m_assetModifiers.Remove(assetModifier);
            --index;
          }
          else if (inventoryItemDefinition.ItemSlot == itemSlot)
          {
            this.m_assetModifiers.Remove(assetModifier);
            --index;
          }
        }
      }
      this.m_assetModifiers.Add(id);
    }

    public override void OnRemovedFromScene()
    {
      base.OnRemovedFromScene();
      this.m_sessionComponent = (MySessionComponentAssetModifiers) null;
    }

    public override bool IsSerialized() => true;

    public override string ComponentTypeDebugString => "Asset Modifier Component";

    protected sealed class ResetAssetModifierSync\u003C\u003ESystem_Int64\u0023VRage_GameServices_MyGameInventoryItemSlot : ICallSite<IMyEventOwner, long, MyGameInventoryItemSlot, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in MyGameInventoryItemSlot slot,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyAssetModifierComponent.ResetAssetModifierSync(entityId, slot);
      }
    }

    protected sealed class ApplyAssetModifierSync\u003C\u003ESystem_Int64\u0023System_Byte\u003C\u0023\u003E\u0023System_Boolean : ICallSite<IMyEventOwner, long, byte[], bool, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in byte[] checkData,
        in bool addToList,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyAssetModifierComponent.ApplyAssetModifierSync(entityId, checkData, addToList);
      }
    }

    protected sealed class ApplyAssetModifierSync\u003C\u003ESystem_Int64\u0023System_String\u0023System_Boolean : ICallSite<IMyEventOwner, long, string, bool, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in string assetModifierId,
        in bool addToList,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyAssetModifierComponent.ApplyAssetModifierSync(entityId, assetModifierId, addToList);
      }
    }

    private class Sandbox_Game_EntityComponents_MyAssetModifierComponent\u003C\u003EActor : IActivator, IActivator<MyAssetModifierComponent>
    {
      object IActivator.CreateInstance() => (object) new MyAssetModifierComponent();

      MyAssetModifierComponent IActivator<MyAssetModifierComponent>.CreateInstance() => new MyAssetModifierComponent();
    }
  }
}
