// Decompiled with JetBrains decompiler
// Type: Medieval.ObjectBuilders.MyObjectBuilder_WorldGeneratorOperation_WorldFromSave
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Medieval.ObjectBuilders
{
  [MyObjectBuilderDefinition(null, null)]
  [XmlType("WorldFromSave")]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_WorldGeneratorOperation_WorldFromSave : MyObjectBuilder_WorldGeneratorOperation
  {
    [ProtoMember(13)]
    [XmlAttribute]
    public string PrefabDirectory;

    protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_WorldFromSave\u003C\u003EPrefabDirectory\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_WorldFromSave, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_WorldFromSave owner,
        in string value)
      {
        owner.PrefabDirectory = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_WorldFromSave owner,
        out string value)
      {
        value = owner.PrefabDirectory;
      }
    }

    protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_WorldFromSave\u003C\u003EFactionTag\u003C\u003EAccessor : MyObjectBuilder_WorldGeneratorOperation.VRage_Game_MyObjectBuilder_WorldGeneratorOperation\u003C\u003EFactionTag\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_WorldFromSave, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_WorldFromSave owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_WorldGeneratorOperation&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_WorldFromSave owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_WorldGeneratorOperation&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_WorldFromSave\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_WorldFromSave, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_WorldFromSave owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_WorldFromSave owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_WorldFromSave\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_WorldFromSave, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_WorldFromSave owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_WorldFromSave owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_WorldFromSave\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_WorldFromSave, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_WorldFromSave owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_WorldFromSave owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_WorldFromSave\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_WorldFromSave, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorOperation_WorldFromSave owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorOperation_WorldFromSave owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_WorldFromSave\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_WorldGeneratorOperation_WorldFromSave>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_WorldGeneratorOperation_WorldFromSave();

      MyObjectBuilder_WorldGeneratorOperation_WorldFromSave IActivator<MyObjectBuilder_WorldGeneratorOperation_WorldFromSave>.CreateInstance() => new MyObjectBuilder_WorldGeneratorOperation_WorldFromSave();
    }
  }
}
