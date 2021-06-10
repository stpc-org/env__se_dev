// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.MySessionComponentBase
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Reflection;
using VRage.Game.Components.Interfaces;
using VRage.Game.Components.Session;
using VRage.Game.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.Components
{
  public abstract class MySessionComponentBase : IMyUserInputComponent
  {
    public readonly string DebugName;
    public readonly int Priority;
    public readonly Type ComponentType;
    public IMySession Session;
    private bool m_initialized;

    public MyUpdateOrder UpdateOrder { get; protected set; }

    public MyObjectBuilderType ObjectBuilderType { get; private set; }

    public IMyModContext ModContext { get; set; }

    public virtual bool UpdatedBeforeInit() => false;

    public bool Loaded { get; private set; }

    public bool Initialized => this.m_initialized;

    public bool UpdateOnPause { get; set; }

    public bool IsServerOnly { get; private set; }

    public MySessionComponentBase()
    {
      Type type = this.GetType();
      MySessionComponentDescriptor customAttribute = (MySessionComponentDescriptor) Attribute.GetCustomAttribute((MemberInfo) type, typeof (MySessionComponentDescriptor), false);
      this.DebugName = type.Name;
      this.Priority = customAttribute.Priority;
      this.UpdateOrder = customAttribute.UpdateOrder;
      this.ObjectBuilderType = customAttribute.ObjectBuilderType;
      this.ComponentType = customAttribute.ComponentType;
      this.IsServerOnly = customAttribute.IsServerOnly;
      if (this.ObjectBuilderType != MyObjectBuilderType.Invalid)
        MySessionComponentMapping.Map(this.GetType(), this.ObjectBuilderType);
      if (this.ComponentType == (Type) null)
      {
        this.ComponentType = this.GetType();
      }
      else
      {
        if (!(this.ComponentType == this.GetType()) && !this.ComponentType.IsSubclassOf(this.GetType()))
          return;
        MyLog.Default.Error("Component {0} tries to register itself as a component it does not inherit from ({1}). Ignoring...", (object) this.GetType(), (object) this.ComponentType);
        this.ComponentType = this.GetType();
      }
    }

    public MyDefinitionId? Definition { get; set; }

    public void SetUpdateOrder(MyUpdateOrder order)
    {
      this.Session.SetComponentUpdateOrder(this, order);
      this.UpdateOrder = order;
    }

    public virtual Type[] Dependencies => Type.EmptyTypes;

    public virtual bool IsRequiredByGame => false;

    public virtual void InitFromDefinition(MySessionComponentDefinition definition)
    {
    }

    public virtual void Init(MyObjectBuilder_SessionComponent sessionComponent)
    {
      this.m_initialized = true;
      if (sessionComponent != null && sessionComponent.Definition.HasValue)
      {
        SerializableDefinitionId? definition = sessionComponent.Definition;
        this.Definition = definition.HasValue ? new MyDefinitionId?((MyDefinitionId) definition.GetValueOrDefault()) : new MyDefinitionId?();
      }
      if (!this.Definition.HasValue)
        return;
      MySessionComponentDefinition definition1 = MyDefinitionManagerBase.Static.GetDefinition<MySessionComponentDefinition>(this.Definition.Value);
      if (definition1 == null)
        MyLog.Default.Warning("Missing definition {0} : for session component {1}", (object) this.Definition, (object) this.GetType().Name);
      else
        this.InitFromDefinition(definition1);
    }

    public virtual MyObjectBuilder_SessionComponent GetObjectBuilder()
    {
      if (!(this.ObjectBuilderType != MyObjectBuilderType.Invalid))
        return (MyObjectBuilder_SessionComponent) null;
      MyObjectBuilder_SessionComponent instance = Activator.CreateInstance((Type) this.ObjectBuilderType) as MyObjectBuilder_SessionComponent;
      MyDefinitionId? definition = this.Definition;
      instance.Definition = definition.HasValue ? new SerializableDefinitionId?((SerializableDefinitionId) definition.GetValueOrDefault()) : new SerializableDefinitionId?();
      return instance;
    }

    public void AfterLoadData() => this.Loaded = true;

    public void UnloadDataConditional()
    {
      if (!this.Loaded)
        return;
      this.UnloadData();
      this.Loaded = false;
    }

    public virtual void LoadData()
    {
    }

    protected virtual void UnloadData()
    {
    }

    public virtual void SaveData()
    {
    }

    public virtual void BeforeStart()
    {
    }

    public virtual void UpdateBeforeSimulation()
    {
    }

    public virtual void Simulate()
    {
    }

    public virtual void UpdateAfterSimulation()
    {
    }

    public virtual void UpdatingStopped()
    {
    }

    public virtual void Draw()
    {
    }

    public virtual void HandleInput()
    {
    }

    public override string ToString() => this.DebugName;
  }
}
