﻿// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_EnvironmentItems
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
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  [MyEnvironmentItems(typeof (MyObjectBuilder_EnvironmentItemDefinition))]
  public class MyObjectBuilder_EnvironmentItems : MyObjectBuilder_EntityBase
  {
    [XmlArrayItem("Item")]
    [ProtoMember(7)]
    public MyObjectBuilder_EnvironmentItems.MyOBEnvironmentItemData[] Items;
    [ProtoMember(10)]
    public Vector3D CellsOffset;

    [ProtoContract]
    public struct MyOBEnvironmentItemData
    {
      [ProtoMember(1)]
      public MyPositionAndOrientation PositionAndOrientation;
      [ProtoMember(4)]
      public string SubtypeName;

      protected class VRage_Game_MyObjectBuilder_EnvironmentItems\u003C\u003EMyOBEnvironmentItemData\u003C\u003EPositionAndOrientation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentItems.MyOBEnvironmentItemData, MyPositionAndOrientation>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_EnvironmentItems.MyOBEnvironmentItemData owner,
          in MyPositionAndOrientation value)
        {
          owner.PositionAndOrientation = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_EnvironmentItems.MyOBEnvironmentItemData owner,
          out MyPositionAndOrientation value)
        {
          value = owner.PositionAndOrientation;
        }
      }

      protected class VRage_Game_MyObjectBuilder_EnvironmentItems\u003C\u003EMyOBEnvironmentItemData\u003C\u003ESubtypeName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentItems.MyOBEnvironmentItemData, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_EnvironmentItems.MyOBEnvironmentItemData owner,
          in string value)
        {
          owner.SubtypeName = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_EnvironmentItems.MyOBEnvironmentItemData owner,
          out string value)
        {
          value = owner.SubtypeName;
        }
      }

      private class VRage_Game_MyObjectBuilder_EnvironmentItems\u003C\u003EMyOBEnvironmentItemData\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_EnvironmentItems.MyOBEnvironmentItemData>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_EnvironmentItems.MyOBEnvironmentItemData();

        MyObjectBuilder_EnvironmentItems.MyOBEnvironmentItemData IActivator<MyObjectBuilder_EnvironmentItems.MyOBEnvironmentItemData>.CreateInstance() => new MyObjectBuilder_EnvironmentItems.MyOBEnvironmentItemData();
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentItems\u003C\u003EItems\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentItems, MyObjectBuilder_EnvironmentItems.MyOBEnvironmentItemData[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentItems owner,
        in MyObjectBuilder_EnvironmentItems.MyOBEnvironmentItemData[] value)
      {
        owner.Items = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentItems owner,
        out MyObjectBuilder_EnvironmentItems.MyOBEnvironmentItemData[] value)
      {
        value = owner.Items;
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentItems\u003C\u003ECellsOffset\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentItems, Vector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentItems owner, in Vector3D value) => owner.CellsOffset = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentItems owner, out Vector3D value) => value = owner.CellsOffset;
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentItems\u003C\u003EEntityId\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EEntityId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItems, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentItems owner, in long value) => this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentItems owner, out long value) => this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentItems\u003C\u003EPersistentFlags\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EPersistentFlags\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItems, MyPersistentEntityFlags2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentItems owner,
        in MyPersistentEntityFlags2 value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentItems owner,
        out MyPersistentEntityFlags2 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentItems\u003C\u003EName\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItems, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentItems owner, in string value) => this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentItems owner, out string value) => this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentItems\u003C\u003EPositionAndOrientation\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EPositionAndOrientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItems, MyPositionAndOrientation?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentItems owner,
        in MyPositionAndOrientation? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentItems owner,
        out MyPositionAndOrientation? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentItems\u003C\u003ELocalPositionAndOrientation\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003ELocalPositionAndOrientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItems, MyPositionAndOrientation?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentItems owner,
        in MyPositionAndOrientation? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentItems owner,
        out MyPositionAndOrientation? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentItems\u003C\u003EComponentContainer\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EComponentContainer\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItems, MyObjectBuilder_ComponentContainer>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentItems owner,
        in MyObjectBuilder_ComponentContainer value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentItems owner,
        out MyObjectBuilder_ComponentContainer value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentItems\u003C\u003EEntityDefinitionId\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EEntityDefinitionId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItems, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentItems owner,
        in SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentItems owner,
        out SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentItems\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItems, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentItems owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentItems owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentItems\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItems, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentItems owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentItems owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentItems\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItems, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentItems owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentItems owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentItems\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentItems, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentItems owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentItems owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_EnvironmentItems\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_EnvironmentItems>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_EnvironmentItems();

      MyObjectBuilder_EnvironmentItems IActivator<MyObjectBuilder_EnvironmentItems>.CreateInstance() => new MyObjectBuilder_EnvironmentItems();
    }
  }
}
