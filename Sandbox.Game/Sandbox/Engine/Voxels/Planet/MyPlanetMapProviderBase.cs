// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.Planet.MyPlanetMapProviderBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using VRage.Factory;
using VRage.Game;
using VRage.Game.ObjectBuilders;

namespace Sandbox.Engine.Voxels.Planet
{
  [MyFactorable(typeof (MyObjectFactory<MyPlanetMapProviderAttribute, MyPlanetMapProviderBase>))]
  public abstract class MyPlanetMapProviderBase
  {
    public abstract void Init(
      long seed,
      MyPlanetGeneratorDefinition generator,
      MyObjectBuilder_PlanetMapProvider builder);

    public abstract MyCubemap[] GetMaps(MyPlanetMapTypeSet types);

    public abstract MyHeightCubemap GetHeightmap();

    public static MyObjectFactory<MyPlanetMapProviderAttribute, MyPlanetMapProviderBase> Factory => MyObjectFactory<MyPlanetMapProviderAttribute, MyPlanetMapProviderBase>.Get();
  }
}
