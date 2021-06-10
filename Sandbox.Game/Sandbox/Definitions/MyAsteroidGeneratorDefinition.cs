// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyAsteroidGeneratorDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Collections;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_AsteroidGeneratorDefinition), null)]
  public class MyAsteroidGeneratorDefinition : MyDefinitionBase
  {
    public int Version;
    public int ObjectSizeMin;
    public int ObjectSizeMax;
    public int SubcellSize;
    public int SubCells;
    public int ObjectMaxInCluster;
    public int ObjectMinDistanceInCluster;
    public int ObjectMaxDistanceInClusterMin;
    public int ObjectMaxDistanceInClusterMax;
    public int ObjectSizeMinCluster;
    public int ObjectSizeMaxCluster;
    public double ObjectDensityCluster;
    public bool ClusterDispersionAbsolute;
    public bool AllowPartialClusterObjectOverlap;
    public bool UseClusterDefAsAsteroid;
    public bool RotateAsteroids;
    public bool UseLinearPowOfTwoSizeDistribution;
    public bool UseGeneratorSeed;
    public bool UseClusterVariableSize;
    public DictionaryReader<MyObjectSeedType, double> SeedTypeProbability;
    public DictionaryReader<MyObjectSeedType, double> SeedClusterTypeProbability;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_AsteroidGeneratorDefinition generatorDefinition = (MyObjectBuilder_AsteroidGeneratorDefinition) builder;
      this.Version = int.Parse(generatorDefinition.Id.SubtypeId);
      this.SubCells = generatorDefinition.SubCells;
      this.ObjectSizeMax = generatorDefinition.ObjectSizeMax;
      this.SubcellSize = 4096 + generatorDefinition.ObjectSizeMax * 2;
      this.ObjectSizeMin = generatorDefinition.ObjectSizeMin;
      this.RotateAsteroids = generatorDefinition.RotateAsteroids;
      this.UseGeneratorSeed = generatorDefinition.UseGeneratorSeed;
      this.ObjectMaxInCluster = generatorDefinition.ObjectMaxInCluster;
      this.ObjectDensityCluster = generatorDefinition.ObjectDensityCluster;
      this.ObjectSizeMaxCluster = generatorDefinition.ObjectSizeMaxCluster;
      this.ObjectSizeMinCluster = generatorDefinition.ObjectSizeMinCluster;
      this.UseClusterVariableSize = generatorDefinition.UseClusterVariableSize;
      this.UseClusterDefAsAsteroid = generatorDefinition.UseClusterDefAsAsteroid;
      this.ClusterDispersionAbsolute = generatorDefinition.ClusterDispersionAbsolute;
      this.ObjectMinDistanceInCluster = generatorDefinition.ObjectMinDistanceInCluster;
      this.ObjectMaxDistanceInClusterMax = generatorDefinition.ObjectMaxDistanceInClusterMax;
      this.ObjectMaxDistanceInClusterMin = generatorDefinition.ObjectMaxDistanceInClusterMin;
      this.AllowPartialClusterObjectOverlap = generatorDefinition.AllowPartialClusterObjectOverlap;
      this.UseLinearPowOfTwoSizeDistribution = generatorDefinition.UseLinearPowOfTwoSizeDistribution;
      this.SeedTypeProbability = (DictionaryReader<MyObjectSeedType, double>) generatorDefinition.SeedTypeProbability.Dictionary;
      this.SeedClusterTypeProbability = (DictionaryReader<MyObjectSeedType, double>) generatorDefinition.SeedClusterTypeProbability.Dictionary;
    }

    private class Sandbox_Definitions_MyAsteroidGeneratorDefinition\u003C\u003EActor : IActivator, IActivator<MyAsteroidGeneratorDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyAsteroidGeneratorDefinition();

      MyAsteroidGeneratorDefinition IActivator<MyAsteroidGeneratorDefinition>.CreateInstance() => new MyAsteroidGeneratorDefinition();
    }
  }
}
