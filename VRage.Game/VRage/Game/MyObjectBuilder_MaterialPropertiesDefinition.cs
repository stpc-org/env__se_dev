// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_MaterialPropertiesDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
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
  public class MyObjectBuilder_MaterialPropertiesDefinition : MyObjectBuilder_DefinitionBase
  {
    [ProtoMember(22)]
    public List<MyObjectBuilder_MaterialPropertiesDefinition.ContactProperty> ContactProperties;
    [XmlArrayItem("Property")]
    [ProtoMember(25)]
    public List<MyObjectBuilder_MaterialPropertiesDefinition.GeneralProperty> GeneralProperties;
    [ProtoMember(28)]
    public string InheritFrom;

    [ProtoContract]
    public struct ContactProperty
    {
      [ProtoMember(1)]
      public string Type;
      [ProtoMember(4)]
      public string Material;
      [ProtoMember(7)]
      public string SoundCue;
      [ProtoMember(10)]
      public string ParticleEffect;
      [ProtoMember(13)]
      public List<AlternativeImpactSounds> AlternativeImpactSounds;

      protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition\u003C\u003EContactProperty\u003C\u003EType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition.ContactProperty, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_MaterialPropertiesDefinition.ContactProperty owner,
          in string value)
        {
          owner.Type = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_MaterialPropertiesDefinition.ContactProperty owner,
          out string value)
        {
          value = owner.Type;
        }
      }

      protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition\u003C\u003EContactProperty\u003C\u003EMaterial\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition.ContactProperty, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_MaterialPropertiesDefinition.ContactProperty owner,
          in string value)
        {
          owner.Material = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_MaterialPropertiesDefinition.ContactProperty owner,
          out string value)
        {
          value = owner.Material;
        }
      }

      protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition\u003C\u003EContactProperty\u003C\u003ESoundCue\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition.ContactProperty, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_MaterialPropertiesDefinition.ContactProperty owner,
          in string value)
        {
          owner.SoundCue = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_MaterialPropertiesDefinition.ContactProperty owner,
          out string value)
        {
          value = owner.SoundCue;
        }
      }

      protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition\u003C\u003EContactProperty\u003C\u003EParticleEffect\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition.ContactProperty, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_MaterialPropertiesDefinition.ContactProperty owner,
          in string value)
        {
          owner.ParticleEffect = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_MaterialPropertiesDefinition.ContactProperty owner,
          out string value)
        {
          value = owner.ParticleEffect;
        }
      }

      protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition\u003C\u003EContactProperty\u003C\u003EAlternativeImpactSounds\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition.ContactProperty, List<AlternativeImpactSounds>>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_MaterialPropertiesDefinition.ContactProperty owner,
          in List<AlternativeImpactSounds> value)
        {
          owner.AlternativeImpactSounds = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_MaterialPropertiesDefinition.ContactProperty owner,
          out List<AlternativeImpactSounds> value)
        {
          value = owner.AlternativeImpactSounds;
        }
      }

      private class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition\u003C\u003EContactProperty\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_MaterialPropertiesDefinition.ContactProperty>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_MaterialPropertiesDefinition.ContactProperty();

        MyObjectBuilder_MaterialPropertiesDefinition.ContactProperty IActivator<MyObjectBuilder_MaterialPropertiesDefinition.ContactProperty>.CreateInstance() => new MyObjectBuilder_MaterialPropertiesDefinition.ContactProperty();
      }
    }

    [ProtoContract]
    public struct GeneralProperty
    {
      [ProtoMember(16)]
      public string Type;
      [ProtoMember(19)]
      public string SoundCue;

      protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition\u003C\u003EGeneralProperty\u003C\u003EType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition.GeneralProperty, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_MaterialPropertiesDefinition.GeneralProperty owner,
          in string value)
        {
          owner.Type = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_MaterialPropertiesDefinition.GeneralProperty owner,
          out string value)
        {
          value = owner.Type;
        }
      }

      protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition\u003C\u003EGeneralProperty\u003C\u003ESoundCue\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition.GeneralProperty, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_MaterialPropertiesDefinition.GeneralProperty owner,
          in string value)
        {
          owner.SoundCue = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_MaterialPropertiesDefinition.GeneralProperty owner,
          out string value)
        {
          value = owner.SoundCue;
        }
      }

      private class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition\u003C\u003EGeneralProperty\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_MaterialPropertiesDefinition.GeneralProperty>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_MaterialPropertiesDefinition.GeneralProperty();

        MyObjectBuilder_MaterialPropertiesDefinition.GeneralProperty IActivator<MyObjectBuilder_MaterialPropertiesDefinition.GeneralProperty>.CreateInstance() => new MyObjectBuilder_MaterialPropertiesDefinition.GeneralProperty();
      }
    }

    protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition\u003C\u003EContactProperties\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition, List<MyObjectBuilder_MaterialPropertiesDefinition.ContactProperty>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_MaterialPropertiesDefinition owner,
        in List<MyObjectBuilder_MaterialPropertiesDefinition.ContactProperty> value)
      {
        owner.ContactProperties = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_MaterialPropertiesDefinition owner,
        out List<MyObjectBuilder_MaterialPropertiesDefinition.ContactProperty> value)
      {
        value = owner.ContactProperties;
      }
    }

    protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition\u003C\u003EGeneralProperties\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition, List<MyObjectBuilder_MaterialPropertiesDefinition.GeneralProperty>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_MaterialPropertiesDefinition owner,
        in List<MyObjectBuilder_MaterialPropertiesDefinition.GeneralProperty> value)
      {
        owner.GeneralProperties = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_MaterialPropertiesDefinition owner,
        out List<MyObjectBuilder_MaterialPropertiesDefinition.GeneralProperty> value)
      {
        value = owner.GeneralProperties;
      }
    }

    protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition\u003C\u003EInheritFrom\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_MaterialPropertiesDefinition owner,
        in string value)
      {
        owner.InheritFrom = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_MaterialPropertiesDefinition owner,
        out string value)
      {
        value = owner.InheritFrom;
      }
    }

    protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_MaterialPropertiesDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_MaterialPropertiesDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_MaterialPropertiesDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_MaterialPropertiesDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_MaterialPropertiesDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_MaterialPropertiesDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_MaterialPropertiesDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_MaterialPropertiesDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_MaterialPropertiesDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_MaterialPropertiesDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_MaterialPropertiesDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_MaterialPropertiesDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_MaterialPropertiesDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_MaterialPropertiesDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_MaterialPropertiesDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_MaterialPropertiesDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_MaterialPropertiesDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_MaterialPropertiesDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_MaterialPropertiesDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_MaterialPropertiesDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_MaterialPropertiesDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_MaterialPropertiesDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_MaterialPropertiesDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_MaterialPropertiesDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_MaterialPropertiesDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_MaterialPropertiesDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_MaterialPropertiesDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_MyObjectBuilder_MaterialPropertiesDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_MaterialPropertiesDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_MaterialPropertiesDefinition();

      MyObjectBuilder_MaterialPropertiesDefinition IActivator<MyObjectBuilder_MaterialPropertiesDefinition>.CreateInstance() => new MyObjectBuilder_MaterialPropertiesDefinition();
    }
  }
}
