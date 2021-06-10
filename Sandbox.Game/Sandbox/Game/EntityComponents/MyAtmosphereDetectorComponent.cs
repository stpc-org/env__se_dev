// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.MyAtmosphereDetectorComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using VRage.Audio;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.EntityComponents
{
  [MyComponentBuilder(typeof (MyObjectBuilder_AtmosphereDetectorComponent), true)]
  public class MyAtmosphereDetectorComponent : MyEntityComponentBase
  {
    private MyCharacter m_character;
    private bool m_localPlayer = true;
    private bool m_inAtmosphere;
    private MyAtmosphereDetectorComponent.AtmosphereStatus m_atmosphereStatus;

    public bool InAtmosphere => this.m_atmosphereStatus == MyAtmosphereDetectorComponent.AtmosphereStatus.Atmosphere;

    public bool InShipOrStation => this.m_atmosphereStatus == MyAtmosphereDetectorComponent.AtmosphereStatus.ShipOrStation;

    public bool InVoid => this.m_atmosphereStatus == MyAtmosphereDetectorComponent.AtmosphereStatus.Space;

    public void InitComponent(bool onlyLocalPlayer, MyCharacter character)
    {
      this.m_localPlayer = onlyLocalPlayer;
      this.m_character = character;
    }

    public void UpdateAtmosphereStatus()
    {
      if (this.m_character == null || this.m_localPlayer && (MySession.Static == null || this.m_character != MySession.Static.LocalCharacter))
        return;
      MyAtmosphereDetectorComponent.AtmosphereStatus atmosphereStatus = this.m_atmosphereStatus;
      Vector3D position = this.m_character.PositionComp.GetPosition();
      if ((double) MyGravityProviderSystem.CalculateNaturalGravityInPoint(position).LengthSquared() > 0.0)
      {
        MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(position);
        this.m_atmosphereStatus = closestPlanet == null || !closestPlanet.HasAtmosphere || (double) closestPlanet.GetAirDensity(position) <= 0.5 ? MyAtmosphereDetectorComponent.AtmosphereStatus.Space : MyAtmosphereDetectorComponent.AtmosphereStatus.Atmosphere;
      }
      else
        this.m_atmosphereStatus = MyAtmosphereDetectorComponent.AtmosphereStatus.Space;
      if (this.m_atmosphereStatus == MyAtmosphereDetectorComponent.AtmosphereStatus.Space)
      {
        float num = 0.0f;
        if (this.m_character.OxygenComponent != null)
          num = !this.m_localPlayer ? this.m_character.EnvironmentOxygenLevel : (!(MySession.Static.ControlledEntity is MyCharacter) ? (float) this.m_character.OxygenLevelAtCharacterLocation : this.m_character.EnvironmentOxygenLevel);
        if ((double) num > 0.100000001490116)
          this.m_atmosphereStatus = MyAtmosphereDetectorComponent.AtmosphereStatus.ShipOrStation;
      }
      if (!MyFakes.ENABLE_REALISTIC_LIMITER || !MyFakes.ENABLE_NEW_SOUNDS || (atmosphereStatus == this.m_atmosphereStatus || MySession.Static == null) || !MySession.Static.Settings.RealisticSound)
        return;
      MyAudio.Static.EnableMasterLimiter(!this.InAtmosphere && !this.InShipOrStation);
    }

    public override string ComponentTypeDebugString => "AtmosphereDetector";

    private enum AtmosphereStatus
    {
      NotSet,
      Space,
      ShipOrStation,
      Atmosphere,
    }

    private class Sandbox_Game_EntityComponents_MyAtmosphereDetectorComponent\u003C\u003EActor : IActivator, IActivator<MyAtmosphereDetectorComponent>
    {
      object IActivator.CreateInstance() => (object) new MyAtmosphereDetectorComponent();

      MyAtmosphereDetectorComponent IActivator<MyAtmosphereDetectorComponent>.CreateInstance() => new MyAtmosphereDetectorComponent();
    }
  }
}
