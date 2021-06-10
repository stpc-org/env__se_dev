// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.Conveyors.PullInformation
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;

namespace Sandbox.Game.GameSystems.Conveyors
{
  public class PullInformation
  {
    public MyInventory Inventory { get; set; }

    public long OwnerID { get; set; }

    public MyInventoryConstraint Constraint { get; set; }

    public MyDefinitionId ItemDefinition { get; set; }
  }
}
