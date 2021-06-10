// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpRefiningOre
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_RefiningOre", 210)]
  internal class MyIngameHelpRefiningOre : MyIngameHelpObjective
  {
    private HashSet<MyRefinery> m_observedRefineries = new HashSet<MyRefinery>();
    private bool m_ingotProduced;

    public MyIngameHelpRefiningOre()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_RefiningOre_Title;
      this.RequiredIds = new string[2]
      {
        "IngameHelp_HandDrill",
        "IngameHelp_Building"
      };
      this.RequiredCondition = this.RequiredCondition + new Func<bool>(this.OreInInventory);
      this.Details = new MyIngameHelpDetail[2]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_RefiningOre_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_RefiningOre_Detail2,
          FinishCondition = new Func<bool>(this.IngotFromRefinery)
        }
      };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
    }

    public override void OnActivated()
    {
      base.OnActivated();
      MyInventory.OnTransferByUser += new Action<IMyInventory, IMyInventory, IMyInventoryItem, MyFixedPoint>(this.MyInventory_OnTransferByUser);
    }

    private void MyInventory_OnTransferByUser(
      IMyInventory inventory1,
      IMyInventory inventory2,
      IMyInventoryItem item,
      MyFixedPoint amount)
    {
      MyCharacter myCharacter = MySession.Static != null ? MySession.Static.LocalCharacter : (MyCharacter) null;
      if (myCharacter == null || !(item.Content is MyObjectBuilder_Ore) || (inventory1.Owner != myCharacter || !(inventory2.Owner is MyRefinery owner)) || this.m_observedRefineries.Contains(owner))
        return;
      owner.OutputInventory.ContentsAdded += new Action<MyPhysicalInventoryItem, MyFixedPoint>(this.OutputInventory_ContentsAdded);
      this.m_observedRefineries.Add(owner);
    }

    private void OutputInventory_ContentsAdded(MyPhysicalInventoryItem item, MyFixedPoint amount)
    {
      if (!(item.Content is MyObjectBuilder_Ingot))
        return;
      this.m_ingotProduced = true;
    }

    private bool OreInInventory()
    {
      MyCharacter thisEntity = MySession.Static != null ? MySession.Static.LocalCharacter : (MyCharacter) null;
      if (thisEntity != null)
      {
        foreach (MyPhysicalInventoryItem physicalInventoryItem in MyEntityExtensions.GetInventory(thisEntity).GetItems())
        {
          if (physicalInventoryItem.Content is MyObjectBuilder_Ore)
            return true;
        }
      }
      return false;
    }

    private bool IngotFromRefinery() => this.m_ingotProduced;
  }
}
