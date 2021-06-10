// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Components.MyObjectBuilder_CoordinateSystem
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

namespace VRage.Game.ObjectBuilders.Components
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_CoordinateSystem : MyObjectBuilder_SessionComponent
  {
    [ProtoMember(13)]
    public long LastCoordSysId = 1;
    [ProtoMember(16)]
    public List<MyObjectBuilder_CoordinateSystem.CoordSysInfo> CoordSystems = new List<MyObjectBuilder_CoordinateSystem.CoordSysInfo>();

    [ProtoContract]
    public struct CoordSysInfo
    {
      [ProtoMember(1)]
      public long Id;
      [ProtoMember(4)]
      public long EntityCount;
      [ProtoMember(7)]
      public SerializableQuaternion Rotation;
      [ProtoMember(10)]
      public SerializableVector3D Position;

      protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_CoordinateSystem\u003C\u003ECoordSysInfo\u003C\u003EId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CoordinateSystem.CoordSysInfo, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CoordinateSystem.CoordSysInfo owner,
          in long value)
        {
          owner.Id = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CoordinateSystem.CoordSysInfo owner,
          out long value)
        {
          value = owner.Id;
        }
      }

      protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_CoordinateSystem\u003C\u003ECoordSysInfo\u003C\u003EEntityCount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CoordinateSystem.CoordSysInfo, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CoordinateSystem.CoordSysInfo owner,
          in long value)
        {
          owner.EntityCount = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CoordinateSystem.CoordSysInfo owner,
          out long value)
        {
          value = owner.EntityCount;
        }
      }

      protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_CoordinateSystem\u003C\u003ECoordSysInfo\u003C\u003ERotation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CoordinateSystem.CoordSysInfo, SerializableQuaternion>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CoordinateSystem.CoordSysInfo owner,
          in SerializableQuaternion value)
        {
          owner.Rotation = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CoordinateSystem.CoordSysInfo owner,
          out SerializableQuaternion value)
        {
          value = owner.Rotation;
        }
      }

      protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_CoordinateSystem\u003C\u003ECoordSysInfo\u003C\u003EPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CoordinateSystem.CoordSysInfo, SerializableVector3D>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CoordinateSystem.CoordSysInfo owner,
          in SerializableVector3D value)
        {
          owner.Position = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CoordinateSystem.CoordSysInfo owner,
          out SerializableVector3D value)
        {
          value = owner.Position;
        }
      }

      private class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_CoordinateSystem\u003C\u003ECoordSysInfo\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_CoordinateSystem.CoordSysInfo>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_CoordinateSystem.CoordSysInfo();

        MyObjectBuilder_CoordinateSystem.CoordSysInfo IActivator<MyObjectBuilder_CoordinateSystem.CoordSysInfo>.CreateInstance() => new MyObjectBuilder_CoordinateSystem.CoordSysInfo();
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_CoordinateSystem\u003C\u003ELastCoordSysId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CoordinateSystem, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CoordinateSystem owner, in long value) => owner.LastCoordSysId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CoordinateSystem owner, out long value) => value = owner.LastCoordSysId;
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_CoordinateSystem\u003C\u003ECoordSystems\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CoordinateSystem, List<MyObjectBuilder_CoordinateSystem.CoordSysInfo>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CoordinateSystem owner,
        in List<MyObjectBuilder_CoordinateSystem.CoordSysInfo> value)
      {
        owner.CoordSystems = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CoordinateSystem owner,
        out List<MyObjectBuilder_CoordinateSystem.CoordSysInfo> value)
      {
        value = owner.CoordSystems;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_CoordinateSystem\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CoordinateSystem, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CoordinateSystem owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CoordinateSystem owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_CoordinateSystem\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CoordinateSystem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CoordinateSystem owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CoordinateSystem owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_CoordinateSystem\u003C\u003EDefinition\u003C\u003EAccessor : MyObjectBuilder_SessionComponent.VRage_Game_MyObjectBuilder_SessionComponent\u003C\u003EDefinition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CoordinateSystem, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CoordinateSystem owner,
        in SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_SessionComponent&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CoordinateSystem owner,
        out SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_SessionComponent&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_CoordinateSystem\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CoordinateSystem, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CoordinateSystem owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CoordinateSystem owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_CoordinateSystem\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CoordinateSystem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CoordinateSystem owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CoordinateSystem owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_CoordinateSystem\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_CoordinateSystem>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_CoordinateSystem();

      MyObjectBuilder_CoordinateSystem IActivator<MyObjectBuilder_CoordinateSystem>.CreateInstance() => new MyObjectBuilder_CoordinateSystem();
    }
  }
}
