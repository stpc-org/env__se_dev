// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpIngots
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
  [IngameObjective("IngameHelp_Ingots", 220)]
  internal class MyIngameHelpIngots : MyIngameHelpObjective
  {
    private HashSet<MyAssembler> m_observedAssemblers = new HashSet<MyAssembler>();
    private bool m_ingotAdded;
    private bool m_steelProduced;

    public MyIngameHelpIngots()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_Ingots_Title;
      this.RequiredIds = new string[1]
      {
        "IngameHelp_RefiningOre"
      };
      this.RequiredCondition = this.RequiredCondition + new Func<bool>(this.IngotsInInventory);
      this.Details = new MyIngameHelpDetail[3]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Ingots_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Ingots_Detail2,
          FinishCondition = new Func<bool>(this.PutToAssembler)
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Ingots_Detail3,
          FinishCondition = new Func<bool>(this.SteelFromAssembler)
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
      if (myCharacter == null || !(item.Content is MyObjectBuilder_Ingot) || (!(item.Content.SubtypeName == "Iron") || inventory1.Owner != myCharacter) || (!(inventory2.Owner is MyAssembler owner) || this.m_observedAssemblers.Contains(owner)))
        return;
      owner.OutputInventory.ContentsAdded += new Action<MyPhysicalInventoryItem, MyFixedPoint>(this.OutputInventory_ContentsAdded);
      this.m_observedAssemblers.Add(owner);
      this.m_ingotAdded = true;
    }

    private bool PutToAssembler() => this.m_ingotAdded;

    private void OutputInventory_ContentsAdded(MyPhysicalInventoryItem item, MyFixedPoint amount)
    {
      if (!(item.Content is MyObjectBuilder_Component) || !(item.Content.SubtypeName == "SteelPlate"))
        return;
      this.m_steelProduced = true;
    }

    private bool IngotsInInventory()
    {
      MyCharacter thisEntity = MySession.Static != null ? MySession.Static.LocalCharacter : (MyCharacter) null;
      if (thisEntity != null)
      {
        foreach (MyPhysicalInventoryItem physicalInventoryItem in MyEntityExtensions.GetInventory(thisEntity).GetItems())
        {
          if (physicalInventoryItem.Content is MyObjectBuilder_Ingot && physicalInventoryItem.Content.SubtypeName == "Iron")
            return true;
        }
      }
      return false;
    }

    private bool SteelFromAssembler() => this.m_steelProduced;
  }
}
