// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.ObjectBuilders.MyObjectBuilder_PlanetEnvironmentComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace Sandbox.Game.WorldEnvironment.ObjectBuilders
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("Sandbox.Game.XmlSerializers")]
  public class MyObjectBuilder_PlanetEnvironmentComponent : MyObjectBuilder_ComponentBase
  {
    [ProtoMember(1)]
    [XmlElement("Provider", Type = typeof (MyAbstractXmlSerializer<MyObjectBuilder_EnvironmentDataProvider>))]
    [DynamicNullableObjectBuilderItem(false)]
    public MyObjectBuilder_EnvironmentDataProvider[] DataProviders;
    [ProtoMember(10)]
    [XmlArrayItem("Sector")]
    [Nullable]
    public List<MyObjectBuilder_PlanetEnvironmentComponent.ObstructingBox> SectorObstructions;

    public MyObjectBuilder_PlanetEnvironmentComponent()
    {
      this.DataProviders = new MyObjectBuilder_EnvironmentDataProvider[0];
      this.SectorObstructions = new List<MyObjectBuilder_PlanetEnvironmentComponent.ObstructingBox>();
    }

    [ProtoContract]
    public struct ObstructingBox
    {
      [ProtoMember(4)]
      public long SectorId;
      [ProtoMember(7)]
      public List<SerializableOrientedBoundingBoxD> ObstructingBoxes;

      protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_PlanetEnvironmentComponent\u003C\u003EObstructingBox\u003C\u003ESectorId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetEnvironmentComponent.ObstructingBox, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_PlanetEnvironmentComponent.ObstructingBox owner,
          in long value)
        {
          owner.SectorId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_PlanetEnvironmentComponent.ObstructingBox owner,
          out long value)
        {
          value = owner.SectorId;
        }
      }

      protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_PlanetEnvironmentComponent\u003C\u003EObstructingBox\u003C\u003EObstructingBoxes\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetEnvironmentComponent.ObstructingBox, List<SerializableOrientedBoundingBoxD>>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_PlanetEnvironmentComponent.ObstructingBox owner,
          in List<SerializableOrientedBoundingBoxD> value)
        {
          owner.ObstructingBoxes = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_PlanetEnvironmentComponent.ObstructingBox owner,
          out List<SerializableOrientedBoundingBoxD> value)
        {
          value = owner.ObstructingBoxes;
        }
      }

      private class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_PlanetEnvironmentComponent\u003C\u003EObstructingBox\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_PlanetEnvironmentComponent.ObstructingBox>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_PlanetEnvironmentComponent.ObstructingBox();

        MyObjectBuilder_PlanetEnvironmentComponent.ObstructingBox IActivator<MyObjectBuilder_PlanetEnvironmentComponent.ObstructingBox>.CreateInstance() => new MyObjectBuilder_PlanetEnvironmentComponent.ObstructingBox();
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_PlanetEnvironmentComponent\u003C\u003EDataProviders\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetEnvironmentComponent, MyObjectBuilder_EnvironmentDataProvider[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetEnvironmentComponent owner,
        in MyObjectBuilder_EnvironmentDataProvider[] value)
      {
        owner.DataProviders = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetEnvironmentComponent owner,
        out MyObjectBuilder_EnvironmentDataProvider[] value)
      {
        value = owner.DataProviders;
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_PlanetEnvironmentComponent\u003C\u003ESectorObstructions\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PlanetEnvironmentComponent, List<MyObjectBuilder_PlanetEnvironmentComponent.ObstructingBox>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetEnvironmentComponent owner,
        in List<MyObjectBuilder_PlanetEnvironmentComponent.ObstructingBox> value)
      {
        owner.SectorObstructions = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetEnvironmentComponent owner,
        out List<MyObjectBuilder_PlanetEnvironmentComponent.ObstructingBox> value)
      {
        value = owner.SectorObstructions;
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_PlanetEnvironmentComponent\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PlanetEnvironmentComponent, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetEnvironmentComponent owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetEnvironmentComponent owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_PlanetEnvironmentComponent\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PlanetEnvironmentComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetEnvironmentComponent owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetEnvironmentComponent owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_PlanetEnvironmentComponent\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PlanetEnvironmentComponent, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetEnvironmentComponent owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetEnvironmentComponent owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_PlanetEnvironmentComponent\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PlanetEnvironmentComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PlanetEnvironmentComponent owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PlanetEnvironmentComponent owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_PlanetEnvironmentComponent\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_PlanetEnvironmentComponent>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_PlanetEnvironmentComponent();

      MyObjectBuilder_PlanetEnvironmentComponent IActivator<MyObjectBuilder_PlanetEnvironmentComponent>.CreateInstance() => new MyObjectBuilder_PlanetEnvironmentComponent();
    }
  }
}
