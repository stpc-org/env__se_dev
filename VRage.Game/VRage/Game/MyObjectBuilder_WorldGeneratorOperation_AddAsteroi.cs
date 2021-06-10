// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab
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
  [XmlType("AddAsteroidPrefab")]
  public class MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab : MyObjectBuilder_WorldGeneratorOperation
  {
    [ProtoMember(124)]
    [XmlAttribute]
    public string PrefabFile;
    [ProtoMember(127)]
    [XmlAttribute]
    public string Name;
    [ProtoMember(130)]
    public SerializableVector3 Position;

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab\u003C\u003EPrefabFile\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab owner,
        in string value)
      {
        owner.PrefabFile = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab owner,
        out string value)
      {
        value = owner.PrefabFile;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab owner,
        in string value)
      {
        owner.Name = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab owner,
        out string value)
      {
        value = owner.Name;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab\u003C\u003EPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab owner,
        in SerializableVector3 value)
      {
        owner.Position = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab owner,
        out SerializableVector3 value)
      {
        value = owner.Position;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab\u003C\u003EFactionTag\u003C\u003EAccessor : MyObjectBuilder_WorldGeneratorOperation.VRage_Game_MyObjectBuilder_WorldGeneratorOperation\u003C\u003EFactionTag\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_WorldGeneratorOperation&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_WorldGeneratorOperation&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab();

      MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab IActivator<MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab>.CreateInstance() => new MyObjectBuilder_WorldGeneratorOperation_AddAsteroidPrefab();
    }
  }
}
