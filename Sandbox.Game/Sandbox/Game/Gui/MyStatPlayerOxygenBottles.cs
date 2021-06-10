// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatPlayerOxygenBottles
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Library.Utils;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
  public class MyStatPlayerOxygenBottles : MyStatBase
  {
    private static readonly MyDefinitionId OXYGEN_BOTTLE_ID = MyDefinitionId.Parse("MyObjectBuilder_OxygenContainerObject/OxygenBottle");
    private static readonly double CHECK_INTERVAL_MS = 1000.0;
    private static readonly MyGameTimer TIMER = new MyGameTimer();
    private double m_lastCheck;

    public MyStatPlayerOxygenBottles()
    {
      this.Id = MyStringHash.GetOrCompute("player_oxygen_bottles");
      this.m_lastCheck = 0.0;
    }

    public override void Update()
    {
      if (MyStatPlayerOxygenBottles.TIMER.ElapsedTimeSpan.TotalMilliseconds - MyStatPlayerOxygenBottles.CHECK_INTERVAL_MS < this.m_lastCheck)
        return;
      this.m_lastCheck = MyStatPlayerOxygenBottles.TIMER.ElapsedTimeSpan.TotalMilliseconds;
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      if (localCharacter == null)
      {
        this.CurrentValue = 0.0f;
      }
      else
      {
        MyInventory inventory = MyEntityExtensions.GetInventory(localCharacter);
        if (inventory == null)
        {
          this.CurrentValue = 0.0f;
        }
        else
        {
          this.CurrentValue = 0.0f;
          foreach (MyPhysicalInventoryItem physicalInventoryItem in inventory.GetItems())
          {
            if (physicalInventoryItem.Content.GetId() == MyStatPlayerOxygenBottles.OXYGEN_BOTTLE_ID && (double) ((MyObjectBuilder_GasContainerObject) physicalInventoryItem.Content).GasLevel > 9.99999997475243E-07)
              ++this.CurrentValue;
          }
        }
      }
    }
  }
}
