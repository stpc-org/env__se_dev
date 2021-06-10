// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyToolbarItemEmote
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Networking;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Definitions.Animation;
using VRage.Game.Entity;
using VRage.GameServices;

namespace Sandbox.Game.Screens.Helpers
{
  [MyToolbarItemDescriptor(typeof (MyObjectBuilder_ToolbarItemEmote))]
  internal class MyToolbarItemEmote : MyToolbarItemDefinition
  {
    public override bool Activate()
    {
      if (this.Definition == null || !MyGameService.HasInventoryItem(this.Definition.Id.SubtypeName))
        return false;
      bool flag = MySession.Static.ControlledEntity is MyCockpit;
      MyCharacter myCharacter = flag ? ((MyShipController) MySession.Static.ControlledEntity).Pilot : MySession.Static.LocalCharacter;
      if (myCharacter != null)
      {
        MyAnimationDefinition animationForCharacter = ((MyEmoteDefinition) this.Definition).GetAnimationForCharacter(myCharacter.Definition.Id);
        if (animationForCharacter == null)
          return false;
        if (myCharacter.IsOnLadder || flag && !animationForCharacter.AllowInCockpit || !myCharacter.UseNewAnimationSystem)
          return true;
        if (!MySession.Static.GetComponent<MySessionComponentDLC>().HasDefinitionDLC(this.Definition, MySession.Static.LocalHumanPlayer.Id.SteamId))
          return false;
        string subtypeName = animationForCharacter.Id.SubtypeName;
        myCharacter.TriggerCharacterAnimationEvent("emote", true, animationForCharacter.InfluenceAreas);
        myCharacter.TriggerCharacterAnimationEvent(subtypeName, true, animationForCharacter.InfluenceAreas);
      }
      return true;
    }

    public override bool Init(MyObjectBuilder_ToolbarItem objBuilder)
    {
      base.Init(objBuilder);
      this.ActivateOnClick = true;
      this.WantsToBeActivated = true;
      if (Sync.IsDedicated)
        return true;
      MyGameInventoryItemDefinition inventoryItemDefinition = MyGameService.GetInventoryItemDefinition(this.Definition.Id.SubtypeName);
      return inventoryItemDefinition != null && inventoryItemDefinition.ItemSlot == MyGameInventoryItemSlot.Emote && MyGameService.HasInventoryItem(this.Definition.Id.SubtypeName);
    }

    public override MyObjectBuilder_ToolbarItem GetObjectBuilder() => (MyObjectBuilder_ToolbarItem) (base.GetObjectBuilder() as MyObjectBuilder_ToolbarItemEmote);

    public override bool AllowedInToolbarType(MyToolbarType type) => type == MyToolbarType.Character || type == MyToolbarType.Ship || type == MyToolbarType.Seat;

    public override MyToolbarItem.ChangeInfo Update(MyEntity owner, long playerID = 0)
    {
      MyToolbarItem.ChangeInfo changeInfo = MyToolbarItem.ChangeInfo.None;
      if (Sync.IsDedicated)
        return changeInfo;
      if (MySession.Static.LocalHumanPlayer == null)
      {
        if (this.Enabled)
          changeInfo |= this.SetEnabled(false);
        return changeInfo;
      }
      bool newEnabled = MyGameService.HasInventoryItem(this.Definition.Id.SubtypeName) & MySession.Static.GetComponent<MySessionComponentDLC>().HasDefinitionDLC(this.Definition, MySession.Static.LocalHumanPlayer.Id.SteamId);
      if (this.Enabled != newEnabled)
        changeInfo |= this.SetEnabled(newEnabled);
      return changeInfo;
    }
  }
}
