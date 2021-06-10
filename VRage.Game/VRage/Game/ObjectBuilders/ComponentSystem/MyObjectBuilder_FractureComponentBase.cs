// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.ComponentSystem.MyObjectBuilder_FractureComponentBase
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
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
  public class MyObjectBuilder_FractureComponentBase : MyObjectBuilder_ComponentBase
  {
    [ProtoMember(7)]
    public List<MyObjectBuilder_FractureComponentBase.FracturedShape> Shapes = new List<MyObjectBuilder_FractureComponentBase.FracturedShape>();

    [ProtoContract]
    public struct FracturedShape
    {
      [ProtoMember(1)]
      public string Name;
      [ProtoMember(4)]
      [DefaultValue(false)]
      public bool Fixed;

      protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_FractureComponentBase\u003C\u003EFracturedShape\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FractureComponentBase.FracturedShape, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_FractureComponentBase.FracturedShape owner,
          in string value)
        {
          owner.Name = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_FractureComponentBase.FracturedShape owner,
          out string value)
        {
          value = owner.Name;
        }
      }

      protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_FractureComponentBase\u003C\u003EFracturedShape\u003C\u003EFixed\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FractureComponentBase.FracturedShape, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_FractureComponentBase.FracturedShape owner,
          in bool value)
        {
          owner.Fixed = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_FractureComponentBase.FracturedShape owner,
          out bool value)
        {
          value = owner.Fixed;
        }
      }

      private class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_FractureComponentBase\u003C\u003EFracturedShape\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_FractureComponentBase.FracturedShape>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_FractureComponentBase.FracturedShape();

        MyObjectBuilder_FractureComponentBase.FracturedShape IActivator<MyObjectBuilder_FractureComponentBase.FracturedShape>.CreateInstance() => new MyObjectBuilder_FractureComponentBase.FracturedShape();
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_FractureComponentBase\u003C\u003EShapes\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FractureComponentBase, List<MyObjectBuilder_FractureComponentBase.FracturedShape>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FractureComponentBase owner,
        in List<MyObjectBuilder_FractureComponentBase.FracturedShape> value)
      {
        owner.Shapes = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FractureComponentBase owner,
        out List<MyObjectBuilder_FractureComponentBase.FracturedShape> value)
      {
        value = owner.Shapes;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_FractureComponentBase\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FractureComponentBase, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FractureComponentBase owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FractureComponentBase owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_FractureComponentBase\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FractureComponentBase, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FractureComponentBase owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FractureComponentBase owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_FractureComponentBase\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FractureComponentBase, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FractureComponentBase owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FractureComponentBase owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_FractureComponentBase\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FractureComponentBase, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FractureComponentBase owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FractureComponentBase owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_FractureComponentBase\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_FractureComponentBase>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_FractureComponentBase();

      MyObjectBuilder_FractureComponentBase IActivator<MyObjectBuilder_FractureComponentBase>.CreateInstance() => new MyObjectBuilder_FractureComponentBase();
    }
  }
}
