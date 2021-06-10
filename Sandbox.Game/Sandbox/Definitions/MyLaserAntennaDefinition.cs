// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyLaserAntennaDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_LaserAntennaDefinition), null)]
  public class MyLaserAntennaDefinition : MyCubeBlockDefinition
  {
    public MyStringHash ResourceSinkGroup;
    public float PowerInputIdle;
    public float PowerInputTurning;
    public float PowerInputLasing;
    public float RotationRate;
    public float MaxRange;
    public bool RequireLineOfSight;
    public int MinElevationDegrees;
    public int MaxElevationDegrees;
    public int MinAzimuthDegrees;
    public int MaxAzimuthDegrees;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_LaserAntennaDefinition antennaDefinition = (MyObjectBuilder_LaserAntennaDefinition) builder;
      this.ResourceSinkGroup = MyStringHash.GetOrCompute(antennaDefinition.ResourceSinkGroup);
      this.PowerInputIdle = antennaDefinition.PowerInputIdle;
      this.PowerInputTurning = antennaDefinition.PowerInputTurning;
      this.PowerInputLasing = antennaDefinition.PowerInputLasing;
      this.RotationRate = antennaDefinition.RotationRate;
      this.MaxRange = antennaDefinition.MaxRange;
      this.RequireLineOfSight = antennaDefinition.RequireLineOfSight;
      this.MinElevationDegrees = antennaDefinition.MinElevationDegrees;
      this.MaxElevationDegrees = antennaDefinition.MaxElevationDegrees;
      this.MinAzimuthDegrees = antennaDefinition.MinAzimuthDegrees;
      this.MaxAzimuthDegrees = antennaDefinition.MaxAzimuthDegrees;
    }

    private class Sandbox_Definitions_MyLaserAntennaDefinition\u003C\u003EActor : IActivator, IActivator<MyLaserAntennaDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyLaserAntennaDefinition();

      MyLaserAntennaDefinition IActivator<MyLaserAntennaDefinition>.CreateInstance() => new MyLaserAntennaDefinition();
    }
  }
}
