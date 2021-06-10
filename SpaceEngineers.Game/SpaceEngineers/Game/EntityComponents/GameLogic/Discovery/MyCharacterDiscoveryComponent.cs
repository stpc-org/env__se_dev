// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.EntityComponents.GameLogic.Discovery.MyCharacterDiscoveryComponent
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using ObjectBuilders.Discovery;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;

namespace SpaceEngineers.Game.EntityComponents.GameLogic.Discovery
{
  [MyComponentBuilder(typeof (MyObjectBuilder_CharacterDiscoveryComponent), true)]
  public class MyCharacterDiscoveryComponent : MyEntityComponentBase
  {
    private MyHudNotification m_hudNotificiation;
    private MyRadioReceiver m_reciever;

    public override string ComponentTypeDebugString => "CharacterDiscoveryComponent";

    public MyCharacterDiscoveryComponent() => this.m_hudNotificiation = new MyHudNotification(MySpaceTexts.Faction_Discovered_Info);

    public override void Init(MyComponentDefinitionBase definition) => base.Init(definition);

    public override void OnAddedToScene()
    {
      base.OnAddedToScene();
      this.m_reciever = this.Container.Get<MyDataReceiver>() as MyRadioReceiver;
      if (this.m_reciever == null)
        return;
      if (Sync.IsServer)
        this.m_reciever.OnBroadcasterFound += new MyDataReceiver.BroadcasterChangeInfo(this.OnBroadcasterDiscovered);
      MySession.Static.Factions.OnFactionDiscovered += new Action<MyFaction, MyPlayer.PlayerId>(this.OnFactionDiscovered);
    }

    public override void OnRemovedFromScene()
    {
      MySession.Static.Factions.OnFactionDiscovered -= new Action<MyFaction, MyPlayer.PlayerId>(this.OnFactionDiscovered);
      if (Sync.IsServer)
        this.m_reciever.OnBroadcasterFound -= new MyDataReceiver.BroadcasterChangeInfo(this.OnBroadcasterDiscovered);
      base.OnRemovedFromScene();
    }

    private void OnBroadcasterDiscovered(MyDataBroadcaster broadcaster)
    {
      MyPlayer controllingPlayer = this.GetControllingPlayer();
      if (controllingPlayer == null || !(broadcaster.Entity is MyCubeBlock entity))
        return;
      MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(entity.OwnerId);
      if (playerFaction == null || !MySession.Static.Factions.IsNpcFaction(playerFaction.Tag) || MySession.Static.Factions.IsFactionDiscovered(controllingPlayer.Id, playerFaction.FactionId))
        return;
      MySession.Static.Factions.AddDiscoveredFaction(controllingPlayer.Id, playerFaction.FactionId);
    }

    private void OnFactionDiscovered(MyFaction discoveredFaction, MyPlayer.PlayerId playerId)
    {
      if (Sync.IsDedicated || MySession.Static == null)
        return;
      MyPlayer controllingPlayer = this.GetControllingPlayer();
      MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
      MyPlayer.PlayerId? id1 = controllingPlayer?.Id;
      MyPlayer.PlayerId id2 = localHumanPlayer.Id;
      if ((id1.HasValue ? (id1.HasValue ? (id1.GetValueOrDefault() == id2 ? 1 : 0) : 1) : 0) == 0 || controllingPlayer == null || !(controllingPlayer.Id == playerId))
        return;
      this.m_hudNotificiation.SetTextFormatArguments((object) discoveredFaction.Name);
      MyHud.Notifications.Add((MyHudNotificationBase) this.m_hudNotificiation);
    }

    private MyPlayer GetControllingPlayer()
    {
      if (!(this.Entity is MyEntity entity))
        return (MyPlayer) null;
      MyPlayer controllingPlayer = MySession.Static.Players.GetControllingPlayer(entity);
      if (controllingPlayer == null && entity is MyCharacter myCharacter && myCharacter.IsUsing != null)
        controllingPlayer = MySession.Static.Players.GetControllingPlayer(myCharacter.IsUsing);
      return controllingPlayer;
    }
  }
}
