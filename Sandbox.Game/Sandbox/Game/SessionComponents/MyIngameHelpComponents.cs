// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpComponents
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Localization;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System;
using VRage.Game;
using VRage.Game.Entity;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_Components", 230)]
  internal class MyIngameHelpComponents : MyIngameHelpObjective
  {
    public MyIngameHelpComponents()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_Components_Title;
      this.RequiredIds = new string[1]
      {
        "IngameHelp_Ingots"
      };
      this.RequiredCondition = this.RequiredCondition + new Func<bool>(this.ComponentsInInventory);
      this.Details = new MyIngameHelpDetail[2]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Components_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Components_Detail2,
          FinishCondition = new Func<bool>(this.BlockRepaired)
        }
      };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
      this.FollowingId = "IngameHelp_ComponentsTip";
    }

    private bool ComponentsInInventory()
    {
      MyCharacter thisEntity = MySession.Static != null ? MySession.Static.LocalCharacter : (MyCharacter) null;
      if (thisEntity != null)
      {
        foreach (MyPhysicalInventoryItem physicalInventoryItem in MyEntityExtensions.GetInventory(thisEntity).GetItems())
        {
          if (physicalInventoryItem.Content is MyObjectBuilder_Component)
            return true;
        }
      }
      return false;
    }

    private bool BlockRepaired()
    {
      MyCharacter myCharacter = MySession.Static != null ? MySession.Static.LocalCharacter : (MyCharacter) null;
      return myCharacter != null && myCharacter.EquippedTool is MyWelder && (!string.IsNullOrEmpty((myCharacter.EquippedTool as MyWelder).EffectId) && (myCharacter.EquippedTool as MyWelder).IsShooting) && (myCharacter.EquippedTool as MyWelder).HasHitBlock && !(myCharacter.EquippedTool as MyWelder).GetTargetBlock().IsFullIntegrity;
    }
  }
}
