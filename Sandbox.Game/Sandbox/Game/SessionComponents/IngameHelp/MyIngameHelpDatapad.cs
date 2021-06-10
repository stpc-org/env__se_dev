// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.IngameHelp.MyIngameHelpDatapad
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Input;

namespace Sandbox.Game.SessionComponents.IngameHelp
{
  [IngameObjective("IngameHelp_Datapad", 300)]
  internal class MyIngameHelpDatapad : MyIngameHelpObjective
  {
    private MyDefinitionId m_datapadDefinitionId;
    private bool m_datapadOpenned;
    private bool m_iPressed;

    public MyIngameHelpDatapad()
    {
      this.TitleEnum = MySpaceTexts.IngameHelp_Datapad_Title;
      this.RequiredIds = new string[3]
      {
        "IngameHelp_Intro",
        "IngameHelp_HUD",
        "IngameHelp_EconomyStation"
      };
      this.Details = new MyIngameHelpDetail[3]
      {
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Datapad_Desc
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Inventory_Detail3,
          Args = new object[1]
          {
            MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.INVENTORY)
          },
          FinishCondition = new Func<bool>(this.IPressed)
        },
        new MyIngameHelpDetail()
        {
          TextEnum = MySpaceTexts.IngameHelp_Datapad_Detail1,
          Args = new object[1]
          {
            MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.SECONDARY_TOOL_ACTION)
          },
          FinishCondition = new Func<bool>(this.CheckDatapadOpened)
        }
      };
      this.DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
      this.RequiredCondition = new Func<bool>(this.CheckDatapadInInventory);
      MyGuiDatapadEditScreen.OnDatapadOpened += new Action(this.OnDatapadOpened);
    }

    private void OnDatapadOpened() => this.m_datapadOpenned = true;

    public override void CleanUp() => MyGuiDatapadEditScreen.OnDatapadOpened -= new Action(this.OnDatapadOpened);

    private bool CheckDatapadInInventory()
    {
      if (MySession.Static == null || !(MySession.Static.ControlledEntity is MyEntity controlledEntity))
        return false;
      MyInventoryBase myInventoryBase = controlledEntity.Components.Get<MyInventoryBase>();
      if (myInventoryBase == null)
        return false;
      if (this.m_datapadDefinitionId.TypeId.IsNull)
      {
        MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
        if (component == null)
          return false;
        this.m_datapadDefinitionId = (MyDefinitionId) component.EconomyDefinition.DatapadDefinition;
      }
      return myInventoryBase.GetItemAmount(this.m_datapadDefinitionId) > (MyFixedPoint) 0;
    }

    private bool CheckDatapadOpened() => this.m_datapadOpenned;

    private bool IPressed()
    {
      if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsSpace.INVENTORY))
        this.m_iPressed = true;
      return this.m_iPressed;
    }
  }
}
