// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Inventory.MyInventoryBaseExtensions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Utils;

namespace Sandbox.Game.Entities.Inventory
{
  public static class MyInventoryBaseExtensions
  {
    private static List<MyComponentBase> m_tmpList = new List<MyComponentBase>();

    public static MyInventoryBase GetInventory(
      this MyEntity entity,
      MyStringHash inventoryId)
    {
      MyInventoryBase myInventoryBase1 = entity.Components.Get<MyInventoryBase>();
      if (myInventoryBase1 != null && inventoryId.Equals(MyStringHash.GetOrCompute(myInventoryBase1.InventoryId.ToString())))
        return myInventoryBase1;
      if (myInventoryBase1 is MyInventoryAggregate)
      {
        MyInventoryAggregate aggregate = myInventoryBase1 as MyInventoryAggregate;
        MyInventoryBaseExtensions.m_tmpList.Clear();
        List<MyComponentBase> tmpList = MyInventoryBaseExtensions.m_tmpList;
        aggregate.GetComponentsFlattened(tmpList);
        foreach (MyComponentBase tmp in MyInventoryBaseExtensions.m_tmpList)
        {
          MyInventoryBase myInventoryBase2 = tmp as MyInventoryBase;
          if (inventoryId.Equals(MyStringHash.GetOrCompute(myInventoryBase2.InventoryId.ToString())))
            return myInventoryBase2;
        }
      }
      return (MyInventoryBase) null;
    }
  }
}
