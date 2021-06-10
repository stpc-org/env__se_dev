// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.MyGameLogicComponent
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Xml.Serialization;
using VRage.Game.Entity;
using VRage.Game.Entity.EntityComponents.Interfaces;
using VRage.ModAPI;
using VRage.ObjectBuilders;

namespace VRage.Game.Components
{
  public abstract class MyGameLogicComponent : MyEntityComponentBase, IMyGameLogicComponent
  {
    private MyEntityUpdateEnum m_needsUpdate;
    private bool m_entityUpdate;

    public bool EntityUpdate
    {
      get => this.m_entityUpdate;
      set => this.m_entityUpdate = value;
    }

    void IMyGameLogicComponent.UpdateOnceBeforeFrame(bool entityUpdate)
    {
      if (entityUpdate != this.m_entityUpdate)
        return;
      this.UpdateOnceBeforeFrame();
    }

    void IMyGameLogicComponent.UpdateBeforeSimulation(bool entityUpdate)
    {
      if (entityUpdate != this.m_entityUpdate)
        return;
      this.UpdateBeforeSimulation();
    }

    void IMyGameLogicComponent.UpdateBeforeSimulation10(bool entityUpdate)
    {
      if (entityUpdate != this.m_entityUpdate)
        return;
      this.UpdateBeforeSimulation10();
    }

    void IMyGameLogicComponent.UpdateBeforeSimulation100(bool entityUpdate)
    {
      if (entityUpdate != this.m_entityUpdate)
        return;
      this.UpdateBeforeSimulation100();
    }

    void IMyGameLogicComponent.UpdateAfterSimulation(bool entityUpdate)
    {
      if (entityUpdate != this.m_entityUpdate)
        return;
      this.UpdateAfterSimulation();
    }

    void IMyGameLogicComponent.UpdateAfterSimulation10(bool entityUpdate)
    {
      if (entityUpdate != this.m_entityUpdate)
        return;
      this.UpdateAfterSimulation10();
    }

    void IMyGameLogicComponent.UpdateAfterSimulation100(bool entityUpdate)
    {
      if (entityUpdate != this.m_entityUpdate)
        return;
      this.UpdateAfterSimulation100();
    }

    void IMyGameLogicComponent.Close()
    {
      MyGameLogic.UnregisterForUpdate(this);
      this.Close();
    }

    void IMyGameLogicComponent.RegisterForUpdate() => MyGameLogic.RegisterForUpdate(this);

    void IMyGameLogicComponent.UnregisterForUpdate() => MyGameLogic.UnregisterForUpdate(this);

    public MyEntityUpdateEnum NeedsUpdate
    {
      get
      {
        if (!this.m_entityUpdate)
          return this.m_needsUpdate;
        MyEntityUpdateEnum entityUpdateEnum = MyEntityUpdateEnum.NONE;
        if ((this.Entity.Flags & EntityFlags.NeedsUpdate) != (EntityFlags) 0)
          entityUpdateEnum |= MyEntityUpdateEnum.EACH_FRAME;
        if ((this.Entity.Flags & EntityFlags.NeedsUpdate10) != (EntityFlags) 0)
          entityUpdateEnum |= MyEntityUpdateEnum.EACH_10TH_FRAME;
        if ((this.Entity.Flags & EntityFlags.NeedsUpdate100) != (EntityFlags) 0)
          entityUpdateEnum |= MyEntityUpdateEnum.EACH_100TH_FRAME;
        if ((this.Entity.Flags & EntityFlags.NeedsUpdateBeforeNextFrame) != (EntityFlags) 0)
          entityUpdateEnum |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
        return entityUpdateEnum;
      }
      set
      {
        if (value == this.NeedsUpdate)
          return;
        if (this.m_entityUpdate)
        {
          if (this.Entity.InScene)
            MyAPIGatewayShortcuts.UnregisterEntityUpdate(this.Entity, false);
          this.Entity.Flags &= ~EntityFlags.NeedsUpdateBeforeNextFrame;
          this.Entity.Flags &= ~EntityFlags.NeedsUpdate;
          this.Entity.Flags &= ~EntityFlags.NeedsUpdate10;
          this.Entity.Flags &= ~EntityFlags.NeedsUpdate100;
          if ((value & MyEntityUpdateEnum.BEFORE_NEXT_FRAME) != MyEntityUpdateEnum.NONE)
            this.Entity.Flags |= EntityFlags.NeedsUpdateBeforeNextFrame;
          if ((value & MyEntityUpdateEnum.EACH_FRAME) != MyEntityUpdateEnum.NONE)
            this.Entity.Flags |= EntityFlags.NeedsUpdate;
          if ((value & MyEntityUpdateEnum.EACH_10TH_FRAME) != MyEntityUpdateEnum.NONE)
            this.Entity.Flags |= EntityFlags.NeedsUpdate10;
          if ((value & MyEntityUpdateEnum.EACH_100TH_FRAME) != MyEntityUpdateEnum.NONE)
            this.Entity.Flags |= EntityFlags.NeedsUpdate100;
          if (!this.Entity.InScene)
            return;
          MyAPIGatewayShortcuts.RegisterEntityUpdate(this.Entity);
        }
        else
        {
          if (this.Entity.InScene)
            MyGameLogic.ChangeUpdate(this, value);
          this.m_needsUpdate = value;
        }
      }
    }

    [XmlIgnore]
    public bool Closed { get; protected set; }

    [XmlIgnore]
    public bool MarkedForClose { get; protected set; }

    public virtual void UpdateOnceBeforeFrame()
    {
    }

    public virtual void UpdateBeforeSimulation()
    {
    }

    public virtual void UpdateBeforeSimulation10()
    {
    }

    public virtual void UpdateBeforeSimulation100()
    {
    }

    public virtual void UpdateAfterSimulation()
    {
    }

    public virtual void UpdateAfterSimulation10()
    {
    }

    public virtual void UpdateAfterSimulation100()
    {
    }

    public virtual void UpdatingStopped()
    {
    }

    public virtual void Init(MyObjectBuilder_EntityBase objectBuilder)
    {
    }

    public virtual MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false) => (MyObjectBuilder_EntityBase) null;

    public virtual void MarkForClose()
    {
    }

    public virtual void Close()
    {
    }

    public override string ComponentTypeDebugString => "Game Logic";
  }
}
