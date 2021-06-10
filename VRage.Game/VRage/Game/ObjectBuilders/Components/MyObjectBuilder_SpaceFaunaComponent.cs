// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Components.MyObjectBuilder_SpaceFaunaComponent
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
  public class MyObjectBuilder_SpaceFaunaComponent : MyObjectBuilder_SessionComponent
  {
    [XmlArrayItem("Info")]
    [ProtoMember(28)]
    public List<MyObjectBuilder_SpaceFaunaComponent.SpawnInfo> SpawnInfos = new List<MyObjectBuilder_SpaceFaunaComponent.SpawnInfo>();
    [XmlArrayItem("Info")]
    [ProtoMember(31)]
    public List<MyObjectBuilder_SpaceFaunaComponent.TimeoutInfo> TimeoutInfos = new List<MyObjectBuilder_SpaceFaunaComponent.TimeoutInfo>();

    [ProtoContract]
    public class SpawnInfo
    {
      [ProtoMember(1)]
      [XmlAttribute]
      public double X;
      [ProtoMember(4)]
      [XmlAttribute]
      public double Y;
      [ProtoMember(7)]
      [XmlAttribute]
      public double Z;
      [ProtoMember(10)]
      [XmlAttribute("S")]
      public int SpawnTime;
      [ProtoMember(13)]
      [XmlAttribute("A")]
      public int AbandonTime;

      protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent\u003C\u003ESpawnInfo\u003C\u003EX\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SpaceFaunaComponent.SpawnInfo, double>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_SpaceFaunaComponent.SpawnInfo owner,
          in double value)
        {
          owner.X = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_SpaceFaunaComponent.SpawnInfo owner,
          out double value)
        {
          value = owner.X;
        }
      }

      protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent\u003C\u003ESpawnInfo\u003C\u003EY\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SpaceFaunaComponent.SpawnInfo, double>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_SpaceFaunaComponent.SpawnInfo owner,
          in double value)
        {
          owner.Y = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_SpaceFaunaComponent.SpawnInfo owner,
          out double value)
        {
          value = owner.Y;
        }
      }

      protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent\u003C\u003ESpawnInfo\u003C\u003EZ\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SpaceFaunaComponent.SpawnInfo, double>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_SpaceFaunaComponent.SpawnInfo owner,
          in double value)
        {
          owner.Z = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_SpaceFaunaComponent.SpawnInfo owner,
          out double value)
        {
          value = owner.Z;
        }
      }

      protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent\u003C\u003ESpawnInfo\u003C\u003ESpawnTime\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SpaceFaunaComponent.SpawnInfo, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_SpaceFaunaComponent.SpawnInfo owner,
          in int value)
        {
          owner.SpawnTime = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_SpaceFaunaComponent.SpawnInfo owner,
          out int value)
        {
          value = owner.SpawnTime;
        }
      }

      protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent\u003C\u003ESpawnInfo\u003C\u003EAbandonTime\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SpaceFaunaComponent.SpawnInfo, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_SpaceFaunaComponent.SpawnInfo owner,
          in int value)
        {
          owner.AbandonTime = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_SpaceFaunaComponent.SpawnInfo owner,
          out int value)
        {
          value = owner.AbandonTime;
        }
      }

      private class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent\u003C\u003ESpawnInfo\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_SpaceFaunaComponent.SpawnInfo>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_SpaceFaunaComponent.SpawnInfo();

        MyObjectBuilder_SpaceFaunaComponent.SpawnInfo IActivator<MyObjectBuilder_SpaceFaunaComponent.SpawnInfo>.CreateInstance() => new MyObjectBuilder_SpaceFaunaComponent.SpawnInfo();
      }
    }

    [ProtoContract]
    public class TimeoutInfo
    {
      [ProtoMember(16)]
      [XmlAttribute]
      public double X;
      [ProtoMember(19)]
      [XmlAttribute]
      public double Y;
      [ProtoMember(22)]
      [XmlAttribute]
      public double Z;
      [ProtoMember(25)]
      [XmlAttribute("T")]
      public int Timeout;

      protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent\u003C\u003ETimeoutInfo\u003C\u003EX\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SpaceFaunaComponent.TimeoutInfo, double>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_SpaceFaunaComponent.TimeoutInfo owner,
          in double value)
        {
          owner.X = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_SpaceFaunaComponent.TimeoutInfo owner,
          out double value)
        {
          value = owner.X;
        }
      }

      protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent\u003C\u003ETimeoutInfo\u003C\u003EY\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SpaceFaunaComponent.TimeoutInfo, double>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_SpaceFaunaComponent.TimeoutInfo owner,
          in double value)
        {
          owner.Y = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_SpaceFaunaComponent.TimeoutInfo owner,
          out double value)
        {
          value = owner.Y;
        }
      }

      protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent\u003C\u003ETimeoutInfo\u003C\u003EZ\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SpaceFaunaComponent.TimeoutInfo, double>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_SpaceFaunaComponent.TimeoutInfo owner,
          in double value)
        {
          owner.Z = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_SpaceFaunaComponent.TimeoutInfo owner,
          out double value)
        {
          value = owner.Z;
        }
      }

      protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent\u003C\u003ETimeoutInfo\u003C\u003ETimeout\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SpaceFaunaComponent.TimeoutInfo, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_SpaceFaunaComponent.TimeoutInfo owner,
          in int value)
        {
          owner.Timeout = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_SpaceFaunaComponent.TimeoutInfo owner,
          out int value)
        {
          value = owner.Timeout;
        }
      }

      private class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent\u003C\u003ETimeoutInfo\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_SpaceFaunaComponent.TimeoutInfo>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_SpaceFaunaComponent.TimeoutInfo();

        MyObjectBuilder_SpaceFaunaComponent.TimeoutInfo IActivator<MyObjectBuilder_SpaceFaunaComponent.TimeoutInfo>.CreateInstance() => new MyObjectBuilder_SpaceFaunaComponent.TimeoutInfo();
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent\u003C\u003ESpawnInfos\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SpaceFaunaComponent, List<MyObjectBuilder_SpaceFaunaComponent.SpawnInfo>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SpaceFaunaComponent owner,
        in List<MyObjectBuilder_SpaceFaunaComponent.SpawnInfo> value)
      {
        owner.SpawnInfos = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SpaceFaunaComponent owner,
        out List<MyObjectBuilder_SpaceFaunaComponent.SpawnInfo> value)
      {
        value = owner.SpawnInfos;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent\u003C\u003ETimeoutInfos\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SpaceFaunaComponent, List<MyObjectBuilder_SpaceFaunaComponent.TimeoutInfo>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SpaceFaunaComponent owner,
        in List<MyObjectBuilder_SpaceFaunaComponent.TimeoutInfo> value)
      {
        owner.TimeoutInfos = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SpaceFaunaComponent owner,
        out List<MyObjectBuilder_SpaceFaunaComponent.TimeoutInfo> value)
      {
        value = owner.TimeoutInfos;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SpaceFaunaComponent, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SpaceFaunaComponent owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SpaceFaunaComponent owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SpaceFaunaComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SpaceFaunaComponent owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SpaceFaunaComponent owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent\u003C\u003EDefinition\u003C\u003EAccessor : MyObjectBuilder_SessionComponent.VRage_Game_MyObjectBuilder_SessionComponent\u003C\u003EDefinition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SpaceFaunaComponent, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SpaceFaunaComponent owner,
        in SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_SessionComponent&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SpaceFaunaComponent owner,
        out SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_SessionComponent&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SpaceFaunaComponent, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SpaceFaunaComponent owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SpaceFaunaComponent owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SpaceFaunaComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SpaceFaunaComponent owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SpaceFaunaComponent owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SpaceFaunaComponent\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_SpaceFaunaComponent>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_SpaceFaunaComponent();

      MyObjectBuilder_SpaceFaunaComponent IActivator<MyObjectBuilder_SpaceFaunaComponent>.CreateInstance() => new MyObjectBuilder_SpaceFaunaComponent();
    }
  }
}
