// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_AmmoMagazine
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
  public class MyObjectBuilder_AmmoMagazine : MyObjectBuilder_PhysicalObject
  {
    [ProtoMember(1)]
    public int ProjectilesCount;

    protected class VRage_Game_MyObjectBuilder_AmmoMagazine\u003C\u003EProjectilesCount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AmmoMagazine, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazine owner, in int value) => owner.ProjectilesCount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazine owner, out int value) => value = owner.ProjectilesCount;
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazine\u003C\u003EFlags\u003C\u003EAccessor : MyObjectBuilder_PhysicalObject.VRage_Game_MyObjectBuilder_PhysicalObject\u003C\u003EFlags\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazine, MyItemFlags>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazine owner, in MyItemFlags value) => this.Set((MyObjectBuilder_PhysicalObject&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazine owner, out MyItemFlags value) => this.Get((MyObjectBuilder_PhysicalObject&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazine\u003C\u003EDurabilityHP\u003C\u003EAccessor : MyObjectBuilder_PhysicalObject.VRage_Game_MyObjectBuilder_PhysicalObject\u003C\u003EDurabilityHP\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazine, float?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazine owner, in float? value) => this.Set((MyObjectBuilder_PhysicalObject&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazine owner, out float? value) => this.Get((MyObjectBuilder_PhysicalObject&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazine\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazine, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazine owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazine owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazine\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazine, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazine owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazine owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazine\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazine, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazine owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazine owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AmmoMagazine\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AmmoMagazine, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AmmoMagazine owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AmmoMagazine owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_AmmoMagazine\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AmmoMagazine>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_AmmoMagazine();

      MyObjectBuilder_AmmoMagazine IActivator<MyObjectBuilder_AmmoMagazine>.CreateInstance() => new MyObjectBuilder_AmmoMagazine();
    }
  }
}
