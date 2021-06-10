// Decompiled with JetBrains decompiler
// Type: VRage.Game.Networking.MySnapshotFlags
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

namespace VRage.Game.Networking
{
  public class MySnapshotFlags
  {
    public bool ApplyPosition;
    public bool ApplyRotation;
    public bool ApplyPhysicsAngular;
    public bool ApplyPhysicsLinear;
    public bool ApplyPhysicsLocal;
    public bool InheritRotation = true;

    public void Init(MySnapshotFlags flags)
    {
      this.ApplyPosition = flags.ApplyPosition;
      this.ApplyRotation = flags.ApplyRotation;
      this.ApplyPhysicsAngular = flags.ApplyPhysicsAngular;
      this.ApplyPhysicsLinear = flags.ApplyPhysicsLinear;
      this.ApplyPhysicsLocal = flags.ApplyPhysicsLocal;
      this.InheritRotation = flags.InheritRotation;
    }

    public void Init(bool state)
    {
      this.ApplyPosition = state;
      this.ApplyRotation = state;
      this.ApplyPhysicsAngular = state;
      this.ApplyPhysicsLinear = state;
    }
  }
}
