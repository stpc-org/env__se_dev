// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyModifiableEntity
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Entities
{
  [MyEntityType(typeof (MyObjectBuilder_ModifiableEntity), true)]
  public class MyModifiableEntity : MyEntity, IMyEventProxy, IMyEventOwner
  {
    private List<MyDefinitionId> m_assetModifiers;
    private bool m_assetModifiersDirty;

    public override void Init(MyObjectBuilder_EntityBase objectBuilder)
    {
      base.Init(objectBuilder);
      if (!(objectBuilder is MyObjectBuilder_ModifiableEntity modifiableEntity))
        return;
      this.m_assetModifiersDirty = false;
      if (modifiableEntity.AssetModifiers == null || modifiableEntity.AssetModifiers.Count <= 0)
        return;
      this.m_assetModifiersDirty = true;
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      this.m_assetModifiers = new List<MyDefinitionId>();
      foreach (SerializableDefinitionId assetModifier in modifiableEntity.AssetModifiers)
        this.m_assetModifiers.Add((MyDefinitionId) assetModifier);
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      if (!this.m_assetModifiersDirty)
        return;
      MySession.Static.GetComponent<MySessionComponentAssetModifiers>();
      foreach (MyDefinitionId assetModifier in this.m_assetModifiers)
        ;
      this.m_assetModifiersDirty = false;
    }

    public void AddAssetModifier(MyDefinitionId id) => MyMultiplayer.RaiseEvent<MyModifiableEntity, SerializableDefinitionId>(this, (Func<MyModifiableEntity, Action<SerializableDefinitionId>>) (x => new Action<SerializableDefinitionId>(x.AddAssetModifierSync)), (SerializableDefinitionId) id);

    [Event(null, 65)]
    [Reliable]
    [Broadcast]
    [Server]
    private void AddAssetModifierSync(SerializableDefinitionId id)
    {
      if (this.m_assetModifiers == null)
        this.m_assetModifiers = new List<MyDefinitionId>();
      this.m_assetModifiers.Add((MyDefinitionId) id);
      MySession.Static.GetComponent<MySessionComponentAssetModifiers>();
    }

    public void RemoveAssetModifier(MyDefinitionId id)
    {
      if (this.m_assetModifiers == null)
        return;
      this.m_assetModifiers.Remove(id);
    }

    public List<MyDefinitionId> GetAssetModifiers() => this.m_assetModifiers;

    public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
    {
      MyObjectBuilder_ModifiableEntity objectBuilder = (MyObjectBuilder_ModifiableEntity) base.GetObjectBuilder(copy);
      if (this.m_assetModifiers != null && this.m_assetModifiers.Count > 0)
      {
        objectBuilder.AssetModifiers = new List<SerializableDefinitionId>();
        foreach (MyDefinitionId assetModifier in this.m_assetModifiers)
          objectBuilder.AssetModifiers.Add((SerializableDefinitionId) assetModifier);
      }
      return (MyObjectBuilder_EntityBase) objectBuilder;
    }

    protected sealed class AddAssetModifierSync\u003C\u003EVRage_ObjectBuilders_SerializableDefinitionId : ICallSite<MyModifiableEntity, SerializableDefinitionId, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyModifiableEntity @this,
        in SerializableDefinitionId id,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.AddAssetModifierSync(id);
      }
    }

    private class Sandbox_Game_Entities_MyModifiableEntity\u003C\u003EActor : IActivator, IActivator<MyModifiableEntity>
    {
      object IActivator.CreateInstance() => (object) new MyModifiableEntity();

      MyModifiableEntity IActivator<MyModifiableEntity>.CreateInstance() => new MyModifiableEntity();
    }
  }
}
