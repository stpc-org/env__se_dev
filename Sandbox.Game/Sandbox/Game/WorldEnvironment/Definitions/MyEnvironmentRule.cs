// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.Definitions.MyEnvironmentRule
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRageMath;

namespace Sandbox.Game.WorldEnvironment.Definitions
{
  public class MyEnvironmentRule
  {
    public SerializableRange Height = new SerializableRange(0.0f, 1f);
    public SymmetricSerializableRange Latitude = new SymmetricSerializableRange(-90f, 90f);
    public SerializableRange Longitude = new SerializableRange(-180f, 180f);
    public SerializableRange Slope = new SerializableRange(0.0f, 90f);

    public void ConvertRanges()
    {
      this.Latitude.ConvertToSine();
      this.Longitude.ConvertToCosineLongitude();
      this.Slope.ConvertToCosine();
    }

    public bool Check(float height, float latitude, float longitude, float slope) => this.Height.ValueBetween(height) && this.Latitude.ValueBetween(latitude) && this.Longitude.ValueBetween(longitude) && this.Slope.ValueBetween(slope);
  }
}
