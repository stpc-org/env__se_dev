// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.Ingame.IMyEntity
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Game.Components;
using VRageMath;

namespace VRage.Game.ModAPI.Ingame
{
  public interface IMyEntity
  {
    MyEntityComponentContainer Components { get; }

    long EntityId { get; }

    string Name { get; }

    string DisplayName { get; }

    bool HasInventory { get; }

    int InventoryCount { get; }

    IMyInventory GetInventory();

    IMyInventory GetInventory(int index);

    BoundingBoxD WorldAABB { get; }

    BoundingBoxD WorldAABBHr { get; }

    MatrixD WorldMatrix { get; }

    BoundingSphereD WorldVolume { get; }

    BoundingSphereD WorldVolumeHr { get; }

    Vector3D GetPosition();
  }
}
