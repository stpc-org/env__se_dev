// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.ComponentSystem.MyObjectBuilder_UseObjectsComponent
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.ObjectBuilders.ComponentSystem
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_UseObjectsComponent : MyObjectBuilder_ComponentBase
  {
    [ProtoMember(1)]
    public uint CustomDetectorsCount;
    [ProtoMember(4)]
    [DefaultValue(null)]
    public string[] CustomDetectorsNames;
    [ProtoMember(7)]
    [DefaultValue(null)]
    public Matrix[] CustomDetectorsMatrices;

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UseObjectsComponent\u003C\u003ECustomDetectorsCount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_UseObjectsComponent, uint>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_UseObjectsComponent owner, in uint value) => owner.CustomDetectorsCount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_UseObjectsComponent owner, out uint value) => value = owner.CustomDetectorsCount;
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UseObjectsComponent\u003C\u003ECustomDetectorsNames\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_UseObjectsComponent, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_UseObjectsComponent owner, in string[] value) => owner.CustomDetectorsNames = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_UseObjectsComponent owner, out string[] value) => value = owner.CustomDetectorsNames;
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UseObjectsComponent\u003C\u003ECustomDetectorsMatrices\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_UseObjectsComponent, Matrix[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_UseObjectsComponent owner, in Matrix[] value) => owner.CustomDetectorsMatrices = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_UseObjectsComponent owner, out Matrix[] value) => value = owner.CustomDetectorsMatrices;
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UseObjectsComponent\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_UseObjectsComponent, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_UseObjectsComponent owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_UseObjectsComponent owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UseObjectsComponent\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_UseObjectsComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_UseObjectsComponent owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_UseObjectsComponent owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UseObjectsComponent\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_UseObjectsComponent, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_UseObjectsComponent owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_UseObjectsComponent owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UseObjectsComponent\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_UseObjectsComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_UseObjectsComponent owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_UseObjectsComponent owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UseObjectsComponent\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_UseObjectsComponent>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_UseObjectsComponent();

      MyObjectBuilder_UseObjectsComponent IActivator<MyObjectBuilder_UseObjectsComponent>.CreateInstance() => new MyObjectBuilder_UseObjectsComponent();
    }
  }
}
