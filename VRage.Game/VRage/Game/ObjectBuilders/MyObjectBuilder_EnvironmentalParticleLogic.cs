// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.MyObjectBuilder_EnvironmentalParticleLogic
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.ObjectBuilders
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_EnvironmentalParticleLogic : MyObjectBuilder_Base
  {
    [ProtoMember(1)]
    public string Material;
    [ProtoMember(4)]
    public Vector4 ParticleColor;
    [ProtoMember(7)]
    public float MaxSpawnDistance;
    [ProtoMember(10)]
    public float DespawnDistance;
    [ProtoMember(13)]
    public float Density;
    [ProtoMember(16)]
    public int MaxLifeTime;
    [ProtoMember(19)]
    public int MaxParticles;
    [ProtoMember(22)]
    public string MaterialPlanet;
    [ProtoMember(25)]
    public Vector4 ParticleColorPlanet;

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic\u003C\u003EMaterial\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogic, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentalParticleLogic owner,
        in string value)
      {
        owner.Material = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentalParticleLogic owner,
        out string value)
      {
        value = owner.Material;
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic\u003C\u003EParticleColor\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogic, Vector4>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentalParticleLogic owner,
        in Vector4 value)
      {
        owner.ParticleColor = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentalParticleLogic owner,
        out Vector4 value)
      {
        value = owner.ParticleColor;
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic\u003C\u003EMaxSpawnDistance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogic, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentalParticleLogic owner,
        in float value)
      {
        owner.MaxSpawnDistance = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentalParticleLogic owner,
        out float value)
      {
        value = owner.MaxSpawnDistance;
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic\u003C\u003EDespawnDistance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogic, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentalParticleLogic owner,
        in float value)
      {
        owner.DespawnDistance = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentalParticleLogic owner,
        out float value)
      {
        value = owner.DespawnDistance;
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic\u003C\u003EDensity\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogic, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentalParticleLogic owner,
        in float value)
      {
        owner.Density = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentalParticleLogic owner,
        out float value)
      {
        value = owner.Density;
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic\u003C\u003EMaxLifeTime\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogic, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentalParticleLogic owner,
        in int value)
      {
        owner.MaxLifeTime = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentalParticleLogic owner,
        out int value)
      {
        value = owner.MaxLifeTime;
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic\u003C\u003EMaxParticles\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogic, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentalParticleLogic owner,
        in int value)
      {
        owner.MaxParticles = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentalParticleLogic owner,
        out int value)
      {
        value = owner.MaxParticles;
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic\u003C\u003EMaterialPlanet\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogic, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentalParticleLogic owner,
        in string value)
      {
        owner.MaterialPlanet = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentalParticleLogic owner,
        out string value)
      {
        value = owner.MaterialPlanet;
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic\u003C\u003EParticleColorPlanet\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogic, Vector4>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentalParticleLogic owner,
        in Vector4 value)
      {
        owner.ParticleColorPlanet = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentalParticleLogic owner,
        out Vector4 value)
      {
        value = owner.ParticleColorPlanet;
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogic, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentalParticleLogic owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentalParticleLogic owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogic, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentalParticleLogic owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentalParticleLogic owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogic, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentalParticleLogic owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentalParticleLogic owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentalParticleLogic, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EnvironmentalParticleLogic owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EnvironmentalParticleLogic owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_ObjectBuilders_MyObjectBuilder_EnvironmentalParticleLogic\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_EnvironmentalParticleLogic>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_EnvironmentalParticleLogic();

      MyObjectBuilder_EnvironmentalParticleLogic IActivator<MyObjectBuilder_EnvironmentalParticleLogic>.CreateInstance() => new MyObjectBuilder_EnvironmentalParticleLogic();
    }
  }
}
