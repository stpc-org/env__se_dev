// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenConfigComponents
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  internal class MyGuiScreenConfigComponents : MyGuiScreenBase
  {
    private MyGuiControlButton m_confirmButton;
    private MyGuiControlButton m_cancelButton;
    private long m_entityId;
    private List<MyEntity> m_entities;
    private MyGuiControlCombobox m_entitiesSelection;
    private MyGuiControlListbox m_removeComponentsListBox;
    private MyGuiControlListbox m_addComponentsListBox;

    public MyGuiScreenConfigComponents(List<MyEntity> entities)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR))
    {
      this.m_entities = entities;
      this.m_entityId = entities.FirstOrDefault<MyEntity>().EntityId;
      this.CanHideOthers = false;
      this.EnabledBackgroundFade = true;
      this.RecreateControls(true);
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenConfigComponents);

    public override void RecreateControls(bool contructor)
    {
      base.RecreateControls(contructor);
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(0.0f, -0.46f)), text: "Select components to remove and components to add", originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
      if (this.m_entitiesSelection == null)
      {
        this.m_entitiesSelection = new MyGuiControlCombobox();
        this.m_entitiesSelection.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.EntitySelected);
      }
      this.m_entitiesSelection.Position = new Vector2(0.0f, -0.42f);
      this.m_entitiesSelection.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
      this.m_entitiesSelection.ClearItems();
      foreach (MyEntity entity in this.m_entities)
        this.m_entitiesSelection.AddItem(entity.EntityId, entity.ToString());
      this.m_entitiesSelection.SelectItemByKey(this.m_entityId, false);
      this.Controls.Add((MyGuiControlBase) this.m_entitiesSelection);
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(0.0f, -0.39f)), text: string.Format("EntityID = {0}", (object) this.m_entityId), font: "White", originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
      MyEntity entity1;
      if (MyEntities.TryGetEntityById(this.m_entityId, out entity1))
        this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(0.0f, -0.36f)), text: string.Format("Name: {1}, Type: {0}", (object) entity1.GetType().Name, (object) entity1.DisplayNameText), font: "White", originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(-0.21f, -0.32f)), text: "Select components to remove", originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
      if (this.m_removeComponentsListBox == null)
        this.m_removeComponentsListBox = new MyGuiControlListbox();
      this.m_removeComponentsListBox.ClearItems();
      this.m_removeComponentsListBox.MultiSelect = true;
      this.m_removeComponentsListBox.Name = "RemoveComponents";
      List<Type> components;
      if (MyComponentContainerExtension.TryGetEntityComponentTypes(this.m_entityId, out components))
      {
        foreach (Type type in components)
          this.m_removeComponentsListBox.Add(new MyGuiControlListbox.Item(new StringBuilder(type.Name), userData: ((object) type)));
        this.m_removeComponentsListBox.VisibleRowsCount = components.Count + 1;
      }
      this.m_removeComponentsListBox.Position = new Vector2(-0.21f, 0.0f);
      this.m_removeComponentsListBox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
      this.m_removeComponentsListBox.ItemSize = new Vector2(0.38f, 0.036f);
      this.m_removeComponentsListBox.Size = new Vector2(0.4f, 0.6f);
      this.Controls.Add((MyGuiControlBase) this.m_removeComponentsListBox);
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(0.21f, -0.32f)), text: "Select components to add", originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
      if (this.m_addComponentsListBox == null)
        this.m_addComponentsListBox = new MyGuiControlListbox();
      this.m_addComponentsListBox.ClearItems();
      this.m_addComponentsListBox.MultiSelect = true;
      this.m_addComponentsListBox.Name = "AddComponents";
      components.Clear();
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
      this.m_addComponentsListBox.Position = new Vector2(0.21f, 0.0f);
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

    private void EntitySelected()
    {
      this.m_entityId = this.m_entitiesSelection.GetSelectedKey();
      this.RecreateControls(false);
    }

    public override void HandleUnhandledInput(bool receivedFocusInThisUpdate) => base.HandleUnhandledInput(receivedFocusInThisUpdate);

    private void confirmButton_OnButtonClick(MyGuiControlButton sender)
    {
      foreach (MyGuiControlListbox.Item selectedItem in this.m_removeComponentsListBox.SelectedItems)
        MyComponentContainerExtension.TryRemoveComponent(this.m_entityId, selectedItem.UserData as Type);
      foreach (MyGuiControlListbox.Item selectedItem in this.m_addComponentsListBox.SelectedItems)
      {
        if (selectedItem.UserData is MyDefinitionId)
          MyComponentContainerExtension.TryAddComponent(this.m_entityId, (MyDefinitionId) selectedItem.UserData);
      }
      this.CloseScreen();
    }

    private void cancelButton_OnButtonClick(MyGuiControlButton sender) => this.CloseScreen();
  }
}
