// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_ToolbarItemWeapon
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
  public class MyObjectBuilder_ToolbarItemWeapon : MyObjectBuilder_ToolbarItemDefinition
  {
    public SerializableDefinitionId defId
    {
      get => this.DefinitionId;
      set => this.DefinitionId = value;
    }

    public bool ShouldSerializedefId() => false;

    protected class VRage_Game_MyObjectBuilder_ToolbarItemWeapon\u003C\u003EDefinitionId\u003C\u003EAccessor : MyObjectBuilder_ToolbarItemDefinition.VRage_Game_MyObjectBuilder_ToolbarItemDefinition\u003C\u003EDefinitionId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolbarItemWeapon, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ToolbarItemWeapon owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ToolbarItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ToolbarItemWeapon owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ToolbarItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ToolbarItemWeapon\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolbarItemWeapon, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolbarItemWeapon owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolbarItemWeapon owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolbarItemWeapon\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolbarItemWeapon, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolbarItemWeapon owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolbarItemWeapon owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolbarItemWeapon\u003C\u003EdefId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolbarItemWeapon, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ToolbarItemWeapon owner,
        in SerializableDefinitionId value)
      {
        owner.defId = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ToolbarItemWeapon owner,
        out SerializableDefinitionId value)
      {
        value = owner.defId;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ToolbarItemWeapon\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolbarItemWeapon, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolbarItemWeapon owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolbarItemWeapon owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ToolbarItemWeapon\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolbarItemWeapon, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ToolbarItemWeapon owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ToolbarItemWeapon owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_ToolbarItemWeapon\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ToolbarItemWeapon>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ToolbarItemWeapon();

      MyObjectBuilder_ToolbarItemWeapon IActivator<MyObjectBuilder_ToolbarItemWeapon>.CreateInstance() => new MyObjectBuilder_ToolbarItemWeapon();
    }
  }
}
