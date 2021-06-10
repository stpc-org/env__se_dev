// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.ComponentSystem.MyObjectBuilder_ComponentContainer
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.ComponentSystem
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_ComponentContainer : MyObjectBuilder_Base
  {
    [ProtoMember(7)]
    public List<MyObjectBuilder_ComponentContainer.ComponentData> Components = new List<MyObjectBuilder_ComponentContainer.ComponentData>();

    [ProtoContract]
    public class ComponentData
    {
      [ProtoMember(1)]
      public string TypeId;
      [ProtoMember(4)]
      [DynamicObjectBuilder(false)]
      [XmlElement(Type = typeof (MyAbstractXmlSerializer<MyObjectBuilder_ComponentBase>))]
      public MyObjectBuilder_ComponentBase Component;

      protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_ComponentContainer\u003C\u003EComponentData\u003C\u003ETypeId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ComponentContainer.ComponentData, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ComponentContainer.ComponentData owner,
          in string value)
        {
          owner.TypeId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ComponentContainer.ComponentData owner,
          out string value)
        {
          value = owner.TypeId;
        }
      }

      protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_ComponentContainer\u003C\u003EComponentData\u003C\u003EComponent\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ComponentContainer.ComponentData, MyObjectBuilder_ComponentBase>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ComponentContainer.ComponentData owner,
          in MyObjectBuilder_ComponentBase value)
        {
          owner.Component = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ComponentContainer.ComponentData owner,
          out MyObjectBuilder_ComponentBase value)
        {
          value = owner.Component;
        }
      }

      private class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_ComponentContainer\u003C\u003EComponentData\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ComponentContainer.ComponentData>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_ComponentContainer.ComponentData();

        MyObjectBuilder_ComponentContainer.ComponentData IActivator<MyObjectBuilder_ComponentContainer.ComponentData>.CreateInstance() => new MyObjectBuilder_ComponentContainer.ComponentData();
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_ComponentContainer\u003C\u003EComponents\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ComponentContainer, List<MyObjectBuilder_ComponentContainer.ComponentData>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ComponentContainer owner,
        in List<MyObjectBuilder_ComponentContainer.ComponentData> value)
      {
        owner.Components = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ComponentContainer owner,
        out List<MyObjectBuilder_ComponentContainer.ComponentData> value)
      {
        value = owner.Components;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_ComponentContainer\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentContainer, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ComponentContainer owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ComponentContainer owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_ComponentContainer\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentContainer, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ComponentContainer owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ComponentContainer owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_ComponentContainer\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentContainer, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ComponentContainer owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ComponentContainer owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_ComponentContainer\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentContainer, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ComponentContainer owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ComponentContainer owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_ComponentContainer\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ComponentContainer>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ComponentContainer();

      MyObjectBuilder_ComponentContainer IActivator<MyObjectBuilder_ComponentContainer>.CreateInstance() => new MyObjectBuilder_ComponentContainer();
    }
  }
}
