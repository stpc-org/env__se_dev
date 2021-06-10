// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugHandItemBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.Gui
{
  internal abstract class MyGuiScreenDebugHandItemBase : MyGuiScreenDebugBase
  {
    private List<MyHandItemDefinition> m_handItemDefinitions = new List<MyHandItemDefinition>();
    private MyGuiControlCombobox m_handItemsCombo;
    protected MyHandItemDefinition CurrentSelectedItem;
    private MyCharacter m_playerCharacter;

    public void OnWeaponChanged(object sender, System.EventArgs e) => this.SelectFirstHandItem();

    protected override void OnShow()
    {
      this.m_playerCharacter = MySession.Static.LocalCharacter;
      if (this.m_playerCharacter != null)
        this.m_playerCharacter.OnWeaponChanged += new EventHandler(this.OnWeaponChanged);
      base.OnShow();
    }

    protected override void OnClosed()
    {
      if (this.m_playerCharacter != null)
        this.m_playerCharacter.OnWeaponChanged -= new EventHandler(this.OnWeaponChanged);
      base.OnClosed();
    }

    protected void RecreateHandItemsCombo()
    {
      this.m_handItemsCombo = this.AddCombo();
      this.m_handItemDefinitions.Clear();
      foreach (MyHandItemDefinition handItemDefinition in MyDefinitionManager.Static.GetHandItemDefinitions())
      {
        MyDefinitionBase definition = MyDefinitionManager.Static.GetDefinition(handItemDefinition.PhysicalItemId);
        int count = this.m_handItemDefinitions.Count;
        this.m_handItemDefinitions.Add(handItemDefinition);
        this.m_handItemsCombo.AddItem((long) count, definition.DisplayNameText);
      }
      this.m_handItemsCombo.SortItemsByValueText();
      this.m_handItemsCombo.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.handItemsCombo_ItemSelected);
    }

    protected void RecreateSaveAndReloadButtons()
    {
      this.AddButton(new StringBuilder("Save"), new Action<MyGuiControlButton>(this.OnSave));
      this.AddButton(new StringBuilder("Reload"), new Action<MyGuiControlButton>(this.OnReload));
      this.AddButton(new StringBuilder("Transform"), new Action<MyGuiControlButton>(this.OnTransform));
      this.AddButton(new StringBuilder("Transform All"), new Action<MyGuiControlButton>(this.OnTransformAll));
    }

    protected void SelectFirstHandItem()
    {
      IMyHandheldGunObject<MyDeviceBase> currentWeapon = MySession.Static.LocalCharacter.CurrentWeapon;
      if (currentWeapon == null)
      {
        if (this.m_handItemsCombo.GetItemsCount() <= 0)
          return;
        this.m_handItemsCombo.SelectItemByIndex(0);
      }
      else
      {
        if (this.m_handItemsCombo.GetItemsCount() <= 0)
          return;
        try
        {
          if (currentWeapon.DefinitionId.TypeId != typeof (MyObjectBuilder_PhysicalGunObject))
          {
            MyDefinitionId physicalItemId = MyDefinitionManager.Static.GetPhysicalItemForHandItem(currentWeapon.DefinitionId).Id;
            this.m_handItemsCombo.SelectItemByKey((long) this.m_handItemDefinitions.FindIndex((Predicate<MyHandItemDefinition>) (x => x.PhysicalItemId == physicalItemId)));
          }
          else
          {
            MyDefinitionBase def = MyDefinitionManager.Static.GetDefinition(currentWeapon.DefinitionId);
            this.m_handItemsCombo.SelectItemByKey((long) this.m_handItemDefinitions.FindIndex((Predicate<MyHandItemDefinition>) (x => x.DisplayNameText == def.DisplayNameText)));
          }
        }
        catch (Exception ex)
        {
          this.m_handItemsCombo.SelectItemByIndex(0);
        }
      }
    }

    protected virtual void handItemsCombo_ItemSelected() => this.CurrentSelectedItem = this.m_handItemDefinitions[(int) this.m_handItemsCombo.GetSelectedKey()];

    private void OnSave(MyGuiControlButton button) => MyDefinitionManager.Static.SaveHandItems();

    private void OnReload(MyGuiControlButton button) => MyDefinitionManager.Static.ReloadHandItems();

    private void OnTransformAll(MyGuiControlButton button)
    {
      foreach (MyHandItemDefinition handItemDefinition in MyDefinitionManager.Static.GetHandItemDefinitions())
        this.TransformItem(handItemDefinition);
    }

    private void OnTransform(MyGuiControlButton button) => this.TransformItem(this.CurrentSelectedItem);

    private void TransformItem(MyHandItemDefinition item)
    {
      this.Reorientate(ref item.LeftHand);
      this.Reorientate(ref item.RightHand);
    }

    private void Reorientate(ref Matrix m)
    {
      MatrixD matrixD = new MatrixD(-1.0, 0.0, 0.0, 0.0, 0.0, -1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0);
      Matrix matrix = (Matrix) ref matrixD;
      Vector3 translation = m.Translation;
      m = matrix * m;
      m.Translation = translation;
    }

    private void Reorientate(ref Vector3 v)
    {
      v.X = -v.X;
      v.Y = -v.Y;
    }

    private void SwapYZ(ref Matrix m)
    {
      Vector3 translation = m.Translation;
      float y = m.Translation.Y;
      translation.Y = m.Translation.Z;
      translation.Z = y;
      m.Translation = translation;
    }

    private void SwapYZ(ref Vector3 v)
    {
      float y = v.Y;
      v.Y = v.Z;
      v.Z = y;
    }

    protected MyGuiScreenDebugHandItemBase()
      : base()
    {
    }
  }
}
