// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_VoxelMaterialDefinition
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
  [XmlType("VoxelMaterial")]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_VoxelMaterialDefinition : MyObjectBuilder_DefinitionBase
  {
    [ProtoMember(1)]
    public string MaterialTypeName = "Rock";
    [ProtoMember(4)]
    public string MinedOre;
    [ProtoMember(7)]
    public float MinedOreRatio;
    [ProtoMember(10)]
    public bool CanBeHarvested;
    [ProtoMember(13)]
    public bool IsRare;
    [ProtoMember(16)]
    public bool UseTwoTextures;
    [ProtoMember(19)]
    [ModdableContentFile("dds")]
    public string VoxelHandPreview;
    [ProtoMember(22)]
    public int MinVersion;
    [ProtoMember(25)]
    public int MaxVersion = int.MaxValue;
    [ProtoMember(28)]
    public bool SpawnsInAsteroids = true;
    [ProtoMember(31)]
    public bool SpawnsFromMeteorites = true;
    public string DamagedMaterial;
    [ProtoMember(34, IsRequired = false)]
    public float Friction = 1f;
    [ProtoMember(37, IsRequired = false)]
    public float Restitution = 1f;
    [ProtoMember(40, IsRequired = false)]
    public ColorDefinitionRGBA? ColorKey;
    [ProtoMember(43, IsRequired = false)]
    [DefaultValue("")]
    public string LandingEffect;
    [ProtoMember(46, IsRequired = false)]
    public int AsteroidGeneratorSpawnProbabilityMultiplier = 1;
    [ProtoMember(49, IsRequired = false)]
    [DefaultValue("")]
    public string BareVariant;

    protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EMaterialTypeName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in string value) => owner.MaterialTypeName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out string value) => value = owner.MaterialTypeName;
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EMinedOre\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in string value) => owner.MinedOre = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out string value) => value = owner.MinedOre;
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EMinedOreRatio\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in float value) => owner.MinedOreRatio = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out float value) => value = owner.MinedOreRatio;
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003ECanBeHarvested\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in bool value) => owner.CanBeHarvested = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out bool value) => value = owner.CanBeHarvested;
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EIsRare\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in bool value) => owner.IsRare = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out bool value) => value = owner.IsRare;
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EUseTwoTextures\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in bool value) => owner.UseTwoTextures = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out bool value) => value = owner.UseTwoTextures;
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EVoxelHandPreview\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in string value) => owner.VoxelHandPreview = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out string value) => value = owner.VoxelHandPreview;
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EMinVersion\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in int value) => owner.MinVersion = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out int value) => value = owner.MinVersion;
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EMaxVersion\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in int value) => owner.MaxVersion = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out int value) => value = owner.MaxVersion;
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003ESpawnsInAsteroids\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in bool value) => owner.SpawnsInAsteroids = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out bool value) => value = owner.SpawnsInAsteroids;
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003ESpawnsFromMeteorites\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in bool value) => owner.SpawnsFromMeteorites = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out bool value) => value = owner.SpawnsFromMeteorites;
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EDamagedMaterial\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in string value) => owner.DamagedMaterial = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out string value) => value = owner.DamagedMaterial;
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EFriction\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in float value) => owner.Friction = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out float value) => value = owner.Friction;
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003ERestitution\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in float value) => owner.Restitution = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out float value) => value = owner.Restitution;
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EColorKey\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, ColorDefinitionRGBA?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VoxelMaterialDefinition owner,
        in ColorDefinitionRGBA? value)
      {
        owner.ColorKey = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VoxelMaterialDefinition owner,
        out ColorDefinitionRGBA? value)
      {
        value = owner.ColorKey;
      }
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003ELandingEffect\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in string value) => owner.LandingEffect = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out string value) => value = owner.LandingEffect;
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EAsteroidGeneratorSpawnProbabilityMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in int value) => owner.AsteroidGeneratorSpawnProbabilityMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out int value) => value = owner.AsteroidGeneratorSpawnProbabilityMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EBareVariant\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in string value) => owner.BareVariant = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out string value) => value = owner.BareVariant;
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VoxelMaterialDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VoxelMaterialDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VoxelMaterialDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VoxelMaterialDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VoxelMaterialDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VoxelMaterialDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMaterialDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMaterialDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMaterialDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_VoxelMaterialDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_VoxelMaterialDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_VoxelMaterialDefinition();

      MyObjectBuilder_VoxelMaterialDefinition IActivator<MyObjectBuilder_VoxelMaterialDefinition>.CreateInstance() => new MyObjectBuilder_VoxelMaterialDefinition();
    }
  }
}
