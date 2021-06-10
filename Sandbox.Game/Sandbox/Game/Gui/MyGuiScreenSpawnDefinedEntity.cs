// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenSpawnDefinedEntity
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  internal class MyGuiScreenSpawnDefinedEntity : MyGuiScreenBase
  {
    private MyGuiControlButton m_confirmButton;
    private MyGuiControlButton m_cancelButton;
    private MyGuiControlListbox m_containersListBox;
    private MyGuiControlCheckbox m_replicableEntityCheckBox;
    private Vector3 m_position;

    public MyGuiScreenSpawnDefinedEntity(Vector3 position)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR))
    {
      this.m_position = position;
      this.CanHideOthers = false;
      this.EnabledBackgroundFade = true;
      this.RecreateControls(true);
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenSpawnDefinedEntity);

    public override void RecreateControls(bool contructor)
    {
      base.RecreateControls(contructor);
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(0.0f, -0.46f)), text: "Select entity to spawn", originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
      if (this.m_containersListBox == null)
        this.m_containersListBox = new MyGuiControlListbox();
      this.m_containersListBox.ClearItems();
      this.m_containersListBox.MultiSelect = false;
      this.m_containersListBox.Name = "Containers";
      List<MyDefinitionId> definedContainers = new List<MyDefinitionId>();
      MyDefinitionManager.Static.GetDefinedEntityContainers(ref definedContainers);
      foreach (MyDefinitionId myDefinitionId in definedContainers)
      {
        string str = myDefinitionId.ToString();
        if (str.StartsWith("MyObjectBuilder_"))
          str = str.Remove(0, "MyObectBuilder_".Length + 1);
        this.m_containersListBox.Add(new MyGuiControlListbox.Item(new StringBuilder(str), userData: ((object) myDefinitionId)));
      }
      this.m_containersListBox.VisibleRowsCount = definedContainers.Count + 1;
      this.m_containersListBox.Position = new Vector2(0.0f, 0.0f);
      this.m_containersListBox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
      this.m_containersListBox.ItemSize = new Vector2(0.36f, 0.036f);
      this.m_containersListBox.Size = new Vector2(0.4f, 0.6f);
      this.Controls.Add((MyGuiControlBase) this.m_containersListBox);
      this.m_confirmButton = new MyGuiControlButton(new Vector2?(new Vector2(0.21f, 0.35f)), size: new Vector2?(new Vector2(0.2f, 0.05f)), text: new StringBuilder("Confirm"));
      this.m_cancelButton = new MyGuiControlButton(new Vector2?(new Vector2(-0.21f, 0.35f)), size: new Vector2?(new Vector2(0.2f, 0.05f)), text: new StringBuilder("Cancel"));
      this.Controls.Add((MyGuiControlBase) this.m_confirmButton);
      this.Controls.Add((MyGuiControlBase) this.m_cancelButton);
      this.m_confirmButton.ButtonClicked += new Action<MyGuiControlButton>(this.confirmButton_OnButtonClick);
      this.m_cancelButton.ButtonClicked += new Action<MyGuiControlButton>(this.cancelButton_OnButtonClick);
    }

    public override void HandleUnhandledInput(bool receivedFocusInThisUpdate) => base.HandleUnhandledInput(receivedFocusInThisUpdate);

    private void confirmButton_OnButtonClick(MyGuiControlButton sender)
    {
      MyContainerDefinition containerDefinition = new MyContainerDefinition();
      foreach (MyGuiControlListbox.Item selectedItem in this.m_containersListBox.SelectedItems)
      {
        if (selectedItem.UserData is MyDefinitionId)
          MyEntities.CreateEntityAndAdd((MyDefinitionId) selectedItem.UserData, false, true, new Vector3?(this.m_position));
      }
      this.CloseScreen();
    }

    private void cancelButton_OnButtonClick(MyGuiControlButton sender) => this.CloseScreen();
  }
}
