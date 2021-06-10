// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.IMyGridTerminalSystem
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;
using System.Collections.Generic;

namespace Sandbox.ModAPI
{
  public interface IMyGridTerminalSystem : Sandbox.ModAPI.Ingame.IMyGridTerminalSystem
  {
    void GetBlocks(List<IMyTerminalBlock> blocks);

    void GetBlockGroups(List<IMyBlockGroup> blockGroups);

    void GetBlocksOfType<T>(List<IMyTerminalBlock> blocks, Func<IMyTerminalBlock, bool> collect = null);

    void SearchBlocksOfName(
      string name,
      List<IMyTerminalBlock> blocks,
      Func<IMyTerminalBlock, bool> collect = null);

    IMyTerminalBlock GetBlockWithName(string name);

    IMyBlockGroup GetBlockGroupWithName(string name);
  }
}
