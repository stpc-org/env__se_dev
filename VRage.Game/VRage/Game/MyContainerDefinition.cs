// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyContainerDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using VRage.Game.Definitions;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
  [MyDefinitionType(typeof (MyObjectBuilder_ContainerDefinition), null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyContainerDefinition : MyDefinitionBase
  {
    public List<MyContainerDefinition.DefaultComponent> DefaultComponents = new List<MyContainerDefinition.DefaultComponent>();
    public EntityFlags? Flags;

    public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
    {
      MyObjectBuilder_ContainerDefinition objectBuilder = (MyObjectBuilder_ContainerDefinition) base.GetObjectBuilder();
      objectBuilder.Flags = this.Flags;
      if (this.DefaultComponents != null && this.DefaultComponents.Count > 0)
      {
        objectBuilder.DefaultComponents = new MyObjectBuilder_ContainerDefinition.DefaultComponentBuilder[this.DefaultComponents.Count];
        int index = 0;
        foreach (MyContainerDefinition.DefaultComponent defaultComponent in this.DefaultComponents)
        {
          if (!defaultComponent.BuilderType.IsNull)
            objectBuilder.DefaultComponents[index].BuilderType = defaultComponent.BuilderType.ToString();
          if (defaultComponent.InstanceType != (Type) null)
            objectBuilder.DefaultComponents[index].InstanceType = defaultComponent.InstanceType.Name;
          if (defaultComponent.SubtypeId.HasValue)
            objectBuilder.DefaultComponents[index].SubtypeId = defaultComponent.SubtypeId.Value.ToString();
          objectBuilder.DefaultComponents[index].ForceCreate = defaultComponent.ForceCreate;
          ++index;
        }
      }
      return (MyObjectBuilder_DefinitionBase) objectBuilder;
    }

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_ContainerDefinition containerDefinition = builder as MyObjectBuilder_ContainerDefinition;
      this.Flags = containerDefinition.Flags;
      if (containerDefinition.DefaultComponents == null || containerDefinition.DefaultComponents.Length == 0)
        return;
      if (this.DefaultComponents == null)
        this.DefaultComponents = new List<MyContainerDefinition.DefaultComponent>();
      foreach (MyObjectBuilder_ContainerDefinition.DefaultComponentBuilder defaultComponent1 in containerDefinition.DefaultComponents)
      {
        MyContainerDefinition.DefaultComponent defaultComponent2 = new MyContainerDefinition.DefaultComponent();
        try
        {
          if (defaultComponent1.BuilderType != null)
          {
            MyObjectBuilderType objectBuilderType = MyObjectBuilderType.Parse(defaultComponent1.BuilderType);
            defaultComponent2.BuilderType = objectBuilderType;
          }
        }
        catch (Exception ex)
        {
          MyLog.Default.WriteLine(string.Format("Container definition error: can not parse defined component type {0} for container {1}", (object) defaultComponent1, (object) this.Id.ToString()));
        }
        try
        {
          if (defaultComponent1.InstanceType != null)
          {
            Type type = Type.GetType(defaultComponent1.InstanceType, true);
            defaultComponent2.InstanceType = type;
          }
        }
        catch (Exception ex)
        {
          MyLog.Default.WriteLine(string.Format("Container definition error: can not parse defined component type {0} for container {1}", (object) defaultComponent1, (object) this.Id.ToString()));
        }
        defaultComponent2.ForceCreate = defaultComponent1.ForceCreate;
        if (defaultComponent1.SubtypeId != null)
          defaultComponent2.SubtypeId = new MyStringHash?(MyStringHash.GetOrCompute(defaultComponent1.SubtypeId));
        if (defaultComponent2.IsValid())
          this.DefaultComponents.Add(defaultComponent2);
        else
          MyLog.Default.WriteLine(string.Format("Defined component {0} for container {1} is invalid, none builder type or instance type is defined! Skipping it.", (object) defaultComponent1, (object) this.Id.ToString()));
      }
    }

    public bool HasDefaultComponent(string component)
    {
      foreach (MyContainerDefinition.DefaultComponent defaultComponent in this.DefaultComponents)
      {
        if (!defaultComponent.BuilderType.IsNull && defaultComponent.BuilderType.ToString() == component || defaultComponent.InstanceType != (Type) null && defaultComponent.InstanceType.ToString() == component)
          return true;
      }
      return false;
    }

    public class DefaultComponent
    {
      public MyObjectBuilderType BuilderType = (MyObjectBuilderType) (Type) null;
      public Type InstanceType;
      public bool ForceCreate;
      public MyStringHash? SubtypeId;

      public bool IsValid() => this.InstanceType != (Type) null || !this.BuilderType.IsNull;
    }

    private class VRage_Game_MyContainerDefinition\u003C\u003EActor : IActivator, IActivator<MyContainerDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyContainerDefinition();

      MyContainerDefinition IActivator<MyContainerDefinition>.CreateInstance() => new MyContainerDefinition();
    }
  }
}
