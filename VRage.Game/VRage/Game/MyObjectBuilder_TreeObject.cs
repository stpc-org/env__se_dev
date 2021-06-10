// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_TreeObject
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
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_TreeObject : MyObjectBuilder_PhysicalObject
  {
    public override bool CanStack(MyObjectBuilder_PhysicalObject a) => false;

    protected class VRage_Game_MyObjectBuilder_TreeObject\u003C\u003EFlags\u003C\u003EAccessor : MyObjectBuilder_PhysicalObject.VRage_Game_MyObjectBuilder_PhysicalObject\u003C\u003EFlags\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TreeObject, MyItemFlags>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TreeObject owner, in MyItemFlags value) => this.Set((MyObjectBuilder_PhysicalObject&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TreeObject owner, out MyItemFlags value) => this.Get((MyObjectBuilder_PhysicalObject&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TreeObject\u003C\u003EDurabilityHP\u003C\u003EAccessor : MyObjectBuilder_PhysicalObject.VRage_Game_MyObjectBuilder_PhysicalObject\u003C\u003EDurabilityHP\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TreeObject, float?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TreeObject owner, in float? value) => this.Set((MyObjectBuilder_PhysicalObject&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TreeObject owner, out float? value) => this.Get((MyObjectBuilder_PhysicalObject&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TreeObject\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TreeObject, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TreeObject owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TreeObject owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TreeObject\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TreeObject, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TreeObject owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TreeObject owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TreeObject\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TreeObject, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TreeObject owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TreeObject owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TreeObject\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TreeObject, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TreeObject owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TreeObject owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_TreeObject\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_TreeObject>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_TreeObject();

      MyObjectBuilder_TreeObject IActivator<MyObjectBuilder_TreeObject>.CreateInstance() => new MyObjectBuilder_TreeObject();
    }
  }
}
