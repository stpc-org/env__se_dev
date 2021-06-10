// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugNetworkCharacter
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Replication.StateGroups;
using Sandbox.Graphics.GUI;
using System;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Gui
{
  [MyDebugScreen("VRage", "Network Character")]
  internal class MyGuiScreenDebugNetworkCharacter : MyGuiScreenDebugBase
  {
    private MyGuiControlSlider m_maxJetpackGridDistanceSlider;
    private MyGuiControlSlider m_maxDisconnectDistanceSlider;
    private MyGuiControlSlider m_minJetpackGridSpeedSlider;
    private MyGuiControlSlider m_minJetpackDisconnectGridSpeedSlider;
    private MyGuiControlSlider m_minJetpackInsideGridSpeedSlider;
    private MyGuiControlSlider m_minJetpackDisconnectInsideGridSpeedSlider;
    private MyGuiControlSlider m_maxJetpackGridAccelerationSlider;
    private MyGuiControlSlider m_maxJetpackDisconnectGridAccelerationSlider;

    public MyGuiScreenDebugNetworkCharacter()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.AddCaption("Network Character", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.AddSlider("Animation fallback threshold [m]", MyCharacterPhysicsStateGroup.EXCESSIVE_CORRECTION_THRESHOLD, 0.0f, 100f, (Action<MyGuiControlSlider>) (slider => MyCharacterPhysicsStateGroup.EXCESSIVE_CORRECTION_THRESHOLD = slider.Value));
      this.AddLabel("Support", (Vector4) Color.White, 1f);
      this.AddSlider("Change delay [ms]", (float) MyCharacterPhysicsStateGroup.ParentChangeTimeOut.Milliseconds, 0.0f, 5000f, (Action<MyGuiControlSlider>) (slider => MyMultiplayer.RaiseStaticEvent<double>((Func<IMyEventOwner, Action<double>>) (x => new Action<double>(MyMultiplayerBase.OnCharacterParentChangeTimeOut)), (double) slider.Value)));
      this.AddLabel("Jetpack Connect", (Vector4) Color.White, 1f);
      this.m_maxJetpackGridDistanceSlider = this.AddSlider("Max distance [m]", MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentDistance, 0.0f, 1000f, (Action<MyGuiControlSlider>) (slider =>
      {
        MyMultiplayer.RaiseStaticEvent<float>((Func<IMyEventOwner, Action<float>>) (x => new Action<float>(MyMultiplayerBase.OnCharacterMaxJetpackGridDistance)), slider.Value);
        MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentDistance = slider.Value;
        this.m_maxDisconnectDistanceSlider.Value = Math.Max(MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentDisconnectDistance, slider.Value);
      }));
      this.m_maxJetpackGridAccelerationSlider = this.AddSlider("Max acceleration [m/s^2]", MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentAcceleration, 0.0f, 1000f, (Action<MyGuiControlSlider>) (slider =>
      {
        MyMultiplayer.RaiseStaticEvent<float>((Func<IMyEventOwner, Action<float>>) (x => new Action<float>(MyMultiplayerBase.OnCharacterMaxJetpackGridAcceleration)), slider.Value);
        MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentAcceleration = slider.Value;
        this.m_maxJetpackDisconnectGridAccelerationSlider.Value = Math.Max(MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxDisconnectParentAcceleration, slider.Value);
      }));
      this.m_minJetpackGridSpeedSlider = this.AddSlider("Min speed [m/s]", MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinParentSpeed, 0.0f, 100f, (Action<MyGuiControlSlider>) (slider =>
      {
        MyMultiplayer.RaiseStaticEvent<float>((Func<IMyEventOwner, Action<float>>) (x => new Action<float>(MyMultiplayerBase.OnCharacterMinJetpackGridSpeed)), slider.Value);
        MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinParentSpeed = slider.Value;
        this.m_minJetpackDisconnectGridSpeedSlider.Value = Math.Min(MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinDisconnectParentSpeed, slider.Value);
      }));
      this.m_minJetpackInsideGridSpeedSlider = this.AddSlider("Min inside speed [m/s]", MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinInsideParentSpeed, 0.0f, 100f, (Action<MyGuiControlSlider>) (slider =>
      {
        MyMultiplayer.RaiseStaticEvent<float>((Func<IMyEventOwner, Action<float>>) (x => new Action<float>(MyMultiplayerBase.OnCharacterMinJetpackInsideGridSpeed)), slider.Value);
        MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinInsideParentSpeed = slider.Value;
        this.m_minJetpackDisconnectInsideGridSpeedSlider.Value = Math.Min(MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinDisconnectInsideParentSpeed, slider.Value);
      }));
      this.AddLabel("Jetpack Disconnect", (Vector4) Color.White, 1f);
      this.m_maxDisconnectDistanceSlider = this.AddSlider("Max distance [m]", MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentDisconnectDistance, 0.0f, 1000f, (Action<MyGuiControlSlider>) (slider =>
      {
        MyMultiplayer.RaiseStaticEvent<float>((Func<IMyEventOwner, Action<float>>) (x => new Action<float>(MyMultiplayerBase.OnCharacterMaxJetpackGridDisconnectDistance)), slider.Value);
        MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentDisconnectDistance = slider.Value;
        this.m_maxJetpackGridDistanceSlider.Value = Math.Min(MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentDistance, slider.Value);
      }));
      this.m_maxJetpackDisconnectGridAccelerationSlider = this.AddSlider("Max acceleration [m/s^2]", MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxDisconnectParentAcceleration, 0.0f, 1000f, (Action<MyGuiControlSlider>) (slider =>
      {
        MyMultiplayer.RaiseStaticEvent<float>((Func<IMyEventOwner, Action<float>>) (x => new Action<float>(MyMultiplayerBase.OnCharacterMaxJetpackDisconnectGridAcceleration)), slider.Value);
        MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxDisconnectParentAcceleration = slider.Value;
        this.m_maxJetpackGridAccelerationSlider.Value = Math.Min(MyCharacterPhysicsStateGroup.JetpackParentingSetup.MaxParentAcceleration, slider.Value);
      }));
      this.m_minJetpackDisconnectGridSpeedSlider = this.AddSlider("Min speed [m/s]", MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinDisconnectParentSpeed, 0.0f, 100f, (Action<MyGuiControlSlider>) (slider =>
      {
        MyMultiplayer.RaiseStaticEvent<float>((Func<IMyEventOwner, Action<float>>) (x => new Action<float>(MyMultiplayerBase.OnCharacterMinJetpackDisconnectGridSpeed)), slider.Value);
        MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinDisconnectParentSpeed = slider.Value;
        this.m_minJetpackGridSpeedSlider.Value = Math.Max(MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinParentSpeed, slider.Value);
      }));
      this.m_minJetpackDisconnectInsideGridSpeedSlider = this.AddSlider("Min inside speed [m/s]", MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinDisconnectInsideParentSpeed, 0.0f, 100f, (Action<MyGuiControlSlider>) (slider =>
      {
        MyMultiplayer.RaiseStaticEvent<float>((Func<IMyEventOwner, Action<float>>) (x => new Action<float>(MyMultiplayerBase.OnCharacterMinJetpackDisconnectInsideGridSpeed)), slider.Value);
        MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinDisconnectInsideParentSpeed = slider.Value;
        this.m_minJetpackInsideGridSpeedSlider.Value = Math.Max(MyCharacterPhysicsStateGroup.JetpackParentingSetup.MinInsideParentSpeed, slider.Value);
      }));
    }
  }
}
