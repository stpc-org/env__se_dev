// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyPlanetInitArguments
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using VRage.Game.Voxels;
using VRageMath;
using VRageRender.Messages;

namespace Sandbox.Game.Entities
{
  public struct MyPlanetInitArguments
  {
    public string StorageName;
    public int Seed;
    public IMyStorage Storage;
    public Vector3D PositionMinCorner;
    public float Radius;
    public float AtmosphereRadius;
    public float MaxRadius;
    public float MinRadius;
    public bool HasAtmosphere;
    public Vector3 AtmosphereWavelengths;
    public float GravityFalloff;
    public bool MarkAreaEmpty;
    public MyAtmosphereSettings AtmosphereSettings;
    public float SurfaceGravity;
    public bool AddGps;
    public bool SpherizeWithDistance;
    public MyPlanetGeneratorDefinition Generator;
    public bool UserCreated;
    public bool InitializeComponents;
    public bool FadeIn;

    public override string ToString() => "Planet init arguments: \nStorage name: " + (this.StorageName ?? "<null>") + "\n Storage: " + (this.Storage != null ? (object) this.Storage.ToString() : (object) "<null>") + "\n PositionMinCorner: " + (object) this.PositionMinCorner + "\n Radius: " + (object) this.Radius + "\n AtmosphereRadius: " + (object) this.AtmosphereRadius + "\n MaxRadius: " + (object) this.MaxRadius + "\n MinRadius: " + (object) this.MinRadius + "\n HasAtmosphere: " + this.HasAtmosphere.ToString() + "\n AtmosphereWavelengths: " + (object) this.AtmosphereWavelengths + "\n GravityFalloff: " + (object) this.GravityFalloff + "\n MarkAreaEmpty: " + this.MarkAreaEmpty.ToString() + "\n AtmosphereSettings: " + this.AtmosphereSettings.ToString() + "\n SurfaceGravity: " + (object) this.SurfaceGravity + "\n AddGps: " + this.AddGps.ToString() + "\n SpherizeWithDistance: " + this.SpherizeWithDistance.ToString() + "\n Generator: " + (this.Generator != null ? (object) this.Generator.ToString() : (object) "<null>") + "\n UserCreated: " + this.UserCreated.ToString() + "\n InitializeComponents: " + this.InitializeComponents.ToString();
  }
}
