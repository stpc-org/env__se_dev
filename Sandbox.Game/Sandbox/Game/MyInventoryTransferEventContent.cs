// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.MyInventoryTransferEventContent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage;
using VRage.Utils;

namespace Sandbox.Game
{
  public struct MyInventoryTransferEventContent
  {
    public MyFixedPoint Amount;
    public uint ItemId;
    public long SourceOwnerId;
    public MyStringHash SourceInventoryId;
    public long DestinationOwnerId;
    public MyStringHash DestinationInventoryId;
  }
}
