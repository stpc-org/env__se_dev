// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_RespawnShipDefinition
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

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_RespawnShipDefinition : MyObjectBuilder_DefinitionBase
  {
    [ProtoMember(1)]
    public string Prefab;
    [ProtoMember(4)]
    public int CooldownSeconds;
    [ProtoMember(7, IsRequired = false)]
    public bool SpawnWithDefaultItems = true;
    [ProtoMember(10, IsRequired = false)]
    public bool UseForSpace;
    [ProtoMember(13, IsRequired = false)]
    public float MinimalAirDensity = 0.7f;
    [ProtoMember(16, IsRequired = false)]
    public bool UseForPlanetsWithAtmosphere;
    [ProtoMember(19, IsRequired = false)]
    public bool UseForPlanetsWithoutAtmosphere;
    [ProtoMember(22, IsRequired = false)]
    public float? PlanetDeployAltitude = new float?(500f);
    [ProtoMember(25, IsRequired = false)]
    public SerializableVector3 InitialLinearVelocity = (SerializableVector3) Vector3.Zero;
    [ProtoMember(28, IsRequired = false)]
    public SerializableVector3 InitialAngularVelocity = (SerializableVector3) Vector3.Zero;
    [ProtoMember(31)]
    public string HelpTextLocalizationId;
    [ProtoMember(33)]
    public bool SpawnNearProceduralAsteroids = true;
    [ProtoMember(35)]
    [XmlArrayItem("PlanetType")]
    public string[] PlanetTypes;
    [ProtoMember(37)]
    public SerializableVector3D? SpawnPosition;
    [ProtoMember(39)]
    public float SpawnPositionDispersionMin;
    [ProtoMember(41)]
    public float SpawnPositionDispersionMax = 10000f;

    protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition\u003C\u003EPrefab\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in string value) => owner.Prefab = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out string value) => value = owner.Prefab;
    }

    protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition\u003C\u003ECooldownSeconds\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in int value) => owner.CooldownSeconds = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out int value) => value = owner.CooldownSeconds;
    }

    protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition\u003C\u003ESpawnWithDefaultItems\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in bool value) => owner.SpawnWithDefaultItems = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out bool value) => value = owner.SpawnWithDefaultItems;
    }

    protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition\u003C\u003EUseForSpace\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in bool value) => owner.UseForSpace = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out bool value) => value = owner.UseForSpace;
    }

    protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition\u003C\u003EMinimalAirDensity\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in float value) => owner.MinimalAirDensity = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out float value) => value = owner.MinimalAirDensity;
    }

    protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition\u003C\u003EUseForPlanetsWithAtmosphere\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in bool value) => owner.UseForPlanetsWithAtmosphere = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out bool value) => value = owner.UseForPlanetsWithAtmosphere;
    }

    protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition\u003C\u003EUseForPlanetsWithoutAtmosphere\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in bool value) => owner.UseForPlanetsWithoutAtmosphere = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out bool value) => value = owner.UseForPlanetsWithoutAtmosphere;
    }

    protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition\u003C\u003EPlanetDeployAltitude\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, float?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in float? value) => owner.PlanetDeployAltitude = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out float? value) => value = owner.PlanetDeployAltitude;
    }

    protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition\u003C\u003EInitialLinearVelocity\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_RespawnShipDefinition owner,
        in SerializableVector3 value)
      {
        owner.InitialLinearVelocity = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_RespawnShipDefinition owner,
        out SerializableVector3 value)
      {
        value = owner.InitialLinearVelocity;
      }
    }

    protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition\u003C\u003EInitialAngularVelocity\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_RespawnShipDefinition owner,
        in SerializableVector3 value)
      {
        owner.InitialAngularVelocity = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_RespawnShipDefinition owner,
        out SerializableVector3 value)
      {
        value = owner.InitialAngularVelocity;
      }
    }

    protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition\u003C\u003EHelpTextLocalizationId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in string value) => owner.HelpTextLocalizationId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out string value) => value = owner.HelpTextLocalizationId;
    }

    protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition\u003C\u003ESpawnNearProceduralAsteroids\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in bool value) => owner.SpawnNearProceduralAsteroids = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out bool value) => value = owner.SpawnNearProceduralAsteroids;
    }

    protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition\u003C\u003EPlanetTypes\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in string[] value) => owner.PlanetTypes = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out string[] value) => value = owner.PlanetTypes;
    }

    protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition\u003C\u003ESpawnPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, SerializableVector3D?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_RespawnShipDefinition owner,
        in SerializableVector3D? value)
      {
        owner.SpawnPosition = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_RespawnShipDefinition owner,
        out SerializableVector3D? value)
      {
        value = owner.SpawnPosition;
      }
    }

    protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition\u003C\u003ESpawnPositionDispersionMin\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in float value) => owner.SpawnPositionDispersionMin = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out float value) => value = owner.SpawnPositionDispersionMin;
    }

    protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition\u003C\u003ESpawnPositionDispersionMax\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in float value) => owner.SpawnPositionDispersionMax = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out float value) => value = owner.SpawnPositionDispersionMax;
    }

    protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_RespawnShipDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_RespawnShipDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_RespawnShipDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_RespawnShipDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_RespawnShipDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_RespawnShipDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_RespawnShipDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RespawnShipDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RespawnShipDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RespawnShipDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_RespawnShipDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_RespawnShipDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_RespawnShipDefinition();

      MyObjectBuilder_RespawnShipDefinition IActivator<MyObjectBuilder_RespawnShipDefinition>.CreateInstance() => new MyObjectBuilder_RespawnShipDefinition();
    }
  }
}
