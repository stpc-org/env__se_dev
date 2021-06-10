// Decompiled with JetBrains decompiler
// Type: Medieval.ObjectBuilders.MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage;
using VRage.Game;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Medieval.ObjectBuilders
{
  [MyObjectBuilderDefinition(null, null)]
  [XmlType("WorldFromMaps")]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps : MyObjectBuilder_WorldGeneratorOperation
  {
    [ProtoMember(16)]
    [XmlAttribute]
    public string Name;
    [ProtoMember(19)]
    public SerializableVector3 Size;
    [ProtoMember(22)]
    public string HeightMapFile;
    [ProtoMember(25)]
    public string BiomeMapFile;
    [ProtoMember(28)]
    public string TreeMapFile;
    [ProtoMember(31)]
    public string TreeMaskFile;

    protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner,
        in string value)
      {
        owner.Name = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner,
        out string value)
      {
        value = owner.Name;
      }
    }

    protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps\u003C\u003ESize\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner,
        in SerializableVector3 value)
      {
        owner.Size = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner,
        out SerializableVector3 value)
      {
        value = owner.Size;
      }
    }

    protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps\u003C\u003EHeightMapFile\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner,
        in string value)
      {
        owner.HeightMapFile = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner,
        out string value)
      {
        value = owner.HeightMapFile;
      }
    }

    protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps\u003C\u003EBiomeMapFile\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner,
        in string value)
      {
        owner.BiomeMapFile = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner,
        out string value)
      {
        value = owner.BiomeMapFile;
      }
    }

    protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps\u003C\u003ETreeMapFile\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner,
        in string value)
      {
        owner.TreeMapFile = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner,
        out string value)
      {
        value = owner.TreeMapFile;
      }
    }

    protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps\u003C\u003ETreeMaskFile\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner,
        in string value)
      {
        owner.TreeMaskFile = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner,
        out string value)
      {
        value = owner.TreeMaskFile;
      }
    }

    protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps\u003C\u003EFactionTag\u003C\u003EAccessor : MyObjectBuilder_WorldGeneratorOperation.VRage_Game_MyObjectBuilder_WorldGeneratorOperation\u003C\u003EFactionTag\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_WorldGeneratorOperation&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_WorldGeneratorOperation&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps();

      MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps IActivator<MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps>.CreateInstance() => new MyObjectBuilder_WorldGeneratorOperation_WorldFromMaps();
    }
  }
}
