// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
  [MyObjectBuilderDefinition(null, null)]
  [XmlType("AddPlanetPrefab")]
  public class MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab : MyObjectBuilder_WorldGeneratorOperation
  {
    [ProtoMember(160)]
    [XmlAttribute]
    public string PrefabName;
    [ProtoMember(163)]
    [XmlAttribute]
    public string DefinitionName;
    [ProtoMember(166)]
    [XmlAttribute]
    public bool AddGPS;
    [ProtoMember(169)]
    public SerializableVector3D Position;

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab\u003C\u003EPrefabName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner,
        in string value)
      {
        owner.PrefabName = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner,
        out string value)
      {
        value = owner.PrefabName;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab\u003C\u003EDefinitionName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner,
        in string value)
      {
        owner.DefinitionName = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner,
        out string value)
      {
        value = owner.DefinitionName;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab\u003C\u003EAddGPS\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner,
        in bool value)
      {
        owner.AddGPS = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner,
        out bool value)
      {
        value = owner.AddGPS;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab\u003C\u003EPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab, SerializableVector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner,
        in SerializableVector3D value)
      {
        owner.Position = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner,
        out SerializableVector3D value)
      {
        value = owner.Position;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab\u003C\u003EFactionTag\u003C\u003EAccessor : MyObjectBuilder_WorldGeneratorOperation.VRage_Game_MyObjectBuilder_WorldGeneratorOperation\u003C\u003EFactionTag\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_WorldGeneratorOperation&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_WorldGeneratorOperation&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab();

      MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab IActivator<MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab>.CreateInstance() => new MyObjectBuilder_WorldGeneratorOperation_AddPlanetPrefab();
    }
  }
}
