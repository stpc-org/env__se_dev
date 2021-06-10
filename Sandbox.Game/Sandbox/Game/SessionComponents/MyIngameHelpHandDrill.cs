// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpHandDrill
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Localization;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System;
using VRage.Game;
using VRage.Game.Entity.UseObject;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_HandDrill", 210)]
  internal class MyIngameHelpHandDrill : MyIngameHelpObjective
  {
    private IMyUseObject m_interactiveObject;
    private bool m_rockPicked;
    private bool m_isDrilling;
    private bool m_diggedTunnel;

    public MyIngameHelpHandDrill()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_HandDrill_Title;
      this.RequiredIds = new string[1]{ "IngameHelp_Intro" };
      this.RequiredCondition = this.RequiredCondition + new Func<bool>(this.PlayerHasHandDrill);
      this.Details = new MyIngameHelpDetail[4]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_HandDrill_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_HandDrill_Detail2,
          FinishCondition = new Func<bool>(this.PlayerIsDrillingStone)
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_HandDrill_Detail3,
          FinishCondition = new Func<bool>(this.PickedRocks)
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_HandDrill_Detail4,
          FinishCondition = new Func<bool>(this.DiggedTunnel)
        }
      };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
      MyCharacterDetectorComponent.OnInteractiveObjectChanged += new Action<IMyUseObject>(this.MyCharacterDetectorComponent_OnInteractiveObjectChanged);
    }

    public override void OnActivated()
    {
      base.OnActivated();
      MyCharacterDetectorComponent.OnInteractiveObjectUsed += new Action<IMyUseObject>(this.MyCharacterDetectorComponent_OnInteractiveObjectUsed);
    }

    public override void CleanUp()
    {
      MyCharacterDetectorComponent.OnInteractiveObjectChanged -= new Action<IMyUseObject>(this.MyCharacterDetectorComponent_OnInteractiveObjectChanged);
      MyCharacterDetectorComponent.OnInteractiveObjectUsed -= new Action<IMyUseObject>(this.MyCharacterDetectorComponent_OnInteractiveObjectUsed);
    }

    private bool PlayerHasHandDrill()
    {
      MyCharacter myCharacter = MySession.Static != null ? MySession.Static.LocalCharacter : (MyCharacter) null;
      return myCharacter != null && myCharacter.EquippedTool is MyHandDrill;
    }

    private bool PlayerIsDrillingStone()
    {
      MyCharacter myCharacter = MySession.Static != null ? MySession.Static.LocalCharacter : (MyCharacter) null;
      if (myCharacter != null && myCharacter.EquippedTool is MyHandDrill equippedTool && (equippedTool.IsShooting && equippedTool.DrilledEntity is MyVoxelBase) && equippedTool.CollectingOre)
        this.m_isDrilling = true;
      return this.m_isDrilling;
    }

    private void MyCharacterDetectorComponent_OnInteractiveObjectChanged(IMyUseObject obj)
    {
      this.m_interactiveObject = (IMyUseObject) null;
      if (!(obj is MyFloatingObject myFloatingObject) || myFloatingObject.ItemDefinition == null)
        return;
      MyDefinitionId id = myFloatingObject.ItemDefinition.Id;
      if (!myFloatingObject.ItemDefinition.Id.SubtypeName.Contains("Stone"))
        return;
      this.m_interactiveObject = obj;
    }

    private void MyCharacterDetectorComponent_OnInteractiveObjectUsed(IMyUseObject obj)
    {
      if (this.m_interactiveObject != obj)
        return;
      this.m_rockPicked = true;
    }

    private bool PickedRocks() => this.m_rockPicked;

    private bool DiggedTunnel()
    {
      MyCharacter myCharacter = MySession.Static != null ? MySession.Static.LocalCharacter : (MyCharacter) null;
      if (myCharacter != null && myCharacter.EquippedTool is MyHandDrill equippedTool && (equippedTool.IsShooting && equippedTool.DrilledEntity is MyVoxelBase) && !equippedTool.CollectingOre)
        this.m_diggedTunnel = true;
      return this.m_diggedTunnel;
    }
  }
}
