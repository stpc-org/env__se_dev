// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_AnimationDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.ComponentModel;
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
  public class MyObjectBuilder_AnimationDefinition : MyObjectBuilder_DefinitionBase
  {
    [ProtoMember(16)]
    [ModdableContentFile("mwm")]
    public string AnimationModel;
    [ProtoMember(19)]
    [ModdableContentFile("mwm")]
    public string AnimationModelFPS;
    [ProtoMember(22)]
    public int ClipIndex;
    [ProtoMember(25)]
    public string InfluenceArea;
    [ProtoMember(28)]
    public bool AllowInCockpit = true;
    [ProtoMember(31)]
    public bool AllowWithWeapon;
    [ProtoMember(34)]
    public string SupportedSkeletons = "Humanoid";
    [ProtoMember(37)]
    public bool Loop;
    [ProtoMember(40)]
    public SerializableDefinitionId LeftHandItem;
    [ProtoMember(43)]
    [DefaultValue(null)]
    [XmlArrayItem("AnimationSet")]
    public AnimationSet[] AnimationSets;
    [ProtoMember(50)]
    public string ChatCommand;
    [ProtoMember(51)]
    public string ChatCommandName;
    [ProtoMember(52)]
    public string ChatCommandDescription;
    [ProtoMember(53)]
    public int Priority;

    protected class VRage_Game_MyObjectBuilder_AnimationDefinition\u003C\u003EAnimationModel\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationDefinition owner, in string value) => owner.AnimationModel = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationDefinition owner, out string value) => value = owner.AnimationModel;
    }

    protected class VRage_Game_MyObjectBuilder_AnimationDefinition\u003C\u003EAnimationModelFPS\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationDefinition owner, in string value) => owner.AnimationModelFPS = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationDefinition owner, out string value) => value = owner.AnimationModelFPS;
    }

    protected class VRage_Game_MyObjectBuilder_AnimationDefinition\u003C\u003EClipIndex\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationDefinition owner, in int value) => owner.ClipIndex = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationDefinition owner, out int value) => value = owner.ClipIndex;
    }

    protected class VRage_Game_MyObjectBuilder_AnimationDefinition\u003C\u003EInfluenceArea\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationDefinition owner, in string value) => owner.InfluenceArea = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationDefinition owner, out string value) => value = owner.InfluenceArea;
    }

    protected class VRage_Game_MyObjectBuilder_AnimationDefinition\u003C\u003EAllowInCockpit\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationDefinition owner, in bool value) => owner.AllowInCockpit = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationDefinition owner, out bool value) => value = owner.AllowInCockpit;
    }

    protected class VRage_Game_MyObjectBuilder_AnimationDefinition\u003C\u003EAllowWithWeapon\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationDefinition owner, in bool value) => owner.AllowWithWeapon = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationDefinition owner, out bool value) => value = owner.AllowWithWeapon;
    }

    protected class VRage_Game_MyObjectBuilder_AnimationDefinition\u003C\u003ESupportedSkeletons\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationDefinition owner, in string value) => owner.SupportedSkeletons = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationDefinition owner, out string value) => value = owner.SupportedSkeletons;
    }

    protected class VRage_Game_MyObjectBuilder_AnimationDefinition\u003C\u003ELoop\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationDefinition owner, in bool value) => owner.Loop = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationDefinition owner, out bool value) => value = owner.Loop;
    }

    protected class VRage_Game_MyObjectBuilder_AnimationDefinition\u003C\u003ELeftHandItem\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationDefinition owner,
        in SerializableDefinitionId value)
      {
        owner.LeftHandItem = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationDefinition owner,
        out SerializableDefinitionId value)
      {
        value = owner.LeftHandItem;
      }
    }

    protected class VRage_Game_MyObjectBuilder_AnimationDefinition\u003C\u003EAnimationSets\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationDefinition, AnimationSet[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationDefinition owner,
        in AnimationSet[] value)
      {
        owner.AnimationSets = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationDefinition owner,
        out AnimationSet[] value)
      {
        value = owner.AnimationSets;
      }
    }

    protected class VRage_Game_MyObjectBuilder_AnimationDefinition\u003C\u003EChatCommand\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationDefinition owner, in string value) => owner.ChatCommand = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationDefinition owner, out string value) => value = owner.ChatCommand;
    }

    protected class VRage_Game_MyObjectBuilder_AnimationDefinition\u003C\u003EChatCommandName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationDefinition owner, in string value) => owner.ChatCommandName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationDefinition owner, out string value) => value = owner.ChatCommandName;
    }

    protected class VRage_Game_MyObjectBuilder_AnimationDefinition\u003C\u003EChatCommandDescription\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationDefinition owner, in string value) => owner.ChatCommandDescription = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationDefinition owner, out string value) => value = owner.ChatCommandDescription;
    }

    protected class VRage_Game_MyObjectBuilder_AnimationDefinition\u003C\u003EPriority\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationDefinition owner, in int value) => owner.Priority = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationDefinition owner, out int value) => value = owner.Priority;
    }

    protected class VRage_Game_MyObjectBuilder_AnimationDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_AnimationDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AnimationDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AnimationDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AnimationDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AnimationDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AnimationDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AnimationDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AnimationDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AnimationDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationDefinition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationDefinition owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AnimationDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AnimationDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationDefinition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationDefinition owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AnimationDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_AnimationDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AnimationDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_AnimationDefinition();

      MyObjectBuilder_AnimationDefinition IActivator<MyObjectBuilder_AnimationDefinition>.CreateInstance() => new MyObjectBuilder_AnimationDefinition();
    }
  }
}
