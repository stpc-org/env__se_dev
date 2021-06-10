﻿// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_Trees
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
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [MyEnvironmentItems(typeof (MyObjectBuilder_Tree))]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_Trees : MyObjectBuilder_EnvironmentItems
  {
    protected class VRage_Game_MyObjectBuilder_Trees\u003C\u003EItems\u003C\u003EAccessor : MyObjectBuilder_EnvironmentItems.VRage_Game_MyObjectBuilder_EnvironmentItems\u003C\u003EItems\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Trees, MyObjectBuilder_EnvironmentItems.MyOBEnvironmentItemData[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Trees owner,
        in MyObjectBuilder_EnvironmentItems.MyOBEnvironmentItemData[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EnvironmentItems&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Trees owner,
        out MyObjectBuilder_EnvironmentItems.MyOBEnvironmentItemData[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EnvironmentItems&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_Trees\u003C\u003ECellsOffset\u003C\u003EAccessor : MyObjectBuilder_EnvironmentItems.VRage_Game_MyObjectBuilder_EnvironmentItems\u003C\u003ECellsOffset\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Trees, Vector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Trees owner, in Vector3D value) => this.Set((MyObjectBuilder_EnvironmentItems&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Trees owner, out Vector3D value) => this.Get((MyObjectBuilder_EnvironmentItems&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Trees\u003C\u003EEntityId\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EEntityId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Trees, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Trees owner, in long value) => this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Trees owner, out long value) => this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Trees\u003C\u003EPersistentFlags\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EPersistentFlags\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Trees, MyPersistentEntityFlags2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Trees owner, in MyPersistentEntityFlags2 value) => this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Trees owner, out MyPersistentEntityFlags2 value) => this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Trees\u003C\u003EName\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Trees, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Trees owner, in string value) => this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Trees owner, out string value) => this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Trees\u003C\u003EPositionAndOrientation\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EPositionAndOrientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Trees, MyPositionAndOrientation?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Trees owner, in MyPositionAndOrientation? value) => this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Trees owner, out MyPositionAndOrientation? value) => this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Trees\u003C\u003ELocalPositionAndOrientation\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003ELocalPositionAndOrientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Trees, MyPositionAndOrientation?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Trees owner, in MyPositionAndOrientation? value) => this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Trees owner, out MyPositionAndOrientation? value) => this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Trees\u003C\u003EComponentContainer\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EComponentContainer\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Trees, MyObjectBuilder_ComponentContainer>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Trees owner,
        in MyObjectBuilder_ComponentContainer value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Trees owner,
        out MyObjectBuilder_ComponentContainer value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_Trees\u003C\u003EEntityDefinitionId\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EEntityDefinitionId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Trees, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Trees owner, in SerializableDefinitionId? value) => this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Trees owner, out SerializableDefinitionId? value) => this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Trees\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Trees, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Trees owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Trees owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Trees\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Trees, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Trees owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Trees owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Trees\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Trees, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Trees owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Trees owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Trees\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Trees, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Trees owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Trees owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_Trees\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Trees>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_Trees();

      MyObjectBuilder_Trees IActivator<MyObjectBuilder_Trees>.CreateInstance() => new MyObjectBuilder_Trees();
    }
  }
}
