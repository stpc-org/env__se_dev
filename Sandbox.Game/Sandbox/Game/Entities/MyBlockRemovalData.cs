// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyBlockRemovalData
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Cube;

namespace Sandbox.Game.Entities
{
  public class MyBlockRemovalData
  {
    public MySlimBlock Block;
    public ushort? BlockIdInCompound;
    public bool CheckExisting;

    public MyBlockRemovalData(MySlimBlock block, ushort? blockIdInCompound = null, bool checkExisting = false)
    {
      this.Block = block;
      this.BlockIdInCompound = blockIdInCompound;
      this.CheckExisting = checkExisting;
    }
  }
}
