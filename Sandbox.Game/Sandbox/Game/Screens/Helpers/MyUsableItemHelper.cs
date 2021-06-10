// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyUsableItemHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Input;

namespace Sandbox.Game.Screens.Helpers
{
  public class MyUsableItemHelper
  {
    public static void ItemActivatedGridKeyboard(
      MyPhysicalInventoryItem item,
      MyInventory inventory,
      MyCharacter character,
      MySharedButtonsEnum button)
    {
      if (item.Content is MyObjectBuilder_ConsumableItem && button == MySharedButtonsEnum.Secondary)
      {
        MyFixedPoint amount1 = item.Amount;
        if (amount1 > (MyFixedPoint) 0)
        {
          MyFixedPoint amount2 = MyFixedPoint.Min(amount1, (MyFixedPoint) 1);
          if (character != null && character.StatComp != null && amount2 > (MyFixedPoint) 0)
          {
            if (character.StatComp.HasAnyComsumableEffect() || character.SuitBattery != null && character.SuitBattery.HasAnyComsumableEffect())
            {
              if (MyHud.Notifications != null)
              {
                MyHudNotification myHudNotification = new MyHudNotification(MyCommonTexts.ConsumableCooldown);
                MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification);
                return;
              }
            }
            else
              inventory.ConsumeItem(item.Content.GetId(), amount2, character.EntityId);
          }
        }
      }
      if (!(item.Content is MyObjectBuilder_Datapad content) || button != MySharedButtonsEnum.Secondary)
        return;
      MyScreenManager.AddScreen((MyGuiScreenBase) new MyGuiDatapadEditScreen(content, item, inventory, character));
    }

    public static bool ItemActivatedGridGamepad(
      MyPhysicalInventoryItem item,
      MyInventory inventory,
      MyCharacter character,
      MyGridItemAction action)
    {
      if (item.Content is MyObjectBuilder_ConsumableItem && action == MyGridItemAction.Button_Y)
      {
        MyFixedPoint amount1 = item.Amount;
        if (amount1 > (MyFixedPoint) 0)
        {
          MyFixedPoint amount2 = MyFixedPoint.Min(amount1, (MyFixedPoint) 1);
          if (character != null && character.StatComp != null && amount2 > (MyFixedPoint) 0)
          {
            if (character.StatComp.HasAnyComsumableEffect() || character.SuitBattery != null && character.SuitBattery.HasAnyComsumableEffect())
            {
              if (MyHud.Notifications != null)
              {
                MyHudNotification myHudNotification = new MyHudNotification(MyCommonTexts.ConsumableCooldown);
                MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification);
                return false;
              }
            }
            else
              inventory.ConsumeItem(item.Content.GetId(), amount2, character.EntityId);
            return true;
          }
        }
      }
      if (!(item.Content is MyObjectBuilder_Datapad content) || action != MyGridItemAction.Button_Y)
        return false;
      MyScreenManager.AddScreen((MyGuiScreenBase) new MyGuiDatapadEditScreen(content, item, inventory, character));
      return true;
    }
  }
}
