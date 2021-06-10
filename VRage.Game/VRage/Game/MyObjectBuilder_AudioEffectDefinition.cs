// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_AudioEffectDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Data.Audio;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_AudioEffectDefinition : MyObjectBuilder_DefinitionBase
  {
    [XmlArrayItem("Sound")]
    [ProtoMember(22)]
    public List<MyObjectBuilder_AudioEffectDefinition.SoundList> Sounds;
    [ProtoMember(25)]
    [DefaultValue(0)]
    public int OutputSound;

    [ProtoContract]
    public struct SoundList
    {
      [ProtoMember(1)]
      public List<MyObjectBuilder_AudioEffectDefinition.SoundEffect> SoundEffects;

      protected class VRage_Game_MyObjectBuilder_AudioEffectDefinition\u003C\u003ESoundList\u003C\u003ESoundEffects\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioEffectDefinition.SoundList, List<MyObjectBuilder_AudioEffectDefinition.SoundEffect>>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_AudioEffectDefinition.SoundList owner,
          in List<MyObjectBuilder_AudioEffectDefinition.SoundEffect> value)
        {
          owner.SoundEffects = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_AudioEffectDefinition.SoundList owner,
          out List<MyObjectBuilder_AudioEffectDefinition.SoundEffect> value)
        {
          value = owner.SoundEffects;
        }
      }

      private class VRage_Game_MyObjectBuilder_AudioEffectDefinition\u003C\u003ESoundList\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AudioEffectDefinition.SoundList>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_AudioEffectDefinition.SoundList();

        MyObjectBuilder_AudioEffectDefinition.SoundList IActivator<MyObjectBuilder_AudioEffectDefinition.SoundList>.CreateInstance() => new MyObjectBuilder_AudioEffectDefinition.SoundList();
      }
    }

    [ProtoContract]
    public class SoundEffect
    {
      [ProtoMember(4)]
      public string VolumeCurve;
      [ProtoMember(7)]
      public float Duration;
      [ProtoMember(10)]
      [DefaultValue(MyAudioEffect.FilterType.None)]
      public MyAudioEffect.FilterType Filter = MyAudioEffect.FilterType.None;
      [ProtoMember(13)]
      [DefaultValue(1f)]
      public float Frequency = 1f;
      [ProtoMember(16)]
      [DefaultValue(false)]
      public bool StopAfter;
      [ProtoMember(19)]
      [DefaultValue(1f)]
      public float Q = 1f;

      protected class VRage_Game_MyObjectBuilder_AudioEffectDefinition\u003C\u003ESoundEffect\u003C\u003EVolumeCurve\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioEffectDefinition.SoundEffect, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_AudioEffectDefinition.SoundEffect owner,
          in string value)
        {
          owner.VolumeCurve = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_AudioEffectDefinition.SoundEffect owner,
          out string value)
        {
          value = owner.VolumeCurve;
        }
      }

      protected class VRage_Game_MyObjectBuilder_AudioEffectDefinition\u003C\u003ESoundEffect\u003C\u003EDuration\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioEffectDefinition.SoundEffect, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_AudioEffectDefinition.SoundEffect owner,
          in float value)
        {
          owner.Duration = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_AudioEffectDefinition.SoundEffect owner,
          out float value)
        {
          value = owner.Duration;
        }
      }

      protected class VRage_Game_MyObjectBuilder_AudioEffectDefinition\u003C\u003ESoundEffect\u003C\u003EFilter\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioEffectDefinition.SoundEffect, MyAudioEffect.FilterType>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_AudioEffectDefinition.SoundEffect owner,
          in MyAudioEffect.FilterType value)
        {
          owner.Filter = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_AudioEffectDefinition.SoundEffect owner,
          out MyAudioEffect.FilterType value)
        {
          value = owner.Filter;
        }
      }

      protected class VRage_Game_MyObjectBuilder_AudioEffectDefinition\u003C\u003ESoundEffect\u003C\u003EFrequency\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioEffectDefinition.SoundEffect, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_AudioEffectDefinition.SoundEffect owner,
          in float value)
        {
          owner.Frequency = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_AudioEffectDefinition.SoundEffect owner,
          out float value)
        {
          value = owner.Frequency;
        }
      }

      protected class VRage_Game_MyObjectBuilder_AudioEffectDefinition\u003C\u003ESoundEffect\u003C\u003EStopAfter\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioEffectDefinition.SoundEffect, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_AudioEffectDefinition.SoundEffect owner,
          in bool value)
        {
          owner.StopAfter = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_AudioEffectDefinition.SoundEffect owner,
          out bool value)
        {
          value = owner.StopAfter;
        }
      }

      protected class VRage_Game_MyObjectBuilder_AudioEffectDefinition\u003C\u003ESoundEffect\u003C\u003EQ\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioEffectDefinition.SoundEffect, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_AudioEffectDefinition.SoundEffect owner,
          in float value)
        {
          owner.Q = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_AudioEffectDefinition.SoundEffect owner,
          out float value)
        {
          value = owner.Q;
        }
      }

      private class VRage_Game_MyObjectBuilder_AudioEffectDefinition\u003C\u003ESoundEffect\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AudioEffectDefinition.SoundEffect>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_AudioEffectDefinition.SoundEffect();

        MyObjectBuilder_AudioEffectDefinition.SoundEffect IActivator<MyObjectBuilder_AudioEffectDefinition.SoundEffect>.CreateInstance() => new MyObjectBuilder_AudioEffectDefinition.SoundEffect();
      }
    }

    protected class VRage_Game_MyObjectBuilder_AudioEffectDefinition\u003C\u003ESounds\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioEffectDefinition, List<MyObjectBuilder_AudioEffectDefinition.SoundList>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AudioEffectDefinition owner,
        in List<MyObjectBuilder_AudioEffectDefinition.SoundList> value)
      {
        owner.Sounds = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AudioEffectDefinition owner,
        out List<MyObjectBuilder_AudioEffectDefinition.SoundList> value)
      {
        value = owner.Sounds;
      }
    }

    protected class VRage_Game_MyObjectBuilder_AudioEffectDefinition\u003C\u003EOutputSound\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AudioEffectDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioEffectDefinition owner, in int value) => owner.OutputSound = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioEffectDefinition owner, out int value) => value = owner.OutputSound;
    }

    protected class VRage_Game_MyObjectBuilder_AudioEffectDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AudioEffectDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AudioEffectDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AudioEffectDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_AudioEffectDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AudioEffectDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioEffectDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioEffectDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AudioEffectDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AudioEffectDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioEffectDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioEffectDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AudioEffectDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AudioEffectDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioEffectDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioEffectDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AudioEffectDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AudioEffectDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioEffectDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioEffectDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AudioEffectDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AudioEffectDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioEffectDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioEffectDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AudioEffectDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AudioEffectDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioEffectDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioEffectDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AudioEffectDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AudioEffectDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioEffectDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioEffectDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AudioEffectDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AudioEffectDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioEffectDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioEffectDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AudioEffectDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AudioEffectDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AudioEffectDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AudioEffectDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_AudioEffectDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AudioEffectDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioEffectDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioEffectDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_AudioEffectDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AudioEffectDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AudioEffectDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AudioEffectDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_AudioEffectDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AudioEffectDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AudioEffectDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AudioEffectDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_AudioEffectDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AudioEffectDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_AudioEffectDefinition();

      MyObjectBuilder_AudioEffectDefinition IActivator<MyObjectBuilder_AudioEffectDefinition>.CreateInstance() => new MyObjectBuilder_AudioEffectDefinition();
    }
  }
}
