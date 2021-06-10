// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Utils.MyCutsceneCamera
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game.Entity;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Engine.Utils
{
  internal class MyCutsceneCamera : MyEntity, IMyCameraController
  {
    public float FOV = 70f;

    public MyEntity Entity => (MyEntity) this;

    public MyCutsceneCamera() => this.Init((MyObjectBuilder_EntityBase) null);

    public void ControlCamera(MyCamera currentCamera)
    {
      currentCamera.FieldOfViewDegrees = this.FOV;
      currentCamera.SetViewMatrix(MatrixD.Invert(this.WorldMatrix));
    }

    public void Rotate(Vector2 rotationIndicator, float rollIndicator)
    {
    }

    public void RotateStopped()
    {
    }

    public void OnAssumeControl(IMyCameraController previousCameraController)
    {
    }

    public void OnReleaseControl(IMyCameraController newCameraController)
    {
    }

    public bool HandleUse() => false;

    public bool HandlePickUp() => false;

    public bool IsInFirstPersonView
    {
      get => true;
      set
      {
      }
    }

    public bool EnableFirstPersonView
    {
      get => true;
      set
      {
      }
    }

    public bool ForceFirstPersonCamera
    {
      get => true;
      set
      {
      }
    }

    public bool AllowCubeBuilding => false;

    private class Sandbox_Engine_Utils_MyCutsceneCamera\u003C\u003EActor : IActivator, IActivator<MyCutsceneCamera>
    {
      object IActivator.CreateInstance() => (object) new MyCutsceneCamera();

      MyCutsceneCamera IActivator<MyCutsceneCamera>.CreateInstance() => new MyCutsceneCamera();
    }
  }
}
