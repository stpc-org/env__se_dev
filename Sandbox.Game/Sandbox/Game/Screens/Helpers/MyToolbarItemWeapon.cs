// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyToolbarItemWeapon
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System.Text;
using VRage.Game;
using VRage.Game.Entity;

namespace Sandbox.Game.Screens.Helpers
{
  [MyToolbarItemDescriptor(typeof (MyObjectBuilder_ToolbarItemWeapon))]
  public class MyToolbarItemWeapon : MyToolbarItemDefinition
  {
    protected int m_lastAmmoCount = -1;
    protected int m_lastMagazineCount = -1;
    protected bool m_needsWeaponSwitching = true;
    protected string m_lastTextValue = string.Empty;
    private bool m_areValuesDirty = true;

    public int AmmoCount => this.m_lastAmmoCount;

    public int MagazineCount => this.m_lastMagazineCount;

    public override bool Init(MyObjectBuilder_ToolbarItem data)
    {
      int num = base.Init(data) ? 1 : 0;
      this.ActivateOnClick = false;
      return num != 0;
    }

    public override bool Equals(object obj)
    {
      bool flag = base.Equals(obj);
      if (flag && !(obj is MyToolbarItemWeapon))
        flag = false;
      return flag;
    }

    public override MyObjectBuilder_ToolbarItem GetObjectBuilder() => base.GetObjectBuilder();

    public override bool Activate()
    {
      if (this.Definition == null)
        return false;
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      if (controlledEntity != null)
      {
        if (this.m_needsWeaponSwitching)
        {
          controlledEntity.SwitchToWeapon(this);
          this.WantsToBeActivated = true;
        }
        else
          controlledEntity.SwitchAmmoMagazine();
      }
      return true;
    }

    public override bool AllowedInToolbarType(MyToolbarType type) => true;

    public override MyToolbarItem.ChangeInfo Update(MyEntity owner, long playerID = 0)
    {
      bool flag1 = false;
      bool flag2 = false;
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      bool flag3 = localCharacter != null && (localCharacter.FindWeaponItemByDefinition(this.Definition.Id).HasValue || !localCharacter.WeaponTakesBuilderFromInventory(new MyDefinitionId?(this.Definition.Id)));
      MyToolbarItem.ChangeInfo changeInfo1 = MyToolbarItem.ChangeInfo.None;
      if (flag3)
      {
        IMyHandheldGunObject<MyDeviceBase> currentWeapon = localCharacter.CurrentWeapon;
        if (currentWeapon != null)
          flag1 = MyDefinitionManager.Static.GetPhysicalItemForHandItem(currentWeapon.DefinitionId).Id == this.Definition.Id;
        if (localCharacter.LeftHandItem != null)
          flag1 |= this.Definition == localCharacter.LeftHandItem.PhysicalItemDefinition;
        if (flag1 && currentWeapon != null)
        {
          if (MyDefinitionManager.Static.GetPhysicalItemForHandItem(currentWeapon.DefinitionId) is MyWeaponItemDefinition physicalItemForHandItem && physicalItemForHandItem.ShowAmmoCount)
          {
            int ammunitionAmount = localCharacter.CurrentWeapon.GetAmmunitionAmount();
            int magazineAmount = localCharacter.CurrentWeapon.GetMagazineAmount();
            if (this.m_lastAmmoCount != ammunitionAmount || this.m_lastMagazineCount != magazineAmount)
            {
              this.m_lastAmmoCount = ammunitionAmount;
              this.m_lastMagazineCount = magazineAmount;
              this.IconText.Clear().Append(string.Format("{0} • {1}", (object) ammunitionAmount, (object) magazineAmount));
              changeInfo1 |= MyToolbarItem.ChangeInfo.IconText;
            }
          }
          this.m_areValuesDirty = false;
        }
        else if (this.m_areValuesDirty && localCharacter != null && (this.Definition is MyWeaponItemDefinition definition && definition != null) && definition.ShowAmmoCount)
        {
          this.m_areValuesDirty = false;
          int num1 = 0;
          int num2 = 0;
          string str = string.Empty;
          MyInventory inventory = MyEntityExtensions.GetInventory(localCharacter);
          foreach (MyPhysicalInventoryItem physicalInventoryItem in inventory.GetItems())
          {
            if (physicalInventoryItem.Content != null && !(physicalInventoryItem.Content.SubtypeName != this.Definition.Id.SubtypeName) && (physicalInventoryItem.Content is MyObjectBuilder_PhysicalGunObject content && content.GunEntity is MyObjectBuilder_AutomaticRifle gunEntity) && gunEntity.GunBase != null)
            {
              num1 = gunEntity.GunBase.RemainingAmmo;
              str = gunEntity.GunBase.CurrentAmmoMagazineName;
              break;
            }
          }
          if (!string.IsNullOrEmpty(str))
          {
            foreach (MyPhysicalInventoryItem physicalInventoryItem in inventory.GetItems())
            {
              if (physicalInventoryItem.Content != null && !(physicalInventoryItem.Content.SubtypeName != str))
              {
                num2 = physicalInventoryItem.Amount.ToIntSafe();
                break;
              }
            }
          }
          this.m_lastAmmoCount = num1;
          this.m_lastMagazineCount = num2;
          this.IconText.Clear().Append(string.Format("{0} • {1}", (object) num1, (object) num2));
          changeInfo1 |= MyToolbarItem.ChangeInfo.IconText;
        }
      }
      if (MySession.Static.ControlledEntity is MyShipController controlledEntity && controlledEntity.GridSelectionSystem.WeaponSystem != null)
      {
        flag2 = controlledEntity.GridSelectionSystem.WeaponSystem.HasGunsOfId(this.Definition.Id);
        if (flag2)
        {
          IMyGunObject<MyDeviceBase> gun = controlledEntity.GridSelectionSystem.WeaponSystem.GetGun(this.Definition.Id);
          if (gun.GunBase is MyGunBase)
          {
            bool flag4 = gun is MySmallGatlingGun || gun is MySmallMissileLauncher;
            int number = 0;
            foreach (IMyGunObject<MyDeviceBase> myGunObject in controlledEntity.GridSelectionSystem.WeaponSystem.GetGunsById(this.Definition.Id))
            {
              if (flag4)
                number += myGunObject.GetTotalAmmunitionAmount();
              else
                number += myGunObject.GetAmmunitionAmount();
            }
            if (number != this.m_lastAmmoCount)
            {
              this.m_lastAmmoCount = number;
              this.IconText.Clear().AppendInt32(number);
              changeInfo1 |= MyToolbarItem.ChangeInfo.IconText;
            }
          }
        }
        MyDefinitionId? gunId = controlledEntity.GridSelectionSystem.GetGunId();
        MyDefinitionId id = this.Definition.Id;
        flag1 = gunId.HasValue && (!gunId.HasValue || gunId.GetValueOrDefault() == id);
      }
      MyToolbarItem.ChangeInfo changeInfo2 = changeInfo1 | this.SetEnabled(flag3 | flag2);
      this.WantsToBeSelected = flag1;
      this.m_needsWeaponSwitching = !flag1;
      if (this.m_lastTextValue != this.IconText.ToString())
        changeInfo2 |= MyToolbarItem.ChangeInfo.IconText;
      this.m_lastTextValue = this.IconText.ToString();
      return changeInfo2;
    }
  }
}
