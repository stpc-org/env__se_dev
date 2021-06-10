// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab
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
  [XmlType("SetupBasePrefab")]
  public class MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab : MyObjectBuilder_WorldGeneratorOperation
  {
    [ProtoMember(148)]
    [XmlAttribute]
    public string PrefabFile;
    [ProtoMember(151)]
    public SerializableVector3 Offset;
    [ProtoMember(154)]
    [XmlAttribute]
    public string AsteroidName;
    [ProtoMember(157)]
    [XmlAttribute]
    public string BeaconName;

    public bool ShouldSerializeOffset() => this.Offset != new SerializableVector3(0.0f, 0.0f, 0.0f);

    public bool ShouldSerializeBeaconName() => !string.IsNullOrEmpty(this.BeaconName);

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab\u003C\u003EPrefabFile\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner,
        in string value)
      {
        owner.PrefabFile = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner,
        out string value)
      {
        value = owner.PrefabFile;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab\u003C\u003EOffset\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner,
        in SerializableVector3 value)
      {
        owner.Offset = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner,
        out SerializableVector3 value)
      {
        value = owner.Offset;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab\u003C\u003EAsteroidName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner,
        in string value)
      {
        owner.AsteroidName = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner,
        out string value)
      {
        value = owner.AsteroidName;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab\u003C\u003EBeaconName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner,
        in string value)
      {
        owner.BeaconName = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner,
        out string value)
      {
        value = owner.BeaconName;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab\u003C\u003EFactionTag\u003C\u003EAccessor : MyObjectBuilder_WorldGeneratorOperation.VRage_Game_MyObjectBuilder_WorldGeneratorOperation\u003C\u003EFactionTag\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_WorldGeneratorOperation&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_WorldGeneratorOperation&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab();

      MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab IActivator<MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab>.CreateInstance() => new MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab();
    }
  }
}
