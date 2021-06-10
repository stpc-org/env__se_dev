// Decompiled with JetBrains decompiler
// Type: VRage.Game.Entity.EntityComponents.MyEntityOwnershipComponent
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders.Components;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;

namespace VRage.Game.Entity.EntityComponents
{
  [MyComponentType(typeof (MyEntityOwnershipComponent))]
  [MyComponentBuilder(typeof (MyObjectBuilder_EntityOwnershipComponent), true)]
  public class MyEntityOwnershipComponent : MyEntityComponentBase
  {
    private long m_ownerId;
    private MyOwnershipShareModeEnum m_shareMode = MyOwnershipShareModeEnum.All;
    public Action<long, long> OwnerChanged;
    public Action<MyOwnershipShareModeEnum> ShareModeChanged;

    public long OwnerId
    {
      get => this.m_ownerId;
      set
      {
        if (this.m_ownerId != value && this.OwnerChanged != null)
          this.OwnerChanged(this.m_ownerId, value);
        this.m_ownerId = value;
      }
    }

    public MyOwnershipShareModeEnum ShareMode
    {
      get => this.m_shareMode;
      set
      {
        if (this.m_shareMode != value && this.ShareModeChanged != null)
          this.ShareModeChanged(value);
        this.m_shareMode = value;
      }
    }

    public override bool IsSerialized() => true;

    public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
    {
      MyObjectBuilder_EntityOwnershipComponent ownershipComponent = base.Serialize() as MyObjectBuilder_EntityOwnershipComponent;
      ownershipComponent.OwnerId = this.m_ownerId;
      ownershipComponent.ShareMode = this.m_shareMode;
      return (MyObjectBuilder_ComponentBase) ownershipComponent;
    }

    public override void Deserialize(MyObjectBuilder_ComponentBase builder)
    {
      base.Deserialize(builder);
      MyObjectBuilder_EntityOwnershipComponent ownershipComponent = builder as MyObjectBuilder_EntityOwnershipComponent;
      this.m_ownerId = ownershipComponent.OwnerId;
      this.m_shareMode = ownershipComponent.ShareMode;
    }

    public override string ComponentTypeDebugString => this.GetType().Name;

    private class VRage_Game_Entity_EntityComponents_MyEntityOwnershipComponent\u003C\u003EActor : IActivator, IActivator<MyEntityOwnershipComponent>
    {
      object IActivator.CreateInstance() => (object) new MyEntityOwnershipComponent();

      MyEntityOwnershipComponent IActivator<MyEntityOwnershipComponent>.CreateInstance() => new MyEntityOwnershipComponent();
    }
  }
}
