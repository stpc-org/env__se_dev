// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyCameraBlockDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_CameraBlockDefinition), null)]
  public class MyCameraBlockDefinition : MyCubeBlockDefinition
  {
    public string ResourceSinkGroup;
    public float RequiredPowerInput;
    public float RequiredChargingInput;
    public string OverlayTexture;
    public float MinFov;
    public float MaxFov;
    public float RaycastConeLimit;
    public double RaycastDistanceLimit;
    public float RaycastTimeMultiplier;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_CameraBlockDefinition cameraBlockDefinition = builder as MyObjectBuilder_CameraBlockDefinition;
      this.ResourceSinkGroup = cameraBlockDefinition.ResourceSinkGroup;
      this.RequiredPowerInput = cameraBlockDefinition.RequiredPowerInput;
      this.RequiredChargingInput = cameraBlockDefinition.RequiredChargingInput;
      this.OverlayTexture = cameraBlockDefinition.OverlayTexture;
      this.MinFov = cameraBlockDefinition.MinFov;
      this.MaxFov = cameraBlockDefinition.MaxFov;
      this.RaycastConeLimit = cameraBlockDefinition.RaycastConeLimit;
      this.RaycastDistanceLimit = cameraBlockDefinition.RaycastDistanceLimit;
      this.RaycastTimeMultiplier = cameraBlockDefinition.RaycastTimeMultiplier;
    }

    private class Sandbox_Definitions_MyCameraBlockDefinition\u003C\u003EActor : IActivator, IActivator<MyCameraBlockDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyCameraBlockDefinition();

      MyCameraBlockDefinition IActivator<MyCameraBlockDefinition>.CreateInstance() => new MyCameraBlockDefinition();
    }
  }
}
