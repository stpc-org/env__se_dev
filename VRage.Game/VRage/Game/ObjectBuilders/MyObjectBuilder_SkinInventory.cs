// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.MyObjectBuilder_SkinInventory
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders
{
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_SkinInventory : MyObjectBuilder_Base
  {
    public List<ulong> Character;
    public List<ulong> Tools;
    public SerializableVector3 Color;
    public string Model;

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_SkinInventory\u003C\u003ECharacter\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SkinInventory, List<ulong>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SkinInventory owner, in List<ulong> value) => owner.Character = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SkinInventory owner, out List<ulong> value) => value = owner.Character;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_SkinInventory\u003C\u003ETools\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SkinInventory, List<ulong>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SkinInventory owner, in List<ulong> value) => owner.Tools = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SkinInventory owner, out List<ulong> value) => value = owner.Tools;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_SkinInventory\u003C\u003EColor\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SkinInventory, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SkinInventory owner, in SerializableVector3 value) => owner.Color = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SkinInventory owner,
        out SerializableVector3 value)
      {
        value = owner.Color;
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_SkinInventory\u003C\u003EModel\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SkinInventory, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SkinInventory owner, in string value) => owner.Model = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SkinInventory owner, out string value) => value = owner.Model;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_SkinInventory\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SkinInventory, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SkinInventory owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SkinInventory owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_SkinInventory\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SkinInventory, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SkinInventory owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SkinInventory owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_SkinInventory\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SkinInventory, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SkinInventory owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SkinInventory owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_SkinInventory\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SkinInventory, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SkinInventory owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SkinInventory owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_MyObjectBuilder_SkinInventory\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_SkinInventory>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_SkinInventory();

      MyObjectBuilder_SkinInventory IActivator<MyObjectBuilder_SkinInventory>.CreateInstance() => new MyObjectBuilder_SkinInventory();
    }
  }
}
