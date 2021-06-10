// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.ComponentSystem.MyObjectBuilder_ContainerDropComponent
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.ComponentSystem
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_ContainerDropComponent : MyObjectBuilder_ComponentBase
  {
    [ProtoMember(1, IsRequired = false)]
    public bool Competetive;
    [ProtoMember(4, IsRequired = false)]
    public string GPSName = "";
    [ProtoMember(7, IsRequired = false)]
    public long Owner;
    [ProtoMember(10, IsRequired = false)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public string PlayingSound = "";

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_ContainerDropComponent\u003C\u003ECompetetive\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerDropComponent, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContainerDropComponent owner, in bool value) => owner.Competetive = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContainerDropComponent owner, out bool value) => value = owner.Competetive;
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_ContainerDropComponent\u003C\u003EGPSName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerDropComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContainerDropComponent owner, in string value) => owner.GPSName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContainerDropComponent owner, out string value) => value = owner.GPSName;
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_ContainerDropComponent\u003C\u003EOwner\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerDropComponent, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContainerDropComponent owner, in long value) => owner.Owner = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContainerDropComponent owner, out long value) => value = owner.Owner;
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_ContainerDropComponent\u003C\u003EPlayingSound\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerDropComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContainerDropComponent owner, in string value) => owner.PlayingSound = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContainerDropComponent owner, out string value) => value = owner.PlayingSound;
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_ContainerDropComponent\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerDropComponent, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContainerDropComponent owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContainerDropComponent owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_ContainerDropComponent\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerDropComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContainerDropComponent owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContainerDropComponent owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_ContainerDropComponent\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerDropComponent, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContainerDropComponent owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContainerDropComponent owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_ContainerDropComponent\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerDropComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContainerDropComponent owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContainerDropComponent owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_ContainerDropComponent\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ContainerDropComponent>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ContainerDropComponent();

      MyObjectBuilder_ContainerDropComponent IActivator<MyObjectBuilder_ContainerDropComponent>.CreateInstance() => new MyObjectBuilder_ContainerDropComponent();
    }
  }
}
