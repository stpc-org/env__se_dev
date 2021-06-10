// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Components.MyObjectBuilder_ItemAutoswitchComponent
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Components
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_ItemAutoswitchComponent : MyObjectBuilder_SessionComponent
  {
    [ProtoMember(1)]
    public SerializableDefinitionId? AutoswitchTargetDefinition;

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_ItemAutoswitchComponent\u003C\u003EAutoswitchTargetDefinition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ItemAutoswitchComponent, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ItemAutoswitchComponent owner,
        in SerializableDefinitionId? value)
      {
        owner.AutoswitchTargetDefinition = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ItemAutoswitchComponent owner,
        out SerializableDefinitionId? value)
      {
        value = owner.AutoswitchTargetDefinition;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_ItemAutoswitchComponent\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ItemAutoswitchComponent, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ItemAutoswitchComponent owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ItemAutoswitchComponent owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_ItemAutoswitchComponent\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ItemAutoswitchComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ItemAutoswitchComponent owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ItemAutoswitchComponent owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_ItemAutoswitchComponent\u003C\u003EDefinition\u003C\u003EAccessor : MyObjectBuilder_SessionComponent.VRage_Game_MyObjectBuilder_SessionComponent\u003C\u003EDefinition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ItemAutoswitchComponent, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ItemAutoswitchComponent owner,
        in SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_SessionComponent&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ItemAutoswitchComponent owner,
        out SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_SessionComponent&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_ItemAutoswitchComponent\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ItemAutoswitchComponent, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ItemAutoswitchComponent owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ItemAutoswitchComponent owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_ItemAutoswitchComponent\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ItemAutoswitchComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ItemAutoswitchComponent owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ItemAutoswitchComponent owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_ItemAutoswitchComponent\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ItemAutoswitchComponent>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ItemAutoswitchComponent();

      MyObjectBuilder_ItemAutoswitchComponent IActivator<MyObjectBuilder_ItemAutoswitchComponent>.CreateInstance() => new MyObjectBuilder_ItemAutoswitchComponent();
    }
  }
}
