// Decompiled with JetBrains decompiler
// Type: VRage.Game.VisualScripting.IMyStateMachineScript
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

namespace VRage.Game.VisualScripting
{
  public interface IMyStateMachineScript
  {
    string TransitionTo { get; set; }

    long OwnerId { get; set; }

    [VisualScriptingMember(true, false)]
    void Init();

    [VisualScriptingMember(true, false)]
    void Update();

    [VisualScriptingMember(true, false)]
    void Dispose();

    [VisualScriptingMiscData("Self", "Completes the scripts by setting state to completed.", -10510688)]
    [VisualScriptingMember(true, true)]
    void Complete(string transitionName = "Completed");

    [VisualScriptingMember(false, true)]
    long GetOwnerId();

    [VisualScriptingMember(true, false)]
    void Deserialize();
  }
}
