// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.ObjectBuilders.MyObjectBuilder_ProceduralEnvironmentProvider
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.WorldEnvironment.ObjectBuilders
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("Sandbox.Game.XmlSerializers")]
  public class MyObjectBuilder_ProceduralEnvironmentProvider : MyObjectBuilder_EnvironmentDataProvider
  {
    [ProtoMember(1)]
    [XmlElement("Sector")]
    public List<MyObjectBuilder_ProceduralEnvironmentSector> Sectors = new List<MyObjectBuilder_ProceduralEnvironmentSector>();

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralEnvironmentProvider\u003C\u003ESectors\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProceduralEnvironmentProvider, List<MyObjectBuilder_ProceduralEnvironmentSector>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProceduralEnvironmentProvider owner,
        in List<MyObjectBuilder_ProceduralEnvironmentSector> value)
      {
        owner.Sectors = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProceduralEnvironmentProvider owner,
        out List<MyObjectBuilder_ProceduralEnvironmentSector> value)
      {
        value = owner.Sectors;
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralEnvironmentProvider\u003C\u003EFace\u003C\u003EAccessor : MyObjectBuilder_EnvironmentDataProvider.Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_EnvironmentDataProvider\u003C\u003EFace\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralEnvironmentProvider, Base6Directions.Direction>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProceduralEnvironmentProvider owner,
        in Base6Directions.Direction value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EnvironmentDataProvider&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProceduralEnvironmentProvider owner,
        out Base6Directions.Direction value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EnvironmentDataProvider&) ref owner, out value);
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralEnvironmentProvider\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralEnvironmentProvider, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProceduralEnvironmentProvider owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProceduralEnvironmentProvider owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralEnvironmentProvider\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralEnvironmentProvider, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProceduralEnvironmentProvider owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProceduralEnvironmentProvider owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralEnvironmentProvider\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralEnvironmentProvider, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProceduralEnvironmentProvider owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProceduralEnvironmentProvider owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralEnvironmentProvider\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralEnvironmentProvider, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProceduralEnvironmentProvider owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProceduralEnvironmentProvider owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralEnvironmentProvider\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ProceduralEnvironmentProvider>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ProceduralEnvironmentProvider();

      MyObjectBuilder_ProceduralEnvironmentProvider IActivator<MyObjectBuilder_ProceduralEnvironmentProvider>.CreateInstance() => new MyObjectBuilder_ProceduralEnvironmentProvider();
    }
  }
}
