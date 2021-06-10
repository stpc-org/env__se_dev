// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.ComponentSystem.MyObjectBuilder_EntityStorageComponent
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilder;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.ComponentSystem
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_EntityStorageComponent : MyObjectBuilder_ComponentBase
  {
    [ProtoMember(5)]
    [DefaultValue(null)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public SerializableDictionary<string, bool> BoolStorage = new SerializableDictionary<string, bool>();
    [ProtoMember(10)]
    [DefaultValue(null)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public SerializableDictionary<string, int> IntStorage = new SerializableDictionary<string, int>();
    [ProtoMember(15)]
    [DefaultValue(null)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public SerializableDictionary<string, long> LongStorage = new SerializableDictionary<string, long>();
    [ProtoMember(20)]
    [DefaultValue(null)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public SerializableDictionary<string, string> StringStorage = new SerializableDictionary<string, string>();
    [ProtoMember(25)]
    [DefaultValue(null)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public SerializableDictionary<string, float> FloatStorage = new SerializableDictionary<string, float>();
    [ProtoMember(30)]
    [DefaultValue(null)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public SerializableDictionary<string, SerializableVector3D> Vector3DStorage = new SerializableDictionary<string, SerializableVector3D>();
    [ProtoMember(35)]
    [DefaultValue(null)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public SerializableDictionary<string, MySerializableList<bool>> BoolListStorage = new SerializableDictionary<string, MySerializableList<bool>>();
    [ProtoMember(40)]
    [DefaultValue(null)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public SerializableDictionary<string, MySerializableList<int>> IntListStorage = new SerializableDictionary<string, MySerializableList<int>>();
    [ProtoMember(45)]
    [DefaultValue(null)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public SerializableDictionary<string, MySerializableList<long>> LongListStorage = new SerializableDictionary<string, MySerializableList<long>>();
    [ProtoMember(50)]
    [DefaultValue(null)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public SerializableDictionary<string, MySerializableList<string>> StringListStorage = new SerializableDictionary<string, MySerializableList<string>>();
    [ProtoMember(55)]
    [DefaultValue(null)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public SerializableDictionary<string, MySerializableList<float>> FloatListStorage = new SerializableDictionary<string, MySerializableList<float>>();
    [ProtoMember(60)]
    [DefaultValue(null)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public SerializableDictionary<string, MySerializableList<SerializableVector3D>> Vector3DListStorage = new SerializableDictionary<string, MySerializableList<SerializableVector3D>>();

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_EntityStorageComponent\u003C\u003EBoolStorage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStorageComponent, SerializableDictionary<string, bool>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EntityStorageComponent owner,
        in SerializableDictionary<string, bool> value)
      {
        owner.BoolStorage = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EntityStorageComponent owner,
        out SerializableDictionary<string, bool> value)
      {
        value = owner.BoolStorage;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_EntityStorageComponent\u003C\u003EIntStorage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStorageComponent, SerializableDictionary<string, int>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EntityStorageComponent owner,
        in SerializableDictionary<string, int> value)
      {
        owner.IntStorage = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EntityStorageComponent owner,
        out SerializableDictionary<string, int> value)
      {
        value = owner.IntStorage;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_EntityStorageComponent\u003C\u003ELongStorage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStorageComponent, SerializableDictionary<string, long>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EntityStorageComponent owner,
        in SerializableDictionary<string, long> value)
      {
        owner.LongStorage = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EntityStorageComponent owner,
        out SerializableDictionary<string, long> value)
      {
        value = owner.LongStorage;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_EntityStorageComponent\u003C\u003EStringStorage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStorageComponent, SerializableDictionary<string, string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EntityStorageComponent owner,
        in SerializableDictionary<string, string> value)
      {
        owner.StringStorage = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EntityStorageComponent owner,
        out SerializableDictionary<string, string> value)
      {
        value = owner.StringStorage;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_EntityStorageComponent\u003C\u003EFloatStorage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStorageComponent, SerializableDictionary<string, float>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EntityStorageComponent owner,
        in SerializableDictionary<string, float> value)
      {
        owner.FloatStorage = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EntityStorageComponent owner,
        out SerializableDictionary<string, float> value)
      {
        value = owner.FloatStorage;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_EntityStorageComponent\u003C\u003EVector3DStorage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStorageComponent, SerializableDictionary<string, SerializableVector3D>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EntityStorageComponent owner,
        in SerializableDictionary<string, SerializableVector3D> value)
      {
        owner.Vector3DStorage = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EntityStorageComponent owner,
        out SerializableDictionary<string, SerializableVector3D> value)
      {
        value = owner.Vector3DStorage;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_EntityStorageComponent\u003C\u003EBoolListStorage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStorageComponent, SerializableDictionary<string, MySerializableList<bool>>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EntityStorageComponent owner,
        in SerializableDictionary<string, MySerializableList<bool>> value)
      {
        owner.BoolListStorage = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EntityStorageComponent owner,
        out SerializableDictionary<string, MySerializableList<bool>> value)
      {
        value = owner.BoolListStorage;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_EntityStorageComponent\u003C\u003EIntListStorage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStorageComponent, SerializableDictionary<string, MySerializableList<int>>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EntityStorageComponent owner,
        in SerializableDictionary<string, MySerializableList<int>> value)
      {
        owner.IntListStorage = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EntityStorageComponent owner,
        out SerializableDictionary<string, MySerializableList<int>> value)
      {
        value = owner.IntListStorage;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_EntityStorageComponent\u003C\u003ELongListStorage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStorageComponent, SerializableDictionary<string, MySerializableList<long>>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EntityStorageComponent owner,
        in SerializableDictionary<string, MySerializableList<long>> value)
      {
        owner.LongListStorage = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EntityStorageComponent owner,
        out SerializableDictionary<string, MySerializableList<long>> value)
      {
        value = owner.LongListStorage;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_EntityStorageComponent\u003C\u003EStringListStorage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStorageComponent, SerializableDictionary<string, MySerializableList<string>>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EntityStorageComponent owner,
        in SerializableDictionary<string, MySerializableList<string>> value)
      {
        owner.StringListStorage = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EntityStorageComponent owner,
        out SerializableDictionary<string, MySerializableList<string>> value)
      {
        value = owner.StringListStorage;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_EntityStorageComponent\u003C\u003EFloatListStorage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStorageComponent, SerializableDictionary<string, MySerializableList<float>>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EntityStorageComponent owner,
        in SerializableDictionary<string, MySerializableList<float>> value)
      {
        owner.FloatListStorage = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EntityStorageComponent owner,
        out SerializableDictionary<string, MySerializableList<float>> value)
      {
        value = owner.FloatListStorage;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_EntityStorageComponent\u003C\u003EVector3DListStorage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStorageComponent, SerializableDictionary<string, MySerializableList<SerializableVector3D>>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EntityStorageComponent owner,
        in SerializableDictionary<string, MySerializableList<SerializableVector3D>> value)
      {
        owner.Vector3DListStorage = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EntityStorageComponent owner,
        out SerializableDictionary<string, MySerializableList<SerializableVector3D>> value)
      {
        value = owner.Vector3DListStorage;
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_EntityStorageComponent\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStorageComponent, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EntityStorageComponent owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EntityStorageComponent owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_EntityStorageComponent\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStorageComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStorageComponent owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityStorageComponent owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_EntityStorageComponent\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStorageComponent, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EntityStorageComponent owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EntityStorageComponent owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_EntityStorageComponent\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStorageComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStorageComponent owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityStorageComponent owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_EntityStorageComponent\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_EntityStorageComponent>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_EntityStorageComponent();

      MyObjectBuilder_EntityStorageComponent IActivator<MyObjectBuilder_EntityStorageComponent>.CreateInstance() => new MyObjectBuilder_EntityStorageComponent();
    }
  }
}
