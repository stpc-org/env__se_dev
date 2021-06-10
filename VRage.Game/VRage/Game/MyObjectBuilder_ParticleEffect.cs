// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_ParticleEffect
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Render.Particles;
using VRage.Utils;

namespace VRage.Game
{
  [ProtoContract]
  [XmlType("ParticleEffect")]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public sealed class MyObjectBuilder_ParticleEffect : MyObjectBuilder_DefinitionBase
  {
    [ProtoMember(1)]
    public int ParticleId;
    [ProtoMember(4)]
    public float Length = 10f;
    [ProtoMember(7)]
    public float Preload;
    [ProtoMember(10)]
    public bool LowRes;
    [ProtoMember(13)]
    public bool Loop;
    [ProtoMember(16)]
    public float DurationMin;
    [ProtoMember(19)]
    public float DurationMax;
    [ProtoMember(22)]
    public int Version;
    [ProtoMember(25)]
    public List<ParticleGeneration> ParticleGenerations;
    [ProtoMember(28)]
    public List<ParticleLight> ParticleLights;
    [ProtoMember(34)]
    public float DistanceMax;
    [ProtoMember(38)]
    public float Priority = 1f;

    protected class VRage_Game_MyObjectBuilder_ParticleEffect\u003C\u003EParticleId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ParticleEffect, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ParticleEffect owner, in int value) => owner.ParticleId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ParticleEffect owner, out int value) => value = owner.ParticleId;
    }

    protected class VRage_Game_MyObjectBuilder_ParticleEffect\u003C\u003ELength\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ParticleEffect, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ParticleEffect owner, in float value) => owner.Length = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ParticleEffect owner, out float value) => value = owner.Length;
    }

    protected class VRage_Game_MyObjectBuilder_ParticleEffect\u003C\u003EPreload\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ParticleEffect, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ParticleEffect owner, in float value) => owner.Preload = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ParticleEffect owner, out float value) => value = owner.Preload;
    }

    protected class VRage_Game_MyObjectBuilder_ParticleEffect\u003C\u003ELowRes\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ParticleEffect, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ParticleEffect owner, in bool value) => owner.LowRes = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ParticleEffect owner, out bool value) => value = owner.LowRes;
    }

    protected class VRage_Game_MyObjectBuilder_ParticleEffect\u003C\u003ELoop\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ParticleEffect, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ParticleEffect owner, in bool value) => owner.Loop = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ParticleEffect owner, out bool value) => value = owner.Loop;
    }

    protected class VRage_Game_MyObjectBuilder_ParticleEffect\u003C\u003EDurationMin\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ParticleEffect, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ParticleEffect owner, in float value) => owner.DurationMin = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ParticleEffect owner, out float value) => value = owner.DurationMin;
    }

    protected class VRage_Game_MyObjectBuilder_ParticleEffect\u003C\u003EDurationMax\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ParticleEffect, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ParticleEffect owner, in float value) => owner.DurationMax = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ParticleEffect owner, out float value) => value = owner.DurationMax;
    }

    protected class VRage_Game_MyObjectBuilder_ParticleEffect\u003C\u003EVersion\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ParticleEffect, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ParticleEffect owner, in int value) => owner.Version = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ParticleEffect owner, out int value) => value = owner.Version;
    }

    protected class VRage_Game_MyObjectBuilder_ParticleEffect\u003C\u003EParticleGenerations\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ParticleEffect, List<ParticleGeneration>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ParticleEffect owner,
        in List<ParticleGeneration> value)
      {
        owner.ParticleGenerations = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ParticleEffect owner,
        out List<ParticleGeneration> value)
      {
        value = owner.ParticleGenerations;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ParticleEffect\u003C\u003EParticleLights\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ParticleEffect, List<ParticleLight>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ParticleEffect owner,
        in List<ParticleLight> value)
      {
        owner.ParticleLights = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ParticleEffect owner,
        out List<ParticleLight> value)
      {
        value = owner.ParticleLights;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ParticleEffect\u003C\u003EDistanceMax\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ParticleEffect, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ParticleEffect owner, in float value) => owner.DistanceMax = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ParticleEffect owner, out float value) => value = owner.DistanceMax;
    }

    protected class VRage_Game_MyObjectBuilder_ParticleEffect\u003C\u003EPriority\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ParticleEffect, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ParticleEffect owner, in float value) => owner.Priority = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ParticleEffect owner, out float value) => value = owner.Priority;
    }

    protected class VRage_Game_MyObjectBuilder_ParticleEffect\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ParticleEffect, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ParticleEffect owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ParticleEffect owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ParticleEffect\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ParticleEffect, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ParticleEffect owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ParticleEffect owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ParticleEffect\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ParticleEffect, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ParticleEffect owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ParticleEffect owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ParticleEffect\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ParticleEffect, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ParticleEffect owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ParticleEffect owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ParticleEffect\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ParticleEffect, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ParticleEffect owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ParticleEffect owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ParticleEffect\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ParticleEffect, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ParticleEffect owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ParticleEffect owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ParticleEffect\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ParticleEffect, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ParticleEffect owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ParticleEffect owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ParticleEffect\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ParticleEffect, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ParticleEffect owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ParticleEffect owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ParticleEffect\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ParticleEffect, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ParticleEffect owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ParticleEffect owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ParticleEffect\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ParticleEffect, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ParticleEffect owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ParticleEffect owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ParticleEffect\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ParticleEffect, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ParticleEffect owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ParticleEffect owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ParticleEffect\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ParticleEffect, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ParticleEffect owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ParticleEffect owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ParticleEffect\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ParticleEffect, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ParticleEffect owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ParticleEffect owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_ParticleEffect\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ParticleEffect>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ParticleEffect();

      MyObjectBuilder_ParticleEffect IActivator<MyObjectBuilder_ParticleEffect>.CreateInstance() => new MyObjectBuilder_ParticleEffect();
    }
  }
}
