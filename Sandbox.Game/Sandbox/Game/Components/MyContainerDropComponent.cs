// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyContainerDropComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage.Audio;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;

namespace Sandbox.Game.Components
{
  [MyComponentBuilder(typeof (MyObjectBuilder_ContainerDropComponent), true)]
  public class MyContainerDropComponent : MyEntityComponentBase
  {
    private MyEntity3DSoundEmitter m_soundEmitter;
    private string m_playingSound;
    private bool m_playSound;

    public bool Competetive { get; private set; }

    public string GPSName { get; private set; }

    public long Owner { get; private set; }

    public long GridEntityId { get; set; }

    public MyContainerDropComponent()
    {
    }

    public MyContainerDropComponent(bool competetive, string gpsName, long owner, string sound)
    {
      this.Competetive = competetive;
      this.GPSName = gpsName;
      this.Owner = owner;
      this.m_playingSound = sound;
      this.m_playSound = !string.IsNullOrEmpty(this.m_playingSound);
    }

    public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
    {
      MyObjectBuilder_ContainerDropComponent objectBuilder = MyComponentFactory.CreateObjectBuilder((MyComponentBase) this) as MyObjectBuilder_ContainerDropComponent;
      objectBuilder.Competetive = this.Competetive;
      objectBuilder.GPSName = this.GPSName;
      objectBuilder.Owner = this.Owner;
      objectBuilder.PlayingSound = this.m_playingSound;
      return (MyObjectBuilder_ComponentBase) objectBuilder;
    }

    public override void Deserialize(MyObjectBuilder_ComponentBase baseBuilder)
    {
      MyObjectBuilder_ContainerDropComponent containerDropComponent = baseBuilder as MyObjectBuilder_ContainerDropComponent;
      this.Competetive = containerDropComponent.Competetive;
      this.GPSName = containerDropComponent.GPSName;
      this.Owner = containerDropComponent.Owner;
      this.m_playingSound = containerDropComponent.PlayingSound;
      this.m_playSound = !string.IsNullOrEmpty(this.m_playingSound);
    }

    public bool PlaySound(string soundName)
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated)
      {
        this.m_playingSound = soundName;
        return true;
      }
      MySoundPair soundId = new MySoundPair(soundName);
      MyCueId myCueId = soundId.Arcade;
      if (myCueId.IsNull)
      {
        myCueId = soundId.Realistic;
        if (myCueId.IsNull)
          return true;
      }
      if (this.m_soundEmitter == null)
      {
        this.m_soundEmitter = new MyEntity3DSoundEmitter((MyEntity) this.Entity, true);
        if (this.Entity is MyCubeBlock entity)
          entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
      }
      int num = this.m_soundEmitter.PlaySound(soundId, true) ? 1 : 0;
      this.m_playingSound = soundName;
      return num != 0;
    }

    public void StopSound()
    {
      if (this.m_soundEmitter == null || !this.m_soundEmitter.IsPlaying)
        return;
      this.m_soundEmitter.StopSound(true);
    }

    public override void OnAddedToScene()
    {
      base.OnAddedToScene();
      if (!this.m_playSound || !this.PlaySound(this.m_playingSound))
        return;
      this.m_playSound = false;
    }

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      if (!this.m_playSound || !this.PlaySound(this.m_playingSound))
        return;
      this.m_playSound = false;
    }

    public void UpdateSound()
    {
      if (this.m_soundEmitter == null)
        return;
      this.m_soundEmitter.Update();
    }

    public override bool IsSerialized() => true;

    public override void OnBeforeRemovedFromContainer()
    {
      base.OnBeforeRemovedFromContainer();
      this.StopSound();
      this.m_soundEmitter = (MyEntity3DSoundEmitter) null;
    }

    public override string ComponentTypeDebugString => "ContainerDropComponent";

    public override void OnRemovedFromScene()
    {
      base.OnRemovedFromScene();
      this.StopSound();
      this.m_soundEmitter = (MyEntity3DSoundEmitter) null;
      if (!Sync.IsServer)
        return;
      MySession.Static.GetComponent<MySessionComponentContainerDropSystem>().ContainerDestroyed(this);
    }

    private class Sandbox_Game_Components_MyContainerDropComponent\u003C\u003EActor : IActivator, IActivator<MyContainerDropComponent>
    {
      object IActivator.CreateInstance() => (object) new MyContainerDropComponent();

      MyContainerDropComponent IActivator<MyContainerDropComponent>.CreateInstance() => new MyContainerDropComponent();
    }
  }
}
