// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Components.MyObjectBuilder_EntityOwnershipComponent
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Components
{
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_EntityOwnershipComponent : MyObjectBuilder_ComponentBase
  {
    public long OwnerId;
    public MyOwnershipShareModeEnum ShareMode;

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_EntityOwnershipComponent\u003C\u003EOwnerId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityOwnershipComponent, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityOwnershipComponent owner, in long value) => owner.OwnerId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityOwnershipComponent owner, out long value) => value = owner.OwnerId;
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_EntityOwnershipComponent\u003C\u003EShareMode\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityOwnershipComponent, MyOwnershipShareModeEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EntityOwnershipComponent owner,
        in MyOwnershipShareModeEnum value)
      {
        owner.ShareMode = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EntityOwnershipComponent owner,
        out MyOwnershipShareModeEnum value)
      {
        value = owner.ShareMode;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_EntityOwnershipComponent\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EntityOwnershipComponent, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EntityOwnershipComponent owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EntityOwnershipComponent owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_EntityOwnershipComponent\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EntityOwnershipComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityOwnershipComponent owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityOwnershipComponent owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_EntityOwnershipComponent\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EntityOwnershipComponent, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EntityOwnershipComponent owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EntityOwnershipComponent owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_EntityOwnershipComponent\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EntityOwnershipComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityOwnershipComponent owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityOwnershipComponent owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_EntityOwnershipComponent\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_EntityOwnershipComponent>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_EntityOwnershipComponent();

      MyObjectBuilder_EntityOwnershipComponent IActivator<MyObjectBuilder_EntityOwnershipComponent>.CreateInstance() => new MyObjectBuilder_EntityOwnershipComponent();
    }
  }
}
