// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_DebrisDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Data;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_DebrisDefinition : MyObjectBuilder_DefinitionBase
  {
    [ProtoMember(1)]
    [ModdableContentFile("mwm")]
    public string Model;
    [ProtoMember(4)]
    public MyDebrisType Type;
    [ProtoMember(7)]
    public float MinAmount;

    protected class VRage_Game_MyObjectBuilder_DebrisDefinition\u003C\u003EModel\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DebrisDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebrisDefinition owner, in string value) => owner.Model = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebrisDefinition owner, out string value) => value = owner.Model;
    }

    protected class VRage_Game_MyObjectBuilder_DebrisDefinition\u003C\u003EType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DebrisDefinition, MyDebrisType>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebrisDefinition owner, in MyDebrisType value) => owner.Type = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebrisDefinition owner, out MyDebrisType value) => value = owner.Type;
    }

    protected class VRage_Game_MyObjectBuilder_DebrisDefinition\u003C\u003EMinAmount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_DebrisDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebrisDefinition owner, in float value) => owner.MinAmount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebrisDefinition owner, out float value) => value = owner.MinAmount;
    }

    protected class VRage_Game_MyObjectBuilder_DebrisDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebrisDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_DebrisDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_DebrisDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_DebrisDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebrisDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebrisDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebrisDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DebrisDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebrisDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebrisDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebrisDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DebrisDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebrisDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebrisDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebrisDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DebrisDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebrisDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebrisDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebrisDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DebrisDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebrisDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebrisDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebrisDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DebrisDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebrisDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebrisDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebrisDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DebrisDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebrisDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebrisDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebrisDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DebrisDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebrisDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebrisDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebrisDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DebrisDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebrisDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebrisDefinition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebrisDefinition owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DebrisDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebrisDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebrisDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebrisDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DebrisDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebrisDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebrisDefinition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebrisDefinition owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DebrisDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebrisDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebrisDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebrisDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_DebrisDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_DebrisDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_DebrisDefinition();

      MyObjectBuilder_DebrisDefinition IActivator<MyObjectBuilder_DebrisDefinition>.CreateInstance() => new MyObjectBuilder_DebrisDefinition();
    }
  }
}
