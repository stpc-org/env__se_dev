// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_WorldGeneratorOperation_CreatePlanet
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
  [MyObjectBuilderDefinition(null, null)]
  [XmlType("CreatePlanet")]
  public class MyObjectBuilder_WorldGeneratorOperation_CreatePlanet : MyObjectBuilder_WorldGeneratorOperation
  {
    [ProtoMember(172)]
    [XmlAttribute]
    public string DefinitionName;
    [ProtoMember(175)]
    [XmlAttribute]
    public bool AddGPS;
    [ProtoMember(178)]
    public SerializableVector3D PositionMinCorner;
    [ProtoMember(181)]
    public SerializableVector3D PositionCenter = new SerializableVector3D((Vector3D) Vector3.Invalid);
    [ProtoMember(184)]
    public float Diameter;

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_CreatePlanet\u003C\u003EDefinitionName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_CreatePlanet, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner,
        in string value)
      {
        owner.DefinitionName = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner,
        out string value)
      {
        value = owner.DefinitionName;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_CreatePlanet\u003C\u003EAddGPS\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_CreatePlanet, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner,
        in bool value)
      {
        owner.AddGPS = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner,
        out bool value)
      {
        value = owner.AddGPS;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_CreatePlanet\u003C\u003EPositionMinCorner\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_CreatePlanet, SerializableVector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner,
        in SerializableVector3D value)
      {
        owner.PositionMinCorner = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner,
        out SerializableVector3D value)
      {
        value = owner.PositionMinCorner;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_CreatePlanet\u003C\u003EPositionCenter\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_CreatePlanet, SerializableVector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner,
        in SerializableVector3D value)
      {
        owner.PositionCenter = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner,
        out SerializableVector3D value)
      {
        value = owner.PositionCenter;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_CreatePlanet\u003C\u003EDiameter\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_CreatePlanet, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner,
        in float value)
      {
        owner.Diameter = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner,
        out float value)
      {
        value = owner.Diameter;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_CreatePlanet\u003C\u003EFactionTag\u003C\u003EAccessor : MyObjectBuilder_WorldGeneratorOperation.VRage_Game_MyObjectBuilder_WorldGeneratorOperation\u003C\u003EFactionTag\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_CreatePlanet, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_WorldGeneratorOperation&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_WorldGeneratorOperation&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_CreatePlanet\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_CreatePlanet, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_CreatePlanet\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_CreatePlanet, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_CreatePlanet\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_CreatePlanet, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_CreatePlanet\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_CreatePlanet, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_CreatePlanet owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_CreatePlanet\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_WorldGeneratorOperation_CreatePlanet>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_WorldGeneratorOperation_CreatePlanet();

      MyObjectBuilder_WorldGeneratorOperation_CreatePlanet IActivator<MyObjectBuilder_WorldGeneratorOperation_CreatePlanet>.CreateInstance() => new MyObjectBuilder_WorldGeneratorOperation_CreatePlanet();
    }
  }
}
