// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenSpawnEntity
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  internal class MyGuiScreenSpawnEntity : MyGuiScreenBase
  {
    private MyGuiControlButton m_confirmButton;
    private MyGuiControlButton m_cancelButton;
    private MyGuiControlListbox m_addComponentsListBox;
    private MyGuiControlCheckbox m_replicableEntityCheckBox;
    private Vector3 m_position;

    public MyGuiScreenSpawnEntity(Vector3 position)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR))
    {
      this.m_position = position;
      this.CanHideOthers = false;
      this.EnabledBackgroundFade = true;
      this.RecreateControls(true);
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenSpawnEntity);

    public override void RecreateControls(bool contructor)
    {
      base.RecreateControls(contructor);
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(0.0f, -0.46f)), text: "Select components to include in entity", originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
      this.m_replicableEntityCheckBox = new MyGuiControlCheckbox();
      this.m_replicableEntityCheckBox.Position = new Vector2(0.0f, -0.42f);
      this.m_replicableEntityCheckBox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
      this.Controls.Add((MyGuiControlBase) this.m_replicableEntityCheckBox);
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(0.0f, -0.39f)), text: "MyEntityReplicable / MyEntity", font: "White", originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(0.0f, -0.32f)), text: "Select components to add", originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
      if (this.m_addComponentsListBox == null)
        this.m_addComponentsListBox = new MyGuiControlListbox();
      this.m_addComponentsListBox.ClearItems();
      this.m_addComponentsListBox.MultiSelect = true;
      this.m_addComponentsListBox.Name = "AddComponents";
      List<MyDefinitionId> definedComponents = new List<MyDefinitionId>();
      MyDefinitionManager.Static.GetDefinedEntityComponents(ref definedComponents);
      foreach (MyDefinitionId myDefinitionId in definedComponents)
      {
        string str = myDefinitionId.ToString();
        if (str.StartsWith("MyObjectBuilder_"))
          str = str.Remove(0, "MyObectBuilder_".Length + 1);
        this.m_addComponentsListBox.Add(new MyGuiControlListbox.Item(new StringBuilder(str), userData: ((object) myDefinitionId)));
      }
      this.m_addComponentsListBox.VisibleRowsCount = definedComponents.Count + 1;
      this.m_addComponentsListBox.Position = new Vector2(0.0f, 0.0f);
      this.m_addComponentsListBox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
      this.m_addComponentsListBox.ItemSize = new Vector2(0.36f, 0.036f);
      this.m_addComponentsListBox.Size = new Vector2(0.4f, 0.6f);
      this.Controls.Add((MyGuiControlBase) this.m_addComponentsListBox);
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
      MyContainerDefinition newDefinition = new MyContainerDefinition();
      foreach (MyGuiControlListbox.Item selectedItem in this.m_addComponentsListBox.SelectedItems)
      {
        if (selectedItem.UserData is MyDefinitionId)
        {
          MyDefinitionId userData = (MyDefinitionId) selectedItem.UserData;
          newDefinition.DefaultComponents.Add(new MyContainerDefinition.DefaultComponent()
          {
            BuilderType = userData.TypeId,
            SubtypeId = new MyStringHash?(userData.SubtypeId)
          });
        }
      }
      MyObjectBuilder_EntityBase objectBuilder;
      if (this.m_replicableEntityCheckBox.IsChecked)
      {
        objectBuilder = (MyObjectBuilder_EntityBase) new MyObjectBuilder_ReplicableEntity();
        newDefinition.Id = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_ReplicableEntity), "DebugTest");
      }
      else
      {
        objectBuilder = new MyObjectBuilder_EntityBase();
        newDefinition.Id = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_EntityBase), "DebugTest");
      }
      MyDefinitionManager.Static.SetEntityContainerDefinition(newDefinition);
      objectBuilder.SubtypeName = newDefinition.Id.SubtypeName;
      objectBuilder.PositionAndOrientation = new MyPositionAndOrientation?(new MyPositionAndOrientation((Vector3D) this.m_position, Vector3.Forward, Vector3.Up));
      MyEntities.CreateFromObjectBuilderAndAdd(objectBuilder, false);
      this.CloseScreen();
    }

    private void cancelButton_OnButtonClick(MyGuiControlButton sender) => this.CloseScreen();
  }
}
