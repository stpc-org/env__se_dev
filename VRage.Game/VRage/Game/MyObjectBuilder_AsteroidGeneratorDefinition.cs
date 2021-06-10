// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_AsteroidGeneratorDefinition
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

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_AsteroidGeneratorDefinition : MyObjectBuilder_DefinitionBase
  {
    [ProtoMember(1)]
    public int ObjectSizeMin;
    [ProtoMember(4)]
    public int ObjectSizeMax;
    [ProtoMember(7)]
    public int SubCells;
    [ProtoMember(10)]
    public int ObjectMaxInCluster;
    [ProtoMember(13)]
    public int ObjectMinDistanceInCluster;
    [ProtoMember(16)]
    public int ObjectMaxDistanceInClusterMin;
    [ProtoMember(19)]
    public int ObjectMaxDistanceInClusterMax;
    [ProtoMember(22)]
    public int ObjectSizeMinCluster;
    [ProtoMember(25)]
    public int ObjectSizeMaxCluster;
    [ProtoMember(28)]
    public double ObjectDensityCluster;
    [ProtoMember(31)]
    public bool ClusterDispersionAbsolute;
    [ProtoMember(34)]
    public bool AllowPartialClusterObjectOverlap;
    [ProtoMember(37)]
    public bool UseClusterDefAsAsteroid;
    [ProtoMember(40)]
    public bool RotateAsteroids;
    [ProtoMember(43)]
    public bool UseLinearPowOfTwoSizeDistribution;
    [ProtoMember(46)]
    public bool UseGeneratorSeed;
    [ProtoMember(49)]
    public bool UseClusterVariableSize;
    [ProtoMember(52)]
    public SerializableDictionary<MyObjectSeedType, double> SeedTypeProbability;
    [ProtoMember(55)]
    public SerializableDictionary<MyObjectSeedType, double> SeedClusterTypeProbability;

    protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition\u003C\u003EObjectSizeMin\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        in int value)
      {
        owner.ObjectSizeMin = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        out int value)
      {
        value = owner.ObjectSizeMin;
      }
    }

    protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition\u003C\u003EObjectSizeMax\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        in int value)
      {
        owner.ObjectSizeMax = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        out int value)
      {
        value = owner.ObjectSizeMax;
      }
    }

    protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition\u003C\u003ESubCells\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        in int value)
      {
        owner.SubCells = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        out int value)
      {
        value = owner.SubCells;
      }
    }

    protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition\u003C\u003EObjectMaxInCluster\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        in int value)
      {
        owner.ObjectMaxInCluster = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        out int value)
      {
        value = owner.ObjectMaxInCluster;
      }
    }

    protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition\u003C\u003EObjectMinDistanceInCluster\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        in int value)
      {
        owner.ObjectMinDistanceInCluster = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        out int value)
      {
        value = owner.ObjectMinDistanceInCluster;
      }
    }

    protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition\u003C\u003EObjectMaxDistanceInClusterMin\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        in int value)
      {
        owner.ObjectMaxDistanceInClusterMin = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        out int value)
      {
        value = owner.ObjectMaxDistanceInClusterMin;
      }
    }

    protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition\u003C\u003EObjectMaxDistanceInClusterMax\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        in int value)
      {
        owner.ObjectMaxDistanceInClusterMax = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        out int value)
      {
        value = owner.ObjectMaxDistanceInClusterMax;
      }
    }

    protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition\u003C\u003EObjectSizeMinCluster\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        in int value)
      {
        owner.ObjectSizeMinCluster = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        out int value)
      {
        value = owner.ObjectSizeMinCluster;
      }
    }

    protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition\u003C\u003EObjectSizeMaxCluster\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        in int value)
      {
        owner.ObjectSizeMaxCluster = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        out int value)
      {
        value = owner.ObjectSizeMaxCluster;
      }
    }

    protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition\u003C\u003EObjectDensityCluster\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        in double value)
      {
        owner.ObjectDensityCluster = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        out double value)
      {
        value = owner.ObjectDensityCluster;
      }
    }

    protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition\u003C\u003EClusterDispersionAbsolute\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        in bool value)
      {
        owner.ClusterDispersionAbsolute = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        out bool value)
      {
        value = owner.ClusterDispersionAbsolute;
      }
    }

    protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition\u003C\u003EAllowPartialClusterObjectOverlap\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        in bool value)
      {
        owner.AllowPartialClusterObjectOverlap = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        out bool value)
      {
        value = owner.AllowPartialClusterObjectOverlap;
      }
    }

    protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition\u003C\u003EUseClusterDefAsAsteroid\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        in bool value)
      {
        owner.UseClusterDefAsAsteroid = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        out bool value)
      {
        value = owner.UseClusterDefAsAsteroid;
      }
    }

    protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition\u003C\u003ERotateAsteroids\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        in bool value)
      {
        owner.RotateAsteroids = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        out bool value)
      {
        value = owner.RotateAsteroids;
      }
    }

    protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition\u003C\u003EUseLinearPowOfTwoSizeDistribution\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        in bool value)
      {
        owner.UseLinearPowOfTwoSizeDistribution = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        out bool value)
      {
        value = owner.UseLinearPowOfTwoSizeDistribution;
      }
    }

    protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition\u003C\u003EUseGeneratorSeed\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        in bool value)
      {
        owner.UseGeneratorSeed = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        out bool value)
      {
        value = owner.UseGeneratorSeed;
      }
    }

    protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition\u003C\u003EUseClusterVariableSize\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        in bool value)
      {
        owner.UseClusterVariableSize = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        out bool value)
      {
        value = owner.UseClusterVariableSize;
      }
    }

    protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition\u003C\u003ESeedTypeProbability\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, SerializableDictionary<MyObjectSeedType, double>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        in SerializableDictionary<MyObjectSeedType, double> value)
      {
        owner.SeedTypeProbability = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        out SerializableDictionary<MyObjectSeedType, double> value)
      {
        value = owner.SeedTypeProbability;
      }
    }

    protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition\u003C\u003ESeedClusterTypeProbability\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, SerializableDictionary<MyObjectSeedType, double>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        in SerializableDictionary<MyObjectSeedType, double> value)
      {
        owner.SeedClusterTypeProbability = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        out SerializableDictionary<MyObjectSeedType, double> value)
      {
        value = owner.SeedClusterTypeProbability;
      }
    }

    protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AsteroidGeneratorDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AsteroidGeneratorDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_MyObjectBuilder_AsteroidGeneratorDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AsteroidGeneratorDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_AsteroidGeneratorDefinition();

      MyObjectBuilder_AsteroidGeneratorDefinition IActivator<MyObjectBuilder_AsteroidGeneratorDefinition>.CreateInstance() => new MyObjectBuilder_AsteroidGeneratorDefinition();
    }
  }
}
