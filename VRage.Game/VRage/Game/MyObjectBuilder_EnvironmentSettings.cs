// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_EnvironmentSettings
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
  public class MyObjectBuilder_EnvironmentSettings : MyObjectBuilder_Base
  {
    [ProtoMember(1)]
    public float SunAzimuth;
    [ProtoMember(4)]
    public float SunElevation;
    [ProtoMember(7)]
    public float SunIntensity;
    [ProtoMember(10)]
    public float FogMultiplier;
    [ProtoMember(13)]
    public float FogDensity;
    [ProtoMember(16)]
    public SerializableVector3 FogColor;
    [ProtoMember(19)]
    public SerializableDefinitionId EnvironmentDefinition = new SerializableDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_EnvironmentDefinition), "Default");

    protected class VRage_Game_MyObjectBuilder_EnvironmentSettings\u003C\u003ESunAzimuth\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentSettings owner, in float value) => owner.SunAzimuth = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentSettings owner, out float value) => value = owner.SunAzimuth;
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentSettings\u003C\u003ESunElevation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentSettings owner, in float value) => owner.SunElevation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentSettings owner, out float value) => value = owner.SunElevation;
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentSettings\u003C\u003ESunIntensity\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentSettings owner, in float value) => owner.SunIntensity = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentSettings owner, out float value) => value = owner.SunIntensity;
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentSettings\u003C\u003EFogMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentSettings owner, in float value) => owner.FogMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentSettings owner, out float value) => value = owner.FogMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentSettings\u003C\u003EFogDensity\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentSettings, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentSettings owner, in float value) => owner.FogDensity = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentSettings owner, out float value) => value = owner.FogDensity;
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentSettings\u003C\u003EFogColor\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentSettings, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentSettings owner,
        in SerializableVector3 value)
      {
        owner.FogColor = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentSettings owner,
        out SerializableVector3 value)
      {
        value = owner.FogColor;
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentSettings\u003C\u003EEnvironmentDefinition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentSettings, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentSettings owner,
        in SerializableDefinitionId value)
      {
        owner.EnvironmentDefinition = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentSettings owner,
        out SerializableDefinitionId value)
      {
        value = owner.EnvironmentDefinition;
      }
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentSettings\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentSettings, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentSettings owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentSettings owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentSettings\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentSettings, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentSettings owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentSettings owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentSettings\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentSettings, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentSettings owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentSettings owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_EnvironmentSettings\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentSettings, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EnvironmentSettings owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EnvironmentSettings owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_EnvironmentSettings\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_EnvironmentSettings>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_EnvironmentSettings();

      MyObjectBuilder_EnvironmentSettings IActivator<MyObjectBuilder_EnvironmentSettings>.CreateInstance() => new MyObjectBuilder_EnvironmentSettings();
    }
  }
}
