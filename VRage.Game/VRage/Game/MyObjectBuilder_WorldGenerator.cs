// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_WorldGenerator
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

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_WorldGenerator : MyObjectBuilder_SessionComponent
  {
    [ProtoMember(25)]
    public HashSet<EmptyArea> MarkedAreas = new HashSet<EmptyArea>();
    [ProtoMember(28)]
    public HashSet<EmptyArea> DeletedAreas = new HashSet<EmptyArea>();
    [ProtoMember(31)]
    public HashSet<MyObjectSeedParams> ExistingObjectsSeeds = new HashSet<MyObjectSeedParams>();

    protected class VRage_Game_MyObjectBuilder_WorldGenerator\u003C\u003EMarkedAreas\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGenerator, HashSet<EmptyArea>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WorldGenerator owner, in HashSet<EmptyArea> value) => owner.MarkedAreas = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGenerator owner,
        out HashSet<EmptyArea> value)
      {
        value = owner.MarkedAreas;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGenerator\u003C\u003EDeletedAreas\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGenerator, HashSet<EmptyArea>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WorldGenerator owner, in HashSet<EmptyArea> value) => owner.DeletedAreas = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGenerator owner,
        out HashSet<EmptyArea> value)
      {
        value = owner.DeletedAreas;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGenerator\u003C\u003EExistingObjectsSeeds\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGenerator, HashSet<MyObjectSeedParams>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGenerator owner,
        in HashSet<MyObjectSeedParams> value)
      {
        owner.ExistingObjectsSeeds = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGenerator owner,
        out HashSet<MyObjectSeedParams> value)
      {
        value = owner.ExistingObjectsSeeds;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGenerator\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGenerator, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WorldGenerator owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WorldGenerator owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WorldGenerator\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGenerator, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WorldGenerator owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WorldGenerator owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WorldGenerator\u003C\u003EDefinition\u003C\u003EAccessor : MyObjectBuilder_SessionComponent.VRage_Game_MyObjectBuilder_SessionComponent\u003C\u003EDefinition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGenerator, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGenerator owner,
        in SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_SessionComponent&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGenerator owner,
        out SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_SessionComponent&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGenerator\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGenerator, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WorldGenerator owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WorldGenerator owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WorldGenerator\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGenerator, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WorldGenerator owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WorldGenerator owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_WorldGenerator\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_WorldGenerator>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_WorldGenerator();

      MyObjectBuilder_WorldGenerator IActivator<MyObjectBuilder_WorldGenerator>.CreateInstance() => new MyObjectBuilder_WorldGenerator();
    }
  }
}
