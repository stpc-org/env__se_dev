// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyInventoryOwnerTypeEnum
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;

namespace Sandbox.Game.Entities
{
  [Obsolete("IMyInventoryOwner interface and MyInventoryOwnerTypeEnum enum is obsolete. Use type checking and inventory methods on MyEntity.")]
  public enum MyInventoryOwnerTypeEnum
  {
    Character,
    Storage,
    Energy,
    System,
    Conveyor,
  }
}
