// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyToolbarItemConsumable
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Screens.Helpers
{
  [MyToolbarItemDescriptor(typeof (MyObjectBuilder_ToolbarItemConsumable))]
  public class MyToolbarItemConsumable : MyToolbarItemDefinition
  {
    public MyInventory Inventory => !(MySession.Static.ControlledEntity is MyCharacter controlledEntity) ? (MyInventory) null : MyEntityExtensions.GetInventory(controlledEntity);

    public override bool Activate()
    {
      MyFixedPoint a = this.Inventory != null ? this.Inventory.GetItemAmount(this.Definition.Id, MyItemFlags.None, false) : (MyFixedPoint) 0;
      if (a > (MyFixedPoint) 0)
      {
        MyCharacter controlledEntity = MySession.Static.ControlledEntity as MyCharacter;
        MyFixedPoint amount = MyFixedPoint.Min(a, (MyFixedPoint) 1);
        if (controlledEntity != null && controlledEntity.StatComp != null && amount > (MyFixedPoint) 0)
        {
          if (controlledEntity.StatComp.HasAnyComsumableEffect() || controlledEntity.SuitBattery != null && controlledEntity.SuitBattery.HasAnyComsumableEffect())
          {
            if (MyHud.Notifications != null)
            {
              MyHudNotification myHudNotification = new MyHudNotification(MyCommonTexts.ConsumableCooldown);
              MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification);
              return false;
            }
          }
          else
            this.Inventory.ConsumeItem(this.Definition.Id, amount, controlledEntity.EntityId);
        }
      }
      return true;
    }

    public override bool Init(MyObjectBuilder_ToolbarItem data)
    {
      int num = base.Init(data) ? 1 : 0;
      this.ActivateOnClick = false;
      this.WantsToBeActivated = false;
      return num != 0;
    }

    public override MyObjectBuilder_ToolbarItem GetObjectBuilder()
    {
      if (this.Definition == null)
        return (MyObjectBuilder_ToolbarItem) null;
      MyObjectBuilder_ToolbarItemConsumable objectBuilder = (MyObjectBuilder_ToolbarItemConsumable) MyToolbarItemFactory.CreateObjectBuilder((MyToolbarItem) this);
      objectBuilder.DefinitionId = (SerializableDefinitionId) this.Definition.Id;
      return (MyObjectBuilder_ToolbarItem) objectBuilder;
    }

    public override bool AllowedInToolbarType(MyToolbarType type) => type == MyToolbarType.Character;

    public override MyToolbarItem.ChangeInfo Update(MyEntity owner, long playerID = 0) => this.SetEnabled(this.Inventory != null && this.Inventory.GetItemAmount(this.Definition.Id, MyItemFlags.None, false) > (MyFixedPoint) 0);
  }
}
