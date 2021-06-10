// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.MyDeviceBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.Weapons
{
  public abstract class MyDeviceBase
  {
    public static string GetGunNotificationName(MyDefinitionId gunId) => MyDefinitionManager.Static.GetDefinition(gunId).DisplayNameText;

    public uint? InventoryItemId { get; set; }

    public void Init(MyObjectBuilder_DeviceBase objectBuilder) => this.InventoryItemId = objectBuilder.InventoryItemId;

    public abstract Vector3D GetMuzzleLocalPosition();

    public abstract Vector3D GetMuzzleWorldPosition();

    public abstract bool CanSwitchAmmoMagazine();

    public abstract bool SwitchToNextAmmoMagazine();

    public abstract bool SwitchAmmoMagazineToNextAvailable();
  }
}
