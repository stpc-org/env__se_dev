// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatPlayerInventoryCapacity
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using VRage;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
  public class MyStatPlayerInventoryCapacity : MyStatBase
  {
    private float m_max;

    public override float MaxValue => this.m_max;

    public MyStatPlayerInventoryCapacity() => this.Id = MyStringHash.GetOrCompute("player_inventory_capacity");

    public override void Update()
    {
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      if (localCharacter == null)
        return;
      MyInventory inventory = MyEntityExtensions.GetInventory(localCharacter);
      if (inventory == null)
        return;
      this.m_max = (float) MyFixedPoint.MultiplySafe(inventory.MaxVolume, 1000).ToIntSafe();
      this.CurrentValue = (float) MyFixedPoint.MultiplySafe(inventory.CurrentVolume, 1000).ToIntSafe();
    }
  }
}
