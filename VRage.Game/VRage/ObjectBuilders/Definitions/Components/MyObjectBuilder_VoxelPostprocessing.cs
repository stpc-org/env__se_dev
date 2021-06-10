// Decompiled with JetBrains decompiler
// Type: VRage.ObjectBuilders.Definitions.Components.MyObjectBuilder_VoxelPostprocessing
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.Utils;

namespace VRage.ObjectBuilders.Definitions.Components
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_VoxelPostprocessing : MyObjectBuilder_Base
  {
    [ProtoMember(1)]
    [XmlAttribute]
    public bool ForPhysics;

    protected class VRage_ObjectBuilders_Definitions_Components_MyObjectBuilder_VoxelPostprocessing\u003C\u003EForPhysics\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelPostprocessing, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelPostprocessing owner, in bool value) => owner.ForPhysics = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelPostprocessing owner, out bool value) => value = owner.ForPhysics;
    }

    protected class VRage_ObjectBuilders_Definitions_Components_MyObjectBuilder_VoxelPostprocessing\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelPostprocessing, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelPostprocessing owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelPostprocessing owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_ObjectBuilders_Definitions_Components_MyObjectBuilder_VoxelPostprocessing\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelPostprocessing, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelPostprocessing owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelPostprocessing owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_ObjectBuilders_Definitions_Components_MyObjectBuilder_VoxelPostprocessing\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelPostprocessing, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelPostprocessing owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelPostprocessing owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_ObjectBuilders_Definitions_Components_MyObjectBuilder_VoxelPostprocessing\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelPostprocessing, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelPostprocessing owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelPostprocessing owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_ObjectBuilders_Definitions_Components_MyObjectBuilder_VoxelPostprocessing\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_VoxelPostprocessing>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_VoxelPostprocessing();

      MyObjectBuilder_VoxelPostprocessing IActivator<MyObjectBuilder_VoxelPostprocessing>.CreateInstance() => new MyObjectBuilder_VoxelPostprocessing();
    }
  }
}
