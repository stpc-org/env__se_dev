// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_DestroyableItem
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_DestroyableItem : MyObjectBuilder_EntityBase
  {
    protected class VRage_Game_MyObjectBuilder_DestroyableItem\u003C\u003EEntityId\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EEntityId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DestroyableItem, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DestroyableItem owner, in long value) => this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DestroyableItem owner, out long value) => this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DestroyableItem\u003C\u003EPersistentFlags\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EPersistentFlags\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DestroyableItem, MyPersistentEntityFlags2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_DestroyableItem owner,
        in MyPersistentEntityFlags2 value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_DestroyableItem owner,
        out MyPersistentEntityFlags2 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_DestroyableItem\u003C\u003EName\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DestroyableItem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DestroyableItem owner, in string value) => this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DestroyableItem owner, out string value) => this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DestroyableItem\u003C\u003EPositionAndOrientation\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EPositionAndOrientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DestroyableItem, MyPositionAndOrientation?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_DestroyableItem owner,
        in MyPositionAndOrientation? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_DestroyableItem owner,
        out MyPositionAndOrientation? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_DestroyableItem\u003C\u003ELocalPositionAndOrientation\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003ELocalPositionAndOrientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DestroyableItem, MyPositionAndOrientation?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_DestroyableItem owner,
        in MyPositionAndOrientation? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_DestroyableItem owner,
        out MyPositionAndOrientation? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_DestroyableItem\u003C\u003EComponentContainer\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EComponentContainer\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DestroyableItem, MyObjectBuilder_ComponentContainer>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_DestroyableItem owner,
        in MyObjectBuilder_ComponentContainer value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_DestroyableItem owner,
        out MyObjectBuilder_ComponentContainer value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_DestroyableItem\u003C\u003EEntityDefinitionId\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EEntityDefinitionId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DestroyableItem, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_DestroyableItem owner,
        in SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_DestroyableItem owner,
        out SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_DestroyableItem\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DestroyableItem, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DestroyableItem owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DestroyableItem owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DestroyableItem\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DestroyableItem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DestroyableItem owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DestroyableItem owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DestroyableItem\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DestroyableItem, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DestroyableItem owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DestroyableItem owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DestroyableItem\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DestroyableItem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DestroyableItem owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DestroyableItem owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_DestroyableItem\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_DestroyableItem>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_DestroyableItem();

      MyObjectBuilder_DestroyableItem IActivator<MyObjectBuilder_DestroyableItem>.CreateInstance() => new MyObjectBuilder_DestroyableItem();
    }
  }
}
