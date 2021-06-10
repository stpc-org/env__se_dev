// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDialogInventoryCheat
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Input;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  internal class MyGuiScreenDialogInventoryCheat : MyGuiScreenBase
  {
    private List<MyPhysicalItemDefinition> m_physicalItemDefinitions = new List<MyPhysicalItemDefinition>();
    private MyGuiControlTextbox m_amountTextbox;
    private MyGuiControlButton m_confirmButton;
    private MyGuiControlButton m_cancelButton;
    private MyGuiControlCombobox m_items;
    private static double m_lastAmount;
    private static int m_lastSelectedItem;
    private static int addedAsteroidsCount;

    public MyGuiScreenDialogInventoryCheat()
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR))
    {
      this.CanHideOthers = false;
      this.EnabledBackgroundFade = true;
      this.RecreateControls(true);
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDialogInventoryCheat);

    public override void RecreateControls(bool contructor)
    {
      base.RecreateControls(contructor);
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(0.0f, -0.1f)), text: "Select the amount and type of items to spawn in your inventory", originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
      this.m_amountTextbox = new MyGuiControlTextbox(new Vector2?(new Vector2(-0.2f, 0.0f)), maxLength: 9, type: MyGuiControlTextboxType.DigitsOnly);
      this.m_items = new MyGuiControlCombobox(new Vector2?(new Vector2(0.2f, 0.0f)), new Vector2?(new Vector2(0.3f, 0.05f)));
      this.m_confirmButton = new MyGuiControlButton(new Vector2?(new Vector2(0.21f, 0.1f)), size: new Vector2?(new Vector2(0.2f, 0.05f)), text: new StringBuilder("Confirm"));
      this.m_cancelButton = new MyGuiControlButton(new Vector2?(new Vector2(-0.21f, 0.1f)), size: new Vector2?(new Vector2(0.2f, 0.05f)), text: new StringBuilder("Cancel"));
      foreach (MyDefinitionBase allDefinition in MyDefinitionManager.Static.GetAllDefinitions())
      {
        if (allDefinition is MyPhysicalItemDefinition physicalItemDefinition && physicalItemDefinition.CanSpawnFromScreen)
        {
          int count = this.m_physicalItemDefinitions.Count;
          this.m_physicalItemDefinitions.Add(physicalItemDefinition);
          this.m_items.AddItem((long) count, allDefinition.DisplayNameText);
        }
      }
      this.Controls.Add((MyGuiControlBase) this.m_amountTextbox);
      this.Controls.Add((MyGuiControlBase) this.m_items);
      this.Controls.Add((MyGuiControlBase) this.m_confirmButton);
      this.Controls.Add((MyGuiControlBase) this.m_cancelButton);
      this.m_amountTextbox.Text = string.Format("{0}", (object) MyGuiScreenDialogInventoryCheat.m_lastAmount);
      this.m_items.SelectItemByIndex(MyGuiScreenDialogInventoryCheat.m_lastSelectedItem);
      this.m_confirmButton.ButtonClicked += new Action<MyGuiControlButton>(this.confirmButton_OnButtonClick);
      this.m_cancelButton.ButtonClicked += new Action<MyGuiControlButton>(this.cancelButton_OnButtonClick);
    }

    public override void HandleUnhandledInput(bool receivedFocusInThisUpdate)
    {
      base.HandleUnhandledInput(receivedFocusInThisUpdate);
      if (MyInput.Static.IsKeyPress(MyKeys.Enter))
        this.confirmButton_OnButtonClick(this.m_confirmButton);
      if (!MyInput.Static.IsKeyPress(MyKeys.Escape))
        return;
      this.cancelButton_OnButtonClick(this.m_cancelButton);
    }

    private void confirmButton_OnButtonClick(MyGuiControlButton sender)
    {
      if (MySession.Static.ControlledEntity is MyEntity controlledEntity && controlledEntity.HasInventory)
      {
        double result = 0.0;
        double.TryParse(this.m_amountTextbox.Text, out result);
        MyGuiScreenDialogInventoryCheat.m_lastAmount = result;
        MyFixedPoint myFixedPoint = (MyFixedPoint) result;
        if (this.m_items.GetSelectedKey() < 0L || (int) this.m_items.GetSelectedKey() >= this.m_physicalItemDefinitions.Count)
          return;
        MyDefinitionId id = this.m_physicalItemDefinitions[(int) this.m_items.GetSelectedKey()].Id;
        MyGuiScreenDialogInventoryCheat.m_lastSelectedItem = (int) this.m_items.GetSelectedKey();
        MyInventory inventory = MyEntityExtensions.GetInventory(controlledEntity);
        if (inventory != null)
        {
          if (!MySession.Static.CreativeMode)
            myFixedPoint = MyFixedPoint.Min(inventory.ComputeAmountThatFits(id, 0.0f, 0.0f), myFixedPoint);
          MyObjectBuilder_PhysicalObject newObject = (MyObjectBuilder_PhysicalObject) MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) id);
          inventory.DebugAddItems(myFixedPoint, (MyObjectBuilder_Base) newObject);
        }
      }
      this.CloseScreen();
    }

    private void cancelButton_OnButtonClick(MyGuiControlButton sender) => this.CloseScreen();
  }
}
