// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpOwnership
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;

namespace Sandbox.Game.SessionComponents
{
  [IngameObjective("IngameHelp_Ownership", 90)]
  internal class MyIngameHelpOwnership : MyIngameHelpObjective
  {
    private bool m_accessDeniedHappened;
    private bool m_blockHacked;
    private HashSet<MyTerminalBlock> m_hackingBlocks = new HashSet<MyTerminalBlock>();

    public MyIngameHelpOwnership()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_Ownership_Title;
      this.RequiredIds = new string[1]
      {
        "IngameHelp_Building"
      };
      this.RequiredCondition = this.RequiredCondition + new Func<bool>(this.AccessDeniedHappened);
      this.Details = new MyIngameHelpDetail[2]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Ownership_Detail1
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Ownership_Detail2,
          FinishCondition = new Func<bool>(this.BlockHackedCondition)
        }
      };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
      this.FollowingId = "IngameHelp_OwnershipTip";
      if (MyHud.Notifications == null)
        return;
      MyHud.Notifications.OnNotificationAdded += new Action<MyNotificationSingletons>(this.Notifications_OnNotificationAdded);
    }

    public override void CleanUp()
    {
      if (MyHud.Notifications == null)
        return;
      MyHud.Notifications.OnNotificationAdded -= new Action<MyNotificationSingletons>(this.Notifications_OnNotificationAdded);
    }

    public override void OnActivated()
    {
      base.OnActivated();
      MySlimBlock.OnAnyBlockHackedChanged += new Action<MyTerminalBlock, long>(this.MyTerminalBlock_OnAnyBlockHackedChanged);
    }

    private void MyTerminalBlock_OnAnyBlockHackedChanged(MyTerminalBlock obj, long grinderOwner)
    {
      MyCharacter controlledEntity = MySession.Static.ControlledEntity as MyCharacter;
      if (this.m_hackingBlocks.Contains(obj) || controlledEntity == null || controlledEntity.GetPlayerIdentityId() != grinderOwner)
        return;
      this.m_hackingBlocks.Add(obj);
      obj.OwnershipChanged += new Action<MyTerminalBlock>(this.obj_OwnershipChanged);
    }

    private void obj_OwnershipChanged(MyTerminalBlock obj)
    {
      if (!(MySession.Static.ControlledEntity is MyCharacter controlledEntity) || controlledEntity.GetPlayerIdentityId() != obj.OwnerId)
        return;
      this.m_blockHacked = true;
    }

    private void Notifications_OnNotificationAdded(MyNotificationSingletons obj)
    {
      if (obj != MyNotificationSingletons.AccessDenied)
        return;
      this.m_accessDeniedHappened = true;
    }

    private bool BlockHackedCondition() => this.m_blockHacked;

    private bool AccessDeniedHappened() => this.m_accessDeniedHappened;
  }
}
