// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.Ingame.MyPhysicalInventoryItemExtensions_ModAPI
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;

namespace VRage.Game.ModAPI.Ingame
{
  public static class MyPhysicalInventoryItemExtensions_ModAPI
  {
    public static MyItemInfo GetItemInfo(this MyItemType itemType)
    {
      MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition((MyDefinitionId) itemType);
      if (physicalItemDefinition == null)
        return new MyItemInfo();
      return new MyItemInfo()
      {
        Size = physicalItemDefinition.Size,
        Mass = physicalItemDefinition.Mass,
        Volume = physicalItemDefinition.Volume,
        MaxStackAmount = physicalItemDefinition.MaxStackAmount,
        UsesFractions = !physicalItemDefinition.HasIntegralAmounts,
        IsOre = physicalItemDefinition.Id.TypeId == typeof (MyObjectBuilder_Ore),
        IsIngot = physicalItemDefinition.Id.TypeId == typeof (MyObjectBuilder_Ingot),
        IsAmmo = physicalItemDefinition.Id.TypeId == typeof (MyObjectBuilder_AmmoMagazine),
        IsComponent = physicalItemDefinition.Id.TypeId == typeof (MyObjectBuilder_Component),
        IsTool = physicalItemDefinition.Id.TypeId == typeof (MyObjectBuilder_PhysicalGunObject)
      };
    }
  }
}
